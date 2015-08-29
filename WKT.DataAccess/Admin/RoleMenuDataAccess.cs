using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Data.SQL;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class RoleMenuDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public RoleMenuDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static RoleMenuDataAccess _instance = new RoleMenuDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static RoleMenuDataAccess Instance
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
        public string RoleMenuQueryToSQLWhere(RoleMenuQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JournalID = " + query.JournalID);
            if (query.RoleID != null)
            {
                sbWhere.Append(" AND RoleID = ").Append(query.RoleID.Value);
            }
            if (query.RoleIDList != null)
            {
                if (query.RoleIDList.Count > 0)
                {
                    if (query.RoleIDList.Count == 1)
                    {
                        sbWhere.Append(" AND RoleID = ").Append(query.RoleIDList[0]);
                    }
                    else
                    {
                        sbWhere.Append(" AND RoleID IN (").Append(string.Join(",", query.RoleIDList)).Append(")");
                    }
                }
            }
            return sbWhere.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string RoleMenuQueryToSQLOrder(RoleMenuQuery query)
        {
            return " MapID DESC";
        }
        
        #endregion 组装SQL条件

        # region 查询

        /// <summary>
        /// 获取指定角色拥有的菜单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long, long> GetRoleMenuDict(RoleMenuQuery query)
        {
            IDictionary<long, long> dictRoleMap = new Dictionary<long, long>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT MapID,JournalID,RoleID,MenuID,AddDate FROM dbo.RoleMenu WITH(NOLOCK)");
            string whereSQL = RoleMenuQueryToSQLWhere(query);
            string orderBy = RoleMenuQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            long MenuID = 0;
            long RoleID = 0;
            int MenuIDIndex = 0;
            int RoleIDIndex = 0;
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                MenuIDIndex = dr.GetOrdinal("MenuID");
                RoleIDIndex = dr.GetOrdinal("RoleID");
                while (dr.Read())
                {
                    MenuID = WKT.Common.Utils.TypeParse.ToLong(dr[MenuIDIndex], 0);
                    RoleID = WKT.Common.Utils.TypeParse.ToLong(dr[RoleIDIndex], 0);
                    if (!dictRoleMap.ContainsKey(MenuID))
                    {
                        dictRoleMap.Add(MenuID, RoleID);
                    }
                }
                dr.Close();
            }
            return dictRoleMap;
        }

        /// <summary>
        /// 获取指定角色拥有的菜单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<MenuEntity> GetHaveRightMenuList(RoleMenuQuery query)
        {
            string sql = "SELECT DISTINCT m.MenuID,m.MenuName,m.MenuUrl,m.PMenuID,m.IconUrl,m.SortID,m.IsContentMenu FROM dbo.RoleMenu r WITH(NOLOCK),dbo.Menu m WITH(NOLOCK) WHERE m.JournalID=@JournalID AND m.GroupID=@GroupID AND m.JournalID=r.JournalID AND r.RoleID {0} AND r.MenuID=m.MenuID AND m.Status=1 ORDER BY m.SortID ASC,m.MenuID ASC";
            StringBuilder sqlCommandText = new StringBuilder();
            if (query.GroupID == (Byte)EnumMemberGroup.Editor)
            {
                if (query.RoleID != null)
                {
                    sqlCommandText.AppendFormat(sql, "= " + query.RoleID);
                }
                else if (query.RoleIDList != null && query.RoleIDList.Count > 0)
                {
                    if (query.RoleIDList.Count == 1)
                    {
                        sqlCommandText.AppendFormat(sql, "= " + query.RoleIDList[0]);
                    }
                    else
                    {
                        sqlCommandText.AppendFormat(sql, " IN (" + string.Join(",", query.RoleIDList) + ")");
                    }
                }
                else
                {
                    sqlCommandText.AppendFormat(sql, " = 0");
                }
            }
            else
            {
                sqlCommandText = new StringBuilder("SELECT m.MenuID,m.MenuName,m.MenuUrl,m.PMenuID,m.IconUrl,m.SortID,m.IsContentMenu FROM dbo.Menu m WITH(NOLOCK) WHERE m.JournalID=@JournalID AND m.GroupID=@GroupID AND m.Status=1 ORDER BY m.SortID ASC,m.MenuID ASC");
            }
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,query.JournalID);
            db.AddInParameter(cmd, "@GroupID", DbType.Byte, query.GroupID);

            IList<MenuEntity> menuList = new List<MenuEntity>();
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                MenuEntity entity = null;
                while (dr.Read())
                {
                    entity = new MenuEntity();
                    entity.MenuID = WKT.Common.Utils.TypeParse.ToLong(dr["MenuID"], 0);
                    entity.PMenuID = WKT.Common.Utils.TypeParse.ToInt(dr["PMenuID"], 0);
                    entity.MenuName = dr["MenuName"].ToString();
                    entity.MenuUrl = dr["MenuUrl"].ToString();
                    entity.IconUrl = dr["IconUrl"].ToString();
                    entity.IsContentMenu = WKT.Common.Utils.TypeParse.ToBool(dr["IsContentMenu"], false);
                    menuList.Add(entity);
                }
                dr.Close();
            }
            return menuList;
        }

        /// <summary>
        /// 是否有权限访问当前地址
        /// 如果该地址出现在menu表中了，则进行判断，如果没有则不判断
        /// </summary>
        /// <param name="RoleIDList"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsHaveAccessRightByGroup(RoleMenuQuery query)
        {
            bool flag = false;
            string sql = @" SELECT TOP 1 m.MenuID,m.GroupID FROM dbo.Menu m WITH(NOLOCK) WHERE m.JournalID=@JournalID AND m.MenuUrl=@MenuUrl";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@MenuUrl", DbType.String, query.Url);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            IDataReader dr = db.ExecuteReader(cmd);
            if (dr.Read())
            {
                if (WKT.Common.Utils.TypeParse.ToInt16(dr["GroupID"], 0) == query.GroupID.Value)
                {
                    flag = true;
                }
                dr.Close();
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 是否有权限访问当前地址
        /// 如果该地址出现在menu表中了，则进行判断，如果没有则不判断
        /// </summary>
        /// <param name="RoleIDList"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsHaveAccessRight(RoleMenuQuery query)
        {
            bool flag = false;
            string sql = @" DECLARE @MenuID BIGINT,@IsHaveRight TINYINT
                            SET @IsHaveRight = 0
                            SELECT TOP 1 @MenuID=m.MenuID FROM dbo.Menu m WITH(NOLOCK) WHERE m.JournalID=@JournalID AND m.MenuUrl=@MenuUrl
                            IF @MenuID IS NULL
                            BEGIN
	                            SET @IsHaveRight = 1
                            END
                            BEGIN
                                SET @MenuID = NULL
	                            SELECT TOP 1 @MenuID=m.MenuID FROM dbo.RoleMenu r WITH(NOLOCK),dbo.Menu m WITH(NOLOCK) WHERE r.JournalID=@JournalID AND r.MenuID=m.MenuID AND r.RoleID {0} AND m.MenuUrl=@MenuUrl
	                            IF @MenuID IS NOT NULL
	                            BEGIN
		                            SET @IsHaveRight=1
	                            END
                            END
                            SELECT @IsHaveRight AS IsHaveRight";

            StringBuilder sqlCommandText = new StringBuilder();
            if (query.RoleID != null)
            {
                sqlCommandText.AppendFormat(sql, "= " + query.RoleID);
            }
            else if (query.RoleIDList != null && query.RoleIDList.Count > 0)
            {
                if (query.RoleIDList.Count == 1)
                {
                    sqlCommandText.AppendFormat(sql, "= " + query.RoleIDList[0]);
                }
                else
                {
                    sqlCommandText.AppendFormat(sql, " IN (" + string.Join(",", query.RoleIDList) + ")");
                }
            }
            else
            {
                sqlCommandText.AppendFormat(sql, " = 0");
            }
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@MenuUrl", DbType.String, query.Url);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            object returnVal = db.ExecuteScalar(cmd);
            if (returnVal != null)
            {
                if (WKT.Common.Utils.TypeParse.ToInt16(returnVal,0) == 1)
                {
                    flag = true; 
                }
            }
            return flag;
        }

        /// <summary>
        /// 获取指定用户例外权限的菜单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long, string> GetAuthorExceptionMenuDict(AuthorMenuRightExceptionEntity authorExceptionMenuEntity)
        {
            IDictionary<long, string> dictAuthorExceptionMap = new Dictionary<long, string>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT a.MenuID,m.MenuUrl FROM dbo.AuthorMenuRightException a WITH(NOLOCK),dbo.Menu m WITH(NOLOCK) WHERE a.MenuID = m.MenuID AND a.AuthorID=@AuthorID AND a.JournalID=@JournalID");
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, authorExceptionMenuEntity.JournalID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, authorExceptionMenuEntity.AuthorID);
            long MenuID = 0;
            int MenuIDIndex = 0;
            int MenuUrlIndex = 0;
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                MenuIDIndex = dr.GetOrdinal("MenuID");
                MenuUrlIndex = dr.GetOrdinal("MenuID");
                while (dr.Read())
                {
                    MenuID = WKT.Common.Utils.TypeParse.ToLong(dr[MenuIDIndex], 0);
                    if (!dictAuthorExceptionMap.ContainsKey(MenuID))
                    {
                        dictAuthorExceptionMap.Add(MenuID, dr[MenuUrlIndex].ToString());
                    }
                }
                dr.Close();
            }
            return dictAuthorExceptionMap;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<RoleMenuEntity> GetRoleMenuList(RoleMenuQuery query)
        {
            List<RoleMenuEntity> list = new List<RoleMenuEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT MapID,JournalID,RoleID,MenuID,AddDate FROM dbo.RoleMenu WITH(NOLOCK)");
            string whereSQL = RoleMenuQueryToSQLWhere(query);
            string orderBy = RoleMenuQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeRoleMenuList(dr);
            }
            return list;
        }

        /// <summary>
        /// 构造列表
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public List<RoleMenuEntity> MakeRoleMenuList(IDataReader dr)
        {
            List<RoleMenuEntity> list = new List<RoleMenuEntity>();
            while (dr.Read())
            {
                RoleMenuEntity roleMenuEntity = new RoleMenuEntity();
                roleMenuEntity.MapID = (Int64)dr["MapID"];
                roleMenuEntity.JournalID = (Int64)dr["JournalID"];
                roleMenuEntity.RoleID = Convert.IsDBNull(dr["RoleID"]) ? null : (Int64?)dr["RoleID"];
                roleMenuEntity.MenuID = Convert.IsDBNull(dr["MenuID"]) ? null : (Int64?)dr["MenuID"];
                roleMenuEntity.AddDate = (DateTime)dr["AddDate"];
                list.Add(roleMenuEntity);
            }
            dr.Close();
            return list;
        }

        # endregion

        # region 保存角色授权菜单

        /// <summary>
        /// 保存角色拥有权限的菜单映射关系
        /// </summary>
        /// <param name="menuMapList">菜单权限列表</param>
        /// <returns></returns>
        public bool SaveRoleMenuRight(List<RoleMenuEntity> menuMapList)
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
                        var bulkCopy = new SqlBulkCopy(connection as SqlConnection, SqlBulkCopyOptions.Default, transaction as SqlTransaction);
                        try
                        {
                            # region 先删除

                            string sqlDel = "DELETE dbo.RoleMenu WHERE RoleID=" + menuMapList[0].RoleID;
                            DbCommand delCmd = db.GetSqlStringCommand(sqlDel);
                            // 先批量删除
                            db.ExecuteNonQuery(transaction as DbTransaction,CommandType.Text, sqlDel);

                            # endregion

                            # region 再批量插入

                            // 开始批量插入
                            DataTable dtMenuAuth = ConstructionMenuRoleDT();
                            foreach (RoleMenuEntity mapItem in menuMapList)
                            {
                                DataRow row = dtMenuAuth.NewRow();
                                row["JournalID"] = mapItem.JournalID;
                                row["RoleID"] = mapItem.RoleID;
                                row["MenuID"] = mapItem.MenuID;
                                row["AddDate"] = DateTime.Now;
                                dtMenuAuth.Rows.Add(row);
                            }
                            bulkCopy.DestinationTableName = "dbo.RoleMenu";
                            bulkCopy.WriteToServer(dtMenuAuth);

                            # endregion

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
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
        private DataTable ConstructionMenuRoleDT()
        {
            DataTable menuAuthTable = new DataTable("RoleMenu");
            DataColumn colMenuAuthID = menuAuthTable.Columns.Add("MapID", Type.GetType("System.Int64"));
            colMenuAuthID.AutoIncrement = true;
            colMenuAuthID.AutoIncrementSeed = 1;
            menuAuthTable.Columns.Add("JournalID", Type.GetType("System.Int64"));
            menuAuthTable.Columns.Add("RoleID", Type.GetType("System.Int64"));
            menuAuthTable.Columns.Add("MenuID", Type.GetType("System.Int64"));
            menuAuthTable.Columns.Add("AddDate", Type.GetType("System.DateTime"));
            return menuAuthTable;
        }

        /// <summary>
        ///  构造批量插入的Table
        /// </summary>
        /// <returns></returns>
        private DataTable ConstructionMenuIDDT()
        {
            DataTable menuAuthTable = new DataTable("RoleMenu");
            menuAuthTable.Columns.Add("MenuID", Type.GetType("System.Int64"));
            return menuAuthTable;
        }

        # endregion

        # region 给指定角色内的用户设置例外菜单权限

        /// <summary>
        /// 给指定角色内的用户设置例外菜单权限
        /// </summary>
        /// <param name="authorExceptionRight"></param>
        /// <returns></returns>
        public bool SetAuthorExceptionMenuRight(AuthorMenuRightExceptionEntity authorExceptionRight)
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
                            # region 1.创建临时表

                            string createTempTable = "CREATE TABLE #AllHaveRight(MenuID BIGINT)";
                            DbCommand cmdCreateTempTable = db.GetSqlStringCommand(createTempTable);
                            db.ExecuteNonQuery(cmdCreateTempTable, (DbTransaction)transaction);

                            # endregion

                            # region 2.往临时表中写入数据

                            // 开始批量插入
                            DataTable dtMenuID = ConstructionMenuIDDT();
                            foreach (long MenuID in authorExceptionRight.MenuIDList)
                            {
                                DataRow row = dtMenuID.NewRow();
                                row["MenuID"] = MenuID;
                                dtMenuID.Rows.Add(row);
                            }

                            SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction);
                            bulkCopy.DestinationTableName = "#AllHaveRight";
                            bulkCopy.WriteToServer(dtMenuID);


                            # endregion

                            # region 3.设置例外菜单权限，并删除临时表

                            string sql = @" 
                            DELETE FROM dbo.AuthorMenuRightException WHERE JournalID=@JournalID AND AuthorID=@AuthorID;
                            INSERT INTO AuthorMenuRightException(AuthorID,JournalID,MenuID)
                            SELECT @AuthorID as AuthorID,@JournalID as JournalID,r.MenuID FROM dbo.RoleMenu r WITH(NOLOCK) 
                            WHERE r.RoleID=@RoleID AND NOT EXISTS(
	                            SELECT MenuID FROM #AllHaveRight WITH(NOLOCK) WHERE MenuID=r.MenuID
                            );
                            DROP TABLE #AllHaveRight;";
                            DbCommand cmdSet = db.GetSqlStringCommand(sql);
                            db.AddInParameter(cmdSet, "@AuthorID", DbType.Int64, authorExceptionRight.AuthorID);
                            db.AddInParameter(cmdSet, "@JournalID", DbType.Int64, authorExceptionRight.JournalID);
                            db.AddInParameter(cmdSet, "@RoleID", DbType.Int64, authorExceptionRight.RoleID);
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

        # endregion
    }
}

