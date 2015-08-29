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
    public partial class DBServerInfoDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public DBServerInfoDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTSysDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static DBServerInfoDataAccess _instance = new DBServerInfoDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static DBServerInfoDataAccess Instance
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
        public string DBServerInfoQueryToSQLWhere(DBServerInfoQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            if (query.Status != null)
            {
                sbWhere.Append(" AND Status= ").Append(query.Status.Value);
            }
            if (!string.IsNullOrEmpty(query.Account))
            {
                sbWhere.AppendFormat(" AND Account='{0}'", WKT.Common.Security.SecurityUtils.SafeSqlString(query.Account));
            }
            if (!string.IsNullOrEmpty(query.ServerIP))
            {
                sbWhere.AppendFormat(" AND ServerIP='{0}'", WKT.Common.Security.SecurityUtils.SafeSqlString(query.ServerIP));
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
        public string DBServerInfoQueryToSQLOrder(DBServerInfoQuery query)
        {
            return " DBServerID DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public DBServerInfoEntity GetDBServerInfo(Int32 dBServerID)
        {
            DBServerInfoEntity dBServerInfoEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  DBServerID,ServerIP,Port,Account,Pwd,Note,Status,AddDate FROM dbo.DBServerInfo WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  DBServerID=@DBServerID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@DBServerID",DbType.Int32,dBServerID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                dBServerInfoEntity = MakeDBServerInfo(dr);
            }
            return dBServerInfoEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<DBServerInfoEntity> GetDBServerInfoList()
        {
            List<DBServerInfoEntity> dBServerInfoEntity=new List<DBServerInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  DBServerID,ServerIP,Port,Account,Pwd,Note,Status,AddDate FROM dbo.DBServerInfo WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                dBServerInfoEntity = MakeDBServerInfoList(dr);
            }
            return dBServerInfoEntity;
        }
        
        public List<DBServerInfoEntity> GetDBServerInfoList(DBServerInfoQuery query)
        {
            List<DBServerInfoEntity> list = new List<DBServerInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT DBServerID,ServerIP,Port,Account,Pwd,Note,Status,AddDate FROM dbo.DBServerInfo WITH(NOLOCK)");
            string whereSQL = DBServerInfoQueryToSQLWhere(query);
            string orderBy=DBServerInfoQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeDBServerInfoList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<DBServerInfoEntity></returns>
        public Pager<DBServerInfoEntity> GetDBServerInfoPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("DBServerInfo","DBServerID,ServerIP,Port,Account,Pwd,Note,Status,AddDate",query.Order,query.Where,query.CurrentPage,query.PageSize,out recordCount);
            Pager<DBServerInfoEntity>  pager = new Pager<DBServerInfoEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeDBServerInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<DBServerInfoEntity> GetDBServerInfoPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("DBServerInfo","DBServerID,ServerIP,Port,Account,Pwd,Note,Status,AddDate"," DBServerID DESC","",query.CurrentPage,query.PageSize,out recordCount);
            Pager<DBServerInfoEntity>  pager=new Pager<DBServerInfoEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeDBServerInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<DBServerInfoEntity> GetDBServerInfoPageList(DBServerInfoQuery query)
        {
            int recordCount=0;
            string whereSQL=DBServerInfoQueryToSQLWhere(query);
            string orderBy=DBServerInfoQueryToSQLOrder(query);
            DataSet ds=db.GetPagingData("DBServerInfo","DBServerID,ServerIP,Port,Account,Pwd,Note,Status,AddDate",orderBy,whereSQL,query.CurrentPage,query.PageSize,out recordCount);
            Pager<DBServerInfoEntity>  pager=new Pager<DBServerInfoEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeDBServerInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddDBServerInfo(DBServerInfoEntity dBServerInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("@ServerIP");
            sqlCommandText.Append(", @Port");
            sqlCommandText.Append(", @Account");
            sqlCommandText.Append(", @Pwd");
            sqlCommandText.Append(", @Note");
            sqlCommandText.Append(", @Status");
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.DBServerInfo ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
            
            db.AddInParameter(cmd,"@ServerIP",DbType.AnsiString,dBServerInfoEntity.ServerIP);
            db.AddInParameter(cmd,"@Port",DbType.Int32,dBServerInfoEntity.Port);
            db.AddInParameter(cmd,"@Account",DbType.AnsiString,dBServerInfoEntity.Account);
            db.AddInParameter(cmd,"@Pwd",DbType.AnsiString,dBServerInfoEntity.Pwd);
            db.AddInParameter(cmd,"@Note",DbType.AnsiString,dBServerInfoEntity.Note);
            db.AddInParameter(cmd,"@Status",DbType.Byte,dBServerInfoEntity.Status);
            
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
     
        public bool UpdateDBServerInfo(DBServerInfoEntity dBServerInfoEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  DBServerID=@DBServerID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" ServerIP=@ServerIP");
            sqlCommandText.Append(", Port=@Port");
            sqlCommandText.Append(", Account=@Account");
            sqlCommandText.Append(", Pwd=@Pwd");
            sqlCommandText.Append(", Note=@Note");
            sqlCommandText.Append(", Status=@Status");
            sqlCommandText.Append(", AddDate=@AddDate");
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.DBServerInfo SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@DBServerID",DbType.Int32,dBServerInfoEntity.DBServerID);
            db.AddInParameter(cmd,"@ServerIP",DbType.AnsiString,dBServerInfoEntity.ServerIP);
            db.AddInParameter(cmd,"@Port",DbType.Int32,dBServerInfoEntity.Port);
            db.AddInParameter(cmd,"@Account",DbType.AnsiString,dBServerInfoEntity.Account);
            db.AddInParameter(cmd,"@Pwd",DbType.AnsiString,dBServerInfoEntity.Pwd);
            db.AddInParameter(cmd,"@Note",DbType.AnsiString,dBServerInfoEntity.Note);
            db.AddInParameter(cmd,"@Status",DbType.Byte,dBServerInfoEntity.Status);
            db.AddInParameter(cmd,"@AddDate",DbType.DateTime,dBServerInfoEntity.AddDate);

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
        
        public bool DeleteDBServerInfo(DBServerInfoEntity dBServerInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.DBServerInfo");
            sqlCommandText.Append(" WHERE  DBServerID=@DBServerID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@DBServerID",DbType.Int32,dBServerInfoEntity.DBServerID);
            
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
        
        public bool DeleteDBServerInfo(Int32 dBServerID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.DBServerInfo");
            sqlCommandText.Append(" WHERE  DBServerID=@DBServerID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@DBServerID",DbType.Int32,dBServerID);
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
        /// <param name="dBServerID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteDBServerInfo(Int32[] dBServerID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from DBServerInfo where ");
           
            for(int i=0;i<dBServerID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( DBServerID=@DBServerID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<dBServerID.Length;i++)
            {
            db.AddInParameter(cmd,"@DBServerID"+i,DbType.Int32,dBServerID[i]);
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
        
        public DBServerInfoEntity MakeDBServerInfo(IDataReader dr)
        {
            DBServerInfoEntity dBServerInfoEntity = null;
            if(dr.Read())
            {
             dBServerInfoEntity=new DBServerInfoEntity();
             dBServerInfoEntity.DBServerID = (Int32)dr["DBServerID"];
             dBServerInfoEntity.ServerIP = (String)dr["ServerIP"];
             dBServerInfoEntity.Port = (Int32)dr["Port"];
             dBServerInfoEntity.Account = (String)dr["Account"];
             dBServerInfoEntity.Pwd = (String)dr["Pwd"];
             dBServerInfoEntity.Note = (String)dr["Note"];
             dBServerInfoEntity.Status = (Byte)dr["Status"];
             dBServerInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return dBServerInfoEntity;
        }
        
        public DBServerInfoEntity MakeDBServerInfo(DataRow dr)
        {
            DBServerInfoEntity dBServerInfoEntity=null;
            if(dr!=null)
            {
                 dBServerInfoEntity=new DBServerInfoEntity();
                 dBServerInfoEntity.DBServerID = (Int32)dr["DBServerID"];
                 dBServerInfoEntity.ServerIP = (String)dr["ServerIP"];
                 dBServerInfoEntity.Port = (Int32)dr["Port"];
                 dBServerInfoEntity.Account = (String)dr["Account"];
                 dBServerInfoEntity.Pwd = (String)dr["Pwd"];
                 dBServerInfoEntity.Note = (String)dr["Note"];
                 dBServerInfoEntity.Status = (Byte)dr["Status"];
                 dBServerInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return dBServerInfoEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<DBServerInfoEntity> MakeDBServerInfoList(IDataReader dr)
        {
            List<DBServerInfoEntity> list=new List<DBServerInfoEntity>();
            while(dr.Read())
            {
             DBServerInfoEntity dBServerInfoEntity=new DBServerInfoEntity();
            dBServerInfoEntity.DBServerID = (Int32)dr["DBServerID"];
            dBServerInfoEntity.ServerIP = (String)dr["ServerIP"];
            dBServerInfoEntity.Port = (Int32)dr["Port"];
            dBServerInfoEntity.Account = (String)dr["Account"];
            dBServerInfoEntity.Pwd = (String)dr["Pwd"];
            dBServerInfoEntity.Note = (String)dr["Note"];
            dBServerInfoEntity.Status = (Byte)dr["Status"];
            dBServerInfoEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(dBServerInfoEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<DBServerInfoEntity> MakeDBServerInfoList(DataTable dt)
        {
            List<DBServerInfoEntity> list=new List<DBServerInfoEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   DBServerInfoEntity dBServerInfoEntity=MakeDBServerInfo(dt.Rows[i]);
                   list.Add(dBServerInfoEntity);
                }
            }
            return list;
        }
        
        #endregion

    }
}

