using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace Web.Admin.Controllers
{
    public class SiteNoticeController : BaseController
    {
        public ActionResult Index(Int64 ChannelID)
        {
            ViewBag.ChannelID = ChannelID;
            return View();
        }

        [HttpPost]
        public ActionResult GetPageList(SiteNoticeQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<SiteNoticeEntity> pager = service.GetSiteNoticePageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetList(SiteNoticeQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<SiteNoticeEntity> list = service.GetSiteNoticeList(query);
            return Json(new { list });
        }

        [HttpPost]
        public ActionResult Detail(Int64 NoticeID = 0)
        {
            return View(GetModel(NoticeID));
        }

        private SiteNoticeEntity GetModel(Int64 NoticeID)
        {
            SiteNoticeEntity model = null;
            if (NoticeID > 0)
            {
                SiteNoticeQuery query = new SiteNoticeQuery();
                query.JournalID = CurAuthor.JournalID;
                query.NoticeID = NoticeID;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                model = service.GetSiteNoticeModel(query);
            }
            if (model == null)
                model = new SiteNoticeEntity();
            return model;
        }

        public ActionResult Create(Int64 NoticeID = 0)
        {
            return View(GetModel(NoticeID));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(SiteNoticeEntity model)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            model.JournalID = CurAuthor.JournalID;           
            if (!string.IsNullOrWhiteSpace(model.Content))
                model.Content = Server.UrlDecode(model.Content);
            ExecResult result = service.SaveSiteNotice(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult Delete(Int64[] NoticeIDs)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteNoticeQuery query = new SiteNoticeQuery();
            query.JournalID = CurAuthor.JournalID;
            query.NoticeIDs = NoticeIDs;
            ExecResult result = service.DelSiteNotice(query);
            return Json(new { result = result.result, msg = result.msg });
        }
    }
}
