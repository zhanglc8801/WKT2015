using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Config;
using WKT.Common.Extension;
using WKT.Common.Utils;
using WKT.Common.Security;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace Web.Site.Controllers
{
    public class MessageController : BaseController
    {
        /// <summary>
        /// 在线留言
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1)
        {
            GuestbookQuery guestBookQuery = new GuestbookQuery();
            guestBookQuery.JournalID = JournalID;
            guestBookQuery.CurrentPage = page;
            guestBookQuery.PageSize = 15;
            Pager<GuestbookEntity> pageGuestBook = new Pager<GuestbookEntity>();
            try
            {
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                pageGuestBook = service.GetSiteGuestBookPageList(guestBookQuery);
                if (pageGuestBook != null)
                {
                    ViewBag.PagerInfo = Utils.GetPageNumbers(page, pageGuestBook.TotalPage, "/Message/", 5);
                }
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("获取留言列表出现异常:" + ex.Message);
            }
            return View(pageGuestBook);
        }

        /// <summary>
        /// 在线留言
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        [AjaxRequest]
        public ActionResult AddMessage(GuestbookEntity guestBookEntit)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                if (guestBookEntit != null)
                {
                    guestBookEntit.JournalID = JournalID;
                    guestBookEntit.IP = WKT.Common.Utils.Utils.GetRealIP();
                    ISiteConfigFacadeService guestService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                    execResult = guestService.SaveSiteGuestBook(guestBookEntit);
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "在线留言出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("在线留言出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }
    }
}
