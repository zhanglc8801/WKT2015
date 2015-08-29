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
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class AuthorInfoDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db = null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public AuthorInfoDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if (db == null)
                throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static AuthorInfoDataAccess _instance = new AuthorInfoDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static AuthorInfoDataAccess Instance
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
        public string AuthorInfoQueryToSQLWhere(AuthorInfoQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JournalID = " + query.JournalID);
            if (query.AuthorID != null)
            {
                sbWhere.Append(" AND AuthorID = " + query.AuthorID);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(query.LoginName))
                {
                    sbWhere.Append(" AND LoginName = '").Append(WKT.Common.Security.SecurityUtils.SafeSqlString(query.LoginName)).Append("'");
                }
                if (!string.IsNullOrWhiteSpace(query.Pwd))
                {
                    sbWhere.Append(" AND Pwd = '").Append(WKT.Common.Security.MD5Handle.Encrypt(query.Pwd)).Append("'");
                }
                if (query.Status != null)
                {
                    sbWhere.Append(" AND Status = '").Append(query.Status.Value).Append("'");
                }
                if (query.GroupID != null)
                {
                    sbWhere.Append(" AND GroupID = '").Append(query.GroupID.Value).Append("'");
                }
                if (query.AuthorIDs != null && query.AuthorIDs.Length > 0)
                {
                    if (query.AuthorIDs.Length == 1)
                        sbWhere.AppendFormat(" AND AuthorID = {0}", query.AuthorIDs[0]);
                    else
                        sbWhere.AppendFormat(" AND AuthorID in ({0})", string.Join(",", query.AuthorIDs));
                }
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

        #region 获取一个实体对象

        public AuthorInfoEntity GetAuthorInfo(Int64 authorID)
        {
            AuthorInfoEntity authorInfoEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  AuthorID,JournalID,LoginName,Pwd,RealName,Mobile,LoginIP,LoginCount,LoginDate,GroupID,Status,AddDate FROM dbo.AuthorInfo WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  AuthorID=@AuthorID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, authorID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                authorInfoEntity = MakeAuthorInfo(dr);
            }
            return authorInfoEntity;
        }

        /// <summary>
        /// 获取作者信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public AuthorInfoEntity GetAuthorInfo(AuthorInfoQuery query)
        {
            AuthorInfoEntity authorInfoEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  AuthorID,JournalID,LoginName,Pwd,RealName,Mobile,LoginIP,LoginCount,LoginDate,GroupID,Status,AddDate FROM dbo.AuthorInfo WITH(NOLOCK)");
            string whereSQL = AuthorInfoQueryToSQLWhere(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                authorInfoEntity = MakeAuthorInfo(dr);
            }
            return authorInfoEntity;
        }

        /// <summary>
        /// 获取编辑部成员信息
        /// </summary>
        /// <param name="authorID"></param>
        /// <returns></returns>
        public AuthorInfoEntity GetMemberInfo(AuthorInfoQuery authorQuery)
        {
            AuthorInfoEntity authorInfoEntity = null;
            string sql = @"SELECT TOP 1 a.AuthorID,a.JournalID,LoginName,Pwd,RealName,Mobile,LoginIP,LoginCount,LoginDate,a.GroupID,Status,a.AddDate,
                            ISNULL(r.RoleID,0) AS RoleID
                            FROM dbo.AuthorInfo a WITH(NOLOCK) LEFT JOIN dbo.RoleAuthor r WITH(NOLOCK) ON a.AuthorID=r.AuthorID
                            WHERE  a.AuthorID=@AuthorID AND a.JournalID=@JournalID";

            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, authorQuery.AuthorID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, authorQuery.JournalID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                authorInfoEntity = new AuthorInfoEntity();
                if (dr.Read())
                {
                    authorInfoEntity = new AuthorInfoEntity();
                    authorInfoEntity.AuthorID = (Int64)dr["AuthorID"];
                    authorInfoEntity.JournalID = (Int64)dr["JournalID"];
                    authorInfoEntity.LoginName = (String)dr["LoginName"];
                    authorInfoEntity.Pwd = (String)dr["Pwd"];
                    authorInfoEntity.RealName = (String)dr["RealName"];
                    authorInfoEntity.Mobile = (String)dr["Mobile"];
                    authorInfoEntity.Status = (Byte)dr["Status"];
                    authorInfoEntity.RoleID = (Int64)dr["RoleID"];
                }
                dr.Close();
            }
            return authorInfoEntity;
        }

        #endregion

        #region 根据条件获取所有实体对象

        public List<AuthorInfoEntity> GetAuthorInfoList()
        {
            List<AuthorInfoEntity> authorInfoEntity = new List<AuthorInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  AuthorID,JournalID,LoginName,Pwd,RealName,Mobile,LoginIP,LoginCount,LoginDate,Status,GroupID,AddDate FROM dbo.AuthorInfo WITH(NOLOCK)");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                authorInfoEntity = MakeAuthorInfoList(dr);
            }
            return authorInfoEntity;
        }

        public List<AuthorInfoEntity> GetAuthorInfoList(AuthorInfoQuery query)
        {
            List<AuthorInfoEntity> list = new List<AuthorInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT AuthorID,JournalID,LoginName,Pwd,RealName,Mobile,LoginIP,LoginCount,LoginDate,Status,GroupID,AddDate FROM dbo.AuthorInfo WITH(NOLOCK)");
            string whereSQL = AuthorInfoQueryToSQLWhere(query);
            string orderBy = AuthorInfoQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeAuthorInfoList(dr);
            }
            return list;
        }

        /// <summary>
        /// 根据角色获取作者列表
        /// </summary>
        /// <param name="authorInfoQuery">AuthorInfoQuery查询实体对象</param>
        /// <returns>List<AuthorInfoEntity></returns>
        public List<AuthorInfoEntity> GetAuthorInfoListByRole(AuthorInfoQuery authorInfoQuery)
        {
            List<AuthorInfoEntity> list = new List<AuthorInfoEntity>();
            string sql = @"SELECT a.AuthorID,a.JournalID,LoginName,Pwd,RealName,a.Mobile,LoginIP,LoginCount,LoginDate,Status,a.GroupID,a.AddDate ,b.RoleID,c.RoleName
                            FROM dbo.AuthorInfo a WITH(NOLOCK),dbo.RoleAuthor b WITH(NOLOCK),dbo.RoleInfo as c
                            WHERE a.JournalID=@JournalID AND a.GroupID=@GroupID and b.RoleID=c.RoleID and a.GroupID=c.GroupID  AND a.AuthorID=b.AuthorID AND a.JournalID=b.JournalID ";
            if (authorInfoQuery.RoleID >0)
            {
                sql += " and b.RoleID=@RoleID";
            }
            DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, authorInfoQuery.JournalID);
            db.AddInParameter(cmd, "@GroupID", DbType.Byte, authorInfoQuery.GroupID);
            if (authorInfoQuery.RoleID > 0)
            {
                db.AddInParameter(cmd, "@RoleID", DbType.Int64, authorInfoQuery.RoleID.Value);

            }
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeAuthorInfoList(dr);
            }
            return list;
        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<AuthorInfoEntity></returns>
        public Pager<AuthorInfoEntity> GetAuthorInfoPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("AuthorInfo", "AuthorID,JournalID,LoginName,Pwd,RealName,Mobile,LoginIP,LoginCount,LoginDate,Status,GroupID,AddDate", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<AuthorInfoEntity> pager = new Pager<AuthorInfoEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeAuthorInfoList(ds.Tables[0]);
                foreach (AuthorInfoEntity item in pager.ItemList)
                {
                    item.Pwd = "";
                }
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<AuthorInfoEntity> GetAuthorInfoPageList(QueryBase query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("AuthorInfo", "AuthorID,JournalID,LoginName,Pwd,RealName,Mobile,LoginIP,LoginCount,LoginDate,Status,GroupID,AddDate", " AuthorID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<AuthorInfoEntity> pager = new Pager<AuthorInfoEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeAuthorInfoList(ds.Tables[0]);
                foreach (AuthorInfoEntity item in pager.ItemList)
                {
                    item.Pwd = "";
                }
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<AuthorInfoEntity> GetAuthorInfoPageList(AuthorInfoQuery query)
        {
            int recordCount = 0;
            string whereSQL = AuthorInfoQueryToSQLWhere(query);
            string orderBy = AuthorInfoQueryToSQLOrder(query);
            DataSet ds = db.GetPagingData("AuthorInfo", "AuthorID,JournalID,LoginName,Pwd,RealName,Mobile,LoginIP,LoginCount,LoginDate,Status,GroupID,AddDate", orderBy, whereSQL, query.CurrentPage, query.PageSize, out recordCount);
            Pager<AuthorInfoEntity> pager = new Pager<AuthorInfoEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeAuthorInfoList(ds.Tables[0]);
                foreach (AuthorInfoEntity item in pager.ItemList)
                {
                    item.Pwd = "";
                }
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        /// <summary>
        /// 获取编辑部成员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<AuthorInfoEntity> GetMemberInfoPageList(AuthorInfoQuery query)
        {
            int recordCount = 0;
            List<SqlParameter> listParameters = new List<SqlParameter>();
            string sql = "";
            StringBuilder sbSQL = new StringBuilder("SELECT a.AuthorID,a.JournalID,LoginName,Pwd,RealName,Mobile,LoginIP,LoginCount,LoginDate,Status,a.GroupID,a.AddDate,(SELECT ( SELECT ri.RoleName + ',' FROM dbo.RoleInfo ri WITH(NOLOCK) WHERE ISNULL(ra.RoleID,0)=ri.RoleID AND ra.AuthorID=a.AuthorID FOR XML PATH(''))) AS RoleList FROM dbo.AuthorInfo a WITH(NOLOCK) LEFT JOIN dbo.RoleAuthor ra ON a.AuthorID=ra.AuthorID AND a.JournalID=ra.JournalID WHERE a.JournalID=@JournalID");
            StringBuilder sbNoRole = new StringBuilder("SELECT a.AuthorID,a.JournalID,LoginName,Pwd,RealName,Mobile,LoginIP,LoginCount,LoginDate,Status,a.GroupID,a.AddDate,(SELECT ( SELECT ri.RoleName + ',' FROM dbo.RoleInfo ri WITH(NOLOCK) INNER JOIN dbo.RoleAuthor ra WITH(NOLOCK) ON ri.RoleID=ra.RoleID AND ri.JournalID=ra.JournalID WHERE ra.AuthorID=a.AuthorID FOR XML PATH(''))) AS RoleList FROM dbo.AuthorInfo a WITH(NOLOCK) WHERE a.JournalID=@JournalID");
            SqlParameter pJournalID = new SqlParameter("@JournalID", SqlDbType.BigInt);
            pJournalID.Value = query.JournalID;
            listParameters.Add(pJournalID);
            if (!string.IsNullOrEmpty(query.LoginName))
            {
                // SqlParameter p2 = new SqlParameter("@LoginName", SqlDbType.VarChar, 100);
                //  p2.Value = query.LoginName;
                // listParameters.Add(p2);
                //sbSQL.Append(" AND LoginName=@LoginName");
                sbNoRole.Append(" AND LoginName like '%" + query.LoginName + "%'");
            }
            if (!string.IsNullOrEmpty(query.RealName))
            {
                sbSQL.Append(" AND RealName LIKE '%").Append(WKT.Common.Security.SecurityUtils.SafeSqlString(query.RealName)).Append("%'");
                sbNoRole.Append(" AND RealName LIKE '%").Append(WKT.Common.Security.SecurityUtils.SafeSqlString(query.RealName)).Append("%'");
            }
            if (query.GroupID != null)
            {
                SqlParameter pGroupID = new SqlParameter("@GroupID", SqlDbType.TinyInt);
                pGroupID.Value = query.GroupID.Value;
                listParameters.Add(pGroupID);
                sbSQL.Append(" AND a.GroupID=@GroupID");
                sbNoRole.Append(" AND a.GroupID=@GroupID");
            }
            if (query.Status != null)
            {
                SqlParameter pStatus = new SqlParameter("@Status", SqlDbType.TinyInt);
                pStatus.Value = query.Status.Value;
                listParameters.Add(pStatus);
                sbSQL.Append(" AND a.Status=@Status");
                sbNoRole.Append(" AND a.Status=@Status");
            }

            if (query.RoleID != null)
            {
                SqlParameter pRoleID = new SqlParameter("@RoleID", SqlDbType.BigInt);
                pRoleID.Value = query.RoleID;
                listParameters.Add(pRoleID);
                sbSQL.Append(" AND ra.RoleID=@RoleID");
                sql = sbSQL.ToString();
            }
            else
            {
                sql = sbNoRole.ToString();
            }

            DataSet ds = db.PageingQuery(query.CurrentPage, query.PageSize, sql, "a.AuthorID DESC", listParameters.ToArray(), ref recordCount);
            Pager<AuthorInfoEntity> pager = new Pager<AuthorInfoEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                List<AuthorInfoEntity> list = new List<AuthorInfoEntity>();
                if (ds != null)
                {
                    AuthorInfoEntity authorInfoEntity = null;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        authorInfoEntity = new AuthorInfoEntity();
                        authorInfoEntity.AuthorID = (Int64)row["AuthorID"];
                        authorInfoEntity.JournalID = (Int64)row["JournalID"];
                        authorInfoEntity.LoginName = (String)row["LoginName"];
                        authorInfoEntity.RealName = row["RealName"]==System.DBNull.Value?"无姓名":(String)row["RealName"];
                        authorInfoEntity.Mobile = row["Mobile"]==System.DBNull.Value?"":(String)row["Mobile"];
                        authorInfoEntity.LoginIP = row["LoginIP"]==System.DBNull.Value?"127.0.0.1":(String)row["LoginIP"];
                        authorInfoEntity.LoginCount = (Int32)row["LoginCount"];
                        authorInfoEntity.LoginDate = (DateTime)row["LoginDate"];
                        authorInfoEntity.GroupID = (Byte)row["GroupID"];
                        authorInfoEntity.Status = (Byte)row["Status"];
                        authorInfoEntity.RoleName = row.IsNull("RoleList") ? "" : row["RoleList"].ToString();
                        if (!string.IsNullOrEmpty(authorInfoEntity.RoleName))
                        {
                            authorInfoEntity.RoleName = authorInfoEntity.RoleName.Remove(authorInfoEntity.RoleName.Length - 1);
                        }
                        authorInfoEntity.AddDate = (DateTime)row["AddDate"];
                        list.Add(authorInfoEntity);
                    }
                }
                pager.ItemList = list;
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        /// <summary>
        /// 获取专家列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<AuthorInfoEntity> GetExpertPageList(AuthorInfoQuery query)
        {
            int recordCount = 0;
            string sql = "";
//            string sql = @"
//                        SELECT ai.AuthorID,ai.Mobile,ai.LoginName,ai.RealName,
//                        (SELECT ( SELECT dv.ValueText + ',' 
//	                        FROM dbo.ExpertGroupMap e WITH(NOLOCK) 
//	                        INNER JOIN dbo.DictValue dv WITH(NOLOCK) ON e.ExpertGroupID=dv.ValueID AND ai.AuthorID=e.AuthorID AND ai.JournalID=e.JournalID
//	                        WHERE dv.DictKey='ExpertGroupMap' AND dv.JournalID=ai.JournalID FOR XML PATH(''))) AS ExpertList,
//	                        (SELECT COUNT(DISTINCT fi.CID) FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.RecUserID=ai.AuthorID AND fi.JournalID=ai.JournalID AND fi.Status=0) AS AuditCount,
//                            (SELECT COUNT(DISTINCT fi.CID) FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.RecUserID=ai.AuthorID AND fi.JournalID=ai.JournalID AND fi.Status=1) AS AuditedCount
//                        FROM dbo.AuthorInfo ai WITH(NOLOCK) INNER JOIN dbo.AuthorDetail ad WITH(NOLOCK) ON ai.AuthorID=ad.AuthorID AND ai.JournalID=ad.JournalID
//                        WHERE ai.JournalID=@JournalID AND ai.GroupID=3 {0}
//                        UNION
//                        SELECT ai.AuthorID,ai.Mobile,ai.LoginName,ai.RealName,
//                        (SELECT ( SELECT dv.ValueText + ',' 
//	                        FROM dbo.ExpertGroupMap e WITH(NOLOCK) 
//	                        INNER JOIN dbo.DictValue dv WITH(NOLOCK) ON e.ExpertGroupID=dv.ValueID AND ai.AuthorID=e.AuthorID AND ai.JournalID=e.JournalID
//	                        WHERE dv.DictKey='ExpertGroupMap' AND dv.JournalID=ai.JournalID FOR XML PATH(''))) AS ExpertList,
//	                        (SELECT COUNT(DISTINCT fi.CID) FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.RecUserID=ai.AuthorID AND fi.JournalID=ai.JournalID AND fi.Status=0) AS AuditCount,
//                            (SELECT COUNT(DISTINCT fi.CID) FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.RecUserID=ai.AuthorID AND fi.JournalID=ai.JournalID AND fi.Status=1) AS AuditedCount
//                        FROM dbo.AuthorInfo ai WITH(NOLOCK) INNER JOIN dbo.RoleAuthor ra WITH(NOLOCK) ON ai.AuthorID=ra.AuthorID AND ai.JournalID=ra.JournalID AND ra.RoleID=3
//                                 INNER JOIN dbo.AuthorDetail ad WITH(NOLOCK) ON ai.AuthorID=ad.AuthorID AND ai.JournalID=ad.JournalID
//                        WHERE ai.JournalID=@JournalID {0}";
            //2014-1-15 文海峰

            if (query.IsSelEnExpert == true)
            {
                sql = @"
                        SELECT ai.AuthorID,ai.Mobile,ai.LoginName,ai.RealName,ad.ResearchTopics,
	                        (SELECT COUNT(DISTINCT fi.CID) FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.RecUserID=ai.AuthorID AND fi.JournalID=ai.JournalID AND fi.Status=0) AS AuditCount,
                            (SELECT COUNT(DISTINCT fi.CID) FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.RecUserID=ai.AuthorID AND fi.JournalID=ai.JournalID AND fi.Status=1) AS AuditedCount
                        FROM dbo.AuthorInfo ai WITH(NOLOCK) INNER JOIN dbo.AuthorDetail ad WITH(NOLOCK) ON ai.AuthorID=ad.AuthorID AND ai.JournalID=ad.JournalID
                        WHERE ai.JournalID=@JournalID AND ai.GroupID=4 {0}
                        UNION
                        SELECT ai.AuthorID,ai.Mobile,ai.LoginName,ai.RealName,ad.ResearchTopics,
	                        (SELECT COUNT(DISTINCT fi.CID) FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.RecUserID=ai.AuthorID AND fi.JournalID=ai.JournalID AND fi.Status=0) AS AuditCount,
                            (SELECT COUNT(DISTINCT fi.CID) FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.RecUserID=ai.AuthorID AND fi.JournalID=ai.JournalID AND fi.Status=1) AS AuditedCount
                        FROM dbo.AuthorInfo ai WITH(NOLOCK) INNER JOIN dbo.RoleAuthor ra WITH(NOLOCK) ON ai.AuthorID=ra.AuthorID AND ai.JournalID=ra.JournalID AND ra.RoleID=4
                                 INNER JOIN dbo.AuthorDetail ad WITH(NOLOCK) ON ai.AuthorID=ad.AuthorID AND ai.JournalID=ad.JournalID
                        WHERE ai.JournalID=@JournalID {0}";
            }
            else
            {
                sql = @"
                        SELECT ai.AuthorID,ai.Mobile,ai.LoginName,ai.RealName,ad.ResearchTopics,
	                        (SELECT COUNT(DISTINCT fi.CID) FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.RecUserID=ai.AuthorID AND fi.JournalID=ai.JournalID AND fi.Status=0) AS AuditCount,
                            (SELECT COUNT(DISTINCT fi.CID) FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.RecUserID=ai.AuthorID AND fi.JournalID=ai.JournalID AND fi.Status=1) AS AuditedCount
                        FROM dbo.AuthorInfo ai WITH(NOLOCK) INNER JOIN dbo.AuthorDetail ad WITH(NOLOCK) ON ai.AuthorID=ad.AuthorID AND ai.JournalID=ad.JournalID
                        WHERE ai.JournalID=@JournalID AND ai.GroupID=3 {0}
                        UNION
                        SELECT ai.AuthorID,ai.Mobile,ai.LoginName,ai.RealName,ad.ResearchTopics,
	                        (SELECT COUNT(DISTINCT fi.CID) FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.RecUserID=ai.AuthorID AND fi.JournalID=ai.JournalID AND fi.Status=0) AS AuditCount,
                            (SELECT COUNT(DISTINCT fi.CID) FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.RecUserID=ai.AuthorID AND fi.JournalID=ai.JournalID AND fi.Status=1) AS AuditedCount
                        FROM dbo.AuthorInfo ai WITH(NOLOCK) INNER JOIN dbo.RoleAuthor ra WITH(NOLOCK) ON ai.AuthorID=ra.AuthorID AND ai.JournalID=ra.JournalID AND ra.RoleID=3
                                 INNER JOIN dbo.AuthorDetail ad WITH(NOLOCK) ON ai.AuthorID=ad.AuthorID AND ai.JournalID=ad.JournalID
                        WHERE ai.JournalID=@JournalID {0}";
            }
            
            List<SqlParameter> listParameters = new List<SqlParameter>();
            SqlParameter pJournalID = new SqlParameter("@JournalID", SqlDbType.BigInt);
            pJournalID.Value = query.JournalID;
            listParameters.Add(pJournalID);
            string whereCondition = "";
            if (!string.IsNullOrEmpty(query.LoginName))
            {
                SqlParameter p2 = new SqlParameter("@LoginName", SqlDbType.VarChar, 100);
                p2.Value = query.LoginName;
                listParameters.Add(p2);
                whereCondition += " AND ai.LoginName like '%" + p2.Value + "%'";
            }

            //研究方向
            if (!string.IsNullOrEmpty(query.ResearchTopics))
            {
                whereCondition += " AND ad.ResearchTopics  LIKE '%" + WKT.Common.Security.SecurityUtils.SafeSqlString(query.ResearchTopics) + "%'";
            }
            if (!string.IsNullOrEmpty(query.RealName))
            {
                whereCondition += " AND RealName LIKE '%" + WKT.Common.Security.SecurityUtils.SafeSqlString(query.RealName) + "%'";
            }
            if (!string.IsNullOrEmpty(query.Remark))
            {
                whereCondition += " AND ad.Remark LIKE '%" + WKT.Common.Security.SecurityUtils.SafeSqlString(query.Remark) + "%'";
            }
            if (query.ExpertGroupID != null && query.ExpertGroupID.Value > 0)
            {
                whereCondition += " AND EXISTS(SELECT TOP 1 1 FROM dbo.ExpertGroupMap e WITH(NOLOCK),dbo.DictValue d WITH(NOLOCK)WHERE e.AuthorID=ai.AuthorID AND e.JournalID=ai.JournalID AND e.ExpertGroupID=d.ValueID AND e.JournalID=d.JournalID AND d.DictKey='ExpertGroupMap' AND d.ValueID=" + query.ExpertGroupID.Value + ")";
            }
            string execSQL = string.Format(sql, whereCondition);
            DbCommand cmd = db.GetSqlStringCommand(execSQL);
            foreach (SqlParameter pItem in listParameters)
            {
                db.AddInParameter(cmd, pItem.ParameterName, pItem.DbType, pItem.Value);
            }
            DataSet ds = db.ExecuteDataSet(cmd);
            Pager<AuthorInfoEntity> pager = new Pager<AuthorInfoEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                List<AuthorInfoEntity> list = new List<AuthorInfoEntity>();
                recordCount = ds.Tables[0].Rows.Count;
                int startID = (query.CurrentPage - 1) * query.PageSize;
                int endID = startID + query.PageSize;
                if (endID >= recordCount)
                {
                    endID = recordCount;
                }
                AuthorInfoEntity authorInfoEntity = null;
                for (int i = startID; i < endID; i++)
                {
                    authorInfoEntity = new AuthorInfoEntity();
                    authorInfoEntity.AuthorID = (Int64)ds.Tables[0].Rows[i]["AuthorID"];
                    authorInfoEntity.JournalID = query.JournalID;
                    authorInfoEntity.LoginName = (String)ds.Tables[0].Rows[i]["LoginName"];
                    authorInfoEntity.RealName = (String)ds.Tables[0].Rows[i]["RealName"];
                    authorInfoEntity.Mobile = (String)ds.Tables[0].Rows[i]["Mobile"];
                    authorInfoEntity.ExpertList = ds.Tables[0].Rows[i].IsNull("ResearchTopics") ? "" : ds.Tables[0].Rows[i]["ResearchTopics"].ToString();
                    authorInfoEntity.AuditCount = (Int32)ds.Tables[0].Rows[i]["AuditCount"];
                    authorInfoEntity.AuditedCount = (Int32)ds.Tables[0].Rows[i]["AuditedCount"];
                    if (!string.IsNullOrEmpty(authorInfoEntity.ExpertList))
                    {
                        authorInfoEntity.ExpertList = authorInfoEntity.ExpertList.Remove(authorInfoEntity.ExpertList.Length - 1);
                    }
                    list.Add(authorInfoEntity);
                }
                pager.ItemList = list;
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        public bool AddAuthorInfo(AuthorInfoEntity authorInfoEntity)
        {
            bool flag = false;
            DbCommand cmd = null;
            if (authorInfoEntity.RoleID != null) // 编辑部成员注册
            {
                cmd = db.GetStoredProcCommand("dbo.UP_AddJournalMember");
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, authorInfoEntity.JournalID);
                db.AddInParameter(cmd, "@LoginName", DbType.AnsiString, authorInfoEntity.LoginName);
                db.AddInParameter(cmd, "@Pwd", DbType.AnsiString, authorInfoEntity.Pwd);
                db.AddInParameter(cmd, "@RealName", DbType.AnsiString, authorInfoEntity.RealName);
                db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, authorInfoEntity.Mobile);
                db.AddInParameter(cmd, "@RoleID", DbType.Int64, authorInfoEntity.RoleID.Value);
                db.AddInParameter(cmd, "@GroupID", DbType.Int16, authorInfoEntity.GroupID);
            }
            else // 普通作者注册
            {
                StringBuilder sqlCommandText = new StringBuilder();
                sqlCommandText.Append(" @JournalID");
                sqlCommandText.Append(", @LoginName");
                sqlCommandText.Append(", @Pwd");
                sqlCommandText.Append(", @RealName");
                sqlCommandText.Append(", @Mobile");
                sqlCommandText.Append(", @Status");

                cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.AuthorInfo ({0}) VALUES ({1})", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

                db.AddInParameter(cmd, "@JournalID", DbType.Int64, authorInfoEntity.JournalID);
                db.AddInParameter(cmd, "@LoginName", DbType.AnsiString, authorInfoEntity.LoginName);
                db.AddInParameter(cmd, "@Pwd", DbType.AnsiString, authorInfoEntity.Pwd);
                db.AddInParameter(cmd, "@RealName", DbType.AnsiString, authorInfoEntity.RealName);
                db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, authorInfoEntity.Mobile);
                db.AddInParameter(cmd, "@Status", DbType.Byte, authorInfoEntity.Status);
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

        #region 更新数据

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="AdminID"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        public bool UpdatePwd(AuthorInfoEntity authorItem)
        {
            bool flag = false;
            string sql = "UPDATE dbo.AuthorInfo SET Pwd=@Pwd WHERE AuthorID=@AuthorID AND JournalID=@JournalID";
            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, authorItem.AuthorID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, authorItem.JournalID);
            db.AddInParameter(cmd, "@Pwd", DbType.AnsiString, authorItem.Pwd);

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
        /// 修改登录信息
        /// </summary>
        /// <param name="AdminID"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        public bool UpdateLoginInfo(AuthorInfoEntity authorItem)
        {
            bool flag = false;
            string sql = "UPDATE dbo.AuthorInfo SET LoginDate=@LoginDate,LoginCount=LoginCount+1,LoginIP=@LoginIP WHERE AuthorID=@AuthorID AND JournalID=@JournalID ";
            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, authorItem.AuthorID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, authorItem.JournalID);
            db.AddInParameter(cmd, "@LoginIP", DbType.AnsiString, authorItem.LoginIP);
            db.AddInParameter(cmd, "@LoginDate", DbType.DateTime, authorItem.LoginDate);

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
        /// 修改成员信息
        /// </summary>
        /// <param name="authorInfoEntity"></param>
        /// <returns></returns>
        public bool UpdateMembaerInfo(AuthorInfoEntity authorInfoEntity)
        {
            bool flag = false;

            if (authorInfoEntity.GroupID == 2)
            {
                return NewUpdateAuthorInfo(authorInfoEntity);
            }

            DbCommand cmd = db.GetStoredProcCommand("dbo.UP_EditMemberInfo");

            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, authorInfoEntity.AuthorID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, authorInfoEntity.JournalID);
            db.AddInParameter(cmd, "@LoginName", DbType.AnsiString, authorInfoEntity.LoginName);
            db.AddInParameter(cmd, "@Pwd", DbType.AnsiString, string.IsNullOrEmpty(authorInfoEntity.Pwd) ? "" : WKT.Common.Security.MD5Handle.Encrypt(authorInfoEntity.Pwd));
            db.AddInParameter(cmd, "@RealName", DbType.AnsiString, authorInfoEntity.RealName);
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, authorInfoEntity.Mobile);
            db.AddInParameter(cmd, "@OldRoleID", DbType.Int64, authorInfoEntity.OldRoleID);
            db.AddInParameter(cmd, "@RoleID", DbType.Int64, authorInfoEntity.RoleID.Value);
            db.AddInParameter(cmd, "@Status", DbType.Int16, authorInfoEntity.Status);
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

        public bool UpdateAuthorInfo(AuthorInfoEntity authorInfoEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  AuthorID=@AuthorID ");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" RealName=@RealName");
            if (!string.IsNullOrEmpty(authorInfoEntity.Pwd))
            {
                sqlCommandText.Append(", Pwd=@Pwd");
            }
            sqlCommandText.Append(", Mobile=@Mobile");
            sqlCommandText.Append(", Status=@Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.AuthorInfo SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, authorInfoEntity.AuthorID);
            if (!string.IsNullOrEmpty(authorInfoEntity.Pwd))
            {
                db.AddInParameter(cmd, "@Pwd", DbType.AnsiString, WKT.Common.Security.MD5Handle.Encrypt(authorInfoEntity.Pwd));
            }
            db.AddInParameter(cmd, "@RealName", DbType.AnsiString, authorInfoEntity.RealName);
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, authorInfoEntity.Mobile);
            db.AddInParameter(cmd, "@Status", DbType.AnsiString, authorInfoEntity.Status);
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

        public bool NewUpdateAuthorInfo(AuthorInfoEntity authorInfoEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  AuthorID=@AuthorID ");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("Status=@Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.AuthorInfo SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, authorInfoEntity.AuthorID);
            db.AddInParameter(cmd, "@Status", DbType.AnsiString, authorInfoEntity.Status);
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

        public bool DeleteAuthorInfo(AuthorInfoEntity authorInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.AuthorInfo");
            sqlCommandText.Append(" WHERE  AuthorID=@AuthorID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, authorInfoEntity.AuthorID);

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

        public bool DeleteAuthorInfo(Int64 authorID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.AuthorInfo");
            sqlCommandText.Append(" WHERE  AuthorID=@AuthorID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, authorID);
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
        /// <param name="authorID">作者信息</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteAuthorInfo(AuthorInfoQuery authorQuery)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.AppendFormat("DELETE FROM dbo.AuthorInfo WHERE AuthorID IN ({0}) AND JournalID=@JournalID", string.Join(",", authorQuery.AuthorIDList));

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, authorQuery.JournalID);
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

        public AuthorInfoEntity MakeAuthorInfo(IDataReader dr)
        {
            AuthorInfoEntity authorInfoEntity = null;
            if (dr.Read())
            {
                authorInfoEntity = new AuthorInfoEntity();
                authorInfoEntity.AuthorID = (Int64)dr["AuthorID"];
                authorInfoEntity.JournalID = (Int64)dr["JournalID"];
                authorInfoEntity.LoginName = (String)dr["LoginName"];
                authorInfoEntity.Pwd = (String)dr["Pwd"];
                authorInfoEntity.RealName = (String)dr["RealName"];
                authorInfoEntity.Mobile = (String)dr["Mobile"];
                authorInfoEntity.LoginIP = (String)dr["LoginIP"];
                authorInfoEntity.LoginCount = (Int32)dr["LoginCount"];
                authorInfoEntity.LoginDate = (DateTime)dr["LoginDate"];
                authorInfoEntity.Status = (Byte)dr["Status"];
                authorInfoEntity.GroupID = (Byte)dr["GroupID"];
                authorInfoEntity.RoleID = dr.HasColumn("RoleID") ? Convert.IsDBNull(dr["RoleID"]) ? -999 : (Int64)dr["RoleID"] : -1;
                authorInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return authorInfoEntity;
        }

        public AuthorInfoEntity MakeAuthorInfo(DataRow dr)
        {
            AuthorInfoEntity authorInfoEntity = null;
            if (dr != null)
            {
                authorInfoEntity = new AuthorInfoEntity();
                authorInfoEntity.AuthorID = (Int64)dr["AuthorID"];
                authorInfoEntity.JournalID = (Int64)dr["JournalID"];
                authorInfoEntity.LoginName = (String)dr["LoginName"];
                authorInfoEntity.Pwd = (String)dr["Pwd"];
                authorInfoEntity.RealName = (String)dr["RealName"];
                authorInfoEntity.Mobile = (String)dr["Mobile"];
                authorInfoEntity.LoginIP = (String)dr["LoginIP"];
                authorInfoEntity.LoginCount = (Int32)dr["LoginCount"];
                authorInfoEntity.LoginDate = (DateTime)dr["LoginDate"];
                authorInfoEntity.GroupID = (Byte)dr["GroupID"];
                authorInfoEntity.RoleID = dr.HasColumn("RoleID") ? Convert.IsDBNull(dr["RoleID"]) ? -999 : (Int64)dr["RoleID"] : -1;
                authorInfoEntity.Status = (Byte)dr["Status"];
                authorInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return authorInfoEntity;
        }
        #endregion

        #region 根据数据组装一组对象数据

        public List<AuthorInfoEntity> MakeAuthorInfoList(IDataReader dr)
        {
            List<AuthorInfoEntity> list = new List<AuthorInfoEntity>();
            while (dr.Read())
            {
                AuthorInfoEntity authorInfoEntity = new AuthorInfoEntity();
                authorInfoEntity.AuthorID = (Int64)dr["AuthorID"];
                authorInfoEntity.JournalID = (Int64)dr["JournalID"];
                authorInfoEntity.LoginName = (String)dr["LoginName"];
                authorInfoEntity.Pwd = (String)dr["Pwd"];
                authorInfoEntity.RealName = (String)dr["RealName"];
                authorInfoEntity.Mobile = (String)dr["Mobile"];
                authorInfoEntity.LoginIP = (String)dr["LoginIP"];
                authorInfoEntity.LoginCount = (Int32)dr["LoginCount"];
                authorInfoEntity.LoginDate = (DateTime)dr["LoginDate"];
                authorInfoEntity.Status = (Byte)dr["Status"];
                authorInfoEntity.GroupID = (Byte)dr["GroupID"];
                authorInfoEntity.AddDate = (DateTime)dr["AddDate"];
                authorInfoEntity.RoleID=dr.HasColumn("RoleID")?Convert.IsDBNull(dr["RoleID"])?-999:(Int64)dr["RoleID"]:-1;
                authorInfoEntity.RoleName = dr.HasColumn("RoleName") ? Convert.IsDBNull(dr["RoleName"]) ? "" : (string)dr["RoleName"] :"";
                
                list.Add(authorInfoEntity);

            }
            dr.Close();
            return list;
        }


        public List<AuthorInfoEntity> MakeAuthorInfoList(DataTable dt)
        {
            List<AuthorInfoEntity> list = new List<AuthorInfoEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AuthorInfoEntity authorInfoEntity = MakeAuthorInfo(dt.Rows[i]);
                    list.Add(authorInfoEntity);
                }
            }
            return list;
        }

        #endregion

        # region 作者统计数据

        /// <summary>
        /// 作者总数及投稿作者数量统计
        /// </summary>
        /// <returns></returns>
        public IDictionary<String, Int32> GetAuthorContributeStat(QueryBase query)
        {
            IDictionary<String, Int32> dictStat = new Dictionary<String, Int32>();
            string strSql = "SELECT COUNT(1) AS TotalCount,(select COUNT(DISTINCT AuthorID) FROM dbo.ContributionInfo) AS AuthorCount FROM dbo.AuthorInfo a WITH(NOLOCK) WHERE JournalID=@JournalID AND GroupID=2";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    dictStat.Add("TotalCount", TypeParse.ToInt(dr["TotalCount"]));
                    dictStat.Add("AuthorCount", TypeParse.ToInt(dr["AuthorCount"]));
                }
                dr.Close();
            }
            return dictStat;
        }

        /// <summary>
        /// 获取作者按省份统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorProvinceStat(QueryBase query)
        {
            IList<AuthorStatEntity> authorStatList = new List<AuthorStatEntity>();
            string strSql = "SELECT b.Province,COUNT(DISTINCT b.AuthorID) AS Count FROM dbo.AuthorInfo a WITH(NOLOCK),dbo.AuthorDetail b WITH(NOLOCK) WHERE a.JournalID=@JournalID AND a.GroupID=2 AND a.JournalID=b.JournalID GROUP BY b.Province";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    AuthorStatEntity item = new AuthorStatEntity();
                    item.StatItem = dr.IsDBNull(dr.GetOrdinal("Province")) ? "未知地区" : dr["Province"].ToString();
                    if (string.IsNullOrEmpty(item.StatItem))
                    {
                        item.StatItem = "未知地区";
                    }
                    item.Count = TypeParse.ToInt(dr["Count"]);
                    authorStatList.Add(item);
                }
                dr.Close();
            }
            return authorStatList;
        }

        /// <summary>
        /// 获取作者按学历统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorEducationStat(QueryBase query)
        {
            IList<AuthorStatEntity> authorStatList = new List<AuthorStatEntity>();
            string strSql = "SELECT b.Education,COUNT(DISTINCT b.AuthorID) AS Count FROM dbo.AuthorInfo a WITH(NOLOCK),dbo.AuthorDetail b WITH(NOLOCK) WHERE a.JournalID=@JournalID AND a.GroupID=2 AND a.JournalID=b.JournalID GROUP BY b.Education";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    AuthorStatEntity item = new AuthorStatEntity();
                    item.StatID = dr.IsDBNull(dr.GetOrdinal("Education")) ? 0 : TypeParse.ToInt(dr["Education"]);
                    item.Count = TypeParse.ToInt(dr["Count"]);
                    authorStatList.Add(item);
                }
                dr.Close();
            }
            return authorStatList;
        }

        /// <summary>
        /// 获取作者按专业统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorProfessionalStat(QueryBase query)
        {
            IList<AuthorStatEntity> authorStatList = new List<AuthorStatEntity>();
            string strSql = "SELECT b.Professional,COUNT(DISTINCT b.AuthorID) AS Count FROM dbo.AuthorInfo a WITH(NOLOCK),dbo.AuthorDetail b WITH(NOLOCK) WHERE a.JournalID=@JournalID AND a.GroupID=2 AND a.JournalID=b.JournalID GROUP BY b.Professional";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    AuthorStatEntity item = new AuthorStatEntity();
                    item.StatItem = dr.IsDBNull(dr.GetOrdinal("Professional")) ? "" : dr["Professional"].ToString();
                    item.Count = TypeParse.ToInt(dr["Count"]);
                    authorStatList.Add(item);
                }
                dr.Close();
            }
            return authorStatList;
        }

        /// <summary>
        /// 获取作者按职称统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorJobTitleStat(QueryBase query)
        {
            IList<AuthorStatEntity> authorStatList = new List<AuthorStatEntity>();
            string strSql = "SELECT b.JobTitle,COUNT(DISTINCT b.AuthorID) AS Count FROM dbo.AuthorInfo a WITH(NOLOCK),dbo.AuthorDetail b WITH(NOLOCK) WHERE a.JournalID=@JournalID AND a.GroupID=2 AND a.JournalID=b.JournalID GROUP BY b.JobTitle";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    AuthorStatEntity item = new AuthorStatEntity();
                    item.StatID = dr.IsDBNull(dr.GetOrdinal("JobTitle")) ? 0 : TypeParse.ToInt(dr["JobTitle"]);
                    item.Count = TypeParse.ToInt(dr["Count"]);
                    authorStatList.Add(item);
                }
                dr.Close();
            }
            return authorStatList;
        }

        /// <summary>
        /// 获取作者按性别统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorGenderStat(QueryBase query)
        {
            IList<AuthorStatEntity> authorStatList = new List<AuthorStatEntity>();
            string strSql = "SELECT b.Gender,COUNT(DISTINCT b.AuthorID) AS Count FROM dbo.AuthorInfo a WITH(NOLOCK),dbo.AuthorDetail b WITH(NOLOCK) WHERE a.JournalID=@JournalID AND a.GroupID=2 AND a.JournalID=b.JournalID GROUP BY b.Gender";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                int Gender = 0;
                while (dr.Read())
                {
                    Gender = TypeParse.ToInt(dr["Gender"]);
                    AuthorStatEntity item = new AuthorStatEntity();
                    if (Gender == 1)
                    {
                        item.StatItem = "男";
                    }
                    else if (Gender == 2)
                    {
                        item.StatItem = "女";
                    }
                    else
                    {
                        item.StatItem = "未知";
                    }
                    item.Count = TypeParse.ToInt(dr["Count"]);
                    authorStatList.Add(item);
                }
                dr.Close();
            }
            return authorStatList;
        }

        # endregion

        # region 编辑、专家工作量统计

        /// <summary>
        /// 编辑工作量统计(不再统计已删除与已撤稿件) zhanglc 2014-09-27
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<WorkloadEntity> GetWorkloadList(WorkloadQuery query)
        {
            IList<WorkloadEntity> authorStatList = new List<WorkloadEntity>();
            string strSql = "";
            if (query.GroupID == 1)
            {
                strSql = @"
                            SELECT a.AuthorID,a.LoginName,a.RealName,ad.AuthorName,ad.Address,ad.WorkUnit,ad.InvoiceUnit,ad.ZipCode,ad.Mobile,ad.Tel,COUNT(DISTINCT f.CID) AS AlreadyCount,COUNT(DISTINCT f2.CID) AS WaitCount FROM 
                            dbo.AuthorInfo a WITH(NOLOCK)
                            left join dbo.AuthorDetail ad WITH(NOLOCK) on a.JournalID=ad.JournalID and a.AuthorID=ad.AuthorID
                            left join dbo.FlowLogInfo f WITH(NOLOCK) on a.JournalID=f.JournalID and a.AuthorID=f.RecUserID and f.Status=1 and f.CID in (select CID from ContributionInfo where Status!=-999 and Status!=-1) and f.DealDate>=@StartDate AND f.DealDate<@EndDate 
                            left join dbo.FlowLogInfo f2 WITH(NOLOCK) on a.JournalID=f2.JournalID and a.AuthorID=f2.RecUserID and f2.Status=0 and f2.CID in (select CID from ContributionInfo where Status!=-999 and Status!=-1) and f2.AddDate>=@StartDate AND f2.AddDate<@EndDate
                            where a.JournalID=@JournalID and a.RealName like '%" + query.RealName + @"%' and a.GroupID=@GroupID 
                            GROUP BY a.AuthorID,a.LoginName,a.RealName,ad.Address,ad.WorkUnit,ad.InvoiceUnit,ad.ZipCode,ad.Mobile,ad.Tel,ad.ReserveField1,ad.ReserveField2,ad.ReserveField3";
            }
            else
            {
                strSql = @"
                            SELECT a.AuthorID,a.LoginName,a.RealName,ad.Address,ad.WorkUnit,ad.InvoiceUnit,ad.ZipCode,ad.Mobile,ad.Tel,ad.ReserveField1,ad.ReserveField2,ad.ReserveField3,COUNT(DISTINCT f.CID) AS AlreadyCount,COUNT(DISTINCT f2.CID) AS WaitCount FROM 
                            dbo.AuthorInfo a WITH(NOLOCK)
                            left join dbo.AuthorDetail ad WITH(NOLOCK) on a.JournalID=ad.JournalID and a.AuthorID=ad.AuthorID
                            left join dbo.FlowLogInfo f WITH(NOLOCK) on a.JournalID=f.JournalID and a.AuthorID=f.RecUserID and f.Status=1 and f.CID in (select CID from ContributionInfo where Status!=-999 and Status!=-1) and f.DealDate>=@StartDate AND f.DealDate<@EndDate 
                            left join dbo.FlowLogInfo f2 WITH(NOLOCK) on a.JournalID=f2.JournalID and a.AuthorID=f2.RecUserID and f2.Status=0 and f2.CID in (select CID from ContributionInfo where Status!=-999 and Status!=-1) and f2.AddDate>=@StartDate AND f2.AddDate<@EndDate
                            where a.JournalID=@JournalID and a.RealName like '%" + query.RealName + @"%' and a.GroupID in (3,4) 
                            GROUP BY a.AuthorID,a.LoginName,a.RealName,ad.Address,ad.WorkUnit,ad.InvoiceUnit,ad.ZipCode,ad.Mobile,ad.Tel,ad.ReserveField1,ad.ReserveField2,ad.ReserveField3";
            }
            
            if (!string.IsNullOrEmpty(query.OrderBy))
            {
                strSql += " ORDER BY " + WKT.Common.Security.SecurityUtils.SafeSqlString(query.OrderBy.TextFilter());
            }
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@StartDate", DbType.DateTime, query.StartDate == null ? Convert.ToDateTime("2000-01-01") : query.StartDate);
            db.AddInParameter(cmd, "@EndDate", DbType.DateTime, query.EndDate == null ? DateTime.Now.AddDays(1) : query.EndDate.Value.AddDays(1));
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    WorkloadEntity item = new WorkloadEntity();
                    item.AuthorID = TypeParse.ToLong(dr["AuthorID"]);
                    item.LoginName = dr["LoginName"].ToString();
                    item.RealName = dr["RealName"].ToString();
                    item.Address = dr["Address"].ToString();
                    item.WorkUnit = dr["WorkUnit"].ToString();
                    item.InvoiceUnit = dr["InvoiceUnit"].ToString();
                    item.ZipCode = dr["ZipCode"].ToString();
                    item.Mobile = dr["Mobile"].ToString();
                    item.Tel = dr["Tel"].ToString();
                    if (query.GroupID == 3)
                    {
                        item.ReserveField1 = dr["ReserveField1"].ToString();
                        item.ReserveField2 = dr["ReserveField2"].ToString();
                        item.ReserveField3 = dr["ReserveField3"].ToString();
                    }
                    item.AlreadyCount = TypeParse.ToInt(dr["AlreadyCount"]);
                    item.WaitCount = TypeParse.ToInt(dr["WaitCount"]);                   
                    authorStatList.Add(item);
                }
                dr.Close();
            }
            return authorStatList;
        }

        /// <summary>
        /// 个人收稿量 文海峰 2014-1-7
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<WorkloadEntity> GetPersonCirCountList(WorkloadQuery query)
        {
            IList<WorkloadEntity> authorStatList = new List<WorkloadEntity>();
            string strSql = @"select ar.AuthorID,RealName,count(CID) as CirCount from  [FlowLogInfo]  inner join (select AuthorID,RealName from  AuthorInfo where GroupID=1) as ar on ar.AuthorID=FlowLogInfo.RecUserID and  FlowLogID in (select FlowLogID from  [FlowLogInfo] where   ";
            if (!string.IsNullOrEmpty(query.StatusList))
            {
                string[] paras = query.StatusList.Split(',');
                for (int i = 0; i < paras.Length; i++)
                {
                    if (i == paras.Length-1)
                    {
                         strSql += "  TargetStatusID="+paras[i];
                    }
                    else
                    {
                        strSql += "  TargetStatusID="+ paras[i]+ " OR ";
                    }
                }
               
            }
            if (query.StartDate != null)
            {
                strSql += "  AddDate>=@StartDate AND";
            }
            else if (query.EndDate != null)
            {
                strSql += " AddDate<=@EndDate AND";
            }
            strSql += "  JournalID=@JournalID ) group by AuthorID,RealName ";
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@Statuslist", DbType.Int64, query.StatusList);
            db.AddInParameter(cmd, "@StartDate", DbType.DateTime, query.StartDate == null ? Convert.ToDateTime("2000-01-01") : query.StartDate);
            db.AddInParameter(cmd, "@EndDate", DbType.DateTime, query.EndDate == null ? DateTime.Now.AddDays(1) : query.EndDate.Value.AddDays(1));


            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    WorkloadEntity item = new WorkloadEntity();
                    item.RealName = Convert.IsDBNull(dr["RealName"])?"" : Convert.ToString(dr["RealName"]);
                    item.AlreadyCount = TypeParse.ToInt(dr["CirCount"]);
                    authorStatList.Add(item);
                }
                dr.Close();
            }
            return authorStatList;
        }
        # endregion

        # region 编辑工作量统计_New

        /// <summary>
        /// 编辑工作量统计
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
//        public IList<WorkloadEntity> GetEditorWorkloadList(WorkloadQuery query)
//        {
//            IList<WorkloadEntity> authorStatList = new List<WorkloadEntity>();
//            string strSql = @"
//                            SELECT ai.AuthorID,ai.LoginName,ai.RealName
//                            FROM dbo.AuthorInfo ai WITH(NOLOCK)
//                            WHERE ai.JournalID=@JournalID AND ai.GroupID=1
//                            ";
//            if (!string.IsNullOrEmpty(query.RealName))
//            {
//                //strSql = strSql + " AND a.RealName LIKE '%" + WKT.Common.Security.SecurityUtils.SafeSqlString(query.RealName) + "%'";
//                strSql = strSql + " AND ai.RealName LIKE '%" + query.RealName + "%'";
//            }

