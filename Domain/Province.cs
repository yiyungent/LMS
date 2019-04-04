using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：省份
    /// </summary>
    [ActiveRecord]
    public class Province : BaseEntity<Province>
    {
        /// <summary>
        /// 省份名称
        /// </summary>
        [Display(Name = "省份名称")]
        [Property(Length = 50, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        [Display(Name = "排序码")]
        [Property]
        public int SortCode { get; set; }
    }
}
