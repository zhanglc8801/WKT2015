using System;
using System.Collections.Generic;
using System.Data;

namespace Web.Site
{
    public class GetSiteSetCfg
    {

        /// <summary>
        /// ��ȡվ��ȫ��������Ϣ
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
        /// ��ȡվ�����������Ϣ
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