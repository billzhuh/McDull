using Microsoft.OpenApi.Models;
using RazorEngine;
using RazorEngine.Templating;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace XUnit.Core.Helper
{
    public class HtmlHelper
    {
        /// <summary>
        /// 将数据遍历静态页面中
        /// </summary>
        /// <param name="templatePath">静态页面地址</param>
        /// <param name="model">获取到的文件数据</param>
        /// <returns></returns>
        public static string GeneritorSwaggerHtml(string templatePath, OpenApiDocument model)
        {
            var template = System.IO.File.ReadAllText(templatePath);
            var result = Engine.Razor.RunCompile(template, "i3yuan", typeof(OpenApiDocument), model);
            return result;
        }
    }
}
