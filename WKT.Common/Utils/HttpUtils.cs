using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Web;

namespace WKT.Common.Utils
{
    public class HttpUtils
    {
        /// <summary>
        /// 规范化并合并URL
        /// </summary>
        /// <param name="baseUri">基本url</param>
        /// <param name="urlpath">访问路径，或完全的url</param>
        /// <returns></returns>
        public static Uri CombineUrl(Uri baseUri, string urlpath) {
            if (string.IsNullOrEmpty(urlpath)) {
                return null;
            }

            string lowverUrl = urlpath.ToLower();
            try
            {
                if (!lowverUrl.StartsWith("http://") && !lowverUrl.StartsWith("https://"))
                {
                    return new Uri(baseUri, urlpath);
                }
                else
                {
                    return new Uri(urlpath);
                }
            }
            catch
            {
                return null; //无效的url
            }
        }

        /// <summary>
        /// 自动检测内容的charset,通过字符集和html中指定的contentType来获取
        /// 如果无法获得字符集，则返回null
        /// </summary>
        /// <param name="stringcontent"></param>
        /// <returns></returns>
        public static Encoding  DetectCharset(QuickWebResponse response, byte[] bytes) {

            string regexstr = "(text/html|text/xml).*charset=(?<charset>\\w+\\-*\\d*)";
            Regex regex = new Regex(regexstr, RegexOptions.IgnoreCase);
             
            string contentType = response.ContentTypeStr;
            if (contentType != null)
            {
                Match match1 = regex.Match(contentType);
                if (match1.Success)
                {
                    string charset = match1.Groups["charset"].Value.ToUpper();
                    Encoding encoder = System.Text.Encoding.GetEncoding(charset);
                    if (encoder != null)
                    {
                        return encoder;
                    }
                }
            }

            string ascii = System.Text.Encoding.ASCII.GetString(bytes);
            //<META http-equiv="Content-Type" content="text/html; charset=GB2312">
            //Content-Type=text/html;
            
            Match match = regex.Match(ascii);
            if ( match.Success )
            {
                string charset =  match.Groups["charset"].Value.ToUpper();
                return System.Text.Encoding.GetEncoding(charset);
            }
            else {
                return null;
            }
        }

        public static string HtmlEncode(String str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            
            string _txtString = str;
            _txtString = _txtString.Replace("<", "&lt;")
                .Replace(">", "&rt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#039;")
                .Replace(" ", "&nbsp;")
                .Replace("\r\n", "<br/>")
                .Replace("\r", "<br/>")
                .Replace("\n", "<br/>");

            return _txtString;
        }

        public static string LeftSub(string str, int len)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return str.Length > len ? str.Substring(0, len) + "..." : str;
        }

        public static string LeftSubHtmlEncode(string str, int len)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            string subStr = LeftSub(str, len);
            return HttpUtility.HtmlEncode(subStr);
        }

    }
}
