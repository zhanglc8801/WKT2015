using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Common.Xml;
using WKT.Common.Utils;
using WKT.Config;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;
using WKT.Log;
using WKT.Common.Data;

namespace Web.Admin.Controllers
{
    public class ContributionController : BaseController
    {
        // 投稿自动分配
        public ActionResult AutoAllot()
        {
            string ss = System.DateTime.Now.ToString("yymdd");
            EditorAutoAllotQuery query = new EditorAutoAllotQuery();
            query.JournalID = JournalID;
            IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
            EditorAutoAllotEntity autoAllotEntity = service.GetAllowAllotInfo(query);
            return View(autoAllotEntity);
        }

        /// <summary>
        /// 异常稿件处理
        /// </summary>
        /// <returns></returns>
        public ActionResult ExceptionContribution()
        {
            return View();
        }

        /// <summary>
        /// 设置年卷期
        /// </summary>
        public ActionResult SetYearIssue()
        {
            return View();
        }

        /// <summary>
        /// 合并稿件状态
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult MergeStatus(long CID)
        {
            ViewBag.CID = CID;
            return View();
        }

        /// <summary>
        /// 稿件状态列表
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult ContributionDialog(long StatusID)
        {
            ViewBag.StatusID = StatusID;
            return View();
        }

        # region 稿件下载

