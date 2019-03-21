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
    public class SysMenuController : Controller
    {
        #region 列表
        public ActionResult Index()
        {
            // 1.获取全部菜单
            IList<SysMenu> lst = Container.Instance.Resolve<SysMenuService>().GetAll();
            // 2.生成树表数据
            IList<SysMenu> treeData = new List<SysMenu>();
            var firstMenu = from m in lst
                            where m.ParentMenu == null
                            orderby m.SortCode
                            select m;
            foreach (var item in firstMenu)
            {
                AddSelfAndChild(item, treeData, lst);
            }

            return View(treeData); // 强类型视图
        }
        #endregion

        #region 查看明细
        public ActionResult Details(int id)
        {
            SysMenu mo = Container.Instance.Resolve<SysMenuService>().GetEntity(id);

            return View(mo);
        }
        #endregion

        #region 修改
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // 1.准备实体
            SysMenu mo = Container.Instance.Resolve<SysMenuService>().GetEntity(id);
            // 2.返回前预处理
            int parentId = mo.ParentMenu == null ? 0 : mo.ParentMenu.ID;
            ViewBag.ddlParent = InitDDLForParent(mo, parentId);
            // 3.返回视图
            return View(mo);
        }

        [HttpPost]
        public string Edit(SysMenu mo)
        {
            try
            {
                // 1.提交前预处理
                SysMenu dbmo = Container.Instance.Resolve<SysMenuService>().GetEntity(mo.ID);
                // 上级菜单
                if (mo.ParentMenu.ID == 0)
                {
                    mo.ParentMenu = null;
                }
                // 设置修改后的值
                dbmo.Name = mo.Name;
                dbmo.ClassName = mo.ClassName;
                dbmo.ControllerName = mo.ControllerName;
                dbmo.ActionName = mo.ActionName;
                dbmo.SortCode = mo.SortCode;
                dbmo.ParentMenu = mo.ParentMenu;
                // 2.提交数据库
                Container.Instance.Resolve<SysMenuService>().Edit(dbmo);

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
        /// 添加自己及子女的递归方法--生成树表数据
        /// </summary>
        /// <param name="self">当前菜单</param>
        /// <param name="treeData">树表菜单集</param>
        /// <param name="all">全部菜单</param>
        private void AddSelfAndChild(SysMenu self, IList<SysMenu> treeData, IList<SysMenu> all)
        {
            // 1.增加自己
            treeData.Add(self);
            // 2.递归增加子女
            var children = from m in all
                           where m.ParentMenu != null && m.ParentMenu.ID == self.ID
                           orderby m.SortCode
                           select m;
            foreach (var item in children)
            {
                AddSelfAndChild(item, treeData, all);
            }
        }

        private IList<SelectListItem> InitDDLForParent(SysMenu self, int parentId)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();
            ret.Add(new SelectListItem()
            {
                Text = "请选择",
                Value = "0",
                Selected = (0 == parentId),
            });

            IList<SysMenu> all = Container.Instance.Resolve<SysMenuService>().GetAll();
            // 找出自己及后代的ID
            IList<int> idRange = new List<int>();
            GetIdRange(self, idRange, all);

            // not in 查询
            var find = from m in all
                       where idRange.Contains(m.ID) == false
                       select m;
            // 方案一：不考虑层级的实现
            //foreach (var item in find)
            //{
            //    ret.Add(new SelectListItem()
            //    {
            //        Text = item.Name,
            //        Value = item.ID.ToString(),
            //        Selected = (item.ID == parentId)
            //    });
            //}
            // 方案二：考虑层级的实现
            // 1.一级菜单
            var first = from m in find
                        where m.ParentMenu == null
                        orderby m.SortCode
                        select m;
            foreach (var item in first)
            {
                AddSelfAndChildrenForDDL("", item, ret, find.ToList(), parentId);
            }

            return ret;
        }

        private void AddSelfAndChildrenForDDL(string pref, SysMenu self, IList<SelectListItem> target, IList<SysMenu> all, int parentId)
        {
            // 1.添加自己
            target.Add(new SelectListItem()
            {
                Text = pref + self.Name,
                Value = self.ID.ToString(),
                Selected = (self.ID == parentId)
            });
            // 2.递归添加子女
            var child = from m in all
                        where m.ParentMenu != null && m.ParentMenu.ID == self.ID
                        orderby m.SortCode
                        select m;
            foreach (var item in child)
            {
                AddSelfAndChildrenForDDL(pref + "--", item, target, all, parentId);
            }
        }

        /// <summary>
        /// 递归方法：添加自己及其子女
        /// </summary>
        /// <param name="self"></param>
        /// <param name="idRange"></param>
        /// <param name="all"></param>
        private void GetIdRange(SysMenu self, IList<int> idRange, IList<SysMenu> all)
        {
            // 添加自己
            idRange.Add(self.ID);
            // 关于子女循环
            foreach (var item in self.Children)
            {
                // 递归调用---添加自己和自己的子女
                GetIdRange(item, idRange, all);
            }
        }
        #endregion
    }
}
