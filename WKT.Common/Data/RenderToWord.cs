using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Globalization;

namespace WKT.Common.Data
{
    public class RenderToWord
    {
        /// <summary>
        /// html标签导出到word
        /// </summary>
        /// <param name="html"></param>
        /// <param name="fileName">带后缀</param>
        /// <param name="IsWord2003"></param>
        /// <param name="ext"></param>
        public static void HtmlToWord(string html, string fileName, bool IsWord2003 = true)
        {
            try
            {
                HttpResponse response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.Buffer = false;
                response.Charset = "GB2312";
                response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
                response.ContentEncoding = Encoding.GetEncoding("GB2312");

                response.ContentType = IsWord2003 ? "application/ms-word" : "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                //response.ContentType = "application/octet-stream";

                CultureInfo formatProvider = new CultureInfo("zh-CN", true);
                StringWriter writer = new StringWriter(formatProvider);

                response.Write(html);

                response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
