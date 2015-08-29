using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;
using WKT.Common.Utils;
using WKT.Facade.Service.Interface;

namespace WKT.Facade.Service
{
    public class IssueFacadeAPIService : ServiceBase, IIssueFacadeService
    {
        #region 年卷设置
        /// <summary>
        /// 获取年卷设置分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>        
        public Pager<YearVolumeEntity> GetYearVolumePageList(YearVolumeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<YearVolumeEntity> pager = clientHelper.Post<Pager<YearVolumeEntity>, YearVolumeQuery>(GetAPIUrl(APIConstant.YEARVOLUME_GETPAGELIST), query);
            return pager;           
        }

        /// <summary>
        /// 获取年卷设置列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public IList<YearVolumeEntity> GetYearVolumeList(YearVolumeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<YearVolumeEntity> list = clientHelper.Post<IList<YearVolumeEntity>, YearVolumeQuery>(GetAPIUrl(APIConstant.YEARVOLUME_GETLIST), query);
            return list;    
        }

        /// <summary>
        /// 获取年卷设置实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public YearVolumeEntity GetYearVolumeModel(YearVolumeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            YearVolumeEntity model = clientHelper.Post<YearVolumeEntity, YearVolumeQuery>(GetAPIUrl(APIConstant.YEARVOLUME_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存年卷设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public ExecResult SaveYearVolume(YearVolumeEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, YearVolumeEntity>(GetAPIUrl(APIConstant.YEARVOLUME_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除年卷设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>      
        public ExecResult DelYearVolume(YearVolumeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, YearVolumeQuery>(GetAPIUrl(APIConstant.YEARVOLUME_DEL), query);
            return result;
        }
        #endregion

        #region 期设置
        /// <summary>
        /// 获取期设置分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public Pager<IssueSetEntity> GetIssueSetPageList(IssueSetQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<IssueSetEntity> pager = clientHelper.Post<Pager<IssueSetEntity>, IssueSetQuery>(GetAPIUrl(APIConstant.ISSUESET_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取期设置列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>        
        public IList<IssueSetEntity> GetIssueSetList(IssueSetQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<IssueSetEntity> list = clientHelper.Post<IList<IssueSetEntity>, IssueSetQuery>(GetAPIUrl(APIConstant.ISSUESET_GETLIST), query);
            return list;    
        }

