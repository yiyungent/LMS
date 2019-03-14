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

        #region 删除
        public string Delete(int id)
        {
            try
            {
                Container.Instance.Resolve<ClazzService>().Delete(id);
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 修改
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Clazz mo = Container.Instance.Resolve<ClazzService>().GetEntity(id);

            return View(mo);
        }

        [HttpPost]
        public string Edit(Clazz mo)
        {
            try
            {
                Container.Instance.Resolve<ClazzService>().Edit(mo);
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 查看明细
        public ActionResult Details(int id)
        {
            // 1.准备实体
            Clazz mo = Container.Instance.Resolve<ClazzService>().GetEntity(id);
            // 2.返回视图前预处理

            // 3.返回视图
            return View(mo);
        }
        #endregion
    }
}
