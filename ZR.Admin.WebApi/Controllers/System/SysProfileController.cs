﻿using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using ZR.Admin.WebApi.Extensions;
using ZR.Admin.WebApi.Filters;
using ZR.Common;
using ZR.Model.System.Dto;
using ZR.Model.System;
using ZR.Service.System.IService;

namespace ZR.Admin.WebApi.Controllers.System
{
    [Verify]
    [Route("system/user/profile")]
    public class SysProfileController : BaseController
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ISysUserService UserService;
        private readonly ISysRoleService RoleService;
        private readonly ISysUserPostService UserPostService;
        private readonly ISysDeptService DeptService;
        private OptionsSetting OptionsSetting;
        private IWebHostEnvironment hostEnvironment;

        public SysProfileController(
            ISysUserService userService,
            ISysRoleService roleService,
            ISysUserPostService postService,
            ISysDeptService deptService,
            IOptions<OptionsSetting> options,
            IWebHostEnvironment hostEnvironment)
        {
            UserService = userService;
            RoleService = roleService;
            UserPostService = postService;
            DeptService = deptService;
            OptionsSetting = options.Value;
            this.hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// 个人中心用户信息获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Profile()
        {
            long userId = HttpContext.GetUId();
            var user = UserService.SelectUserById(userId);

            var roles = RoleService.SelectUserRoleNames(userId);
            var postGroup = UserPostService.GetPostsStrByUserId(userId);
            var deptInfo = DeptService.GetFirst(f => f.DeptId == user.DeptId);
            user.DeptName = deptInfo?.DeptName ?? "-";

            return SUCCESS(new { user, roles, postGroup }, TIME_FORMAT_FULL);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "system")]
        [Log(Title = "修改信息", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateProfile([FromBody] SysUserDto userDto)
        {
            if (userDto == null)
            {
                throw new CustomException(ResultCode.PARAM_ERROR, "请求参数错误");
            }
            //从 Dto 映射到 实体
            var user = userDto.Adapt<SysUser>().ToUpdate(HttpContext);

            int result = UserService.ChangeUser(user);
            return ToResponse(result);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPut("updatePwd")]
        [ActionPermissionFilter(Permission = "system")]
        [Log(Title = "修改密码", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePwd(string oldPassword, string newPassword)
        {
            LoginUser loginUser = Framework.JwtUtil.GetLoginUser(HttpContext);

            SysUser user = UserService.SelectUserById(loginUser.UserId);
            string oldMd5 = NETCore.Encrypt.EncryptProvider.Md5(oldPassword);
            string newMd5 = NETCore.Encrypt.EncryptProvider.Md5(newPassword);
            if (user.Password != oldMd5)
            {
                return ToResponse(ApiResult.Error("修改密码失败，旧密码错误"));
            }
            if (user.Password == newMd5)
            {
                return ToResponse(ApiResult.Error("新密码不能和旧密码相同"));
            }
            if (UserService.ResetPwd(loginUser.UserId, newMd5) > 0)
            {
                //TODO 更新缓存

                return SUCCESS(1);
            }

            return ToResponse(ApiResult.Error("修改密码异常，请联系管理员"));
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("Avatar")]
        [ActionPermissionFilter(Permission = "system")]
        [Log(Title = "修改头像", BusinessType = BusinessType.UPDATE, IsSaveRequestData = false)]
        public IActionResult Avatar([FromForm(Name = "picture")] IFormFile formFile)
        {
            LoginUser loginUser = Framework.JwtUtil.GetLoginUser(HttpContext);

            if (formFile == null) throw new CustomException("请选择文件");

            string fileExt = Path.GetExtension(formFile.FileName);
            string savePath = Path.Combine(hostEnvironment.WebRootPath, FileUtil.GetdirPath("uploads"));

            Console.WriteLine(savePath);
            if (!Directory.Exists(savePath)) { Directory.CreateDirectory(savePath); }

            string fileName = FileUtil.HashFileName() + fileExt;
            string finalFilePath = savePath + fileName;

            using (var stream = new FileStream(finalFilePath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }
            string accessUrl = $"{OptionsSetting.Upload.UploadUrl}/{FileUtil.GetdirPath("uploads")}{fileName}";

            UserService.UpdatePhoto(new SysUser() { Avatar = accessUrl, UserId = loginUser.UserId });
            logger.Info("修改头像：" + accessUrl);
            return SUCCESS(new { imgUrl = accessUrl });
        }
    }
}
