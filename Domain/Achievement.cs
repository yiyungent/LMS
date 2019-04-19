using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：货运单产值
    /// </summary>
    [ActiveRecord]
    public class Achievement : BaseEntity<Achievement>
    {
        /// <summary>
        /// 托运单
        /// </summary>
        [Display(Name = "托运单")]
        [BelongsTo(Column = "TransportOrderId")]
        public TransportOrder TransportOrder { get; set; }

        /// <summary>
        /// 收入
        /// </summary>
        [Display(Name = "收入")]
        [Property(NotNull = true)]
        public decimal Income { get; set; }

        /// <summary>
        /// 调度单
        /// </summary>
        [Display(Name = "调度单")]
        [BelongsTo(Column = "DeliveryFormId")]
        public DeliveryForm DeliveryForm { get; set; }

        /// <summary>
        /// 报销单
        /// </summary>
        [Display(Name = "报销单")]
        [BelongsTo(Column = "BillingId")]
        public Billing Billing { get; set; }

        /// <summary>
        /// 支出
        /// </summary>
        [Display(Name = "支出")]
        [Property(NotNull = true)]
        public decimal Payment { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "创建人")]
        [BelongsTo(Column = "CreateUserId")]
        public SysUser Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Property(NotNull = true)]
        public DateTime CreateDate { get; set; }
    }
}
