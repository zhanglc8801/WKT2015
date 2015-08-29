using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;

namespace WKT.Facade.Service.Interface
{
    /// <summary>
    /// 审稿流程服务
    /// </summary>
    public interface IFlowFacadeService
    {
        # region 审稿状态

        /// <summary>
        /// 获取拥有权限的审稿状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<FlowStatusEntity> GetHaveRightFlowStatus(FlowStatusQuery query);

        /// <summary>
        /// 获取拥有权限的审稿状态(用于统计同一稿件一个状态下送多人时按一个计算)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<FlowStatusEntity> GetHaveRightFlowStatusForStat(FlowStatusQuery query);

        /// <summary>
        /// 获取审稿状态键值对，审稿状态名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IDictionary<long, string> GetFlowStatusDictStatusName(FlowStatusQuery query);

        /// <summary>
        /// 获取审稿状态键值对，审稿状态显示名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IDictionary<long, string> GetFlowStatusDictDisplayName(FlowStatusQuery query);

        /// <summary>
        /// 获取审稿状态列表
        /// </summary>
        /// <param name="queryFlowStatus"></param>
        /// <returns></returns>
        List<FlowStatusEntity> GetFlowStatusList(FlowStatusQuery queryFlowStatus);

        /// <summary>
        /// 根据指定的审稿状态ID，得到审稿状态的基本信息
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="StatusID"></param>
        /// <returns></returns>
        FlowStatusEntity GetFlowStatusInfoByID(FlowStatusQuery query);

        /// <summary>
        /// 获取审稿流程状态序号
        /// </summary>
        /// <param name="queryFlowSet"></param>
        /// <returns></returns>
        int GetFlowStatusSortID(FlowStatusQuery queryFlowStatus);

        /// <summary>
        /// 判断审稿状态对应的稿件状态是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        FlowStatusEntity CheckCStatusIsExists(FlowStatusQuery query);

        /// <summary>
        /// 获取审稿状态基本信息及配置信息
        /// </summary>
        /// <param name="queryFlowSet"></param>
        /// <returns></returns>
        FlowStep GetFlowStepInfo(FlowStatusQuery queryFlowStatus);

        /// <summary>
        /// 新增审稿状态
        /// </summary>
        /// <param name="flowStepEntity"></param>
        /// <returns></returns>
        ExecResult AddFlowStatus(FlowStep flowStepEntity);

        /// <summary>
        /// 修改审稿状态
        /// </summary>
        /// <param name="flowStepEntity"></param>
        /// <returns></returns>
        ExecResult EditFlowStatus(FlowStep flowStepEntity);

        /// <summary>
        /// 删除审稿环节
        /// </summary>
        /// <param name="flowStatusEntity"></param>
        /// <returns></returns>
        ExecResult DelFlowStatus(FlowStatusEntity flowStatusEntity);

        /// <summary>
        /// 修改审稿状态状态
        /// </summary>
        /// <param name="flowStatusEntity"></param>
        /// <returns></returns>
        ExecResult UpdateFlowStatus(FlowStatusEntity flowStatusEntity);

        # endregion

        # region 审稿操作设置

        /// <summary>
        /// 获取操作实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        FlowActionEntity GetFlowActionEntity(FlowActionQuery query);

        /// <summary>
        /// 根据当前操作状态获取可以做的操作
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<FlowActionEntity> GetFlowActionByStatus(FlowActionQuery query);

        /// <summary>
        /// 获取当前状态下的所有操作
        /// </summary>
        /// <param name="queryFlowSet"></param>
        /// <returns></returns>
        List<FlowActionEntity> GetFlowActionList(FlowActionQuery queryFlowAction);

        /// <summary>
        /// 新增审稿操作
        /// </summary>
        /// <param name="flowActionEntity"></param>
        /// <returns></returns>
        ExecResult AddFlowAction(FlowActionEntity flowActionEntity);

        /// <summary>
        /// 修改审稿操作
        /// </summary>
        /// <param name="flowActionEntity"></param>
        /// <returns></returns>
        ExecResult EditFlowAction(FlowActionEntity flowActionEntity);

        /// <summary>
        /// 删除审稿操作
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        ExecResult DelFlowAction(FlowActionEntity flowActionEntity);

        # endregion

        # region 审稿环节权限配置

        /// <summary>
        /// 获取审稿环节作者权限设置
        /// </summary>
        /// <param name="queryFlowAuthAuthor"></param>
        /// <returns></returns>
        List<FlowAuthAuthorEntity> GetFlowAuthAuthorList(FlowAuthAuthorQuery queryFlowAuthAuthor);

