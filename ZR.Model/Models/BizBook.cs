using System;
using System.Collections.Generic;
using SqlSugar;
using OfficeOpenXml.Attributes;

namespace ZR.Model.Models
{
    /// <summary>
    /// ，数据实体对象
    ///
    /// @author zr
    /// @date 2022-01-21
    /// </summary>
    [SugarTable("biz_book")]
    public class BizBook
    {
        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 描述 : 
        /// 空值 : true  
        /// </summary>
        public string BookName { get; set; }

        /// <summary>
        /// 描述 : 
        /// 空值 : false  
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 描述 : 
        /// 空值 : true  
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 描述 : 
        /// 空值 : true  
        /// </summary>
        public string Content { get; set; }

    }
}