//            DbCommand cmd = db.GetSqlStringCommand(strSql);
//            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);

//            // 获取统计数量
//            List<WorkloadEntity> listWorkStat = GetEditorStatWorkStat(query);

//            // 获取统计的数据字典
//            IDictionary<int, string> dictKey = null;
//            if (query.IsNewContributes)
//            {
//                dictKey = DictValueDataAccess.Instance.GetDictValueDcit(query.JournalID, "NewContribute");
//            }
//            else
//            {
//                dictKey = DictValueDataAccess.Instance.GetDictValueDcit(query.JournalID, "EditorWorkStat");

//            }
//            using (IDataReader dr = db.ExecuteReader(cmd))
//            {
//                while (dr.Read())
//                {
//                    WorkloadEntity item = new WorkloadEntity();
//                    item.AuthorID = TypeParse.ToLong(dr["AuthorID"]);
//                    item.LoginName = dr["LoginName"].ToString();
//                    item.RealName = dr["RealName"].ToString();
//                    foreach (var item1 in dictKey.Keys)
//                    {
//                        item.DictEditorStatItems.Add(dictKey[item1] + "-" + item1, 0);
//                    }
//                    List<WorkloadEntity> statItemList = listWorkStat.Where(p => p.AuthorID == item.AuthorID).ToList<WorkloadEntity>();
//                    if (statItemList != null && statItemList.Count > 0)
//                    {
//                        foreach (WorkloadEntity wleItem in statItemList)
//                        {
//                            string key = dictKey.ContainsKey(wleItem.StatusID) ? dictKey[wleItem.StatusID] + "-" + wleItem.StatusID.ToString() : "";
//                            int value = wleItem.WorkCount;
//                            if (item.DictEditorStatItems.Keys.Contains(key))
//                            {
//                                item.DictEditorStatItems[key] = value;
//                            }
//                            else
//                            {
//                                item.DictEditorStatItems.Add(key, value);
//                            }

