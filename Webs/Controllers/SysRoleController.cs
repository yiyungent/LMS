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
    public class SysRoleController : Controller
    {
        #region 列表
        public ActionResult Index(int pageIndex = 1)
        {
            // 1.准备查询条件
            IList<ICriterion> qryWhere = new List<ICriterion>();
            // 1.1按名称模糊查询
            string qryName = string.Empty;
            if (Request["qryName"] == null)
            {
                if (Session["role_qryName"] != null)
                {
                    qryName = (string)Session["role_qryName"];
                }
            }
            else
            {
                qryName = Request["qryName"];
                // 将查询条件存入 Session
                Session["role_qryName"] = qryName;
            }
            qryWhere.Add(Expression.Like("Name", qryName, MatchMode.Anywhere));
            // 2.获取数据
            IList<SysRole> lst = Container.Instance.Resolve<SysRoleService>().Query(qryWhere);
            ViewBag.qryName = qryName;
            // 3.返回视图
            return View(lst.ToPagedList(pageIndex, 10)); // 强类型视图
        }
        #endregion

    }
}
