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
    public partial class ApiServerInfoDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public ApiServerInfoDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTSysDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static ApiServerInfoDataAccess _instance = new ApiServerInfoDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static ApiServerInfoDataAccess Instance
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
        public string ApiServerInfoQueryToSQLWhere(ApiServerInfoQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            if (query.Status != null)
            {
                sbWhere.Append(" AND Status= ").Append(query.Status.Value);
            }
            if (!string.IsNullOrEmpty(query.SiteName))
            {
                sbWhere.AppendFormat(" AND SiteName='{0}'", WKT.Common.Security.SecurityUtils.SafeSqlString(query.SiteName));
            }
            if (sbWhere.ToString() == " 1=1 ")
            {
                return string.Empty;
            }
            else
            {
                return sbWhere.ToString();
            }
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string ApiServerInfoQueryToSQLOrder(ApiServerInfoQuery query)
        {
            return " ApiServerID DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public ApiServerInfoEntity GetApiServerInfo(Int32 apiServerID)
        {
            ApiServerInfoEntity apiServerInfoEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  ApiServerID,SiteName,SiteUrl,Status,Note,AddDate FROM dbo.ApiServerInfo WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  ApiServerID=@ApiServerID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@ApiServerID",DbType.Int32,apiServerID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                apiServerInfoEntity = MakeApiServerInfo(dr);
            }
            return apiServerInfoEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<ApiServerInfoEntity> GetApiServerInfoList()
        {
            List<ApiServerInfoEntity> apiServerInfoEntity=new List<ApiServerInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  ApiServerID,SiteName,SiteUrl,Status,Note,AddDate FROM dbo.ApiServerInfo WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                apiServerInfoEntity = MakeApiServerInfoList(dr);
            }
            return apiServerInfoEntity;
        }
        
        public List<ApiServerInfoEntity> GetApiServerInfoList(ApiServerInfoQuery query)
        {
            List<ApiServerInfoEntity> list = new List<ApiServerInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT ApiServerID,SiteName,SiteUrl,Status,Note,AddDate FROM dbo.ApiServerInfo WITH(NOLOCK)");
            string whereSQL = ApiServerInfoQueryToSQLWhere(query);
            string orderBy=ApiServerInfoQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeApiServerInfoList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<ApiServerInfoEntity></returns>
        public Pager<ApiServerInfoEntity> GetApiServerInfoPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("ApiServerInfo","ApiServerID,SiteName,SiteUrl,Status,Note,AddDate",query.Order,query.Where,query.CurrentPage,query.PageSize,out recordCount);
            Pager<ApiServerInfoEntity>  pager = new Pager<ApiServerInfoEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeApiServerInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<ApiServerInfoEntity> GetApiServerInfoPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("ApiServerInfo","ApiServerID,SiteName,SiteUrl,Status,Note,AddDate"," ApiServerID DESC","",query.CurrentPage,query.PageSize,out recordCount);
            Pager<ApiServerInfoEntity>  pager=new Pager<ApiServerInfoEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeApiServerInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<ApiServerInfoEntity> GetApiServerInfoPageList(ApiServerInfoQuery query)
        {
            int recordCount=0;
            string whereSQL=ApiServerInfoQueryToSQLWhere(query);
            string orderBy=ApiServerInfoQueryToSQLOrder(query);
            DataSet ds=db.GetPagingData("ApiServerInfo","ApiServerID,SiteName,SiteUrl,Status,Note,AddDate",orderBy,whereSQL,query.CurrentPage,query.PageSize,out recordCount);
            Pager<ApiServerInfoEntity>  pager=new Pager<ApiServerInfoEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeApiServerInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddApiServerInfo(ApiServerInfoEntity apiServerInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("@SiteName");
            sqlCommandText.Append(", @SiteUrl");
            sqlCommandText.Append(", @Status");
            sqlCommandText.Append(", @Note");
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.ApiServerInfo ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
            
            db.AddInParameter(cmd,"@SiteName",DbType.AnsiString,apiServerInfoEntity.SiteName);
            db.AddInParameter(cmd,"@SiteUrl",DbType.AnsiString,apiServerInfoEntity.SiteUrl);
            db.AddInParameter(cmd,"@Status",DbType.Byte,apiServerInfoEntity.Status);
            db.AddInParameter(cmd,"@Note",DbType.AnsiString,apiServerInfoEntity.Note);

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
     
        public bool UpdateApiServerInfo(ApiServerInfoEntity apiServerInfoEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  ApiServerID=@ApiServerID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" SiteName=@SiteName");
            sqlCommandText.Append(", SiteUrl=@SiteUrl");
            sqlCommandText.Append(", Status=@Status");
            sqlCommandText.Append(", Note=@Note");
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.ApiServerInfo SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@ApiServerID",DbType.Int32,apiServerInfoEntity.ApiServerID);
            db.AddInParameter(cmd,"@SiteName",DbType.AnsiString,apiServerInfoEntity.SiteName);
            db.AddInParameter(cmd,"@SiteUrl",DbType.AnsiString,apiServerInfoEntity.SiteUrl);
            db.AddInParameter(cmd,"@Status",DbType.Byte,apiServerInfoEntity.Status);
            db.AddInParameter(cmd,"@Note",DbType.AnsiString,apiServerInfoEntity.Note);

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
        
        public bool DeleteApiServerInfo(ApiServerInfoEntity apiServerInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.ApiServerInfo");
            sqlCommandText.Append(" WHERE  ApiServerID=@ApiServerID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@ApiServerID",DbType.Int32,apiServerInfoEntity.ApiServerID);
            
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
        
        public bool DeleteApiServerInfo(Int32 apiServerID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.ApiServerInfo");
            sqlCommandText.Append(" WHERE  ApiServerID=@ApiServerID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@ApiServerID",DbType.Int32,apiServerID);
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
        /// <param name="apiServerID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteApiServerInfo(Int32[] apiServerID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from ApiServerInfo where ");
           
            for(int i=0;i<apiServerID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( ApiServerID=@ApiServerID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<apiServerID.Length;i++)
            {
            db.AddInParameter(cmd,"@ApiServerID"+i,DbType.Int32,apiServerID[i]);
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
        
        public ApiServerInfoEntity MakeApiServerInfo(IDataReader dr)
        {
            ApiServerInfoEntity apiServerInfoEntity = null;
            if(dr.Read())
            {
             apiServerInfoEntity=new ApiServerInfoEntity();
             apiServerInfoEntity.ApiServerID = (Int32)dr["ApiServerID"];
             apiServerInfoEntity.SiteName = (String)dr["SiteName"];
             apiServerInfoEntity.SiteUrl = (String)dr["SiteUrl"];
             apiServerInfoEntity.Status = (Byte)dr["Status"];
             apiServerInfoEntity.Note = (String)dr["Note"];
             apiServerInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return apiServerInfoEntity;
        }
        
        public ApiServerInfoEntity MakeApiServerInfo(DataRow dr)
        {
            ApiServerInfoEntity apiServerInfoEntity=null;
            if(dr!=null)
            {
                 apiServerInfoEntity=new ApiServerInfoEntity();
                 apiServerInfoEntity.ApiServerID = (Int32)dr["ApiServerID"];
                 apiServerInfoEntity.SiteName = (String)dr["SiteName"];
                 apiServerInfoEntity.SiteUrl = (String)dr["SiteUrl"];
                 apiServerInfoEntity.Status = (Byte)dr["Status"];
                 apiServerInfoEntity.Note = (String)dr["Note"];
                 apiServerInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return apiServerInfoEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<ApiServerInfoEntity> MakeApiServerInfoList(IDataReader dr)
        {
            List<ApiServerInfoEntity> list=new List<ApiServerInfoEntity>();
            while(dr.Read())
            {
             ApiServerInfoEntity apiServerInfoEntity=new ApiServerInfoEntity();
            apiServerInfoEntity.ApiServerID = (Int32)dr["ApiServerID"];
            apiServerInfoEntity.SiteName = (String)dr["SiteName"];
            apiServerInfoEntity.SiteUrl = (String)dr["SiteUrl"];
            apiServerInfoEntity.Status = (Byte)dr["Status"];
            apiServerInfoEntity.Note = (String)dr["Note"];
            apiServerInfoEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(apiServerInfoEntity);
            }

            dr.Close();
            return list;
        }
        
        
        public List<ApiServerInfoEntity> MakeApiServerInfoList(DataTable dt)
        {
            List<ApiServerInfoEntity> list=new List<ApiServerInfoEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   ApiServerInfoEntity apiServerInfoEntity=MakeApiServerInfo(dt.Rows[i]);
                   list.Add(apiServerInfoEntity);
                }
            }
            return list;
        }
        
        #endregion

    }
}

