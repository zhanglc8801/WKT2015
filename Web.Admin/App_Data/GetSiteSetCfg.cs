using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;

namespace Web.Admin
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

            if (dt.Rows.Count == 0)
            {
                sql = "insert into SiteGlobalSettings(SiteID,SysSuperAdmin,IsEnableRegActivate,IsHideEditorInfoForAuthor,IsHideEditorInfoForExpert,ShowNameForHide,IsStopNotLoginDownPDF,ShowMoreFlowInfoForAuthor,ShowHistoryFlowInfoForExpert,IsAutoHandle,IsStatByGroup) values('" + SiteID + "','zqwyok@163.com','0','1','1','','0','1','1','0','1')";
                SQLiteHelper.ExeSql(sql);
            }

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