using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Webs.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 1.获取路由的控制器和动作
            string controllerName = filterContext.Controller.ToString();
            string actionName = filterContext.ActionDescriptor.ActionName;
            // 2.权限检查
            if (controllerName.Contains("SysUserController") && actionName == "Login")
            {
                // 2.1登录不做权限检查
                base.OnActionExecuting(filterContext);
            }
            else
            {
                // 2.2要做权限检查
                // 获取登录用户
                if (Session["loginUser"] == null)
                {
                    // 跳转登录
                    Response.Redirect("/SysUser/Login");
                }

                SysUser loginUser = (SysUser)Session["loginUser"];
                // 获取菜单权限
                if (HasAuth(loginUser, controllerName, actionName))
                {
                    // 有权限
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    // 无权限
                    //Response.Write("<script>window.location.href='/SysUser/Login';</script>");
                    Response.Redirect("/SysUser/Login");
                }
            }
        }

        /// <summary>
        /// 判断用户对指定控制器与动作路由是否有权限
        /// </summary>
        private bool HasAuth(SysUser loginUser, string controllerName, string actionName)
        {
            bool ret = false;
            foreach (var role in loginUser.SysRoleList)
            {
                foreach (var menu in role.SysMenuList)
                {
                    if (menu.ClassName != null && menu.ClassName.ToUpper() == controllerName.ToUpper() && menu.ActionName == actionName)
                    //if (string.Equals(menu.ClassName, controllerName, StringComparison.OrdinalIgnoreCase) && menu.ActionName == actionName)
                    {
                        ret = true;
                        break;
                    }
                }
            }
            return ret;
        }
    }
}
