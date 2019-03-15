using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Castle.ActiveRecord;
using Domain;
using Service;
using Core;
using Comm;

namespace Webs.Controllers
{
    public class HomeController : Controller
    {
        #region 首页
        public ActionResult Index()
        {
            return View();
        } 
        #endregion

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
            InitMenu();
            InitUser();
            InitRole();
            InitClazz();
            InitStudent();
        }

        #region 初始化学生
        private void InitStudent()
        {
            #region 生成 1700103001 - 1700103100 学生
            int clazzId;
            for (int i = 1; i <= 100; i++)
            {
                clazzId = new Random().Next(1, 90);
                Container.Instance.Resolve<StudentService>().Create(new Student
                {
                    Name = "学生" + i,
                    Sex = i % 2,
                    Clazz = Container.Instance.Resolve<ClazzService>().GetEntity(clazzId),
                    StudyNumber = "1700103" + i.ToString("000"),
                    Mobile = "11320" + i.ToString("000000")
                });
            }
            #endregion

            string[] names = { "张三", "李四", "王五" };
            int[] sexs = { 0, 1, 0 };
            for (int i = 0; i < names.Length; i++)
            {
                Clazz clazz = Container.Instance.Resolve<ClazzService>().GetEntity(1);

                // 左补0
                string str = (i < 9) ? "0" + (i + 1) : (i + 1).ToString();

                Container.Instance.Resolve<StudentService>().Create(new Student()
                {
                    Name = names[i],
                    Sex = sexs[i],

                    #region 左补0其它方法
                    // 左补0, 这里最终2位
                    // 1.
                    //StudyNumber = "1700103" + i.ToString().PadLeft(2, '0'),
                    // 2.
                    //StudyNumber = "1700103" + string.Format("{0:d2}", i),
                    // 3.
                    //StudyNumber = "1700103" + i.ToString("00"), 
                    // 4.
                    //StudyNumber = "1700103" + i.ToString("D2"),
                    #endregion

                    StudyNumber = str,
                    Mobile = "15098443993",
                    Clazz = clazz
                });
            }
        }
        #endregion

        #region 初始化班级
        private void InitClazz()
        {
            #region 生成 1700101 - 1700190 班级
            for (int i = 1; i <= 90; i++)
            {
                Container.Instance.Resolve<ClazzService>().Create(new Clazz()
                {
                    Name = string.Format("17001{0}班", i.ToString("00"))
                });
            }
            #endregion
        }
        #endregion

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
                    Password = StringHelper.EncodeMD5("123456"),
                    Status = 0
                });

                Container.Instance.Resolve<SysUserService>().Create(new SysUser()
                {
                    Name = "莫宇",
                    LoginAccount = "my",
                    Password = StringHelper.EncodeMD5("123456"),
                    Status = 0
                });

                for (int i = 0; i < 100; i++)
                {
                    Container.Instance.Resolve<SysUserService>().Create(new SysUser()
                    {
                        Name = "用户" + i,
                        LoginAccount = "user" + i,
                        Password = StringHelper.EncodeMD5("123456"),
                        Status = 0
                    });
                }
                Response.Write(".........初始化用户ok<br/>");
            }
            catch (Exception ex)
            {
                Response.Write(".........初始化用户Error<br/>");
            }
        }
        #endregion

        #region 初始化角色
        private void InitRole()
        {
            try
            {
                Response.Write(".........初始化角色<br/>");

                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "系统管理员",
                    Status = 0
                });
                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "接单员",
                    Status = 0
                });
                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "调度员",
                    Status = 0
                });
                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "财务员",
                    Status = 0
                });
                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "驾驶员",
                    Status = 0
                });


                Response.Write(".........初始化角色ok<br/>");
            }
            catch (Exception ex)
            {
                Response.Write(".........初始化角色Error<br/>");
            }
        }
        #endregion

        #region 初始化菜单
        private void InitMenu()
        {
            try
            {
                Response.Write(".........初始化菜单<br/>");
                #region 一级菜单
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "系统管理",
                    SortCode = 10,
                });
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "业务管理",
                    SortCode = 20,
                });
                #endregion

                SysMenu parentMenu = null;
                #region 系统管理的二级菜单
                parentMenu = Container.Instance.Resolve<SysMenuService>().GetEntity(1);
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "菜单管理",
                    ClassName = "Webs.Controllers.SysMenuController",
                    ControllerName = "SysMenu",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 10,
                });
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "用户管理",
                    ClassName = "Webs.Controllers.SysUserController",
                    ControllerName = "SysUser",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 20,
                });
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "角色管理",
                    ClassName = "Webs.Controllers.SysRoleController",
                    ControllerName = "SysRole",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 30,
                });
                #endregion

                #region 业务的二级菜单
                parentMenu = Container.Instance.Resolve<SysMenuService>().GetEntity(2);
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "接单管理",
                    ClassName = "Webs.Controllers.TransportOrderController",
                    ControllerName = "TransportOrder",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 10,
                });
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "调度管理",
                    ClassName = "Webs.Controllers.DeliveryFormController",
                    ControllerName = "DeliveryForm",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 20,
                });
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "回车报销管理",
                    ClassName = "Webs.Controllers.BillingController",
                    ControllerName = "Billing",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 30,
                });
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "产值分析",
                    ClassName = "Webs.Controllers.AchievementController",
                    ControllerName = "Achievement",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 40,
                });
                #endregion
                Response.Write(".........初始化菜单ok<br/>");
            }
            catch (Exception ex)
            {
                Response.Write(".........初始化菜单Error<br/>");
            }
        }
        #endregion

        #region  创建数据库结构
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
