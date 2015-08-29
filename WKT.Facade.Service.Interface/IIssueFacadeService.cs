using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Model;

namespace WKT.Facade.Service.Interface
{
    public interface IIssueFacadeService
    {
        #region 年卷设置
        /// <summary>
        /// 获取年卷设置分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>        
        Pager<YearVolumeEntity> GetYearVolumePageList(YearVolumeQuery query);

        /// <summary>
        /// 获取年卷设置列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        IList<YearVolumeEntity> GetYearVolumeList(YearVolumeQuery query);

        /// <summary>
        /// 获取年卷设置实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        YearVolumeEntity GetYearVolumeModel(YearVolumeQuery query);

        /// <summary>
        /// 保存年卷设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        ExecResult SaveYearVolume(YearVolumeEntity model);

        /// <summary>
        /// 删除年卷设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>      
        ExecResult DelYearVolume(YearVolumeQuery query);
        #endregion

        #region 期设置
        /// <summary>
        /// 获取期设置分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        Pager<IssueSetEntity> GetIssueSetPageList(IssueSetQuery query);

        /// <summary>
        /// 获取期设置列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>        
        IList<IssueSetEntity> GetIssueSetList(IssueSetQuery query);

        /// <summary>
        /// 获取期设置实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        IssueSetEntity GetIssueSetModel(IssueSetQuery query);

        /// <summary>
        /// 保存期设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        ExecResult SaveIssueSet(IssueSetEntity model);

        /// <summary>
        /// 删除期设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        ExecResult DelIssueSet(IssueSetQuery query);
        #endregion

        #region 期刊栏目
        /// <summary>
        /// 获取期刊栏目分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        Pager<JournalChannelEntity> GetJournalChannelPageList(JournalChannelQuery query);

        /// <summary>
        /// 获取期刊栏目列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        IList<JournalChannelEntity> GetJournalChannelList(JournalChannelQuery query);

        /// <summary>
        /// 获取期刊栏目实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>        
        JournalChannelEntity GetJournalChannelModel(JournalChannelQuery query);

        /// <summary>
        /// 保存期刊栏目
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>      
        ExecResult SaveJournalChannel(JournalChannelEntity model);

        /// <summary>
        /// 删除期刊栏目
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        ExecResult DelJournalChannel(JournalChannelQuery query);

        /// <summary>
        /// 获取期刊栏目数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<TreeModel> GetJournalChannelTreeList(JournalChannelQuery query);

        /// <summary>
        /// 根据期刊数据 按照期刊栏目数据分组 获取当前期刊数据所属的期刊栏目数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<JournalChannelEntity> GetJournalChannelListByIssueContent(JournalChannelQuery query);

        #endregion

        #region 期刊

        /// <summary>
        /// 设置录用稿件的年期
        /// </summary>
        /// <returns></returns>
        ExecResult SetContributionYearIssue(IssueContentQuery cEntity);

        /// <summary>
        /// 获取期刊分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        Pager<IssueContentEntity> GetIssueContentPageList(IssueContentQuery query);

        /// <summary>
        /// 获取期刊列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>        
        IList<IssueContentEntity> GetIssueContentList(IssueContentQuery query);

        /// <summary>
        /// 获取期刊实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        IssueContentEntity GetIssueContentModel(IssueContentQuery query);

        /// <summary>
        /// 保存期刊
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        ExecResult SaveIssueContent(IssueContentEntity model);

        /// <summary>
        /// 删除期刊
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        ExecResult DelIssueContent(IssueContentQuery query);

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

        /// <summary>
        /// 删除DOI注册日志
        /// </summary>
        /// <param name="loginErrorLogQuery"></param>
        /// <returns></returns>
        ExecResult DelDoiRegLog(long[] PKID);

        /// <summary>
        /// 获取DOI注册日志实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        DoiRegLogEntity GetDoiRegLog(DoiRegLogQuery query);

        /// <summary>
        /// 获取登录错误日志信息
        /// </summary>
        /// <param name="loginErrorLogQuery"></param>
        /// <returns></returns>
        IList<DoiRegLogEntity> GetDoiRegLogList(DoiRegLogQuery doiRegLogQuery);

        /// <summary>
        /// 获取DOI注册日志分页数据
        /// </summary>
        /// <param name="doiRegLogQuery"></param>
        /// <returns></returns>
        Pager<DoiRegLogEntity> GetDoiRegLogPageList(DoiRegLogQuery doiRegLogQuery);

        /// <summary>
        /// 保存DOI注册日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveDoiRegLog(DoiRegLogEntity model);

        #region 期刊订阅
        /// <summary>
        /// 获取期刊订阅分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>     
        Pager<IssueSubscribeEntity> GetIssueSubscribePageList(IssueSubscribeQuery query);

        /// <summary>
        /// 获取期刊订阅列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        IList<IssueSubscribeEntity> GetIssueSubscribeList(IssueSubscribeQuery query);

        /// <summary>
        /// 获取期刊订阅实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        IssueSubscribeEntity GetIssueSubscribeModel(IssueSubscribeQuery query);

        /// <summary>
        /// 保存期刊订阅
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        ExecResult SaveIssueSubscribe(IssueSubscribeEntity model);

        /// <summary>
        /// 删除期刊订阅
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>      
        ExecResult DelIssueSubscribe(IssueSubscribeQuery query);
        #endregion

        /// <summary>
        /// 获取当前期刊最新的年、卷、期、当期封面图片
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IssueSiteEntity GetCurIssueInfo(IssueSetQuery query);

        # region 访问下载日志

        /// <summary>
        /// 保存访问日志
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult SaveViewLog(IssueViewLogEntity model);

        /// <summary>
        /// 保存下载日志
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult SaveDownloadLog(IssueDownLogEntity model);

        # endregion

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
