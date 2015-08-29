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
    public partial class FlowAuthRoleDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public FlowAuthRoleDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static FlowAuthRoleDataAccess _instance = new FlowAuthRoleDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static FlowAuthRoleDataAccess Instance
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
        public string FlowAuthRoleQueryToSQLWhere(FlowAuthRoleQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JournalID = " + query.JournalID + " AND ActionID = " + query.ActionID);
            return sbWhere.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string FlowAuthRoleQueryToSQLOrder(FlowAuthRoleQuery query)
        {
            return " RoleAuthID DESC";
        }
        
        #endregion 组装SQL条件

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="flowAuthRoleEntity"></param>
        /// <returns></returns>
        public bool AddFlowAuthRole(FlowAuthRoleEntity flowAuthRoleEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @ActionID");
            sqlCommandText.Append(", @RoleID");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.FlowAuthRole ({0}) VALUES ({1})", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, flowAuthRoleEntity.JournalID);
            db.AddInParameter(cmd, "@ActionID", DbType.Int64, flowAuthRoleEntity.ActionID);
            db.AddInParameter(cmd, "@RoleID", DbType.Int64, flowAuthRoleEntity.RoleID);

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
        
        public List<FlowAuthRoleEntity> GetFlowAuthRoleList(FlowAuthRoleQuery query)
        {
            List<FlowAuthRoleEntity> list = new List<FlowAuthRoleEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT RoleAuthID,JournalID,ActionID,RoleID,AddDate FROM dbo.FlowAuthRole WITH(NOLOCK)");
            string whereSQL = FlowAuthRoleQueryToSQLWhere(query);
            string orderBy = FlowAuthRoleQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeFlowAuthRoleList(dr);
            }
            return list;
        }
        
        #endregion

        # region 设置审稿流程环节角色权限

        /// <summary>
        /// 设置审稿流程环节角色权限
        /// </summary>
        /// <param name="flowAuthRoleList">审稿流程环节角色权限列表</param>
        /// <returns></returns>
        public bool SaveFlowAuthRole(List<FlowAuthRoleEntity> flowAuthRoleList)
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
                            string sqlCreateTempTable = "CREATE TABLE #FlowAuthRoleTable(JournalID BIGINT,ActionID BIGINT,RoleID BIGINT)";
                            string execSql = @"MERGE dbo.FlowAuthRole AS t
                                USING #FlowAuthRoleTable AS s ON t.JournalID=s.JournalID AND t.ActionID=s.ActionID AND t.RoleID=s.RoleID
                                WHEN NOT MATCHED THEN INSERT(JournalID,ActionID,RoleID) VALUES(s.JournalID,s.ActionID,s.RoleID);
                                DROP TABLE #FlowAuthRoleTable";

                            # region 1.创建临时表

                            DbCommand cmdCreateTempTable = db.GetSqlStringCommand(sqlCreateTempTable);
                            db.ExecuteNonQuery(cmdCreateTempTable, (DbTransaction)transaction);

                            # endregion

                            # region 2.往临时表中写入数据

                            // 开始批量插入
                            DataTable dtFlowAuthRole = ConstructionDT();
                            foreach (FlowAuthRoleEntity item in flowAuthRoleList)
                            {
                                DataRow row = dtFlowAuthRole.NewRow();
                                row["JournalID"] = item.JournalID;
                                row["ActionID"] = item.ActionID;
                                row["RoleID"] = item.RoleID;
                                dtFlowAuthRole.Rows.Add(row);
                            }

                            SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction);
                            bulkCopy.DestinationTableName = "#FlowAuthRoleTable";
                            bulkCopy.WriteToServer(dtFlowAuthRole);


                            # endregion

                            # region 3.设置审稿环节角色权限，并删除临时表

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
            DataTable menuTempTable = new DataTable("FlowAuthRole");
            menuTempTable.Columns.Add("JournalID", Type.GetType("System.Int64"));
            menuTempTable.Columns.Add("ActionID", Type.GetType("System.Int64"));
            menuTempTable.Columns.Add("RoleID", Type.GetType("System.Int64"));
            return menuTempTable;
        }

        # endregion

        #region 批量删除

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteAuthRole(List<FlowAuthRoleEntity> flowAuthRoleList)
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
                            string sqlCreateTempTable = "CREATE TABLE #FlowAuthRoleDelTable(JournalID BIGINT,ActionID BIGINT,RoleID BIGINT)";
                            string execSql = @"DELETE dbo.FlowAuthRole 
                                                FROM dbo.FlowAuthRole t,#FlowAuthRoleDelTable s
                                                WHERE t.JournalID=s.JournalID AND t.ActionID=s.ActionID AND t.RoleID=s.RoleID
                                                DROP TABLE #FlowAuthRoleDelTable";

                            # region 1.创建临时表

                            DbCommand cmdCreateTempTable = db.GetSqlStringCommand(sqlCreateTempTable);
                            db.ExecuteNonQuery(cmdCreateTempTable, (DbTransaction)transaction);

                            # endregion

                            # region 2.往临时表中写入数据

                            // 开始批量插入
                            DataTable dtFlowAuthRole = ConstructionDT();
                            foreach (FlowAuthRoleEntity item in flowAuthRoleList)
                            {
                                DataRow row = dtFlowAuthRole.NewRow();
                                row["JournalID"] = item.JournalID;
                                row["ActionID"] = item.ActionID;
                                row["RoleID"] = item.RoleID;
                                dtFlowAuthRole.Rows.Add(row);
                            }

                            SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction);
                            bulkCopy.DestinationTableName = "#FlowAuthRoleDelTable";
                            bulkCopy.WriteToServer(dtFlowAuthRole);


                            # endregion

                            # region 3.删除角色权限，并删除临时表

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
        
        #region 根据数据组装一个对象
        
        public FlowAuthRoleEntity MakeFlowAuthRole(DataRow dr)
        {
            FlowAuthRoleEntity flowAuthRoleEntity=null;
            if(dr!=null)
            {
                 flowAuthRoleEntity=new FlowAuthRoleEntity();
                 flowAuthRoleEntity.RoleAuthID = (Int64)dr["RoleAuthID"];
                 flowAuthRoleEntity.JournalID = (Int64)dr["JournalID"];
                 flowAuthRoleEntity.ActionID = (Int64)dr["ActionID"];
                 flowAuthRoleEntity.RoleID = (Int64)dr["RoleID"];
                 flowAuthRoleEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return flowAuthRoleEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据

        public List<FlowAuthRoleEntity> MakeFlowAuthRoleList(IDataReader dr)
        {
            List<FlowAuthRoleEntity> list = new List<FlowAuthRoleEntity>();
            while (dr.Read())
            {
                FlowAuthRoleEntity flowAuthRoleEntity = new FlowAuthRoleEntity();
                flowAuthRoleEntity.RoleAuthID = (Int64)dr["RoleAuthID"];
                flowAuthRoleEntity.JournalID = (Int64)dr["JournalID"];
                flowAuthRoleEntity.ActionID = (Int64)dr["ActionID"];
                flowAuthRoleEntity.RoleID = (Int64)dr["RoleID"];
                flowAuthRoleEntity.AddDate = (DateTime)dr["AddDate"];
                list.Add(flowAuthRoleEntity);
            }
            dr.Close();
            return list;
        }
        
        public List<FlowAuthRoleEntity> MakeFlowAuthRoleList(DataTable dt)
        {
            List<FlowAuthRoleEntity> list=new List<FlowAuthRoleEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   FlowAuthRoleEntity flowAuthRoleEntity=MakeFlowAuthRole(dt.Rows[i]);
                   list.Add(flowAuthRoleEntity);
                }
            }
            return list;
        }
        
        #endregion

    }
}

