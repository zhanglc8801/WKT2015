using System;
using System.Linq;
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
using HCMS.Utilities;

namespace WKT.DataAccess
{
    public partial class ExpertApplyDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db = null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public ExpertApplyDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if (db == null)
                throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static ExpertApplyDataAccess _instance = new ExpertApplyDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static ExpertApplyDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }
        # region 组装SQL条件

        /// <summary>
        /// 将查询实体转换为Where语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Where语句，不包含Where</returns>
        /// </summary>
        public string ExpertApplyQueryToSQLWhere(ExpertApplyLogQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JournalID = " + query.JournalID);
            if (query.PKID>0)
            {
                sbWhere.Append(" AND PKID = " + query.PKID);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(query.LoginName))
                {
                    sbWhere.Append(" AND LoginName = '").Append(WKT.Common.Security.SecurityUtils.SafeSqlString(query.LoginName)).Append("'");
                }
                //if (!string.IsNullOrWhiteSpace(query.RealName))
                //{
                //    sbWhere.Append(" AND RealName like '%").Append(WKT.Common.Security.SecurityUtils.SafeSqlString(query.RealName)).Append("%'");
                //}
                //if (query.Status != null)
                //{
                //    sbWhere.Append(" AND Status = '").Append(query.Status).Append("'");
                //}
                //if (query.ActionUser != null)
                //{
                //    sbWhere.Append(" AND ActionUser = '").Append(query.ActionUser).Append("'");
                //}
            }
            return sbWhere.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string AuthorInfoQueryToSQLOrder(AuthorInfoQuery query)
        {
            return " AuthorID DESC";
        }

        #endregion 组装SQL条件

        #region 将查询实体转换为Order语句
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string ExpertApplyQueryToSQLOrder(ExpertApplyLogQuery query)
        {
            if (query.OrderByStr != null)
                return query.OrderByStr;
            else
                return " PKID DESC";
        } 
        #endregion

        #region 根据数据组装一个对象
        /// <summary>
        /// 根据数据组装一个对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public ExpertApplyLogEntity MakeExpertApplyInfo(IDataReader dr)
        {
            ExpertApplyLogEntity expertApplyEntity = null;
            if (dr.Read())
            {
                expertApplyEntity = new ExpertApplyLogEntity();
                expertApplyEntity.PKID = (Int64)dr["PKID"];
                expertApplyEntity.JournalID = (Int64)dr["JournalID"];
                expertApplyEntity.LoginName = (String)dr["LoginName"];
                expertApplyEntity.RealName = (String)dr["RealName"];
                expertApplyEntity.Gender = (Byte)dr["Gender"];
                expertApplyEntity.Mobile = (String)dr["Mobile"];
                expertApplyEntity.Tel = (String)dr["Tel"];
                expertApplyEntity.Education = (Int32)dr["Education"];
                expertApplyEntity.JobTitle = (Int32)dr["JobTitle"];
                expertApplyEntity.WorkUnit = (String)dr["WorkUnit"];
                expertApplyEntity.Address = (String)dr["Address"];
                expertApplyEntity.ZipCode = (String)dr["ZipCode"];
                expertApplyEntity.Status = (Byte)dr["Status"];
                expertApplyEntity.ResearchTopics = (String)dr["ResearchTopics"];
                expertApplyEntity.Remark = (String)dr["Remark"];
                expertApplyEntity.ActionUser = (Int64)dr["ActionUser"];
                expertApplyEntity.AddDate = (DateTime)dr["AddDate"];
                expertApplyEntity.ActionDate = (DateTime)dr["ActionDate"];
            }
            dr.Close();
            return expertApplyEntity;
        }

        /// <summary>
        /// 根据数据组装一个对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public ExpertApplyLogEntity MakeExpertApplyInfo(DataRow dr)
        {
            ExpertApplyLogEntity expertApplyEntity = null;
            if (dr != null)
            {
                expertApplyEntity = new ExpertApplyLogEntity();
                expertApplyEntity.PKID = (Int64)dr["PKID"];
                expertApplyEntity.JournalID = (Int64)dr["JournalID"];
                expertApplyEntity.LoginName = (String)dr["LoginName"];
                expertApplyEntity.RealName = (String)dr["RealName"];
                expertApplyEntity.Gender = (Byte)dr["Gender"];
                expertApplyEntity.Mobile = (String)dr["Mobile"];
                expertApplyEntity.Tel = (String)dr["Tel"];
                expertApplyEntity.Education = (Int32)dr["Education"];
                expertApplyEntity.JobTitle = (Int32)dr["JobTitle"];
                expertApplyEntity.WorkUnit = (String)dr["WorkUnit"];
                expertApplyEntity.Address = (String)dr["Address"];
                expertApplyEntity.ZipCode = (String)dr["ZipCode"];
                expertApplyEntity.Status = (Byte)dr["Status"];
                expertApplyEntity.ResearchTopics = (String)dr["ResearchTopics"];
                expertApplyEntity.Remark = (String)dr["Remark"];
                expertApplyEntity.ActionUser = (Int64)dr["ActionUser"];
                expertApplyEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return expertApplyEntity;
        }
        #endregion

        #region 根据数据组装一组对象数据

        public List<ExpertApplyLogEntity> MakeExpertApplyInfoList(IDataReader dr)
        {
            List<ExpertApplyLogEntity> list = new List<ExpertApplyLogEntity>();
            while (dr.Read())
            {
                ExpertApplyLogEntity expertApplyEntity = new ExpertApplyLogEntity();
                expertApplyEntity.PKID = (Int64)dr["PKID"];
                expertApplyEntity.JournalID = (Int64)dr["JournalID"];
                expertApplyEntity.LoginName = (String)dr["LoginName"];
                expertApplyEntity.RealName = (String)dr["RealName"];
                expertApplyEntity.Gender = (Byte)dr["Gender"];
                expertApplyEntity.Mobile = (String)dr["Mobile"];
                expertApplyEntity.Tel = (String)dr["Tel"];
                expertApplyEntity.Education = (Int32)dr["Education"];
                expertApplyEntity.JobTitle = (Int32)dr["JobTitle"];
                expertApplyEntity.WorkUnit = (String)dr["WorkUnit"];
                expertApplyEntity.Address = (String)dr["Address"];
                expertApplyEntity.ZipCode = (String)dr["ZipCode"];
                expertApplyEntity.Status = (Byte)dr["Status"];
                expertApplyEntity.ResearchTopics = (String)dr["ResearchTopics"];
                expertApplyEntity.Remark = (String)dr["Remark"];
                expertApplyEntity.ActionUser = (Int64)dr["ActionUser"];
                expertApplyEntity.AddDate = (DateTime)dr["AddDate"];
                expertApplyEntity.ActionDate = (DateTime)dr["ActionDate"];
                list.Add(expertApplyEntity);

            }
            dr.Close();
            return list;
        }
        public List<ExpertApplyLogEntity> MakeExpertApplyInfoList(DataTable dt)
        {
            List<ExpertApplyLogEntity> list = new List<ExpertApplyLogEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ExpertApplyLogEntity expertApplyEntity = MakeExpertApplyInfo(dt.Rows[i]);
                    list.Add(expertApplyEntity);
                }
            }
            return list;
        }

        #endregion

        #region 获取一个实体对象

        public ExpertApplyLogEntity GetExpertApplyInfo(Int64 PKID)
        {
            ExpertApplyLogEntity expertApplyEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 * FROM dbo.ExpertApplyLog WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  PKID=@PKID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@PKID", DbType.Int64, PKID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                expertApplyEntity = MakeExpertApplyInfo(dr);
            }
            return expertApplyEntity;
        }

        /// <summary>
        /// 获取专家申请信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExpertApplyLogEntity GetExpertApplyInfo(ExpertApplyLogQuery query)
        {
            ExpertApplyLogEntity expertApplyEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 * FROM dbo.ExpertApplyLog WITH(NOLOCK)");
            string whereSQL = ExpertApplyQueryToSQLWhere(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                expertApplyEntity = MakeExpertApplyInfo(dr);
            }
            return expertApplyEntity;
        }

        #endregion

        #region 根据条件获取所有实体对象

        public List<ExpertApplyLogEntity> GetExpertApplyInfoList()
        {
            List<ExpertApplyLogEntity> expertApplyEntity = new List<ExpertApplyLogEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT * FROM dbo.ExpertApplyLog WITH(NOLOCK)");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                expertApplyEntity = MakeExpertApplyInfoList(dr);
            }
            return expertApplyEntity;
        }

        public List<ExpertApplyLogEntity> GetExpertApplyInfoList(ExpertApplyLogQuery query)
        {
            List<ExpertApplyLogEntity> list = new List<ExpertApplyLogEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT * FROM dbo.ExpertApplyLog WITH(NOLOCK)");
            string whereSQL = ExpertApplyQueryToSQLWhere(query);
            string orderBy = ExpertApplyQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeExpertApplyInfoList(dr);
            }
            return list;
        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">查询对象</param>
        /// <returns>Pager<AuthorInfoEntity></returns>
        public Pager<ExpertApplyLogEntity> GetExpertApplyInfoPageList(ExpertApplyLogQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("ExpertApplyLog", "PKID,JournalID,LoginName,RealName,Gender,Birthday,Education,JobTitle,Tel,Mobile,WorkUnit,Address,ReviewDomain,ResearchTopics,BankID,BankType,ZipCode,Remark,Status,AddDate,ActionUser,ActionDate", query.OrderByStr, "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<ExpertApplyLogEntity> pager = new Pager<ExpertApplyLogEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeExpertApplyInfoList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }
        #endregion

        #region 提交专家申请（保存新对象到存储媒介中）

        public bool SubmitApply(ExpertApplyLogEntity expertApplyEntity)
        {
            bool flag = false;
            DbCommand cmd = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @LoginName");
            sqlCommandText.Append(", @RealName");
            sqlCommandText.Append(", @Gender");
            sqlCommandText.Append(", @Birthday");
            sqlCommandText.Append(", @Education");
            sqlCommandText.Append(", @JobTitle");
            sqlCommandText.Append(", @Tel");
            sqlCommandText.Append(", @Mobile");
            sqlCommandText.Append(", @WorkUnit");
            sqlCommandText.Append(", @Address");
            sqlCommandText.Append(", @ReviewDomain");
            sqlCommandText.Append(", @ResearchTopics");
            sqlCommandText.Append(", @BankID");
            sqlCommandText.Append(", @BankType");
            sqlCommandText.Append(", @ZipCode");
            sqlCommandText.Append(", @Remark");
            sqlCommandText.Append(", @Status");
            sqlCommandText.Append(", @AddDate");
            sqlCommandText.Append(", @ActionUser");
            sqlCommandText.Append(", @ActionDate");

            cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.ExpertApplyLog ({0}) VALUES ({1})", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, expertApplyEntity.JournalID);
            db.AddInParameter(cmd, "@LoginName", DbType.String, expertApplyEntity.LoginName);
            db.AddInParameter(cmd, "@RealName", DbType.String, expertApplyEntity.RealName);
            db.AddInParameter(cmd, "@Gender", DbType.Int16, expertApplyEntity.Gender);
            db.AddInParameter(cmd, "@Birthday", DbType.DateTime, expertApplyEntity.Birthday);
            db.AddInParameter(cmd, "@Education", DbType.Int16, expertApplyEntity.Education);
            db.AddInParameter(cmd, "@JobTitle", DbType.Int16, expertApplyEntity.JobTitle);
            db.AddInParameter(cmd, "@Tel", DbType.String, expertApplyEntity.Tel);
            db.AddInParameter(cmd, "@Mobile", DbType.String, expertApplyEntity.Mobile);
            db.AddInParameter(cmd, "@WorkUnit", DbType.String, expertApplyEntity.WorkUnit);
            db.AddInParameter(cmd, "@Address", DbType.String, expertApplyEntity.Address);
            db.AddInParameter(cmd, "@ReviewDomain", DbType.String, expertApplyEntity.ReviewDomain);
            db.AddInParameter(cmd, "@ResearchTopics", DbType.String, expertApplyEntity.ResearchTopics);
            db.AddInParameter(cmd, "@BankID", DbType.String, expertApplyEntity.BankID);
            db.AddInParameter(cmd, "@BankType", DbType.String, expertApplyEntity.BankType);

            db.AddInParameter(cmd, "@ZipCode", DbType.String, expertApplyEntity.ZipCode);
            db.AddInParameter(cmd, "@Remark", DbType.String, expertApplyEntity.Remark);
            db.AddInParameter(cmd, "@Status", DbType.Byte, expertApplyEntity.Status);
            db.AddInParameter(cmd, "@AddDate", DbType.DateTime, DateTime.Now);
            db.AddInParameter(cmd, "@ActionUser", DbType.Int64, expertApplyEntity.ActionUser);
            db.AddInParameter(cmd, "@ActionDate", DbType.DateTime, Convert.ToDateTime("1970-01-01"));

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

        #region 更新专家申请数据

        /// <summary>
        /// 更新专家申请数据
        /// </summary>
        /// <param name="AdminID"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        public bool UpdateApply(ExpertApplyLogEntity expertApplyEntity)
        {
            bool flag = false;
            string sql = "UPDATE dbo.ExpertApplyLog SET Status=@Status,ActionUser=@ActionUser,ActionDate=@ActionDate WHERE JournalID=@JournalID AND LoginName=@LoginName ";
            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, expertApplyEntity.JournalID);
            db.AddInParameter(cmd, "@LoginName", DbType.AnsiString, expertApplyEntity.LoginName);       
            db.AddInParameter(cmd, "@Status", DbType.Byte, expertApplyEntity.Status);
            db.AddInParameter(cmd, "@ActionUser", DbType.Int64, expertApplyEntity.ActionUser);
            db.AddInParameter(cmd, "@ActionDate", DbType.DateTime, DateTime.Now);

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

        #region 删除专家申请数据

        public bool DelApply(long PKID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.ExpertApplyLog");
            sqlCommandText.Append(" WHERE  PKID=@PKID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            db.AddInParameter(cmd, "@PKID", DbType.Int64, PKID);

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


    }
}
