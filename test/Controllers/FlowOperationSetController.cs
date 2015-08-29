using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Common.Utils;
using WKT.Common.Security;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Config;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace Web.Admin.Controllers
{
    /// <summary>
    /// 流程操作设置
    /// </summary>
    public class FlowOperationSetController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加、修改流程操作
        /// </summary>
        /// <param name="OperationID"></param>
        /// <returns></returns>
        public ActionResult AddOperationSet(long? OperationID)
        {
            return View();
        }

        # region 删除

        [HttpPost]
        [AjaxRequest]
        public ActionResult DelOperationSetAjax(long[] IDAarry)
        {
            ExecResult exeResult = new ExecResult();
            if (IDAarry == null || IDAarry.Length == 0)
            {
                exeResult.msg = "请选择要删除的审稿操作";
                exeResult.result = EnumJsonResult.failure.ToString();
                return Content(JsonConvert.SerializeObject(exeResult));
            }
            AuthorInfoQuery authorQuery = new AuthorInfoQuery();
            authorQuery.JournalID = JournalID;
            authorQuery.AuthorIDList = IDAarry.ToList<long>();

            ISiteSystemFacadeService sysService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            exeResult = sysService.DelMember(authorQuery);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        # endregion
    }
}
