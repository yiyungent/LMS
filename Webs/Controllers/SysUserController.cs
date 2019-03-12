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

namespace Webs.Controllers
{
    public class SysUserController : Controller
    {
        #region 列表
        public ActionResult Index()
        {
            IList<SysUser> lst = Container.Instance.Resolve<SysUserService>().GetAll();


            return View(lst); // 强类型视图
        }
        #endregion

        #region 新增
        [HttpGet]
        public ActionResult Create()
        {
            // 1.准备实体
            SysUser mo = new SysUser();
            // 2.返回前预处理
            ViewBag.rblStatus = InitRBLForStatus(0);
            ViewBag.cblRole = InitCBLForRole(new List<int>());
            // 3.返回视图
            return View(mo);
        }

        [HttpPost]
        public string Create(SysUser mo)
        {
            try
            {
                if (Request["cblRole"].Length > 0)
                {
                    IList<int> idRange = StringHelper.ConvertIdRange(Request["cblRole"]);

                    mo.SysRoleList = Container.Instance.Resolve<SysRoleService>().Query(new List<ICriterion>() { Expression.In("ID", idRange.ToArray()) });

                }
                Container.Instance.Resolve<SysUserService>().Create(mo);
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
        /// 初始化单选备选项-状态
        /// </summary>
        private IList<SelectListItem> InitRBLForStatus(int selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();
            ret.Add(new SelectListItem()
            {
                Text = "正常",
                Value = "0",
                Selected = (selectedValue == 0),
            });
            ret.Add(new SelectListItem()
            {
                Text = "禁用",
                Value = "1",
                Selected = (selectedValue == 1),
            });

            return ret;
        }
        /// <summary>
        /// 初始化多选备选项-角色
        /// </summary>
        private IList<SelectListItem> InitCBLForRole(List<int> selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();
            IList<SysRole> all = Container.Instance.Resolve<SysRoleService>().GetAll();
            foreach (var item in all)
            {
                ret.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.ID.ToString(),
                    Selected = selectedValue.Contains(item.ID)
                });
            }

            return ret;
        }
        #endregion
    }
}
