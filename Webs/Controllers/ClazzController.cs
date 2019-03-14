using Core;
using Domain;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Webs.Controllers
{
    public class ClazzController : Controller
    {
        #region 列表
        public ActionResult Index(int pageIndex = 1, int pageSize = 10)
        {
            IList<Clazz> list = Container.Instance.Resolve<ClazzService>().GetAll();
            ViewBag.TotalCount = list.Count;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;

            var data = (from m in list
                        orderby m.ID ascending
                        select m).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            if (!string.IsNullOrEmpty(Request.Headers["X-PJAX"]) && Convert.ToBoolean(Request.Headers["X-PJAX"]))
            {
                // pjax -- 部分视图
                return PartialView("ClazzTable", data.ToList());
            }
            else
            {
                //  非 pjax -- 完整视图
                return View(data.ToList());
            }
        }
        #endregion

        #region 新增
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public string Create(Clazz mo)
        {
            try
            {
                Container.Instance.Resolve<ClazzService>().Create(mo);

                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}
