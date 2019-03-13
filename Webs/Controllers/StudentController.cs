using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Service;
using Core;
using NHibernate.Criterion;
using Comm;

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

        #region 新增
        [HttpGet]
        public ActionResult Create()
        {
            // 1.准备实体
            Student mo = new Student();
            // 2.返回前预处理
            ViewBag.rblSex = InitRBLForSex(0);

            IList<Clazz> list = Container.Instance.Resolve<ClazzService>().GetAll();
            var clazzsIDAndName = from m in list
                                  select new { m.ID, m.Name };
            Dictionary<int, string> idAndNameDic = new Dictionary<int, string>();
            foreach (int id in clazzsIDAndName.Select(m => m.ID))
            {
                idAndNameDic.Add(id, clazzsIDAndName.Where(m => m.ID == id).First().Name);
            }
            ViewBag.ddlClazz = idAndNameDic;

            // 3.返回视图
            return View(mo);
        }

        [HttpPost]
        public string Create(Student mo)
        {
            try
            {
                mo.Sex = mo.Sex == 0 || mo.Sex == 1 ? mo.Sex : 0;
                int clazzId;
                if (Request["Clazz"] != null && int.TryParse(Request["Clazz"], out  clazzId))
                {
                    mo.Clazz = Container.Instance.Resolve<ClazzService>().GetEntity(clazzId);
                }

                Container.Instance.Resolve<StudentService>().Create(mo);

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
                Container.Instance.Resolve<StudentService>().Delete(id);
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
            Student mo = Container.Instance.Resolve<StudentService>().GetEntity(id);
            // 2.返回视图前预处理

            // 3.返回视图
            return View(mo);
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 初始化单选备选项-性别
        /// </summary>
        private IList<SelectListItem> InitRBLForSex(int selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();
            ret.Add(new SelectListItem()
            {
                Text = "男",
                Value = "0",
                Selected = (selectedValue == 0),
            });
            ret.Add(new SelectListItem()
            {
                Text = "女",
                Value = "1",
                Selected = (selectedValue == 1),
            });

            return ret;
        }
        #endregion
    }
}
