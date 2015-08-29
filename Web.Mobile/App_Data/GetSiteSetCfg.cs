using System;
using System.Collections.Generic;
using System.Data;

namespace Web.Mobile
{
    public class GetSiteSetCfg
    {

        /// <summary>
        /// 获取站点全局设置信息
        /// </summary>
        /// <param name="SiteID"></param>
        /// <returns></returns>
        public static DataTable GetSiteGlobalSet(string SiteID)
        {
            SiteGlobalSettings SiteGlobalSet = new SiteGlobalSettings();
            string sql = "select * from SiteGlobalSettings where SiteID='" + SiteID + "' Limit 1";
            DataTable dt = SQLiteHelper.GetTable(sql);
            return dt;
        }

        /// <summary>
        /// 获取站点个人设置信息
        /// </summary>
        /// <param name="SiteID"></param>
        /// <param name="EditorID"></param>
        /// <returns></returns>
        public static DataTable GetSitePersonalSet(string SiteID,string EditorID)
        {
            SiteGlobalSettings SiteGlobalSet = new SiteGlobalSettings();
            string sql = "select * from SitePersonalSettings where SiteID='" + SiteID + "' and EditorID='" + EditorID + "' Limit 1";
            DataTable dt = SQLiteHelper.GetTable(sql);
            return dt;
        }
        

    }
}