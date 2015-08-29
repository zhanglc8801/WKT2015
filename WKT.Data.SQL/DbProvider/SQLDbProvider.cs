using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data;

using WKT.Common;
using WKT.Common.Extension;
using WKT.Common.Utils;
using WKT.Model;

namespace WKT.Data.SQL
{
    [Serializable]
    public partial class SQLDbProvider : IDbProvider
    {

        private bool isDisposed = false;
        private Database db;
        private DbConnection conn;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataBaseName">DatabaseName</param>
        public SQLDbProvider()
        {
            db = DatabaseFactory.CreateDatabase("WKTDataBase");
            conn = null;

        }

        public SQLDbProvider(string dataname)
        {
            db = DatabaseFactory.CreateDatabase(dataname);
            conn = null;
        }

        # region 

        /// <summary>
        /// add by sxd
        /// </summary>
        /// <returns></returns>
        public DbConnection  GetConnection()
        {
            return db.CreateConnection();
        }

        # endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (this.isDisposed)
                return;
            if (this.conn != null && this.conn.State != ConnectionState.Closed)
            {
                this.conn.Close();
            }
            GC.SuppressFinalize(this);
            this.isDisposed = true;
        }

        #endregion

        #region IDbProvider Members

        public System.Data.Common.DbProviderFactory DbProviderFactory
        {
            get
            {
                return db.DbProviderFactory;
            }
        }

        public bool SupportsAsync
        {
            get
            {
                return db.SupportsAsync;
            }
        }

        public bool SupportsParemeterDiscovery
        {
            get
            {
                return db.SupportsParemeterDiscovery;
            }
        }

        public void AddInParameter(DbCommand command, string name, DbType dbType)
        {
            db.AddInParameter(command, name, dbType);
        }

        public void AddInParameter(DbCommand command, string name, DbType dbType, object value)
        {
            db.AddInParameter(command, name, dbType, value);
        }

        public void AddInParameter(DbCommand command, string name, DbType dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            db.AddInParameter(command, name, dbType, sourceColumn, sourceVersion);
        }

        public void AddOutParameter(DbCommand command, string name, DbType dbType, int size)
        {
            db.AddOutParameter(command, name, dbType, size);
        }

        public void AddParameter(DbCommand command, string name, DbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            db.AddParameter(command, name, dbType, direction, sourceColumn, sourceVersion, value);
        }

        public void AddParameter(DbCommand command, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            db.AddParameter(command, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
        }

        public void AssignParameters(DbCommand command, object[] parameterValues)
        {
            db.AssignParameters(command, parameterValues);
        }

        public IAsyncResult BeginExecuteNonQuery(DbCommand command, AsyncCallback callback, object state)
        {
            return db.BeginExecuteNonQuery(command, callback, state);
        }

        public IAsyncResult BeginExecuteNonQuery(CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            return db.BeginExecuteNonQuery(commandType, commandText, callback, state);
        }

        public IAsyncResult BeginExecuteNonQuery(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state)
        {
            return db.BeginExecuteNonQuery(command, transaction, callback, state);
        }

        public IAsyncResult BeginExecuteNonQuery(string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            return db.BeginExecuteNonQuery(storedProcedureName, callback, state, parameterValues);
        }

        public IAsyncResult BeginExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            return db.BeginExecuteNonQuery(transaction, commandType, commandText, callback, state);
        }

        public IAsyncResult BeginExecuteNonQuery(DbTransaction transaction, string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            return db.BeginExecuteNonQuery(transaction, storedProcedureName, callback, state, parameterValues);
        }

        public IAsyncResult BeginExecuteReader(DbCommand command, AsyncCallback callback, object state)
        {
            return db.BeginExecuteReader(command, callback, state);
        }

        public IAsyncResult BeginExecuteReader(CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            return db.BeginExecuteReader(commandType, commandText, callback, state);
        }

        public IAsyncResult BeginExecuteReader(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state)
        {
            return db.BeginExecuteReader(command, transaction, callback, state);
        }

