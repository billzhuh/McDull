﻿using SqlSugar;
using System.Collections.Generic;

namespace ZR.Model.System
{
    /// <summary>
    /// Sys_menu表
    /// </summary>
    [SugarTable("sys_menu")]
    [Tenant("0")]
    public class SysMenu : SysBase
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        //[Key]//非自动增长主键时使用ExplicitKey
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long menuId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string menuName { get; set; }

        /// <summary>
        /// 父菜单名称
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string parentName { get; set; }

        /// <summary>
        /// 父菜单ID
        /// </summary>
        public long parentId { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int orderNum { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string path { get; set; } = "#";

        /// <summary>
        /// 组件路径
        /// </summary>
        public string component { get; set; }

        /// <summary>
        /// 是否缓存（1缓存 0不缓存）
        /// </summary>
        public string isCache { get; set; }
        /// <summary>
        /// 是否外链 1、是 0、否
        /// </summary>
        public string isFrame { get; set; }

        /// <summary>
        /// 类型（M目录 C菜单 F按钮 L链接）
        /// </summary>
        public string menuType { get; set; }

        /// <summary>
        /// 显示状态（0显示 1隐藏）
        /// </summary>
        public string visible { get; set; }

        /// <summary>
        /// 菜单状态（0正常 1停用）
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 权限字符串
        /// </summary>
        public string perms { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string icon { get; set; } = string.Empty;

        /// <summary>
        /// 子菜单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<SysMenu> children { get; set; } = new List<SysMenu>();
    }
}
