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

namespace HanFang360.InterfaceService.Plugins
{
    public partial class Download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long CID = TypeParse.ToLong(Request.QueryString["CID"],0);
            if (CID <= 0)
            {
                Response.Write("请选择正确的稿件");
            }
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            // 附件路径
            FlowLogQuery logQuery = new FlowLogQuery();
            logQuery.JournalID = SiteConfig.SiteID;
            logQuery.FlowLogID = 0;
            logQuery.CID = CID;
            string _filePath = flowService.GetFlowLogAttachment(logQuery);
            string fileName = "";
            if (!string.IsNullOrEmpty(_filePath))
            {
                fileName = _filePath.Substring(_filePath.LastIndexOf("/") + 1);
            }

            string downPath = GetUploadPath(_filePath);
            if (!System.IO.File.Exists(downPath))
                Response.Write("文件不存在");
            if (!fileName.Contains("."))
                fileName += Path.GetExtension(downPath);

            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            ContributionInfoEntity cEntity = service.GetContributionInfoModel(new ContributionInfoQuery { JournalID = SiteConfig.SiteID, CID = CID });
            string docTitle = "";
            if (cEntity != null)
            {
                docTitle = cEntity.Title + Path.GetExtension(downPath);
            }
            else
            {
                docTitle = fileName;
            }

            FileStream fs = new FileStream(downPath, FileMode.Open);
            //fs.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            Response.AddHeader("Content-Disposition", "attachment;  filename=" + docTitle);
            Response.AddHeader("Content-Length", bytes.Length.ToString());
            Response.ContentEncoding = Encoding.UTF8;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

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
    }
}