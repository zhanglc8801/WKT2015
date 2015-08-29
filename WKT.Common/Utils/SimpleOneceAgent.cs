using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WKT.Common.Utils
{
    public class SimpleOneceAgent
    {
         QuickWebRequest gRequest;
         QuickWebResponse gResponse;
         string Encoding = "";
        /// <summary>
        /// 读取一个指定URL的内容并转换为UTF-8字符串返回
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ReadUrlContent(string url) {
            DateTime tbegin = DateTime.Now;
            //WriteHttpLog("begin request", url);
            string result =  ReadUrlContent(url, true,true);
            DateTime tend = DateTime.Now;
            TimeSpan tspan = tend - tbegin;

            //WriteHttpLog("end request(" + tspan.TotalMilliseconds + ")", url);
            return result;
        }

        public static void WriteHttpLog(string flag,string url) {
#if DEBUG
            
            if ( !Directory.Exists(@"D:\web\evalapi.horise.com\" ) ){
                return;
            }
            FileStream fs = System.IO.File.Open(@"D:\web\evalapi.horise.com\httplog.txt", FileMode.Append);
            StreamWriter writer = new StreamWriter(fs);
            writer.WriteLine(string.Format("[{0}]{1}{2}", flag, url, DateTime.Now));
            writer.Close();
            fs.Close();
             
#endif
        }

        /// <summary>
        /// 读取一个指定URL的内容并转换为UTF-8字符串返回
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ReadUrlContent(string url,string encoding)
        {
            this.Encoding = encoding;
            return ReadUrlContent(url, true, true);
        }

        /// <summary>
        /// 读取一个指定URL的内容并转换为UTF-8字符串返回
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ReadUrlContent(string url,int timeoutsecond)
        {
            return ReadUrlContent(ref url, true, true,timeoutsecond);
        }

        /// <summary>
        /// 读取一个指定URL的内容并转换为UTF-8字符串返回
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ReadUrlContent(string url, bool checkredirect, bool checkHtmlRedurect)
        {
            return ReadUrlContent(ref url, checkredirect, checkHtmlRedurect,15);
        }

        private string ReadUrlContent(ref string url, bool checkredirect, bool checkHtmlRedurect,int timeoutsecond)
        {
            string content = String.Empty;

            try
            {
                Uri uri = new Uri(url);
                gRequest = QuickWebRequest.Create(uri, null, gRequest, false);
                gResponse = gRequest.GetResponse(timeoutsecond);

                if (checkredirect)
                {
                    if (gResponse.RedirectUri != null && gResponse.RedirectUri.AbsoluteUri != url)
                    {
                        return ReadUrlContent(gResponse.RedirectUri.AbsoluteUri, false, checkHtmlRedurect);
                    }
                }

                byte[] bytes = gResponse.ReadResponse();
                if (bytes == null)
                {
                    return null;
                }

                System.Text.Encoding charset = HttpUtils.DetectCharset(gResponse, bytes);
                if (charset == null)
                { //默认使用GB2312
                    charset = System.Text.Encoding.GetEncoding("GB2312");
                }

                // 使用指定的编码
                if (!string.IsNullOrEmpty(Encoding))
                {
                    charset = System.Text.Encoding.GetEncoding(Encoding);
                }

                //完成到UTF8的编码转换
                if (charset != System.Text.Encoding.UTF8)
                {
                    bytes = System.Text.Encoding.Convert(charset, System.Text.Encoding.UTF8, bytes);
                    charset = System.Text.Encoding.UTF8;
                }

                content = charset.GetString(bytes);
                //检查一次Html重定向
                if (checkHtmlRedurect)
                {
                    System.Text.RegularExpressions.Regex redirectRegex = new System.Text.RegularExpressions.Regex("<META\\s+HTTP-EQUIV\\s*=\\s*[\"]*Refresh[\"]*\\s+CONTENT=[\"\\s]*\\d+\\s*[;]\\s*URL=(?<match>.*?)[\\s\"]*>", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    System.Text.RegularExpressions.Match match = redirectRegex.Match(content);
                    if (match.Success)
                    {
                        if (!string.IsNullOrEmpty(match.Groups["match"].Value) && match.Groups["match"].Value != url)
                        {
                            url = match.Groups["match"].Value;
                            return ReadUrlContent(url, checkredirect, false);
                        }
                    }
                }
            }
            catch
            {
                //暂时不做处理。
            }
            finally
            {
                gResponse.Close();
            }

            return content;
        }
    }
}
