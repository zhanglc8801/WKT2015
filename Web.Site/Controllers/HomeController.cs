using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;

using WKT.Config;
using WKT.Common.Extension;
using WKT.Common.Utils;
using WKT.Common.Security;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace Web.Site.Controllers
{
    public class HomeController : BaseController
    {
        int PageSize = 15;

        public ActionResult Index()
        {
            return View();
        }

        # region show

        /// <summary>
        /// 显示指定的频道页面
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public ActionResult show(long ChannelID)
        {
            ViewBag.Title = "";
            ViewBag.ChannelName = "";
            SiteModel siteModel = new SiteModel();
            
            long ItemID = TypeParse.ToLong(Request.QueryString["itemid"]);
            if (ItemID > 0)
            {
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                SiteChannelQuery query = new SiteChannelQuery();
                query.JournalID = JournalID;
                query.ChannelID = ChannelID;
                SiteChannelEntity channelEntity = service.GetSiteChannelModel(query);

                if (channelEntity != null)
                {
                    ViewBag.ChannelName = channelEntity.Keywords;
                    if (channelEntity.ContentType == (int)EnumContentType.Information)
                    {
                        SiteContentQuery contentQuery = new SiteContentQuery();
                        contentQuery.ChannelID = ChannelID;
                        contentQuery.JournalID = JournalID;
                        contentQuery.ContentID = ItemID;
                        SiteContentEntity contentEntity = service.GetSiteContentModel(contentQuery);
                        if (contentEntity != null)
                        {
                            string contentTitle = "";
                            contentTitle = string.Format("<span style=\"font-weight:{2};font-style:{3};color:{1}\">{0}</span>", contentEntity.Title, !string.IsNullOrEmpty(contentEntity.TitleColor) ? contentEntity.TitleColor : "#000",
                                contentEntity.IsBold ? "bold" : "normal", contentEntity.IsItalic ? "italic" : "normal");
                            siteModel.ConentEntity.Title = contentTitle;
                            siteModel.ConentEntity.FJPath = contentEntity.FJPath;
                            siteModel.ConentEntity.Content = contentEntity.Content;
                            siteModel.ConentEntity.ChannelID = contentEntity.ChannelID;
                            siteModel.ConentEntity.PublishDate = contentEntity.AddDate;
                        }
                    }
                    else if (channelEntity.ContentType == (int)EnumContentType.SiteDescribe)
                    {
                        SiteNoticeQuery noticeQuery = new SiteNoticeQuery();
                        noticeQuery.ChannelID = ChannelID;
                        noticeQuery.JournalID = JournalID;
                        noticeQuery.NoticeID = ItemID;
                        SiteNoticeEntity noticeEntity = service.GetSiteNoticeModel(noticeQuery);
                        if (noticeEntity != null)
                        {
                            siteModel.ConentEntity.Title = noticeEntity.Title;
                            siteModel.ConentEntity.Content = noticeEntity.Content;
                            siteModel.ConentEntity.ChannelID = noticeEntity.ChannelID;
                            siteModel.ConentEntity.PublishDate = noticeEntity.AddDate;
                        }
                    }
                }
            }
            ViewBag.Title = Utils.ClearHtml(siteModel.ConentEntity.Title);
            return View(siteModel);
        }

        # endregion

        # region list

        /// <summary>
        /// 显示指定的列表页
        /// </summary>
        /// <param name="TemplateID"></param>
        /// <param name="ChannelID"></param>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public ActionResult list(long ChannelID,int page=1)
        {
            ViewBag.ChannelName = "";
            ViewBag.PagerInfo = "";
            SiteModel siteModel = new SiteModel();

            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteChannelQuery query = new SiteChannelQuery();
            query.JournalID = JournalID;
            query.ChannelID = ChannelID;
            SiteChannelEntity channelEntity = service.GetSiteChannelModel(query);
            if (channelEntity != null)
            {
                ViewBag.ChannelName = channelEntity.Keywords;
                if (channelEntity.ContentType == (int)EnumContentType.Information)
                {
                    SiteContentQuery contentQuery = new SiteContentQuery();
                    contentQuery.ChannelID = ChannelID;
                    contentQuery.JournalID = JournalID;
                    contentQuery.CurrentPage = page;
                    contentQuery.PageSize = PageSize;
                    Pager<SiteContentEntity> pagerContentEntity = service.GetSiteContentPageList(contentQuery);
                    if (pagerContentEntity != null)
                    {
                        ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/list/" + ChannelID, 10);
                        foreach (SiteContentEntity item in pagerContentEntity.ItemList)
                        {
                            NewsModel newsItem = new NewsModel();
                            newsItem.Title = item.Title;
                            newsItem.Content = item.Content;
                            newsItem.ChannelID = ChannelID;
                            newsItem.PKID = item.ContentID;
                            newsItem.LinkUrl = string.Format("/show/{0}/?itemid={1}",item.ChannelID,item.ContentID);
                            newsItem.PublishDate = item.AddDate;
                            siteModel.ListConentEntity.Add(newsItem);
                        }
                    }
                }
                else if (channelEntity.ContentType == (int)EnumContentType.SiteDescribe)
                {
                    SiteNoticeQuery noticeQuery = new SiteNoticeQuery();
                    noticeQuery.ChannelID = ChannelID;
                    noticeQuery.JournalID = JournalID;
                    noticeQuery.CurrentPage = page;
                    noticeQuery.PageSize = PageSize;
                    Pager<SiteNoticeEntity> pagerNoticeEntity = service.GetSiteNoticePageList(noticeQuery);
                    if (pagerNoticeEntity != null)
                    {
                        ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerNoticeEntity.TotalPage, "/list/" + ChannelID, 10);
                        foreach (SiteNoticeEntity item in pagerNoticeEntity.ItemList)
                        {
                            NewsModel newsItem = new NewsModel();
                            newsItem.Title = item.Title;
                            newsItem.Content = item.Content;
                            newsItem.ChannelID = ChannelID;
                            newsItem.PKID = item.NoticeID;
                            newsItem.LinkUrl = string.Format("/show/{0}/?itemid={1}", item.ChannelID, item.NoticeID);
                            newsItem.PublishDate = item.AddDate;
                            siteModel.ListConentEntity.Add(newsItem);
                        }
                    }
                }
                else if (channelEntity.ContentType == (int)EnumContentType.Resources)
                {
                    SiteResourceQuery resourceQuery = new SiteResourceQuery();
                    resourceQuery.ChannelID = ChannelID;
                    resourceQuery.JournalID = JournalID;
                    resourceQuery.CurrentPage = page;
                    resourceQuery.PageSize = PageSize;
                    Pager<SiteResourceEntity> pagerResourceEntity = service.GetSiteResourcePageList(resourceQuery);
                    if (pagerResourceEntity != null)
                    {
                        ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerResourceEntity.TotalPage, "/list/" + ChannelID, 10);
                        foreach (SiteResourceEntity item in pagerResourceEntity.ItemList)
                        {
                            NewsModel newsItem = new NewsModel();
                            newsItem.Title = item.Name;
                            newsItem.LinkUrl = string.Format("/resource/?id={0}", item.ResourceID);
                            newsItem.PublishDate = item.AddDate;
                            siteModel.ListConentEntity.Add(newsItem);
                        }
                    }

                }
            }
            return View(siteModel);
        }

        # endregion

        # region channel

        /// <summary>
        /// 显示指定的频道页面
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public ActionResult channel(string ChannelUrl,int page=1)
        {
            ViewBag.Title = "";
            ViewBag.ChannelName = "";
            SiteModel siteModel = new SiteModel();
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteChannelQuery query = new SiteChannelQuery();
            query.JournalID = JournalID;
            query.ChannelUrl = ChannelUrl;
            SiteChannelEntity channelEntity = service.GetSiteChannelModel(query);
            if (channelEntity != null)
            {
                ViewBag.ChannelName = channelEntity.Keywords;
                if (channelEntity.ContentType == (int)EnumContentType.SiteDescribe)
                {
                    SiteNoticeQuery noticeQuery = new SiteNoticeQuery();
                    noticeQuery.ChannelID = channelEntity.ChannelID;
                    noticeQuery.JournalID = JournalID;
                    noticeQuery.CurrentPage = 1;
                    noticeQuery.PageSize = 1;
                    Pager<SiteNoticeEntity> pagerNoticeEntity = service.GetSiteNoticePageList(noticeQuery);
                    if (pagerNoticeEntity != null && pagerNoticeEntity.ItemList.Count > 0)
                    {
                        siteModel.ConentEntity.Title = pagerNoticeEntity.ItemList[0].Title;
                        siteModel.ConentEntity.Content = pagerNoticeEntity.ItemList[0].Content;
                        siteModel.ConentEntity.FJPath = pagerNoticeEntity.ItemList[0].FjPath;
                        siteModel.ConentEntity.PublishDate = pagerNoticeEntity.ItemList[0].AddDate;
                    }
                }
                else if (channelEntity.ContentType == (int)EnumContentType.Information)
                {
                    SiteContentQuery contentQuery = new SiteContentQuery();
                    contentQuery.ChannelID = channelEntity.ChannelID;
                    contentQuery.JournalID = JournalID;
                    contentQuery.CurrentPage = page;
                    contentQuery.PageSize = PageSize;
                    Pager<SiteContentEntity> pagerContentEntity = service.GetSiteContentPageList(contentQuery);
                   
                    if (pagerContentEntity != null)
                    {
                        ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/channel/" + channelEntity.ChannelID, 10);
                        siteModel.SiteContentList = new List<SiteContentEntity>();
                        foreach (SiteContentEntity item in pagerContentEntity.ItemList)
                        {
                            NewsModel newsItem = new NewsModel();
                            newsItem.Title = item.Title;
                            newsItem.Content = item.Content;
                            newsItem.ChannelID = channelEntity.ChannelID;
                            newsItem.PKID = item.ContentID;
                            newsItem.FJPath = item.FJPath;
                            newsItem.PublishDate = item.AddDate;
                            siteModel.ListConentEntity.Add(newsItem);
                            siteModel.SiteContentList.Add(item);
                        }
                    }
                }
            }
            ViewBag.Title = siteModel.ConentEntity.Title;
            if (channelEntity != null)
            {
                if (channelEntity.ContentType == (int)EnumContentType.Information && channelEntity.ChannelUrl=="list")
                {
                    return View("list", siteModel);
                }
            }
            return View(siteModel);
        }

        # endregion

        # region jpkc

        /// <summary>
        /// 显示指定的频道页面_精品课程
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public ActionResult jpkc(string ChannelUrl, int page = 1)
        {
            ViewBag.Title = "";
            ViewBag.ChannelName = "";
            SiteModel siteModel = new SiteModel();
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteChannelQuery query = new SiteChannelQuery();
            query.JournalID = JournalID;
            query.ChannelUrl = ChannelUrl;
            SiteChannelEntity channelEntity = service.GetSiteChannelModel(query);
            if (channelEntity != null)
            {
                ViewBag.ChannelName = channelEntity.Keywords;
                if (channelEntity.ContentType == (int)EnumContentType.SiteDescribe)
                {
                    SiteNoticeQuery noticeQuery = new SiteNoticeQuery();
                    noticeQuery.ChannelID = channelEntity.ChannelID;
                    noticeQuery.JournalID = JournalID;
                    noticeQuery.CurrentPage = 1;
                    noticeQuery.PageSize = 1;
                    Pager<SiteNoticeEntity> pagerNoticeEntity = service.GetSiteNoticePageList(noticeQuery);
                    if (pagerNoticeEntity != null && pagerNoticeEntity.ItemList.Count > 0)
                    {
                        siteModel.ConentEntity.Title = pagerNoticeEntity.ItemList[0].Title;
                        siteModel.ConentEntity.Content = pagerNoticeEntity.ItemList[0].Content;
                        siteModel.ConentEntity.PublishDate = pagerNoticeEntity.ItemList[0].AddDate;
                    }
                }
                else if (channelEntity.ContentType == (int)EnumContentType.Information)
                {
                    SiteContentQuery contentQuery = new SiteContentQuery();
                    contentQuery.ChannelID = channelEntity.ChannelID;
                    contentQuery.JournalID = JournalID;
                    contentQuery.CurrentPage = page;
                    contentQuery.PageSize = PageSize;
                    Pager<SiteContentEntity> pagerContentEntity = service.GetSiteContentPageList(contentQuery);

                    if (pagerContentEntity != null)
                    {
                        ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/jpkc/" + channelEntity.ChannelID, 10);
                        siteModel.SiteContentList = new List<SiteContentEntity>();
                        foreach (SiteContentEntity item in pagerContentEntity.ItemList)
                        {
                            NewsModel newsItem = new NewsModel();
                            newsItem.Title = item.Title;
                            newsItem.Content = item.Content;
                            newsItem.ChannelID = channelEntity.ChannelID;
                            newsItem.PKID = item.ContentID;
                            newsItem.PublishDate = item.AddDate;
                            siteModel.ListConentEntity.Add(newsItem);
                            siteModel.SiteContentList.Add(item);
                        }
                    }
                }
            }
            ViewBag.Title = siteModel.ConentEntity.Title;
            if (channelEntity != null)
            {
                if (channelEntity.ContentType == (int)EnumContentType.Information && channelEntity.ChannelUrl == "list")
                {
                    return View("list", siteModel);
                }
            }
            return View(siteModel);
        }

        # endregion

        # region resource

        /// <summary>
        /// 显示指定的频道页面
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public ActionResult resource()
        {
            long ItemID = TypeParse.ToLong(Request.QueryString["id"]);
            SiteResourceEntity contentEntity = new SiteResourceEntity();
            if (ItemID > 0)
            {
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                SiteResourceQuery resourceQuery = new SiteResourceQuery();
                resourceQuery.JournalID = JournalID;
                resourceQuery.ResourceID = ItemID;
                contentEntity = service.GetSiteResourceModel(resourceQuery);
            }
            ViewBag.Title = Utils.ClearHtml(contentEntity.Name);
            return View(contentEntity);
        }

        # endregion

        public ActionResult zjjy(string ChannelUrl, int page = 1)
        {
            ((dynamic)base.ViewBag).Title = "";
            ((dynamic)base.ViewBag).ChannelName = "";
            SiteModel model = new SiteModel();
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>(new ResolverOverride[0]);
            SiteChannelQuery query = new SiteChannelQuery
            {
                JournalID = base.JournalID,
                ChannelUrl = ChannelUrl
            };
            SiteChannelEntity siteChannelModel = service.GetSiteChannelModel(query);
            if (siteChannelModel != null)
            {
                ((dynamic)base.ViewBag).ChannelName = siteChannelModel.Keywords;
                if (siteChannelModel.ContentType == 4)
                {
                    SiteNoticeQuery query2 = new SiteNoticeQuery
                    {
                        ChannelID = siteChannelModel.ChannelID,
                        JournalID = base.JournalID,
                        CurrentPage = 1,
                        PageSize = 1
                    };
                    Pager<SiteNoticeEntity> siteNoticePageList = service.GetSiteNoticePageList(query2);
                    if ((siteNoticePageList != null) && (siteNoticePageList.ItemList.Count > 0))
                    {
                        model.ConentEntity.Title = siteNoticePageList.ItemList[0].Title;
                        model.ConentEntity.Content = siteNoticePageList.ItemList[0].Content;
                        model.ConentEntity.PublishDate = siteNoticePageList.ItemList[0].AddDate;
                    }
                }
                else if (siteChannelModel.ContentType == 1)
                {
                    SiteContentQuery query3 = new SiteContentQuery
                    {
                        ChannelID = new long?(siteChannelModel.ChannelID),
                        JournalID = base.JournalID,
                        CurrentPage = page,
                        PageSize = this.PageSize
                    };
                    Pager<SiteContentEntity> siteContentPageList = service.GetSiteContentPageList(query3);
                    if (siteContentPageList != null)
                    {
                        ((dynamic)base.ViewBag).PagerInfo = WKT.Common.Utils.Utils.GetPageNumbers(page, siteContentPageList.TotalPage, "/zjjy/" + siteChannelModel.ChannelID, 10);
                        model.SiteContentList = new List<SiteContentEntity>();
                        foreach (SiteContentEntity entity2 in siteContentPageList.ItemList)
                        {
                            NewsModel item = new NewsModel
                            {
                                Title = entity2.Title,
                                Content = entity2.Content,
                                ChannelID = siteChannelModel.ChannelID,
                                PKID = entity2.ContentID,
                                PublishDate = entity2.AddDate
                            };
                            model.ListConentEntity.Add(item);
                            model.SiteContentList.Add(entity2);
                        }
                    }
                }
            }
            ((dynamic)base.ViewBag).Title = model.ConentEntity.Title;
            if ((siteChannelModel != null) && ((siteChannelModel.ContentType == 1) && (siteChannelModel.ChannelUrl == "list")))
            {
                return base.View("list", model);
            }
            return base.View(model);
        }

        public ActionResult spjz(long ChannelID, int page = 1)
        {
            ViewBag.ChannelName = "";
            ViewBag.PagerInfo = "";
            SiteModel siteModel = new SiteModel();

            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteChannelQuery query = new SiteChannelQuery();
            query.JournalID = JournalID;
            query.ChannelID = ChannelID;
            SiteChannelEntity channelEntity = service.GetSiteChannelModel(query);
            if (channelEntity != null)
            {
                ViewBag.ChannelName = channelEntity.Keywords;
                if (channelEntity.ContentType == (int)EnumContentType.Information)
                {
                    SiteContentQuery contentQuery = new SiteContentQuery();
                    contentQuery.ChannelID = ChannelID;
                    contentQuery.JournalID = JournalID;
                    contentQuery.CurrentPage = page;
                    contentQuery.PageSize = PageSize;
                    Pager<SiteContentEntity> pagerContentEntity = service.GetSiteContentPageList(contentQuery);
                    if (pagerContentEntity != null)
                    {
                        ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/spjz/" + ChannelID, 10);
                        foreach (SiteContentEntity item in pagerContentEntity.ItemList)
                        {
                            NewsModel newsItem = new NewsModel();
                            newsItem.Title = item.Title;
                            newsItem.Content = item.Content;
                            newsItem.ChannelID = ChannelID;
                            newsItem.PKID = item.ContentID;
                            newsItem.FJPath = item.FJPath;
                            newsItem.LinkUrl = item.Linkurl;
                            newsItem.PublishDate = item.AddDate;
                            siteModel.ListConentEntity.Add(newsItem);
                        }
                    }
                }
                else if (channelEntity.ContentType == (int)EnumContentType.SiteDescribe)
                {
                    SiteNoticeQuery noticeQuery = new SiteNoticeQuery();
                    noticeQuery.ChannelID = ChannelID;
                    noticeQuery.JournalID = JournalID;
                    noticeQuery.CurrentPage = page;
                    noticeQuery.PageSize = PageSize;
                    Pager<SiteNoticeEntity> pagerNoticeEntity = service.GetSiteNoticePageList(noticeQuery);
                    if (pagerNoticeEntity != null)
                    {
                        ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerNoticeEntity.TotalPage, "/spjz/" + ChannelID, 10);
                        foreach (SiteNoticeEntity item in pagerNoticeEntity.ItemList)
                        {
                            NewsModel newsItem = new NewsModel();
                            newsItem.Title = item.Title;
                            newsItem.Content = item.Content;
                            newsItem.ChannelID = ChannelID;
                            newsItem.PKID = item.NoticeID;
                            newsItem.FJPath = item.FjPath;
                            newsItem.LinkUrl = string.Format("/show/{0}/?itemid={1}", item.ChannelID, item.NoticeID);
                            newsItem.PublishDate = item.AddDate;
                            siteModel.ListConentEntity.Add(newsItem);
                        }
                    }
                }
                else if (channelEntity.ContentType == (int)EnumContentType.Resources)
                {
                    SiteResourceQuery resourceQuery = new SiteResourceQuery();
                    resourceQuery.ChannelID = ChannelID;
                    resourceQuery.JournalID = JournalID;
                    resourceQuery.CurrentPage = page;
                    resourceQuery.PageSize = PageSize;
                    Pager<SiteResourceEntity> pagerResourceEntity = service.GetSiteResourcePageList(resourceQuery);
                    if (pagerResourceEntity != null)
                    {
                        ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerResourceEntity.TotalPage, "/spjz/" + ChannelID, 10);
                        foreach (SiteResourceEntity item in pagerResourceEntity.ItemList)
                        {
                            NewsModel newsItem = new NewsModel();
                            newsItem.Title = item.Name;
                            newsItem.LinkUrl = string.Format("/resource/?id={0}", item.ResourceID);
                            newsItem.PublishDate = item.AddDate;
                            siteModel.ListConentEntity.Add(newsItem);
                        }
                    }

                }
            }
            return View(siteModel);
        }

        # region stat

        public ActionResult Stat()
        {
            try
            {
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                SiteConfigQuery query = new SiteConfigQuery();
                query.JournalID = JournalID;
                service.UpdateSiteAccessCount(query);
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("记录访问次数出现异常：" + ex.Message);
            }
            return Content("");
        }

        # endregion
    }
}
