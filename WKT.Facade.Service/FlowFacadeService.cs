using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Log;
using WKT.Config;
using WKT.Common.Utils;
using WKT.Facade.Service.Interface;

namespace WKT.Facade.Service
{
    /// <summary>
    /// 审稿流程服务
    /// </summary>
    public class FlowFacadeService : ServiceBase, IFlowFacadeService
    {
        # region 审稿状态

        /// <summary>
        /// 获取拥有权限的审稿状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<FlowStatusEntity> GetHaveRightFlowStatus(FlowStatusQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<FlowStatusEntity> dictStatusName = clientHelper.PostAuth<IList<FlowStatusEntity>, FlowStatusQuery>(GetAPIUrl(APIConstant.FLOW_GETHAVERIGHTFLOWSTATUS), query);
            return dictStatusName;
        }

        /// <summary>
        /// 获取拥有权限的审稿状态(用于统计同一稿件一个状态下送多人时按一个计算)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<FlowStatusEntity> GetHaveRightFlowStatusForStat(FlowStatusQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<FlowStatusEntity> dictStatusName = clientHelper.PostAuth<IList<FlowStatusEntity>, FlowStatusQuery>(GetAPIUrl(APIConstant.FLOW_GETHAVERIGHTFLOWSTATUSFORSTAT), query);
            return dictStatusName;
        }

        /// <summary>
        /// 获取审稿状态键值对，审稿状态名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long, string> GetFlowStatusDictStatusName(FlowStatusQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IDictionary<long, string> dictStatusName = clientHelper.PostAuth<IDictionary<long, string>, FlowStatusQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWSTATUSDICTSTATUSNAME), query);
            return dictStatusName;
        }

        /// <summary>
        /// 获取审稿状态键值对，审稿状态显示名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long, string> GetFlowStatusDictDisplayName(FlowStatusQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IDictionary<long, string> dictStatusDisplay = clientHelper.PostAuth<IDictionary<long, string>, FlowStatusQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWSTATUSDICTDISPLAYNAME), query);
            return dictStatusDisplay;
        }

        /// <summary>
        /// 获取审稿环节配置
        /// </summary>
        /// <param name="queryFlowSet"></param>
        /// <returns></returns>
        public List<FlowStatusEntity> GetFlowStatusList(FlowStatusQuery queryFlowStatus)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            List<FlowStatusEntity> flowStatusList = clientHelper.PostAuth<List<FlowStatusEntity>, FlowStatusQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWSTATUSLIST), queryFlowStatus);
            return flowStatusList;
        }

        /// <summary>
        /// 获取审稿流程状态序号
        /// </summary>
        /// <param name="queryFlowStatus"></param>
        /// <returns></returns>
        public int GetFlowStatusSortID(FlowStatusQuery queryFlowStatus)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            int SortID = clientHelper.PostAuth<int, FlowStatusQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWSTATUSSORTID), queryFlowStatus);
            return SortID;
        }

        /// <summary>
        /// 判断审稿状态对应的稿件状态是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public FlowStatusEntity CheckCStatusIsExists(FlowStatusQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            FlowStatusEntity statusEntity = clientHelper.PostAuth<FlowStatusEntity, FlowStatusQuery>(GetAPIUrl(APIConstant.FLOW_CHECKCSTATUSISEXISTS), query);
            return statusEntity;
        }

        /// <summary>
        /// 根据指定的审稿状态ID，得到审稿状态的基本信息
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="StatusID"></param>
        /// <returns></returns>
        public FlowStatusEntity GetFlowStatusInfoByID(FlowStatusQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            FlowStatusEntity statusEntity = clientHelper.PostAuth<FlowStatusEntity, FlowStatusQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWSTATUSINFOBYID), query);
            return statusEntity;
        }

        /// <summary>
        /// 获取审稿步骤基本信息及配置信息
        /// </summary>
        /// <param name="queryFlowSet"></param>
        /// <returns></returns>
        public FlowStep GetFlowStepInfo(FlowStatusQuery queryFlowStatus)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            FlowStep stepEntity = clientHelper.PostAuth<FlowStep, FlowStatusQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWSTEPINFO), queryFlowStatus);
            return stepEntity;
        }

        /// <summary>
        /// 新增审稿环节
        /// </summary>
        /// <param name="flowStepEntity"></param>
        /// <returns></returns>
        public ExecResult AddFlowStatus(FlowStep flowStepEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, FlowStep>(GetAPIUrl(APIConstant.FLOW_ADDFLOWSTATUS), flowStepEntity);
            return execResult;
        }

        /// <summary>
        /// 编辑审稿环节
        /// </summary>
        /// <param name="flowStepEntity"></param>
        /// <returns></returns>
        public ExecResult EditFlowStatus(FlowStep flowStepEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, FlowStep>(GetAPIUrl(APIConstant.FLOW_EDITFLOWSTATUS), flowStepEntity);
            return execResult;
        }

        /// <summary>
        /// 删除审稿环节
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        public ExecResult DelFlowStatus(FlowStatusEntity flowStatusEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, FlowStatusEntity>(GetAPIUrl(APIConstant.FLOW_DELFLOWSTATUS), flowStatusEntity);
            return execResult;
        }

        /// <summary>
        /// 修改审稿状态状态
        /// </summary>
        /// <param name="flowStatusEntity"></param>
        /// <returns></returns>
        public ExecResult UpdateFlowStatus(FlowStatusEntity flowStatusEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, FlowStatusEntity>(GetAPIUrl(APIConstant.FLOW_UPDATEFLOWSTATUS), flowStatusEntity);
            return execResult;
        }

        # endregion

        # region 审稿操作

        /// <summary>
        /// 获取操作实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public FlowActionEntity GetFlowActionEntity(FlowActionQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            FlowActionEntity flowActionEntity = clientHelper.PostAuth<FlowActionEntity, FlowActionQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWACTIONENTITY), query);
            return flowActionEntity;
        }

        /// <summary>
        /// 根据当前操作状态获取可以做的操作
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<FlowActionEntity> GetFlowActionByStatus(FlowActionQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            List<FlowActionEntity> flowActionList = clientHelper.PostAuth<List<FlowActionEntity>, FlowActionQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWACTIONBYSTATUS), query);
            return flowActionList;
        }

        /// <summary>
        /// 获取指定审稿状态下的审稿操作列表
        /// </summary>
        /// <param name="queryFlowSet"></param>
        /// <returns></returns>
        public List<FlowActionEntity> GetFlowActionList(FlowActionQuery queryFlowAction)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            List<FlowActionEntity> flowActionList = clientHelper.PostAuth<List<FlowActionEntity>, FlowActionQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWACTIONLIST), queryFlowAction);
            FlowStatusQuery statusQuery = new FlowStatusQuery();
            statusQuery.JournalID = queryFlowAction.JournalID;
            IDictionary<long, string> dictGetFlowStatus = GetFlowStatusDictStatusName(statusQuery);
            string value = "";
            foreach (FlowActionEntity item in flowActionList)
            {
                dictGetFlowStatus.TryGetValue(item.TOStatusID, out value);
                item.StatusName = value;
                value = "";
            }
            return flowActionList;
        }

        /// <summary>
        /// 新增审稿操作
        /// </summary>
        /// <param name="flowActionEntity"></param>
        /// <returns></returns>
        public ExecResult AddFlowAction(FlowActionEntity flowActionEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, FlowActionEntity>(GetAPIUrl(APIConstant.FLOW_ADDFLOWACTION), flowActionEntity);
            return execResult;
        }

        /// <summary>
        /// 修改审稿操作
        /// </summary>
        /// <param name="flowActionEntity"></param>
        /// <returns></returns>
        public ExecResult EditFlowAction(FlowActionEntity flowActionEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, FlowActionEntity>(GetAPIUrl(APIConstant.FLOW_EDITFLOWACTION), flowActionEntity);
            return execResult;
        }

        /// <summary>
        /// 删除审稿操作
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        public ExecResult DelFlowAction(FlowActionEntity flowActionEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, FlowActionEntity>(GetAPIUrl(APIConstant.FLOW_DELFLOWACTION), flowActionEntity);
            return execResult;
        }

        # endregion

        # region 审稿环节权限配置

        /// <summary>
        /// 获取审稿环节作者权限列表
        /// </summary>
        /// <param name="queryFlowAuthAuthor"></param>
        /// <returns></returns>
        public List<FlowAuthAuthorEntity> GetFlowAuthAuthorList(FlowAuthAuthorQuery queryFlowAuthAuthor)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            List<FlowAuthAuthorEntity> flowAuthAuthorList = clientHelper.PostAuth<List<FlowAuthAuthorEntity>, FlowAuthAuthorQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWAUTHAUTHORLIST), queryFlowAuthAuthor);
            if (flowAuthAuthorList != null)
            {
                foreach (FlowAuthAuthorEntity item in flowAuthAuthorList)
                {
                    item.AuthorName = GetMemberName(item.AuthorID);
                }
            }
            return flowAuthAuthorList;
        }

        /// <summary>
        /// 设置审稿环节作者权限
        /// </summary>
        /// <param name="flowAuthAuthorEntity"></param>
        /// <returns></returns>
        public ExecResult SetFlowAuthAuthor(IList<FlowAuthAuthorEntity> flowAuthAuthorEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, IList<FlowAuthAuthorEntity>>(GetAPIUrl(APIConstant.FLOW_SETFLOWAUTHAUTHOR), flowAuthAuthorEntity);
            return execResult;
        }

        /// <summary>
        /// 删除审稿环节作者权限
        /// </summary>
        /// <param name="flowAuthAuthorEntity"></param>
        /// <returns></returns>
        public ExecResult DeleteFlowAuthAuthor(IList<FlowAuthAuthorEntity> flowAuthAuthorEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, IList<FlowAuthAuthorEntity>>(GetAPIUrl(APIConstant.FLOW_DELETEFLOWAUTHAUTHOR), flowAuthAuthorEntity);
            return execResult;
        }

        /// <summary>
        /// 获取审稿环节角色权限列表
        /// </summary>
        /// <param name="queryFlowAuthRole"></param>
        /// <returns></returns>
        public List<FlowAuthRoleEntity> GetFlowAuthRoleList(FlowAuthRoleQuery queryFlowAuthRole)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            List<FlowAuthRoleEntity> flowAuthRoleList = clientHelper.PostAuth<List<FlowAuthRoleEntity>, FlowAuthRoleQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWAUTHROLELIST), queryFlowAuthRole);
            if (flowAuthRoleList != null)
            {
                foreach (FlowAuthRoleEntity item in flowAuthRoleList)
                {
                    item.RoleName = GetRoleName(item.RoleID);
                }
            }
            return flowAuthRoleList;
        }

        /// <summary>
        /// 设置审稿环节角色权限
        /// </summary>
        /// <param name="flowAuthRoleEntity"></param>
        /// <returns></returns>
        public ExecResult SetFlowAuthRole(IList<FlowAuthRoleEntity> flowAuthRoleEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, IList<FlowAuthRoleEntity>>(GetAPIUrl(APIConstant.FLOW_SETFLOWAUTHROLE), flowAuthRoleEntity);
            return execResult;
        }

        /// <summary>
        /// 删除审稿环节角色权限
        /// </summary>
        /// <param name="flowAuthRoleEntity"></param>
        /// <returns></returns>
        public ExecResult DeleteFlowAuthRole(IList<FlowAuthRoleEntity> flowAuthRoleEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, IList<FlowAuthRoleEntity>>(GetAPIUrl(APIConstant.FLOW_DELETEFLOWAUTHROLE), flowAuthRoleEntity);
            return execResult;
        }

        # endregion

        # region 审稿流转

        /// <summary>
        /// 得到稿件的处理人
        /// </summary>
        /// <param name="cirEntity">稿件ID(CID)、稿件状态(EnumCStatus)、编辑部ID(JournalID)</param>
        /// <returns></returns>
        public AuthorInfoEntity GetContributionProcesser(CirculationEntity cirEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            AuthorInfoEntity authorEntity = clientHelper.PostAuth<AuthorInfoEntity, CirculationEntity>(GetAPIUrl(APIConstant.FLOW_GETCONTRIBUTIONPROCESSER), cirEntity);
            return authorEntity;
        }

        /// <summary>
        /// 获取处理的稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public Pager<FlowContribution> GetFlowContributionList(CirculationEntity cirEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<FlowContribution> contributionList = clientHelper.PostAuth<Pager<FlowContribution>, CirculationEntity>(GetAPIUrl(APIConstant.FLOW_GETFLOWCONTRIBUTIONLIST), cirEntity);

            JournalChannelQuery query = new JournalChannelQuery();
            query.JournalID = cirEntity.JournalID;
            IssueFacadeAPIService service = new IssueFacadeAPIService();
            IList<JournalChannelEntity> list = service.GetJournalChannelList(query);
            if (list == null)
            {
                list = new List<JournalChannelEntity>();
            }

            if (contributionList != null)
            {
                foreach (FlowContribution item in contributionList.ItemList)
                {
                    item.AuthorName = GetMemberName(item.AuthorID);
                    if (item.RecUserID > 0)
                    {
                        item.RecUserName = GetMemberName(item.RecUserID);
                    }
                    if (item.SendUserID > 0)
                    {
                        item.SendUserName = GetMemberName(item.SendUserID);
                    }
                    if (item.JChannelID > 0)
                    {
                        JournalChannelEntity itemChannel = list.SingleOrDefault(p => p.JChannelID == item.JChannelID);
                        if (itemChannel != null)
                        {
                            item.JChannelName = itemChannel.ChannelName;
                        }
                    }
                }
            }
            return contributionList;
        }

        /// <summary>
        /// 获取作者最新稿件状态稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public Pager<FlowContribution> GetAuthorContributionList(CirculationEntity cirEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<FlowContribution> contributionList = clientHelper.PostAuth<Pager<FlowContribution>, CirculationEntity>(GetAPIUrl(APIConstant.FLOW_GETAUTHORCONTRIBUTIONLIST), cirEntity);
            if (contributionList != null)
            {
                foreach (FlowContribution item in contributionList.ItemList)
                {
                    item.AuthorName = GetMemberName(item.AuthorID);
                }
            }
            return contributionList;
        }

        /// <summary>
        /// 获取专家待审、已审稿件列表
        /// </summary>
        /// <param name="cirEntity">需要传值:JournalID，CurAuthorID，Status</param>
        /// <returns></returns>
        public Pager<FlowContribution> GetExpertContributionList(CirculationEntity cirEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<FlowContribution> contributionList = clientHelper.PostAuth<Pager<FlowContribution>, CirculationEntity>(GetAPIUrl(APIConstant.FLOW_GETEXPERTCONTRIBUTIONLIST), cirEntity);
            if (contributionList != null)
            {
                foreach (FlowContribution item in contributionList.ItemList)
                {
                    item.AuthorName = GetMemberName(item.AuthorID);
                    if (item.RecUserID > 0)
                    {
                        item.RecUserName = GetMemberName(item.RecUserID);
                    }
                }
            }
            return contributionList;
        }


        /// <summary>
        /// 获取同步状态稿件列表，例如：专家拒审，作者退修、已发校样、录用、退稿稿件列表
        /// </summary>
        /// <param name="cirEntity">需要传值:JournalID，CurAuthorID，EnumCStatus</param>
        /// <returns></returns>
        public Pager<FlowContribution> GetSynchroStatusContributionList(CirculationEntity cirEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<FlowContribution> contributionList = clientHelper.PostAuth<Pager<FlowContribution>, CirculationEntity>(GetAPIUrl(APIConstant.FLOW_GETSYNCHROSTATUSCONTRIBUTIONLIST), cirEntity);
            if (contributionList != null)
            {
                foreach (FlowContribution item in contributionList.ItemList)
                {
                    item.AuthorName = GetMemberName(item.AuthorID);
                    if (item.RecUserID > 0)
                    {
                        item.RecUserName = GetMemberName(item.RecUserID);
                    }
                }
            }
            return contributionList;
        }

        /// <summary>
        /// 得到下一步骤信息
        /// </summary>
        /// <param name="ciEntity">流转信息</param>
        /// <returns></returns>
        public FlowStep GetNextFlowStep(CirculationEntity cirEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            FlowStep stepEntity = clientHelper.PostAuth<FlowStep, CirculationEntity>(GetAPIUrl(APIConstant.FLOW_GETNEXTFLOWSTEP), cirEntity);
            return stepEntity;
        }

        /// <summary>
        /// 发送系统通知
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        public ExecResult SendSysMessage(AuditBillEntity auditBillEntity, bool isRecode = true)
        {
            ExecResult stepEntity = null;
            SiteConfigFacadeAPIService service = new SiteConfigFacadeAPIService();
            List<long> receiverList  = auditBillEntity.ReveiverList.Split(',').Select(p => TypeParse.ToLong(p, 0)).ToList<long>();
            if (receiverList != null && receiverList.Count > 0)
            {
                foreach (long ReceiverID in receiverList)
                {
                    MessageRecodeEntity messageEntity = new MessageRecodeEntity();
                    messageEntity.MsgType = 1;
                    messageEntity.JournalID = auditBillEntity.JournalID;
                    messageEntity.CID = auditBillEntity.CID;
                    messageEntity.MsgTitle = auditBillEntity.EmailTitle;
                    messageEntity.MsgContent = ReplaceMailContent(auditBillEntity.CID, auditBillEntity.JournalID, auditBillEntity.EmailBody, ReceiverID, auditBillEntity.AuditedDate.ToString(), auditBillEntity.Processer);
                    messageEntity.SendUser = auditBillEntity.Processer;
                    messageEntity.ReciveUser = ReceiverID;
                    messageEntity.TemplateID = auditBillEntity.TemplateID;
                    List<long> listRecevie = new List<long>();
                    listRecevie.Add(ReceiverID);

                    if (auditBillEntity.IsEmail)
                    {
                        messageEntity.MsgType = 1;
                        stepEntity = service.SendEmailOrSms(receiverList.Where(o=>o==ReceiverID).ToList<long>(), messageEntity, isRecode);
                    }
                    if(auditBillEntity.IsSMS)
                    {
                        messageEntity.MsgType = 2;
                        messageEntity.MsgContent = ReplaceMailContent(auditBillEntity.CID, auditBillEntity.JournalID, auditBillEntity.SMSBody, ReceiverID, auditBillEntity.AuditedDate.ToString(), auditBillEntity.Processer);
                        stepEntity = service.SendEmailOrSms(receiverList.Where(o => o == ReceiverID).ToList<long>(), messageEntity, isRecode);         
                    }              
                }
            }
            return stepEntity;
        }

        private string ReplaceMailContent(long CID, long JournalID, string Content, long RecevierID)
        {
            IList<long> RecevierList = new List<long>();
            RecevierList.Add(RecevierID);
            return ReplaceMailContent(CID, JournalID, Content, RecevierList, "", 0);
        }

        private string ReplaceMailContent(long CID, long JournalID, string Content, long RecevierID, string AudingtedDate)
        {
            IList<long> RecevierList = new List<long>();
            RecevierList.Add(RecevierID);
            return ReplaceMailContent(CID, JournalID, Content, RecevierList, AudingtedDate, 0);
        }

        private string ReplaceMailContent(long CID, long JournalID, string Content, long RecevierID, string AudingtedDate, long SendUser)
        {
            IList<long> RecevierList = new List<long>();
            RecevierList.Add(RecevierID);
            return ReplaceMailContent(CID, JournalID, Content, RecevierList, AudingtedDate, SendUser);
        }

        private string ReplaceMailContent(long CID, long JournalID, string Content, IList<long> RecevierIDList, string AuditedDate, long SendUser)
        {
            SiteConfigFacadeAPIService service = new SiteConfigFacadeAPIService();
            IDictionary<string, string> dict = service.GetEmailVariable();
            if (dict != null && dict.Count > 0)
            {
                dict.Remove("${审稿链接}$");
            }
            if (!string.IsNullOrEmpty(Content))
            {
                # region 获取稿件信息

                AuthorPlatformFacadeAPIService authorService = new AuthorPlatformFacadeAPIService();
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
                if (Content.Contains("${接收人}$") || Content.Contains("${邮箱}$") || Content.Contains("${手机}$"))
                {
                    if (RecevierIDList[0] == contribution.AuthorID)
                    {
                        var user = new AuthorFacadeAPIService().GetAuthorInfo(new AuthorInfoQuery() { JournalID = JournalID, AuthorID = RecevierIDList[0] });
                        dict["${接收人}$"] = user.RealName;
                        dict["${邮箱}$"] = user.LoginName;
                        dict["${手机}$"] = user.Mobile;
                    }
                    else
                    {
                        var user = new AuthorFacadeAPIService().GetAuthorInfo(new AuthorInfoQuery() { JournalID = JournalID, AuthorID = RecevierIDList[0] });
                        if (user == null)
                        {
                            var user2 = new ContributionFacadeAPIService().GetContributionAuthorInfo(new ContributionAuthorQuery() { JournalID = JournalID, CAuthorID = RecevierIDList[0] });
                            dict["${接收人}$"] = user2.AuthorName;
                            dict["${邮箱}$"] = user2.Email;
                            dict["${手机}$"] = user2.Mobile;
                        }
                        else
                        {
                            dict["${接收人}$"] = user.RealName;
                            dict["${邮箱}$"] = user.LoginName;
                            dict["${手机}$"] = user.Mobile;
                        }
                        
                    }
                    
                }
                if (Content.Contains("A1${审稿链接}$A2"))
                {
                    string AuditLink = "";
                    foreach (long RecevierID in RecevierIDList)
                    {
                        AuditLink += AuditContributionUrl(RecevierID) + ";";
                    }
                    AuditLink = AuditLink.Remove(AuditLink.Length - 1);
                    // dict["${审稿链接}$"] = "<a" + AuditLink + ">" + AuditLink + "</a>";
                    dict["A1${审稿链接}$A2"] = "<a href='" + AuditLink + "' target='_blank'>" + AuditLink + "</a>";
                }

                if (Content.Contains("${作者链接}$"))
                {
                    string AuditLink = string.Empty;
                    foreach (long RecevierID in RecevierIDList)
                    {
                        AuditLink += AuditAuthorContributionUrl(RecevierID) + ";";
                    }
                    AuditLink = AuditLink.Remove(AuditLink.Length - 1);
                    dict["${作者链接}$"] = "<a href='" + AuditLink + "' target='_blank'>" + AuditLink + "</a>";
                }

                if (!string.IsNullOrEmpty(AuditedDate))
                {
                    dict["${审毕日期}$"] = AuditedDate;
                }
                if (Content.Contains("${发送人}$"))
                {
                    dict["${发送人}$"] = "";
                    var user = new AuthorFacadeAPIService().GetAuthorInfo(new AuthorInfoQuery() { JournalID = JournalID, AuthorID = SendUser });
                    if (user != null)
                    {
                        dict["${发送人}$"] = user.RealName;
                    }
                }
            }
            string strContent = service.GetEmailOrSmsContent(dict, Content);
            return strContent;
        }

        /// <summary>
        /// 提交审稿单
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        public ExecResult SubmitAuditBill(AuditBillEntity auditBillEntity)
        {
            FlowLogInfoEntity flowlogEntity = null;
            HttpClientHelper clientHelper = new HttpClientHelper();
            if (!string.IsNullOrEmpty(auditBillEntity.DealAdvice))
            {
                // 如果是原路返回或者原路撤回，则发送消息的接收人为稿件当前所处环节的接收人
                if (auditBillEntity.ActionType == 2 || auditBillEntity.ActionType == 4)
                {
                    # region 根据日志ID获取日志信息

                    FlowLogInfoQuery logQery = new FlowLogInfoQuery();
                    logQery.JournalID = auditBillEntity.JournalID;
                    logQery.FlowLogID = auditBillEntity.FlowLogID;
                    flowlogEntity = GetFlowLogEntity(logQery);

                    # endregion

                    foreach (long ReceverID in auditBillEntity.ReveiverList.Split(',').Select(p => TypeParse.ToLong(p, 0)).ToList<long>())
                    {
                        if (!auditBillEntity.DictDealAdvice.ContainsKey(ReceverID))
                        {
                            auditBillEntity.DictDealAdvice.Add(ReceverID, ReplaceMailContent(auditBillEntity.CID, auditBillEntity.JournalID, auditBillEntity.DealAdvice, flowlogEntity.RecUserID, auditBillEntity.AuditedDate != null ? auditBillEntity.AuditedDate.Value.ToString("yyyy-MM-dd") : "", auditBillEntity.Processer));
                        }
                    }
                }
                else
                {
                    foreach (long ReceverID in auditBillEntity.ReveiverList.Split(',').Select(p => TypeParse.ToLong(p, 0)).ToList<long>())
                    {
                        if (!auditBillEntity.DictDealAdvice.ContainsKey(ReceverID))
                        {
                            auditBillEntity.DictDealAdvice.Add(ReceverID, ReplaceMailContent(auditBillEntity.CID, auditBillEntity.JournalID, auditBillEntity.DealAdvice, ReceverID, auditBillEntity.AuditedDate != null ? auditBillEntity.AuditedDate.Value.ToString("yyyy-MM-dd") : "", auditBillEntity.Processer));
                        }
                    }
                }
            }
            ExecResult stepEntity = clientHelper.PostAuth<ExecResult, AuditBillEntity>(GetAPIUrl(APIConstant.FLOW_SUBMITAUDITBILL), auditBillEntity);
            if (string.Compare(stepEntity.result, EnumJsonResult.success.ToString(), true) == 0)
            { 
                if (flowlogEntity != null && (auditBillEntity.ActionType == 2 || auditBillEntity.ActionType == 4))
                {
                    auditBillEntity.ReveiverList = flowlogEntity.RecUserID.ToString();
                }
                
                SendSysMessage(auditBillEntity, false);       
            }
            return stepEntity;
        }

       
        /// <summary>
        /// 获取处理日志信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public FlowLogInfoEntity GetFlowLogEntity(FlowLogInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            FlowLogInfoEntity flowLogEntity = clientHelper.PostAuth<FlowLogInfoEntity, FlowLogInfoQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWLOGENTITY), query);
            return flowLogEntity;
        }

        // <summary>
        /// 获取稿件的处理日志
        /// </summary>
        /// <param name="cirEntity">稿件ID,JournalID,分组</param>
        /// <returns></returns>
        public IList<FlowLogInfoEntity> GetFlowLog(CirculationEntity cirEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<FlowLogInfoEntity> flowLogList = clientHelper.PostAuth<IList<FlowLogInfoEntity>, CirculationEntity>(GetAPIUrl(APIConstant.FLOW_GETFLOWLOG), cirEntity);
            if (flowLogList != null)
            {
                foreach (FlowLogInfoEntity item in flowLogList)
                {
                    # region 处理人角色

                    item.SendUserGroupName = GetRoleName(item.SendRoleID);
                    item.RecUserGroupName = GetRoleName(item.RecRoleID);

                    # endregion
                }
            }
            return flowLogList;
        }

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public string GetFlowLogAttachment(FlowLogQuery flowLogQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            string cPath = clientHelper.PostAuth<string, FlowLogQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWLOGATTACHMENT), flowLogQuery);
            return cPath;
        }

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public IDictionary<string, string> GetFlowLogAllAttachment(FlowLogQuery flowLogQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IDictionary<string, string> dictPath = clientHelper.PostAuth<IDictionary<string, string>, FlowLogQuery>(GetAPIUrl(APIConstant.FLOW_GETFLOWLOGATTALLACHMENT), flowLogQuery);
            return dictPath;
        }

        /// <summary>
        /// 更新日志的查看状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public ExecResult UpdateFlowLogIsView(FlowLogQuery flowLogQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, FlowLogQuery>(GetAPIUrl(APIConstant.FLOW_UPDATEFLOWLOGISVIEW), flowLogQuery);
            return execResult;
        }

        /// <summary>
        /// 更新日志的下载状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public ExecResult UpdateFlowLogIsDown(FlowLogQuery flowLogQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, FlowLogQuery>(GetAPIUrl(APIConstant.FLOW_UPDATEFLOWLOGISDOWN), flowLogQuery);
            return execResult;
        }

        /// <summary>
        /// 专家拒审
        /// </summary>
        /// <param name="cirEntity">CID,AuthorID,CNumber,JournalID,EnumCStatus</param>
        /// <returns></returns>
        public bool ExpertDeledit(CirculationEntity cirEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            bool result = clientHelper.PostAuth<bool, CirculationEntity>(GetAPIUrl(APIConstant.FLOW_EXPERTDELEDIT), cirEntity);
            return result;
        }

        /// <summary>
        /// 处理在入款时改变稿件状态
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        public ExecResult DealFinaceInAccount(CirculationEntity cirEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.PostAuth<ExecResult, CirculationEntity>(GetAPIUrl(APIConstant.DealFinaceInAccount), cirEntity);

            return result;
        }

        # endregion

        #region 审稿单相关
        /// <summary>
        /// 获取审稿单项分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public Pager<ReviewBillEntity> GetReviewBillPageList(ReviewBillQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<ReviewBillEntity> pager = clientHelper.PostAuth<Pager<ReviewBillEntity>, ReviewBillQuery>(GetAPIUrl(APIConstant.REVIEWBILL_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取审稿单项列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public IList<ReviewBillEntity> GetReviewBillList(ReviewBillQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<ReviewBillEntity> list = clientHelper.PostAuth<IList<ReviewBillEntity>, ReviewBillQuery>(GetAPIUrl(APIConstant.REVIEWBILL_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取审稿单项实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>        
        public ReviewBillEntity GetReviewBillModel(ReviewBillQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ReviewBillEntity model = clientHelper.PostAuth<ReviewBillEntity, ReviewBillQuery>(GetAPIUrl(APIConstant.REVIEWBILL_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存审稿单项
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>      
        public ExecResult SaveReviewBill(ReviewBillEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.PostAuth<ExecResult, ReviewBillEntity>(GetAPIUrl(APIConstant.REVIEWBILL_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除审稿单项
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public ExecResult DelReviewBill(ReviewBillQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.PostAuth<ExecResult, ReviewBillQuery>(GetAPIUrl(APIConstant.REVIEWBILL_DEL), query);
            return result;
        }

        /// <summary>
        /// 审稿单项是否已经被使用
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public bool ReviewBillIsEnabled(ReviewBillQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            bool result = clientHelper.PostAuth<bool, ReviewBillQuery>(GetAPIUrl(APIConstant.REVIEWBILL_ISENABLED), query);
            return result;
        }

        /// <summary>
        /// 获取审稿单项数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<TreeModel> GetReviewBillTreeList(ReviewBillQuery query)
        {
            var list = GetReviewBillList(query);
            TreeModel treeNode = new TreeModel();
            treeNode.Id = 0;
            treeNode.text = "审稿单项";
            treeNode.url = "";
            treeNode.icon = "";
            treeNode.key = "0";
            treeNode.isexpand = true;
            if (list != null && list.Count > 0)
            {
                var first = list.Where(p => p.PItemID == 0);
                TreeModel node = null;
                foreach (var item in first)
                {
                    node = new TreeModel();
                    node.Id = item.ItemID;
                    node.text = item.Title + "[" + item.ItemTypeName + "]";
                    node.url = string.Empty;
                    node.icon = "";
                    node.key = "0";
                    node.isexpand = true;
                    GetReviewBillTreeList(node, list);
                    treeNode.children.Add(node);
                }
            }
            IList<TreeModel> resultList = new List<TreeModel>();
            resultList.Add(treeNode);
            return resultList;
        }

        /// <summary>
        /// 处理子级
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        private void GetReviewBillTreeList(TreeModel treeNode, IList<ReviewBillEntity> list)
        {
            var child = list.Where(p => p.PItemID == treeNode.Id);
            if (child.Count() == 0)
            {
                treeNode.isexpand = false;
                treeNode.children = null;
                return;
            }
            TreeModel node = null;
            foreach (var item in child)
            {
                node = new TreeModel();
                node.Id = item.ItemID;
                node.text = item.Title + "[" + item.ItemTypeName + "]";
                node.url = string.Empty;
                node.icon = "";
                node.key = "1";
                node.isexpand = true;
                GetReviewBillTreeList(node, list);
                treeNode.children.Add(node);
            }
        }

        /// <summary>
        /// 保存审稿单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>      
        public ExecResult SaveReviewBillContent(ReviewBillContentQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.PostAuth<ExecResult, ReviewBillContentQuery>(GetAPIUrl(APIConstant.REVIEWBILLCONTENT_SAVE), query);
            #region 审稿回执
            Action action = () =>
            {
                try
                {
                    SiteConfigFacadeAPIService service = new SiteConfigFacadeAPIService();

                    MessageTemplateQuery queryTemp = new MessageTemplateQuery();
                    queryTemp.JournalID =query.JournalID;
                    queryTemp.TCategory =-7;//回执
                    var tempList = service.GetMessageTempList(queryTemp).ToList();
                    if (tempList == null)
                        return;
                    var EmailModel = tempList.Find(p => p.TType == 1);
                    var SmsModel = tempList.Find(p => p.TType == 2);
                    if (EmailModel == null && SmsModel == null)
                        return;

                    MessageRecodeEntity LogModel = new MessageRecodeEntity();
                    LogModel.JournalID = query.JournalID;
                    LogModel.SendType = -7;
                    LogModel.SendUser = (long)query.AddUser;

                    IDictionary<string, string> dict = service.GetEmailVariable();
                    var user = new AuthorFacadeAPIService().GetAuthorInfo(new AuthorInfoQuery() { JournalID = query.JournalID, AuthorID = query.AddUser });
                    ContributionInfoEntity model = null;
                    if (query.CID > 0)
                    {
                        ContributionInfoQuery contributionInfoQuery = new ContributionInfoQuery();
                        contributionInfoQuery.JournalID = query.JournalID;
                        contributionInfoQuery.IsAuxiliary = false;
                        contributionInfoQuery.CID = (long)query.CID;
                        AuthorPlatformFacadeAPIService contributionInfoService=new AuthorPlatformFacadeAPIService();
                        model = contributionInfoService.GetContributionInfoModel(contributionInfoQuery);
                        if (model != null)
                        {
                            dict["${稿件标题}$"] = model.Title;
                            dict["${稿件编号}$"] = model.CNumber;
                        }
                    }
                    dict["${接收人}$"] = user.RealName;
                   
                    dict["$稿件主键$"] = result.resultID.ToString();
                    ExecResult execResult = new ExecResult();
                    if (EmailModel != null)
                    {
                        LogModel.MsgType = 1;
                        execResult = service.SendEmailOrSms(new Dictionary<Int64, IDictionary<string, string>>() { { (long)query.AddUser, dict } }, LogModel);
                    }
                    if (SmsModel != null)
                    {
                        LogModel.MsgType = 2;
                        execResult = service.SendEmailOrSms(new Dictionary<Int64, IDictionary<string, string>>() { { (long)query.AddUser, dict } }, LogModel);
                    }

                    if (!execResult.result.Equals(EnumJsonResult.success.ToString()))
                        throw new Exception(execResult.msg);
                }
                catch (Exception ex)
                {
                    LogProvider.Instance.Error("发送审稿回执失败,稿件编码【" + result.resultStr + "】：" + ex.ToString());
                }
            };
            action.BeginInvoke(null, null);
            #endregion
            return result;
        }

        /// <summary>
        /// 获取审稿单项列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public IList<ReviewBillContentEntity> GetReviewBillContentList(ReviewBillContentQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<ReviewBillContentEntity> list = clientHelper.PostAuth<IList<ReviewBillContentEntity>, ReviewBillContentQuery>(GetAPIUrl(APIConstant.REVIEWBILLCONTENT_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取审稿单初始化项
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public IList<ReviewBillContentEntity> GetReviewBillContentListByCID(ReviewBillContentQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<ReviewBillContentEntity> list = clientHelper.PostAuth<IList<ReviewBillContentEntity>, ReviewBillContentQuery>(GetAPIUrl(APIConstant.REVIEWBILLCONTENT_GETLISTBYCID), query);
            return list;
        }

        /// <summary>
        /// 获取审稿单创建字符串
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="AuthorID"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public String GetReviewBillContentStr(Int64 JournalID, Int64 AuthorID, Int64 CID)
        {
            ReviewBillContentQuery query = new ReviewBillContentQuery();
            query.JournalID = JournalID;
            query.AddUser = AuthorID;
            query.CID = CID;
            var list = GetReviewBillContentListByCID(query);
            return GetReviewBillContentTreeStr(list);
        }

        /// <summary>
        /// 获取审稿单前言
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="ExpertID"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public String GetReviewBillContentHead(Int64 JournalID, Int64 ExpertID, Int64 CID)
        {
            SiteConfigFacadeAPIService service = new SiteConfigFacadeAPIService();
            MessageTemplateEntity temp = service.GetMessageTemplate(JournalID, -6, 1);
            if (temp == null)
                return string.Empty;
            var headStr = temp.TContent;
            if (string.IsNullOrWhiteSpace(headStr))
                return string.Empty;
            IDictionary<string, string> dict = service.GetEmailVariable();
            if (headStr.Contains("${接收人}$") || headStr.Contains("${邮箱}$") || headStr.Contains("${手机}$"))
            {
                var user = new AuthorFacadeAPIService().GetAuthorInfo(new AuthorInfoQuery() { JournalID = JournalID, AuthorID = ExpertID });
                dict["${接收人}$"] = user.RealName;
                dict["${邮箱}$"] = user.LoginName;
                dict["${手机}$"] = user.Mobile;
            }
            if (headStr.Contains("${稿件编号}$") || headStr.Contains("${稿件标题}$"))
            {
                ContributionInfoQuery authorQuery = new ContributionInfoQuery();
                authorQuery.JournalID = JournalID;
                authorQuery.CID = CID;
                authorQuery.IsAuxiliary = false;
                var contribution = new AuthorPlatformFacadeAPIService().GetContributionInfoModel(authorQuery);
                dict["${稿件编号}$"] = contribution.CNumber;
                dict["${稿件标题}$"] = contribution.Title;
            }
            SiteConfigEntity config = null;
            if (headStr.Contains("${网站名称}$") || headStr.Contains("${编辑部地址}$") || temp.TContent.Contains("${编辑部邮编}$"))
            {
                config = service.GetSiteConfig(WKT.Config.SiteConfig.SiteID);
            }
            return service.GetEmailOrSmsContent(dict, config, headStr);
        }

        /// <summary>
        /// 获取审稿单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private String GetReviewBillContentTreeStr(IList<ReviewBillContentEntity> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;
            StringBuilder strHtml = new StringBuilder();
            var group = list.GroupBy(p => new { p.JournalID, p.AddUser, p.CID });
            int i = 1;
            foreach (var item in group)
            {
                strHtml.Append("<table border=\"0\" class=\"mainTable\" cellpadding=\"0\" cellspacing=\"1\" align=\"center\"  width=\"100%\" user=\"")
                    .Append(item.Key.AddUser).Append("\" cid=\"").Append(item.Key.CID).Append("\">");
                var first = item.Where(p => p.PItemID == 0).OrderBy(p => p.SortID);
                foreach (var model in first)
                {
                    strHtml.Append("<tr><td id=\""+model.ItemID+"\" class=\"title\">").Append(i).Append(".").Append(model.Title);

                    strHtml.Append("</td></tr>");
                    GetReviewBillContentTreeStrNew(item.ToList(), model, 20, strHtml);
                    i++;
                }
                strHtml.Append("</table>");
            }
            return strHtml.ToString();
        }

        /// <summary>
        /// 获取审稿单(支持多级使用的方法)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="PModel"></param>
        /// <param name="Margin"></param>
        /// <param name="strHtml"></param>
        private void GetReviewBillContentTreeStr(IEnumerable<ReviewBillContentEntity> source, ReviewBillContentEntity PModel, Int32 Margin, StringBuilder strHtml)
        {
            var list = source.Where(p => p.PItemID == PModel.ItemID).OrderBy(p => p.SortID).ToList();
            if (list == null || list.Count == 0)
                return;
            foreach (var model in list)
            {
                strHtml.Append("<tr><td style=\"padding-left:").Append(Margin).Append("px;\"><span>");
                switch (PModel.ItemType)
                {
                    case 1:
                        strHtml.AppendFormat("<input type=\"radio\" id=\"radio_{0}\" value=\"{0}\" name=\"radio_{1}\") ", model.ItemID, PModel.ItemID);
                        if (model.IsChecked)
                            strHtml.Append(" checked=\"checked\" ");
                        strHtml.AppendFormat("/><label for=\"radio_{0}\">{1}</label>", model.ItemID, model.Title);
                        break;
                    case 2:
                        strHtml.AppendFormat("<input type=\"checkbox\" id=\"chk_{0}\" value=\"{0}\" name=\"chk_{1}\") ", model.ItemID, PModel.ItemID);
                        if (model.IsChecked)
                            strHtml.Append(" checked=\"checked\" ");
                        strHtml.AppendFormat("/><label for=\"chk_{0}\">{1}</label>", model.ItemID, model.Title);
                        break;
                    default:
                        strHtml.AppendFormat("{3}：<input type=\"text\" id=\"txt_{0}\" value=\"{1}\" name=\"txt_{2}\" style=\"width:300px;\" class=\"txtbox\"  />"
                            , model.ItemID, model.ContentValue, PModel.ItemID, model.Title);
                        break;
                }
                strHtml.Append("</span></td></tr>");
                GetReviewBillContentTreeStr(source, model, Margin * 2, strHtml);
            }
        }

        /// <summary>
        /// 获取审稿单(支持2级使用的方法)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="PModel"></param>
        /// <param name="Margin"></param>
        /// <param name="strHtml"></param>
        private void GetReviewBillContentTreeStrNew(IEnumerable<ReviewBillContentEntity> source, ReviewBillContentEntity PModel, Int32 Margin, StringBuilder strHtml)
        {
            var list = source.Where(p => p.PItemID == PModel.ItemID).OrderBy(p => p.SortID).ToList();
            if (PModel.ItemType == 1 || PModel.ItemType == 2)
            {
                if (list == null || list.Count == 0)
                    return;
                strHtml.Append("<tr><td style=\"padding-left:").Append(Margin).Append("px;\"><span>");
                foreach (var model in list)
                {

                    switch (PModel.ItemType)
                    {
                        case 1:
                            strHtml.AppendFormat("<input type=\"radio\" id=\"radio_{0}\" value=\"{0}\" name=\"radio_{1}\") ", model.ItemID, PModel.ItemID);
                            if (model.IsChecked)
                                strHtml.Append(" checked=\"checked\" ");
                            strHtml.AppendFormat("/ ><label for=\"radio_{0}\" style=\"margin-right:20px;\">{1}</label>", model.ItemID, model.Title);
                            break;
                        case 2:
                            strHtml.AppendFormat("<input type=\"checkbox\" id=\"chk_{0}\" value=\"{0}\" name=\"chk_{1}\") ", model.ItemID, PModel.ItemID);
                            if (model.IsChecked)
                                strHtml.Append(" checked=\"checked\" ");
                            strHtml.AppendFormat("/><label for=\"chk_{0}\" style=\"margin-right:20px;\">{1}</label>", model.ItemID, model.Title);
                            break;
                    }
                }
                strHtml.Append("</span></td></tr>");
            }
            else
            {
                if (list == null || list.Count == 0)
                {
                    var item = source.Where(p => p.ItemID == PModel.ItemID);
                    strHtml.Append("<tr><td style=\"padding-left:").Append(Margin).Append("px;\"><span>");
                    strHtml.AppendFormat("<input type=\"text\" value=\"{1}\" alt=\"{0}\" name=\"txt_{0}\" style=\"width:300px;\" class=\"txtbox\"  />"
                        , PModel.ItemID, item == null ? string.Empty : item.First().ContentValue);
                    strHtml.Append("</span></td></tr>");
                }
                else
                {
                    foreach (var model in list)
                    {
                        strHtml.Append("<tr><td style=\"padding-left:").Append(Margin).Append("px;\"><span>");
                        strHtml.AppendFormat("{3}：<input type=\"text\" id=\"txt_{0}\" value=\"{1}\" alt=\"{0}\" name=\"txt_{2}\" style=\"width:300px;\" class=\"txtbox\" />"
                            , model.ItemID, model.ContentValue, PModel.ItemID, model.Title);
                    }
                    strHtml.Append("</span></td></tr>");
                }
            }
        }
        #endregion

        # region 稿件多状态合并

        /// <summary>
        /// 查看该稿件是否存在多个状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public long JudgeIsMoreStatus(FlowLogInfoQuery flowLogQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            long flag = clientHelper.PostAuth<long, FlowLogInfoQuery>(GetAPIUrl(APIConstant.FLOW_JUDGEISMORESTATUS), flowLogQuery);
            return flag;
        }

        /// <summary>
        /// 合并多状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult MergeMoreStatus(FlowLogInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, FlowLogInfoQuery>(GetAPIUrl(APIConstant.FLOW_MERGEMORESTATUS), query);
            return execResult;
        }

        /// <summary>
        /// 根据当前所处环节获取稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public List<FlowContribution> GetContributionMoreStatusList(FlowLogInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            List<FlowContribution> execResult = clientHelper.PostAuth<List<FlowContribution>, FlowLogInfoQuery>(GetAPIUrl(APIConstant.FLOW_GETCONTRIBTIONMORESTATUSLIST), query);
            if (execResult != null)
            {
                foreach (FlowContribution item in execResult)
                {
                    if (item.RecUserID > 0)
                    {
                        item.RecUserName = GetMemberName(item.RecUserID);
                    }
                }
            }
            return execResult;
        }

        # endregion

        /// <summary>
        /// 继续送交
        /// </summary>
        /// <param name="sendEntity"></param>
        /// <returns></returns>
        public ExecResult ContinuSend(ContinuSendEntity sendEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, ContinuSendEntity>(GetAPIUrl(APIConstant.FLOW_CONTINUSEND), sendEntity);
            return execResult;
        }
    }
}
