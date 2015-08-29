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
    public partial class AuthorDetailDataAccess : DataAccessBase
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db = null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public AuthorDetailDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if (db == null)
                throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static AuthorDetailDataAccess _instance = new AuthorDetailDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static AuthorDetailDataAccess Instance
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
        public string AuthorDetailQueryToSQLWhere(AuthorDetailQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" b.JournalID=" + query.JournalID);
            if (query.GroupID != null)
            {
                if (query.GroupID.Value == 3)
                {
                    strFilter.Append(@" and (b.GroupID=3 or exists(select 1 from dbo.RoleAuthor m with(nolock) 
                                        WHERE b.JournalID=m.JournalID and b.AuthorID=m.AuthorID and m.RoleID=3))");
                }
                else
                {
                    strFilter.Append(" and b.GroupID=").Append(query.GroupID.Value);
                }
            }
            if (query.Status != null)
                strFilter.Append(" and b.Status=").Append(query.Status.Value);
            query.LoginName = query.LoginName.TextFilter();
            if (!string.IsNullOrWhiteSpace(query.LoginName))
                strFilter.AppendFormat(" and b.LoginName like '%{0}%'", query.LoginName);

            query.RealName = query.RealName.TextFilter();
            if (!string.IsNullOrWhiteSpace(query.RealName))
                strFilter.AppendFormat(" and b.RealName like '%{0}%'", query.RealName);

            query.WorkUnit = query.WorkUnit.TextFilter();
            if (!string.IsNullOrWhiteSpace(query.WorkUnit))
                strFilter.AppendFormat(" and a.WorkUnit like '%{0}%'", query.WorkUnit);

            query.Mobile = query.Mobile.TextFilter();
            if (!string.IsNullOrWhiteSpace(query.Mobile))
                strFilter.AppendFormat(" and b.Mobile like '%{0}%'", query.Mobile);

            query.Address = query.Address.TextFilter();
            if (!string.IsNullOrWhiteSpace(query.Address))
                strFilter.AppendFormat(" and a.Address like '%{0}%'", query.Address);

            query.ZipCode = query.ZipCode.TextFilter();
            if (!string.IsNullOrWhiteSpace(query.ZipCode))
                strFilter.AppendFormat(" and a.ZipCode like '%{0}%'", query.ZipCode);
            
            if (query.AuthorIDs != null && query.AuthorIDs.Length > 0)
            {
                if (query.AuthorIDs.Length == 1)
                {
                    strFilter.Append(" and b.AuthorID=").Append(query.AuthorIDs[0]);
                }
                else
                {
                    strFilter.Append(" and b.AuthorID in (").Append(string.Join(",", query.AuthorIDs)).Append(")");
                }
            }
            return strFilter.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string AuthorDetailQueryToSQLOrder(AuthorDetailQuery query)
        {
            if (query.OrderStr != null && query.OrderStr.Trim() == "AddDate DESC")
                return "bAddDate DESC";
            else
                return " a.PKID DESC";
        }

        #endregion 组装SQL条件

        #region 获取一个实体对象

        public AuthorDetailEntity GetAuthorDetail(Int64 pKID)
        {
            AuthorDetailEntity authorDetailEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 a.*,b.Title as CTitle,b.CNumber FROM dbo.PayNotice a with(nolock)");
            sqlCommandText.Append(" INNER JOIN dbo.ContributionInfo b with(nolock) ON a.JournalID=b.JournalID and a.CID=b.CID");
            sqlCommandText.Append(" WHERE  a.PKID=@PKID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@PKID", DbType.Int64, pKID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                authorDetailEntity = MakeAuthorDetail(dr);
            }
            return authorDetailEntity;
        }

        /// <summary>
        /// 根据作者编号获取实体
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        public AuthorDetailEntity GetAuthorDetailModel(Int64 AuthorID)
        {
            AuthorDetailEntity authorDetailEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 b.AuthorID as bAuthorID,b.JournalID as bJournalID,b.LoginName,b.Pwd,b.RealName,b.LoginIP,b.LoginCount,b.LoginDate,b.Status,b.GroupID,b.AddDate as bAddDate,a.PKID,a.AuthorID,a.AuthorName,a.EnglishName,(case when a.Gender=1 then '男' when a.Gender=0 then '女' end) as Gender,a.Nation,a.Birthday,a.NativePlace,a.Province,a.City,a.Area,a.IDCard,a.Address,a.ZipCode,a.Mobile,a.Tel,a.Fax,a.Education,a.Professional,a.JobTitle,a.Job,a.ResearchTopics,a.WorkUnit,a.WorkUnitLevel,a.SectionOffice,a.InvoiceUnit,a.Mentor,a.Remark,a.AuthorOpus,a.QQ,a.MSN,a.ReserveField,a.ReserveField1,a.ReserveField2,a.ReserveField3,a.ReserveField4,a.ReserveField5 FROM dbo.AuthorDetail a with(nolock)");
            sqlCommandText.Append(" RIGHT JOIN dbo.AuthorInfo b with(nolock) ON a.JournalID=b.JournalID and a.AuthorID=b.AuthorID");
            sqlCommandText.Append(" WHERE b.AuthorID=@AuthorID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, AuthorID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                authorDetailEntity = MakeAuthorDetail(dr);
            }
            return authorDetailEntity;
        }

        #endregion

        #region 根据条件获取所有实体对象

        public List<AuthorDetailEntity> GetAuthorDetailList()
        {
            List<AuthorDetailEntity> authorDetailEntity = new List<AuthorDetailEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  * FROM dbo.AuthorDetail WITH(NOLOCK)");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                authorDetailEntity = MakeAuthorDetailList(dr);
            }
            return authorDetailEntity;
        }

        public List<AuthorDetailEntity> GetAuthorDetailList(AuthorDetailQuery query)
        {
            string strSql = @"SELECT b.AuthorID as bAuthorID,b.JournalID as bJournalID,b.LoginName,b.Pwd,b.RealName,b.LoginIP,b.LoginCount,b.LoginDate,b.Status,b.GroupID,b.AddDate as bAddDate,a.PKID,a.AuthorID,a.AuthorName,a.EnglishName,(case when a.Gender=1 then '男' when a.Gender=0 then '女' end) as Gender,a.Nation,a.Birthday,a.NativePlace,a.Province,a.City,a.Area,a.IDCard,a.Address,a.ZipCode,a.Mobile,a.Tel,a.Fax,a.Education,a.Professional,a.JobTitle,a.Job,a.ResearchTopics,a.WorkUnit,a.WorkUnitLevel,a.SectionOffice,a.InvoiceUnit,a.Mentor,a.Remark,a.AuthorOpus,a.QQ,a.MSN,a.ReserveField,a.ReserveField1,a.ReserveField2,a.ReserveField3,a.ReserveField4,a.ReserveField5 FROM dbo.AuthorDetail a with(nolock) 
                              RIGHT JOIN dbo.AuthorInfo b with(nolock) ON a.JournalID=b.JournalID and a.AuthorID=b.AuthorID";
            string where = AuthorDetailQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(where))
                strSql += " WHERE " + where;
            strSql += " ORDER BY " + AuthorDetailQueryToSQLOrder(query);
            return db.GetList<AuthorDetailEntity>(strSql, MakeAuthorDetailList);
        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<AuthorDetailEntity></returns>
        public Pager<AuthorDetailEntity> GetAuthorDetailPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("AuthorDetail", "*", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<AuthorDetailEntity> pager = new Pager<AuthorDetailEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeAuthorDetailList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<AuthorDetailEntity> GetAuthorDetailPageList(QueryBase query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("AuthorDetail", "*", " PKID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<AuthorDetailEntity> pager = new Pager<AuthorDetailEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeAuthorDetailList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<AuthorDetailEntity> GetAuthorDetailPageList(AuthorDetailQuery query)
        {
            string tableSql = @"SELECT {0} FROM dbo.AuthorDetail a with(nolock) 
                              RIGHT JOIN dbo.AuthorInfo b with(nolock) ON a.JournalID=b.JournalID and a.AuthorID=b.AuthorID";
            string where = AuthorDetailQueryToSQLWhere(query);
            if (!string.IsNullOrWhiteSpace(where))
                tableSql += " WHERE " + where;
            string strSql = string.Format(tableSql, "b.AuthorID as bAuthorID,b.JournalID as bJournalID,b.LoginName,b.Pwd,b.RealName,b.LoginIP,b.LoginCount,b.LoginDate,b.Status,b.GroupID,b.AddDate as bAddDate,a.PKID,a.AuthorID,a.AuthorName,a.EnglishName,(case when a.Gender=1 then '男' when a.Gender=0 then '女' end) as Gender,a.Nation,a.Birthday,a.NativePlace,a.Province,a.City,a.Area,a.IDCard,a.Address,a.ZipCode,a.Mobile,a.Tel,a.Fax,a.Education,a.Professional,a.JobTitle,a.Job,a.ResearchTopics,a.WorkUnit,a.WorkUnitLevel,a.SectionOffice,a.InvoiceUnit,a.Mentor,a.Remark,a.AuthorOpus,a.QQ,a.MSN,a.ReserveField,a.ReserveField1,a.ReserveField2,a.ReserveField3,a.ReserveField4,a.ReserveField5,ROW_NUMBER() OVER(ORDER BY " + AuthorDetailQueryToSQLOrder(query) + ") AS ROW_ID")
                , sumStr = string.Format(tableSql, "RecordCount=COUNT(1)");
            return db.GetPageList<AuthorDetailEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeAuthorDetailList);
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        public bool AddAuthorDetail(AuthorDetailEntity authorDetailEntity, DbTransaction trans = null)
        {
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @AuthorID");
            sqlCommandText.Append(", @JournalID");
            sqlCommandText.Append(", @AuthorName");
            sqlCommandText.Append(", @EnglishName");
            sqlCommandText.Append(", @Gender");
            sqlCommandText.Append(", @Nation");
            sqlCommandText.Append(", @Birthday");
            sqlCommandText.Append(", @NativePlace");
            sqlCommandText.Append(", @Province");
            sqlCommandText.Append(", @City");
            sqlCommandText.Append(", @Area");
            sqlCommandText.Append(", @IDCard");
            sqlCommandText.Append(", @Address");
            sqlCommandText.Append(", @ZipCode");
            sqlCommandText.Append(", @Mobile");
            sqlCommandText.Append(", @Tel");
            sqlCommandText.Append(", @Fax");
            sqlCommandText.Append(", @Education");
            sqlCommandText.Append(", @Professional");
            sqlCommandText.Append(", @JobTitle");
            sqlCommandText.Append(", @Job");
            sqlCommandText.Append(", @ResearchTopics");
            sqlCommandText.Append(", @WorkUnit");
            sqlCommandText.Append(", @WorkUnitLevel");
            sqlCommandText.Append(", @SectionOffice");
            sqlCommandText.Append(", @InvoiceUnit");
            sqlCommandText.Append(", @Mentor");
            sqlCommandText.Append(", @Remark");
            sqlCommandText.Append(", @QQ");
            sqlCommandText.Append(", @MSN");
            sqlCommandText.Append(", @AuthorOpus");
            sqlCommandText.Append(", @ReserveField");
            sqlCommandText.Append(", @ReserveField1");
            sqlCommandText.Append(", @ReserveField2");
            sqlCommandText.Append(", @ReserveField3");
            sqlCommandText.Append(", @ReserveField4");
            sqlCommandText.Append(", @ReserveField5");


            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.AuthorDetail ({0}) VALUES ({1})", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, authorDetailEntity.AuthorID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, authorDetailEntity.JournalID);
            db.AddInParameter(cmd, "@AuthorName", DbType.AnsiString, authorDetailEntity.AuthorName);
            db.AddInParameter(cmd, "@EnglishName", DbType.AnsiString, authorDetailEntity.EnglishName);
            db.AddInParameter(cmd, "@Gender", DbType.AnsiString, authorDetailEntity.Gender);
            db.AddInParameter(cmd, "@Nation", DbType.AnsiString, authorDetailEntity.Nation);
            db.AddInParameter(cmd, "@Birthday", DbType.DateTime, authorDetailEntity.Birthday);
            db.AddInParameter(cmd, "@NativePlace", DbType.AnsiString, authorDetailEntity.NativePlace);
            db.AddInParameter(cmd, "@Province", DbType.AnsiString, authorDetailEntity.Province);
            db.AddInParameter(cmd, "@City", DbType.AnsiString, authorDetailEntity.City);
            db.AddInParameter(cmd, "@Area", DbType.AnsiString, authorDetailEntity.Area);
            db.AddInParameter(cmd, "@IDCard", DbType.AnsiString, authorDetailEntity.IDCard);
            db.AddInParameter(cmd, "@Address", DbType.AnsiString, authorDetailEntity.Address);
            db.AddInParameter(cmd, "@ZipCode", DbType.AnsiString, authorDetailEntity.ZipCode);
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, authorDetailEntity.Mobile);
            db.AddInParameter(cmd, "@Tel", DbType.AnsiString, authorDetailEntity.Tel);
            db.AddInParameter(cmd, "@Fax", DbType.AnsiString, authorDetailEntity.Fax);
            db.AddInParameter(cmd, "@Education", DbType.AnsiString, authorDetailEntity.Education);
            db.AddInParameter(cmd, "@Professional", DbType.AnsiString, authorDetailEntity.Professional);
            db.AddInParameter(cmd, "@JobTitle", DbType.AnsiString, authorDetailEntity.JobTitle);
            db.AddInParameter(cmd, "@Job", DbType.AnsiString, authorDetailEntity.Job);
            db.AddInParameter(cmd, "@ResearchTopics", DbType.AnsiString, authorDetailEntity.ResearchTopics);
            db.AddInParameter(cmd, "@WorkUnit", DbType.AnsiString, authorDetailEntity.WorkUnit);
            db.AddInParameter(cmd, "@WorkUnitLevel", DbType.Int32, authorDetailEntity.WorkUnitLevel);
            db.AddInParameter(cmd, "@SectionOffice", DbType.AnsiString, authorDetailEntity.SectionOffice);
            db.AddInParameter(cmd, "@InvoiceUnit", DbType.AnsiString, authorDetailEntity.InvoiceUnit);
            db.AddInParameter(cmd, "@Mentor", DbType.AnsiString, authorDetailEntity.Mentor);
            db.AddInParameter(cmd, "@Remark", DbType.AnsiString, authorDetailEntity.Remark);
            db.AddInParameter(cmd, "@QQ", DbType.AnsiString, authorDetailEntity.QQ);
            db.AddInParameter(cmd, "@MSN", DbType.AnsiString, authorDetailEntity.MSN);
            db.AddInParameter(cmd, "@AuthorOpus", DbType.String, authorDetailEntity.AuthorOpus);
            db.AddInParameter(cmd, "@ReserveField", DbType.AnsiString, authorDetailEntity.ReserveField);
            db.AddInParameter(cmd, "@ReserveField1", DbType.AnsiString, authorDetailEntity.ReserveField1);
            db.AddInParameter(cmd, "@ReserveField2", DbType.AnsiString, authorDetailEntity.ReserveField2);
            db.AddInParameter(cmd, "@ReserveField3", DbType.AnsiString, authorDetailEntity.ReserveField3);
            db.AddInParameter(cmd, "@ReserveField4", DbType.AnsiString, authorDetailEntity.ReserveField4);
            db.AddInParameter(cmd, "@ReserveField5", DbType.AnsiString, authorDetailEntity.ReserveField5);

            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("新增作者详细信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 更新数据

        public bool UpdateAuthorDetail(AuthorDetailEntity authorDetailEntity, DbTransaction trans = null)
        {
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  PKID=@PKID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" AuthorName=@AuthorName");
            sqlCommandText.Append(", EnglishName=@EnglishName");
            sqlCommandText.Append(", Gender=@Gender");
            sqlCommandText.Append(", Nation=@Nation");
            sqlCommandText.Append(", Birthday=@Birthday");
            sqlCommandText.Append(", NativePlace=@NativePlace");
            sqlCommandText.Append(", Province=@Province");
            sqlCommandText.Append(", City=@City");
            sqlCommandText.Append(", Area=@Area");
            sqlCommandText.Append(", IDCard=@IDCard");
            sqlCommandText.Append(", Address=@Address");
            sqlCommandText.Append(", ZipCode=@ZipCode");
            sqlCommandText.Append(", Mobile=@Mobile");
            sqlCommandText.Append(", Tel=@Tel");
            sqlCommandText.Append(", Fax=@Fax");
            sqlCommandText.Append(", Education=@Education");
            sqlCommandText.Append(", Professional=@Professional");
            sqlCommandText.Append(", JobTitle=@JobTitle");
            sqlCommandText.Append(", Job=@Job");
            sqlCommandText.Append(", ResearchTopics=@ResearchTopics");
            sqlCommandText.Append(", WorkUnit=@WorkUnit");
            sqlCommandText.Append(", WorkUnitLevel=@WorkUnitLevel");
            sqlCommandText.Append(", SectionOffice=@SectionOffice");
            sqlCommandText.Append(", InvoiceUnit=@InvoiceUnit");
            sqlCommandText.Append(", Mentor=@Mentor");
            sqlCommandText.Append(", Remark=@Remark");
            sqlCommandText.Append(", QQ=@QQ");
            sqlCommandText.Append(", MSN=@MSN");
            sqlCommandText.Append(", AuthorOpus=@AuthorOpus");
            sqlCommandText.Append(", ReserveField=@ReserveField");
            sqlCommandText.Append(", ReserveField1=@ReserveField1");
            sqlCommandText.Append(", ReserveField2=@ReserveField2");
            sqlCommandText.Append(", ReserveField3=@ReserveField3");
            sqlCommandText.Append(", ReserveField4=@ReserveField4");
            sqlCommandText.Append(", ReserveField5=@ReserveField5");


            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.AuthorDetail SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@PKID", DbType.Int64, authorDetailEntity.PKID);
            db.AddInParameter(cmd, "@AuthorName", DbType.AnsiString, authorDetailEntity.AuthorName);
            db.AddInParameter(cmd, "@EnglishName", DbType.AnsiString, authorDetailEntity.EnglishName);
            db.AddInParameter(cmd, "@Gender", DbType.AnsiString, authorDetailEntity.Gender);
            db.AddInParameter(cmd, "@Nation", DbType.AnsiString, authorDetailEntity.Nation);
            db.AddInParameter(cmd, "@Birthday", DbType.DateTime, authorDetailEntity.Birthday);
            db.AddInParameter(cmd, "@NativePlace", DbType.AnsiString, authorDetailEntity.NativePlace);
            db.AddInParameter(cmd, "@Province", DbType.AnsiString, authorDetailEntity.Province);
            db.AddInParameter(cmd, "@City", DbType.AnsiString, authorDetailEntity.City);
            db.AddInParameter(cmd, "@Area", DbType.AnsiString, authorDetailEntity.Area);
            db.AddInParameter(cmd, "@IDCard", DbType.AnsiString, authorDetailEntity.IDCard);
            db.AddInParameter(cmd, "@Address", DbType.AnsiString, authorDetailEntity.Address);
            db.AddInParameter(cmd, "@ZipCode", DbType.AnsiString, authorDetailEntity.ZipCode);
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, authorDetailEntity.Mobile);
            db.AddInParameter(cmd, "@Tel", DbType.AnsiString, authorDetailEntity.Tel);
            db.AddInParameter(cmd, "@Fax", DbType.AnsiString, authorDetailEntity.Fax);
            db.AddInParameter(cmd, "@Education", DbType.AnsiString, authorDetailEntity.Education);
            db.AddInParameter(cmd, "@Professional", DbType.AnsiString, authorDetailEntity.Professional);
            db.AddInParameter(cmd, "@JobTitle", DbType.AnsiString, authorDetailEntity.JobTitle);
            db.AddInParameter(cmd, "@Job", DbType.AnsiString, authorDetailEntity.Job);
            db.AddInParameter(cmd, "@ResearchTopics", DbType.AnsiString, authorDetailEntity.ResearchTopics);
            db.AddInParameter(cmd, "@WorkUnit", DbType.AnsiString, authorDetailEntity.WorkUnit);
            db.AddInParameter(cmd, "@WorkUnitLevel", DbType.Int32, authorDetailEntity.WorkUnitLevel);
            db.AddInParameter(cmd, "@SectionOffice", DbType.AnsiString, authorDetailEntity.SectionOffice);
            db.AddInParameter(cmd, "@InvoiceUnit", DbType.AnsiString, authorDetailEntity.InvoiceUnit);
            db.AddInParameter(cmd, "@Mentor", DbType.AnsiString, authorDetailEntity.Mentor);
            db.AddInParameter(cmd, "@Remark", DbType.AnsiString, authorDetailEntity.Remark);
            db.AddInParameter(cmd, "@QQ", DbType.AnsiString, authorDetailEntity.QQ);
            db.AddInParameter(cmd, "@MSN", DbType.AnsiString, authorDetailEntity.MSN);
            db.AddInParameter(cmd, "@AuthorOpus", DbType.AnsiString, authorDetailEntity.AuthorOpus);
            db.AddInParameter(cmd, "@ReserveField", DbType.AnsiString, authorDetailEntity.ReserveField);
            db.AddInParameter(cmd, "@ReserveField1", DbType.AnsiString, authorDetailEntity.ReserveField1);
            db.AddInParameter(cmd, "@ReserveField2", DbType.AnsiString, authorDetailEntity.ReserveField2);
            db.AddInParameter(cmd, "@ReserveField3", DbType.AnsiString, authorDetailEntity.ReserveField3);
            db.AddInParameter(cmd, "@ReserveField4", DbType.AnsiString, authorDetailEntity.ReserveField4);
            db.AddInParameter(cmd, "@ReserveField5", DbType.AnsiString, authorDetailEntity.ReserveField5);

            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("编辑作者详细信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除对象

        #region 删除一个对象

        public bool DeleteAuthorDetail(AuthorDetailEntity authorDetailEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.AuthorDetail");
            sqlCommandText.Append(" WHERE  PKID=@PKID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            db.AddInParameter(cmd, "@PKID", DbType.Int64, authorDetailEntity.PKID);

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

        public bool DeleteAuthorDetail(Int64 pKID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.AuthorDetail");
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
        public bool BatchDeleteAuthorDetail(Int64[] pKID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from AuthorDetail where ");

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

        public AuthorDetailEntity MakeAuthorDetail(IDataReader dr)
        {
            AuthorDetailEntity authorDetailEntity = null;
            if (dr.Read())
            {
                authorDetailEntity = new AuthorDetailEntity();
                if (dr.HasColumn("PKID"))
                {
                    authorDetailEntity.PKID = dr.GetDrValue<Int64>("PKID");
                }
                if (dr.HasColumn("bAuthorID"))
                {
                    authorDetailEntity.AuthorID = dr.GetDrValue<Int64>("bAuthorID");
                }
                if (dr.HasColumn("bJournalID"))
                {
                    authorDetailEntity.JournalID = dr.GetDrValue<Int64>("bJournalID");
                }
                authorDetailEntity.AuthorName = dr.GetDrValue<String>("AuthorName");
                authorDetailEntity.EnglishName = dr.GetDrValue<String>("EnglishName");
                authorDetailEntity.Gender = dr.GetDrValue<String>("Gender");
                authorDetailEntity.Nation = dr.GetDrValue<String>("Nation");
                authorDetailEntity.Birthday = dr.GetDrValue<DateTime?>("Birthday");
                authorDetailEntity.NativePlace = dr.GetDrValue<String>("NativePlace");
                authorDetailEntity.Province = dr.GetDrValue<String>("Province");
                authorDetailEntity.City = dr.GetDrValue<String>("City");
                authorDetailEntity.Area = dr.GetDrValue<String>("Area");
                authorDetailEntity.IDCard = dr.GetDrValue<String>("IDCard");
                authorDetailEntity.Address = dr.GetDrValue<String>("Address");
                authorDetailEntity.ZipCode = dr.GetDrValue<String>("ZipCode");
                authorDetailEntity.Mobile = dr.GetDrValue<String>("Mobile");
                authorDetailEntity.Tel = dr.GetDrValue<String>("Tel");
                authorDetailEntity.Fax = dr.GetDrValue<String>("Fax");
                authorDetailEntity.Education = dr.GetDrValue<Int32>("Education");
                authorDetailEntity.Professional = dr.GetDrValue<String>("Professional");
                authorDetailEntity.JobTitle = dr.GetDrValue<Int32>("JobTitle");
                authorDetailEntity.Job = dr.GetDrValue<String>("Job");
                authorDetailEntity.ResearchTopics = dr.GetDrValue<String>("ResearchTopics");
                authorDetailEntity.WorkUnit = dr.GetDrValue<String>("WorkUnit");
                authorDetailEntity.WorkUnitLevel = dr.GetDrValue<Int32>("WorkUnitLevel");
                authorDetailEntity.SectionOffice = dr.GetDrValue<String>("SectionOffice");
                authorDetailEntity.InvoiceUnit = dr.GetDrValue<String>("InvoiceUnit");
                authorDetailEntity.Mentor = dr.GetDrValue<String>("Mentor");
                authorDetailEntity.Remark = dr.GetDrValue<String>("Remark");
                authorDetailEntity.QQ = dr.GetDrValue<String>("QQ");
                authorDetailEntity.MSN = dr.GetDrValue<String>("MSN");
                authorDetailEntity.AuthorOpus = dr.GetDrValue<String>("AuthorOpus");
                authorDetailEntity.ReserveField = dr.GetDrValue<String>("ReserveField");
                authorDetailEntity.ReserveField1 = dr.GetDrValue<String>("ReserveField1");
                authorDetailEntity.ReserveField2 = dr.GetDrValue<String>("ReserveField2");
                authorDetailEntity.ReserveField3 = dr.GetDrValue<String>("ReserveField3");
                authorDetailEntity.ReserveField4 = dr.GetDrValue<String>("ReserveField4");
                authorDetailEntity.ReserveField5 = dr.GetDrValue<String>("ReserveField5");
                if (dr.HasColumn("bAddDate"))
                {
                    authorDetailEntity.AddDate = dr.GetDrValue<DateTime>("bAddDate");
                }
                authorDetailEntity.AuthorModel = MakeAuthorInfo(dr);
                if (authorDetailEntity.AuthorModel.GroupID == 3)
                    authorDetailEntity.ExpertGroupList = GetExpertGroupMapList(authorDetailEntity.AuthorID);
            }
            dr.Close();
            return authorDetailEntity;
        }

        public AuthorDetailEntity MakeAuthorDetail(DataRow dr)
        {
            AuthorDetailEntity authorDetailEntity = null;
            if (dr != null)
            {
                authorDetailEntity = new AuthorDetailEntity();
                authorDetailEntity.PKID = (Int64)dr["PKID"];
                authorDetailEntity.AuthorID = (Int64)dr["AuthorID"];
                authorDetailEntity.JournalID = (Int64)dr["JournalID"];
                authorDetailEntity.AuthorName = (String)dr["AuthorName"];
                authorDetailEntity.EnglishName = (String)dr["EnglishName"];
                authorDetailEntity.Gender = (String)dr["Gender"];
                authorDetailEntity.Nation = (String)dr["Nation"];
                authorDetailEntity.Birthday = Convert.IsDBNull(dr["Birthday"]) ? null : (DateTime?)dr["Birthday"];
                authorDetailEntity.NativePlace = (String)dr["NativePlace"];
                authorDetailEntity.IDCard = "'"+(String)dr["IDCard"]+"'";
                authorDetailEntity.Address = (String)dr["Address"];
                authorDetailEntity.ZipCode = (String)dr["ZipCode"];
                authorDetailEntity.Mobile = (String)dr["Mobile"];
                authorDetailEntity.Tel = (String)dr["Tel"];
                authorDetailEntity.Fax = (String)dr["Fax"];
                authorDetailEntity.Education = (Int32)dr["Education"];
                authorDetailEntity.Professional = (String)dr["Professional"];
                authorDetailEntity.JobTitle = (Int32)dr["JobTitle"];
                authorDetailEntity.Job = (String)dr["Job"];
                authorDetailEntity.ResearchTopics = (String)dr["ResearchTopics"];
                authorDetailEntity.WorkUnit = (String)dr["WorkUnit"];
                authorDetailEntity.WorkUnitLevel = (Int32)dr["WorkUnitLevel"];
                authorDetailEntity.SectionOffice = (String)dr["SectionOffice"];
                authorDetailEntity.InvoiceUnit = (String)dr["InvoiceUnit"];
                authorDetailEntity.Mentor = (String)dr["Mentor"];
                authorDetailEntity.Remark = (String)dr["Remark"];
                authorDetailEntity.AuthorOpus = (String)dr["AuthorOpus"];
                authorDetailEntity.QQ = (String)dr["QQ"];
                authorDetailEntity.MSN = (String)dr["MSN"];
                authorDetailEntity.ReserveField = Convert.IsDBNull(dr["ReserveField"]) ? null : (String)dr["ReserveField"];
                authorDetailEntity.ReserveField1 = Convert.IsDBNull(dr["ReserveField1"]) ? null : (String)dr["ReserveField1"];
                authorDetailEntity.ReserveField2 = Convert.IsDBNull(dr["ReserveField2"]) ? null : (String)dr["ReserveField2"];
                authorDetailEntity.ReserveField3 = Convert.IsDBNull(dr["ReserveField3"]) ? null : (String)dr["ReserveField3"];
                authorDetailEntity.ReserveField4 = Convert.IsDBNull(dr["ReserveField4"]) ? null : (String)dr["ReserveField4"];
                authorDetailEntity.ReserveField5 = Convert.IsDBNull(dr["ReserveField5"]) ? null : (String)dr["ReserveField5"];
                authorDetailEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return authorDetailEntity;
        }
        #endregion

        #region 根据数据组装一组对象数据

        public List<AuthorDetailEntity> MakeAuthorDetailList(IDataReader dr)
        {
            List<AuthorDetailEntity> list = new List<AuthorDetailEntity>();
            while (dr.Read())
            {
                AuthorDetailEntity authorDetailEntity = new AuthorDetailEntity();
                authorDetailEntity.PKID = dr.GetDrValue<Int64>("PKID");
                if (dr.HasColumn("bAuthorID"))
                {
                    authorDetailEntity.AuthorID = dr.GetDrValue<Int64>("bAuthorID");
                }
                if (dr.HasColumn("bJournalID"))
                {
                    authorDetailEntity.JournalID = dr.GetDrValue<Int64>("bJournalID");
                }
                authorDetailEntity.AuthorName = dr.GetDrValue<String>("AuthorName");
                authorDetailEntity.EnglishName = dr.GetDrValue<String>("EnglishName");
                authorDetailEntity.Gender = dr.GetDrValue<String>("Gender");
                authorDetailEntity.Nation = dr.GetDrValue<String>("Nation");
                authorDetailEntity.Birthday = dr.GetDrValue<DateTime?>("Birthday");
                authorDetailEntity.NativePlace = dr.GetDrValue<String>("NativePlace");
                authorDetailEntity.Province = dr.GetDrValue<String>("Province");
                authorDetailEntity.City = dr.GetDrValue<String>("City");
                authorDetailEntity.Area = dr.GetDrValue<String>("Area");
                authorDetailEntity.IDCard = "'"+dr.GetDrValue<String>("IDCard")+"'";
                authorDetailEntity.Address = dr.GetDrValue<String>("Address");
                authorDetailEntity.ZipCode = dr.GetDrValue<String>("ZipCode");
                authorDetailEntity.Mobile = dr.GetDrValue<String>("Mobile");
                authorDetailEntity.Tel = dr.GetDrValue<String>("Tel");
                authorDetailEntity.Fax = dr.GetDrValue<String>("Fax");
                authorDetailEntity.Education = dr.GetDrValue<Int32>("Education");
                authorDetailEntity.Professional = dr.GetDrValue<String>("Professional");
                authorDetailEntity.JobTitle = dr.GetDrValue<Int32>("JobTitle");
                authorDetailEntity.Job = dr.GetDrValue<String>("Job");
                authorDetailEntity.ResearchTopics = dr.GetDrValue<String>("ResearchTopics");
                authorDetailEntity.WorkUnit = dr.GetDrValue<String>("WorkUnit");
                authorDetailEntity.WorkUnitLevel = dr.GetDrValue<Int32>("WorkUnitLevel");
                authorDetailEntity.SectionOffice = dr.GetDrValue<String>("SectionOffice");
                authorDetailEntity.InvoiceUnit = dr.GetDrValue<String>("InvoiceUnit");
                authorDetailEntity.Mentor = dr.GetDrValue<String>("Mentor");
                authorDetailEntity.Remark = dr.GetDrValue<String>("Remark");
                authorDetailEntity.AuthorOpus = dr.GetDrValue<String>("AuthorOpus");
                authorDetailEntity.QQ = dr.GetDrValue<String>("QQ");
                authorDetailEntity.MSN = dr.GetDrValue<String>("MSN");
                authorDetailEntity.ReserveField = dr.GetDrValue<String>("ReserveField");
                authorDetailEntity.ReserveField1 = dr.GetDrValue<String>("ReserveField1");
                authorDetailEntity.ReserveField2 = dr.GetDrValue<String>("ReserveField2");
                authorDetailEntity.ReserveField3 = dr.GetDrValue<String>("ReserveField3");
                authorDetailEntity.ReserveField4 = dr.GetDrValue<String>("ReserveField4");
                authorDetailEntity.ReserveField5 = dr.GetDrValue<String>("ReserveField5");
                if (dr.HasColumn("bAddDate"))
                {
                    authorDetailEntity.AddDate = dr.GetDrValue<DateTime>("bAddDate");
                }
                authorDetailEntity.AuthorModel = MakeAuthorInfo(dr);
                authorDetailEntity.AuthorName = authorDetailEntity.AuthorModel.RealName;
                list.Add(authorDetailEntity);
            }
            dr.Close();
            return list;
        }

        public AuthorInfoEntity MakeAuthorInfo(IDataReader dr)
        {
            AuthorInfoEntity authorInfoEntity = new AuthorInfoEntity();
            if (dr.HasColumn("bAuthorID"))
            {
                authorInfoEntity.AuthorID = dr.GetDrValue<Int64>("bAuthorID");
            }
            if (dr.HasColumn("bJournalID"))
            {
                authorInfoEntity.JournalID = dr.GetDrValue<Int64>("bJournalID");
            }
            authorInfoEntity.LoginName = dr.GetDrValue<String>("LoginName");
            authorInfoEntity.Pwd = dr.GetDrValue<String>("Pwd");
            authorInfoEntity.RealName = dr.GetDrValue<String>("RealName");
            authorInfoEntity.Mobile = dr.GetDrValue<String>("Mobile");
            authorInfoEntity.LoginIP = dr.GetDrValue<String>("LoginIP");
            authorInfoEntity.LoginCount = dr.GetDrValue<Int32>("LoginCount");
            authorInfoEntity.LoginDate = dr.GetDrValue<DateTime>("LoginDate");
            authorInfoEntity.Status = dr.GetDrValue<Byte>("Status");
            authorInfoEntity.GroupID = dr.GetDrValue<Byte>("GroupID");
            if (dr.HasColumn("bAddDate"))
            {
                authorInfoEntity.AddDate = dr.GetDrValue<DateTime>("bAddDate");
            }
            return authorInfoEntity;
        }


        public List<AuthorDetailEntity> MakeAuthorDetailList(DataTable dt)
        {
            List<AuthorDetailEntity> list = new List<AuthorDetailEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AuthorDetailEntity authorDetailEntity = MakeAuthorDetail(dt.Rows[i]);
                    list.Add(authorDetailEntity);
                }
            }
            return list;
        }

        #endregion

        #region 保存作者信息
        /// <summary>
        /// 保存作者信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveAuthor(AuthorDetailEntity model)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    if (model.PKID == 0)//新增
                    {
                        if (model.AuthorModel == null)
                            model.AuthorModel = new AuthorInfoEntity();
                        model.AuthorModel.Mobile = model.Mobile;
                        model.AuthorModel.RealName = model.AuthorName;
                        model.AuthorModel.JournalID = model.JournalID;
                        if (model.AuthorModel.AuthorID == 0)  //作者基本信息也不存在
                        {
                            //新增作者基本信息

                            Int64 AuthorID = AddAuthor(model.AuthorModel, trans);

                            if (AuthorID == 0)
                                throw new Exception();

                            model.AuthorID = AuthorID;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(model.AuthorModel.LoginName))
                                UpdateAuthor(model.AuthorModel, trans);
                            model.AuthorID = model.AuthorModel.AuthorID;
                        }

                        //新增作者详细
                        AddAuthorDetail(model, trans);

                        //新增专家分组
                        if (model.AuthorModel.GroupID == 3)
                        {
                            if (model.ExpertGroupList != null && model.ExpertGroupList.Count > 0)
                            {
                                foreach (var item in model.ExpertGroupList)
                                {
                                    item.JournalID = model.JournalID;
                                    item.AuthorID = model.AuthorID;
                                    AddExpertGroupMap(item, trans);
                                }
                            }
                        }
                    }
                    else  //编辑
                    {
                        UpdateAuthorDetail(model, trans);
                        UpdateAuthor(model.AuthorName, model.Mobile,model.AuthorID);

                        if (model.AuthorModel != null)
                        {
                            model.AuthorModel.JournalID = model.JournalID;
                            model.AuthorModel.AuthorID = model.AuthorID;
                            model.AuthorModel.Mobile = model.Mobile;
                            model.AuthorModel.RealName = model.AuthorName;
                            if (!string.IsNullOrWhiteSpace(model.AuthorModel.LoginName))
                                UpdateAuthor(model.AuthorModel, trans);

                            if (model.AuthorModel.GroupID == 3 && model.ExpertGroupList != null)
                            {
                                DelExpertGroupMap(new Int64[] { model.AuthorID }, trans);
                                if (model.ExpertGroupList.Count > 0)
                                {
                                    foreach (var item in model.ExpertGroupList)
                                    {
                                        item.JournalID = model.JournalID;
                                        item.AuthorID = model.AuthorID;
                                        AddExpertGroupMap(item, trans);
                                    }
                                }
                            }
                        }
                        else
                        {
                            UpdateAuthorMobile(model, trans);
                        }

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

        /// <summary>
        /// 登录名是否存在
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        private bool LoginNameIsExists(string loginName, Int64 AuthorID, Int64 JournalID)
        {
            string strSql = "SELECT 1 FROM dbo.AuthorInfo with(nolock) WHERE LoginName=@LoginName AND JournalID=@JournalID";
            if (AuthorID > 0)
                strSql += " and AuthorID<>" + AuthorID;
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@LoginName", DbType.AnsiString, loginName);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, JournalID);
            return db.ExecuteScalar(cmd).TryParse<Int32>() == 1;
        }

        /// <summary>
        /// 添加作者基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        private Int64 AddAuthor(AuthorInfoEntity model, DbTransaction trans = null)
        {
            if (LoginNameIsExists(model.LoginName, 0, model.JournalID))
                throw new Exception("该登录名已经存在！");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @LoginName");
            sqlCommandText.Append(", @Pwd");
            sqlCommandText.Append(", @RealName");
            sqlCommandText.Append(", @Mobile");
            sqlCommandText.Append(", @GroupID");
            sqlCommandText.Append(", @Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.AuthorInfo ({0}) VALUES ({1});select SCOPE_IDENTITY();", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@LoginName", DbType.AnsiString, model.LoginName);
            db.AddInParameter(cmd, "@Pwd", DbType.AnsiString, model.Pwd);
            db.AddInParameter(cmd, "@RealName", DbType.AnsiString, model.RealName);
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, model.Mobile);
            db.AddInParameter(cmd, "@GroupID", DbType.Byte, model.GroupID);
            db.AddInParameter(cmd, "@Status", DbType.Byte, model.Status);

            try
            {
                object obj = null;
                if (trans == null)
                    obj = db.ExecuteScalar(cmd);
                else
                    obj = db.ExecuteScalar(cmd, trans);
                if (obj == null)
                    return 0;
                Int64 id = 0;
                Int64.TryParse(obj.ToString(), out id);
                if (id == 0)
                    throw new Exception("新增" + (model.GroupID == 2 ? "作者" : "专家") + "基本信息失败！");
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑作者基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        private bool UpdateAuthor(AuthorInfoEntity model, DbTransaction trans = null)
        {
            if (LoginNameIsExists(model.LoginName, model.AuthorID, model.JournalID))
                throw new Exception("该登录名已经存在！");
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  AuthorID=@AuthorID ");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("  LoginName=@LoginName");
            if (!string.IsNullOrWhiteSpace(model.Pwd))
                sqlCommandText.Append(", Pwd=@Pwd");
            sqlCommandText.Append(", RealName=@RealName");
            sqlCommandText.Append(", Mobile=@Mobile");
            sqlCommandText.Append(", Status=@Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.AuthorInfo SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, model.AuthorID);
            db.AddInParameter(cmd, "@LoginName", DbType.AnsiString, model.LoginName);
            if (!string.IsNullOrWhiteSpace(model.Pwd))
                db.AddInParameter(cmd, "@Pwd", DbType.AnsiString, model.Pwd);
            db.AddInParameter(cmd, "@RealName", DbType.AnsiString, model.RealName);
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, model.Mobile);
            db.AddInParameter(cmd, "@Status", DbType.Byte, model.Status);

            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("编辑" + (model.GroupID == 2 ? "作者" : "专家") + "基本信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改个人信息时同步修改基本信息
        /// </summary>
        /// <param name="RealName"></param>
        /// <param name="Mobile"></param>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        private bool UpdateAuthor(string RealName, string Mobile,long AuthorID)
        {
            
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  AuthorID=@AuthorID ");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" RealName=@RealName");
            sqlCommandText.Append(", Mobile=@Mobile");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.AuthorInfo SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@AuthorID", DbType.Int64,AuthorID);
            db.AddInParameter(cmd, "@RealName", DbType.AnsiString, RealName);
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, Mobile);

            try
            {
                bool result = false;
                result = db.ExecuteNonQuery(cmd) > 0;
                
                if (!result)
                    throw new Exception("同步修改基本信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除作者基本信息
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        private bool DelAuthor(Int64[] AuthorID, DbTransaction trans = null)
        {
            string strSql = "DELETE dbo.AuthorInfo WHERE AuthorID";
            if (AuthorID.Length == 1)
                strSql += " = " + AuthorID[0];
            else
                strSql += string.Format(" in ({0})", string.Join(",", AuthorID));
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
        /// 删除专家分类
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        private bool DelExpertGroupMap(Int64[] AuthorID, DbTransaction trans = null)
        {
            string strSql = "DELETE dbo.ExpertGroupMap WHERE AuthorID";
            if (AuthorID.Length == 1)
                strSql += " = " + AuthorID[0];
            else
                strSql += string.Format(" in ({0})", string.Join(",", AuthorID));
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
        /// 添加专家分类
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        private bool AddExpertGroupMap(ExpertGroupMapEntity model, DbTransaction trans)
        {
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @AuthorID");
            sqlCommandText.Append(", @ExpertGroupID");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.ExpertGroupMap ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, model.AuthorID);
            db.AddInParameter(cmd, "@ExpertGroupID", DbType.Int32, model.ExpertGroupID);

            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("新增专家分类信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取专家分类信息
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        private IList<ExpertGroupMapEntity> GetExpertGroupMapList(Int64 AuthorID)
        {
            string strSql = "SELECT * FROM dbo.ExpertGroupMap with(nolock) WHERE AuthorID=" + AuthorID;
            return db.GetList<ExpertGroupMapEntity>(strSql, (dr) =>
                {
                    List<ExpertGroupMapEntity> list = new List<ExpertGroupMapEntity>();
                    ExpertGroupMapEntity model = null;
                    while (dr.Read())
                    {
                        model = new ExpertGroupMapEntity();
                        model.PKID = dr.GetDrValue<Int64>("PKID");
                        model.JournalID = dr.GetDrValue<Int64>("JournalID");
                        model.AuthorID = dr.GetDrValue<Int64>("AuthorID");
                        model.ExpertGroupID = dr.GetDrValue<Int32>("ExpertGroupID");
                        model.AddDate = dr.GetDrValue<DateTime>("AddDate");
                        list.Add(model);
                    }
                    return list;
                });
        }

        /// <summary>
        /// 修改作者信息表手机信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        private bool UpdateAuthorMobile(AuthorDetailEntity model, DbTransaction trans)
        {
            DbCommand cmd = db.GetSqlStringCommand("UPDATE dbo.AuthorInfo set Mobile=@Mobile,RealName=@RealName WHERE AuthorID=@AuthorID");
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, model.Mobile);
            db.AddInParameter(cmd, "@RealName", DbType.AnsiString, model.AuthorName);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, model.AuthorID);
            return db.ExecuteNonQuery(cmd, trans) > 0;
        }
        #endregion

        #region 删除作者信息
        /// <summary>
        /// 删除作者信息
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        public bool DelAuthorDetail(Int64[] AuthorID)
        {
            if (AuthorID == null || AuthorID.Length < 1)
                return false;
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    StringBuilder sqlCommandText = new StringBuilder();
                    sqlCommandText.Append("delete from AuthorDetail where AuthorID");
                    if (AuthorID.Length > 1)
                        sqlCommandText.AppendFormat(" in ({0})", string.Join(",", AuthorID));
                    else
                        sqlCommandText.AppendFormat(" ={0}", AuthorID[0]);
                    DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

                    db.ExecuteNonQuery(cmd, trans);

                    DelAuthor(AuthorID, trans);

                    DelExpertGroupMap(AuthorID, trans);

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

        #region 专家分组配置
        /// <summary>
        /// 获取专家分组信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorDetailEntity> GetExpertGroupMapList(ExpertGroupMapEntity query)
        {
            string strSql = @"    select a.AuthorID,a.LoginName,a.RealName,c.PKID   from                         
(SELECT b.AuthorID ,b.LoginName ,b.RealName,B.JournalID FROM dbo.AuthorDetail a with(nolock) 
                              RIGHT JOIN dbo.AuthorInfo b with(nolock) ON a.JournalID=b.JournalID and a.AuthorID=b.AuthorID WHERE  b.JournalID=" + query.JournalID + " and (b.GroupID=3 or exists(select 1 from dbo.RoleAuthor m with(nolock) WHERE b.JournalID=m.JournalID and b.AuthorID=m.AuthorID and m.RoleID=3))) as A LEFT JOIN dbo.ExpertGroupMap c  ON   A.JournalID=c.JournalID and a.AuthorID=c.AuthorID and c.ExpertGroupID=" + query.ExpertGroupID + "  ORDER BY RealName";

            return db.GetList<AuthorDetailEntity>(strSql, (dr) =>
            {
                List<AuthorDetailEntity> list = new List<AuthorDetailEntity>();
                AuthorDetailEntity model = null;
                while (dr.Read())
                {
                    model = new AuthorDetailEntity();
                    model.AuthorID = dr.GetDrValue<Int64>("AuthorID");
                    model.Emial = dr.GetDrValue<String>("LoginName");
                    model.AuthorName = dr.GetDrValue<String>("RealName");
                    model.IsChecked = dr.GetDrValue<Int64>("PKID") > 0;
                    list.Add(model);
                }
                return list;
            });
        }

        /// <summary>
        /// 保存专家分组信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ExpertGroupID"></param>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        public bool SaveExpertGroupMap(IList<ExpertGroupMapEntity> list, Int32 ExpertGroupID, Int64 JournalID)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    DbCommand cmd = db.GetSqlStringCommand("Delete dbo.ExpertGroupMap WHERE ExpertGroupID=@ExpertGroupID AND JournalID=@JournalID");
                    db.AddInParameter(cmd, "@ExpertGroupID", DbType.Int64, ExpertGroupID);
                    db.AddInParameter(cmd, "@JournalID", DbType.Int64, JournalID);
                    db.ExecuteNonQuery(cmd, trans);

                    foreach (var model in list)
                    {
                        model.JournalID = JournalID;
                        AddExpertGroupMap(model, trans);
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

        /// <summary>
        /// 设置作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool SetAuthorExpert(AuthorDetailQuery query)
        {
            bool flag = false;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("UP_SetAuthorExpert");
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
                db.AddInParameter(cmd, "@AuthorIDs", DbType.String, string.Join(",", query.AuthorIDs));
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 取消作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool CancelAuthorExpert(AuthorDetailQuery query)
        {
            bool flag = false;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("UP_CancelAuthorExpert");
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
                db.AddInParameter(cmd, "@AuthorIDs", DbType.String, string.Join(",", query.AuthorIDs));
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }
    }
}

