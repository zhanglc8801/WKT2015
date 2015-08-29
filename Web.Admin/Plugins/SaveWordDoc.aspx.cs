using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Plugins
{
    public partial class SaveWordDoc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ZSOfficeX.SaveDocObj SaveObj = new ZSOfficeX.SaveDocObj();
            //SaveObj.FileFilter = ".doc,.xls"; //设置合法的上传文档类型,默认值：.doc,.xls,.ppt,.vsd,.docx
            //SaveObj.FileName '获取文件名
            //SaveObj.FileExtName '获取文件扩展名
            //SaveObj.FileSize '获取文件大小，以字节为单位
            //SaveObj.FileContent '获取文件二进制流，可以保存到数据库字段
            SaveObj.SaveToFile(Server.UrlDecode(Request.QueryString["fn"]));   //这里设置参数为空字符串，则保存文档到原位置；如果设置为物理路径（如："d:\abc.doc"），表明另存为参数指定的新文件
            SaveObj.ReturnOK();
        }
    }
}