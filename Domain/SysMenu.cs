﻿using Castle.ActiveRecord;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类: 系统菜单
    /// </summary>
    [ActiveRecord]
    public class SysMenu : BaseEntity<SysMenu>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        [Property(Length = 100, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        [Display(Name = "类名")]
        [Property(Length = 100)]
        public string ClassName { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        [Display(Name = "控制器")]
        [Property(Length = 100)]
        public string ControllerName { get; set; }

        /// <summary>
        /// 动作
        /// </summary>
        [Display(Name = "动作")]
        [Property(Length = 100)]
        public string ActionName { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        [Display(Name = "排序码")]
        [Property]
        public int SortCode { get; set; }

        /// <summary>
        /// 上级菜单
        ///     多对一关系
        /// </summary>
        [Display(Name = "上级菜单")]
        [BelongsTo(Column = "ParentId")]
        public SysMenu ParentMenu { get; set; }

        /// <summary>
        /// 子菜单列表
        ///     一对多关系
        /// </summary>
        [Display(Name = "子菜单列表")]
        [HasMany(ColumnKey = "ParentId")]
        public IList<SysMenu> Children { get; set; }

        /// <summary>
        /// 操作列表
        ///     一对多关系
        /// </summary>
        [Display(Name = "操作列表")]
        [HasMany(ColumnKey = "MenuId")]
        public IList<SysFunction> SysFunctionList { get; set; }
    }
}
