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
using WKT.Common.Utils;
using Newtonsoft.Json;

namespace HanFang360.InterfaceService.Controllers
{
    public class SmsTempController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetPageList(MessageTemplateQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.TType = 2;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<MessageTemplateEntity> pager = service.GetMessageTempPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetList(MessageTemplateQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.TType = 2;
            IList<MessageTemplateEntity> list = service.GetMessageTempList(query);
            return Json(new { list });
        }

        [HttpPost]
        public ActionResult Detail(Int64 TemplateID = 0)
        {
            return View(GetModel(TemplateID));
        }

        private MessageTemplateEntity GetModel(Int64 TemplateID)
        {
            MessageTemplateEntity model = null;
            if (TemplateID > 0)
            {
                MessageTemplateQuery query = new MessageTemplateQuery();
                query.JournalID = CurAuthor.JournalID;
                query.TType = 2;
                query.TemplateID = TemplateID;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                model = service.GetMessageTempModel(query);
            }
            if (model == null)
                model = new MessageTemplateEntity();
            return model;
        }

        public ActionResult Create(Int64 TemplateID = 0)
        {
            return View(GetModel(TemplateID));
        }

        [HttpPost]
        public ActionResult Save(MessageTemplateEntity model)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            model.TType = 2;
            if (model.TemplateID == 0)
                model.InUser = CurAuthor.AuthorID;
            else
                model.EditUser = CurAuthor.AuthorID;
            model.TContent = Server.UrlDecode(model.TContent);
            ExecResult result = service.SaveMessageTemp(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult Delete(Int64[] TemplateIDs)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            MessageTemplateQuery query = new MessageTemplateQuery();
            query.JournalID = CurAuthor.JournalID;
            query.TType = 2;
            query.TemplateIDs = TemplateIDs;
            ExecResult result = service.DelMessageTemp(query);
            return Json(new { result = result.result, msg = result.msg });
        }

        public ActionResult Send()
        {
            SiteConfigQuery query = new SiteConfigQuery();
            query.JournalID = CurAuthor.JournalID;
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigEntity model = service.GetSiteConfigModel(query);
            if (model == null)
                model = new SiteConfigEntity();

            return View(model);
        }

        [HttpPost]
        public ActionResult SendSms(MessageRecodeEntity model, bool IsAuthor, string ReciveUserStr)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            ExecResult result = new ExecResult();
            model.JournalID = CurAuthor.JournalID;
            model.SendUser = CurAuthor.AuthorID;
            model.MsgType = 2;
            model.SendDate = DateTime.Now;
            model.SendType = 0;
            model.SMServices = 1;
            if (ReciveUserStr.EndsWith(","))
                ReciveUserStr = ReciveUserStr.Substring(0, ReciveUserStr.Length - 1);
            ReciveUserStr = ReciveUserStr.Replace("，", ",");
            if (!ReciveUserStr.Contains(",") && !IsAuthor)
            {
                model.ReciveAddress = ReciveUserStr;
                result = service.SendEmailOrSms(model);
            }
            else
            {
                var strList = ReciveUserStr.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                if (IsAuthor)
                {
                    result = service.SendEmailOrSms(strList.Select(p => TypeParse.ToLong(p)).ToList(), model);
                }
                else
                {
                    result = service.SendEmailOrSms(strList, model);
                }
            }
            return Json(new { result = result.result, msg = result.msg });
        }

        /// <summary>
        /// 作者平台发送短信(不用模版)
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AuthorSendSms(Int64 CID, Int32 Status, MessageRecodeEntity model)
        {
            var user = GetAuthorInfo(CID, Status);
            if (user == null)
                return Json(new { result = EnumJsonResult.failure.ToString(), msg = "获取接收人失败，发送短信失败！" });

            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            ExecResult result = new ExecResult();
            model.JournalID = CurAuthor.JournalID;
            model.SendUser = CurAuthor.AuthorID;
            model.MsgType = 2;           
            model.SendDate = DateTime.Now;
            model.SendType = 0;

            //通过稿件编号获取接收人、接受地址
            model.ReciveUser = user.AuthorID;
            model.ReciveAddress = user.Mobile;

            result = service.SendEmailOrSms(model);
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
