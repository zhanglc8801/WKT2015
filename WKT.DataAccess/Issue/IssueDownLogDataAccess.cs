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
    public partial class IssueDownLogDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public IssueDownLogDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static IssueDownLogDataAccess _instance = new IssueDownLogDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static IssueDownLogDataAccess Instance
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
        public string IssueDownLogQueryToSQLWhere(IssueDownLogQuery query)
        {
            return string.Empty;
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string IssueDownLogQueryToSQLOrder(IssueDownLogQuery query)
        {
            return " DownLogID DESC";
        }

        #endregion 组装SQL条件

        #region 获取一个实体对象

        public IssueDownLogEntity GetIssueDownLog(Int64 downLogID)
        {
            IssueDownLogEntity issueDownLogEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  DownLogID,JournalID,ContentID,AuthorID,Daytime,Year,Month,IP,AddDate FROM dbo.IssueDownLog WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  DownLogID=@DownLogID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@DownLogID", DbType.Int64, downLogID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                issueDownLogEntity = MakeIssueDownLog(dr);
            }
            return issueDownLogEntity;
        }

        #endregion

        #region 根据条件获取所有实体对象

        public List<IssueDownLogEntity> GetIssueDownLogList()
        {
            List<IssueDownLogEntity> issueDownLogEntity = new List<IssueDownLogEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  DownLogID,JournalID,ContentID,AuthorID,Daytime,Year,Month,IP,AddDate FROM dbo.IssueDownLog WITH(NOLOCK)");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                issueDownLogEntity = MakeIssueDownLogList(dr);
            }
            return issueDownLogEntity;
        }

        public List<IssueDownLogEntity> GetIssueDownLogList(IssueDownLogQuery query)
        {
            List<IssueDownLogEntity> list = new List<IssueDownLogEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT DownLogID,JournalID,ContentID,AuthorID,Daytime,Year,Month,IP,AddDate FROM dbo.IssueDownLog WITH(NOLOCK)");
            string whereSQL = IssueDownLogQueryToSQLWhere(query);
            string orderBy = IssueDownLogQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeIssueDownLogList(dr);
            }
            return list;
        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<IssueDownLogEntity></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("IssueDownLog", "DownLogID,JournalID,ContentID,AuthorID,Daytime,Year,Month,IP,AddDate", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<IssueDownLogEntity> pager = new Pager<IssueDownLogEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeIssueDownLogList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(QueryBase query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("IssueDownLog", "DownLogID,JournalID,ContentID,AuthorID,Daytime,Year,Month,IP,AddDate", " DownLogID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<IssueDownLogEntity> pager = new Pager<IssueDownLogEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeIssueDownLogList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(IssueDownLogQuery query)
        {
            int recordCount = 0;
            string whereSQL = IssueDownLogQueryToSQLWhere(query);
            string orderBy = IssueDownLogQueryToSQLOrder(query);
            DataSet ds = db.GetPagingData("IssueDownLog", "DownLogID,JournalID,ContentID,AuthorID,Daytime,Year,Month,IP,AddDate", orderBy, whereSQL, query.CurrentPage, query.PageSize, out recordCount);
            Pager<IssueDownLogEntity> pager = new Pager<IssueDownLogEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeIssueDownLogList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        public bool AddIssueDownLog(IssueDownLogEntity issueDownLogEntity)
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

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.IssueDownLog ({0}) VALUES ({1})", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, issueDownLogEntity.JournalID);
            db.AddInParameter(cmd, "@ContentID", DbType.Int64, issueDownLogEntity.ContentID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, issueDownLogEntity.AuthorID);
            db.AddInParameter(cmd, "@Daytime", DbType.Int32, issueDownLogEntity.Daytime);
            db.AddInParameter(cmd, "@Year", DbType.Int32, issueDownLogEntity.Year);
            db.AddInParameter(cmd, "@Month", DbType.Int32, issueDownLogEntity.Month);
            db.AddInParameter(cmd, "@IP", DbType.AnsiString, issueDownLogEntity.IP);
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

        public IssueDownLogEntity MakeIssueDownLog(IDataReader dr)
        {
            IssueDownLogEntity issueDownLogEntity = null;
            if (dr.Read())
            {
                issueDownLogEntity = new IssueDownLogEntity();
                issueDownLogEntity.DownLogID = (Int64)dr["DownLogID"];
                issueDownLogEntity.JournalID = (Int64)dr["JournalID"];
                issueDownLogEntity.ContentID = (Int64)dr["ContentID"];
                issueDownLogEntity.AuthorID = (Int64)dr["AuthorID"];
                issueDownLogEntity.Daytime = (Int32)dr["Daytime"];
                issueDownLogEntity.Year = (Int32)dr["Year"];
                issueDownLogEntity.Month = (Int32)dr["Month"];
                issueDownLogEntity.IP = (String)dr["IP"];
                issueDownLogEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return issueDownLogEntity;
        }

        public IssueDownLogEntity MakeIssueDownLog(DataRow dr)
        {
            IssueDownLogEntity issueDownLogEntity = null;
            if (dr != null)
            {
                issueDownLogEntity = new IssueDownLogEntity();
                issueDownLogEntity.DownLogID = (Int64)dr["DownLogID"];
                issueDownLogEntity.JournalID = (Int64)dr["JournalID"];
                issueDownLogEntity.ContentID = (Int64)dr["ContentID"];
                issueDownLogEntity.AuthorID = (Int64)dr["AuthorID"];
                issueDownLogEntity.Daytime = (Int32)dr["Daytime"];
                issueDownLogEntity.Year = (Int32)dr["Year"];
                issueDownLogEntity.Month = (Int32)dr["Month"];
                issueDownLogEntity.IP = (String)dr["IP"];
                issueDownLogEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return issueDownLogEntity;
        }
        #endregion

        #region 根据数据组装一组对象数据

        public List<IssueDownLogEntity> MakeIssueDownLogList(IDataReader dr)
        {
            List<IssueDownLogEntity> list = new List<IssueDownLogEntity>();
            while (dr.Read())
            {
                IssueDownLogEntity issueDownLogEntity = new IssueDownLogEntity();
                issueDownLogEntity.DownLogID = (Int64)dr["DownLogID"];
                issueDownLogEntity.JournalID = (Int64)dr["JournalID"];
                issueDownLogEntity.ContentID = (Int64)dr["ContentID"];
                issueDownLogEntity.AuthorID = (Int64)dr["AuthorID"];
                issueDownLogEntity.Daytime = (Int32)dr["Daytime"];
                issueDownLogEntity.Year = (Int32)dr["Year"];
                issueDownLogEntity.Month = (Int32)dr["Month"];
                issueDownLogEntity.IP = (String)dr["IP"];
                issueDownLogEntity.AddDate = (DateTime)dr["AddDate"];
                list.Add(issueDownLogEntity);
            }
            dr.Close();
            return list;
        }


        public List<IssueDownLogEntity> MakeIssueDownLogList(DataTable dt)
        {
            List<IssueDownLogEntity> list = new List<IssueDownLogEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IssueDownLogEntity issueDownLogEntity = MakeIssueDownLog(dt.Rows[i]);
                    list.Add(issueDownLogEntity);
                }
            }
            return list;
        }

        #endregion

    }
}

