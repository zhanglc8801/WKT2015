using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using WKT.Common.Data;
using WKT.Log;
using System.Text;
using WKT.Common.Utils;
using WKT.Config;
using System.Threading;
using System.Xml.Linq;

namespace HanFang360.InterfaceService.Controllers
{
    public class IssueController : BaseController
    {
        private string CreateDoiRegFilePath = string.Empty;//生成的DOI注册用XML文件保存位置
        private string DoiRegResultFilePath = string.Empty;//系统返回的DOI注册结果保存位置

        #region 年卷设置
        public ActionResult YearVolume()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult GetYearVolumePageList(YearVolumeQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<YearVolumeEntity> pager = service.GetYearVolumePageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetYearVolumeList(YearVolumeQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<YearVolumeEntity> list = service.GetYearVolumeList(query);
            return Json(new { list });
        }
        
        public ActionResult DetailYearVolume(Int64 SetID = 0)
        {
            return View(GetYearVolumeModel(SetID));
        }

        private YearVolumeEntity GetYearVolumeModel(Int64 SetID)
        {
            YearVolumeQuery query = new YearVolumeQuery();
            query.JournalID = CurAuthor.JournalID;
            query.setID = SetID;
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            YearVolumeEntity model = service.GetYearVolumeModel(query);
            if (model == null)
                model = new YearVolumeEntity();
            return model;
        }

        public ActionResult CreateYearVolume(Int64 SetID = 0)
        {
            YearVolumeEntity model = GetYearVolumeModel(SetID);
            //if (SetID == 0)
            //    model.Status = 1;
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveYearVolume(YearVolumeEntity model)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            ExecResult result = service.SaveYearVolume(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult DelYearVolume(Int64[] SetIDs)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            YearVolumeQuery query = new YearVolumeQuery();
            query.JournalID = CurAuthor.JournalID;
            query.setIDs = SetIDs;
            ExecResult result = service.DelYearVolume(query);
            return Json(new { result = result.result, msg = result.msg });
        }
        #endregion

        #region 期设置
        public ActionResult IssueSet()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetIssueSetPageList(IssueSetQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<IssueSetEntity> pager = service.GetIssueSetPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetIssueSetList(IssueSetQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<IssueSetEntity> list = service.GetIssueSetList(query);
            return Json(new { list });
        }
        
        public ActionResult DetailIssueSet(Int64 IssueSetID = 0)
        {
            return View(GetIssueSetModel(IssueSetID,0));
        }

        private IssueSetEntity GetIssueSetModel(Int64 IssueSetID,int year)
        {
            IssueSetQuery query = new IssueSetQuery();
            query.JournalID = CurAuthor.JournalID;
            query.IssueSetID = IssueSetID;
            query.Year = year;
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueSetEntity model = service.GetIssueSetModel(query);
            if (model == null)
                model = new IssueSetEntity();
            return model;
        }

        public ActionResult CreateIssueSet(Int64 IssueSetID = 0,int year=0)
        {
            IssueSetEntity model = GetIssueSetModel(IssueSetID,year);
            if (IssueSetID == 0)
                model.Status = 1;
            ViewBag.year=year;

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveIssueSet(IssueSetEntity model)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            ExecResult result = service.SaveIssueSet(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult DelIssueSet(Int64[] IssueSetIDs)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueSetQuery query = new IssueSetQuery();
            query.JournalID = CurAuthor.JournalID;
            query.IssueSetIDs = IssueSetIDs;
            ExecResult result = service.DelIssueSet(query);
            return Json(new { result = result.result, msg = result.msg });
        }
        #endregion

        public IssueSiteEntity GetCurIssueInfo()
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueSetQuery query = new IssueSetQuery();
            query.JournalID = CurAuthor.JournalID;
            IssueSiteEntity issueInfoEntity = service.GetCurIssueInfo(query);
            if (issueInfoEntity == null)
            {
                issueInfoEntity = new IssueSiteEntity();
            }
            return issueInfoEntity;
        }

        #region 期刊栏目
        public ActionResult JournalChannel()
        {
            return View();
        }

        /// <summary>
        /// 获取菜单列表Ajax
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetJournalChannelTree(string defaultText)
        {
            JournalChannelQuery query = new JournalChannelQuery();
            query.JournalID = JournalID;
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            var list = service.GetJournalChannelTreeList(query);
            if (!string.IsNullOrWhiteSpace(defaultText))
                list[0].text = Server.UrlDecode(defaultText);
            return Content(JsonConvert.SerializeObject(list));
        }

        [HttpPost]
        public ActionResult GetJournalChannelPageList(JournalChannelQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<JournalChannelEntity> pager = service.GetJournalChannelPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetJournalChannelList(JournalChannelQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<JournalChannelEntity> list = service.GetJournalChannelList(query);
            return Json(new { list });
        }

        public ActionResult DetailJournalChannel(Int64 JChannelID = 0)
        {
            return View(GetJournalChannelModel(JChannelID));
        }

        private JournalChannelEntity GetJournalChannelModel(Int64 JChannelID)
        {
            JournalChannelEntity model = null;
            if (JChannelID > 0)
            {
                JournalChannelQuery query = new JournalChannelQuery();
                query.JournalID = CurAuthor.JournalID;
                query.JChannelID = JChannelID;
                IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
                model = service.GetJournalChannelModel(query);
            }
            if (model == null)
                model = new JournalChannelEntity();
            return model;
        }

        public ActionResult CreateJournalChannel(Int64 JChannelID = 0)
        {
            JournalChannelEntity model = GetJournalChannelModel(JChannelID);
            if (JChannelID == 0)
                model.Status = 1;
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveJournalChannel(JournalChannelEntity model)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            ExecResult result = service.SaveJournalChannel(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult DelJournalChannel(Int64 JChannelID)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            JournalChannelQuery query = new JournalChannelQuery();
            query.JournalID = CurAuthor.JournalID;
            query.JChannelID = JChannelID;
            ExecResult result = service.DelJournalChannel(query);
            return Json(new { result = result.result, msg = result.msg });
        }
        #endregion

        #region 期刊
        public ActionResult IssueContent()
        {
            return View();
        }

        public ActionResult IssuePush()
        {
            return View();
        }
        /// <summary>
        /// 获取期刊内容分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetIssueContentPageList(IssueContentQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            query.SortName = " EditDate ";
            query.SortOrder = " DESC ";
            if (Request.Params["sortname"] != null)
            {
                query.SortName = Request.Params["sortname"].ToString();
                query.SortOrder = Request.Params["sortorder"].ToString();
            }
            Pager<IssueContentEntity> pager = service.GetIssueContentPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 获取期刊内容分页列表(不包含已注册DOI的数据)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetIssueContentDoiPageList(IssueContentQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            query.SortName = " EditDate ";
            query.SortOrder = " DESC ";
            if (Request.Params["sortname"] != null)
            {
                query.SortName = Request.Params["sortname"].ToString();
                query.SortOrder = Request.Params["sortorder"].ToString();
            }
            Pager<IssueContentEntity> pager = service.GetIssueContentPageList(query);
            return Json(new { Rows = pager.ItemList.Where(p => p.isRegDoi == false), Total = pager.ItemList.Where(p => p.isRegDoi == false).Count() });
        }

        [HttpPost]
        public ActionResult GetIssueContentList(IssueContentQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<IssueContentEntity> list = service.GetIssueContentList(query);
            return Json(new { list });
        }
        

        
        public ActionResult DetailIssueContent(Int64 contentID = 0)
        {
            return View(GetIssueContentModel(contentID));
        }

        private IssueContentEntity GetIssueContentModel(Int64 contentID)
        {
            IssueContentEntity model = null;
            if (contentID > 0)
            {
                IssueContentQuery query = new IssueContentQuery();
                query.JournalID = CurAuthor.JournalID;
                query.contentID = contentID;
                query.IsAuxiliary = true;
                IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
                model = service.GetIssueContentModel(query);
            }
            if (model == null)
                model = new IssueContentEntity();
            return model;
        }

        public ActionResult CreateIssueContent(Int64 contentID = 0)
        {
            IssueContentEntity model = GetIssueContentModel(contentID);
            if(model.ReferenceList==null)
                model.ReferenceList = new List<IssueReferenceEntity>() { new IssueReferenceEntity() };
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveIssueContent(IssueContentEntity model)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            model.JournalID = CurAuthor.JournalID;
            model.InUser = CurAuthor.AuthorID;
            model.EditUser = CurAuthor.AuthorID;
            ExecResult result = service.SaveIssueContent(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult DelIssueContent(Int64[] ContentIDs)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = CurAuthor.JournalID;
            query.contentIDs = ContentIDs;
            ExecResult result = service.DelIssueContent(query);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost] 
        public ActionResult IssueListToExcel(IssueContentQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<IssueContentEntity> list = service.GetIssueContentList(query);
            for (int i = 0; i < list.Count;i++ )
            {
                list[i].Title = WKT.Common.Utils.Utils.ClearHtmlNbsp(list[i].Title);
                list[i].Authors = WKT.Common.Utils.Utils.ClearHtmlNbsp(list[i].Authors);
                list[i].FilePath = "http://"+WKT.Common.Utils.Utils.GetHost() + list[i].FilePath;
            }

            string[] titleFiles = new string[] { "标题", "年", "卷", "期", "作者", "关键字", "下载地址", "文件大小", "文件类型", "添加日期" };
            int[] titleWidth = new int[] { 300, 40, 40, 40, 100, 100, 200, 50, 50, 135 };
            string[] dataFiles = new string[] { "Title", "Year", "Volume", "Issue", "Authors", "Keywords", "FilePath", "FileSize", "FileExt", "AddDate" };
            string[] fomateFiles = new string[] { "", "", "", "", "", "", "", "", "", "{0:yyyy-MM-dd}" };
            string strTempPath = "/UploadFile/TempFile/" + "IssueList.xls";
            ExcelHelperEx.CreateExcel<IssueContentEntity>("期刊列表", titleFiles, titleWidth, dataFiles, fomateFiles, list, strTempPath);
            return Json(new { flag = 1, ExcelPath = strTempPath });
        }

        #endregion

        #region 期刊目录推送
        [HttpPost]
        public ActionResult IssuePush(IssueContentQuery query)
        {
            //获取模板文件内容
            string str = WKT.Common.Utils.Utils.ReadFileContent(Utils.GetMapPath(SiteConfig.RootPath + "/data/Issue.Template"));
            StringBuilder sb = new StringBuilder(str);

            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            //根据年获取卷
            YearVolumeQuery YVQuery = new YearVolumeQuery();
            YVQuery.JournalID = CurAuthor.JournalID;
            IList<YearVolumeEntity> YearVolList = service.GetYearVolumeList(YVQuery);
            int Volume = YearVolList.Single<YearVolumeEntity>(p => p.Year == query.Year).Volume;

            //根据查询条件获取期刊列表
            query.JournalID = CurAuthor.JournalID;
            IList<IssueContentEntity> list = service.GetIssueContentList(query);

            //替换模板文件基本内容
            str = str.Replace("%Year%", query.Year.ToString()).Replace("%Vol%", Volume.ToString()).Replace("%Issue%", query.Issue.ToString()).Replace("%SiteName%", SiteConfig.SiteName).Replace("%SiteURL%", "<a href=\"http://" + Request.Url.Host + "\" target=\"_blank\">http://" + Request.Url.Host + "</a>");
            //向模板文件加入期刊列表内容
            string listing = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                listing += "<tr>";
                listing += "<td style=\"text-align:center; width:50px; font-weight:bold;\">" + (i + 1) + "</td>";
                listing += "<td style=\"text-align:left; font-weight:bold;\"><a href=\"http://" + Request.Url.Host + "/Magazine/Show?id=" + list[i].ContentID + "\" target=\"_blank\">" + list[i].Title + "</a></td>";
                listing += "</tr>";
                listing += "<tr>";
                listing += "<td colspan=\"2\" style=\"text-align:left; padding-left:55px; font-size:12px;border-bottom:1px solid #e2e2e2;\">" + list[i].Authors + "</td>";
                listing += "</tr>";
            }
            str = str.Replace("%Content%", listing);
            //WKT.Common.Utils.Utils.WritStrToFile(str, @"D:\xx.htm");
            return Json(new { flag = 1, htmlContent = str });
        }


        #region 手动输入
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendIssueListInput(string RecUserName, string RecEmail, string Content)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            ExecResult result = new ExecResult();
            var strList = RecEmail.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
            Content = Content.Replace("%Name%", RecUserName);
            service.SendEmail("《" + SiteConfig.SiteName + "》期刊目录推送", Content, RecEmail, SiteConfig.SiteName, CurAuthor.JournalID);

            return Json(new { flag = 1 });
        } 
        #endregion

        #region 在系统中选择
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendIssueListSelectAuthor(string RecEmail, string Content)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            MessageRecodeEntity model = new MessageRecodeEntity();
            model.ReciveAddress = RecEmail;
            model.JournalID = CurAuthor.JournalID;
            model.MsgType = 1;
            model.MsgTitle = "《" + SiteConfig.SiteName + "》期刊目录推送";
            model.MsgContent = Content;

            ExecResult result = new ExecResult();
            var strList = RecEmail.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            IAuthorPlatformFacadeService AuthorService = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            AuthorDetailQuery query = new AuthorDetailQuery();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = 1;
            query.PageSize = 1;


            for (int i = 0; i < strList.Count; i++)
            {
                query.LoginName = strList[i];
                Pager<AuthorDetailEntity> pager = AuthorService.GetAuthorDetailPageList(query);
                if (i == 0)
                    Content = Content.Replace("%Name%", pager.ItemList[0].AuthorName);
                else
                {
                    query.LoginName = strList[i - 1];
                    Pager<AuthorDetailEntity> pagerTemp = AuthorService.GetAuthorDetailPageList(query);
                    Content = Content.Replace(pagerTemp.ItemList[0].AuthorName, pager.ItemList[0].AuthorName);
                }
                service.SendEmail("《" + SiteConfig.SiteName + "》期刊目录推送", Content, strList[i], SiteConfig.SiteName, CurAuthor.JournalID);
            }


            return Json(new { flag = 1 });
        }  
        #endregion
        #endregion


        //期刊DOI注册查询
        public ActionResult IssueDoiRegQuery(IssueContentQuery query)
        {
            ViewBag.Year = GetCurIssueInfo().Year;
            ViewBag.Issue = GetCurIssueInfo().Issue;
            return View();
        }
        [HttpPost]
        public ActionResult IssueDoiRegQueryAjax(Int64[] ContentIDs)
        {
            
            //获取期刊详细数据
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueContentQuery query = new IssueContentQuery();
            query.JournalID = CurAuthor.JournalID;
            query.contentIDs = ContentIDs;
            query.CurrentPage = 1;
            query.PageSize = 50;
            query.SortName = " EditDate ";
            query.SortOrder = " DESC ";
            IList<IssueContentEntity> list = service.GetIssueContentList(query);
            //获取站点配置信息
            SiteConfigQuery sitequery = new SiteConfigQuery();
            sitequery.JournalID = CurAuthor.JournalID;
            ISiteConfigFacadeService siteservice = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigEntity model = siteservice.GetSiteConfigModel(sitequery);
            //生成注册用XML文件
            CreateDoiRegFilePath = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xml";
            WKT.Common.Utils.DoiUtils.CreateDoiRegisterFile(Utils.GetMapPath(SiteConfig.RootPath + "/data/DoiRegister.xml"), Utils.GetMapPath(SiteConfig.RootPath + "/data/" + CreateDoiRegFilePath), model.DoiJournalID, model.DoiPrefix, model.Title, model.EnTitle, model.ISSN, model.CN, list[0].Year, list[0].Volume, list[0].Issue, list, model.Home);
            //上传注册用XML文件到中文DOI网站
            string ReturnXMLFile = WKT.Common.Utils.DoiUtils.Upload(Utils.GetMapPath(SiteConfig.RootPath + "/data/" + CreateDoiRegFilePath), model.DoiUserName, model.DoiUserPwd, 3);
            //获取DOI注册结果
            string DoiRegResult = WKT.Common.Utils.DoiUtils.GetDoiRegResult(ReturnXMLFile, model.DoiUserName, model.DoiUserPwd);

            //保存DOI注册日志
            DoiRegLogEntity doiRegLogEntity = new DoiRegLogEntity();
            doiRegLogEntity.JournalID = CurAuthor.JournalID;
            doiRegLogEntity.DoiFileName = ReturnXMLFile;
            doiRegLogEntity.State = DoiRegResult;
            doiRegLogEntity.Year = list[0].Year; ;
            doiRegLogEntity.Issue = list[0].Issue; ;
            doiRegLogEntity.AdminID = CurAuthor.AuthorID;
            if (service.SaveDoiRegLog(doiRegLogEntity).result == "success")
            {
                //删除生成的XML注册文件
                System.IO.File.Delete(Utils.GetMapPath(SiteConfig.RootPath + "/data/" + CreateDoiRegFilePath));
                return Json(new { flag = 1 });
            }
                
            else
                return Json(new { flag = 0 });
            
        }


        #region DOI注册日志
        public ActionResult IssueDoiRegLog()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDoiRegLogPageList(DoiRegLogQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);

            Pager<DoiRegLogEntity> pager = service.GetDoiRegLogPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        } 
        #endregion

        [HttpPost]
        public ActionResult ReCheckDoiStateAjax(Int64 PKID)
        {
            //获取站点配置信息
            SiteConfigQuery sitequery = new SiteConfigQuery();
            sitequery.JournalID = CurAuthor.JournalID;
            ISiteConfigFacadeService siteservice = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigEntity model = siteservice.GetSiteConfigModel(sitequery);
            //获取DOI注册日志实体
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            DoiRegLogQuery query = new DoiRegLogQuery();
            query.PKID = PKID;
            query.JournalID = CurAuthor.JournalID;
            string DoiFileName = service.GetDoiRegLog(query).DoiFileName.Trim();
            //CreateDoiRegFilePath = Utils.GetMapPath(SiteConfig.RootPath + "/data/" + DoiFileName);
            string DoiRegResult = WKT.Common.Utils.DoiUtils.GetDoiRegResult(DoiFileName, model.DoiUserName, model.DoiUserPwd);

            if (DoiRegResult == "已完成")
            {
                
                DoiRegLogEntity doiRegLogEntity = new DoiRegLogEntity();
                doiRegLogEntity.State = "已完成";
                doiRegLogEntity.PKID = PKID;
                doiRegLogEntity.isUpdate = true;
                service.SaveDoiRegLog(doiRegLogEntity);
                
                return Json(new { flag = 1 });
            }
            if (DoiRegResult == "不合格")
            {

                DoiRegLogEntity doiRegLogEntity = new DoiRegLogEntity();
                doiRegLogEntity.State = "不合格";
                doiRegLogEntity.PKID = PKID;
                doiRegLogEntity.isUpdate = true;
                service.SaveDoiRegLog(doiRegLogEntity);

                return Json(new { flag = 1 });
            }
            if (DoiRegResult == "未审核")
            {
                return Json(new { flag = -1 });
            }
            else
                return Json(new { flag = 0 });
        }

        [HttpPost]
        public ActionResult UpdateLocalDataAjax(Int64 PKID)
        {
            //获取站点配置信息
            SiteConfigQuery sitequery = new SiteConfigQuery();
            sitequery.JournalID = CurAuthor.JournalID;
            ISiteConfigFacadeService siteservice = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigEntity model = siteservice.GetSiteConfigModel(sitequery);
            //获取DOI注册日志实体
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            DoiRegLogQuery query = new DoiRegLogQuery();
            query.PKID = PKID;
            query.JournalID = CurAuthor.JournalID;
            string DoiFileName = service.GetDoiRegLog(query).DoiFileName.Trim();
            //保存DOI注册结果
            DoiRegResultFilePath = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xml";
            WKT.Common.Utils.DoiUtils.GetDoiRegResult(DoiFileName, model.DoiUserName, model.DoiUserPwd, Utils.GetMapPath(SiteConfig.RootPath + "/data/" + DoiRegResultFilePath));

            XElement root = XElement.Load(Utils.GetMapPath(SiteConfig.RootPath + "/data/" + DoiRegResultFilePath), LoadOptions.SetLineInfo);
            XElement journal = root.Element("body").Element("journal");

            var Articles = from Article in journal.Elements("journal_article")
                           select Article;

            foreach (var Article in Articles)
            {
                string doi = Article.Element("doi_data").Element("doi").Value;
                string resource = Article.Element("doi_data").Element("resource").Value;
                Int64 ContentID = Convert.ToInt64(resource.Substring(resource.LastIndexOf('=') + 1, resource.Length - resource.LastIndexOf('=') - 4));

                IIssueFacadeService IssueService = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
                IssueContentEntity issueContentEntity = new IssueContentEntity();
                //获取期刊实体
                IssueContentQuery IssueQuery = new IssueContentQuery();
                IssueQuery.JournalID = CurAuthor.JournalID;
                IssueQuery.contentID = ContentID;
                IssueQuery.IsAuxiliary = true;
                issueContentEntity=IssueService.GetIssueContentModel(IssueQuery);
                //更新期刊DOI数据
                issueContentEntity.DOI = doi;
                issueContentEntity.isRegDoi = true;
                ExecResult result = IssueService.SaveIssueContent(issueContentEntity);
            }
            //更新DOI注册日志
            DoiRegLogEntity doiRegLogEntity = new DoiRegLogEntity();
            doiRegLogEntity.State = "已更新";
            doiRegLogEntity.PKID = PKID;
            doiRegLogEntity.isUpdate = true;
            service.SaveDoiRegLog(doiRegLogEntity);
            //删除临时文件
            System.IO.File.Delete(Utils.GetMapPath(SiteConfig.RootPath + "/data/" + DoiRegResultFilePath));
            return Json(new { flag = 1 });

        }


        #region 期刊订阅
        public ActionResult IssueSubscribe()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetIssueSubscribePageList(IssueSubscribeQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<IssueSubscribeEntity> pager = service.GetIssueSubscribePageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetIssueSubscribeList(IssueSubscribeQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            IList<IssueSubscribeEntity> list = service.GetIssueSubscribeList(query);
            return Json(new { list });
        }

        public ActionResult DetailIssueSubscribe(Int64 SubscribeID = 0)
        {
            return View(GetIssueSubscribeModel(SubscribeID));
        }

        private IssueSubscribeEntity GetIssueSubscribeModel(Int64 subscribeID)
        {
            IssueSubscribeEntity model = null;
            if (subscribeID > 0)
            {
                IssueSubscribeQuery query = new IssueSubscribeQuery();
                query.JournalID = CurAuthor.JournalID;
                query.subscribeID = subscribeID;
                IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
                model = service.GetIssueSubscribeModel(query);
            }
            if (model == null)
                model = new IssueSubscribeEntity();
            return model;
        }

        public ActionResult CreateIssueSubscribe(Int64 subscribeID = 0)
        {
            return View(GetIssueSubscribeModel(subscribeID));
        }

        [HttpPost]
        public ActionResult SaveIssueSubscribe(IssueSubscribeEntity model)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            model.JournalID = CurAuthor.JournalID;           
            ExecResult result = service.SaveIssueSubscribe(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult DelIssueSubscribe(Int64[] SubscribeIDs)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueSubscribeQuery query = new IssueSubscribeQuery();
            query.JournalID = CurAuthor.JournalID;
            query.subscribeIDs = SubscribeIDs;
            ExecResult result = service.DelIssueSubscribe(query);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult IssueSubscribeToExcel(IssueSubscribeQuery query, string strDict)
        {
            try
            {
                strDict = Server.UrlDecode(strDict);
                JavaScriptSerializer s = new JavaScriptSerializer();
                Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(strDict);
                IDictionary<string, string> dict = ((object[])JsonData["dict"]).Select(p => (Dictionary<string, object>)p).ToDictionary(p => p["key"].ToString(), q => q["value"].ToString());
                IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
                query.JournalID = CurAuthor.JournalID;
                IList<IssueSubscribeEntity> list = service.GetIssueSubscribeList(query);
                if (list == null || list.Count <= 0)
                {
                    return Content("没有数据不能导出，请先进行查询！");
                }
                RenderToExcel.ExportListToExcel<IssueSubscribeEntity>(list, dict
                    , null
                    , "期刊订阅信息_导出" + DateTime.Now.ToString("yyyy-MM-dd"), false, "xls");
                return Content("导出成功！");
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出期刊订阅信息出现异常：" + ex.Message);
                return Content("导出期刊订阅信息异常！");
            }
        }
        #endregion

        #region 下载次数统计
        public ActionResult IssueDownLoad()
        {
            return View();
        }

        public ActionResult IssueDownLoadDetail(Int64 ContentID)
        {
            ViewBag.ContentID = ContentID;
            return View();
        }

        [HttpPost]
        public ActionResult GetIssueDownLogPageList(IssueDownLogQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.IsReport = false;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<IssueDownLogEntity> pager = service.GetIssueDownLogPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetIssueDownLogDetailPageList(IssueDownLogQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.IsReport = false;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<IssueDownLogEntity> pager = service.GetIssueDownLogDetailPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }
        #endregion

        #region 浏览次数统计
        public ActionResult IssueView()
        {
            return View();
        }

        public ActionResult IssueViewDetail(Int64 ContentID)
        {
            ViewBag.ContentID = ContentID;
            return View();
        }

        [HttpPost]
        public ActionResult GetIssueViewLogPageList(IssueViewLogQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.IsReport = false;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<IssueViewLogEntity> pager = service.GetIssueViewLogPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetIssueViewLogDetailPageList(IssueViewLogQuery query)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.IsReport = false;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<IssueViewLogEntity> pager = service.GetIssueViewLogDetailPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }
        #endregion

        [HttpPost]
        public ActionResult SendIssueToEmails(string ReciveUserStr, string Content)
        {
            return Json(new { flag = 1 });
        }

        [HttpPost]
        public ActionResult SendIssueToEmail(string UserName,string RecMail,string Content)
        {
            return Json(new { flag = 1 });
        }


    }
}
