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
    public class SiteResourceDataAccess:DataAccessBase
    {
        #region 属性
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public SiteResourceDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static SiteResourceDataAccess _instance = new SiteResourceDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static SiteResourceDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 获取资源分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteResourceEntity> GetSiteResourcePageList(SiteResourceQuery query)
        {
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY ResourceID DESC) AS ROW_ID FROM dbo.SiteResource with(nolock)",
                   sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.SiteResource with(nolock)";
            string whereSQL = GetSiteResourceFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<SiteResourceEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeSiteResourceList);
        }

        /// <summary>
        /// 获取资源数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteResourceEntity> GetSiteResourceList(SiteResourceQuery query)
        {
            string strSql = "SELECT * FROM dbo.SiteResource with(nolock)";
            string whereSQL = GetSiteResourceFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;              
            }
            strSql += " order by ResourceID";
            return db.GetList<SiteResourceEntity>(strSql, MakeSiteResourceList);
        }

        /// <summary>
        /// 获取资源实体
        /// </summary>
        /// <param name="ResourceID"></param>
        /// <returns></returns>
        public SiteResourceEntity GetSiteResourceModel(SiteResourceQuery query)
        {
            SiteResourceEntity item = null;
            string strSql = string.Format("SELECT TOP 1 * FROM dbo.SiteResource with(nolock) WHERE ResourceID=@ResourceID AND JournalID=@JournalID");
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@ResourceID", DbType.Int64, query.ResourceID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                    item = MakeSiteResourceModel(dr);
            }
            return item;
        }

        /// <summary>
        /// 新增资源
        /// </summary>
        /// <param name="SiteResourceEntity"></param>
        /// <returns></returns>
        public bool AddSiteResource(SiteResourceEntity model)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @ChannelID");
            sqlCommandText.Append(", @Name");
            sqlCommandText.Append(", @FileIntro");
            sqlCommandText.Append(", @FilePath");
            sqlCommandText.Append(", @FileEx");
            sqlCommandText.Append(", @FileSize");
            sqlCommandText.Append(", @DownloadCount");
            sqlCommandText.Append(", @InUserID");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.SiteResource ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@ChannelID", DbType.Int64, model.ChannelID);
            db.AddInParameter(cmd, "@Name", DbType.AnsiString, model.Name);
            db.AddInParameter(cmd, "@FileIntro", DbType.AnsiString, model.FileIntro);
            db.AddInParameter(cmd, "@FilePath", DbType.AnsiString, model.FilePath);
            db.AddInParameter(cmd, "@FileEx", DbType.AnsiString, model.FileEx);
            db.AddInParameter(cmd, "@FileSize", DbType.Decimal, model.FileSize);
            db.AddInParameter(cmd, "@DownloadCount", DbType.Int32, model.DownloadCount);
            db.AddInParameter(cmd, "@InUserID", DbType.Int64, model.InUserID);

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
        /// 编辑资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSiteResource(SiteResourceEntity model)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  ResourceID=@ResourceID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Name=@Name");
            sqlCommandText.Append(", FileIntro=@FileIntro");
            sqlCommandText.Append(", FilePath=@FilePath");
            sqlCommandText.Append(", FileEx=@FileEx");
            sqlCommandText.Append(", FileSize=@FileSize");           
            sqlCommandText.Append(", EditUserID=@EditUserID");
            sqlCommandText.Append(", EditDate=getdate()");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.SiteResource SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@ResourceID", DbType.Int64, model.ResourceID);
            db.AddInParameter(cmd, "@Name", DbType.AnsiString, model.Name);
            db.AddInParameter(cmd, "@FileIntro", DbType.AnsiString, model.FileIntro);
            db.AddInParameter(cmd, "@FilePath", DbType.AnsiString, model.FilePath);
            db.AddInParameter(cmd, "@FileEx", DbType.AnsiString, model.FileEx);
            db.AddInParameter(cmd, "@FileSize", DbType.Decimal, model.FileSize);
            db.AddInParameter(cmd, "@EditUserID", DbType.Int64, model.EditUserID);

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
        /// 累加下载次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AccumulationDownloadCount(SiteResourceEntity model)
        {
            string strSql = string.Format("UPDATE dbo.SiteResource set DownloadCount=DownloadCount+1 WHERE ResourceID={0}", model.ResourceID);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(cmd) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteResource(Int64[] ResourceID)
        {
            if (ResourceID == null || ResourceID.Length < 1)
                return false;           
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from SiteResource where ResourceID");
            if (ResourceID.Length > 1)
                sqlCommandText.AppendFormat(" in ({0})", string.Join(",", ResourceID));
            else
                sqlCommandText.AppendFormat(" ={0}", ResourceID[0]);
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
        private string GetSiteResourceFilter(SiteResourceQuery query)
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
        private List<SiteResourceEntity> MakeSiteResourceList(IDataReader dr)
        {
            List<SiteResourceEntity> list = new List<SiteResourceEntity>();            
            while (dr.Read())
            {
                list.Add(MakeSiteResourceModel(dr));
            }         
            return list;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private SiteResourceEntity MakeSiteResourceModel(IDataReader dr)
        {
            SiteResourceEntity model = new SiteResourceEntity();
            model.ResourceID = (Int64)dr["ResourceID"];
            model.JournalID = (Int64)dr["JournalID"];
            model.ChannelID = (Int64)dr["ChannelID"];
            model.Name = (String)dr["Name"];
            model.FileIntro = (String)dr["FileIntro"];
            model.FilePath = (String)dr["FilePath"];
            model.FileEx = (String)dr["FileEx"];
            model.FileSize = (Decimal)dr["FileSize"];
            model.DownloadCount = (Int32)dr["DownloadCount"];
            model.InUserID = (Int64)dr["InUserID"];
            model.EditUserID = (Int64)dr["EditUserID"];
            model.EditDate = (DateTime)dr["EditDate"];
            model.AddDate = (DateTime)dr["AddDate"];
            return model;
        }
        #endregion
    }
}