        /// <summary>
        /// 稿件下载
        /// </summary>
        /// <param name="CType">稿件类型：Flow：流程附件 为空则为稿件附件</param>
        /// <param name="CID">稿件ID</param>
        /// <param name="AttType">附件类型，当为流程附件时可以指定下载哪个附件，附件一：CPath附件二：FigurePath附件三：OtherPath</param>
        /// <returns></returns>
        public ActionResult Download(string CType, long CID, string AttType)
        {
            string Message = "";
            string filePath = GetFilePath(CType, CID, AttType, ref Message);
            
                if (!System.IO.File.Exists(filePath))
                {
                    Message = "文件不存在";
                }
                else
                {
                    string fileName = Path.GetFileName(filePath);
                    HttpResponseBase response = this.HttpContext.Response;
                    HttpServerUtilityBase server = this.HttpContext.Server;
                    response.Clear();
                    response.Buffer = true;
                    if (Request.Browser.Browser == "IE")
                    {
                        response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                    }
                    else
                    {
                        response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
                    }
                    response.ContentType = "application/octet-stream";
                    response.TransmitFile(filePath);
                    Response.Flush();
                    Response.Close();
                }
            
            return Content(Message);
        }
        private string GetFilePath(string CType, long CID, string AttType, ref string Message)
        {
            string _filePath = "";
            // 根据稿件ID或者流程日志ID得到稿件路径
            if (CID > 0)
            {
                if (CType.Equals("Flow", StringComparison.CurrentCultureIgnoreCase))
                {
                    IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                    // 附件路径
                    FlowLogQuery logQuery = new FlowLogQuery();
                    logQuery.JournalID = SiteConfig.SiteID;
                    logQuery.CID = CID;
                    IDictionary<string, string> dictPath = flowService.GetFlowLogAllAttachment(logQuery);
                    if (dictPath.Count == 1)
                    {
                        _filePath = dictPath[dictPath.Keys.Single().ToLower()].ToString();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(AttType))
                        {
                            _filePath = dictPath[AttType.ToLower()].ToString();
                        }
                        else
                        {
                            StringBuilder sbMessage = new StringBuilder();
                            foreach (string key in dictPath.Keys)
                            {
                                if (key.Equals("CPath", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    sbMessage.AppendFormat("<a href=\"{0}/Contribution/Download/?CID={1}&CType=Flow&AttType=CPath\">稿件附件下载</a><br/>", SiteConfig.RootPath, CID);
                                }
                                else if (key.Equals("FigurePath", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    sbMessage.AppendFormat("<a href=\"{0}/Contribution/Download/?CID={1}&CType=Flow&AttType=FigurePath\">附件下载</a><br/>", SiteConfig.RootPath, CID);
                                }
                                else if (key.Equals("OtherPath", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    sbMessage.AppendFormat("<a href=\"{0}/Contribution/Download/?CID={1}&CType=Flow&AttType=OtherPath\">其他附件下载</a><br/>", SiteConfig.RootPath, CID);
                                }
                            }
                            Message = sbMessage.ToString();
                        }
                    }
                }
                else
                {
                    IContributionFacadeService cService = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                    // 得到稿件路径
                    ContributionInfoQuery cQuery = new ContributionInfoQuery();
                    cQuery.JournalID = SiteConfig.SiteID;
                    cQuery.CID = CID;
                    _filePath = cService.GetContributionAttachment(cQuery);
                }
            }
            else
            {
                Message = "请指定正确的稿件ID";
            }
            string folder = SiteConfig.UploadPath;
            string uploadPath;
            if (!string.IsNullOrWhiteSpace(folder))
            {
                uploadPath = folder + _filePath.Replace("/", "\\");
            }
            else
            {
                uploadPath = Server.MapPath(_filePath);
            }
            return uploadPath;
        }

        # endregion

        # region 设置年卷期

        /// <summary>
        /// 设置年卷期
        /// </summary>
        /// <param name="autoAllotEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult SetYearVolumnIssueAjax(IssueContentQuery query)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                query.JournalID = JournalID;
                IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
                execResult = service.SetContributionYearIssue(query);
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "设置稿件的年卷期出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("设置稿件的年卷期出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion

        # region 稿件状态合并

        /// <summary>
        /// 获取指定稿件的状态列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetContributionFlowStatusListAjax(FlowLogInfoQuery query)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            query.JournalID = JournalID;
            List<FlowContribution> listResult = service.GetContributionMoreStatusList(query);
            return Json(new { Rows = listResult });
        }

        /// <summary>
        /// 稿件状态合并
        /// </summary>
        /// <param name="flowLogQuery"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult MergeStatusAjax(FlowLogInfoQuery flowLogQuery)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                flowLogQuery.JournalID = JournalID;
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                execResult = service.MergeMoreStatus(flowLogQuery);
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "稿件状态合并出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("稿件状态合并出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        /// <summary>
        /// 稿件状态合并
        /// </summary>
        /// <param name="flowLogQuery"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult JudgeIsMoreStatusAjax(FlowLogInfoQuery flowLogQuery)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                flowLogQuery.JournalID = JournalID;
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                long flag = service.JudgeIsMoreStatus(flowLogQuery);
                if (flag > 0)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "该稿件是单一状态，不用合并";
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "判断稿件多状态出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("判断稿件多状态出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion

        # region 投稿自动分配设置

        /// <summary>
        /// 投稿自动分配设置
        /// </summary>
        /// <param name="autoAllotEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult SetAutoAllotAjax(EditorAutoAllotEntity autoAllotEntity)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                autoAllotEntity.JournalID = JournalID;
                IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                // 如果是按照学科分类进行设置
                if (autoAllotEntity.SubjectAuthorMap != null && autoAllotEntity.AllotPattern == 1)
                {
                    foreach (SubjectAuthorMapEntity item in autoAllotEntity.SubjectAuthorMap)
                    {
                        item.JournalID = JournalID;
                    }
                }
                execResult = service.SetAllowAllot(autoAllotEntity);
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "设置投稿自动分配出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("设置投稿自动分配出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion

        # region 投稿设置

        /// <summary>
        /// 稿件编号设置
        /// </summary>
        /// <returns></returns>
        public ActionResult NumberSet()
        {

            IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
            QueryBase query = new QueryBase();
            query.JournalID = JournalID;
            ContributeSetEntity cSetEntity = service.GetContributeSetInfo(query);
            if (cSetEntity == null)
            {
                cSetEntity = new ContributeSetEntity();
            }
            return View(cSetEntity);
        }

        /// <summary>
        /// 投稿声明
        /// </summary>
        /// <returns></returns>
        public ActionResult Statement()
        {
            IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
            QueryBase query = new QueryBase();
            query.JournalID = JournalID;
            ContributeSetEntity cSetEntity = service.GetContributeSetInfo(query);
            if (cSetEntity == null)
            {
                cSetEntity = new ContributeSetEntity();
            }
            return View(cSetEntity);
        }

        /// <summary>
        /// 投稿字段设置
        /// </summary>
        /// <returns></returns>
        public ActionResult FieldsSet()
        {
            return View();
        }

        # region ajax

        /// <summary>
        /// 稿件编号格式设置
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult SetNumberFormatAjax(ContributeSetEntity cSetEntity)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                cSetEntity.JournalID = JournalID;
                IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                execResult = service.SetContributeNumberFormat(cSetEntity);
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "设置稿件格式出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("设置稿件格式出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        /// <summary>
        /// 投稿声明设置
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetStatementAjax(ContributeSetEntity cSetEntity)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                cSetEntity.JournalID = JournalID;
                IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                execResult = service.SetContruibuteStatement(cSetEntity);
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "设置投稿声明出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("设置投稿声明出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        /// <summary>
        /// 得到稿件字段设置
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult GetFieldsAjax()
        {
            try
            {
                IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                var result = new { Rows = service.GetFieldsSet() };
                return Content(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("获取投稿字段设置出现异常：" + ex.Message);
                return Content("获取投稿字段设置出现异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 得到稿件作者字段设置
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult GetContributionAuthorFieldsAjax()
        {
            try
            {
                IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                var result = new { Rows = service.GetContributionAuthorFieldsSet() };
                return Content(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("获取稿件作者字段设置出现异常：" + ex.Message);
                return Content("获取稿件作者字段设置出现异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 保存稿件字段设置
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult SaveFieldsSetAjax(List<FieldsSet> FieldsArray)
        {
            ExecResult execResult = new ExecResult();
            if (FieldsArray != null)
            {
                if (FieldsArray.Count() > 0)
                {
                    if (string.IsNullOrEmpty(FieldsArray[0].DisplayName))
                    {
                        execResult.result = EnumJsonResult.error.ToString();
                        execResult.msg = "具体的值没有取到";
                    }
                    else
                    {
                        IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                        execResult = service.SetFields(FieldsArray);
                    }
                }
                else
                {
                    execResult.result = EnumJsonResult.error.ToString();
                    execResult.msg = "接收到的参数个数为0";
                }
            }
            else
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "接收到的参数为空";
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        /// <summary>
        /// 保存稿件作者字段设置
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult SaveContributionAuthorFieldsSetAjax(List<FieldsSet> FieldsArray)
        {
            ExecResult execResult = new ExecResult();
            if (FieldsArray != null)
            {
                if (FieldsArray.Count() > 0)
                {
                    if (string.IsNullOrEmpty(FieldsArray[0].DisplayName))
                    {
                        execResult.result = EnumJsonResult.error.ToString();
                        execResult.msg = "具体的值没有取到";
                    }
                    else
                    {
                        IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                        execResult = service.SetContributionAuthorFields(FieldsArray);
                    }
                }
                else
                {
                    execResult.result = EnumJsonResult.error.ToString();
                    execResult.msg = "接收到的参数个数为0";
                }
            }
            else
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "接收到的参数为空";
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion

        # endregion

        #region 稿件处理专区

        # region 稿签打印

        /// <summary>
        /// 打印信封
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult PrintSign(string CIDS)
        {
            if (string.IsNullOrWhiteSpace(CIDS))
            {
                ViewBag.SignContent = "请选择需要打印稿签的稿件";
                return View();
            }

            Int64[] CIDArray = CIDS.Split(',').Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => TypeParse.ToLong(p)).ToArray();

            if (CIDArray == null || CIDArray.Length == 0)
            {
                ViewBag.SignContent = "请选择需要打印稿签的稿件";
                return View();
            }

            //获取模版
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Utils.GetMapPath(SiteConfig.RootPath + "/data/signtemplate.config"));
            XmlNode signNode = xmlDoc.SelectSingleNode("ContributionSign");

            //获取数据
            IAuthorPlatformFacadeService cService = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            ContributionInfoQuery query = new ContributionInfoQuery();
            query.JournalID = CurAuthor.JournalID;
            query.CIDs = CIDArray;
            query.IsAuxiliary = true;
            IList<ContributionInfoEntity> list = cService.GetContributionInfoList(query);

            if (list == null || list.Count == 0)
            {
                ViewBag.SignContent = "获取稿件信息失败";
                return View();
            }

            ISiteConfigFacadeService siteConfigService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigEntity currentEntity = siteConfigService.GetSiteConfigModel(new SiteConfigQuery() { JournalID = CurAuthor.JournalID });
            StringBuilder strHtml = new StringBuilder();
            foreach (var model in list)
            {
                var linkAuthor = model.AuthorList.SingleOrDefault(p => p.IsCommunication == true);
                var firstAuthor = model.AuthorList.SingleOrDefault(p => p.IsFirst == true);
                if (linkAuthor == null)
                {
                    linkAuthor = new ContributionAuthorEntity();
                }
                if (firstAuthor == null)
                {
                    firstAuthor = new ContributionAuthorEntity();
                }
                strHtml.Append(signNode.InnerText
                     .Replace("${审稿单}$", currentEntity != null ? currentEntity.Title + "稿签" : "稿签")
                    .Replace("${稿件编号}$", model.CNumber)
                    .Replace("${投稿日期}$", model.AddDate.ToString("yyyy-MM-dd"))
                    .Replace("${期刊栏目}$", model.JChannelName)
                    .Replace("${中文标题}$", model.Title)
                    .Replace("${英文标题}$", model.EnTitle)
                    .Replace("${关键词}$", model.Keywords)
                    .Replace("${作者}$", firstAuthor.AuthorName)
                    .Replace("${联系人}$", linkAuthor.AuthorName)
                    .Replace("${联系人电子邮件}$", linkAuthor.Email)
                    .Replace("${联系人电话}$", linkAuthor.Tel)
                    .Replace("${联系人地址}$", linkAuthor.Address)
                    .Replace("${联系人邮编}$", linkAuthor.ZipCode)
                    .Replace("${联系人单位}$", linkAuthor.WorkUnit)
                    );
            }
            ViewBag.SignContent = strHtml.ToString();
            return View();
        }

        [HttpPost]
        public ActionResult SignToWord(string html)
        {
            try
            {
                RenderToWord.HtmlToWord(Server.UrlDecode(html), string.Format("稿签_{0}.doc", DateTime.Now.ToString("yyyyMMdd")), true);
                return Content("导出成功！");
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出稿签出现异常：" + ex.Message);
                return Content("导出稿签异常:" + ex.Message);
            }
        }

        # endregion

        # region 打印信封

        /// <summary>
        /// 打印信封
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult PrintEnvelope(string AuthorIDStr)
        {
            if (string.IsNullOrWhiteSpace(AuthorIDStr))
            {
                ViewBag.EnvelopeContent = "请选择需要打印的稿件信息";
                return View();
            }

            Int64[] AuthorIDs = AuthorIDStr.Split(',').Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => TypeParse.ToLong(p)).ToArray();

            if (AuthorIDs == null || AuthorIDs.Length == 0)
            {
                ViewBag.EnvelopeContent = "请选择需要打印的稿件信息";
                return View();
            }

            //获取模版
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Utils.GetMapPath(SiteConfig.RootPath + "/data/envelopetemplate.config"));
            XmlNode envelopeNode = xmlDoc.SelectSingleNode("envelope");

            //获取站点信息
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigEntity siteModel = service.GetSiteConfigModel(new SiteConfigQuery() { JournalID = CurAuthor.JournalID });
            if (siteModel == null)
            {
                ViewBag.EnvelopeContent = "请配置站点信息";
                return View();
            }

            //获取数据
            IAuthorPlatformFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            AuthorDetailQuery query = new AuthorDetailQuery();
            query.JournalID = CurAuthor.JournalID;
            query.AuthorIDs = AuthorIDs;
            IList<AuthorDetailEntity> list = authorService.GetAuthorDetailList(query);

            if (list == null || list.Count == 0)
            {
                ViewBag.EnvelopeContent = "获取接收人信息失败";
                return View();
            }

            //替换内容
            StringBuilder strHtml = new StringBuilder();
            foreach (var model in list)
            {
                strHtml.Append(envelopeNode.InnerText
                    .Replace("${接收邮编}$", model.ZipCode)
                    .Replace("${接收地址}$", model.Address)
                    .Replace("${接收人}$", model.AuthorModel.RealName)
                    .Replace("${编辑部地址}$", siteModel.Address)
                    .Replace("${站点名称}$", siteModel.Title)
                    .Replace("${编辑部邮编}$", siteModel.ZipCode));
            }

            ViewBag.EnvelopeContent = strHtml.ToString();
            return View();
        }

        [HttpPost]
        public ActionResult EnvelopeToWord(string html)
        {
            try
            {
                RenderToWord.HtmlToWord(Server.UrlDecode(html), "邮寄信封.doc", true);
                return Content("导出成功！");
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出邮寄信封出现异常：" + ex.Message);
                return Content("导出邮寄信封异常！");
            }
        }

        # endregion

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public ActionResult Search()
        {
            return View();
        }

        /// <summary>
        /// 稿件处理专区
        /// </summary>
        /// <returns></returns>
        public ActionResult ContributionArea()
        {
            return View();
        }

        /// <summary>
        /// 查看撤稿申请
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult ViewRetraction(long CID)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            RetractionsBillsQuery rQuery = new RetractionsBillsQuery();
            rQuery.CID = CID;
            rQuery.JournalID = JournalID;
            RetractionsBillsEntity rEntity = service.GetRetractionsBillsModel(rQuery);
            if (rEntity == null)
            {
                rEntity = new RetractionsBillsEntity();
            }
            return View(rEntity);
        }

        # region ajax

        # region 获取当前步骤的稿件列表

        /// <summary>
        /// 获取当前步骤的稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetContributionListAjax(CirculationEntity cirEntity)
        {
            ISiteConfigFacadeService facadeService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            DictEntity dicteEntity = facadeService.GetDictModelByKey(new DictQuery() { JournalID = CurAuthor.JournalID, DictKey = "TemplateID" });
            if (dicteEntity != null)
            {
                ISiteConfigFacadeService currentService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                IList<DictValueEntity> list = currentService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = dicteEntity.DictKey });
                if (list != null && list.Count > 0)
                {
                    DictValueEntity entity = list.Single<DictValueEntity>();
                    if (entity != null)
                    {
                        cirEntity.TemplateID = entity.ValueID;
                    }
                }
            }

            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            cirEntity.JournalID = JournalID;
            cirEntity.CurAuthorID = CurAuthor.AuthorID;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            cirEntity.CurrentPage = pageIndex;
            cirEntity.PageSize = TypeParse.ToInt(Request.Params["pagesize"], 10);
            if (Request.Params["sortname"] != null)
            {
                cirEntity.SortName = Request.Params["sortname"].ToString();
                cirEntity.SortOrder = Request.Params["sortorder"].ToString();
            }

            FlowStatusQuery query = new FlowStatusQuery();
            query.JournalID = JournalID;
            query.StatusID = cirEntity.StatusID;
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            FlowStep stepEntity = flowService.GetFlowStepInfo(query);
            if (stepEntity != null)
            {
                FlowStatusEntity statusEntity = stepEntity.FlowStatus;
                if (statusEntity != null && statusEntity.CStatus==888)
                {
                    cirEntity.IsExpertAudited = true;
                }
            }
            Pager<FlowContribution> pager = service.GetFlowContributionList(cirEntity);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 获取当前状态的稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetEditorContributionListAjax(CirculationEntity cirEntity)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            cirEntity.JournalID = JournalID;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            cirEntity.CurrentPage = pageIndex;
            cirEntity.PageSize = TypeParse.ToInt(Request.Params["pagesize"], 10);
            Pager<FlowContribution> pager = service.GetFlowContributionList(cirEntity);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 获取当前步骤的稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult SearchContributionListAjax(CirculationEntity cirEntity)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            cirEntity.JournalID = JournalID;
            cirEntity.CurAuthorID = CurAuthor.AuthorID;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            cirEntity.CurrentPage = pageIndex;
            cirEntity.IsSearch = true;
            string sortname = Request.Params["sortname"];
            string sortorder = Request.Params["sortorder"];
            cirEntity.OrderStr = sortname + " " + sortorder;
            cirEntity.PageSize = TypeParse.ToInt(Request.Params["pagesize"], 10);
            Pager<FlowContribution> pager = service.GetFlowContributionList(cirEntity);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 导出搜索的稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchImportExcel(CirculationEntity cirEntity)
        {
            try
            {
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                cirEntity.JournalID = JournalID;
                cirEntity.CurrentPage = 1;
                cirEntity.IsSearch = true;
                cirEntity.PageSize = 1000000;
                Pager<FlowContribution> pager = service.GetFlowContributionList(cirEntity);
                if (pager == null || pager.ItemList.Count <= 0)
                {
                    return Content("没有数据不能导出，请先进行查询！");
                }

                # region 列头

                IDictionary<string, string> dict = new Dictionary<string, string>();

                dict.Add("CNumber", "稿件编号");
                dict.Add("Title", "稿件标题");
                dict.Add("FirstAuthor", "第一作者");
                dict.Add("AuthorName", "通信作者");
                dict.Add("RecUserName", "审稿人");
                dict.Add("AuditStatus", "稿件状态");
                dict.Add("AddDate", "投稿时间");
                dict.Add("Tel", "联系方式");
                dict.Add("Address", "单位");
                dict.Add("LoginName", "邮箱");
                dict.Add("Remark", "备注");
                # endregion

                RenderToExcel.ExportListToExcel<FlowContribution>(pager.ItemList.OrderBy(o => o.CNumber).ToList<FlowContribution>(), dict
                    , null
                    , "稿件信息_导出" + DateTime.Now.ToString("yyyy-MM-dd"), false, "xls");
                return Content("导出成功！");
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出稿件信息出现异常：" + ex.Message);
                return Content("导出稿件信息异常！");
            }
        }

        /// <summary>
        /// 获取没有设置责任编辑的异常稿件
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetExceptionContributionListAjax(CirculationEntity cirEntity)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            cirEntity.JournalID = JournalID;
            cirEntity.CurAuthorID = 0;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            cirEntity.CurrentPage = pageIndex;
            cirEntity.PageSize = TypeParse.ToInt(Request.Params["pagesize"], 10);
            Pager<FlowContribution> pager = service.GetFlowContributionList(cirEntity);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 获取没有设置责任编辑的异常稿件
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetReturnContributionListAjax(CirculationEntity cirEntity)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            cirEntity.JournalID = JournalID;
            cirEntity.CStatus = -100;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            cirEntity.CurrentPage = pageIndex;
            cirEntity.PageSize = TypeParse.ToInt(Request.Params["pagesize"], 10);
            Pager<FlowContribution> pager = service.GetFlowContributionList(cirEntity);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        # endregion

        # region 获取当前人可以处理的稿件状态

        /// <summary>
        /// 获取当前人可以处理的稿件状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetHaveRightFlowStatus(byte? WorkStatus)
        {
            JsonExecResult<FlowStatusEntity> jsonResult = new JsonExecResult<FlowStatusEntity>();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            FlowStatusQuery query = new FlowStatusQuery();
            query.JournalID = JournalID;
            query.CurAuthorID = CurAuthor.AuthorID;
            query.RoleID = CurAuthor.RoleID == null ? 0 : CurAuthor.RoleID.Value;
            query.IsHandled = WorkStatus == null ? (byte)2 : WorkStatus.Value;
            try
            {
                jsonResult.ItemList = service.GetHaveRightFlowStatus(query);
                if (jsonResult.ItemList != null)
                {
                    jsonResult.ItemList = jsonResult.ItemList.Where(p => p.ContributionCount > 0).ToList<FlowStatusEntity>();
                }
                jsonResult.result = EnumJsonResult.success.ToString();
            }
            catch (Exception ex)
            {
                jsonResult.result = EnumJsonResult.error.ToString();
                jsonResult.msg = "获取当前人可以处理的稿件状态出现异常:" + ex.Message;
            }
            return Json(jsonResult);
        }

        /// <summary>
        /// 获取稿件状态统计
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetFlowStatusStat()
        {
            JsonExecResult<FlowStatusEntity> jsonResult = new JsonExecResult<FlowStatusEntity>();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            FlowStatusQuery query = new FlowStatusQuery();
            query.JournalID = JournalID;
            query.IsHandled = 2;
            query.CurAuthorID = 0;
            query.RoleID = 0;
            try
            {
                jsonResult.ItemList = service.GetHaveRightFlowStatus(query);
                jsonResult.result = EnumJsonResult.success.ToString();
            }
            catch (Exception ex)
            {
                jsonResult.result = EnumJsonResult.error.ToString();
                jsonResult.msg = "获取稿件状态统计出现异常:" + ex.Message;
            }
            return Json(new { Rows = jsonResult.ItemList, Total = 200 });
        }

        # endregion

        # region 获取当前状态下的可做操作

        /// <summary>
        /// 获取当前状态下的可做操作
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetFlowActionByStatus(FlowActionQuery query)
        {
            JsonExecResult<FlowActionEntity> jsonResult = new JsonExecResult<FlowActionEntity>();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            query.JournalID = JournalID;
            query.AuthorID = CurAuthor.AuthorID;
            query.RoleID = CurAuthor.RoleID == null ? 0 : CurAuthor.RoleID.Value;
            try
            {
                List<FlowActionEntity> listResult = service.GetFlowActionByStatus(query);
                jsonResult.ItemList = listResult;
                jsonResult.result = EnumJsonResult.success.ToString();
            }
            catch (Exception ex)
            {
                jsonResult.result = EnumJsonResult.error.ToString();
                jsonResult.msg = "获取当前状态下的可做操作出现异常:" + ex.Message;
            }
            return Json(jsonResult);
        }

        # endregion

        # region 设置标记

        /// <summary>
        /// 设置稿件旗帜标记
        /// </summary>
        /// <param name="cEntityList"></param>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult SetContributionFlagAjax(List<ContributionInfoQuery> cEntityList)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                if (cEntityList.Count > 0)
                {
                    foreach (ContributionInfoQuery item in cEntityList)
                    {
                        item.JournalID = JournalID;
                    }
                    IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                    execResult = service.SetContributeFlag(cEntityList);
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "请选择要设置的稿件";
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "设置稿件旗帜标志出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("设置稿件旗帜标志出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        /// <summary>
        /// 设置稿件加急标记
        /// </summary>
        /// <param name="cEntityList"></param>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult SetContributionQuickAjax(List<ContributionInfoQuery> cEntityList)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                if (cEntityList.Count > 0)
                {
                    foreach (ContributionInfoQuery item in cEntityList)
                    {
                        item.JournalID = JournalID;
                    }
                    IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                    execResult = service.SetContributeQuick(cEntityList);
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "请选择要设置的稿件";
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "设置稿件旗帜标志出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("设置稿件旗帜标志出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        /// <summary>
        /// 设置稿件删除标记
        /// </summary>
        /// <param name="cEntityList"></param>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult DeleteContributionAjax(List<ContributionInfoQuery> cEntityList)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                if (cEntityList.Count > 0)
                {
                    foreach (ContributionInfoQuery item in cEntityList)
                    {
                        item.JournalID = JournalID;
                        item.Status = (int)EnumContributionStatus.Delete;// 删除
                    }
                    IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                    execResult = service.DeleteContribute(cEntityList);
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "请选择要删除的稿件";
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "删除稿件出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("删除稿件出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion

        # region 导出稿件列表

        [HttpPost]
        public ActionResult ContributionRenderToExcel(CirculationEntity cirEntity)
        {
            try
            {
                IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                cirEntity.JournalID = JournalID;
                cirEntity.CurAuthorID = CurAuthor.AuthorID;
                int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
                cirEntity.CurrentPage = 1;
                cirEntity.PageSize = 100000;
                Pager<FlowContribution> list = service.GetFlowContributionList(cirEntity);
                if (list == null || list.ItemList.Count <= 0)
                {
                    return Content("没有数据不能导出，请先进行查询！");
                }

                # region 列头

                IDictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("CNumber", "稿件编号");
                dict.Add("Title", "稿件标题");
                dict.Add("FirstAuthor", "第一作者");
                dict.Add("AuthorName", "投稿人");
                dict.Add("RecUserName", "审稿人");
                dict.Add("AuditStatus", "稿件状态");
                dict.Add("AddDate", "投稿时间");
                dict.Add("Tel", "联系方式");
                dict.Add("Address", "单位");
                dict.Add("LoginName", "邮箱");
                dict.Add("Remark", "备注");
                # endregion

                RenderToExcel.ExportListToExcel<FlowContribution>(list.ItemList, dict
                    , null
                    , "稿件信息_导出" + DateTime.Now.ToString("yyyy-MM-dd"), false, "xls");
                return Content("导出成功！");
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出稿件信息出现异常：" + ex.Message);
                return Content("导出稿件信息异常！");
            }
        }

        # endregion

        # endregion

        # endregion

        # region 设置责任编辑

        [AjaxRequest]
        [HttpPost]
        public ActionResult SetContributionEditor(long[] CIDArray, long AuthorID)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                if (AuthorID == 0)
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "请选择责任编辑";
                }
                else if (CIDArray != null && CIDArray.Length == 0)
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "请选择要设置的稿件";
                }
                else
                {
                    SetContributionEditorEntity setEntity = new SetContributionEditorEntity();
                    setEntity.JournalID = JournalID;
                    setEntity.AuthorID = AuthorID;
                    setEntity.CIDArray = CIDArray;
                    IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                    execResult = service.SetContributeEditor(setEntity);
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "设置稿件责任编辑出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("设置稿件责任编辑出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion

        # region 设置介绍信标记

        [AjaxRequest]
        [HttpPost]
        public ActionResult SetIntroLetter(long CID, int flag)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                ContributionInfoQuery cQueryEntity = new ContributionInfoQuery();
                cQueryEntity.JournalID = JournalID;
                cQueryEntity.CID = CID;
                cQueryEntity.IntroLetterPath = flag == 1 ? "已交" : "";
                IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                execResult = service.UpdateIntroLetter(cQueryEntity);
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "设置稿件的已交标记出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("设置稿件的已交标记出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion

        # region 处理撤稿申请

        [AjaxRequest]
        [HttpPost]
        public ActionResult DealWithdrawal(long CID)
        {
            ExecResult execResult = new ExecResult();
            try
            {
                ContributionInfoQuery cQueryEntity = new ContributionInfoQuery();
                cQueryEntity.JournalID = JournalID;
                cQueryEntity.CID = CID;
                IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                execResult = service.DealWithdrawal(cQueryEntity);
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "处理撤稿申请出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("处理撤稿申请出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion

        # region 撤销删除状态

        [AjaxRequest]
        [HttpPost]
        public ActionResult CancelDelete(long CID)
        {
            ExecResult execResult = new ExecResult();
            try
            {
                ContributionInfoQuery cQueryEntity = new ContributionInfoQuery();
                cQueryEntity.JournalID = JournalID;
                cQueryEntity.CID = CID;
                IContributionFacadeService service = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                execResult = service.CancelDelete(cQueryEntity);
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "撤销删除状态出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("撤销删除状态出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion

    }
}
