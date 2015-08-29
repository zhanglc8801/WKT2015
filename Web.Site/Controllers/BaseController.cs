using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Common.Utils;
using WKT.Common.Security;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Config;
using WKT.Log;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace Web.Site.Controllers
{
    /// <summary>
    /// 基类
    /// </summary>
    public class BaseController : Controller
    {
        ISiteSystemFacadeService systemFacadeService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();

        public long JournalID
        {
            get
            {
                return TypeParse.ToLong(ConfigurationManager.AppSettings["SiteID"]);
            }
        }

        public AuthorInfoEntity CurAuthor
        {
            get;
            set;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            # region 记录访问数量

            try
            {
                ISiteConfigFacadeService siteConfigService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                SiteConfigQuery query = new SiteConfigQuery();
                query.JournalID = JournalID;
                siteConfigService.UpdateSiteAccessCount(query);
            }
            catch (Exception ex)
            {
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];
                LogProvider.Instance.Error("[Controller:" + controllerName + ",ActionName:" + actionName + "],记录访问次数异常:" + ex.Message);
            }

            # endregion

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            LogProvider.Instance.Error("[Controller:" + controllerName + ",ActionName:" + actionName + "]" + filterContext.Exception.ToString());
            // 执行基类中的OnException
            base.OnException(filterContext);
        }
    }
}
