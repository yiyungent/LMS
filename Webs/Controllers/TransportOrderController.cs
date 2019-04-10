using Core;
using Domain;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Service;

namespace Webs.Controllers
{
    public class TransportOrderController : BaseController
    {
        #region 列表
        public ActionResult Index(int pageIndex = 1)
        {
            // 1.准备查询条件
            IList<ICriterion> qryWhere = new List<ICriterion>();

            #region 1.1按名称模糊查询
            // 1.1按名称模糊查询

            // 1.1.1 开始站查询
            #region 1.1.1 开始站查询
            string startStation_QryName = string.Empty;
            if (Request["startStation_QryName"] == null)
            {
                if (Session["startStation_QryName"] != null)
                {
                    startStation_QryName = (string)Session["startStation_QryName"];
                }
            }
            else
            {
                startStation_QryName = Request["startStation_QryName"];
                // 将查询条件存入 Session
                Session["startStation_QryName"] = startStation_QryName;
            }
            qryWhere.Add(Expression.Like("StartStation", startStation_QryName, MatchMode.Anywhere));
            #endregion

            // 1.1.2 终止站查询
            #region 1.1.2 终止站查询
            string endStation_QryName = string.Empty;
            if (Request["endStation_QryName"] == null)
            {
                if (Session["endStation_QryName"] != null)
                {
                    startStation_QryName = (string)Session["endStation_QryName"];
                }
            }
            else
            {
                startStation_QryName = Request["endStation_QryName"];
                // 将查询条件存入 Session
                Session["endStation_QryName"] = startStation_QryName;
            }
            qryWhere.Add(Expression.Like("EndStation", startStation_QryName, MatchMode.Anywhere));
            #endregion

            #region 1.1.2 收货人(名字)查询
            //// 1.1.2 收货人(名字)查询
            //string receiverName_QryName = string.Empty;
            //if (Request["receiverName_QryName"] == null)
            //{
            //    if (Session["receiverName_QryName"] != null)
            //    {
            //        receiverName_QryName = (string)Session["receiverName_QryName"];
            //    }
            //}
            //else
            //{
            //    receiverName_QryName = Request["receiverName_QryName"];
            //    // 将查询条件存入 Session
            //    Session["receiverName_QryName"] = receiverName_QryName;
            //}

            //var clients = Container.Instance.Resolve<ClientService>().Query(new List<ICriterion>()
            //{
            //    Expression.Like("Name", receiverName_QryName, MatchMode.Anywhere)
            //});

            //List<int> receiverIdList = (from m in clients
            //                            select m.ID).ToList();
            //if (!(string.IsNullOrEmpty(Request["receiverName_QryName"]) && receiverIdList.Count == 0))
            //{
            //    // 非首次加载---输入了收货人
            //    qryWhere.Add(Expression.In("Receiver.ID", receiverIdList));
            //}
            #endregion

            #region 托运人或收货人模糊查找
            // 1.3 托运人或收货人模糊查找 
            string qryName = string.Empty;
            if (Request["qryName"] == null)
            {
                if (Session["TransportOrder_qryName"] != null)
                {
                    qryName = (string)Session["TransportOrder_qryName"];
                }
            }
            else
            {
                qryName = Request["TransportOrder_qryName"];
                // 将查询条件存入 Session
                Session["TransportOrder_qryName"] = qryName;
            }
            if (qryName.Length > 0)
            {
                // 嵌套属性查询-主键属性才能直接嵌套
                // 非主键属性 需转换为主键属性
                IList<Client> findClient = Container.Instance.Resolve<ClientService>().Query(new List<ICriterion>()
                {
                    Expression.Like("Name", qryName, MatchMode.Anywhere)
                });
                var idRange = from m in findClient select m.ID;

                qryWhere.Add(Expression.Or(Expression.In("Customer.ID", idRange.ToArray()), Expression.In("Receiver.ID", idRange.ToArray())));
            }
            ViewBag.qryName = qryName;
            #endregion

            // 2.获取数据
            IList<TransportOrder> lst = Container.Instance.Resolve<TransportOrderService>().Query(qryWhere);
            // 3.返回视图前预处理
            ViewBag.startStation_QryName = startStation_QryName;
            ViewBag.endStation_QryName = endStation_QryName;
            //ViewBag.receiverName_QryName = receiverName_QryName;
            // 操作权限-----当前账号当前模块的操作权限
            ViewBag.authFunctionList = GetAuthForMenu("TransportOrder", "Index");

            return View(lst.ToPagedList(pageIndex, 10)); // 强类型视图
        }
            #endregion

        #region 修改
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // 1.准备实体
            TransportOrder mo = Container.Instance.Resolve<TransportOrderService>().GetEntity(id);
            // 2.返回视图前预处理
            // 2.1 省市
            int provinceId = mo.Province == null ? 0 : mo.Province.ID;
            ViewBag.ddlProvince = InitDDLForProvince(provinceId);
            // 2.2 运输方式 DeliverType
            int deliverTypeId = mo.DeliveryType == null ? 0 : mo.DeliveryType.ID;
            ViewBag.rblDeliverType = InitRBLForDeliverType(deliverTypeId);
            // 2.3 托运人
            int customerId = mo.Customer == null ? 0 : mo.Customer.ID;
            ViewBag.ddlCustomer = InitDDLForCustomer(customerId);
            // 2.4 收货人
            int receiverId = mo.Receiver == null ? 0 : mo.Receiver.ID;
            ViewBag.ddlReceiver = InitDDLForCustomer(receiverId);
            // 2.5 状态
            ViewBag.ddlStatus = InitDDLForStatus(mo.Status);
            // 2.6 全部客户信息
            ViewBag.allClientInfo = InitStrForClient();

