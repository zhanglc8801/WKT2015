using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Common.Utils;
using WKT.Config;
using WKT.Log;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace Web.Admin
{
    public partial class WordOnline : System.Web.UI.Page
    {
        private string _filePath = "";
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetFilePath();
            string FileFullPath = string.Empty;
            if (string.IsNullOrEmpty(_filePath))
            {
                _filePath = Request.QueryString["filePath"];
                if (string.IsNullOrEmpty(_filePath))
                {
                    lblMsg.Text = "参数异常，文件不存在";
                    return;
                }
            }
            FileFullPath = GetUploadPath(_filePath);
            if (!System.IO.File.Exists(FileFullPath))
            {
                lblMsg.Text = "文件不存在";
                return;
            }
            FileInfo info = new FileInfo(FileFullPath);
            string fileName = info.Name;
            if (!CheckWord(FilePath))
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                if (Request.Browser.Browser == "IE")
                {
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                }
                else
                {
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
                }
                Response.AppendHeader("Content-Length", info.Length.ToString());
                Response.WriteFile(fileName);
                Response.Flush();
                Response.End();
            }
            else
            {
                // 设置卓正OFFICE组件服务页面
                ZSOfficeCtrl.ServerURL = SiteConfig.RootPath + "/Plugins/zsservice/server.aspx";
                ZSOfficeCtrl.MainStyle = ZSOfficeX.enumMainStyle.Silver;
                ZSOfficeCtrl.BorderStyle = ZSOfficeX.enumBorderStyle.Border3DThin;
                ZSOfficeCtrl.FileTitle = Request.QueryString["FileName"];
                ;
                ZSOfficeCtrl.SaveDocURL = SiteConfig.RootPath + "/Plugins/SaveWordDoc.aspx?fn=" + Server.UrlEncode(FileFullPath);
                ZSOfficeCtrl.WebOpen("word.aspx?fn=" + Server.UrlEncode(FileFullPath), ZSOfficeX.enumWorkMode.docNoRevision, "WKT", "Word.Document");
            }
        }

        private void GetFilePath()
        {
            // 根据稿件ID或者流程日志ID得到稿件路径
            long CIDParam = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["CID"]))
            {
                CIDParam = TypeParse.ToLong(Request.QueryString["CID"], 0);
            }
            // 日志ID
            long FlowLogID = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["FlowID"]))
            {
                FlowLogID = TypeParse.ToLong(Request.QueryString["FlowID"], 0);
            }

            if (CIDParam > 0)
            {
                IContributionFacadeService cService = ServiceContainer.Instance.Container.Resolve<IContributionFacadeService>();
                // 得到稿件路径
                ContributionInfoQuery cQuery = new ContributionInfoQuery();
                cQuery.JournalID = SiteConfig.SiteID;
                cQuery.CID = CIDParam;
                _filePath = cService.GetContributionAttachment(cQuery);
            }
            else if (FlowLogID > 0)
            {
                IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                // 附件路径
                FlowLogQuery logQuery = new FlowLogQuery();
                logQuery.JournalID = SiteConfig.SiteID;
                logQuery.FlowLogID = FlowLogID;
                _filePath = flowService.GetFlowLogAttachment(logQuery);
            }
        }

        # region method

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

        /// <summary>
        /// 判断是否为Word文档
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CheckWord(string path)
        {
            string ext = Path.GetExtension(path);
            if (ext.Equals(".doc") || ext.Equals(".docx") || ext.Equals(".xls") || ext.Equals(".xlsx"))
            {
                return true;
            }
            return false;
        }

        # endregion
    }
}