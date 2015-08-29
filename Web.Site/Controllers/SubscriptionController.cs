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

namespace Web.Site.Controllers
{
    public class SubscriptionController : BaseController
    {
        /// <summary>
        /// 在线订阅
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region ajax

        /// <summary>
        /// 添加用户到指定角色
        /// </summary>
        /// <param name="authorEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult AddSubscriptionInfo(IssueSubscribeEntity issueSubscribeEntity)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                if (issueSubscribeEntity != null)
                {
                    issueSubscribeEntity.JournalID = JournalID;
                    IIssueFacadeService issueService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
                    execResult = issueService.SaveIssueSubscribe(issueSubscribeEntity);
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "在线订阅出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("在线订阅出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion
    }
}
