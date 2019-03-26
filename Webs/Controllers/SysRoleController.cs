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
using System.Text;

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

            // 空值替换操作
            //mo.SysMenuList = mo.SysMenuList ?? new List<SysMenu>();
            //var menuIds = from m in mo.SysMenuList
            //              select m.ID;

            mo.SysFunctionList = mo.SysFunctionList ?? new List<SysFunction>();
            var funcIds = from m in mo.SysFunctionList
                          select m.ID;
            ViewBag.AuthIds = string.Join("|", funcIds);
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

        public void GenXmlForMenuTree()
        {
            StringBuilder sb = new StringBuilder();
            // 1.获取全部菜单与操作
            IList<SysMenu> allMenu = Container.Instance.Resolve<SysMenuService>().GetAll();
            IList<SysFunction> allFunction = Container.Instance.Resolve<SysFunctionService>().GetAll();
            // 2.写XML的头
            sb.Append("<?xml version='1.0' encoding='utf-8'?>");
            sb.Append("<tree id='0'>");
            // 3.写 XML 数据区
            // 3.1 确定一级菜单
            var first = from m in allMenu
                        where m.ParentMenu == null
                        orderby m.SortCode
                        select m;
            // 3.2 关于一级菜单循环，递归添加菜单及其子女到sb
            foreach (var item in first)
            {
                AddSelfAndChildren(item, allMenu, allFunction, sb);
            }
            // 4.写 XML 的尾
            sb.Append("</tree>");

            // 5.输出文件流
            Response.ContentType = "text/xml;charset=utf-8";
            Response.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        private void AddSelfAndChildren(SysMenu self, IList<SysMenu> allMenu, IList<SysFunction> allFunction, StringBuilder sb)
        {
            // 1.添加自己
            // 1.1添加 Menu
            sb.Append(string.Format("<item id='m{0}' text='{1}' im0='{2}' im1='{2}' im2='{2}' open='1'>", self.ID, self.Name, "book_titel.gif"));
            // 1.2添加 Function
            var find = from m in allFunction
                       where m.SysMenu != null && m.SysMenu.ID == self.ID
                       select m;
            foreach (var item in find)
            {
                sb.Append(string.Format("<item id='f{0}' text='{1}' im0='{2}' im1='{2}' im2='{2}' open='1' />", item.ID, item.Name, "book_titel.gif"));
            }
            // 2.递归添加子女
            // 2.1筛选子女
            //var children = from m in allMenu
            //               where m.ParentMenu != null && m.ParentMenu.ID == self.ID
            //               orderby m.SortCode
            //               select m;
            var children = self.Children.OrderBy(m => m.SortCode);
            // 2.2关于子女循环，递归调用
            foreach (var item in children)
            {
                AddSelfAndChildren(item, allMenu, allFunction, sb);
            }
            // 3.关闭自己
            sb.Append("</item>");
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
