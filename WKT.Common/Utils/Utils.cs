using System;
using System.Web;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace WKT.Common.Utils
{
    public class Utils
    {

        #region 判断是不是IP地址
        private static bool IsIP(string ip)
        {
            string[] sections = ip.Split('.');
            if (sections.Length != 4)
                return false;
            foreach (string s in sections)
            {
                int tmp = int.Parse(s.Trim());
                if (tmp > 255)
                    return false;
            }
            return true;
        }  
        #endregion
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetRealIP()
        {
            #region 获取外网IP 存在BUG 已注释
            //获取外网IP 存在BUG，部署到ISS后获取的是服务器IP
            //using (var webClient = new WebClient())
            //{
            //    try
            //    {
            //        var temp = webClient.DownloadString("http://iframe.ip138.com/ic.asp");
            //        var ip = Regex.Match(temp, @"\[(?<ip>\d+\.\d+\.\d+\.\d+)]").Groups["ip"].Value;
            //        return !string.IsNullOrEmpty(ip) ? ip : null;
            //    }
            //    catch (Exception ex)
            //    {
            //        return ex.Message;
            //    }
            //} 
            #endregion

            #region MyRegion
            //var svrVar = HttpContext.Current.Request.ServerVariables;     
            //string result = svrVar["HTTP_X_FORWARDED_FOR"];      
            //if (!String.IsNullOrEmpty(result))     
            //{         
            //    //可能有代理         
            //    if (result.IndexOf(".") == -1)    //沒有"."，非 IPv4 格式             
            //        result = null;         
            //    else        
            //    {             
            //        if (result.IndexOf(",") != -1)             
            //        {   //有","，估計多個代理。取第一個不是內網的IP。                 
            //            result = result.Replace(" ", "");                 
            //            string[] temparyip = result.Split(',', ';');                 
            //            foreach (string ip in temparyip)                 
            //            {                     
            //                string tmp = ip.Substring(0, 7);                     
            //                if (IsIP(ip) && ip.Substring(0, 3) != "10." && tmp != "192.168" && tmp != "172.16.")                     
            //                {                         
            //                    result = ip;                         
            //                    break;                     
            //                }                 
            //            }             
            //        }             
            //        else if (!IsIP(result))                 
            //            result = null;                             
            //    }     
            //}      
            //if (String.IsNullOrEmpty(result))     
            //{         
            //    result = svrVar["REMOTE_ADDR"];         
            //    if(String.IsNullOrEmpty(result))             
            //        result = HttpContext.Current.Request.UserHostAddress;
            //}                       
            //return result;  
            #endregion

            string ip = HttpContext.Current.Request.UserHostAddress;
            string agentip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(agentip))
            {
                //有代理 
                if (agentip.IndexOf(".") == -1)
                    agentip = null;
                if (agentip != null)
                {
                    if (agentip.IndexOf("unknow") != -1)
                        agentip = agentip.Replace("unknow", string.Empty);
                    string[] temparyip = agentip.Replace("   ", string.Empty).Replace("'", string.Empty).Split(new char[] { ',', ';' });
                    //过滤代理格式中的非IP和内网IP 
                    for (int i = 0; i < temparyip.Length; i++)
                    {
                        if (temparyip[i] != string.Empty && IsIP(temparyip[i])
                                                && temparyip[i].Substring(0, 3) != "10."
                                                && temparyip[i].Substring(0, 7) != "192.168"
                                                && temparyip[i].Substring(0, 7) != "172.16.")
                        {
                            ip += "," + temparyip;
                        }
                    }
                }
            }
            else
            {
                agentip = null;
            }
            return ip;

        }

        /// <summary>
        /// 获取当前访问者机器名称
        /// </summary>
        /// <returns></returns>
        public static string GetPCName()
        {
            return HttpContext.Current.Request.UserHostName;
        }

        /// <summary>
        /// 获取访问者内网IP
        /// </summary>
        /// <returns></returns>
        private string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }


        /// <summary>
        /// 获取当前访问者域名
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
        }

        /// <summary>
        /// 取得当月第一天的日期
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthFirstDay()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            DateTime firstDayOfThisMonth = new DateTime(year, month, 1);
            return firstDayOfThisMonth;
        }

        /// <summary>
        /// 取得当月第最后一的日期
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthLastDay()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            DateTime lastDayOfThisMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            return lastDayOfThisMonth;
        }

        /// <summary>
        /// 取得当月第最后一的日期
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthLastDay(DateTime dt)
        {
            int year = dt.Year;
            int month = dt.Month;
            DateTime lastDayOfThisMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            return lastDayOfThisMonth;
        }

        /// <summary>
        /// 获取指定日期和当前日期相差天数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int SpanDays(DateTime? dt)
        {
            if (dt == null)
            {
                return 0;
            }
            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts2 = new TimeSpan(dt.Value.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts.Days;
        }


        /// <summary>
        /// 读Cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }

            return "";
        }

        /// <summary>
        /// 删除Cookie值
        /// </summary>
        /// <param name="strName">Cookie名称</param>
        /// <returns>cookie值</returns>
        public static void RemoveCookie(string strName)
        {
            HttpContext.Current.Request.Cookies.Remove(strName);
        }

        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            return HttpContext.Current.Server.MapPath(strPath);
        }

        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        /// <summary>
        /// 获取文件编码
        /// </summary>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static Encoding GetEncodingByEncode(string encode)
        {
            Encoding encoding;
            switch (encode.ToLower())
            {
                case "gb2312":
                    encoding = Encoding.Default;
                    break;
                case "utf-8":
                    encoding = new UTF8Encoding(false);
                    break;
                default:
                    encoding = Encoding.Default;
                    break;
            }
            return encoding;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string SiteHome()
        {
            string homeurl = "";
            string domain = ConfigurationManager.AppSettings["SiteDomain"];
            if (string.IsNullOrEmpty(domain))
            {
                homeurl = "/";
            }
            else
            {
                if (domain.StartsWith("www."))
                {
                    homeurl = "http://" + domain;
                }
                else
                {
                    homeurl = "http://www." + domain;
                }
            }
            return homeurl;
        }

        /// <summary>
        /// 补零
        /// </summary>
        /// <param name="num"></param>
        /// <param name="numlen"></param>
        /// <returns></returns>
        public static string MendZero(long num, int numlen)
        {
            StringBuilder retValue = new StringBuilder("");
            if (num.ToString().Length < numlen)
            {
                int length = numlen - num.ToString().Length;

                for (int i = 0; i < length; i++)
                {
                    retValue.Append("0");
                }
            }
            retValue.Append(num);
            return retValue.ToString();
        }

        /// <summary>
        /// 获得页码显示链接
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="countPage">总页数</param>
        /// <param name="url">超级链接地址</param>
        /// <param name="extendPage">周边页码显示个数上限</param>
        /// <returns>页码html</returns>
        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage)
        {
            int startPage = 1;
            int endPage = 1;

            string t1 = "<a href=\"" + RegularUrl("page", "1", url) + "\">&laquo;</a>&nbsp;";
            string t2 = "<a href=\"" + RegularUrl("page", countPage.ToString(), url) + "\">&raquo;</a>&nbsp;";

            if (countPage < 1) countPage = 1;
            if (extendPage < 3) extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                {
                    s.Append("&nbsp;");
                    s.Append(i);
                    s.Append("&nbsp;");
                }
                else
                {
                    s.Append("&nbsp;<a href=\"");
                    s.Append(RegularUrl("page", i.ToString(), url));
                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>&nbsp;");
                }
            }
            s.Append(t2);

            return s.ToString();
        }

        /// 将Url中key的值替换为value，如果不存在key则追加
        public static string RegularUrl(string key, string value, string url)
        {
            int fragPos = url.LastIndexOf("#");
            string fragment = "";
            if (fragPos > -1)
            {
                fragment = url.Substring(fragPos);
                url = url.Substring(0, fragPos);
            }
            int querystart = url.IndexOf("?");
            if (querystart < 0)
            {
                url += "?" + key + "=" + value;
            }
            else if (querystart == url.Length - 1)
            {
                url += key + "=" + value;
            }
            else
            {
                Regex Re = new Regex(key + "=[^\\s&#]*", RegexOptions.IgnoreCase);
                url = (Re.IsMatch(url)) ? Re.Replace(url, key + "=" + value) : url + "&" + key + "=" + value;
            }
            return url + fragment;
        }
        /// <summary>
        /// 去掉HTML标签
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string ClearHtml(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml))
            {
                return "";
            }
            else
            {
                Regex r = null;
                Match m = null;

                r = new Regex(@"<\/?[^>]*>", RegexOptions.IgnoreCase);
                for (m = r.Match(strHtml); m.Success; m = m.NextMatch())
                {
                    strHtml = strHtml.Replace(m.Groups[0].ToString(), "");
                }
            }
            return strHtml;
        }
        /// <summary>
        /// 去掉HTML标签 并去掉&nbsp空格
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string ClearHtmlNbsp(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml))
                return "";
            else
                strHtml = ClearHtml(strHtml).Replace("&nbsp;","");   
            return strHtml;
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string ReadFileContent(string filePath)
        {
            string str=File.ReadAllText(filePath,Encoding.UTF8);
            #region 如果要读取的模板文件太大，请使用此方法。
            //FileStream fs = new FileStream(filePath, FileMode.Open);
            //StreamReader m_streamReader = new StreamReader(fs);
            //m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);// 从数据流中读取每一行，直到文件的最后一行
            //string str = "";
            //while (m_streamReader.ReadLine() != null)
            //{
            //    str += m_streamReader.ReadLine();
            //}
            //m_streamReader.Close(); 
            #endregion
            return str;
        }
        /// <summary>
        /// 将字符串写入到文本文件中
        /// </summary>
        /// <param name="str">将要写入的字符串</param>
        /// <param name="filePath">保存的路径</param>
        /// <returns></returns>
        public static bool WritStrToFile(string str,string filePath)
        {
            StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8); 
            //StreamWriter sw = File.CreateText(filePath);
            sw.Write(str);
            sw.Close();
            return true;
        }


    }
}
