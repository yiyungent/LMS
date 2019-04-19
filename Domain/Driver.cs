using Castle.ActiveRecord;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：司机
    /// </summary>
    [ActiveRecord]
    public class Driver : BaseEntity<Driver>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "联系电话")]
        [Property(Length = 30, NotNull = true)]
        public string Telephone { get; set; }

        /// <summary>
        /// 状态
        ///     0: 正常
        ///     1: 禁用
        /// </summary>
        [Display(Name = "状态")]
        [Property]
        public int Status { get; set; }

        /// <summary>
        /// 绑定账号
        /// </summary>
        [Display(Name = "绑定账号")]
        [BelongsTo(Column = "SysUserId")]
        public SysUser SysUser { get; set; }

    }
}
