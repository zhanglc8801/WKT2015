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
using System.IO;
using System.Threading;

namespace Web.Site.Controllers
{
    /// <summary>
    /// 浏览下载统计
    /// </summary>
    public class StatController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 浏览日志
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult ViewStat(long CID)
        {
            return View();
        }

        /// <summary>
        /// 下载日志
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DownStat(long CID)
        {
            try
            {
                IssueContentQuery issueCQuery = new IssueContentQuery();
                issueCQuery.contentID = CID;
                issueCQuery.JournalID = JournalID;
                IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
                IssueContentEntity issueEntity = service.GetIssueContentModel(issueCQuery);
                if (issueEntity == null)
                {
                    return Json(new { flag = "NOfile" });
                }
                else
                {
                    if (!string.IsNullOrEmpty(issueEntity.FilePath))
                    {
                        string downPath = GetUploadPath(issueEntity.FilePath);
                        if (!System.IO.File.Exists(downPath))
                            return Json(new { flag = "NOfile" });
                        //更新下载次数
                        service.UpdateIssueContentDownloads(issueCQuery);
                        # region 记录下载日志

                        try
                        {
                            IssueDownLogEntity issueLogEntity = new IssueDownLogEntity();
                            issueLogEntity.ContentID = CID;
                            issueLogEntity.JournalID = JournalID;
                            //issueLogEntity.IP = Utils.GetRealIP();
                            issueLogEntity.Daytime = TypeParse.ToInt(DateTime.Now.ToString("yyyyMMdd"));
                            issueLogEntity.Year = DateTime.Now.Year;
                            issueLogEntity.Month = DateTime.Now.Month;
                            issueLogEntity.AuthorID = 0;

                            service.SaveDownloadLog(issueLogEntity);
                        }
                        catch
                        {
                        }

                        # endregion

                        return Json(new { flag = "success", URL = "http://" + Request.Url.Host + issueEntity.FilePath });
                    }
                    else
                    {
                        return Json(new { flag = "NOfile" });
                    }
                }
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("记录下载日志出现异常：" + ex.Message);

                return Json(new { flag = "记录下载日志出现异常：" + ex.Message });
            }
        }