        /// <summary>
        /// 获取期设置实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public IssueSetEntity GetIssueSetModel(IssueSetQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IssueSetEntity model = clientHelper.Post<IssueSetEntity, IssueSetQuery>(GetAPIUrl(APIConstant.ISSUESET_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存期设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public ExecResult SaveIssueSet(IssueSetEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, IssueSetEntity>(GetAPIUrl(APIConstant.ISSUESET_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除期设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public ExecResult DelIssueSet(IssueSetQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, IssueSetQuery>(GetAPIUrl(APIConstant.ISSUESET_DEL), query);
            return result;
        }
        #endregion

        #region 期刊栏目
        /// <summary>
        /// 获取期刊栏目分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public Pager<JournalChannelEntity> GetJournalChannelPageList(JournalChannelQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<JournalChannelEntity> pager = clientHelper.Post<Pager<JournalChannelEntity>, JournalChannelQuery>(GetAPIUrl(APIConstant.JOURNALCHANNEL_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取期刊栏目列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public IList<JournalChannelEntity> GetJournalChannelList(JournalChannelQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<JournalChannelEntity> list = clientHelper.Post<IList<JournalChannelEntity>, JournalChannelQuery>(GetAPIUrl(APIConstant.JOURNALCHANNEL_GETLIST), query);
            return list;    
        }

        /// <summary>
        /// 根据期刊数据 按照期刊栏目数据分组 获取当前期刊数据所属的期刊栏目数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public   IList<JournalChannelEntity> GetJournalChannelListByIssueContent(JournalChannelQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<JournalChannelEntity> list = clientHelper.Post<IList<JournalChannelEntity>, JournalChannelQuery>(GetAPIUrl(APIConstant.JOURNALCHANNEL_GETLIST_BY_ISSUECONTENT), query);
            return list;
        }

        /// <summary>
        /// 获取期刊栏目实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>        
        public JournalChannelEntity GetJournalChannelModel(JournalChannelQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            JournalChannelEntity model = clientHelper.Post<JournalChannelEntity, JournalChannelQuery>(GetAPIUrl(APIConstant.JOURNALCHANNEL_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存期刊栏目
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>      
        public ExecResult SaveJournalChannel(JournalChannelEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, JournalChannelEntity>(GetAPIUrl(APIConstant.JOURNALCHANNEL_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除期刊栏目
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public ExecResult DelJournalChannel(JournalChannelQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, JournalChannelQuery>(GetAPIUrl(APIConstant.JOURNALCHANNEL_DEL), query);
            return result;
        }

        /// <summary>
        /// 获取期刊栏目数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<TreeModel> GetJournalChannelTreeList(JournalChannelQuery query)
        {
            var list = GetJournalChannelList(query);
            TreeModel treeNode = new TreeModel();
            treeNode.Id = 0;
            treeNode.text = "期刊栏目";
            treeNode.url = "";
            treeNode.icon = "";
            treeNode.isexpand = true;
            if (list != null && list.Count > 0)
            {
                var first = list.Where(p => p.PChannelID == 0);
                TreeModel node = null;
                foreach (var item in first)
                {
                    node = new TreeModel();
                    node.Id = item.JChannelID;
                    node.text = item.ChannelName;
                    node.url = string.Empty;
                    node.icon = "";
                    node.isexpand = true;
                    GetJournalChannelTreeList(node, list);
                    treeNode.children.Add(node);
                }
            }
            IList<TreeModel> resultList = new List<TreeModel>();
            resultList.Add(treeNode);
            return resultList;
        }

        /// <summary>
        /// 处理子级
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        private void GetJournalChannelTreeList(TreeModel treeNode, IList<JournalChannelEntity> list)
        {
            var child = list.Where(p => p.PChannelID == treeNode.Id);
            if (child.Count() == 0)
            {
                treeNode.isexpand = false;
                treeNode.children = null;
                return;
            }
            TreeModel node = null;
            foreach (var item in child)
            {
                node = new TreeModel();
                node.Id = item.JChannelID;
                node.text = item.ChannelName;
                node.url = string.Empty;
                node.icon = "";
                node.isexpand = true;
                GetJournalChannelTreeList(node, list);
                treeNode.children.Add(node);
            }
        }
        #endregion

        #region 期刊

        /// <summary>
        /// 设置录用稿件的年期
        /// </summary>
        /// <returns></returns>
        public ExecResult SetContributionYearIssue(IssueContentQuery cEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, IssueContentQuery>(GetAPIUrl(APIConstant.ISSUE_SETCONTRIBUTIONYEARISSUE), cEntity);
            return result;
        }

        /// <summary>
        /// 获取期刊分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public Pager<IssueContentEntity> GetIssueContentPageList(IssueContentQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<IssueContentEntity> pager = clientHelper.Post<Pager<IssueContentEntity>, IssueContentQuery>(GetAPIUrl(APIConstant.ISSUECONTENT_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取期刊列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>        
        public IList<IssueContentEntity> GetIssueContentList(IssueContentQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<IssueContentEntity> list = clientHelper.Post<IList<IssueContentEntity>, IssueContentQuery>(GetAPIUrl(APIConstant.ISSUECONTENT_GETLIST), query);
            return list;    
        }

        /// <summary>
        /// 获取期刊实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public IssueContentEntity GetIssueContentModel(IssueContentQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IssueContentEntity model = clientHelper.Post<IssueContentEntity, IssueContentQuery>(GetAPIUrl(APIConstant.ISSUECONTENT_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存期刊
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public ExecResult SaveIssueContent(IssueContentEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, IssueContentEntity>(GetAPIUrl(APIConstant.ISSUECONTENT_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除期刊
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public ExecResult DelIssueContent(IssueContentQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, IssueContentQuery>(GetAPIUrl(APIConstant.ISSUECONTENT_DEL), query);
            return result;
        }

        /// <summary>
        /// 更新期刊内容浏览次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContentHits(IssueContentQuery model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            bool result = clientHelper.Post<bool, IssueContentQuery>(GetAPIUrl(APIConstant.ISSUECONTENT_UPDATEHITS), model);
            return result;
        }

        /// <summary>
        /// 更新期刊内容浏览次数(RichHTML)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContentHtmlHits(IssueContentQuery model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            bool result = clientHelper.Post<bool, IssueContentQuery>(GetAPIUrl(APIConstant.ISSUECONTENT_UPDATEHTMLHITS), model);
            return result;
        }

	    /// <summary>
        /// 更新期刊内容下载次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContentDownloads(IssueContentQuery model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            bool result = clientHelper.Post<bool, IssueContentQuery>(GetAPIUrl(APIConstant.ISSUECONTENT_UPDATEDOWNLOADS), model);
            return result;
        }

        #endregion

        #region 期刊订阅
        /// <summary>
        /// 获取期刊订阅分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>     
        public Pager<IssueSubscribeEntity> GetIssueSubscribePageList(IssueSubscribeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<IssueSubscribeEntity> pager = clientHelper.Post<Pager<IssueSubscribeEntity>, IssueSubscribeQuery>(GetAPIUrl(APIConstant.ISSUESUBSCRIBE_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取期刊订阅列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public IList<IssueSubscribeEntity> GetIssueSubscribeList(IssueSubscribeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<IssueSubscribeEntity> list = clientHelper.Post<IList<IssueSubscribeEntity>, IssueSubscribeQuery>(GetAPIUrl(APIConstant.ISSUESUBSCRIBE_GETLIST), query);
            return list;  
        }

        /// <summary>
        /// 获取期刊订阅实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public IssueSubscribeEntity GetIssueSubscribeModel(IssueSubscribeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IssueSubscribeEntity model = clientHelper.Post<IssueSubscribeEntity, IssueSubscribeQuery>(GetAPIUrl(APIConstant.ISSUESUBSCRIBE_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存期刊订阅
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public ExecResult SaveIssueSubscribe(IssueSubscribeEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, IssueSubscribeEntity>(GetAPIUrl(APIConstant.ISSUESUBSCRIBE_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除期刊订阅
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>      
        public ExecResult DelIssueSubscribe(IssueSubscribeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, IssueSubscribeQuery>(GetAPIUrl(APIConstant.ISSUESUBSCRIBE_DEL), query);
            return result;
        }
        #endregion

        /// <summary>
        /// 获取当前期刊最新的年、卷、期、当期封面图片
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IssueSiteEntity GetCurIssueInfo(IssueSetQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IssueSiteEntity model = clientHelper.Post<IssueSiteEntity, IssueSetQuery>(GetAPIUrl(APIConstant.ISSUE_GETCURISSUEINFO), query);
            return model;
        }

        # region 访问下载日志

        /// <summary>
        /// 保存访问日志
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult SaveViewLog(IssueViewLogEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.Post<ExecResult, IssueViewLogEntity>(GetAPIUrl(APIConstant.ISSUE_SAVEVIEWLOG), model);
            return execResult;
        }

        /// <summary>
        /// 保存下载日志
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult SaveDownloadLog(IssueDownLogEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.Post<ExecResult, IssueDownLogEntity>(GetAPIUrl(APIConstant.ISSUE_SAVEDOWNLOADLOG), model);
            return execResult;
        }

        # endregion

        #region 下载次数统计
        /// <summary>
        /// 获取期刊下载次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(IssueDownLogQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<IssueDownLogEntity> pager = clientHelper.Post<Pager<IssueDownLogEntity>, IssueDownLogQuery>(GetAPIUrl(APIConstant.ISSUEDOWNLOAD_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取期刊下载明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogDetailPageList(IssueDownLogQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<IssueDownLogEntity> pager = clientHelper.Post<Pager<IssueDownLogEntity>, IssueDownLogQuery>(GetAPIUrl(APIConstant.ISSUEDOWNLOAD_GETDETAILPAGELIST), query);
            return pager;
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
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<IssueViewLogEntity> pager = clientHelper.Post<Pager<IssueViewLogEntity>, IssueViewLogQuery>(GetAPIUrl(APIConstant.ISSUEVIEW_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取期刊浏览明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueViewLogEntity> GetIssueViewLogDetailPageList(IssueViewLogQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<IssueViewLogEntity> pager = clientHelper.Post<Pager<IssueViewLogEntity>, IssueViewLogQuery>(GetAPIUrl(APIConstant.ISSUEVIEW_GETDETAILPAGELIST), query);
            return pager;
        }
        #endregion

        public ExecResult DelDoiRegLog(long[] PKID)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, Int64[]>(GetAPIUrl(APIConstant.DOIREGLOG_DELDOIREGLOG), PKID);
            return execResult;
        }

        public DoiRegLogEntity GetDoiRegLog(DoiRegLogQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            DoiRegLogEntity model = clientHelper.Post<DoiRegLogEntity, DoiRegLogQuery>(GetAPIUrl(APIConstant.DOIREGLOG_GETDOIREGLOG), query);
            return model;
        }

        public IList<DoiRegLogEntity> GetDoiRegLogList(DoiRegLogQuery doiRegLogQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<DoiRegLogEntity> list = clientHelper.Post<IList<DoiRegLogEntity>, DoiRegLogQuery>(GetAPIUrl(APIConstant.DOIREGLOG_GETLIST), doiRegLogQuery);
            return list;  
        }

        public Pager<DoiRegLogEntity> GetDoiRegLogPageList(DoiRegLogQuery doiRegLogQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<DoiRegLogEntity> pager = clientHelper.Post<Pager<DoiRegLogEntity>, DoiRegLogQuery>(GetAPIUrl(APIConstant.DOIREGLOG_GETPAGELIST), doiRegLogQuery);
            return pager;
        }

        public ExecResult SaveDoiRegLog(DoiRegLogEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, DoiRegLogEntity>(GetAPIUrl(APIConstant.DOIREGLOG_SAVEDOIREGLOG), model);
            return result;
        }


    }
}
