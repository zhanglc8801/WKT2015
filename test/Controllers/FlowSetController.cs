using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Common.Utils;
using WKT.Config;
using WKT.Log;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;
using WKT.Common.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Net;

namespace HanFang360.InterfaceService.Controllers
{
    /// <summary>
    /// 流程设置
    /// </summary>
    public class FlowSetController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加流程状态
        /// </summary>
        /// <returns></returns>
        public ActionResult AddFlowStatus()
        {
            return View();
        }

        /// <summary>
        /// 修改流程状态
        /// </summary>
        /// <returns></returns>
        public ActionResult EditFlowStatus(long StatusID)
        {
            FlowStep stepEntity = new FlowStep();
            try
            {
                FlowStatusQuery query = new FlowStatusQuery();
                query.JournalID = JournalID;
                query.StatusID = StatusID;
                IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                stepEntity = flowService.GetFlowStepInfo(query);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取指定的审稿状态及配置信息时出现异常：" + ex.Message);
            }

            return View(stepEntity);
        }

        /// <summary>
        /// 流转状态操作设置
        /// </summary>
        /// <returns></returns>
        public ActionResult FlowActionSet(long StatusID, long? ActionID)
        {
            FlowActionEntity actionEntity = null;
            if (ActionID == null)
            {
                actionEntity = new FlowActionEntity();
                actionEntity.JournalID = JournalID;
                actionEntity.StatusID = StatusID;
            }
            else
            {
                FlowActionQuery actionQuery = new FlowActionQuery();
                actionQuery.JournalID = JournalID;
                actionQuery.ActionID = ActionID.Value;
                IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                actionEntity = flowService.GetFlowActionEntity(actionQuery);
            }
            return View(actionEntity);
        }

        /// <summary>
        /// 流转步骤权限设置
        /// </summary>
        /// <returns></returns>
        public ActionResult FlowAuthSet(long ActionID, long StatusID)
        {
            ViewBag.ActionID = ActionID;
            return View();
        }

