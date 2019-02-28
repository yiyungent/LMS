using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;

namespace LMS.Controllers
{
    public class SysUserController : Controller
    {
        #region 列表
        public ActionResult Index()
        {
            IList<SysUser> lst = new List<SysUser>();
            lst.Add(new SysUser()
            {
                ID = 1,
                Name = "刘备",
                LoginAccount = "lb",
                Password = "123456",
                Status = 0
            });
            lst.Add(new SysUser()
            {
                ID = 2,
                Name = "关羽",
                LoginAccount = "gy",
                Password = "123456",
                Status = 0
            });
            lst.Add(new SysUser()
            {
                ID = 3,
                Name = "张飞",
                LoginAccount = "zf",
                Password = "12345",
                Status = 0
            });
            lst.Add(new SysUser()
            {
                ID = 4,
                Name = "赵云",
                LoginAccount = "zy",
                Password = "123456",
                Status = 0
            });
            lst.Add(new SysUser()
            {
                ID = 5,
                Name = "马超",
                LoginAccount = "mc",
                Password = "123456",
                Status = 0
            });

            return View(lst); // 强类型视图
        }
        #endregion

        #region 新增
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region 修改
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // 根据ID获取实体
            SysUser entity = new SysUser()
            {
                ID = 2,
                Name = "关羽",
                LoginAccount = "gy",
                Password = "123456",
                Status = 0
            };
            return View(entity);
        }
        #endregion

        #region 删除
        public ActionResult Delete()
        {
            return View();
        }
        #endregion

        #region 明细
        public ActionResult Details(int id)
        {
            // 根据ID获取实体
            SysUser entity = new SysUser()
            {
                ID = 2,
                Name = "关羽",
                LoginAccount = "gy",
                Password = "123456",
                Status = 0
            };
            return View(entity);
        }
        #endregion

        #region 登录
        public ActionResult Login()
        {
            return View();
        }
        #endregion
    }
}
