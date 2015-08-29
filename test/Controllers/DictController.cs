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
    public class DictController : BaseController
    {
        #region 数据字典
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDictPageList(DictQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);           
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<DictEntity> pager = service.GetDictPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        public ActionResult Create(Int64 dictId = 0)
        {
            DictEntity model=null;
            if (dictId > 0)
            {
                DictQuery query = new DictQuery();
                query.JournalID = CurAuthor.JournalID;
                query.DictID = dictId;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                model = service.GetDictModel(query);
            }
            if (model == null)
                model = new DictEntity();
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(DictEntity model)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            if (model.DictID == 0)
                model.InUserID = CurAuthor.AuthorID;
            else
                model.EditUserID = CurAuthor.AuthorID;
            model.JournalID = CurAuthor.JournalID;
            ExecResult result = service.SaveDict(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult Delete(Int64[] dictIDs)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            DictQuery query = new DictQuery();
            query.JournalID = CurAuthor.JournalID;
            query.DictIDs = dictIDs;
            ExecResult result = service.DelDict(query);
            return Json(new { result = result.result, msg = result.msg });
        }
        #endregion

        #region 数据字典值
        public ActionResult DictValue(Int64 dictId, string dictKey)
        {
            ViewBag.DictId = dictId;
            ViewBag.DictKey = Server.UrlDecode(dictKey);
            return View();
        }

        [HttpPost]
        public ActionResult GetDictValuePageList(DictValueQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<DictValueEntity> pager = service.GetDictValuePageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        public ActionResult CreateDictValue(Int64 dictValueId = 0)
        {
            DictValueEntity model = null;
            if (dictValueId > 0)
            {
                DictValueQuery query = new DictValueQuery();
                query.JournalID = CurAuthor.JournalID;
                query.DictValueID = dictValueId;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                model = service.GetDictValueModel(query);
            }
            if (model == null)
            {
                model = new DictValueEntity();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveDictValue(DictValueEntity model)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            if (model.DictValueID == 0)
                model.InUserID = CurAuthor.AuthorID;
            else
                model.EditUserID = CurAuthor.AuthorID;
            model.JournalID = CurAuthor.JournalID;
            ExecResult result = service.SaveDictValue(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult DeleteDictValue(Int64[] dictValueIDs)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            DictValueQuery query = new DictValueQuery();
            query.JournalID = CurAuthor.JournalID;
            query.DictValueIDs = dictValueIDs;
            ExecResult result = service.DelDictValue(query);
            return Json(new { result = result.result, msg = result.msg });
        }
        #endregion
    }
}
