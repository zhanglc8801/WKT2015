using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using Microsoft.Practices.Unity;

using WKT.Log;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Service.Interface;
using WKT.Service.Wrapper;

namespace Web.API.Controllers
{
    /// <summary>
    /// Api基类
    /// </summary>
    public class ApiBaseController : ApiController
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public string SiteID
        {
            get;
            set;
        }
    }
}