using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Linq;
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
    public partial class ContributionInfoDataAccess:DataAccessBase
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;

        /// <summary>
        /// 数据初始化
        /// </summary>
        public ContributionInfoDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static ContributionInfoDataAccess _instance = new ContributionInfoDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static ContributionInfoDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }
        
        #region 稿件信息

        /// <summary>
        /// 获取稿件的附件
        /// </summary>
        /// <param name="cQuery">稿件ID</param>
        /// <returns></returns>
        public string GetContributionAttachment(ContributionInfoQuery cQuery)
        {
            string CPath = "";
            string sql = "SELECT TOP 1 ContributePath FROM dbo.ContributionInfo ci WITH(NOLOCK) WHERE ci.CID=@CID AND ci.JournalID=@JournalID";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@CID", DbType.Int64, cQuery.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, cQuery.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if ((dr.Read()))
                {
                    CPath = dr["ContributePath"].ToString();
                }
                dr.Close();
            }
            return CPath;
        }

        /// <summary>
        /// 投稿
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Int64 AuthorPlatform(ContributionInfoEntity model)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    if (model.CID == 0)//投稿
                    {
                        //稿件信息
                        Int64 CID = AddContributionInfo(model, trans);

                        if (CID == 0)
                            throw new Exception();

                        model.CID = CID;

                        //稿件作者
                        if (model.AuthorList != null && model.AuthorList.Count > 0)
                        {
                            foreach (var item in model.AuthorList)
                            {
                                item.JouranalID = model.JournalID;
                                item.CID = CID;
                                item.AuthorID = model.AuthorID;
                                AddContributionAuthor(item, trans);
                            }
                        }

                        //参考文献
                        if (model.ReferenceList != null && model.ReferenceList.Count > 0)
                        {
                            foreach (var item in model.ReferenceList)
                            {
                                item.JournalID = model.JournalID;
                                item.CID = CID;
                                AddContributionReference(item, trans);
                            }
                        }

                        //基金信息
                        if (model.FundList != null && model.FundList.Count > 0)
                        {
                            foreach (var item in model.FundList)
                            {
                                item.JournalID = model.JournalID;
                                item.CID = CID;
                                AddContributionFund(item, trans);
                            }
                        }

                        //大字段信息
                        if (model.AttModel == null)
                            model.AttModel = new ContributionInfoAttEntity();
                        var attModel = model.AttModel;
                        attModel.JournalID = model.JournalID;
                        attModel.CID = CID;
                        AddContributionInfoAtt(attModel, trans);
                    }
                    else  //编辑
                    {
                        UpdateContributionInfo(model, trans);

                        Int64[] CID = new Int64[] { model.CID };

                        //稿件作者
                        Int64[] CAuthorIDs = null;
                        if (model.AuthorList != null && model.AuthorList.Count > 0)
                        {
                            CAuthorIDs = model.AuthorList.Where(p => p.CAuthorID > 0).Select(p => p.CAuthorID).ToArray();
                        }
                        DelContributionAuthorByCID(CID, trans, CAuthorIDs);
                        if (model.AuthorList != null && model.AuthorList.Count > 0)
                        {
                            foreach (var item in model.AuthorList)
                            {                                
                                if (item.CAuthorID > 0)
                                {
                                    UpdateContributionAuthor(item, trans);
                                }
                                else
                                {
                                    item.JouranalID = model.JournalID;
                                    item.CID = model.CID;
                                    item.AuthorID = model.AuthorID;
                                    AddContributionAuthor(item, trans);
                                }
                            }
                        }

                        //参考文献
                        Int64[] ReferenceIDs = null;
                        if (model.ReferenceList != null && model.ReferenceList.Count > 0)
                        {
                            ReferenceIDs = model.ReferenceList.Where(p => p.ReferenceID > 0).Select(p => p.ReferenceID).ToArray();
                        }
                        DelContributionReferenceByCID(CID, trans, ReferenceIDs);
                        if (model.ReferenceList != null && model.ReferenceList.Count > 0)
                        {
                            foreach (var item in model.ReferenceList)
                            {
                                if (item.ReferenceID > 0)
                                {
                                    UpdateContributionReference(item, trans);
                                }
                                else
                                {
                                    item.JournalID = model.JournalID;
                                    item.CID = model.CID;
                                    AddContributionReference(item, trans);
                                }
                            }
                        }

                        //基金信息
                        Int64[] FundIDs = null;
                        if (model.FundList != null && model.FundList.Count > 0)
                        {
                            FundIDs = model.FundList.Where(p => p.FundID > 0).Select(p => p.FundID).ToArray();
                        }
                        DelContributionFundByCID(CID, trans, FundIDs);
                        if (model.FundList != null && model.FundList.Count > 0)
                        {
                            foreach (var item in model.FundList)
                            {
                                if (item.FundID > 0)
                                {
                                    UpdateContributionFund(item, trans);
                                }
                                else
                                {
                                    item.JournalID = model.JournalID;
                                    item.CID = model.CID;
                                    AddContributionFund(item, trans);
                                }
                            }
                        }

                        //大字段信息
                        if (model.AttModel == null)
                            model.AttModel = new ContributionInfoAttEntity();
                        var attModel = model.AttModel;
                        attModel.JournalID = model.JournalID;
                        attModel.CID = model.CID;
                        UpdateContributionInfoAtt(attModel, trans);
                    }

                    trans.Commit();
                    conn.Close();
                    return model.CID;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 保存稿件格式修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveFormat(ContributionInfoEntity model)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    UpdateContributionInfo(model, trans);

                    Int64[] CID = new Int64[] { model.CID };

                    //稿件作者
                    Int64[] CAuthorIDs = null;
                    if (model.AuthorList != null && model.AuthorList.Count > 0)
                    {
                        CAuthorIDs = model.AuthorList.Where(p => p.CAuthorID > 0).Select(p => p.CAuthorID).ToArray();
                    }
                    DelContributionAuthorByCID(CID, trans, CAuthorIDs);
                    if (model.AuthorList != null && model.AuthorList.Count > 0)
                    {
                        foreach (var item in model.AuthorList)
                        {
                            if (item.CAuthorID > 0)
                            {
                                UpdateContributionAuthor(item, trans);
                            }
                            else
                            {
                                item.JouranalID = model.JournalID;
                                item.CID = model.CID;
                                item.AuthorID = model.AuthorID;
                                AddContributionAuthor(item, trans);
                            }
                        }
                    }

                    //参考文献
                    Int64[] ReferenceIDs = null;
                    if (model.ReferenceList != null && model.ReferenceList.Count > 0)
                    {
                        ReferenceIDs = model.ReferenceList.Where(p => p.ReferenceID > 0).Select(p => p.ReferenceID).ToArray();
                    }
                    DelContributionReferenceByCID(CID, trans, ReferenceIDs);
                    if (model.ReferenceList != null && model.ReferenceList.Count > 0)
                    {
                        foreach (var item in model.ReferenceList)
                        {
                            if (item.ReferenceID > 0)
                            {
                                UpdateContributionReference(item, trans);
                            }
                            else
                            {
                                item.JournalID = model.JournalID;
                                item.CID = model.CID;
                                AddContributionReference(item, trans);
                            }
                        }
                    }

                    //基金信息
                    Int64[] FundIDs = null;
                    if (model.FundList != null && model.FundList.Count > 0)
                    {
                        FundIDs = model.FundList.Where(p => p.FundID > 0).Select(p => p.FundID).ToArray();
                    }
                    DelContributionFundByCID(CID, trans, FundIDs);
                    if (model.FundList != null && model.FundList.Count > 0)
                    {
                        foreach (var item in model.FundList)
                        {
                            if (item.FundID > 0)
                            {
                                UpdateContributionFund(item, trans);
                            }
                            else
                            {
                                item.JournalID = model.JournalID;
                                item.CID = model.CID;
                                AddContributionFund(item, trans);
                            }
                        }
                    }

                    //大字段信息
                    if (model.AttModel == null)
                        model.AttModel = new ContributionInfoAttEntity();
                    var attModel = model.AttModel;
                    attModel.JournalID = model.JournalID;
                    attModel.CID = model.CID;
                    UpdateContributionInfoAtt(attModel, trans);

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
        /// 新增稿件表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddContributionInfo(ContributionInfoEntity model)
        {
            return AddContributionInfo(model, null) > 0;
        }

        /// <summary>
        /// 编辑稿件表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateContributionInfo(ContributionInfoEntity model)
        {
            return UpdateContributionInfo(model, null);
        }

        /// <summary>
        /// 获取稿件信息实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionInfoEntity GetContributionInfo(Int64 cID)
        {
            ContributionInfoEntity model = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 * FROM dbo.ContributionInfo WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  CID=@CID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@CID", DbType.Int64, cID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    model = MakeContributionInfo(dr);
                }
                dr.Close();
            }
            return model;
        }

        /// <summary>
        /// 判断稿件标题是否存在
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="CID"></param>
        /// <param name="CTitle"></param>
        /// <returns></returns>
        public bool ContributionTitleIsExists(Int64 JournalID, Int64 CID, string CTitle)
        {
            string strSql = "SELECT 1 FROM dbo.ContributionInfo with(nolock) WHERE JournalID=@JournalID and Title=@Title and CID<>@CID";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, JournalID);
            db.AddInParameter(cmd, "@Title", DbType.String, CTitle);
            db.AddInParameter(cmd, "@CID", DbType.Int64, CID);
            return db.ExecuteScalar(cmd).TryParse<Int32>() == 1;
        }

        /// <summary>
        /// 改变稿件状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool ChangeStatus(ContributionInfoQuery query)
        {
            if (query.Status == null)
                return false;
            if (query.CID == 0 && query.CIDs.Length<1)
                return false;
            string strSql = string.Format("UPDATE dbo.ContributionInfo set Status={0} WHERE CID ", query.Status.Value);
            if (query.CID > 0)
                strSql += " = " + query.CID;
            else if (query.CIDs.Length == 1)
                strSql += " = " + query.CIDs[0];
            else
                strSql += string.Format(" in ({0})", string.Join(",", query.CIDs));
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(cmd) > 0;
        }

        /// <summary>
        /// 删除投稿信息
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public bool DelContributionInfo(Int64[] CID)
        {
            if (CID == null || CID.Length < 1)
                return false;
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    DelContributionAuthorByCID(CID, trans);

                    DelContributionReferenceByCID(CID, trans);

                    DelContributionFundByCID(CID, trans);

                    DelContributionInfoAttByCID(CID, trans);

                    string strSql = "Delete dbo.ContributionInfo where CID";
                    if (CID.Length == 0)
                        strSql += " = " + CID[0];
                    else
                        strSql += string.Format(" in ({0})", string.Join(",", CID));
                    DbCommand cmd = db.GetSqlStringCommand(strSql);
                    if (db.ExecuteNonQuery(cmd, trans) < 1)
                        throw new Exception("删除稿件表信息失败！");

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
        /// 获取稿件分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionInfoEntity> GetContributionInfoPageList(ContributionInfoQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderStr))
                query.OrderStr = " CID DESC";
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY " + query.OrderStr + ") AS ROW_ID FROM dbo.ContributionInfo with(nolock)"
                , sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.ContributionInfo with(nolock)";
            string whereSQL = GetContributionInfoFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<ContributionInfoEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeContributionInfoList);
        }

        /// <summary>
        /// 获取稿件数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionInfoEntity> GetContributionInfoList(ContributionInfoQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderStr))
                query.OrderStr = " CID DESC";
            string strSql = "SELECT * FROM dbo.ContributionInfo with(nolock)";
            string whereSQL = GetContributionInfoFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " ORDER BY " + query.OrderStr;
            return db.GetList<ContributionInfoEntity>(strSql, MakeContributionInfoList);
        }

        /// <summary>
        /// 新增稿件信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        private Int64 AddContributionInfo(ContributionInfoEntity model, DbTransaction trans)
        {
            StringBuilder sqlCommandText = new StringBuilder();          
            sqlCommandText.Append(" @CNumber");
            sqlCommandText.Append(", @AuthorID");
            sqlCommandText.Append(", @JournalID");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @EnTitle");
            sqlCommandText.Append(", @SubjectCat");
            sqlCommandText.Append(", @JChannelID");
            sqlCommandText.Append(", @ContributionType");
            sqlCommandText.Append(", @Special");
            sqlCommandText.Append(", @Keywords");
            sqlCommandText.Append(", @EnKeywords");
            sqlCommandText.Append(", @CLC");
            sqlCommandText.Append(", @IsFund");
            sqlCommandText.Append(", @InvoiceTitle");
            sqlCommandText.Append(", @CommendExpert");
            sqlCommandText.Append(", @ContributePath");
            sqlCommandText.Append(", @FigurePath");
            sqlCommandText.Append(", @PDFPath");
            sqlCommandText.Append(", @IntroLetterPath");
            sqlCommandText.Append(", @WordNiumber");
            sqlCommandText.Append(", @FigureNumber");
            sqlCommandText.Append(", @TableNumber");
            sqlCommandText.Append(", @ReferenceNumber");
            sqlCommandText.Append(", @Status");
            sqlCommandText.Append(", @IsPayPageFee");
            sqlCommandText.Append(", @IsPayAuditFee");
            sqlCommandText.Append(", @Year");
            sqlCommandText.Append(", @Volumn");
            sqlCommandText.Append(", @Issue");
            sqlCommandText.Append(", @SortID");
            sqlCommandText.Append(", @HireChannelID");
            sqlCommandText.Append(", @IsQuick");
            sqlCommandText.Append(", @ReserveField");
            sqlCommandText.Append(", @ReserveField1");
            sqlCommandText.Append(", @ReserveField2");
            sqlCommandText.Append(", @ReserveField3");
            sqlCommandText.Append(", @ReserveField4");
            sqlCommandText.Append(", @ReserveField5");
            sqlCommandText.Append(", @IsRetractions");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.ContributionInfo ({0},AddDate) VALUES ({1},getdate());select SCOPE_IDENTITY();", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@CNumber", DbType.AnsiString, model.CNumber);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, model.AuthorID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, model.Title.TextFilter());
            db.AddInParameter(cmd, "@EnTitle", DbType.AnsiString, model.EnTitle.TextFilter());
            db.AddInParameter(cmd, "@SubjectCat", DbType.Int32, model.SubjectCat);
            db.AddInParameter(cmd, "@JChannelID", DbType.Int64, model.JChannelID);
            db.AddInParameter(cmd, "@ContributionType", DbType.Int32, model.ContributionType);
            db.AddInParameter(cmd, "@Special", DbType.Int32, model.Special);
            db.AddInParameter(cmd, "@Keywords", DbType.AnsiString, model.Keywords.TextFilter());
            db.AddInParameter(cmd, "@EnKeywords", DbType.AnsiString, model.EnKeywords.TextFilter());
            db.AddInParameter(cmd, "@CLC", DbType.AnsiString, model.CLC.TextFilter());
            db.AddInParameter(cmd, "@IsFund", DbType.Boolean, model.IsFund);
            db.AddInParameter(cmd, "@InvoiceTitle", DbType.AnsiString, model.InvoiceTitle.TextFilter());
            db.AddInParameter(cmd, "@CommendExpert", DbType.AnsiString, model.CommendExpert.TextFilter());
            db.AddInParameter(cmd, "@ContributePath", DbType.AnsiString, model.ContributePath);
            db.AddInParameter(cmd, "@FigurePath", DbType.AnsiString, model.FigurePath);
            db.AddInParameter(cmd, "@PDFPath", DbType.AnsiString, model.PDFPath);
            db.AddInParameter(cmd, "@IntroLetterPath", DbType.AnsiString, model.IntroLetterPath);
            db.AddInParameter(cmd, "@WordNiumber", DbType.Int32, model.WordNiumber);
            db.AddInParameter(cmd, "@FigureNumber", DbType.Int32, model.FigureNumber);
            db.AddInParameter(cmd, "@TableNumber", DbType.Int32, model.TableNumber);
            db.AddInParameter(cmd, "@ReferenceNumber", DbType.Int32, model.ReferenceNumber);
            db.AddInParameter(cmd, "@Status", DbType.Int32, model.Status);
            db.AddInParameter(cmd, "@IsPayPageFee", DbType.Byte, model.IsPayPageFee);
            db.AddInParameter(cmd, "@IsPayAuditFee", DbType.Byte, model.IsPayAuditFee);
            db.AddInParameter(cmd, "@Year", DbType.Int32, model.Year);
            db.AddInParameter(cmd, "@Volumn", DbType.Int32, model.Volumn);
            db.AddInParameter(cmd, "@Issue", DbType.Int32, model.Issue);
            db.AddInParameter(cmd, "@SortID", DbType.Int32, model.SortID);
            db.AddInParameter(cmd, "@HireChannelID", DbType.Int64, model.HireChannelID);
            db.AddInParameter(cmd, "@IsQuick", DbType.Boolean, model.IsQuick);
            db.AddInParameter(cmd, "@ReserveField", DbType.AnsiString, model.ReserveField.TextFilter());
            db.AddInParameter(cmd, "@ReserveField1", DbType.AnsiString, model.ReserveField1.TextFilter());
            db.AddInParameter(cmd, "@ReserveField2", DbType.AnsiString, model.ReserveField2.TextFilter());
            db.AddInParameter(cmd, "@ReserveField3", DbType.AnsiString, model.ReserveField3.TextFilter());
            db.AddInParameter(cmd, "@ReserveField4", DbType.AnsiString, model.ReserveField4.TextFilter());
            db.AddInParameter(cmd, "@ReserveField5", DbType.AnsiString, model.ReserveField5.TextFilter());
            db.AddInParameter(cmd, "@IsRetractions", DbType.Boolean, model.IsRetractions);
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
                    throw new Exception("新增稿件信息失败！");
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑稿件信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        private bool UpdateContributionInfo(ContributionInfoEntity model, DbTransaction trans)
        {            
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  CID=@CID and JournalID=@JournalID and AuthorID=@AuthorID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("  CNumber=@CNumber");
            sqlCommandText.Append(", Title=@Title");
            sqlCommandText.Append(", EnTitle=@EnTitle");
            sqlCommandText.Append(", SubjectCat=@SubjectCat");
            sqlCommandText.Append(", JChannelID=@JChannelID");
            sqlCommandText.Append(", ContributionType=@ContributionType");
            sqlCommandText.Append(", Special=@Special");
            sqlCommandText.Append(", Keywords=@Keywords");
            sqlCommandText.Append(", EnKeywords=@EnKeywords");
            sqlCommandText.Append(", CLC=@CLC");
            sqlCommandText.Append(", IsFund=@IsFund");
            sqlCommandText.Append(", InvoiceTitle=@InvoiceTitle");
            sqlCommandText.Append(", CommendExpert=@CommendExpert");
            sqlCommandText.Append(", ContributePath=@ContributePath");
            sqlCommandText.Append(", FigurePath=@FigurePath");
            sqlCommandText.Append(", PDFPath=@PDFPath");
            sqlCommandText.Append(", IntroLetterPath=@IntroLetterPath");
            sqlCommandText.Append(", WordNiumber=@WordNiumber");
            sqlCommandText.Append(", FigureNumber=@FigureNumber");
            sqlCommandText.Append(", TableNumber=@TableNumber");
            sqlCommandText.Append(", ReferenceNumber=@ReferenceNumber");
            sqlCommandText.Append(", Status=@Status");
            sqlCommandText.Append(", IsPayPageFee=@IsPayPageFee");
            sqlCommandText.Append(", IsPayAuditFee=@IsPayAuditFee");
            sqlCommandText.Append(", Year=@Year");
            sqlCommandText.Append(", Volumn=@Volumn");
            sqlCommandText.Append(", Issue=@Issue");
            sqlCommandText.Append(", SortID=@SortID");
            sqlCommandText.Append(", HireChannelID=@HireChannelID");
            sqlCommandText.Append(", IsQuick=@IsQuick");
            sqlCommandText.Append(", IsRetractions=@IsRetractions");
            sqlCommandText.Append(", ReserveField=@ReserveField");
            sqlCommandText.Append(", ReserveField1=@ReserveField1");
            sqlCommandText.Append(", ReserveField2=@ReserveField2");
            sqlCommandText.Append(", ReserveField3=@ReserveField3");
            sqlCommandText.Append(", ReserveField4=@ReserveField4");
            sqlCommandText.Append(", ReserveField5=@ReserveField5");
            //sqlCommandText.Append(", AddDate=getdate()");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.ContributionInfo SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@CID", DbType.Int64, model.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, model.AuthorID);
            db.AddInParameter(cmd, "@CNumber", DbType.String, model.CNumber);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, model.Title.TextFilter());
            db.AddInParameter(cmd, "@EnTitle", DbType.AnsiString, model.EnTitle.TextFilter());
            db.AddInParameter(cmd, "@SubjectCat", DbType.Int32, model.SubjectCat);
            db.AddInParameter(cmd, "@JChannelID", DbType.Int64, model.JChannelID);
            db.AddInParameter(cmd, "@ContributionType", DbType.Int32, model.ContributionType);
            db.AddInParameter(cmd, "@Special", DbType.Int32, model.Special);
            db.AddInParameter(cmd, "@Keywords", DbType.AnsiString, model.Keywords.TextFilter());
            db.AddInParameter(cmd, "@EnKeywords", DbType.AnsiString, model.EnKeywords.TextFilter());
            db.AddInParameter(cmd, "@CLC", DbType.AnsiString, model.CLC.TextFilter());
            db.AddInParameter(cmd, "@IsFund", DbType.Boolean, model.IsFund);
            db.AddInParameter(cmd, "@InvoiceTitle", DbType.AnsiString, model.InvoiceTitle.TextFilter());
            db.AddInParameter(cmd, "@CommendExpert", DbType.AnsiString, model.CommendExpert.TextFilter());
            db.AddInParameter(cmd, "@ContributePath", DbType.AnsiString, model.ContributePath);
            db.AddInParameter(cmd, "@FigurePath", DbType.AnsiString, model.FigurePath);
            db.AddInParameter(cmd, "@PDFPath", DbType.AnsiString, model.PDFPath);
            db.AddInParameter(cmd, "@IntroLetterPath", DbType.AnsiString, model.IntroLetterPath);
            db.AddInParameter(cmd, "@WordNiumber", DbType.Int32, model.WordNiumber);
            db.AddInParameter(cmd, "@FigureNumber", DbType.Int32, model.FigureNumber);
            db.AddInParameter(cmd, "@TableNumber", DbType.Int32, model.TableNumber);
            db.AddInParameter(cmd, "@ReferenceNumber", DbType.Int32, model.ReferenceNumber);
            db.AddInParameter(cmd, "@Status", DbType.Int32, model.Status);
            db.AddInParameter(cmd, "@IsPayPageFee", DbType.Byte, model.IsPayPageFee);
            db.AddInParameter(cmd, "@IsPayAuditFee", DbType.Byte, model.IsPayAuditFee);
            db.AddInParameter(cmd, "@Year", DbType.Int32, model.Year);
            db.AddInParameter(cmd, "@Volumn", DbType.Int32, model.Volumn);
            db.AddInParameter(cmd, "@Issue", DbType.Int32, model.Issue);
            db.AddInParameter(cmd, "@SortID", DbType.Int32, model.SortID);
            db.AddInParameter(cmd, "@HireChannelID", DbType.Int64, model.HireChannelID);
            db.AddInParameter(cmd, "@IsQuick", DbType.Boolean, model.IsQuick);
            db.AddInParameter(cmd, "@IsRetractions", DbType.Boolean, model.IsRetractions);
            db.AddInParameter(cmd, "@ReserveField", DbType.AnsiString, model.ReserveField.TextFilter());
            db.AddInParameter(cmd, "@ReserveField1", DbType.AnsiString, model.ReserveField1.TextFilter());
            db.AddInParameter(cmd, "@ReserveField2", DbType.AnsiString, model.ReserveField2.TextFilter());
            db.AddInParameter(cmd, "@ReserveField3", DbType.AnsiString, model.ReserveField3.TextFilter());
            db.AddInParameter(cmd, "@ReserveField4", DbType.AnsiString, model.ReserveField4.TextFilter());
            db.AddInParameter(cmd, "@ReserveField5", DbType.AnsiString, model.ReserveField5.TextFilter());
            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("编辑稿件信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        /// <summary>
        /// 获取稿件查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetContributionInfoFilter(ContributionInfoQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID=" + query.JournalID);
            if (query.CIDs != null && query.CIDs.Count() > 0)
            {
                if (query.CIDs.Count() == 1)
                {
                    strFilter.AppendFormat(" and CID={0}", query.CIDs[0]);
                }
                else
                {
                    strFilter.AppendFormat(" and CID IN ({0})", string.Join(",",query.CIDs));
                }
            }
            if (query.AuthorID != null)
                strFilter.AppendFormat(" and AuthorID={0}", query.AuthorID.Value);
            if (query.Status != null)
                strFilter.AppendFormat(" and Status={0}", query.Status.Value);
            if (query.ContributionType != null)
                strFilter.AppendFormat(" and ContributionType={0}", query.ContributionType.Value);
            if (query.Special != null)
                strFilter.AppendFormat(" and Special={0}", query.Special.Value);
            if(query.SubjectCat!=null)
                strFilter.AppendFormat(" and SubjectCat={0}", query.SubjectCat.Value);
            if (query.Year != null && query.Year > 0)
                strFilter.AppendFormat(" and Year={0}", query.Year.Value);
            if (query.Issue != null && query.Issue > 0)
                strFilter.AppendFormat(" and Issue={0}", query.Issue.Value);
            if (query.JChannelID != null)
                strFilter.AppendFormat(" and JChannelID={0}", query.JChannelID.Value);
            query.CNumber = query.CNumber.TextFilter();
            if (!string.IsNullOrWhiteSpace(query.CNumber))
                strFilter.AppendFormat(" and CNumber like '%{0}%'", query.CNumber);
            query.Title = query.Title.TextFilter();
            if (!string.IsNullOrWhiteSpace(query.Title))
                strFilter.AppendFormat(" and Title like '%{0}%'", query.Title);
            if (query.StartDate!=null && query.EndDate!=null && (query.StartDate<query.EndDate))
                strFilter.AppendFormat(" and AddDate between '{0}' and '{1}'", query.StartDate, Convert.ToDateTime(query.EndDate).AddDays(1));
            return strFilter.ToString();
        }

        /// <summary>
        /// 组装稿件信息数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<ContributionInfoEntity> MakeContributionInfoList(IDataReader dr)
        {
            List<ContributionInfoEntity> list = new List<ContributionInfoEntity>();
            while (dr.Read())
            {
                list.Add(MakeContributionInfo(dr));
            }           
            return list;
        }

        /// <summary>
        /// 组装稿件信息数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private ContributionInfoEntity MakeContributionInfo(IDataReader dr)
        {
            ContributionInfoEntity model = new ContributionInfoEntity();
            model.CID = (Int64)dr["CID"];
            model.CNumber = (String)dr["CNumber"];
            model.AuthorID = (Int64)dr["AuthorID"];
            model.JournalID = (Int64)dr["JournalID"];
            model.Title = (String)dr["Title"];
            model.EnTitle = Convert.IsDBNull(dr["EnTitle"]) ? null : (String)dr["EnTitle"];
            model.SubjectCat = (Int32)dr["SubjectCat"];
            model.JChannelID = (Int64)dr["JChannelID"];
            model.ContributionType = (Int32)dr["ContributionType"];
            model.Special = (Int32)dr["Special"];
            model.Keywords = Convert.IsDBNull(dr["Keywords"]) ? null : (String)dr["Keywords"];
            model.EnKeywords = Convert.IsDBNull(dr["EnKeywords"]) ? null : (String)dr["EnKeywords"];
            model.CLC = Convert.IsDBNull(dr["CLC"]) ? null : (String)dr["CLC"];
            model.IsFund = Convert.IsDBNull(dr["IsFund"]) ? false : (Boolean)dr["IsFund"];
            model.InvoiceTitle = (String)dr["InvoiceTitle"];
            model.CommendExpert = Convert.IsDBNull(dr["CommendExpert"]) ? null : (String)dr["CommendExpert"];
            model.ContributePath = Convert.IsDBNull(dr["ContributePath"]) ? null : (String)dr["ContributePath"];
            model.FigurePath = Convert.IsDBNull(dr["FigurePath"]) ? null : (String)dr["FigurePath"];
            model.PDFPath = Convert.IsDBNull(dr["PDFPath"]) ? null : (String)dr["PDFPath"];
            model.IntroLetterPath = Convert.IsDBNull(dr["IntroLetterPath"]) ? null : (String)dr["IntroLetterPath"];
            model.WordNiumber = Convert.IsDBNull(dr["WordNiumber"]) ? null : (Int32?)dr["WordNiumber"];
            model.FigureNumber = Convert.IsDBNull(dr["FigureNumber"]) ? null : (Int32?)dr["FigureNumber"];
            model.TableNumber = Convert.IsDBNull(dr["TableNumber"]) ? null : (Int32?)dr["TableNumber"];
            model.ReferenceNumber = Convert.IsDBNull(dr["ReferenceNumber"]) ? null : (Int32?)dr["ReferenceNumber"];
            model.Status = (Int32)dr["Status"];
            model.IsPayPageFee = Convert.IsDBNull(dr["IsPayPageFee"]) ? null : (Byte?)dr["IsPayPageFee"];
            model.IsPayAuditFee = Convert.IsDBNull(dr["IsPayAuditFee"]) ? null : (Byte?)dr["IsPayAuditFee"];
            model.Year = Convert.IsDBNull(dr["Year"]) ? null : (Int32?)dr["Year"];
            model.Volumn = Convert.IsDBNull(dr["Volumn"]) ? null : (Int32?)dr["Volumn"];
            model.Issue = Convert.IsDBNull(dr["Issue"]) ? null : (Int32?)dr["Issue"];
            model.SortID = Convert.IsDBNull(dr["SortID"]) ? null : (Int32?)dr["SortID"];
            model.HireChannelID = Convert.IsDBNull(dr["HireChannelID"]) ? null : (Int64?)dr["HireChannelID"];
            model.IsQuick = Convert.IsDBNull(dr["IsQuick"]) ? null : (Boolean?)dr["IsQuick"];
            model.ReserveField = Convert.IsDBNull(dr["ReserveField"]) ? null : (String)dr["ReserveField"];
            model.ReserveField1 = Convert.IsDBNull(dr["ReserveField1"]) ? null : (String)dr["ReserveField1"];
            model.ReserveField2 = Convert.IsDBNull(dr["ReserveField2"]) ? null : (String)dr["ReserveField2"];
            model.ReserveField3 = Convert.IsDBNull(dr["ReserveField3"]) ? null : (String)dr["ReserveField3"];
            model.ReserveField4 = Convert.IsDBNull(dr["ReserveField4"]) ? null : (String)dr["ReserveField4"];
            model.ReserveField5 = Convert.IsDBNull(dr["ReserveField5"]) ? null : (String)dr["ReserveField5"];
            model.AddDate = (DateTime)dr["AddDate"];
            model.IsRetractions = (Boolean)dr["IsRetractions"];
            return model;
        }
        #endregion

        #region 稿件作者信息
        /// <summary>
        /// 新增稿件作者信息
        /// </summary>
        /// <param name="contributionAuthorEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool AddContributionAuthor(ContributionAuthorEntity contributionAuthorEntity, DbTransaction trans = null)
        {
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @CID");
            sqlCommandText.Append(", @JouranalID");
            sqlCommandText.Append(", @AuthorID");
            sqlCommandText.Append(", @AuthorName");
            sqlCommandText.Append(", @Gender");
            sqlCommandText.Append(", @Birthday");
            sqlCommandText.Append(", @Tel");
            sqlCommandText.Append(", @Mobile");
            sqlCommandText.Append(", @Email");
            sqlCommandText.Append(", @Nation");
            sqlCommandText.Append(", @NativePlace");
            sqlCommandText.Append(", @WorkUnit");
            sqlCommandText.Append(", @SectionOffice");
            sqlCommandText.Append(", @Address");
            sqlCommandText.Append(", @ZipCode");
            sqlCommandText.Append(", @IsFirst");
            sqlCommandText.Append(", @IsCommunication");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.ContributionAuthor ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@CID", DbType.Int64, contributionAuthorEntity.CID);
            db.AddInParameter(cmd, "@JouranalID", DbType.Int64, contributionAuthorEntity.JouranalID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, contributionAuthorEntity.AuthorID);
            db.AddInParameter(cmd, "@AuthorName", DbType.AnsiString, contributionAuthorEntity.AuthorName.TextFilter());
            db.AddInParameter(cmd, "@Gender", DbType.AnsiString, contributionAuthorEntity.Gender);
            db.AddInParameter(cmd, "@Birthday", DbType.DateTime, contributionAuthorEntity.Birthday);
            db.AddInParameter(cmd, "@Tel", DbType.AnsiString, contributionAuthorEntity.Tel.TextFilter());
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, contributionAuthorEntity.Mobile.TextFilter());
            db.AddInParameter(cmd, "@Email", DbType.AnsiString, contributionAuthorEntity.Email.TextFilter());
            db.AddInParameter(cmd, "@Nation", DbType.AnsiString, contributionAuthorEntity.Nation.TextFilter());
            db.AddInParameter(cmd, "@NativePlace", DbType.AnsiString, contributionAuthorEntity.NativePlace.TextFilter());
            db.AddInParameter(cmd, "@WorkUnit", DbType.AnsiString, contributionAuthorEntity.WorkUnit.TextFilter());
            db.AddInParameter(cmd, "@SectionOffice", DbType.AnsiString, contributionAuthorEntity.SectionOffice.TextFilter());
            db.AddInParameter(cmd, "@Address", DbType.AnsiString, contributionAuthorEntity.Address.TextFilter());
            db.AddInParameter(cmd, "@ZipCode", DbType.AnsiString, contributionAuthorEntity.ZipCode.TextFilter());
            db.AddInParameter(cmd, "@IsFirst", DbType.Boolean, contributionAuthorEntity.IsFirst);
            db.AddInParameter(cmd, "@IsCommunication", DbType.Boolean, contributionAuthorEntity.IsCommunication);
            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("新增稿件作者失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑稿件作者信息
        /// </summary>
        /// <param name="contributionAuthorEntity"></param>      
        /// <returns></returns>
        public bool UpdateContributionAuthor(ContributionAuthorEntity contributionAuthorEntity, DbTransaction trans = null)
        {
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  CAuthorID=@CAuthorID");
            StringBuilder sqlCommandText = new StringBuilder();            
            sqlCommandText.Append("  AuthorName=@AuthorName");
            sqlCommandText.Append(", Gender=@Gender");
            sqlCommandText.Append(", Birthday=@Birthday");
            sqlCommandText.Append(", Tel=@Tel");
            sqlCommandText.Append(", Mobile=@Mobile");
            sqlCommandText.Append(", Email=@Email");
            sqlCommandText.Append(", Nation=@Nation");
            sqlCommandText.Append(", NativePlace=@NativePlace");
            sqlCommandText.Append(", WorkUnit=@WorkUnit");
            sqlCommandText.Append(", SectionOffice=@SectionOffice");
            sqlCommandText.Append(", Address=@Address");
            sqlCommandText.Append(", ZipCode=@ZipCode");
            sqlCommandText.Append(", IsFirst=@IsFirst");
            sqlCommandText.Append(", IsCommunication=@IsCommunication");           

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.ContributionAuthor SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@CAuthorID", DbType.Int64, contributionAuthorEntity.CAuthorID);            
            db.AddInParameter(cmd, "@AuthorName", DbType.AnsiString, contributionAuthorEntity.AuthorName.TextFilter());
            db.AddInParameter(cmd, "@Gender", DbType.AnsiString, contributionAuthorEntity.Gender);
            db.AddInParameter(cmd, "@Birthday", DbType.DateTime, contributionAuthorEntity.Birthday);
            db.AddInParameter(cmd, "@Tel", DbType.AnsiString, contributionAuthorEntity.Tel.TextFilter());
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, contributionAuthorEntity.Mobile.TextFilter());
            db.AddInParameter(cmd, "@Email", DbType.AnsiString, contributionAuthorEntity.Email.TextFilter());
            db.AddInParameter(cmd, "@Nation", DbType.AnsiString, contributionAuthorEntity.Nation.TextFilter());
            db.AddInParameter(cmd, "@NativePlace", DbType.AnsiString, contributionAuthorEntity.NativePlace.TextFilter());
            db.AddInParameter(cmd, "@WorkUnit", DbType.AnsiString, contributionAuthorEntity.WorkUnit.TextFilter());
            db.AddInParameter(cmd, "@SectionOffice", DbType.AnsiString, contributionAuthorEntity.SectionOffice.TextFilter());
            db.AddInParameter(cmd, "@Address", DbType.AnsiString, contributionAuthorEntity.Address.TextFilter());
            db.AddInParameter(cmd, "@ZipCode", DbType.AnsiString, contributionAuthorEntity.ZipCode.TextFilter());
            db.AddInParameter(cmd, "@IsFirst", DbType.Boolean, contributionAuthorEntity.IsFirst);
            db.AddInParameter(cmd, "@IsCommunication", DbType.Boolean, contributionAuthorEntity.IsCommunication);
           
            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("编辑稿件作者失败！");
                return result;           
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        /// <summary>
        /// 获取稿件作者实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionAuthorEntity GetContributionAuthor(Int64 CAuthorID)
        {
            ContributionAuthorEntity model = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(@"SELECT TOP 1  
                                 [CAuthorID]
	                            ,[CID]
	                            ,a.[JouranalID]
	                            ,a.[AuthorID]
	                            ,a.[AuthorName]
	                            ,(case when a.[Gender]=1 then '男' when a.[Gender]=2 then '女' end) as Gender
	                            ,a.[Birthday]
	                            ,a.Tel
                                ,a.Mobile
	                            ,a.[Email]
	                            ,a.[Nation]
                                ,a.[NativePlace]
	                            ,a.[City]
	                            ,a.[WorkUnit]
	                            ,a.[SectionOffice]
	                            ,a.[Address]
	                            ,a.[ZipCode]
	                            ,[IsFirst],[IsCommunication]
	                            ,a.[AddDate] 
	                            FROM dbo.ContributionAuthor a WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  CAuthorID=@CAuthorID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@CAuthorID", DbType.Int64, CAuthorID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    model = MakeContributionAuthorInfo(dr);
                }
                dr.Close();
            }
            return model;
        }

        /// <summary>
        /// 根据稿件编号删除稿件作者信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelContributionAuthorByCID(Int64[] CID, DbTransaction trans = null, Int64[] CAuthorID=null)
        {
            if (CID == null || CID.Length < 1)
                return false;
            string strSql = "Delete dbo.ContributionAuthor WHERE CID";
            if (CID.Length == 0)
                strSql += " = " + CID[0];
            else
                strSql += string.Format(" in ({0}) ", string.Join(",", CID));
            if (CAuthorID != null && CAuthorID.Length > 0) 
            {
                strSql += string.Format(" and CAuthorID not in ({0})", string.Join(",", CAuthorID));
            }
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
        /// 删除稿件作者信息
        /// </summary>
        /// <param name="CAuthorID"></param>
        /// <returns></returns>
        public bool DelContributionAuthor(Int64[] CAuthorID)
        {
            if (CAuthorID == null || CAuthorID.Length < 1)
                return false;
            string strSql = "Delete dbo.ContributionAuthor WHERE CAuthorID";
            if (CAuthorID.Length == 0)
                strSql += " = " + CAuthorID[0];
            else
                strSql += string.Format(" in ({0}) ", string.Join(",", CAuthorID));
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(cmd) > 0;
        }

        /// <summary>
        /// 获取稿件作者分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionAuthorEntity> GetContributionAuthorPageList(ContributionAuthorQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderStr))
                query.OrderStr = " CAuthorID DESC";
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY " + query.OrderStr + ") AS ROW_ID FROM dbo.ContributionAuthor with(nolock)"
                , sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.ContributionAuthor with(nolock)";
            string whereSQL = GetContributionAuthorFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<ContributionAuthorEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeContributionAuthorList);
        }

        /// <summary>
        /// 获取稿件作者数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionAuthorEntity> GetContributionAuthorList(ContributionAuthorQuery query)
        {
            string strSql = string.Empty;
            if (string.IsNullOrWhiteSpace(query.OrderStr))
                query.OrderStr = " IsFirst DESC";
            if (query.isModify == true)//如果isModify=true(由\WKT.Service\Author\ContributionInfoService.cs中的GetContributionInfo方法传递) 则仅显示稿件作者数据中的联系电话
            {
                strSql = @"SELECT  [CAuthorID]
	                            ,[CID]
	                            ,a.[JouranalID]
	                            ,a.[AuthorID]
	                            ,a.[AuthorName]
	                            ,(case when a.[Gender]=1 then '男' when a.[Gender]=2 then '女' end) as Gender
	                            ,a.[Birthday]
	                            ,a.Tel
                                ,a.Mobile
	                            ,a.[Email]
	                            ,a.[Nation]
                                ,a.[NativePlace] 
	                            ,b.[Province]
	                            ,b.[City]
	                            ,b.[Area]
	                            ,a.[WorkUnit]
	                            ,a.[SectionOffice]
	                            ,a.[Address]
	                            ,a.[ZipCode]
	                            ,[IsFirst],[IsCommunication]
	                            ,a.[AddDate] 
	                            FROM dbo.ContributionAuthor a LEFT JOIN AuthorDetail b on a.AuthorID=b.AuthorID";
            }
            else
            {
                strSql = @"SELECT  [CAuthorID]
	                            ,[CID]
	                            ,a.[JouranalID]
	                            ,a.[AuthorID]
	                            ,a.[AuthorName]
	                            ,(case when a.[Gender]=1 then '男' when a.[Gender]=2 then '女' end) as Gender
	                            ,a.[Birthday]
	                            --,case when a.Tel is null then b.Mobile else a.Tel + N' '+ b.Mobile end  as Tel 
                                ,a.Tel 
	                            ,a.Mobile
                                ,a.[Email]
	                            ,a.[Nation]
                                ,a.[NativePlace] 
	                            ,b.[Province]
	                            ,b.[City]
	                            ,b.[Area]
	                            ,a.[WorkUnit]
	                            ,a.[SectionOffice]
	                            ,a.[Address]
	                            ,a.[ZipCode]
	                            ,[IsFirst],[IsCommunication]
	                            ,a.[AddDate] 
	                            FROM dbo.ContributionAuthor a LEFT JOIN AuthorDetail b on a.AuthorID=b.AuthorID";
            }
            
            string whereSQL = GetContributionAuthorFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " ORDER BY " + query.OrderStr;
            return db.GetList<ContributionAuthorEntity>(strSql, MakeContributionAuthorList);
        }

        /// <summary>
        /// 获取稿件作者查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetContributionAuthorFilter(ContributionAuthorQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JouranalID=" + query.JournalID);
            if (query.CID != null)
            {
                strFilter.AppendFormat(" and CID={0}", query.CID.Value);
            }
            if (query.CAuthorID>0)
            {
                strFilter.AppendFormat(" and CAuthorID={0}", query.CAuthorID);
            }
            return strFilter.ToString();
        }

        /// <summary>
        /// 组装稿件作者信息数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<ContributionAuthorEntity> MakeContributionAuthorList(IDataReader dr)
        {
            List<ContributionAuthorEntity> list = new List<ContributionAuthorEntity>();
            while (dr.Read())
            {
                list.Add(MakeContributionAuthorInfo(dr));
            }
            return list;
        }

        /// <summary>
        /// 组装稿件作者信息数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private ContributionAuthorEntity MakeContributionAuthorInfo(IDataReader dr)
        {
            ContributionAuthorEntity model = new ContributionAuthorEntity();
            model.CAuthorID = dr.GetDrValue<Int64>("CAuthorID");
            model.CID = dr.GetDrValue<Int64>("CID");
            model.JouranalID = dr.GetDrValue<Int64>("JouranalID");
            model.AuthorID = dr.GetDrValue<Int64>("AuthorID");
            model.AuthorName = dr.GetDrValue<String>("AuthorName");
            model.Gender = dr.GetDrValue<String>("Gender");
            model.Birthday = dr.GetDrValue<DateTime?>("Birthday");
            model.Tel = dr.GetDrValue<String>("Tel");
            model.Mobile = dr.GetDrValue<String>("Mobile");
            model.Email = dr.GetDrValue<String>("Email");
            model.Nation = dr.GetDrValue<String>("Nation");
            model.NativePlace = dr.GetDrValue<String>("NativePlace");
            model.WorkUnit = dr.GetDrValue<String>("WorkUnit");
            model.SectionOffice = dr.GetDrValue<String>("SectionOffice");
            model.Address = dr.GetDrValue<String>("Address");
            model.ZipCode = dr.GetDrValue<String>("ZipCode");
            model.IsFirst = dr.GetDrValue<bool>("IsFirst");
            model.IsCommunication = dr.GetDrValue<bool>("IsCommunication");
            model.AddDate = dr.GetDrValue<DateTime>("AddDate");
            return model;
        }
        #endregion

        #region 参考文献信息
        /// <summary>
        /// 新增参考文献
        /// </summary>
        /// <param name="contributionReferenceEntity"></param>
        /// <returns></returns>
        public bool AddContributionReference(ContributionReferenceEntity contributionReferenceEntity,DbTransaction trans=null)
        {
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" @CID");
            sqlCommandText.Append(", @JournalID");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @RefUrl");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.ContributionReference ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@CID", DbType.Int64, contributionReferenceEntity.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, contributionReferenceEntity.JournalID);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, contributionReferenceEntity.Title.TextFilter());
            db.AddInParameter(cmd, "@RefUrl", DbType.AnsiString, contributionReferenceEntity.RefUrl.TextFilter());

            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("新增稿件参考文献失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑参考文献
        /// </summary>
        /// <param name="contributionReferenceEntity"></param>
        /// <returns></returns>
        public bool UpdateContributionReference(ContributionReferenceEntity contributionReferenceEntity, DbTransaction trans = null)
        {           
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  ReferenceID=@ReferenceID");
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" Title=@Title");
            sqlCommandText.Append(", RefUrl=@RefUrl");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.ContributionReference SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@ReferenceID", DbType.Int64, contributionReferenceEntity.ReferenceID);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, contributionReferenceEntity.Title.TextFilter());
            db.AddInParameter(cmd, "@RefUrl", DbType.AnsiString, contributionReferenceEntity.RefUrl.TextFilter());

            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("编辑稿件参考文献失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取参考文献实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionReferenceEntity GetContributionReference(Int64 ReferenceID)
        {
            ContributionReferenceEntity model = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 * FROM dbo.ContributionReference WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  ReferenceID=@ReferenceID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@ReferenceID", DbType.Int64, ReferenceID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    model = MakeContributionReferenceInfo(dr);
                }
                dr.Close();
            }
            return model;
        }

        /// <summary>
        /// 根据稿件编号删除参考文献
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelContributionReferenceByCID(Int64[] CID, DbTransaction trans = null, Int64[] ReferenceID=null)
        {
            if (CID == null || CID.Length < 1)
                return false;
            string strSql = "Delete dbo.ContributionReference WHERE CID";
            if (CID.Length == 0)
                strSql += " = " + CID[0];
            else
                strSql += string.Format(" in ({0}) ", string.Join(",", CID));
            if (ReferenceID != null && ReferenceID.Length > 0)
            {
                strSql += string.Format(" and ReferenceID not in ({0})", string.Join(",", ReferenceID));
            }
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
        /// 删除参考文献
        /// </summary>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        public bool DelContributionReference(Int64[] ReferenceID)
        {
            if (ReferenceID == null || ReferenceID.Length < 1)
                return false;
            string strSql = "Delete dbo.ContributionReference WHERE ReferenceID";
            if (ReferenceID.Length == 0)
                strSql += " = " + ReferenceID[0];
            else
                strSql += string.Format(" in ({0}) ", string.Join(",", ReferenceID));
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(cmd) > 0;
        }

        /// <summary>
        /// 获取参考文献分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionReferenceEntity> GetContributionReferencePageList(ContributionReferenceQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderStr))
                query.OrderStr = " ReferenceID DESC";
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY " + query.OrderStr + ") AS ROW_ID FROM dbo.ContributionReference with(nolock)"
                , sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.ContributionReference with(nolock)";
            string whereSQL = GetContributionReferenceFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<ContributionReferenceEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeContributionReferenceList);
        }

        /// <summary>
        /// 获取参考文献数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionReferenceEntity> GetContributionReferenceList(ContributionReferenceQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderStr))
                query.OrderStr = " ReferenceID DESC";
            string strSql = "SELECT * FROM dbo.ContributionReference with(nolock)";
            string whereSQL = GetContributionReferenceFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " ORDER BY " + query.OrderStr;
            return db.GetList<ContributionReferenceEntity>(strSql, MakeContributionReferenceList);
        }

        /// <summary>
        /// 获取参考文献查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetContributionReferenceFilter(ContributionReferenceQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID=" + query.JournalID);
            if (query.CID != null)
            {
                strFilter.AppendFormat(" and CID={0}", query.CID.Value);
            }
            return strFilter.ToString();
        }

        /// <summary>
        /// 组装参考文献数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<ContributionReferenceEntity> MakeContributionReferenceList(IDataReader dr)
        {
            List<ContributionReferenceEntity> list = new List<ContributionReferenceEntity>();
            while (dr.Read())
            {
                list.Add(MakeContributionReferenceInfo(dr));
            }
            return list;
        }

        /// <summary>
        /// 组装参考文献数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private ContributionReferenceEntity MakeContributionReferenceInfo(IDataReader dr)
        {
            ContributionReferenceEntity model = new ContributionReferenceEntity();
            model.ReferenceID = dr.GetDrValue<Int64>("ReferenceID");
            model.CID = dr.GetDrValue<Int64>("CID");
            model.JournalID = dr.GetDrValue<Int64>("JournalID");
            model.Title = dr.GetDrValue<String>("Title");
            model.RefUrl = dr.GetDrValue<String>("RefUrl");
            model.AddDate = dr.GetDrValue<DateTime>("AddDate");
            return model;
        }
        #endregion

        #region 基金信息
        /// <summary>
        /// 新增基金信息
        /// </summary>
        /// <param name="contributionFundEntity"></param>
        /// <returns></returns>
        public bool AddContributionFund(ContributionFundEntity contributionFundEntity,DbTransaction trans=null)
        {
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" @CID");
            sqlCommandText.Append(", @JournalID");
            sqlCommandText.Append(", @FundLevel");
            sqlCommandText.Append(", @FundCode");
            sqlCommandText.Append(", @FundName");
            sqlCommandText.Append(", @FundCertPath");
            sqlCommandText.Append(", @Note");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.ContributionFund ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@CID", DbType.Int64, contributionFundEntity.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, contributionFundEntity.JournalID);
            db.AddInParameter(cmd, "@FundLevel", DbType.Int32, contributionFundEntity.FundLevel);
            db.AddInParameter(cmd, "@FundCode", DbType.AnsiString, contributionFundEntity.FundCode.TextFilter());
            db.AddInParameter(cmd, "@FundName", DbType.AnsiString, contributionFundEntity.FundName.TextFilter());
            db.AddInParameter(cmd, "@FundCertPath", DbType.AnsiString, contributionFundEntity.FundCertPath);
            db.AddInParameter(cmd, "@Note", DbType.AnsiString, contributionFundEntity.Note.TextFilter());

            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("新增稿件基金信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑基金信息
        /// </summary>
        /// <param name="contributionFundEntity"></param>
        /// <returns></returns>
        public bool UpdateContributionFund(ContributionFundEntity contributionFundEntity, DbTransaction trans = null)
        {
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  FundID=@FundID");
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" FundLevel=@FundLevel");
            sqlCommandText.Append(", FundCode=@FundCode");
            sqlCommandText.Append(", FundName=@FundName");
            sqlCommandText.Append(", FundCertPath=@FundCertPath");
            sqlCommandText.Append(", Note=@Note");            

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.ContributionFund SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@FundID", DbType.Int64, contributionFundEntity.FundID);
            db.AddInParameter(cmd, "@FundLevel", DbType.Int32, contributionFundEntity.FundLevel);
            db.AddInParameter(cmd, "@FundCode", DbType.AnsiString, contributionFundEntity.FundCode.TextFilter());
            db.AddInParameter(cmd, "@FundName", DbType.AnsiString, contributionFundEntity.FundName.TextFilter());
            db.AddInParameter(cmd, "@FundCertPath", DbType.AnsiString, contributionFundEntity.FundCertPath);
            db.AddInParameter(cmd, "@Note", DbType.AnsiString, contributionFundEntity.Note.TextFilter());

            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("编辑稿件基金信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取基金信息实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionFundEntity GetContributionFund(Int64 FundID)
        {
            ContributionFundEntity model = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 * FROM dbo.ContributionFund WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  FundID=@FundID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@FundID", DbType.Int64, FundID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    model = MakeContributionFundInfo(dr);
                }
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
            return model;
        }

        /// <summary>
        /// 根据稿件编号删除基金信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelContributionFundByCID(Int64[] CID, DbTransaction trans = null, Int64[] FundID=null)
        {
            if (CID == null || CID.Length < 1)
                return false;
            string strSql = "Delete dbo.ContributionFund WHERE CID";
            if (CID.Length == 0)
                strSql += " = " + CID[0];
            else
                strSql += string.Format(" in ({0}) ", string.Join(",", CID));
            if (FundID != null && FundID.Length > 0)
            {
                strSql += string.Format(" and FundID not in ({0})", string.Join(",", FundID));
            }
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
        /// 删除基金信息
        /// </summary>
        /// <param name="FundID"></param>
        /// <returns></returns>
        public bool DelContributionFund(Int64[] FundID)
        {
            if (FundID == null || FundID.Length < 1)
                return false;
            string strSql = "Delete dbo.ContributionFund WHERE FundID";
            if (FundID.Length == 0)
                strSql += " = " + FundID[0];
            else
                strSql += string.Format(" in ({0}) ", string.Join(",", FundID));
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(cmd) > 0;
        }

        /// <summary>
        /// 获取基金信息分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionFundEntity> GetContributionFundPageList(ContributionFundQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderStr))
                query.OrderStr = " ReferenceID DESC";
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY " + query.OrderStr + ") AS ROW_ID FROM dbo.ContributionFund with(nolock)"
                , sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.ContributionFund with(nolock)";
            string whereSQL = GetContributionFundFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<ContributionFundEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeContributionFundList);
        }

        /// <summary>
        /// 获取基金信息数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionFundEntity> GetContributionFundList(ContributionFundQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderStr))
                query.OrderStr = " FundID DESC";
            string strSql = "SELECT * FROM dbo.ContributionFund with(nolock)";
            string whereSQL = GetContributionFundFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " ORDER BY " + query.OrderStr;
            return db.GetList<ContributionFundEntity>(strSql, MakeContributionFundList);
        }

        /// <summary>
        /// 获取基金信息查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetContributionFundFilter(ContributionFundQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID=" + query.JournalID);
            if (query.CID != null)
            {
                strFilter.AppendFormat(" and CID={0}", query.CID.Value);
            }
            return strFilter.ToString();
        }

        /// <summary>
        /// 组装基金信息数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<ContributionFundEntity> MakeContributionFundList(IDataReader dr)
        {
            List<ContributionFundEntity> list = new List<ContributionFundEntity>();
            while (dr.Read())
            {
                list.Add(MakeContributionFundInfo(dr));
            }
            return list;
        }

        /// <summary>
        /// 组装基金信息数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private ContributionFundEntity MakeContributionFundInfo(IDataReader dr)
        {
            ContributionFundEntity model = new ContributionFundEntity();
            model.FundID = dr.GetDrValue<Int64>("FundID");
            model.CID = dr.GetDrValue<Int64>("CID");
            model.JournalID = dr.GetDrValue<Int64>("JournalID");
            model.FundLevel = dr.GetDrValue<Int32>("FundLevel");
            model.FundCode = dr.GetDrValue<String>("FundCode");
            model.FundName = dr.GetDrValue<String>("FundName");
            model.FundCertPath = dr.GetDrValue<String>("FundCertPath");
            model.Note = dr.GetDrValue<String>("Note");
            model.AddDate = dr.GetDrValue<DateTime>("AddDate");
            return model;
        }
        #endregion

        #region 大字段信息
        /// <summary>
        /// 获取大字段信息实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionInfoAttEntity GetContributionInfoAtt(Int64 PKID)
        {
            ContributionInfoAttEntity model = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 * FROM dbo.ContributionInfoAtt WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  PKID=@PKID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@PKID", DbType.Int64, PKID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    model = MakeContributionInfoAtt(dr);
                }
            }
            return model;
        }

        /// <summary>
        /// 获取大字段信息实体
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ContributionInfoAttEntity GetContributionInfoAttByCID(Int64 CID)
        {
            ContributionInfoAttEntity model = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 * FROM dbo.ContributionInfoAtt WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  CID=@CID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@CID", DbType.Int64, CID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    model = MakeContributionInfoAtt(dr);
                }
                dr.Close();
            }
            return model;
        }
        
        /// <summary>
        /// 根据稿件编号删除大字段信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelContributionInfoAttByCID(Int64[] CID, DbTransaction trans = null)
        {
            if (CID == null || CID.Length < 1)
                return false;
            string strSql = "Delete dbo.ContributionInfoAtt WHERE CID";
            if (CID.Length == 0)
                strSql += " = " + CID[0];
            else
                strSql += string.Format(" in ({0}) ", string.Join(",", CID));
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("删除稿件大字段信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除大字段信息
        /// </summary>
        /// <param name="FundID"></param>
        /// <returns></returns>
        public bool DelContributionInfoAtt(Int64[] PKID)
        {
            if (PKID == null || PKID.Length < 1)
                return false;
            string strSql = "Delete dbo.ContributionInfoAtt WHERE PKID";
            if (PKID.Length == 0)
                strSql += " = " + PKID[0];
            else
                strSql += string.Format(" in ({0}) ", string.Join(",", PKID));
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(cmd) > 0;
        }

        /// <summary>
        /// 新增大字段信息
        /// </summary>
        /// <param name="contributionInfoAttEntity"></param>
        /// <returns></returns>
        private bool AddContributionInfoAtt(ContributionInfoAttEntity contributionInfoAttEntity, DbTransaction trans)
        {
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @CID");
            sqlCommandText.Append(", @JournalID");
            sqlCommandText.Append(", @Abstract");
            sqlCommandText.Append(", @EnAbstract");
            sqlCommandText.Append(", @Reference");
            sqlCommandText.Append(", @Funds");
            sqlCommandText.Append(", @Remark");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.ContributionInfoAtt ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@CID", DbType.Int64, contributionInfoAttEntity.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, contributionInfoAttEntity.JournalID);
            db.AddInParameter(cmd, "@Abstract", DbType.AnsiString, contributionInfoAttEntity.Abstract.TextFilter());
            db.AddInParameter(cmd, "@EnAbstract", DbType.AnsiString, contributionInfoAttEntity.EnAbstract.TextFilter());
            db.AddInParameter(cmd, "@Reference", DbType.AnsiString, contributionInfoAttEntity.Reference.TextFilter());
            db.AddInParameter(cmd, "@Funds", DbType.AnsiString, contributionInfoAttEntity.Funds.TextFilter());
            db.AddInParameter(cmd, "@Remark", DbType.AnsiString, contributionInfoAttEntity.Remark.TextFilter());

            try
            {
                bool result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("新增大字段信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑大字段信息
        /// </summary>
        /// <param name="contributionInfoAttEntity"></param>
        /// <returns></returns>
        private bool UpdateContributionInfoAtt(ContributionInfoAttEntity contributionInfoAttEntity, DbTransaction trans)
        {
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  CID=@CID");
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" Abstract=@Abstract");
            sqlCommandText.Append(", EnAbstract=@EnAbstract");
            sqlCommandText.Append(", Reference=@Reference");
            sqlCommandText.Append(", Funds=@Funds");
            sqlCommandText.Append(", Remark=@Remark");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.ContributionInfoAtt SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@CID", DbType.Int64, contributionInfoAttEntity.CID);
            db.AddInParameter(cmd, "@Abstract", DbType.AnsiString, contributionInfoAttEntity.Abstract.TextFilter());
            db.AddInParameter(cmd, "@EnAbstract", DbType.AnsiString, contributionInfoAttEntity.EnAbstract.TextFilter());
            db.AddInParameter(cmd, "@Reference", DbType.AnsiString, contributionInfoAttEntity.Reference.TextFilter());
            db.AddInParameter(cmd, "@Funds", DbType.AnsiString, contributionInfoAttEntity.Funds.TextFilter());
            db.AddInParameter(cmd, "@Remark", DbType.AnsiString, contributionInfoAttEntity.Remark.TextFilter());

            try
            {
                bool result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("编辑大字段信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 组装大字段信息数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private ContributionInfoAttEntity MakeContributionInfoAtt(IDataReader dr)
        {
            ContributionInfoAttEntity model = new ContributionInfoAttEntity();
            model.PKID = dr.GetDrValue<Int64>("PKID");
            //model.PKID = Convert.ToInt64(dr["PKID"].ToString());
            model.CID = dr.GetDrValue<Int64>("CID");
            model.JournalID = dr.GetDrValue<Int64>("JournalID");
            model.Abstract = dr.GetDrValue<String>("Abstract");
            model.EnAbstract = dr.GetDrValue<String>("EnAbstract");
            model.Reference = dr.GetDrValue<String>("Reference");
            model.Funds = dr.GetDrValue<String>("Funds");
            model.Remark = dr.GetDrValue<String>("Remark");
            model.AddDate = dr.GetDrValue<DateTime>("AddDate");
            return model;
        }
        #endregion

        #region 撤稿相关
        /// <summary>
        /// 撤稿
        /// </summary>
        /// <param name="retractionsBillsEntity"></param>
        /// <returns></returns>
        public bool DraftContribution(RetractionsBillsEntity model)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    string strSql = "Update dbo.ContributionInfo set IsRetractions=1 WHERE CID=" + model.CID;                   
                    DbCommand cmd = db.GetSqlStringCommand(strSql);
                    if (db.ExecuteNonQuery(cmd, trans) < 1)
                        throw new Exception("修改稿件表撤稿状态失败！");

                    AddRetractionsBills(model, trans);

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
        /// 添加撤稿信息
        /// </summary>
        /// <param name="retractionsBillsEntity"></param>
        /// <returns></returns>
        public bool AddRetractionsBills(RetractionsBillsEntity retractionsBillsEntity,DbTransaction trans=null)
        {
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @CID");
            sqlCommandText.Append(", @AuthorID");
            sqlCommandText.Append(", @Reason");            
            sqlCommandText.Append(", @Handler");
            sqlCommandText.Append(", @HandAdvice");           
            sqlCommandText.Append(", @Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.RetractionsBills ({0}) VALUES ({1})", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));
            
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, retractionsBillsEntity.JournalID);
            db.AddInParameter(cmd, "@CID", DbType.Int64, retractionsBillsEntity.CID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, retractionsBillsEntity.AuthorID);
            db.AddInParameter(cmd, "@Reason", DbType.AnsiString, TextHelper.SubStr(retractionsBillsEntity.Reason.TextFilter(),1000));           
            db.AddInParameter(cmd, "@Handler", DbType.Int64, retractionsBillsEntity.Handler);
            db.AddInParameter(cmd, "@HandAdvice", DbType.AnsiString, TextHelper.SubStr(retractionsBillsEntity.HandAdvice.TextFilter(),1000));          
            db.AddInParameter(cmd, "@Status", DbType.Byte, retractionsBillsEntity.Status);
            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("新增撤稿信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑撤稿信息
        /// </summary>
        /// <param name="retractionsBillsEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool UpdateRetractionsBills(RetractionsBillsEntity retractionsBillsEntity, DbTransaction trans = null)
        {
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  PKID=@PKID");
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" Reason=@Reason");           
            sqlCommandText.Append(", Handler=@Handler");
            sqlCommandText.Append(", HandAdvice=@HandAdvice");
            sqlCommandText.Append(", HandDate=@HandDate");
            sqlCommandText.Append(", Status=@Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.RetractionsBills SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@PKID", DbType.Int64, retractionsBillsEntity.PKID);
            db.AddInParameter(cmd, "@Reason", DbType.AnsiString, TextHelper.SubStr(retractionsBillsEntity.Reason.TextFilter(),1000));           
            db.AddInParameter(cmd, "@Handler", DbType.Int64, retractionsBillsEntity.Handler);
            db.AddInParameter(cmd, "@HandAdvice", DbType.AnsiString, TextHelper.SubStr(retractionsBillsEntity.HandAdvice.TextFilter(),1000));
            db.AddInParameter(cmd, "@HandDate", DbType.DateTime, retractionsBillsEntity.HandDate);
            db.AddInParameter(cmd, "@Status", DbType.Byte, retractionsBillsEntity.Status);

            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("编辑撤稿信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取撤稿信息
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns></returns>
        public RetractionsBillsEntity GetRetractionsBills(RetractionsBillsQuery rQuery)
        {
            RetractionsBillsEntity retractionsBillsEntity = null;
            string strSql = "SELECT TOP 1 * FROM dbo.RetractionsBills WITH(NOLOCK) WHERE CID=@CID AND JournalID=@JournalID";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@CID", DbType.Int64, rQuery.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, rQuery.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                    retractionsBillsEntity = MakeRetractionsBills(dr);
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
            return retractionsBillsEntity;
        }

        /// <summary>
        /// 组装撤稿数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<RetractionsBillsEntity> MakeRetractionsBillsList(IDataReader dr)
        {
            List<RetractionsBillsEntity> list = new List<RetractionsBillsEntity>();
            while (dr.Read())
            {
                list.Add(MakeRetractionsBills(dr));
            }           
            return list;
        }

        /// <summary>
        /// 组装撤稿数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private RetractionsBillsEntity MakeRetractionsBills(IDataReader dr)
        {
            RetractionsBillsEntity retractionsBillsEntity = new RetractionsBillsEntity();
            retractionsBillsEntity.PKID = (Int64)dr["PKID"];
            retractionsBillsEntity.JournalID = (Int64)dr["JournalID"];
            retractionsBillsEntity.CID = (Int64)dr["CID"];
            retractionsBillsEntity.AuthorID = (Int64)dr["AuthorID"];
            retractionsBillsEntity.Reason = (String)dr["Reason"];
            retractionsBillsEntity.ApplyDate = (DateTime)dr["ApplyDate"];
            retractionsBillsEntity.Handler = (Int64)dr["Handler"];
            retractionsBillsEntity.HandAdvice = (String)dr["HandAdvice"];
            retractionsBillsEntity.HandDate = Convert.IsDBNull(dr["HandDate"]) ? null : (DateTime?)dr["HandDate"];
            retractionsBillsEntity.Status = (Byte)dr["Status"];
            return retractionsBillsEntity;
        }
        #endregion

        # region 更新稿件标记

        /// <summary>
        /// 批量更新稿件旗帜标记
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool BatchUpdateContributionFlag(List<ContributionInfoQuery> cEntityList)
        {
            bool flag = false;
            if (cEntityList.Count > 1)
            {
                try
                {
                    // 先删除
                    using (IDbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        // 启动事务
                        using (IDbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                # region 1.创建临时表

                                string createTempTable = "CREATE TABLE #ContributionTemp(CID BIGINT,JournalID BIGINT,Flag VARCHAR(20))";
                                DbCommand cmdCreateTempTable = db.GetSqlStringCommand(createTempTable);
                                db.ExecuteNonQuery(cmdCreateTempTable, (DbTransaction)transaction);

                                # endregion

                                # region 2.往临时表中写入数据

                                // 开始批量插入
                                DataTable dtContribution = ContributionFlagDT();
                                foreach (ContributionInfoQuery item in cEntityList)
                                {
                                    if (string.IsNullOrEmpty(item.Flag))
                                    {
                                        item.Flag = "";
                                    }
                                    DataRow row = dtContribution.NewRow();
                                    row["CID"] = item.CID;
                                    row["JournalID"] = item.JournalID;
                                    row["Flag"] = item.Flag;
                                    dtContribution.Rows.Add(row);
                                }

                                SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction);
                                bulkCopy.DestinationTableName = "#ContributionTemp";
                                bulkCopy.ColumnMappings.Add("CID", "CID");
                                bulkCopy.ColumnMappings.Add("JournalID", "JournalID");
                                bulkCopy.ColumnMappings.Add("Flag", "Flag");
                                bulkCopy.WriteToServer(dtContribution);

                                # endregion

                                # region 3.更新旗帜标记，并删除临时表

                                string sql = @" UPDATE c SET c.Flag=t.Flag FROM dbo.ContributionInfo c,#ContributionTemp t WHERE c.CID=t.CID AND c.JournalID=t.JournalID
                                            DROP TABLE #ContributionTemp;";
                                DbCommand cmdSet = db.GetSqlStringCommand(sql);
                                db.ExecuteNonQuery(cmdSet, (DbTransaction)transaction);

                                # endregion

                                transaction.Commit();
                            }
                            catch (Exception ext)
                            {
                                transaction.Rollback();
                                throw ext;
                            }
                        }

                        connection.Close();
                    }
                    flag = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                flag = UpdateContributionFlag(cEntityList[0]);
            }
            return flag;
        }

        /// <summary>
        /// 更新稿件旗帜标记
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool UpdateContributionFlag(ContributionInfoQuery cEntity)
        {
            bool flag = false;
            string sql = "UPDATE dbo.ContributionInfo SET Flag=@Flag WHERE CID=@CID AND JournalID=@JournalID";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            if (string.IsNullOrEmpty(cEntity.Flag))
            {
                cEntity.Flag = "";
            }
            db.AddInParameter(cmd, "@CID", DbType.Int64, cEntity.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, cEntity.JournalID);
            db.AddInParameter(cmd, "@Flag", DbType.AnsiString, cEntity.Flag);

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

        # endregion

        # region 处理撤稿申请

        /// <summary>
        /// 处理撤稿申请
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool DealWithdrawal(ContributionInfoQuery cEntity)
        {
            bool flag = false;
            DbCommand cmd = db.GetStoredProcCommand("UP_WithdrawalContribution");
            db.AddInParameter(cmd, "@CID", DbType.Int64, cEntity.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, cEntity.JournalID);

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
        /// 撤销删除
        /// </summary>
        /// <param name="cEntity"></param>
        /// <returns></returns>
        public bool CancelDelete(ContributionInfoQuery cEntity)
        {
            bool flag = false;
            DbCommand cmd = db.GetStoredProcCommand("UP_CancelDelete");
            db.AddInParameter(cmd, "@CID", DbType.Int64, cEntity.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, cEntity.JournalID);

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

        # endregion

        # region 更新稿件加急标记

        /// <summary>
        /// 批量更新稿件加急标记
        /// </summary>        
        /// <param name="cEntityList">稿件信息</param>
        /// <returns></returns>
        public bool BatchUpdateContributionIsQuick(List<ContributionInfoQuery> cEntityList)
        {
            bool flag = false;
            if (cEntityList.Count > 1)
            {
                try
                {
                    // 先删除
                    using (IDbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        // 启动事务
                        using (IDbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                # region 1.创建临时表

                                string createTempTable = "CREATE TABLE #ContributionTemp(CID BIGINT,JournalID BIGINT,IsQuick BIT)";
                                DbCommand cmdCreateTempTable = db.GetSqlStringCommand(createTempTable);
                                db.ExecuteNonQuery(cmdCreateTempTable, (DbTransaction)transaction);

                                # endregion

                                # region 2.往临时表中写入数据

                                // 开始批量插入
                                DataTable dtContribution = ContributionIsQuickDT();
                                foreach (ContributionInfoQuery item in cEntityList)
                                {
                                    DataRow row = dtContribution.NewRow();
                                    row["CID"] = item.CID;
                                    row["JournalID"] = item.JournalID;
                                    row["IsQuick"] = item.IsQuick;
                                    dtContribution.Rows.Add(row);
                                }

                                SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction);
                                bulkCopy.DestinationTableName = "#ContributionTemp";
                                bulkCopy.ColumnMappings.Add("CID", "CID");
                                bulkCopy.ColumnMappings.Add("JournalID", "JournalID");
                                bulkCopy.ColumnMappings.Add("IsQuick", "IsQuick");
                                bulkCopy.WriteToServer(dtContribution);

                                # endregion

                                # region 3.更新加急标记，并删除临时表

                                string sql = @" UPDATE c SET c.IsQuick=t.IsQuick FROM dbo.ContributionInfo c,#ContributionTemp t WHERE c.CID=t.CID AND c.JournalID=t.JournalID
                                            DROP TABLE #ContributionTemp;";
                                DbCommand cmdSet = db.GetSqlStringCommand(sql);
                                db.ExecuteNonQuery(cmdSet, (DbTransaction)transaction);

                                # endregion

                                transaction.Commit();
                            }
                            catch (Exception ext)
                            {
                                transaction.Rollback();
                                throw ext;
                            }
                        }

                        connection.Close();
                    }
                    flag = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                flag = UpdateContributionIsQuick(cEntityList[0]);
            }
            return flag;
        }

        /// <summary>
        /// 更新稿件加急标记
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool UpdateContributionIsQuick(ContributionInfoQuery cEntity)
        {
            bool flag = false;
            string sql = "UPDATE dbo.ContributionInfo SET IsQuick=@IsQuick WHERE CID=@CID AND JournalID=@JournalID";
            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "@CID", DbType.Int64, cEntity.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, cEntity.JournalID);
            db.AddInParameter(cmd, "@IsQuick", DbType.AnsiString, cEntity.IsQuick);

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

        # endregion

        # region 更新稿件状态及审核状态

        /// <summary>
        /// 更新稿件状态及审核状态
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool UpdateContributionStatus(ContributionInfoQuery cEntity)
        {
            bool flag = false;
            string sql = "";
            if(cEntity.Status != null && !string.IsNullOrEmpty(cEntity.AuditStatus))
            {
                sql = "UPDATE dbo.ContributionInfo SET Status=@Status,AuditStatus=@AuditStatus WHERE CID=@CID AND JournalID=@JournalID";
            }
            else if(cEntity.Status != null)
            {
                sql = "UPDATE dbo.ContributionInfo SET Status=@Status WHERE CID=@CID AND JournalID=@JournalID";
            }
            else if(!string.IsNullOrEmpty(cEntity.AuditStatus))
            {
                sql = "UPDATE dbo.ContributionInfo SET AuditStatus=@AuditStatus WHERE CID=@CID AND JournalID=@JournalID";
            }
            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "@CID", DbType.Int64, cEntity.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, cEntity.JournalID);
            if (cEntity.Status != null)
            {
                db.AddInParameter(cmd, "@Status", DbType.Int32, cEntity.Status);
            }
            if (!string.IsNullOrEmpty(cEntity.AuditStatus))
            {
                db.AddInParameter(cmd, "@AuditStatus", DbType.AnsiString, cEntity.AuditStatus);
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

        # endregion

        #region 稿件备注
         /// <summary>
        /// 新增稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddCRemark(CRemarkEntity model)
        {
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @CID");
            sqlCommandText.Append(", @JournalID");
            sqlCommandText.Append(", @AuthorID");
            sqlCommandText.Append(", @Remark");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.CRemark ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@CID", DbType.Int64, model.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, model.AuthorID);
            db.AddInParameter(cmd, "@Remark", DbType.AnsiString, model.Remark.TextFilter());

            return db.ExecuteNonQuery(cmd) > 0;
        }

        /// <summary>
        /// 编辑稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCRemark(CRemarkEntity model)
        {           
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  JournalID=@JournalID and AuthorID=@AuthorID and CID=@CID and RemarkID=@RemarkID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Remark=@Remark");           

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.CRemark SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, model.AuthorID);
            db.AddInParameter(cmd, "@CID", DbType.Int64, model.CID);
            db.AddInParameter(cmd, "@RemarkID", DbType.Int64, model.RemarkID);
            db.AddInParameter(cmd, "@Remark", DbType.AnsiString, model.Remark.TextFilter());

            return db.ExecuteNonQuery(cmd) > 0;
        }

        /// <summary>
        /// 获取稿件备注
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public CRemarkEntity GetCRemark(CRemarkQuery query)
        {
            CRemarkEntity model = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 * FROM dbo.CRemark WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE JournalID=@JournalID and AuthorID=@AuthorID and CID=@CID order by RemarkID desc ");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, query.AuthorID);
            db.AddInParameter(cmd, "@CID", DbType.Int64, query.CID);           

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    model = new CRemarkEntity();
                    model.RemarkID = dr.GetDrValue<Int64>("RemarkID");
                    model.AuthorID = dr.GetDrValue<Int64>("AuthorID");
                    model.CID = dr.GetDrValue<Int64>("CID");
                    model.JournalID = dr.GetDrValue<Int64>("JournalID");
                    model.Remark = dr.GetDrValue<String>("Remark");
                    model.AddDate = dr.GetDrValue<DateTime>("AddDate");
                }
                dr.Close();
            }
            return model;
        }
        #endregion

        # region 稿件介绍信

        /// <summary>
        /// 更新介绍信标记
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIntroLetter(ContributionInfoQuery model)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  CID=@CID AND JournalID=@JournalID ");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" IntroLetterPath=@IntroLetterPath");
            try
            {
                DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.ContributionInfo SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);
                db.AddInParameter(cmd, "@CID", DbType.Int64, model.CID);
                db.AddInParameter(cmd, "@IntroLetterPath", DbType.String, model.IntroLetterPath);
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return flag;
        }

        # endregion

        #region 收稿量统计
        /// <summary>
        /// 按年月统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionAccountEntity> GetContributionAccountListByYear(ContributionAccountQuery query)
        {
            string strSql = @"SELECT Year(AddDate) as Year,Month(AddDate) as Month,COUNT(1) as Account FROM dbo.ContributionInfo a with(nolock)
                              WHERE Status<>-999 and JournalID="+query.JournalID+" and Year(AddDate)=" + query.Year +  
                             " GROUP BY Year(AddDate),Month(AddDate)";
            return db.GetList<ContributionAccountEntity>(strSql, (dr) =>
                {
                    List<ContributionAccountEntity> list = new List<ContributionAccountEntity>();
                    ContributionAccountEntity model = null;
                    while (dr.Read())
                    {
                        model = new ContributionAccountEntity();
                        model.Year = dr.GetDrValue<Int32>("Year");
                        model.Month = dr.GetDrValue<Int32>("Month");
                        model.Account = TypeParse.ToDecimal(dr["Account"]);
                        list.Add(model);
                    }
                    dr.Close();
                    return list;
                });
        }

        /// <summary>
        /// 按基金级别统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionAccountEntity> GetContributionAccountListByFund(ContributionAccountQuery query)
        {
            string strSql = @"SELECT b.FundLevel,COUNT(1) as Account FROM dbo.ContributionInfo a with(nolock) 
                            INNER JOIN dbo.ContributionFund b with(nolock) ON a.JournalID=b.JournalID and a.CID=b.CID
                            WHERE a.Status<>-999 and a.JournalID=" + query.JournalID + " GROUP BY b.FundLevel";
            return db.GetList<ContributionAccountEntity>(strSql, (dr) =>
            {
                List<ContributionAccountEntity> list = new List<ContributionAccountEntity>();
                ContributionAccountEntity model = null;
                while (dr.Read())
                {
                    model = new ContributionAccountEntity();
                    model.FundLevel = dr.GetDrValue<Int32>("FundLevel");                  
                    model.Account = TypeParse.ToDecimal(dr["Account"]);
                    list.Add(model);
                }
                return list;
            });
        }

        /// <summary>
        /// 按作者统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionAccountEntity> GetContributionAccountListByAuhor(ContributionAccountQuery query)
        {
            string tableSql = @"SELECT AuthorID,COUNT(1) as Account FROM dbo.ContributionInfo a with(nolock)
                              WHERE Status<>-999 and JournalID=" + query.JournalID + " GROUP BY AuthorID";
            string strSql = tableSql, sumStr = string.Empty;
            if (!query.IsReport)
            {
                sumStr = string.Format("SELECT RecordCount=COUNT(1),Account=SUM(Account) FROM ({0}) t", tableSql);
                strSql = string.Format(SQL_Page_Select_ROWNumber, tableSql, query.StartIndex, query.EndIndex, "AuthorID asc");
            }
            return db.GetPageList<ContributionAccountEntity>(strSql
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                    pager.Money = TypeParse.ToDecimal(dr["Account"]);
                }
                , (dr) =>
                {
                    List<ContributionAccountEntity> list = new List<ContributionAccountEntity>();
                    ContributionAccountEntity model = null;
                    while (dr.Read())
                    {
                        model = new ContributionAccountEntity();
                        model.AuthorID = dr.GetDrValue<Int64>("AuthorID");
                        model.Account = TypeParse.ToDecimal(dr["Account"]);
                        list.Add(model);
                    }
                    return list;
                });
        }
        #endregion

        /// <summary>
        ///  构造批量插入的Table
        /// </summary>
        /// <returns></returns>
        private DataTable ContributionFlagDT()
        {
            DataTable menuAuthTable = new DataTable("Contribution");
            menuAuthTable.Columns.Add("CID", Type.GetType("System.Int64"));
            menuAuthTable.Columns.Add("JournalID", Type.GetType("System.Int64"));
            menuAuthTable.Columns.Add("Flag", Type.GetType("System.String"));
            return menuAuthTable;
        }

        /// <summary>
        ///  构造批量插入的Table
        /// </summary>
        /// <returns></returns>
        private DataTable ContributionIsQuickDT()
        {
            DataTable menuAuthTable = new DataTable("Contribution");
            menuAuthTable.Columns.Add("CID", Type.GetType("System.Int64"));
            menuAuthTable.Columns.Add("JournalID", Type.GetType("System.Int64"));
            menuAuthTable.Columns.Add("IsQuick", Type.GetType("System.Boolean"));
            return menuAuthTable;
        }

        /// <summary>
        /// 获取稿件作者数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<Int64, String> GetContributionAuthorDict(ContributionInfoQuery query)
        {
            IDictionary<Int64, String> dict = new Dictionary<Int64, String>();
            string strSql = "SELECT CAuthorID,AuthorName FROM dbo.ContributionAuthor with(nolock) WHERE " + ContributionInfoQueryToSQLWhere(query);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                Int64 id = 0;
                while (dr.Read())
                {
                    id = (Int64)dr["CAuthorID"];
                    if (!dict.ContainsKey(id))
                        dict.Add(id, (String)dr["AuthorName"]);
                    
                }
                dr.Close();
            }
            return dict;
        }

        /// <summary>
        /// 将查询实体转换为Where语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Where语句，不包含Where</returns>
        /// </summary>
        public string ContributionInfoQueryToSQLWhere(ContributionInfoQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JouranalID = " + query.JournalID);
            if (query.AuthorID != null)
            {
                sbWhere.Append(" AND AuthorID = " + query.AuthorID);
            }
            if (query.CID != 0)
            {
                sbWhere.Append(" AND CID = " + query.CID);
            }
            if (query.CNumber != null)
            {
                sbWhere.Append(" AND CNumber = " + query.CNumber);
            }
            return sbWhere.ToString();
        }


    }
}

