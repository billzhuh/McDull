using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Mapster;
using ZR.Model.Dto;
using ZR.Model.Models;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Extensions;
using ZR.Admin.WebApi.Filters;
using ZR.Common;
using Infrastructure.Extensions;
using System.Linq;

namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// @author zr
    /// </summary>
    [Verify]
    [Route("business/BizBook")]
    public class BizBookController : BaseController
    {
        /// <summary>
        /// 接口
        /// </summary>
        private readonly IBizBookService _BizBookService;

        public BizBookController(IBizBookService BizBookService)
        {
            _BizBookService = BizBookService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "business:bizbook:list")]
        public IActionResult QueryBizBook([FromQuery] BizBookQueryDto parm)
        {
            var response = _BizBookService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "business:bizbook:query")]
        public IActionResult GetBizBook(long Id)
        {
            var response = _BizBookService.GetFirst(x => x.Id == Id);
            
            return SUCCESS(response);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "business:bizbook:add")]
        [Log(Title = "", BusinessType = BusinessType.INSERT)]
        public IActionResult AddBizBook([FromBody] BizBookDto parm)
        {
            if (parm == null)
            {
                throw new CustomException("请求参数错误");
            }
            //从 Dto 映射到 实体
            var model = parm.Adapt<BizBook>().ToCreate(HttpContext);

            var response = _BizBookService.Insert(model, it => new
            {
                it.BookName,
                it.Price,
                it.Author,
                it.Content,
            });
            return ToResponse(response);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "business:bizbook:edit")]
        [Log(Title = "", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateBizBook([FromBody] BizBookDto parm)
        {
            if (parm == null)
            {
                throw new CustomException("请求实体不能为空");
            }
            //从 Dto 映射到 实体
            var model = parm.Adapt<BizBook>().ToUpdate(HttpContext);

            var response = _BizBookService.Update(w => w.Id == model.Id, it => new BizBook()
            {
                //Update 字段映射
                BookName = model.BookName,
                Price = model.Price,
                Author = model.Author,
                Content = model.Content,
            });

            return ToResponse(response);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:bizbook:delete")]
        [Log(Title = "", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteBizBook(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _BizBookService.Delete(idsArr);

            return ToResponse(response);
        }


    }
}