        public IAsyncResult BeginExecuteReader(string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            return db.BeginExecuteReader(storedProcedureName, callback, state, parameterValues);
        }

        public IAsyncResult BeginExecuteReader(DbTransaction transaction, CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            return db.BeginExecuteReader(transaction, commandType, commandText, callback, state);
        }

        public IAsyncResult BeginExecuteReader(DbTransaction transaction, string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            return db.BeginExecuteReader(transaction, storedProcedureName, callback, state, parameterValues);
        }

        public IAsyncResult BeginExecuteScalar(DbCommand command, AsyncCallback callback, object state)
        {
            return db.BeginExecuteScalar(command, callback, state);
        }

        public IAsyncResult BeginExecuteScalar(CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            return db.BeginExecuteScalar(commandType, commandText, callback, state);
        }

        public IAsyncResult BeginExecuteScalar(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state)
        {
            return db.BeginExecuteScalar(command, transaction, callback, state);
        }

        public IAsyncResult BeginExecuteScalar(string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            return db.BeginExecuteScalar(storedProcedureName, callback, state, parameterValues);
        }

        public IAsyncResult BeginExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText, AsyncCallback callback, object state)
        {
            return db.BeginExecuteScalar(transaction, commandType, commandText, callback, state);
        }

        public IAsyncResult BeginExecuteScalar(DbTransaction transaction, string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues)
        {
            return db.BeginExecuteScalar(transaction, storedProcedureName, callback, state, parameterValues);
        }

        public string BuildParameterName(string name)
        {
            return db.BuildParameterName(name);
        }

        public DbConnection CreateConnection()
        {
            return db.CreateConnection();
        }

        public void DiscoverParameters(DbCommand command)
        {
            db.DiscoverParameters(command);
        }

        public int EndExecuteNonQuery(IAsyncResult asyncResult)
        {
            return db.EndExecuteNonQuery(asyncResult);
        }

        public IDataReader EndExecuteReader(IAsyncResult asyncResult)
        {
            return db.EndExecuteReader(asyncResult);
        }

        public object EndExecuteScalar(IAsyncResult asyncResult)
        {
            return db.EndExecuteScalar(asyncResult);
        }

        public DataSet ExecuteDataSet(DbCommand command)
        {
            return db.ExecuteDataSet(command);
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            return db.ExecuteDataSet(commandType, commandText);
        }

        public DataSet ExecuteDataSet(DbCommand command, DbTransaction transaction)
        {
            return db.ExecuteDataSet(command, transaction);
        }

        public DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues)
        {
            return db.ExecuteDataSet(storedProcedureName, parameterValues);
        }

        public DataSet ExecuteDataSet(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return db.ExecuteDataSet(transaction, commandType, commandText);
        }

        public DataSet ExecuteDataSet(DbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            return db.ExecuteDataSet(transaction, storedProcedureName, parameterValues);
        }

        public int ExecuteNonQuery(DbCommand command)
        {
            return db.ExecuteNonQuery(command);
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return db.ExecuteNonQuery(commandType, commandText);
        }

        public int ExecuteNonQuery(DbCommand command, DbTransaction transaction)
        {
            return db.ExecuteNonQuery(command, transaction);
        }

        public int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues)
        {
            return db.ExecuteNonQuery(storedProcedureName, parameterValues);
        }

        public int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return db.ExecuteNonQuery(transaction, commandType, commandText);
        }

        public int ExecuteNonQuery(DbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            return db.ExecuteNonQuery(transaction, storedProcedureName, parameterValues);
        }

        public IDataReader ExecuteReader(DbCommand command)
        {
            return db.ExecuteReader(command);
        }

        public IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            return db.ExecuteReader(commandType, commandText);
        }

        public IDataReader ExecuteReader(DbCommand command, DbTransaction transaction)
        {
            return db.ExecuteReader(command, transaction);
        }

        public IDataReader ExecuteReader(string storedProcedureName, params object[] parameterValues)
        {
            return db.ExecuteReader(storedProcedureName, parameterValues);
        }

        public IDataReader ExecuteReader(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return db.ExecuteReader(transaction, commandType, commandText);
        }

        public IDataReader ExecuteReader(DbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            return db.ExecuteReader(transaction, storedProcedureName, parameterValues);
        }

        public object ExecuteScalar(DbCommand command)
        {
            return db.ExecuteScalar(command);
        }

        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            return db.ExecuteScalar(commandType, commandText);
        }

        public object ExecuteScalar(DbCommand command, DbTransaction transaction)
        {
            return db.ExecuteScalar(command, transaction);
        }

        public object ExecuteScalar(string storedProcedureName, params object[] parameterValues)
        {
            return db.ExecuteScalar(storedProcedureName, parameterValues);
        }

        public object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return db.ExecuteScalar(transaction, commandType, commandText);
        }

        public object ExecuteScalar(DbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            return db.ExecuteScalar(transaction, storedProcedureName, parameterValues);
        }

        public DbDataAdapter GetDataAdapter()
        {
            return db.GetDataAdapter();
        }

        public object GetParameterValue(DbCommand command, string name)
        {
            return db.GetParameterValue(command, name);
        }

        public DbCommand GetSqlStringCommand(string query)
        {
            return db.GetSqlStringCommand(query);
        }

        public DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            return db.GetStoredProcCommand(storedProcedureName);
        }

        public DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues)
        {
            return db.GetStoredProcCommand(storedProcedureName, parameterValues);
        }

        public DbCommand GetStoredProcCommandWithSourceColumns(string storedProcedureName, params string[] sourceColumns)
        {
            return db.GetStoredProcCommandWithSourceColumns(storedProcedureName, sourceColumns);
        }

        public void LoadDataSet(DbCommand command, DataSet dataSet, string tableName)
        {
            db.LoadDataSet(command, dataSet, tableName);
        }

        public void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames)
        {
            db.LoadDataSet(command, dataSet, tableNames);
        }

        public void LoadDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            db.LoadDataSet(commandType, commandText, dataSet, tableNames);
        }

        public void LoadDataSet(DbCommand command, DataSet dataSet, string tableName, DbTransaction transaction)
        {
            db.LoadDataSet(command, dataSet, tableName, transaction);
        }

        public void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames, DbTransaction transaction)
        {
            db.LoadDataSet(command, dataSet, tableNames, transaction);
        }

        public void LoadDataSet(string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            db.LoadDataSet(storedProcedureName, dataSet, tableNames, parameterValues);
        }

        public void LoadDataSet(DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            db.LoadDataSet(transaction, commandType, commandText, dataSet, tableNames);
        }

        public void LoadDataSet(DbTransaction transaction, string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            db.LoadDataSet(transaction, storedProcedureName, dataSet, tableNames, parameterValues);
        }

        public void SetParameterValue(DbCommand command, string parameterName, object value)
        {
            db.SetParameterValue(command, parameterName, value);
        }

        public int UpdateDataSet(DataSet dataSet, string tableName, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, DbTransaction transaction)
        {
            return db.UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand, transaction);
        }

        public int UpdateDataSet(DataSet dataSet, string tableName, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, DbTransaction transaction, int? updateBatchSize)
        {
            return db.UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand, transaction, updateBatchSize);
        }

        #endregion

        /// <summary>
        /// 获取分页数据
        /// by tlq
        /// </summary>
        /// <param name="tblName">表名</param>
        /// <param name="strFldName">要显示的列(如:"id,name")</param>
        /// <param name="strOrder">排序(如：id desc)</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="strWhere">查询条件(注意:不要加where 如:"xname like ''%a%''") </param>        
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        public IDataReader GetPagingData(string tblName, string strFldName, string strOrder, int pageIndex, string strWhere, int pageSize)
        {

            string sql = string.Format("SELECT {0} FROM (SELECT ROW_NUMBER() OVER (order by  {1} ) AS ROWID,{2} FROM {3} WITH(NOLOCK)  where {4}) AS sp WHERE ROWID BETWEEN {5} AND {6}", strFldName, strOrder, strFldName, tblName, strWhere, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);

            DbCommand command = db.GetSqlStringCommand(sql);
            IDataReader dr = db.ExecuteReader(command);
            return dr;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// by tlq
        /// <param name="tblName">表名</param>
        /// <param name="strFldName">要显示的列(如:"id,name")</param>
        /// <param name="strOrder">排序(如：id desc)</param>
        /// <param name="strWhere">查询条件(注意:不要加where 如:"xname like ''%a%''") </param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="doCount">是否统计记录总数</param>
        /// <param name="recordCount">记录总数</param>
        /// <returns>Dataset 相应页面的数据</returns>
        public DataSet GetPagingData(string tblName, string strFldName, string strOrder, string strWhere, int pageIndex, int pageSize, bool doCount, out int recordCount)
        {
            recordCount = 0;
            string sql = string.Empty;
            if (doCount)
            {
                sql = string.Format("SELECT {0} FROM (SELECT ROW_NUMBER() OVER (order by  {1} ) AS ROWID,{2} FROM {3} WITH(NOLOCK)  {4}) AS sp WHERE ROWID BETWEEN {5} AND {6};select count(1) from {3}  WITH(NOLOCK)  {4} ", strFldName, strOrder, strFldName, tblName, string.IsNullOrWhiteSpace(strWhere) ? "" : " WHERE " + strWhere, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);

            }
            else
            {
                sql = string.Format("SELECT {0} FROM (SELECT ROW_NUMBER() OVER (order by  {1} ) AS ROWID,{2} FROM {3} WITH(NOLOCK)  {4}) AS sp WHERE ROWID BETWEEN {5} AND {6}", strFldName, strOrder, strFldName, tblName, string.IsNullOrWhiteSpace(strWhere) ? "" : "WHERE " + strWhere, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
            }

            DbCommand command = db.GetSqlStringCommand(sql);
            DataSet ds = db.ExecuteDataSet(command);
            if (ds.Tables.Count == 2)
            {
                recordCount = TypeParse.ToInt(ds.Tables[1].Rows[0][0]);
            }

            return ds;

            #region 分页存储过程 过期
            //DbCommand dbcommand = db.GetStoredProcCommand("[dbo].[UP_GetPageData]");
            //db.AddInParameter(dbcommand, "@tblName", DbType.String, tblName);
            //db.AddInParameter(dbcommand, "@strGetFields", DbType.String, strFldName);
            //db.AddInParameter(dbcommand, "@strWhere", DbType.String, strWhere);
            //db.AddInParameter(dbcommand, "@strOrder", DbType.String, strOrder);
            //db.AddInParameter(dbcommand, "@pageIndex", DbType.Int32, pageIndex);
            //db.AddInParameter(dbcommand, "@pageSize", DbType.Int32, pageSize);
            //db.AddInParameter(dbcommand, "@doCount", DbType.Byte, doCount ? 1 : 0);
            //db.AddOutParameter(dbcommand, "@recordCount", DbType.Int32, recordCount);

            //DataSet ds = db.ExecuteDataSet(dbcommand);

            //object obj = db.GetParameterValue(dbcommand, "@recordCount");
            //recordCount = obj != System.DBNull.Value ? Convert.ToInt32(db.GetParameterValue(dbcommand, "@recordCount")) : 0;
            #endregion

        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="tblName">表名</param>
        /// <param name="strFldName">字段名(如:"id,name")</param>
        /// <param name="strOrder">排序(如：id desc)</param>
        /// <param name="strWhere">查询条件(注意:不要加where 如:"xname like ''%a%''")</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetPagingData(string tblName, string strFldName, string strOrder, string strWhere, int pageIndex, int pageSize)
        {
            Int32 recordCount;

            return GetPagingData(tblName, strFldName, strOrder, strWhere, pageIndex, pageSize, false, out recordCount);
        }

        /// <summary>
        /// 获取分页数据不显示总记录数
        /// </summary>
        /// by tlq
        /// <param name="tblName">表名</param>
        /// <param name="strFldName">要显示的列(如:"id,name")。注意：不能为*号</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>Dataset 相应页面的数据</returns>
        public DataSet GetPagingData(string tblName, string strFldName, int pageIndex, int pageSize)
        {
            int recordCount = 0;
            string strOrder = strFldName.Substring(0, strFldName.IndexOf(',')) + " asc";
            DataSet ds = GetPagingData(tblName, strFldName, strOrder, "1=1", pageIndex, pageSize, false, out recordCount);
            return ds;
        }
        /// <summary>
        /// 获取分页数据显示总记录数
        /// </summary>
        /// by tlq
        /// <param name="tblName">表名</param>
        /// <param name="strFldName">要显示的列(如:"id,name")。注意：不能为*号</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns>Dataset 相应页面的数据</returns>
        public DataSet GetPagingData(string tblName, string strFldName, int pageIndex, int pageSize, out int recordCount)
        {
            recordCount = 0;
            string strOrder = strFldName.Substring(0, strFldName.IndexOf(',')) + " asc";
            DataSet ds = GetPagingData(tblName, strFldName, strOrder, "1=1", pageIndex, pageSize, true, out recordCount);

            return ds;
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="tblName">表名</param>
        /// <param name="strFldName">要显示的列(如:"id,name")</param>
        /// <param name="strOrder">排序列(如：id desc)。注意：不能为*号</param>
        /// <param name="strWhere">查询条件(注意:不要加where 如:"xname like ''%a%''") </param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="recordCount">记录总数</param>
        /// <returns>Dataset 相应页面的数据</returns>
        public DataSet GetPagingData(string tblName, string strFldName, string strOrder, string strWhere, int pageIndex, int pageSize, out int recordCount)
        {
            recordCount = 0;
            DataSet ds = GetPagingData(tblName, strFldName, strOrder, strWhere, pageIndex, pageSize, true, out recordCount);

            return ds;
        }
        /// <summary>
        /// 获取分页数据(默认按第一列升序)
        /// </summary>
        /// <param name="tblName">表名</param>
        /// <param name="strFldName">要显示的列(如:"id,name")注意：不能为*号</param>        
        /// <param name="strWhere">查询条件(注意:不要加where 如:"xname like ''%a%''") </param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>        
        /// <param name="recordCount">记录总数</param>        
        /// <returns>Dataset 相应页面的数据</returns>
        public DataSet GetPagingData(string tblName, string strFldName, string strWhere, int pageIndex, int pageSize, out int recordCount)
        {
            recordCount = 0;
            string strOrder = strFldName.Substring(0, strFldName.IndexOf(',')) + " asc";
            DataSet ds = GetPagingData(tblName, strFldName, strOrder, strWhere, pageIndex, pageSize, true, out recordCount);

            return ds;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <param name="orderByFields"></param>
        /// <param name="recourdCount"></param>
        /// <returns></returns>
        public DataSet PageingQuery(int pageNo, int pageSize, string sql, string orderByFields,SqlParameter[] parameters, ref int recourdCount)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new Exception("invalid sql statement!");
            }

            sql = sql.Trim();
            string sqlCount = "select count(1) as Total from ( " + sql + ") t";

            if (sql.ToLower().StartsWith("select"))
            {
                sql = sql.Substring("select".Length, sql.Length - "select".Length);
            }

            int startID = 0;
            int endID = 0;
            GetPageID(pageNo, pageSize, ref startID, ref endID);

            string orderby = string.Empty;

            #region 
            /*
             * 注：此处代码用于解决无法按稿件处理时间进行排序的问题以及无法按多个字段进行排序的问题
             * 如有问题请注释掉此处代码并将下面已注释掉的还原
             * 使用方法（c.CID DESC | t.HandTime DESC）：参考WKT.DataAccess\Flow\FlowCirculationDataAccess.cs下的方法GetFlowContributionList()
             * 2014-11-20 by zhanglc
             */
            string[] orderbys = orderByFields.Split('|');
            if (orderbys.Length > 1)
            {
                orderby = orderbys[1];
                orderByFields = orderbys[0];
            }
            else
            {
                orderby = orderbys[0].Split('.')[1];
            } 
            #endregion

            //if (orderby.IndexOf('.') > 0)
            //{
            //    orderby = orderby.Split('.')[1];
            //}
            

            string fullSql = sqlCount + "; select * from ( SELECT row_number() OVER (ORDER BY " + orderByFields + ") as rownum__identy,"
                       + sql + " ) t   where rownum__identy between @startID and @endID Order By  " + orderby;

            DbCommand dbcommand = db.GetSqlStringCommand(fullSql);
            if (parameters != null)
            {
                foreach(SqlParameter paramItem in parameters)
                {
                    db.AddInParameter(dbcommand, paramItem.ParameterName, paramItem.DbType, paramItem.Value);
                }
            }
            db.AddInParameter(dbcommand, "@startID", DbType.Int32, startID);
            db.AddInParameter(dbcommand, "@endID", DbType.Int32, endID);

            DataSet dsResult = db.ExecuteDataSet(dbcommand);
            recourdCount = Convert.ToInt32(dsResult.Tables[0].Rows[0][0]);
            dsResult.Tables.RemoveAt(0);
            dsResult.Tables[0].Columns.RemoveAt(0);
            return dsResult;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <param name="orderByFields"></param>
        /// <param name="recourdCount"></param>
        /// <returns></returns>
        public DataSet PageingQuery(int pageNo, int pageSize, string sql, string orderByFields, ref int recourdCount)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new Exception("invalid sql statement!");
            }

            return PageingQuery(pageNo, pageSize, sql, orderByFields,null, ref recourdCount);
        }

        /// <summary>
        /// 使用rowno进行分页的存储过程包含具体的业务逻辑
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="storeProcedure"></param>
        /// <param name="recourdCount"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet PageingQuery(int pageNo, int pageSize, string storeProcedure, ref int recourdCount, Dictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(storeProcedure))
            {
                throw new Exception("invalid stroe procedure statement!");
            }

            int startID = 0;
            int endID = 0;
            GetPageID(pageNo, pageSize, ref startID, ref endID);

            DbCommand dbcommand = db.GetStoredProcCommand(storeProcedure);
            db.AddInParameter(dbcommand, "@startID", DbType.Int32, startID);
            db.AddInParameter(dbcommand, "@endID", DbType.Int32, endID);
            if (parameters != null)
            {
                foreach (KeyValuePair<string, string> item in parameters)
                {
                    db.AddInParameter(dbcommand, "@" + item.Key, DbType.String, item.Value);
                }
            }

            DataSet dsResult = db.ExecuteDataSet(dbcommand);
            recourdCount = Convert.ToInt32(dsResult.Tables[0].Rows[0][0]);
            dsResult.Tables.RemoveAt(0);
            dsResult.Tables[0].Columns.RemoveAt(0);
            return dsResult;
        }

        /// <summary>
        /// 计算页面的开始和结束编号
        /// </summary>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="startID">返回的开始编号</param>
        /// <param name="endID">返回的结束编号</param>
        public void GetPageID(int pageNo, int pageSize, ref int startID, ref int endID)
        {
            startID = (pageNo - 1) * pageSize + 1;
            endID = startID + pageSize - 1;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="tblName">表名</param>
        /// <param name="strFldName">要显示的列(如:"id,name")</param>
        /// <param name="strOrder">排序(如：id desc)</param>
        /// <param name="strWhere">查询条件(注意:不要加where 如:"xname like ''%a%''") </param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="doCount">是否统计记录总数</param>
        /// <param name="MakeSource">返回List《T》对象的方法</param>
        /// <returns></returns>
        public Pager<T> GetPagingData<T>(string tblName, string strFldName, string strOrder, string strWhere, int pageIndex, int pageSize, bool doCount, Func<IDataReader, List<T>> MakeSource)
        {
            DbCommand cmd = db.GetStoredProcCommand("[dbo].[UP_GetPageData]");
            db.AddInParameter(cmd, "@tblName", DbType.String, tblName);
            db.AddInParameter(cmd, "@strGetFields", DbType.String, strFldName);
            db.AddInParameter(cmd, "@strWhere", DbType.String, strWhere);
            db.AddInParameter(cmd, "@strOrder", DbType.String, strOrder == "*" ? "1" : strOrder);
            db.AddInParameter(cmd, "@pageIndex", DbType.Int32, pageIndex);
            db.AddInParameter(cmd, "@pageSize", DbType.Int32, pageSize);
            db.AddInParameter(cmd, "@doCount", DbType.Byte, doCount ? 1 : 0);
            db.AddOutParameter(cmd, "@recordCount", DbType.Int32, 4);


            Pager<T> pager = new Pager<T>();
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr != null)
                {
                    pager.ItemList = MakeSource(dr);
                    dr.Close();
                    pager.TotalRecords = db.GetParameterValue(cmd, "@recordCount").TryParse<Int64>(0);
                }
            }
            pager.CurrentPage = pageIndex;
            pager.PageSize = pageSize;
            return pager;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">查询语句</param>
        /// <param name="makeSource">返回List《T》对象的方法</param>
        /// <returns></returns>
        public List<T> GetList<T>(string sql, Func<IDataReader, List<T>> makeSource)
        {
            List<T> list = new List<T>();
            DbCommand cmd = db.GetSqlStringCommand(sql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr != null)
                {
                    list = makeSource(dr);
                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql">查询数据语句</param>
        /// <param name="strTotalSql">查询合计语句</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="action">设置合计委托</param>
        /// <param name="makeSource">获取数据委托</param>
        /// <returns></returns>
        public Pager<T> GetPageList<T>(string strSql, string strTotalSql, int currentPage, int pageSize, Action<IDataReader, Pager<T>> action, Func<IDataReader, List<T>> makeSource)
        {
            Pager<T> pager = new Pager<T>();
            bool flag = true;
            if (!string.IsNullOrWhiteSpace(strTotalSql))
            {
                SetCount<T>(strTotalSql, pager, action);
                if (pager.TotalRecords == 0)
                    flag = false;
            }
            flag = flag && !string.IsNullOrWhiteSpace(strSql);
            if (flag)
            {
                DbCommand cmd = db.GetSqlStringCommand(strSql);
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    if (dr != null)
                    {
                        pager.ItemList = makeSource(dr);
                        if (!dr.IsClosed)
                        {
                            dr.Close();
                        }
                    }
                }
            }
            pager.CurrentPage = currentPage;
            pager.PageSize = pageSize;
            return pager;
        }

        /// <summary>
        /// 设置合计信息
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="Pager<T>"></param>
        /// <param name="action">设置合计委托</param>
        private void SetCount<T>(string strSql, Pager<T> pager, Action<IDataReader, Pager<T>> action)
        {
            if (string.IsNullOrWhiteSpace(strSql))
                return;
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr != null)
                {
                    if (dr.Read())
                    {
                        action(dr, pager);
                        dr.Close();
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public long RecordsCount(string tableName, string where)
        {
            long cnt = 0;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("select  count(1) from " + tableName + " with(nolock)");
            if (!string.IsNullOrWhiteSpace(where))
                sqlCommandText.Append(" where " + where);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                cnt = dr[0].TryParse<Int64>(0);
                dr.Close();
            }
            return cnt;
        }

    }
}
