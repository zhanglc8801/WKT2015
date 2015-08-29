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
    public class SiteMessageController : BaseController
    {        
        public ActionResult Index()
        {
            ViewBag.User = CurAuthor.AuthorID;
            return View();
        }

        [HttpPost]
        public ActionResult GetPageList(SiteMessageQuery query, Int32? MsgType)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            if (MsgType == null)
            {
                query.IsUserRelevant = true;
                query.SendUser = CurAuthor.AuthorID;
                query.ReciverID = CurAuthor.AuthorID;
            }
            else if (MsgType.Value == 1)
                query.SendUser = CurAuthor.AuthorID;
            else if (MsgType.Value == 2)
                query.ReciverID = CurAuthor.AuthorID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<SiteMessageEntity> pager = service.GetSiteMessagePageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetList(SiteMessageQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<SiteMessageEntity> list = service.GetSiteMessageList(query);
            return Json(new { list });
        }

        /// <summary>
        /// 获取当前人所有未读的站内消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCurrentMsgList()
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteMessageQuery query = new SiteMessageQuery();
            query.JournalID = CurAuthor.JournalID;
            query.ReciverID = CurAuthor.AuthorID;
            query.IsUserRelevant = false;
            query.IsView = 0;
            IList<SiteMessageEntity> list = service.GetSiteMessageList(query);
            var result = list.Select(p => new { ID = p.MessageID, Title = p.Title, SendUser = p.SendUserName, SendDate = p.SendDate.ToString("yyyy-MM-dd hh:mm") }).ToList();
            return Json(new { list = result });
        }

        public ActionResult Detail(Int32 Type = 1, Int64 MessageID = 0)
        {
            ViewBag.Type = Type;
            return View(GetModel(MessageID));
        }

        public ActionResult Content(SiteMessageQuery query)
        {
            return View(GetModel(query.MessageID));
        }

        public ActionResult View(Int64 RecodeID)
        {
            return View(GetMsgModel(RecodeID));
        }

        private SiteMessageEntity GetModel(Int64 MessageID)
        {
            SiteMessageEntity model = null;
            if (MessageID > 0)
            {
                SiteMessageQuery query = new SiteMessageQuery();
                query.JournalID = CurAuthor.JournalID;
                query.MessageID = MessageID;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                model = service.GetSiteMessageModel(query);
            }
            if (model == null)
                model = new SiteMessageEntity();
            return model;
        }

        private MessageRecodeEntity GetMsgModel(Int64 RecodeID)
        {
            MessageRecodeEntity model = null;
            if (RecodeID > 0)
            {
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                MessageRecodeQuery query = new MessageRecodeQuery();
                query.JournalID = CurAuthor.JournalID;
                query.RecodeID = RecodeID;
                model = service.GetMsgRecodeModel(query);
            }
            if (model == null)
                model = new MessageRecodeEntity();
            return model;
        }

        public ActionResult Create(Int64 MessageID = 0)
        {
            return View(GetModel(MessageID));
        }

        [HttpPost]
        public ActionResult Save(SiteMessageEntity model)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            model.Content = Server.UrlDecode(model.Content);
            ExecResult result = service.SaveSiteMessage(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult Delete(Int64[] MessageIDs)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteMessageQuery query = new SiteMessageQuery();
            query.JournalID = CurAuthor.JournalID;
            query.MessageIDs = MessageIDs;
            ExecResult result = service.DelSiteMessage(query);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult Viewed(Int64 MessageID)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteMessageQuery query = new SiteMessageQuery();
            query.JournalID = CurAuthor.JournalID;
            query.MessageID = MessageID;
            bool result = service.UpdateMsgViewed(query);
            return Json(new { result = result ? EnumJsonResult.success.ToString() : EnumJsonResult.failure.ToString() });
        }

        /// <summary>
        /// 作者平台发送短信(不用模版)
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AuthorSendMessage(Int64 CID, Int32 Status, SiteMessageEntity model)
        {
            var user = GetAuthorInfo(CID, Status);
            if (user == null)
                return Json(new { result = EnumJsonResult.failure.ToString(), msg = "获取接收人失败，发送站内消息失败！" });

            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            model.SendUser = CurAuthor.AuthorID;
            model.Content = Server.UrlDecode(model.Content);

            //通过稿件编号获取接收人、接受地址
            model.ReciverID = user.AuthorID;

            ExecResult result = service.SaveSiteMessage(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        /// <summary>
        /// 获取接收人信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        private AuthorInfoEntity GetAuthorInfo(Int64 CID, Int32 Status)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity query = new CirculationEntity();
            query.JournalID = CurAuthor.JournalID;
            query.CID = CID;
            if (Status == 0)
                query.EnumCStatus = EnumContributionStatus.New;
            else if (Status == 200)
                query.EnumCStatus = EnumContributionStatus.Employment;
            else if (Status == 100)
                query.EnumCStatus = EnumContributionStatus.Proof;
            else if (Status == -3)
                query.EnumCStatus = EnumContributionStatus.Retreat;
            var model = service.GetContributionProcesser(query);
            return model;
        }
    }
}
