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
    public class BillingController : BaseController
    {
        #region 列表
        public ActionResult Index(int pageIndex = 1)
        {
            // 1. 查询条件
            IList<ICriterion> qryWhere = new List<ICriterion>();

            #region 1.1 司机名字查询--下拉选择司机

            #region 自己所写
            //string qry_DriverId = string.Empty;
            //if (Request["qry_DriverId"] == null)
            //{
            //    if (Session["qry_DriverId"] != null)
            //    {
            //        qry_DriverId = (string)Session["qry_DriverId"];
            //    }
            //}
            //else
            //{
            //    qry_DriverId = Request["qry_DriverId"];
            //    // 将查询条件存入 Session
            //    Session["qry_DriverId"] = qry_DriverId;
            //}
            //if (qry_DriverId.Length > 0)
            //{
            //    // 根据司机ID 查询 其关联 调度单
            //    var deliveryForms = Container.Instance.Resolve<DeliveryFormService>().Query(new List<ICriterion>()
            //    {
            //        Expression.Eq("Driver.ID", qry_DriverId)
            //    });
            //    int[] deliveryFormIdArr = (from m in deliveryForms
            //                               select m.ID).ToArray();
            //    // 根据调运单 查询  报销单
            //    qryWhere.Add(Expression.In("DeliveryForm.ID", deliveryFormIdArr));
            //} 
            #endregion

            #region 老师
            int qryDriverId = 0;
            if (Request["qryDriverId"] == null)
            {
                if (Session["Billing_qryDriverId"] != null)
                {
                    qryDriverId = (int)Session["Billing_qryDriverId"];
                }
            }
            else
            {
                qryDriverId = int.Parse(Request["qryDriverId"]);
                Session["Billing_qryDriverId"] = qryDriverId;
            }
            if (qryDriverId != 0)
            {
                var findDeliverForm = Container.Instance.Resolve<DeliveryFormService>().Query(new List<ICriterion>()
                {
                    Expression.Eq("Driver.ID", qryDriverId)
                });
                var idRangeDeliveryForm = (from m in findDeliverForm
                                           select m.ID).ToArray();
                qryWhere.Add(Expression.In("DeliveryForm.ID", idRangeDeliveryForm));

                // 嵌套查询不行，下面一行代码不行
                //qryWhere.Add(Expression.Eq("DeliveryForm.Driver.ID", qryDriverId));
            }
            ViewBag.ddlDriver = InitDDLForDriver(qryDriverId);
            #endregion

            #endregion

            #region 1.2 报账时间范围（起-止）

            string qryStartDate = string.Empty;
            if (Request["qryStartDate"] == null)
            {
                if (Session["Billing_qryStartDate"] != null)
                {
                    qryStartDate = Session["Billing_qryStartDate"].ToString();
                }
            }
            else
            {
                qryStartDate = Request["qryStartDate"];
                Session["Billing_qryStartDate"] = qryStartDate;
            }
            //if (qryStartDate.Contains("0001"))
            //{
            //    qryStartDate = string.Empty;
            //}
            if (qryStartDate != "")
            {
                if (DateTime.Parse(qryStartDate).Year == 1)
                {
                    qryStartDate = string.Empty;
                }
                else
                {
                    qryWhere.Add(Expression.Ge("CreateDate", DateTime.Parse(qryStartDate)));
                }
            }
            ViewBag.qryStartDate = qryStartDate;

            string qryEndDate = string.Empty;
            if (Request["qryEndDate"] == null)
            {
                if (Session["Billing_qryEndDate"] != null)
                {
                    qryEndDate = Session["Billing_qryEndDate"].ToString();
                }
            }
            else
            {
                qryEndDate = Request["qryEndDate"];
                Session["Billing_qryEndDate"] = qryEndDate;
            }
            if (qryEndDate != "")
            {
                if (DateTime.Parse(qryEndDate).Year == 1)
                {
                    qryEndDate = string.Empty;
                }
                else
                {
                    qryWhere.Add(Expression.Lt("CreateDate", DateTime.Parse(qryEndDate).AddDays(1)));
                }
            }
            ViewBag.qryEndDate = qryEndDate;

            #endregion

            // 2. 查询数据
            IList<Billing> lst = Container.Instance.Resolve<BillingService>().Query(qryWhere);
            // 3. 转换为分页数据并传到视图

            // 返回视图前预处理
            // 操作权限
            ViewBag.authFunctionList = GetAuthForMenu("Billing", "Index");

            return View(lst.ToPagedList<Billing>(pageIndex, 10));
        }
        #endregion

        #region 新增
        [HttpGet]
        public ViewResult Create()
        {
            // 1. 准备实体
            Billing mo = new Billing();
            mo.BillingItemList = mo.BillingItemList ?? new List<BillingItem>();
            mo.Creator = (SysUser)Session["loginUser"];
            // 2. 返回视图前预处理
            // 2.1 调度单
            int deliveryFormId = mo.DeliveryForm == null ? 0 : mo.DeliveryForm.ID;
            ViewBag.ddlDeliveryForm = InitDDLForDeliveryForm(deliveryFormId);
            // 2.2 报销费用类型
            ViewBag.allBillingItemType = Container.Instance.Resolve<BillingItemTypeService>().GetAll();
            // 3. 返回视图
            return View(mo);
        }

        [HttpPost]
        public string Create(Billing mo)
        {
            try
            {
                // 1. 主表部分
                // 1.1 创建人
                //if (Session["LoginUser"] != null)
                //{
                //    mo.Creator = (SysUser)Session["loginUser"];
                //}
                // 1.2 报销时间
                mo.CreateDate = DateTime.Now;

                // 2. 明细表
                mo.BillingItemList = new List<BillingItem>();
                var allBillingItemType = Container.Instance.Resolve<BillingItemTypeService>().GetAll();
                // 最大行数
                int detailRowCount = int.Parse(Request["detailRowCount"]);
                // 关于行循环
                for (int i = 1; i <= detailRowCount; i++)
                {
                    // 检查明细行是否存在
                    if (Request["BillingItemType" + i] == null) continue;
                    // 处理明细行输入
                    BillingItem bi = new BillingItem();
                    if (Request["ID" + i] != null)
                    {
                        // 原有行
                        bi.ID = int.Parse(Request["ID" + i]);
                    }

                    bi.Fee = decimal.Parse(Request["Fee" + i]);
                    bi.Remark = Request["Remark" + i];
                    int billingItemTypeId = int.Parse(Request["BillingItemType" + i]);
                    bi.BillingItemType = (from m in allBillingItemType
                                          where m.ID == billingItemTypeId
                                          select m).FirstOrDefault();
                    // 关联主表与从表
                    bi.Billing = mo;
                    mo.BillingItemList.Add(bi);
                }

                // 3. 提交主表（级联提交明细表）
                Container.Instance.Resolve<BillingService>().Create(mo);

                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 初始化调度单的下拉备选项
        /// </summary>
        private IList<SelectListItem> InitDDLForDeliveryForm(int selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();
            ret.Add(new SelectListItem()
            {
                Text = "(请选择)",
                Value = "0",
                Selected = (selectedValue == 0)
            });
            var all = Container.Instance.Resolve<DeliveryFormService>().GetAll();
            foreach (var item in all)
            {
                if (item.Driver != null && item.TransportDate.Year != 1)
                {
                    ret.Add(new SelectListItem()
                    {
                        Text = string.Format("{0}{1:yyyy-MM-dd}", item.Driver == null ? "" : item.Driver.Name, item.TransportDate),
                        Value = item.ID.ToString(),
                        Selected = (selectedValue == item.ID)
                    });
                }
            }

            return ret;
        }

        /// <summary>
        /// 初始化查询条件司机的备选项
        /// </summary>
        private IList<SelectListItem> InitDDLForDriver(int selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();

            ret.Add(new SelectListItem()
            {
                Text = "(请选择)",
                Value = "0",
                Selected = (selectedValue == 0)
            });

            var all = Container.Instance.Resolve<DriverService>().GetAll();
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
