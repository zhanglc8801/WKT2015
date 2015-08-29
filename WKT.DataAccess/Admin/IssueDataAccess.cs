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
using WKT.Common.Security;
using WKT.Common.Utils;
using WKT.Common.Extension;

namespace WKT.DataAccess
{
    public class IssueDataAccess : DataAccessBase
    {
        #region 属性
        private string dbPrividerName = string.Empty;
        private IDbProvider db = null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public IssueDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if (db == null)
                throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static IssueDataAccess _instance = new IssueDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static IssueDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region 年卷设置
        /// <summary>
        /// 新增年卷
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        public bool AddYearVolume(YearVolumeEntity yearVolumeEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @Year");
            sqlCommandText.Append(", @Volume");
            sqlCommandText.Append(", @Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.YearVolume ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, yearVolumeEntity.JournalID);
            db.AddInParameter(cmd, "@Year", DbType.Int32, yearVolumeEntity.Year);
            db.AddInParameter(cmd, "@Volume", DbType.Int32, yearVolumeEntity.Volume);
            db.AddInParameter(cmd, "@Status", DbType.Byte, yearVolumeEntity.Status);

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
        /// 编辑年卷
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        public bool UpdateYearVolume(YearVolumeEntity yearVolumeEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  SetID=@SetID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Year=@Year");
            sqlCommandText.Append(", Volume=@Volume");
            sqlCommandText.Append(", Status=@Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.YearVolume SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@SetID", DbType.Int64, yearVolumeEntity.SetID);
            db.AddInParameter(cmd, "@Year", DbType.Int32, yearVolumeEntity.Year);
            db.AddInParameter(cmd, "@Volume", DbType.Int32, yearVolumeEntity.Volume);
            db.AddInParameter(cmd, "@Status", DbType.Byte, yearVolumeEntity.Status);

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
        /// 年是否存在
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        public bool YearVolumeYearIsExists(YearVolumeEntity yearVolumeEntity)
        {
            string strSql = string.Format("SELECT 1 FROM dbo.YearVolume WHERE JournalID={0} and Year={1}", yearVolumeEntity.JournalID, yearVolumeEntity.Year);
            if (yearVolumeEntity.SetID > 0)
                strSql += " and SetID<>" + yearVolumeEntity.SetID;
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteScalar(cmd).TryParse<Int32>() == 1;
        }

        /// <summary>
        /// 卷是否存在
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        public bool YearVolumeVolumeIsExists(YearVolumeEntity yearVolumeEntity)
        {
            string strSql = string.Format("SELECT 1 FROM dbo.YearVolume WHERE JournalID={0} and Volume={1}", yearVolumeEntity.JournalID, yearVolumeEntity.Volume);
            if (yearVolumeEntity.SetID > 0)
                strSql += " and SetID<>" + yearVolumeEntity.SetID;
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteScalar(cmd).TryParse<Int32>() == 1;
        }

        /// <summary>
        /// 获取最新的年卷
        /// </summary>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        public YearVolumeEntity GetMaxYearVolume(Int64 JournalID)
        {
            YearVolumeEntity model = null;
            string strSql = string.Format("SELECT MAX(Year) as Year,MAX(Volume) as Volume FROM dbo.YearVolume WHERE JournalID={0}", JournalID);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    model = new YearVolumeEntity();
                    model.Year = dr.GetDrValue<Int32>("Year", 2004) + 1;
                    model.Volume = dr.GetDrValue<Int32>("Volume", 0) + 1;
                }
                dr.Close();
            }
            return model;
        }

        /// <summary>
        /// 获取年卷实体
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        public YearVolumeEntity GetYearVolume(Int64 setID)
        {
            YearVolumeEntity model = null;
            string strSql = string.Format("SELECT TOP 1 * FROM dbo.YearVolume WHERE setID={0}", setID);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    model = MakeYearVolume(dr);
                }
                dr.Close();
            }
            return model;
        }

        /// <summary>
        /// 删除年卷
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        public IList<Int64> DelYearVolume(Int64[] setID)
        {
            if (setID == null || setID.Length < 1)
                return null;
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    IList<Int64> list = new List<Int64>();
                    string strSql = @"if not exists(select 1 from dbo.IssueContent a with(nolock) 
                                              INNER JOIN dbo.YearVolume b on a.JournalID=b.JournalID and a.Year=b.Year and a.Volume=b.Volume and b.setID={0})
                                       begin
                                          DELETE dbo.YearVolume WHERE setID={0}
                                       end";
                    DbCommand cmd = null;
                    foreach (var id in setID)
                    {
                        cmd = db.GetSqlStringCommand(string.Format(strSql, id));
                        if (db.ExecuteNonQuery(cmd, trans) < 1)
                            list.Add(id);
                    }