//                        }
//                    }
//                    authorStatList.Add(item);
//                }
//                dr.Close();
//            }
//            return authorStatList;
//        }

        /// <summary>
        /// 获取人员数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private List<WorkloadEntity> GetEditorStatWorkStat(WorkloadQuery query)
        {
            List<WorkloadEntity> listWork = new List<WorkloadEntity>();
            string strSql = string.Empty;
            if (query.IsNewContributes)
            {
                strSql = @"
                            SELECT ai.AuthorID,fi.StatusID, ai.RealName,COUNT(ai.AuthorID) as WorkCount
                            FROM dbo.AuthorInfo ai WITH(NOLOCK) 
	                            INNER JOIN dbo.FlowLogInfo fi WITH(NOLOCK) ON ai.AuthorID=fi.RecUserID and ai.GroupID=1
                                INNER JOIN dbo.ContributionInfo ci WITH(NOLOCK) ON fi.CID=ci.CID AND ci.Status!=-999
                            WHERE ai.JournalID=@JournalID AND ai.JournalID=fi.JournalID AND fi.JournalID=ci.JournalID AND fi.StatusID=fi.TargetStatusID and fi.TargetStatusID IN (
                            SELECT dv.ValueID FROM DictValue dv WITH(NOLOCK) WHERE dv.DictKey='NewContribute'
                            ) AND fi.AddDate>=@StartDate AND fi.AddDate<@EndDate
                            ";
            }
            else
            {
                strSql = @"
                            SELECT ai.AuthorID,fi.TargetStatusID,COUNT(DISTINCT fi.CID) as WorkCount
                            FROM dbo.AuthorInfo ai WITH(NOLOCK) 
	                            INNER JOIN dbo.FlowLogInfo fi WITH(NOLOCK) ON ai.AuthorID=fi.SendUserID and ai.GroupID=1
                                INNER JOIN dbo.ContributionInfo ci WITH(NOLOCK) ON fi.CID=ci.CID AND ci.Status!=-999
                            WHERE ai.JournalID=@JournalID AND ai.JournalID=fi.JournalID AND fi.JournalID=ci.JournalID AND fi.TargetStatusID IN (
                            SELECT dv.ValueID FROM DictValue dv WITH(NOLOCK) WHERE dv.DictKey='EditorWorkStat'
                            ) AND fi.AddDate>=@StartDate AND fi.AddDate<@EndDate
                            ";
            }
            if (!string.IsNullOrEmpty(query.RealName))
            {
                strSql = strSql + " AND ai.RealName LIKE '%" + WKT.Common.Security.SecurityUtils.SafeSqlString(query.RealName) + "%'";
            }
            strSql += query.IsNewContributes ? " GROUP BY ai.RealName,ai.AuthorID,fi.StatusID" : " GROUP BY ai.AuthorID,fi.TargetStatusID";
            DbCommand cmd = db.GetSqlStringCommand(strSql);

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@StartDate", DbType.DateTime, query.StartDate == null ? Convert.ToDateTime("2000-01-01") : query.StartDate);
            db.AddInParameter(cmd, "@EndDate", DbType.DateTime, query.EndDate == null ? DateTime.Now.AddDays(1) : query.EndDate.Value.AddDays(1));

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                WorkloadEntity item = null;
                while (dr.Read())
                {
                    item = new WorkloadEntity();
                    if (DBConfigUtility.IsContainsColumn(dr, "TargetStatusID"))
                    {
                        item.StatusID = TypeParse.ToInt(dr["TargetStatusID"]);
                    }
                    if (DBConfigUtility.IsContainsColumn(dr, "RealName"))
                    {
                        item.RealName = TypeParse.ToString(dr["RealName"]);
                    }

                    if (DBConfigUtility.IsContainsColumn(dr, "StatusID"))
                    {
                        item.StatusID = TypeParse.ToInt(dr["StatusID"]);
                    }
                    item.AuthorID = TypeParse.ToLong(dr["AuthorID"]);
                    item.WorkCount = TypeParse.ToInt(dr["WorkCount"]);
                    listWork.Add(item);
                }
                dr.Close();
            }
            return listWork;
        }

        # endregion

        # region 编辑、专家处理稿件明细

        /// <summary>
        /// 编辑、专家处理稿件明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<StatDealContributionDetailEntity> GetDealContributionDetail(StatQuery query)
        {
            IList<StatDealContributionDetailEntity> authorStatList = new List<StatDealContributionDetailEntity>();
            DbCommand cmd = null;
            AuthorInfoEntity authorEntity = GetAuthorInfo(new AuthorInfoQuery { AuthorID = query.AuthorID, JournalID = query.JournalID });
            if (authorEntity == null)
            {
                authorEntity = new AuthorInfoEntity();
            }
            if (authorEntity.GroupID == 3)
            {
                cmd = db.GetStoredProcCommand("UP_GetContributionDealDetail_Expert");
            }
            else
            {
                if (query.Status > 1)
                {
                    cmd = db.GetStoredProcCommand("UP_GetContributionDealDetail_Editor");
                }
                else
                {
                    cmd = db.GetStoredProcCommand("UP_GetContributionDealDetail");
                }
            }
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@Status", DbType.Int32, query.Status);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, query.AuthorID);
            db.AddInParameter(cmd, "@StartDate", DbType.DateTime, query.StartDate == null ? Convert.ToDateTime("2000-01-01") : query.StartDate);
            db.AddInParameter(cmd, "@EndDate", DbType.DateTime, query.EndDate == null ? DateTime.Now.AddDays(1) : query.EndDate.Value.AddDays(1));
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    StatDealContributionDetailEntity item = new StatDealContributionDetailEntity();
                    item.CID = TypeParse.ToLong(dr["CID"]);
                    item.CNumber = dr["CNumber"].ToString();
                    item.Title = dr["Title"].ToString();
                    item.StatusName = dr["StatusName"].ToString();
                    if (dr.HasColumn("Adddate"))
                    {
                        item.DealDate = TypeParse.ToDateTime(dr["Adddate"]);
                    }
                    else
                    {
                        item.DealDate = TypeParse.ToDateTime(dr["DealDate"]);
                    }
                    authorStatList.Add(item);
                }
                dr.Close();
            }
            return authorStatList;
        }

        # endregion

        /// <summary>
        /// 获取人员数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<Int64, String> GetAuthorDict(AuthorInfoQuery query)
        {
            IDictionary<Int64, String> dict = new Dictionary<Int64, String>();
            string strSql = "SELECT AuthorID,RealName FROM dbo.AuthorInfo with(nolock) WHERE " + AuthorInfoQueryToSQLWhere(query);
            DbCommand cmd = db.GetSqlStringCommand(strSql);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                Int64 id = 0;
                while (dr.Read())
                {
                    id = (Int64)dr["AuthorID"];
                    if (!dict.ContainsKey(id))
                        dict.Add(id, (String)dr["RealName"]);
                }
                dr.Close();
            }
            return dict;
        }

    }
}

