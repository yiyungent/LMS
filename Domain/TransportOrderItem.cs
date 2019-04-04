using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：托运单明细
    /// </summary>
    [ActiveRecord]
    public class TransportOrderItem : BaseEntity<TransportOrderItem>
    {
        /// <summary>
        /// 货物品名
        /// </summary>
        [Property(Length = 30, NotNull = true)]
        [Display(Name = "货物品名")]
        public string CargoName { get; set; }

        /// <summary>
        /// 集装箱箱型
        /// </summary>
        [Property(Length = 30, NotNull = true)]
        [Display(Name = "集装箱箱型")]
        public string CabinetSort { get; set; }

        /// <summary>
        /// 集装箱箱类
        /// </summary>
        [Property(Length = 30, NotNull = true)]
        [Display(Name = "集装箱箱类")]
        public string CabinetType { get; set; }

        /// <summary>
        /// 集装箱数量
        /// </summary>
        [Property]
        [Display(Name = "集装箱数量")]
        public int Amount { get; set; }

        /// <summary>
        /// 集装箱号码
        /// </summary>
        [Property(Length = 30, NotNull = true)]
        [Display(Name = "集装箱号码")]
        public string CabinetNumber { get; set; }

        /// <summary>
        /// 施封号码
        /// </summary>
        [Property(Length = 30, NotNull = true)]
        [Display(Name = "施封号码")]
        public string SealedNumber { get; set; }

        /// <summary>
        /// 运输费用
        /// </summary>
        [Property]
        [Display(Name = "运输费用")]
        public double TransportFee { get; set; }


        /// <summary>
        /// 托运单
        ///     多对一
        /// </summary>
        [BelongsTo(Column = "TransportOrderId")]
        [Display(Name = "托运单")]
        public TransportOrder TransportOrder { get; set; }
    }
}
