using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Data.SQL;
using WKT.Common.Utils;
using WKT.Common.Security;
using WKT.Common.Extension;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class DictValueDataAccess:DataAccessBase
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public DictValueDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static DictValueDataAccess _instance = new DictValueDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static DictValueDataAccess Instance
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
        public string DictValueQueryToSQLWhere(DictValueQuery query)
        {
            StringBuilder strFilter = new StringBuilder();
            strFilter.Append(" 1=1");
            if (query.JournalID > 0)
                strFilter.AppendFormat(" and a.JournalID={0}", query.JournalID);
            if (query.DictID > 0)
                strFilter.AppendFormat(" and exists(SELECT 1 FROM dbo.Dict b with(nolock) where a.DictKey=b.DictKey and b.DictID={0})", query.DictID);
            if (!string.IsNullOrEmpty(query.DictKey))
            {
                strFilter.AppendFormat(" and a.DictKey='{0}'", SecurityUtils.SafeSqlString(query.DictKey));
            }
            return strFilter.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string DictValueQueryToSQLOrder(DictValueQuery query)
        {
            return " SortID ASC,DictValueID DESC";
        }
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public DictValueEntity GetDictValue(Int64 dictValueID)
        {
            DictValueEntity dictValueEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  DictValueID,JournalID,DictKey,ValueID,ValueText,SortID,InUserID,EditUserID,EditDate,AddDate FROM dbo.DictValue WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  DictValueID=@DictValueID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@DictValueID",DbType.Int64,dictValueID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                dictValueEntity = MakeDictValue(dr);
            }
            return dictValueEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<DictValueEntity> GetDictValueList()
        {
            List<DictValueEntity> dictValueEntity=new List<DictValueEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  DictValueID,JournalID,DictKey,ValueID,ValueText,SortID,InUserID,EditUserID,EditDate,AddDate FROM dbo.DictValue WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                dictValueEntity = MakeDictValueList(dr);
            }
            return dictValueEntity;
        }
        
        public List<DictValueEntity> GetDictValueList(DictValueQuery query)
        {
            List<DictValueEntity> list = new List<DictValueEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT DictValueID,JournalID,DictKey,ValueID,ValueText,SortID,InUserID,EditUserID,EditDate,AddDate FROM dbo.DictValue a WITH(NOLOCK)");
            string whereSQL = DictValueQueryToSQLWhere(query);
            string orderBy=DictValueQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeDictValueList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<DictValueEntity></returns>
        public Pager<DictValueEntity> GetDictValuePageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("DictValue","DictValueID,JournalID,DictKey,ValueID,ValueText,SortID,InUserID,EditUserID,EditDate,AddDate",query.Order,query.Where,query.CurrentPage,query.PageSize,out recordCount);
            Pager<DictValueEntity>  pager = new Pager<DictValueEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeDictValueList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<DictValueEntity> GetDictValuePageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("DictValue","DictValueID,JournalID,DictKey,ValueID,ValueText,SortID,InUserID,EditUserID,EditDate,AddDate"," DictValueID DESC","",query.CurrentPage,query.PageSize,out recordCount);
            Pager<DictValueEntity>  pager=new Pager<DictValueEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeDictValueList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<DictValueEntity> GetDictValuePageList(DictValueQuery query)
        {
            string strSql = "SELECT DictValueID,JournalID,DictKey,ValueID,ValueText,SortID,InUserID,EditUserID,EditDate,AddDate,ROW_NUMBER() OVER(ORDER BY DictValueID desc) AS ROW_ID FROM dbo.DictValue a with(nolock)",
                   sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.DictValue a with(nolock)";
            string whereSQL = DictValueQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<DictValueEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeDictValueList);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddDictValue(DictValueEntity dictValueEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();         
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @DictKey");
            sqlCommandText.Append(", @ValueID");
            sqlCommandText.Append(", @ValueText");
            sqlCommandText.Append(", @SortID");
            sqlCommandText.Append(", @InUserID");          
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.DictValue ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
            
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,dictValueEntity.JournalID);
            db.AddInParameter(cmd,"@DictKey",DbType.AnsiString,dictValueEntity.DictKey);
            db.AddInParameter(cmd,"@ValueID",DbType.Int32,dictValueEntity.ValueID);
            db.AddInParameter(cmd,"@ValueText",DbType.AnsiString,dictValueEntity.ValueText);
            db.AddInParameter(cmd,"@SortID",DbType.Int32,dictValueEntity.SortID);
            db.AddInParameter(cmd,"@InUserID",DbType.Int64,dictValueEntity.InUserID);           
            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch(SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }
        
        #endregion
        
        #region 更新数据
     
        public bool UpdateDictValue(DictValueEntity dictValueEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  DictValueID=@DictValueID");
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" ValueID=@ValueID");
            sqlCommandText.Append(", ValueText=@ValueText");
            sqlCommandText.Append(", SortID=@SortID");           
            sqlCommandText.Append(", EditUserID=@EditUserID");
            sqlCommandText.Append(", EditDate=getdate()");           
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.DictValue SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@DictValueID",DbType.Int64,dictValueEntity.DictValueID);           
            db.AddInParameter(cmd,"@ValueID",DbType.Int32,dictValueEntity.ValueID);
            db.AddInParameter(cmd,"@ValueText",DbType.AnsiString,dictValueEntity.ValueText);
            db.AddInParameter(cmd,"@SortID",DbType.Int32,dictValueEntity.SortID);          
            db.AddInParameter(cmd,"@EditUserID",DbType.Int64,dictValueEntity.EditUserID);
           
            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch(SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }
        
        #endregion
       
        #region 删除对象
        
        #region 删除一个对象
        
        public bool DeleteDictValue(DictValueEntity dictValueEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.DictValue");
            sqlCommandText.Append(" WHERE  DictValueID=@DictValueID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@DictValueID",DbType.Int64,dictValueEntity.DictValueID);
            
            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch(SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }
        
        public bool DeleteDictValue(Int64 dictValueID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.DictValue");
            sqlCommandText.Append(" WHERE  DictValueID=@DictValueID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@DictValueID",DbType.Int64,dictValueID);
            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch(SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }
        
        #endregion
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dictValueID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteDictValue(Int64[] dictValueID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from DictValue where ");
           
            for(int i=0;i<dictValueID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( DictValueID=@DictValueID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<dictValueID.Length;i++)
            {
            db.AddInParameter(cmd,"@DictValueID"+i,DbType.Int64,dictValueID[i]);
            }
            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch(SqlException sqlEx)
            {
                throw sqlEx;
            }
            
            return flag;
        }
        
        #endregion
        
        #endregion
        
        #region 根据数据组装一个对象
        
        public DictValueEntity MakeDictValue(IDataReader dr)
        {
            DictValueEntity dictValueEntity = null;
            if(dr.Read())
            {
             dictValueEntity=new DictValueEntity();
             dictValueEntity.DictValueID = (Int64)dr["DictValueID"];
             dictValueEntity.JournalID = (Int64)dr["JournalID"];
             dictValueEntity.DictKey = (String)dr["DictKey"];
             dictValueEntity.ValueID = (Int32)dr["ValueID"];
             dictValueEntity.ValueText = (String)dr["ValueText"];
             dictValueEntity.SortID = (Int32)dr["SortID"];
             dictValueEntity.InUserID = (Int64)dr["InUserID"];
             dictValueEntity.EditUserID = (Int64)dr["EditUserID"];
             dictValueEntity.EditDate = (DateTime)dr["EditDate"];
             dictValueEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return dictValueEntity;
        }
        
        public DictValueEntity MakeDictValue(DataRow dr)
        {
            DictValueEntity dictValueEntity=null;
            if(dr!=null)
            {
                 dictValueEntity=new DictValueEntity();
                 dictValueEntity.DictValueID = (Int64)dr["DictValueID"];
                 dictValueEntity.JournalID = (Int64)dr["JournalID"];
                 dictValueEntity.DictKey = (String)dr["DictKey"];
                 dictValueEntity.ValueID = (Int32)dr["ValueID"];
                 dictValueEntity.ValueText = (String)dr["ValueText"];
                 dictValueEntity.SortID = (Int32)dr["SortID"];
                 dictValueEntity.InUserID = (Int64)dr["InUserID"];
                 dictValueEntity.EditUserID = (Int64)dr["EditUserID"];
                 dictValueEntity.EditDate = (DateTime)dr["EditDate"];
                 dictValueEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return dictValueEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<DictValueEntity> MakeDictValueList(IDataReader dr)
        {
            List<DictValueEntity> list=new List<DictValueEntity>();
            while(dr.Read())
            {
             DictValueEntity dictValueEntity=new DictValueEntity();
            dictValueEntity.DictValueID = (Int64)dr["DictValueID"];
            dictValueEntity.JournalID = (Int64)dr["JournalID"];
            dictValueEntity.DictKey = (String)dr["DictKey"];
            dictValueEntity.ValueID = (Int32)dr["ValueID"];
            dictValueEntity.ValueText = (String)dr["ValueText"];
            dictValueEntity.SortID = (Int32)dr["SortID"];
            dictValueEntity.InUserID = (Int64)dr["InUserID"];
            dictValueEntity.EditUserID = (Int64)dr["EditUserID"];
            dictValueEntity.EditDate = (DateTime)dr["EditDate"];
            dictValueEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(dictValueEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<DictValueEntity> MakeDictValueList(DataTable dt)
        {
            List<DictValueEntity> list=new List<DictValueEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   DictValueEntity dictValueEntity=MakeDictValue(dt.Rows[i]);
                   list.Add(dictValueEntity);
                }
            }
            return list;
        }
        
        #endregion

        /// <summary>
        /// 数据字典值是否已经存在
        /// </summary>
        /// <param name="model"></param>       
        /// <returns></returns>
        public bool DictValueIsExists(DictValueEntity model)
        {
            string strSql = "SELECT 1 FROM dbo.DictValue a with(nolock) WHERE JournalID=@JournalID and DictKey=@DictKey and ValueID=@ValueID";
            if (model.DictValueID > 0)
            {
                strSql += " and DictValueID<>" + model.DictValueID;
            }
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@DictKey", DbType.AnsiString, model.DictKey);
            db.AddInParameter(cmd, "@ValueID", DbType.Int32, model.ValueID);
            object obj = db.ExecuteScalar(cmd);
            if (obj == null) return false;
            return obj.TryParse<Int32>() == 1;
        }

        /// <summary>
        /// 获取数据字典键值对
        /// </summary>
        /// <param name="dictKey"></param>
        /// <returns></returns>
        public IDictionary<int, string> GetDictValueDcit(Int64 JournalID,string dictKey)
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();
            string strSql = "SELECT ValueID,ValueText FROM DictValue WHERE JournalID=@JournalID and DictKey=@DictKey order by SortID";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd,"@JournalID", DbType.Int64, JournalID);
            db.AddInParameter(cmd, "@DictKey", DbType.AnsiString, dictKey);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                Int32 valueId = 0;
                while (dr.Read())
                {
                    valueId = (Int32)dr["ValueID"];
                    if (!dict.ContainsKey(valueId))
                        dict.Add(valueId, (String)dr["ValueText"]);
                }
                dr.Close();
            }
            return dict;
        }

    }
}

