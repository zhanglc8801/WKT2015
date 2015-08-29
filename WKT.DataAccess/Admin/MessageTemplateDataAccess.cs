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
    public partial class MessageTemplateDataAccess : DataAccessBase
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public MessageTemplateDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static MessageTemplateDataAccess _instance = new MessageTemplateDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static MessageTemplateDataAccess Instance
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
        public string MessageTemplateQueryToSQLWhere(MessageTemplateQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID=" + query.JournalID);
            if (query.TType != null)
                strFilter.AppendFormat(" and TType={0}", query.TType.Value);
            if (query.TCategory != null)
                strFilter.AppendFormat(" and TCategory={0}", query.TCategory);
            query.Title = query.Title.TextFilter();
            if (string.IsNullOrWhiteSpace(query.Title))
            {                
                strFilter.AppendFormat(" and Title like '%{0}%'", query.Title);
            }
            return strFilter.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string MessageTemplateQueryToSQLOrder(MessageTemplateQuery query)
        {
            return " TemplateID DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public MessageTemplateEntity GetMessageTemplate(Int64 templateID)
        {
            MessageTemplateEntity messageTemplateEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  TemplateID,JournalID,TType,Title,TContent,TCategory,EditUser,EditDate,InUser,AddDate FROM dbo.MessageTemplate WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  TemplateID=@TemplateID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@TemplateID",DbType.Int64,templateID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                messageTemplateEntity = MakeMessageTemplate(dr);
            }
            return messageTemplateEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<MessageTemplateEntity> GetMessageTemplateList()
        {
            List<MessageTemplateEntity> messageTemplateEntity=new List<MessageTemplateEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  TemplateID,JournalID,TType,Title,TContent,TCategory,EditUser,EditDate,InUser,AddDate FROM dbo.MessageTemplate WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                messageTemplateEntity = MakeMessageTemplateList(dr);
            }
            return messageTemplateEntity;
        }
        
        public List<MessageTemplateEntity> GetMessageTemplateList(MessageTemplateQuery query)
        {
            string strSql = "SELECT * FROM dbo.MessageTemplate with(nolock)";
            string whereSQL = MessageTemplateQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " order by TemplateID";
            return db.GetList<MessageTemplateEntity>(strSql, MakeMessageTemplateList);
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<MessageTemplateEntity></returns>
        public Pager<MessageTemplateEntity> GetMessageTemplatePageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("MessageTemplate","TemplateID,JournalID,TType,Title,TContent,TCategory,EditUser,EditDate,InUser,AddDate",query.Order,query.Where,query.CurrentPage,query.PageSize,out recordCount);
            Pager<MessageTemplateEntity>  pager = new Pager<MessageTemplateEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeMessageTemplateList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<MessageTemplateEntity> GetMessageTemplatePageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("MessageTemplate","TemplateID,JournalID,TType,Title,TContent,TCategory,EditUser,EditDate,InUser,AddDate"," TemplateID DESC","",query.CurrentPage,query.PageSize,out recordCount);
            Pager<MessageTemplateEntity>  pager=new Pager<MessageTemplateEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeMessageTemplateList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<MessageTemplateEntity> GetMessageTemplatePageList(MessageTemplateQuery query)
        {
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY TemplateID DESC) AS ROW_ID FROM dbo.MessageTemplate with(nolock)",
                  sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.MessageTemplate with(nolock)";
            string whereSQL = MessageTemplateQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<MessageTemplateEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeMessageTemplateList);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddMessageTemplate(MessageTemplateEntity messageTemplateEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();          
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @TType");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @TContent");
            sqlCommandText.Append(", @TCategory");           
            sqlCommandText.Append(", @InUser");           
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.MessageTemplate ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
            
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,messageTemplateEntity.JournalID);
            db.AddInParameter(cmd,"@TType",DbType.Byte,messageTemplateEntity.TType);
            db.AddInParameter(cmd,"@Title",DbType.AnsiString,messageTemplateEntity.Title);
            db.AddInParameter(cmd,"@TContent",DbType.AnsiString,messageTemplateEntity.TContent);
            db.AddInParameter(cmd,"@TCategory",DbType.Int32,messageTemplateEntity.TCategory);          
            db.AddInParameter(cmd,"@InUser",DbType.Int64,messageTemplateEntity.InUser);           
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
     
        public bool UpdateMessageTemplate(MessageTemplateEntity messageTemplateEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  TemplateID=@TemplateID");
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" Title=@Title");
            sqlCommandText.Append(", TContent=@TContent");
            sqlCommandText.Append(", TCategory=@TCategory");
            sqlCommandText.Append(", EditUser=@EditUser");
            sqlCommandText.Append(", EditDate=getdate()");
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.MessageTemplate SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@TemplateID",DbType.Int64,messageTemplateEntity.TemplateID);           
            db.AddInParameter(cmd,"@Title",DbType.AnsiString,messageTemplateEntity.Title);
            db.AddInParameter(cmd,"@TContent",DbType.AnsiString,messageTemplateEntity.TContent);
            db.AddInParameter(cmd,"@TCategory",DbType.Int32,messageTemplateEntity.TCategory);
            db.AddInParameter(cmd,"@EditUser",DbType.Int64,messageTemplateEntity.EditUser);            

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
        
        public bool DeleteMessageTemplate(MessageTemplateEntity messageTemplateEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.MessageTemplate");
            sqlCommandText.Append(" WHERE  TemplateID=@TemplateID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@TemplateID",DbType.Int64,messageTemplateEntity.TemplateID);
            
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
        
        public bool DeleteMessageTemplate(Int64 templateID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.MessageTemplate");
            sqlCommandText.Append(" WHERE  TemplateID=@TemplateID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@TemplateID",DbType.Int64,templateID);
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
        /// <param name="templateID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteMessageTemplate(Int64[] templateID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from MessageTemplate where ");
           
            for(int i=0;i<templateID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( TemplateID=@TemplateID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<templateID.Length;i++)
            {
            db.AddInParameter(cmd,"@TemplateID"+i,DbType.Int64,templateID[i]);
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
        
        public MessageTemplateEntity MakeMessageTemplate(IDataReader dr)
        {
            MessageTemplateEntity messageTemplateEntity = null;
            if(dr.Read())
            {
             messageTemplateEntity=new MessageTemplateEntity();
             messageTemplateEntity.TemplateID = (Int64)dr["TemplateID"];
             messageTemplateEntity.JournalID = (Int64)dr["JournalID"];
             messageTemplateEntity.TType = Convert.IsDBNull(dr["TType"]) ? (Byte)1 : (Byte)dr["TType"];
             messageTemplateEntity.Title = Convert.IsDBNull(dr["Title"]) ? string.Empty : (String)dr["Title"];
             messageTemplateEntity.TContent = Convert.IsDBNull(dr["TContent"]) ? string.Empty : (String)dr["TContent"];
             messageTemplateEntity.TCategory = Convert.IsDBNull(dr["TCategory"]) ? -1 : (Int32)dr["TCategory"];
             messageTemplateEntity.EditUser = (Int64)dr["EditUser"];
             messageTemplateEntity.EditDate = (DateTime)dr["EditDate"];
             messageTemplateEntity.InUser = (Int64)dr["InUser"];
             messageTemplateEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return messageTemplateEntity;
        }
        
        public MessageTemplateEntity MakeMessageTemplate(DataRow dr)
        {
            MessageTemplateEntity messageTemplateEntity=null;
            if(dr!=null)
            {
                 messageTemplateEntity=new MessageTemplateEntity();
                 messageTemplateEntity.TemplateID = (Int64)dr["TemplateID"];
                 messageTemplateEntity.JournalID = (Int64)dr["JournalID"];
                 messageTemplateEntity.TType = Convert.IsDBNull(dr["TType"]) ? (Byte)1 : (Byte)dr["TType"];
                 messageTemplateEntity.Title = Convert.IsDBNull(dr["Title"]) ? string.Empty : (String)dr["Title"];
                 messageTemplateEntity.TContent = Convert.IsDBNull(dr["TContent"]) ? string.Empty : (String)dr["TContent"];
                 messageTemplateEntity.TCategory = Convert.IsDBNull(dr["TCategory"]) ? -1 : (Int32)dr["TCategory"];
                 messageTemplateEntity.EditUser = (Int64)dr["EditUser"];
                 messageTemplateEntity.EditDate = (DateTime)dr["EditDate"];
                 messageTemplateEntity.InUser = (Int64)dr["InUser"];
                 messageTemplateEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return messageTemplateEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<MessageTemplateEntity> MakeMessageTemplateList(IDataReader dr)
        {
            List<MessageTemplateEntity> list=new List<MessageTemplateEntity>();
            while(dr.Read())
            {
             MessageTemplateEntity messageTemplateEntity=new MessageTemplateEntity();
            messageTemplateEntity.TemplateID = (Int64)dr["TemplateID"];
            messageTemplateEntity.JournalID = (Int64)dr["JournalID"];
            messageTemplateEntity.TType = Convert.IsDBNull(dr["TType"]) ? (Byte)1 : (Byte)dr["TType"];
            messageTemplateEntity.Title = Convert.IsDBNull(dr["Title"]) ? string.Empty : (String)dr["Title"];
            messageTemplateEntity.TContent = Convert.IsDBNull(dr["TContent"]) ? string.Empty : (String)dr["TContent"];
            messageTemplateEntity.TCategory = Convert.IsDBNull(dr["TCategory"]) ? -1 : (Int32)dr["TCategory"];
            messageTemplateEntity.EditUser = (Int64)dr["EditUser"];
            messageTemplateEntity.EditDate = (DateTime)dr["EditDate"];
            messageTemplateEntity.InUser = (Int64)dr["InUser"];
            messageTemplateEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(messageTemplateEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<MessageTemplateEntity> MakeMessageTemplateList(DataTable dt)
        {
            List<MessageTemplateEntity> list=new List<MessageTemplateEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   MessageTemplateEntity messageTemplateEntity=MakeMessageTemplate(dt.Rows[i]);
                   list.Add(messageTemplateEntity);
                }
            }
            return list;
        }
        
        #endregion

        /// <summary>
        /// 获取已经使用模版类型
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="TType"></param>
        /// <returns></returns>
        public IList<Int32> GetUserdTCategory(Int64 JournalID, Byte TType)
        {
            string strSql = string.Format("SELECT distinct TCategory FROM dbo.MessageTemplate with(nolock) where JournalID={0} and TType={1}", JournalID, TType);
            return db.GetList<Int32>(strSql, (dr) =>
                {
                    List<Int32> list = new List<Int32>();
                    while (dr.Read())
                        list.Add((Int32)dr["TCategory"]);
                    return list;
                });
        }

        /// <summary>
        /// 获取短信邮件模版实体
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="TCategory"></param>
        /// <param name="TType"></param>
        /// <returns></returns>
        public MessageTemplateEntity GetMessageTemplate(Int64 JournalID, Int32 TCategory, Byte TType)
        {
            MessageTemplateEntity model = new MessageTemplateEntity();
            string strSql = string.Format("SELECT top 1 * FROM dbo.MessageTemplate with(nolock) where JournalID={0} and TCategory={1} and TType={2}", JournalID, TCategory, TType);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                model = MakeMessageTemplate(dr);
            }
            return model;
        }
    }
}

