using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Data.SQL;
using WKT.Common.Utils;
using WKT.Common.Extension;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class DictDataAccess:DataAccessBase
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public DictDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static DictDataAccess _instance = new DictDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static DictDataAccess Instance
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
        public string DictQueryToSQLWhere(DictQuery query)
        {            
            if (query.JournalID > 0)
                return " JournalID=" + query.JournalID;
            return string.Empty;
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string DictQueryToSQLOrder(DictQuery query)
        {
            return " DictID DESC";
        }
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public DictEntity GetDict(Int64 dictID)
        {
            DictEntity dictEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  DictID,JournalID,DictKey,Note,InUserID,EditUserID,EditDate,AddDate FROM dbo.Dict WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  DictID=@DictID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@DictID",DbType.Int64,dictID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                dictEntity = MakeDict(dr);
            }
            return dictEntity;
        }

        public DictEntity GetDictByKey(string dictKey, long JournalID)
        {
            DictEntity dictEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  DictID,JournalID,DictKey,Note,InUserID,EditUserID,EditDate,AddDate FROM dbo.Dict WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  DictKey=@DictKey");
            sqlCommandText.Append("  AND  JournalID=@JournalID");
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@DictKey", DbType.String, dictKey);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                dictEntity = MakeDict(dr);
            }
            return dictEntity;
        }

        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<DictEntity> GetDictList()
        {
            List<DictEntity> dictEntity=new List<DictEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  DictID,JournalID,DictKey,Note,InUserID,EditUserID,EditDate,AddDate FROM dbo.Dict WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                dictEntity = MakeDictList(dr);
            }
            return dictEntity;
        }
        
        public List<DictEntity> GetDictList(DictQuery query)
        {
            List<DictEntity> list = new List<DictEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT DictID,JournalID,DictKey,Note,InUserID,EditUserID,EditDate,AddDate FROM dbo.Dict WITH(NOLOCK)");
            string whereSQL = DictQueryToSQLWhere(query);
            string orderBy=DictQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeDictList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<DictEntity></returns>
        public Pager<DictEntity> GetDictPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("Dict","DictID,JournalID,DictKey,Note,InUserID,EditUserID,EditDate,AddDate",query.Order,query.Where,query.CurrentPage,query.PageSize,out recordCount);
            Pager<DictEntity>  pager = new Pager<DictEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeDictList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<DictEntity> GetDictPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("Dict","DictID,JournalID,DictKey,Note,InUserID,EditUserID,EditDate,AddDate"," DictID DESC","",query.CurrentPage,query.PageSize,out recordCount);
            Pager<DictEntity>  pager=new Pager<DictEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeDictList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<DictEntity> GetDictPageList(DictQuery query)
        {         
            string strSql = "SELECT DictID,JournalID,DictKey,Note,InUserID,EditUserID,EditDate,AddDate,ROW_NUMBER() OVER(ORDER BY DictID DESC) AS ROW_ID FROM dbo.Dict with(nolock)",
                   sumStr="SELECT RecordCount=COUNT(1) FROM dbo.Dict with(nolock)";
            string whereSQL=DictQueryToSQLWhere(query);
            if(!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql+=" WHERE "+whereSQL;
                sumStr+=" WHERE "+whereSQL;
            }
            return db.GetPageList<DictEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeDictList);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddDict(DictEntity dictEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @DictKey");
            sqlCommandText.Append(", @Note");
            sqlCommandText.Append(", @InUserID");           
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.Dict ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
           
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,dictEntity.JournalID);
            db.AddInParameter(cmd,"@DictKey",DbType.AnsiString,dictEntity.DictKey);
            db.AddInParameter(cmd,"@Note",DbType.AnsiString,dictEntity.Note);
            db.AddInParameter(cmd,"@InUserID",DbType.Int64,dictEntity.InUserID);           
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
     
        public bool UpdateDict(DictEntity dictEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  DictID=@DictID");
            StringBuilder sqlCommandText = new StringBuilder();          
            sqlCommandText.Append(" DictKey=@DictKey");
            sqlCommandText.Append(", Note=@Note");          
            sqlCommandText.Append(", EditUserID=@EditUserID");
            sqlCommandText.Append(", EditDate=getdate()");        
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.Dict SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@DictID",DbType.Int64,dictEntity.DictID);           
            db.AddInParameter(cmd,"@DictKey",DbType.AnsiString,dictEntity.DictKey);
            db.AddInParameter(cmd,"@Note",DbType.AnsiString,dictEntity.Note);          
            db.AddInParameter(cmd,"@EditUserID",DbType.Int64,dictEntity.EditUserID);
            
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
        
        public bool DeleteDict(DictEntity dictEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.Dict");
            sqlCommandText.Append(" WHERE  DictID=@DictID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@DictID",DbType.Int64,dictEntity.DictID);
            
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

        public bool DeleteDict(Int64 dictID, DbTransaction tran = null)
        {            
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(@"if not exists(select 1 from dbo.DictValue a with(nolock) 
                                              INNER JOIN dbo.Dict b on a.JournalID=b.JournalID and a.DictKey=b.DictKey and b.DictID=@DictID)");
            sqlCommandText.Append(" begin ");
            sqlCommandText.Append("DELETE FROM dbo.Dict");
            sqlCommandText.Append(" WHERE  DictID=@DictID");
            sqlCommandText.Append(" end ");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@DictID", DbType.Int64, dictID);
            try
            {
                if (tran == null)
                    return db.ExecuteNonQuery(cmd) > 0;
                else
                    return db.ExecuteNonQuery(cmd, tran) > 0;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }          
        }
        
        #endregion
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dictID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public IList<Int64> BatchDeleteDict(IList<Int64> dictID)
        {
            if (dictID == null)
                return null;
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    IList<Int64> list = new List<Int64>();
                    foreach (var id in dictID)
                    {
                        if (!DeleteDict(id, trans))
                            list.Add(id);
                    }
                                        
                    trans.Commit();
                    return list;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }
        
        #endregion
        
        #endregion
        
        #region 根据数据组装一个对象
        
        public DictEntity MakeDict(IDataReader dr)
        {
            DictEntity dictEntity = null;
            if(dr.Read())
            {
             dictEntity=new DictEntity();
             dictEntity.DictID = (Int64)dr["DictID"];
             dictEntity.JournalID = (Int64)dr["JournalID"];
             dictEntity.DictKey = (String)dr["DictKey"];
             dictEntity.Note = (String)dr["Note"];
             dictEntity.InUserID = (Int64)dr["InUserID"];
             dictEntity.EditUserID = (Int64)dr["EditUserID"];
             dictEntity.EditDate = (DateTime)dr["EditDate"];
             dictEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return dictEntity;
        }
        
        public DictEntity MakeDict(DataRow dr)
        {
            DictEntity dictEntity=null;
            if(dr!=null)
            {
                 dictEntity=new DictEntity();
                 dictEntity.DictID = (Int64)dr["DictID"];
                 dictEntity.JournalID = (Int64)dr["JournalID"];
                 dictEntity.DictKey = (String)dr["DictKey"];
                 dictEntity.Note = (String)dr["Note"];
                 dictEntity.InUserID = (Int64)dr["InUserID"];
                 dictEntity.EditUserID = (Int64)dr["EditUserID"];
                 dictEntity.EditDate = (DateTime)dr["EditDate"];
                 dictEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return dictEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<DictEntity> MakeDictList(IDataReader dr)
        {
            List<DictEntity> list=new List<DictEntity>();
            while(dr.Read())
            {
             DictEntity dictEntity=new DictEntity();
            dictEntity.DictID = (Int64)dr["DictID"];
            dictEntity.JournalID = (Int64)dr["JournalID"];
            dictEntity.DictKey = (String)dr["DictKey"];
            dictEntity.Note = (String)dr["Note"];
            dictEntity.InUserID = (Int64)dr["InUserID"];
            dictEntity.EditUserID = (Int64)dr["EditUserID"];
            dictEntity.EditDate = (DateTime)dr["EditDate"];
            dictEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(dictEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<DictEntity> MakeDictList(DataTable dt)
        {
            List<DictEntity> list=new List<DictEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   DictEntity dictEntity=MakeDict(dt.Rows[i]);
                   list.Add(dictEntity);
                }
            }
            return list;
        }
        
        #endregion

        /// <summary>
        /// 数据字典是否已经存在
        /// </summary>
        /// <param name="model"></param>       
        /// <returns></returns>
        public bool DictkeyIsExists(DictEntity model)
        {
            string strSql = "SELECT 1 FROM dbo.Dict a with(nolock) WHERE JournalID=@JournalID and DictKey=@DictKey";
            if (model.DictID > 0)
            {
                strSql += " and DictID<>" + model.DictID;
            }
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@DictKey", DbType.AnsiString, model.DictKey);
            object obj = db.ExecuteScalar(cmd);
            if (obj == null) return false;
            return obj.TryParse<Int32>() == 1;
        }

    }
}

