using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WKT.Config;
using System.IO;

public partial class SaveFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var extionName = string.IsNullOrEmpty(Request.QueryString["extion"]) ? ".doc" : Request.QueryString["extion"];
        PageOffice.FileSaver fs = new PageOffice.FileSaver();
        string path = ("/UploadFile/ContributeInfo/ContributePath/PageOffice/").Replace("//", "/");
        string uploadPath = GetFullPath(path);
        string filename = Path.ChangeExtension(Path.GetRandomFileName(), extionName);
        string url = Path.Combine(uploadPath, filename);
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);
        fs.SaveToFile(url);
        //保存成功后，设置返回值，此处设置为：OK
        fs.CustomSaveResult = Path.Combine(path, filename);
        fs.Close();


    }

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


}
