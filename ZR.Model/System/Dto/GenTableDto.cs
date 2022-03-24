﻿using System;
using System.Collections.Generic;
using System.Text;
using ZR.Model.System.Generate;

namespace ZR.Model.System.Dto
{
    public class GenTableDto
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public string TableComment { get; set; }
        public string SubTableName { get; set; }
        public string SubTableFkName { get; set; }
        public string ClassName { get; set; }
        public string TplCategory { get; set; }
        public string BaseNameSpace { get; set; }
        public string ModuleName { get; set; }
        public string BusinessName { get; set; }
        public string FunctionName { get; set; }
        public string FunctionAuthor { get; set; }
        public string GenType { get; set; }
        public string GenPath { get; set; }
        /// <summary>
        /// 额外参数
        /// </summary>
        public Options Params { get; set; }
        public List<GenTableColumnDto> Columns { get; set; }
    }

    /// <summary>
    /// 额外参数
    /// ****注意里面参数统一首字母小写*****
    /// </summary>
    public class Options
    {
        public string treeCode { get; set; }
        public string treeName { get; set; }
        public string treeParentCode { get; set; }
        public int? parentMenuId { get; set; }
        public string sortField { get; set; }
        public string sortType { get; set; }
        /// <summary>
        /// 额外参数字符串
        /// </summary>
        public string checkedBtn { get; set; }
    }
    public class GenTableColumnDto
    {
        public int ColumnId { get; set; }
        public int TableId { get; set; }
        public string ColumnComment { get; set; }
        public string CsharpType { get; set; }
        public string CsharpField { get; set; }
        public bool IsInsert { get; set; }
        public bool IsEdit { get; set; }
        public bool IsList { get; set; }
        public bool IsQuery { get; set; }
        public bool IsRequired { get; set; }
        /// <summary>
        /// 显示类型（文本框、文本域、下拉框、复选框、单选框、日期控件）
        /// </summary>
        public string HtmlType { get; set; }
        /// <summary>
        /// 查询类型（等于、不等于、大于、小于、范围）
        /// </summary>
        public string QueryType { get; set; } = "EQ";
        public int Sort { get; set; }
        /// <summary>
        /// 字典类型
        /// </summary>
        public string DictType { get; set; }
    }

}
