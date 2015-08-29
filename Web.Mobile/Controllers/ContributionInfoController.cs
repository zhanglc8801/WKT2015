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
using WKT.Config;
using WKT.Common.Utils;
using System.Configuration;

namespace Web.Mobile.Controllers
{
    public class ContributionInfoController : BaseController
    {
        /// <summary>
        /// 最新状态
        /// </summary>
        /// <returns></returns>
        public ActionResult NewStatus()
        {
            return View();
        }

        # region 获取最新状态稿件列表
        /// <summary>
        /// 获取当前步骤的稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetContributionListAjax(long AuthorID)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity cirEntity = new CirculationEntity();
            cirEntity.JournalID = JournalID;
            cirEntity.CurAuthorID = AuthorID;
            cirEntity.AuthorID = AuthorID;
            cirEntity.CurrentPage = 1;
            cirEntity.PageSize = 500;
            Pager<FlowContribution> pager = service.GetAuthorContributionList(cirEntity);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        # endregion

        #region 查看详情
        /// <summary>
        /// 查看详情(作者平台)
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult View(Int64 CID, Int64 AuthorID)
        {
            ContributionInfoEntity model = GetModel(CID);
            if (model.AuthorList == null)
                model.AuthorList = new List<ContributionAuthorEntity>();
            if (model.ReferenceList == null)
                model.ReferenceList = new List<ContributionReferenceEntity>();
            if (model.FundList == null)
                model.FundList = new List<ContributionFundEntity>();
            if (model.AttModel == null)
                model.AttModel = new ContributionInfoAttEntity();
            model.CRemarkInfo = GetCRemarkModel(CID, AuthorID);
            return View();
        }

        /// <summary>
        /// 查看详情(编辑平台)
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult ViewDetail(Int64 CID, long StatusID, long FlowLogID)
        {
            ContributionInfoEntity model = GetModel(CID);
            model.StatusID = StatusID;
            model.FlowLogID = FlowLogID;
            if (model.AuthorList == null)
                model.AuthorList = new List<ContributionAuthorEntity>();
            if (model.ReferenceList == null)
                model.ReferenceList = new List<ContributionReferenceEntity>();
            if (model.FundList == null)
                model.FundList = new List<ContributionFundEntity>();
            if (model.AttModel == null)
                model.AttModel = new ContributionInfoAttEntity();
            model.CRemarkInfo = GetCRemarkModel(CID, CurAuthor.AuthorID);

            # region 更新查看状态

            try
            {
                FlowLogQuery query = new FlowLogQuery();
                query.JournalID = TypeParse.ToLong(ConfigurationManager.AppSettings["JournalID"]);
                query.FlowLogID = FlowLogID;
                IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                flowService.UpdateFlowLogIsView(query);
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("更新审稿日志的查看状态出现异常：" + ex.Message);
            }

            # endregion

            # region 排版后稿件

            ViewBag.ComposingContribution = "";
            string path = Server.MapPath("/UploadFile/Fangzheng/");
            string fileFullName = path + CID + "_single.pdf";
            if (System.IO.File.Exists(fileFullName))
            {
                ViewBag.ComposingContribution = CID + "_single.pdf";
            }
            # endregion

            return View(model);
        }

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult SearchViewDetail(Int64 CID)
        {
            ContributionInfoEntity model = GetModel(CID);
            if (model.AuthorList == null)
                model.AuthorList = new List<ContributionAuthorEntity>();
            if (model.ReferenceList == null)
                model.ReferenceList = new List<ContributionReferenceEntity>();
            if (model.FundList == null)
                model.FundList = new List<ContributionFundEntity>();
            if (model.AttModel == null)
                model.AttModel = new ContributionInfoAttEntity();
            model.CRemarkInfo = GetCRemarkModel(CID, CurAuthor.AuthorID);
            return View(model);
        }

        /// <summary>
        /// 查看详情(专家平台)
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult ViewExpert(Int64 CID, Int64 ExpertID, Int32 IsWait = 0)
        {
            ContributionInfoEntity model = GetModel(CID);
            if (model.AuthorList == null)
                model.AuthorList = new List<ContributionAuthorEntity>();
            if (model.ReferenceList == null)
                model.ReferenceList = new List<ContributionReferenceEntity>();
            if (model.FundList == null)
                model.FundList = new List<ContributionFundEntity>();
            if (model.AttModel == null)
                model.AttModel = new ContributionInfoAttEntity();
            model.CRemarkInfo = GetCRemarkModel(CID, CurAuthor.AuthorID);
            ViewBag.IsWait = IsWait;
            ViewBag.ExpertID = ExpertID;
            return View(model);
        }

