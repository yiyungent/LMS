using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Domain
{
    /// <summary>
    /// 实体类：客户
    /// </summary>
    [ActiveRecord]
    public class Client : BaseEntity<Client>
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        [Display(Name = "客户名称")]
        [Property(Length = 100, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "联系电话")]
        [Property(Length = 20, NotNull = true)]
        public string Telephone { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [Display(Name = "联系地址")]
        [Property(Length = 50, NotNull = false)]
        public string Address { get; set; }

        /// <summary>
        /// 电子信箱
        /// </summary>
        [Display(Name = "电子信箱")]
        [Property(Length = 50, NotNull = false)]
        public string Email { get; set; }
    }
}
