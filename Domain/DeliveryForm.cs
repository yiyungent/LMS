using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：调度单
    /// </summary>
    [ActiveRecord]
    public class DeliveryForm : BaseEntity<DeliveryForm>
    {
        /// <summary>
        /// 运输时间
        /// </summary>
        [Display(Name = "运输时间")]
        [Property(NotNull = true)]
        public DateTime TransportDate { get; set; }

        /// <summary>
        /// 运输司机
        /// </summary>
        [Display(Name = "运输司机")]
        [BelongsTo(Column = "DriverId")]
        public Driver Driver { get; set; }

        /// <summary>
        /// 运输卡车
        /// </summary>
        [Display(Name = "运输卡车")]
        [BelongsTo(Column = "VehicleId")]
        public Vehicle Vehicle { get; set; }

        /// <summary>
        /// 托运单
        /// </summary>
        [Display(Name = "托运单")]
        [BelongsTo(Column = "TransportOrderId")]
        public TransportOrder TransportOrder { get; set; }

        /// <summary>
        /// 调度时间
        /// </summary>
        [Display(Name = "调度时间")]
        [Property(NotNull = true)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 调度员
        /// </summary>
        [Display(Name = "调度员")]
        [BelongsTo(Column = "DeliveryUserId")]
        public SysUser Creator { get; set; }
    }
}
