using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.SiteConsole.Controllers
{
    public class AccessLogController : BaseController
    {
        /// <summary>
        /// 访问日志
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

    }
}
