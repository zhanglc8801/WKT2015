using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
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
using System.Text.RegularExpressions;

namespace Web.Site.Controllers
{
    public class MagazineController : BaseController
    {
        /// <summary>
        /// 期刊检索
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(IssueContentQuery contentQuery,int page=1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            contentQuery.JournalID = JournalID;
            contentQuery.CurrentPage = page;
            contentQuery.PageSize = 20;
            contentQuery.SortName = " Year,StartPageNum ";
            contentQuery.SortOrder = " ASC ";
            contentQuery.Authors = contentQuery.Authors == "作者姓名" ? "" : contentQuery.Authors;
            contentQuery.Keywords = contentQuery.Keywords == "关键词" ? "" : contentQuery.Keywords;
            contentQuery.Title = contentQuery.Title == "标题" ? "" : contentQuery.Title;
            contentQuery.WorkUnit = contentQuery.WorkUnit == "作者单位" ? "" : contentQuery.WorkUnit;
            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);
            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine?Year=" + contentQuery.Year + "&Issue=" + contentQuery.Issue + "&JChannelID=" + contentQuery.JChannelID, 10);
            }
            else
            {
                pagerContentEntity = new Pager<IssueContentEntity>();
            }
            return View(pagerContentEntity);
        }
        [HttpPost]
        public ActionResult GetIssueContentList(IssueContentQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = TypeParse.ToLong(ConfigurationManager.AppSettings["JournalID"]);//CurAuthor.JournalID;
            IList<IssueContentEntity> list = service.GetIssueContentList(query); 
            return Json(new { list });
        }

        public static string ClearHtml(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml))
            {
                return "";
            }
            else
            {
                Regex r = null;
                Match m = null;

                r = new Regex(@"<\/?[^>]*>", RegexOptions.IgnoreCase);
                for (m = r.Match(strHtml); m.Success; m = m.NextMatch())
                {
                    strHtml = strHtml.Replace(m.Groups[0].ToString(), "");
                }
            }
            return strHtml;
        }
        

        /// <summary>
        /// 历史下载排行
        /// </summary>
        /// <param name="contentQuery"></param>
        /// <param name="year1"></param>
        /// <param name="year2"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult DownOrder(IssueContentQuery contentQuery,int year1,int year2, int page = 1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            contentQuery.JournalID = JournalID;
            contentQuery.CurrentPage = page;
            contentQuery.PageSize = 20;
            contentQuery.SortName = " Downloads ";
            contentQuery.SortOrder = " DESC ";
            contentQuery.Year = year1;
            contentQuery.Volume = year2;
            contentQuery.Authors = contentQuery.Authors == "作者姓名" ? "" : contentQuery.Authors;
            contentQuery.Keywords = contentQuery.Keywords == "关键词" ? "" : contentQuery.Keywords;
            contentQuery.Title = contentQuery.Title == "标题" ? "" : contentQuery.Title;
            contentQuery.WorkUnit = contentQuery.WorkUnit == "作者单位" ? "" : contentQuery.WorkUnit;
            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);
            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine/DownOrder?year1=" + contentQuery.Year + "&year2=" + contentQuery.Volume + "&JChannelID=1", 10);
            }
            else
            {
                pagerContentEntity = new Pager<IssueContentEntity>();
            }
            return View(pagerContentEntity);
        }

        /// <summary>
        /// 历史点击排行
        /// </summary>
        /// <param name="contentQuery"></param>
        /// <param name="year1"></param>
        /// <param name="year2"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ShowOrder(IssueContentQuery contentQuery, int year1, int year2, int page = 1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            contentQuery.JournalID = JournalID;
            contentQuery.CurrentPage = page;
            contentQuery.PageSize = 20;
            contentQuery.SortName = " Hits ";
            contentQuery.SortOrder = " DESC ";
            contentQuery.Year = year1;
            contentQuery.Volume = year2;
            contentQuery.Authors = contentQuery.Authors == "作者姓名" ? "" : contentQuery.Authors;
            contentQuery.Keywords = contentQuery.Keywords == "关键词" ? "" : contentQuery.Keywords;
            contentQuery.Title = contentQuery.Title == "标题" ? "" : contentQuery.Title;
            contentQuery.WorkUnit = contentQuery.WorkUnit == "作者单位" ? "" : contentQuery.WorkUnit;
            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);
            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine/ShowOrder?year1=" + contentQuery.Year + "&year2=" + contentQuery.Volume + "&JChannelID=1", 10);
            }
            else
            {
                pagerContentEntity = new Pager<IssueContentEntity>();
            }
            return View(pagerContentEntity);
        }

        

        

        /// <summary>
        /// 期刊排序
        /// </summary>
        /// <returns></returns>
        public ActionResult ChannelList(int? OrderType,long JChannelID,int page = 1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery contentQuery = new IssueContentQuery();
            contentQuery.JournalID = JournalID;
            contentQuery.JChannelID = JChannelID;

            if (Convert.ToInt32(Request["Year"]) == 0)
                contentQuery.Year = null;
            else
                contentQuery.Year = Convert.ToInt32(Request["Year"]);

            if (Convert.ToInt32(Request["Issue"]) == 0)
                contentQuery.Issue = null;
            else
                contentQuery.Issue = Convert.ToInt32(Request["Issue"]);

            if (Request["Title"].ToString() == "0")
                contentQuery.Title = null;
            else
                contentQuery.Title = Request["Title"].ToString();

            if (Request["JChannelID"].ToString() == "0")
                contentQuery.JChannelID = null;
            else
                contentQuery.JChannelID = Convert.ToInt64(Request["JChannelID"]);
            contentQuery.CurrentPage = page;           
            contentQuery.SortName = " SortID ";
            contentQuery.SortOrder = " ASC ";
            contentQuery.PageSize = 20;

            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);

            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine/ChannelList/?Year=" + Convert.ToInt32(Request["Year"]) + "&Issue=" + Convert.ToInt32(Request["Issue"]) + "&JChannelID=" + JChannelID + "&Title=" + Request["Title"].ToString(), contentQuery.PageSize);
            }
            else
            {
                pagerContentEntity = new Pager<IssueContentEntity>();
            }  
            //ViewBag.Year = Year;
            //ViewBag.Issue = Issue;
            //ViewBag.journalChannelList = HtmlHelperExtensions.GeIssueContentList(1000, Issue, Year).Count;
            return View(pagerContentEntity);
        }

        /// <summary>
        /// 合并摘要
        /// </summary>
        /// <param name="OrderType"></param>
        /// <param name="JChannelID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult hbzy(int? OrderType, long JChannelID,string ContentID, int page = 1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery contentQuery = new IssueContentQuery();
            contentQuery.JournalID = JournalID;
            //contentQuery.JChannelID = JChannelID;
            contentQuery.Keywords = ContentID;

            if (Convert.ToInt32(Request["Year"]) == 0)
                contentQuery.Year = null;
            else
                contentQuery.Year = Convert.ToInt32(Request["Year"]);

            if (Convert.ToInt32(Request["Issue"]) == 0)
                contentQuery.Issue = null;
            else
                contentQuery.Issue = Convert.ToInt32(Request["Issue"]);

            if (Request["Title"].ToString() == "0")
                contentQuery.Title = null;
            else
                contentQuery.Title = Request["Title"].ToString();

            if (Request["JChannelID"].ToString() == "1")
                contentQuery.JChannelID = null;
            else
                contentQuery.JChannelID = Convert.ToInt64(Request["JChannelID"]);


            contentQuery.CurrentPage = page;
            if (OrderType == null | OrderType == 1)
            {
                contentQuery.SortName = " Hits ";
            }
            else
            {
                contentQuery.SortName = " Downloads ";
            }
            contentQuery.SortOrder = " DESC ";
            contentQuery.PageSize = 20;

            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);

            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine/hbzy/?Year=" + Convert.ToInt32(Request["Year"]) + "&Issue=" + Convert.ToInt32(Request["Issue"]) + "&JChannelID=" + JChannelID + "&Title=" + Request["Title"].ToString(), contentQuery.PageSize);
            }
            else
            {
                pagerContentEntity = new Pager<IssueContentEntity>();
            }
            //ViewBag.Year = Year;
            //ViewBag.Issue = Issue;
            //ViewBag.journalChannelList = HtmlHelperExtensions.GeIssueContentList(1000, Issue, Year).Count;
            return View(pagerContentEntity);
        }

        //专辑文章
        public ActionResult Album(int? OrderType, long JChannelID, int page = 1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery contentQuery = new IssueContentQuery();
            contentQuery.JournalID = JournalID;
            contentQuery.JChannelID = JChannelID;

            if (Convert.ToInt32(Request["Year"]) == 0)
                contentQuery.Year = null;
            else
                contentQuery.Year = Convert.ToInt32(Request["Year"]);

            if (Convert.ToInt32(Request["Issue"]) == 0)
                contentQuery.Issue = null;
            else
                contentQuery.Issue = Convert.ToInt32(Request["Issue"]);

            if (Request["Title"].ToString() == "0")
                contentQuery.Title = null;
            else
                contentQuery.Title = Request["Title"].ToString();

            if (Request["JChannelID"].ToString() == "2")
                contentQuery.Keywords = "271,272";
            else
                contentQuery.JChannelID = Convert.ToInt64(Request["JChannelID"]);
            contentQuery.CurrentPage = page;

            contentQuery.SortName = " Year DESC,Issue ";
              
            contentQuery.SortOrder = " DESC ";
            contentQuery.PageSize = 20;

            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);

            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine/ChannelList/?Year=" + Convert.ToInt32(Request["Year"]) + "&Issue=" + Convert.ToInt32(Request["Issue"]) + "&JChannelID=" + JChannelID + "&Title=" + Request["Title"].ToString(), contentQuery.PageSize);
            }
            else
            {
                pagerContentEntity = new Pager<IssueContentEntity>();
            }
            //ViewBag.Year = Year;
            //ViewBag.Issue = Issue;
            //ViewBag.journalChannelList = HtmlHelperExtensions.GeIssueContentList(1000, Issue, Year).Count;
            return View(pagerContentEntity);
        }


        public ActionResult Search(IssueContentQuery contentQuery, int page = 1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            contentQuery.JournalID = JournalID;
            contentQuery.CurrentPage = page;
            contentQuery.PageSize = 20;
            contentQuery.Authors = contentQuery.Authors == "作者姓名" ? "" : contentQuery.Authors;
            contentQuery.Keywords = contentQuery.Keywords == "关键词" ? "" : contentQuery.Keywords;
            contentQuery.Title = contentQuery.Title == "标题" ? "" : Request.QueryString["Title"];
            contentQuery.WorkUnit = contentQuery.WorkUnit == "作者单位" ? "" : contentQuery.WorkUnit;
            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);
            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine/Search/?Year=" + contentQuery.Year + "&Issue=" + contentQuery.Issue + "&JChannelID=" + contentQuery.JChannelID+"&Title="+contentQuery.Title, 10);
            }
            else
            {
                pagerContentEntity = new Pager<IssueContentEntity>();
            }
            ViewBag.Year = Request.QueryString["Year"] == "" ? 0 : int.Parse(Request.QueryString["Year"]);
            ViewBag.Issue = Request.QueryString["Issue"] == "" ? 0 : int.Parse(Request.QueryString["Issue"]);
            ViewBag.journalChannelList = HtmlHelperExtensions.GeIssueContentList(1000, ViewBag.Issue, ViewBag.Year).Count;
            return View(pagerContentEntity);
        }

        /// <summary>
        /// 查看期刊
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Show(long ID)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery contentQuery = new IssueContentQuery();
            contentQuery.JournalID = JournalID;
            contentQuery.contentID = ID;
            IssueContentEntity contentEntity = service.GetIssueContentModel(contentQuery);
            if (contentEntity == null)
            {
                contentEntity = new IssueContentEntity();
            }
            else
            {
                service.UpdateIssueContentHits(contentQuery);

                # region 记录浏览日志

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["RecodeViewLog"]))
                {
                    try
                    {
                        IssueViewLogEntity issueLogEntity = new IssueViewLogEntity();
                        issueLogEntity.ContentID = ID;
                        issueLogEntity.JournalID = JournalID;
                        issueLogEntity.IP = Utils.GetRealIP();
                        issueLogEntity.Daytime = TypeParse.ToInt(DateTime.Now.ToString("yyyyMMdd"));
                        issueLogEntity.Year = DateTime.Now.Year;
                        issueLogEntity.Month = DateTime.Now.Month;
                        issueLogEntity.AuthorID = 0;
                        service.SaveViewLog(issueLogEntity);
                    }
                    catch
                    {
                    }
                }
                # endregion
            }
            return View(contentEntity);
        }

        public ActionResult ShowDownloads(long ID)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery contentQuery = new IssueContentQuery();
            contentQuery.JournalID = JournalID;
            contentQuery.contentID = ID;
            IssueContentEntity contentEntity = service.GetIssueContentModel(contentQuery);
            if (contentEntity == null)
            {
                contentEntity = new IssueContentEntity();
            }
            else
            {
                service.UpdateIssueContentHits(contentQuery);
            }
            return View(contentEntity);
        }

        public ActionResult HtmlShow(long ID)
        {
            return View();
        }

        public ActionResult PDFShow(long ID)
        {
            return View();
        }
    }
}
