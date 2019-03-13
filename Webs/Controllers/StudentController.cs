using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Service;
using Core;
using NHibernate.Criterion;

namespace Webs.Controllers
{
    public class StudentController : Controller
    {
        #region 列表
        public ActionResult Index(int pageIndex = 1, int pageSize = 10)
        {
            IList<Student> list = Container.Instance.Resolve<StudentService>().GetAll();
            ViewBag.TotalCount = list.Count;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;

            var data = (from m in list
                        orderby m.ID ascending
                        select m).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            if (!string.IsNullOrEmpty(Request.Headers["X-PJAX"]) && Convert.ToBoolean(Request.Headers["X-PJAX"]))
            {
                // pjax -- 部分视图
                return PartialView("StudentTable", data.ToList());
            }
            else
            {
                //  非 pjax -- 完整视图
                return View(data.ToList());
            }
        }
        #endregion
    }
}
