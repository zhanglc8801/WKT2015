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
    public class FriendlyLinkDataAccess:DataAccessBase
    {
        #region 属性
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public FriendlyLinkDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static FriendlyLinkDataAccess _instance = new FriendlyLinkDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static FriendlyLinkDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 获取友情链接分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FriendlyLinkEntity> GetFriendlyLinkPageList(FriendlyLinkQuery query)
        {
            string strSql = "SELECT LinkID,JournalID,ChannelID,SiteName,SiteUrl,LogoUrl,LinkType,SortID,AddDate,ROW_NUMBER() OVER(ORDER BY LinkID DESC) AS ROW_ID FROM dbo.FriendlyLink with(nolock)",
                   sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.FriendlyLink with(nolock)";
            string whereSQL = GetFriendlyLinkFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<FriendlyLinkEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeFriendlyLinkList);
        }

        /// <summary>
        /// 获取友情链接数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<FriendlyLinkEntity> GetFriendlyLinkList(FriendlyLinkQuery query)
        {
            string strSql = "SELECT LinkID,JournalID,ChannelID,SiteName,SiteUrl,LogoUrl,LinkType,SortID,AddDate FROM dbo.FriendlyLink with(nolock)";
            string whereSQL = GetFriendlyLinkFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;              
            }
            strSql += " order by SortID";
            return db.GetList<FriendlyLinkEntity>(strSql, MakeFriendlyLinkList);
        }

        /// <summary>
        /// 获取友情链接实体
        /// </summary>
        /// <param name="LinkID"></param>
        /// <returns></returns>
        public FriendlyLinkEntity GetFriendlyLinkModel(Int64 LinkID)
        {
            FriendlyLinkEntity item = null;
            string strSql = string.Format("SELECT TOP 1 * FROM dbo.FriendlyLink with(nolock) WHERE LinkID={0}", LinkID);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                    item = MakeFriendlyLinkModel(dr);
                dr.Close();
            }
            return item;
        }

        /// <summary>
        /// 新增友情链接
        /// </summary>
        /// <param name="siteNoticeEntity"></param>
        /// <returns></returns>
        public bool AddFriendlyLink(FriendlyLinkEntity model)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @ChannelID");
            sqlCommandText.Append(", @SiteName");
            sqlCommandText.Append(", @SiteUrl");
            sqlCommandText.Append(", @LogoUrl");
            sqlCommandText.Append(", @LinkType");
            sqlCommandText.Append(", @SortID");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.FriendlyLink ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@ChannelID", DbType.Int64, model.ChannelID);
            db.AddInParameter(cmd, "@SiteName", DbType.AnsiString, model.SiteName);
            db.AddInParameter(cmd, "@SiteUrl", DbType.AnsiString, model.SiteUrl);
            db.AddInParameter(cmd, "@LogoUrl", DbType.AnsiString, model.LogoUrl);
            db.AddInParameter(cmd, "@LinkType", DbType.Byte, model.LinkType);
            db.AddInParameter(cmd, "@SortID", DbType.Int32, model.SortID);

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
        /// 编辑友情链接
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateFriendlyLink(FriendlyLinkEntity model)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  LinkID=@LinkID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" SiteName=@SiteName");
            sqlCommandText.Append(", SiteUrl=@SiteUrl");
            sqlCommandText.Append(", LogoUrl=@LogoUrl");
            sqlCommandText.Append(", LinkType=@LinkType");
            sqlCommandText.Append(", SortID=@SortID");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.FriendlyLink SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@LinkID", DbType.Int64, model.LinkID);
            db.AddInParameter(cmd, "@SiteName", DbType.AnsiString, model.SiteName);
            db.AddInParameter(cmd, "@SiteUrl", DbType.AnsiString, model.SiteUrl);
            db.AddInParameter(cmd, "@LogoUrl", DbType.AnsiString, model.LogoUrl);
            db.AddInParameter(cmd, "@LinkType", DbType.Byte, model.LinkType);
            db.AddInParameter(cmd, "@SortID", DbType.Int32, model.SortID);

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
        public bool BatchDeleteFriendlyLink(Int64[] LinkID)
        {
            if (LinkID == null || LinkID.Length < 1)
                return false;           
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from FriendlyLink where LinkID");
            if (LinkID.Length > 1)
                sqlCommandText.AppendFormat(" in ({0})", string.Join(",", LinkID));
            else
                sqlCommandText.AppendFormat(" ={0}", LinkID[0]);
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
        private string GetFriendlyLinkFilter(FriendlyLinkQuery query)
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
        /// 组装数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<FriendlyLinkEntity> MakeFriendlyLinkList(IDataReader dr)
        {
            List<FriendlyLinkEntity> list = new List<FriendlyLinkEntity>();            
            while (dr.Read())
            {
                list.Add(MakeFriendlyLinkModel(dr));
            }         
            return list;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private FriendlyLinkEntity MakeFriendlyLinkModel(IDataReader dr)
        {
            FriendlyLinkEntity model = new FriendlyLinkEntity();
            model.LinkID = (Int64)dr["LinkID"];
            model.JournalID = (Int64)dr["JournalID"];
            model.ChannelID = (Int64)dr["ChannelID"];
            model.SiteName = (String)dr["SiteName"];
            model.SiteUrl = (String)dr["SiteUrl"];
            model.LogoUrl = (String)dr["LogoUrl"];
            model.LinkType = (Byte)dr["LinkType"];
            model.SortID = (Int32)dr["SortID"];
            model.AddDate = (DateTime)dr["AddDate"];
            return model;
        }
        #endregion
    }
}
