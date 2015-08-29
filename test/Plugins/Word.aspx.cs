using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

namespace HanFang360.InterfaceService.Plugins
{
    public partial class Word : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string filename = Server.UrlDecode(Request.QueryString["fn"]);

            // ***  以下是输出文档的代码  **********************

            string fileLength = "";
            FileStream fs = File.OpenRead(filename);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fileLength = fs.Length.ToString();
            fs.Close();

            string strFileN = System.IO.Path.GetFileName(filename);
            Response.Clear();
            Response.ContentType = "application/msword";  //换成相应类型即可 application/x-excel, application/ms-powerpoint, application/pdf   
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileN);
            Response.AddHeader("Content-Length", fileLength);

            Stream fos = Response.OutputStream;
            fos.Write(data, 0, data.Length);
            fos.Close();
            Response.End();
        }
    }
}