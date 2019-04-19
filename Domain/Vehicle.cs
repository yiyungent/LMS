using Castle.ActiveRecord;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：卡车
    /// </summary>
    [ActiveRecord]
    public class Vehicle : BaseEntity<Vehicle>
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "车牌号")]
        [Property(Length = 50, NotNull = true)]
        public string VehicleNumber { get; set; }

        /// <summary>
        /// 所属单位
        /// </summary>
        [Display(Name = "所属单位")]
        [Property(Length = 50, NotNull = true)]
        public string DeptName { get; set; }

        /// <summary>
        /// 车辆型号
        /// </summary>
        [Display(Name = "车辆型号")]
        [Property(Length = 50, NotNull = true)]
        public string VehicleType { get; set; }

        /// <summary>
        /// 核定载重（吨）
        /// </summary>
        [Display(Name = "核定载重（吨）")]
        [Property(NotNull = true, Default = "10")]
        public decimal LoadCount { get; set; }

        /// <summary>
        /// 月折旧（%）
        /// </summary>
        [Display(Name = "月折旧（%）")]
        [Property(NotNull = true, Default = "1.6")]
        public decimal DepreciateRate { get; set; }

        /// <summary>
        /// 载重油耗（升）
        /// </summary>
        [Display(Name = "载重油耗（升）")]
        [Property(NotNull = true, Default = "100")]
        public decimal LoadUseOil { get; set; }

        /// <summary>
        /// 空车油耗（升）
        /// </summary>
        [Display(Name = "空车油耗（升）")]
        [Property(NotNull = true, Default = "20")]
        public decimal EmptyUseOil { get; set; }

        /// <summary>
        /// 发动机号码
        /// </summary>
        [Display(Name = "发动机号码")]
        [Property(Length = 50, NotNull = true)]
        public string GenerateNumber { get; set; }

        /// <summary>
        /// 保单号
        /// </summary>
        [Display(Name = "保单号")]
        [Property(Length = 50, NotNull = false)]
        public string InsuranceNumber { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        [Display(Name = "车架号")]
        [Property(Length = 50, NotNull = true)]
        public string FrameNumber { get; set; }

        /// <summary>
        /// 底盘号
        /// </summary>
        [Display(Name = "底盘号")]
        [Property(Length = 50, NotNull = true)]
        public string ChassisNumber { get; set; }

        /// <summary>
        /// 状态
        ///     0: 正常
        ///     1: 禁用
        /// </summary>
        [Display(Name = "状态")]
        [Property]
        public int Status { get; set; }
    }
}
