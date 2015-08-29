using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Reflection;
using System.ComponentModel;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using WKT.Config;
using WKT.Common.Extension;
using WKT.Common.Utils;
using WKT.Common.Security;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;
using WKT.Cache;

namespace Web.Mobile.Controllers
{
    public static class HtmlHelperExtensions
    {
        #region 全局配置属性

        /// <summary>
        /// 访问统计地址
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string StatUrl(this HtmlHelper helper)
        {
            return SiteConfig.APIHost + "web/stat?JournalID=" + ConfigurationManager.AppSettings["SiteID"];
        }

        /// <summary>
        /// 站点ID
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static long JournalID(this HtmlHelper helper)
        {
            return TypeParse.ToLong(ConfigurationManager.AppSettings["SiteID"]);
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string SiteName(this HtmlHelper helper)
        {
            return SiteConfig.SiteName;
        }

        /// <summary>
        /// 站点首页
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string SiteHome(this HtmlHelper helper)
        {
            return WKT.Common.Utils.Utils.SiteHome();
        }

        /// <summary>
        /// 管理后台地址
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string AdminUrl(this HtmlHelper helper)
        {
            return ConfigurationManager.AppSettings["AdminUrl"];
        }

        /// <summary>
        /// 资源根路径
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string RootPath(this HtmlHelper helper)
        {
            return ConfigurationManager.AppSettings["RootPath"];
        }

        /// <summary>
        /// 获取总访问量数
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static int GetAllHits(this HtmlHelper helper)
        {
            int totalCount = 0;
            try
            {
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                SiteConfigQuery query = new SiteConfigQuery();
                query.JournalID = GetJournalID();
                totalCount = service.GetSiteAccessCount(query);
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("获取访问量总数出现异常：" + ex.Message);
            }
            return totalCount;
        }

        /// <summary>
        /// 获取当前在线数
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static int GetCurOnlineCount(this HtmlHelper helper)
        {
            int onlineCount = 0;
            try
            {
                string ONLINE_KEY = "WKT_ONLINE";
                if (HttpContext.Current.Application[ONLINE_KEY] == null)
                {
                    onlineCount = 0;
                }
                else
                {
                    onlineCount = TypeParse.ToInt(HttpContext.Current.Application[ONLINE_KEY]);
                }
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("获取当前在线用户数出现异常：" + ex.Message);
            }
            return onlineCount;
        }

        #endregion

        # region 获取前台网站数据

        # region 获取网站导航

        /// <summary>
        /// 获取导航
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public static IList<SiteChannelEntity> GetSiteNav(this HtmlHelper helper)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteChannelQuery query = new SiteChannelQuery();
            query.JournalID = GetJournalID();
            query.IsNav = 1;
            query.Status = 1;
            string key = string.Format(CacheKey.SITE_GETJOURNAL_CHANNELLIST, query.JournalID);
            object objCacheData = CacheStrategyFactory.Instance.GetObject(key);
            IList<SiteChannelEntity> navList = null;
            if (objCacheData == null)
            {
                navList = service.GetSiteChannelList(query);
                CacheStrategyFactory.Instance.AddObject(key, navList, DateTime.Now.AddHours(12));
            }
            else
            {
                navList = (IList<SiteChannelEntity>)objCacheData;
            }
            return navList;
        }

        # endregion

        # region 网站显示数据

        /// <summary>
        /// 获取数据块
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public static IList<SiteBlockEntity> GetBlockList(this HtmlHelper helper, long ChannelID, int TopCount)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteBlockQuery query = new SiteBlockQuery();
            query.JournalID = GetJournalID();
            query.CurrentPage = 1;
            query.PageSize = TopCount;
            query.ChannelID = ChannelID;
            Pager<SiteBlockEntity> pager = null;
            string key = string.Format(CacheKey.SITE_GETJOURNAL_BLOCKCONTENTLIST, query.JournalID, TopCount);
            object objCacheData = CacheStrategyFactory.Instance.GetObject(key);
            if (objCacheData == null)
            {
                pager = service.GetSiteBlockPageList(query);
                CacheStrategyFactory.Instance.AddObject(key, pager);
            }
            else
            {
                pager = (Pager<SiteBlockEntity>)objCacheData;
            }
            if (pager == null)
            {
                pager = new Pager<SiteBlockEntity>();
            }
            return pager.ItemList;
        }

