using Core;
using Domain;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Criterion;
using Comm;
using PagedList;

namespace Webs.Controllers
{
    public class DeliveryFormController : BaseController
    {
        #region 列表
        public ActionResult Index(int pageIndex = 1)
        {
            // 1.准备查询条件
            IList<ICriterion> qryWhere = new List<ICriterion>();
            qryWhere.Add(Expression.In("Status", new int[] { 0, 1 }));
            // 2.获取数据
            IList<TransportOrder> lst = Container.Instance.Resolve<TransportOrderService>().Query(qryWhere);
            // 3.返回视图前预处理
            // 操作权限-----当前账号当前模块的操作权限
            ViewBag.authFunctionList = GetAuthForMenu("DeliveryForm", "Index");

            // 4.返回视图
            return View(lst.ToPagedList(pageIndex, 10)); // 强类型视图
        }
        #endregion

        #region 调度
        [HttpGet]
        public ActionResult Create(int transportOrderId)
        {
            // 1.准备实体
            DeliveryForm mo = new DeliveryForm();
            mo.TransportOrder = Container.Instance.Resolve<TransportOrderService>().GetEntity(transportOrderId);
            // 2.返回前预处理
            int driverId = mo.Driver == null ? 0 : mo.Driver.ID;
            ViewBag.ddlDriver = InitDDLForDriver(driverId);

            int vehicleId = mo.Vehicle == null ? 0 : mo.Vehicle.ID;
            ViewBag.ddlVehicle = InitDDLForVehicle(vehicleId);

            int deliveryTypeId = mo.TransportOrder.DeliveryType == null || mo.TransportOrder.DeliveryType == null ? 0 : mo.TransportOrder.DeliveryType.ID;
            ViewBag.rblDeliveryType = InitRBLForDeliveryType(deliveryTypeId);

            // 3.返回视图
            return View(mo);
        }

        [HttpPost]
        public string Create(DeliveryForm mo)
        {
            try
            {
                // 1.提交前预处理
                mo.CreateTime = DateTime.Now;
                mo.Creator = (SysUser)Session["loginUser"];
                // 2.提交数据库
                // 2.1 保存调度单
                Container.Instance.Resolve<DeliveryFormService>().Create(mo);
                // 2.2 修改托运单状态
                TransportOrder transportOrder = Container.Instance.Resolve<TransportOrderService>().GetEntity(mo.TransportOrder.ID);
                transportOrder.Status = 1;
                Container.Instance.Resolve<TransportOrderService>().Edit(transportOrder);

                // 3.保存成功返回
                return "ok";
            }
            catch (Exception ex)
            {
                // 4.保存失败返回异常
                return ex.Message;
            }
        }
        #endregion

        #region 重新调度
        [HttpGet]
        public ActionResult Edit(int transportOrderId)
        {
            // 1.准备实体
            // 根据托运单ID确定调度单
            DeliveryForm mo = Container.Instance.Resolve<DeliveryFormService>().Query(new List<ICriterion>()
            {
                Expression.Eq("TransportOrder.ID", transportOrderId)
            }).FirstOrDefault();
            //mo.TransportOrder = Container.Instance.Resolve<TransportOrderService>().GetEntity(transportOrderId);

            // 2.返回前预处理
            int driverId = mo.Driver == null ? 0 : mo.Driver.ID;
            ViewBag.ddlDriver = InitDDLForDriver(driverId);

            int vehicleId = mo.Vehicle == null ? 0 : mo.Vehicle.ID;
            ViewBag.ddlVehicle = InitDDLForVehicle(vehicleId);

            int deliveryTypeId = mo.TransportOrder.DeliveryType == null || mo.TransportOrder.DeliveryType == null ? 0 : mo.TransportOrder.DeliveryType.ID;
            ViewBag.rblDeliveryType = InitRBLForDeliveryType(deliveryTypeId);

            // 3.返回视图
            return View(mo);
        }

