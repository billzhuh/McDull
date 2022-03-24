using Infrastructure;
using Infrastructure.Attribute;
using ZR.Model;
using ZR.Model.Dto;
using ZR.Model.Models;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using System;
using SqlSugar;
using System.Collections.Generic;

namespace ZR.Service.Business
{
    /// <summary>
    /// Service业务层处理
    ///
    /// @author zr
    /// @date 2022-01-21
    /// </summary>
    [AppService(ServiceType = typeof(IBizBookService), ServiceLifetime = LifeTime.Transient)]
    public class BizBookService : BaseService<BizBook>, IBizBookService
    {
        private readonly BizBookRepository _BizBookrepository;
        public BizBookService(BizBookRepository repository) : base(repository)
        {
            _BizBookrepository = repository;
        }

        #region 业务逻辑代码

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<BizBook> GetList(BizBookQueryDto parm)
        {
            //开始拼装查询条件
            var predicate = Expressionable.Create<BizBook>();
            predicate.And(t=>t.BookName==parm.BookName);
            //搜索条件查询语法参考Sqlsugar
            var response = _BizBookrepository
                .Queryable()
                .Where(predicate.ToExpression())
                .ToPage(parm);
            return response;
        }

        #endregion
    }
}