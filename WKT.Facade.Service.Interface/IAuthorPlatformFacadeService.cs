using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Model;

namespace WKT.Facade.Service.Interface
{
    public interface IAuthorPlatformFacadeService
    {
        #region 作者详细信息
        /// <summary>
        /// 获取作者详细信息分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<AuthorDetailEntity> GetAuthorDetailPageList(AuthorDetailQuery query);

        /// <summary>
        /// 获取作者详细信息数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<AuthorDetailEntity> GetAuthorDetailList(AuthorDetailQuery query);

        /// <summary>
        /// 获取作者详细信息实体
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        AuthorDetailEntity GetAuthorDetailModel(AuthorDetailQuery query);

        /// <summary>
        /// 保存作者详细信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveAuthorDetail(AuthorDetailEntity model);

        /// <summary>
        /// 删除作者详细信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelAuthorDetail(AuthorDetailQuery query);

        /// <summary>
        /// 设置作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult SetAuthorExpert(AuthorDetailQuery query);

        /// <summary>
        /// 取消作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult CancelAuthorExpert(AuthorDetailQuery query);

        /// <summary>
        /// 设置作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<AuthorDetailEntity> GetExpertGroupMapList(ExpertGroupMapEntity query);

        /// <summary>
        /// 取消作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult SaveExpertGroupMap(ExpertGroupMapQuery query);
        #endregion

        #region 投稿相关
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

        /// <summary>
        /// 获取稿件实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ContributionInfoEntity GetContributionInfoModel(ContributionInfoQuery query);

        /// <summary>
        /// 投稿
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveContributionInfo(ContributionInfoEntity model);

        /// <summary>
        /// 保存稿件格式修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveContributionInfoFormat(ContributionInfoEntity model);

        /// <summary>
        /// 删除稿件数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelContributionInfo(ContributionInfoQuery query);

        /// <summary>
        /// 改变稿件状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult ChangeContributionInfoStatus(ContributionInfoQuery query);
        #endregion

        #region 撤稿相关
        /// <summary>
        /// 撤稿
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult DraftContributionInfo(RetractionsBillsEntity model);

        /// <summary>
        /// 新增撤稿表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddRetractionsBills(RetractionsBillsEntity model);

        /// <summary>
        /// 编辑撤稿表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateRetractionsBills(RetractionsBillsEntity model);

        /// <summary>
        /// 获取撤稿信息实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        RetractionsBillsEntity GetRetractionsBillsModel(RetractionsBillsQuery query);
        #endregion

        #region 稿件备注相关
        /// <summary>
        /// 保存稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveCRemark(CRemarkEntity model);

        /// <summary>
        /// 获取稿件备注实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        CRemarkEntity GetCRemarkModel(CRemarkQuery query);
        #endregion

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
    }
}
