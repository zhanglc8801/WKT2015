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

namespace Web.Mobile.Controllers
{
    public class MagazineController : BaseController
    {
        /// <summary>
        /// 期刊检索
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(IssueContentQuery contentQuery, int page = 1)
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
            query.JournalID = SiteConfig.SiteID;
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
        public ActionResult DownOrder(IssueContentQuery contentQuery, int year1, int year2, int page = 1)
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




        public ActionResult List(IssueContentQuery contentQuery, int page = 1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            contentQuery.JournalID = JournalID;
            contentQuery.CurrentPage = page;
            contentQuery.PageSize = 10;
            contentQuery.Authors = contentQuery.Authors == "作者姓名" ? "" : contentQuery.Authors;
            contentQuery.Keywords = contentQuery.Keywords == "关键词" ? "" : contentQuery.Keywords;
            contentQuery.Title = contentQuery.Title == "标题" ? "" : Request.QueryString["Title"];
            contentQuery.WorkUnit = contentQuery.WorkUnit == "作者单位" ? "" : contentQuery.WorkUnit;
            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);
            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine/List/?Year=" + contentQuery.Year + "&Issue=" + contentQuery.Issue + "&JChannelID=" + contentQuery.JChannelID + "&Title=" + contentQuery.Title, 10);
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

        //期刊检索
        public ActionResult SearchList(IssueContentQuery contentQuery, int page = 1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            contentQuery.JournalID = JournalID;
            contentQuery.CurrentPage = page;
            contentQuery.PageSize = 10;
            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);
            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine/SearchList/?Year=" + contentQuery.Year + "&Issue=" + contentQuery.Issue + "&JChannelID=" + contentQuery.JChannelID + "&Title=" + contentQuery.Title, 10);
            }
            else
            {
                pagerContentEntity = new Pager<IssueContentEntity>();
            }
            ViewBag.Year = contentQuery.Year;
            ViewBag.Issue = contentQuery.Issue;
            //ViewBag.journalChannelList = HtmlHelperExtensions.GeIssueContentList(1000, ViewBag.Issue, ViewBag.Year).Count;
            return View(pagerContentEntity);
        }

        //本期浏览排行
        public ActionResult curList(IssueContentQuery contentQuery, int page = 1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            contentQuery.JournalID = JournalID;
            contentQuery.CurrentPage = page;
            contentQuery.SortName = " Hits ";
            contentQuery.SortOrder = " DESC ";
            contentQuery.PageSize = 10;
            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);
            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine/curList/?Year=" + contentQuery.Year + "&Issue=" + contentQuery.Issue, 10);
            }
            else
            {
                pagerContentEntity = new Pager<IssueContentEntity>();
            }
            ViewBag.Year = contentQuery.Year;
            ViewBag.Issue = contentQuery.Issue;
            //ViewBag.journalChannelList = HtmlHelperExtensions.GeIssueContentList(1000, ViewBag.Issue, ViewBag.Year).Count;
            return View(pagerContentEntity);
        }
        //本期下载排行
        public ActionResult curDown(IssueContentQuery contentQuery, int page = 1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            contentQuery.JournalID = JournalID;
            contentQuery.CurrentPage = page;
            contentQuery.SortName = " Downloads ";
            contentQuery.SortOrder = " DESC ";
            contentQuery.PageSize = 10;
            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);
            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine/curDown/?Year=" + contentQuery.Year + "&Issue=" + contentQuery.Issue, 10);
            }
            else
            {
                pagerContentEntity = new Pager<IssueContentEntity>();
            }
            ViewBag.Year = contentQuery.Year;
            ViewBag.Issue = contentQuery.Issue;
            return View(pagerContentEntity);
        }

        //历史点击排行
        public ActionResult oldList(IssueContentQuery contentQuery, int page = 1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            contentQuery.JournalID = JournalID;
            contentQuery.CurrentPage = page;
            contentQuery.SortName = " Downloads ";
            contentQuery.SortOrder = " DESC ";
            contentQuery.PageSize = 10;
            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);
            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine/oldList/", 10);
            }
            else
            {
                pagerContentEntity = new Pager<IssueContentEntity>();
            }
            return View(pagerContentEntity);
        }

        //历史下载排行
        public ActionResult oldDown(IssueContentQuery contentQuery, int page = 1)
        {
            IIssueFacadeService magazineService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            contentQuery.JournalID = JournalID;
            contentQuery.CurrentPage = page;
            contentQuery.SortName = " Downloads ";
            contentQuery.SortOrder = " DESC ";
            contentQuery.PageSize = 10;
            Pager<IssueContentEntity> pagerContentEntity = magazineService.GetIssueContentPageList(contentQuery);
            if (pagerContentEntity != null)
            {
                ViewBag.PagerInfo = Utils.GetPageNumbers(page, pagerContentEntity.TotalPage, "/Magazine/oldDown/", 10);
            }
            else
            {
                pagerContentEntity = new Pager<IssueContentEntity>();
            }
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
