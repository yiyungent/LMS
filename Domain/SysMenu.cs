using Castle.ActiveRecord;
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
        /// 上级菜单编号
        /// </summary>
        [Display(Name = "上级菜单编号")]
        [Property]
        public int ParentId { get; set; }
    }
}
