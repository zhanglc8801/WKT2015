using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public interface IFlowCirculationService
    {
        /// <summary>
        /// 得到稿件的处理人
        /// </summary>
        /// <param name="cirEntity">稿件ID(CID)、稿件状态(EnumCStatus)、编辑部ID(JournalID)</param>
        /// <returns></returns>
        AuthorInfoEntity GetContributionProcesser(CirculationEntity cirEntity);

        /// <summary>
        /// 根据当前所处环节获取稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        Pager<FlowContribution> GetFlowContributionList(CirculationEntity cirEntity);

        /// <summary>
        /// 获取作者最新稿件状态稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        Pager<FlowContribution> GetAuthorContributionList(CirculationEntity cirEntity);

        /// <summary>
        /// 得到下一步骤信息
        /// </summary>
        /// <param name="ciEntity">流转信息</param>
        /// <returns></returns>
        FlowStep GetNextFlowStep(CirculationEntity cirEntity);

        /// <summary>
        /// 提交审稿单
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        bool SubmitAuditBill(AuditBillEntity auditBillEntity);

        /// <summary>
        /// 处理审稿流程
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        bool ProcessFlowCirulation(AuditBillEntity auditBillEntity);

        /// <summary>
        /// 获取处理日志信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        FlowLogInfoEntity GetFlowLogEntity(FlowLogInfoQuery query);

        /// <summary>
        /// 获取稿件的处理日志
        /// </summary>
        /// <param name="cirEntity">稿件ID,JournalID,分组</param>
        /// <returns></returns>
        IList<FlowLogInfoEntity> GetFlowLog(CirculationEntity cirEntity);

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        string GetFlowLogAttachment(FlowLogQuery flowLogQuery);

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        IDictionary<string, string> GetFlowLogAllAttachment(FlowLogQuery flowLogQuery);

        /// <summary>
        /// 更新日志的查看状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        bool UpdateFlowLogIsView(FlowLogQuery flowLogQuery);

        /// <summary>
        /// 更新日志的下载状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        bool UpdateFlowLogIsDown(FlowLogQuery flowLogQuery);

        /// <summary>
        /// 获取专家待审、已审稿件列表
        /// </summary>
        /// <param name="cirEntity">需要传值:JournalID，CurAuthorID，Status</param>
        /// <returns></returns>
        Pager<FlowContribution> GetExpertContributionList(CirculationEntity cirEntity);


        /// <summary>
        /// 获取同步状态稿件列表，例如：专家拒审，作者退修、已发校样、录用、退稿稿件列表
        /// </summary>
        /// <param name="cirEntity">需要传值:JournalID，CurAuthorID，EnumCStatus</param>
        /// <returns></returns>
        Pager<FlowContribution> GetSynchroStatusContributionList(CirculationEntity cirEntity);

        /// <summary>
        /// 专家拒审
        /// </summary>
        /// <param name="cirEntity">CID,AuthorID,CNumber,JournalID,EnumCStatus</param>
        /// <returns></returns>
        bool ExpertDeledit(CirculationEntity cirEntity);

        /// <summary>
        /// 处理在入款时改变稿件状态
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        bool DealFinaceInAccount(CirculationEntity cirEntity);

        /// <summary>
        /// 查看该稿件是否存在多个状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        long JudgeIsMoreStatus(FlowLogInfoQuery flowLogQuery);

		/// <summary>
        /// 合并多状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        bool MergeMoreStatus(FlowLogInfoQuery query);

		/// <summary>
        /// 根据当前所处环节获取稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        List<FlowContribution> GetContributionMoreStatusList(FlowLogInfoQuery query);

        /// <summary>
        /// 继续送交s
        /// </summary>
        /// <param name="sendEntity"></param>
        /// <returns></returns>
        bool ContinuSend(ContinuSendEntity sendEntity);
    }
}
