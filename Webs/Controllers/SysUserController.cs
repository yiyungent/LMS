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
                // 1.提交前预处理
                // 1.1角色
                if (Request["cblRole"].Length > 0)
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

        #region 修改
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // 1.准备实体
            SysUser mo = Container.Instance.Resolve<SysUserService>().GetEntity(id);
            // 2.返回前预处理
            ViewBag.rblStatus = InitRBLForStatus(mo.Status);
            var idRange = from m in mo.SysRoleList
                          select m.ID;
            ViewBag.cblRole = InitCBLForRole(idRange.ToList());
            // 3.返回视图
            return View(mo);
        }

        [HttpPost]
        public string Edit(SysUser mo)
        {
            try
            {
                // 1.提交前预处理
                SysUser dbmo = Container.Instance.Resolve<SysUserService>().GetEntity(mo.ID);
                // 1.1角色
                if (Request["cblRole"].Length > 0)
                {
                    IList<int> idRange = StringHelper.ConvertIdRange(Request["cblRole"]);

                    mo.SysRoleList = Container.Instance.Resolve<SysRoleService>().Query(new List<ICriterion>() { Expression.In("ID", idRange.ToArray()) });

                }
                // 设置修改后的值
                dbmo.Name = mo.Name;
                dbmo.LoginAccount = mo.LoginAccount;
                dbmo.Status = mo.Status;
                dbmo.SysRoleList = mo.SysRoleList;
                // 2.提交数据库
                Container.Instance.Resolve<SysUserService>().Edit(dbmo);

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
