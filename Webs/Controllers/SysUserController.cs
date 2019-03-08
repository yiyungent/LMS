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
            // 3.返回视图
            return View(mo);
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
        #endregion
    }
}