            // 3.返回视图
            return View(mo);
        }

        [HttpPost]
        public string Edit(TransportOrder mo)
        {
            try
            {
                // 1.1 明细表
                // 提交前预处理
                mo.TransportOrderItemList = new List<TransportOrderItem>();
                // 明细行最大行数
                int detailRowCount = int.Parse(Request["detailRowCount"]);

                TransportOrderItem toi = new TransportOrderItem();
                for (int i = 1; i <= detailRowCount; i++)
                {
                    // 根据货物名称判断是否被删除
                    if (Request["CargoName" + i] == null) continue;

                    if (Request["ID" + i] == null)
                    {
                        // 新增行

                    }
                    else
                    {
                        // 原有行
                        toi.ID = int.Parse(Request["ID" + i]);
                    }
                    toi.CargoName = Request["CargoName" + i];
                    toi.CabinetSort = Request["CabinetSort" + i];
                    toi.CabinetType = Request["CabinetType" + i];
                    toi.Amount = int.Parse(Request["Amount" + i]);
                    toi.CabinetNumber = Request["CabinetNumber" + i];
                    toi.SealedNumber = Request["SealedNumber" + i];
                    toi.TransportFee = double.Parse(Request["TransportFee" + i]);

                    // 添加到明细列表
                    mo.TransportOrderItemList.Add(toi);
                }
                // 1.2 创建人
                if (Session["LoginUser"] != null)
                {
                    mo.Creator = (SysUser)Session["LoginUser"];
                }
                // 1.3 创建时间
                mo.CreateDate = DateTime.Now;
                // 提交主表, 级联提交明细表
                Container.Instance.Resolve<TransportOrderService>().Edit(mo);

                return "ok";
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }
        #endregion


        #region 辅助方式
        /// <summary>
        /// 全部客户信息的字符串格式
        ///     客户之间分隔符: $
        ///     客户的属性之间分割符: |
        /// </summary>
        private string InitStrForClient()
        {
            string ret = string.Empty;
            IList<Client> all = Container.Instance.Resolve<ClientService>().GetAll();
            IList<string> lst = new List<string>();
            foreach (var item in all)
            {
                lst.Add(string.Format("{0}|{1}|{2}|{3}", item.ID, item.Telephone, item.Address, item.Email));
            }
            ret = string.Join("$", lst);

            return ret;
        }
        /// <summary>
        /// 初始化状态下拉备选项
        /// </summary>
        private IList<SelectListItem> InitDDLForStatus(int selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();
            string[] arr = new string[] { "待调度", "待运输", "待报账", "结束" };
            for (int i = 0; i < arr.Length; i++)
            {
                ret.Add(new SelectListItem()
                {
                    Text = arr[i],
                    Value = i.ToString(),
                    Selected = (selectedValue == i)
                });
            }

            #region 废弃
            // ret.Add(new SelectListItem()
            //    {
            //        Text = "待调度",
            //        Value = "0",
            //        Selected = (selectedValue == 0)
            //    });
            // ret.Add(new SelectListItem()
            //{
            //    Text = "待运输",
            //    Value = "1",
            //    Selected = (selectedValue == 1)
            //});
            // ret.Add(new SelectListItem()
            //{
            //    Text = "待报账",
            //    Value = "2",
            //    Selected = (selectedValue == 2)
            //});
            // ret.Add(new SelectListItem()
            //{
            //    Text = "结束",
            //    Value = "3",
            //    Selected = (selectedValue == 3)
            //});
            #endregion

            return ret;
        }

        /// <summary>
        /// 初始化托运人或收货人下拉备选项
        /// </summary>
        private IList<SelectListItem> InitDDLForCustomer(int selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();

            ret.Add(new SelectListItem()
            {
                Text = "(请选择)",
                Value = "0",
                Selected = (selectedValue == 0)
            });
            var all = Container.Instance.Resolve<ClientService>().GetAll();
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

        /// <summary>
        /// 初始化运输方式单选备选项
        /// </summary>
        private IList<SelectListItem> InitRBLForDeliverType(int selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();

            ret.Add(new SelectListItem()
            {
                Text = "(请选择)",
                Value = "0",
                Selected = (selectedValue == 0)
            });
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

        /// <summary>
        /// 初始化省市下拉备选项
        /// </summary>
        private IList<SelectListItem> InitDDLForProvince(int selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();

            ret.Add(new SelectListItem()
            {
                Text = "(请选择)",
                Value = "0",
                Selected = (selectedValue == 0)
            });
            var all = Container.Instance.Resolve<ProvinceService>().GetAll();
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
