﻿using Infrastructure;
using Infrastructure.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Linq;
using ZR.Admin.WebApi.Extensions;
using ZR.Admin.WebApi.Framework;
using ZR.Model.System;

namespace ZR.Admin.WebApi.Filters
{
    /// <summary>
    /// 授权校验访问
    /// 如果跳过授权登录在Action 或controller加上 AllowAnonymousAttribute
    /// </summary>
    public class VerifyAttribute : Attribute, IAuthorizationFilter
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //IWebHostEnvironment webHostEnvironment = (IWebHostEnvironment)App.ServiceProvider.GetService(typeof(IWebHostEnvironment));

        //public VerifyAttribute() { }

        /// <summary>
        /// 只判断token是否正确，不判断权限
        /// 如果需要判断权限的在Action上加上ApiActionPermission属性标识权限类别，ActionPermissionFilter作权限处理
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var noNeedCheck = false;
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                noNeedCheck = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                  .Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)));
            }

            if (noNeedCheck) return;

            string ip = HttpContextExtension.GetClientUserIp(context.HttpContext);
            string url = context.HttpContext.Request.Path;
            var isAuthed = context.HttpContext.User.Identity.IsAuthenticated;
            var userName = context.HttpContext.User.Identity.Name;

            //使用jwt token校验2020-11-21
            LoginUser info = JwtUtil.GetLoginUser(context.HttpContext);

            if (info != null && info.UserId > 0)
            {
                logger.Info($"[{info.UserId}-{userName}-{ip}]用户登录校验成功");
            }
            else
            {
                string msg = $"请求访问:{url}授权认证失败，无法访问系统资源";
                logger.Info($"用户{userName}{msg}");
                
                context.Result = new JsonResult(new ApiResult((int)ResultCode.DENY, msg));
            }
        }
    }
}