                    trans.Commit();
                    return list;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取年卷分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<YearVolumeEntity> GetYearVolumePageList(YearVolumeQuery query)
        {
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY " + GetYearVolumeOrder(query) + ") AS ROW_ID FROM dbo.YearVolume with(nolock)",
                   sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.YearVolume with(nolock)";
            string whereSQL = GetYearVolumeFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<YearVolumeEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeYearVolumeList);
        }

        /// <summary>
        /// 获取年卷数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<YearVolumeEntity> GetYearVolumeList(YearVolumeQuery query)
        {
            string strSql = "SELECT * FROM dbo.YearVolume with(nolock)";
            string whereSQL = GetYearVolumeFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " order by " + GetYearVolumeOrder(query);
            return db.GetList<YearVolumeEntity>(strSql, MakeYearVolumeList);
        }

        /// <summary>
        /// 获取年卷条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetYearVolumeFilter(YearVolumeQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID= " + query.JournalID);
            if (query.Status != null)
                strFilter.Append(" and Status=").Append(query.Status.Value);
            return strFilter.ToString();
        }

        /// <summary>
        /// 获取年卷排序
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetYearVolumeOrder(YearVolumeQuery query)
        {
            return " Year DESC ";
        }

        /// <summary>
        /// 组装年卷数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<YearVolumeEntity> MakeYearVolumeList(IDataReader dr)
        {
            List<YearVolumeEntity> list = new List<YearVolumeEntity>();
            while (dr.Read())
            {
                list.Add(MakeYearVolume(dr));
            }
            return list;
        }

        /// <summary>
        /// 组装年卷数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private YearVolumeEntity MakeYearVolume(IDataReader dr)
        {
            YearVolumeEntity model = new YearVolumeEntity();
            model.SetID = (Int64)dr["SetID"];
            model.JournalID = (Int64)dr["JournalID"];
            model.Year = (Int32)dr["Year"];
            model.Volume = (Int32)dr["Volume"];
            model.Status = (Byte)dr["Status"];
            model.AddDate = (DateTime)dr["AddDate"];
            return model;
        }
        #endregion

        #region 期数设置
        /// <summary>
        /// 新增期设置
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        public bool AddIssueSet(IssueSetEntity issueSetEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @Issue");
            sqlCommandText.Append(", @TitlePhoto");
            sqlCommandText.Append(", @Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.IssueSet ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, issueSetEntity.JournalID);
            db.AddInParameter(cmd, "@Issue", DbType.Int32, issueSetEntity.Issue);
            db.AddInParameter(cmd, "@TitlePhoto", DbType.AnsiString, issueSetEntity.TitlePhoto);
            db.AddInParameter(cmd, "@Status", DbType.Byte, issueSetEntity.Status);

            try
            {
                db.ExecuteNonQuery(cmd);
                AddJournalOfCost(issueSetEntity);
                flag = true;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }


        /// <summary>
        /// 期刊费用   列于：印刷费
        /// </summary>
        public bool AddJournalOfCost(IssueSetEntity issueSetEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @Issue");
            sqlCommandText.Append(", @Year");
            sqlCommandText.Append(", @PrintExpenses");
            sqlCommandText.Append(", @Type");
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.JournalOfCost ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, issueSetEntity.JournalID);
            db.AddInParameter(cmd, "@Issue", DbType.Int32, issueSetEntity.Issue);
            db.AddInParameter(cmd, "@Year", DbType.Int32, issueSetEntity.Year);
            db.AddInParameter(cmd, "@PrintExpenses", DbType.Decimal, issueSetEntity.PrintExpenses);
            db.AddInParameter(cmd, "@Type", DbType.Int32, issueSetEntity.Type);
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
        /// 编辑期设置
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        public bool UpdateIssueSet(IssueSetEntity issueSetEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  IssueSetID=@IssueSetID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Issue=@Issue");
            sqlCommandText.Append(", TitlePhoto=@TitlePhoto");
            sqlCommandText.Append(", Status=@Status");


            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.IssueSet SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@IssueSetID", DbType.Int64, issueSetEntity.IssueSetID);
            db.AddInParameter(cmd, "@Issue", DbType.Int32, issueSetEntity.Issue);
            db.AddInParameter(cmd, "@TitlePhoto", DbType.AnsiString, issueSetEntity.TitlePhoto);
            db.AddInParameter(cmd, "@Status", DbType.Byte, issueSetEntity.Status);

            try
            {
                db.ExecuteNonQuery(cmd);
                UpdateJournalOfCost(issueSetEntity);
                flag = true;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }

        /// <summary>
        /// 期刊费用   列于：印刷费
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        public bool UpdateJournalOfCost(IssueSetEntity issueSetEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  Issue=@Issue and Year=@Year AND JournalID=@JournalID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("PrintExpenses=@PrintExpenses");
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.JournalOfCost SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));
            db.AddInParameter(cmd, "@Issue", DbType.Int32, issueSetEntity.Issue);
            db.AddInParameter(cmd, "@Year", DbType.Int32, issueSetEntity.Year);
            db.AddInParameter(cmd, "@PrintExpenses", DbType.Decimal, issueSetEntity.PrintExpenses);
            db.AddInParameter(cmd, "@JournalID", DbType.Decimal, issueSetEntity.JournalID);
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
        /// 期设置是否存在
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        public bool IssueSetIsExists(IssueSetEntity issueSetEntity)
        {
            string strSql = string.Format("SELECT 1 FROM dbo.IssueSet with(nolock) WHERE JournalID={0} and Issue={1}", issueSetEntity.JournalID, issueSetEntity.Issue);
            if (issueSetEntity.IssueSetID > 0)
                strSql += " and IssueSetID<>" + issueSetEntity.IssueSetID;
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(cmd).TryParse<Int32>() == 1;
        }

        /// <summary>
        /// 获取最新的期设置
        /// </summary>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        public IssueSetEntity GetMaxIssueSet(Int64 JournalID)
        {
            IssueSetEntity model = null;
            string strSql = "SELECT MAX(Issue) as Issue FROM dbo.IssueSet with(nolock) WHERE JournalID=" + JournalID;
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    model = new IssueSetEntity();
                    model.Issue = dr.GetDrValue<Int32>("Issue", 0) + 1;
                }
                dr.Close();
            }
            return model;
        }

        /// <summary>
        /// 获取期设置
        /// </summary>
        /// <param name="IssueSetID"></param>
        /// <returns></returns>
        public IssueSetEntity GetIssueSet(Int64 IssueSetID)
        {
            IssueSetEntity model = null;
            string strSql = "SELECT TOP 1 * FROM dbo.IssueSet with(nolock) WHERE IssueSetID=" + IssueSetID;
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                    model = MakeIssueSetInit(dr);
                dr.Close();
            }
            return model;
        }

        /// <summary>
        /// 获取期设置
        /// </summary>
        /// <param name="IssueSetID"></param>
        /// <returns></returns>
        public IssueSetEntity GetJournalOfCostByYearAndIssue(int year,int issue)
        {
            IssueSetEntity model = null;
            string strSql = "SELECT TOP 1 * FROM dbo.JournalOfCost with(nolock) WHERE Issue=" + issue + " and Year=" + year;
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                    model = MakeJournalOfCost(dr);
                dr.Close();
            }
            return model;
        }

        /// <summary>
        /// 删除期设置
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        public IList<Int64> DelIssueSet(Int64[] IssueSetID)
        {
            if (IssueSetID == null || IssueSetID.Length < 1)
                return null;
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    IList<Int64> list = new List<Int64>();
                    string strSql = @"if not exists(select 1 from dbo.IssueContent a with(nolock) 
                                              INNER JOIN dbo.IssueSet b on a.JournalID=b.JournalID and a.Issue=b.Issue and b.IssueSetID={0})
                                       begin
                                          DELETE dbo.IssueSet WHERE IssueSetID={0}
                                       end";
                    DbCommand cmd = null;
                    foreach (var id in IssueSetID)
                    {
                        cmd = db.GetSqlStringCommand(string.Format(strSql, id));
                        if (db.ExecuteNonQuery(cmd, trans) < 1)
                            list.Add(id);
                    }

                    trans.Commit();
                    conn.Close();
                    return list;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取期设置分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueSetEntity> GetIssueSetPageList(IssueSetQuery query)
        {
            string strSql = "SELECT IssueSet.*,(case when  a.PrintExpenses  is null then 0 else a.PrintExpenses end)as PrintExpenses ,ROW_NUMBER() OVER(ORDER BY " + GetIssueSetOrder(query) + ") AS ROW_ID FROM dbo.IssueSet LEFT JOIN (select Issue,PrintExpenses from  JournalOfCost where Year=" + query.Year + " AND JournalID="+query.JournalID+" ) as a  ON IssueSet.Issue=a.Issue ",
                   sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.IssueSet with(nolock)";
            string whereSQL = GetIssueSetFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<IssueSetEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeIssueSetList);
        }

        /// <summary>
        /// 获取期设置数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<IssueSetEntity> GetIssueSetList(IssueSetQuery query)
        {
            string strSql = "SELECT * FROM dbo.IssueSet with(nolock)";
            string whereSQL = GetIssueSetFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " order by " + GetIssueSetOrder(query);
            return db.GetList<IssueSetEntity>(strSql, MakeIssueSetList);
        }

        /// <summary>
        /// 获取期设置条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetIssueSetFilter(IssueSetQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID= " + query.JournalID);
            if (query.Status != null)
                strFilter.Append(" and Status=").Append(query.Status.Value);
            return strFilter.ToString();
        }

        /// <summary>
        /// 获取期设置排序
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetIssueSetOrder(IssueSetQuery query)
        {
            return " IssueSet.Issue ASC ";
        }

        /// <summary>
        /// 组装期设置数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<IssueSetEntity> MakeIssueSetList(IDataReader dr)
        {
            List<IssueSetEntity> list = new List<IssueSetEntity>();
            while (dr.Read())
            {
                list.Add(MakeIssueSet(dr));
            }

            return list;
        }

        /// <summary>
        /// 组装期设置数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private IssueSetEntity MakeIssueSet(IDataReader dr)
        {
            IssueSetEntity issueSetEntity = new IssueSetEntity();
            issueSetEntity.IssueSetID = (Int64)dr["IssueSetID"];
            issueSetEntity.JournalID = (Int64)dr["JournalID"];
            issueSetEntity.Issue = (Int32)dr["Issue"];
            issueSetEntity.TitlePhoto = (String)dr["TitlePhoto"];
            issueSetEntity.Status = (Byte)dr["Status"];
            issueSetEntity.AddDate = (DateTime)dr["AddDate"];
            if (dr.HasColumn("PrintExpenses"))
            {
                issueSetEntity.PrintExpenses = Convert.IsDBNull(dr["PrintExpenses"]) ? 0 : (decimal)dr["PrintExpenses"];
            }

            return issueSetEntity;
        }

        /// <summary>
        /// 组装期设置数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private IssueSetEntity MakeJournalOfCost(IDataReader dr)
        {
            IssueSetEntity issueSetEntity = new IssueSetEntity();
            issueSetEntity.PrintExpenses = Convert.IsDBNull(dr["PrintExpenses"]) ? 0 : (decimal)dr["PrintExpenses"];

            return issueSetEntity;
        }

        /// <summary>
        /// 组装期设置数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private IssueSetEntity MakeIssueSetInit(IDataReader dr)
        {
            IssueSetEntity issueSetEntity = new IssueSetEntity();
            issueSetEntity.IssueSetID = (Int64)dr["IssueSetID"];
            issueSetEntity.JournalID = (Int64)dr["JournalID"];
            issueSetEntity.Issue = (Int32)dr["Issue"];
            issueSetEntity.TitlePhoto = (String)dr["TitlePhoto"];
            issueSetEntity.Status = (Byte)dr["Status"];
            issueSetEntity.AddDate = (DateTime)dr["AddDate"];

            return issueSetEntity;
        }

        #endregion

        #region 期刊栏目
        /// <summary>
        /// 新增期刊栏目
        /// </summary>
        /// <param name="journalChannelEntity"></param>
        /// <returns></returns>
        public bool AddJournalChannel(JournalChannelEntity journalChannelEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @PChannelID");
            sqlCommandText.Append(", @ChannelName");
            sqlCommandText.Append(", @SortID");
            sqlCommandText.Append(", @Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.JournalChannel ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, journalChannelEntity.JournalID);
            db.AddInParameter(cmd, "@PChannelID", DbType.Int64, journalChannelEntity.PChannelID);
            db.AddInParameter(cmd, "@ChannelName", DbType.AnsiString, journalChannelEntity.ChannelName);
            db.AddInParameter(cmd, "@SortID", DbType.Int32, journalChannelEntity.SortID);
            db.AddInParameter(cmd, "@Status", DbType.Byte, journalChannelEntity.Status);

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
        /// 编辑期刊栏目
        /// </summary>
        /// <param name="journalChannelEntity"></param>
        /// <returns></returns>
        public bool UpdateJournalChannel(JournalChannelEntity journalChannelEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  JChannelID=@JChannelID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" ChannelName=@ChannelName");
            sqlCommandText.Append(", SortID=@SortID");
            sqlCommandText.Append(", Status=@Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.JournalChannel SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@JChannelID", DbType.Int64, journalChannelEntity.JChannelID);
            db.AddInParameter(cmd, "@ChannelName", DbType.AnsiString, journalChannelEntity.ChannelName);
            db.AddInParameter(cmd, "@SortID", DbType.Int32, journalChannelEntity.SortID);
            db.AddInParameter(cmd, "@Status", DbType.Byte, journalChannelEntity.Status);

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
        /// 获取期刊栏目
        /// </summary>
        /// <param name="jChannelID"></param>
        /// <returns></returns>
        public JournalChannelEntity GetJournalChannel(Int64 jChannelID)
        {
            JournalChannelEntity journalChannelEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  JChannelID,JournalID,PChannelID,ChannelName,SortID,Status,AddDate FROM dbo.JournalChannel WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  JChannelID=@JChannelID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JChannelID", DbType.Int64, jChannelID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                    journalChannelEntity = MakeJournalChannel(dr);
                dr.Close();
            }
            return journalChannelEntity;
        }

        /// <summary>
        /// 删除期刊栏目设置
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        public bool DelJournalChannel(Int64 jChannelID)
        {
            string strSql = "DELETE dbo.JournalChannel WHERE JChannelID=" + jChannelID;
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(cmd) > 0;
        }

        /// <summary>
        /// 期刊栏目前需要做的判断
        /// </summary>
        /// <param name="jChannelID"></param>
        /// <param name="JournalID"></param>
        /// <param name="type">0:是否存在下级 1:是否存在期刊表中 2:是否存在稿件表中</param>
        /// <returns>true:不能删除</returns>
        public bool DelJournalChannelJudge(Int64 jChannelID, Int64 JournalID, Int32 type)
        {
            string strSql = string.Empty;
            switch (type)
            {
                case 0: strSql = "SELECT 1 FROM dbo.JournalChannel with(nolock) WHERE JournalID={0} and PChannelID={1}"; break;
                case 1: strSql = "SELECT 1 FROM dbo.IssueContent with(nolock) WHERE JournalID={0} and JChannelID={1}"; break;
                case 2: strSql = "SELECT 1 FROM dbo.ContributionInfo with(nolock) WHERE JournalID={0} and JChannelID={1}"; break;
            }
            if (string.IsNullOrWhiteSpace(strSql))
                return false;
            DbCommand cmd = db.GetSqlStringCommand(string.Format(strSql, JournalID, jChannelID));
            return db.ExecuteScalar(cmd).TryParse<Int32>() == 1;
        }

        /// <summary>
        /// 获取期刊栏目分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<JournalChannelEntity> GetJournalChannelPageList(JournalChannelQuery query)
        {
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY " + GetJournalChannelOrder(query) + ") AS ROW_ID FROM dbo.JournalChannel with(nolock)",
                   sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.JournalChannel with(nolock)";
            string whereSQL = GetJournalChannelFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<JournalChannelEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeJournalChannelList);
        }

        /// <summary>
        /// 获取期刊栏目数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<JournalChannelEntity> GetJournalChannelList(JournalChannelQuery query)
        {
            string strSql = "SELECT * FROM dbo.JournalChannel with(nolock)";
            string whereSQL = GetJournalChannelFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " order by " + GetJournalChannelOrder(query);
            return db.GetList<JournalChannelEntity>(strSql, MakeJournalChannelList);
        }

        /// <summary>
        /// 根据期刊数据 按照期刊栏目数据分组 获取当前期刊数据所属的期刊栏目数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<JournalChannelEntity> GetJournalChannelListByIssueContent(JournalChannelQuery query)
        {
            string strSql = "SELECT [JChannelID],[JournalID],[PChannelID] ,[ChannelName],[SortID],[Status],[AddDate] FROM dbo.JournalChannel with(nolock) where JChannelID in  (select JChannelID  from  dbo.IssueContent    group  by  JChannelID) ";
            string whereSQL = GetJournalChannelFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " AND " + whereSQL;
            }
            strSql += " order by " + GetJournalChannelOrder(query);

            return db.GetList<JournalChannelEntity>(strSql, MakeJournalChannelList);
        }


        /// <summary>
        /// 获取期刊栏目条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetJournalChannelFilter(JournalChannelQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID= " + query.JournalID);
            if (query.Status != null)
                strFilter.Append(" and Status=").Append(query.Status.Value);
            return strFilter.ToString();
        }

        /// <summary>
        /// 获取期刊栏目排序
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetJournalChannelOrder(JournalChannelQuery query)
        {
            return " SortID ASC ";
        }

        /// <summary>
        /// 组装期刊栏目数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<JournalChannelEntity> MakeJournalChannelList(IDataReader dr)
        {
            List<JournalChannelEntity> list = new List<JournalChannelEntity>();
            while (dr.Read())
            {
                list.Add(MakeJournalChannel(dr));
            }
            if (!dr.IsClosed)
            {
                dr.Close();
            }
            return list;
        }

        /// <summary>
        /// 组装期刊栏目数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private JournalChannelEntity MakeJournalChannel(IDataReader dr)
        {
            JournalChannelEntity journalChannelEntity = new JournalChannelEntity();
            journalChannelEntity.JChannelID = (Int64)dr["JChannelID"];
            journalChannelEntity.JournalID = (Int64)dr["JournalID"];
            journalChannelEntity.PChannelID = (Int64)dr["PChannelID"];
            journalChannelEntity.ChannelName = (String)dr["ChannelName"];
            journalChannelEntity.SortID = (Int32)dr["SortID"];
            journalChannelEntity.Status = (Byte)dr["Status"];
            journalChannelEntity.AddDate = (DateTime)dr["AddDate"];
            return journalChannelEntity;
        }
        #endregion

        #region 期刊内容

        /// <summary>
        /// 保存期刊信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Int64 SaveIssueContent(IssueContentEntity model)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    if (model.ContentID == 0)
                    {
                        Int64 ContentID = AddIssueContent(model, trans);

                        if (ContentID == 0)
                            throw new Exception();

                        model.ContentID = ContentID;

                        //参考文献
                        if (model.ReferenceList != null && model.ReferenceList.Count > 0)
                        {
                            foreach (var item in model.ReferenceList)
                            {
                                item.ContentID = model.ContentID;
                                item.JournalID = model.JournalID;
                                AddIssueReference(item, trans);
                            }
                        }
                    }
                    else
                    {
                        UpdateIssueContent(model, trans);

                        Int64[] ContentID = new Int64[] { model.ContentID };

                        //参考文献
                        Int64[] ReferenceIDs = null;
                        if (model.ReferenceList != null && model.ReferenceList.Count > 0)
                        {
                            ReferenceIDs = model.ReferenceList.Where(p => p.ReferenceID > 0).Select(p => p.ReferenceID).ToArray();
                        }
                        DelIssueReferenceByContentID(ContentID, trans, ReferenceIDs);
                        if (model.ReferenceList != null && model.ReferenceList.Count > 0)
                        {
                            foreach (var item in model.ReferenceList)
                            {
                                if (item.ReferenceID > 0)
                                {
                                    UpdateIssueReference(item, trans);
                                }
                                else
                                {
                                    item.JournalID = model.JournalID;
                                    item.ContentID = model.ContentID;
                                    AddIssueReference(item, trans);
                                }
                            }
                        }
                    }

                    trans.Commit();
                    conn.Close();
                    return model.ContentID;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 新增期刊表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddIssueContent(IssueContentEntity model)
        {
            return AddIssueContent(model, null) > 0;
        }

        /// <summary>
        /// 编辑期刊表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContent(IssueContentEntity model)
        {
            return UpdateIssueContent(model, null);
        }

        /// <summary>
        /// 更新期刊内容浏览次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContentHits(IssueContentQuery model)
        {
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  ContentID=@ContentID AND JournalID=@JournalID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Hits=Hits+1");
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.IssueContent SET {0} WHERE {1}", sqlCommandText.ToString(), whereCommandText.ToString()));
            db.AddInParameter(cmd, "@ContentID", DbType.Int64, model.contentID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);

            bool result = false;
            try
            {
                db.ExecuteNonQuery(cmd);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 更新期刊内容浏览次数(RichHTML)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContentHtmlHits(IssueContentQuery model)
        {
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  ContentID=@ContentID AND JournalID=@JournalID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" HtmlHits=HtmlHits+1");
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.IssueContent SET {0} WHERE {1}", sqlCommandText.ToString(), whereCommandText.ToString()));
            db.AddInParameter(cmd, "@ContentID", DbType.Int64, model.contentID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);

            bool result = false;
            try
            {
                db.ExecuteNonQuery(cmd);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 更新期刊内容下载次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContentDownloads(IssueContentQuery model)
        {
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  ContentID=@ContentID AND  JournalID=@JournalID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Downloads=Downloads+1");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.IssueContent SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));
            db.AddInParameter(cmd, "@ContentID", DbType.Int64, model.contentID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, model.JournalID);

            bool result = false;
            try
            {
                db.ExecuteNonQuery(cmd);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 获取期刊实体
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        public IssueContentEntity GetIssueContent(IssueContentQuery query)
        {
            IssueContentEntity issueContentEntity = null;
            string strSql = "SELECT TOP 1 * FROM dbo.vw_IssueContent WITH(NOLOCK) WHERE ContentID=@ContentID AND JournalID=@JournalID";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@ContentID", DbType.Int64, query.contentID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                    issueContentEntity = MakeIssueContent(dr);
                dr.Close();
            }
            return issueContentEntity;
        }

        /// <summary>
        /// 删除期刊信息
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public bool DelIssueContent(Int64[] contentID)
        {
            if (contentID == null || contentID.Length < 1)
                return false;
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    //删除参考文献
                    DelIssueReferenceByContentID(contentID, trans, null);

                    string strSql = "Delete dbo.IssueContent where contentID";
                    if (contentID.Length == 0)
                        strSql += " = " + contentID[0];
                    else
                        strSql += string.Format(" in ({0})", string.Join(",", contentID));
                    DbCommand cmd = db.GetSqlStringCommand(strSql);
                    if (db.ExecuteNonQuery(cmd, trans) < 1)
                        throw new Exception("删除期刊表信息失败！");

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
        /// 获取期刊分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueContentEntity> GetIssueContentPageList(IssueContentQuery query)
        {
            if (query.contentIDs!=null&&query.contentIDs.Length > 0)
            {
                string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY " + query.OrderStr + ") AS ROW_ID FROM dbo.vw_IssueContent with(nolock)"
                , sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.vw_IssueContent with(nolock)";
                string whereSQL = string.Format(" ContentID in ({0})", string.Join(",", query.contentIDs));// "ContentID in (" + query.Keywords + ")";
                if (!string.IsNullOrWhiteSpace(whereSQL))
                {
                    strSql += " WHERE " + whereSQL;
                    sumStr += " WHERE " + whereSQL;
                }
                return db.GetPageList<IssueContentEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                    , sumStr
                    , query.CurrentPage, query.PageSize
                    , (dr, pager) =>
                    {
                        pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                    }
                    , MakeIssueContentList);
            }

            else if (query.JChannelID == -1)//地震学报(合并摘要所用)，Keywords用于存储文章ID列表
            {
                string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY " + query.OrderStr + ") AS ROW_ID FROM dbo.vw_IssueContent with(nolock)"
                , sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.vw_IssueContent with(nolock)";
                string whereSQL = "ContentID in (" + query.Keywords + ")";
                if (!string.IsNullOrWhiteSpace(whereSQL))
                {
                    strSql += " WHERE " + whereSQL;
                    sumStr += " WHERE " + whereSQL;
                }
                return db.GetPageList<IssueContentEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                    , sumStr
                    , query.CurrentPage, query.PageSize
                    , (dr, pager) =>
                    {
                        pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                    }
                    , MakeIssueContentList);
            }
            else if (query.JChannelID == 1)//地震学报(期刊下载/查看排行所用)，其中query.Volume表示年范围的第二个year2 【year1,year2】
            {
                string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY " + query.OrderStr + ") AS ROW_ID FROM dbo.vw_IssueContent with(nolock) where Year between '" + query.Year + "' and '" + query.Volume + "'"
                , sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.vw_IssueContent with(nolock) where Year between '" + query.Year + "' and '" + query.Volume + "'";
                string whereSQL = string.Empty;
                if (query.JChannelID == 1)
                    whereSQL = "";
                else
                    whereSQL = GetIssueContentFilter(query);
                if (!string.IsNullOrWhiteSpace(whereSQL))
                {
                    strSql += " WHERE " + whereSQL;
                    sumStr += " WHERE " + whereSQL;
                }
                return db.GetPageList<IssueContentEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                    , sumStr
                    , query.CurrentPage, query.PageSize
                    , (dr, pager) =>
                    {
                        pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                    }
                    , MakeIssueContentList);
            }
            else
            {
                string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY " + query.OrderStr + ") AS ROW_ID FROM dbo.vw_IssueContent with(nolock)"
                , sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.vw_IssueContent with(nolock)";
                string whereSQL = GetIssueContentFilter(query);
                if (!string.IsNullOrWhiteSpace(whereSQL))
                {
                    strSql += " WHERE " + whereSQL;
                    sumStr += " WHERE " + whereSQL;
                }
                return db.GetPageList<IssueContentEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                    , sumStr
                    , query.CurrentPage, query.PageSize
                    , (dr, pager) =>
                    {
                        pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                    }
                    , MakeIssueContentList);
            }

        }

        /// <summary>
        /// 获取期刊数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<IssueContentEntity> GetIssueContentList(IssueContentQuery query)
        {
            string strSql = "SELECT * FROM dbo.vw_IssueContent with(nolock)";
            string whereSQL = GetIssueContentFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " ORDER BY " + query.OrderStr;
            return db.GetList<IssueContentEntity>(strSql, MakeIssueContentList);
        }

        # region 确定年期

        /// <summary>
        /// 设置录用稿件的年期
        /// </summary>
        /// <returns></returns>
        public bool SetContributionYearIssue(IssueContentQuery cEntity)
        {
            bool flag = false;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("UP_SetContributionYearIssue");
                db.AddInParameter(cmd, "@CID", DbType.Int64, cEntity.CID);
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, cEntity.JournalID);
                db.AddInParameter(cmd, "@AuthorID", DbType.Int64, cEntity.AuthorID);
                db.AddInParameter(cmd, "@JChannelID", DbType.Int64, cEntity.JChannelID);
                db.AddInParameter(cmd, "@Year", DbType.Int32, cEntity.Year);
                db.AddInParameter(cmd, "@Volume", DbType.Int32, cEntity.Volume);
                db.AddInParameter(cmd, "@Issue", DbType.Int32, cEntity.Issue);
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 确定录用稿件列表  TODO:没有用到，备用
        /// 确定年卷期使用
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueContentEntity> GetContributionIssuePageList(IssueContentQuery query)
        {
            int recordCount = 0;
            List<SqlParameter> listParameters = new List<SqlParameter>();
            StringBuilder sbSQL = new StringBuilder("SELECT i.ContentID,c.Title,c.CID,c.JournalID,c.CNumber,c.SubjectCat,c.AuthorID,c.AddDate,i.Issue,i.Year,i.Volume FROM dbo.ContributionInfo c WITH(NOLOCK) LEFT JOIN dbo.IssueContent i WITH(NOLOCK) ON c.JournalID=i.JournalID AND c.CID=i.CID WHERE c.JournalID=@JournalID AND c.Status=200");
            SqlParameter pJournalID = new SqlParameter("@JournalID", SqlDbType.BigInt);
            pJournalID.Value = query.JournalID;
            listParameters.Add(pJournalID);

            if (query.AuthorID != null && query.AuthorID > 0)
            {
                SqlParameter pAuthorID = new SqlParameter("@AuthorID", SqlDbType.BigInt);
                pAuthorID.Value = query.AuthorID.Value;
                listParameters.Add(pAuthorID);
                sbSQL.Append(" AND i.AuthorID=@AuthorID");
            }

            if (!string.IsNullOrEmpty(query.CNumber))
            {
                SqlParameter pCNumber = new SqlParameter("@CNumber", SqlDbType.VarChar, 50);
                pCNumber.Value = SecurityUtils.SafeSqlString(query.CNumber);
                listParameters.Add(pCNumber);
                sbSQL.Append(" AND c.CNumber=@CNumber");
            }
            if (query.JChannelID != null && query.JChannelID > 0)
            {
                SqlParameter pSubjectCat = new SqlParameter("@JChannelID", SqlDbType.BigInt);
                pSubjectCat.Value = query.JChannelID.Value;
                listParameters.Add(pSubjectCat);
                sbSQL.Append(" AND i.JChannelID=@JChannelID");
            }
            if (!string.IsNullOrEmpty(query.Title))
            {
                sbSQL.Append(" AND c.Title LIKE '%" + SecurityUtils.SafeSqlString(query.Title) + "%'");
            }

            if (query.Year != null && query.Year > 0)
            {
                SqlParameter pYear = new SqlParameter("@Year", SqlDbType.Int);
                pYear.Value = query.Year.Value;
                listParameters.Add(pYear);
                sbSQL.Append(" AND i.Year=@Year");
            }

            if (query.Issue != null && query.Issue > 0)
            {
                SqlParameter pIssue = new SqlParameter("@Issue", SqlDbType.Int);
                pIssue.Value = query.Issue.Value;
                listParameters.Add(pIssue);
                sbSQL.Append(" AND i.Issue=@Issue");
            }

            DataSet ds = db.PageingQuery(query.CurrentPage, query.PageSize, sbSQL.ToString(), "c.AddDate DESC", listParameters.ToArray(), ref recordCount);
            Pager<IssueContentEntity> pager = new Pager<IssueContentEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                List<IssueContentEntity> list = new List<IssueContentEntity>();
                if (ds != null)
                {
                    IssueContentEntity cEntity = null;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        cEntity = new IssueContentEntity();
                        cEntity.ContentID = row.IsNull("ContentID") ? 0 : (Int64)row["ContentID"];
                        cEntity.CID = row.IsNull("CID") ? 0 : (Int64)row["CID"];
                        cEntity.JournalID = (Int64)row["JournalID"];
                        cEntity.CNumber = row["CNumber"].ToString();
                        cEntity.Title = row["Title"].ToString();
                        cEntity.AuthorID = (Int64)row["AuthorID"];
                        cEntity.Year = row.IsNull("Year") ? 0 : TypeParse.ToInt(row["Year"]);
                        cEntity.Volume = row.IsNull("Volume") ? 0 : TypeParse.ToInt(row["Volume"]);
                        cEntity.Issue = row.IsNull("Issue") ? 0 : TypeParse.ToInt(row["Issue"]);
                        cEntity.AddDate = Convert.ToDateTime(row["AddDate"]);
                        list.Add(cEntity);
                    }
                }
                pager.ItemList = list;
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        # endregion

        /// <summary>
        /// 新增期刊内容
        /// </summary>
        /// <param name="issueContentEntity"></param>
        /// <returns></returns>
        private Int64 AddIssueContent(IssueContentEntity issueContentEntity, DbTransaction trans = null)
        {
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @CID");
            sqlCommandText.Append(", @AuthorID");
            sqlCommandText.Append(", @Year");
            sqlCommandText.Append(", @Volume");
            sqlCommandText.Append(", @Issue");
            sqlCommandText.Append(", @JChannelID");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @EnTitle");
            sqlCommandText.Append(", @Authors");
            sqlCommandText.Append(", @EnAuthors");
            sqlCommandText.Append(", @WorkUnit");
            sqlCommandText.Append(", @EnWorkUnit");
            sqlCommandText.Append(", @Keywords");
            sqlCommandText.Append(", @EnKeywords");
            sqlCommandText.Append(", @CLC");
            sqlCommandText.Append(", @DOI");
            sqlCommandText.Append(", @Abstract");
            sqlCommandText.Append(", @EnAbstract");
            sqlCommandText.Append(", @Reference");
            sqlCommandText.Append(", @Funds");
            sqlCommandText.Append(", @AuthorIntro");
            sqlCommandText.Append(", @StartPageNum");
            sqlCommandText.Append(", @EndPageNum");
            sqlCommandText.Append(", @FilePath");
            sqlCommandText.Append(", @FileKey");
            sqlCommandText.Append(", @FileSize");
            sqlCommandText.Append(", @FileExt");
            sqlCommandText.Append(", @HtmlPath");
            sqlCommandText.Append(", @HtmlSize");
            sqlCommandText.Append(", @HtmlHits");
            sqlCommandText.Append(", @Hits");
            sqlCommandText.Append(", @Downloads");
            sqlCommandText.Append(", @ViewMoney");
            sqlCommandText.Append(", @IsHot");
            sqlCommandText.Append(", @IsCommend");
            sqlCommandText.Append(", @IsTop");
            sqlCommandText.Append(", @SortID");
            sqlCommandText.Append(", @InUser");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.IssueContent ({0},EditDate,AddDate) VALUES ({1},getdate(),getdate());select SCOPE_IDENTITY();", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, issueContentEntity.JournalID);
            db.AddInParameter(cmd, "@CID", DbType.Int64, issueContentEntity.CID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, issueContentEntity.AuthorID);
            db.AddInParameter(cmd, "@Year", DbType.Int32, issueContentEntity.Year);
            db.AddInParameter(cmd, "@Volume", DbType.Int32, issueContentEntity.Volume);
            db.AddInParameter(cmd, "@Issue", DbType.Int32, issueContentEntity.Issue);
            db.AddInParameter(cmd, "@JChannelID", DbType.Int64, issueContentEntity.JChannelID);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, issueContentEntity.Title);
            db.AddInParameter(cmd, "@EnTitle", DbType.AnsiString, issueContentEntity.EnTitle);
            db.AddInParameter(cmd, "@Authors", DbType.AnsiString, issueContentEntity.Authors);
            db.AddInParameter(cmd, "@EnAuthors", DbType.AnsiString, issueContentEntity.EnAuthors);
            db.AddInParameter(cmd, "@WorkUnit", DbType.AnsiString, issueContentEntity.WorkUnit);
            db.AddInParameter(cmd, "@EnWorkUnit", DbType.AnsiString, issueContentEntity.EnWorkUnit);
            db.AddInParameter(cmd, "@Keywords", DbType.AnsiString, issueContentEntity.Keywords);
            db.AddInParameter(cmd, "@EnKeywords", DbType.AnsiString, issueContentEntity.EnKeywords);
            db.AddInParameter(cmd, "@CLC", DbType.AnsiString, issueContentEntity.CLC);
            db.AddInParameter(cmd, "@DOI", DbType.AnsiString, issueContentEntity.DOI);
            db.AddInParameter(cmd, "@Abstract", DbType.AnsiString, issueContentEntity.Abstract);
            db.AddInParameter(cmd, "@EnAbstract", DbType.AnsiString, issueContentEntity.EnAbstract);
            db.AddInParameter(cmd, "@Reference", DbType.AnsiString, issueContentEntity.Reference);
            db.AddInParameter(cmd, "@Funds", DbType.AnsiString, issueContentEntity.Funds);
            db.AddInParameter(cmd, "@AuthorIntro", DbType.AnsiString, issueContentEntity.AuthorIntro);
            db.AddInParameter(cmd, "@StartPageNum", DbType.Int32, issueContentEntity.StartPageNum);
            db.AddInParameter(cmd, "@EndPageNum", DbType.Int32, issueContentEntity.EndPageNum);
            db.AddInParameter(cmd, "@FilePath", DbType.AnsiString, issueContentEntity.FilePath);
            db.AddInParameter(cmd, "@FileKey", DbType.AnsiString, issueContentEntity.FileKey);
            db.AddInParameter(cmd, "@FileSize", DbType.Decimal, issueContentEntity.FileSize);
            db.AddInParameter(cmd, "@FileExt", DbType.AnsiString, issueContentEntity.FileExt);
            db.AddInParameter(cmd, "@HtmlPath", DbType.AnsiString, issueContentEntity.HtmlPath);
            db.AddInParameter(cmd, "@HtmlSize", DbType.Decimal, issueContentEntity.HtmlSize);
            db.AddInParameter(cmd, "@HtmlHits", DbType.Decimal, issueContentEntity.HtmlHits);
            db.AddInParameter(cmd, "@Hits", DbType.Int32, issueContentEntity.Hits);
            db.AddInParameter(cmd, "@Downloads", DbType.Int32, issueContentEntity.Downloads);
            db.AddInParameter(cmd, "@ViewMoney", DbType.Decimal, issueContentEntity.ViewMoney);
            db.AddInParameter(cmd, "@IsHot", DbType.Boolean, issueContentEntity.IsHot);
            db.AddInParameter(cmd, "@IsCommend", DbType.Boolean, issueContentEntity.IsCommend);
            db.AddInParameter(cmd, "@IsTop", DbType.Boolean, issueContentEntity.IsTop);
            db.AddInParameter(cmd, "@SortID", DbType.Int32, issueContentEntity.SortID);
            db.AddInParameter(cmd, "@InUser", DbType.Int64, issueContentEntity.InUser);
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
                    throw new Exception("新增期刊信息失败！");
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑期刊内容
        /// </summary>
        /// <param name="issueContentEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        private bool UpdateIssueContent(IssueContentEntity issueContentEntity, DbTransaction trans = null)
        {
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  ContentID=@ContentID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Year=@Year");
            sqlCommandText.Append(", Volume=@Volume");
            sqlCommandText.Append(", Issue=@Issue");
            sqlCommandText.Append(", JChannelID=@JChannelID");
            sqlCommandText.Append(", Title=@Title");
            sqlCommandText.Append(", EnTitle=@EnTitle");
            sqlCommandText.Append(", Authors=@Authors");
            sqlCommandText.Append(", EnAuthors=@EnAuthors");
            sqlCommandText.Append(", WorkUnit=@WorkUnit");
            sqlCommandText.Append(", EnWorkUnit=@EnWorkUnit");
            sqlCommandText.Append(", Keywords=@Keywords");
            sqlCommandText.Append(", EnKeywords=@EnKeywords");
            sqlCommandText.Append(", CLC=@CLC");
            sqlCommandText.Append(", DOI=@DOI");
            sqlCommandText.Append(", isRegDoi=@isRegDoi");
            sqlCommandText.Append(", Abstract=@Abstract");
            sqlCommandText.Append(", EnAbstract=@EnAbstract");
            sqlCommandText.Append(", Reference=@Reference");
            sqlCommandText.Append(", Funds=@Funds");
            sqlCommandText.Append(", AuthorIntro=@AuthorIntro");
            sqlCommandText.Append(", StartPageNum=@StartPageNum");
            sqlCommandText.Append(", EndPageNum=@EndPageNum");
            sqlCommandText.Append(", FilePath=@FilePath");
            sqlCommandText.Append(", FileKey=@FileKey");
            sqlCommandText.Append(", FileSize=@FileSize");
            sqlCommandText.Append(", FileExt=@FileExt");
            sqlCommandText.Append(", HtmlPath=@HtmlPath");
            sqlCommandText.Append(", HtmlSize=@HtmlSize");
            //sqlCommandText.Append(", Hits=@Hits");
            //sqlCommandText.Append(", Downloads=@Downloads");
            sqlCommandText.Append(", ViewMoney=@ViewMoney");
            sqlCommandText.Append(", IsHot=@IsHot");
            sqlCommandText.Append(", IsCommend=@IsCommend");
            sqlCommandText.Append(", IsTop=@IsTop");
            sqlCommandText.Append(", SortID=@SortID");
            sqlCommandText.Append(", EditUser=@EditUser");
            sqlCommandText.Append(", EditDate=getdate()");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.IssueContent SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@ContentID", DbType.Int64, issueContentEntity.ContentID);
            db.AddInParameter(cmd, "@Year", DbType.Int32, issueContentEntity.Year);
            db.AddInParameter(cmd, "@Volume", DbType.Int32, issueContentEntity.Volume);
            db.AddInParameter(cmd, "@Issue", DbType.Int32, issueContentEntity.Issue);
            db.AddInParameter(cmd, "@JChannelID", DbType.Int64, issueContentEntity.JChannelID);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, issueContentEntity.Title);
            db.AddInParameter(cmd, "@EnTitle", DbType.AnsiString, issueContentEntity.EnTitle);
            db.AddInParameter(cmd, "@Authors", DbType.AnsiString, issueContentEntity.Authors);
            db.AddInParameter(cmd, "@EnAuthors", DbType.AnsiString, issueContentEntity.EnAuthors);
            db.AddInParameter(cmd, "@WorkUnit", DbType.AnsiString, issueContentEntity.WorkUnit);
            db.AddInParameter(cmd, "@EnWorkUnit", DbType.AnsiString, issueContentEntity.EnWorkUnit);
            db.AddInParameter(cmd, "@Keywords", DbType.AnsiString, issueContentEntity.Keywords);
            db.AddInParameter(cmd, "@EnKeywords", DbType.AnsiString, issueContentEntity.EnKeywords);
            db.AddInParameter(cmd, "@CLC", DbType.AnsiString, issueContentEntity.CLC);
            db.AddInParameter(cmd, "@DOI", DbType.AnsiString, issueContentEntity.DOI);
            db.AddInParameter(cmd, "@isRegDoi", DbType.Boolean, issueContentEntity.isRegDoi);
            db.AddInParameter(cmd, "@Abstract", DbType.AnsiString, issueContentEntity.Abstract);
            db.AddInParameter(cmd, "@EnAbstract", DbType.AnsiString, issueContentEntity.EnAbstract);
            db.AddInParameter(cmd, "@Reference", DbType.AnsiString, issueContentEntity.Reference);
            db.AddInParameter(cmd, "@Funds", DbType.AnsiString, issueContentEntity.Funds);
            db.AddInParameter(cmd, "@AuthorIntro", DbType.AnsiString, issueContentEntity.AuthorIntro);
            db.AddInParameter(cmd, "@StartPageNum", DbType.Int32, issueContentEntity.StartPageNum);
            db.AddInParameter(cmd, "@EndPageNum", DbType.Int32, issueContentEntity.EndPageNum);

            db.AddInParameter(cmd, "@FilePath", DbType.AnsiString, issueContentEntity.FilePath);
            db.AddInParameter(cmd, "@FileKey", DbType.AnsiString, issueContentEntity.FileKey);
            db.AddInParameter(cmd, "@FileSize", DbType.Decimal, issueContentEntity.FileSize);
            db.AddInParameter(cmd, "@FileExt", DbType.AnsiString, issueContentEntity.FileExt);

            db.AddInParameter(cmd, "@HtmlPath", DbType.AnsiString, issueContentEntity.HtmlPath);
            db.AddInParameter(cmd, "@HtmlSize", DbType.Decimal, issueContentEntity.HtmlSize);
            //db.AddInParameter(cmd, "@Hits", DbType.Int32, issueContentEntity.Hits);
            //db.AddInParameter(cmd, "@Downloads", DbType.Int32, issueContentEntity.Downloads);
            db.AddInParameter(cmd, "@ViewMoney", DbType.Decimal, issueContentEntity.ViewMoney);
            db.AddInParameter(cmd, "@IsHot", DbType.Boolean, issueContentEntity.IsHot);
            db.AddInParameter(cmd, "@IsCommend", DbType.Boolean, issueContentEntity.IsCommend);
            db.AddInParameter(cmd, "@IsTop", DbType.Boolean, issueContentEntity.IsTop);
            db.AddInParameter(cmd, "@SortID", DbType.Int32, issueContentEntity.SortID);
            db.AddInParameter(cmd, "@EditUser", DbType.Int64, issueContentEntity.EditUser);
            db.AddInParameter(cmd, "@EditDate", DbType.DateTime, issueContentEntity.EditDate);
            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("编辑期刊信息失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取期刊查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetIssueContentFilter(IssueContentQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID=" + query.JournalID);
            if (query.Year != null)
                strFilter.Append(" and Year=").Append(query.Year.Value);
            if (query.Issue != null)
                strFilter.Append(" and Issue=").Append(query.Issue.Value);
            if (query.JChannelID != null)
                strFilter.Append(" and JChannelID=").Append(query.JChannelID.Value);
            query.Title = query.Title.TextFilter();
            if (!string.IsNullOrWhiteSpace(query.Title))
                strFilter.AppendFormat(" and Title like '%{0}%'", WKT.Common.Security.SecurityUtils.SafeSqlString(query.Title));
            query.Authors = query.Authors.TextFilter();
            if (!string.IsNullOrWhiteSpace(query.Authors))
                strFilter.AppendFormat(" and Authors like '%{0}%'", WKT.Common.Security.SecurityUtils.SafeSqlString(query.Authors));
            query.Keywords = query.Keywords.TextFilter();
            if (!string.IsNullOrWhiteSpace(query.Keywords))
                strFilter.AppendFormat(" and Keywords like '%{0}%'", WKT.Common.Security.SecurityUtils.SafeSqlString(query.Keywords));
            query.WorkUnit = query.WorkUnit.TextFilter();
            if (!string.IsNullOrWhiteSpace(query.WorkUnit))
                strFilter.AppendFormat(" and WorkUnit like '%{0}%'", WKT.Common.Security.SecurityUtils.SafeSqlString(query.WorkUnit));
            if(query.contentIDs!=null)
                strFilter.AppendFormat(" and ContentID in ({0})", string.Join(",", query.contentIDs));
            //if (query.isRegDoi != -1)
            //    strFilter.Append(" and isRegDoi=").Append(query.isRegDoi);
            if (query.IsHot)
                strFilter.AppendFormat(" and IsHot=1");
            return strFilter.ToString();
        }

        /// <summary>
        /// 组装期刊信息数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<IssueContentEntity> MakeIssueContentList(IDataReader dr)
        {
            List<IssueContentEntity> list = new List<IssueContentEntity>();
            while (dr.Read())
            {
                list.Add(MakeIssueContent(dr));
            }
            if (!dr.IsClosed)
            {
                dr.Close();
            }
            return list;
        }

        /// <summary>
        /// 组装期刊信息数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private IssueContentEntity MakeIssueContent(IDataReader dr)
        {
            IssueContentEntity model = new IssueContentEntity();
            model.ContentID = (Int64)dr["ContentID"];
            model.JournalID = (Int64)dr["JournalID"];
            if (dr.HasColumn("CID"))
            {
                model.CID = TypeParse.ToLong(dr["CID"]);
            }
            if (dr.HasColumn("AuthorID"))
            {
                model.AuthorID = TypeParse.ToLong(dr["AuthorID"]);
            }
            model.Year = (Int32)dr["Year"];
            model.Volume = (Int32)dr["Volume"];
            model.Issue = (Int32)dr["Issue"];
            model.ChannelName = dr["ChannelName"]==System.DBNull.Value?"":(String)dr["ChannelName"];
            model.JChannelID = (Int64)dr["JChannelID"];
            model.Title = (String)dr["Title"];
            model.EnTitle = (String)dr["EnTitle"];
            model.Authors = (String)dr["Authors"];
            model.EnAuthors = (String)dr["EnAuthors"];
            model.WorkUnit = (String)dr["WorkUnit"];
            model.EnWorkUnit = (String)dr["EnWorkUnit"];
            model.Keywords = (String)dr["Keywords"];
            model.EnKeywords = (String)dr["EnKeywords"];
            model.CLC = dr["CLC"] == System.DBNull.Value ? "" : (String)dr["CLC"];
            model.DOI = dr["DOI"] == System.DBNull.Value ? "" : (String)dr["DOI"];
            model.isRegDoi = (Boolean)dr["isRegDoi"];
            model.Abstract = dr["Abstract"]== System.DBNull.Value?"":(String)dr["Abstract"];
            model.EnAbstract = (String)dr["EnAbstract"];
            model.Reference = (String)dr["Reference"];
            model.Funds = (String)dr["Funds"];
            model.AuthorIntro = (String)dr["AuthorIntro"];
            model.StartPageNum = (Int32)dr["StartPageNum"];
            model.EndPageNum = (Int32)dr["EndPageNum"];
            model.FilePath = dr["FilePath"]==System.DBNull.Value?"": (String)dr["FilePath"];
            model.FileKey = dr["FileKey"]==System.DBNull.Value?"":(String)dr["FileKey"];
            model.FileSize = (Decimal)dr["FileSize"];
            model.FileExt = dr["FileExt"]==System.DBNull.Value?"":(String)dr["FileExt"];

            model.HtmlPath = dr["HtmlPath"]==System.DBNull.Value?"":(String)dr["HtmlPath"];
            model.HtmlSize = (Decimal)dr["HtmlSize"];
            model.HtmlHits = (Int32)dr["HtmlHits"];
            model.Hits = (Int32)dr["Hits"];
            model.Downloads = (Int32)dr["Downloads"];
            model.ViewMoney = (Decimal)dr["ViewMoney"];
            model.IsHot = (Boolean)dr["IsHot"];
            model.IsCommend = (Boolean)dr["IsCommend"];
            model.IsTop = (Boolean)dr["IsTop"];
            model.SortID = (Int32)dr["SortID"];
            model.InUser = (Int64)dr["InUser"];
            model.EditUser = (Int64)dr["EditUser"];
            model.EditDate = (DateTime)dr["EditDate"];
            model.AddDate = (DateTime)dr["AddDate"];
            return model;
        }
        #endregion

        #region DOI注册
        /// <summary>
        /// 新增DOI注册日志
        /// </summary>
        /// <param name="doiRegLogEntity"></param>
        /// <returns></returns>
        public bool AddDoiRegLog(DoiRegLogEntity doiRegLogEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @DoiFileName");
            sqlCommandText.Append(", @State");
            sqlCommandText.Append(", @Year");
            sqlCommandText.Append(", @Issue");
            sqlCommandText.Append(", @AdminID");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.DoiRegLog ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, doiRegLogEntity.JournalID);
            db.AddInParameter(cmd, "@DoiFileName", DbType.String, doiRegLogEntity.DoiFileName);
            db.AddInParameter(cmd, "@State", DbType.String, doiRegLogEntity.State);
            db.AddInParameter(cmd, "@Year", DbType.Int32, doiRegLogEntity.Year);
            db.AddInParameter(cmd, "@Issue", DbType.Int32, doiRegLogEntity.Issue);
            db.AddInParameter(cmd, "@AdminID", DbType.Int64, doiRegLogEntity.AdminID);

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

        public bool UpdateDoiRegLog(DoiRegLogEntity doiRegLogEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append(" PKID=@PKID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" State=@State");
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.DoiRegLog SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));
            db.AddInParameter(cmd, "@PKID", DbType.Int64, doiRegLogEntity.PKID);
            db.AddInParameter(cmd, "@State", DbType.String, doiRegLogEntity.State);

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
        /// 删除DOI注册日志
        /// </summary>
        /// <param name="doiRegLogQuery"></param>
        /// <returns></returns>
        public bool DelDoiRegLog(long[] PKID)
        {
            if (PKID == null || PKID.Length < 1)
                return false;
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    string strSql = "Delete dbo.DoiRegLog where PKID";
                    if (PKID.Length == 0)
                        strSql += " = " + PKID[0];
                    else
                        strSql += string.Format(" in ({0})", string.Join(",", PKID));
                    DbCommand cmd = db.GetSqlStringCommand(strSql);
                    if (db.ExecuteNonQuery(cmd, trans) < 1)
                        throw new Exception("删除期刊表信息失败！");
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
        //获取DOI注册日志实体
        public DoiRegLogEntity GetDoiRegLog(DoiRegLogQuery query)
        {
            DoiRegLogEntity doiRegLogEntity = null;
            string strSql = "SELECT TOP 1 * FROM dbo.DoiRegLog WITH(NOLOCK) WHERE PKID=@PKID AND JournalID=@JournalID";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@PKID", DbType.Int64, query.PKID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                    doiRegLogEntity = MakeDoiRegLog(dr);
                dr.Close();
            }
            return doiRegLogEntity;
        }
        //获取DOI注册日志分页数据
        public Pager<DoiRegLogEntity> GetDoiRegLogPageList(DoiRegLogQuery query)
        {
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY PKID) AS ROW_ID FROM dbo.DoiRegLog with(nolock)"
                , sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.DoiRegLog with(nolock)";
            string whereSQL = GetDoiRegLogFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<DoiRegLogEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeDoiRegLogList);
        }
        //获取DOI注册日志列表
        public IList<DoiRegLogEntity> GetDoiRegLogList(DoiRegLogQuery query)
        {
            string strSql = "SELECT * FROM dbo.DoiRegLog with(nolock)";
            string whereSQL = GetDoiRegLogFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " ORDER BY AddDate DESC";
            return db.GetList<DoiRegLogEntity>(strSql, MakeDoiRegLogList);
        }
        /// <summary>
        /// 获取DOI注册日志查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetDoiRegLogFilter(DoiRegLogQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID=" + query.JournalID);
            if (query.PKID != 0)
                strFilter.Append(" and PKID=").Append(query.PKID);
            if (query.Year != 0)
                strFilter.Append(" and Year=").Append(query.Year);
            if (query.Issue != 0)
                strFilter.Append(" and Issue=").Append(query.Issue);
            return strFilter.ToString();
        } 
        #endregion
        
        #region 组装DOI注册日志信息数据
        private List<DoiRegLogEntity> MakeDoiRegLogList(IDataReader dr)
        {
            List<DoiRegLogEntity> list = new List<DoiRegLogEntity>();
            while (dr.Read())
            {
                list.Add(MakeDoiRegLog(dr));
            }
            if (!dr.IsClosed)
            {
                dr.Close();
            }
            return list;
        }
        private DoiRegLogEntity MakeDoiRegLog(IDataReader dr)
        {
            DoiRegLogEntity model = new DoiRegLogEntity();
            model.PKID = (Int64)dr["PKID"];
            model.JournalID = (Int64)dr["JournalID"];
            model.DoiFileName = dr["DoiFileName"] == System.DBNull.Value ? "" : (String)dr["DoiFileName"];
            model.State = dr["State"] == System.DBNull.Value ? "" : (String)dr["State"];
            model.Year = (Int32)dr["Year"];
            model.Issue = (Int32)dr["Issue"];
            model.AdminID = (Int64)dr["AdminID"];
            model.AddDate = (DateTime)dr["AddDate"];
            return model;
        } 
        #endregion

        #region 期刊参考文献
        /// <summary>
        /// 新增期刊参考文献
        /// </summary>
        /// <param name="issueReferenceEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool AddIssueReference(IssueReferenceEntity issueReferenceEntity, DbTransaction trans = null)
        {
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @ContentID");
            sqlCommandText.Append(", @JournalID");
            sqlCommandText.Append(", @Title");
            sqlCommandText.Append(", @RefUrl");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.IssueReference ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@ContentID", DbType.Int64, issueReferenceEntity.ContentID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, issueReferenceEntity.JournalID);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, issueReferenceEntity.Title.TextFilter());
            db.AddInParameter(cmd, "@RefUrl", DbType.AnsiString, issueReferenceEntity.RefUrl.TextFilter());
            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("新增期刊参考文献失败！");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑期刊参考文献
        /// </summary>
        /// <param name="issueReferenceEntity"></param>
        /// <returns></returns>
        public bool UpdateIssueReference(IssueReferenceEntity issueReferenceEntity, DbTransaction trans = null)
        {
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  ReferenceID=@ReferenceID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Title=@Title");
            sqlCommandText.Append(", RefUrl=@RefUrl");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.IssueReference SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@ReferenceID", DbType.Int64, issueReferenceEntity.ReferenceID);
            db.AddInParameter(cmd, "@Title", DbType.AnsiString, issueReferenceEntity.Title.TextFilter());
            db.AddInParameter(cmd, "@RefUrl", DbType.AnsiString, issueReferenceEntity.RefUrl.TextFilter());

            try
            {
                bool result = false;
                if (trans == null)
                    result = db.ExecuteNonQuery(cmd) > 0;
                else
                    result = db.ExecuteNonQuery(cmd, trans) > 0;
                if (!result)
                    throw new Exception("编辑期刊参考文献失败！");
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
        public IssueReferenceEntity GetIssueReference(Int64 ReferenceID)
        {
            IssueReferenceEntity model = null;
            string strSql = "SELECT TOP 1 * FROM dbo.IssueReference WITH(NOLOCK) WHERE ReferenceID=@ReferenceID";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@ReferenceID", DbType.Int64, ReferenceID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                    model = MakeIssueReferenceInfo(dr);
                dr.Close();
            }
            return model;
        }

        /// <summary>
        /// 根据期刊编号删除参考文献
        /// </summary>
        /// <param name="ContentID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelIssueReferenceByContentID(Int64[] ContentID, DbTransaction trans = null, Int64[] ReferenceID = null)
        {
            if (ContentID == null || ContentID.Length < 1)
                return false;
            string strSql = "Delete dbo.IssueReference WHERE ContentID";
            if (ContentID.Length == 0)
                strSql += " = " + ContentID[0];
            else
                strSql += string.Format(" in ({0}) ", string.Join(",", ContentID));
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
        /// 删除期刊参考文献
        /// </summary>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        public bool DelIssueReference(Int64[] ReferenceID)
        {
            if (ReferenceID == null || ReferenceID.Length < 1)
                return false;
            string strSql = "Delete dbo.IssueReference WHERE ReferenceID";
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
        public Pager<IssueReferenceEntity> GetIssueReferencePageList(IssueReferenceQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderStr))
                query.OrderStr = " ReferenceID DESC";
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY " + query.OrderStr + ") AS ROW_ID FROM dbo.IssueReference with(nolock)"
                , sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.IssueReference with(nolock)";
            string whereSQL = GetIssueReferenceFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<IssueReferenceEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeIssueReferenceList);
        }

        /// <summary>
        /// 获取参考文献数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<IssueReferenceEntity> GetIssueReferenceList(IssueReferenceQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderStr))
                query.OrderStr = " ReferenceID DESC";
            string strSql = "SELECT * FROM dbo.IssueReference with(nolock)";
            string whereSQL = GetIssueReferenceFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " ORDER BY " + query.OrderStr;
            return db.GetList<IssueReferenceEntity>(strSql, MakeIssueReferenceList);
        }

        /// <summary>
        /// 获取参考文献查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetIssueReferenceFilter(IssueReferenceQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID=" + query.JournalID);
            if (query.ContentID != null)
            {
                strFilter.AppendFormat(" and ContentID={0}", query.ContentID.Value);
            }
            return strFilter.ToString();
        }

        /// <summary>
        /// 组装参考文献数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<IssueReferenceEntity> MakeIssueReferenceList(IDataReader dr)
        {
            List<IssueReferenceEntity> list = new List<IssueReferenceEntity>();
            while (dr.Read())
            {
                list.Add(MakeIssueReferenceInfo(dr));
            }
            return list;
        }

        /// <summary>
        /// 组装参考文献数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private IssueReferenceEntity MakeIssueReferenceInfo(IDataReader dr)
        {
            IssueReferenceEntity issueReferenceEntity = new IssueReferenceEntity();
            issueReferenceEntity.ReferenceID = (Int64)dr["ReferenceID"];
            issueReferenceEntity.ContentID = (Int64)dr["ContentID"];
            issueReferenceEntity.JournalID = (Int64)dr["JournalID"];
            issueReferenceEntity.Title = Convert.IsDBNull(dr["Title"]) ? null : (String)dr["Title"];
            issueReferenceEntity.RefUrl = Convert.IsDBNull(dr["RefUrl"]) ? null : (String)dr["RefUrl"];
            issueReferenceEntity.AddDate = (DateTime)dr["AddDate"];
            return issueReferenceEntity;
        }
        #endregion

        #region 期刊订阅
        /// <summary>
        /// 新增期刊订阅
        /// </summary>
        /// <param name="issueSubscribeEntity"></param>
        /// <returns></returns>
        public bool AddIssueSubscribe(IssueSubscribeEntity issueSubscribeEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @Subscriber");
            sqlCommandText.Append(", @Mobile");
            sqlCommandText.Append(", @Tel");
            sqlCommandText.Append(", @Fax");
            sqlCommandText.Append(", @Email");
            sqlCommandText.Append(", @Address");
            sqlCommandText.Append(", @ZipCode");
            sqlCommandText.Append(", @ContactUser");
            sqlCommandText.Append(", @SubscribeInfo");
            sqlCommandText.Append(", @IsInvoice");
            sqlCommandText.Append(", @InvoiceHead");
            sqlCommandText.Append(", @Note");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.IssueSubscribe ({0},SubscribeDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, issueSubscribeEntity.JournalID);
            db.AddInParameter(cmd, "@Subscriber", DbType.AnsiString, issueSubscribeEntity.Subscriber.TextFilter());
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, issueSubscribeEntity.Mobile);
            db.AddInParameter(cmd, "@Tel", DbType.AnsiString, issueSubscribeEntity.Tel.TextFilter());
            db.AddInParameter(cmd, "@Fax", DbType.AnsiString, issueSubscribeEntity.Fax.TextFilter());
            db.AddInParameter(cmd, "@Email", DbType.AnsiString, issueSubscribeEntity.Email.TextFilter());
            db.AddInParameter(cmd, "@Address", DbType.AnsiString, issueSubscribeEntity.Address.TextFilter());
            db.AddInParameter(cmd, "@ZipCode", DbType.AnsiString, issueSubscribeEntity.ZipCode.TextFilter());
            db.AddInParameter(cmd, "@ContactUser", DbType.AnsiString, issueSubscribeEntity.ContactUser.TextFilter());
            db.AddInParameter(cmd, "@SubscribeInfo", DbType.AnsiString, issueSubscribeEntity.SubscribeInfo.TextFilter());
            db.AddInParameter(cmd, "@IsInvoice", DbType.Boolean, issueSubscribeEntity.IsInvoice);
            db.AddInParameter(cmd, "@InvoiceHead", DbType.AnsiString, issueSubscribeEntity.InvoiceHead.TextFilter());
            db.AddInParameter(cmd, "@Note", DbType.AnsiString, issueSubscribeEntity.Note.TextFilter());
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
        /// 编辑期刊订阅
        /// </summary>
        /// <param name="issueSubscribeEntity"></param>
        /// <returns></returns>
        public bool UpdateIssueSubscribe(IssueSubscribeEntity issueSubscribeEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  SubscribeID=@SubscribeID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Subscriber=@Subscriber");
            sqlCommandText.Append(", Mobile=@Mobile");
            sqlCommandText.Append(", Tel=@Tel");
            sqlCommandText.Append(", Fax=@Fax");
            sqlCommandText.Append(", Email=@Email");
            sqlCommandText.Append(", Address=@Address");
            sqlCommandText.Append(", ZipCode=@ZipCode");
            sqlCommandText.Append(", ContactUser=@ContactUser");
            sqlCommandText.Append(", SubscribeInfo=@SubscribeInfo");
            sqlCommandText.Append(", IsInvoice=@IsInvoice");
            sqlCommandText.Append(", InvoiceHead=@InvoiceHead");
            sqlCommandText.Append(", Note=@Note");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.IssueSubscribe SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@SubscribeID", DbType.Int64, issueSubscribeEntity.SubscribeID);
            db.AddInParameter(cmd, "@Subscriber", DbType.AnsiString, issueSubscribeEntity.Subscriber);
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, issueSubscribeEntity.Mobile);
            db.AddInParameter(cmd, "@Tel", DbType.AnsiString, issueSubscribeEntity.Tel);
            db.AddInParameter(cmd, "@Fax", DbType.AnsiString, issueSubscribeEntity.Fax);
            db.AddInParameter(cmd, "@Email", DbType.AnsiString, issueSubscribeEntity.Email);
            db.AddInParameter(cmd, "@Address", DbType.AnsiString, issueSubscribeEntity.Address);
            db.AddInParameter(cmd, "@ZipCode", DbType.AnsiString, issueSubscribeEntity.ZipCode);
            db.AddInParameter(cmd, "@ContactUser", DbType.AnsiString, issueSubscribeEntity.ContactUser);
            db.AddInParameter(cmd, "@SubscribeInfo", DbType.AnsiString, issueSubscribeEntity.SubscribeInfo);
            db.AddInParameter(cmd, "@IsInvoice", DbType.Boolean, issueSubscribeEntity.IsInvoice);
            db.AddInParameter(cmd, "@InvoiceHead", DbType.AnsiString, issueSubscribeEntity.InvoiceHead);
            db.AddInParameter(cmd, "@Note", DbType.AnsiString, issueSubscribeEntity.Note);

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
        /// 获取期刊订阅
        /// </summary>
        /// <param name="subscribeID"></param>
        /// <returns></returns>
        public IssueSubscribeEntity GetIssueSubscribe(Int64 subscribeID)
        {
            IssueSubscribeEntity issueSubscribeEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  SubscribeID,JournalID,Subscriber,Mobile,Tel,Fax,Email,Address,ZipCode,ContactUser,SubscribeInfo,IsInvoice,InvoiceHead,Note,SubscribeDate FROM dbo.IssueSubscribe WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  SubscribeID=@SubscribeID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@SubscribeID", DbType.Int64, subscribeID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                    issueSubscribeEntity = MakeIssueSubscribe(dr);
                dr.Close();
            }
            return issueSubscribeEntity;
        }

        /// <summary>
        /// 删除期刊参考文献
        /// </summary>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        public bool DelIssueSubscribe(Int64[] subscribeID)
        {
            if (subscribeID == null || subscribeID.Length < 1)
                return false;
            string strSql = "Delete dbo.IssueSubscribe WHERE subscribeID";
            if (subscribeID.Length == 0)
                strSql += " = " + subscribeID[0];
            else
                strSql += string.Format(" in ({0}) ", string.Join(",", subscribeID));
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(cmd) > 0;
        }

        /// <summary>
        /// 获取期刊订阅分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueSubscribeEntity> GetIssueSubscribePageList(IssueSubscribeQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderStr))
                query.OrderStr = " SubscribeID DESC";
            string strSql = "SELECT *,ROW_NUMBER() OVER(ORDER BY " + query.OrderStr + ") AS ROW_ID FROM dbo.IssueSubscribe with(nolock)"
                , sumStr = "SELECT RecordCount=COUNT(1) FROM dbo.IssueSubscribe with(nolock)";
            string whereSQL = GetIssueSubscribeFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
                sumStr += " WHERE " + whereSQL;
            }
            return db.GetPageList<IssueSubscribeEntity>(string.Format(SQL_Page_Select, strSql, query.StartIndex, query.EndIndex)
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , MakeIssueSubscribeList);
        }

        /// <summary>
        /// 获取期刊订阅数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<IssueSubscribeEntity> GetIssueSubscribeList(IssueSubscribeQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderStr))
                query.OrderStr = " SubscribeID DESC";
            string strSql = "SELECT * FROM dbo.IssueSubscribe with(nolock)";
            string whereSQL = GetIssueSubscribeFilter(query);
            if (!string.IsNullOrWhiteSpace(whereSQL))
            {
                strSql += " WHERE " + whereSQL;
            }
            strSql += " ORDER BY " + query.OrderStr;
            return db.GetList<IssueSubscribeEntity>(strSql, MakeIssueSubscribeList);
        }

        /// <summary>
        /// 获取期刊订阅查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetIssueSubscribeFilter(IssueSubscribeQuery query)
        {
            StringBuilder strFilter = new StringBuilder(" JournalID=" + query.JournalID);
            return strFilter.ToString();
        }

        /// <summary>
        /// 组装期刊订阅数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<IssueSubscribeEntity> MakeIssueSubscribeList(IDataReader dr)
        {
            List<IssueSubscribeEntity> list = new List<IssueSubscribeEntity>();
            while (dr.Read())
            {
                list.Add(MakeIssueSubscribe(dr));
            }
            return list;
        }

        /// <summary>
        /// 组装期刊订阅数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private IssueSubscribeEntity MakeIssueSubscribe(IDataReader dr)
        {
            IssueSubscribeEntity issueSubscribeEntity = new IssueSubscribeEntity();
            issueSubscribeEntity.SubscribeID = (Int64)dr["SubscribeID"];
            issueSubscribeEntity.JournalID = (Int64)dr["JournalID"];
            issueSubscribeEntity.Subscriber = (String)dr["Subscriber"];
            issueSubscribeEntity.Mobile = (String)dr["Mobile"];
            issueSubscribeEntity.Tel = (String)dr["Tel"];
            issueSubscribeEntity.Fax = (String)dr["Fax"];
            issueSubscribeEntity.Email = (String)dr["Email"];
            issueSubscribeEntity.Address = (String)dr["Address"];
            issueSubscribeEntity.ZipCode = (String)dr["ZipCode"];
            issueSubscribeEntity.ContactUser = (String)dr["ContactUser"];
            issueSubscribeEntity.SubscribeInfo = (String)dr["SubscribeInfo"];
            issueSubscribeEntity.IsInvoice = (Boolean)dr["IsInvoice"];
            issueSubscribeEntity.InvoiceHead = (String)dr["InvoiceHead"];
            issueSubscribeEntity.Note = (String)dr["Note"];
            issueSubscribeEntity.SubscribeDate = (DateTime)dr["SubscribeDate"];
            return issueSubscribeEntity;
        }
        #endregion

        # region 获取当前期刊最新的年、卷、期、当期封面图片

        /// <summary>
        /// 获取当前期刊最新的年、卷、期、当期封面图片
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IssueSiteEntity GetCurIssueInfo(IssueSetQuery query)
        {
            IssueSiteEntity issueInfoEntity = null;
            string sql = @"SELECT TOP 1 y.[Year],y.Volume,issue.Issue,issue.TitlePhoto
                            FROM dbo.YearVolume y WITH(NOLOCK),(SELECT TOP 1 i.JournalID,i.Issue,i.TitlePhoto FROM dbo.IssueSet i WITH(NOLOCK) WHERE i.JournalID=@JournalID AND i.Status=1 AND i.TitlePhoto<>'' AND i.TitlePhoto IS NOT NULL ORDER BY i.Issue DESC) issue
                            WHERE y.JournalID=@JournalID AND y.Status=1 ORDER BY [Year] DESC";

            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                issueInfoEntity = new IssueSiteEntity();
                if (dr.Read())
                {
                    issueInfoEntity.Year = TypeParse.ToInt(dr["Year"], DateTime.Now.Year);
                    issueInfoEntity.Volume = TypeParse.ToInt(dr["Volume"], 0);
                    issueInfoEntity.Issue = TypeParse.ToInt(dr["Issue"], 0);
                    issueInfoEntity.TitlePhoto = dr.IsDBNull(dr.GetOrdinal("TitlePhoto")) ? "" : dr["TitlePhoto"].ToString();
                }
                dr.Close();
            }
            return issueInfoEntity;
        }

        # endregion

        #region 下载次数统计
        /// <summary>
        /// 获取期刊下载次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(IssueDownLogQuery query)
        {
            //            string tableSql = @"SELECT a.JournalID,a.ContentID,c.CNumber,b.Title,c.AuthorID,d.RealName,d.LoginName,d.Mobile,a.DownLoadCount FROM (
            //                                  SELECT ContentID,JournalID,COUNT(1) as DownLoadCount 
            //                                  FROM dbo.IssueDownLog with(nolock) 
            //                                  WHERE JournalID={0} GROUP BY ContentID,JournalID
            //                              ) a INNER JOIN dbo.IssueContent b with(nolock) ON a.JournalID=b.JournalID and a.ContentID=b.ContentID
            //                              INNER JOIN dbo.ContributionInfo c with(nolock) ON a.JournalID=b.JournalID and b.CID=c.CID
            //                              INNER JOIN dbo.AuthorInfo d with(nolock) ON a.JournalID=d.JournalID and c.AuthorID=d.AuthorID";
            string tableSql = "select Title,A.Year,A.DownLoadCount,Authors,A.ContentID,A.[Month],B.JournalID from   dbo.IssueContent as B right join (select ContentID,COUNT(ContentID) as DownLoadCount,Year,Month from dbo.IssueDownLog group by ContentID,Year,Month) as A  on A.ContentID=B.ContentID where JournalID={0}";
            tableSql = string.Format(tableSql, query.JournalID);
            query.Title = SecurityUtils.SafeSqlString(query.Title);
            if (!string.IsNullOrWhiteSpace(query.Title))
                // tableSql += string.Format(" and b.Title like '%{0}%'", query.Title);
                tableSql += string.Format(" and  Title like '%{0}%'", query.Title);
            query.RealName = SecurityUtils.SafeSqlString(query.RealName);
            if (!string.IsNullOrWhiteSpace(query.RealName))
                tableSql += string.Format(" and Authors like '%{0}%'", query.RealName);

            if (query.Year != 0)
                tableSql += string.Format(" and Year='{0}'", query.Year);
            if (query.Month != 0)
                tableSql += string.Format(" and Month='{0}'", query.Month);

            string strSql = string.Format(SQL_Page_Select_ROWNumber, tableSql, query.StartIndex, query.EndIndex, " DownLoadCount desc "), sumStr = string.Empty;
            if (!query.IsReport)
                sumStr = string.Format("SELECT RecordCount=COUNT(1) FROM ({0}) t", tableSql);
            return db.GetPageList<IssueDownLogEntity>(strSql
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , (dr) =>
                {
                    List<IssueDownLogEntity> list = new List<IssueDownLogEntity>();
                    IssueDownLogEntity model = null;
                    while (dr.Read())
                    {
                        model = new IssueDownLogEntity();
                        model.JournalID = dr.GetDrValue<Int64>("JournalID");
                        model.ContentID = dr.GetDrValue<Int64>("ContentID");
                        //  model.CNumber = dr.GetDrValue<String>("CNumber");
                        model.Title = dr.GetDrValue<String>("Title");
                        // model.AuthorID = dr.GetDrValue<Int64>("AuthorID");
                        model.RealName = dr.GetDrValue<String>("Authors");
                        // model.LoginName = dr.GetDrValue<String>("LoginName");
                        // model.Mobile = dr.GetDrValue<String>("Mobile");
                        model.DownLoadCount = TypeParse.ToLong(dr["DownLoadCount"]);
                        list.Add(model);
                    }
                    dr.Close();
                    return list;
                });
        }

        /// <summary>
        /// 获取期刊下载明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogDetailPageList(IssueDownLogQuery query)
        {
            string tableSql = @"SELECT a.JournalID,a.ContentID,a.DownLogID,a.Year,a.Month,a.Daytime,b.Title,a.AuthorID,(case when d.RealName is null then '游客' else d.RealName end) as RealName,(case when d.LoginName is null then '无' else d.LoginName end) as LoginName,(case when d.Mobile is null then '无' else d.Mobile end) as Mobile,a.IP,a.AddDate 
                              FROM dbo.IssueDownLog a with(nolock)
                              INNER JOIN dbo.IssueContent b with(nolock) ON a.JournalID=b.JournalID and a.ContentID=b.ContentID
                              LEFT JOIN dbo.AuthorInfo d with(nolock) ON a.JournalID=d.JournalID and a.AuthorID=d.AuthorID
                              WHERE a.JournalID=" + query.JournalID;
            if (query.ContentID != null)
                tableSql += " and a.ContentID=" + query.ContentID.Value;
            query.Title = SecurityUtils.SafeSqlString(query.Title);
            if (!string.IsNullOrWhiteSpace(query.Title))
                tableSql += string.Format(" and b.Title like '%{0}%'", query.Title);
            query.RealName = SecurityUtils.SafeSqlString(query.RealName);

            if (query.Year != 0)
                tableSql += string.Format(" and a.Year='{0}'", query.Year);
            if (query.Month != 0)
                tableSql += string.Format(" and a.Month='{0}'", query.Month);

            if (!string.IsNullOrWhiteSpace(query.RealName))
                tableSql += string.Format(" and d.RealName like '%{0}%'", query.RealName);
            if (!string.IsNullOrWhiteSpace(query.IP))
                tableSql += string.Format(" and a.IP = '{0}'", query.IP);
            string strSql = string.Format(SQL_Page_Select_ROWNumber, tableSql, query.StartIndex, query.EndIndex, " AddDate desc"), sumStr = string.Empty;
            if (!query.IsReport)
                sumStr = string.Format("SELECT RecordCount=COUNT(1) FROM ({0}) t", tableSql);
            return db.GetPageList<IssueDownLogEntity>(strSql
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , (dr) =>
                {
                    List<IssueDownLogEntity> list = new List<IssueDownLogEntity>();
                    IssueDownLogEntity model = null;
                    while (dr.Read())
                    {
                        model = new IssueDownLogEntity();
                        model.DownLogID = dr.GetDrValue<Int64>("DownLogID");
                        model.Year = dr.GetDrValue<Int32>("Year");
                        model.Month = dr.GetDrValue<Int32>("Month");
                        model.JournalID = dr.GetDrValue<Int64>("JournalID");
                        model.ContentID = dr.GetDrValue<Int64>("ContentID");
                        model.Title = dr.GetDrValue<String>("Title");
                        model.AuthorID = dr.GetDrValue<Int64>("AuthorID");
                        model.RealName = dr.GetDrValue<String>("RealName");
                        model.LoginName = dr.GetDrValue<String>("LoginName");
                        model.Mobile = dr.GetDrValue<String>("Mobile");
                        model.IP = dr.GetDrValue<String>("IP");
                        model.AddDate = dr.GetDrValue<DateTime>("AddDate");
                        list.Add(model);
                    }
                    dr.Close();
                    return list;
                });
        }
        #endregion

        #region 浏览次数统计
        /// <summary>
        /// 获取期刊浏览次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueViewLogEntity> GetIssueViewLogPageList(IssueViewLogQuery query)
        {
            //            string tableSql = @"SELECT a.JournalID,a.ContentID,c.CNumber,b.Title,c.AuthorID,d.RealName,d.LoginName,d.Mobile,a.DownLoadCount FROM (
            //                                  SELECT ContentID,JournalID,COUNT(1) as DownLoadCount 
            //                                  FROM dbo.IssueDownLog with(nolock) 
            //                                  WHERE JournalID={0} GROUP BY ContentID,JournalID
            //                              ) a INNER JOIN dbo.IssueContent b with(nolock) ON a.JournalID=b.JournalID and a.ContentID=b.ContentID
            //                              INNER JOIN dbo.ContributionInfo c with(nolock) ON a.JournalID=b.JournalID and b.CID=c.CID
            //                              INNER JOIN dbo.AuthorInfo d with(nolock) ON a.JournalID=d.JournalID and c.AuthorID=d.AuthorID";
            string tableSql = "select Title,A.Year,A.ViewCount,A.ContentID,A.[Month],Authors,B.JournalID from   dbo.IssueContent as B right join (select ContentID,COUNT(ContentID) as ViewCount,Year,Month from dbo.IssueViewLog group by ContentID,Year,Month) as A  on A.ContentID=B.ContentID where JournalID={0}";
            tableSql = string.Format(tableSql, query.JournalID);
            query.Title = SecurityUtils.SafeSqlString(query.Title);
            if (!string.IsNullOrWhiteSpace(query.Title))
                // tableSql += string.Format(" and b.Title like '%{0}%'", query.Title);
                tableSql += string.Format(" and  Title like '%{0}%'", query.Title);
            query.RealName = SecurityUtils.SafeSqlString(query.RealName);
            if (!string.IsNullOrWhiteSpace(query.RealName))
                tableSql += string.Format(" and Authors like '%{0}%'", query.RealName);
            if (query.Year!=0)
                tableSql += string.Format(" and Year='{0}'", query.Year);

            if (query.Month != 0)
                tableSql += string.Format(" and Month='{0}'", query.Month);

            //tableSql += string.Format(" and d.RealName like '%{0}%'", query.RealName);
            string strSql = string.Format(SQL_Page_Select_ROWNumber, tableSql, query.StartIndex, query.EndIndex, " ViewCount desc "), sumStr = string.Empty;
            if (!query.IsReport)
                sumStr = string.Format("SELECT RecordCount=COUNT(1) FROM ({0}) t", tableSql);
            return db.GetPageList<IssueViewLogEntity>(strSql
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , (dr) =>
                {
                    List<IssueViewLogEntity> list = new List<IssueViewLogEntity>();
                    IssueViewLogEntity model = null;
                    while (dr.Read())
                    {
                        model = new IssueViewLogEntity();
                        model.JournalID = dr.GetDrValue<Int64>("JournalID");
                        model.ContentID = dr.GetDrValue<Int64>("ContentID");
                        model.Title = dr.GetDrValue<String>("Title");
                        model.RealName = dr.GetDrValue<String>("Authors");
                        model.ViewCount = TypeParse.ToLong(dr["ViewCount"]);
                        list.Add(model);
                    }
                    dr.Close();
                    return list;
                });
        }

        /// <summary>
        /// 获取期刊浏览明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueViewLogEntity> GetIssueViewLogDetailPageList(IssueViewLogQuery query)
        {
            string tableSql = @"SELECT a.JournalID,a.ContentID,a.ViewLogID,a.Year,a.Month,a.Daytime,b.Title,a.AuthorID,(case when d.RealName is null then '游客' else d.RealName end) as RealName,(case when d.LoginName is null then '无' else d.LoginName end) as LoginName,(case when d.Mobile is null then '无' else d.Mobile end) as Mobile,a.IP,a.AddDate 
                              FROM dbo.IssueViewLog a with(nolock)
                              INNER JOIN dbo.IssueContent b with(nolock) ON a.JournalID=b.JournalID and a.ContentID=b.ContentID
                              LEFT JOIN dbo.AuthorInfo d with(nolock) ON a.JournalID=d.JournalID and a.AuthorID=d.AuthorID
                              WHERE a.JournalID=" + query.JournalID;
            if (query.ContentID != null)
                tableSql += " and a.ContentID=" + query.ContentID.Value;
            query.Title = SecurityUtils.SafeSqlString(query.Title);
            if (!string.IsNullOrWhiteSpace(query.Title))
                tableSql += string.Format(" and b.Title like '%{0}%'", query.Title);
            query.RealName = SecurityUtils.SafeSqlString(query.RealName);

            if (query.Year != 0)
                tableSql += string.Format(" and a.Year='{0}'", query.Year);
            if (query.Month != 0)
                tableSql += string.Format(" and a.Month='{0}'", query.Month);

            if (!string.IsNullOrWhiteSpace(query.RealName))
                tableSql += string.Format(" and d.RealName like '%{0}%'", query.RealName);
            string strSql = string.Format(SQL_Page_Select_ROWNumber, tableSql, query.StartIndex, query.EndIndex, " AddDate desc"), sumStr = string.Empty;
            if (!query.IsReport)
                sumStr = string.Format("SELECT RecordCount=COUNT(1) FROM ({0}) t", tableSql);
            return db.GetPageList<IssueViewLogEntity>(strSql
                , sumStr
                , query.CurrentPage, query.PageSize
                , (dr, pager) =>
                {
                    pager.TotalRecords = TypeParse.ToLong(dr["RecordCount"]);
                }
                , (dr) =>
                {
                    List<IssueViewLogEntity> list = new List<IssueViewLogEntity>();
                    IssueViewLogEntity model = null;
                    while (dr.Read())
                    {
                        model = new IssueViewLogEntity();
                        model.ViewLogID = dr.GetDrValue<Int64>("ViewLogID");
                        model.Year = dr.GetDrValue<Int32>("Year");
                        model.Month = dr.GetDrValue<Int32>("Month");
                        model.JournalID = dr.GetDrValue<Int64>("JournalID");
                        model.ContentID = dr.GetDrValue<Int64>("ContentID");
                        model.Title = dr.GetDrValue<String>("Title");
                        model.AuthorID = dr.GetDrValue<Int64>("AuthorID");
                        model.RealName = dr.GetDrValue<String>("RealName");
                        model.LoginName = dr.GetDrValue<String>("LoginName");
                        model.Mobile = dr.GetDrValue<String>("Mobile");
                        model.IP = dr.GetDrValue<String>("IP");
                        model.AddDate = dr.GetDrValue<DateTime>("AddDate");
                        list.Add(model);
                    }
                    dr.Close();
                    return list;
                });
        }
        #endregion


        /// <summary>
        /// 判断列是否存在
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        static bool readerExists(SqlDataReader dr, string columnName)
        {

            dr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" +

            columnName + "'";

            return (dr.GetSchemaTable().DefaultView.Count > 0);

        }
    }
}
