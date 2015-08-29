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
    public class SiteBlockController : BaseController
    {
        public ActionResult Index(Int64 ChannelID)
        {
            ViewBag.ChannelID = ChannelID;
            return View();
        }

        [HttpPost]
        public ActionResult GetPageList(SiteBlockQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<SiteBlockEntity> pager = service.GetSiteBlockPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetList(SiteBlockQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<SiteBlockEntity> list = service.GetSiteBlockList(query);
            return Json(new { list });
        }

        [HttpPost]
        public ActionResult Detail(Int64 BlockID = 0)
        {
            return View(GetModel(BlockID));
        }

        private SiteBlockEntity GetModel(Int64 BlockID)
        {
            SiteBlockEntity model = null;
            if (BlockID > 0)
            {
                SiteBlockQuery query = new SiteBlockQuery();
                query.JournalID = CurAuthor.JournalID;
                query.BlockID = BlockID;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                model = service.GetSiteBlockModel(query);
            }
            if (model == null)
                model = new SiteBlockEntity();
            return model;
        }

        public ActionResult Create(Int64 BlockID = 0)
        {
            return View(GetModel(BlockID));
        }

        [HttpPost]
        public ActionResult Save(SiteBlockEntity model)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            model.JournalID = CurAuthor.JournalID;           
            ExecResult result = service.SaveSiteBlock(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult Delete(Int64[] BlockIDs)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteBlockQuery query = new SiteBlockQuery();
            query.JournalID = CurAuthor.JournalID;
            query.BlockIDs = BlockIDs;
            ExecResult result = service.DelSiteBlock(query);
            return Json(new { result = result.result, msg = result.msg });
        }
    }
}
