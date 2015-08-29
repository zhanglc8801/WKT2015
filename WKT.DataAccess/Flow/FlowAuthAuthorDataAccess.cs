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
    public partial class FlowAuthAuthorDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public FlowAuthAuthorDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static FlowAuthAuthorDataAccess _instance = new FlowAuthAuthorDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static FlowAuthAuthorDataAccess Instance
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
        public string FlowAuthAuthorQueryToSQLWhere(FlowAuthAuthorQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JournalID = " + query.JournalID + " AND ActionID = " + query.ActionID);
            return sbWhere.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string FlowAuthAuthorQueryToSQLOrder(FlowAuthAuthorQuery query)
        {
            return " AuthorAuthID DESC";
        }
        
        #endregion 组装SQL条件

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="flowAuthAuthorEntity"></param>
        /// <returns></returns>
        public bool AddFlowAuthAuthor(FlowAuthAuthorEntity flowAuthAuthorEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @ActionID");
            sqlCommandText.Append(", @AuthorID");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.FlowAuthAuthor ({0}) VALUES ({1})", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, flowAuthAuthorEntity.JournalID);
            db.AddInParameter(cmd, "@ActionID", DbType.Int64, flowAuthAuthorEntity.ActionID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, flowAuthAuthorEntity.AuthorID);
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
        
        #region 根据查询条件分页获取对象
        
        public List<FlowAuthAuthorEntity> GetFlowAuthAuthorList(FlowAuthAuthorQuery query)
        {
            List<FlowAuthAuthorEntity> list = new List<FlowAuthAuthorEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT AuthorAuthID,JournalID,ActionID,AuthorID,AddDate FROM dbo.FlowAuthAuthor WITH(NOLOCK)");
            string whereSQL = FlowAuthAuthorQueryToSQLWhere(query);
            string orderBy = FlowAuthAuthorQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeFlowAuthAuthorList(dr);
            }
            return list;
        }
        
        #endregion

        # region 设置审稿流程环节作者权限

        /// <summary>
        /// 设置审稿流程环节作者权限
        /// </summary>
        /// <param name="flowAuthAuthorList">审稿流程环节作者权限列表</param>
        /// <returns></returns>
        public bool SaveFlowAuthAuthor(List<FlowAuthAuthorEntity> flowAuthAuthorList)
        {
            bool flag = false;
            try
            {
                // 先删除
                using (IDbConnection connection = db.CreateConnection())
                {
                    connection.Open();

                    // 启动事务
                    using (IDbTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string sqlCreateTempTable = "CREATE TABLE #FlowAuthAuthorTable(JournalID BIGINT,ActionID BIGINT,AuthorID BIGINT)";
                            string execSql = @"MERGE dbo.FlowAuthAuthor AS t
                                USING #FlowAuthAuthorTable AS s ON t.JournalID=s.JournalID AND t.ActionID=s.ActionID AND t.AuthorID=s.AuthorID
                                WHEN NOT MATCHED THEN INSERT(JournalID,ActionID,AuthorID) VALUES(s.JournalID,s.ActionID,s.AuthorID);
                                DROP TABLE #FlowAuthAuthorTable";

                            # region 1.创建临时表

                            DbCommand cmdCreateTempTable = db.GetSqlStringCommand(sqlCreateTempTable);
                            db.ExecuteNonQuery(cmdCreateTempTable, (DbTransaction)transaction);

                            # endregion

                            # region 2.往临时表中写入数据

                            // 开始批量插入
                            DataTable dtFlowAuthAuthor = ConstructionDT();
                            foreach (FlowAuthAuthorEntity item in flowAuthAuthorList)
                            {
                                DataRow row = dtFlowAuthAuthor.NewRow();
                                row["JournalID"] = item.JournalID;
                                row["ActionID"] = item.ActionID;
                                row["AuthorID"] = item.AuthorID;
                                dtFlowAuthAuthor.Rows.Add(row);
                            }

                            SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction);
                            bulkCopy.DestinationTableName = "#FlowAuthAuthorTable";
                            bulkCopy.WriteToServer(dtFlowAuthAuthor);


                            # endregion

                            # region 3.设置审稿环节作者权限，并删除临时表

                            DbCommand cmdSet = db.GetSqlStringCommand(execSql);
                            db.ExecuteNonQuery(cmdSet, (DbTransaction)transaction);

                            # endregion

                            transaction.Commit();
                        }
                        catch (Exception ext)
                        {
                            transaction.Rollback();
                            throw ext;
                        }
                    }

                    connection.Close();
                }
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        ///  构造批量插入的Table
        /// </summary>
        /// <returns></returns>
        private DataTable ConstructionDT()
        {
            DataTable menuTempTable = new DataTable("FlowAuthAuthor");
            menuTempTable.Columns.Add("JournalID", Type.GetType("System.Int64"));
            menuTempTable.Columns.Add("ActionID", Type.GetType("System.Int64"));
            menuTempTable.Columns.Add("AuthorID", Type.GetType("System.Int64"));
            return menuTempTable;
        }

        # endregion

        #region 批量删除

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteAuthAuthor(List<FlowAuthAuthorEntity> flowAuthAuthorList)
        {
            bool flag = false;
            try
            {
                // 先删除
                using (IDbConnection connection = db.CreateConnection())
                {
                    connection.Open();

                    // 启动事务
                    using (IDbTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string sqlCreateTempTable = "CREATE TABLE #FlowAuthAuthorDelTable(JournalID BIGINT,ActionID BIGINT,AuthorID BIGINT)";
                            string execSql = @"DELETE dbo.FlowAuthAuthor 
                                                FROM dbo.FlowAuthAuthor t,#FlowAuthAuthorDelTable s
                                                WHERE t.JournalID=s.JournalID AND t.ActionID=s.ActionID AND t.AuthorID=s.AuthorID
                                                DROP TABLE #FlowAuthAuthorDelTable";

                            # region 1.创建临时表

                            DbCommand cmdCreateTempTable = db.GetSqlStringCommand(sqlCreateTempTable);
                            db.ExecuteNonQuery(cmdCreateTempTable, (DbTransaction)transaction);

                            # endregion

                            # region 2.往临时表中写入数据

                            // 开始批量插入
                            DataTable dtFlowAuthAuthor= ConstructionDT();
                            foreach (FlowAuthAuthorEntity item in flowAuthAuthorList)
                            {
                                DataRow row = dtFlowAuthAuthor.NewRow();
                                row["JournalID"] = item.JournalID;
                                row["ActionID"] = item.ActionID;
                                row["AuthorID"] = item.AuthorID;
                                dtFlowAuthAuthor.Rows.Add(row);
                            }

                            SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction);
                            bulkCopy.DestinationTableName = "#FlowAuthAuthorDelTable";
                            bulkCopy.WriteToServer(dtFlowAuthAuthor);


                            # endregion

                            # region 3.删除成员权限，并删除临时表

                            DbCommand cmdSet = db.GetSqlStringCommand(execSql);
                            db.ExecuteNonQuery(cmdSet, (DbTransaction)transaction);

                            # endregion

                            transaction.Commit();
                        }
                        catch (Exception ext)
                        {
                            transaction.Rollback();
                            throw ext;
                        }
                    }

                    connection.Close();
                }
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        #endregion

        # region 根据数据组装一个对象

        public FlowAuthAuthorEntity MakeFlowAuthAuthor(IDataReader dr)
        {
            FlowAuthAuthorEntity flowAuthAuthorEntity = null;
            if (dr.Read())
            {
                flowAuthAuthorEntity = new FlowAuthAuthorEntity();
                flowAuthAuthorEntity.AuthorAuthID = (Int64)dr["AuthorAuthID"];
                flowAuthAuthorEntity.JournalID = (Int64)dr["JournalID"];
                flowAuthAuthorEntity.ActionID = (Int64)dr["ActionID"];
                flowAuthAuthorEntity.AuthorID = (Int64)dr["AuthorID"];
                flowAuthAuthorEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return flowAuthAuthorEntity;
        }

        public FlowAuthAuthorEntity MakeFlowAuthAuthor(DataRow dr)
        {
            FlowAuthAuthorEntity flowAuthAuthorEntity = null;
            if (dr != null)
            {
                flowAuthAuthorEntity = new FlowAuthAuthorEntity();
                flowAuthAuthorEntity.AuthorAuthID = (Int64)dr["AuthorAuthID"];
                flowAuthAuthorEntity.JournalID = (Int64)dr["JournalID"];
                flowAuthAuthorEntity.ActionID = (Int64)dr["ActionID"];
                flowAuthAuthorEntity.AuthorID = (Int64)dr["AuthorID"];
                flowAuthAuthorEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return flowAuthAuthorEntity;
        }

        # endregion
        
        # region 根据数据组装一组对象数据
        
        public List<FlowAuthAuthorEntity> MakeFlowAuthAuthorList(IDataReader dr)
        {
            List<FlowAuthAuthorEntity> list=new List<FlowAuthAuthorEntity>();
            while(dr.Read())
            {
             FlowAuthAuthorEntity flowAuthAuthorEntity=new FlowAuthAuthorEntity();
            flowAuthAuthorEntity.AuthorAuthID = (Int64)dr["AuthorAuthID"];
            flowAuthAuthorEntity.JournalID = (Int64)dr["JournalID"];
            flowAuthAuthorEntity.AuthorID = (Int64)dr["AuthorID"];
            flowAuthAuthorEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(flowAuthAuthorEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<FlowAuthAuthorEntity> MakeFlowAuthAuthorList(DataTable dt)
        {
            List<FlowAuthAuthorEntity> list=new List<FlowAuthAuthorEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   FlowAuthAuthorEntity flowAuthAuthorEntity=MakeFlowAuthAuthor(dt.Rows[i]);
                   list.Add(flowAuthAuthorEntity);
                }
            }
            return list;
        }
        
        # endregion
    }
}

