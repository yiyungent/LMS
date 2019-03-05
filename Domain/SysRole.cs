using Castle.ActiveRecord;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类: 角色
    /// </summary>
    [ActiveRecord]
    public class SysRole : BaseEntity<SysRole>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Display(Name = "角色名")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 状态
        ///     0: 正常
        ///     1: 禁用
        /// </summary>
        [Display(Name = "状态")]
        [Property]
        public int Status { get; set; }
    }
}
