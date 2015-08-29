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
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class ContactWayDataAccess : DataAccessBase
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public ContactWayDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static ContactWayDataAccess _instance = new ContactWayDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static ContactWayDataAccess Instance
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
        public string ContactWayQueryToSQLWhere(ContactWayQuery query)
        {
            StringBuilder strFilter = new StringBuilder();
            strFilter.Append(" 1=1");
            if (query.JournalID > 0)
                strFilter.AppendFormat(" and JournalID={0}", query.JournalID);
            if(query.ChannelID>0)
                strFilter.AppendFormat(" and ChannelID={0}", query.ChannelID);
            return strFilter.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string ContactWayQueryToSQLOrder(ContactWayQuery query)
        {
            return " ContactID DESC";
        }
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public ContactWayEntity GetContactWay(Int64 contactID)
        {
            ContactWayEntity contactWayEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  ContactID,JournalID,ChannelID,Company,LinkMan,Address,ZipCode,Tel,Fax,AddDate FROM dbo.ContactWay WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  ContactID=@ContactID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@ContactID",DbType.Int64,contactID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                contactWayEntity = MakeContactWay(dr);
            }
            return contactWayEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<ContactWayEntity> GetContactWayList()
        {
            List<ContactWayEntity> contactWayEntity = new List<ContactWayEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  ContactID,JournalID,ChannelID,Company,LinkMan,Address,ZipCode,Tel,Fax,AddDate FROM dbo.ContactWay WITH(NOLOCK)");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                contactWayEntity = MakeContactWayList(dr);
            }
            return contactWayEntity;
        }
        
        public List<ContactWayEntity> GetContactWayList(ContactWayQuery query)
        {
            string strSql = "SELECT ContactID,JournalID,ChannelID,Company,LinkMan,Address,ZipCode,Tel,Fax,AddDate FROM dbo.ContactWay with(nolock) ORDER BY ContactID desc";
            string whereSQL = ContactWayQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;               
            }
            return db.GetList<ContactWayEntity>(strSql, MakeContactWayList);
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<ContactWayEntity></returns>
        public Pager<ContactWayEntity> GetContactWayPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("ContactWay", "ContactID,JournalID,ChannelID,Company,LinkMan,Address,ZipCode,Tel,Fax,AddDate", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<ContactWayEntity> pager = new Pager<ContactWayEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeContactWayList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }
        
        public Pager<ContactWayEntity> GetContactWayPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("ContactWay", "ContactID,JournalID,ChannelID,Company,LinkMan,Address,ZipCode,Tel,Fax,AddDate", " ContactID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<ContactWayEntity>  pager=new Pager<ContactWayEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeContactWayList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<ContactWayEntity> GetContactWayPageList(ContactWayQuery query)
        {
            string strSql = "SELECT ContactID,JournalID,ChannelID,Company,LinkMan,Address,ZipCode,Tel,Fax,AddDate,ROW_NUMBER() OVER(ORDER BY ContactID DESC) AS ROW_ID FROM dbo.ContactWay with(nolock)",
                   sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.ContactWay with(nolock)";
            string whereSQL = ContactWayQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<ContactWayEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeContactWayList);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddContactWay(ContactWayEntity contactWayEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append("@JournalID");
            sqlCommandText.Append(", @ChannelID");
            sqlCommandText.Append(", @Company");
            sqlCommandText.Append(", @LinkMan");
            sqlCommandText.Append(", @Address");
            sqlCommandText.Append(", @ZipCode");
            sqlCommandText.Append(", @Tel");
            sqlCommandText.Append(", @Fax");        
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.ContactWay ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
          
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,contactWayEntity.JournalID);
            db.AddInParameter(cmd,"@ChannelID", DbType.Int64, contactWayEntity.ChannelID);
            db.AddInParameter(cmd,"@Company", DbType.AnsiString, contactWayEntity.Company);
            db.AddInParameter(cmd,"@LinkMan",DbType.AnsiString,contactWayEntity.LinkMan);
            db.AddInParameter(cmd,"@Address",DbType.AnsiString,contactWayEntity.Address);
            db.AddInParameter(cmd,"@ZipCode",DbType.AnsiString,contactWayEntity.ZipCode);
            db.AddInParameter(cmd,"@Tel",DbType.AnsiString,contactWayEntity.Tel);
            db.AddInParameter(cmd,"@Fax",DbType.AnsiString,contactWayEntity.Fax);          
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
     
        public bool UpdateContactWay(ContactWayEntity contactWayEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  ContactID=@ContactID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("Company=@Company");
            sqlCommandText.Append(", LinkMan=@LinkMan");
            sqlCommandText.Append(", Address=@Address");
            sqlCommandText.Append(", ZipCode=@ZipCode");
            sqlCommandText.Append(", Tel=@Tel");
            sqlCommandText.Append(", Fax=@Fax");            
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.ContactWay SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@ContactID",DbType.Int64,contactWayEntity.ContactID);
            db.AddInParameter(cmd,"@Company", DbType.AnsiString, contactWayEntity.Company);
            db.AddInParameter(cmd,"@LinkMan",DbType.AnsiString,contactWayEntity.LinkMan);
            db.AddInParameter(cmd,"@Address",DbType.AnsiString,contactWayEntity.Address);
            db.AddInParameter(cmd,"@ZipCode",DbType.AnsiString,contactWayEntity.ZipCode);
            db.AddInParameter(cmd,"@Tel",DbType.AnsiString,contactWayEntity.Tel);
            db.AddInParameter(cmd,"@Fax",DbType.AnsiString,contactWayEntity.Fax);
           
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
        
        public bool DeleteContactWay(ContactWayEntity contactWayEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.ContactWay");
            sqlCommandText.Append(" WHERE  ContactID=@ContactID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@ContactID",DbType.Int64,contactWayEntity.ContactID);
            
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
        
        public bool DeleteContactWay(Int64 contactID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.ContactWay");
            sqlCommandText.Append(" WHERE  ContactID=@ContactID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@ContactID",DbType.Int64,contactID);
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
        /// <param name="contactID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteContactWay(Int64[] contactID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from ContactWay where ");
           
            for(int i=0;i<contactID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( ContactID=@ContactID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<contactID.Length;i++)
            {
            db.AddInParameter(cmd,"@ContactID"+i,DbType.Int64,contactID[i]);
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
        
        public ContactWayEntity MakeContactWay(IDataReader dr)
        {
            ContactWayEntity contactWayEntity = null;
            if(dr.Read())
            {
             contactWayEntity=new ContactWayEntity();
             contactWayEntity.ContactID = (Int64)dr["ContactID"];
             contactWayEntity.JournalID = (Int64)dr["JournalID"];
             contactWayEntity.ChannelID = (Int64)dr["ChannelID"];
             contactWayEntity.Company = (String)dr["Company"];
             contactWayEntity.LinkMan = (String)dr["LinkMan"];
             contactWayEntity.Address = (String)dr["Address"];
             contactWayEntity.ZipCode = (String)dr["ZipCode"];
             contactWayEntity.Tel = (String)dr["Tel"];
             contactWayEntity.Fax = (String)dr["Fax"];
             contactWayEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return contactWayEntity;
        }
        
        public ContactWayEntity MakeContactWay(DataRow dr)
        {
            ContactWayEntity contactWayEntity=null;
            if(dr!=null)
            {
                 contactWayEntity=new ContactWayEntity();
                 contactWayEntity.ContactID = (Int64)dr["ContactID"];
                 contactWayEntity.JournalID = (Int64)dr["JournalID"];
                 contactWayEntity.ChannelID = (Int64)dr["ChannelID"];
                 contactWayEntity.Company = (String)dr["Company"];
                 contactWayEntity.LinkMan = (String)dr["LinkMan"];
                 contactWayEntity.Address = (String)dr["Address"];
                 contactWayEntity.ZipCode = (String)dr["ZipCode"];
                 contactWayEntity.Tel = (String)dr["Tel"];
                 contactWayEntity.Fax = (String)dr["Fax"];
                 contactWayEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return contactWayEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<ContactWayEntity> MakeContactWayList(IDataReader dr)
        {
            List<ContactWayEntity> list=new List<ContactWayEntity>();
            while(dr.Read())
            {
             ContactWayEntity contactWayEntity=new ContactWayEntity();
            contactWayEntity.ContactID = (Int64)dr["ContactID"];
            contactWayEntity.JournalID = (Int64)dr["JournalID"];
            contactWayEntity.ChannelID = (Int64)dr["ChannelID"];
            contactWayEntity.Company = (String)dr["Company"];
            contactWayEntity.LinkMan = (String)dr["LinkMan"];
            contactWayEntity.Address = (String)dr["Address"];
            contactWayEntity.ZipCode = (String)dr["ZipCode"];
            contactWayEntity.Tel = (String)dr["Tel"];
            contactWayEntity.Fax = (String)dr["Fax"];
            contactWayEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(contactWayEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<ContactWayEntity> MakeContactWayList(DataTable dt)
        {
            List<ContactWayEntity> list=new List<ContactWayEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   ContactWayEntity contactWayEntity=MakeContactWay(dt.Rows[i]);
                   list.Add(contactWayEntity);
                }
            }
            return list;
        }
        
        #endregion

    }
}