        /// <summary>
        /// 设置审稿环节作者权限
        /// </summary>
        /// <param name="flowAuthAuthorEntity"></param>
        /// <returns></returns>
        ExecResult SetFlowAuthAuthor(IList<FlowAuthAuthorEntity> flowAuthAuthorEntity);

        /// <summary>
        /// 删除审稿环节作者权限
        /// </summary>
        /// <param name="flowAuthAuthorEntity"></param>
        /// <returns></returns>
        ExecResult DeleteFlowAuthAuthor(IList<FlowAuthAuthorEntity> flowAuthAuthorEntity);

        /// <summary>
        /// 获取审稿环节角色权限设置
        /// </summary>
        /// <param name="queryFlowAuthRole"></param>
        /// <returns></returns>
        List<FlowAuthRoleEntity> GetFlowAuthRoleList(FlowAuthRoleQuery queryFlowAuthRole);

        /// <summary>
        /// 设置审稿环节角色权限
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        ExecResult SetFlowAuthRole(IList<FlowAuthRoleEntity> flowAuthRoleEntity);

        /// <summary>
        /// 删除审稿环节角色权限
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        ExecResult DeleteFlowAuthRole(IList<FlowAuthRoleEntity> flowAuthRoleEntity);

        # endregion

        # region 审稿环节条件设置

        # endregion

        # region 流程流转

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
        /// 得到下一步骤信息
        /// </summary>
        /// <param name="ciEntity">流转信息</param>
        /// <returns></returns>
        FlowStep GetNextFlowStep(CirculationEntity cirEntity);

        /// <summary>
        /// 发送系统通知
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        ExecResult SendSysMessage(AuditBillEntity auditBillEntity, bool isRecode = true );

        /// <summary>
        /// 提交审稿单
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        ExecResult SubmitAuditBill(AuditBillEntity auditBillEntity);

        /// <summary>
        /// 获取处理日志信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        FlowLogInfoEntity GetFlowLogEntity(FlowLogInfoQuery query);

        // <summary>
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
        ExecResult UpdateFlowLogIsView(FlowLogQuery flowLogQuery);

        /// <summary>
        /// 更新日志的下载状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        ExecResult UpdateFlowLogIsDown(FlowLogQuery flowLogQuery);

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
        ExecResult DealFinaceInAccount(CirculationEntity cirEntity);

        # endregion

        #region 审稿单相关
        /// <summary>
        /// 获取审稿单项分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        Pager<ReviewBillEntity> GetReviewBillPageList(ReviewBillQuery query);

        /// <summary>
        /// 获取审稿单项列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        IList<ReviewBillEntity> GetReviewBillList(ReviewBillQuery query);

        /// <summary>
        /// 获取审稿单项实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>        
        ReviewBillEntity GetReviewBillModel(ReviewBillQuery query);

        /// <summary>
        /// 保存审稿单项
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>      
        ExecResult SaveReviewBill(ReviewBillEntity model);

        /// <summary>
        /// 删除审稿单项
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        ExecResult DelReviewBill(ReviewBillQuery query);

        /// <summary>
        /// 审稿单项是否已经被使用
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        bool ReviewBillIsEnabled(ReviewBillQuery query);

        /// <summary>
        /// 获取审稿单项数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<TreeModel> GetReviewBillTreeList(ReviewBillQuery query);

        /// <summary>
        /// 保存审稿单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>      
        ExecResult SaveReviewBillContent(ReviewBillContentQuery query);

        /// <summary>
        /// 获取审稿单项列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        IList<ReviewBillContentEntity> GetReviewBillContentList(ReviewBillContentQuery query);

        /// <summary>
        /// 获取审稿单初始化项
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        IList<ReviewBillContentEntity> GetReviewBillContentListByCID(ReviewBillContentQuery query);

        /// <summary>
        /// 获取审稿单创建字符串
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="AuthorID"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        String GetReviewBillContentStr(Int64 JournalID, Int64 AuthorID, Int64 CID);

        /// <summary>
        /// 获取审稿单前言
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="ExpertID"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        String GetReviewBillContentHead(Int64 JournalID, Int64 ExpertID, Int64 CID);
        #endregion

        # region 稿件多状态合并

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
        ExecResult MergeMoreStatus(FlowLogInfoQuery query);

        /// <summary>
        /// 根据当前所处环节获取稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        List<FlowContribution> GetContributionMoreStatusList(FlowLogInfoQuery query);

        # endregion

        /// <summary>
        /// 继续送交
        /// </summary>
        /// <param name="sendEntity"></param>
        /// <returns></returns>
        ExecResult ContinuSend(ContinuSendEntity sendEntity);
    }
}
