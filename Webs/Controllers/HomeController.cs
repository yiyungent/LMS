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
            InitClazz();
            InitStudent();

        }

        #region 初始化学生
        private void InitStudent()
        {
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
            Container.Instance.Resolve<ClazzService>().Create(new Clazz()
            {
                Name = "1700103班",
            });
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
