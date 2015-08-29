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
    public partial class SiteConfigDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public SiteConfigDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static SiteConfigDataAccess _instance = new SiteConfigDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static SiteConfigDataAccess Instance
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
        public string SiteConfigQueryToSQLWhere(SiteConfigQuery query)
        {
            return string.Empty;
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string SiteConfigQueryToSQLOrder(SiteConfigQuery query)
        {
            return " SiteConfigID DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public SiteConfigEntity GetSiteConfig(Int64 siteConfigID)
        {
            SiteConfigEntity siteConfigEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 * FROM dbo.SiteConfig WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  SiteConfigID=@SiteConfigID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@SiteConfigID",DbType.Int64,siteConfigID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                siteConfigEntity = MakeSiteConfig(dr);
            }
            return siteConfigEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<SiteConfigEntity> GetSiteConfigList()
        {
            List<SiteConfigEntity> siteConfigEntity=new List<SiteConfigEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT * FROM dbo.SiteConfig WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                siteConfigEntity = MakeSiteConfigList(dr);
            }
            return siteConfigEntity;
        }
        
        public List<SiteConfigEntity> GetSiteConfigList(SiteConfigQuery query)
        {
            List<SiteConfigEntity> list = new List<SiteConfigEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT * FROM dbo.SiteConfig WITH(NOLOCK)");
            string whereSQL = SiteConfigQueryToSQLWhere(query);
            string orderBy=SiteConfigQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeSiteConfigList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<SiteConfigEntity></returns>
        public Pager<SiteConfigEntity> GetSiteConfigPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("SiteConfig", "SiteConfigID,JournalID,Title,EnTitle,CN,ISSN,Home,Keywords,Description,ICPCode,ZipCode,Address,SendMail,MailServer,MailPort,MailAccount,MailPwd,MailIsSSL,SMSUserName,SMSPwd,DoiUserName,DoiUserPwd,DoiJournalID,DoiPrefix,InUserID,EditUserID,EditDate,AddDate,EBankType,EBankAccount,EBankCode,EBankEncryKey", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<SiteConfigEntity>  pager = new Pager<SiteConfigEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeSiteConfigList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<SiteConfigEntity> GetSiteConfigPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("SiteConfig", "SiteConfigID,JournalID,Title,EnTitle,CN,ISSN,Home,Keywords,Description,ICPCode,ZipCode,Address,SendMail,MailServer,MailPort,MailAccount,MailPwd,MailIsSSL,SMSUserName,SMSPwd,DoiUserName,DoiUserPwd,DoiJournalID,DoiPrefix,InUserID,EditUserID,EditDate,AddDate,EBankType,EBankAccount,EBankCode,EBankEncryKey", " SiteConfigID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<SiteConfigEntity>  pager=new Pager<SiteConfigEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeSiteConfigList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<SiteConfigEntity> GetSiteConfigPageList(SiteConfigQuery query)
        {
            int recordCount=0;
            string whereSQL=SiteConfigQueryToSQLWhere(query);
            string orderBy=SiteConfigQueryToSQLOrder(query);
            DataSet ds = db.GetPagingData("SiteConfig", "SiteConfigID,JournalID,Title,EnTitle,CN,ISSN,Home,Keywords,Description,ICPCode,ZipCode,Address,SendMail,MailServer,MailPort,MailAccount,MailPwd,MailIsSSL,SMSUserName,SMSPwd,DoiUserName,DoiUserPwd,DoiJournalID,DoiPrefix,InUserID,EditUserID,EditDate,AddDate,EBankType,EBankAccount,EBankCode,EBankEncryKey", orderBy, whereSQL, query.CurrentPage, query.PageSize, out recordCount);
            Pager<SiteConfigEntity>  pager=new Pager<SiteConfigEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeSiteConfigList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）

        public bool AddSiteConfig(SiteConfigEntity siteConfigEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("@JournalID");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @EnTitle");
            sqlCommandText.Append(", @CN");
            sqlCommandText.Append(", @ISSN");
            sqlCommandText.Append(", @Home");
            sqlCommandText.Append(", @Keywords");
            sqlCommandText.Append(", @Description");
            sqlCommandText.Append(", @ICPCode");
            sqlCommandText.Append(", @ZipCode");
            sqlCommandText.Append(", @Address");
            sqlCommandText.Append(", @SendMail");
            sqlCommandText.Append(", @MailServer");
            sqlCommandText.Append(", @MailPort");
            sqlCommandText.Append(", @MailAccount");
            sqlCommandText.Append(", @MailPwd");
            sqlCommandText.Append(", @MailIsSSL");
            //商讯·中国
            sqlCommandText.Append(", @SMSUserName");
            sqlCommandText.Append(", @SMSPwd");
            //在线支付
            sqlCommandText.Append(", @EBankType");
            sqlCommandText.Append(", @EBankAccount");
            sqlCommandText.Append(", @EBankCode");
            sqlCommandText.Append(", @EBankEncryKey");
            //DOI
            sqlCommandText.Append(", @DoiUserName");
            sqlCommandText.Append(", @DoiUserPwd");
            sqlCommandText.Append(", @DoiJournalID");
            sqlCommandText.Append(", @DoiPrefix");
            sqlCommandText.Append(", @InUserID");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.SiteConfig ({0}) VALUES ({1})", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, siteConfigEntity.JournalID);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, siteConfigEntity.Title);
            db.AddInParameter(cmd, "@EnTitle", DbType.AnsiString, siteConfigEntity.EnTitle);
            db.AddInParameter(cmd, "@CN", DbType.AnsiString, siteConfigEntity.CN);
            db.AddInParameter(cmd, "@ISSN", DbType.AnsiString, siteConfigEntity.ISSN);
            db.AddInParameter(cmd, "@Home", DbType.AnsiString, siteConfigEntity.Home);
            db.AddInParameter(cmd, "@Keywords", DbType.AnsiString, siteConfigEntity.Keywords);
            db.AddInParameter(cmd, "@Description", DbType.AnsiString, siteConfigEntity.Description);
            db.AddInParameter(cmd, "@ICPCode", DbType.AnsiString, siteConfigEntity.ICPCode);
            db.AddInParameter(cmd, "@ZipCode", DbType.AnsiString, siteConfigEntity.ZipCode);
            db.AddInParameter(cmd, "@Address", DbType.AnsiString, siteConfigEntity.Address);
            db.AddInParameter(cmd, "@SendMail", DbType.AnsiString, siteConfigEntity.SendMail);
            db.AddInParameter(cmd, "@MailServer", DbType.AnsiString, siteConfigEntity.MailServer);
            db.AddInParameter(cmd, "@MailPort", DbType.Int32, siteConfigEntity.MailPort);
            db.AddInParameter(cmd, "@MailAccount", DbType.AnsiString, siteConfigEntity.MailAccount);
            db.AddInParameter(cmd, "@MailPwd", DbType.AnsiString, siteConfigEntity.MailPwd);
            db.AddInParameter(cmd, "@MailIsSSL", DbType.Boolean, siteConfigEntity.MailIsSSL);
            //商讯·中国
            db.AddInParameter(cmd, "@SMSUserName", DbType.AnsiString, siteConfigEntity.SMSUserName);
            db.AddInParameter(cmd, "@SMSPwd", DbType.AnsiString, siteConfigEntity.SMSPwd);
            //在线支付
            db.AddInParameter(cmd, "@EBankType", DbType.Byte, siteConfigEntity.EBankType);
            db.AddInParameter(cmd, "@EBankAccount", DbType.AnsiString, siteConfigEntity.EBankAccount);
            db.AddInParameter(cmd, "@EBankCode", DbType.AnsiString, siteConfigEntity.EBankCode);
            db.AddInParameter(cmd, "@EBankEncryKey", DbType.AnsiString, siteConfigEntity.EBankEncryKey);
            //DOI
            db.AddInParameter(cmd, "@DoiUserName", DbType.AnsiString, siteConfigEntity.DoiUserName);
            db.AddInParameter(cmd, "@DoiUserPwd", DbType.AnsiString, siteConfigEntity.DoiUserPwd);
            db.AddInParameter(cmd, "@DoiJournalID", DbType.AnsiString, siteConfigEntity.DoiJournalID);
            db.AddInParameter(cmd, "@DoiPrefix", DbType.AnsiString, siteConfigEntity.DoiPrefix);

            db.AddInParameter(cmd, "@InUserID", DbType.Int64, siteConfigEntity.InUserID);

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
     
        public bool UpdateSiteConfig(SiteConfigEntity siteConfigEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  SiteConfigID=@SiteConfigID");
            StringBuilder sqlCommandText = new StringBuilder();            
            sqlCommandText.Append(" Title=@Title");
            sqlCommandText.Append(", EnTitle=@EnTitle");
            sqlCommandText.Append(", CN=@CN");
            sqlCommandText.Append(", ISSN=@ISSN");
            sqlCommandText.Append(", Home=@Home");
            sqlCommandText.Append(", Keywords=@Keywords");
            sqlCommandText.Append(", Description=@Description");
            sqlCommandText.Append(", ICPCode=@ICPCode");
            sqlCommandText.Append(", ZipCode=@ZipCode");
            sqlCommandText.Append(", Address=@Address");
            sqlCommandText.Append(", SendMail=@SendMail");
            sqlCommandText.Append(", MailServer=@MailServer");
            sqlCommandText.Append(", MailPort=@MailPort");
            sqlCommandText.Append(", MailAccount=@MailAccount");
            sqlCommandText.Append(", MailPwd=@MailPwd");
            sqlCommandText.Append(", MailIsSSL=@MailIsSSL");
            //商讯·中国
            sqlCommandText.Append(", SMSUserName=@SMSUserName");
            sqlCommandText.Append(", SMSPwd=@SMSPwd");
            //在线支付
            sqlCommandText.Append(", EBankType=@EBankType");
            sqlCommandText.Append(", EBankAccount=@EBankAccount");
            sqlCommandText.Append(", EBankCode=@EBankCode");
            sqlCommandText.Append(", EBankEncryKey=@EBankEncryKey");
            //DOI
            sqlCommandText.Append(", DoiUserName=@DoiUserName");
            sqlCommandText.Append(", DoiUserPwd=@DoiUserPwd");
            sqlCommandText.Append(", DoiJournalID=@DoiJournalID");
            sqlCommandText.Append(", DoiPrefix=@DoiPrefix");

            sqlCommandText.Append(", EditUserID=@EditUserID");
            sqlCommandText.Append(", EditDate=getdate()");           
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.SiteConfig SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@SiteConfigID",DbType.Int64,siteConfigEntity.SiteConfigID);            
            db.AddInParameter(cmd,"@Title",DbType.AnsiString,siteConfigEntity.Title);
            db.AddInParameter(cmd, "@EnTitle", DbType.AnsiString, siteConfigEntity.EnTitle);
            db.AddInParameter(cmd, "@CN", DbType.AnsiString, siteConfigEntity.CN);
            db.AddInParameter(cmd, "@ISSN", DbType.AnsiString, siteConfigEntity.ISSN);
            db.AddInParameter(cmd, "@Home", DbType.AnsiString, siteConfigEntity.Home);
            db.AddInParameter(cmd,"@Keywords",DbType.AnsiString,siteConfigEntity.Keywords);
            db.AddInParameter(cmd,"@Description",DbType.AnsiString,siteConfigEntity.Description);
            db.AddInParameter(cmd,"@ICPCode",DbType.AnsiString,siteConfigEntity.ICPCode);
            db.AddInParameter(cmd, "@ZipCode", DbType.AnsiString, siteConfigEntity.ZipCode);
            db.AddInParameter(cmd, "@Address", DbType.AnsiString, siteConfigEntity.Address);
            db.AddInParameter(cmd,"@SendMail",DbType.AnsiString,siteConfigEntity.SendMail);
            db.AddInParameter(cmd,"@MailServer",DbType.AnsiString,siteConfigEntity.MailServer);
            db.AddInParameter(cmd,"@MailPort",DbType.Int32,siteConfigEntity.MailPort);
            db.AddInParameter(cmd,"@MailAccount",DbType.AnsiString,siteConfigEntity.MailAccount);
            db.AddInParameter(cmd,"@MailPwd",DbType.AnsiString,siteConfigEntity.MailPwd);
            db.AddInParameter(cmd,"@MailIsSSL",DbType.Boolean,siteConfigEntity.MailIsSSL);
            //商讯·中国
            db.AddInParameter(cmd,"@SMSUserName",DbType.AnsiString,siteConfigEntity.SMSUserName);
            db.AddInParameter(cmd,"@SMSPwd",DbType.AnsiString,siteConfigEntity.SMSPwd);
            //在线支付
            db.AddInParameter(cmd, "@EBankType", DbType.Byte, siteConfigEntity.EBankType);
            db.AddInParameter(cmd, "@EBankAccount", DbType.AnsiString, siteConfigEntity.EBankAccount);
            db.AddInParameter(cmd, "@EBankCode", DbType.AnsiString, siteConfigEntity.EBankCode);
            db.AddInParameter(cmd, "@EBankEncryKey", DbType.AnsiString, siteConfigEntity.EBankEncryKey);
            //DOI
            db.AddInParameter(cmd, "@DoiUserName", DbType.AnsiString, siteConfigEntity.DoiUserName);
            db.AddInParameter(cmd, "@DoiUserPwd", DbType.AnsiString, siteConfigEntity.DoiUserPwd);
            db.AddInParameter(cmd, "@DoiJournalID", DbType.AnsiString, siteConfigEntity.DoiJournalID);
            db.AddInParameter(cmd, "@DoiPrefix", DbType.AnsiString, siteConfigEntity.DoiPrefix);

            db.AddInParameter(cmd,"@EditUserID",DbType.Int64,siteConfigEntity.EditUserID);

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
        
        public bool DeleteSiteConfig(SiteConfigEntity siteConfigEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.SiteConfig");
            sqlCommandText.Append(" WHERE  SiteConfigID=@SiteConfigID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@SiteConfigID",DbType.Int64,siteConfigEntity.SiteConfigID);
            
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
        
        public bool DeleteSiteConfig(Int64 siteConfigID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.SiteConfig");
            sqlCommandText.Append(" WHERE  SiteConfigID=@SiteConfigID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@SiteConfigID",DbType.Int64,siteConfigID);
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
        /// <param name="siteConfigID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteConfig(Int64[] siteConfigID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from SiteConfig where ");
           
            for(int i=0;i<siteConfigID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( SiteConfigID=@SiteConfigID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<siteConfigID.Length;i++)
            {
            db.AddInParameter(cmd,"@SiteConfigID"+i,DbType.Int64,siteConfigID[i]);
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
        
        public SiteConfigEntity MakeSiteConfig(IDataReader dr)
        {
            SiteConfigEntity siteConfigEntity = null;
            if(dr.Read())
            {
             siteConfigEntity=new SiteConfigEntity();
             siteConfigEntity.SiteConfigID = (Int64)dr["SiteConfigID"];
             siteConfigEntity.JournalID = (Int64)dr["JournalID"];
             siteConfigEntity.Title = (String)dr["Title"];
             siteConfigEntity.EnTitle = (String)dr["EnTitle"];
             siteConfigEntity.CN = (String)dr["CN"];
             siteConfigEntity.ISSN = (String)dr["ISSN"];
             siteConfigEntity.Home = (String)dr["Home"];
             siteConfigEntity.Keywords = (String)dr["Keywords"];
             siteConfigEntity.Description = (String)dr["Description"];
             siteConfigEntity.ICPCode = (String)dr["ICPCode"];
             siteConfigEntity.ZipCode = (String)dr["ZipCode"];
             siteConfigEntity.Address = (String)dr["Address"];
             siteConfigEntity.SendMail = (String)dr["SendMail"];
             siteConfigEntity.MailServer = (String)dr["MailServer"];
             siteConfigEntity.MailPort = (Int32)dr["MailPort"];
             siteConfigEntity.MailAccount = (String)dr["MailAccount"];
             siteConfigEntity.MailPwd = (String)dr["MailPwd"];
             siteConfigEntity.MailIsSSL = (Boolean)dr["MailIsSSL"];
             //商讯·中国
             siteConfigEntity.SMSUserName = (String)dr["SMSUserName"];
             siteConfigEntity.SMSPwd = (String)dr["SMSPwd"];
             //在线支付
             siteConfigEntity.EBankType = (Byte)dr["EBankType"];
             siteConfigEntity.EBankAccount = (String)dr["EBankAccount"];
             siteConfigEntity.EBankCode = (String)dr["EBankCode"];
             siteConfigEntity.EBankEncryKey = (String)dr["EBankEncryKey"];
             //DOI
             siteConfigEntity.DoiUserName = dr["DoiUserName"] == System.DBNull.Value ? "" : (String)dr["DoiUserName"];
             siteConfigEntity.DoiUserPwd = dr["DoiUserPwd"] == System.DBNull.Value ? "" : (String)dr["DoiUserPwd"];
             siteConfigEntity.DoiJournalID = dr["DoiJournalID"] == System.DBNull.Value ? "" : (String)dr["DoiJournalID"];
             siteConfigEntity.DoiPrefix = dr["DoiPrefix"] == System.DBNull.Value ? "" : (String)dr["DoiPrefix"];

             siteConfigEntity.InUserID = (Int64)dr["InUserID"];
             siteConfigEntity.EditUserID = (Int64)dr["EditUserID"];
             siteConfigEntity.EditDate = (DateTime)dr["EditDate"];
             siteConfigEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return siteConfigEntity;
        }
        
        public SiteConfigEntity MakeSiteConfig(DataRow dr)
        {
            SiteConfigEntity siteConfigEntity=null;
            if(dr!=null)
            {
                 siteConfigEntity=new SiteConfigEntity();
                 siteConfigEntity.SiteConfigID = (Int64)dr["SiteConfigID"];
                 siteConfigEntity.JournalID = (Int64)dr["JournalID"];
                 siteConfigEntity.Title = (String)dr["Title"];
                 siteConfigEntity.EnTitle = (String)dr["EnTitle"];
                 siteConfigEntity.CN = (String)dr["CN"];
                 siteConfigEntity.ISSN = (String)dr["ISSN"];
                 siteConfigEntity.Home = (String)dr["Home"];
                 siteConfigEntity.Keywords = (String)dr["Keywords"];
                 siteConfigEntity.Description = (String)dr["Description"];
                 siteConfigEntity.ICPCode = (String)dr["ICPCode"];
                 siteConfigEntity.ZipCode = (String)dr["ZipCode"];
                 siteConfigEntity.Address = (String)dr["Address"];
                 siteConfigEntity.SendMail = (String)dr["SendMail"];
                 siteConfigEntity.MailServer = (String)dr["MailServer"];
                 siteConfigEntity.MailPort = (Int32)dr["MailPort"];
                 siteConfigEntity.MailAccount = (String)dr["MailAccount"];
                 siteConfigEntity.MailPwd = (String)dr["MailPwd"];
                 siteConfigEntity.MailIsSSL = (Boolean)dr["MailIsSSL"];
                 //商讯·中国
                 siteConfigEntity.SMSUserName = (String)dr["SMSUserName"];
                 siteConfigEntity.SMSPwd = (String)dr["SMSPwd"];
                 //在线支付
                 siteConfigEntity.EBankType = (Byte)dr["EBankType"];
                 siteConfigEntity.EBankAccount = (String)dr["EBankAccount"];
                 siteConfigEntity.EBankCode = (String)dr["EBankCode"];
                 siteConfigEntity.EBankEncryKey = (String)dr["EBankEncryKey"];
                 //DOI
                 siteConfigEntity.DoiUserName = (String)dr["DoiUserName"];
                 siteConfigEntity.DoiUserPwd = (String)dr["DoiUserPwd"];
                 siteConfigEntity.DoiJournalID = (String)dr["DoiJournalID"];
                 siteConfigEntity.DoiPrefix = (String)dr["DoiPrefix"];

                 siteConfigEntity.InUserID = (Int64)dr["InUserID"];
                 siteConfigEntity.EditUserID = (Int64)dr["EditUserID"];
                 siteConfigEntity.EditDate = (DateTime)dr["EditDate"];
                 siteConfigEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return siteConfigEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<SiteConfigEntity> MakeSiteConfigList(IDataReader dr)
        {
            List<SiteConfigEntity> list=new List<SiteConfigEntity>();
            while(dr.Read())
            {
             SiteConfigEntity siteConfigEntity=new SiteConfigEntity();
            siteConfigEntity.SiteConfigID = (Int64)dr["SiteConfigID"];
            siteConfigEntity.JournalID = (Int64)dr["JournalID"];
            siteConfigEntity.Title = (String)dr["Title"];
            siteConfigEntity.EnTitle = (String)dr["EnTitle"];
            siteConfigEntity.CN = (String)dr["CN"];
            siteConfigEntity.ISSN = (String)dr["ISSN"];
            siteConfigEntity.Home = (String)dr["Home"];
            siteConfigEntity.Keywords = (String)dr["Keywords"];
            siteConfigEntity.Description = (String)dr["Description"];
            siteConfigEntity.ICPCode = (String)dr["ICPCode"];
            siteConfigEntity.ZipCode = (String)dr["ZipCode"];
            siteConfigEntity.Address = (String)dr["Address"];
            siteConfigEntity.SendMail = (String)dr["SendMail"];
            siteConfigEntity.MailServer = (String)dr["MailServer"];
            siteConfigEntity.MailPort = (Int32)dr["MailPort"];
            siteConfigEntity.MailAccount = (String)dr["MailAccount"];
            siteConfigEntity.MailPwd = (String)dr["MailPwd"];
            siteConfigEntity.MailIsSSL = (Boolean)dr["MailIsSSL"];
            //商讯·中国
            siteConfigEntity.SMSUserName = (String)dr["SMSUserName"];
            siteConfigEntity.SMSPwd = (String)dr["SMSPwd"];
            //在线支付
            siteConfigEntity.EBankType = (Byte)dr["EBankType"];
            siteConfigEntity.EBankAccount = (String)dr["EBankAccount"];
            siteConfigEntity.EBankCode = (String)dr["EBankCode"];
            siteConfigEntity.EBankEncryKey = (String)dr["EBankEncryKey"];
            //DOI
            siteConfigEntity.DoiUserName = (String)dr["DoiUserName"];
            siteConfigEntity.DoiUserPwd = (String)dr["DoiUserPwd"];
            siteConfigEntity.DoiJournalID = (String)dr["DoiJournalID"];
            siteConfigEntity.DoiPrefix = (String)dr["DoiPrefix"];

            siteConfigEntity.InUserID = (Int64)dr["InUserID"];
            siteConfigEntity.EditUserID = (Int64)dr["EditUserID"];
            siteConfigEntity.EditDate = (DateTime)dr["EditDate"];
            siteConfigEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(siteConfigEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<SiteConfigEntity> MakeSiteConfigList(DataTable dt)
        {
            List<SiteConfigEntity> list=new List<SiteConfigEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   SiteConfigEntity siteConfigEntity=MakeSiteConfig(dt.Rows[i]);
                   list.Add(siteConfigEntity);
                }
            }
            return list;
        }
        
        #endregion

        /// <summary>
        /// 获取站点配置实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SiteConfigEntity GetSiteConfig(SiteConfigQuery query)
        {
            string strSql = "SELECT TOP 1 * FROM dbo.SiteConfig WITH(NOLOCK)";
            strSql += string.Format(" WHERE JournalID={0}", query.JournalID);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                return MakeSiteConfig(dr);
            }
        }

        /// <summary>
        /// 更新总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool UpdateSiteAccessCount(SiteConfigQuery query)
        {
            bool flag = false;
            try
            {
                string strSql = "UPDATE dbo.SiteConfig SET Hits=Hits+1";
                strSql += string.Format(" WHERE JournalID={0}", query.JournalID);
                DbCommand cmd = db.GetSqlStringCommand(strSql);
                db.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 获取总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetSiteAccessCount(SiteConfigQuery query)
        {
            int totalAccessCount = 0;
            string strSql = "SELECT TOP 1 Hits FROM dbo.SiteConfig WITH(NOLOCK)";
            strSql += string.Format(" WHERE JournalID={0}", query.JournalID);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    totalAccessCount = WKT.Common.Utils.TypeParse.ToInt(dr["Hits"]);
                }
                dr.Close();
            }
            return totalAccessCount;
        }
    }
}