        /// <summary>
        /// 获取公告
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public static IList<SiteNoticeEntity> GetNoticeList(this HtmlHelper helper, long ChannelID, int TopCount)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteNoticeQuery query = new SiteNoticeQuery();
            query.JournalID = GetJournalID();
            query.CurrentPage = 1;
            query.PageSize = TopCount;
            query.ChannelID = ChannelID;
            Pager<SiteNoticeEntity> pager = null;
            string key = string.Format(CacheKey.SITE_GETJOURNAL_NOTICELIST, query.JournalID, ChannelID, TopCount);
            object objCacheData = CacheStrategyFactory.Instance.GetObject(key);
            if (objCacheData == null)
            {
                pager = service.GetSiteNoticePageList(query);
                CacheStrategyFactory.Instance.AddObject(key, pager);
            }
            else
            {
                pager = (Pager<SiteNoticeEntity>)objCacheData;
            }
            if (pager == null)
            {
                pager = new Pager<SiteNoticeEntity>();
            }
            return pager.ItemList;
        }

        /// <summary>
        /// 获取资讯
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public static IList<SiteContentEntity> GetNewsList(this HtmlHelper helper, long ChannelID, int TopCount)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteContentQuery query = new SiteContentQuery();
            query.JournalID = GetJournalID();
            query.CurrentPage = 1;
            query.PageSize = TopCount;
            query.ChannelID = ChannelID;
            Pager<SiteContentEntity> pager = service.GetSiteContentPageList(query);
            if (pager == null)
            {
                pager = new Pager<SiteContentEntity>();
            }
            return pager.ItemList;
        }

        /// <summary>
        /// 获取图片资讯
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public static IList<SiteContentEntity> GetPhotoNewsList(this HtmlHelper helper, long ChannelID, int TopCount)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteContentQuery query = new SiteContentQuery();
            query.JournalID = GetJournalID();
            query.CurrentPage = 1;
            query.PageSize = TopCount;
            query.ChannelID = ChannelID;
            query.IsPhoto = true;
            Pager<SiteContentEntity> pager = service.GetSiteContentPageList(query);
            if (pager == null)
            {
                pager = new Pager<SiteContentEntity>();
            }
            return pager.ItemList;
        }

        /// <summary>
        /// 获取友情链接
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public static IList<FriendlyLinkEntity> GetFriendLinkList(this HtmlHelper helper, long ChannelID, int TopCount)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            FriendlyLinkQuery query = new FriendlyLinkQuery();
            query.JournalID = GetJournalID();
            query.CurrentPage = 1;
            query.PageSize = TopCount;
            query.ChannelID = ChannelID;
            Pager<FriendlyLinkEntity> pager = service.GetFriendlyLinkPageList(query);
            if (pager == null)
            {
                pager = new Pager<FriendlyLinkEntity>();
            }
            return pager.ItemList;
        }

        /// <summary>
        /// 获取资源类数据
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public static IList<SiteResourceEntity> GetResourceList(this HtmlHelper helper, long ChannelID, int TopCount)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteResourceQuery query = new SiteResourceQuery();
            query.JournalID = GetJournalID();
            query.CurrentPage = 1;
            query.PageSize = TopCount;
            query.ChannelID = ChannelID;
            Pager<SiteResourceEntity> pager = service.GetSiteResourcePageList(query);
            if (pager == null)
            {
                pager = new Pager<SiteResourceEntity>();
            }
            return pager.ItemList;
        }

        private static long GetJournalID()
        {
            return TypeParse.ToLong(ConfigurationManager.AppSettings["SiteID"]);
        }

        # endregion

        # region 获取期刊相关数据

        /// <summary>
        /// 获取年卷
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IList<YearVolumeEntity> GetYearVolumeList(this HtmlHelper helper)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            YearVolumeQuery query = new YearVolumeQuery();
            query.JournalID = GetJournalID();
            IList<YearVolumeEntity> listYearVolume = null;
            string key = string.Format(CacheKey.SITE_GETJOURNAL_YEARVOLUMNLIST, query.JournalID);
            object objCacheData = CacheStrategyFactory.Instance.GetObject(key);
            if (objCacheData == null)
            {
                listYearVolume = service.GetYearVolumeList(query);
                CacheStrategyFactory.Instance.AddObject(key, listYearVolume);
            }
            else
            {
                listYearVolume = (IList<YearVolumeEntity>)objCacheData;
            }
            return listYearVolume;
        }

        /// <summary>
        /// 获取期
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IList<IssueSetEntity> GetIssueSetList(this HtmlHelper helper)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueSetQuery query = new IssueSetQuery();
            query.JournalID = GetJournalID();
            string key = string.Format(CacheKey.SITE_GETJOURNAL_ISSLUELIST, query.JournalID);
            object objCacheData = CacheStrategyFactory.Instance.GetObject(key);
            IList<IssueSetEntity> listIssue = null;
            if (objCacheData == null)
            {
                listIssue = service.GetIssueSetList(query);
                CacheStrategyFactory.Instance.AddObject(key, listIssue);
            }
            else
            {
                listIssue = (IList<IssueSetEntity>)objCacheData;
            }
            return listIssue;
        }

        /// <summary>
        /// 获取期刊栏目分类
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IList<JournalChannelEntity> GetJournalChannelList(this HtmlHelper helper)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            JournalChannelQuery query = new JournalChannelQuery();
            query.JournalID = GetJournalID();
            query.Status = 1;
            string key = string.Format(CacheKey.SITE_GETJOURNAL_JOURNALICHANNELLIST, query.JournalID);
            object objCacheData = CacheStrategyFactory.Instance.GetObject(key);
            IList<JournalChannelEntity> listJournalChannel = null;
            if (objCacheData == null)
            {
                listJournalChannel = service.GetJournalChannelList(query);
                CacheStrategyFactory.Instance.AddObject(key, listJournalChannel, DateTime.Now.AddHours(8));
            }
            else
            {
                listJournalChannel = (IList<JournalChannelEntity>)objCacheData;
            }
            return listJournalChannel;
        }



        /// <summary>
        /// 获取期刊内容列表(全部)
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IList<IssueContentEntity> GeIssueContentList(this HtmlHelper helper, int TopCount)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = GetJournalID();
            query.PageSize = TopCount;
            query.CurrentPage = 1;
            string key = string.Format(CacheKey.SITE_GETJOURNAL_ISSUECONTENTLIST, query.JournalID, 1, TopCount);
            object objCacheData = CacheStrategyFactory.Instance.GetObject(key);
            Pager<IssueContentEntity> listIssueContentEntity = null;
            if (objCacheData == null)
            {
                listIssueContentEntity = service.GetIssueContentPageList(query);
            }
            else
            {
                listIssueContentEntity = (Pager<IssueContentEntity>)objCacheData;
            }
            if (listIssueContentEntity != null)
            {
                return listIssueContentEntity.ItemList;
            }
            else
            {
                return new List<IssueContentEntity>();
            }
        }

        /// <summary>
        /// 获取期刊内容列表(根据期刊栏目ID,年，期)
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="JChannelID">栏目ID</param>
        /// <param name="Year">年</param>
        /// <param name="Issue">期</param>
        /// <param name="TopCount">显示条数</param>
        /// <returns></returns>
        public static IList<IssueContentEntity> GeIssueContentList(this HtmlHelper helper, long JChannelID, int Year, int Issue, int TopCount)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = GetJournalID();
            query.JChannelID = JChannelID;
            query.Year = Year;
            query.Issue = Issue;
            query.PageSize = TopCount;
            query.CurrentPage = 1;
            string key = string.Format(CacheKey.SITE_GETJOURNAL_ISSUECONTENTLIST, query.JournalID, 1, JChannelID, TopCount);
            object objCacheData = CacheStrategyFactory.Instance.GetObject(key);
            Pager<IssueContentEntity> listIssueContentEntity = null;
            if (objCacheData == null)
            {
                listIssueContentEntity = service.GetIssueContentPageList(query);
            }
            else
            {
                listIssueContentEntity = (Pager<IssueContentEntity>)objCacheData;
            }
            if (listIssueContentEntity != null)
            {
                return listIssueContentEntity.ItemList;
            }
            else
            {
                return new List<IssueContentEntity>();
            }
        }

        /// <summary>
        /// 获取期刊内容列表(根据期刊栏目ID)
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="JChannelID">栏目ID</param>
        /// <param name="TopCount">显示条数</param>
        /// <returns></returns>
        public static IList<IssueContentEntity> GeIssueContentList(this HtmlHelper helper, long JChannelID, int TopCount)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = GetJournalID();
            query.JChannelID = JChannelID;
            query.PageSize = TopCount;
            query.CurrentPage = 1;
            query.SortName = " Year DESC,Issue ";
            query.SortOrder = " DESC ";
            string key = string.Format(CacheKey.SITE_GETJOURNAL_ISSUECONTENTLIST, query.JournalID, 1, JChannelID, TopCount);
            object objCacheData = CacheStrategyFactory.Instance.GetObject(key);
            Pager<IssueContentEntity> listIssueContentEntity = null;
            if (objCacheData == null)
            {
                listIssueContentEntity = service.GetIssueContentPageList(query);
            }
            else
            {
                listIssueContentEntity = (Pager<IssueContentEntity>)objCacheData;
            }
            if (listIssueContentEntity != null)
            {
                return listIssueContentEntity.ItemList;
            }
            else
            {
                return new List<IssueContentEntity>();
            }
        }

        /// <summary>
        /// 获取期刊内容列表(根据年/期)
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IList<IssueContentEntity> GeIssueContentList(this HtmlHelper helper, int TopCount, int Issue, int Year)
        {
            # region 判断传过来的期数是否已经超过最大期

            IList<IssueSetEntity> issueList = GetIssueSetList(helper);
            var byList = issueList.OrderByDescending(p => p.Issue).ToList<IssueSetEntity>();
            if (byList.Count > 0)
            {
                int MaxIssue = byList[0].Issue;
                if (Issue > MaxIssue)
                {
                    Issue = 1;
                    Year = Year + 1;
                }
            }

            # endregion

            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = GetJournalID();
            query.PageSize = TopCount;
            query.CurrentPage = 1;
            query.Issue = Issue;
            query.Year = Year;
            query.SortName = " AddDate ";
            query.SortOrder = " ASC ";
            string key = string.Format(CacheKey.SITE_GETJOURNAL_ISSUECONTENTISSUESLIST, query.JournalID, 1, TopCount, Issue, Year);
            object objCacheData = CacheStrategyFactory.Instance.GetObject(key);
            Pager<IssueContentEntity> listIssueContentEntity = null;
            if (objCacheData == null)
            {
                listIssueContentEntity = service.GetIssueContentPageList(query);
            }
            else
            {
                listIssueContentEntity = (Pager<IssueContentEntity>)objCacheData;
            }
            if (listIssueContentEntity != null)
            {
                return listIssueContentEntity.ItemList;
            }
            else
            {
                return new List<IssueContentEntity>();
            }
        }

        public static IList<IssueContentEntity> GeIssueContentList(int TopCount, int Issue, int Year)
        {

            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = GetJournalID();
            query.PageSize = TopCount;
            query.CurrentPage = 1;
            query.Issue = Issue;
            query.Year = Year;
            query.SortName = " AddDate ";
            query.SortOrder = " DESC ";
            string key = string.Format(CacheKey.SITE_GETJOURNAL_ISSUECONTENTISSUESLIST, query.JournalID, 1, TopCount, Issue, Year);
            object objCacheData = CacheStrategyFactory.Instance.GetObject(key);
            Pager<IssueContentEntity> listIssueContentEntity = null;
            if (objCacheData == null)
            {
                listIssueContentEntity = service.GetIssueContentPageList(query);
            }
            else
            {
                listIssueContentEntity = (Pager<IssueContentEntity>)objCacheData;
            }
            if (listIssueContentEntity != null)
            {
                return listIssueContentEntity.ItemList;
            }
            else
            {
                return new List<IssueContentEntity>();
            }
        }

        /// <summary>
        /// 获取当前期刊最新的年、卷、期、当期封面图片
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IssueSiteEntity GetCurIssueInfo(this HtmlHelper helper)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueSetQuery query = new IssueSetQuery();
            query.JournalID = GetJournalID();
            IssueSiteEntity issueInfoEntity = service.GetCurIssueInfo(query);
            if (issueInfoEntity == null)
            {
                issueInfoEntity = new IssueSiteEntity();
            }
            return issueInfoEntity;
        }

        # endregion

        # region 获取内容版本变换图数据

        /// <summary>
        /// 获取内容版本变换图数据
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public static string GetHotImageList(this HtmlHelper helper, long ChannelID, int TopCount)
        {
            StringBuilder sbContent = new StringBuilder("");

            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteBlockQuery query = new SiteBlockQuery();
            query.JournalID = GetJournalID();
            query.CurrentPage = 1;
            query.PageSize = TopCount;
            query.ChannelID = ChannelID;
            Pager<SiteBlockEntity> pager = null;
            string key = string.Format(CacheKey.SITE_GETJOURNAL_BLOCKCONTENTLIST, query.JournalID, TopCount);
            object objCacheData = CacheStrategyFactory.Instance.GetObject(key);
            if (objCacheData == null)
            {
                pager = service.GetSiteBlockPageList(query);
                CacheStrategyFactory.Instance.AddObject(key, pager);
            }
            else
            {
                pager = (Pager<SiteBlockEntity>)objCacheData;
            }
            if (pager == null)
            {
                pager = new Pager<SiteBlockEntity>();
            }
            int i = 1;
            foreach (SiteBlockEntity item in pager.ItemList)
            {
                if (i == pager.ItemList.Count)
                {
                    sbContent.Append("{ title: '").Append(item.Title).Append("', src: '").Append(item.TitlePhoto).Append("', href: '").Append(item.Linkurl).Append("', target: '_blank' }");
                }
                else
                {
                    sbContent.Append("{ title: '").Append(item.Title).Append("', src: '").Append(item.TitlePhoto).Append("', href: '").Append(item.Linkurl).Append("', target: '_blank' },");
                }
                i++;
            }
            return sbContent.ToString();
        }

        # endregion

        # endregion

        # region 获取稿件信息

        /// <summary>
        /// 获取录用稿件信息
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public static IList<ContributionInfoEntity> GetContributionList(this HtmlHelper helper, int topCount)
        {
            IList<ContributionInfoEntity> listC = new List<ContributionInfoEntity>();
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            ContributionInfoQuery query = new ContributionInfoQuery();
            query.JournalID = GetJournalID(); ;
            query.CurrentPage = 1;
            query.PageSize = topCount;
            query.Status = (int)EnumContributionStatus.Employment;
            query.OrderStr = " AddDate desc";
            Pager<ContributionInfoEntity> pager = service.GetContributionInfoPageList(query);
            if (pager != null)
            {
                listC = pager.ItemList;
            }
            return listC; ;
        }

        # endregion

        # region 方法

        public static string CutString(this HtmlHelper helper, string str, int length)
        {
            return WKT.Common.Utils.TextHelper.SubStr(str, length);
        }
        public static string CutString(this HtmlHelper helper, string str, int length, string suffix)
        {
            return WKT.Common.Utils.TextHelper.SubStr(str, length, suffix);
        }
        public static string CleanHtmlCutString(this HtmlHelper helper, string str, int length, string suffix)
        {
            return WKT.Common.Utils.TextHelper.SubStr(WKT.Common.Utils.Utils.ClearHtml(str), length, suffix);
        }
        public static string CleanHtml(this HtmlHelper helper, string str)
        {
            return WKT.Common.Utils.Utils.ClearHtml(str);
        }
        public static string CleanHtmlNbsp(this HtmlHelper helper, string str)
        {
            return WKT.Common.Utils.Utils.ClearHtmlNbsp(str);
        }

        public static string GetAuthorsString(this HtmlHelper helper, string str)
        {
            str = str.Replace("；", ",").Replace(";", ",").Trim();
            if (str.EndsWith(","))
            {
                str = str.Remove(str.Length - 1);
            }
            return str;
        }

        # endregion

        # region 博客

        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public static IList<BlogEntity> GetBlogList(this HtmlHelper helper, int TopCount)
        {
            IList<BlogEntity> listBlog = new List<BlogEntity>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("BLOGDB");
                string sql = "select top " + TopCount + " DisplayName,DoctorID From tb_Doctor with(nolock) where DisplayName <>''  order by DisplayName";
                DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    while (dr.Read())
                    {
                        BlogEntity item = new BlogEntity();
                        item.BlogID = TypeParse.ToInt(dr["DoctorID"]);
                        item.BlogName = TypeParse.ToString(dr["DisplayName"]);
                        listBlog.Add(item);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("获取博客列表出现异常：" + ex.Message);
            }
            return listBlog;
        }

        /// <summary>
        /// 获取最新更新的博客列表
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public static IList<BlogEntity> GetUpdateBlogList(this HtmlHelper helper, int TopCount)
        {
            IList<BlogEntity> listBlog = new List<BlogEntity>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase("BLOGDB");
                string sql = "";
                DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    while (dr.Read())
                    {
                        BlogEntity item = new BlogEntity();
                        item.BlogID = TypeParse.ToInt(dr["DoctorID"]);
                        item.BlogName = TypeParse.ToString(dr["DisplayName"]);
                        listBlog.Add(item);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("获取更新博客列表出现异常：" + ex.Message);
            }
            return listBlog;
        }

        # endregion

        # region 排行

        # region 本期点击排行

        /// <summary>
        /// 本期点击排行
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IList<IssueContentEntity> GeCurIssueHitsOrderList(this HtmlHelper helper, int TopCount)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();

            # region 年卷期

            IssueSetQuery issuequery = new IssueSetQuery();
            issuequery.JournalID = GetJournalID();
            IssueSiteEntity issueInfoEntity = service.GetCurIssueInfo(issuequery);
            if (issueInfoEntity == null)
            {
                issueInfoEntity = new IssueSiteEntity();
                issueInfoEntity.Year = DateTime.Now.Year;
                issueInfoEntity.Volume = 1;
                issueInfoEntity.Issue = 1;
            }
            # endregion

            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = GetJournalID();
            query.PageSize = TopCount;
            query.Year = issueInfoEntity.Year;
            query.Issue = issueInfoEntity.Issue;
            query.CurrentPage = 1;
            query.SortName = " Hits ";
            query.SortOrder = " DESC ";
            Pager<IssueContentEntity> listIssueContentEntity = service.GetIssueContentPageList(query);
            if (listIssueContentEntity != null)
            {
                return listIssueContentEntity.ItemList;
            }
            else
            {
                return new List<IssueContentEntity>();
            }
        }

        # endregion

        # region 历史点击排行

        /// <summary>
        /// 历史点击排行
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IList<IssueContentEntity> GeHitsOrderList(this HtmlHelper helper, int TopCount)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = GetJournalID();
            query.PageSize = TopCount;
            query.CurrentPage = 1;
            query.SortName = " Hits ";
            query.SortOrder = " DESC ";
            Pager<IssueContentEntity> listIssueContentEntity = service.GetIssueContentPageList(query);
            if (listIssueContentEntity != null)
            {
                return listIssueContentEntity.ItemList;
            }
            else
            {
                return new List<IssueContentEntity>();
            }
        }

        /// <summary>
        /// 历史点击排行(根据年)
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IList<IssueContentEntity> GeHitsOrderList(this HtmlHelper helper, int Year, int TopCount)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = GetJournalID();
            query.PageSize = TopCount;
            query.CurrentPage = 1;
            query.Year = Year;
            query.SortName = " Hits ";
            query.SortOrder = " DESC ";
            Pager<IssueContentEntity> listIssueContentEntity = service.GetIssueContentPageList(query);
            if (listIssueContentEntity != null)
            {
                return listIssueContentEntity.ItemList;
            }
            else
            {
                return new List<IssueContentEntity>();
            }
        }

        # endregion

        # region 本期下载排行

        /// <summary>
        /// 本期下载排行
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IList<IssueContentEntity> GeCurIssueDownloadsOrderList(this HtmlHelper helper, int TopCount)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();

            # region 年卷期

            IssueSetQuery issuequery = new IssueSetQuery();
            issuequery.JournalID = GetJournalID();
            IssueSiteEntity issueInfoEntity = service.GetCurIssueInfo(issuequery);
            if (issueInfoEntity == null)
            {
                issueInfoEntity = new IssueSiteEntity();
                issueInfoEntity.Year = DateTime.Now.Year;
                issueInfoEntity.Volume = 1;
                issueInfoEntity.Issue = 1;
            }
            # endregion

            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = GetJournalID();
            query.PageSize = TopCount;
            query.Year = issueInfoEntity.Year;
            query.Issue = issueInfoEntity.Issue;
            query.CurrentPage = 1;
            query.SortName = " Downloads ";
            query.SortOrder = " DESC ";
            Pager<IssueContentEntity> listIssueContentEntity = service.GetIssueContentPageList(query);
            if (listIssueContentEntity != null)
            {
                return listIssueContentEntity.ItemList;
            }
            else
            {
                return new List<IssueContentEntity>();
            }
        }

        # endregion

        # region 历史下载排行

        /// <summary>
        /// 历史下载排行
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IList<IssueContentEntity> GeDownloadsOrderList(this HtmlHelper helper, int TopCount)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = GetJournalID();
            query.PageSize = TopCount;
            query.CurrentPage = 1;
            query.SortName = " Downloads ";
            query.SortOrder = " DESC ";
            Pager<IssueContentEntity> listIssueContentEntity = service.GetIssueContentPageList(query);
            if (listIssueContentEntity != null)
            {
                return listIssueContentEntity.ItemList;
            }
            else
            {
                return new List<IssueContentEntity>();
            }
        }

        /// <summary>
        /// 历史下载排行_根据年
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="Year"></param>
        /// <param name="TopCount"></param>
        /// <returns></returns>
        public static IList<IssueContentEntity> GeDownloadsOrderList(this HtmlHelper helper, int Year, int TopCount)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = GetJournalID();
            query.PageSize = TopCount;
            query.CurrentPage = 1;
            query.Year = Year;
            query.SortName = " Downloads ";
            query.SortOrder = " DESC ";
            Pager<IssueContentEntity> listIssueContentEntity = service.GetIssueContentPageList(query);
            if (listIssueContentEntity != null)
            {
                return listIssueContentEntity.ItemList;
            }
            else
            {
                return new List<IssueContentEntity>();
            }
        }

        # endregion

        # endregion


        #region 获取期刊所属学科名称

        public static JournalChannelEntity GetJChannelNameByJChannelID(this HtmlHelper helper, Int64 ID)
        {
            JournalChannelEntity currentEntity = null;
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            JournalChannelQuery query = new JournalChannelQuery();
            query.JournalID = ID;
            currentEntity = service.GetJournalChannelModel(query);
            if (currentEntity == null)
            {
                return currentEntity = new JournalChannelEntity();
            }

            return currentEntity;
        }

        #endregion

        #region 获取资讯实体

        public static SiteContentEntity GetNewsByContentID(this HtmlHelper helper, Int64 ContentID)
        {
            SiteContentEntity currentEntity = null;
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteContentQuery query = new SiteContentQuery();
            query.JournalID = GetJournalID();
            query.ContentID = ContentID;
            currentEntity = service.GetSiteContentModel(query);
            if (currentEntity == null)
            {
                currentEntity = new SiteContentEntity();
            }

            return currentEntity;
        }

        #endregion



    }
}