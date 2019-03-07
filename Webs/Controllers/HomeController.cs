using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Castle.ActiveRecord;
using Domain;
using Service;
using Core;

namespace Webs.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        #region 初始化数据库
        public ActionResult InitDB()
        {
            return View();
        }
        public ActionResult StartInitDB()
        {
            CreateDB();
            return View("InitDB");
        }

        private void CreateDB()
        {
            CreateSchema();
            InitUser();
        }

        #region 初始化用户
        private void InitUser()
        {
            try
            {
                Response.Write(".........初始化用户<br/>");
                //SysUser sysUser = new SysUser();
                //sysUser.Name = "系统管理员";
                //sysUser.LoginAccount = "admin";
                //sysUser.Password = "123456";
                //sysUser.Status = 0;
                //Container.Instance.Resolve<SysUserService>().Create(sysUser);

                //SysUser sysUser = new SysUser()
                //{
                //    Name = "系统管理员",
                //    LoginAccount = "admin",
                //    Password = "123456",
                //    Status = 0
                //};
                //Container.Instance.Resolve<SysUserService>().Create(sysUser);

                Container.Instance.Resolve<SysUserService>().Create(new SysUser()
                {
                    Name = "系统管理员",
                    LoginAccount = "admin",
                    Password = "123456",
                    Status = 0
                });

                Container.Instance.Resolve<SysUserService>().Create(new SysUser()
                {
                    Name = "莫宇",
                    LoginAccount = "my",
                    Password = "123456",
                    Status = 0
                });

                Response.Write(".........初始化用户ok<br/>");
            }
            catch (Exception ex)
            {
                Response.Write(".........初始化用户Error<br/>");
            }
        }
        #endregion

        #region 创建数据库结构
        private void CreateSchema()
        {
            try
            {
                Response.Write("开始创建数据库结构<br/>");
                ActiveRecordStarter.CreateSchema();
                Response.Write("......成功<br/>");
            }
            catch (Exception ex)
            {
                //Response.Write("......失败！原因：" + ex.Message + "<br/>");
                Response.Write(string.Format("......失败！原因： {0}<br/>", ex.Message));
            }
        }
        #endregion

        #endregion

    }
}
