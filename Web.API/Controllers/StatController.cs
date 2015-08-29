using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;

using Microsoft.Practices.Unity;

using Newtonsoft.Json;
using WKT.Common;
using WKT.Common.Utils;
using WKT.Log;
using WKT.Config;
using WKT.Model;
using WKT.Service.Interface;
using WKT.Service.Wrapper;

namespace Web.API.Controllers
{
    /// <summary>
    /// 访问情况统计
    /// </summary>
    public class StatController : AsyncController
    {
        [HttpGet]
        [AsyncTimeout(30000)]// 超时时间30秒钟
        public void IndexAsync(long? JournalID)
        {
            IAccessLogService logService = ServiceContainer.Instance.Container.Resolve<IAccessLogService>();
            
            HttpBrowserCapabilitiesBase bc = HttpContext.Request.Browser;
            AccessLog stat = new AccessLog();
            stat.JournalID = JournalID == null ? 0 : JournalID.Value;
            stat.Browser = bc.Browser;
            stat.BrowserType = bc.Type;
            stat.Version = bc.Version;
            stat.Platform = bc.Platform;
            stat.UrlReferrer = HttpContext.Request.UrlReferrer == null ? "" : HttpContext.Request.UrlReferrer.ToString();
            stat.UserHostAddress = Utils.GetRealIP();
            stat.HttpMethod = HttpContext.Request.HttpMethod;
            stat.IsAuthenticated = HttpContext.Request.IsAuthenticated;
            stat.LogDateTime = DateTime.Now.ToLocalTime();

            try
            {
                QQWryLocator ipLocator = new QQWryLocator();
                IPLocation ipInfo = ipLocator.Query(stat.UserHostAddress);
                stat.Country = ipInfo.Country;
                stat.City = ipInfo.Local;
                logService.AddAccessLog(stat);
                //参数要放在这个字典里面实现向Completed action传递
                //AsyncManager.Parameters["ExecResult"] = "<script> var result = 'success';</script>";
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("访问日志统计：" + ex.Message);
                //AsyncManager.Parameters["ExecResult"] = "error:" + ex.Message;
            }
            AsyncManager.OutstandingOperations.Decrement();
        }

        public ActionResult IndexCompleted()
        {
            return Content("");
        }   
    }
}