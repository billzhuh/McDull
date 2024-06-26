﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Model.System;

namespace ZR.Service.System.IService
{
    public interface IArticleCategoryService : IBaseService<ArticleCategory>
    {
        List<ArticleCategory> BuildCategoryTree(List<ArticleCategory> categories);
    }
}
