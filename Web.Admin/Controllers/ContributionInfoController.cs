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
using WKT.Common.Data;
using System.Text;

namespace Web.Admin.Controllers
{
    public class ContributionInfoController : BaseController
    {
        /// <summary>
        /// 上传稿件提示页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Tips()
        {
            return View();
        }

        /// <summary>
        /// 投稿声明
        /// </summary>
        /// <returns></returns>
        public ActionResult Stat()
        {
            var model = GetAuthorDetailModel();
            if (model.PKID == 0)/// TODO:因为提取用户详情信息时是左联，所以这里用详细信息中的必填项来判断是否设置了详情信息
            {
                return Content("为了方便编辑与您联系，请先完善您的个人信息！<a href=\"" + SiteConfig.RootPath + "/AuthorDetail/UpdateSelf/\">设置个人信息</a>");
            }
            IContributionFacadeService serviceSet = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
            ContributeSetEntity cSetEntity = serviceSet.GetContributeSetInfo(new QueryBase() { JournalID = CurAuthor.JournalID });
            if (cSetEntity == null)
            {
                return Content("请先设置投稿相关信息！");
            }
            if (string.IsNullOrWhiteSpace(cSetEntity.Statement))
            {
                Response.Redirect(SiteConfig.RootPath + "/ContributionInfo/Index/", true);
            }
            return View(cSetEntity);
        }

        /// <summary>
        /// 投稿主页
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult Index(Int64 CID = 0)
        {
            var model = GetModelForModify(CID);
            var IsModifyFormat = Request["IsModifyFormat"];
            if( (model.Status > 0) && (model.Status != 100) )
            {
                return base.Content("当前稿件状态下不允许编辑！");
            }
            if (model.Status <= 0 || model.Status == 100)
            {
                if (model.AuthorList == null || model.AuthorList.Count < 1)
                {
                    if (CID == 0)
                    {
                        var authorModel = GetAuthorDetailModel();
                        if (authorModel.PKID == 0)/// TODO:因为提取用户详情信息时是左联，所以这里用详细信息中的必填项来判断是否设置了详情信息
                        {
                            return Content("为了方便编辑与您联系，请先完善您的个人信息！<a href=\"" + SiteConfig.RootPath + "/AuthorDetail/UpdateSelf/\">设置个人信息</a>");
                        }
                        model.AuthorList = new List<ContributionAuthorEntity>() { 
                            new ContributionAuthorEntity() { 
                                AuthorName = authorModel.AuthorName,
                                Gender=authorModel.Gender,
                                Birthday=authorModel.Birthday,
                                Nation=authorModel.Nation,
                                NativePlace=authorModel.NativePlace,
                                WorkUnit=authorModel.WorkUnit,
                                Tel=authorModel.Tel,
                                Email=authorModel.Emial,
                                SectionOffice=authorModel.SectionOffice,
                                ZipCode=authorModel.ZipCode,
                                Address=authorModel.Address
                            }
                        };
                    }
                    else
                    {
                        model.AuthorList = new List<ContributionAuthorEntity>() { new ContributionAuthorEntity() { AuthorName = CurAuthor.RealName } };
                    }
                }
                if (model.ReferenceList == null || model.ReferenceList.Count < 1)
                    model.ReferenceList = new List<ContributionReferenceEntity>() { new ContributionReferenceEntity() };
                if (model.FundList == null || model.FundList.Count < 1)
                    model.FundList = new List<ContributionFundEntity>() { new ContributionFundEntity() };
                if (model.AttModel == null)
                    model.AttModel = new ContributionInfoAttEntity();
                ViewBag.QQ = SiteConfig.QQ;
                ViewBag.IsModifyFormat = IsModifyFormat;
                if(CurAuthor.JournalID==20130107001)
                {
                    AuthorDetailQuery authorDetailQuery = new AuthorDetailQuery();
                    authorDetailQuery.JournalID = CurAuthor.JournalID;
                    authorDetailQuery.AuthorID = CurAuthor.AuthorID;
                    IAuthorPlatformFacadeService AuthorService = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                    ViewBag.Province = AuthorService.GetAuthorDetailModel(authorDetailQuery).Province;//获取作者地址以用于学科分类
                }
                return View(model);
            }
            else
            {
                return Content("当前稿件状态下不允许编辑！");
            }
        }

        
        /// <summary>
        /// 编辑为作者投稿 投稿主页
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult EditorIndex(long authorID,Int64 CID = 0)
        {
            var model = GetModelForModify(CID);
            if (model.Status <= 0 || model.Status == 100)
            {
                if (model.AuthorList == null || model.AuthorList.Count < 1)
                {
                    if (CID == 0)
                    {
                        var authorModel = GetAuthorDetailModel(authorID);
                        if (authorModel.PKID == 0)/// TODO:因为提取用户详情信息时是左联，所以这里用详细信息中的必填项来判断是否设置了详情信息
                        {
                            return Content("为了方便编辑与您联系，请先完善您的个人信息！<a href=\"" + SiteConfig.RootPath + "/AuthorDetail/UpdateSelf/\">设置个人信息</a>");
                        }
                        model.AuthorID = authorID;
                        AuthorInfoEntity  currentEntity=GetAuthorModel(authorID);
                        model.AuthorList = new List<ContributionAuthorEntity>() { 
                            new ContributionAuthorEntity() { 
                                AuthorName = authorModel.AuthorName,
                                Gender=authorModel.Gender,
                                Birthday=authorModel.Birthday,
                                Nation=authorModel.Nation,
                                NativePlace=authorModel.NativePlace,
                                WorkUnit=authorModel.WorkUnit,
                                Tel=authorModel.Tel,
                                Email=currentEntity.LoginName,
                                SectionOffice=authorModel.SectionOffice,
                                ZipCode=authorModel.ZipCode,
                                Address=authorModel.Address
                            }
                        };
                    }
                    else
                    {
                        model.AuthorList = new List<ContributionAuthorEntity>() { new ContributionAuthorEntity() { AuthorName = CurAuthor.RealName } };
                    }
                }
                if (model.ReferenceList == null || model.ReferenceList.Count < 1)
                    model.ReferenceList = new List<ContributionReferenceEntity>() { new ContributionReferenceEntity() };
                if (model.FundList == null || model.FundList.Count < 1)
                    model.FundList = new List<ContributionFundEntity>() { new ContributionFundEntity() };
                if (model.AttModel == null)
                    model.AttModel = new ContributionInfoAttEntity();
                ViewBag.QQ = SiteConfig.QQ;
                return View(model);
            }
            else
            {
                return Content("当前稿件状态下不允许编辑！");
            }
        }