        [HttpPost]
        public string Edit(DeliveryForm mo)
        {
            try
            {
                // 1.提交前预处理
                mo.CreateTime = DateTime.Now;
                mo.Creator = (SysUser)Session["loginUser"];
                // 2.提交数据库
                // 2.1 保存调度单
                Container.Instance.Resolve<DeliveryFormService>().Edit(mo);
                // 2.2 修改托运单状态
                TransportOrder transportOrder = Container.Instance.Resolve<TransportOrderService>().GetEntity(mo.TransportOrder.ID);
                transportOrder.Status = 1;
                Container.Instance.Resolve<TransportOrderService>().Edit(transportOrder);

                // 3.保存成功返回
                return "ok";
            }
            catch (Exception ex)
            {
                // 4.保存失败返回异常
                return ex.Message;
            }
        }
        #endregion

        #region 查看
        [HttpGet]
        public ActionResult Details(int transportOrderId)
        {
            // 1.准备实体
            // 根据托运单ID确定调度单
            DeliveryForm mo = Container.Instance.Resolve<DeliveryFormService>().Query(new List<ICriterion>()
            {
                Expression.Eq("TransportOrder.ID", transportOrderId)
            }).FirstOrDefault();
            //mo.TransportOrder = Container.Instance.Resolve<TransportOrderService>().GetEntity(transportOrderId);

            // 2.返回前预处理
            int driverId = mo.Driver == null ? 0 : mo.Driver.ID;
            ViewBag.ddlDriver = InitDDLForDriver(driverId);

            int vehicleId = mo.Vehicle == null ? 0 : mo.Vehicle.ID;
            ViewBag.ddlVehicle = InitDDLForVehicle(vehicleId);

            int deliveryTypeId = mo.TransportOrder.DeliveryType == null || mo.TransportOrder.DeliveryType == null ? 0 : mo.TransportOrder.DeliveryType.ID;
            ViewBag.rblDeliveryType = InitRBLForDeliveryType(deliveryTypeId);

            // 3.返回视图
            return View(mo);
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 初始化司机下拉备选项
        ///     不含禁用的司机
        /// </summary>
        private IList<SelectListItem> InitDDLForDriver(int selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();
            // 1. 添加请选择
            ret.Add(new SelectListItem()
            {
                Text = "(请选择)",
                Value = "0",
                Selected = (selectedValue == 0)
            });
            // 2. 添加备选实际
            var all = Container.Instance.Resolve<DriverService>().Query(new List<ICriterion>()
            {
                Expression.Eq("Status", 0)
            });
            foreach (var item in all)
            {
                ret.Add(new SelectListItem()
                {
                    Text = item.Name + item.Telephone,
                    Value = item.ID.ToString(),
                    Selected = (selectedValue == item.ID)
                });
            }

            return ret;
        }

        /// <summary>
        /// 初始化卡车下拉备选项
        ///     不含禁用的卡车
        /// </summary>
        private IList<SelectListItem> InitDDLForVehicle(int selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();
            // 1. 添加请选择
            ret.Add(new SelectListItem()
            {
                Text = "(请选择)",
                Value = "0",
                Selected = (selectedValue == 0)
            });
            // 2. 添加备选司机
            var all = Container.Instance.Resolve<VehicleService>().Query(new List<ICriterion>()
            {
                Expression.Eq("Status", 0)
            });
            foreach (var item in all)
            {
                ret.Add(new SelectListItem()
                {
                    Text = item.VehicleNumber,
                    Value = item.ID.ToString(),
                    Selected = (selectedValue == item.ID)
                });
            }

            return ret;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private IList<SelectListItem> InitRBLForDeliveryType(int selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();
            // 1. 添加请选择
            ret.Add(new SelectListItem()
            {
                Text = "(请选择)",
                Value = "0",
                Selected = (selectedValue == 0)
            });
            // 2. 添加
            var all = Container.Instance.Resolve<DeliveryTypeService>().GetAll();
            foreach (var item in all)
            {
                ret.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.ID.ToString(),
                    Selected = (selectedValue == item.ID)
                });
            }

            return ret;
        }
        #endregion
    }
}
