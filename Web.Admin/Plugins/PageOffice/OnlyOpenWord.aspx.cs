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

public partial class OnlyOpenWord : System.Web.UI.Page
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

    private string _fileName = "";
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName
    {
        get
        {
            return _fileName;
        }
    }

    
    protected void Page_Load(object sender, EventArgs e)
    {
        //设置服务器页面
        PageOfficeCtrl1.ServerPage = Request.ApplicationPath + "/Plugins/pageoffice/server.aspx";
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
        FileFullPath = GetFullPath(_filePath);
        if (!System.IO.File.Exists(FileFullPath))
        {
            lblMsg.Text = "文件不存在";
            return;
        }
        FileInfo info = new FileInfo(FileFullPath);
        string fileName = info.Name;
        if (!CheckWord(FilePath))
        {
            lblMsg.Text = "文件格式不正确,请检查文件格式！";
            return;
        }
        //PageOffice组件的引用
        //首先确保已安装了pageoffice服务器端组件，且在项目中已添加了pageoffice文件夹，
        //在该文件夹中已存在posetup.exe和server.aspx服务器端页面，再在前台页面中引入PageOfficeCtrl控件

        
        PageOfficeCtrl1.Caption = FileName;
        //在只读模式下工具条和菜单栏都已不起作用，不需要显示
        PageOfficeCtrl1.OfficeToolbars = false;
        PageOfficeCtrl1.CustomToolbar = false;

        //打开文件
        //PageOfficeCtrl1.WebOpen("doc/template.doc", PageOffice.OpenModeType.docReadOnly, "张佚名");
        PageOfficeCtrl1.WebOpen(FileFullPath, PageOffice.OpenModeType.docReadOnly, "WKT.Admin");
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

        if (!string.IsNullOrEmpty(Request.QueryString["fileName"]))
        {
            _fileName = Request.QueryString["fileName"];
        }
        else
        {
            _fileName = "在线审阅稿件";
        }

    }

    # region method

    /// <summary>
    /// 获取完整路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private string GetFullPath(string path)
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