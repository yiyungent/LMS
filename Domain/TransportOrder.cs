using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：货运单
    /// </summary>
    [ActiveRecord]
    public class TransportOrder : BaseEntity<TransportOrder>
    {
        /// <summary>
        /// 货运发站
        /// </summary>
        [Display(Name = "货运发站")]
        [Property(Length = 30, NotNull = false)]
        public string StartStation { get; set; }


        /// <summary>
        /// 货运到站
        /// </summary>
        [Display(Name = "货运到站")]
        [Property(Length = 30, NotNull = true)]
        public string EndStation { get; set; }

        /// <summary>
        /// 省市
        ///     多对一
        /// </summary>
        [Display(Name = "省市")]
        [BelongsTo(Column = "ProvinceId")]
        public Province Province { get; set; }

        /// <summary>
        /// 运输方式
        ///     多对一
        /// </summary>
        [Display(Name = "运输方式")]
        [BelongsTo(Column = "DeliveryTypeId")]
        public DeliveryType DeliveryType { get; set; }

        /// <summary>
        /// 发货地点
        /// </summary>
        [Display(Name = "发货地点")]
        [Property(Length = 30, NotNull = true)]
        public string StartPlace { get; set; }

        /// <summary>
        /// 交货地点
        /// </summary>
        [Display(Name = "交货地点")]
        [Property(Length = 30, NotNull = true)]
        public string EndPlace { get; set; }

        /// <summary>
        /// 托运人
        ///     多对一
        /// </summary>
        [Display(Name = "托运人")]
        [BelongsTo(Column = "CustomerId")]
        public Client Customer { get; set; }

        /// <summary>
        /// 收货人
        ///     多对一
        /// </summary>
        [Display(Name = "收货人")]
        [BelongsTo(Column = "ReceiverId")]
        public Client Receiver { get; set; }

        /// <summary>
        /// 托运人记载事项
        /// </summary>
        [Display(Name = "托运人记载事项")]
        [Property(Length = 1000, NotNull = true)]
        public string CustomerRemark { get; set; }

        /// <summary>
        /// 承运人记载事项
        /// </summary>
        [Display(Name = "承运人记载事项")]
        [Property(Length = 1000, NotNull = true)]
        public string TransportRemark { get; set; }

        /// <summary>
        /// 货物价格
        /// </summary>
        [Display(Name = "货物价格")]
        [Property]
        public double Price { get; set; }

        /// <summary>
        /// 创建人
        ///     多对一
        /// </summary>
        [Display(Name = "创建人")]
        [BelongsTo(Column = "CreatorId")]
        public SysUser Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Property(NotNull = true)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        [Display(Name = "附件")]
        [Property(Length = 1000)]
        public string AttachedFile { get; set; }

        /// <summary>
        /// 货运单状态
        ///     0 接单
        ///     1 已调度
        ///     2 已运输
        ///     3 已报账
        /// </summary>
        [Display(Name = "货运单状态")]
        [Property(NotNull = false)]
        public int Status { get; set; }

        /// <summary>
        /// 托运单明细
        ///     一对多
        /// </summary>
        [HasMany(ColumnKey = "TransportOrderId")]
        public IList<TransportOrderItem> TransportOrderItemList { get; set; }
    }
}
