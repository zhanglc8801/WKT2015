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
    public partial class RoleInfoDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public RoleInfoDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static RoleInfoDataAccess _instance = new RoleInfoDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static RoleInfoDataAccess Instance
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
        public string RoleInfoQueryToSQLWhere(RoleInfoQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JournalID = " + query.JournalID);
            if (query.RoleID!= null)
            {
                sbWhere.Append(" AND RoleID = " + query.RoleID);
            }
            else
            {
                if (query.GroupID != null)
                {
                    sbWhere.Append(" AND GroupID = ").Append(query.GroupID.Value);
                }
            }
            return sbWhere.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string RoleInfoQueryToSQLOrder(RoleInfoQuery query)
        {
            return " RoleID DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public RoleInfoEntity GetRoleInfo(Int64 roleID)
        {
            RoleInfoEntity roleInfoEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  RoleID,JournalID,RoleName,Note,GroupID,AddDate FROM dbo.RoleInfo WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  RoleID=@RoleID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@RoleID",DbType.Int64,roleID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                roleInfoEntity = MakeRoleInfo(dr);
            }
            return roleInfoEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<RoleInfoEntity> GetRoleInfoList()
        {
            List<RoleInfoEntity> roleInfoEntity=new List<RoleInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  RoleID,JournalID,RoleName,Note,GroupID,AddDate FROM dbo.RoleInfo WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                roleInfoEntity = MakeRoleInfoList(dr);
            }
            return roleInfoEntity;
        }
        
        public List<RoleInfoEntity> GetRoleInfoList(RoleInfoQuery query)
        {
            List<RoleInfoEntity> list = new List<RoleInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT RoleID,JournalID,RoleName,Note,GroupID,AddDate FROM dbo.RoleInfo WITH(NOLOCK)");
            string whereSQL = RoleInfoQueryToSQLWhere(query);
            string orderBy=RoleInfoQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeRoleInfoList(dr);
            }
            return list;
        }

        /// <summary>
        /// 得到角色Dict
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long,string> GetRoleInfoDict(RoleInfoQuery query)
        {
            IDictionary<long, string> dictRole = new Dictionary<long, string>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT RoleID,RoleName FROM dbo.RoleInfo WITH(NOLOCK)");
            string whereSQL = RoleInfoQueryToSQLWhere(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            long RoleID = 0;
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    RoleID = WKT.Common.Utils.TypeParse.ToLong(dr["RoleID"],0);
                    if (!dictRole.ContainsKey(RoleID))
                    {
                        dictRole.Add(RoleID, dr["RoleName"].ToString());
                    }
                }
                dr.Close();
            }
            return dictRole;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<RoleInfoEntity></returns>
        public Pager<RoleInfoEntity> GetRoleInfoPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("RoleInfo","RoleID,JournalID,RoleName,Note,GroupID,AddDate",query.Order,query.Where,query.CurrentPage,query.PageSize,out recordCount);
            Pager<RoleInfoEntity>  pager = new Pager<RoleInfoEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeRoleInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<RoleInfoEntity> GetRoleInfoPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("RoleInfo","RoleID,JournalID,RoleName,Note,GroupID,AddDate"," RoleID DESC","",query.CurrentPage,query.PageSize,out recordCount);
            Pager<RoleInfoEntity>  pager=new Pager<RoleInfoEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeRoleInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<RoleInfoEntity> GetRoleInfoPageList(RoleInfoQuery query)
        {
            int recordCount=0;
            string whereSQL=RoleInfoQueryToSQLWhere(query);
            string orderBy=RoleInfoQueryToSQLOrder(query);
            DataSet ds=db.GetPagingData("RoleInfo","RoleID,JournalID,RoleName,Note,GroupID,AddDate",orderBy,whereSQL,query.CurrentPage,query.PageSize,out recordCount);
            Pager<RoleInfoEntity>  pager=new Pager<RoleInfoEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeRoleInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddRoleInfo(RoleInfoEntity roleInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("@JournalID");
            sqlCommandText.Append(", @RoleName");
            sqlCommandText.Append(", @GroupID");
            sqlCommandText.Append(", @Note");
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.RoleInfo ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
            
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,roleInfoEntity.JournalID);
            db.AddInParameter(cmd, "@GroupID", DbType.Int16, roleInfoEntity.GroupID);
            db.AddInParameter(cmd,"@RoleName",DbType.AnsiString,roleInfoEntity.RoleName);
            db.AddInParameter(cmd,"@Note",DbType.AnsiString,roleInfoEntity.Note);

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
     
        public bool UpdateRoleInfo(RoleInfoEntity roleInfoEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  RoleID=@RoleID AND JournalID=@JournalID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("RoleName=@RoleName");
            sqlCommandText.Append(", Note=@Note");
            sqlCommandText.Append(", GroupID=@GroupID");
           
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.RoleInfo SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@RoleID",DbType.Int64,roleInfoEntity.RoleID);
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,roleInfoEntity.JournalID);
            db.AddInParameter(cmd,"@RoleName",DbType.AnsiString,roleInfoEntity.RoleName);
            db.AddInParameter(cmd,"@Note",DbType.AnsiString,roleInfoEntity.Note);
            db.AddInParameter(cmd,"@GroupID",DbType.Byte,roleInfoEntity.GroupID);

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
        
        public bool DeleteRoleInfo(RoleInfoEntity roleInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.RoleInfo");
            sqlCommandText.Append(" WHERE  RoleID=@RoleID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@RoleID",DbType.Int64,roleInfoEntity.RoleID);
            
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
        
        public bool DeleteRoleInfo(Int64 roleID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.RoleInfo");
            sqlCommandText.Append(" WHERE  RoleID=@RoleID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@RoleID",DbType.Int64,roleID);
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
        /// <param name="roleID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteRoleInfo(RoleInfoQuery queryRole)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.AppendFormat("DELETE FROM dbo.RoleInfo WHERE JournalID=@JournalID AND RoleID IN ({0})", string.Join(",", queryRole.RoleIDList));
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, queryRole.JournalID);

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
        
        public RoleInfoEntity MakeRoleInfo(IDataReader dr)
        {
            RoleInfoEntity roleInfoEntity = null;
            if(dr.Read())
            {
             roleInfoEntity=new RoleInfoEntity();
             roleInfoEntity.RoleID = (Int64)dr["RoleID"];
             roleInfoEntity.JournalID = (Int64)dr["JournalID"];
             roleInfoEntity.RoleName = (String)dr["RoleName"];
             roleInfoEntity.Note = (String)dr["Note"];
             roleInfoEntity.GroupID = (Byte)dr["GroupID"];
             roleInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return roleInfoEntity;
        }
        
        public RoleInfoEntity MakeRoleInfo(DataRow dr)
        {
            RoleInfoEntity roleInfoEntity=null;
            if(dr!=null)
            {
                 roleInfoEntity=new RoleInfoEntity();
                 roleInfoEntity.RoleID = (Int64)dr["RoleID"];
                 roleInfoEntity.JournalID = (Int64)dr["JournalID"];
                 roleInfoEntity.RoleName = (String)dr["RoleName"];
                 roleInfoEntity.Note = (String)dr["Note"];
                 roleInfoEntity.GroupID = (Byte)dr["GroupID"];
                 roleInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return roleInfoEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<RoleInfoEntity> MakeRoleInfoList(IDataReader dr)
        {
            List<RoleInfoEntity> list=new List<RoleInfoEntity>();
            while (dr.Read())
            {
                RoleInfoEntity roleInfoEntity = new RoleInfoEntity();
                roleInfoEntity.RoleID = (Int64)dr["RoleID"];
                roleInfoEntity.JournalID = (Int64)dr["JournalID"];
                roleInfoEntity.RoleName = (String)dr["RoleName"];
                roleInfoEntity.Note = (String)dr["Note"];
                roleInfoEntity.GroupID = (Byte)dr["GroupID"];
                roleInfoEntity.AddDate = (DateTime)dr["AddDate"];
                list.Add(roleInfoEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<RoleInfoEntity> MakeRoleInfoList(DataTable dt)
        {
            List<RoleInfoEntity> list=new List<RoleInfoEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   RoleInfoEntity roleInfoEntity=MakeRoleInfo(dt.Rows[i]);
                   list.Add(roleInfoEntity);
                }
            }
            return list;
        }
        
        #endregion

    }
}

