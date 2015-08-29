using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WKT.Common.SMS
{
    public class SmsHelper
    {
        private static string FToken = "";
        /// <summary>
        /// SHP大众插件，基于HTTP协议的接入方式，在网页程序或应用程序中直接调用URL语句来完成短信功能。
        /// </summary>       
        /// <param name="DstMobile">手机号码列表，多个手机号用英文逗号分隔</param>
        /// <param name="SmsMsg">短信内容</param>
        /// <param name="UserName">商户发送短信用户名</param>
        /// <param name="Password">商户发送短信密码</param>
        /// <returns>发送成功返回true，失败返回false</returns>
        public static bool SendSmsSHP(string DstMobile, string SmsMsg, string userName, string Password)
        {

            string mToUrl = "";	    //即将引用的url   			
            string mRtv = "";		//引用的返回字符串

            //编码
            SmsMsg = System.Web.HttpUtility.UrlEncode(SmsMsg, System.Text.Encoding.GetEncoding("gb2312"));

            // 用户调用
            mToUrl = "http://www.139000.com/send/gsend.asp?name=" + userName + "&pwd=" + Password + "&dst=" + DstMobile + "&msg=" + SmsMsg + "";

            //System.Web.HttpContext.Current.Response.Write(mToUrl.ToString());

            try
            {
                System.Net.HttpWebResponse rs = (System.Net.HttpWebResponse)System.Net.HttpWebRequest.Create(mToUrl).GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(rs.GetResponseStream(), System.Text.Encoding.Default);
                mRtv = sr.ReadToEnd();
            }
            catch(Exception ex)
            {
                throw ex;	//对 url http 请求的时候发生的错误  比如页面不存在 或者页面本身执行发生错误
            }

            if (mRtv.Substring(0, 5) != "num=0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取短信余额
        /// </summary>
        /// <param name="SiteID"></param>
        /// <returns></returns>
        public static string GetSmsCount(string SiteID)
        {
            string UserName = "";
            string Password = "";

            string mToUrl = "";	    //即将引用的url   			
            string mRtv = "";		//引用的返回字符串

            string strSMSConfigPath = System.Web.HttpContext.Current.Server.MapPath("/Admin/Config/" + SiteID + "/SiteConfig.config");
            if (System.IO.File.Exists(strSMSConfigPath))
            {
                System.Xml.XmlDocument XDTop = new System.Xml.XmlDocument();
                XDTop.Load(strSMSConfigPath);

                UserName = XDTop.SelectSingleNode("DB/SMSConfig/SMSUserName").InnerText;
                Password = XDTop.SelectSingleNode("DB/SMSConfig/SMSPassword").InnerText;
            }


            // 用户调用
            mToUrl = "http://www.139000.com/send/getfee.asp?name=" + UserName + "&pwd=" + Password;

            System.Net.HttpWebResponse rs = (System.Net.HttpWebResponse)System.Net.HttpWebRequest.Create(mToUrl).GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(rs.GetResponseStream(), System.Text.Encoding.Default);
            mRtv = sr.ReadToEnd();

            string[] arr = mRtv.Split(new char[] { '=', '&' });

            return arr[1];
        }



    }
}
