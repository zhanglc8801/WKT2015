using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.SiteConsole.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(AccountEntity);
        }

        /// <summary>
        /// 默认首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Welcome()
        {
            return View(AccountEntity);
        }
    }
}
