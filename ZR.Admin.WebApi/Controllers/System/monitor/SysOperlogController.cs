﻿using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;
using ZR.Admin.WebApi.Filters;
using ZR.Common;
using ZR.Model;
using ZR.Model.System.Dto;
using ZR.Model.System;
using ZR.Model.Vo;
using ZR.Service.System.IService;
using System;
using System.IO;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace ZR.Admin.WebApi.Controllers.monitor
{
    [Verify]
    [Route("/monitor/operlog")]
    public class SysOperlogController : BaseController
    {
        private ISysOperLogService sysOperLogService;
        private IWebHostEnvironment WebHostEnvironment;

        public SysOperlogController(ISysOperLogService sysOperLogService, IWebHostEnvironment hostEnvironment)
        {
            this.sysOperLogService = sysOperLogService;
            WebHostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// 查询操作日志
        /// </summary>
        /// <param name="sysOperLog"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public IActionResult OperList([FromQuery] SysOperLogDto sysOperLog)
        {
            PagerInfo pagerInfo = new PagerInfo(sysOperLog.pageNum);

            var list = sysOperLogService.SelectOperLogList(sysOperLog, pagerInfo);

            return ToResponse(ToJson(list.TotalNum, list), TIME_FORMAT_FULL_2);
        }

        /// <summary>
        /// 删除操作日志
        /// </summary>
        /// <param name="operIds"></param>
        /// <returns></returns>
        [Log(Title = "操作日志", BusinessType = BusinessType.DELETE)]
        [ActionPermissionFilter(Permission = "monitor:operlog:delete")]
        [HttpDelete("{operIds}")]
        public IActionResult Remove(string operIds)
        {
            long[] operIdss = Tools.SpitLongArrary(operIds);
            return SUCCESS(sysOperLogService.DeleteOperLogByIds(operIdss));
        }

        /// <summary>
        /// 清空操作日志
        /// </summary>
        /// <returns></returns>
        [Log(Title = "清空操作日志", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "monitor:operlog:delete")]
        [HttpDelete("clean")]
        public ApiResult ClearOperLog()
        {
            sysOperLogService.CleanOperLog();

            return ToJson(1);
        }

        /// <summary>
        /// 导出操作日志
        /// </summary>
        /// <returns></returns>
        [Log(Title = "操作日志", BusinessType = BusinessType.EXPORT)]
        [ActionPermissionFilter(Permission = "monitor:operlog:export")]
        [HttpGet("export")]
        public IActionResult Export([FromQuery] SysOperLogDto sysOperLog)
        {
            var list = sysOperLogService.SelectOperLogList(sysOperLog, new PagerInfo(1, 10000));
            string sFileName = ExportExcel(list.Result, "operlog", "操作日志");
            return SUCCESS(new { path = "/export/" + sFileName, fileName = sFileName });
        }

    }
}
