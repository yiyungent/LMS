using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.ActiveRecord;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类： 操作
    /// </summary>
    [ActiveRecord]
    public class SysFunction : BaseEntity<SysFunction>
    {
        /// <summary>
        /// 操作名称
        /// </summary>
        [Display(Name = "操作名称")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 系统菜单
        ///     多对一关系
        /// </summary>
        [Display(Name = "系统菜单")]
        [BelongsTo(Column = "MenuId")]
        public SysMenu SysMenu { get; set; }
    }
}
