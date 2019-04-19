using Castle.ActiveRecord;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：报销费用类型
    /// </summary>
    [ActiveRecord]
    public class BillingItemType : BaseEntity<BillingItemType>
    {
        /// <summary>
        /// 报销费用类型名称
        /// </summary>
        [Display(Name = "报销费用类型名称")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        [Display(Name = "排序码")]
        [Property]
        public int SortCode { get; set; }
    }
}
