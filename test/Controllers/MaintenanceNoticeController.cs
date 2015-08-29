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
    /// <summary>
    /// 维护公告
    /// </summary>
    public class MaintenanceNoticeController:BaseController
    {
        //
        // GET: /MaintenanceNotice/

        #region 维护公告
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDictPageList(DictQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            //query.JournalID = CurAuthor.JournalID;
            query.IsNotice = true;        
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<DictEntity> pager = service.GetDictPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        public ActionResult Create(Int64 dictId = 0)
        {
            DictEntity model = null;
            if (dictId > 0)
            {
                DictQuery query = new DictQuery();
                //query.JournalID = CurAuthor.JournalID;
                query.DictID = dictId;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                model = service.GetDictModel(query);
            }
            if (model == null)
                model = new DictEntity();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(DictEntity model)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            if (model.DictID == 0)
                model.InUserID = CurAuthor.AuthorID;
            else
                model.EditUserID = CurAuthor.AuthorID;
            //model.JournalID = CurAuthor.JournalID;
            model.DictKey = "Notice" + Guid.NewGuid().ToString();
            ExecResult result = service.SaveDict(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult Delete(Int64[] dictIDs)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            DictQuery query = new DictQuery();
            //query.JournalID = CurAuthor.JournalID;
            query.DictIDs = dictIDs;
            ExecResult result = service.DelDict(query);
            return Json(new { result = result.result, msg = result.msg });
        }
        #endregion

    }
}
