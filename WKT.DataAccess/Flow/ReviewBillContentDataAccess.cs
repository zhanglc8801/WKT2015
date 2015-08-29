using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

using WKT.Model;
using WKT.Data.SQL;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class ReviewBillContentDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public ReviewBillContentDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static ReviewBillContentDataAccess _instance = new ReviewBillContentDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static ReviewBillContentDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 保存审稿单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool SaveReviewBillContent(IList<ReviewBillContentEntity> list)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    var group = list.GroupBy(p => new { p.JournalID, p.AddUser, p.CID });
                    foreach (var item in group)
                    {
                        DelReviewBillContent(item.Key.JournalID, item.Key.AddUser, item.Key.CID, trans);
                        foreach (var model in item)
                        {
                            AddReviewBillContent(model, trans);
                        }
                    }

                    trans.Commit();
                    conn.Close();
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
        /// 添加审稿单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddReviewBillContent(ReviewBillContentEntity model, DbTransaction trans = null)
        {
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @CID");
            sqlCommandText.Append(", @JournalID");
            sqlCommandText.Append(", @ItemID");
            sqlCommandText.Append(", @ContentValue");
            sqlCommandText.Append(", @IsChecked");
            sqlCommandText.Append(", @AddUser");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.ReviewBillContent ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@ItemContentID", DbType.Int64, model.ItemContentID);
            db.AddInParameter(cmd, "@CID", DbType.Int64, model.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@ItemID", DbType.AnsiString, model.ItemID);
            db.AddInParameter(cmd, "@ContentValue", DbType.AnsiString, model.ContentValue);
            db.AddInParameter(cmd, "@IsChecked", DbType.Boolean, model.IsChecked);
            db.AddInParameter(cmd, "@AddUser", DbType.Int64, model.AddUser);
            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("新增审稿单失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除专家审稿单
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="AuthorID"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public bool DelReviewBillContent(Int64 JournalID, Int64 AuthorID, Int64 CID, DbTransaction trans = null)
        {
            string strSql = string.Format("Delete dbo.ReviewBillContent WHERE JournalID={0} and AddUser={1} and CID={2}", JournalID, AuthorID, CID);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            try
            {
                if (trans == null)
                    db.ExecuteNonQuery(cmd);
                else
                    db.ExecuteNonQuery(cmd, trans);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取审稿单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ReviewBillContentEntity> GetReviewBillContentList(ReviewBillContentQuery query)
        {
            string strSql = @"SELECT a.*,b.Title,b.ItemType,b.PItemID,b.SortID FROM dbo.ReviewBillContent a with(nolock) 
                              INNER JOIN dbo.ReviewBill b with(nolock) ON a.JournalID=b.JournalID and a.ItemID=b.ItemID"
                , where = GetReviewBillContentFilter(query);
            if (!string.IsNullOrWhiteSpace(where))
                strSql += " WHERE " + where;
            return db.GetList<ReviewBillContentEntity>(strSql, MakeReviewBillContentList);
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetReviewBillContentFilter(ReviewBillContentQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" a.JournalID = " + query.JournalID);
            if (query.AddUser != null)
                strFilter.Append(" and a.AddUser=").Append(query.AddUser.Value);
            if (query.CID != null)
                strFilter.Append(" and a.CID=").Append(query.CID.Value);
            return strFilter.ToString();
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<ReviewBillContentEntity> MakeReviewBillContentList(IDataReader dr)
        {
            List<ReviewBillContentEntity> list = new List<ReviewBillContentEntity>();
            while (dr.Read())
            {
                list.Add(MakeReviewBillContent(dr));
            }           
            return list;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private ReviewBillContentEntity MakeReviewBillContent(IDataReader dr)
        {
            ReviewBillContentEntity reviewBillContentEntity = new ReviewBillContentEntity();
            reviewBillContentEntity.ItemContentID = (Int64)dr["ItemContentID"];
            reviewBillContentEntity.CID = (Int64)dr["CID"];
            reviewBillContentEntity.JournalID = (Int64)dr["JournalID"];
            reviewBillContentEntity.ItemID = (Int64)dr["ItemID"];
            reviewBillContentEntity.ContentValue = (String)dr["ContentValue"];
            reviewBillContentEntity.IsChecked = (Boolean)dr["IsChecked"];
            reviewBillContentEntity.AddUser = (Int64)dr["AddUser"];
            reviewBillContentEntity.AddDate = (DateTime)dr["AddDate"];
            reviewBillContentEntity.Title = (String)dr["Title"];
            reviewBillContentEntity.ItemType = (Byte)dr["ItemType"];
            reviewBillContentEntity.PItemID = (Int64)dr["PItemID"];
            reviewBillContentEntity.SortID = (Int32)dr["SortID"];
            return reviewBillContentEntity;
        }
    }
}

