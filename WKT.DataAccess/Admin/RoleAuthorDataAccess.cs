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
    public partial class RoleAuthorDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public RoleAuthorDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static RoleAuthorDataAccess _instance = new RoleAuthorDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static RoleAuthorDataAccess Instance
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
        public string RoleAuthorQueryToSQLWhere(RoleAuthorQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JournalID = " + query.JournalID);
            if (query.AuthorID != null)
            {
                sbWhere.Append(" AND AuthorID = ").Append(query.AuthorID.Value).Append("");
            }
            if (query.RoleID != null)
            {
                sbWhere.Append(" AND RoleID = ").Append(query.RoleID.Value).Append("");
            }
            return sbWhere.ToString();
        }

        public string RoleAuthorDetailQueryToSQLWhere(RoleAuthorQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" RA.JournalID = " + query.JournalID);
            if (query.AuthorID != null)
            {
                sbWhere.Append(" AND RA.AuthorID = ").Append(query.AuthorID.Value).Append("");
            }

            if (query.RealName != null)
            {
                sbWhere.Append(" AND RealName like '%").Append(query.RealName).Append("%'");
            }

            if (query.GroupID != null)
            {
                sbWhere.Append(" AND AI.GroupID = ").Append(query.GroupID.Value).Append("");
            }

            if (query.RoleID != null)
            {
                sbWhere.Append(" AND RI.RoleID = ").Append(query.RoleID.Value).Append("");
            }

            if (query.RoleID != null)
            {
                sbWhere.Append(" AND RA.RoleID = ").Append(query.RoleID.Value).Append("");
            }
            if (query.RoleName != null)
            {
                sbWhere.Append(" AND RI.RoleName = ").Append(query.RoleName).Append("");
            }
            return sbWhere.ToString();
        }

        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string RoleAuthorQueryToSQLOrder(RoleAuthorQuery query)
        {
            if (query.OrderStr != null)
                return query.OrderStr;
            else
                return " MapID ASC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public RoleAuthorEntity GetRoleAuthor(Int64 mapID)
        {
            RoleAuthorEntity roleAuthorEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  MapID,JournalID,RoleID,AuthorID,AddDate FROM dbo.RoleAuthor WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  MapID=@MapID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@MapID",DbType.Int64,mapID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                roleAuthorEntity = MakeRoleAuthor(dr);
            }
            return roleAuthorEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<RoleAuthorEntity> GetRoleAuthorList()
        {
            List<RoleAuthorEntity> roleAuthorEntity=new List<RoleAuthorEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  MapID,JournalID,RoleID,AuthorID,AddDate FROM dbo.RoleAuthor WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                roleAuthorEntity = MakeRoleAuthorList(dr);
            }
            return roleAuthorEntity;
        }
        
        public List<RoleAuthorEntity> GetRoleAuthorList(RoleAuthorQuery query)
        {
            List<RoleAuthorEntity> list = new List<RoleAuthorEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT MapID,JournalID,RoleID,AuthorID,AddDate FROM dbo.RoleAuthor WITH(NOLOCK)");
            string whereSQL = RoleAuthorQueryToSQLWhere(query);
            string orderBy=RoleAuthorQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeRoleAuthorList(dr);
            }
            return list;
        }

        public List<RoleAuthorEntity> GetRoleAuthorDetailList(RoleAuthorQuery query)
        {
            List<RoleAuthorEntity> list = new List<RoleAuthorEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(@"SELECT MapID,RA.JournalID,RA.AuthorID,AI.LoginName,AI.RealName,RA.RoleID,RI.RoleName,RA.AddDate FROM dbo.RoleAuthor RA WITH(NOLOCK) 
                                    LEFT JOIN dbo.AuthorInfo AI WITH(NOLOCK) ON RA.AuthorID=AI.AuthorID AND AI.Status=1
                                    LEFT JOIN dbo.RoleInfo RI WITH(NOLOCK) ON RA.JournalID=RI.JournalID AND RA.RoleID=RI.RoleID ");
            string whereSQL = RoleAuthorDetailQueryToSQLWhere(query);
            string orderBy = RoleAuthorQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeRoleAuthorDetailList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<RoleAuthorEntity></returns>
        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("RoleAuthor","MapID,JournalID,RoleID,AuthorID,AddDate",query.Order,query.Where,query.CurrentPage,query.PageSize,out recordCount);
            Pager<RoleAuthorEntity>  pager = new Pager<RoleAuthorEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeRoleAuthorList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("RoleAuthor","MapID,JournalID,RoleID,AuthorID,AddDate"," MapID DESC","",query.CurrentPage,query.PageSize,out recordCount);
            Pager<RoleAuthorEntity>  pager=new Pager<RoleAuthorEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeRoleAuthorList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(RoleAuthorQuery query)
        {
            int recordCount=0;
            string whereSQL=RoleAuthorQueryToSQLWhere(query);
            string orderBy=RoleAuthorQueryToSQLOrder(query);
            DataSet ds=db.GetPagingData("RoleAuthor","MapID,JournalID,RoleID,AuthorID,AddDate",orderBy,whereSQL,query.CurrentPage,query.PageSize,out recordCount);
            Pager<RoleAuthorEntity>  pager=new Pager<RoleAuthorEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeRoleAuthorList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 赋权
        /// </summary>
        /// <param name="roleAuthorEntity"></param>
        /// <returns></returns>
        public bool AddRoleAuthor(RoleAuthorEntity roleAuthorEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("@JournalID");
            sqlCommandText.Append(", @RoleID");
            sqlCommandText.Append(", @AuthorID");
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.RoleAuthor ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
            
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,roleAuthorEntity.JournalID);
            db.AddInParameter(cmd,"@RoleID",DbType.Int64,roleAuthorEntity.RoleID);
            db.AddInParameter(cmd,"@AuthorID",DbType.Int64,roleAuthorEntity.AuthorID);

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
     
        public bool UpdateRoleAuthor(RoleAuthorEntity roleAuthorEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  MapID=@MapID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" JournalID=@JournalID");
            sqlCommandText.Append(", RoleID=@RoleID");
            sqlCommandText.Append(", AuthorID=@AuthorID");
            sqlCommandText.Append(", AddDate=@AddDate");
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.RoleAuthor SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@MapID",DbType.Int64,roleAuthorEntity.MapID);
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,roleAuthorEntity.JournalID);
            db.AddInParameter(cmd,"@RoleID",DbType.Int64,roleAuthorEntity.RoleID);
            db.AddInParameter(cmd,"@AuthorID",DbType.Int64,roleAuthorEntity.AuthorID);
            db.AddInParameter(cmd,"@AddDate",DbType.DateTime,roleAuthorEntity.AddDate);

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
        
        public bool DeleteRoleAuthor(RoleAuthorEntity roleAuthorEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.RoleAuthor");
            sqlCommandText.Append(" WHERE  RoleID=@RoleID AND AuthorID=@AuthorID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            db.AddInParameter(cmd, "@RoleID", DbType.Int64, roleAuthorEntity.RoleID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, roleAuthorEntity.AuthorID);

            
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
        
        public bool DeleteRoleAuthor(Int64 mapID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.RoleAuthor");
            sqlCommandText.Append(" WHERE  MapID=@MapID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@MapID",DbType.Int64,mapID);
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
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteRoleAuthor(Int64[] mapID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from RoleAuthor where ");
           
            for(int i=0;i<mapID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( MapID=@MapID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<mapID.Length;i++)
            {
            db.AddInParameter(cmd,"@MapID"+i,DbType.Int64,mapID[i]);
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
        
        public RoleAuthorEntity MakeRoleAuthor(IDataReader dr)
        {
            RoleAuthorEntity roleAuthorEntity = null;
            if(dr.Read())
            {
             roleAuthorEntity=new RoleAuthorEntity();
             roleAuthorEntity.MapID = (Int64)dr["MapID"];
             roleAuthorEntity.JournalID = (Int64)dr["JournalID"];
             roleAuthorEntity.RoleID = (Int64)dr["RoleID"];
             roleAuthorEntity.AuthorID = (Int64)dr["AuthorID"];
             roleAuthorEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return roleAuthorEntity;
        }
        public RoleAuthorEntity MakeRoleAuthorDetail(IDataReader dr)
        {
            RoleAuthorEntity roleAuthorEntity = null;
            if (dr.Read())
            {
                roleAuthorEntity = new RoleAuthorEntity();
                roleAuthorEntity.MapID = (Int64)dr["MapID"];
                roleAuthorEntity.JournalID = (Int64)dr["JournalID"];
                roleAuthorEntity.RoleID = (Int64)dr["RoleID"];
                roleAuthorEntity.RoleName = (String)dr["RoleName"];
                roleAuthorEntity.AuthorID = (Int64)dr["AuthorID"];
                roleAuthorEntity.RealName = (String)dr["RealName"];
                roleAuthorEntity.LoginName = (String)dr["LoginName"];
                roleAuthorEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return roleAuthorEntity;
        }
        
        public RoleAuthorEntity MakeRoleAuthor(DataRow dr)
        {
            RoleAuthorEntity roleAuthorEntity=null;
            if(dr!=null)
            {
                 roleAuthorEntity=new RoleAuthorEntity();
                 roleAuthorEntity.MapID = (Int64)dr["MapID"];
                 roleAuthorEntity.JournalID = (Int64)dr["JournalID"];
                 roleAuthorEntity.RoleID = (Int64)dr["RoleID"];
                 roleAuthorEntity.AuthorID = (Int64)dr["AuthorID"];
                 roleAuthorEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return roleAuthorEntity;
        }
        public RoleAuthorEntity MakeRoleAuthorDetail(DataRow dr)
        {
            RoleAuthorEntity roleAuthorEntity = null;
            if (dr != null)
            {
                roleAuthorEntity = new RoleAuthorEntity();
                roleAuthorEntity.MapID = (Int64)dr["MapID"];
                roleAuthorEntity.JournalID = (Int64)dr["JournalID"];
                roleAuthorEntity.RoleID = (Int64)dr["RoleID"];
                roleAuthorEntity.RoleName = (String)dr["RoleName"];
                roleAuthorEntity.AuthorID = (Int64)dr["AuthorID"];
                roleAuthorEntity.RealName = (String)dr["RealName"];
                roleAuthorEntity.LoginName = (String)dr["LoginName"];
                roleAuthorEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return roleAuthorEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<RoleAuthorEntity> MakeRoleAuthorList(IDataReader dr)
        {
            List<RoleAuthorEntity> list=new List<RoleAuthorEntity>();
            while(dr.Read())
            {
             RoleAuthorEntity roleAuthorEntity=new RoleAuthorEntity();
            roleAuthorEntity.MapID = (Int64)dr["MapID"];
            roleAuthorEntity.JournalID = (Int64)dr["JournalID"];
            roleAuthorEntity.RoleID = (Int64)dr["RoleID"];
            roleAuthorEntity.AuthorID = (Int64)dr["AuthorID"];
            roleAuthorEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(roleAuthorEntity);
            }
            dr.Close();
            return list;
        }

        public List<RoleAuthorEntity> MakeRoleAuthorDetailList(IDataReader dr)
        {
            List<RoleAuthorEntity> list = new List<RoleAuthorEntity>();
            while (dr.Read())
            {
                RoleAuthorEntity roleAuthorEntity = new RoleAuthorEntity();
                roleAuthorEntity.MapID = (Int64)dr["MapID"];
                roleAuthorEntity.JournalID = (Int64)dr["JournalID"];
                roleAuthorEntity.RoleID = (Int64)dr["RoleID"];
                roleAuthorEntity.RoleName = dr["RoleName"]==System.DBNull.Value?"":(String)dr["RoleName"];
                roleAuthorEntity.AuthorID = (Int64)dr["AuthorID"];
                roleAuthorEntity.RealName = (String)dr["RealName"];
                roleAuthorEntity.LoginName = (String)dr["LoginName"];
                roleAuthorEntity.AddDate = (DateTime)dr["AddDate"];
                list.Add(roleAuthorEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<RoleAuthorEntity> MakeRoleAuthorList(DataTable dt)
        {
            List<RoleAuthorEntity> list=new List<RoleAuthorEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   RoleAuthorEntity roleAuthorEntity=MakeRoleAuthor(dt.Rows[i]);
                   list.Add(roleAuthorEntity);
                }
            }
            return list;
        }
        public List<RoleAuthorEntity> MakeRoleAuthorDetailList(DataTable dt)
        {
            List<RoleAuthorEntity> list = new List<RoleAuthorEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    RoleAuthorEntity roleAuthorEntity = MakeRoleAuthorDetail(dt.Rows[i]);
                    list.Add(roleAuthorEntity);
                }
            }
            return list;
        }
        
        #endregion

    }
}

