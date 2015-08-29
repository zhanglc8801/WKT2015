using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

using WKT.Model;

namespace WKT.Data.SQL
{
    public interface IDbProvider : IDisposable
    {
        System.Data.Common.DbProviderFactory DbProviderFactory
        {
            get;
        }
        bool SupportsAsync
        {
            get;
        }
        bool SupportsParemeterDiscovery
        {
            get;
        }
        DbConnection GetConnection();
        void AddInParameter(DbCommand command, string name, DbType dbType);
        void AddInParameter(DbCommand command, string name, DbType dbType, object value);
        void AddInParameter(DbCommand command, string name, DbType dbType, string sourceColumn, DataRowVersion sourceVersion);
        void AddOutParameter(DbCommand command, string name, DbType dbType, int size);
        void AddParameter(DbCommand command, string name, DbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value);
        void AddParameter(DbCommand command, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value);
        void AssignParameters(DbCommand command, object[] parameterValues);
        IAsyncResult BeginExecuteNonQuery(DbCommand command, AsyncCallback callback, object state);
        IAsyncResult BeginExecuteNonQuery(CommandType commandType, string commandText, AsyncCallback callback, object state);
        IAsyncResult BeginExecuteNonQuery(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state);
        IAsyncResult BeginExecuteNonQuery(string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues);
        IAsyncResult BeginExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText, AsyncCallback callback, object state);
        IAsyncResult BeginExecuteNonQuery(DbTransaction transaction, string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues);
        IAsyncResult BeginExecuteReader(DbCommand command, AsyncCallback callback, object state);
        IAsyncResult BeginExecuteReader(CommandType commandType, string commandText, AsyncCallback callback, object state);
        IAsyncResult BeginExecuteReader(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state);
        IAsyncResult BeginExecuteReader(string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues);
        IAsyncResult BeginExecuteReader(DbTransaction transaction, CommandType commandType, string commandText, AsyncCallback callback, object state);
        IAsyncResult BeginExecuteReader(DbTransaction transaction, string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues);
        IAsyncResult BeginExecuteScalar(DbCommand command, AsyncCallback callback, object state);
        IAsyncResult BeginExecuteScalar(CommandType commandType, string commandText, AsyncCallback callback, object state);
        IAsyncResult BeginExecuteScalar(DbCommand command, DbTransaction transaction, AsyncCallback callback, object state);
        IAsyncResult BeginExecuteScalar(string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues);
        IAsyncResult BeginExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText, AsyncCallback callback, object state);
        IAsyncResult BeginExecuteScalar(DbTransaction transaction, string storedProcedureName, AsyncCallback callback, object state, params object[] parameterValues);
        string BuildParameterName(string name);
        DbConnection CreateConnection();
        void DiscoverParameters(DbCommand command);
        int EndExecuteNonQuery(IAsyncResult asyncResult);
        IDataReader EndExecuteReader(IAsyncResult asyncResult);
        object EndExecuteScalar(IAsyncResult asyncResult);
        DataSet ExecuteDataSet(DbCommand command);
        DataSet ExecuteDataSet(CommandType commandType, string commandText);
        DataSet ExecuteDataSet(DbCommand command, DbTransaction transaction);
        DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues);
        DataSet ExecuteDataSet(DbTransaction transaction, CommandType commandType, string commandText);
        DataSet ExecuteDataSet(DbTransaction transaction, string storedProcedureName, params object[] parameterValues);
        int ExecuteNonQuery(DbCommand command);
        int ExecuteNonQuery(CommandType commandType, string commandText);
        int ExecuteNonQuery(DbCommand command, DbTransaction transaction);
        int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues);
        int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText);
        int ExecuteNonQuery(DbTransaction transaction, string storedProcedureName, params object[] parameterValues);
        IDataReader ExecuteReader(DbCommand command);
        IDataReader ExecuteReader(CommandType commandType, string commandText);
        IDataReader ExecuteReader(DbCommand command, DbTransaction transaction);
        IDataReader ExecuteReader(string storedProcedureName, params object[] parameterValues);
        IDataReader ExecuteReader(DbTransaction transaction, CommandType commandType, string commandText);
        IDataReader ExecuteReader(DbTransaction transaction, string storedProcedureName, params object[] parameterValues);
        object ExecuteScalar(DbCommand command);
        object ExecuteScalar(CommandType commandType, string commandText);
        object ExecuteScalar(DbCommand command, DbTransaction transaction);
        object ExecuteScalar(string storedProcedureName, params object[] parameterValues);
        object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText);
        object ExecuteScalar(DbTransaction transaction, string storedProcedureName, params object[] parameterValues);
        DbDataAdapter GetDataAdapter();
        object GetParameterValue(DbCommand command, string name);
        DbCommand GetSqlStringCommand(string query);
        DbCommand GetStoredProcCommand(string storedProcedureName);
        DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues);
        DbCommand GetStoredProcCommandWithSourceColumns(string storedProcedureName, params string[] sourceColumns);
        void LoadDataSet(DbCommand command, DataSet dataSet, string tableName);
        void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames);
        void LoadDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames);
        void LoadDataSet(DbCommand command, DataSet dataSet, string tableName, DbTransaction transaction);
        void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames, DbTransaction transaction);
        void LoadDataSet(string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues);
        void LoadDataSet(DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames);
        void LoadDataSet(DbTransaction transaction, string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues);
        void SetParameterValue(DbCommand command, string parameterName, object value);
        int UpdateDataSet(DataSet dataSet, string tableName, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, DbTransaction transaction);
        int UpdateDataSet(DataSet dataSet, string tableName, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, DbTransaction transaction, int? updateBatchSize);
        //分页
        DataSet GetPagingData(string tblName, string strFldName, string strOrder, string strWhere, int pageIndex, int pageSize, bool doCount, out int recordCount);

        DataSet GetPagingData(string tblName, string strFldName, string strOrder, string strWhere, int pageIndex, int pageSize);
        DataSet GetPagingData(string tblName, string strFldName, int pageIndex, int pageSize);
        DataSet GetPagingData(string tblName, string strFldName, int pageIndex, int pageSize, out int recordCount);
        DataSet GetPagingData(string tblName, string strFldName, string strOrder, string strWhere, int pageIndex, int pageSize, out int recordCount);
        DataSet GetPagingData(string tblName, string strFldName, string strWhere, int pageIndex, int pageSize, out int recordCount);
        DataSet PageingQuery(int pageNo, int pageSize, string sql, string orderByFields, ref int recourdCount);
        DataSet PageingQuery(int pageNo, int pageSize, string storeProcedure, ref int recourdCount, Dictionary<string, string> parameters);
        IDataReader GetPagingData(string tblName, string strFldName, string strOrder, int pageIndex, string strWhere, int pageSize);
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <param name="orderByFields"></param>
        /// <param name="recourdCount"></param>
        /// <returns></returns>
        DataSet PageingQuery(int pageNo, int pageSize, string sql, string orderByFields, SqlParameter[] parameters, ref int recourdCount);


        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">查询语句</param>
        /// <param name="makeSource">返回List《T》对象的方法</param>
        /// <returns></returns>
        List<T> GetList<T>(string sql, Func<IDataReader, List<T>> makeSource);

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
        Pager<T> GetPageList<T>(string strSql, string strTotalSql, int currentPage, int pageSize, Action<IDataReader, Pager<T>> action, Func<IDataReader, List<T>> makeSource);

        /// <summary>
        /// 保存大量数据
        /// </summary>
        /// <param name="dtInsert">新增数据源</param>
        /// <param name="dtUpdate">编辑数据源</param>
        /// <param name="tableName">数据库表名</param>
        /// <param name="dicField">新增时 源数据列与目标表数据列的对应</param>
        /// <param name="updateCommand">编辑时 编辑语句</param>
        void SaveLargeData(DataTable dtInsert, DataTable dtUpdate, string tableName, IDictionary<string, string> dicField, DbCommand updateCommand);

        /// <summary>
        /// 保存大量数据
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事物</param>
        /// <param name="dtInsert">新增数据源</param>
        /// <param name="tableName">数据库表名</param>
        /// <param name="dicField">源数据列与目标表数据列的对应</param>
        void SaveLargeData(DbConnection conn, DbTransaction trans, DataTable dtInsert, string tableName, IDictionary<string, string> dicField);

        /// <summary>
        /// 保存大量数据
        /// </summary>
        /// <param name="trans">事物</param>
        /// <param name="dtUpdate">编辑数据源</param>
        /// <param name="tableName">数据库表名</param>
        /// <param name="updateCommand">编辑语句</param>
        void SaveLargeData(DbTransaction trans, DataTable dtUpdate, string tableName, DbCommand updateCommand);

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
        Pager<T> LargeDataPagerSingleTalbe<T>(string primaryKey, string orderBy, int pageSize, int pageCurrent, string fieldStr, string tableStr, string filter,
            string sumStr, Action<IDataReader, Pager<T>> MakeCount, Func<IDataReader, List<T>> MakeSource, Pager<T> pager = null);

        /// <summary>
        /// 表值参数
        /// by sxd
        /// </summary>
        /// <param name="trans">事务 可为null</param>
        /// <param name="StroreName">存储过程名称</param>
        /// <param name="tableParameterName">存储过程参数名称</param>
        /// <param name="dtValue">表值</param>
        /// <returns></returns>
        bool BatchByTableValueParameter(DbTransaction trans, string StroreName, string tableParameterName, DataTable dtValue);

        /// <summary>
        /// 文海峰 2013-10-24
        /// 将繁星集合转换成Table  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i_objlist"></param>
        /// <returns></returns>
        DataTable ConvertToDataTable<T>(IList<T> i_objlist);
    }
}
