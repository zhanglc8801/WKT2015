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
    public class SiteContentDataAccess:DataAccessBase
    {
        #region 属性
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public SiteContentDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static SiteContentDataAccess _instance = new SiteContentDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static SiteContentDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 获取资讯分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteContentEntity> GetSiteContentPageList(SiteContentQuery query)
        {
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY SortID ASC) AS ROW_ID FROM dbo.SiteContent a with(nolock)",
                sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.SiteContent with(nolock)";
            string whereSQL = GetSiteContentFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<SiteContentEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeSiteContentList);
        }

        /// <summary>
        /// 获取资讯数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteContentEntity> GetSiteContentList(SiteContentQuery query)
        {
            string strSql = "SELECT * FROM dbo.SiteContent a with(nolock)";
            string whereSQL = GetSiteContentFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " order by SortID";
            return db.GetList<SiteContentEntity>(strSql, MakeSiteContentList);
        }

        /// <summary>
        /// 获取资讯实体
        /// </summary>
        /// <param name="ContentID"></param>
        /// <returns></returns>
        public SiteContentEntity GetSiteContentModel(Int64 ContentID)
        {
            string strSql = string.Format(@"SELECT TOP 1 a.*,b.Content FROM dbo.SiteContent a with(nolock) 
                                            INNER JOIN dbo.SiteContentAtt b with(nolock) ON a.ContentID=b.ContentID 
                                            WHERE a.ContentID={0}", ContentID);
            SiteContentEntity model = null;
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    model = MakeSiteContentModel(dr);
                    model.Content = (String)dr["Content"];
                }
                dr.Close();
            }
            return model;
        }

        /// <summary>
        /// 新增资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddSiteContent(SiteContentEntity model)
        {            
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @ChannelID");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @Linkurl");
            sqlCommandText.Append(", @TitleColor");
            sqlCommandText.Append(", @IsBold");
            sqlCommandText.Append(", @IsItalic");
            sqlCommandText.Append(", @Source");
            sqlCommandText.Append(", @Author");
            sqlCommandText.Append(", @Tags");
            sqlCommandText.Append(", @Abstruct");
            sqlCommandText.Append(", @TitlePhoto");
            sqlCommandText.Append(", @FJPath");
            sqlCommandText.Append(", @SortID");
            sqlCommandText.Append(", @InAuthor");

            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.SiteContent ({0},AddDate) VALUES ({1},getdate());select SCOPE_IDENTITY();", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

                    db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
                    db.AddInParameter(cmd, "@ChannelID", DbType.Int64, model.ChannelID);
                    db.AddInParameter(cmd, "@Title", DbType.AnsiString, model.Title);
                    db.AddInParameter(cmd, "@Linkurl", DbType.AnsiString, model.Linkurl);
                    db.AddInParameter(cmd, "@TitleColor", DbType.AnsiString, model.TitleColor);
                    db.AddInParameter(cmd, "@IsBold", DbType.Boolean, model.IsBold);
                    db.AddInParameter(cmd, "@IsItalic", DbType.Boolean, model.IsItalic);
                    db.AddInParameter(cmd, "@Source", DbType.AnsiString, model.Source);
                    db.AddInParameter(cmd, "@Author", DbType.AnsiString, model.Author);
                    db.AddInParameter(cmd, "@Tags", DbType.AnsiString, model.Tags);
                    db.AddInParameter(cmd, "@Abstruct", DbType.AnsiString, model.Abstruct);
                    db.AddInParameter(cmd, "@TitlePhoto", DbType.AnsiString, model.TitlePhoto);
                    db.AddInParameter(cmd, "@FJPath", DbType.AnsiString, model.FJPath);
                    db.AddInParameter(cmd, "@SortID", DbType.Int32, model.SortID);
                    db.AddInParameter(cmd, "@InAuthor", DbType.AnsiString, model.InAuthor);

                    Int64 ContentID = db.ExecuteScalar(cmd, trans).TryParse<Int64>();

                    if (ContentID == 0)
                    {
                        trans.Rollback();
                        return false;
                    }

                    cmd = db.GetSqlStringCommand("INSERT INTO dbo.SiteContentAtt(ContentID,Content) VALUES(@ContentID,@Content)");
                    db.AddInParameter(cmd, "@ContentID", DbType.Int64, ContentID);
                    db.AddInParameter(cmd, "@Content", DbType.String, model.Content);
                    if (db.ExecuteNonQuery(cmd, trans) < 1)
                    {
                        trans.Rollback();
                        return false;
                    }

                    trans.Commit();
                    conn.Close();
                    return true;
                }
                catch (SqlException sqlEx)
                {
                    trans.Rollback();
                    throw sqlEx;
                }
            }           
        }

        /// <summary>
        /// 编辑资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSiteContent(SiteContentEntity model)
        {          
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  ContentID=@ContentID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Title=@Title");
            sqlCommandText.Append(", Linkurl=@Linkurl");
            sqlCommandText.Append(", TitleColor=@TitleColor");
            sqlCommandText.Append(", IsBold=@IsBold");
            sqlCommandText.Append(", IsItalic=@IsItalic");
            sqlCommandText.Append(", Source=@Source");
            sqlCommandText.Append(", Author=@Author");
            sqlCommandText.Append(", Tags=@Tags");
            sqlCommandText.Append(", Abstruct=@Abstruct");
            sqlCommandText.Append(", TitlePhoto=@TitlePhoto");
            sqlCommandText.Append(", FJPath=@FJPath");
            sqlCommandText.Append(", SortID=@SortID");
            sqlCommandText.Append(", EditAuthor=@EditAuthor");
            sqlCommandText.Append(", EditDate=getdate()");

            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.SiteContent SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

                    db.AddInParameter(cmd, "@ContentID", DbType.Int64, model.ContentID);                   
                    db.AddInParameter(cmd, "@Title", DbType.AnsiString, model.Title);
                    db.AddInParameter(cmd, "@Linkurl", DbType.AnsiString, model.Linkurl);
                    db.AddInParameter(cmd, "@TitleColor", DbType.AnsiString, model.TitleColor);
                    db.AddInParameter(cmd, "@IsBold", DbType.Boolean, model.IsBold);
                    db.AddInParameter(cmd, "@IsItalic", DbType.Boolean, model.IsItalic);
                    db.AddInParameter(cmd, "@Source", DbType.AnsiString, model.Source);
                    db.AddInParameter(cmd, "@Author", DbType.AnsiString, model.Author);
                    db.AddInParameter(cmd, "@Tags", DbType.AnsiString, model.Tags);
                    db.AddInParameter(cmd, "@Abstruct", DbType.AnsiString, model.Abstruct);
                    db.AddInParameter(cmd, "@TitlePhoto", DbType.AnsiString, model.TitlePhoto);
                    db.AddInParameter(cmd, "@FJPath", DbType.AnsiString, model.FJPath);
                    db.AddInParameter(cmd, "@SortID", DbType.Int32, model.SortID);
                    db.AddInParameter(cmd, "@EditAuthor", DbType.AnsiString, model.EditAuthor);

                    if (db.ExecuteNonQuery(cmd,trans)<1)
                    {
                        trans.Rollback();
                        return false;
                    }

                    cmd = db.GetSqlStringCommand("UPDATE dbo.SiteContentAtt set Content=@Content where ContentID=@ContentID");                    
                    db.AddInParameter(cmd, "@Content", DbType.String, model.Content);
                    db.AddInParameter(cmd, "@ContentID", DbType.Int64, model.ContentID);
                    if (db.ExecuteNonQuery(cmd, trans) < 1)
                    {
                        trans.Rollback();
                        return false;
                    }

                    trans.Commit();
                    conn.Close();
                    return true;
                }
                catch (SqlException sqlEx)
                {
                    trans.Rollback();
                    throw sqlEx;
                }
            }           
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteContent(Int64[] ContentID)
        {
            if (ContentID == null || ContentID.Length < 1)
                return false;
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    string strContent=string.Join(",",ContentID);

                    DbCommand cmd = db.GetSqlStringCommand(string.Format("DELETE dbo.SiteContentAtt WHERE ContentID in ({0})", strContent));   
                    if (db.ExecuteNonQuery(cmd, trans) < 1)
                    {
                        trans.Rollback();
                        return false;
                    }

                    cmd = db.GetSqlStringCommand(string.Format("DELETE dbo.SiteContent WHERE ContentID in ({0})", strContent));
                    if (db.ExecuteNonQuery(cmd, trans) < 1)
                    {
                        trans.Rollback();
                        return false;
                    }

                    trans.Commit();
                    conn.Close();
                    return true;
                }
                catch (SqlException sqlEx)
                {
                    trans.Rollback();
                    throw sqlEx;
                }
            }           
        }

        #region 私有方法
        /// <summary>
        /// 获取条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetSiteContentFilter(SiteContentQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID="+query.JournalID);        
            if (query.ChannelID != null)
                strFilter.AppendFormat(" and ChannelID={0}", query.ChannelID.Value);
            if (query.IsPhoto)
            {
                strFilter.Append(" and TitlePhoto<>''");
            }
            return strFilter.ToString();
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<SiteContentEntity> MakeSiteContentList(IDataReader dr)
        {
            List<SiteContentEntity> list = new List<SiteContentEntity>();
            while (dr.Read())
            {
                list.Add(MakeSiteContentModel(dr));
            }
            return list;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private SiteContentEntity MakeSiteContentModel(IDataReader dr)
        {
            SiteContentEntity model = new SiteContentEntity();
            model.ContentID = (Int64)dr["ContentID"];
            model.JournalID = (Int64)dr["JournalID"];
            model.ChannelID = (Int64)dr["ChannelID"];
            model.Title = (String)dr["Title"];
            model.Linkurl = (String)dr["Linkurl"];
            model.TitleColor = (String)dr["TitleColor"];
            model.IsBold = (Boolean)dr["IsBold"];
            model.IsItalic = (Boolean)dr["IsItalic"];
            model.Source = (String)dr["Source"];
            model.Author = (String)dr["Author"];
            model.Tags = (String)dr["Tags"];
            model.Abstruct = (String)dr["Abstruct"];
            model.TitlePhoto = (String)dr["TitlePhoto"];
            model.FJPath = dr["FJPath"]==System.DBNull.Value?"": (String)dr["FJPath"];
            model.SortID = dr["SortID"] == System.DBNull.Value ? 0 : (Int32)dr["SortID"];
            model.InAuthor = (Int64)dr["InAuthor"];
            model.EditAuthor = (Int64)dr["EditAuthor"];
            model.EditDate = (DateTime)dr["EditDate"];
            model.AddDate = (DateTime)dr["AddDate"];
            return model;
        }
        #endregion
    }
}
