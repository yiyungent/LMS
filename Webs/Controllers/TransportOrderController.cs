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

            // 1.1.2 收货人(名字)查询
            #region 1.1.2 收货人(名字)查询
            string receiverName_QryName = string.Empty;
            if (Request["receiverName_QryName"] == null)
            {
                if (Session["receiverName_QryName"] != null)
                {
                    receiverName_QryName = (string)Session["receiverName_QryName"];
                }
            }
            else
            {
                receiverName_QryName = Request["receiverName_QryName"];
                // 将查询条件存入 Session
                Session["receiverName_QryName"] = receiverName_QryName;
            }

            var clients = Container.Instance.Resolve<ClientService>().Query(new List<ICriterion>()
            {
                Expression.Like("Name", receiverName_QryName, MatchMode.Anywhere)
            });

            List<int> receiverIdList = (from m in clients
                                        select m.ID).ToList();
            if (!(string.IsNullOrEmpty(Request["receiverName_QryName"]) && receiverIdList.Count == 0))
            {
                // 非首次加载---输入了收货人
                qryWhere.Add(Expression.In("Receiver.ID", receiverIdList));
            }
            #endregion

            #endregion

            // 2.获取数据
            IList<TransportOrder> lst = Container.Instance.Resolve<TransportOrderService>().Query(qryWhere);
            // 3.返回视图前预处理
            ViewBag.startStation_QryName = startStation_QryName;
            ViewBag.endStation_QryName = endStation_QryName;
            ViewBag.receiverName_QryName = receiverName_QryName;
            // 操作权限-----当前账号当前模块的操作权限
            ViewBag.authFunctionList = GetAuthForMenu("SysUser", "Index");

            return View(lst.ToPagedList(pageIndex, 10)); // 强类型视图
        }
        #endregion

    }
}
