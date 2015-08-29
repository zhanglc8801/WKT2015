using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public class IssueBusiness:IIssueBusiness
    {
        #region 年卷设置
        /// <summary>
        /// 新增年卷
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        public bool AddYearVolume(YearVolumeEntity yearVolumeEntity)
        {
            return IssueDataAccess.Instance.AddYearVolume(yearVolumeEntity);
        }

        /// <summary>
        /// 编辑年卷
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        public bool UpdateYearVolume(YearVolumeEntity yearVolumeEntity)
        {
            return IssueDataAccess.Instance.UpdateYearVolume(yearVolumeEntity);
        }

        /// <summary>
        /// 年是否存在
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        public bool YearVolumeYearIsExists(YearVolumeEntity yearVolumeEntity)
        {
            return IssueDataAccess.Instance.YearVolumeYearIsExists(yearVolumeEntity);
        }

        /// <summary>
        /// 卷是否存在
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        public bool YearVolumeVolumeIsExists(YearVolumeEntity yearVolumeEntity)
        {
            return IssueDataAccess.Instance.YearVolumeVolumeIsExists(yearVolumeEntity);
        }

        /// <summary>
        /// 获取最新的年卷
        /// </summary>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        public YearVolumeEntity GetMaxYearVolume(Int64 JournalID)
        {
            return IssueDataAccess.Instance.GetMaxYearVolume(JournalID);
        }

        /// <summary>
        /// 获取年卷试实体
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        public YearVolumeEntity GetYearVolume(Int64 setID)
        {
            return IssueDataAccess.Instance.GetYearVolume(setID);
        }

        /// <summary>
        /// 删除年卷
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        public IList<Int64> DelYearVolume(Int64[] setID)
        {
            return IssueDataAccess.Instance.DelYearVolume(setID);
        }

        /// <summary>
        /// 获取年卷分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<YearVolumeEntity> GetYearVolumePageList(YearVolumeQuery query)
        {
            return IssueDataAccess.Instance.GetYearVolumePageList(query);
        }

        /// <summary>
        /// 获取年卷数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<YearVolumeEntity> GetYearVolumeList(YearVolumeQuery query)
        {
            return IssueDataAccess.Instance.GetYearVolumeList(query);
        }
        #endregion

        #region 期数设置
        /// <summary>
        /// 新增期设置
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        public bool AddIssueSet(IssueSetEntity issueSetEntity)
        {
            return IssueDataAccess.Instance.AddIssueSet(issueSetEntity);
        }

        /// <summary>
        /// 编辑期设置
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        public bool UpdateIssueSet(IssueSetEntity issueSetEntity)
        {
            return IssueDataAccess.Instance.UpdateIssueSet(issueSetEntity);
        }

        /// <summary>
        /// 期设置是否存在
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        public bool IssueSetIsExists(IssueSetEntity issueSetEntity)
        {
            return IssueDataAccess.Instance.IssueSetIsExists(issueSetEntity);
        }

        /// <summary>
        /// 获取最新的期设置
        /// </summary>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        public IssueSetEntity GetMaxIssueSet(Int64 JournalID)
        {
            return IssueDataAccess.Instance.GetMaxIssueSet(JournalID);
        }

        /// <summary>
        /// 获取期设置
        /// </summary>
        /// <param name="IssueSetID"></param>
        /// <returns></returns>
        public IssueSetEntity GetIssueSet(Int64 IssueSetID)
        {
            return IssueDataAccess.Instance.GetIssueSet(IssueSetID);
        }


        public IssueSetEntity GetJournalOfCostByYearAndIssue(int year, int issue)
        {
            return IssueDataAccess.Instance.GetJournalOfCostByYearAndIssue(year, issue);
        }
        /// <summary>
        /// 删除期设置
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        public IList<Int64> DelIssueSet(Int64[] IssueSetID)
        {
            return IssueDataAccess.Instance.DelIssueSet(IssueSetID);
        }

        /// <summary>
        /// 获取期设置分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueSetEntity> GetIssueSetPageList(IssueSetQuery query)
        {
            return IssueDataAccess.Instance.GetIssueSetPageList(query);
        }

        /// <summary>
        /// 获取期设置数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<IssueSetEntity> GetIssueSetList(IssueSetQuery query)
        {
            return IssueDataAccess.Instance.GetIssueSetList(query);
        }
        #endregion

        #region 期刊栏目
        /// <summary>
        /// 新增期刊栏目
        /// </summary>
        /// <param name="journalChannelEntity"></param>
        /// <returns></returns>
        public bool AddJournalChannel(JournalChannelEntity journalChannelEntity)
        {
            return IssueDataAccess.Instance.AddJournalChannel(journalChannelEntity);
        }

        /// <summary>
        /// 编辑期刊栏目
        /// </summary>
        /// <param name="journalChannelEntity"></param>
        /// <returns></returns>
        public bool UpdateJournalChannel(JournalChannelEntity journalChannelEntity)
        {
            return IssueDataAccess.Instance.UpdateJournalChannel(journalChannelEntity);
        }

        /// <summary>
        /// 获取期刊栏目
        /// </summary>
        /// <param name="jChannelID"></param>
        /// <returns></returns>
        public JournalChannelEntity GetJournalChannel(Int64 jChannelID)
        {
            return IssueDataAccess.Instance.GetJournalChannel(jChannelID);
        }

        /// <summary>
        /// 删除期刊栏目设置
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        public bool DelJournalChannel(Int64 jChannelID)
        {
            return IssueDataAccess.Instance.DelJournalChannel(jChannelID);
        }

        /// <summary>
        /// 期刊栏目前需要做的判断
        /// </summary>
        /// <param name="jChannelID"></param>
        /// <param name="JournalID"></param>
        /// <param name="type">0:是否存在下级 1:是否存在期刊表中 2:是否存在稿件表中</param>
        /// <returns>true:不能删除</returns>
        public bool DelJournalChannelJudge(Int64 jChannelID, Int64 JournalID, Int32 type)
        {
            return IssueDataAccess.Instance.DelJournalChannelJudge(jChannelID, JournalID, type);
        }

        /// <summary>
        /// 获取期刊栏目分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<JournalChannelEntity> GetJournalChannelPageList(JournalChannelQuery query)
        {
            return IssueDataAccess.Instance.GetJournalChannelPageList(query);
        }

        /// <summary>
        /// 获取期刊栏目数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<JournalChannelEntity> GetJournalChannelList(JournalChannelQuery query)
        {
            return IssueDataAccess.Instance.GetJournalChannelList(query);
        }

        /// <summary>
        /// 根据期刊数据 按照期刊栏目数据分组 获取当前期刊数据所属的期刊栏目数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public  IList<JournalChannelEntity> GetJournalChannelListByIssueContent(JournalChannelQuery query)
        {
            return IssueDataAccess.Instance.GetJournalChannelListByIssueContent(query);
        }
        #endregion

        #region 期刊内容

        /// <summary>
        /// 设置录用稿件的年期
        /// </summary>
        /// <returns></returns>
        public bool SetContributionYearIssue(IssueContentQuery cEntity)
        {
            return IssueDataAccess.Instance.SetContributionYearIssue(cEntity);
        }

        /// <summary>
        /// 保存期刊信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Int64 SaveIssueContent(IssueContentEntity model)
        {
            return IssueDataAccess.Instance.SaveIssueContent(model);
        }

        /// <summary>
        /// 新增期刊表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddIssueContent(IssueContentEntity model)
        {
            return IssueDataAccess.Instance.AddIssueContent(model);
        }

        /// <summary>
        /// 编辑期刊表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContent(IssueContentEntity model)
        {
            return IssueDataAccess.Instance.UpdateIssueContent(model);
        }

        /// <summary>
        /// 获取期刊实体
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        public IssueContentEntity GetIssueContent(IssueContentQuery query)
        {
            return IssueDataAccess.Instance.GetIssueContent(query);
        }

        /// <summary>
        /// 删除期刊信息
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public bool DelIssueContent(Int64[] contentID)
        {
            return IssueDataAccess.Instance.DelIssueContent(contentID);
        }

        /// <summary>
        /// 获取期刊分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueContentEntity> GetIssueContentPageList(IssueContentQuery query)
        {
            return IssueDataAccess.Instance.GetIssueContentPageList(query);
        }
        

        /// <summary>
        /// 获取期刊数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<IssueContentEntity> GetIssueContentList(IssueContentQuery query)
        {
            return IssueDataAccess.Instance.GetIssueContentList(query);
        }

        /// <summary>
        /// 更新期刊内容浏览次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContentHits(IssueContentQuery model)
        {
            return IssueDataAccess.Instance.UpdateIssueContentHits(model);
        }

        /// <summary>
        /// 更新期刊内容浏览次数(RichHTML)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContentHtmlHits(IssueContentQuery model)
        {
            return IssueDataAccess.Instance.UpdateIssueContentHtmlHits(model);
        }

	    /// <summary>
        /// 更新期刊内容下载次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContentDownloads(IssueContentQuery model)
        {
            return IssueDataAccess.Instance.UpdateIssueContentDownloads(model);
        }

        #endregion

        #region DOI注册
        //新增DOI注册日志
        public bool AddDoiRegLog(DoiRegLogEntity model)
        {
            return IssueDataAccess.Instance.AddDoiRegLog(model);
        }
        /// <summary>
        /// 编辑DOI注册日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateDoiRegLog(DoiRegLogEntity model)
        {
            return IssueDataAccess.Instance.UpdateDoiRegLog(model);
        }

        //删除DOI注册日志
        public bool DelDoiRegLog(long[] PKID)
        {
            return IssueDataAccess.Instance.DelDoiRegLog(PKID);
        }

        //获取DOI注册日志实体
        public DoiRegLogEntity GetDoiRegLog(DoiRegLogQuery query)
        {
            return IssueDataAccess.Instance.GetDoiRegLog(query);
        }
        //获取DOI注册日志分页数据
        public Pager<DoiRegLogEntity> GetDoiRegLogPageList(DoiRegLogQuery query)
        {
            return IssueDataAccess.Instance.GetDoiRegLogPageList(query);
        }
        //获取DOI注册日志列表
        public IList<DoiRegLogEntity> GetDoiRegLogList(DoiRegLogQuery query)
        {
            return IssueDataAccess.Instance.GetDoiRegLogList(query);
        } 
        #endregion

        #region 期刊参考文献
        /// <summary>
        /// 新增期刊参考文献
        /// </summary>
        /// <param name="issueReferenceEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool AddIssueReference(IssueReferenceEntity issueReferenceEntity)
        {
            return IssueDataAccess.Instance.AddIssueReference(issueReferenceEntity);
        }

        /// <summary>
        /// 编辑期刊参考文献
        /// </summary>
        /// <param name="issueReferenceEntity"></param>
        /// <returns></returns>
        public bool UpdateIssueReference(IssueReferenceEntity issueReferenceEntity)
        {
            return IssueDataAccess.Instance.UpdateIssueReference(issueReferenceEntity);
        }

        /// <summary>
        /// 获取参考文献实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public IssueReferenceEntity GetIssueReference(Int64 ReferenceID)
        {
            return IssueDataAccess.Instance.GetIssueReference(ReferenceID);
        }

        /// <summary>
        /// 根据期刊编号删除参考文献
        /// </summary>
        /// <param name="ContentID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelIssueReferenceByContentID(Int64[] ContentID)
        {
            return IssueDataAccess.Instance.DelIssueReferenceByContentID(ContentID);
        }

        /// <summary>
        /// 删除期刊参考文献
        /// </summary>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        public bool DelIssueReference(Int64[] ReferenceID)
        {
            return IssueDataAccess.Instance.DelIssueReference(ReferenceID);
        }

        /// <summary>
        /// 获取参考文献分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueReferenceEntity> GetIssueReferencePageList(IssueReferenceQuery query)
        {
            return IssueDataAccess.Instance.GetIssueReferencePageList(query);
        }

        /// <summary>
        /// 获取参考文献数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<IssueReferenceEntity> GetIssueReferenceList(IssueReferenceQuery query)
        {
            return IssueDataAccess.Instance.GetIssueReferenceList(query);
        }
        #endregion

        #region 期刊订阅

        /// <summary>
        /// 新增期刊订阅
        /// </summary>
        /// <param name="issueSubscribeEntity"></param>
        /// <returns></returns>
        public bool AddIssueSubscribe(IssueSubscribeEntity issueSubscribeEntity)
        {
            return IssueDataAccess.Instance.AddIssueSubscribe(issueSubscribeEntity);
        }

        /// <summary>
        /// 编辑期刊订阅
        /// </summary>
        /// <param name="issueSubscribeEntity"></param>
        /// <returns></returns>
        public bool UpdateIssueSubscribe(IssueSubscribeEntity issueSubscribeEntity)
        {
            return IssueDataAccess.Instance.UpdateIssueSubscribe(issueSubscribeEntity);
        }

        /// <summary>
        /// 获取期刊订阅
        /// </summary>
        /// <param name="subscribeID"></param>
        /// <returns></returns>
        public IssueSubscribeEntity GetIssueSubscribe(Int64 subscribeID)
        {
            return IssueDataAccess.Instance.GetIssueSubscribe(subscribeID);
        }

        /// <summary>
        /// 删除期刊参考文献
        /// </summary>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        public bool DelIssueSubscribe(Int64[] subscribeID)
        {
            return IssueDataAccess.Instance.DelIssueSubscribe(subscribeID);
        }

        /// <summary>
        /// 获取期刊订阅分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueSubscribeEntity> GetIssueSubscribePageList(IssueSubscribeQuery query)
        {
            return IssueDataAccess.Instance.GetIssueSubscribePageList(query);
        }

        /// <summary>
        /// 获取期刊订阅数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<IssueSubscribeEntity> GetIssueSubscribeList(IssueSubscribeQuery query)
        {
            return IssueDataAccess.Instance.GetIssueSubscribeList(query);
        }
        #endregion

        /// <summary>
        /// 获取当前期刊最新的年、卷、期、当期封面图片
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IssueSiteEntity GetCurIssueInfo(IssueSetQuery query)
        {
            return IssueDataAccess.Instance.GetCurIssueInfo(query);
        }

        #region 下载次数统计
        /// <summary>
        /// 获取期刊下载次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(IssueDownLogQuery query)
        {
            return IssueDataAccess.Instance.GetIssueDownLogPageList(query);
        }

        /// <summary>
        /// 获取期刊下载明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogDetailPageList(IssueDownLogQuery query)
        {
            return IssueDataAccess.Instance.GetIssueDownLogDetailPageList(query);
        }
        #endregion

        #region 浏览次数统计
        /// <summary>
        /// 获取期刊浏览次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueViewLogEntity> GetIssueViewLogPageList(IssueViewLogQuery query)
        {
            return IssueDataAccess.Instance.GetIssueViewLogPageList(query);
        }

        /// <summary>
        /// 获取期刊浏览明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueViewLogEntity> GetIssueViewLogDetailPageList(IssueViewLogQuery query)
        {
            return IssueDataAccess.Instance.GetIssueViewLogDetailPageList(query);
        }
        #endregion



        
    }
}