        [HttpPost]
        public ActionResult GetMsgList(Int64 CID)
        {
            var list = GetMsgList(CID, CurAuthor.AuthorID, CurAuthor.GroupID);
            return Json(list);
        }

        [HttpPost]
        public ActionResult GetFinanceList(Int64 CID)
        {
            var list = GetFinanceList(CID, CurAuthor.AuthorID, CurAuthor.GroupID);
            return Json(list);
        }

        //[AjaxRequest]
        //public ActionResult GetFlowLogList(Int64 CID, Int64 AuthorID, int GroupID)
        //{
        //    var list = GetFlowLogList(CID, AuthorID, GroupID);
        //    return Json(list);
        //}
        //获取专家组的流程日志
        [HttpPost]
        public ActionResult GetExpertFlowLogList(Int64 CID, Int64 AuthorID)
        {
            var list = GetFlowLogList(CID, AuthorID, 3);
            return Json(list);
        }



        [HttpPost]
        public ActionResult SaveCRemark(CRemarkEntity model)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            model.JournalID = TypeParse.ToLong(ConfigurationManager.AppSettings["JournalID"]);
            model.AuthorID = CurAuthor.AuthorID;
            ExecResult result = service.SaveCRemark(model);
            return Json(new { result = result.result, msg = result.msg });
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 获取稿件实体
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        private ContributionInfoEntity GetModel(Int64 CID)
        {
            ContributionInfoEntity model = null;
            if (CID > 0)
            {
                ContributionInfoQuery query = new ContributionInfoQuery();
                query.JournalID = SiteConfig.SiteID;
                query.IsAuxiliary = true;
                query.CID = CID;
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                model = service.GetContributionInfoModel(query);
            }
            if (model == null)
                model = new ContributionInfoEntity();
            IContributionFacadeService serviceSet = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
            model.FieldList = serviceSet.GetFieldsSet().ToList();
            if (model.FieldList == null)
                model.FieldList = new List<FieldsSet>();
            return model;
        }

        private ContributionInfoEntity GetModel2(Int64 CID)
        {
            ContributionInfoEntity model = null;
            if (CID > 0)
            {
                ContributionInfoQuery query = new ContributionInfoQuery();
                query.JournalID = SiteConfig.SiteID;
                query.IsAuxiliary = true;
                query.CID = CID;
                query.isModify = true;
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                model = service.GetContributionInfoModel(query);
            }
            if (model == null)
                model = new ContributionInfoEntity();
            IContributionFacadeService serviceSet = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
            model.FieldList = serviceSet.GetFieldsSet().ToList();
            if (model.FieldList == null)
                model.FieldList = new List<FieldsSet>();
            return model;
        }

        /// <summary>
        /// 获取消息记录
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        private IList<MessageRecodeEntity> GetMsgList(Int64 CID, Int64 AuthorID, Byte GroupID)
        {
            if (CID == 0)
                return new List<MessageRecodeEntity>();
            IList<MessageRecodeEntity> list = null;
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            MessageRecodeQuery query = new MessageRecodeQuery();
            query.JournalID = SiteConfig.SiteID;
            query.CID = CID;
            if (GroupID != 1)
            {
                query.SendUser = AuthorID;
                query.ReciveUser = AuthorID;
                query.IsUserRelevant = true;
            }
            list = service.GetMessageRecodeList(query);

            if (list == null)
                list = new List<MessageRecodeEntity>();

            SiteMessageQuery msgQuery = new SiteMessageQuery();
            msgQuery.JournalID = CurAuthor.JournalID;
            msgQuery.CID = CID;
            if (GroupID != 1)
            {
                msgQuery.SendUser = AuthorID;
                msgQuery.ReciverID = AuthorID;
                msgQuery.IsUserRelevant = true;
            }
            var msgList = service.GetSiteMessageList(msgQuery).ToList();
            if (msgList != null && msgList.Count > 0)
            {
                MessageRecodeEntity model = null;
                foreach (var item in msgList)
                {
                    model = new MessageRecodeEntity();
                    model.JournalID = item.JournalID;
                    model.CID = item.CID;
                    model.SendUser = item.SendUser;
                    model.SendUserName = item.SendUserName;
                    model.ReciveUser = item.ReciverID;
                    model.ReciveUserName = item.ReciverName;
                    model.ReciveAddress = string.Empty;
                    model.SendDate = item.SendDate;
                    model.MsgType = 3;
                    model.SendType = 0;
                    model.MsgTitle = item.Title;
                    model.MsgContent = item.Content;
                    list.Add(model);
                }

            }

            return list.OrderBy(p => p.SendDate).ToList();
        }

