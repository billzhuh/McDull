using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XUnit.Core.Helper;

namespace ZR.Admin.WebApi.Controllers
{
    [Route("swagger")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class SwaggerController : ControllerBase
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly SwaggerGenerator _swaggerGenerator;

        private readonly SpireDocHelper _spireDocHelper;

        public SwaggerController(IWebHostEnvironment hostingEnvironment, SpireDocHelper spireDocHelper, SwaggerGenerator swaggerGenerator)
        {
            _webHostEnvironment = hostingEnvironment;
            _spireDocHelper = spireDocHelper;
            _swaggerGenerator = swaggerGenerator;
        }
        /// <summary>
        /// 导出文件
        /// </summary>
        /// <param name="type">文件类型</param>
        /// <param name="version">版本号V1</param>
        /// <returns></returns>
        [HttpGet]
        public FileResult ExportWord(string type, string version)
        {
            string contenttype = string.Empty;

            var model = _swaggerGenerator.GetSwagger(version); //1. 根据指定版本获取指定版本的json对象。

            var html = HtmlHelper.GeneritorSwaggerHtml($"{_webHostEnvironment.WebRootPath}\\SwaggerDoc.cshtml", model); //2. 根据模板引擎生成html

            var op = _spireDocHelper.SwaggerConversHtml(html, type, out contenttype); //3.将html导出文件类型

            return File(op, contenttype, $"XUnit.Core接口文档{type}");
        }


    }
}
