using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.Site
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //show channel
            routes.MapRoute(
                "show", // Route name
                "show/{ChannelID}/", // URL with parameters
                new { controller = "Home", action = "show" } // Parameter defaults
            );

            //show channel
            routes.MapRoute(
                "resource", // Route name
                "resource", // URL with parameters
                new { controller = "Home", action = "resource" } // Parameter defaults
            );

            //show list page
            routes.MapRoute(
                "list", // Route name
                "list/{ChannelID}", // URL with parameters
                new { controller = "Home", action = "list" } // Parameter defaults
            );

    

            //show list page
            routes.MapRoute(
                "channel", // Route name
                "channel/{ChannelUrl}", // URL with parameters
                new { controller = "Home", action = "channel" } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

        }

        private const string ONLINE_KEY = "WKT_ONLINE";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Application[ONLINE_KEY] = 0;
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Application.Lock();//锁定后，只有这个Session能够会话
            if (Application[ONLINE_KEY] != null)
            {
                Application[ONLINE_KEY] = (int)Application[ONLINE_KEY] + 1;
            }
            else
            {
                Application[ONLINE_KEY] = 1;
            }
            Application.UnLock();//会话完毕后解锁
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            Application.Lock();
            if (Application[ONLINE_KEY] == null)
            {
                Application[ONLINE_KEY] = 0;
            }
            else
            {
                Application[ONLINE_KEY] = (int)Application[ONLINE_KEY] - 1;
            }
            Application.UnLock();
        }
    }
}