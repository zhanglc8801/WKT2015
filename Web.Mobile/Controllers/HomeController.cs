using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using WKT.Config;
using WKT.Common.Extension;
using WKT.Common.Utils;
using WKT.Common.Security;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace Web.Mobile.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        
    }
}
