using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public interface IIssueBusiness
    {
        #region 年卷设置
        /// <summary>
        /// 新增年卷
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        bool AddYearVolume(YearVolumeEntity yearVolumeEntity);

        /// <summary>
        /// 编辑年卷
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        bool UpdateYearVolume(YearVolumeEntity yearVolumeEntity);

        /// <summary>
        /// 年是否存在
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        bool YearVolumeYearIsExists(YearVolumeEntity yearVolumeEntity);

        /// <summary>
        /// 卷是否存在
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        bool YearVolumeVolumeIsExists(YearVolumeEntity yearVolumeEntity);

        /// <summary>
        /// 获取最新的年卷
        /// </summary>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        YearVolumeEntity GetMaxYearVolume(Int64 JournalID);

        /// <summary>
        /// 获取年卷试实体
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        YearVolumeEntity GetYearVolume(Int64 setID);

        /// <summary>
        /// 删除年卷
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        IList<Int64> DelYearVolume(Int64[] setID);

        /// <summary>
        /// 获取年卷分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<YearVolumeEntity> GetYearVolumePageList(YearVolumeQuery query);

        /// <summary>
        /// 获取年卷数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<YearVolumeEntity> GetYearVolumeList(YearVolumeQuery query);
        #endregion

        #region 期数设置
        /// <summary>
        /// 新增期设置
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        bool AddIssueSet(IssueSetEntity issueSetEntity);

        /// <summary>
        /// 编辑期设置
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        bool UpdateIssueSet(IssueSetEntity issueSetEntity);

        /// <summary>
        /// 期设置是否存在
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        bool IssueSetIsExists(IssueSetEntity issueSetEntity);

        /// <summary>
        /// 获取最新的期设置
        /// </summary>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        IssueSetEntity GetMaxIssueSet(Int64 JournalID);

        /// <summary>
        /// 获取期设置
        /// </summary>
        /// <param name="IssueSetID"></param>
        /// <returns></returns>
        IssueSetEntity GetIssueSet(Int64 IssueSetID);

        IssueSetEntity GetJournalOfCostByYearAndIssue(int year, int issue);

        /// <summary>
        /// 删除期设置
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        IList<Int64> DelIssueSet(Int64[] IssueSetID);

        /// <summary>
        /// 获取期设置分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<IssueSetEntity> GetIssueSetPageList(IssueSetQuery query);

        /// <summary>
        /// 获取期设置数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<IssueSetEntity> GetIssueSetList(IssueSetQuery query);
        #endregion

        #region 期刊栏目
        /// <summary>
        /// 新增期刊栏目
        /// </summary>
        /// <param name="journalChannelEntity"></param>
        /// <returns></returns>
        bool AddJournalChannel(JournalChannelEntity journalChannelEntity);

        /// <summary>
        /// 编辑期刊栏目
        /// </summary>
        /// <param name="journalChannelEntity"></param>
        /// <returns></returns>
        bool UpdateJournalChannel(JournalChannelEntity journalChannelEntity);

        /// <summary>
        /// 获取期刊栏目
        /// </summary>
        /// <param name="jChannelID"></param>
        /// <returns></returns>
        JournalChannelEntity GetJournalChannel(Int64 jChannelID);

        /// <summary>
        /// 删除期刊栏目设置
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        bool DelJournalChannel(Int64 jChannelID);

        /// <summary>
        /// 期刊栏目前需要做的判断
        /// </summary>
        /// <param name="jChannelID"></param>
        /// <param name="JournalID"></param>
        /// <param name="type">0:是否存在下级 1:是否存在期刊表中 2:是否存在稿件表中</param>
        /// <returns>true:不能删除</returns>
        bool DelJournalChannelJudge(Int64 jChannelID, Int64 JournalID, Int32 type);

        /// <summary>
        /// 获取期刊栏目分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<JournalChannelEntity> GetJournalChannelPageList(JournalChannelQuery query);

        /// <summary>
        /// 获取期刊栏目数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<JournalChannelEntity> GetJournalChannelList(JournalChannelQuery query);


        /// <summary>
        /// 根据期刊数据 按照期刊栏目数据分组 获取当前期刊数据所属的期刊栏目数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<JournalChannelEntity> GetJournalChannelListByIssueContent(JournalChannelQuery query);
        #endregion

        #region 期刊内容

        /// <summary>
        /// 设置录用稿件的年期
        /// </summary>
        /// <returns></returns>
        bool SetContributionYearIssue(IssueContentQuery cEntity);

        /// <summary>
        /// 保存期刊信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Int64 SaveIssueContent(IssueContentEntity model);

        /// <summary>
        /// 新增期刊表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddIssueContent(IssueContentEntity model);

        /// <summary>
        /// 编辑期刊表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateIssueContent(IssueContentEntity model);

        /// <summary>
        /// 获取期刊实体
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        IssueContentEntity GetIssueContent(IssueContentQuery query);

        /// <summary>
        /// 删除期刊信息
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        bool DelIssueContent(Int64[] contentID);

        /// <summary>
        /// 获取期刊分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<IssueContentEntity> GetIssueContentPageList(IssueContentQuery query);

        /// <summary>
        /// 获取期刊数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<IssueContentEntity> GetIssueContentList(IssueContentQuery query);

        /// <summary>
        /// 更新期刊内容浏览次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateIssueContentHits(IssueContentQuery model);

        /// <summary>
        /// 更新期刊内容浏览次数(RichHTML)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateIssueContentHtmlHits(IssueContentQuery model);

	    /// <summary>
        /// 更新期刊内容下载次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateIssueContentDownloads(IssueContentQuery model);

        #endregion

        #region DOI注册日志
        /// <summary>
        /// 新增DOI注册日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddDoiRegLog(DoiRegLogEntity model);

        /// <summary>
        /// 编辑DOI注册日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateDoiRegLog(DoiRegLogEntity model);

        /// <summary>
        /// 删除DOI注册日志
        /// </summary>
        /// <param name="PKID"></param>
        /// <returns></returns>
        bool DelDoiRegLog(Int64[] PKID);

        /// <summary>
        /// 获取DOI注册日志实体
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        DoiRegLogEntity GetDoiRegLog(DoiRegLogQuery query);

        
        /// <summary>
        /// 获取DOI注册日志分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<DoiRegLogEntity> GetDoiRegLogPageList(DoiRegLogQuery query);

        /// <summary>
        /// 获取DOI注册日志列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<DoiRegLogEntity> GetDoiRegLogList(DoiRegLogQuery query); 
        #endregion

        #region 期刊参考文献
        /// <summary>
        /// 新增期刊参考文献
        /// </summary>
        /// <param name="issueReferenceEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        bool AddIssueReference(IssueReferenceEntity issueReferenceEntity);

        /// <summary>
        /// 编辑期刊参考文献
        /// </summary>
        /// <param name="issueReferenceEntity"></param>
        /// <returns></returns>
        bool UpdateIssueReference(IssueReferenceEntity issueReferenceEntity);

        /// <summary>
        /// 获取参考文献实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        IssueReferenceEntity GetIssueReference(Int64 ReferenceID);

        /// <summary>
        /// 根据期刊编号删除参考文献
        /// </summary>
        /// <param name="ContentID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        bool DelIssueReferenceByContentID(Int64[] ContentID);

        /// <summary>
        /// 删除期刊参考文献
        /// </summary>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        bool DelIssueReference(Int64[] ReferenceID);

        /// <summary>
        /// 获取参考文献分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<IssueReferenceEntity> GetIssueReferencePageList(IssueReferenceQuery query);

        /// <summary>
        /// 获取参考文献数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<IssueReferenceEntity> GetIssueReferenceList(IssueReferenceQuery query);
        #endregion

        #region 期刊订阅
        /// <summary>
        /// 新增期刊订阅
        /// </summary>
        /// <param name="issueSubscribeEntity"></param>
        /// <returns></returns>
        bool AddIssueSubscribe(IssueSubscribeEntity issueSubscribeEntity);

        /// <summary>
        /// 编辑期刊订阅
        /// </summary>
        /// <param name="issueSubscribeEntity"></param>
        /// <returns></returns>
        bool UpdateIssueSubscribe(IssueSubscribeEntity issueSubscribeEntity);

        /// <summary>
        /// 获取期刊订阅
        /// </summary>
        /// <param name="subscribeID"></param>
        /// <returns></returns>
        IssueSubscribeEntity GetIssueSubscribe(Int64 subscribeID);

        /// <summary>
        /// 删除期刊参考文献
        /// </summary>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        bool DelIssueSubscribe(Int64[] subscribeID);

        /// <summary>
        /// 获取期刊订阅分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<IssueSubscribeEntity> GetIssueSubscribePageList(IssueSubscribeQuery query);

        /// <summary>
        /// 获取期刊订阅数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<IssueSubscribeEntity> GetIssueSubscribeList(IssueSubscribeQuery query);
        #endregion

        /// <summary>
        /// 获取当前期刊最新的年、卷、期、当期封面图片
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IssueSiteEntity GetCurIssueInfo(IssueSetQuery query);

        #region 下载次数统计
        /// <summary>
        /// 获取期刊下载次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<IssueDownLogEntity> GetIssueDownLogPageList(IssueDownLogQuery query);

        /// <summary>
        /// 获取期刊下载明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<IssueDownLogEntity> GetIssueDownLogDetailPageList(IssueDownLogQuery query);
        #endregion

        #region 浏览次数统计
        /// <summary>
        /// 获取期刊浏览次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<IssueViewLogEntity> GetIssueViewLogPageList(IssueViewLogQuery query);

        /// <summary>
        /// 获取期刊浏览明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<IssueViewLogEntity> GetIssueViewLogDetailPageList(IssueViewLogQuery query);
        #endregion

    }
}
