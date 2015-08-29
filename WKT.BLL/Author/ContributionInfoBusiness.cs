using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class ContributionInfoBusiness : IContributionInfoBusiness
    {
        #region 稿件信息

        /// <summary>
        /// 获取稿件的附件
        /// </summary>
        /// <param name="cQuery">稿件ID</param>
        /// <returns></returns>
        public string GetContributionAttachment(ContributionInfoQuery cQuery)
        {
            return ContributionInfoDataAccess.Instance.GetContributionAttachment(cQuery);
        }

        /// <summary>
        /// 投稿
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Int64 AuthorPlatform(ContributionInfoEntity model)
        {
            return ContributionInfoDataAccess.Instance.AuthorPlatform(model);
        }

        /// <summary>
        /// 保存稿件格式修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveFormat(ContributionInfoEntity model)
        {
            return ContributionInfoDataAccess.Instance.SaveFormat(model);
        }

        /// <summary>
        /// 新增稿件表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddContributionInfo(ContributionInfoEntity model)
        {
            return ContributionInfoDataAccess.Instance.AddContributionInfo(model);
        }

        /// <summary>
        /// 编辑稿件表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateContributionInfo(ContributionInfoEntity model)
        {
            return ContributionInfoDataAccess.Instance.UpdateContributionInfo(model);
        }

        /// <summary>
        /// 获取稿件信息实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionInfoEntity GetContributionInfo(Int64 cID)
        {
            return ContributionInfoDataAccess.Instance.GetContributionInfo(cID);
        }

        /// <summary>
        /// 判断稿件标题是否存在
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="CID"></param>
        /// <param name="CTitle"></param>
        /// <returns></returns>
        public bool ContributionTitleIsExists(Int64 JournalID, Int64 CID, string CTitle)
        {
            return ContributionInfoDataAccess.Instance.ContributionTitleIsExists(JournalID, CID, CTitle);
        }

        /// <summary>
        /// 改变稿件状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool ChangeStatus(ContributionInfoQuery query)
        {
            return ContributionInfoDataAccess.Instance.ChangeStatus(query);
        }

        /// <summary>
        /// 删除投稿信息
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public bool DelContributionInfo(Int64[] CID)
        {
            return ContributionInfoDataAccess.Instance.DelContributionInfo(CID);
        }

        /// <summary>
        /// 获取稿件分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionInfoEntity> GetContributionInfoPageList(ContributionInfoQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetContributionInfoPageList(query);
        }

        /// <summary>
        /// 获取稿件数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionInfoEntity> GetContributionInfoList(ContributionInfoQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetContributionInfoList(query);
        }
        #endregion

        #region 稿件作者信息
        /// <summary>
        /// 新增稿件作者信息
        /// </summary>
        /// <param name="contributionAuthorEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool AddContributionAuthor(ContributionAuthorEntity contributionAuthorEntity)
        {
            return ContributionInfoDataAccess.Instance.AddContributionAuthor(contributionAuthorEntity);
        }

        /// <summary>
        /// 编辑稿件作者信息
        /// </summary>
        /// <param name="contributionAuthorEntity"></param>      
        /// <returns></returns>
        public bool UpdateContributionAuthor(ContributionAuthorEntity contributionAuthorEntity)
        {
            return ContributionInfoDataAccess.Instance.UpdateContributionAuthor(contributionAuthorEntity);
        }

        /// <summary>
        /// 获取稿件作者实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionAuthorEntity GetContributionAuthor(Int64 CAuthorID)
        {
            return ContributionInfoDataAccess.Instance.GetContributionAuthor(CAuthorID);
        }

        /// <summary>
        /// 根据稿件编号删除稿件作者信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelContributionAuthorByCID(Int64[] CID)
        {
            return ContributionInfoDataAccess.Instance.DelContributionAuthorByCID(CID);
        }

        /// <summary>
        /// 删除稿件作者信息
        /// </summary>
        /// <param name="CAuthorID"></param>
        /// <returns></returns>
        public bool DelContributionAuthor(Int64[] CAuthorID)
        {
            return ContributionInfoDataAccess.Instance.DelContributionAuthor(CAuthorID);
        }

        /// <summary>
        /// 获取稿件作者分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionAuthorEntity> GetContributionAuthorPageList(ContributionAuthorQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetContributionAuthorPageList(query);
        }

        /// <summary>
        /// 获取稿件作者数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionAuthorEntity> GetContributionAuthorList(ContributionAuthorQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetContributionAuthorList(query);
        }
        #endregion

        #region 参考文献信息
        /// <summary>
        /// 新增参考文献
        /// </summary>
        /// <param name="contributionReferenceEntity"></param>
        /// <returns></returns>
        public bool AddContributionReference(ContributionReferenceEntity contributionReferenceEntity)
        {
            return ContributionInfoDataAccess.Instance.AddContributionReference(contributionReferenceEntity);
        }

        /// <summary>
        /// 编辑参考文献
        /// </summary>
        /// <param name="contributionReferenceEntity"></param>
        /// <returns></returns>
        public bool UpdateContributionReference(ContributionReferenceEntity contributionReferenceEntity)
        {
            return ContributionInfoDataAccess.Instance.UpdateContributionReference(contributionReferenceEntity);
        }

        /// <summary>
        /// 获取参考文献实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionReferenceEntity GetContributionReference(Int64 ReferenceID)
        {
            return ContributionInfoDataAccess.Instance.GetContributionReference(ReferenceID);
        }

        /// <summary>
        /// 根据稿件编号删除参考文献
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelContributionReferenceByCID(Int64[] CID)
        {
            return ContributionInfoDataAccess.Instance.DelContributionReferenceByCID(CID);
        }

        /// <summary>
        /// 删除参考文献
        /// </summary>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        public bool DelContributionReference(Int64[] ReferenceID)
        {
            return ContributionInfoDataAccess.Instance.DelContributionReference(ReferenceID);
        }

        /// <summary>
        /// 获取参考文献分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionReferenceEntity> GetContributionReferencePageList(ContributionReferenceQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetContributionReferencePageList(query);
        }

        /// <summary>
        /// 获取参考文献数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionReferenceEntity> GetContributionReferenceList(ContributionReferenceQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetContributionReferenceList(query);
        }
        #endregion

        #region 基金信息
        /// <summary>
        /// 新增基金信息
        /// </summary>
        /// <param name="contributionFundEntity"></param>
        /// <returns></returns>
        public bool AddContributionFund(ContributionFundEntity contributionFundEntity)
        {
            return ContributionInfoDataAccess.Instance.AddContributionFund(contributionFundEntity);
        }

        /// <summary>
        /// 编辑基金信息
        /// </summary>
        /// <param name="contributionFundEntity"></param>
        /// <returns></returns>
        public bool UpdateContributionFund(ContributionFundEntity contributionFundEntity)
        {
            return ContributionInfoDataAccess.Instance.UpdateContributionFund(contributionFundEntity);
        }

        /// <summary>
        /// 获取基金信息实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionFundEntity GetContributionFund(Int64 FundID)
        {
            return ContributionInfoDataAccess.Instance.GetContributionFund(FundID);
        }

        /// <summary>
        /// 根据稿件编号删除基金信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelContributionFundByCID(Int64[] CID)
        {
            return ContributionInfoDataAccess.Instance.DelContributionFundByCID(CID);
        }

        /// <summary>
        /// 删除基金信息
        /// </summary>
        /// <param name="FundID"></param>
        /// <returns></returns>
        public bool DelContributionFund(Int64[] FundID)
        {
            return ContributionInfoDataAccess.Instance.DelContributionFund(FundID);
        }

        /// <summary>
        /// 获取基金信息分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionFundEntity> GetContributionFundPageList(ContributionFundQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetContributionFundPageList(query);
        }

        /// <summary>
        /// 获取基金信息数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionFundEntity> GetContributionFundList(ContributionFundQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetContributionFundList(query);
        }
        #endregion

        #region 大字段信息
        /// <summary>
        /// 获取大字段信息实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionInfoAttEntity GetContributionInfoAtt(Int64 PKID)
        {
            return ContributionInfoDataAccess.Instance.GetContributionInfoAtt(PKID);
        }

        /// <summary>
        /// 获取大字段信息实体
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ContributionInfoAttEntity GetContributionInfoAttByCID(Int64 CID)
        {
            return ContributionInfoDataAccess.Instance.GetContributionInfoAttByCID(CID);
        }

        /// <summary>
        /// 根据稿件编号删除大字段信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelContributionInfoAttByCID(Int64[] CID)
        {
            return ContributionInfoDataAccess.Instance.DelContributionInfoAttByCID(CID);
        }

        /// <summary>
        /// 删除大字段信息
        /// </summary>
        /// <param name="FundID"></param>
        /// <returns></returns>
        public bool DelContributionInfoAtt(Int64[] PKID)
        {
            return ContributionInfoDataAccess.Instance.DelContributionInfoAtt(PKID);
        }
        #endregion

        #region 撤稿相关
        /// <summary>
        /// 撤稿
        /// </summary>
        /// <param name="retractionsBillsEntity"></param>
        /// <returns></returns>
        public bool DraftContribution(RetractionsBillsEntity model)
        {
            return ContributionInfoDataAccess.Instance.DraftContribution(model);
        }

        /// <summary>
        /// 添加撤稿信息
        /// </summary>
        /// <param name="retractionsBillsEntity"></param>
        /// <returns></returns>
        public bool AddRetractionsBills(RetractionsBillsEntity model)
        {
            return ContributionInfoDataAccess.Instance.AddRetractionsBills(model);
        }

        /// <summary>
        /// 编辑撤稿信息
        /// </summary>
        /// <param name="retractionsBillsEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool UpdateRetractionsBills(RetractionsBillsEntity model)
        {
            return ContributionInfoDataAccess.Instance.UpdateRetractionsBills(model);
        }

        /// <summary>
        /// 获取撤稿信息
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns></returns>
        public RetractionsBillsEntity GetRetractionsBills(RetractionsBillsQuery rQuery)
        {
            return ContributionInfoDataAccess.Instance.GetRetractionsBills(rQuery);
        }
        #endregion

        #region 添加稿件备注
        /// <summary>
        /// 新增稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddCRemark(CRemarkEntity model)
        {
            return ContributionInfoDataAccess.Instance.AddCRemark(model);
        }

        /// <summary>
        /// 编辑稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCRemark(CRemarkEntity model)
        {
            return ContributionInfoDataAccess.Instance.UpdateCRemark(model);
        }

        /// <summary>
        /// 获取稿件备注
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public CRemarkEntity GetCRemark(CRemarkQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetCRemark(query);
        }
        #endregion

        # region 更新稿件标记及状态 by sxd

        /// <summary>
        /// 更新稿件旗帜标记
        /// </summary>        
        /// <param name="cEntityList">稿件信息</param>
        /// <returns></returns>
        public bool BatchUpdateContributionFlag(List<ContributionInfoQuery> cEntityList)
        {
            return ContributionInfoDataAccess.Instance.BatchUpdateContributionFlag(cEntityList);
        }

        /// <summary>
        /// 更新稿件旗帜标记
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool UpdateContributionFlag(ContributionInfoQuery cEntity)
        {
            return ContributionInfoDataAccess.Instance.UpdateContributionFlag(cEntity);
        }

        /// <summary>
        /// 更新稿件加急标记
        /// </summary>        
        /// <param name="cEntityList">稿件信息</param>
        /// <returns></returns>
        public bool BatchUpdateContributionIsQuick(List<ContributionInfoQuery> cEntityList)
        {
            return ContributionInfoDataAccess.Instance.BatchUpdateContributionIsQuick(cEntityList);
        }

        /// <summary>
        /// 更新稿件加急标记
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool UpdateContributionIsQuick(ContributionInfoQuery cEntity)
        {
            return ContributionInfoDataAccess.Instance.UpdateContributionIsQuick(cEntity);
        }

        /// <summary>
        /// 更新稿件状态及审核状态
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool UpdateContributionStatus(ContributionInfoQuery cEntity)
        {
            return ContributionInfoDataAccess.Instance.UpdateContributionStatus(cEntity);
        }

        # endregion

        #region 收稿量统计
        /// <summary>
        /// 按年月统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionAccountEntity> GetContributionAccountListByYear(ContributionAccountQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetContributionAccountListByYear(query);
        }

        /// <summary>
        /// 按基金级别统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionAccountEntity> GetContributionAccountListByFund(ContributionAccountQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetContributionAccountListByFund(query);
        }

        /// <summary>
        /// 按作者统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionAccountEntity> GetContributionAccountListByAuhor(ContributionAccountQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetContributionAccountListByAuhor(query);
        }
        #endregion

        /// <summary>
        /// 更新介绍信标记
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIntroLetter(ContributionInfoQuery model)
        {
            return ContributionInfoDataAccess.Instance.UpdateIntroLetter(model);
        }

        /// <summary>
        /// 处理撤稿申请
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool DealWithdrawal(ContributionInfoQuery cEntity)
        {
            return ContributionInfoDataAccess.Instance.DealWithdrawal(cEntity);
        }
        /// <summary>
        /// 撤销删除
        /// </summary>
        /// <param name="cEntity"></param>
        /// <returns></returns>
        public bool CancelDelete(ContributionInfoQuery cEntity)
        {
            return ContributionInfoDataAccess.Instance.CancelDelete(cEntity);
        }
        /// <summary>
        /// 获取稿件作者数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<Int64, String> GetContributionAuthorDict(ContributionInfoQuery query)
        {
            return ContributionInfoDataAccess.Instance.GetContributionAuthorDict(query);
        }


    }
}
