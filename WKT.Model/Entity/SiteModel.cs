using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model
{
    /// <summary>
    /// 网站前端实体
    /// </summary>
    public class SiteModel
    {
        private NewsModel _newsModel = new NewsModel();
        public NewsModel ConentEntity
        {
            get { return _newsModel; }
            set { _newsModel = value; }
        }

        private IList<NewsModel> _newsModelList = new List<NewsModel>();
        public IList<NewsModel> ListConentEntity
        {
            get { return _newsModelList; }
            set { _newsModelList = value; }
        }

        /// <summary>
        /// 网站内容块数据
        /// </summary>
        public IList<SiteBlockEntity> SiteBlockList
        {
            get;
            set;
        }

        /// <summary>
        /// 网站描述类，例如：公告、关于我们等
        /// </summary>
        public IList<SiteNoticeEntity> SiteNoticeList
        {
            get;
            set;
        }

        /// <summary>
        /// 网站友情链接数据
        /// </summary>
        public IList<FriendlyLinkEntity> FriendlyLinkList
        {
            get;
            set;
        }

        /// <summary>
        /// 网站资讯等数据
        /// </summary>
        public IList<SiteContentEntity> SiteContentList
        {
            get;
            set;
        }

        /// <summary>
        /// 网站联系我们数据
        /// </summary>
        public IList<ContactWayEntity> ContactWayList
        {
            get;
            set;
        }

        /// <summary>
        /// 电子期刊数据
        /// </summary>
        public IList<IssueContentEntity> IssueContentList
        {
            get;
            set;
        }

        /// <summary>
        /// 网站资讯等分页数据
        /// </summary>
        public Pager<SiteContentEntity> PagerSiteContent
        {
            get;
            set;
        }

        /// <summary>
        /// 电子期刊分页数据
        /// </summary>
        public Pager<IssueContentEntity> PagerIssueContent
        {
            get;
            set;
        }
    }

    public class NewsModel
    {
        public long PKID
        {
            get;
            set;
        }

        public long ChannelID
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string FJPath
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public string LinkUrl
        {
            get;
            set;
        }

        public DateTime PublishDate
        {
            get;
            set;
        }
    }
}
