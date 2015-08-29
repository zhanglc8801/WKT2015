using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using WKT.Model;

namespace WKT.Data.SQL
{
    public partial class SQLDbProvider : IDbProvider
    {
        /// <summary>
        /// 表值参数
        /// 待扩展多个参数
        /// by sxd
        /// </summary>
        /// <param name="trans">事务 可为null</param>
        /// <param name="StroreName">存储过程名称</param>
        /// <param name="tableParameterName">存储过程参数名称</param>
        /// <param name="dtValue">表值</param>
        /// <returns></returns>
        public bool BatchByTableValueParameter(DbTransaction trans,string StroreName,string tableParameterName,DataTable dtValue)
        {
            if (string.IsNullOrWhiteSpace(StroreName))
                throw new ArgumentException("参数StroreName：不能为空！");
            if (string.IsNullOrWhiteSpace(tableParameterName))
                throw new ArgumentException("参数tableParameterName：不能为空！");
            if (dtValue == null)
                throw new ArgumentException("参数dtValue：不能为空！");
            bool flag = false;
            if (dtValue.Rows.Count > 0)
            {
                try
                {
                    using (var cmd = db.GetStoredProcCommand(StroreName))
                    {
                        var database = (SqlDatabase)db;
                        database.AddInParameter(cmd, "@" + tableParameterName, SqlDbType.Structured, dtValue);
                        if (trans != null)
                        {
                            db.ExecuteNonQuery(cmd, trans);
                        }
                        else
                        {
                            db.ExecuteNonQuery(cmd);
                        }
                    }
                    flag = true;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 保存大量数据
        /// </summary>
        /// <param name="dtInsert">新增数据源</param>
        /// <param name="dtUpdate">编辑数据源</param>
        /// <param name="tableName">数据库表名</param>
        /// <param name="dicField">新增时 源数据列与目标表数据列的对应</param>
        /// <param name="updateCommand">编辑时 编辑语句</param>
        public void SaveLargeData(DataTable dtInsert, DataTable dtUpdate, string tableName, IDictionary<string, string> dicField, DbCommand updateCommand)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("参数tableName：不能为空！");
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    if (dtInsert != null && dtInsert.Rows.Count > 0)
                        SaveLargeData(conn, trans, dtInsert, tableName, dicField);
                    if (dtUpdate != null && dtUpdate.Rows.Count > 0)
                        SaveLargeData(trans, dtUpdate, tableName, updateCommand);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 保存大量数据
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事物</param>
        /// <param name="dtInsert">新增数据源</param>
        /// <param name="tableName">数据库表名</param>
        /// <param name="dicField">源数据列与目标表数据列的对应</param>
        public void SaveLargeData(DbConnection conn, DbTransaction trans, DataTable dtInsert, string tableName, IDictionary<string, string> dicField)
        {
            if (dtInsert == null || dtInsert.Rows.Count == 0)
                return;
            if (conn == null)
                throw new ArgumentException("参数conn：不能为空！");
            if (trans == null)
                throw new ArgumentException("参数trans：不能为空！");
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("参数tableName：不能为空！");
            try
            {
                using (SqlBulkCopy sqlbulkCopy = new SqlBulkCopy(conn as SqlConnection, SqlBulkCopyOptions.CheckConstraints, trans as SqlTransaction))
                {
                    sqlbulkCopy.DestinationTableName = tableName;
                    if (dicField != null)
                    {
                        foreach (var d in dicField)
                        {
                            sqlbulkCopy.ColumnMappings.Add(d.Key, d.Value);
                        }
                    }
                    sqlbulkCopy.BatchSize = 500;//每次处理的行数
                    sqlbulkCopy.WriteToServer(dtInsert);
                    sqlbulkCopy.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存大量数据
        /// </summary>
        /// <param name="trans">事物</param>
        /// <param name="dtUpdate">编辑数据源</param>
        /// <param name="tableName">数据库表名</param>
        /// <param name="updateCommand">编辑语句</param>
        public void SaveLargeData(DbTransaction trans, DataTable dtUpdate, string tableName, DbCommand updateCommand)
        {
            if (dtUpdate == null || dtUpdate.Rows.Count == 0)
                return;
            if (trans == null)
                throw new ArgumentException("参数trans：不能为空！");
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("参数tableName：不能为空！");
            if (updateCommand == null)
                throw new ArgumentException("参数updateCommand：不能为空！");
            try
            {
                DataSet ds = new DataSet();
                dtUpdate.TableName = tableName;
                dtUpdate.AcceptChanges();
                for (int i = 0; i < dtUpdate.Rows.Count; i++)
                    dtUpdate.Rows[i].SetModified();
                ds.Tables.Add(dtUpdate);
                db.UpdateDataSet(ds, tableName, null, updateCommand, null, trans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 单表大数据量分页
        /// by sxd
        /// </summary>
        /// <param name="TableName">表名：UserInfo</param>
        /// <param name="GetFields">获取的字段：UserID,UserName,Gender</param>
        /// <param name="PrimaryKey">主健：UserID</param>
        /// <param name="OrderBy">排序方式：UserName ASC</param>
        /// <param name="Where">查询条件：Gender=1</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageNumber">页码索引</param>
        /// <param name="TotalCount">返回总记录数</param>
        /// <returns></returns>
        public DataSet LargeDataPagerSingleTalbe(string TableName, string GetFields, string PrimaryKey, string OrderBy, string Where, int PageSize, int PageNumber, out int TotalCount)
        {
            DataSet dsResult = null;

            # region 构造查询SQL

            StringBuilder sbSQL = new StringBuilder("");
            sbSQL.Append("DECLARE @PageNumber INT; ");
            sbSQL.Append("DECLARE @PageSize INT; ");

            // 判断是否是主健排序
            if (OrderBy.TrimStart().StartsWith(PrimaryKey))
            {
                if (string.IsNullOrEmpty(Where))
                {
                    // 得到最少的主健
                    sbSQL.AppendFormat("WITH Keys AS(SELECT TOP(@PageNumber * @PageSize) ROW_NUMBER() OVER(ORDER BY {1}) AS rn,t.{0} FROM {2} t WITH(NOLOCK) ORDER BY {1}), ", PrimaryKey, OrderBy, TableName);
                }
                else
                {
                    // 得到最少的主健
                    sbSQL.AppendFormat("WITH Keys AS(SELECT TOP(@PageNumber * @PageSize) ROW_NUMBER() OVER(ORDER BY {1}) AS rn,t.{0} FROM {2} t WITH(NOLOCK) WHERE {3} ORDER BY {1}), ", PrimaryKey, OrderBy, TableName, Where);
                }
            }
            else
            {
                // 如果不是主健排序，则也要把排序字段的值取出来
                string patternASC = "ASC";
                string patternDESC = "DESC";
                string OrderByFiled = Regex.Replace(OrderBy, patternASC, "", RegexOptions.IgnoreCase);
                OrderByFiled = Regex.Replace(OrderByFiled, patternDESC, "", RegexOptions.IgnoreCase);
                OrderByFiled = OrderByFiled.Trim();
                if (string.IsNullOrEmpty(Where))
                {
                    // 得到最少的主健
                    sbSQL.AppendFormat("WITH Keys AS(SELECT TOP(@PageNumber * @PageSize) ROW_NUMBER() OVER(ORDER BY {1}) AS rn,t.{0},t.{3} FROM {2} t WITH(NOLOCK) ORDER BY {1}), ", PrimaryKey, OrderBy, TableName, OrderByFiled);
                }
                else
                {
                    // 得到最少的主健
                    sbSQL.AppendFormat("WITH Keys AS(SELECT TOP(@PageNumber * @PageSize) ROW_NUMBER() OVER(ORDER BY {1}) AS rn,t.{0},t.{3} FROM {2} t WITH(NOLOCK) WHERE {4} ORDER BY {1}), ", PrimaryKey, OrderBy, TableName, OrderByFiled, Where);
                }
            }
            // 得到当前页的主健
            sbSQL.AppendFormat("SelectedKeys AS(SELECT TOP(@PageSize) SK.rn,SK.{0} FROM Keys SK WHERE SK.rn > ((@PageNumber - 1) * @PageSize) ORDER BY SK.{1}) ", PrimaryKey, OrderBy);
            // 得到数据
            sbSQL.AppendFormat("SELECT SK.rn,{0} FROM SelectedKeys SK JOIN {2} t on t.{1} = SK.{1};", GetFields, PrimaryKey, TableName);

            // 记录条数ＳＱＬ
            StringBuilder sbCount = new StringBuilder("");
            if (string.IsNullOrEmpty(Where))
            {
                sbCount.AppendFormat("SELECT COUNT({0}) FROM {1} WITH(NOLOCK);", PrimaryKey, TableName);
            }
            else
            {
                sbCount.AppendFormat("SELECT COUNT({0}) FROM {1} WITH(NOLOCK) WHERE {2};", PrimaryKey, TableName, Where);
            }

            # endregion

            DbCommand dbcommand = db.GetSqlStringCommand(sbSQL.ToString() + " " + sbCount.ToString());
            db.AddInParameter(dbcommand, "@PageNumber", DbType.Int32, PageNumber);
            db.AddInParameter(dbcommand, "@PageSize", DbType.Int32, PageSize);
            dsResult = db.ExecuteDataSet(dbcommand);
            if (dsResult != null)
            {
                Int32.TryParse(dsResult.Tables[1].Rows[0][0].ToString(), out TotalCount);
            }
            else
            {
                TotalCount = 0;
            }
            return dsResult;
        }

        /// <summary>
        /// 分页大数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">主键(多个逗号隔开 带前缀)</param>       
        /// <param name="orderBy">排序字符串(带前缀)</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageCurrent">当前页</param>
        /// <param name="fieldStr">查询字段(带前缀)</param>
        /// <param name="tableStr">查询表字符串(带联立条件 如：dbo.InventoryTransaction a with(nolock) inner join YinTaiContent.dbo.ItemProfileCommonInfo b with(nolock) on a.ItemCode=b.ItemCode)</param>
        /// <param name="filter">搜索条件(带前缀)</param>       
        /// <param name="sumStr">求和字符串(带前缀 如：Sum(a.Money) as Money,Count(1) as Count)</param>
        /// <param name="MakeCount">设置合计委托</param>      
        /// <param name="MakeSource">获取数据委托</param> 
        /// <param name="pager">返回Pager(可空)</param>
        /// <returns></returns>
        public Pager<T> LargeDataPagerSingleTalbe<T>(string primaryKey, string orderBy, int pageSize, int pageCurrent, string fieldStr, string tableStr, string filter,
            string sumStr, Action<IDataReader, Pager<T>> MakeCount, Func<IDataReader, List<T>> MakeSource, Pager<T> pager = null)
        {
            if (pager == null)
                pager = new Pager<T>();

            #region 构造查询sql
            StringBuilder strSql = new StringBuilder();
            strSql.Append("WITH Keys AS(");
            strSql.AppendFormat("SELECT TOP {1} ROW_NUMBER() OVER(ORDER BY {0}) AS Row_ID,{2}", orderBy, pageSize * pageCurrent, primaryKey);                
            strSql.AppendFormat(" FROM {0} ", tableStr);
            #region where条件
            string strFilter = string.Empty;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                strFilter = " WHERE ";
                bool flagWhere = filter.TrimStart().StartsWith("AND", true, null);//搜索条件前是否带and
                if (flagWhere)
                    strFilter += " 1=1 ";
                strFilter += filter;
            }
            strSql.Append(strFilter);
            #endregion
            strSql.AppendFormat(" ORDER BY {0}) ", orderBy);
            // 得到当前页的主健
            strSql.Append(",SelectedKeys AS(");
            strSql.AppendFormat("SELECT TOP {0} * FROM Keys WHERE Row_ID > {1} ORDER BY Row_ID) ", pageSize, pageSize * (pageCurrent - 1));
            //得到数据
            strSql.AppendFormat("\r\nSELECT {0} FROM {1} INNER JOIN SelectedKeys sk ON ", fieldStr, tableStr);
            string[] keys = primaryKey.Split(',');//主键
            Regex regex = new Regex(@"\S+\.");
            for (int i = 0; i < keys.Length; i++)
            {
                if (i > 0)
                    strSql.Append(" AND ");
                strSql.AppendFormat(" {0}=sk.{1} ", keys[i], regex.Replace(keys[i], string.Empty));
            }
            strSql.Append(strFilter);
            strSql.Append(" ORDER BY sk.Row_ID");
            //得到总数
            if (!string.IsNullOrWhiteSpace(sumStr))
                strSql.AppendFormat("\r\nSELECT {0} FROM {1} {2}", sumStr, tableStr, strFilter.ToString());
            #endregion

            DbCommand cmd = db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr != null)
                {
                    pager.ItemList = MakeSource(dr);
                    if (dr.NextResult() && dr.Read())
                    {
                        MakeCount(dr, pager);
                    }
                    dr.Close();
                }
            }
            pager.CurrentPage = pageCurrent;
            pager.PageSize = pageSize;
            return pager;
        }

        /// <summary>
        /// 分页大数据表(带group by)
        /// </summary>
        /// <typeparam name="T"></typeparam>      
        /// <param name="orderBy">排序字符串(带前缀)</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageCurrent">当前页</param>
        /// <param name="fieldStr">查询字段(带前缀)</param>
        /// <param name="sumStr">求和字符串(带前缀 如：Sum(a.Money) as Money,Count(1) as Count)</param>
        /// <param name="tableStr">查询表字符串(带联立条件 如：dbo.InventoryTransaction a with(nolock) inner join YinTaiContent.dbo.ItemProfileCommonInfo b with(nolock) on a.ItemCode=b.ItemCode)</param>
        /// <param name="filter">搜索条件(带前缀)</param>
        /// <param name="groupBy">分组条件(带前缀)</param>
        /// <param name="MakeSource">获取数据委托</param>
        /// <param name="MakeCount">设置合计委托</param>
        /// <param name="pager">返回Pager(可空)</param>
        /// <returns></returns>
        public Pager<T> LargeDataPagerSingleTalbe<T>(string orderBy, int pageSize, int pageCurrent, string fieldStr,string sumStr, string tableStr, string filter,string groupBy,  
            Func<IDataReader, List<T>> MakeSource, Action<IDataReader, Pager<T>> MakeCount, Pager<T> pager = null)
        {
            if (string.IsNullOrWhiteSpace(groupBy))
                throw new Exception("参数groupBy不能为空！");
            if (pager == null)
                pager = new Pager<T>();

            #region 构造查询sql
            StringBuilder strSql = new StringBuilder();
            strSql.Append("WITH Keys AS(");
            strSql.AppendFormat("SELECT TOP {1} ROW_NUMBER() OVER(ORDER BY {0}) AS Row_ID,{2}", orderBy, pageSize * pageCurrent,groupBy);            
            strSql.AppendFormat(" FROM {0} ", tableStr);
            #region where条件
            string strFilter = string.Empty;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                strFilter = " WHERE ";
                bool flagWhere = filter.TrimStart().StartsWith("AND", true, null);//搜索条件前是否带and
                if (flagWhere)
                    strFilter += " 1=1 ";
                strFilter += filter;
            }
            strSql.Append(strFilter);
            #endregion
            strSql.AppendFormat(" GROUP BY {0}", groupBy);
            strSql.AppendFormat(" ORDER BY {0}) ", orderBy);
            // 得到当前页的主健
            strSql.Append(",SelectedKeys AS(");
            strSql.AppendFormat("SELECT TOP {1} * FROM Keys WHERE Row_ID > {1} ORDER BY Row_ID) ", pageSize, pageSize * (pageCurrent - 1));
            //得到数据
            strSql.AppendFormat("\r\nSELECT {0} FROM {1} INNER JOIN SelectedKeys sk ON ", fieldStr, tableStr);
            string[] keys = groupBy.Split(',');//分组字段
            Regex regex = new Regex(@"\S+\.");
            for (int i = 0; i < keys.Length; i++)
            {
                if (i > 0)
                    strSql.Append(" AND ");
                strSql.AppendFormat(" {0}=sk.{1} ", keys[i], regex.Replace(keys[i], string.Empty));
            }
            strSql.Append(strFilter);
            strSql.AppendFormat(" GROUP BY {0}", groupBy);
            strSql.Append(" ORDER BY sk.Row_ID");
            //得到总数
            strSql.AppendFormat("\r\nSELECT {0} FROM {1} {2} GROUP BY {3}", sumStr, tableStr, strFilter.ToString(), groupBy);
            #endregion

            DbCommand cmd = db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr != null)
                {
                    pager.ItemList = MakeSource(dr);
                    if (dr.NextResult() && dr.Read())
                    {
                        MakeCount(dr, pager);
                    }
                    dr.Close();
                }
            }
            pager.CurrentPage = pageCurrent;
            pager.PageSize = pageSize;
            return pager;
        }

        /// <summary>
        /// 文海峰 2013-10-24
        /// 将繁星集合转换成Table  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i_objlist"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable<T>(IList<T> i_objlist)
        {
            if (i_objlist == null || i_objlist.Count <= 0)
            {
                return null;
            }
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;
            System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (T t in i_objlist)
            {
                if (t == null)
                {
                    continue;
                }
                row = dt.NewRow();
                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                    string name = pi.Name;

                    if (dt.Columns[name] == null)
                    {
                        column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }

                    row[name] = pi.GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }  
    }
}
