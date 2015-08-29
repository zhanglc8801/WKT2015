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
    public class SiteBlockDataAccess:DataAccessBase
    {
        #region 属性
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public SiteBlockDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static SiteBlockDataAccess _instance = new SiteBlockDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static SiteBlockDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 获取内容块分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteBlockEntity> GetSSiteBlockPageList(SiteBlockQuery query)
        {
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY BlockID DESC) AS ROW_ID FROM dbo.SiteBlock with(nolock)",
                   sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.SiteBlock with(nolock)";
            string whereSQL = GetSiteBlockFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<SiteBlockEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeSiteBlockList);
        }

        /// <summary>
        /// 获取内容块数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteBlockEntity> GetSiteBlockList(SiteBlockQuery query)
        {
            string strSql = "SELECT * FROM dbo.SiteBlock with(nolock)";
            string whereSQL = GetSiteBlockFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;              
            }
            strSql += " order by BlockID";
            return db.GetList<SiteBlockEntity>(strSql, MakeSiteBlockList);
        }

        /// <summary>
        /// 获取内容块实体
        /// </summary>
        /// <param name="ResourceID"></param>
        /// <returns></returns>
        public SiteBlockEntity GetSiteBlockModel(Int64 BlockID)
        {
            SiteBlockEntity item = null;
            string strSql = string.Format("SELECT TOP 1 * FROM dbo.SiteBlock with(nolock) WHERE BlockID={0}", BlockID);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                    item = MakeSiteBlockModel(dr);
                dr.Close();
            }
            return item;
        }

        /// <summary>
        /// 新增内容块
        /// </summary>
        /// <param name="SiteResourceEntity"></param>
        /// <returns></returns>
        public bool AddSiteBlock(SiteBlockEntity model)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @ChannelID");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @Linkurl");
            sqlCommandText.Append(", @TitlePhoto");
            sqlCommandText.Append(", @Note");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.SiteBlock ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@ChannelID", DbType.Int64, model.ChannelID);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, model.Title);
            db.AddInParameter(cmd, "@Linkurl", DbType.AnsiString, model.Linkurl);
            db.AddInParameter(cmd, "@TitlePhoto", DbType.AnsiString, model.TitlePhoto);
            db.AddInParameter(cmd, "@Note", DbType.AnsiString, model.Note);
           
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
        /// 编辑内容块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSiteBlock(SiteBlockEntity model)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  BlockID=@BlockID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Title=@Title");
            sqlCommandText.Append(", Linkurl=@Linkurl");
            sqlCommandText.Append(", TitlePhoto=@TitlePhoto");
            sqlCommandText.Append(", Note=@Note");           

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.SiteBlock SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@BlockID", DbType.Int64, model.BlockID);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, model.Title);
            db.AddInParameter(cmd, "@Linkurl", DbType.AnsiString, model.Linkurl);
            db.AddInParameter(cmd, "@TitlePhoto", DbType.AnsiString, model.TitlePhoto);
            db.AddInParameter(cmd, "@Note", DbType.AnsiString, model.Note);
           
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
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteBlock(Int64[] BlockID)
        {
            if (BlockID == null || BlockID.Length < 1)
                return false;           
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from SiteBlock where BlockID");
            if (BlockID.Length > 1)
                sqlCommandText.AppendFormat(" in ({0})", string.Join(",", BlockID));
            else
                sqlCommandText.AppendFormat(" ={0}", BlockID[0]);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            try
            {
                return db.ExecuteNonQuery(cmd) > 0;
               
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
        }

        #region 私有方法
        /// <summary>
        /// 获取条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetSiteBlockFilter(SiteBlockQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID=" + query.JournalID);           
            if (query.ChannelID > 0)
                strFilter.AppendFormat(" and ChannelID={0}", query.ChannelID);
            return strFilter.ToString();
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<SiteBlockEntity> MakeSiteBlockList(IDataReader dr)
        {
            List<SiteBlockEntity> list = new List<SiteBlockEntity>();            
            while (dr.Read())
            {
                list.Add(MakeSiteBlockModel(dr));
            }
            if (!dr.IsClosed)
            {
                dr.Close();
            }
            return list;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private SiteBlockEntity MakeSiteBlockModel(IDataReader dr)
        {
            SiteBlockEntity model = new SiteBlockEntity();
            model.BlockID = (Int64)dr["BlockID"];
            model.JournalID = (Int64)dr["JournalID"];
            model.ChannelID = (Int64)dr["ChannelID"];
            model.Title = (String)dr["Title"];
            model.Linkurl = (String)dr["Linkurl"];
            model.TitlePhoto = (String)dr["TitlePhoto"];
            model.Note = (String)dr["Note"];
            model.AddDate = (DateTime)dr["AddDate"];          
            return model;
        }
        #endregion
    }
}
