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

        #region 新增
        [HttpGet]
        public ActionResult Create()
        {
            // 1.准备实体
            SysRole mo = new SysRole();
            // 2.返回前预处理
            ViewBag.rblStatus = InitRBLForStatus(0);
            // 3.返回视图
            return View(mo);
        }

        [HttpPost]
        public string Create(SysUser mo)
        {
            try
            {
                // 1.提交前预处理
                // 1.1角色
                if (Request["cblRole"] != null && Request["cblRole"].Length > 0)
                {
                    IList<int> idRange = StringHelper.ConvertIdRange(Request["cblRole"]);

                    mo.SysRoleList = Container.Instance.Resolve<SysRoleService>().Query(new List<ICriterion>() { Expression.In("ID", idRange.ToArray()) });

                }
                // 1.2密码
                mo.Password = StringHelper.EncodeMD5(mo.Password);
                // 2.提交数据库
                Container.Instance.Resolve<SysUserService>().Create(mo);

                // 3.保存成功返回
                return "ok";
            }
            catch (Exception ex)
            {
                // 4.保存失败返回异常
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
