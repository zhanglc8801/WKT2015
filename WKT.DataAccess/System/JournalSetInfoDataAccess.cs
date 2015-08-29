using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Data.SQL;
using WKT.Model;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class JournalSetInfoDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public JournalSetInfoDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static JournalSetInfoDataAccess _instance = new JournalSetInfoDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static JournalSetInfoDataAccess Instance
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
        public string JournalSetInfoQueryToSQLWhere(JournalSetInfoQuery query)
        {
            return string.Empty;
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string JournalSetInfoQueryToSQLOrder(JournalSetInfoQuery query)
        {
            return " SetID DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public JournalSetInfoEntity GetJournalSetInfo(Int32 setID)
        {
            JournalSetInfoEntity journalSetInfoEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  SetID,JournalID,ApiSiteID,DBServerID,AddDate FROM dbo.JournalSetInfo WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  SetID=@SetID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@SetID",DbType.Int32,setID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                journalSetInfoEntity = MakeJournalSetInfo(dr);
            }
            return journalSetInfoEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<JournalSetInfoEntity> GetJournalSetInfoList()
        {
            List<JournalSetInfoEntity> journalSetInfoEntity=new List<JournalSetInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  SetID,JournalID,ApiSiteID,DBServerID,AddDate FROM dbo.JournalSetInfo WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                journalSetInfoEntity = MakeJournalSetInfoList(dr);
            }
            return journalSetInfoEntity;
        }
        
        public List<JournalSetInfoEntity> GetJournalSetInfoList(JournalSetInfoQuery query)
        {
            List<JournalSetInfoEntity> list = new List<JournalSetInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT SetID,JournalID,ApiSiteID,DBServerID,AddDate FROM dbo.JournalSetInfo WITH(NOLOCK)");
            string whereSQL = JournalSetInfoQueryToSQLWhere(query);
            string orderBy=JournalSetInfoQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeJournalSetInfoList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<JournalSetInfoEntity></returns>
        public Pager<JournalSetInfoEntity> GetJournalSetInfoPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("JournalSetInfo","SetID,JournalID,ApiSiteID,DBServerID,AddDate",query.Order,query.Where,query.CurrentPage,query.PageSize,out recordCount);
            Pager<JournalSetInfoEntity>  pager = new Pager<JournalSetInfoEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeJournalSetInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<JournalSetInfoEntity> GetJournalSetInfoPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("JournalSetInfo","SetID,JournalID,ApiSiteID,DBServerID,AddDate"," SetID DESC","",query.CurrentPage,query.PageSize,out recordCount);
            Pager<JournalSetInfoEntity>  pager=new Pager<JournalSetInfoEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeJournalSetInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<JournalSetInfoEntity> GetJournalSetInfoPageList(JournalSetInfoQuery query)
        {
            int recordCount=0;
            string whereSQL=JournalSetInfoQueryToSQLWhere(query);
            string orderBy=JournalSetInfoQueryToSQLOrder(query);
            DataSet ds=db.GetPagingData("JournalSetInfo","SetID,JournalID,ApiSiteID,DBServerID,AddDate",orderBy,whereSQL,query.CurrentPage,query.PageSize,out recordCount);
            Pager<JournalSetInfoEntity>  pager=new Pager<JournalSetInfoEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeJournalSetInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddJournalSetInfo(JournalSetInfoEntity journalSetInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @SetID");
            sqlCommandText.Append(", @JournalID");
            sqlCommandText.Append(", @ApiSiteID");
            sqlCommandText.Append(", @DBServerID");
            sqlCommandText.Append(", @AddDate");
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.JournalSetInfo ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
            
            db.AddInParameter(cmd,"@SetID",DbType.Int32,journalSetInfoEntity.SetID);
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,journalSetInfoEntity.JournalID);
            db.AddInParameter(cmd,"@ApiSiteID",DbType.Int32,journalSetInfoEntity.ApiSiteID);
            db.AddInParameter(cmd,"@DBServerID",DbType.Int32,journalSetInfoEntity.DBServerID);
            db.AddInParameter(cmd,"@AddDate",DbType.DateTime,journalSetInfoEntity.AddDate);
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
     
        public bool UpdateJournalSetInfo(JournalSetInfoEntity journalSetInfoEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  SetID=@SetID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" JournalID=@JournalID");
            sqlCommandText.Append(", ApiSiteID=@ApiSiteID");
            sqlCommandText.Append(", DBServerID=@DBServerID");
            sqlCommandText.Append(", AddDate=@AddDate");
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.JournalSetInfo SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@SetID",DbType.Int32,journalSetInfoEntity.SetID);
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,journalSetInfoEntity.JournalID);
            db.AddInParameter(cmd,"@ApiSiteID",DbType.Int32,journalSetInfoEntity.ApiSiteID);
            db.AddInParameter(cmd,"@DBServerID",DbType.Int32,journalSetInfoEntity.DBServerID);
            db.AddInParameter(cmd,"@AddDate",DbType.DateTime,journalSetInfoEntity.AddDate);

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
        
        public bool DeleteJournalSetInfo(JournalSetInfoEntity journalSetInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.JournalSetInfo");
            sqlCommandText.Append(" WHERE  SetID=@SetID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@SetID",DbType.Int32,journalSetInfoEntity.SetID);
            
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
        
        public bool DeleteJournalSetInfo(Int32 setID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.JournalSetInfo");
            sqlCommandText.Append(" WHERE  SetID=@SetID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@SetID",DbType.Int32,setID);
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
        /// <param name="setID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteJournalSetInfo(Int32[] setID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from JournalSetInfo where ");
           
            for(int i=0;i<setID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( SetID=@SetID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<setID.Length;i++)
            {
            db.AddInParameter(cmd,"@SetID"+i,DbType.Int32,setID[i]);
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
        
        public JournalSetInfoEntity MakeJournalSetInfo(IDataReader dr)
        {
            JournalSetInfoEntity journalSetInfoEntity = null;
            if(dr.Read())
            {
             journalSetInfoEntity=new JournalSetInfoEntity();
             journalSetInfoEntity.SetID = (Int32)dr["SetID"];
             journalSetInfoEntity.JournalID = (Int64)dr["JournalID"];
             journalSetInfoEntity.ApiSiteID = (Int32)dr["ApiSiteID"];
             journalSetInfoEntity.DBServerID = (Int32)dr["DBServerID"];
             journalSetInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return journalSetInfoEntity;
        }
        
        public JournalSetInfoEntity MakeJournalSetInfo(DataRow dr)
        {
            JournalSetInfoEntity journalSetInfoEntity=null;
            if(dr!=null)
            {
                 journalSetInfoEntity=new JournalSetInfoEntity();
                 journalSetInfoEntity.SetID = (Int32)dr["SetID"];
                 journalSetInfoEntity.JournalID = (Int64)dr["JournalID"];
                 journalSetInfoEntity.ApiSiteID = (Int32)dr["ApiSiteID"];
                 journalSetInfoEntity.DBServerID = (Int32)dr["DBServerID"];
                 journalSetInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return journalSetInfoEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<JournalSetInfoEntity> MakeJournalSetInfoList(IDataReader dr)
        {
            List<JournalSetInfoEntity> list=new List<JournalSetInfoEntity>();
            while(dr.Read())
            {
             JournalSetInfoEntity journalSetInfoEntity=new JournalSetInfoEntity();
            journalSetInfoEntity.SetID = (Int32)dr["SetID"];
            journalSetInfoEntity.JournalID = (Int64)dr["JournalID"];
            journalSetInfoEntity.ApiSiteID = (Int32)dr["ApiSiteID"];
            journalSetInfoEntity.DBServerID = (Int32)dr["DBServerID"];
            journalSetInfoEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(journalSetInfoEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<JournalSetInfoEntity> MakeJournalSetInfoList(DataTable dt)
        {
            List<JournalSetInfoEntity> list=new List<JournalSetInfoEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   JournalSetInfoEntity journalSetInfoEntity=MakeJournalSetInfo(dt.Rows[i]);
                   list.Add(journalSetInfoEntity);
                }
            }
            return list;
        }
        
        #endregion

        # region 获取最大编号

        /// <summary>
        /// 获取指定库指定表的最大ID
        /// </summary>
        /// <param name="DBName">数据库名称</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public int GetMaxID(long JournalID,string DBName, string TableName)
        {
            DbCommand cmd = db.GetStoredProcCommand("UP_GetMaxID");
            db.AddInParameter(cmd, "@JournalID", System.Data.DbType.Int64, JournalID);
            db.AddInParameter(cmd, "@DbName", System.Data.DbType.AnsiString, DBName);
            db.AddInParameter(cmd, "@TableName", System.Data.DbType.AnsiString, TableName);
            object objMaxID = db.ExecuteScalar(cmd);
            if (objMaxID != null)
            {
                return Convert.ToInt32(objMaxID);
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 获取指定库指定表的最大ID
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="DBName">数据库名称</param>
        /// <param name="TableName">表名</param>
        /// <param name="Num">位数</param>
        /// <returns></returns>
        public string GetMaxID(long JournalID, string DBName, string TableName,int Num)
        {
            DbCommand cmd = db.GetStoredProcCommand("UP_GetMaxID");
            db.AddInParameter(cmd, "@JournalID", System.Data.DbType.Int64, JournalID);
            db.AddInParameter(cmd, "@DbName", System.Data.DbType.AnsiString, DBName);
            db.AddInParameter(cmd, "@TableName", System.Data.DbType.AnsiString, TableName);
            object objMaxID = db.ExecuteScalar(cmd);
            return WKT.Common.Utils.Utils.MendZero(WKT.Common.Utils.TypeParse.ToLong(objMaxID, 1), Num);
        }


        /// <summary>
        /// 根据JournalID删除IDPool中的一条记录
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="DBName">数据库名称</param>
        /// <param name="TableName">表名</param>
        /// <param name="Num">位数</param>
        /// <returns></returns>
        public bool DeleteIDPoolByJournalID(long JournalID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.IDPool");
            sqlCommandText.Append(" WHERE  JournalID=@JournalID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, JournalID);
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


        /// <summary>
        /// 根据JournalID删除IDPool中的一条记录
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="DBName">数据库名称</param>
        /// <param name="TableName">表名</param>
        /// <param name="Num">位数</param>
        /// <returns></returns>
        public bool UpdateIDPoolByJournalID(long JournalID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("update  dbo.IDPool set MaxID=0 ");
            sqlCommandText.Append(" WHERE  JournalID=@JournalID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, JournalID);
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

        # endregion

    }
}

