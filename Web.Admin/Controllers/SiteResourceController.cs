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
    public class SiteResourceController : BaseController
    {
        public ActionResult Index(Int64 ChannelID)
        {
            ViewBag.ChannelID = ChannelID;
            return View();
        }

        [HttpPost]
        public ActionResult GetPageList(SiteResourceQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<SiteResourceEntity> pager = service.GetSiteResourcePageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetList(SiteResourceQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<SiteResourceEntity> list = service.GetSiteResourceList(query);
            return Json(new { list });
        }

        [HttpPost]
        public ActionResult Detail(Int64 ResourceID = 0)
        {
            return View(GetModel(ResourceID));
        }

        private SiteResourceEntity GetModel(Int64 ResourceID)
        {
            SiteResourceEntity model = null;
            if (ResourceID > 0)
            {
                SiteResourceQuery query = new SiteResourceQuery();
                query.JournalID = CurAuthor.JournalID;
                query.ResourceID = ResourceID;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                model = service.GetSiteResourceModel(query);
            }
            if (model == null)
                model = new SiteResourceEntity();
            return model;
        }

        public ActionResult Create(Int64 ResourceID = 0)
        {
            return View(GetModel(ResourceID));
        }

        [HttpPost]
        public ActionResult Save(SiteResourceEntity model)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            if (model.ResourceID == 0)
                model.InUserID = CurAuthor.AuthorID;
            else
                model.EditUserID = CurAuthor.AuthorID;             
            ExecResult result = service.SaveSiteResource(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult Delete(Int64[] ResourceIDs)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteResourceQuery query = new SiteResourceQuery();
            query.JournalID = CurAuthor.JournalID;
            query.ResourceIDs = ResourceIDs;
            ExecResult result = service.DelSiteResource(query);
            return Json(new { result = result.result, msg = result.msg });
        }

    }
}
