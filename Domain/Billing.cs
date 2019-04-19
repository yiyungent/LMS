using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：报销单
    /// </summary>
    [ActiveRecord]
    public class Billing : BaseEntity<Billing>
    {
        /// <summary>
        /// 报销单时间
        /// </summary>
        [Display(Name = "报销单时间")]
        [Property(NotNull = true)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "创建人")]
        [BelongsTo(Column = "CreateUserId")]
        public SysUser Creator { get; set; }

        /// <summary>
        /// 调度单
        /// </summary>
        [Display(Name = "调度单")]
        [BelongsTo(Column = "DeliveryFormId")]
        public DeliveryForm DeliveryForm { get; set; }

        /// <summary>
        /// 报销单明细
        ///     一对多
        ///     级联操作明细表
        /// </summary>
        [Display(Name = "报销单明细")]
        [HasMany(ColumnKey = "BillingId", Cascade = ManyRelationCascadeEnum.All)]
        public IList<BillingItem> BillingItemList { get; set; }
    }
}
