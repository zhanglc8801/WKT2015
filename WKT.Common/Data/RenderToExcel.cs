using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Globalization;
using System.Reflection;

namespace WKT.Common.Data
{
    public class RenderToExcel
    {
        /// <summary>
        ///  根据List 生成excel
        /// </summary>
        /// <param name="sourceTable">数据源</param>
        /// <param name="fileName">文件名称(生成随机数_fileName)</param>
        public static void ExportListToExcel<T>(IList<T> list, IDictionary<string, string> dic, Func<T, T> GetList, string fileName, bool IsExcel2003 = true, string ext = "xls") where T : class,new()
        {
            try
            {
                StringBuilder strBuilder = new StringBuilder();

                #region 组装数据
                if (list == null || list.Count == 0)
                {
                    strBuilder.Append("无数据！");
                }
                else
                {
                    foreach (var item in dic)
                    {
                        strBuilder.Append(item.Value);
                        strBuilder.Append("\t");
                    }
                    strBuilder.Append("\n");

                    T t = null;
                    PropertyInfo property = null;
                    object obj = null;
                    foreach (T model in list)
                    {
                        t = model;
                        if (GetList != null)
                            t = GetList(t);
                        foreach (var item in dic)
                        {
                            if (item.Key.Contains("."))//实体里面包含实体
                            {
                                var arry = item.Key.Split('.');
                                property = typeof(T).GetProperty(arry[0]);
                                if (property != null)
                                {
                                    obj = property.GetValue(t, null);
                                    if (obj != null)
                                        obj = obj.GetType().GetProperty(arry[1]).GetValue(obj, null);
                                }
                            }
                            else
                            {
                                property = typeof(T).GetProperty(item.Key);
                                obj = property.GetValue(t, null);
                            }
                            strBuilder.Append(obj == null ? string.Empty : obj.ToString());
                            strBuilder.Append("\t");
                        }
                        strBuilder.Append("\n");
                    }
                }
                #endregion

                HttpResponse response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.Buffer = false;
                response.Charset = "GB2312";
                response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(GetRandomFileName(fileName, ext), Encoding.UTF8));
                response.ContentEncoding = Encoding.GetEncoding("GB2312");

                response.ContentType = IsExcel2003 ? "application/ms-excel" : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                CultureInfo formatProvider = new CultureInfo("zh-CN", true);
                StringWriter writer = new StringWriter(formatProvider);

                response.Write(strBuilder.ToString());

                response.End();

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 生成随机文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string GetRandomFileName(string fileName, string ext)
        {
            string strFileName;
            Random random = new Random(DateTime.Now.Millisecond);
            string str = random.Next(0x7fffffff).ToString();

            if (!string.IsNullOrEmpty(fileName))
            {
                strFileName = fileName + "_" + str + "." + ext;
            }
            else
            {
                strFileName = str + "." + ext;
            }
            return strFileName;
        }
    }
}
