using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Core;
using Domain;
using NHibernate;

namespace Webs
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private Container container;
        protected void Application_Start()
        {
            
            // 容器初始化
            try
            {
                // 获取 web.config 配置
                IConfigurationSource source = (IConfigurationSource)System.Configuration.ConfigurationManager.GetSection("ActiveRecord");
                ActiveRecordStarter.Initialize(typeof(SysUser).Assembly, source);
                container = Container.Instance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}