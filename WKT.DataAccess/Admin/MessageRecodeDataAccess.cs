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
    public partial class MessageRecodeDataAccess : DataAccessBase
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db = null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public MessageRecodeDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if (db == null)
                throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static MessageRecodeDataAccess _instance = new MessageRecodeDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static MessageRecodeDataAccess Instance
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
        public string MessageRecodeQueryToSQLWhere(MessageRecodeQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID=" + query.JournalID);
            if (query.MsgType != null)
                strFilter.AppendFormat(" and MsgType={0}", query.MsgType.Value);
            if (query.SendType != null)
                strFilter.AppendFormat(" and SendType={0}", query.SendType.Value);
            if (query.CID != null)
                strFilter.Append(" and CID=").Append(query.CID.Value);
            if (query.IsUserRelevant)
                strFilter.AppendFormat(" and (SendUser={0} or ReciveUser={1})", query.SendUser.Value, query.ReciveUser.Value);
            else
            {
                if (query.SendUser != null)
                    strFilter.AppendFormat(" and SendUser={0}", query.SendUser.Value);
                if (query.ReciveUser != null)
                    strFilter.AppendFormat(" and ReciveUser={0}", query.ReciveUser.Value);
            }
            return strFilter.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string MessageRecodeQueryToSQLOrder(MessageRecodeQuery query)
        {
            return " RecodeID DESC";
        }

        #endregion 组装SQL条件

        #region 获取一个实体对象

        public MessageRecodeEntity GetMessageRecode(MessageRecodeQuery query)
        {
            MessageRecodeEntity messageRecodeEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  * FROM dbo.MessageRecode WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  RecodeID=@RecodeID AND JournalID=@JournalID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@RecodeID", DbType.Int64, query.RecodeID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                messageRecodeEntity = MakeMessageRecode(dr);
            }
            return messageRecodeEntity;
        }

        #endregion

        #region 根据条件获取所有实体对象

        public List<MessageRecodeEntity> GetMessageRecodeList()
        {
            List<MessageRecodeEntity> messageRecodeEntity = new List<MessageRecodeEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  * FROM dbo.MessageRecode WITH(NOLOCK)");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                messageRecodeEntity = MakeMessageRecodeList(dr);
            }
            return messageRecodeEntity;
        }

        public List<MessageRecodeEntity> GetMessageRecodeList(MessageRecodeQuery query)
        {
            List<MessageRecodeEntity> list = new List<MessageRecodeEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT * FROM dbo.MessageRecode WITH(NOLOCK)");
            string whereSQL = MessageRecodeQueryToSQLWhere(query);
            string orderBy = MessageRecodeQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeMessageRecodeList(dr);
            }
            return list;
        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<MessageRecodeEntity></returns>
        public Pager<MessageRecodeEntity> GetMessageRecodePageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("MessageRecode", "*", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<MessageRecodeEntity> pager = new Pager<MessageRecodeEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeMessageRecodeList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<MessageRecodeEntity> GetMessageRecodePageList(QueryBase query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("MessageRecode", "*", " RecodeID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<MessageRecodeEntity> pager = new Pager<MessageRecodeEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeMessageRecodeList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<MessageRecodeEntity> GetMessageRecodePageList(MessageRecodeQuery query)
        {
            int recordCount = 0;
            string whereSQL = MessageRecodeQueryToSQLWhere(query);
            string orderBy = MessageRecodeQueryToSQLOrder(query);
            DataSet ds = db.GetPagingData("MessageRecode", "*", orderBy, whereSQL, query.CurrentPage, query.PageSize, out recordCount);
            Pager<MessageRecodeEntity> pager = new Pager<MessageRecodeEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeMessageRecodeList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        public bool AddMessageRecode(MessageRecodeEntity messageRecodeEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @CID");
            sqlCommandText.Append(", @SendUser");
            sqlCommandText.Append(", @ReciveUser");
            sqlCommandText.Append(", @ReciveAddress");
            sqlCommandText.Append(", @SendDate");
            sqlCommandText.Append(", @MsgType");
            sqlCommandText.Append(", @SendType");
            sqlCommandText.Append(", @MsgTitle");
            sqlCommandText.Append(", @MsgContent");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.MessageRecode ({0},AddDate) VALUES ({1},getdate()) SELECT @@IDENTITY as RecodeID", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, messageRecodeEntity.JournalID);
            db.AddInParameter(cmd, "@CID", DbType.Int64, messageRecodeEntity.CID);
            db.AddInParameter(cmd, "@SendUser", DbType.Int64, messageRecodeEntity.SendUser);
            db.AddInParameter(cmd, "@ReciveUser", DbType.Int64, messageRecodeEntity.ReciveUser);
            db.AddInParameter(cmd, "@ReciveAddress", DbType.AnsiString, messageRecodeEntity.ReciveAddress);
            db.AddInParameter(cmd, "@SendDate", DbType.DateTime, messageRecodeEntity.SendDate);
            db.AddInParameter(cmd, "@MsgType", DbType.Byte, messageRecodeEntity.MsgType);
            db.AddInParameter(cmd, "@SendType", DbType.Int32, messageRecodeEntity.SendType);
            db.AddInParameter(cmd, "@MsgTitle", DbType.AnsiString, messageRecodeEntity.MsgTitle);
            db.AddInParameter(cmd, "@MsgContent", DbType.AnsiString, messageRecodeEntity.MsgContent);
            try
            {
                
                IDataReader dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    long recodeID = Convert.ToInt64(dr["RecodeID"]);
                    MessageRecodeEntity currentEntity = new MessageRecodeEntity() { RecodeID = recodeID, JournalID = messageRecodeEntity.JournalID, TemplateID = messageRecodeEntity.TemplateID };
                    AddMessageRecodeAndTemplate(currentEntity);
                    flag = true;
                }
               
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }

        public bool AddMessageRecodeAndTemplate(MessageRecodeEntity messageRecodeEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @TemplateID");
            sqlCommandText.Append(", @RecodeID");
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.MessageRecodeAndTemplate ({0},AddTime) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, messageRecodeEntity.JournalID);
            db.AddInParameter(cmd, "@TemplateID", DbType.Int64, messageRecodeEntity.TemplateID);
            db.AddInParameter(cmd, "@RecodeID", DbType.Int64, messageRecodeEntity.RecodeID);

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

        #region 更新数据

        public bool UpdateMessageRecode(MessageRecodeEntity messageRecodeEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  RecodeID=@RecodeID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(", SendUser=@SendUser");
            sqlCommandText.Append(", ReciveUser=@ReciveUser");
            sqlCommandText.Append(", ReciveAddress=@ReciveAddress");
            sqlCommandText.Append(", SendDate=@SendDate");
            sqlCommandText.Append(", MsgType=@MsgType");
            sqlCommandText.Append(", SendType=@SendType");
            sqlCommandText.Append(", MsgTitle=@MsgTitle");
            sqlCommandText.Append(", MsgContent=@MsgContent");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.MessageRecode SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@RecodeID", DbType.Int64, messageRecodeEntity.RecodeID);
            db.AddInParameter(cmd, "@SendUser", DbType.Int64, messageRecodeEntity.SendUser);
            db.AddInParameter(cmd, "@ReciveUser", DbType.Int64, messageRecodeEntity.ReciveUser);
            db.AddInParameter(cmd, "@ReciveAddress", DbType.AnsiString, messageRecodeEntity.ReciveAddress);
            db.AddInParameter(cmd, "@SendDate", DbType.DateTime, messageRecodeEntity.SendDate);
            db.AddInParameter(cmd, "@MsgType", DbType.Byte, messageRecodeEntity.MsgType);
            db.AddInParameter(cmd, "@SendType", DbType.Int32, messageRecodeEntity.SendType);
            db.AddInParameter(cmd, "@MsgTitle", DbType.AnsiString, messageRecodeEntity.MsgTitle);
            db.AddInParameter(cmd, "@MsgContent", DbType.AnsiString, messageRecodeEntity.MsgContent);

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

        #region 删除对象

        #region 删除一个对象

        public bool DeleteMessageRecode(MessageRecodeEntity messageRecodeEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.MessageRecode");
            sqlCommandText.Append(" WHERE  RecodeID=@RecodeID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            db.AddInParameter(cmd, "@RecodeID", DbType.Int64, messageRecodeEntity.RecodeID);

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

        public bool DeleteMessageRecode(Int64 recodeID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.MessageRecode");
            sqlCommandText.Append(" WHERE  RecodeID=@RecodeID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@RecodeID", DbType.Int64, recodeID);
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
        /// <param name="recodeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteMessageRecode(Int64[] recodeID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from MessageRecode where ");

            for (int i = 0; i < recodeID.Length; i++)
            {
                if (i > 0) sqlCommandText.Append(" or ");
                sqlCommandText.Append("( RecodeID=@RecodeID" + i + " )");
            }

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for (int i = 0; i < recodeID.Length; i++)
            {
                db.AddInParameter(cmd, "@RecodeID" + i, DbType.Int64, recodeID[i]);
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

        public MessageRecodeEntity MakeMessageRecode(IDataReader dr)
        {
            MessageRecodeEntity messageRecodeEntity = null;
            if (dr.Read())
            {
                messageRecodeEntity = new MessageRecodeEntity();
                messageRecodeEntity.RecodeID = (Int64)dr["RecodeID"];
                messageRecodeEntity.JournalID = (Int64)dr["JournalID"];
                messageRecodeEntity.CID = (Int64)dr["CID"];
                messageRecodeEntity.SendUser = (Int64)dr["SendUser"];
                messageRecodeEntity.ReciveUser = (Int64)dr["ReciveUser"];
                messageRecodeEntity.ReciveAddress = (String)dr["ReciveAddress"];
                messageRecodeEntity.SendDate = (DateTime)dr["SendDate"];
                messageRecodeEntity.MsgType = (Byte)dr["MsgType"];
                messageRecodeEntity.SendType = Convert.IsDBNull(dr["SendType"]) ? null : (Int32?)dr["SendType"];
                messageRecodeEntity.MsgTitle = (String)dr["MsgTitle"];
                messageRecodeEntity.MsgContent = (String)dr["MsgContent"];
                messageRecodeEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return messageRecodeEntity;
        }

        public MessageRecodeEntity MakeMessageRecode(DataRow dr)
        {
            MessageRecodeEntity messageRecodeEntity = null;
            if (dr != null)
            {
                messageRecodeEntity = new MessageRecodeEntity();
                messageRecodeEntity.RecodeID = (Int64)dr["RecodeID"];
                messageRecodeEntity.JournalID = (Int64)dr["JournalID"];
                messageRecodeEntity.CID = (Int64)dr["CID"];
                messageRecodeEntity.SendUser = (Int64)dr["SendUser"];
                messageRecodeEntity.ReciveUser = (Int64)dr["ReciveUser"];
                messageRecodeEntity.ReciveAddress = (String)dr["ReciveAddress"];
                messageRecodeEntity.SendDate = (DateTime)dr["SendDate"];
                messageRecodeEntity.MsgType = (Byte)dr["MsgType"];
                messageRecodeEntity.SendType = Convert.IsDBNull(dr["SendType"]) ? null : (Int32?)dr["SendType"];
                messageRecodeEntity.MsgTitle = (String)dr["MsgTitle"];
                messageRecodeEntity.MsgContent = (String)dr["MsgContent"];
                messageRecodeEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return messageRecodeEntity;
        }
        #endregion

        #region 根据数据组装一组对象数据

        public List<MessageRecodeEntity> MakeMessageRecodeList(IDataReader dr)
        {
            List<MessageRecodeEntity> list = new List<MessageRecodeEntity>();
            while (dr.Read())
            {
                MessageRecodeEntity messageRecodeEntity = new MessageRecodeEntity();

                messageRecodeEntity.RecodeID = (Int64)dr["RecodeID"];
                messageRecodeEntity.JournalID = (Int64)dr["JournalID"];
                messageRecodeEntity.CID = (Int64)dr["CID"];
                messageRecodeEntity.SendUser = (Int64)dr["SendUser"];
                messageRecodeEntity.ReciveUser = (Int64)dr["ReciveUser"];
                messageRecodeEntity.ReciveAddress = (String)dr["ReciveAddress"];
                messageRecodeEntity.SendDate = (DateTime)dr["SendDate"];
                messageRecodeEntity.MsgType = (Byte)dr["MsgType"];
                messageRecodeEntity.SendType = Convert.IsDBNull(dr["SendType"]) ? null : (Int32?)dr["SendType"];
                messageRecodeEntity.MsgTitle = (String)dr["MsgTitle"];
                messageRecodeEntity.MsgContent = (String)dr["MsgContent"];
                messageRecodeEntity.AddDate = (DateTime)dr["AddDate"];
                list.Add(messageRecodeEntity);
            }
            dr.Close();
            return list;
        }


        public List<MessageRecodeEntity> MakeMessageRecodeList(DataTable dt)
        {
            List<MessageRecodeEntity> list = new List<MessageRecodeEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MessageRecodeEntity messageRecodeEntity = MakeMessageRecode(dt.Rows[i]);
                    list.Add(messageRecodeEntity);
                }
            }
            return list;
        }

        #endregion

        /// <summary>
        /// 保存发送记录
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool SaveSendRecode(IList<MessageRecodeEntity> list)
        {
            if (list == null || list.Count == 0)
                return false;
            if (list.Count == 1)
                return AddMessageRecode(list[0]);
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    DataTable dtInsert = null;
                    DbCommand cmd = null;
                    Action SetDataTable = () =>
                    {
                        cmd = db.GetSqlStringCommand("select top 1 * from dbo.MessageRecode with(nolock) where 1=2");
                        DataSet ds = db.ExecuteDataSet(cmd);
                        if (ds == null || ds.Tables.Count == 0)
                            return;
                        DataTable dt = ds.Tables[0];
                        dtInsert = dt.Clone();
                        DataRow dr = null;
                        foreach (var model in list)
                        {
                            dr = dt.NewRow();
                            dr["JournalID"] = model.JournalID;
                            dr["CID"] = model.CID;
                            dr["SendUser"] = model.SendUser;
                            dr["ReciveUser"] = model.ReciveUser;
                            dr["ReciveAddress"] = model.ReciveAddress;
                            dr["SendDate"] = model.SendDate;
                            dr["MsgType"] = model.MsgType;
                            dr["SendType"] = model.SendType;
                            dr["MsgTitle"] = model.MsgType;
                            dr["MsgContent"] = model.MsgContent;
                            dr["AddDate"] = model.AddDate;
                            dtInsert.Rows.Add(dr.ItemArray);
                        }
                    };
                    SetDataTable();
                    if (dtInsert != null && dtInsert.Rows.Count > 0)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>() { 
                            {"JournalID","JournalID"}
                           ,{"CID","CID"}
                           ,{"SendUser","SendUser"}
                           ,{"ReciveUser","ReciveUser"}
                           ,{"ReciveAddress","ReciveAddress"}
                           ,{"SendDate","SendDate"}
                           ,{"MsgType","MsgType"}
                           ,{"SendType","SendType"}
                           ,{"MsgTitle","MsgTitle"}
                           ,{"MsgContent","MsgContent"}
                           ,{"AddDate","AddDate"}  
                        };
                        db.SaveLargeData(conn, trans, dtInsert, "MessageRecode", dic);
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

    }
}

