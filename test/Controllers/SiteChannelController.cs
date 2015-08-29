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
using Newtonsoft.Json;

namespace HanFang360.InterfaceService.Controllers
{
    public class SiteChannelController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取菜单列表Ajax
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetTree()
        {
            SiteChannelQuery query = new SiteChannelQuery();
            query.JournalID = JournalID;
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            return Content(JsonConvert.SerializeObject(service.GetSiteChannelTreeList(query)));
        }

        [HttpPost]
        public ActionResult Detail(Int64 ChannelID = 0)
        {
            return View(GetModel(ChannelID));
        }

        private SiteChannelEntity GetModel(Int64 ChannelID)
        {
            SiteChannelEntity model = null;
            if (ChannelID > 0)
            {
                SiteChannelQuery query = new SiteChannelQuery();
                query.JournalID = CurAuthor.JournalID;
                query.ChannelID = ChannelID;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                model = service.GetSiteChannelModel(query);
            }
            if (model == null)
                model = new SiteChannelEntity();
            return model;
        }

        public ActionResult Create(Int64 ChannelID = 0)
        {
            return View(GetModel(ChannelID));
        }

        [HttpPost]
        public ActionResult Save(SiteChannelEntity model)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            ExecResult result = service.SaveSiteChannel(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult Delete(Int64 ChannelID)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteChannelQuery query = new SiteChannelQuery();
            query.JournalID = CurAuthor.JournalID;
            query.ChannelID = ChannelID;
            ExecResult result = service.DelSiteChannel(query);
            return Json(new { result = result.result, msg = result.msg });
        }
    }
}