        /// <summary>
        /// 验证操作是否是通知交审稿费/版面费
        /// </summary>
        /// <param name="actionID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidateIsPayNotice(long actionID)
        {
            ISiteConfigFacadeService facadeService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            DictEntity PayNoticeDict = facadeService.GetDictModelByKey(new DictQuery() { JournalID = CurAuthor.JournalID, DictKey = "PayNotice" });//通知交审稿费
            DictEntity PayedDict = facadeService.GetDictModelByKey(new DictQuery() { JournalID = CurAuthor.JournalID, DictKey = "Payed" });//已交审稿费

            DictEntity PayPageNoticeDict = facadeService.GetDictModelByKey(new DictQuery() { JournalID = CurAuthor.JournalID, DictKey = "PayPageNotice" });//通知交版面费
            DictEntity PagePayedDict = facadeService.GetDictModelByKey(new DictQuery() { JournalID = CurAuthor.JournalID, DictKey = "PagePayed" });//已交版面费

            #region 验证操作-通知交审稿费
            if (PayNoticeDict != null)
            {
                ISiteConfigFacadeService currentService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                IList<DictValueEntity> list = currentService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = PayNoticeDict.DictKey });
                if (list != null && list.Count > 0)
                {
                    DictValueEntity entity = list.Single<DictValueEntity>();
                    if (entity != null)
                    {
                        FlowActionQuery actionQuery = new FlowActionQuery();
                        actionQuery.JournalID = JournalID;
                        actionQuery.ActionID = actionID;
                        IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                        FlowActionEntity actionEntity = flowService.GetFlowActionEntity(actionQuery);
                        if (actionEntity != null)
                        {
                            if (actionEntity.TOStatusID == entity.ValueID)
                            {
                                return Json(new { flag = 1, AuthorID = (CurAuthor != null ? CurAuthor.AuthorID : 0), payType = 1 });
                            }
                        }

                    }

                }
            } 
            #endregion

            #region 验证操作-已交审稿费
            if (PayedDict != null)
            {
                ISiteConfigFacadeService currentService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                IList<DictValueEntity> list = currentService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = PayedDict.DictKey });
                if (list != null && list.Count > 0)
                {
                    DictValueEntity entity = list.SingleOrDefault<DictValueEntity>();
                    if (entity != null)
                    {
                        FlowActionQuery actionQuery = new FlowActionQuery();
                        actionQuery.JournalID = JournalID;
                        actionQuery.ActionID = actionID;
                        IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                        FlowActionEntity actionEntity = flowService.GetFlowActionEntity(actionQuery);
                        if (actionEntity != null)
                        {
                            if (actionEntity.TOStatusID == entity.ValueID)
                            {
                                return Json(new { flag = 2, AuthorID = (CurAuthor != null ? CurAuthor.AuthorID : 0), payType = 1 });
                            }
                        }

                    }

                }
            } 
            #endregion

            #region 验证操作-通知交版面费
            if (PayPageNoticeDict != null)
            {
                ISiteConfigFacadeService currentService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                IList<DictValueEntity> list = currentService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = PayPageNoticeDict.DictKey });
                if (list != null && list.Count > 0)
                {
                    DictValueEntity entity = list.SingleOrDefault<DictValueEntity>();
                    if (entity != null)
                    {
                        FlowActionQuery actionQuery = new FlowActionQuery();
                        actionQuery.JournalID = JournalID;
                        actionQuery.ActionID = actionID;
                        IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                        FlowActionEntity actionEntity = flowService.GetFlowActionEntity(actionQuery);
                        if (actionEntity != null)
                        {
                            if (actionEntity.TOStatusID == entity.ValueID)
                            {
                                return Json(new { flag = 1, AuthorID = (CurAuthor != null ? CurAuthor.AuthorID : 0), payType = 2 });
                            }
                        }

                    }

                }
            }
            #endregion

            #region 验证操作-已交版面费
            if (PagePayedDict != null)
            {
                ISiteConfigFacadeService currentService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                IList<DictValueEntity> list = currentService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = PagePayedDict.DictKey });
                if (list != null && list.Count > 0)
                {
                    DictValueEntity entity = list.SingleOrDefault<DictValueEntity>();
                    if (entity != null)
                    {
                        FlowActionQuery actionQuery = new FlowActionQuery();
                        actionQuery.JournalID = JournalID;
                        actionQuery.ActionID = actionID;
                        IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                        FlowActionEntity actionEntity = flowService.GetFlowActionEntity(actionQuery);
                        if (actionEntity != null)
                        {
                            if (actionEntity.TOStatusID == entity.ValueID)
                            {
                                return Json(new { flag = 2, AuthorID = (CurAuthor != null ? CurAuthor.AuthorID : 0), payType = 2 });
                            }
                        }

                    }

                }
            }
            #endregion

            return Json(new { flag = 0 });
            
        }

        /// <summary>
        /// 审稿单
        /// </summary>
        /// <param name="ActionID">当前所做操作ID</param>
        /// <param name="StatusID">当前状态</param>
        /// <param name="CIDS">稿件ID,多个稿件ID用逗号分隔</param>
        /// <returns></returns>
        public ActionResult AuditBill(long ActionID, long StatusID, string CIDS)
        {
            //继续送交
            int IsContinueSubmit = 0;
            int ActionType = 0;//操作类型:标识继续送交/继续送专家复审
            if (ActionID == 0)
            {
                ActionType = 0;
                ISiteConfigFacadeService facadeService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                DictEntity dicteEntity = facadeService.GetDictModelByKey(new DictQuery() { JournalID = CurAuthor.JournalID, DictKey = "MoreSubmit" });
                if (dicteEntity != null)
                {
                    IsContinueSubmit = 1;
                    ISiteConfigFacadeService currentService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                    IList<DictValueEntity> list = currentService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = dicteEntity.DictKey });
                    if (list != null && list.Count > 0)
                    {
                        DictValueEntity entity = list.Single<DictValueEntity>();
                        if (entity != null)
                        {
                            ActionID = entity.ValueID;
                        }
                    }
                }
                else
                {
                    IsContinueSubmit = -1;
                }
            }

            //继续送复审
            int IsContinueReSubmit = 0;
            if (ActionID == -1)
            {
                ActionType = -1;
                ISiteConfigFacadeService facadeService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                DictEntity dicteEntity = facadeService.GetDictModelByKey(new DictQuery() { JournalID = CurAuthor.JournalID, DictKey = "MoreReSubmit" });
                if (dicteEntity != null)
                {
                    IsContinueReSubmit = 1;
                    ISiteConfigFacadeService currentService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                    IList<DictValueEntity> list = currentService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = dicteEntity.DictKey });
                    if (list != null && list.Count > 0)
                    {
                        DictValueEntity entity = list.Single<DictValueEntity>();
                        if (entity != null)
                        {
                            ActionID = entity.ValueID;
                        }
                    }
                }
                else
                {
                    IsContinueReSubmit = -1;
                }
                
            }

            //新加实体
            SiteConfigQuery SiteConfigQuery = new SiteConfigQuery();
            SiteConfigQuery.JournalID = CurAuthor.JournalID;
            ISiteConfigFacadeService SiteConfigService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigEntity model = SiteConfigService.GetSiteConfigModel(SiteConfigQuery);
            if (model == null)
                model = new SiteConfigEntity();
            CirculationEntity cirEntity = new CirculationEntity();
            cirEntity.ActionID = ActionID;
            cirEntity.JournalID = JournalID;
            cirEntity.StatusID = StatusID;
            IDictionary<long, long> dictCIDMap = new Dictionary<long, long>();
            string[] CIDArray = CIDS.Split(',');
            long CID = 0;
            long FlowLogID = 0;
            foreach (string CIDLogID in CIDArray)
            {
                if (!string.IsNullOrEmpty(CIDLogID))
                {
                    string[] CIDANDLogID = CIDLogID.Split(':');
                    CID = TypeParse.ToLong(CIDANDLogID[0]);
                    FlowLogID = TypeParse.ToLong(CIDANDLogID[1]);
                    if (!dictCIDMap.ContainsKey(CID) && CID > 0)
                    {
                        dictCIDMap.Add(CID, FlowLogID);
                    }
                }
            }
            cirEntity.CIDS = string.Join(",", dictCIDMap.Keys);
            cirEntity.DictLogID = dictCIDMap;
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            FlowStep flowStepInfo = flowService.GetNextFlowStep(cirEntity);

            # region 让当前用户排第一个

            if (flowStepInfo.FlowAuthorList != null)
            {
                WKT.Model.AuthorInfoEntity curEditor = null;
                foreach (WKT.Model.AuthorInfoEntity item in flowStepInfo.FlowAuthorList)
                {
                    if (item.AuthorID == CurAuthor.AuthorID)
                    {
                        curEditor = item;
                        break;
                    }
                }
                if (curEditor != null)
                {
                    flowStepInfo.FlowAuthorList.Remove(curEditor);
                    flowStepInfo.FlowAuthorList.Insert(0, curEditor);
                }
            }

            # endregion

            flowStepInfo.DictLogID = dictCIDMap;
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            MessageTemplateQuery query = new MessageTemplateQuery();
            query.JournalID = CurAuthor.JournalID;
            query.TType = 1;
            if (flowStepInfo.FlowAction != null)
            {
                if (flowStepInfo.FlowAction.EmailTemplate > 0)
                {
                    query.TemplateID = flowStepInfo.FlowAction.EmailTemplate;
                    flowStepInfo.EmailTemplate = service.GetMessageTempModel(query);
                    if (CIDArray.Length == 1)//仅选择一篇稿件处理时，替换其中的稿件编号/标题变量为具体内容
                        flowStepInfo.EmailTemplate.TContent = ReplaceContent(CID,flowStepInfo.EmailTemplate.TContent);
                }
                if (flowStepInfo.FlowAction.SMSTemplate > 0)
                {
                    query.TType = 2;
                    query.TemplateID = flowStepInfo.FlowAction.SMSTemplate;
                    flowStepInfo.SMSTemplate = service.GetMessageTempModel(query);
                    if (CIDArray.Length == 1)
                        flowStepInfo.SMSTemplate.TContent = ReplaceContent(CID, flowStepInfo.SMSTemplate.TContent);
                }
            }
            else
            {
                flowStepInfo.FlowAction = new FlowActionEntity { ActionID = -1, ActionType = 1, ActionRoleID = 0 };
                flowStepInfo.FlowConfig = new FlowConfigEntity();
            }
            ViewBag.StatusID = StatusID;
            ViewBag.ActionType = ActionType;
            ViewBag.IsContinueSubmit = IsContinueSubmit;
            ViewBag.IsContinueReSubmit = IsContinueReSubmit;
            return View(flowStepInfo);
        }

        #region 替换内容中的稿件编号及稿件标题
        /// <summary>
        /// 替换内容中的稿件编号及稿件标题
        /// 由于接收人可能存在多个，替换后无法使用全局变量更改为每个具体人员信息，故接收人信息不作替换！
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        private string ReplaceContent(long CID, string Content)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            IDictionary<string, string> dict = service.GetEmailVariable();
            if (dict != null && dict.Count > 0)
            {
                dict.Remove("${审稿链接}$");
                dict.Remove("${作者链接}$");
                dict.Remove("${发送人}$");
                dict.Remove("${接收人}$");
                dict.Remove("${邮箱}$");
                dict.Remove("${手机}$");
                dict.Remove("${系统日期}$");
                dict.Remove("${系统时间}$");
                dict.Remove("${网站名称}$");
                dict.Remove("${编辑部地址}$");
                dict.Remove("${编辑部邮编}$");
                dict.Remove("${审毕日期}$");
                dict.Remove("${金额}$");
            }
            if (!string.IsNullOrEmpty(Content))
            {
                # region 获取稿件信息

                IAuthorPlatformFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                ContributionInfoQuery authorQuery = new ContributionInfoQuery();
                authorQuery.JournalID = JournalID;
                authorQuery.CID = CID;
                authorQuery.IsAuxiliary = false;
                var contribution = authorService.GetContributionInfoModel(authorQuery);

                # endregion
                if (Content.Contains("${稿件编号}$") || Content.Contains("${稿件标题}$"))
                {
                    dict["${稿件编号}$"] = contribution.CNumber;
                    dict["${稿件标题}$"] = contribution.Title;
                }
            }
            string strContent = service.GetEmailOrSmsContent(dict, Content);
            return strContent;
        } 
        #endregion
   
        /// <summary>
        /// 审稿单
        /// </summary>
        /// <param name="ActionID">当前所做操作ID</param>
        /// <param name="StatusID">当前状态</param>
        /// <param name="CIDS">稿件ID,多个稿件ID用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetContributeAuthor(long ActionID, long StatusID, string singleCID)
        {
            CirculationEntity cirEntity = new CirculationEntity();
            cirEntity.ActionID = ActionID;
            cirEntity.JournalID = JournalID;
            cirEntity.StatusID = StatusID;
            IDictionary<long, long> dictCIDMap = new Dictionary<long, long>();
            string[] CIDArray = singleCID.Split(',');
            long CID = 0;
            long FlowLogID = 0;
            foreach (string CIDLogID in CIDArray)
            {
                if (!string.IsNullOrEmpty(CIDLogID))
                {
                    string[] CIDANDLogID = CIDLogID.Split(':');
                    CID = TypeParse.ToLong(CIDANDLogID[0]);
                    FlowLogID = TypeParse.ToLong(CIDANDLogID[1]);
                    if (!dictCIDMap.ContainsKey(CID) && CID > 0)
                    {
                        dictCIDMap.Add(CID, FlowLogID);
                    }
                }
            }
            cirEntity.CIDS = string.Join(",", dictCIDMap.Keys);
            cirEntity.DictLogID = dictCIDMap;
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            FlowStep flowStepInfo = flowService.GetNextFlowStep(cirEntity);

            # region 让当前用户排第一个

            if (flowStepInfo.FlowAuthorList != null)
            {
                WKT.Model.AuthorInfoEntity curEditor = null;
                foreach (WKT.Model.AuthorInfoEntity item in flowStepInfo.FlowAuthorList)
                {
                    if (item.AuthorID == CurAuthor.AuthorID)
                    {
                        curEditor = item;
                        break;
                    }
                }
                if (curEditor != null)
                {
                    flowStepInfo.FlowAuthorList.Remove(curEditor);
                    flowStepInfo.FlowAuthorList.Insert(0, curEditor);
                }
            }

            # endregion

            return Content(JsonConvert.SerializeObject(flowStepInfo));
        }

        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="ActionID">操作ID</param>
        /// <param name="StatusID">当前审稿状态</param>
        /// <param name="CIDS">稿件ID,多个稿件ID用逗号分隔</param>
        /// <returns></returns>
        public ActionResult SendMessage(long ActionID, long StatusID, string CIDS)
        {
            CirculationEntity cirEntity = new CirculationEntity();
            cirEntity.ActionID = ActionID;
            cirEntity.JournalID = JournalID;
            cirEntity.StatusID = StatusID;
            IDictionary<long, long> dictCIDMap = new Dictionary<long, long>();
            string[] CIDArray = CIDS.Split(',');
            long CID = 0;
            long FlowLogID = 0;
            foreach (string CIDLogID in CIDArray)
            {
                if (!string.IsNullOrEmpty(CIDLogID))
                {
                    string[] CIDANDLogID = CIDLogID.Split(':');
                    CID = TypeParse.ToLong(CIDANDLogID[0]);
                    FlowLogID = TypeParse.ToLong(CIDANDLogID[1]);
                    if (!dictCIDMap.ContainsKey(CID) && CID > 0)
                    {
                        dictCIDMap.Add(CID, FlowLogID);
                    }
                }
            }
            cirEntity.CIDS = string.Join(",", dictCIDMap.Keys);
            cirEntity.DictLogID = dictCIDMap;
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            FlowStep flowStepInfo = flowService.GetNextFlowStep(cirEntity);
            flowStepInfo.DictLogID = dictCIDMap;
            if (flowStepInfo.FlowAction != null)
            {
                MessageTemplateQuery query = new MessageTemplateQuery();
                query.JournalID = CurAuthor.JournalID;
                query.TType = 1;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();

                if (flowStepInfo.FlowAction.EmailTemplate > 0)
                {
                    query.TemplateID = flowStepInfo.FlowAction.EmailTemplate;
                    flowStepInfo.EmailTemplate = service.GetMessageTempModel(query);
                }

                if (flowStepInfo.FlowAction.SMSTemplate > 0)
                {
                    query.TType = 2;
                    query.TemplateID = flowStepInfo.FlowAction.SMSTemplate;
                    flowStepInfo.SMSTemplate = service.GetMessageTempModel(query);
                }
            }
            ViewBag.StatusID = StatusID;
            return View(flowStepInfo);
        }

        # region ajax

        # region 审稿状态

        /// <summary>
        /// 获取审稿状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetFlowStatusListGridAjax(FlowStatusQuery query)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            List<FlowStatusEntity> listResult = service.GetFlowStatusList(query);
            return Json(new { Rows = listResult });
        }

        /// <summary>
        /// 获取审稿状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetFlowStatusListDataAjax(FlowStatusQuery query)
        {
            JsonExecResult<List<FlowStatusEntity>> execResult = new JsonExecResult<List<FlowStatusEntity>>();
            try
            {
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                query.JournalID = JournalID;
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "";
                execResult.ReturnObject = service.GetFlowStatusList(query);
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "获取审稿状态(数据)出现异常：" + ex.Message;
                execResult.ReturnObject = new List<FlowStatusEntity>(0);
                LogProvider.Instance.Error("获取审稿状态(数据)出现异常：" + ex.Message);
            }
            return Content(JsonConvert.SerializeObject(execResult));
        }

        /// <summary>
        /// 获取审稿状态序号
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetFlowStatusNum()
        {
            JsonExecResult<int> execResult = new JsonExecResult<int>();
            try
            {
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                FlowStatusQuery query = new FlowStatusQuery();
                query.JournalID = JournalID;
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "";
                execResult.ReturnObject = service.GetFlowStatusSortID(query);
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "获取审稿状态序号出现异常：" + ex.Message;
                execResult.ReturnObject = 0;
                LogProvider.Instance.Error("获取审稿状态序号出现异常：" + ex.Message);
            }
            return Content(JsonConvert.SerializeObject(execResult));
        }

        /// <summary>
        /// 获取流程状态及配置信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetFlowStatusInfo(FlowStatusQuery query)
        {
            JsonExecResult<FlowStep> execResult = new JsonExecResult<FlowStep>();
            try
            {
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                query.JournalID = JournalID;
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "";
                execResult.ReturnObject = service.GetFlowStepInfo(query);
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "获取审稿状态及配置信息出现异常：" + ex.Message;
                execResult.ReturnObject = new FlowStep();
                LogProvider.Instance.Error("获取审稿状态及配置信息出现异常：" + ex.Message);
            }
            return Content(JsonConvert.SerializeObject(execResult));
        }

        /// <summary>
        /// 新增审稿状态
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult AddFlowStatusAjax(FlowStep flowStepEntity)
        {
            ExecResult exeResult = new ExecResult();
            try
            {
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                FlowStatusEntity statusEntity = null;
                if (flowStepEntity.FlowStatus.CStatus != 0)
                {
                    // 判断该状态对应的稿件状态是否存在
                    FlowStatusQuery statusQuery = new FlowStatusQuery();
                    statusQuery.JournalID = JournalID;
                    statusQuery.CStatus = flowStepEntity.FlowStatus.CStatus;
                    statusEntity = service.CheckCStatusIsExists(statusQuery);
                    if (statusEntity != null)
                    {
                        exeResult.result = EnumJsonResult.failure.ToString();
                        exeResult.msg = string.Format("当前选择的稿件状态已经有审稿状态[{0}]匹配，请检查!", statusEntity.StatusName);
                        return Content(JsonConvert.SerializeObject(exeResult));
                    }
                }
                flowStepEntity.FlowStatus.JournalID = JournalID;
                flowStepEntity.FlowConfig.JournalID = JournalID;
                exeResult = service.AddFlowStatus(flowStepEntity);
            }
            catch (Exception ex)
            {
                exeResult.result = EnumJsonResult.error.ToString();
                exeResult.msg = "新增审稿状态出现异常：" + ex.Message;
                LogProvider.Instance.Error("新增审稿状态出现异常：" + ex.Message);
            }
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        /// <summary>
        /// 修改审稿状态
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult EditFlowStatusAjax(FlowStep flowStepEntity)
        {
            ExecResult exeResult = new ExecResult();
            try
            {
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                FlowStatusEntity statusEntity = null;
                if (flowStepEntity.FlowStatus.CStatus != 0)
                {
                    // 判断该状态对应的稿件状态是否存在
                    FlowStatusQuery statusQuery = new FlowStatusQuery();
                    statusQuery.JournalID = JournalID;
                    statusQuery.CStatus = flowStepEntity.FlowStatus.CStatus;
                    statusEntity = service.CheckCStatusIsExists(statusQuery);
                    if (statusEntity != null && flowStepEntity.FlowStatus.StatusID != statusEntity.StatusID)
                    {
                        exeResult.result = EnumJsonResult.failure.ToString();
                        exeResult.msg = string.Format("当前选择的稿件状态已经有审稿状态[{0}]匹配，请检查!", statusEntity.StatusName);
                        return Content(JsonConvert.SerializeObject(exeResult));
                    }
                }
                flowStepEntity.FlowStatus.JournalID = JournalID;
                flowStepEntity.FlowStatus.EditAuthorID = CurAuthor.AuthorID;
                flowStepEntity.FlowStatus.EditDate = DateTime.Now;
                flowStepEntity.FlowConfig.JournalID = JournalID;
                exeResult = service.EditFlowStatus(flowStepEntity);
            }
            catch (Exception ex)
            {
                exeResult.result = EnumJsonResult.error.ToString();
                exeResult.msg = "修改审稿状态出现异常：" + ex.Message;
                LogProvider.Instance.Error("修改审稿状态出现异常：" + ex.Message);
            }
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        /// <summary>
        /// 删除审稿状态
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult DelFlowStatusAjax(FlowStatusEntity flowStatusEntity)
        {
            ExecResult exeResult = new ExecResult();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            flowStatusEntity.JournalID = CurAuthor.JournalID;
            exeResult = service.DelFlowStatus(flowStatusEntity);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        /// <summary>
        /// 修改审稿状态的状态
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult EditStatusAjax(FlowStatusEntity flowStatusEntity)
        {
            ExecResult exeResult = new ExecResult();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            flowStatusEntity.JournalID = CurAuthor.JournalID;
            exeResult = service.UpdateFlowStatus(flowStatusEntity);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        # endregion

        # region 审稿操作设置

        /// <summary>
        /// 获取流程操作
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetFlowActionEntityAjax(long ActionID)
        {
            JsonExecResult<FlowActionEntity> jsonResult = new JsonExecResult<FlowActionEntity>();
            try
            {
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                FlowActionQuery query = new FlowActionQuery();
                query.ActionID = ActionID;
                query.JournalID = JournalID;
                jsonResult.ItemList = new List<FlowActionEntity>();
                jsonResult.ItemList.Add(service.GetFlowActionEntity(query));
                jsonResult.result = EnumJsonResult.success.ToString();
            }
            catch (Exception ex)
            {
                jsonResult.result = EnumJsonResult.error.ToString();
                jsonResult.msg = "获取流程操作出现异常：" + ex.Message;
            }
            return Json(jsonResult);
        }

        /// <summary>
        /// 获取流程操作列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetFlowActionListByIDAjax(FlowActionQuery query)
        {
            JsonExecResult<FlowActionEntity> jsonResult = new JsonExecResult<FlowActionEntity>();
            try
            {
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                query.JournalID = JournalID;
                jsonResult.ItemList = service.GetFlowActionList(query);
                jsonResult.result = EnumJsonResult.success.ToString();
            }
            catch (Exception ex)
            {
                jsonResult.result = EnumJsonResult.error.ToString();
                jsonResult.msg = "根据步骤ID获取流程操作列表出现异常：" + ex.Message;
            }
            return Json(jsonResult);
        }

        /// <summary>
        /// 获取流程操作列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetFlowActionListAjax(FlowActionQuery query)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            query.JournalID = JournalID;
            List<FlowActionEntity> listResult = service.GetFlowActionList(query);
            return Json(new { Rows = listResult });
        }

        /// <summary>
        /// 新增流程操作
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult AddFlowActionAjax(FlowActionEntity flowActionEntity)
        {
            ExecResult exeResult = new ExecResult();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            flowActionEntity.JournalID = JournalID;
            exeResult = service.AddFlowAction(flowActionEntity);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        /// <summary>
        /// 编辑流程操作
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult EditFlowActionAjax(FlowActionEntity flowActionEntity)
        {
            ExecResult exeResult = new ExecResult();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            flowActionEntity.JournalID = JournalID;
            exeResult = service.EditFlowAction(flowActionEntity);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        /// <summary>
        /// 删除流程操作
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult DelFlowActionAjax(FlowActionEntity flowActionEntity)
        {
            ExecResult exeResult = new ExecResult();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            flowActionEntity.JournalID = JournalID;
            exeResult = service.DelFlowAction(flowActionEntity);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        # endregion

        # region 审稿环节权限配置

        /// <summary>
        /// 获取流程环节成员权限列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetFlowAuthAuthorListAjax(FlowAuthAuthorQuery query)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            List<FlowAuthAuthorEntity> listResult = service.GetFlowAuthAuthorList(query);
            return Json(new { Rows = listResult });
        }

        /// <summary>
        /// 设置审稿流程成员权限
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult SetFlowAuthAuthorAjax(List<FlowAuthAuthorEntity> flowAuthAuthorEntity)
        {
            ExecResult exeResult = new ExecResult();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            foreach (FlowAuthAuthorEntity item in flowAuthAuthorEntity)
            {
                item.JournalID = JournalID;
            }
            exeResult = service.SetFlowAuthAuthor(flowAuthAuthorEntity);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        /// <summary>
        /// 删除审稿流程成员权限
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult DelFlowAuthAuthorAjax(List<FlowAuthAuthorEntity> flowAuthAuthorEntity)
        {
            ExecResult exeResult = new ExecResult();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            foreach (FlowAuthAuthorEntity item in flowAuthAuthorEntity)
            {
                item.JournalID = JournalID;
            }
            exeResult = service.DeleteFlowAuthAuthor(flowAuthAuthorEntity);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        /// <summary>
        /// 获取流程环节角色权限列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetFlowAuthRoleListAjax(FlowAuthRoleQuery query)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            List<FlowAuthRoleEntity> listResult = service.GetFlowAuthRoleList(query);
            return Json(new { Rows = listResult });
        }

        /// <summary>
        /// 设置审稿流程角色权限
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult SetFlowAuthRoleAjax(List<FlowAuthRoleEntity> flowAuthRoleEntity)
        {
            ExecResult exeResult = new ExecResult();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            foreach (FlowAuthRoleEntity item in flowAuthRoleEntity)
            {
                item.JournalID = JournalID;
            }
            exeResult = service.SetFlowAuthRole(flowAuthRoleEntity);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        /// <summary>
        /// 删除审稿流程角色权限
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult DelFlowAuthRoleAjax(List<FlowAuthRoleEntity> flowAuthRoleEntity)
        {
            ExecResult exeResult = new ExecResult();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            foreach (FlowAuthRoleEntity item in flowAuthRoleEntity)
            {
                item.JournalID = JournalID;
            }
            exeResult = service.DeleteFlowAuthRole(flowAuthRoleEntity);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        # endregion

        # region 审稿流程

        /// <summary>
        /// 发送系统通知
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult SendSysMessageAjax(AuditBillEntity auditBillEntity)
        {
            ExecResult exeResult = new ExecResult();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            auditBillEntity.JournalID = JournalID;
            auditBillEntity.Processer = CurAuthor.AuthorID;
            exeResult = service.SendSysMessage(auditBillEntity);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        /// <summary>
        /// 提交审稿
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult SubmitAuditBillAjax(AuditBillEntity auditBillEntity)
        {
            ExecResult exeResult = new ExecResult();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            auditBillEntity.JournalID = JournalID;
            auditBillEntity.Processer = CurAuthor.AuthorID;
            //zhanglc
            auditBillEntity.CPath = string.IsNullOrEmpty(auditBillEntity.CPath) ? "" : auditBillEntity.CPath;
            auditBillEntity.CFileName = string.IsNullOrEmpty(auditBillEntity.CFileName) ? "" : auditBillEntity.CFileName;
            auditBillEntity.FigurePath = string.IsNullOrEmpty(auditBillEntity.FigurePath) ? "" : auditBillEntity.FigurePath;
            auditBillEntity.FFileName = string.IsNullOrEmpty(auditBillEntity.FFileName) ? "" : auditBillEntity.FFileName;
            auditBillEntity.OtherPath = string.IsNullOrEmpty(auditBillEntity.OtherPath) ? "" : auditBillEntity.OtherPath;
            exeResult = service.SubmitAuditBill(auditBillEntity);
            return base.Content(JsonConvert.SerializeObject(exeResult));
        }
        # endregion

        # endregion

        #region 审稿单项相关

        public ActionResult ReviewBill()
        {
            return View();
        }

        [AjaxRequest]
        public ActionResult GetReviewBillTree(string defaultText)
        {
            ReviewBillQuery query = new ReviewBillQuery();
            query.JournalID = JournalID;
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            var list = service.GetReviewBillTreeList(query);
            if (!string.IsNullOrWhiteSpace(defaultText))
                list[0].text = Server.UrlDecode(defaultText);
            return Content(JsonConvert.SerializeObject(list));
        }

        [HttpPost]
        public ActionResult GetReviewBillList(ReviewBillQuery query)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<ReviewBillEntity> list = service.GetReviewBillList(query);
            return Json(new { list });
        }

        public ActionResult DetailReviewBill(Int64 ItemID = 0)
        {
            return View(GetReviewBillModel(ItemID));
        }

        private ReviewBillEntity GetReviewBillModel(Int64 ItemID)
        {
            ReviewBillEntity model = null;
            if (ItemID > 0)
            {
                ReviewBillQuery query = new ReviewBillQuery();
                query.JournalID = CurAuthor.JournalID;
                query.ItemID = ItemID;
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                model = service.GetReviewBillModel(query);
            }
            if (model == null)
                model = new ReviewBillEntity();
            return model;
        }

        public ActionResult CreateReviewBill(Int64 ItemID = 0)
        {
            ReviewBillEntity model = GetReviewBillModel(ItemID);
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveReviewBill(ReviewBillEntity model)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            ExecResult result = service.SaveReviewBill(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult DelReviewBill(Int64 ItemID)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            ReviewBillQuery query = new ReviewBillQuery();
            query.JournalID = CurAuthor.JournalID;
            query.ItemID = ItemID;
            ExecResult result = service.DelReviewBill(query);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult ReviewBillIsEnabled(Int64 ItemID)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            ReviewBillQuery query = new ReviewBillQuery();
            query.JournalID = CurAuthor.JournalID;
            query.ItemID = ItemID;
            bool result = service.ReviewBillIsEnabled(query);
            return Json(new { result = result });
        }
        #endregion

        #region 审稿单相关

        public ActionResult ReviewBillContent()
        {
            return View();
        }

        public ActionResult CreateReviewBillContent(Int64 CID, string IsView,Boolean IsEnExpert=false, Int64 authorID=0)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            string html = service.GetReviewBillContentStr(CurAuthor.JournalID, authorID==0?CurAuthor.AuthorID:authorID, CID);
            string head = service.GetReviewBillContentHead(CurAuthor.JournalID, authorID==0?CurAuthor.AuthorID:authorID, CID);
            ViewBag.html = html;
            ViewBag.content = head;
            ViewBag.CID = CID;
            ViewBag.IsEnExpert = IsEnExpert;
            ViewBag.authorID = authorID;
            if (string.IsNullOrEmpty(IsView))
            {
                ViewBag.IsView = "";
            }
            else
            {
                ViewBag.IsView = IsView;
            }

            //2013-9-4  文海峰

            //获取模版

            ISiteConfigFacadeService siteConfigService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigEntity currentEntity = siteConfigService.GetSiteConfigModel(new SiteConfigQuery() { JournalID = CurAuthor.JournalID });

            XmlDocument xmlDoc = new XmlDocument();
            string utl = Utils.GetMapPath(SiteConfig.RootPath + "/data/reviewtemplate.config");
            xmlDoc.Load(Utils.GetMapPath(SiteConfig.RootPath + "/data/reviewtemplate.config"));
            XmlNode signNode = xmlDoc.SelectSingleNode("ContributionSign");
            //获取数据
            IAuthorPlatformFacadeService cService = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            ContributionInfoQuery query = new ContributionInfoQuery();
            query.JournalID = CurAuthor.JournalID;
            query.CIDs = new Int64[] { CID };
            query.IsAuxiliary = true;
            IList<ContributionInfoEntity> list = cService.GetContributionInfoList(query);

            if (list == null || list.Count == 0)
            {
                ViewBag.SignContent = "获取稿件信息失败";
                return View();
            }

            //替换内容
            StringBuilder strHtml = new StringBuilder();
            foreach (var model in list)
            {
                var linkAuthor = model.AuthorList.SingleOrDefault(p => p.IsCommunication == true);//要求[AuthorDetail]表中作者信息不重复
                var firstAuthor = model.AuthorList.SingleOrDefault(p => p.IsFirst == true);
                if (linkAuthor == null)
                {
                    linkAuthor = new ContributionAuthorEntity();
                }
                if (firstAuthor == null)
                {
                    firstAuthor = new ContributionAuthorEntity();
                }
                strHtml.Append(signNode.InnerText
                    .Replace("${审稿单}$", currentEntity != null ? currentEntity.Title + "审稿单" : "审稿单")
                    .Replace("${稿件编号}$", model.CNumber)
                    .Replace("${投稿日期}$", model.AddDate.ToString("yyyy-MM-dd"))
                    .Replace("${期刊栏目}$", model.JChannelName)
                    .Replace("${中文标题}$", model.Title)
                    .Replace("${作者}$", firstAuthor.AuthorName)
                    .Replace("${联系人单位}$", linkAuthor.WorkUnit)
                    );
            }


            //IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            //string html = strHtml.ToString();
            //html += "<br></br>";
            //html += service.GetReviewBillContentStr(CurAuthor.JournalID, CurAuthor.AuthorID, CID);
            ViewBag.contributeInfo = strHtml.ToString();
            ViewBag.header = (currentEntity != null ? currentEntity.Title : "") + "审稿单";


            return View();
        }

        [HttpPost]
        public ActionResult BillContentReport(Int64 CID)
        {
            try
            {
                //获取模版
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Utils.GetMapPath(SiteConfig.RootPath + "/data/signtemplate.config"));
                XmlNode signNode = xmlDoc.SelectSingleNode("ContributionSign");

                //获取数据
                IAuthorPlatformFacadeService cService = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                ContributionInfoQuery query = new ContributionInfoQuery();
                query.JournalID = CurAuthor.JournalID;
                query.CIDs = new Int64[] { CID };
                query.IsAuxiliary = true;
                IList<ContributionInfoEntity> list = cService.GetContributionInfoList(query);

                if (list == null || list.Count == 0)
                {
                    ViewBag.SignContent = "获取稿件信息失败";
                    return View();
                }

                //替换内容
                StringBuilder strHtml = new StringBuilder();
                foreach (var model in list)
                {
                    var linkAuthor = model.AuthorList.SingleOrDefault(p => p.IsCommunication == true);
                    var firstAuthor = model.AuthorList.SingleOrDefault(p => p.IsFirst == true);
                    if (linkAuthor == null)
                    {
                        linkAuthor = new ContributionAuthorEntity();
                    }
                    if (firstAuthor == null)
                    {
                        firstAuthor = new ContributionAuthorEntity();
                    }
                    strHtml.Append(signNode.InnerText
                        .Replace("${稿件编号}$", model.CNumber)
                        .Replace("${投稿日期}$", model.AddDate.ToString("yyyy-MM-dd"))
                        .Replace("${期刊栏目}$", model.JChannelName)
                        .Replace("${中文标题}$", model.Title)
                        .Replace("${英文标题}$", model.EnTitle)
                        .Replace("${关键词}$", model.Keywords)
                        .Replace("${作者}$", firstAuthor.AuthorName)
                        .Replace("${联系人}$", linkAuthor.AuthorName)
                        .Replace("${联系人电子邮件}$", linkAuthor.Email)
                        .Replace("${联系人电话}$", linkAuthor.Tel)
                        .Replace("${联系人地址}$", linkAuthor.Address)
                        .Replace("${联系人邮编}$", linkAuthor.ZipCode)
                        .Replace("${联系人单位}$", linkAuthor.WorkUnit)
                        );
                }


                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                string html = service.GetReviewBillContentStr(CurAuthor.JournalID, CurAuthor.AuthorID, CID);
                html += strHtml;
                // RenderToWord.HtmlToWord(Server.UrlDecode(head + html), "空白审稿单.doc", true);
                return Content(html);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("打印空白审稿单出现异常：" + ex.Message);
                return Content("打印空白审稿单异常！");
            }
        }

        [HttpPost]
        public ActionResult SaveReviewBillContent(ReviewBillContentQuery query)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.AddUser = query.AddUser== 0 ? CurAuthor.AuthorID : query.AddUser;

            if (query.IsEnExpert == true)
            {
                //获取专家的审稿流程日志
                CirculationEntity ce = new CirculationEntity
                {
                    CID = (long)query.CID,
                    JournalID = CurAuthor.JournalID,
                    GroupID = 4
                };
                IList<FlowLogInfoEntity> flowLogList = service.GetFlowLog(ce);
                if (flowLogList.Count > 1)
                {
                    for (int i = 0; i < flowLogList.Count; i++)
                    {
                        if (CurAuthor.AuthorID == flowLogList[i].SendUserID && flowLogList[i].SendRoleID == 4 && flowLogList[i].Status == 1 && flowLogList[i].ActionType != 4)
                        {
                            query.IsReReview = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                //获取专家的审稿流程日志
                CirculationEntity ce = new CirculationEntity
                {
                    CID = (long)query.CID,
                    JournalID = CurAuthor.JournalID,
                    GroupID = 3
                };
                IList<FlowLogInfoEntity> flowLogList = service.GetFlowLog(ce);
                if (flowLogList.Count > 1)
                {
                    for (int i = 0; i < flowLogList.Count; i++)
                    {
                        if (CurAuthor.AuthorID == flowLogList[i].SendUserID && flowLogList[i].SendRoleID == 3 && flowLogList[i].Status == 1 && flowLogList[i].ActionType != 4)
                        {
                            query.IsReReview = true;
                            break;
                        }
                    }
                }
            }
            
            
            ExecResult result = service.SaveReviewBillContent(query);
            return Json(new { result = result.result, msg = result.msg });

        }

        [HttpPost]
        public ActionResult ReviewBillContentToWord(Int64 CID)
        {
            try
            {
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                string html = service.GetReviewBillContentHead(CurAuthor.JournalID, CurAuthor.AuthorID, CID);
                html += service.GetReviewBillContentStr(CurAuthor.JournalID, CurAuthor.AuthorID, CID);
                RenderToWord.HtmlToWord(Server.UrlDecode(html), "审稿单.doc", true);
                return Content("打印成功！");
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("打印审稿单出现异常：" + ex.Message);
                return Content("打印审稿单异常！");
            }
        }

        public ActionResult ViewReviewBillContent(Int64 CID, Int64 FlowLogID, Int64 EditorID=0,Int64 ExpertID = 0)
        {
            if (ExpertID == 0)
                ExpertID = CurAuthor.AuthorID;
            ViewBag.html = GetReviewBillContent(CID, EditorID,ExpertID, FlowLogID);
            ViewBag.CID = CID;
            ViewBag.EditorID = EditorID;
            ViewBag.ExpertID = ExpertID;
            ViewBag.FlowLogID = FlowLogID;
            return View();
        }

        //导出审稿单
        public ActionResult ReviewBillContentToReport(Int64 CID, Int64 FlowLogID, Int64 EditorID,Int64 ExpertID)
        {
            try
            {
                if (ExpertID == 0)
                    ExpertID = CurAuthor.AuthorID;
                string html = GetReviewBillContent(CID, EditorID,ExpertID, FlowLogID);
                //获取稿件信息
                ContributionInfoQuery query = new ContributionInfoQuery();
                query.JournalID = JournalID;
                query.IsAuxiliary = false;
                query.CID = CID;
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                var CModel = service.GetContributionInfoModel(query);

                RenderToWord.HtmlToWord(Server.UrlDecode(html), CModel.Title + "_审稿单.doc", true);
                return Content("导出成功！");
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出审稿单出现异常：" + ex.Message);
                return Content("导出审稿单异常！");
            }
        }

        /// <summary>
        /// 获取审稿单内容
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="ExpertID"></param>
        /// <returns></returns>
        private string GetReviewBillContent(Int64 CID, Int64 EditorID, Int64 ExpertID, Int64 FlowLogID)
        {
            StringBuilder strHtml = new StringBuilder();
            strHtml.AppendFormat(" <div style=\"text-align: center; font-family: 黑体; font-size: 16px; padding-bottom: 15px;\">《{0}》 审稿单</div>",
                SiteConfig.SiteName);

            #region 获取稿件信息
            ContributionInfoQuery query = new ContributionInfoQuery();
            query.JournalID = CurAuthor.JournalID;
            query.IsAuxiliary = false;
            query.CID = CID;
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            var CModel = service.GetContributionInfoModel(query);
            if (CModel == null)
                CModel = new ContributionInfoEntity();
            #endregion

            #region 获取作者信息
            AuthorDetailQuery query1 = new AuthorDetailQuery();
            query1.JournalID = CurAuthor.JournalID;
            query1.AuthorIDs = new Int64[] { CModel.AuthorID, ExpertID };
            var listAuthor = service.GetAuthorDetailList(query1).ToList();
            var AModel = listAuthor.Find(p => p.AuthorID == CModel.AuthorID);
            var EModel = listAuthor.Find(p => p.AuthorID == ExpertID);
            if (AModel == null)
                AModel = new AuthorDetailEntity();
            if (EModel == null)
                EModel = new AuthorDetailEntity();

            ISiteConfigFacadeService service1 = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            DictValueQuery query2 = new DictValueQuery();
            query2.JournalID = SiteConfig.SiteID;
            query2.DictKey = EnumDictKey.JobTitle.ToString();
            IDictionary<int, string> dict = service1.GetDictValueDcit(query2);
            string jobTitle = dict.ContainsKey(EModel.JobTitle) ? dict[EModel.JobTitle] : string.Empty;
            #endregion

            #region 获取审稿日志
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity cirEntity = new CirculationEntity();
            cirEntity.CID = CID;
            cirEntity.JournalID = JournalID;
            cirEntity.GroupID = 1;
            IList<FlowLogInfoEntity> flowLogList = flowService.GetFlowLog(cirEntity);

            FlowLogInfoEntity LogSendModel = flowLogList.ToList().Find(p => p.FlowLogID == FlowLogID);

            //var LogModel = flowLogList.ToList().Where(p => p.SendUserID == ExpertID).FirstOrDefault(q => q.TargetStatusID == LogSendModel.TargetStatusID);
            var LogModel = flowLogList.ToList().Find(p => p.SendUserID == EditorID);
            if (LogModel == null)
                LogModel = new FlowLogInfoEntity();
            if (LogSendModel == null)
                LogSendModel = new FlowLogInfoEntity();
            #endregion

            strHtml.Append(flowService.GetReviewBillContentHead(CurAuthor.JournalID, ExpertID, CID));
            strHtml.AppendFormat("<div style=\"padding: 0px 0px 5px 0px; font-size: 11pt; font-family: Times New Roman;\">稿件编号：{0}</div>"
                , CModel.CNumber);
            strHtml.Append("<table border=\"0\" class=\"mainTable\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\">");
            strHtml.AppendFormat("<tr><td class=\"left\">稿件标题：</td><td class=\"right\" style=\"font-weight: bolder;\" colspan=\"3\">{0}</td></tr>"
                , CModel.Title);

            strHtml.AppendFormat("<tr><td class=\"left\">送审日期：</td><td class=\"right\">{0}</td><td class=\"left_m\">审毕日期：</td><td class=\"right\">{1}</td></tr>"
                , LogModel.AddDate, LogModel.DealDate);
            strHtml.AppendFormat("<tr><td class=\"left\">作者姓名：</td><td class=\"right\">{0}</td><td class=\"left_m\">单位：</td><td class=\"right\">{1}</td></tr>"
                , AModel.AuthorName, AModel.WorkUnit);
            strHtml.AppendFormat("<tr><td class=\"left\" style=\"width: 80px;\">对本稿的总评价：</td><td class=\"right\" style=\"padding: 5px 0px 10px 5px;\" colspan=\"3\">");
            IFlowFacadeService service2 = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            strHtml.Append(service2.GetReviewBillContentStr(CurAuthor.JournalID, ExpertID, CID));
            strHtml.Append("</td></tr>");
            strHtml.Append("<tr><td class=\"left\" style=\"width: 80px;\">审稿意见：</td><td class=\"right\" style=\"padding: 5px 0px 10px 5px;\" colspan=\"3\">");
            strHtml.Append(LogSendModel.DealAdvice);
            strHtml.Append("</td></tr>");

            strHtml.AppendFormat("<tr><td class=\"left\">审稿人姓名：</td><td class=\"right\">{0}</td><td class=\"left_m\">身份证号：</td><td class=\"right\">{1}</td></tr>"
              , EModel.AuthorName, EModel.IDCard);
            strHtml.AppendFormat("<tr><td class=\"left\">联系电话：</td><td class=\"right\">{0}</td><td class=\"left_m\">Email：</td><td class=\"right\">{1}</td></tr>"
                , EModel.Mobile, EModel.AuthorModel.LoginName);
            strHtml.AppendFormat("<tr><td class=\"left\">通讯地址：</td><td class=\"right\">{0}</td><td class=\"left_m\">邮政编码：</td><td class=\"right\">{1}</td></tr>"
               , EModel.Address, EModel.ZipCode);
            strHtml.AppendFormat("<tr><td class=\"left\">研究方向：</td><td class=\"right\">{0}</td><td class=\"left_m\">职称：</td><td class=\"right\">{1}</td></tr>"
    , EModel.ResearchTopics, jobTitle);
            strHtml.Append("</table>");
            return strHtml.ToString();
        }
        #endregion

        # region 继续送交_已注释

        /// <summary>
        /// 根据状态响应角色跳转到选择成员、作者或专家页
        /// </summary>
        /// <param name="StatusID"></param>
        /// <returns></returns>
        //[HttpGet]
        //public ActionResult JumpSelAuthor(long StatusID)
        //{
        //    FlowStep stepEntity = new FlowStep();
        //    try
        //    {
        //        FlowStatusQuery query = new FlowStatusQuery();
        //        query.JournalID = JournalID;
        //        query.StatusID = StatusID;
        //        IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
        //        stepEntity = flowService.GetFlowStepInfo(query);
        //        if (stepEntity != null)
        //        {
        //            if (stepEntity.FlowStatus.ActionRoleID == 3)
        //            {
        //                Response.Redirect(SiteConfig.RootPath + "/expert/seldialog", true);// 选择专家
        //            }
        //            else if (stepEntity.FlowStatus.ActionRoleID == 2)
        //            {
        //                Response.Redirect(SiteConfig.RootPath + "/author/seldialog", true);// 选择作者
        //            }
        //            else
        //            {
        //                Response.Redirect(SiteConfig.RootPath + "/member/seldialog", true);// 选择成员
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogProvider.Instance.Error("获取指定的审稿状态及配置信息时出现异常：" + ex.Message);
        //    }

        //    return Content("");
        //}

        /// <summary>
        /// 继续送交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[AjaxRequest]
        //public ActionResult ContinuSend(ContinuSendEntity model)
        //{
        //    ExecResult execResult = new ExecResult();

        //    try
        //    {
        //        model.JournalID = JournalID;
        //        model.SendUserID = CurAuthor.AuthorID;
        //        IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
        //        execResult = service.ContinuSend(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        execResult.result = EnumJsonResult.error.ToString();
        //        execResult.msg = "继续送交出现异常：" + ex.Message;
        //        WKT.Log.LogProvider.Instance.Error("继续送交出现异常：" + ex.Message);
        //    }

        //    return Content(JsonConvert.SerializeObject(execResult));
        //}

        # endregion

        


    }
}
