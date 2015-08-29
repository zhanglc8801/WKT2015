using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;
using WKT.Model.Enum;

namespace WKT.Service
{
    public class FlowCirculationService : IFlowCirculationService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IFlowCirculationBusiness flowCirculationBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IFlowCirculationBusiness FlowCirculationBusProvider
        {
            get
            {
                 if(flowCirculationBusProvider == null)
                 {
                      flowCirculationBusProvider = new FlowCirculationBusiness();//ServiceBusContainer.Instance.Container.Resolve<IFlowActionBusiness>();
                 }
                 return flowCirculationBusProvider;
            }
            set
            {
              flowCirculationBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FlowCirculationService()
        {
        }

        /// <summary>
        /// 得到稿件的处理人
        /// </summary>
        /// <param name="cirEntity">稿件ID(CID)、稿件状态(EnumCStatus)、编辑部ID(JournalID)</param>
        /// <returns></returns>
        public AuthorInfoEntity GetContributionProcesser(CirculationEntity cirEntity)
        {
            return FlowCirculationBusProvider.GetContributionProcesser(cirEntity);
        }

        /// <summary>
        /// 根据当前所处环节获取稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public Pager<FlowContribution> GetFlowContributionList(CirculationEntity cirEntity)
        {
            return FlowCirculationBusProvider.GetFlowContributionList(cirEntity);
        }

        /// <summary>
        /// 获取作者最新稿件状态稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public Pager<FlowContribution> GetAuthorContributionList(CirculationEntity cirEntity)
        {
            return FlowCirculationBusProvider.GetAuthorContributionList(cirEntity);
        }

        /// <summary>
        /// 得到下一步骤信息
        /// </summary>
        /// <param name="ciEntity">流转信息</param>
        /// <returns></returns>
        public FlowStep GetNextFlowStep(CirculationEntity cirEntity)
        {
            return FlowCirculationBusProvider.GetNextFlowStep(cirEntity);
        }

        /// <summary>
        /// 提交审稿单
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        public bool SubmitAuditBill(AuditBillEntity auditBillEntity)
        {
            bool flag = false;

            // 得到审稿操作信息
            FlowActionQuery actionQuery = new FlowActionQuery();
            actionQuery.ActionID = auditBillEntity.ActionID;
            actionQuery.JournalID = auditBillEntity.JournalID;
            FlowActionEntity actionEntity = new FlowActionService().GetFlowActionEntity(actionQuery);

            // 得到审稿状态及配置信息
            FlowStatusQuery flowStatusQuery = new FlowStatusQuery();
            flowStatusQuery.JournalID = auditBillEntity.JournalID;
            flowStatusQuery.StatusID = actionEntity == null ? 0 : actionEntity.StatusID;
            FlowStep flowStep = new FlowStatusService().GetFlowStep(flowStatusQuery);

            //得到稿件信息
            ContributionInfoEntity contributionInfoEntity = new ContributionInfoEntity();
            ContributionInfoQuery query = new ContributionInfoQuery();
            query.JournalID = auditBillEntity.JournalID;
            query.CID = auditBillEntity.CID;
            ContributionInfoService service = new ContributionInfoService();
            contributionInfoEntity=service.GetContributionInfo(query);

            if (contributionInfoEntity.AuthorID == Convert.ToInt64(auditBillEntity.ReveiverList.Split(',')[0]))
            {
                auditBillEntity.ReveiverList = contributionInfoEntity.AuthorID.ToString();
                flag = FlowCirculationBusProvider.ProcessFlowCirulation(auditBillEntity);
            }
            else
                flag = FlowCirculationBusProvider.ProcessFlowCirulation(auditBillEntity);
            

            if (flag)
            {
                if (flowStep.FlowConfig != null)
                {
                    if (flowStep.FlowConfig.IsEmailRemind)
                    {
                        /// TODO:记录定期发送邮件的任务
                    }

                    if (flowStep.FlowConfig.IsSMSRemind)
                    {
                        /// TODO:记录定期发送短信的任务
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 处理审稿流程
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        public bool ProcessFlowCirulation(AuditBillEntity auditBillEntity)
        {
            return FlowCirculationBusProvider.ProcessFlowCirulation(auditBillEntity);
        }

        /// <summary>
        /// 获取处理日志信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public FlowLogInfoEntity GetFlowLogEntity(FlowLogInfoQuery query)
        {
            return FlowCirculationBusProvider.GetFlowLogEntity(query);
        }

        /// <summary>
        /// 获取稿件的处理日志
        /// </summary>
        /// <param name="cirEntity">稿件ID,JournalID,分组</param>
        /// <returns></returns>
        public IList<FlowLogInfoEntity> GetFlowLog(CirculationEntity cirEntity)
        {
            return FlowCirculationBusProvider.GetFlowLog(cirEntity);
        }

        /// <summary>
        /// 更新日志的查看状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public bool UpdateFlowLogIsView(FlowLogQuery flowLogQuery)
        {
            return FlowCirculationBusProvider.UpdateFlowLogIsView(flowLogQuery);
        }

        /// <summary>
        /// 更新日志的下载状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public bool UpdateFlowLogIsDown(FlowLogQuery flowLogQuery)
        {
            return FlowCirculationBusProvider.UpdateFlowLogIsDown(flowLogQuery);
        }

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public string GetFlowLogAttachment(FlowLogQuery flowLogQuery)
        {
            return FlowCirculationBusProvider.GetFlowLogAttachment(flowLogQuery);
        }

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public IDictionary<string, string> GetFlowLogAllAttachment(FlowLogQuery flowLogQuery)
        {
            return FlowCirculationBusProvider.GetFlowLogAllAttachment(flowLogQuery);

        }

        /// <summary>
        /// 获取专家待审、已审稿件列表
        /// </summary>
        /// <param name="cirEntity">需要传值:JournalID，CurAuthorID，Status</param>
        /// <returns></returns>
        public Pager<FlowContribution> GetExpertContributionList(CirculationEntity cirEntity)
        {
            return FlowCirculationBusProvider.GetExpertContributionList(cirEntity);
        }


        /// <summary>
        /// 获取同步状态稿件列表，例如：专家拒审，作者退修、已发校样、录用、退稿稿件列表
        /// </summary>
        /// <param name="cirEntity">需要传值:JournalID，CurAuthorID，EnumCStatus</param>
        /// <returns></returns>
        public Pager<FlowContribution> GetSynchroStatusContributionList(CirculationEntity cirEntity)
        {
            return FlowCirculationBusProvider.GetSynchroStatusContributionList(cirEntity);
        }

        /// <summary>
        /// 专家拒审
        /// </summary>
        /// <param name="cirEntity">CID,AuthorID,CNumber,JournalID,EnumCStatus</param>
        /// <returns></returns>
        public bool ExpertDeledit(CirculationEntity cirEntity)
        {
            bool result = FlowCirculationBusProvider.AuthorContribution(cirEntity);
            return result;
        }

        /// <summary>
        /// 处理在入款时改变稿件状态
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        public bool DealFinaceInAccount(CirculationEntity cirEntity)
        {
            return FlowCirculationBusProvider.DealFinaceInAccount(cirEntity);
        }

        /// <summary>
        /// 查看该稿件是否存在多个状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public long JudgeIsMoreStatus(FlowLogInfoQuery flowLogQuery)
        {
            return FlowCirculationBusProvider.JudgeIsMoreStatus(flowLogQuery);
        }

		/// <summary>
        /// 合并多状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool MergeMoreStatus(FlowLogInfoQuery query)
        {
            return FlowCirculationBusProvider.MergeMoreStatus(query);
        }

		/// <summary>
        /// 根据当前所处环节获取稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public List<FlowContribution> GetContributionMoreStatusList(FlowLogInfoQuery query)
        {
            return FlowCirculationBusProvider.GetContributionMoreStatusList(query);
        }

        /// <summary>
        /// 继续送交s
        /// </summary>
        /// <param name="sendEntity"></param>
        /// <returns></returns>
        public bool ContinuSend(ContinuSendEntity sendEntity)
        {
            return FlowCirculationBusProvider.ContinuSend(sendEntity);
        }
    }
}
