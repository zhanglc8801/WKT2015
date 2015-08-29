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
    public partial class SiteMessageDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public SiteMessageDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static SiteMessageDataAccess _instance = new SiteMessageDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static SiteMessageDataAccess Instance
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
        public string SiteMessageQueryToSQLWhere(SiteMessageQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID=" + query.JournalID);            
            if (query.IsView != null)
                strFilter.Append(" and IsView=").Append(query.IsView.Value);
            if (query.CID != null)
                strFilter.Append(" and CID=").Append(query.CID.Value);
            if (query.IsUserRelevant)
                strFilter.AppendFormat(" and (SendUser={0} or ReciverID={1})", query.SendUser.Value, query.ReciverID.Value);
            else
            {
                if (query.SendUser != null)
                    strFilter.Append(" and SendUser=").Append(query.SendUser.Value);
                if (query.ReciverID != null)
                    strFilter.Append(" and ReciverID=").Append(query.ReciverID.Value);
            }
            return strFilter.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string SiteMessageQueryToSQLOrder(SiteMessageQuery query)
        {
            return " SendDate DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public SiteMessageEntity GetSiteMessage(Int64 messageID)
        {
            SiteMessageEntity siteMessageEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  * FROM dbo.SiteMessage WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  MessageID=@MessageID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@MessageID",DbType.Int64,messageID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                siteMessageEntity = MakeSiteMessage(dr);
            }
            return siteMessageEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<SiteMessageEntity> GetSiteMessageList()
        {
            List<SiteMessageEntity> siteMessageEntity=new List<SiteMessageEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  * FROM dbo.SiteMessage WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                siteMessageEntity = MakeSiteMessageList(dr);
            }
            return siteMessageEntity;
        }
        
        public List<SiteMessageEntity> GetSiteMessageList(SiteMessageQuery query)
        {
            List<SiteMessageEntity> list = new List<SiteMessageEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT * FROM dbo.SiteMessage WITH(NOLOCK)");
            string whereSQL = SiteMessageQueryToSQLWhere(query);
            string orderBy=SiteMessageQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeSiteMessageList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<SiteMessageEntity></returns>
        public Pager<SiteMessageEntity> GetSiteMessagePageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("SiteMessage", "*", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<SiteMessageEntity>  pager = new Pager<SiteMessageEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeSiteMessageList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<SiteMessageEntity> GetSiteMessagePageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("SiteMessage", "*", " MessageID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<SiteMessageEntity>  pager=new Pager<SiteMessageEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeSiteMessageList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<SiteMessageEntity> GetSiteMessagePageList(SiteMessageQuery query)
        {
            int recordCount=0;
            string whereSQL=SiteMessageQueryToSQLWhere(query);
            string orderBy=SiteMessageQueryToSQLOrder(query);
            DataSet ds = db.GetPagingData("SiteMessage", "*", orderBy, whereSQL, query.CurrentPage, query.PageSize, out recordCount);
            Pager<SiteMessageEntity>  pager=new Pager<SiteMessageEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeSiteMessageList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddSiteMessage(SiteMessageEntity siteMessageEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @CID");
            sqlCommandText.Append(", @SendUser");
            sqlCommandText.Append(", @ReciverID");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @IsView");
            sqlCommandText.Append(", @Content");           

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.SiteMessage ({0},SendDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));
            
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,siteMessageEntity.JournalID);
            db.AddInParameter(cmd, "@CID", DbType.Int64, siteMessageEntity.CID);
            db.AddInParameter(cmd,"@SendUser",DbType.Int64,siteMessageEntity.SendUser);
            db.AddInParameter(cmd,"@ReciverID",DbType.Int64,siteMessageEntity.ReciverID);
            db.AddInParameter(cmd,"@Title",DbType.AnsiString,siteMessageEntity.Title);
            db.AddInParameter(cmd, "@IsView", DbType.Byte, siteMessageEntity.IsView);
            db.AddInParameter(cmd,"@Content",DbType.AnsiString,siteMessageEntity.Content);          
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
     
        public bool UpdateSiteMessage(SiteMessageEntity siteMessageEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  MessageID=@MessageID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Title=@Title");
            sqlCommandText.Append(", Content=@Content");           
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.SiteMessage SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@MessageID",DbType.Int64,siteMessageEntity.MessageID);           
            db.AddInParameter(cmd,"@Title",DbType.AnsiString,siteMessageEntity.Title);
            db.AddInParameter(cmd,"@Content",DbType.AnsiString,siteMessageEntity.Content);           
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
        
        public bool DeleteSiteMessage(SiteMessageEntity siteMessageEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.SiteMessage");
            sqlCommandText.Append(" WHERE  MessageID=@MessageID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@MessageID",DbType.Int64,siteMessageEntity.MessageID);
            
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
        
        public bool DeleteSiteMessage(Int64 messageID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.SiteMessage");
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
        public bool BatchDeleteSiteMessage(Int64[] messageID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from SiteMessage where ");
           
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
        
        public SiteMessageEntity MakeSiteMessage(IDataReader dr)
        {
            SiteMessageEntity siteMessageEntity = null;
            if(dr.Read())
            {
             siteMessageEntity=new SiteMessageEntity();
             siteMessageEntity.MessageID = (Int64)dr["MessageID"];
             siteMessageEntity.JournalID = (Int64)dr["JournalID"];
             siteMessageEntity.CID = (Int64)dr["CID"];
             siteMessageEntity.IsView = (Byte)dr["IsView"];
             siteMessageEntity.SendUser = (Int64)dr["SendUser"];
             siteMessageEntity.ReciverID = (Int64)dr["ReciverID"];
             siteMessageEntity.Title = (String)dr["Title"];
             siteMessageEntity.Content = (String)dr["Content"];
             siteMessageEntity.SendDate = (DateTime)dr["SendDate"];
            }
            dr.Close();
            return siteMessageEntity;
        }
        
        public SiteMessageEntity MakeSiteMessage(DataRow dr)
        {
            SiteMessageEntity siteMessageEntity=null;
            if(dr!=null)
            {
                 siteMessageEntity=new SiteMessageEntity();
                 siteMessageEntity.MessageID = (Int64)dr["MessageID"];
                 siteMessageEntity.JournalID = (Int64)dr["JournalID"];
                 siteMessageEntity.CID = (Int64)dr["CID"];
                 siteMessageEntity.IsView = (Byte)dr["IsView"];
                 siteMessageEntity.SendUser = (Int64)dr["SendUser"];
                 siteMessageEntity.ReciverID = (Int64)dr["ReciverID"];
                 siteMessageEntity.Title = (String)dr["Title"];
                 siteMessageEntity.Content = (String)dr["Content"];
                 siteMessageEntity.SendDate = (DateTime)dr["SendDate"];
            }
            return siteMessageEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<SiteMessageEntity> MakeSiteMessageList(IDataReader dr)
        {
            List<SiteMessageEntity> list=new List<SiteMessageEntity>();
            while(dr.Read())
            {
             SiteMessageEntity siteMessageEntity=new SiteMessageEntity();
            siteMessageEntity.MessageID = (Int64)dr["MessageID"];
            siteMessageEntity.JournalID = (Int64)dr["JournalID"];
            siteMessageEntity.CID = (Int64)dr["CID"];
            siteMessageEntity.IsView = (Byte)dr["IsView"];
            siteMessageEntity.SendUser = (Int64)dr["SendUser"];
            siteMessageEntity.ReciverID = (Int64)dr["ReciverID"];
            siteMessageEntity.Title = (String)dr["Title"];
            siteMessageEntity.Content = (String)dr["Content"];
            siteMessageEntity.SendDate = (DateTime)dr["SendDate"];
               list.Add(siteMessageEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<SiteMessageEntity> MakeSiteMessageList(DataTable dt)
        {
            List<SiteMessageEntity> list=new List<SiteMessageEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   SiteMessageEntity siteMessageEntity=MakeSiteMessage(dt.Rows[i]);
                   list.Add(siteMessageEntity);
                }
            }
            return list;
        }
        
        #endregion

        /// <summary>
        /// 修改消息为已查看
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        public bool UpdateMsgViewed(Int64 JournalID, Int64 MessageID)
        {
            string strSql = string.Format("UPDATE dbo.SiteMessage SET IsView=1 WHERE JournalID={0} and MessageID={1}", JournalID, MessageID);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(cmd) > 0;
        }

    }
}

