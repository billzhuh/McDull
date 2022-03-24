using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZR.Model.Dto;
using ZR.Model.Models;

namespace ZR.Model.Dto
{
    /// <summary>
    /// 输入对象
    /// </summary>
    public class BizBookDto
    {
        [Required(ErrorMessage = "不能为空")]
        public long Id { get; set; }
        public string BookName { get; set; }
        [Required(ErrorMessage = "不能为空")]
        public decimal Price { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }

    /// <summary>
    /// 查询对象
    /// </summary>
    public class BizBookQueryDto : PagerInfo 
    {
        public string  BookName { get; set; }
    }
}
