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
        /// SHP���ڲ��������HTTPЭ��Ľ��뷽ʽ������ҳ�����Ӧ�ó�����ֱ�ӵ���URL�������ɶ��Ź��ܡ�
        /// </summary>       
        /// <param name="DstMobile">�ֻ������б�����ֻ�����Ӣ�Ķ��ŷָ�</param>
        /// <param name="SmsMsg">��������</param>
        /// <param name="UserName">�̻����Ͷ����û���</param>
        /// <param name="Password">�̻����Ͷ�������</param>
        /// <returns>���ͳɹ�����true��ʧ�ܷ���false</returns>
        public static bool SendSmsSHP(string DstMobile, string SmsMsg, string userName, string Password)
        {

            string mToUrl = "";	    //�������õ�url   			
            string mRtv = "";		//���õķ����ַ���

            //����
            SmsMsg = System.Web.HttpUtility.UrlEncode(SmsMsg, System.Text.Encoding.GetEncoding("gb2312"));

            // �û�����
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
                throw ex;	//�� url http �����ʱ�����Ĵ���  ����ҳ�治���� ����ҳ�汾��ִ�з�������
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
        /// ��ȡ�������
        /// </summary>
        /// <param name="SiteID"></param>
        /// <returns></returns>
        public static string GetSmsCount(string SiteID)
        {
            string UserName = "";
            string Password = "";

            string mToUrl = "";	    //�������õ�url   			
            string mRtv = "";		//���õķ����ַ���

            string strSMSConfigPath = System.Web.HttpContext.Current.Server.MapPath("/Admin/Config/" + SiteID + "/SiteConfig.config");
            if (System.IO.File.Exists(strSMSConfigPath))
            {
                System.Xml.XmlDocument XDTop = new System.Xml.XmlDocument();
                XDTop.Load(strSMSConfigPath);

                UserName = XDTop.SelectSingleNode("DB/SMSConfig/SMSUserName").InnerText;
                Password = XDTop.SelectSingleNode("DB/SMSConfig/SMSPassword").InnerText;
            }


            // �û�����
            mToUrl = "http://www.139000.com/send/getfee.asp?name=" + UserName + "&pwd=" + Password;

            System.Net.HttpWebResponse rs = (System.Net.HttpWebResponse)System.Net.HttpWebRequest.Create(mToUrl).GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(rs.GetResponseStream(), System.Text.Encoding.Default);
            mRtv = sr.ReadToEnd();

            string[] arr = mRtv.Split(new char[] { '=', '&' });

            return arr[1];
        }



    }
}