        //编辑为作者投稿成功后跳转到的新投稿页面
        public ActionResult EditorNewArticles(long authorID)
        {
            ViewBag.AuthorID = authorID;
            return View();
        }

        //编辑为作者上传修改稿
        public ActionResult EditorRetreatIndex(long authorID)
        {
            ViewBag.AuthorID = authorID;
            return View();
        }


        [HttpPost]
        [AjaxRequest]
        public ActionResult Save(ContributionInfoEntity model)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            model.AuthorID =CurAuthor.GroupID==1?model.AuthorID:CurAuthor.AuthorID;
            model.AddDate = string.IsNullOrEmpty(model.CNumber) ? System.DateTime.Now : model.AddDate;
            ExecResult result = service.SaveContributionInfo(model);
            return Json(new { result = result.result, msg = result.msg, CID = result.resultID });
        }

        /// <summary>
        /// 保存稿件格式修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxRequest]
        public ActionResult SaveFormat(ContributionInfoEntity model)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            model.AuthorID = CurAuthor.GroupID == 1 ? model.AuthorID : CurAuthor.AuthorID;
            ExecResult result = service.SaveContributionInfoFormat(model);
            return Json(new { result = result.result, msg = result.msg});
        }

        [HttpPost]
        [AjaxRequest]
        public ActionResult AuthorManuscript(Int64[] CIDs)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            ContributionInfoQuery query = new ContributionInfoQuery();
            query.JournalID = CurAuthor.JournalID;
            query.CIDs = CIDs;
            query.Status = -100;
            ExecResult result = service.ChangeContributionInfoStatus(query);
            return Json(new { result = result.result, msg = result.result.Equals(EnumJsonResult.success.ToString()) ? "撤稿成功！" : "撤稿失败！" });
        }

        [HttpPost]
        public ActionResult GetPageList(ContributionInfoQuery query)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.AuthorID = CurAuthor.AuthorID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            query.OrderStr = " AddDate desc";
            Pager<ContributionInfoEntity> pager = service.GetContributionInfoPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 编辑代作者投稿/上传修改稿--获取作者固定状态的稿件列表(zhanglc)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAuthorPageList(ContributionInfoQuery query)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            //query.Status = -3;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            query.OrderStr = " AddDate desc";
            Pager<ContributionInfoEntity> pager = service.GetContributionInfoPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        public ActionResult SendSms(Int32 Status, Int64 CID)
        {
            ViewBag.Status = Status;
            ContributionInfoEntity model = GetModel(CID);
            if (model == null)
            {
                model = new ContributionInfoEntity();
            }
            return View(model);
        }

        /// <summary>
        /// 新投稿
        /// </summary>
        /// <returns></returns>
        public ActionResult NewArticles()
        {
            return View();
        }

        /// <summary>
        /// 暂存新稿
        /// </summary>
        /// <returns></returns>
        public ActionResult DraftIndex()
        {
            return View();
        }

        /// <summary>
        /// 退修稿件
        /// </summary>
        /// <returns></returns>
        public ActionResult RetreatIndex()
        {
            return View();
        }

        /// <summary>
        /// 已发校样
        /// </summary>
        /// <returns></returns>
        public ActionResult ProofIndex()
        {
            return View();
        }

        /// <summary>
        /// 录用稿件
        /// </summary>
        /// <returns></returns>
        public ActionResult EmploymentIndex()
        {
            return View();
        }

        /// <summary>
        /// 退稿
        /// </summary>
        /// <returns></returns>
        public ActionResult ManuscriptIndex()
        {
            return View();
        }

        /// <summary>
        /// 格式修改
        /// </summary>
        /// <returns></returns>
        public ActionResult FormatIndex()
        {
            return View();
        }

        /// <summary>
        /// 最新状态
        /// </summary>
        /// <returns></returns>
        public ActionResult NewStatus()
        {
            return View();
        }
        /// <summary>
        /// 查看稿件作者信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewContributionAuthor(long CAuthorID)
        {
            ContributionAuthorEntity CAuthorEntity = new ContributionAuthorEntity();
            ContributionAuthorQuery query= new ContributionAuthorQuery();
            query.JournalID = CurAuthor.JournalID;
            query.CAuthorID = CAuthorID;
            IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
            CAuthorEntity=service.GetContributionAuthorInfo(query);
            return View(CAuthorEntity);
        }


        # region 获取最新状态稿件列表
        /// <summary>
        /// 获取当前步骤的稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetContributionListAjax(CirculationEntity cirEntity)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            cirEntity.JournalID = JournalID;
            cirEntity.CurAuthorID = CurAuthor.AuthorID;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            cirEntity.CurrentPage = pageIndex;
            cirEntity.PageSize = TypeParse.ToInt(Request.Params["pagesize"], 10);
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
        public ActionResult View(Int64 CID)
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
            ViewBag.IsHideEditorInfoForAuthor = GetSiteSetCfg.GetSiteGlobalSet(CurAuthor.JournalID.ToString()).Rows[0]["IsHideEditorInfoForAuthor"].ToString();
            return View(model);
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
                query.JournalID = CurAuthor.JournalID;
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
            ViewBag.AdminID = CurAuthor.AuthorID;
            return View(model);
        }

        /// <summary>
        /// 查看详情(专家平台)
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult ViewExpert(Int64 CID, Int64 ExpertID, Byte GroupID, Int32 IsWait = 0)
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
            ViewBag.GroupID = GroupID;
            ViewBag.IsHideEditorInfoForExpert = GetSiteSetCfg.GetSiteGlobalSet(CurAuthor.JournalID.ToString()).Rows[0]["IsHideEditorInfoForExpert"].ToString();

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

        [HttpPost]
        public ActionResult GetFlowLogList(Int64 CID,Byte isViewMoreFlow)
        {
            var list = GetFlowLogList(CID, CurAuthor.AuthorID, CurAuthor.GroupID, isViewMoreFlow,0);
            return Json(list);
        }
        //获取专家组的流程日志
        [HttpPost]
        public ActionResult GetExpertFlowLogList(Int64 CID, Int64 AuthorID, Byte GroupID, Byte isViewHistoryFlow)
        {
            var list = GetFlowLogList(CID, AuthorID, GroupID, 0, isViewHistoryFlow);
            return Json(list);
        }

        [HttpPost]
        public ActionResult SaveCRemark(CRemarkEntity model)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            model.AuthorID = CurAuthor.AuthorID;
            ExecResult result = service.SaveCRemark(model);
            return Json(new { result = result.result, msg = result.msg });
        }
        #endregion

        #region 撤稿相关
        [HttpPost]
        public ActionResult Draft(RetractionsBillsEntity model, bool isAutoHandle)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            model.AuthorID = CurAuthor.AuthorID;
            ExecResult result = service.DraftContributionInfo(model);
            if (isAutoHandle == true)
            {
                ContributionInfoQuery cQueryEntity = new ContributionInfoQuery();
                cQueryEntity.JournalID = CurAuthor.JournalID;
                cQueryEntity.CID = model.CID;
                IContributionFacadeService cservice = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                cservice.DealWithdrawal(cQueryEntity);
            }
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult GetDraftModel(Int64 CID)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            RetractionsBillsQuery query = new RetractionsBillsQuery();
            query.JournalID = CurAuthor.JournalID;
            query.CID = CID;
            var model = service.GetRetractionsBillsModel(query);
            return Json(new { result = model != null ? EnumJsonResult.success.ToString() : EnumJsonResult.failure.ToString(), model = model });
        }
        #endregion

        #region 收到的信息
        /// <summary>
        /// 收到信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ReciveSmsIndex()
        {
            return View();
        }

        /// <summary>
        /// 回复信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ReplySms(Byte MsgType = 1)
        {
            ViewBag.MsgType = MsgType;
            return View();
        }

        [HttpPost]
        public ActionResult GetReciveSmsPageList(MessageRecodeQuery query)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.ReciveUser = CurAuthor.AuthorID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<MessageRecodeEntity> pager = service.GetMessageRecodePageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
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
                query.JournalID = CurAuthor.JournalID;
                query.IsAuxiliary = true;
                query.CID = CID;
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                model = service.GetContributionInfoModel(query);
            }
            if (model == null)
                model = new ContributionInfoEntity();
            IContributionFacadeService serviceSet = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
            model.FieldList = serviceSet.GetFieldsSet().ToList();
            model.ContributeAuthorFieldList = serviceSet.GetContributionAuthorFieldsSet().ToList();
            if (model.FieldList == null)
                model.FieldList = new List<FieldsSet>();
            return model;
        }

        /// <summary>
        /// 获取处于修改状态下的稿件实体(用于去除重复的联系电话信息)
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        private ContributionInfoEntity GetModelForModify(Int64 CID)
        {
            ContributionInfoEntity model = null;
            if (CID > 0)
            {
                ContributionInfoQuery query = new ContributionInfoQuery();
                query.JournalID = CurAuthor.JournalID;
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
            model.ContributeAuthorFieldList = serviceSet.GetContributionAuthorFieldsSet().ToList();
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
            query.JournalID = CurAuthor.JournalID;
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
            query.JournalID = CurAuthor.JournalID;
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
        /// <param name="CID">稿件ID</param>
        /// <param name="AuthorID">作者、专家ID</param>
        /// <param name="GroupID">组ID</param>
        /// <param name="isViewMoreFlow">是否允许作者界面显示更多审稿信息 1=是 0=否</param>
        /// <param name="isViewMoreFlow">是否允许专家界面显示历史审稿信息 1=是 0=否</param>
        /// <returns></returns>
        private IList<FlowLogInfoEntity> GetFlowLogList(Int64 CID, Int64 AuthorID, Byte GroupID, Byte isViewMoreFlow, Byte isViewHistoryFlow)
        {
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity cirEntity = new CirculationEntity();
            cirEntity.CID = CID;
            cirEntity.JournalID = JournalID;
            cirEntity.GroupID = GroupID;
            cirEntity.isViewMoreFlow = isViewMoreFlow;
            cirEntity.isViewHistoryFlow = isViewHistoryFlow;
            if (isViewHistoryFlow == 1)
                cirEntity.AuthorID = AuthorID;
            IList<FlowLogInfoEntity> flowLogList = flowService.GetFlowLog(cirEntity);
            if (GroupID != (Byte)EnumMemberGroup.Editor && isViewMoreFlow!=1 && isViewHistoryFlow!=1)
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
                query.JournalID = CurAuthor.JournalID;
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

        /// <summary>
        /// 获取稿件作者列表
        /// </summary>
        /// <param name="CID">稿件ID</param>
        /// <returns></returns>
        public ActionResult GetContributionAuthorList(Int64 CID)
        {
            ContributionInfoEntity model = GetModel(CID);
            var authorList = model.AuthorList;
            return Json(new { list = authorList });
        }
        /// <summary>
        /// 获取稿件作者列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetContributionAuthorListAjax(ContributionInfoQuery query)
        {
            query.JournalID = CurAuthor.JournalID;
            query.IsAuxiliary = true;
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            IList<ContributionInfoEntity> list = service.GetContributionInfoList(query);
            return Json(list);
        }

        /// <summary>
        /// 字符串保存为Word文件
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult SaveToWord(string cNumber, string title, string year, string issue, string startDate, string endDate, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            ContributionInfoQuery query = new ContributionInfoQuery();
            query.JournalID = CurAuthor.JournalID;
            query.IsAuxiliary = true;
            query.CNumber = cNumber;
            query.Title = title;
            if (year == "")
                query.Year = null;
            else
                query.Year = int.Parse(year);
            if (issue == "")
                query.Issue = null;
            else
                query.Issue = int.Parse(issue);
            if (startDate == "")
                query.StartDate = null;
            else
                query.StartDate = Convert.ToDateTime(startDate);
            if (endDate == "")
                query.EndDate = null;
            else
                query.EndDate = Convert.ToDateTime(endDate);
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            IList<ContributionInfoEntity> list = service.GetContributionInfoList(query);
            sb.Append("<div id=\"divWorkloadList\" style=\"margin-left:10px;width:1100px;float:left;\">");
            
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].AuthorList.Count;j++ )
                {
                    sb.Append("<table border=\"0\" bgcolor=\"#ffffff\" cellpadding=\"4\" cellspacing=\"0\" style=\"border: solid 1px #333; float:left; margin-right:10px; margin-bottom:10px;width:450px;height:20px;font-size:12px;\">");
                    sb.Append("<tr><td height=\"25\">" + list[i].AuthorList[j].ZipCode + "</td></tr>");
                    sb.Append("<tr><td height=\"25\">&nbsp;&nbsp;邮：" + list[i].AuthorList[j].Address + "</td></tr>");
                    sb.Append("<tr><td height=\"30\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + list[i].AuthorList[j].AuthorName + "（收）</td></tr>");
                    sb.Append("<tr><td height=\"30\" align=\"right\">北京市天坛西里2号《药物分析杂志》编辑部 邮编：100050 &nbsp;&nbsp;&nbsp;&nbsp;</td></tr>");
                    sb.Append("</table>");
                }
            }
            
            sb.Append("</div>");
            RenderToWord.HtmlToWord(Server.UrlDecode(sb.ToString()), "123.doc", true);
            return Content("导出成功！");
        }


        public ActionResult PrintReviewBill(long CID)
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
            ViewBag.AdminID = CurAuthor.AuthorID;
            return View(model);
        }


    }
}
