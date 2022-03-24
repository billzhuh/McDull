﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZR.Admin.WebApi.Extensions;
using ZR.Admin.WebApi.Filters;
using ZR.Admin.WebApi.Framework;
using Infrastructure.Model;
using Infrastructure;
using Infrastructure.Attribute;
using ZR.Model.System;
using ZR.Model.System.Dto;
using ZR.Service.System.IService;
using Hei.Captcha;
using ZR.Common;
using ZR.Service.System;
using Microsoft.Extensions.Options;

namespace ZR.Admin.WebApi.Controllers.System
{
    /// <summary>
    /// 登录
    /// </summary>
    public class SysLoginController : BaseController
    {
        static readonly NLog.Logger logger = NLog.LogManager.GetLogger("LoginController");
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISysUserService sysUserService;
        private readonly ISysMenuService sysMenuService;
        private readonly ISysLoginService sysLoginService;
        private readonly ISysPermissionService permissionService;
        private readonly SecurityCodeHelper SecurityCodeHelper;
        private readonly ISysConfigService sysConfigService;
        private readonly ISysRoleService roleService;
        private readonly OptionsSetting jwtSettings;

        public SysLoginController(
            IHttpContextAccessor contextAccessor,
            ISysMenuService sysMenuService,
            ISysUserService sysUserService,
            ISysLoginService sysLoginService,
            ISysPermissionService permissionService,
            ISysConfigService configService,
            ISysRoleService sysRoleService,
            SecurityCodeHelper captcha,
            IOptions<OptionsSetting> jwtSettings)
        {
            httpContextAccessor = contextAccessor;
            SecurityCodeHelper = captcha;
            this.sysMenuService = sysMenuService;
            this.sysUserService = sysUserService;
            this.sysLoginService = sysLoginService;
            this.permissionService = permissionService;
            this.sysConfigService = configService;
            roleService = sysRoleService;
            this.jwtSettings = jwtSettings.Value;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginBody">登录对象</param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        [Log(Title = "登录")]
        public IActionResult Login([FromBody] LoginBodyDto loginBody)
        {
            if (loginBody == null) { throw new CustomException("请求参数错误"); }
            loginBody.LoginIP = HttpContextExtension.GetClientUserIp(HttpContext);
            SysConfig sysConfig = sysConfigService.GetSysConfigByKey("sys.account.captchaOnOff");
            if (sysConfig?.ConfigValue != "off" && CacheHelper.Get(loginBody.Uuid) is string str && !str.ToLower().Equals(loginBody.Code.ToLower()))
            {
                return ToResponse(ResultCode.CAPTCHA_ERROR, "验证码错误");
            }

            var user = sysLoginService.Login(loginBody, AsyncFactory.RecordLogInfo(httpContextAccessor.HttpContext, "0", "login"));
            
            #region 存入cookie Action校验权限使用
            List<SysRole> roles = roleService.SelectUserRoleListByUserId(user.UserId);
            //权限集合 eg *:*:*,system:user:list
            List<string> permissions = permissionService.GetMenuPermission(user);
            #endregion

            LoginUser loginUser = new(user, roles, permissions);
            CacheHelper.SetCache(GlobalConstant.UserPermKEY + user.UserId, loginUser);
            return SUCCESS(JwtUtil.GenerateJwtToken(HttpContext.AddClaims(loginUser), jwtSettings.JwtSettings));
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [Log(Title = "注销")]
        [HttpPost("logout")]
        public IActionResult LogOut()
        {
            //Task.Run(async () =>
            //{
            //    //注销登录的用户，相当于ASP.NET中的FormsAuthentication.SignOut  
            //    await HttpContext.SignOutAsync();
            //}).Wait();
            var id = HttpContext.GetUId();

            CacheHelper.Remove(GlobalConstant.UserPermKEY + id);
            return SUCCESS(1);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [Verify]
        [HttpGet("getInfo")]
        public IActionResult GetUserInfo()
        {
            long userid = HttpContext.GetUId();
            var user = sysUserService.SelectUserById(userid);

            //前端校验按钮权限使用
            //角色集合 eg: admin,yunying,common
            List<string> roles = permissionService.GetRolePermission(user);
            //权限集合 eg *:*:*,system:user:list
            List<string> permissions = permissionService.GetMenuPermission(user);

            return SUCCESS(new { user, roles, permissions });
        }

        /// <summary>
        /// 获取路由信息
        /// </summary>
        /// <returns></returns>
        [Verify]
        [HttpGet("getRouters")]
        public IActionResult GetRouters()
        {
            long uid = HttpContext.GetUId();
            var menus = sysMenuService.SelectMenuTreeByUserId(uid);

            return ToResponse(ToJson(1, sysMenuService.BuildMenus(menus)));
        }

        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet("captchaImage")]
        public ApiResult CaptchaImage()
        {
            string uuid = Guid.NewGuid().ToString().Replace("-", "");

            SysConfig sysConfig = sysConfigService.GetSysConfigByKey("sys.account.captchaOnOff");
            var captchaOff = sysConfig?.ConfigValue ?? "0";

            var code = SecurityCodeHelper.GetRandomEnDigitalText(4);
            byte[] imgByte;
            if (captchaOff == "1")
            {
                imgByte = SecurityCodeHelper.GetGifEnDigitalCodeByte(code);//动态gif数字字母
            }
            else if (captchaOff == "2")
            {
                imgByte = SecurityCodeHelper.GetGifBubbleCodeByte(code);//动态gif泡泡
            }
            else if (captchaOff == "3")
            {
                imgByte = SecurityCodeHelper.GetBubbleCodeByte(code);//泡泡
            }
            else
            {
                imgByte = SecurityCodeHelper.GetEnDigitalCodeByte(code);//英文字母加数字
            }
            string base64Str = Convert.ToBase64String(imgByte);
            CacheHelper.SetCache(uuid, code);
            var obj = new { uuid, img = base64Str };// File(stream, "image/png")

            return ToJson(1, obj);
        }
    }
}
