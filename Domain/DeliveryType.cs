using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：运输方式
    /// </summary>
    [ActiveRecord]
    public class DeliveryType : BaseEntity<DeliveryType>
    {
        /// <summary>
        /// 运输方式名称
        /// </summary>
        [Display(Name = "运输方式名称")]
        [Property(Length = 20, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        [Display(Name = "排序码")]
        [Property]
        public int SortCode { get; set; }
    }
}
