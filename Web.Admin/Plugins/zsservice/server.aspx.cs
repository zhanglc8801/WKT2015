// 重要提示：本页代码是卓正Office组件的系统保留代码，请勿修改。
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class zsservice_server : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // 重要提示：本页代码是卓正Office组件的系统保留代码，请勿修改。
        ZSServerNet.Server ServerObj = new ZSServerNet.Server();
        ServerObj.Connect();
    }
}
