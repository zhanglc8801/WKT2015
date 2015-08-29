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
    public class ContactWayController : BaseController
    {
        public ActionResult Index(Int64 ChannelID)
        {
            ViewBag.ChannelID = ChannelID;
            return View();
        }

        [HttpPost]
        public ActionResult GetPageList(ContactWayQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<ContactWayEntity> pager = service.GetContactWayPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetList(ContactWayQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<ContactWayEntity> list = service.GetContactWayList(query);
            return Json(new { list });
        }

        [HttpPost]
        public ActionResult Detail(Int64 ContactID = 0)
        {
            return View(GetModel(ContactID));
        }

        private ContactWayEntity GetModel(Int64 ContactID)
        {
            ContactWayEntity model = null;
            if (ContactID > 0)
            {
                ContactWayQuery query = new ContactWayQuery();
                query.JournalID = CurAuthor.JournalID;
                query.ContactID = ContactID;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                model = service.GetContactWayModel(query);
            }
            if (model == null)
                model = new ContactWayEntity();
            return model;
        }

        public ActionResult Create(Int64 ContactID = 0)
        {
            return View(GetModel(ContactID));
        }

        [HttpPost]
        public ActionResult Save(ContactWayEntity model)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();            
            model.JournalID = CurAuthor.JournalID;
            if (!string.IsNullOrWhiteSpace(model.Address))
                model.Address = Server.UrlDecode(model.Address);
            ExecResult result = service.SaveContactWay(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult Delete(Int64[] ContactIDs)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            ContactWayQuery query = new ContactWayQuery();
            query.JournalID = CurAuthor.JournalID;
            query.ContactIDs = ContactIDs;
            ExecResult result = service.DelContactWay(query);
            return Json(new { result = result.result, msg = result.msg });
        }
    }
}