        /// <summary>
        /// 下载日志
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult DownResource(long ID)
        {
            try
            {
                SiteResourceQuery resouceQuery = new SiteResourceQuery();
                resouceQuery.ResourceID = ID;
                resouceQuery.JournalID = JournalID;
                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                SiteResourceEntity resouceEntity = service.GetSiteResourceModel(resouceQuery);
                if (TicketTool.IsLogin())
                {
                    if (resouceEntity == null)
                    {
                        return Json(new { flag = "对不起，要下载的文档不存在" });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(resouceEntity.FilePath))
                        {
                            string downPath = GetUploadPath(resouceEntity.FilePath);
                            if (!System.IO.File.Exists(downPath))
                                return Json(new { flag = "文件不存在！" });
                            string fileName = System.IO.Path.GetFileName(downPath);
                            HttpResponseBase response = this.HttpContext.Response;
                            HttpServerUtilityBase server = this.HttpContext.Server;
                            response.Clear();
                            response.Buffer = true;
                            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                            response.ContentType = "application/octet-stream";
                            response.TransmitFile(downPath);
                            Response.Flush();
                            Response.Close();

                            return Json(new { flag = "下载成功！" });
                        }
                        else
                        {
                            return Json(new { flag = "对不起，要下载的文件不存在！" });
                        }
                    }
                }
                else
                {
                    return Json(new { flag = "error", URL = SiteConfig.RootPath + "/" });
                }
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("记录下载资源日志出现异常：" + ex.Message);

                return Json(new { flag = "记录下载日志出现异常：" + ex.Message });
            }
        }

        #region 查看RichHTML文件(已注释)
        /// <summary>
        /// 查看RichHTML文件(by zhanglc 20140225)
        /// </summary>
        /// <param name="ContentID"></param>
        /// <returns></returns>
        //[ValidateInput(false)]
        //public JsonResult ShowHtml(long ContentID)
        //{
        //    #region 登录验证(未使用)
        //    //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["WKT_SSO.CN"];
        //    //if (cookie != null)
        //    //{

        //    //} 
        //    #endregion
        //    IssueContentQuery issueCQuery = new IssueContentQuery();
        //    issueCQuery.contentID = ContentID;
        //    issueCQuery.JournalID = JournalID;
        //    IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
        //    IssueContentEntity issueEntity = service.GetIssueContentModel(issueCQuery);
        //    //string downPath = GetUploadPath(issueEntity.FilePath);
        //    if (issueEntity.HtmlPath.Length > 0)
        //    {
        //        //更新RichHTML浏览次数
        //        service.UpdateIssueContentHtmlHits(issueCQuery);
        //        return Json(new { flag = "success", URL = "http://" + Request.Url.Host + issueEntity.HtmlPath });
        //    }
        //    else
        //    {
        //        return Json(new { flag = "error" });//文件不存在
        //    }
        //}
        #endregion

        #region 查看RichHTML文件(直接打开)
        [ValidateInput(false)]
        public ActionResult ShowHtml(long ContentID)
        {
            #region 登录验证(未使用)
            //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["WKT_SSO.CN"];
            //if (cookie != null)
            //{

            //} 
            #endregion
            IssueContentQuery issueCQuery = new IssueContentQuery();
            issueCQuery.contentID = ContentID;
            issueCQuery.JournalID = JournalID;
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentEntity issueEntity = service.GetIssueContentModel(issueCQuery);
            //string downPath = GetUploadPath(issueEntity.FilePath);
            if (issueEntity != null && issueEntity.HtmlPath.Length > 0)
            {
                //更新RichHTML浏览次数
                service.UpdateIssueContentHtmlHits(issueCQuery);
                Response.Redirect("http://" + Request.Url.Host + issueEntity.HtmlPath);
                return null;
                //return Json(new { flag = "success", URL = "http://" + Request.Url.Host + issueEntity.HtmlPath });
            }
            else
            {
                Response.Write("<script>alert('RichHTML附件暂未上传!');</script>");
                return null;
            }
        }
        #endregion

        #region PDF文件下载
        /// <summary>
        /// PDF文件下载(by zhanglc 20140225)
        /// </summary>
        /// <param name="ContentID">文章ID</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult FileDownload(long ContentID, string fileName)
        {
            #region 登录验证(未使用)
            //HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["WKT_SSO.CN"];
            //if (cookie != null)
            //{

            //} 
            #endregion
            if (Utils.GetRealIP() == "60.220.197.252" || Utils.GetRealIP() == "218.26.232.181")
                return Content("检查到您的IP异常，已禁止下载。");

            long AuthorID = 0;
            if (Request.Cookies["WKT_PRELOGINUSERID"] != null)
                AuthorID = Convert.ToInt64(Request.Cookies["WKT_PRELOGINUSERID"].Value);
            IssueContentQuery issueCQuery = new IssueContentQuery();
            issueCQuery.contentID = ContentID;
            issueCQuery.JournalID = JournalID;
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentEntity issueEntity = service.GetIssueContentModel(issueCQuery);
            string downPath = GetUploadPath(issueEntity.FilePath);
            if (!System.IO.File.Exists(downPath))
                return Content("文件不存在！");
            fileName += Path.GetExtension(issueEntity.FilePath);
            HttpResponseBase response = this.HttpContext.Response;
            HttpServerUtilityBase server = this.HttpContext.Server;
            response.Clear();
            response.Buffer = true;
            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            response.ContentType = "application/octet-stream";
            response.TransmitFile(downPath);

            Response.Flush();
            response.End();
            //Response.Close();

            //根据客户端IP、期刊ID获取下载记录数
            IIssueFacadeService issueService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueDownLogQuery issueQuery = new IssueDownLogQuery();
            issueQuery.JournalID = JournalID;
            issueQuery.ContentID = ContentID;
            issueQuery.IP = Utils.GetRealIP();
            issueQuery.IsReport = false;
            issueQuery.CurrentPage = 1;
            issueQuery.PageSize = 25;

            if (issueService.GetIssueDownLogDetailPageList(issueQuery).TotalRecords == 0)
            {
                //更新下载次数
                service.UpdateIssueContentDownloads(issueCQuery);
                # region 记录下载日志

                try
                {
                    IssueDownLogEntity issueLogEntity = new IssueDownLogEntity();
                    issueLogEntity.ContentID = ContentID;
                    issueLogEntity.JournalID = JournalID;
                    issueLogEntity.IP = Utils.GetRealIP();
                    issueLogEntity.Daytime = TypeParse.ToInt(DateTime.Now.ToString("yyyyMMdd"));
                    issueLogEntity.Year = DateTime.Now.Year;
                    issueLogEntity.Month = DateTime.Now.Month;
                    issueLogEntity.AuthorID = AuthorID;
                    service.SaveDownloadLog(issueLogEntity);
                }
                catch
                {
                }

                # endregion
            }
            return Content("文件下载成功");

        }



        #endregion


        #region 获取完整路径
        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetUploadPath(string path)
        {
            string folder = SiteConfig.UploadPath;
            string uploadPath;
            if (!string.IsNullOrWhiteSpace(folder))
            {
                uploadPath = folder + path.Replace("/", "\\");
            }
            else
            {
                uploadPath = Server.MapPath(path);
            }
            return uploadPath;
        }
        #endregion


    }
}
