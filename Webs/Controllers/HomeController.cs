using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Castle.ActiveRecord;
using NHibernate.Criterion;
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
            IList<SysMenu> allMenu = GetMenuAuthForLoginUser();
            ViewBag.allMenu = allMenu;
            return View();
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 获取当前登录用户的菜单权限
        /// </summary>
        private IList<SysMenu> GetMenuAuthForLoginUser()
        {
            IList<SysMenu> ret = new List<SysMenu>();
            // 确定 LoginUser
            if (Session["loginUser"] != null)
            {
                SysUser loginUser = (SysUser)Session["loginUser"];
                foreach (var role in loginUser.SysRoleList)
                {
                    foreach (var menu in role.SysMenuList)
                    {
                        // 判断 ret 中是否已经存在当前菜单
                        bool exist = false;
                        foreach (var item in ret)
                        {
                            if (item.ID == menu.ID)
                            {
                                exist = true;
                                break;
                            }
                        }
                        // 如果不存在添加当前菜单到 ret
                        if (exist == false)
                        {
                            ret.Add(menu);
                        }
                    }
                }
            }
            return ret;
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
            InitFunction();
            InitRole();
            InitUser();
            InitClazz();
            InitStudent();
            InitClient();
            InitDeliverType();
            InitProvince();
            InitTransportOrder();
            InitTransportOrderItem();
        }

        #region 初始化货运单
        private void InitTransportOrder()
        {
            try
            {
                Response.Write(".........初始化货运单<br/>");

                string[] startStations = { "重庆", "上海", "云南", "北京", "四川", "成都", "重庆", "上海", "云南", "北京", "四川", "成都" };
                string[] endStations = { "武汉", "香港", "深圳", "广州", "杭州", "台湾", "重庆", "上海", "云南", "北京", "四川", "成都" };

                string[] startPlace = { "重庆", "上海", "云南", "北京", "四川", "成都", "重庆", "上海", "云南", "北京", "四川", "成都" };
                string[] endPlace = { "武汉", "香港", "深圳", "广州", "杭州", "台湾", "重庆", "上海", "云南", "北京", "四川", "成都" };

                for (int i = 0; i < startStations.Length; i++)
                {
                    Container.Instance.Resolve<TransportOrderService>().Create(new TransportOrder()
                    {
                        StartStation = startStations[i],
                        EndStation = endStations[i],
                        AttachedFile = "第" + (i + 1) + "个附件",
                        CreateDate = DateTime.Now.AddDays(i),
                        Creator = Container.Instance.Resolve<SysUserService>().GetEntity(2),
                        Customer = Container.Instance.Resolve<ClientService>().GetEntity(i % 4 + 1),
                        CustomerRemark = "托运人记载事项" + (i + 1),
                        TransportRemark = "运输备注" + (i + 1),
                        DeliveryType = Container.Instance.Resolve<DeliveryTypeService>().GetEntity(i % 3 + 1),
                        Price = Math.Round(new Random().NextDouble() * 100, 2),
                        Receiver = Container.Instance.Resolve<ClientService>().GetEntity(new Random().Next(1, 5)),
                        StartPlace = startPlace[i],
                        EndPlace = endPlace[i],
                        Province = Container.Instance.Resolve<ProvinceService>().GetEntity(i % 3 + 1),
                        TransportOrderItemList = null,
                    });
                }

                Response.Write(".........初始化货运单ok<br/>");
            }
            catch (Exception ex)
            {
                Response.Write(".........初始化货运单Error<br/>");
            }
        }
        #endregion

        #region 初始化货运单项
        private void InitTransportOrderItem()
        {
            try
            {
                Response.Write(".........初始化货运单项<br/>");

                string[] cabinetTypes = { "20英寸", "30英寸", "40英寸", "50英寸", "60英寸", "70英寸" };

                for (int i = 0; i < 12; i++)
                {
                    Container.Instance.Resolve<TransportOrderItemService>().Create(new TransportOrderItem()
                    {
                        CabinetType = cabinetTypes[i % 5 + 1],
                        CabinetSort = "干货箱" + (i + 1),
                        Amount = new Random().Next(0, 100),
                        CabinetNumber = ((i + 1) * 1000).ToString(),
                        CargoName = "货物品" + (i + 1),
                        SealedNumber = new Random().Next(9999, 100000).ToString(),
                        TransportFee = Math.Round(new Random().NextDouble() * 1000, 2),
                        TransportOrder = Container.Instance.Resolve<TransportOrderService>().GetEntity(i + 1),
                    });
                }

                Response.Write(".........初始化货运单项ok<br/>");
            }
            catch (Exception ex)
            {
                Response.Write(".........初始化货运单项Error<br/>");
            }
        }
        #endregion

        #region 初始化省市
        private void InitProvince()
        {
            try
            {
                Response.Write(".........初始化省市<br/>");

                string[] names = { "重庆", "云南", "四川", "上海" };
                for (int i = 0; i < names.Length; i++)
                {
                    Container.Instance.Resolve<ProvinceService>().Create(new Province
                    {
                        Name = names[i],
                        SortCode = 10 * (i + 1)
                    });
                }

                Response.Write(".........初始化省市ok<br/>");
            }
            catch (Exception ex)
            {
                Response.Write(".........初始化省市Error<br/>");
            }
        }
        #endregion

        #region 初始化客户
        private void InitClient()
        {
            try
            {
                Response.Write(".........初始化客户<br/>");

                // 添加五个以上的客户
                string[] names = { "客户A", "客户B", "客户C", "客户D", "客户E", "客户F" };
                string[] telephones = { "15098442556", "15091442456", "12091442551", "15098242556", "15192442556", "15192445557" };
                for (int i = 0; i < names.Length; i++)
                {
                    Container.Instance.Resolve<ClientService>().Create(new Client
                    {
                        Name = names[i],
                        Telephone = telephones[i]
                    });

                }

                Response.Write(".........初始化客户ok<br/>");
            }
            catch (Exception ex)
            {
                Response.Write(".........初始化客户Error<br/>");
            }
        }
        #endregion

        #region 初始化运输方式
        private void InitDeliverType()
        {
            try
            {
                Response.Write(".........初始化运输方式<br/>");

                // 站到站
                // 站到门
                // 门到站
                // 门到门
                string[] names = { "站到站", "站到门", "门到站", "门到门" };
                for (int i = 0; i < names.Length; i++)
                {
                    Container.Instance.Resolve<DeliveryTypeService>().Create(new DeliveryType
                    {
                        Name = names[i],
                        SortCode = 10 * (i + 1)
                    });
                }

                Response.Write(".........初始化运输方式ok<br/>");
            }
            catch (Exception ex)
            {
                Response.Write(".........初始化运输方式Error<br/>");
            }
        }
        #endregion

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
                #region 废弃
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
                #endregion
                var allRole = Container.Instance.Resolve<SysRoleService>().GetAll();

                Container.Instance.Resolve<SysUserService>().Create(new SysUser()
                {
                    Name = "系统管理员",
                    LoginAccount = "admin",
                    Password = StringHelper.EncodeMD5("123456"),
                    Status = 0,
                    SysRoleList = (from m in allRole where m.ID == 2 select m).ToList(),
                });

                Container.Instance.Resolve<SysUserService>().Create(new SysUser()
                {
                    Name = "莫宇",
                    LoginAccount = "my",
                    Password = StringHelper.EncodeMD5("123456"),
                    Status = 0,
                    SysRoleList = (from m in allRole where m.ID == 1 select m).ToList(),
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
                // ID=20 接单员
                SysUser user = Container.Instance.Resolve<SysUserService>().GetEntity(20);
                user.SysRoleList = (from m in allRole where m.ID == 3 select m).ToList();
                Container.Instance.Resolve<SysUserService>().Edit(user);
                // ID=[30, 35] 调度员
                IList<int> idRange = new List<int>() { 30, 31, 32, 33, 34, 35 };
                var findUser = Container.Instance.Resolve<SysUserService>().Query(new List<ICriterion>()
                {
                    Expression.In("ID", idRange.ToArray())
                });
                foreach (var item in findUser)
                {
                    item.SysRoleList = (from m in allRole where m.ID == 4 select m).ToList();
                    Container.Instance.Resolve<SysUserService>().Edit(item);
                }
                // ID=[40, 45] 驾驶员
                idRange = new List<int>() { 40, 41, 42, 43, 44, 45 };
                findUser = Container.Instance.Resolve<SysUserService>().Query(new List<ICriterion>()
                {
                    Expression.In("ID", idRange.ToArray())
                });
                foreach (var item in findUser)
                {
                    item.SysRoleList = (from m in allRole where m.ID == 6 select m).ToList();
                    Container.Instance.Resolve<SysUserService>().Edit(item);
                }
                // ID=[50, 55] 财务员
                idRange = new List<int>() { 50, 51, 52, 53, 54, 55 };
                findUser = Container.Instance.Resolve<SysUserService>().Query(new List<ICriterion>()
                {
                    Expression.In("ID", idRange.ToArray())
                });
                foreach (var item in findUser)
                {
                    item.SysRoleList = (from m in allRole where m.ID == 6 select m).ToList();
                    Container.Instance.Resolve<SysUserService>().Edit(item);
                }
                // ID=[60, 65] 决策员
                idRange = new List<int>() { 60, 61, 62, 63, 64, 65 };
                findUser = Container.Instance.Resolve<SysUserService>().Query(new List<ICriterion>()
                {
                    Expression.In("ID", idRange.ToArray())
                });
                foreach (var item in findUser)
                {
                    item.SysRoleList = (from m in allRole where m.ID == 7 select m).ToList();
                    Container.Instance.Resolve<SysUserService>().Edit(item);
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
                var allMenu = Container.Instance.Resolve<SysMenuService>().GetAll();
                var allFunction = Container.Instance.Resolve<SysFunctionService>().GetAll();

                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "开发人员",
                    Status = 0,
                    SysMenuList = allMenu,
                    SysFunctionList = allFunction
                });
                IList<int> idRange = new List<int>() { 1, 3, 4, 6 };
                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "系统管理员",
                    SysMenuList = (from m in allMenu
                                   where idRange.Contains(m.ID)
                                   select m).ToList(),
                    SysFunctionList = (from m in allFunction where m.SysMenu != null && idRange.Contains(m.SysMenu.ID) select m).ToList(),
                    Status = 0
                });
                idRange = new List<int>() { 2, 6 };
                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "接单员",
                    SysMenuList = (from m in allMenu
                                   where idRange.Contains(m.ID)
                                   select m).ToList(),
                    SysFunctionList = (from m in allFunction where m.SysMenu != null && idRange.Contains(m.SysMenu.ID) select m).ToList(),
                    Status = 0
                });
                idRange = new List<int>() { 2, 7 };
                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "调度员",
                    SysMenuList = (from m in allMenu
                                   where idRange.Contains(m.ID)
                                   select m).ToList(),
                    SysFunctionList = (from m in allFunction where m.SysMenu != null && idRange.Contains(m.SysMenu.ID) select m).ToList(),
                    Status = 0
                });
                idRange = new List<int>() { 2, 8, 9 };
                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "财务员",
                    SysMenuList = (from m in allMenu
                                   where idRange.Contains(m.ID)
                                   select m).ToList(),
                    SysFunctionList = (from m in allFunction where m.SysMenu != null && idRange.Contains(m.SysMenu.ID) select m).ToList(),
                    Status = 0
                });
                idRange = new List<int>() { 2, 8 };
                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "驾驶员",
                    SysMenuList = (from m in allMenu
                                   where idRange.Contains(m.ID)
                                   select m).ToList(),
                    SysFunctionList = (from m in allFunction where m.SysMenu != null && idRange.Contains(m.SysMenu.ID) select m).ToList(),
                    Status = 0
                });
                idRange = new List<int>() { 2, 9 };
                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "决策人员",
                    SysMenuList = (from m in allMenu
                                   where idRange.Contains(m.ID)
                                   select m).ToList(),
                    SysFunctionList = (from m in allFunction where m.SysMenu != null && idRange.Contains(m.SysMenu.ID) select m).ToList(),
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

        #region 初始化系统操作
        private void InitFunction()
        {
            try
            {
                Response.Write(".........初始化系统操作<br/>");
                IList<ICriterion> qryWhere = new List<ICriterion>();
                // 2-8
                qryWhere.Add(Expression.Ge("ID", 2));
                qryWhere.Add(Expression.Le("ID", 8));
                IList<SysMenu> find = Container.Instance.Resolve<SysMenuService>().Query(qryWhere);
                string[] opNames = { "新增", "修改", "删除", "查看" };
                foreach (var item in find)
                {
                    for (int i = 0; i < opNames.Length; i++)
                    {
                        Container.Instance.Resolve<SysFunctionService>().Create(new SysFunction()
                        {
                            Name = opNames[i],
                            SysMenu = item
                        });
                    }
                }
                // 9
                Container.Instance.Resolve<SysFunctionService>().Create(new SysFunction()
               {
                   Name = "查看",
                   SysMenu = Container.Instance.Resolve<SysMenuService>().GetEntity(9)
               });
                Response.Write(".........初始化系统操作ok<br/>");
            }
            catch (Exception ex)
            {
                Response.Write(".........初始化系统操作Error<br/>");
            }
        }
        #endregion

        #endregion
    }
}
