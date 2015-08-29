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
    public partial class SiteChannelDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public SiteChannelDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static SiteChannelDataAccess _instance = new SiteChannelDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static SiteChannelDataAccess Instance
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
        public string SiteChannelQueryToSQLWhere(SiteChannelQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JournalID=" + query.JournalID);
            if (query.ChannelID != null && query.ChannelID > 0)
            {
                sbWhere.Append(" AND ChannelID = ").Append(query.ChannelID.Value);
            }
            if (query.Status != null)
            {
                sbWhere.Append(" AND Status = ").Append(query.Status.Value);
            }
            if (query.IsNav != null)
            {
                sbWhere.Append(" AND IsNav = ").Append(query.IsNav.Value);
            }
            if (!string.IsNullOrEmpty(query.ChannelUrl))
            {
                sbWhere.Append(" AND ChannelUrl = '").Append(WKT.Common.Security.SecurityUtils.SafeSqlString(query.ChannelUrl)).Append("'");
            }
            return sbWhere.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string SiteChannelQueryToSQLOrder(SiteChannelQuery query)
        {
            return " SortID ASC,ChannelID DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象

        public SiteChannelEntity GetSiteChannel(SiteChannelQuery query)
        {
            SiteChannelEntity siteChannelEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  ChannelID,JournalID,PChannelID,ContentType,IsNav,Keywords,Description,ChannelUrl,SortID,Status,AddDate FROM dbo.SiteChannel WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE " + SiteChannelQueryToSQLWhere(query));
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                siteChannelEntity = MakeSiteChannel(dr);
            }
            return siteChannelEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<SiteChannelEntity> GetSiteChannelList()
        {
            List<SiteChannelEntity> siteChannelEntity=new List<SiteChannelEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  ChannelID,JournalID,PChannelID,ContentType,IsNav,Keywords,Description,ChannelUrl,SortID,Status,AddDate FROM dbo.SiteChannel WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                siteChannelEntity = MakeSiteChannelList(dr);
            }
            return siteChannelEntity;
        }
        
        public List<SiteChannelEntity> GetSiteChannelList(SiteChannelQuery query)
        {
            List<SiteChannelEntity> list = new List<SiteChannelEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT ChannelID,JournalID,PChannelID,ContentType,IsNav,Keywords,Description,ChannelUrl,SortID,Status,AddDate FROM dbo.SiteChannel WITH(NOLOCK)");
            string whereSQL = SiteChannelQueryToSQLWhere(query);
            string orderBy=SiteChannelQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeSiteChannelList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<SiteChannelEntity></returns>
        public Pager<SiteChannelEntity> GetSiteChannelPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("SiteChannel", "ChannelID,JournalID,PChannelID,ContentType,IsNav,Keywords,Description,ChannelUrl,SortID,Status,AddDate", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<SiteChannelEntity>  pager = new Pager<SiteChannelEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeSiteChannelList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<SiteChannelEntity> GetSiteChannelPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("SiteChannel", "ChannelID,JournalID,PChannelID,ContentType,IsNav,Keywords,Description,ChannelUrl,SortID,Status,AddDate", " ChannelID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<SiteChannelEntity>  pager=new Pager<SiteChannelEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeSiteChannelList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<SiteChannelEntity> GetSiteChannelPageList(SiteChannelQuery query)
        {
            int recordCount=0;
            string whereSQL=SiteChannelQueryToSQLWhere(query);
            string orderBy=SiteChannelQueryToSQLOrder(query);
            DataSet ds = db.GetPagingData("SiteChannel", "ChannelID,JournalID,PChannelID,ContentType,IsNav,Keywords,Description,ChannelUrl,SortID,Status,AddDate", orderBy, whereSQL, query.CurrentPage, query.PageSize, out recordCount);
            Pager<SiteChannelEntity>  pager=new Pager<SiteChannelEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeSiteChannelList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddSiteChannel(SiteChannelEntity siteChannelEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @PChannelID");
            sqlCommandText.Append(", @ContentType");
            sqlCommandText.Append(", @IsNav");
            sqlCommandText.Append(", @Keywords");
            sqlCommandText.Append(", @Description");
            sqlCommandText.Append(", @ChannelUrl");
            sqlCommandText.Append(", @SortID");
            sqlCommandText.Append(", @Status");            
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.SiteChannel ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
           
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,siteChannelEntity.JournalID);
            db.AddInParameter(cmd,"@PChannelID", DbType.Int64, siteChannelEntity.PChannelID);
            db.AddInParameter(cmd,"@ContentType",DbType.Byte,siteChannelEntity.ContentType);
            db.AddInParameter(cmd,"@IsNav", DbType.Byte, siteChannelEntity.IsNav);
            db.AddInParameter(cmd,"@Keywords",DbType.AnsiString,siteChannelEntity.Keywords);
            db.AddInParameter(cmd,"@Description",DbType.AnsiString,siteChannelEntity.Description);
            db.AddInParameter(cmd,"@ChannelUrl",DbType.AnsiString,siteChannelEntity.ChannelUrl);
            db.AddInParameter(cmd,"@SortID",DbType.Int32,siteChannelEntity.SortID);
            db.AddInParameter(cmd,"@Status",DbType.Byte,siteChannelEntity.Status);           
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
     
        public bool UpdateSiteChannel(SiteChannelEntity siteChannelEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  ChannelID=@ChannelID");
            StringBuilder sqlCommandText = new StringBuilder();          
            sqlCommandText.Append(" ContentType=@ContentType");
            sqlCommandText.Append(", IsNav=@IsNav");
            sqlCommandText.Append(", Keywords=@Keywords");
            sqlCommandText.Append(", Description=@Description");
            sqlCommandText.Append(", ChannelUrl=@ChannelUrl");
            sqlCommandText.Append(", SortID=@SortID");
            sqlCommandText.Append(", Status=@Status");          
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.SiteChannel SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@ChannelID",DbType.Int64,siteChannelEntity.ChannelID);           
            db.AddInParameter(cmd,"@ContentType",DbType.Byte,siteChannelEntity.ContentType);
            db.AddInParameter(cmd,"@IsNav", DbType.Byte, siteChannelEntity.IsNav);
            db.AddInParameter(cmd,"@Keywords",DbType.AnsiString,siteChannelEntity.Keywords);
            db.AddInParameter(cmd,"@Description",DbType.AnsiString,siteChannelEntity.Description);
            db.AddInParameter(cmd,"@ChannelUrl",DbType.AnsiString,siteChannelEntity.ChannelUrl);
            db.AddInParameter(cmd,"@SortID",DbType.Int32,siteChannelEntity.SortID);
            db.AddInParameter(cmd,"@Status",DbType.Byte,siteChannelEntity.Status);
           
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
        
        public bool DeleteSiteChannel(SiteChannelEntity siteChannelEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.SiteChannel");
            sqlCommandText.Append(" WHERE  ChannelID=@ChannelID and not exists(select 1 from dbo.SiteChannel b with(nolock) where dbo.SiteChannel.ChannelID=b.PChannelID)");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@ChannelID",DbType.Int64,siteChannelEntity.ChannelID);
            
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
        
        public bool DeleteSiteChannel(Int64 channelID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.SiteChannel");
            sqlCommandText.Append(" WHERE  ChannelID=@ChannelID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@ChannelID",DbType.Int64,channelID);
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
        /// <param name="channelID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteChannel(Int64[] channelID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from SiteChannel where ");
           
            for(int i=0;i<channelID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( ChannelID=@ChannelID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<channelID.Length;i++)
            {
            db.AddInParameter(cmd,"@ChannelID"+i,DbType.Int64,channelID[i]);
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
        
        public SiteChannelEntity MakeSiteChannel(IDataReader dr)
        {
            SiteChannelEntity siteChannelEntity = null;
            if(dr.Read())
            {
             siteChannelEntity=new SiteChannelEntity();
             siteChannelEntity.ChannelID = (Int64)dr["ChannelID"];
             siteChannelEntity.JournalID = (Int64)dr["JournalID"];
             siteChannelEntity.PChannelID = (Int64)dr["PChannelID"];
             siteChannelEntity.ContentType = (Byte)dr["ContentType"];
             siteChannelEntity.IsNav = (Byte)dr["IsNav"];
             siteChannelEntity.Keywords = (String)dr["Keywords"];
             siteChannelEntity.Description = (String)dr["Description"];
             siteChannelEntity.ChannelUrl = (String)dr["ChannelUrl"];
             siteChannelEntity.SortID = (Int32)dr["SortID"];
             siteChannelEntity.Status = (Byte)dr["Status"];
             siteChannelEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return siteChannelEntity;
        }
        
        public SiteChannelEntity MakeSiteChannel(DataRow dr)
        {
            SiteChannelEntity siteChannelEntity=null;
            if(dr!=null)
            {
                 siteChannelEntity=new SiteChannelEntity();
                 siteChannelEntity.ChannelID = (Int64)dr["ChannelID"];
                 siteChannelEntity.JournalID = (Int64)dr["JournalID"];
                 siteChannelEntity.PChannelID = (Int64)dr["PChannelID"];
                 siteChannelEntity.ContentType = (Byte)dr["ContentType"];
                 siteChannelEntity.IsNav = (Byte)dr["IsNav"];
                 siteChannelEntity.Keywords = (String)dr["Keywords"];
                 siteChannelEntity.Description = (String)dr["Description"];
                 siteChannelEntity.ChannelUrl = (String)dr["ChannelUrl"];
                 siteChannelEntity.SortID = (Int32)dr["SortID"];
                 siteChannelEntity.Status = (Byte)dr["Status"];
                 siteChannelEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return siteChannelEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<SiteChannelEntity> MakeSiteChannelList(IDataReader dr)
        {
            List<SiteChannelEntity> list=new List<SiteChannelEntity>();
            while(dr.Read())
            {
             SiteChannelEntity siteChannelEntity=new SiteChannelEntity();
            siteChannelEntity.ChannelID = (Int64)dr["ChannelID"];
            siteChannelEntity.JournalID = (Int64)dr["JournalID"];
            siteChannelEntity.PChannelID = (Int64)dr["PChannelID"];
            siteChannelEntity.ContentType = (Byte)dr["ContentType"];
            siteChannelEntity.IsNav = (Byte)dr["IsNav"];
            siteChannelEntity.Keywords = (String)dr["Keywords"];
            siteChannelEntity.Description = (String)dr["Description"];
            siteChannelEntity.ChannelUrl = (String)dr["ChannelUrl"];
            siteChannelEntity.SortID = (Int32)dr["SortID"];
            siteChannelEntity.Status = (Byte)dr["Status"];
            siteChannelEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(siteChannelEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<SiteChannelEntity> MakeSiteChannelList(DataTable dt)
        {
            List<SiteChannelEntity> list=new List<SiteChannelEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   SiteChannelEntity siteChannelEntity=MakeSiteChannel(dt.Rows[i]);
                   list.Add(siteChannelEntity);
                }
            }
            return list;
        }
        
        #endregion
    }
}

