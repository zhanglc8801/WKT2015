using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.Data;
using WKT.Common.Utils;
using WKT.Config;

namespace Web.Admin
{
    public class SQLiteHelper
    {
        private static string dbPath = Utils.GetMapPath(SiteConfig.RootPath + "/config/SiteSet.Syscfg");
        private static string connStr = "Data Source=" + dbPath + ";Pooling = true;FaillfMissing = false";
        private static SQLiteConnection conn;
        /// <summary>
        /// 打开可用的连接
        /// </summary>
        public static SQLiteConnection Conn
        {
            get
            {
                if (conn == null)
                {
                    conn = new SQLiteConnection(connStr);
                    conn.Open();
                }
                else if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                }
                else if (conn.State == System.Data.ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }
                return SQLiteHelper.conn;
            }

        }
        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataTable GetTable(string strSql)
        {
            DataTable table = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(strSql, Conn);
            adapter.Fill(table);
            return table;
        }
        /// <summary>
        /// 执行有参数的检索语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static DataTable GetTable(string strSql, params SQLiteParameter[] pars)
        {
            SQLiteCommand cmd = new SQLiteCommand(strSql, SQLiteHelper.Conn);
            cmd.Parameters.AddRange(pars);
            DataTable table = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(table);
            return table;
        }
        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static int Execute(string sql, params SQLiteParameter[] pars)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, Conn);
            cmd.Parameters.AddRange(pars);
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行SQLite命令
        /// </summary>
        /// <param name="sql"></param>
        public static void ExeSql(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, Conn);
            cmd.ExecuteNonQuery();
        }

    }
}