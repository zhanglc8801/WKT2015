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
    public class FriendlyLinkController : BaseController
    {
        public ActionResult Index(Int64 ChannelID)
        {
            ViewBag.ChannelID = ChannelID;
            return View();
        }

        [HttpPost]
        public ActionResult GetPageList(FriendlyLinkQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<FriendlyLinkEntity> pager = service.GetFriendlyLinkPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetList(FriendlyLinkQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<FriendlyLinkEntity> list = service.GetFriendlyLinkList(query);
            return Json(new { list });
        }

        [HttpPost]
        public ActionResult Detail(Int64 LinkID = 0)
        {
            return View(GetModel(LinkID));
        }

        private FriendlyLinkEntity GetModel(Int64 LinkID)
        {
            FriendlyLinkEntity model = null;
            if (LinkID > 0)
            {
                FriendlyLinkQuery query = new FriendlyLinkQuery();
                query.JournalID = CurAuthor.JournalID;
                query.LinkID = LinkID;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                model = service.GetFriendlyLinkModel(query);
            }
            if (model == null)
                model = new FriendlyLinkEntity();
            return model;
        }

        public ActionResult Create(Int64 LinkID = 0)
        {
            return View(GetModel(LinkID));
        }

        [HttpPost]
        public ActionResult Save(FriendlyLinkEntity model)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            model.JournalID = CurAuthor.JournalID;           
            ExecResult result = service.SaveFriendlyLink(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult Delete(Int64[] LinkIDs)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            FriendlyLinkQuery query = new FriendlyLinkQuery();
            query.JournalID = CurAuthor.JournalID;
            query.LinkIDs = LinkIDs;
            ExecResult result = service.DelFriendlyLink(query);
            return Json(new { result = result.result, msg = result.msg });
        }
    }
}
