using Castle.ActiveRecord;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：报销单明细
    /// </summary>
    [ActiveRecord]
    public class BillingItem : BaseEntity<BillingItem>
    {
        /// <summary>
        /// 费用类型
        /// </summary>
        [Display(Name = "费用类型")]
        [BelongsTo(Column = "BillingItemTypeId")]
        public BillingItemType BillingItemType { get; set; }

        /// <summary>
        /// 费用
        /// </summary>
        [Display(Name = "报销费用")]
        [Property(NotNull = true)]
        public decimal Fee { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [Property(Length = 50)]
        public string Remark { get; set; }

        /// <summary>
        /// 报销单
        /// </summary>
        [Display(Name = "报销单")]
        [BelongsTo(Column = "BillingId")]
        public Billing Billing { get; set; }
    }
}
