using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Common.Security;
using WKT.Model;

namespace Web.SiteConsole.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 当前账户信息
        /// </summary>
        public SysAccountInfoEntity AccountEntity
        {
            get;
            set;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (TicketTool.IsLogin())
            {
                AccountEntity = JsonConvert.DeserializeObject<SysAccountInfoEntity>(TicketTool.GetUserData());
            }
            else
            {
                filterContext.HttpContext.Response.Redirect("/login/", true);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
