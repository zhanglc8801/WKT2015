using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using WKT.Common.Utils;

namespace Web.Site
{
    public class SiteVisitLog
    {
        private static string sql = string.Empty;
        private static string ip = string.Empty;
        private static string addDate = string.Empty;
        
        public static int AddSiteVisit()
        {
            ip = Utils.GetRealIP();
            addDate = DateTime.Now.ToString("yyyy-MM-dd");
            sql = "select * from SiteVisitCount where IP='"+ip+"' and AddDate='"+addDate+"'";
            if (SQLiteHelper.GetTable(sql).Rows.Count == 0)
            {
                sql = "insert into SiteVisitCount(IP,AddDate) values(@IP,@AddDate)";
                SQLiteParameter[] pars ={
                    new SQLiteParameter("@IP",Utils.GetRealIP()),
                    new SQLiteParameter("@AddDate",DateTime.Now.ToString("yyyy-MM-dd"))};
                return SQLiteHelper.Execute(sql, pars);
            }
            else
            {
                return 0;
            }
        }



    }
}