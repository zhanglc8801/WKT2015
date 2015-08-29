using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Data.SQL;
using WKT.Common.Utils;
using WKT.Common.Extension;
using HCMS.Utilities;

namespace WKT.DataAccess
{
    public partial class LoginDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db = null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public LoginDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if (db == null)
                throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static LoginDataAccess _instance = new LoginDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static LoginDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }

        # region 组装SQL条件

        /// <summary>
        /// 将查询实体转换为Where语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Where语句，不包含Where</returns>
        /// </summary>
        public string LoginQueryToSQLWhere(LoginErrorLogQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JournalID = " + query.JournalID);
            if (!string.IsNullOrWhiteSpace(query.LoginName))
            {
                sbWhere.Append(" AND LoginName = '").Append(query.LoginName).Append("'");
            }
            if (!string.IsNullOrWhiteSpace(query.LoginIP))
            {
                sbWhere.Append(" AND LoginIP = '").Append(query.LoginIP).Append("'");
            }
            if (!string.IsNullOrWhiteSpace(query.LoginHost))
            {
                sbWhere.Append(" AND LoginHost = '").Append(query.LoginHost).Append("'");
            }
            
            return sbWhere.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string LoginQueryToSQLOrder(LoginErrorLogQuery query)
        {
            return " PKID DESC";
        }

        #endregion 组装SQL条件

        #region 获取一个实体对象

        public LoginErrorLogEntity GetLoginErrorLog(string LoginName)
        {
            LoginErrorLogEntity loginErrorLogEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  * FROM dbo.LoginErrorLog WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  LoginName=@LoginName");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@LoginName", DbType.String, LoginName);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                loginErrorLogEntity = MakeLogin(dr);
            }
            return loginErrorLogEntity;
        }

        /// <summary>
        /// 获取登录错误日志信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public LoginErrorLogEntity GetLoginErrorLogInfo(LoginErrorLogQuery query)
        {
            LoginErrorLogEntity loginErrorLogEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  * FROM dbo.LoginErrorLog WITH(NOLOCK)");
            string whereSQL = LoginQueryToSQLWhere(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                loginErrorLogEntity = MakeLogin(dr);
            }
            return loginErrorLogEntity;
        }


        #endregion

        #region 根据条件获取所有实体对象

        public List<LoginErrorLogEntity> GetLoginErrorLogList()
        {
            List<LoginErrorLogEntity> loginErrorLogEntity = new List<LoginErrorLogEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT * FROM dbo.LoginErrorLog WITH(NOLOCK)");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                loginErrorLogEntity = MakeLoginList(dr);
            }
            return loginErrorLogEntity;
        }

        public List<LoginErrorLogEntity> GetLoginErrorLogList(LoginErrorLogQuery query)
        {
            List<LoginErrorLogEntity> list = new List<LoginErrorLogEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT * FROM dbo.LoginErrorLog WITH(NOLOCK)");
            string whereSQL = LoginQueryToSQLWhere(query);
            string orderBy = LoginQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeLoginList(dr);
            }
            return list;
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        public bool AddLoginErrorLog(LoginErrorLogEntity loginErrorLogEntity)
        {
            bool flag = false;
            DbCommand cmd = null;
           
            cmd = db.GetStoredProcCommand("dbo.UP_AddLoginErrorLog");
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, loginErrorLogEntity.JournalID);
            db.AddInParameter(cmd, "@LoginIP", DbType.AnsiString, loginErrorLogEntity.LoginIP);
            db.AddInParameter(cmd, "@LoginHost", DbType.AnsiString, loginErrorLogEntity.LoginHost);
            db.AddInParameter(cmd, "@LoginName", DbType.AnsiString, loginErrorLogEntity.LoginName);
            db.AddInParameter(cmd, "@AddDate", DbType.DateTime, loginErrorLogEntity.AddDate);
            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }

        #endregion

        #region 更新数据

        /// <summary>
        /// 修改登录错误日志信息
        /// </summary>
        /// <param name="AdminID"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        public bool UpdateLoginErrorLog(LoginErrorLogEntity LoginErrorLogItem)
        {
            bool flag = false;
            string sql = "UPDATE dbo.LoginErrorLog SET LoginName=@LoginName,LoginIP=@LoginIP,LoginHost=@LoginHost WHERE JournalID=@JournalID AND LoginName=@LoginName";
            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, LoginErrorLogItem.JournalID);
            db.AddInParameter(cmd, "@LoginName", DbType.Int64, LoginErrorLogItem.LoginName);
            db.AddInParameter(cmd, "@LoginIP", DbType.AnsiString, LoginErrorLogItem.LoginIP);
            db.AddInParameter(cmd, "@LoginHost", DbType.AnsiString, LoginErrorLogItem.LoginHost);

            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }
        #endregion

        #region 删除对象

        #region 删除一个对象

        public bool DeleteLoginErrorLog(LoginErrorLogQuery loginErrorLogQuery)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.LoginErrorLog");
            sqlCommandText.Append(" WHERE  JournalID=@JournalID AND LoginName=@LoginName");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, loginErrorLogQuery.JournalID);
            db.AddInParameter(cmd, "@LoginName", DbType.String, loginErrorLogQuery.LoginName);

            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }

        #endregion

        #endregion

        #region 根据数据组装一个对象

        public LoginErrorLogEntity MakeLogin(IDataReader dr)
        {
            LoginErrorLogEntity loginErrorLogEntity = null;
            if (dr.Read())
            {
                loginErrorLogEntity = new LoginErrorLogEntity();
                loginErrorLogEntity.JournalID = (Int64)dr["JournalID"];
                loginErrorLogEntity.LoginName = (String)dr["LoginName"];
                loginErrorLogEntity.LoginIP = (String)dr["LoginIP"];
                loginErrorLogEntity.LoginHost = (String)dr["LoginHost"];
                loginErrorLogEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return loginErrorLogEntity;
        }

        public LoginErrorLogEntity MakeLogin(DataRow dr)
        {
            LoginErrorLogEntity loginErrorLogEntity = null;
            if (dr != null)
            {
                loginErrorLogEntity = new LoginErrorLogEntity();
                loginErrorLogEntity.JournalID = (Int64)dr["JournalID"];
                loginErrorLogEntity.LoginName = (String)dr["LoginName"];
                loginErrorLogEntity.LoginIP = (String)dr["LoginIP"];
                loginErrorLogEntity.LoginHost = (String)dr["LoginHost"];
                loginErrorLogEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return loginErrorLogEntity;
        }
        #endregion

        #region 根据数据组装一组对象数据

        public List<LoginErrorLogEntity> MakeLoginList(IDataReader dr)
        {
            List<LoginErrorLogEntity> list = new List<LoginErrorLogEntity>();
            while (dr.Read())
            {
                LoginErrorLogEntity loginErrorLogEntity = new LoginErrorLogEntity();
                loginErrorLogEntity.JournalID = (Int64)dr["JournalID"];
                loginErrorLogEntity.LoginName = (String)dr["LoginName"];
                loginErrorLogEntity.LoginIP = (String)dr["LoginIP"];
                loginErrorLogEntity.LoginHost = (String)dr["LoginHost"];
                loginErrorLogEntity.AddDate = (DateTime)dr["AddDate"];

                list.Add(loginErrorLogEntity);

            }
            dr.Close();
            return list;
        }


        public List<LoginErrorLogEntity> MakeLoginList(DataTable dt)
        {
            List<LoginErrorLogEntity> list = new List<LoginErrorLogEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LoginErrorLogEntity loginErrorLogEntity = MakeLogin(dt.Rows[i]);
                    list.Add(loginErrorLogEntity);
                }
            }
            return list;
        }

        #endregion




    }
}
