using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Data.SQL;
using WKT.Common.Extension;
using WKT.Common.Utils;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class ReviewBillDataAccess:DataAccessBase
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public ReviewBillDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static ReviewBillDataAccess _instance = new ReviewBillDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static ReviewBillDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 新增审稿单项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddReviewBill(ReviewBillEntity model)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();          
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @ItemType");
            sqlCommandText.Append(", @PItemID");
            sqlCommandText.Append(", @SortID");           

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.ReviewBill ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, model.Title.TextFilter());
            db.AddInParameter(cmd, "@ItemType", DbType.Byte, model.ItemType);
            db.AddInParameter(cmd, "@PItemID", DbType.Int64, model.PItemID);
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
        /// 编辑审稿单项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateReviewBill(ReviewBillEntity model)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  ItemID=@ItemID");
            StringBuilder sqlCommandText = new StringBuilder();            
            sqlCommandText.Append(" Title=@Title");
            sqlCommandText.Append(", ItemType=@ItemType");            
            sqlCommandText.Append(", SortID=@SortID");            

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.ReviewBill SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@ItemID", DbType.Int64, model.ItemID);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, model.Title.TextFilter());
            db.AddInParameter(cmd, "@ItemType", DbType.Byte, model.ItemType);
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
        /// 获取审稿单项实体
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public ReviewBillEntity GetReviewBill(Int64 itemID)
        {
            ReviewBillEntity model = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  * FROM dbo.ReviewBill WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  ItemID=@ItemID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@ItemID", DbType.Int64, itemID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                    model = MakeReviewBill(dr);
            }
            return model;
        }

        /// <summary>
        /// 审稿单项是否已经被使用
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public bool ReviewBillIsEnabled(Int64 JournalID, Int64 itemID)
        {
            string strSql = string.Format("SELECT 1 FROM dbo.ReviewBillContent with(nolock) WHERE JournalID={0} and ItemID={1}", JournalID, itemID);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteScalar(cmd).TryParse<Int32>() == 1;
        }

        /// <summary>
        /// 审稿单项是否存在下级
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public bool ReviewBillIsHavDown(Int64 JournalID, Int64 itemID)
        {
            string strSql = string.Format("SELECT 1 FROM dbo.ReviewBill with(nolock) WHERE JournalID={0} and PItemID={1}", JournalID, itemID);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteScalar(cmd).TryParse<Int32>() == 1;
        }

        /// <summary>
        /// 删除审稿单项
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public bool DelReviewBill(Int64 JournalID, Int64 itemID)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    string strSql = string.Format("DELETE dbo.ReviewBill WHERE JournalID={0} and ItemID={1}", JournalID, itemID);
                    DbCommand cmd = db.GetSqlStringCommand(strSql);
                    db.ExecuteNonQuery(cmd, trans);

                    strSql = string.Format("DELETE dbo.ReviewBillContent WHERE JournalID={0} and ItemID={1}", JournalID, itemID);
                    cmd = db.GetSqlStringCommand(strSql);
                    db.ExecuteNonQuery(cmd,trans);

                    trans.Commit();

                    return true;

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取审稿单项分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ReviewBillEntity> GetReviewBillPageList(ReviewBillQuery query)
        {
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY SortID ASC) AS ROW_ID FROM dbo.ReviewBill with(nolock)",
                   sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.ReviewBill with(nolock)";
            string whereSQL = GetReviewBillFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<ReviewBillEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeReviewBillList);
        }

        /// <summary>
        /// 获取审稿单项数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ReviewBillEntity> GetReviewBillList(ReviewBillQuery query)
        {
            string strSql = "SELECT * FROM dbo.ReviewBill with(nolock)";
            string whereSQL = GetReviewBillFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " order by SortID";
            return db.GetList<ReviewBillEntity>(strSql, MakeReviewBillList);
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetReviewBillFilter(ReviewBillQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID = " + query.JournalID);
            if (query.PItemID != null)
                strFilter.Append(" and PItemID=").Append(query.PItemID.Value);
            return strFilter.ToString();
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<ReviewBillEntity> MakeReviewBillList(IDataReader dr)
        {
            List<ReviewBillEntity> list = new List<ReviewBillEntity>();
            while (dr.Read())
            {
                list.Add(MakeReviewBill(dr));
            }            
            return list;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private ReviewBillEntity MakeReviewBill(IDataReader dr)
        {
            ReviewBillEntity reviewBillEntity = new ReviewBillEntity();
            reviewBillEntity.ItemID = (Int64)dr["ItemID"];
            reviewBillEntity.JournalID = (Int64)dr["JournalID"];
            reviewBillEntity.Title = (String)dr["Title"];
            reviewBillEntity.ItemType = (Byte)dr["ItemType"];
            reviewBillEntity.PItemID = (Int64)dr["PItemID"];
            reviewBillEntity.SortID = (Int32)dr["SortID"];
            reviewBillEntity.AddDate = (DateTime)dr["AddDate"];
            return reviewBillEntity;
        }

    }
}

