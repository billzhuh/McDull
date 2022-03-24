using System;
using Infrastructure.Attribute;
using ZR.Repository.System;
using ZR.Model.Models;

namespace ZR.Repository
{
    /// <summary>
    /// 仓储
    ///
    /// @author zr
    /// @date 2022-01-21
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class BizBookRepository : BaseRepository<BizBook>
    {
        #region 业务逻辑代码
        #endregion
    }
}