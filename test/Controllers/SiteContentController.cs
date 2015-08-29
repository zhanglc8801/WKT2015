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

namespace HanFang360.InterfaceService.Controllers
{
    public class SiteContentController : BaseController
    {
        public ActionResult Index(Int64 ChannelID)
        {
            ViewBag.ChannelID = ChannelID;
            return View();
        }

        [HttpPost]
        public ActionResult GetPageList(SiteContentQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<SiteContentEntity> pager = service.GetSiteContentPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetList(SiteContentQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<SiteContentEntity> list = service.GetSiteContentList(query);
            return Json(new { list });
        }

        [HttpPost]
        public ActionResult Detail(Int64 ContentID = 0)
        {
            return View(GetModel(ContentID));
        }

        private SiteContentEntity GetModel(Int64 ContentID)
        {
            SiteContentEntity model = null;
            if (ContentID > 0)
            {
                SiteContentQuery query = new SiteContentQuery();
                query.JournalID = CurAuthor.JournalID;
                query.ContentID = ContentID;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                model = service.GetSiteContentModel(query);
            }
            if (model == null)
                model = new SiteContentEntity();
            return model;
        }

        public ActionResult Create(Int64 ContentID = 0)
        {
            return View(GetModel(ContentID));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(SiteContentEntity model)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            if (model.ContentID == 0)
                model.InAuthor = CurAuthor.AuthorID;
            else
                model.EditAuthor = CurAuthor.AuthorID;
            if (!string.IsNullOrWhiteSpace(model.Content))
                model.Content = Server.UrlDecode(model.Content);           
            ExecResult result = service.SaveSiteContent(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult Delete(Int64[] ContentIDs)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteContentQuery query = new SiteContentQuery();
            query.JournalID = CurAuthor.JournalID;
            query.ContentIDs = ContentIDs;
            ExecResult result = service.DelSiteContent(query);
            return Json(new { result = result.result, msg = result.msg });
        }
    }
}
