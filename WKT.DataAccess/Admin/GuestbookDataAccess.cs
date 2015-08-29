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
    public partial class GuestbookDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public GuestbookDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static GuestbookDataAccess _instance = new GuestbookDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static GuestbookDataAccess Instance
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
        public string GuestbookQueryToSQLWhere(GuestbookQuery query)
        {
            return string.Empty;
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string GuestbookQueryToSQLOrder(GuestbookQuery query)
        {
            return " MessageID DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public GuestbookEntity GetGuestbook(Int64 messageID)
        {
            GuestbookEntity guestbookEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  MessageID,JournalID,PMessageID,UserName,Email,Tel,Title,MessageContent,IP,AddDate FROM dbo.Guestbook WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  MessageID=@MessageID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@MessageID",DbType.Int64,messageID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                guestbookEntity = MakeGuestbook(dr);
            }
            return guestbookEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<GuestbookEntity> GetGuestbookList()
        {
            List<GuestbookEntity> guestbookEntity=new List<GuestbookEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  MessageID,JournalID,PMessageID,UserName,Email,Tel,Title,MessageContent,IP,AddDate FROM dbo.Guestbook WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                guestbookEntity = MakeGuestbookList(dr);
            }
            return guestbookEntity;
        }
        
        public List<GuestbookEntity> GetGuestbookList(GuestbookQuery query)
        {
            List<GuestbookEntity> list = new List<GuestbookEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT MessageID,JournalID,PMessageID,UserName,Email,Tel,Title,MessageContent,IP,AddDate FROM dbo.Guestbook WITH(NOLOCK)");
            string whereSQL = GuestbookQueryToSQLWhere(query);
            string orderBy=GuestbookQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeGuestbookList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<GuestbookEntity></returns>
        public Pager<GuestbookEntity> GetGuestbookPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("Guestbook","MessageID,JournalID,PMessageID,UserName,Email,Tel,Title,MessageContent,IP,AddDate",query.Order,query.Where,query.CurrentPage,query.PageSize,out recordCount);
            Pager<GuestbookEntity>  pager = new Pager<GuestbookEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeGuestbookList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<GuestbookEntity> GetGuestbookPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("Guestbook","MessageID,JournalID,PMessageID,UserName,Email,Tel,Title,MessageContent,IP,AddDate"," MessageID DESC","",query.CurrentPage,query.PageSize,out recordCount);
            Pager<GuestbookEntity>  pager=new Pager<GuestbookEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeGuestbookList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<GuestbookEntity> GetGuestbookPageList(GuestbookQuery query)
        {
            int recordCount=0;
            string whereSQL=GuestbookQueryToSQLWhere(query);
            string orderBy=GuestbookQueryToSQLOrder(query);
            DataSet ds=db.GetPagingData("Guestbook","MessageID,JournalID,PMessageID,UserName,Email,Tel,Title,MessageContent,IP,AddDate",orderBy,whereSQL,query.CurrentPage,query.PageSize,out recordCount);
            Pager<GuestbookEntity>  pager=new Pager<GuestbookEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeGuestbookList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddGuestbook(GuestbookEntity guestbookEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @MessageID");
            sqlCommandText.Append(", @JournalID");
            sqlCommandText.Append(", @PMessageID");
            sqlCommandText.Append(", @UserName");
            sqlCommandText.Append(", @Email");
            sqlCommandText.Append(", @Tel");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @MessageContent");
            sqlCommandText.Append(", @IP");
            sqlCommandText.Append(", @AddDate");
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.Guestbook ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
            
            db.AddInParameter(cmd,"@MessageID",DbType.Int64,guestbookEntity.MessageID);
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,guestbookEntity.JournalID);
            db.AddInParameter(cmd,"@PMessageID",DbType.Int64,guestbookEntity.PMessageID);
            db.AddInParameter(cmd,"@UserName",DbType.AnsiString,guestbookEntity.UserName);
            db.AddInParameter(cmd,"@Email",DbType.AnsiString,guestbookEntity.Email);
            db.AddInParameter(cmd,"@Tel",DbType.AnsiString,guestbookEntity.Tel);
            db.AddInParameter(cmd,"@Title",DbType.AnsiString,guestbookEntity.Title);
            db.AddInParameter(cmd,"@MessageContent",DbType.AnsiString,guestbookEntity.MessageContent);
            db.AddInParameter(cmd,"@IP",DbType.AnsiString,guestbookEntity.IP);
            db.AddInParameter(cmd,"@AddDate",DbType.DateTime,guestbookEntity.AddDate);
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
     
        public bool UpdateGuestbook(GuestbookEntity guestbookEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  MessageID=@MessageID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" JournalID=@JournalID");
            sqlCommandText.Append(", PMessageID=@PMessageID");
            sqlCommandText.Append(", UserName=@UserName");
            sqlCommandText.Append(", Email=@Email");
            sqlCommandText.Append(", Tel=@Tel");
            sqlCommandText.Append(", Title=@Title");
            sqlCommandText.Append(", MessageContent=@MessageContent");
            sqlCommandText.Append(", IP=@IP");
            sqlCommandText.Append(", AddDate=@AddDate");
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.Guestbook SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@MessageID",DbType.Int64,guestbookEntity.MessageID);
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,guestbookEntity.JournalID);
            db.AddInParameter(cmd,"@PMessageID",DbType.Int64,guestbookEntity.PMessageID);
            db.AddInParameter(cmd,"@UserName",DbType.AnsiString,guestbookEntity.UserName);
            db.AddInParameter(cmd,"@Email",DbType.AnsiString,guestbookEntity.Email);
            db.AddInParameter(cmd,"@Tel",DbType.AnsiString,guestbookEntity.Tel);
            db.AddInParameter(cmd,"@Title",DbType.AnsiString,guestbookEntity.Title);
            db.AddInParameter(cmd,"@MessageContent",DbType.AnsiString,guestbookEntity.MessageContent);
            db.AddInParameter(cmd,"@IP",DbType.AnsiString,guestbookEntity.IP);
            db.AddInParameter(cmd,"@AddDate",DbType.DateTime,guestbookEntity.AddDate);

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
        
        public bool DeleteGuestbook(GuestbookEntity guestbookEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.Guestbook");
            sqlCommandText.Append(" WHERE  MessageID=@MessageID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@MessageID",DbType.Int64,guestbookEntity.MessageID);
            
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
        
        public bool DeleteGuestbook(Int64 messageID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.Guestbook");
            sqlCommandText.Append(" WHERE  MessageID=@MessageID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@MessageID",DbType.Int64,messageID);
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
        /// <param name="messageID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteGuestbook(Int64[] messageID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from Guestbook where ");
           
            for(int i=0;i<messageID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( MessageID=@MessageID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<messageID.Length;i++)
            {
            db.AddInParameter(cmd,"@MessageID"+i,DbType.Int64,messageID[i]);
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
        
        public GuestbookEntity MakeGuestbook(IDataReader dr)
        {
            GuestbookEntity guestbookEntity = null;
            if(dr.Read())
            {
             guestbookEntity=new GuestbookEntity();
             guestbookEntity.MessageID = (Int64)dr["MessageID"];
             guestbookEntity.JournalID = (Int64)dr["JournalID"];
             guestbookEntity.PMessageID = (Int64)dr["PMessageID"];
             guestbookEntity.UserName = (String)dr["UserName"];
             guestbookEntity.Email = (String)dr["Email"];
             guestbookEntity.Tel = (String)dr["Tel"];
             guestbookEntity.Title = (String)dr["Title"];
             guestbookEntity.MessageContent = (String)dr["MessageContent"];
             guestbookEntity.IP = (String)dr["IP"];
             guestbookEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return guestbookEntity;
        }
        
        public GuestbookEntity MakeGuestbook(DataRow dr)
        {
            GuestbookEntity guestbookEntity=null;
            if(dr!=null)
            {
                 guestbookEntity=new GuestbookEntity();
                 guestbookEntity.MessageID = (Int64)dr["MessageID"];
                 guestbookEntity.JournalID = (Int64)dr["JournalID"];
                 guestbookEntity.PMessageID = (Int64)dr["PMessageID"];
                 guestbookEntity.UserName = (String)dr["UserName"];
                 guestbookEntity.Email = (String)dr["Email"];
                 guestbookEntity.Tel = (String)dr["Tel"];
                 guestbookEntity.Title = (String)dr["Title"];
                 guestbookEntity.MessageContent = (String)dr["MessageContent"];
                 guestbookEntity.IP = (String)dr["IP"];
                 guestbookEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return guestbookEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<GuestbookEntity> MakeGuestbookList(IDataReader dr)
        {
            List<GuestbookEntity> list=new List<GuestbookEntity>();
            while(dr.Read())
            {
             GuestbookEntity guestbookEntity=new GuestbookEntity();
            guestbookEntity.MessageID = (Int64)dr["MessageID"];
            guestbookEntity.JournalID = (Int64)dr["JournalID"];
            guestbookEntity.PMessageID = (Int64)dr["PMessageID"];
            guestbookEntity.UserName = (String)dr["UserName"];
            guestbookEntity.Email = (String)dr["Email"];
            guestbookEntity.Tel = (String)dr["Tel"];
            guestbookEntity.Title = (String)dr["Title"];
            guestbookEntity.MessageContent = (String)dr["MessageContent"];
            guestbookEntity.IP = (String)dr["IP"];
            guestbookEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(guestbookEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<GuestbookEntity> MakeGuestbookList(DataTable dt)
        {
            List<GuestbookEntity> list=new List<GuestbookEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   GuestbookEntity guestbookEntity=MakeGuestbook(dt.Rows[i]);
                   list.Add(guestbookEntity);
                }
            }
            return list;
        }
        
        #endregion

    }
}