        /// <summary>
        /// 获取稿件费用记录
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        private IList<FinanceContributeEntity> GetFinanceList(Int64 CID, Int64 AuthorID, Byte GroupID)
        {
            if (CID == 0)
                return new List<FinanceContributeEntity>();
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            FinanceContributeQuery query = new FinanceContributeQuery();
            query.JournalID = SiteConfig.SiteID;
            if (GroupID != 1)
            {
                query.AuthorID = AuthorID;
            }
            query.CID = CID;
            IList<FinanceContributeEntity> list = service.GetFinanceContributeList(query);
            return list;
        }

        /// <summary>
        /// 获取审核日志
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="AuthorID"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetFlowLogList(Int64 CID, Int64 AuthorID, Byte GroupID)
        {
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity cirEntity = new CirculationEntity();
            cirEntity.CID = CID;
            cirEntity.JournalID = JournalID;
            cirEntity.GroupID = GroupID;
            IList<FlowLogInfoEntity> list = flowService.GetFlowLog(cirEntity);
            if (GroupID != (Byte)EnumMemberGroup.Editor)
            {
                list = list.Where(p => p.RecUserID == AuthorID || p.SendUserID == AuthorID).ToList<FlowLogInfoEntity>();
            }
            return Json(list);
            
        }

        private IList<FlowLogInfoEntity> GetFlowLogList2(Int64 CID, Int64 AuthorID, Byte GroupID)
        {
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity cirEntity = new CirculationEntity();
            cirEntity.CID = CID;
            cirEntity.JournalID = JournalID;
            cirEntity.GroupID = GroupID;
            IList<FlowLogInfoEntity> flowLogList = flowService.GetFlowLog(cirEntity);
            if (GroupID != (Byte)EnumMemberGroup.Editor)
            {
                flowLogList = flowLogList.Where(p => p.RecUserID == AuthorID || p.SendUserID == AuthorID).ToList<FlowLogInfoEntity>();
            }
            return flowLogList;
        }

        /// <summary>
        /// 获取作者信息
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        private AuthorInfoEntity GetAuthorInfoModel(Int64 AuthorID)
        {
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            AuthorInfoQuery authorQuery = new AuthorInfoQuery();
            authorQuery.JournalID = JournalID;
            authorQuery.AuthorID = AuthorID;
            var model = authorService.GetAuthorInfo(authorQuery);
            return model;
        }

        /// <summary>
        /// 获取稿件备注实体
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="AuthorID"></param>       
        /// <returns></returns>
        private CRemarkEntity GetCRemarkModel(Int64 CID, Int64 AuthorID)
        {
            CRemarkEntity model = null;
            if (CID > 0)
            {
                CRemarkQuery query = new CRemarkQuery();
                query.JournalID = SiteConfig.SiteID;
                query.CID = CID;
                query.AuthorID = AuthorID;
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                model = service.GetCRemarkModel(query);
            }
            if (model == null)
                model = new CRemarkEntity();
            return model;
        }


        /// <summary>
        /// 获取当前作者详细信息
        /// </summary>
        /// <returns></returns>
        private AuthorDetailEntity GetAuthorDetailModel()
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            var model = service.GetAuthorDetailModel(new AuthorDetailQuery() { JournalID = CurAuthor.JournalID, AuthorID = CurAuthor.AuthorID });
            if (model == null)
                model = new AuthorDetailEntity();
            return model;
        }

        /// <summary>
        /// 获取编辑为作者投稿时 所选择的作者详细信息
        /// </summary>
        /// <returns></returns>
        private AuthorDetailEntity GetAuthorDetailModel(Int64 authorID)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            var model = service.GetAuthorDetailModel(new AuthorDetailQuery() { JournalID = CurAuthor.JournalID, AuthorID = authorID });
            if (model == null)
                model = new AuthorDetailEntity();
            return model;
        }

        private AuthorInfoEntity GetAuthorModel(Int64 authorID)
        {
            IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            var model = service.GetAuthorInfo(new AuthorInfoQuery() { JournalID = CurAuthor.JournalID, AuthorID = authorID });
            if (model == null)
                model = new AuthorInfoEntity();
            return model;
        }
        #endregion


    }
}
