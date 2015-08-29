using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    /// <summary>
    /// 审稿流程流转
    /// </summary>
    public class FlowCirculationBusiness : IFlowCirculationBusiness
    {
        /// <summary>
        /// 作者投稿后调用该方法进入审稿流程
        /// </summary>
        /// <returns></returns>
        public bool AuthorContribution(CirculationEntity cirEntity)
        {
            bool flag = false;
            bool isFirstContribute = FlowCirculationDataAccess.Instance.IsFirstContribute(cirEntity);
            if (isFirstContribute)
            {
                // 第一次投稿
                flag = FlowCirculationDataAccess.Instance.AuthorContribute(cirEntity);
            }
            else
            {
                flag = FlowCirculationDataAccess.Instance.ProcessDealBackContribution(cirEntity);
            }
            return flag;
        }

        /// <summary>
        /// 得到稿件的处理人
        /// </summary>
        /// <param name="cirEntity">稿件ID(CID)、稿件状态(EnumCStatus)、编辑部ID(JournalID)</param>
        /// <returns></returns>
        public AuthorInfoEntity GetContributionProcesser(CirculationEntity cirEntity)
        {
            return FlowCirculationDataAccess.Instance.GetContributionProcesser(cirEntity);
        }

        /// <summary>
        /// 根据当前所处环节获取稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public Pager<FlowContribution> GetFlowContributionList(CirculationEntity cirEntity)
        {
            if (cirEntity.CStatus == null)// 获取正常的稿件处理列表
            {
                return FlowCirculationDataAccess.Instance.GetFlowContributionList(cirEntity);
            }
            else
            {
                // 获取指定状态的稿件列表，例如：退稿
                return FlowCirculationDataAccess.Instance.GetStatusContributionList(cirEntity);
            }
        }

        /// <summary>
        /// 获取作者最新稿件状态稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public Pager<FlowContribution> GetAuthorContributionList(CirculationEntity cirEntity)
        {
            return FlowCirculationDataAccess.Instance.GetAuthorContributionList(cirEntity);
        }

        /// <summary>
        /// 得到下一步骤信息
        /// </summary>
        /// <param name="ciEntity">流转信息</param>
        /// <returns></returns>
        public FlowStep GetNextFlowStep(CirculationEntity cirEntity)
        {
            return FlowCirculationDataAccess.Instance.GetNextFlowStep(cirEntity);
        }

        /// <summary>
        /// 处理审稿流程
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        public bool ProcessFlowCirulation(AuditBillEntity auditBillEntity)
        {
            if (auditBillEntity.ActionID == -1)
            {
                return FlowCirculationDataAccess.Instance.ProcessFlowCirulationNoAction(auditBillEntity);   
            }
            else
            {
                return FlowCirculationDataAccess.Instance.ProcessFlowCirulation(auditBillEntity);
            }
        }

        /// <summary>
        /// 处理在入款时改变稿件状态
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        public bool DealFinaceInAccount(CirculationEntity cirEntity)
        {
            return FlowCirculationDataAccess.Instance.DealFinaceInAccount(cirEntity);
        }

        /// <summary>
        /// 获取处理日志信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public FlowLogInfoEntity GetFlowLogEntity(FlowLogInfoQuery query)
        {
            return FlowCirculationDataAccess.Instance.GetFlowLogEntity(query);
        }

        /// <summary>
        /// 获取稿件的处理日志
        /// </summary>
        /// <param name="cirEntity">稿件ID,JournalID,分组</param>
        /// <returns></returns>
        public IList<FlowLogInfoEntity> GetFlowLog(CirculationEntity cirEntity)
        {
            IList<FlowLogInfoEntity> listFlowLog = FlowCirculationDataAccess.Instance.GetFlowLog(cirEntity);
            long TempUserID = 0;
            foreach (FlowLogInfoEntity item in listFlowLog)
            {
                // 如果是原路返回
                if (item.ActionType == 2)
                {
                    TempUserID = item.SendUserID;
                    item.SendUserID = item.RecUserID;
                    item.RecUserID = TempUserID;
                }
            }
            return listFlowLog;
        }


        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public string GetFlowLogAttachment(FlowLogQuery flowLogQuery)
        {
            return FlowCirculationDataAccess.Instance.GetFlowLogAttachment(flowLogQuery);
        }

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public IDictionary<string, string> GetFlowLogAllAttachment(FlowLogQuery flowLogQuery)
        {
            return FlowCirculationDataAccess.Instance.GetFlowLogAllAttachment(flowLogQuery);
        }

        /// <summary>
        /// 更新日志的查看状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public bool UpdateFlowLogIsView(FlowLogQuery flowLogQuery)
        {
            return FlowCirculationDataAccess.Instance.UpdateFlowLogIsView(flowLogQuery);
        }

        /// <summary>
        /// 更新日志的下载状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public bool UpdateFlowLogIsDown(FlowLogQuery flowLogQuery)
        {
            return FlowCirculationDataAccess.Instance.UpdateFlowLogIsDown(flowLogQuery);
        }

        /// <summary>
        /// 获取专家待审、已审稿件列表
        /// </summary>
        /// <param name="cirEntity">需要传值:JournalID，CurAuthorID，Status</param>
        /// <returns></returns>
        public Pager<FlowContribution> GetExpertContributionList(CirculationEntity cirEntity)
        {
            return FlowCirculationDataAccess.Instance.GetExpertContributionList(cirEntity);
        }


        /// <summary>
        /// 获取同步状态稿件列表，例如：专家拒审，作者退修、已发校样、录用、退稿稿件列表
        /// </summary>
        /// <param name="cirEntity">需要传值:JournalID，CurAuthorID，EnumCStatus</param>
        /// <returns></returns>
        public Pager<FlowContribution> GetSynchroStatusContributionList(CirculationEntity cirEntity)
        {
            return FlowCirculationDataAccess.Instance.GetSynchroStatusContributionList(cirEntity);
        }

        /// <summary>
        /// 查看该稿件是否存在多个状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public long JudgeIsMoreStatus(FlowLogInfoQuery flowLogQuery)
        {
            return FlowCirculationDataAccess.Instance.JudgeIsMoreStatus(flowLogQuery);
        }

		/// <summary>
        /// 合并多状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool MergeMoreStatus(FlowLogInfoQuery query)
        {
            return FlowCirculationDataAccess.Instance.MergeMoreStatus(query);
        }

		/// <summary>
        /// 根据当前所处环节获取稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public List<FlowContribution> GetContributionMoreStatusList(FlowLogInfoQuery query)
        {
            return FlowCirculationDataAccess.Instance.GetContributionMoreStatusList(query);
        }

        /// <summary>
        /// 继续送交s
        /// </summary>
        /// <param name="sendEntity"></param>
        /// <returns></returns>
        public bool ContinuSend(ContinuSendEntity sendEntity)
        {
            return FlowCirculationDataAccess.Instance.ContinuSend(sendEntity);
        }
    }
}
