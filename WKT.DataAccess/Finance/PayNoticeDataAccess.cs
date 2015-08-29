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

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class PayNoticeDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db = null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public PayNoticeDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if (db == null)
                throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static PayNoticeDataAccess _instance = new PayNoticeDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static PayNoticeDataAccess Instance
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
        public string PayNoticeQueryToSQLWhere(PayNoticeQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" a.JournalID=" + query.JournalID);
            if (query.AuthorID != null)
            {
                strFilter.Append(" AND b.AuthorID = ").Append(query.AuthorID.Value);
            }
            if (query.PayType != null)
            {
                strFilter.Append(" AND a.PayType = ").Append(query.PayType.Value);
            }
            if (query.Status != null)
            {
                if (query.Status == 10)
                {
                    strFilter.Append(" AND a.Status in (0,2)");
                }
                else
                {
                    strFilter.Append(" AND a.Status = ").Append(query.Status.Value);
                }
            }
            return strFilter.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string PayNoticeQueryToSQLOrder(PayNoticeQuery query)
        {
            return " NoticeID DESC";
        }

        #endregion 组装SQL条件

        #region 获取一个实体对象

        public PayNoticeEntity GetPayNotice(Int64 noticeID)
        {
            PayNoticeEntity payNoticeEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT top 1 a.*,b.Title as CTitle,b.CNumber FROM dbo.PayNotice a with(nolock) INNER JOIN dbo.ContributionInfo b with(nolock) ON a.JournalID=b.JournalID and a.CID=b.CID");
            sqlCommandText.Append(" WHERE  a.NoticeID=@NoticeID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@NoticeID", DbType.Int64, noticeID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                payNoticeEntity = MakePayNotice(dr);
            }
            return payNoticeEntity;
        }

        #endregion

        #region 根据条件获取所有实体对象

        public List<PayNoticeEntity> GetPayNoticeList()
        {
            List<PayNoticeEntity> payNoticeEntity = new List<PayNoticeEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  NoticeID,JournalID,PayType,CID,Amount,Title,Body,SendDate,Status FROM dbo.PayNotice WITH(NOLOCK)");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                payNoticeEntity = MakePayNoticeList(dr);
            }
            return payNoticeEntity;
        }

        public List<PayNoticeEntity> GetPayNoticeList(PayNoticeQuery query)
        {
            string strSql = @"SELECT a.*,b.Title as CTitle,b.CNumber FROM dbo.PayNotice a with(nolock) 
                              INNER JOIN dbo.ContributionInfo b with(nolock) ON a.JournalID=b.JournalID and a.CID=b.CID";
            string where = PayNoticeQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(where))
                strSql += " WHERE " + where;
            strSql += " ORDER BY " + PayNoticeQueryToSQLOrder(query);
            return db.GetList<PayNoticeEntity>(strSql, MakePayNoticeList);
        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<PayNoticeEntity></returns>
        public Pager<PayNoticeEntity> GetPayNoticePageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("PayNotice", "NoticeID,JournalID,PayType,CID,Amount,Title,Body,SendDate,Status", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<PayNoticeEntity> pager = new Pager<PayNoticeEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakePayNoticeList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<PayNoticeEntity> GetPayNoticePageList(QueryBase query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("PayNotice", "NoticeID,JournalID,PayType,CID,Amount,Title,Body,SendDate,Status", " NoticeID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<PayNoticeEntity> pager = new Pager<PayNoticeEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakePayNoticeList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<PayNoticeEntity> GetPayNoticePageList(PayNoticeQuery query)
        {
            string tableSql = @"SELECT {0} FROM dbo.PayNotice a with(nolock) 
                              INNER JOIN dbo.ContributionInfo b with(nolock) ON a.JournalID=b.JournalID and a.CID=b.CID";
            string where = PayNoticeQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(where))
                tableSql += " WHERE " + where;
            string strSql = string.Format(tableSql, "a.*,b.Title as CTitle,b.CNumber,ROW_NUMBER() OVER(ORDER BY " + PayNoticeQueryToSQLOrder(query) + ") AS ROW_ID")
                , sumStr = string.Format(tableSql, "RecordCount=COUNT(1)");
            return db.GetPageList<PayNoticeEntity>(strSql
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakePayNoticeList);
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        public bool AddPayNotice(PayNoticeEntity payNoticeEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @PayType");
            sqlCommandText.Append(", @CID");
            sqlCommandText.Append(", @Amount");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @Body");
            sqlCommandText.Append(", @Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.PayNotice ({0},SendDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, payNoticeEntity.JournalID);
            db.AddInParameter(cmd, "@PayType", DbType.Byte, payNoticeEntity.PayType);
            db.AddInParameter(cmd, "@CID", DbType.Int64, payNoticeEntity.CID);
            db.AddInParameter(cmd, "@Amount", DbType.Decimal, payNoticeEntity.Amount);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, payNoticeEntity.Title);
            db.AddInParameter(cmd, "@Body", DbType.AnsiString, payNoticeEntity.Body);
            db.AddInParameter(cmd, "@Status", DbType.Byte, payNoticeEntity.Status);
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

        public bool BatchAddPayNotice(IList<PayNoticeEntity> payNoticeList)
        {
            bool returnData = false;
            try
            {
                DataTable dt = ConvertListToTable(payNoticeList);
                returnData = db.BatchByTableValueParameter(null, "Usp_PayNoticeTVP", "table", dt);
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            return returnData;

        }

        #endregion

        #region 更新数据

        public bool UpdatePayNotice(PayNoticeEntity payNoticeEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  NoticeID=@NoticeID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" PayType=@PayType");
            sqlCommandText.Append(", Amount=@Amount");
            sqlCommandText.Append(", Title=@Title");
            sqlCommandText.Append(", Body=@Body");
            sqlCommandText.Append(", Status=@Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.PayNotice SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@NoticeID", DbType.Int64, payNoticeEntity.NoticeID);
            db.AddInParameter(cmd, "@PayType", DbType.Byte, payNoticeEntity.PayType);
            db.AddInParameter(cmd, "@Amount", DbType.Decimal, payNoticeEntity.Amount);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, payNoticeEntity.Title);
            db.AddInParameter(cmd, "@Body", DbType.AnsiString, payNoticeEntity.Body);
            db.AddInParameter(cmd, "@Status", DbType.Byte, payNoticeEntity.Status);
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

        public bool BatchUpdatePayNotice(List<PayNoticeEntity> payNoticeList)
        {
            bool returnData = false;
            try
            {
                DataTable dt = ConvertListToTable(payNoticeList);
                returnData = db.BatchByTableValueParameter(null, "Usp_Update_PayNoticeTVP", "table", dt);
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            return returnData;
        }

        #endregion

        #region 删除对象

        #region 删除一个对象

        public bool DeletePayNotice(PayNoticeEntity payNoticeEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.PayNotice");
            sqlCommandText.Append(" WHERE  NoticeID=@NoticeID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            db.AddInParameter(cmd, "@NoticeID", DbType.Int64, payNoticeEntity.NoticeID);

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

        public bool DeletePayNotice(Int64 noticeID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.PayNotice");
            sqlCommandText.Append(" WHERE  NoticeID=@NoticeID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@NoticeID", DbType.Int64, noticeID);
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
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeletePayNotice(Int64[] noticeID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from PayNotice where ");

            for (int i = 0; i < noticeID.Length; i++)
            {
                if (i > 0) sqlCommandText.Append(" or ");
                sqlCommandText.Append("( NoticeID=@NoticeID" + i + " )");
            }

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for (int i = 0; i < noticeID.Length; i++)
            {
                db.AddInParameter(cmd, "@NoticeID" + i, DbType.Int64, noticeID[i]);
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

        public PayNoticeEntity MakePayNotice(IDataReader dr)
        {
            PayNoticeEntity payNoticeEntity = null;
            if (dr.Read())
            {
                payNoticeEntity = new PayNoticeEntity();
                payNoticeEntity.NoticeID = (Int64)dr["NoticeID"];
                payNoticeEntity.JournalID = (Int64)dr["JournalID"];
                payNoticeEntity.PayType = (Byte)dr["PayType"];
                payNoticeEntity.CID = (Int64)dr["CID"];
                payNoticeEntity.Amount = (Decimal)dr["Amount"];
                payNoticeEntity.Title = (String)dr["Title"];
                payNoticeEntity.Body = (String)dr["Body"];
                payNoticeEntity.SendDate = (DateTime)dr["SendDate"];
                payNoticeEntity.Status = (Byte)dr["Status"];
                //if (dr.HasColumn("CNumber"))
                payNoticeEntity.CNumber = (String)dr["CNumber"];
                //if (dr.HasColumn("CTitle"))
                payNoticeEntity.CTitle = (String)dr["CTitle"];
            }
            dr.Close();
            return payNoticeEntity;
        }

        public PayNoticeEntity MakePayNotice(DataRow dr)
        {
            PayNoticeEntity payNoticeEntity = null;
            if (dr != null)
            {
                payNoticeEntity = new PayNoticeEntity();
                payNoticeEntity.NoticeID = (Int64)dr["NoticeID"];
                payNoticeEntity.JournalID = (Int64)dr["JournalID"];
                payNoticeEntity.PayType = (Byte)dr["PayType"];
                payNoticeEntity.CID = (Int64)dr["CID"];
                payNoticeEntity.Amount = (Decimal)dr["Amount"];
                payNoticeEntity.Title = (String)dr["Title"];
                payNoticeEntity.Body = (String)dr["Body"];
                payNoticeEntity.SendDate = (DateTime)dr["SendDate"];
                payNoticeEntity.Status = (Byte)dr["Status"];
            }
            return payNoticeEntity;
        }
        #endregion

        #region 根据数据组装一组对象数据

        public List<PayNoticeEntity> MakePayNoticeList(IDataReader dr)
        {
            List<PayNoticeEntity> list = new List<PayNoticeEntity>();
            while (dr.Read())
            {
                PayNoticeEntity payNoticeEntity = new PayNoticeEntity();
                payNoticeEntity.NoticeID = (Int64)dr["NoticeID"];
                payNoticeEntity.JournalID = (Int64)dr["JournalID"];
                payNoticeEntity.PayType = (Byte)dr["PayType"];
                payNoticeEntity.CID = (Int64)dr["CID"];
                payNoticeEntity.Amount = (Decimal)dr["Amount"];
                payNoticeEntity.Title = (String)dr["Title"];
                payNoticeEntity.Body = (String)dr["Body"];
                payNoticeEntity.SendDate = (DateTime)dr["SendDate"];
                payNoticeEntity.Status = (Byte)dr["Status"];
                //if (dr.HasColumn("CNumber"))
                payNoticeEntity.CNumber = (String)dr["CNumber"];
                //if (dr.HasColumn("CTitle"))
                payNoticeEntity.CTitle = (String)dr["CTitle"];
                list.Add(payNoticeEntity);
            }
            dr.Close();
            return list;
        }


        public List<PayNoticeEntity> MakePayNoticeList(DataTable dt)
        {
            List<PayNoticeEntity> list = new List<PayNoticeEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PayNoticeEntity payNoticeEntity = MakePayNotice(dt.Rows[i]);
                    list.Add(payNoticeEntity);
                }
            }
            return list;
        }

        #endregion

        /// <summary>
        /// 改变缴费通知状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool ChangeStatus(PayNoticeQuery query)
        {
            if (query.Status == null)
                return false;
            if (query.NoticeID == 0 && query.NoticeIDs.Length < 1)
                return false;
            string strSql = string.Format("UPDATE dbo.PayNotice set Status={0} WHERE NoticeID ", query.Status.Value);
            if (query.NoticeID > 0)
                strSql += " = " + query.NoticeID;
            else if (query.NoticeIDs.Length == 1)
                strSql += " = " + query.NoticeIDs[0];
            else
                strSql += string.Format(" in ({0})", string.Join(",", query.NoticeIDs));
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(cmd) > 0;
        }

        /// <summary>
        /// 缴费通知是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool PayNotinceIsExists(PayNoticeQuery query)
        {
            string strSql = "SELECT 1 FROM dbo.PayNotice WHERE JournalID=@JournalID and CID=@CID and PayType=@PayType";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@CID", DbType.Int64, query.CID);
            db.AddInParameter(cmd, "@PayType", DbType.Byte, query.PayType);
            return db.ExecuteScalar(cmd).TryParse<Int32>() == 1;
        }

        public DataTable ConvertListToTable(IList<PayNoticeEntity> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("NoticeID"));
            dt.Columns.Add(new DataColumn("JournalID"));
            dt.Columns.Add(new DataColumn("PayType"));
            dt.Columns.Add(new DataColumn("CID"));
            dt.Columns.Add(new DataColumn("Amount"));
            dt.Columns.Add(new DataColumn("Title"));
            dt.Columns.Add(new DataColumn("Body"));
            dt.Columns.Add(new DataColumn("SendDate"));
            dt.Columns.Add(new DataColumn("Status"));
            if (list != null && list.Count > 0)
            {
                int index = 0;
                foreach (var item in list)
                {

                    DataRow row =dt.NewRow();
                    row["NoticeID"] =item.NoticeID==0?index:item.NoticeID;
                    row["JournalID"] = item.JournalID;
                    row["PayType"] = item.PayType;
                    row["CID"] = item.CID;
                    row["Amount"] = item.Amount;
                    row["Title"] = item.Title;
                    row["Body"] = item.Body;
                    row["SendDate"] = item.SendDate;
                    row["Status"] = item.Status;
                    dt.Rows.Add(row);
                    index++;
                }
            }

            return dt;
        }

    }
}

