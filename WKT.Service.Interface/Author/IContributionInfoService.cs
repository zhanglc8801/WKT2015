using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface IContributionInfoService
    {
        #region 稿件信息

        /// <summary>
        /// 获取稿件的附件
        /// </summary>
        /// <param name="cQuery">稿件ID</param>
        /// <returns></returns>
        string GetContributionAttachment(ContributionInfoQuery cQuery);

        /// <summary>
        /// 投稿
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult AuthorPlatform(ContributionInfoEntity model);

        /// <summary>
        /// 保存稿件格式修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveFormat(ContributionInfoEntity model);

        /// <summary>
        /// 新增稿件表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddContributionInfo(ContributionInfoEntity model);

        /// <summary>
        /// 编辑稿件表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateContributionInfo(ContributionInfoEntity model);

        /// <summary>
        /// 获取稿件信息实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ContributionInfoEntity GetContributionInfo(ContributionInfoQuery query);

        /// <summary>
        /// 判断稿件标题是否存在
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="CID"></param>
        /// <param name="CTitle"></param>
        /// <returns></returns>
        bool ContributionTitleIsExists(Int64 JournalID, Int64 CID, string CTitle);

        /// <summary>
        /// 改变稿件状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult ChangeStatus(ContributionInfoQuery query);

        /// <summary>
        /// 删除投稿信息
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        ExecResult DelContributionInfo(Int64[] CID);

        /// <summary>
        /// 获取稿件分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<ContributionInfoEntity> GetContributionInfoPageList(ContributionInfoQuery query);

        /// <summary>
        /// 获取稿件数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<ContributionInfoEntity> GetContributionInfoList(ContributionInfoQuery query);
        #endregion

        #region 稿件作者信息
        /// <summary>
        /// 新增稿件作者信息
        /// </summary>
        /// <param name="contributionAuthorEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        bool AddContributionAuthor(ContributionAuthorEntity contributionAuthorEntity);

        /// <summary>
        /// 编辑稿件作者信息
        /// </summary>
        /// <param name="contributionAuthorEntity"></param>      
        /// <returns></returns>
        bool UpdateContributionAuthor(ContributionAuthorEntity contributionAuthorEntity);

        /// <summary>
        /// 获取稿件作者实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        ContributionAuthorEntity GetContributionAuthor(Int64 CAuthorID);

        /// <summary>
        /// 根据稿件编号删除稿件作者信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        bool DelContributionAuthorByCID(Int64[] CID);

        /// <summary>
        /// 删除稿件作者信息
        /// </summary>
        /// <param name="CAuthorID"></param>
        /// <returns></returns>
        bool DelContributionAuthor(Int64[] CAuthorID);

        /// <summary>
        /// 获取稿件作者分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<ContributionAuthorEntity> GetContributionAuthorPageList(ContributionAuthorQuery query);

        /// <summary>
        /// 获取稿件作者数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<ContributionAuthorEntity> GetContributionAuthorList(ContributionAuthorQuery query);
        #endregion

        #region 参考文献信息
        /// <summary>
        /// 新增参考文献
        /// </summary>
        /// <param name="contributionReferenceEntity"></param>
        /// <returns></returns>
        bool AddContributionReference(ContributionReferenceEntity contributionReferenceEntity);

        /// <summary>
        /// 编辑参考文献
        /// </summary>
        /// <param name="contributionReferenceEntity"></param>
        /// <returns></returns>
        bool UpdateContributionReference(ContributionReferenceEntity contributionReferenceEntity);

        /// <summary>
        /// 获取参考文献实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        ContributionReferenceEntity GetContributionReference(Int64 ReferenceID);

        /// <summary>
        /// 根据稿件编号删除参考文献
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        bool DelContributionReferenceByCID(Int64[] CID);

        /// <summary>
        /// 删除参考文献
        /// </summary>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        bool DelContributionReference(Int64[] ReferenceID);

        /// <summary>
        /// 获取参考文献分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<ContributionReferenceEntity> GetContributionReferencePageList(ContributionReferenceQuery query);

        /// <summary>
        /// 获取参考文献数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<ContributionReferenceEntity> GetContributionReferenceList(ContributionReferenceQuery query);
        #endregion

        #region 基金信息
        /// <summary>
        /// 新增基金信息
        /// </summary>
        /// <param name="contributionFundEntity"></param>
        /// <returns></returns>
        bool AddContributionFund(ContributionFundEntity contributionFundEntity);

        /// <summary>
        /// 编辑基金信息
        /// </summary>
        /// <param name="contributionFundEntity"></param>
        /// <returns></returns>
        bool UpdateContributionFund(ContributionFundEntity contributionFundEntity);

        /// <summary>
        /// 获取基金信息实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        ContributionFundEntity GetContributionFund(Int64 FundID);

        /// <summary>
        /// 根据稿件编号删除基金信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        bool DelContributionFundByCID(Int64[] CID);

        /// <summary>
        /// 删除基金信息
        /// </summary>
        /// <param name="FundID"></param>
        /// <returns></returns>
        bool DelContributionFund(Int64[] FundID);

        /// <summary>
        /// 获取基金信息分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<ContributionFundEntity> GetContributionFundPageList(ContributionFundQuery query);

        /// <summary>
        /// 获取基金信息数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<ContributionFundEntity> GetContributionFundList(ContributionFundQuery query);
        #endregion

        #region 大字段信息
        /// <summary>
        /// 获取大字段信息实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        ContributionInfoAttEntity GetContributionInfoAtt(Int64 PKID);

        /// <summary>
        /// 获取大字段信息实体
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        ContributionInfoAttEntity GetContributionInfoAttByCID(Int64 CID);

        /// <summary>
        /// 根据稿件编号删除大字段信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        bool DelContributionInfoAttByCID(Int64[] CID);

        /// <summary>
        /// 删除大字段信息
        /// </summary>
        /// <param name="FundID"></param>
        /// <returns></returns>
        bool DelContributionInfoAtt(Int64[] PKID);
        #endregion

        #region 撤稿相关
        /// <summary>
        /// 撤稿
        /// </summary>
        /// <param name="retractionsBillsEntity"></param>
        /// <returns></returns>
        ExecResult DraftContribution(RetractionsBillsEntity model);

        /// <summary>
        /// 添加撤稿信息
        /// </summary>
        /// <param name="retractionsBillsEntity"></param>
        /// <returns></returns>
        bool AddRetractionsBills(RetractionsBillsEntity model);

        /// <summary>
        /// 编辑撤稿信息
        /// </summary>
        /// <param name="retractionsBillsEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        bool UpdateRetractionsBills(RetractionsBillsEntity model);

        /// <summary>
        /// 获取撤稿信息
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns></returns>
        RetractionsBillsEntity GetRetractionsBills(RetractionsBillsQuery rQuery);
        #endregion

        #region 添加稿件备注
        /// <summary>
        /// 新增稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddCRemark(CRemarkEntity model);

        /// <summary>
        /// 编辑稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateCRemark(CRemarkEntity model);

        /// <summary>
        /// 保存稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveCRemark(CRemarkEntity model);

        /// <summary>
        /// 获取稿件备注
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        CRemarkEntity GetCRemark(CRemarkQuery query);
        #endregion

        # region 更新稿件标记及状态 by sxd

        /// <summary>
        /// 更新稿件旗帜标记
        /// </summary>        
        /// <param name="cEntityList">稿件信息</param>
        /// <returns></returns>
        bool BatchUpdateContributionFlag(List<ContributionInfoQuery> cEntityList);

        /// <summary>
        /// 更新稿件旗帜标记
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        bool UpdateContributionFlag(ContributionInfoQuery cEntity);

        /// <summary>
        /// 更新稿件加急标记
        /// </summary>        
        /// <param name="cEntityList">稿件信息</param>
        /// <returns></returns>
        bool BatchUpdateContributionIsQuick(List<ContributionInfoQuery> cEntityList);

        /// <summary>
        /// 更新稿件加急标记
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        bool UpdateContributionIsQuick(ContributionInfoQuery cEntity);

        /// <summary>
        /// 更新稿件状态及审核状态
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        bool UpdateContributionStatus(ContributionInfoQuery cEntity);

        # endregion

        #region 收稿量统计
        /// <summary>
        /// 按年月统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<ContributionAccountEntity> GetContributionAccountListByYear(ContributionAccountQuery query);

        /// <summary>
        /// 按基金级别统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<ContributionAccountEntity> GetContributionAccountListByFund(ContributionAccountQuery query);

        /// <summary>
        /// 按作者统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<ContributionAccountEntity> GetContributionAccountListByAuhor(ContributionAccountQuery query);
        #endregion

        /// <summary>
        /// 更新介绍信标记
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateIntroLetter(ContributionInfoQuery model);

        /// <summary>
        /// 处理撤稿申请
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        bool DealWithdrawal(ContributionInfoQuery cEntity);

        /// <summary>
        /// 撤销删除
        /// </summary>
        /// <param name="cEntity"></param>
        /// <returns></returns>
        bool CancelDelete(ContributionInfoQuery cEntity);

        /// <summary>
        /// 获取稿件作者数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IDictionary<Int64, String> GetContributionAuthorDict(ContributionInfoQuery query);

    }
}






