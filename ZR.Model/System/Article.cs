﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZR.Model.System
{
    /// <summary>
    /// 文章表
    /// </summary>
    [SqlSugar.SugarTable("article")]
    [Tenant("0")]
    public class Article
    {
        [SqlSugar.SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Cid { get; set; }
        public string Title { get; set; }
        public DateTime? CreateTime { get; set; }
        [SqlSugar.SugarColumn(IsOnlyIgnoreInsert = true)]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 作者名
        /// </summary>
        public string AuthorName { get; set; }
        public long UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string Type { get; set; }
        /// <summary>
        /// 文章状态 1、发布 2、草稿
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 编辑器类型 markdown,html
        /// </summary>
        public string Fmt_type { get; set; }
        /// <summary>
        /// 文章标签eg：Net5,java
        /// </summary>
        public string Tags { get; set; }
        public int Hits { get; set; }
        public int Category_id { get; set; }
    }
}
