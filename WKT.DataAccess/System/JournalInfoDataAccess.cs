using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Data.SQL;
using WKT.Model;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class JournalInfoDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public JournalInfoDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTSysDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static JournalInfoDataAccess _instance = new JournalInfoDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static JournalInfoDataAccess Instance
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
        public string JournalInfoQueryToSQLWhere(JournalInfoQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            if (query.Status != null)
            {
                sbWhere.Append(" AND Status= ").Append(query.Status.Value);
            }
            if (!string.IsNullOrEmpty(query.JournalName))
            {
                sbWhere.AppendFormat(" AND JournalName='{0}'", WKT.Common.Security.SecurityUtils.SafeSqlString(query.JournalName));
            }
            if (!string.IsNullOrEmpty(query.ServiceStartDate))
            {
                sbWhere.AppendFormat(" AND ServiceStartDate>='{0}'", WKT.Common.Security.SecurityUtils.SafeSqlString(query.ServiceStartDate));
            }
            if (!string.IsNullOrEmpty(query.ServiceEndDate))
            {
                sbWhere.AppendFormat(" AND ServiceEndDate<='{0}'", WKT.Common.Security.SecurityUtils.SafeSqlString(query.ServiceEndDate));
            }
            if (sbWhere.ToString() == " 1=1 ")
            {
                return string.Empty;
            }
            else
            {
                return sbWhere.ToString();
            }
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string JournalInfoQueryToSQLOrder(JournalInfoQuery query)
        {
            return " JournalID DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public JournalInfoEntity GetJournalInfo(Int64 journalID)
        {
            JournalInfoEntity journalInfoEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  JournalID,JournalName,SiteUrl,ServiceStartDate,ServiceEndDate,Linkman,LinkTel,Fax,Email,Mobile,Address,ZipCode,AuthorizationCode,Status,Note,AddDate FROM dbo.JournalInfo WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  JournalID=@JournalID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,journalID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                journalInfoEntity = MakeJournalInfo(dr);
            }
            return journalInfoEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<JournalInfoEntity> GetJournalInfoList()
        {
            List<JournalInfoEntity> journalInfoEntity=new List<JournalInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  JournalID,JournalName,SiteUrl,ServiceStartDate,ServiceEndDate,Linkman,LinkTel,Fax,Email,Mobile,Address,ZipCode,AuthorizationCode,Status,Note,AddDate FROM dbo.JournalInfo WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                journalInfoEntity = MakeJournalInfoList(dr);
            }
            return journalInfoEntity;
        }
        
        public List<JournalInfoEntity> GetJournalInfoList(JournalInfoQuery query)
        {
            List<JournalInfoEntity> list = new List<JournalInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT JournalID,JournalName,SiteUrl,ServiceStartDate,ServiceEndDate,Linkman,LinkTel,Fax,Email,Mobile,Address,ZipCode,AuthorizationCode,Status,Note,AddDate FROM dbo.JournalInfo WITH(NOLOCK)");
            string whereSQL = JournalInfoQueryToSQLWhere(query);
            string orderBy=JournalInfoQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeJournalInfoList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<JournalInfoEntity></returns>
        public Pager<JournalInfoEntity> GetJournalInfoPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("JournalInfo","JournalID,JournalName,SiteUrl,ServiceStartDate,ServiceEndDate,Linkman,LinkTel,Fax,Email,Mobile,Address,ZipCode,AuthorizationCode,Status,Note,AddDate",query.Order,query.Where,query.CurrentPage,query.PageSize,out recordCount);
            Pager<JournalInfoEntity>  pager = new Pager<JournalInfoEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeJournalInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<JournalInfoEntity> GetJournalInfoPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("JournalInfo","JournalID,JournalName,SiteUrl,ServiceStartDate,ServiceEndDate,Linkman,LinkTel,Fax,Email,Mobile,Address,ZipCode,AuthorizationCode,Status,Note,AddDate"," JournalID DESC","",query.CurrentPage,query.PageSize,out recordCount);
            Pager<JournalInfoEntity>  pager=new Pager<JournalInfoEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeJournalInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<JournalInfoEntity> GetJournalInfoPageList(JournalInfoQuery query)
        {
            int recordCount=0;
            string whereSQL=JournalInfoQueryToSQLWhere(query);
            string orderBy=JournalInfoQueryToSQLOrder(query);
            DataSet ds=db.GetPagingData("JournalInfo","JournalID,JournalName,SiteUrl,ServiceStartDate,ServiceEndDate,Linkman,LinkTel,Fax,Email,Mobile,Address,ZipCode,AuthorizationCode,Status,Note,AddDate",orderBy,whereSQL,query.CurrentPage,query.PageSize,out recordCount);
            Pager<JournalInfoEntity>  pager=new Pager<JournalInfoEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeJournalInfoList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddJournalInfo(JournalInfoEntity journalInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();

            sqlCommandText.Append("@JournalName");
            sqlCommandText.Append(", @SiteUrl");
            sqlCommandText.Append(", @ServiceStartDate");
            sqlCommandText.Append(", @ServiceEndDate");
            sqlCommandText.Append(", @Linkman");
            sqlCommandText.Append(", @LinkTel");
            sqlCommandText.Append(", @Fax");
            sqlCommandText.Append(", @Email");
            sqlCommandText.Append(", @Mobile");
            sqlCommandText.Append(", @Address");
            sqlCommandText.Append(", @ZipCode");
            sqlCommandText.Append(", @AuthorizationCode");
            sqlCommandText.Append(", @Status");
            sqlCommandText.Append(", @Note");
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.JournalInfo ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
            
            db.AddInParameter(cmd,"@JournalName",DbType.AnsiString,journalInfoEntity.JournalName);
            db.AddInParameter(cmd,"@SiteUrl",DbType.AnsiString,journalInfoEntity.SiteUrl);
            db.AddInParameter(cmd,"@ServiceStartDate",DbType.DateTime,journalInfoEntity.ServiceStartDate);
            db.AddInParameter(cmd,"@ServiceEndDate",DbType.DateTime,journalInfoEntity.ServiceEndDate);
            db.AddInParameter(cmd,"@Linkman",DbType.AnsiString,journalInfoEntity.Linkman);
            db.AddInParameter(cmd,"@LinkTel",DbType.AnsiString,journalInfoEntity.LinkTel);
            db.AddInParameter(cmd,"@Fax",DbType.AnsiString,journalInfoEntity.Fax);
            db.AddInParameter(cmd,"@Email",DbType.AnsiString,journalInfoEntity.Email);
            db.AddInParameter(cmd,"@Mobile",DbType.AnsiString,journalInfoEntity.Mobile);
            db.AddInParameter(cmd,"@Address",DbType.AnsiString,journalInfoEntity.Address);
            db.AddInParameter(cmd,"@ZipCode",DbType.AnsiString,journalInfoEntity.ZipCode);
            db.AddInParameter(cmd,"@AuthorizationCode",DbType.AnsiString,journalInfoEntity.AuthorizationCode);
            db.AddInParameter(cmd,"@Status",DbType.Byte,journalInfoEntity.Status);
            db.AddInParameter(cmd,"@Note",DbType.AnsiString,journalInfoEntity.Note);

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
     
        public bool UpdateJournalInfo(JournalInfoEntity journalInfoEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  JournalID=@JournalID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" JournalName=@JournalName");
            sqlCommandText.Append(", SiteUrl=@SiteUrl");
            sqlCommandText.Append(", ServiceStartDate=@ServiceStartDate");
            sqlCommandText.Append(", ServiceEndDate=@ServiceEndDate");
            sqlCommandText.Append(", Linkman=@Linkman");
            sqlCommandText.Append(", LinkTel=@LinkTel");
            sqlCommandText.Append(", Fax=@Fax");
            sqlCommandText.Append(", Email=@Email");
            sqlCommandText.Append(", Mobile=@Mobile");
            sqlCommandText.Append(", Address=@Address");
            sqlCommandText.Append(", ZipCode=@ZipCode");
            sqlCommandText.Append(", AuthorizationCode=@AuthorizationCode");
            sqlCommandText.Append(", Status=@Status");
            sqlCommandText.Append(", Note=@Note");
            sqlCommandText.Append(", AddDate=@AddDate");
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.JournalInfo SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,journalInfoEntity.JournalID);
            db.AddInParameter(cmd,"@JournalName",DbType.AnsiString,journalInfoEntity.JournalName);
            db.AddInParameter(cmd,"@SiteUrl",DbType.AnsiString,journalInfoEntity.SiteUrl);
            db.AddInParameter(cmd,"@ServiceStartDate",DbType.DateTime,journalInfoEntity.ServiceStartDate);
            db.AddInParameter(cmd,"@ServiceEndDate",DbType.DateTime,journalInfoEntity.ServiceEndDate);
            db.AddInParameter(cmd,"@Linkman",DbType.AnsiString,journalInfoEntity.Linkman);
            db.AddInParameter(cmd,"@LinkTel",DbType.AnsiString,journalInfoEntity.LinkTel);
            db.AddInParameter(cmd,"@Fax",DbType.AnsiString,journalInfoEntity.Fax);
            db.AddInParameter(cmd,"@Email",DbType.AnsiString,journalInfoEntity.Email);
            db.AddInParameter(cmd,"@Mobile",DbType.AnsiString,journalInfoEntity.Mobile);
            db.AddInParameter(cmd,"@Address",DbType.AnsiString,journalInfoEntity.Address);
            db.AddInParameter(cmd,"@ZipCode",DbType.AnsiString,journalInfoEntity.ZipCode);
            db.AddInParameter(cmd,"@AuthorizationCode",DbType.AnsiString,journalInfoEntity.AuthorizationCode);
            db.AddInParameter(cmd,"@Status",DbType.Byte,journalInfoEntity.Status);
            db.AddInParameter(cmd,"@Note",DbType.AnsiString,journalInfoEntity.Note);
            db.AddInParameter(cmd,"@AddDate",DbType.DateTime,journalInfoEntity.AddDate);

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
        
        public bool DeleteJournalInfo(JournalInfoEntity journalInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.JournalInfo");
            sqlCommandText.Append(" WHERE  JournalID=@JournalID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,journalInfoEntity.JournalID);
            
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
        
        public bool DeleteJournalInfo(Int64 journalID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.JournalInfo");
            sqlCommandText.Append(" WHERE  JournalID=@JournalID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,journalID);
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
        /// <param name="journalID">站点ID</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteJournalInfo(Int64[] journalID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from JournalInfo where ");
           
            for(int i=0;i<journalID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( JournalID=@JournalID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<journalID.Length;i++)
            {
            db.AddInParameter(cmd,"@JournalID"+i,DbType.Int64,journalID[i]);
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
        
        public JournalInfoEntity MakeJournalInfo(IDataReader dr)
        {
            JournalInfoEntity journalInfoEntity = null;
            if(dr.Read())
            {
             journalInfoEntity=new JournalInfoEntity();
             journalInfoEntity.JournalID = (Int64)dr["JournalID"];
             journalInfoEntity.JournalName = (String)dr["JournalName"];
             journalInfoEntity.SiteUrl = (String)dr["SiteUrl"];
             journalInfoEntity.ServiceStartDate = (DateTime)dr["ServiceStartDate"];
             journalInfoEntity.ServiceEndDate = (DateTime)dr["ServiceEndDate"];
             journalInfoEntity.Linkman = (String)dr["Linkman"];
             journalInfoEntity.LinkTel = (String)dr["LinkTel"];
             journalInfoEntity.Fax = (String)dr["Fax"];
             journalInfoEntity.Email = (String)dr["Email"];
             journalInfoEntity.Mobile = (String)dr["Mobile"];
             journalInfoEntity.Address = (String)dr["Address"];
             journalInfoEntity.ZipCode = (String)dr["ZipCode"];
             journalInfoEntity.AuthorizationCode = (String)dr["AuthorizationCode"];
             journalInfoEntity.Status = (Byte)dr["Status"];
             journalInfoEntity.Note = (String)dr["Note"];
             journalInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return journalInfoEntity;
        }
        
        public JournalInfoEntity MakeJournalInfo(DataRow dr)
        {
            JournalInfoEntity journalInfoEntity=null;
            if(dr!=null)
            {
                 journalInfoEntity=new JournalInfoEntity();
                 journalInfoEntity.JournalID = (Int64)dr["JournalID"];
                 journalInfoEntity.JournalName = (String)dr["JournalName"];
                 journalInfoEntity.SiteUrl = (String)dr["SiteUrl"];
                 journalInfoEntity.ServiceStartDate = (DateTime)dr["ServiceStartDate"];
                 journalInfoEntity.ServiceEndDate = (DateTime)dr["ServiceEndDate"];
                 journalInfoEntity.Linkman = (String)dr["Linkman"];
                 journalInfoEntity.LinkTel = (String)dr["LinkTel"];
                 journalInfoEntity.Fax = (String)dr["Fax"];
                 journalInfoEntity.Email = (String)dr["Email"];
                 journalInfoEntity.Mobile = (String)dr["Mobile"];
                 journalInfoEntity.Address = (String)dr["Address"];
                 journalInfoEntity.ZipCode = (String)dr["ZipCode"];
                 journalInfoEntity.AuthorizationCode = (String)dr["AuthorizationCode"];
                 journalInfoEntity.Status = (Byte)dr["Status"];
                 journalInfoEntity.Note = (String)dr["Note"];
                 journalInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return journalInfoEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<JournalInfoEntity> MakeJournalInfoList(IDataReader dr)
        {
            List<JournalInfoEntity> list=new List<JournalInfoEntity>();
            while(dr.Read())
            {
             JournalInfoEntity journalInfoEntity=new JournalInfoEntity();
            journalInfoEntity.JournalID = (Int64)dr["JournalID"];
            journalInfoEntity.JournalName = (String)dr["JournalName"];
            journalInfoEntity.SiteUrl = (String)dr["SiteUrl"];
            journalInfoEntity.ServiceStartDate = (DateTime)dr["ServiceStartDate"];
            journalInfoEntity.ServiceEndDate = (DateTime)dr["ServiceEndDate"];
            journalInfoEntity.Linkman = (String)dr["Linkman"];
            journalInfoEntity.LinkTel = (String)dr["LinkTel"];
            journalInfoEntity.Fax = (String)dr["Fax"];
            journalInfoEntity.Email = (String)dr["Email"];
            journalInfoEntity.Mobile = (String)dr["Mobile"];
            journalInfoEntity.Address = (String)dr["Address"];
            journalInfoEntity.ZipCode = (String)dr["ZipCode"];
            journalInfoEntity.AuthorizationCode = (String)dr["AuthorizationCode"];
            journalInfoEntity.Status = (Byte)dr["Status"];
            journalInfoEntity.Note = (String)dr["Note"];
            journalInfoEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(journalInfoEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<JournalInfoEntity> MakeJournalInfoList(DataTable dt)
        {
            List<JournalInfoEntity> list=new List<JournalInfoEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   JournalInfoEntity journalInfoEntity=MakeJournalInfo(dt.Rows[i]);
                   list.Add(journalInfoEntity);
                }
            }
            return list;
        }
        
        #endregion

    }
}

