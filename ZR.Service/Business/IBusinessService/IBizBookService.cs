using System;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Models;
using System.Collections.Generic;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// service接口
    ///
    /// @author zr
    /// @date 2022-01-21
    /// </summary>
    public interface IBizBookService : IBaseService<BizBook>
    {
        PagedInfo<BizBook> GetList(BizBookQueryDto parm);

    }
}
