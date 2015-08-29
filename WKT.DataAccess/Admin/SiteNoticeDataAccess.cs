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

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class SiteNoticeDataAccess : DataAccessBase
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public SiteNoticeDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static SiteNoticeDataAccess _instance = new SiteNoticeDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static SiteNoticeDataAccess Instance
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
        public string SiteNoticeQueryToSQLWhere(SiteNoticeQuery query)
        {
            StringBuilder strFilter = new StringBuilder();
            strFilter.Append(" 1=1");
            if (query.JournalID > 0)
                strFilter.AppendFormat(" and JournalID={0}", query.JournalID);
            if (query.ChannelID > 0)
                strFilter.AppendFormat(" and ChannelID={0}", query.ChannelID);
            return strFilter.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string SiteNoticeQueryToSQLOrder(SiteNoticeQuery query)
        {
            return " NoticeID DESC";
        }
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public SiteNoticeEntity GetSiteNotice(Int64 noticeID)
        {
            SiteNoticeEntity siteNoticeEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  DesID,JournalID,ChannelID,Title,Keywords,Description,Content,FjPath,AddDate FROM dbo.SiteDescribe WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  DesID=@DesID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@DesID", DbType.Int64, noticeID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                siteNoticeEntity = MakeSiteNotice(dr);
            }
            return siteNoticeEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<SiteNoticeEntity> GetSiteNoticeList()
        {
            List<SiteNoticeEntity> siteNoticeEntity=new List<SiteNoticeEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  DesID,JournalID,ChannelID,Title,Keywords,Description,Content,FjPath,AddDate FROM dbo.SiteDescribe WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                siteNoticeEntity = MakeSiteNoticeList(dr);
            }
            return siteNoticeEntity;
        }
        
        public List<SiteNoticeEntity> GetSiteNoticeList(SiteNoticeQuery query)
        {
            string strSql = "SELECT DesID,JournalID,ChannelID,Title,Keywords,Description,Content,FjPath,AddDate FROM dbo.SiteDescribe with(nolock)";
            string whereSQL = SiteNoticeQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;              
            }
            return db.GetList<SiteNoticeEntity>(strSql, MakeSiteNoticeList);
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<SiteNoticeEntity></returns>
        public Pager<SiteNoticeEntity> GetSiteNoticePageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("SiteDescribe", "DesID,JournalID,ChannelID,Title,Keywords,Description,Content,FjPath,AddDate", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<SiteNoticeEntity>  pager = new Pager<SiteNoticeEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeSiteNoticeList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<SiteNoticeEntity> GetSiteNoticePageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("SiteDescribe", "DesID,JournalID,ChannelID,Title,Keywords,Description,Content,FjPath,AddDate", " NoticeID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<SiteNoticeEntity>  pager=new Pager<SiteNoticeEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeSiteNoticeList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<SiteNoticeEntity> GetSiteNoticePageList(SiteNoticeQuery query)
        {
            string strSql = "SELECT DesID,JournalID,ChannelID,Title,Keywords,Description,Content,FjPath,AddDate,ROW_NUMBER() OVER(ORDER BY DesID DESC) AS ROW_ID FROM dbo.SiteDescribe with(nolock)",
                   sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.SiteDescribe with(nolock)";
            string whereSQL = SiteNoticeQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<SiteNoticeEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeSiteNoticeList);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddSiteNotice(SiteNoticeEntity siteNoticeEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();          
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @ChannelID");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @Keywords");
            sqlCommandText.Append(", @Description");
            sqlCommandText.Append(", @Content");
            sqlCommandText.Append(", @FjPath");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.SiteDescribe ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));
           
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,siteNoticeEntity.JournalID);
            db.AddInParameter(cmd, "@ChannelID", DbType.Int64, siteNoticeEntity.ChannelID);
            db.AddInParameter(cmd, "@Title", DbType.String, siteNoticeEntity.Title);
            db.AddInParameter(cmd, "@Keywords", DbType.String, siteNoticeEntity.Keywords);
            db.AddInParameter(cmd, "@Description", DbType.String, siteNoticeEntity.Description);
            db.AddInParameter(cmd,"@Content",DbType.String,siteNoticeEntity.Content);
            db.AddInParameter(cmd, "@FjPath", DbType.String, siteNoticeEntity.FjPath);
         
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
     
        public bool UpdateSiteNotice(SiteNoticeEntity siteNoticeEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  DesID=@DesID");
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" Title=@Title");
            sqlCommandText.Append(", Keywords=@Keywords");
            sqlCommandText.Append(", Description=@Description");
            sqlCommandText.Append(", Content=@Content");
            sqlCommandText.Append(", FjPath=@FjPath");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.SiteDescribe SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@DesID", DbType.Int64, siteNoticeEntity.NoticeID);          
            db.AddInParameter(cmd,"@Title",DbType.AnsiString,siteNoticeEntity.Title);
            db.AddInParameter(cmd,"@Keywords",DbType.AnsiString,siteNoticeEntity.Keywords);
            db.AddInParameter(cmd,"@Description",DbType.AnsiString,siteNoticeEntity.Description);
            db.AddInParameter(cmd,"@Content",DbType.AnsiString,siteNoticeEntity.Content);
            db.AddInParameter(cmd, "@FjPath", DbType.AnsiString, siteNoticeEntity.FjPath);
           
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
        
        public bool DeleteSiteNotice(SiteNoticeEntity siteNoticeEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.SiteDescribe");
            sqlCommandText.Append(" WHERE  DesID=@DesID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            db.AddInParameter(cmd, "@DesID", DbType.Int64, siteNoticeEntity.NoticeID);
            
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
        
        public bool DeleteSiteNotice(Int64 noticeID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.SiteDescribe");
            sqlCommandText.Append(" WHERE  DesID=@DesID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@DesID", DbType.Int64, noticeID);
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
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteNotice(Int64[] noticeID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from SiteDescribe where ");
           
            for(int i=0;i<noticeID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                sqlCommandText.Append("( DesID=@DesID" + i + " )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<noticeID.Length;i++)
            {
                db.AddInParameter(cmd, "@DesID" + i, DbType.Int64, noticeID[i]);
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
        
        public SiteNoticeEntity MakeSiteNotice(IDataReader dr)
        {
            SiteNoticeEntity siteNoticeEntity = null;
            if(dr.Read())
            {
             siteNoticeEntity=new SiteNoticeEntity();
             siteNoticeEntity.NoticeID = (Int64)dr["DesID"];
             siteNoticeEntity.JournalID = (Int64)dr["JournalID"];
             siteNoticeEntity.ChannelID = (Int64)dr["ChannelID"];
             siteNoticeEntity.Title = (String)dr["Title"];
             siteNoticeEntity.Keywords = (String)dr["Keywords"];
             siteNoticeEntity.Description = (String)dr["Description"];
             siteNoticeEntity.Content = (String)dr["Content"];
             siteNoticeEntity.FjPath = (String)dr["FjPath"];
             siteNoticeEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return siteNoticeEntity;
        }
        
        public SiteNoticeEntity MakeSiteNotice(DataRow dr)
        {
            SiteNoticeEntity siteNoticeEntity=null;
            if(dr!=null)
            {
                 siteNoticeEntity=new SiteNoticeEntity();
                 siteNoticeEntity.NoticeID = (Int64)dr["DesID"];
                 siteNoticeEntity.JournalID = (Int64)dr["JournalID"];
                 siteNoticeEntity.ChannelID = (Int64)dr["ChannelID"];
                 siteNoticeEntity.Title = (String)dr["Title"];
                 siteNoticeEntity.Keywords = (String)dr["Keywords"];
                 siteNoticeEntity.Description = (String)dr["Description"];
                 siteNoticeEntity.Content = (String)dr["Content"];
                 siteNoticeEntity.FjPath = (String)dr["FjPath"];
                 siteNoticeEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return siteNoticeEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<SiteNoticeEntity> MakeSiteNoticeList(IDataReader dr)
        {
            List<SiteNoticeEntity> list=new List<SiteNoticeEntity>();
            while(dr.Read())
            {
             SiteNoticeEntity siteNoticeEntity=new SiteNoticeEntity();
             siteNoticeEntity.NoticeID = (Int64)dr["DesID"];
            siteNoticeEntity.JournalID = (Int64)dr["JournalID"];
            siteNoticeEntity.ChannelID = (Int64)dr["ChannelID"];
            siteNoticeEntity.Title = (String)dr["Title"];
            siteNoticeEntity.Keywords = (String)dr["Keywords"];
            siteNoticeEntity.Description = (String)dr["Description"];
            siteNoticeEntity.Content = (String)dr["Content"];
            siteNoticeEntity.FjPath = (String)dr["FjPath"];
            siteNoticeEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(siteNoticeEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<SiteNoticeEntity> MakeSiteNoticeList(DataTable dt)
        {
            List<SiteNoticeEntity> list=new List<SiteNoticeEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   SiteNoticeEntity siteNoticeEntity=MakeSiteNotice(dt.Rows[i]);
                   list.Add(siteNoticeEntity);
                }
            }
            return list;
        }
        
        #endregion

    }
}

