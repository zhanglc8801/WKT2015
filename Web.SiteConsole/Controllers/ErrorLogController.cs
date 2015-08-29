using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.SiteConsole.Controllers
{
    public class ErrorLogController : BaseController
    {
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

    }
}
