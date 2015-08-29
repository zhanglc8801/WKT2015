using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Data.SQL;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class IssueViewLogDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public IssueViewLogDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static IssueViewLogDataAccess _instance = new IssueViewLogDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static IssueViewLogDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }

        #region 组装SQL条件

        /// <summary>
        /// 将查询实体转换为Where语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Where语句，不包含Where</returns>
        /// </summary>
        public string IssueViewLogQueryToSQLWhere(IssueViewLogQuery query)
        {
            return string.Empty;
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string IssueViewLogQueryToSQLOrder(IssueViewLogQuery query)
        {
            return " ViewLogID DESC";
        }

        #endregion 组装SQL条件

        #region 获取一个实体对象

        public IssueViewLogEntity GetIssueViewLog(Int64 viewLogID)
        {
            IssueViewLogEntity issueViewLogEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  ViewLogID,JournalID,ContentID,AuthorID,Daytime,Year,Month,IP,AddDate FROM dbo.IssueViewLog WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  ViewLogID=@ViewLogID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@ViewLogID", DbType.Int64, viewLogID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                issueViewLogEntity = MakeIssueViewLog(dr);
            }
            return issueViewLogEntity;
        }

        #endregion

        #region 根据条件获取所有实体对象

        public List<IssueViewLogEntity> GetIssueViewLogList()
        {
            List<IssueViewLogEntity> issueViewLogEntity = new List<IssueViewLogEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  ViewLogID,JournalID,ContentID,AuthorID,Daytime,Year,Month,IP,AddDate FROM dbo.IssueViewLog WITH(NOLOCK)");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                issueViewLogEntity = MakeIssueViewLogList(dr);
            }
            return issueViewLogEntity;
        }

        public List<IssueViewLogEntity> GetIssueViewLogList(IssueViewLogQuery query)
        {
            List<IssueViewLogEntity> list = new List<IssueViewLogEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT ViewLogID,JournalID,ContentID,AuthorID,Daytime,Year,Month,IP,AddDate FROM dbo.IssueViewLog WITH(NOLOCK)");
            string whereSQL = IssueViewLogQueryToSQLWhere(query);
            string orderBy = IssueViewLogQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeIssueViewLogList(dr);
            }
            return list;
        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<IssueViewLogEntity></returns>
        public Pager<IssueViewLogEntity> GetIssueViewLogPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("IssueViewLog", "ViewLogID,JournalID,ContentID,AuthorID,Daytime,Year,Month,IP,AddDate", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<IssueViewLogEntity> pager = new Pager<IssueViewLogEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeIssueViewLogList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<IssueViewLogEntity> GetIssueViewLogPageList(QueryBase query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("IssueViewLog", "ViewLogID,JournalID,ContentID,AuthorID,Daytime,Year,Month,IP,AddDate", " ViewLogID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<IssueViewLogEntity> pager = new Pager<IssueViewLogEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeIssueViewLogList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<IssueViewLogEntity> GetIssueViewLogPageList(IssueViewLogQuery query)
        {
            int recordCount = 0;
            string whereSQL = IssueViewLogQueryToSQLWhere(query);
            string orderBy = IssueViewLogQueryToSQLOrder(query);
            DataSet ds = db.GetPagingData("IssueViewLog", "ViewLogID,JournalID,ContentID,AuthorID,Daytime,Year,Month,IP,AddDate", orderBy, whereSQL, query.CurrentPage, query.PageSize, out recordCount);
            Pager<IssueViewLogEntity> pager = new Pager<IssueViewLogEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeIssueViewLogList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        public bool AddIssueViewLog(IssueViewLogEntity issueViewLogEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("@JournalID");
            sqlCommandText.Append(", @ContentID");
            sqlCommandText.Append(", @AuthorID");
            sqlCommandText.Append(", @Daytime");
            sqlCommandText.Append(", @Year");
            sqlCommandText.Append(", @Month");
            sqlCommandText.Append(", @IP");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.IssueViewLog ({0}) VALUES ({1})", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, issueViewLogEntity.JournalID);
            db.AddInParameter(cmd, "@ContentID", DbType.Int64, issueViewLogEntity.ContentID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, issueViewLogEntity.AuthorID);
            db.AddInParameter(cmd, "@Daytime", DbType.Int32, issueViewLogEntity.Daytime);
            db.AddInParameter(cmd, "@Year", DbType.Int32, issueViewLogEntity.Year);
            db.AddInParameter(cmd, "@Month", DbType.Int32, issueViewLogEntity.Month);
            db.AddInParameter(cmd, "@IP", DbType.AnsiString, issueViewLogEntity.IP);
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

        #region 根据数据组装一个对象

        public IssueViewLogEntity MakeIssueViewLog(IDataReader dr)
        {
            IssueViewLogEntity issueViewLogEntity = null;
            if (dr.Read())
            {
                issueViewLogEntity = new IssueViewLogEntity();
                issueViewLogEntity.ViewLogID = (Int64)dr["ViewLogID"];
                issueViewLogEntity.JournalID = (Int64)dr["JournalID"];
                issueViewLogEntity.ContentID = (Int64)dr["ContentID"];
                issueViewLogEntity.AuthorID = (Int64)dr["AuthorID"];
                issueViewLogEntity.Daytime = (Int32)dr["Daytime"];
                issueViewLogEntity.Year = (Int32)dr["Year"];
                issueViewLogEntity.Month = (Int32)dr["Month"];
                issueViewLogEntity.IP = (String)dr["IP"];
                issueViewLogEntity.AddDate = Convert.IsDBNull(dr["AddDate"]) ? null : (DateTime?)dr["AddDate"];
            }
            dr.Close();
            return issueViewLogEntity;
        }

        public IssueViewLogEntity MakeIssueViewLog(DataRow dr)
        {
            IssueViewLogEntity issueViewLogEntity = null;
            if (dr != null)
            {
                issueViewLogEntity = new IssueViewLogEntity();
                issueViewLogEntity.ViewLogID = (Int64)dr["ViewLogID"];
                issueViewLogEntity.JournalID = (Int64)dr["JournalID"];
                issueViewLogEntity.ContentID = (Int64)dr["ContentID"];
                issueViewLogEntity.AuthorID = (Int64)dr["AuthorID"];
                issueViewLogEntity.Daytime = (Int32)dr["Daytime"];
                issueViewLogEntity.Year = (Int32)dr["Year"];
                issueViewLogEntity.Month = (Int32)dr["Month"];
                issueViewLogEntity.IP = (String)dr["IP"];
                issueViewLogEntity.AddDate = Convert.IsDBNull(dr["AddDate"]) ? null : (DateTime?)dr["AddDate"];
            }
            return issueViewLogEntity;
        }
        #endregion

        #region 根据数据组装一组对象数据

        public List<IssueViewLogEntity> MakeIssueViewLogList(IDataReader dr)
        {
            List<IssueViewLogEntity> list = new List<IssueViewLogEntity>();
            while (dr.Read())
            {
                IssueViewLogEntity issueViewLogEntity = new IssueViewLogEntity();
                issueViewLogEntity.ViewLogID = (Int64)dr["ViewLogID"];
                issueViewLogEntity.JournalID = (Int64)dr["JournalID"];
                issueViewLogEntity.ContentID = (Int64)dr["ContentID"];
                issueViewLogEntity.AuthorID = (Int64)dr["AuthorID"];
                issueViewLogEntity.Daytime = (Int32)dr["Daytime"];
                issueViewLogEntity.Year = (Int32)dr["Year"];
                issueViewLogEntity.Month = (Int32)dr["Month"];
                issueViewLogEntity.IP = (String)dr["IP"];
                issueViewLogEntity.AddDate = Convert.IsDBNull(dr["AddDate"]) ? null : (DateTime?)dr["AddDate"];
                list.Add(issueViewLogEntity);
            }
            dr.Close();
            return list;
        }


        public List<IssueViewLogEntity> MakeIssueViewLogList(DataTable dt)
        {
            List<IssueViewLogEntity> list = new List<IssueViewLogEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IssueViewLogEntity issueViewLogEntity = MakeIssueViewLog(dt.Rows[i]);
                    list.Add(issueViewLogEntity);
                }
            }
            return list;
        }

        #endregion

    }
}

