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
using WKT.Common.Extension;
using WKT.Common.Security;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class FinanceContributeDataAccess : DataAccessBase
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db = null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public FinanceContributeDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if (db == null)
                throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static FinanceContributeDataAccess _instance = new FinanceContributeDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static FinanceContributeDataAccess Instance
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
        public string FinanceContributeQueryToSQLWhere(FinanceContributeQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" a.JournalID=" + query.JournalID);
            if (query.AuthorID != null)
                strFilter.Append(" AND b.AuthorID = ").Append(query.AuthorID.Value);
            if (query.FeeType != null)
                strFilter.Append(" AND a.FeeType=").Append(query.FeeType.Value);
            if (query.Status != null)
                strFilter.Append(" AND a.Status=").Append(query.Status.Value);
            if (query.CID != null)
                strFilter.Append(" AND a.CID=").Append(query.CID.Value);
            return strFilter.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string FinanceContributeQueryToSQLOrder(FinanceContributeQuery query)
        {
            return " PKID DESC";
        }

        #endregion 组装SQL条件

        #region 获取一个实体对象

        public FinanceContributeEntity GetFinanceContribute(Int64 pKID)
        {
            FinanceContributeEntity financeContributeEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  PKID,JournalID,CID,FeeType,Amount,PayType,ArticleType,ArticleCount,RemitBillNo,InUser,InComeDate,InvoiceNo,PostNo,SendDate,Note,AddDate FROM dbo.FinanceContribute WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  PKID=@PKID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@PKID", DbType.Int64, pKID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                financeContributeEntity = MakeFinanceContribute(dr);
            }
            return financeContributeEntity;
        }

        #endregion

        #region 根据条件获取所有实体对象

        public List<FinanceContributeEntity> GetFinanceContributeList()
        {
            List<FinanceContributeEntity> financeContributeEntity = new List<FinanceContributeEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  PKID,JournalID,CID,FeeType,Amount,PayType,ArticleType,ArticleCount,RemitBillNo,InUser,InComeDate,InvoiceNo,PostNo,SendDate,Note,AddDate FROM dbo.FinanceContribute WITH(NOLOCK)");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                financeContributeEntity = MakeFinanceContributeList(dr);
            }
            return financeContributeEntity;
        }

        public List<FinanceContributeEntity> GetFinanceContributeList(FinanceContributeQuery query)
        {
            string strSql = @"SELECT a.*,b.Title,b.AuthorID,b.CNumber FROM dbo.FinanceContribute a with(nolock) 
                              INNER JOIN dbo.ContributionInfo b with(nolock) ON a.JournalID=b.JournalID and a.CID=b.CID";
            string where = FinanceContributeQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(where))
                strSql += " WHERE " + where;
            strSql += " ORDER BY " + FinanceContributeQueryToSQLOrder(query);
            return db.GetList<FinanceContributeEntity>(strSql, MakeFinanceContributeList);

        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<FinanceContributeEntity></returns>
        public Pager<FinanceContributeEntity> GetFinanceContributePageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("FinanceContribute", "PKID,JournalID,CID,FeeType,Amount,PayType,RemitBillNo,InUser,InComeDate,InvoiceNo,PostNo,SendDate,Note,AddDate", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<FinanceContributeEntity> pager = new Pager<FinanceContributeEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeFinanceContributeList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<FinanceContributeEntity> GetFinanceContributePageList(QueryBase query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("FinanceContribute", "PKID,JournalID,CID,FeeType,Amount,PayType,RemitBillNo,InUser,InComeDate,InvoiceNo,PostNo,SendDate,Note,AddDate", " PKID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<FinanceContributeEntity> pager = new Pager<FinanceContributeEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeFinanceContributeList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<FinanceContributeEntity> GetFinanceContributePageList(FinanceContributeQuery query)
        {
            string tableSql = @"SELECT {0} FROM dbo.FinanceContribute a with(nolock) 
                              INNER JOIN dbo.ContributionInfo b with(nolock) ON a.JournalID=b.JournalID and a.CID=b.CID";
            string where = FinanceContributeQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(where))
                tableSql += " WHERE " + where;
            string strSql = string.Format(tableSql, "a.*,b.Title,b.AuthorID,b.CNumber,ROW_NUMBER() OVER(ORDER BY " + FinanceContributeQueryToSQLOrder(query) + ") AS ROW_ID")
                , sumStr = string.Format(tableSql, "RecordCount=COUNT(1)");
            return db.GetPageList<FinanceContributeEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeFinanceContributeList);

        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        public bool AddFinanceContribute(FinanceContributeEntity financeContributeEntity)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    StringBuilder sqlCommandText = new StringBuilder();
                    sqlCommandText.Append(" @JournalID");
                    sqlCommandText.Append(", @CID");
                    sqlCommandText.Append(", @FeeType");
                    sqlCommandText.Append(", @Amount");
                    sqlCommandText.Append(", @ArticleType");
                    sqlCommandText.Append(", @ArticleCount");
                    sqlCommandText.Append(", @PayType");
                    sqlCommandText.Append(", @RemitBillNo");
                    sqlCommandText.Append(", @InUser");
                    sqlCommandText.Append(", @InComeDate");
                    sqlCommandText.Append(", @InvoiceNo");
                    sqlCommandText.Append(", @PostNo");
                    sqlCommandText.Append(", @SendDate");
                    sqlCommandText.Append(", @Note");
                    sqlCommandText.Append(", @Status");

                    DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.FinanceContribute ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

                    db.AddInParameter(cmd, "@JournalID", DbType.Int64, financeContributeEntity.JournalID);
                    db.AddInParameter(cmd, "@CID", DbType.Int64, financeContributeEntity.CID);
                    db.AddInParameter(cmd, "@FeeType", DbType.Byte, financeContributeEntity.FeeType);
                    db.AddInParameter(cmd, "@Amount", DbType.Decimal, financeContributeEntity.Amount);

                    db.AddInParameter(cmd, "@ArticleType", DbType.Int32, financeContributeEntity.ArticleType);
                    db.AddInParameter(cmd, "@ArticleCount", DbType.Decimal, financeContributeEntity.ArticleCount);

                    db.AddInParameter(cmd, "@PayType", DbType.Byte, financeContributeEntity.PayType);
                    db.AddInParameter(cmd, "@RemitBillNo", DbType.AnsiString, financeContributeEntity.RemitBillNo);
                    db.AddInParameter(cmd, "@InUser", DbType.Int64, financeContributeEntity.InUser);
                    db.AddInParameter(cmd, "@InComeDate", DbType.DateTime, financeContributeEntity.InComeDate);
                    db.AddInParameter(cmd, "@InvoiceNo", DbType.AnsiString, financeContributeEntity.InvoiceNo);
                    db.AddInParameter(cmd, "@PostNo", DbType.AnsiString, financeContributeEntity.PostNo);
                    db.AddInParameter(cmd, "@SendDate", DbType.DateTime, financeContributeEntity.SendDate);
                    db.AddInParameter(cmd, "@Note", DbType.AnsiString, financeContributeEntity.Note);
                    db.AddInParameter(cmd, "@Status", DbType.Byte, financeContributeEntity.Status);

                    bool result = db.ExecuteNonQuery(cmd, trans) > 0;

                    if (!result)
                        throw new Exception("新增稿件费用信息失败！");

                    if (financeContributeEntity.Status == 1 && (financeContributeEntity.FeeType == 1 || financeContributeEntity.FeeType == 2))
                    {
                        string strSql = string.Format("Update dbo.ContributionInfo set {0}=1 WHERE CID={1}"
                            , financeContributeEntity.FeeType == 1 ? "IsPayAuditFee" : "IsPayPageFee", financeContributeEntity.CID);
                        cmd = db.GetSqlStringCommand(strSql);
                        if (db.ExecuteNonQuery(cmd, trans) < 1)
                            throw new Exception("更新稿件表" + (financeContributeEntity.FeeType == 1 ? "审稿费" : "版面费") + "状态失败！");
                    }

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

        #endregion

        #region 更新数据

        public bool UpdateFinanceContribute(FinanceContributeEntity financeContributeEntity)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    StringBuilder whereCommandText = new StringBuilder();
                    whereCommandText.Append("  PKID=@PKID");
                    StringBuilder sqlCommandText = new StringBuilder();
                    sqlCommandText.Append(" FeeType=@FeeType");
                    sqlCommandText.Append(", Amount=@Amount");

                    sqlCommandText.Append(", ArticleType=@ArticleType");
                    sqlCommandText.Append(", ArticleCount=@ArticleCount");

                    sqlCommandText.Append(", PayType=@PayType");
                    sqlCommandText.Append(", RemitBillNo=@RemitBillNo");
                    sqlCommandText.Append(", InUser=@InUser");
                    sqlCommandText.Append(", InComeDate=@InComeDate");
                    sqlCommandText.Append(", InvoiceNo=@InvoiceNo");
                    sqlCommandText.Append(", PostNo=@PostNo");
                    sqlCommandText.Append(", SendDate=@SendDate");
                    sqlCommandText.Append(", Note=@Note");
                    sqlCommandText.Append(", Status=@Status");

                    DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.FinanceContribute SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

                    db.AddInParameter(cmd, "@PKID", DbType.Int64, financeContributeEntity.PKID);
                    db.AddInParameter(cmd, "@FeeType", DbType.Byte, financeContributeEntity.FeeType);
                    db.AddInParameter(cmd, "@Amount", DbType.Decimal, financeContributeEntity.Amount);

                    db.AddInParameter(cmd, "@ArticleType", DbType.Int32, financeContributeEntity.ArticleType);
                    db.AddInParameter(cmd, "@ArticleCount", DbType.Decimal, financeContributeEntity.ArticleCount);

                    db.AddInParameter(cmd, "@PayType", DbType.Byte, financeContributeEntity.PayType);
                    db.AddInParameter(cmd, "@RemitBillNo", DbType.AnsiString, financeContributeEntity.RemitBillNo);
                    db.AddInParameter(cmd, "@InUser", DbType.Int64, financeContributeEntity.InUser);
                    db.AddInParameter(cmd, "@InComeDate", DbType.DateTime, financeContributeEntity.InComeDate);
                    db.AddInParameter(cmd, "@InvoiceNo", DbType.AnsiString, financeContributeEntity.InvoiceNo);
                    db.AddInParameter(cmd, "@PostNo", DbType.AnsiString, financeContributeEntity.PostNo);
                    db.AddInParameter(cmd, "@SendDate", DbType.DateTime, financeContributeEntity.SendDate);
                    db.AddInParameter(cmd, "@Note", DbType.AnsiString, financeContributeEntity.Note);
                    db.AddInParameter(cmd, "@Status", DbType.Byte, financeContributeEntity.Status);

                    bool result = db.ExecuteNonQuery(cmd, trans) > 0;

                    if (!result)
                        throw new Exception("跟新稿件费用信息失败！");

                    if (financeContributeEntity.Status == 1 && (financeContributeEntity.FeeType == 1 || financeContributeEntity.FeeType == 2))
                    {
                        string strSql = string.Format("Update dbo.ContributionInfo set {0}=1 WHERE CID={1}"
                            , financeContributeEntity.FeeType == 1 ? "IsPayAuditFee" : "IsPayPageFee", financeContributeEntity.CID);
                        cmd = db.GetSqlStringCommand(strSql);
                        if (db.ExecuteNonQuery(cmd, trans) < 1)
                            throw new Exception("更新稿件表" + (financeContributeEntity.FeeType == 1 ? "审稿费" : "版面费") + "状态失败！");
                    }

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

        #endregion

        #region 删除对象

        #region 删除一个对象

        public bool DeleteFinanceContribute(FinanceContributeEntity financeContributeEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.FinanceContribute");
            sqlCommandText.Append(" WHERE  PKID=@PKID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            db.AddInParameter(cmd, "@PKID", DbType.Int64, financeContributeEntity.PKID);

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

        public bool DeleteFinanceContribute(Int64 pKID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.FinanceContribute");
            sqlCommandText.Append(" WHERE  PKID=@PKID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@PKID", DbType.Int64, pKID);
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

        #endregion

        #region 批量删除

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteFinanceContribute(Int64[] pKID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from FinanceContribute where ");

            for (int i = 0; i < pKID.Length; i++)
            {
                if (i > 0) sqlCommandText.Append(" or ");
                sqlCommandText.Append("( PKID=@PKID" + i + " )");
            }

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for (int i = 0; i < pKID.Length; i++)
            {
                db.AddInParameter(cmd, "@PKID" + i, DbType.Int64, pKID[i]);
            }
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

        #endregion

        #endregion

        #region 根据数据组装一个对象

        public FinanceContributeEntity MakeFinanceContribute(IDataReader dr)
        {
            FinanceContributeEntity financeContributeEntity = null;
            if (dr.Read())
            {
                financeContributeEntity = new FinanceContributeEntity();
                financeContributeEntity.PKID = (Int64)dr["PKID"];
                financeContributeEntity.JournalID = (Int64)dr["JournalID"];
                financeContributeEntity.CID = (Int64)dr["CID"];
                financeContributeEntity.FeeType = (Byte)dr["FeeType"];
                financeContributeEntity.Amount = (Decimal)dr["Amount"];

                financeContributeEntity.ArticleType = dr["ArticleType"] == System.DBNull.Value ? 0 : (Int32)dr["ArticleType"];
                financeContributeEntity.ArticleCount = dr["ArticleCount"] == System.DBNull.Value ? 0 : (Decimal)dr["ArticleCount"];

                financeContributeEntity.PayType = Convert.IsDBNull(dr["PayType"]) ? null : (Byte?)dr["PayType"];
                financeContributeEntity.RemitBillNo = Convert.IsDBNull(dr["RemitBillNo"]) ? null : (String)dr["RemitBillNo"];
                financeContributeEntity.InUser = (Int64)dr["InUser"];
                financeContributeEntity.InComeDate = Convert.IsDBNull(dr["InComeDate"]) ? null : (DateTime?)dr["InComeDate"];
                financeContributeEntity.InvoiceNo = Convert.IsDBNull(dr["InvoiceNo"]) ? null : (String)dr["InvoiceNo"];
                financeContributeEntity.PostNo = Convert.IsDBNull(dr["PostNo"]) ? null : (String)dr["PostNo"];
                financeContributeEntity.SendDate = Convert.IsDBNull(dr["SendDate"]) ? null : (DateTime?)dr["SendDate"];
                financeContributeEntity.Note = Convert.IsDBNull(dr["Note"]) ? null : (String)dr["Note"];
                financeContributeEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return financeContributeEntity;
        }

        public FinanceContributeEntity MakeFinanceContribute(DataRow dr)
        {
            FinanceContributeEntity financeContributeEntity = null;
            if (dr != null)
            {
                financeContributeEntity = new FinanceContributeEntity();
                financeContributeEntity.PKID = (Int64)dr["PKID"];
                financeContributeEntity.JournalID = (Int64)dr["JournalID"];
                financeContributeEntity.CID = (Int64)dr["CID"];
                financeContributeEntity.FeeType = (Byte)dr["FeeType"];
                financeContributeEntity.Amount = (Decimal)dr["Amount"];
                financeContributeEntity.ArticleType = (Int32)dr["ArticleType"];
                financeContributeEntity.ArticleCount = (Decimal)dr["ArticleCount"];
                financeContributeEntity.PayType = Convert.IsDBNull(dr["PayType"]) ? null : (Byte?)dr["PayType"];
                financeContributeEntity.RemitBillNo = Convert.IsDBNull(dr["RemitBillNo"]) ? null : (String)dr["RemitBillNo"];
                financeContributeEntity.InUser = (Int64)dr["InUser"];
                financeContributeEntity.InComeDate = Convert.IsDBNull(dr["InComeDate"]) ? null : (DateTime?)dr["InComeDate"];
                financeContributeEntity.InvoiceNo = Convert.IsDBNull(dr["InvoiceNo"]) ? null : (String)dr["InvoiceNo"];
                financeContributeEntity.PostNo = Convert.IsDBNull(dr["PostNo"]) ? null : (String)dr["PostNo"];
                financeContributeEntity.SendDate = Convert.IsDBNull(dr["SendDate"]) ? null : (DateTime?)dr["SendDate"];
                financeContributeEntity.Note = Convert.IsDBNull(dr["Note"]) ? null : (String)dr["Note"];
                financeContributeEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return financeContributeEntity;
        }
        #endregion

        #region 根据数据组装一组对象数据

        public List<FinanceContributeEntity> MakeFinanceContributeList(IDataReader dr)
        {
            List<FinanceContributeEntity> list = new List<FinanceContributeEntity>();
            while (dr.Read())
            {
                FinanceContributeEntity financeContributeEntity = new FinanceContributeEntity();
                financeContributeEntity.PKID = (Int64)dr["PKID"];
                financeContributeEntity.JournalID = (Int64)dr["JournalID"];
                financeContributeEntity.CID = (Int64)dr["CID"];
                financeContributeEntity.FeeType = (Byte)dr["FeeType"];
                financeContributeEntity.Amount = (Decimal)dr["Amount"];
                financeContributeEntity.ArticleType = (Int32)dr["ArticleType"];
                financeContributeEntity.ArticleCount = (Decimal)dr["ArticleCount"];
                financeContributeEntity.PayType = Convert.IsDBNull(dr["PayType"]) ? null : (Byte?)dr["PayType"];
                financeContributeEntity.RemitBillNo = Convert.IsDBNull(dr["RemitBillNo"]) ? null : (String)dr["RemitBillNo"];
                financeContributeEntity.InUser = (Int64)dr["InUser"];
                financeContributeEntity.InComeDate = Convert.IsDBNull(dr["InComeDate"]) ? null : (DateTime?)dr["InComeDate"];
                financeContributeEntity.InvoiceNo = Convert.IsDBNull(dr["InvoiceNo"]) ? null : (String)dr["InvoiceNo"];
                financeContributeEntity.PostNo = Convert.IsDBNull(dr["PostNo"]) ? null : (String)dr["PostNo"];
                financeContributeEntity.SendDate = Convert.IsDBNull(dr["SendDate"]) ? null : (DateTime?)dr["SendDate"];
                financeContributeEntity.Note = Convert.IsDBNull(dr["Note"]) ? null : (String)dr["Note"];
                financeContributeEntity.AddDate = (DateTime)dr["AddDate"];
                financeContributeEntity.Status = dr.GetDrValue<Byte>("Status");
                if (dr.HasColumn("Title"))
                    financeContributeEntity.Title = dr.GetDrValue<String>("Title");
                if (dr.HasColumn("AuthorID"))
                    financeContributeEntity.AuthorID = dr.GetDrValue<Int64>("AuthorID");
                if (dr.HasColumn("CNumber"))
                    financeContributeEntity.CNumber = dr.GetDrValue<String>("CNumber");
                list.Add(financeContributeEntity);
            }
            dr.Close();
            return list;
        }


        public List<FinanceContributeEntity> MakeFinanceContributeList(DataTable dt)
        {
            List<FinanceContributeEntity> list = new List<FinanceContributeEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FinanceContributeEntity financeContributeEntity = MakeFinanceContribute(dt.Rows[i]);
                    list.Add(financeContributeEntity);
                }
            }
            return list;
        }

        #endregion

        /// <summary>
        /// 缴费通知是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool FinanceContributeIsExists(FinanceContributeQuery query)
        {
            string strSql = "SELECT 1 FROM dbo.FinanceContribute WHERE JournalID=@JournalID and CID=@CID and FeeType=@FeeType";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@CID", DbType.Int64, query.CID);
            db.AddInParameter(cmd, "@FeeType", DbType.Byte, query.FeeType);
            return db.ExecuteScalar(cmd).TryParse<Int32>() == 1;
        }

        /// <summary>
        /// 获取财务通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceAccountPageList(ContributionInfoQuery query)
        {
            return GetFinanceInAccountPageList(query);
        }


        /// <summary>
        /// 获取财务入款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceInAccountPageList(ContributionInfoQuery query)
        {
            string orderby = "a.CID DESC";
            if (query.OrderStr != null)
            {
                orderby = "a.CID " + query.OrderStr + " | t.CID " + query.OrderStr + "";
            }

            StringBuilder strSql = null;
            strSql = new StringBuilder(@"
                              SELECT
                                    a.CID
                                    ,a.CNumber
                                    ,a.AuthorID as UserID
                                    ,ad.Address
                                    ,ad.InvoiceUnit
                                    ,ad.Tel
                                    ,ad.Mobile
                                    ,ad.ZipCode
                                    ,e.NoticeID as ReadingFeeNotice
                                    ,f.NoticeID as LayoutFeeNotice
                                    ,b.PKID as ReadingFeeID
                                    ,b.Status as ReadingFeeStatus
                                    ,b.Amount as ReadingFee
                                    ,b.Note
                                    ,c.PKID as LayoutFeeID
                                    ,c.Status as LayoutFeeStatus
                                    ,c.Amount as LayoutFee
                                    ,c.Note as PageNote
                                    ,a.Title
                                    ,dbo.fn_GetContributionCurrentEditor(a.JournalID,a.CID) as EditAuthorID
                                    ,dd.CAuthorID as FirstAuthorID
                                    ,dd.AuthorName as FirstAuthorName
                                    ,d.CAuthorID as CommunicationAuthorID
                                    ,d.AuthorName as CommunicationAuthorName
                                    ,a.AddDate
                                    ,a.Flag
                              FROM dbo.ContributionInfo a with(nolock)
                              LEFT JOIN  (select distinct (AuthorID),JournalID, Address,InvoiceUnit,Tel,Mobile,ZipCode  from  dbo.AuthorDetail  ) as  ad  ON a.AuthorID=ad.AuthorID AND a.JournalID=ad.JournalID
                              LEFT JOIN dbo.FinanceContribute b  ON a.JournalID=b.JournalID and a.CID=b.CID and b.FeeType=1 
                              LEFT JOIN dbo.FinanceContribute c  ON a.JournalID=c.JournalID and a.CID=c.CID and c.FeeType=2
                              INNER JOIN dbo.ContributionAuthor d  ON a.JournalID=d.JouranalID and a.CID=d.CID and d.IsCommunication=1
                              INNER JOIN dbo.ContributionAuthor dd  ON a.JournalID=dd.JouranalID and a.CID=dd.CID and dd.IsFirst=1
                              LEFT JOIN dbo.PayNotice e  ON a.JournalID=e.JournalID and a.CID=e.CID and e.PayType=1 
                              LEFT JOIN dbo.PayNotice f  ON a.JournalID=f.JournalID and a.CID=f.CID and f.PayType=2 
                              WHERE a.JournalID=@JournalID AND (a.Status<>-999 or a.Status=0 ) ");
            
            int recordCount = 0;
            List<SqlParameter> listParameters = new List<SqlParameter>();
            SqlParameter pJournalID = new SqlParameter("@JournalID", SqlDbType.BigInt);
            pJournalID.Value = query.JournalID;
            listParameters.Add(pJournalID);
            if (query.AuthorID != null)
            {
                SqlParameter pAuthorID = new SqlParameter("@AuthorID", SqlDbType.BigInt);
                pAuthorID.Value = query.AuthorID.Value;
                listParameters.Add(pAuthorID);
                strSql.Append(" AND a.AuthorID=@AuthorID");
            }

            if (!string.IsNullOrWhiteSpace(query.CNumber))
            {
                strSql.Append(" AND a.CNumber LIKE '%" + query.CNumber + "%'");
            }
            if (query.SubjectCat != null)
            {
                strSql.Append(" AND  a.SubjectCat LIKE '%" + query.SubjectCat + "%'");
            }
            query.Title = SecurityUtils.SafeSqlString(query.Title);

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                strSql.Append(" AND a.Title LIKE '%" + query.Title + "%'");
            }
            query.Keyword = SecurityUtils.SafeSqlString(query.Keyword);
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                strSql.Append(" AND a.Keywords LIKE '%" + query.Keyword + "%'");
            }
            if (!string.IsNullOrWhiteSpace(query.Flag))
            {
                SqlParameter pFlag = new SqlParameter("@Flag", SqlDbType.VarChar, 20);
                pFlag.Value = SecurityUtils.SafeSqlString(query.Flag);
                listParameters.Add(pFlag);
                strSql.Append(" AND a.Flag=@Flag");
            }
            // 第一作者
            if (!string.IsNullOrWhiteSpace(query.FirstAuthor))
            {
                SqlParameter PFirst = new SqlParameter("@AuthorName", SqlDbType.VarChar, 50);
                PFirst.Value = SecurityUtils.SafeSqlString(query.FirstAuthor);
                listParameters.Add(PFirst);
                strSql.Append(" AND dd.AuthorName = @AuthorName");
            }
            // 通信作者
            if (!string.IsNullOrWhiteSpace(query.CommunicationAuthor))
            {
                SqlParameter PCommunication = new SqlParameter("@AuthorName", SqlDbType.VarChar, 50);
                PCommunication.Value = SecurityUtils.SafeSqlString(query.CommunicationAuthor);
                listParameters.Add(PCommunication);
                strSql.Append(" AND d.AuthorName = @AuthorName");
            }
            // 投稿日期
            if (query.StartDate != null)
            {
                strSql.Append(" AND a.AddDate>='").Append(query.StartDate.Value.Date).Append("'");
            }
            if (query.EndDate != null)
            {
                strSql.Append(" AND a.AddDate<'").Append(query.EndDate.Value.AddDays(1).Date).Append("'");
            }

            // 录用年期
            if (query.Year != null)
            {
                strSql.Append(" AND a.Year='").Append(query.Year).Append("'");
            }
            if (query.Issue != null)
            {
                strSql.Append(" AND a.Issue='").Append(query.Issue).Append("'");
            }

            DataSet ds = new DataSet();
            if (query.IsReport)
            {
                DbCommand cmd = db.GetSqlStringCommand(strSql.ToString());
                foreach (SqlParameter para in listParameters)
                {
                    db.AddInParameter(cmd, para.ParameterName, para.DbType, para.Value);
                }
                ds = db.ExecuteDataSet(cmd);
            }
            else
            {
                ds = db.PageingQuery(query.CurrentPage, query.PageSize, strSql.ToString(), orderby, listParameters.ToArray(), ref recordCount);
            }
            Pager<FinanceAccountEntity> pager = new Pager<FinanceAccountEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                List<FinanceAccountEntity> list = new List<FinanceAccountEntity>();
                if (ds != null)
                {
                    FinanceAccountEntity model = null;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        model = new FinanceAccountEntity();
                        model.CID = dr.GetDataRowValue<Int64>("CID");
                        model.CNumber = dr.GetDataRowValue<String>("CNumber");
                        model.UserID = dr.GetDataRowValue<Int64>("UserID");
                        model.Address = dr.GetDataRowValue<String>("Address");
                        model.Tel = dr.GetDataRowValue<String>("Tel");
                        model.Mobile = dr.GetDataRowValue<String>("Mobile");
                        model.ZipCode = dr.GetDataRowValue<String>("ZipCode");
                        model.InvoiceUnit = dr.GetDataRowValue<String>("InvoiceUnit");
                        model.ReadingFeeNotice = dr.GetDataRowValue<Int64>("ReadingFeeNotice");
                        model.ReadingFeeNoticeStatus = dr.GetDataRowValue<Int64>("ReadingFeeNotice") == 0 ? "未通知" : "已通知";
                        model.LayoutFeeNoticeStatus = dr.GetDataRowValue<Int64>("LayoutFeeNotice") == 0 ? "未通知" : "已通知";
                        model.LayoutFeeNotice = dr.GetDataRowValue<Int64>("LayoutFeeNotice");
                        model.ReadingFeeID = dr.GetDataRowValue<Int64>("ReadingFeeID");
                        model.ReadingFeeStatus = dr.GetDataRowValue<Byte?>("ReadingFeeStatus");
                        model.ReadingFee = dr.GetDataRowValue<Decimal>("ReadingFee");
                        model.Note = dr.GetDataRowValue<String>("Note");
                        model.LayoutFeeID = dr.GetDataRowValue<Int64>("LayoutFeeID");
                        model.LayoutFeeStatus = dr.GetDataRowValue<Byte?>("LayoutFeeStatus");
                        model.LayoutFee = dr.GetDataRowValue<Decimal>("LayoutFee");
                        model.PageNote = dr.GetDataRowValue<String>("PageNote");
                        model.Title = dr.GetDataRowValue<String>("Title");
                        model.EditAuthorID = dr.GetDataRowValue<Int64>("EditAuthorID");
                        model.FirstAuthorID = dr.GetDataRowValue<Int64>("FirstAuthorID");
                        model.AuthorID = dr.GetDataRowValue<Int64>("UserID");
                        model.FirstAuthor = dr.GetDataRowValue<String>("FirstAuthorName");
                        model.CommunicationAuthorID = dr.GetDataRowValue<Int64>("CommunicationAuthorID");
                        model.CommunicationAuthor = dr.GetDataRowValue<String>("CommunicationAuthorName");
                        model.AddDate = dr.GetDataRowValue<DateTime>("AddDate");
                        model.Flag = dr.GetDataRowValue<String>("Flag");
                        list.Add(model);
                    }
                }
                pager.ItemList = list;
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        /// <summary>
        /// /// 获取稿费统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceGaoFeePageList(ContributionInfoQuery query)
        {
            string tableSql = @"SELECT {0}
                            FROM dbo.ContributionInfo a with(nolock)
                            LEFT JOIN  (select distinct (AuthorID),JournalID, WorkUnit,Address,InvoiceUnit,Tel,Mobile,ZipCode  from  dbo.AuthorDetail  ) as  ad  ON a.AuthorID=ad.AuthorID AND a.JournalID=ad.JournalID
                            LEFT JOIN dbo.FinanceContribute bb  ON a.JournalID=bb.JournalID and a.CID=bb.CID and bb.FeeType=2
                            LEFT JOIN dbo.FinanceContribute b  ON a.JournalID=b.JournalID and a.CID=b.CID and b.FeeType=4
                            INNER JOIN dbo.ContributionAuthor d  ON a.JournalID=d.JouranalID and a.CID=d.CID and d.IsCommunication=1
                            INNER JOIN dbo.ContributionAuthor dd  ON a.JournalID=dd.JouranalID and a.CID=dd.CID and dd.IsFirst=1
                            WHERE " + GetFinanceGaoFeeFilter(query);
            string strSql = string.Format(tableSql, @"a.CID
                                                     ,a.CNumber
                                                     ,a.AuthorID as UserID
                                                     ,ad.WorkUnit
                                                     ,ad.Address
                                                     ,ad.InvoiceUnit
                                                     ,ad.Tel
                                                     ,ad.Mobile
                                                     ,ad.ZipCode
                                                     ,b.PKID as ArticleFeeID
                                                     ,b.Status as ArticleFeeStatus
                                                     ,b.Amount as ArticleFee
                                                     ,b.ArticleType
                                                     ,b.ArticleCount
                                                     ,b.Note as ArticleNote
                                                     ,bb.Amount as PageFee
                                                     ,bb.Note as PageFeeNote
                                                     ,bb.Status as PageFeeStatus
                                                     ,a.Title
                                                     ,dbo.fn_GetContributionCurrentEditor(a.JournalID,a.CID) as EditAuthorID
                                                     ,dd.CAuthorID as FirstAuthorID
                                                     ,dd.AuthorName as FirstAuthorName
                                                     ,d.CAuthorID as CommunicationAuthorID
                                                     ,d.AuthorName as CommunicationAuthorName
                                                     ,a.AddDate
                                                     ,ROW_NUMBER() OVER(ORDER BY a.AddDate desc) AS ROW_ID"), sumStr = string.Empty;
            if (!query.IsReport)
            {
                strSql = string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex);
                if (query.isPageFeeGet)
                    sumStr = string.Format(tableSql, "RecordCount=COUNT(1),Money=SUM(bb.Amount)");
                else
                    sumStr = string.Format(tableSql, "RecordCount=COUNT(1),Money=SUM(b.Amount)");
            }
            return db.GetPageList<FinanceAccountEntity>(strSql
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                    pager.Money = dr.GetDrValue<Decimal>("Money");
                }
                , (dr) =>
                {
                    List<FinanceAccountEntity> list = new List<FinanceAccountEntity>();
                    FinanceAccountEntity model = null;
                    while (dr.Read())
                    {
                        model = new FinanceAccountEntity();
                        model.CID = dr.GetDrValue<Int64>("CID");
                        model.CNumber = dr.GetDrValue<String>("CNumber");
                        model.UserID = dr.GetDrValue<Int64>("UserID");
                        model.WorkUnit = dr.GetDrValue<String>("WorkUnit");
                        model.Address = dr.GetDrValue<String>("Address");
                        model.Tel = dr.GetDrValue<String>("Tel");
                        model.Mobile = dr.GetDrValue<String>("Mobile");
                        model.ZipCode = dr.GetDrValue<String>("ZipCode");
                        model.InvoiceUnit = dr.GetDrValue<String>("InvoiceUnit");
                        model.ArticlePaymentFee = dr.GetDrValue<Decimal>("ArticleFee");
                        model.ArticlePaymentFeeID = dr.GetDrValue<Int64>("ArticleFeeID");
                        model.ArticleType = dr["ArticleType"] == System.DBNull.Value ? "-1" : dr["ArticleType"].ToString();
                        model.ArticleCount = dr["ArticleCount"] == System.DBNull.Value ? (Decimal)0.00 : dr.GetDrValue<Decimal>("ArticleCount");
                        model.ArticlePaymentNote = dr.GetDrValue<String>("ArticleNote");
                        model.ArticlePaymentFeeStatus = dr.GetDrValue<Byte?>("ArticleFeeStatus");

                        model.LayoutFee = dr.GetDrValue<Decimal>("PageFee");
                        model.PageNote = dr.GetDrValue<String>("PageFeeNote");
                        model.LayoutFeeStatus = dr.GetDrValue<Byte?>("PageFeeStatus");
                        
                        model.Title = dr.GetDrValue<String>("Title");
                        model.EditAuthorID = dr.GetDrValue<Int64>("EditAuthorID");
                        model.FirstAuthorID = dr.GetDrValue<Int64>("FirstAuthorID");
                        model.AuthorID = dr.GetDrValue<Int64>("UserID");
                        model.FirstAuthor = dr.GetDrValue<String>("FirstAuthorName");
                        model.CommunicationAuthorID = dr.GetDrValue<Int64>("CommunicationAuthorID");
                        model.CommunicationAuthor = dr.GetDrValue<String>("CommunicationAuthorName");
                        model.AddDate = dr.GetDrValue<DateTime>("AddDate");
                        list.Add(model);
                    }
                    return list;
                });


            //========================================
//            string orderby = "a.CID DESC";
//            if (query.OrderStr != null)
//            {
//                orderby = "a.CID " + query.OrderStr + " | t.CID " + query.OrderStr + "";
//            }

//            StringBuilder strSql = null;
//            strSql = new StringBuilder(@"
//                              SELECT
//                                    a.CID
//                                    ,a.CNumber
//                                    ,a.AuthorID as UserID
//                                    ,ad.Address
//                                    ,ad.InvoiceUnit
//                                    ,ad.Tel
//                                    ,ad.Mobile
//                                    ,ad.ZipCode
//                                    ,b.PKID as ArticleFeeID
//                                    ,b.Status as ArticleFeeStatus
//                                    ,b.Amount as ArticleFee
//                                    ,b.ArticleType
//                                    ,b.ArticleCount
//                                    ,b.Note as ArticleNote
//                                    ,a.Title
//                                    ,dbo.fn_GetContributionCurrentEditor(a.JournalID,a.CID) as EditAuthorID
//                                    ,dd.CAuthorID as FirstAuthorID
//                                    ,dd.AuthorName as FirstAuthorName
//                                    ,d.CAuthorID as CommunicationAuthorID
//                                    ,d.AuthorName as CommunicationAuthorName
//                                    ,a.AddDate
//                                    ,a.Flag
//                              FROM dbo.ContributionInfo a with(nolock)
//                              LEFT JOIN  (select distinct (AuthorID),JournalID, Address,InvoiceUnit,Tel,Mobile,ZipCode  from  dbo.AuthorDetail  ) as  ad  ON a.AuthorID=ad.AuthorID AND a.JournalID=ad.JournalID
//                              LEFT JOIN dbo.FinanceContribute b  ON a.JournalID=b.JournalID and a.CID=b.CID and b.FeeType=4
//                              INNER JOIN dbo.ContributionAuthor d  ON a.JournalID=d.JouranalID and a.CID=d.CID and d.IsCommunication=1
//                              INNER JOIN dbo.ContributionAuthor dd  ON a.JournalID=dd.JouranalID and a.CID=dd.CID and dd.IsFirst=1
//                              WHERE a.JournalID=@JournalID AND (a.Status<>-999 or a.Status=0 ) ");

//            int recordCount = 0;
//            List<SqlParameter> listParameters = new List<SqlParameter>();
//            SqlParameter pJournalID = new SqlParameter("@JournalID", SqlDbType.BigInt);
//            pJournalID.Value = query.JournalID;
//            listParameters.Add(pJournalID);
            
//            // 录用年期
//            if (query.Year != null)
//            {
//                strSql.Append(" AND a.Year='").Append(query.Year).Append("'");
//            }
//            if (query.Issue != null)
//            {
//                strSql.Append(" AND a.Issue='").Append(query.Issue).Append("'");
//            }

//            DataSet ds = new DataSet();
//            if (query.IsReport)
//            {
//                DbCommand cmd = db.GetSqlStringCommand(strSql.ToString());
//                foreach (SqlParameter para in listParameters)
//                {
//                    db.AddInParameter(cmd, para.ParameterName, para.DbType, para.Value);
//                }
//                ds = db.ExecuteDataSet(cmd);
//            }
//            else
//            {
//                ds = db.PageingQuery(query.CurrentPage, query.PageSize, strSql.ToString(), orderby, listParameters.ToArray(), ref recordCount);
//            }
//            Pager<FinanceAccountEntity> pager = new Pager<FinanceAccountEntity>();
//            if (ds != null && ds.Tables.Count > 0)
//            {
//                List<FinanceAccountEntity> list = new List<FinanceAccountEntity>();
//                if (ds != null)
//                {
//                    FinanceAccountEntity model = null;
//                    foreach (DataRow dr in ds.Tables[0].Rows)
//                    {
//                        model = new FinanceAccountEntity();
//                        model.CID = dr.GetDataRowValue<Int64>("CID");
//                        model.CNumber = dr.GetDataRowValue<String>("CNumber");
//                        model.UserID = dr.GetDataRowValue<Int64>("UserID");
//                        model.Address = dr.GetDataRowValue<String>("Address");
//                        model.Tel = dr.GetDataRowValue<String>("Tel");
//                        model.Mobile = dr.GetDataRowValue<String>("Mobile");
//                        model.ZipCode = dr.GetDataRowValue<String>("ZipCode");
//                        model.InvoiceUnit = dr.GetDataRowValue<String>("InvoiceUnit");
//                        model.ArticlePaymentFee = dr.GetDataRowValue<Decimal>("ArticleFee");
//                        model.ArticlePaymentFeeID = dr.GetDataRowValue<Int64>("ArticleFeeID");
//                        model.ArticleType = dr["ArticleType"] == System.DBNull.Value ? "-1" : dr["ArticleType"].ToString();
//                        model.ArticleCount = dr["ArticleCount"] == System.DBNull.Value ? (Decimal)0.00 : dr.GetDataRowValue<Decimal>("ArticleCount");
//                        model.ArticlePaymentNote = dr.GetDataRowValue<String>("ArticleNote");
//                        model.ArticlePaymentFeeStatus = dr.GetDataRowValue<Byte?>("ArticleFeeStatus");
//                        model.Title = dr.GetDataRowValue<String>("Title");
//                        model.EditAuthorID = dr.GetDataRowValue<Int64>("EditAuthorID");
//                        model.FirstAuthorID = dr.GetDataRowValue<Int64>("FirstAuthorID");
//                        model.AuthorID = dr.GetDataRowValue<Int64>("UserID");
//                        model.FirstAuthor = dr.GetDataRowValue<String>("FirstAuthorName");
//                        model.CommunicationAuthorID = dr.GetDataRowValue<Int64>("CommunicationAuthorID");
//                        model.CommunicationAuthor = dr.GetDataRowValue<String>("CommunicationAuthorName");
//                        model.AddDate = dr.GetDataRowValue<DateTime>("AddDate");
//                        model.Flag = dr.GetDataRowValue<String>("Flag");
//                        list.Add(model);
//                    }
//                }
//                pager.ItemList = list;
//            }
//            pager.CurrentPage = query.CurrentPage;
//            pager.PageSize = query.PageSize;
//            pager.TotalRecords = recordCount;
//            return pager;
        }

        private string GetFinanceGaoFeeFilter(ContributionInfoQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" a.JournalID=" + query.JournalID);
            if (query.Year != null)
                strFilter.Append(" and a.Year=").Append(query.Year.Value);
            if (query.Issue != null)
                strFilter.Append(" and a.Issue=").Append(query.Issue.Value);
            return strFilter.ToString();
        }

        /// <summary>
        /// 获取财务出款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceOutAccountPageList(ContributionInfoQuery query)
        {
            StringBuilder strSql = new StringBuilder(@"
                              SELECT
                                    a.CID
                                    ,a.CNumber
                                    ,a.AuthorID as UserID
                                    ,ad.Address
                                    ,ad.InvoiceUnit
                                    ,ad.Tel
                                    ,ad.Mobile
                                    ,b.PKID as ReadingFeeID
                                    ,b.Status as ReadingFeeStatus
                                    ,b.Amount as ReadingFee
                                    ,a.Title
                                    ,d.CAuthorID as AuthorID
                                    ,d.AuthorName
                                    ,d.WorkUnit
                                    ,a.AddDate
                                    ,a.Year
                                    ,a.Issue
                              FROM dbo.ContributionInfo a with(nolock)
                              LEFT JOIN  (select distinct (AuthorID),JournalID, Address,InvoiceUnit,Tel,Mobile  from  dbo.AuthorDetail  ) as  ad  ON a.AuthorID=ad.AuthorID AND a.JournalID=ad.JournalID
                              LEFT JOIN dbo.FinanceContribute b  ON a.JournalID=b.JournalID and a.CID=b.CID and b.FeeType=2
                              INNER JOIN dbo.ContributionAuthor d  ON a.JournalID=d.JouranalID and a.CID=d.CID and d.IsCommunication=1
                              WHERE a.JournalID=@JournalID AND (a.Status<>-999 or a.Status=0 ) AND a.Year>0 AND a.Issue>0  ");
            int recordCount = 0;
            List<SqlParameter> listParameters = new List<SqlParameter>();
            SqlParameter pJournalID = new SqlParameter("@JournalID", SqlDbType.BigInt);
            pJournalID.Value = query.JournalID;
            listParameters.Add(pJournalID);


            if (!string.IsNullOrWhiteSpace(query.CNumber))
            {
                strSql.Append(" AND a.CNumber LIKE '%" + query.CNumber + "%'");
            }

            query.Title = SecurityUtils.SafeSqlString(query.Title);

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                strSql.Append(" AND a.Title LIKE '%" + query.Title + "%'");
            }

            if (query.Year > 0)
            {
                strSql.Append(" AND a.Year=" + query.Year);
            }

            if (query.Issue > 0)
            {
                strSql.Append(" AND a.Issue=" + query.Issue);
            }

            // 投稿日期
            if (query.StartDate != null)
            {
                strSql.Append(" AND a.AddDate>='").Append(query.StartDate.Value.Date).Append("'");
            }
            if (query.EndDate != null)
            {
                strSql.Append(" AND a.AddDate<'").Append(query.EndDate.Value.AddDays(1).Date).Append("'");
            }
            DataSet ds = new DataSet();
            if (query.IsReport)
            {
                DbCommand cmd = db.GetSqlStringCommand(strSql.ToString());
                foreach (SqlParameter para in listParameters)
                {
                    db.AddInParameter(cmd, para.ParameterName, para.DbType, para.Value);
                }
                ds = db.ExecuteDataSet(cmd);
            }
            else
            {
                ds = db.PageingQuery(query.CurrentPage, query.PageSize, strSql.ToString(), "a.CNumber DESC", listParameters.ToArray(), ref recordCount);
            }
            Pager<FinanceAccountEntity> pager = new Pager<FinanceAccountEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                List<FinanceAccountEntity> list = new List<FinanceAccountEntity>();
                if (ds != null)
                {
                    FinanceAccountEntity model = null;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        model = new FinanceAccountEntity();
                        model.CID = dr.GetDataRowValue<Int64>("CID");
                        model.CNumber = dr.GetDataRowValue<String>("CNumber");
                        model.UserID = dr.GetDataRowValue<Int64>("UserID");
                        model.Address = dr.GetDataRowValue<String>("Address");
                        model.Tel = dr.GetDataRowValue<String>("Tel");
                        model.Mobile = dr.GetDataRowValue<String>("Mobile");
                        model.InvoiceUnit = dr.GetDataRowValue<String>("InvoiceUnit");
                        model.ReadingFeeID = dr.GetDataRowValue<Int64>("ReadingFeeID");
                        model.ReadingFeeStatus = dr.GetDataRowValue<Byte?>("ReadingFeeStatus");
                        model.LayoutFee = dr.GetDataRowValue<Decimal>("ReadingFee");
                        model.Title = dr.GetDataRowValue<String>("Title");
                        model.AuthorID = dr.GetDataRowValue<Int64>("AuthorID");
                        model.FirstAuthor = dr.GetDataRowValue<String>("AuthorName");
                        model.CommunicationAuthor = dr.GetDataRowValue<String>("AuthorName");
                        model.AddDate = dr.GetDataRowValue<DateTime>("AddDate");
                        model.Year = dr.IsNull("Year") ? 0 : TypeParse.ToInt(dr["Year"]);
                        model.Issue = dr.IsNull("Issue") ? 0 : TypeParse.ToInt(dr["Issue"]);
                        list.Add(model);
                    }
                }
                pager.ItemList = list;
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        #region 获取财务统计一览表分页数据
        /// <summary>
        /// 获取财务统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceContributeEntity> GetFinanceGlancePageList(FinanceContributeQuery query)
        {
            string tableSql = @"SELECT {0}
                            FROM dbo.ContributionInfo a with(nolock)
                            LEFT JOIN  (select distinct (AuthorID),JournalID,AuthorName,Address,InvoiceUnit,Tel,Mobile  from  dbo.AuthorDetail  ) as  ad ON a.AuthorID=ad.AuthorID AND a.JournalID=ad.JournalID
                            INNER JOIN dbo.FinanceContribute b  ON a.JournalID=b.JournalID and a.CID=b.CID and b.FeeType!=4
                            LEFT JOIN dbo.PayNotice c with(nolock) ON b.JournalID=c.JournalID and b.CID=c.CID and b.FeeType=c.PayType
                            LEFT JOIN dbo.ContributionAuthor d with(nolock) ON d.JouranalID=a.JournalID and d.CID=a.CID and d.IsFirst=1
                            LEFT JOIN dbo.ContributionAuthor e with(nolock) ON e.JouranalID=a.JournalID and e.CID=a.CID and e.IsCommunication=1
                            WHERE " + GetFinanceGlanceFilter(query);
            string strSql = string.Format(tableSql, @"b.PKID
                                                     ,a.CID
                                                     ,a.CNumber
                                                     ,a.Title
                                                     ,a.AuthorID
                                                     ,ad.AuthorName
                                                     ,ad.Address
                                                     ,ad.InvoiceUnit
                                                     ,ad.Tel
                                                     ,ad.Mobile
                                                     ,d.CAuthorID as FirstAuthorID
                                                     ,d.AuthorName as FirstAuthorName
                                                     ,d.WorkUnit
                                                     ,e.CAuthorID as CommunicationAuthorID
                                                     ,e.AuthorName as CommunicationAuthorName
                                                     ,b.FeeType
                                                     ,c.Amount as ShouldMoney
                                                     ,b.Amount as SolidMoney
                                                     ,b.InUser
                                                     ,b.InComeDate
                                                     ,b.InvoiceNo
                                                     ,b.RemitBillNo
                                                     ,b.PostNo
                                                     ,b.SendDate
                                                     ,b.Note
                                                     ,ROW_NUMBER() OVER(ORDER BY a.AddDate desc) AS ROW_ID"), sumStr = string.Empty;
            if (!query.IsReport)
            {
                strSql = string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex);
                sumStr = string.Format(tableSql, "RecordCount=COUNT(1),Money=SUM(b.Amount)");
            }
            return db.GetPageList<FinanceContributeEntity>(strSql
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                    pager.Money = dr.GetDrValue<Decimal>("Money");
                }
                , (dr) =>
                {
                    List<FinanceContributeEntity> list = new List<FinanceContributeEntity>();
                    FinanceContributeEntity model = null;
                    while (dr.Read())
                    {
                        model = new FinanceContributeEntity();
                        model.PKID = dr.GetDrValue<Int64>("PKID");
                        model.CID = dr.GetDrValue<Int64>("CID");
                        model.CNumber = dr.GetDrValue<String>("CNumber");
                        model.Title = dr.GetDrValue<String>("Title");
                        model.UserID = dr.GetDrValue<Int64>("AuthorID");
                        model.Address = dr.GetDrValue<String>("Address");
                        model.Tel = dr.GetDrValue<String>("Tel");
                        model.Mobile = dr.GetDrValue<String>("Mobile");
                        model.InvoiceUnit = dr.GetDrValue<String>("InvoiceUnit");
                        model.AuthorID = dr.GetDrValue<Int64>("AuthorID");
                        model.AuthorName = dr.GetDrValue<String>("AuthorName");
                        model.FirstAuthorID = dr.GetDrValue<Int64>("FirstAuthorID");
                        model.FirstAuthorName = dr.GetDrValue<String>("FirstAuthorName");
                        model.CommunicationAuthorID = dr.GetDrValue<Int64>("CommunicationAuthorID");
                        model.CommunicationAuthorName = dr.GetDrValue<String>("CommunicationAuthorName");
                        model.WorkUnit = dr.GetDrValue<String>("WorkUnit");
                        model.FeeType = dr.GetDrValue<Byte>("FeeType");
                        model.ShouldMoney = dr.GetDrValue<Decimal>("ShouldMoney");
                        model.Amount = dr.GetDrValue<Decimal>("SolidMoney");
                        model.InUser = dr.GetDrValue<Int64>("InUser");
                        model.InComeDate = dr.GetDrValue<DateTime?>("InComeDate");
                        model.InvoiceNo = dr.GetDrValue<String>("InvoiceNo");
                        model.RemitBillNo = dr.GetDrValue<String>("RemitBillNo");
                        model.PostNo = dr.GetDrValue<String>("PostNo");
                        model.SendDate = dr.GetDrValue<DateTime?>("SendDate");
                        model.Note = dr.GetDrValue<String>("Note");
                        list.Add(model);
                    }
                    return list;
                });

        }

        private string GetFinanceGlanceFilter(FinanceContributeQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" a.JournalID=" + query.JournalID);
            if (query.AuthorID != null)
                strFilter.Append(" and a.AuthorID=").Append(query.AuthorID.Value);
            if (query.FeeType != null)
                strFilter.Append(" and b.FeeType=").Append(query.FeeType.Value);
            if (query.Status != null)
                strFilter.Append(" and b.Status=").Append(query.Status.Value);
            if (query.CID != null)
                strFilter.Append(" and a.CID=").Append(query.CID.Value);
            query.Title = SecurityUtils.SafeSqlString(query.Title);
            if (!string.IsNullOrWhiteSpace(query.Title))
                strFilter.Append(" and a.Title like '%").Append(query.Title).Append("%'");
            query.CNumber = SecurityUtils.SafeSqlString(query.CNumber);
            if (!string.IsNullOrWhiteSpace(query.CNumber))
                strFilter.Append(" and a.CNumber like '%").Append(query.CNumber).Append("%'");
            if (!string.IsNullOrWhiteSpace(query.FirstAuthor))
                strFilter.Append(" and d.AuthorName='").Append(query.FirstAuthor).Append("'");

            if (query.Year != null)
                strFilter.Append(" and a.Year='").Append(query.Year).Append("'");
            if (query.Issue != null)
                strFilter.Append(" and a.Issue='").Append(query.Issue).Append("'");

            if (query.StartDate != null)
                strFilter.Append(" and b.InComeDate>='").Append(query.StartDate.Value.Date).Append("'");
            if (query.EndDate != null)
                strFilter.Append(" and b.InComeDate<'").Append(query.EndDate.Value.AddDays(1).Date).Append("'");
            return strFilter.ToString();
        }
        #endregion

        

        #region 获取版面费报表分页数据
        /// <summary>
        /// 获取版面费报表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceContributeEntity> GetFinancePageFeeReportPageList(FinanceContributeQuery query)
        {
            string tableSql = @"SELECT {0}
                            FROM dbo.ContributionInfo a with(nolock)
                            LEFT JOIN  (select distinct (AuthorID),JournalID, Address,InvoiceUnit,Tel,Mobile  from  dbo.AuthorDetail  ) as  ad ON a.AuthorID=ad.AuthorID AND a.JournalID=ad.JournalID
                            LEFT JOIN dbo.PayNotice b with(nolock) ON b.JournalID=a.JournalID and b.CID=a.CID and b.PayType=2                            
                            LEFT JOIN dbo.FinanceContribute c  ON c.JournalID=b.JournalID and c.CID=b.CID and c.FeeType=2 and c.Status=1 
                            INNER JOIN dbo.ContributionAuthor d with(nolock) ON a.JournalID=d.JouranalID and a.CID=d.CID and d.IsCommunication=1
                            WHERE " + GetFinancePageFeeReportFilter(query);
            string strSql = string.Format(tableSql, @"c.PKID,a.CID,a.CNumber,a.Title,a.AuthorID as UserID,a.Year,a.Issue,a.AddDate,dbo.fn_GetContributionCurrentEditor(a.JournalID,a.CID) as EditAuthorID,ad.Address,ad.InvoiceUnit,ad.Tel,ad.Mobile,d.CAuthorID as AuthorID,d.AuthorName,d.WorkUnit,
	                         b.Amount as NoticeMoney,c.Amount as PageMoney,c.FeeType,c.InUser,c.InComeDate,c.InvoiceNo,c.RemitBillNo,c.PostNo,c.SendDate,c.Note,b.NoticeID as PageNoticeID,d.Email,d.ZipCode,ROW_NUMBER() OVER(ORDER BY a.AddDate desc) AS ROW_ID")
                 , sumStr = string.Empty;
            if (!query.IsReport)
            {
                strSql = string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex);
                sumStr = string.Format(tableSql, "RecordCount=COUNT(1),PageMoneyTotal=SUM(c.Amount)");
            }
            return db.GetPageList<FinanceContributeEntity>(strSql
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                    pager.PageMoney = dr.GetDrValue<Decimal>("PageMoneyTotal");
                }
                , (dr) =>
                {
                    List<FinanceContributeEntity> list = new List<FinanceContributeEntity>();
                    FinanceContributeEntity model = null;
                    while (dr.Read())
                    {
                        model = new FinanceContributeEntity();
                        model.PKID = dr.GetDrValue<Int64>("PKID");
                        model.CID = dr.GetDrValue<Int64>("CID");
                        model.CNumber = dr.GetDrValue<String>("CNumber");
                        model.Title = dr.GetDrValue<String>("Title");
                        model.UserID = dr.GetDrValue<Int64>("UserID");
                        model.EditAuthorID = dr.GetDrValue<Int64>("EditAuthorID");
                        model.Address = dr.GetDrValue<String>("Address");
                        model.ZipCode = dr.GetDrValue<String>("ZipCode");
                        model.Tel = dr.GetDrValue<String>("Tel");
                        model.Mobile = dr.GetDrValue<String>("Mobile");
                        model.InvoiceUnit = dr.GetDrValue<String>("InvoiceUnit");
                        model.AuthorID = dr.GetDrValue<Int64>("AuthorID");
                        model.AuthorName = dr.GetDrValue<String>("AuthorName");
                        model.Email = dr.GetDrValue<String>("Email");
                        model.WorkUnit = dr.GetDrValue<String>("WorkUnit");
                        model.PageMoney = dr.GetDrValue<Decimal>("PageMoney");
                        model.PageMoneyNotice = dr.GetDrValue<Decimal>("NoticeMoney");
                        model.FeeType = dr.GetDrValue<Byte>("FeeType");
                        model.InUser = dr.GetDrValue<Int64>("InUser");
                        model.InComeDate = dr.GetDrValue<DateTime?>("InComeDate");
                        model.InvoiceNo = dr.GetDrValue<String>("InvoiceNo");
                        model.RemitBillNo = dr.GetDrValue<String>("RemitBillNo");
                        model.PostNo = dr.GetDrValue<String>("PostNo");
                        model.SendDate = dr.GetDrValue<DateTime?>("SendDate");
                        model.Note = dr.GetDrValue<String>("Note") == null ? "" : dr.GetDrValue<String>("Note");
                        model.PageNoticeID = dr.GetDrValue<Int64>("PageNoticeID");
                        list.Add(model);
                    }
                    return list;
                });

        }

        private string GetFinancePageFeeReportFilter(FinanceContributeQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" a.JournalID=" + query.JournalID);
            if (query.AuthorID != null)
                strFilter.Append(" and a.AuthorID=").Append(query.AuthorID.Value);
            if (query.FeeType != null)
                strFilter.Append(" and c.FeeType=").Append(query.FeeType.Value);
            if (query.Status != null)
                strFilter.Append(" and c.Status=").Append(query.Status.Value);
            if (query.CID != null)
                strFilter.Append(" and a.CID=").Append(query.CID.Value);
            query.Title = SecurityUtils.SafeSqlString(query.Title);
            if (!string.IsNullOrWhiteSpace(query.Title))
                strFilter.Append(" and a.Title like '%").Append(query.Title).Append("%'");
            query.CNumber = SecurityUtils.SafeSqlString(query.CNumber);
            if (!string.IsNullOrWhiteSpace(query.CNumber))
                strFilter.Append(" and a.CNumber like '%").Append(query.CNumber).Append("%'");
            query.Keyword = SecurityUtils.SafeSqlString(query.Keyword);
            if (!string.IsNullOrWhiteSpace(query.Keyword))
                strFilter.Append(" and a.Keyword like '%").Append(query.Keyword).Append("%'");
            query.FirstAuthor = SecurityUtils.SafeSqlString(query.FirstAuthor);
            if (!string.IsNullOrWhiteSpace(query.FirstAuthor))
                strFilter.Append(" and d.AuthorName='").Append(query.FirstAuthor).Append("'");
            if (query.Year != null)
                strFilter.Append(" and a.Year='").Append(query.Year.Value).Append("'");
            if (query.Issue != null)
                strFilter.Append(" and a.Issue='").Append(query.Issue.Value).Append("'");

            if (query.StartDate != null)
                strFilter.Append(" and c.InComeDate>='").Append(query.StartDate.Value.Date).Append("'");
            if (query.EndDate != null)
                strFilter.Append(" and c.InComeDate<'").Append(query.EndDate.Value.AddDays(1).Date).Append("'");
            return strFilter.ToString();
        } 
        #endregion

        

        


        

    }
}

