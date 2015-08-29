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
    /// 管理平台系统账号
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class SysAccountInfoDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public SysAccountInfoDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTSysDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static SysAccountInfoDataAccess _instance = new SysAccountInfoDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static SysAccountInfoDataAccess Instance
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
        public string SysAccountInfoQueryToSQLWhere(SysAccountInfoQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            if (query.Status != null)
            {
                sbWhere.Append(" AND Status= ").Append(query.Status.Value);
            }
            if (!string.IsNullOrEmpty(query.LoginName))
            {
                sbWhere.AppendFormat( " AND LoginName='{0}'",WKT.Common.Security.SecurityUtils.SafeSqlString(query.LoginName));
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
        public string SysAccountInfoQueryToSQLOrder(SysAccountInfoQuery query)
        {
            return " AdminID DESC";
        }

        #endregion 组装SQL条件

        #region 获取一个实体对象

        public SysAccountInfoEntity GetSysAccountInfo(Int32 adminID)
        {
            SysAccountInfoEntity sysAccountInfoEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  AdminID,UserName,LoginName,Pwd,Gender,Email,Mobile,Status,LastIP,LoginDate,LogOnTimes,AddDate FROM dbo.SysAccountInfo WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  AdminID=@AdminID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@AdminID", DbType.Int32, adminID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                sysAccountInfoEntity = MakeSysAccountInfo(dr);
            }
            return sysAccountInfoEntity;
        }

        #endregion

        #region 根据条件获取所有实体对象

        public List<SysAccountInfoEntity> GetSysAccountInfoList()
        {
            List<SysAccountInfoEntity> sysAccountInfoEntity = new List<SysAccountInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT AdminID,UserName,LoginName,Pwd,Gender,Email,Mobile,Status,LastIP,LoginDate,LogOnTimes,AddDate FROM dbo.SysAccountInfo WITH(NOLOCK)");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                sysAccountInfoEntity = MakeSysAccountInfoList(dr);
            }
            return sysAccountInfoEntity;
        }

        public List<SysAccountInfoEntity> GetSysAccountInfoList(SysAccountInfoQuery query)
        {
            List<SysAccountInfoEntity> list = new List<SysAccountInfoEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT AdminID,UserName,LoginName,Pwd,Gender,Email,Mobile,Status,LastIP,LoginDate,LogOnTimes,AddDate FROM dbo.SysAccountInfo WITH(NOLOCK)");
            string whereSQL = SysAccountInfoQueryToSQLWhere(query);
            string orderBy = SysAccountInfoQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeSysAccountInfoList(dr);
            }
            return list;
        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<SysAccountInfoEntity></returns>
        public Pager<SysAccountInfoEntity> GetSysAccountInfoPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("SysAccountInfo", "AdminID,UserName,LoginName,Pwd,Gender,Email,Mobile,Status,LastIP,LoginDate,LogOnTimes,AddDate", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<SysAccountInfoEntity> pager = new Pager<SysAccountInfoEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeSysAccountInfoList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<SysAccountInfoEntity> GetSysAccountInfoPageList(QueryBase query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("SysAccountInfo", "AdminID,UserName,LoginName,Pwd,Gender,Email,Mobile,Status,LastIP,LoginDate,LogOnTimes,AddDate", " AdminID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<SysAccountInfoEntity> pager = new Pager<SysAccountInfoEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeSysAccountInfoList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        public Pager<SysAccountInfoEntity> GetSysAccountInfoPageList(SysAccountInfoQuery query)
        {
            int recordCount = 0;
            string whereSQL = SysAccountInfoQueryToSQLWhere(query);
            string orderBy = SysAccountInfoQueryToSQLOrder(query);
            DataSet ds = db.GetPagingData("SysAccountInfo", "AdminID,UserName,LoginName,Pwd,Gender,Email,Mobile,Status,LastIP,LoginDate,LogOnTimes,AddDate", orderBy, whereSQL, query.CurrentPage, query.PageSize, out recordCount);
            Pager<SysAccountInfoEntity> pager = new Pager<SysAccountInfoEntity>();
            if (ds != null && ds.Tables.Count > 0)
            {
                pager.ItemList = MakeSysAccountInfoList(ds.Tables[0]);
            }
            pager.CurrentPage = query.CurrentPage;
            pager.PageSize = query.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        public bool AddSysAccountInfo(SysAccountInfoEntity sysAccountInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("@UserName");
            sqlCommandText.Append(", @LoginName");
            sqlCommandText.Append(", @Pwd");
            sqlCommandText.Append(", @Gender");
            sqlCommandText.Append(", @Email");
            sqlCommandText.Append(", @Mobile");
            sqlCommandText.Append(", @Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.SysAccountInfo ({0}) VALUES ({1})", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@UserName", DbType.AnsiString, sysAccountInfoEntity.UserName);
            db.AddInParameter(cmd, "@LoginName", DbType.AnsiString, sysAccountInfoEntity.LoginName);
            db.AddInParameter(cmd, "@Pwd", DbType.AnsiString, sysAccountInfoEntity.Pwd);
            db.AddInParameter(cmd, "@Gender", DbType.Byte, sysAccountInfoEntity.Gender);
            db.AddInParameter(cmd, "@Email", DbType.AnsiString, sysAccountInfoEntity.Email);
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, sysAccountInfoEntity.Mobile);
            db.AddInParameter(cmd, "@Status", DbType.Byte, sysAccountInfoEntity.Status);;
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
        public bool UpdatePwd(int AdminID,string NewPwd)
        {
            bool flag = false;
            string sql = "UPDATE dbo.SysAccountInfo SET Pwd=@Pwd WHERE  AdminID=@AdminID";
            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "@AdminID", DbType.Int32, AdminID);
            db.AddInParameter(cmd, "@Pwd", DbType.AnsiString, NewPwd);

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
        public bool UpdateAccountLoginInfo(int AdminID,string IP,string LoginDate)
        {
            bool flag = false;
            string sql = "UPDATE dbo.SysAccountInfo SET LoginDate=@LoginDate,LogOnTimes=LogOnTimes+1,LastIP=@LastIP WHERE  AdminID=@AdminID";
            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "@AdminID", DbType.Int32, AdminID);
            db.AddInParameter(cmd, "@LastIP", DbType.AnsiString, IP);
            db.AddInParameter(cmd, "@LoginDate", DbType.DateTime, LoginDate);

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

        public bool UpdateSysAccountInfo(SysAccountInfoEntity sysAccountInfoEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  AdminID=@AdminID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" UserName=@UserName");
            sqlCommandText.Append(", LoginName=@LoginName");
            sqlCommandText.Append(", Gender=@Gender");
            sqlCommandText.Append(", Email=@Email");
            sqlCommandText.Append(", Mobile=@Mobile");
            sqlCommandText.Append(", Status=@Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.SysAccountInfo SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@AdminID", DbType.Int32, sysAccountInfoEntity.AdminID);
            db.AddInParameter(cmd, "@UserName", DbType.AnsiString, sysAccountInfoEntity.UserName);
            db.AddInParameter(cmd, "@LoginName", DbType.AnsiString, sysAccountInfoEntity.LoginName);
            db.AddInParameter(cmd, "@Gender", DbType.Byte, sysAccountInfoEntity.Gender);
            db.AddInParameter(cmd, "@Email", DbType.AnsiString, sysAccountInfoEntity.Email);
            db.AddInParameter(cmd, "@Mobile", DbType.AnsiString, sysAccountInfoEntity.Mobile);
            db.AddInParameter(cmd, "@Status", DbType.Byte, sysAccountInfoEntity.Status);
            
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

        public bool DeleteSysAccountInfo(SysAccountInfoEntity sysAccountInfoEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.SysAccountInfo");
            sqlCommandText.Append(" WHERE  AdminID=@AdminID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            db.AddInParameter(cmd, "@AdminID", DbType.Int32, sysAccountInfoEntity.AdminID);

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

        public bool DeleteSysAccountInfo(Int32 adminID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.SysAccountInfo");
            sqlCommandText.Append(" WHERE  AdminID=@AdminID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@AdminID", DbType.Int32, adminID);
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
        /// <param name="adminID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSysAccountInfo(Int32[] adminID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("");

            for (int i = 0; i < adminID.Length; i++)
            {
                sqlCommandText.Append("DELETE FROM dbo.SysAccountInfo WHERE AdminID=@AdminID" + i + ";");
            }

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for (int i = 0; i < adminID.Length; i++)
            {
                db.AddInParameter(cmd, "@AdminID" + i, DbType.Int32, adminID[i]);
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

        public SysAccountInfoEntity MakeSysAccountInfo(IDataReader dr)
        {
            SysAccountInfoEntity sysAccountInfoEntity = null;
            if (dr.Read())
            {
                sysAccountInfoEntity = new SysAccountInfoEntity();
                sysAccountInfoEntity.AdminID = (Int32)dr["AdminID"];
                sysAccountInfoEntity.UserName = (String)dr["UserName"];
                sysAccountInfoEntity.LoginName = (String)dr["LoginName"];
                sysAccountInfoEntity.Pwd = (String)dr["Pwd"];
                sysAccountInfoEntity.Gender = (Byte)dr["Gender"];
                sysAccountInfoEntity.Email = (String)dr["Email"];
                sysAccountInfoEntity.Mobile = (String)dr["Mobile"];
                sysAccountInfoEntity.Status = (Byte)dr["Status"];
                sysAccountInfoEntity.LastIP = (String)dr["LastIP"];
                sysAccountInfoEntity.LoginDate = (DateTime)dr["LoginDate"];
                sysAccountInfoEntity.LogOnTimes = (Int32)dr["LogOnTimes"];
                sysAccountInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return sysAccountInfoEntity;
        }

        public SysAccountInfoEntity MakeSysAccountInfo(DataRow dr)
        {
            SysAccountInfoEntity sysAccountInfoEntity = null;
            if (dr != null)
            {
                sysAccountInfoEntity = new SysAccountInfoEntity();
                sysAccountInfoEntity.AdminID = (Int32)dr["AdminID"];
                sysAccountInfoEntity.UserName = (String)dr["UserName"];
                sysAccountInfoEntity.LoginName = (String)dr["LoginName"];
                sysAccountInfoEntity.Pwd = (String)dr["Pwd"];
                sysAccountInfoEntity.Gender = (Byte)dr["Gender"];
                sysAccountInfoEntity.Email = (String)dr["Email"];
                sysAccountInfoEntity.Mobile = (String)dr["Mobile"];
                sysAccountInfoEntity.Status = (Byte)dr["Status"];
                sysAccountInfoEntity.LastIP = (String)dr["LastIP"];
                sysAccountInfoEntity.LoginDate = (DateTime)dr["LoginDate"];
                sysAccountInfoEntity.LogOnTimes = (Int32)dr["LogOnTimes"];
                sysAccountInfoEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return sysAccountInfoEntity;
        }
        #endregion

        #region 根据数据组装一组对象数据

        public List<SysAccountInfoEntity> MakeSysAccountInfoList(IDataReader dr)
        {
            List<SysAccountInfoEntity> list = new List<SysAccountInfoEntity>();
            while (dr.Read())
            {
                SysAccountInfoEntity sysAccountInfoEntity = new SysAccountInfoEntity();
                sysAccountInfoEntity.AdminID = (Int32)dr["AdminID"];
                sysAccountInfoEntity.UserName = (String)dr["UserName"];
                sysAccountInfoEntity.LoginName = (String)dr["LoginName"];
                sysAccountInfoEntity.Pwd = (String)dr["Pwd"];
                sysAccountInfoEntity.Gender = (Byte)dr["Gender"];
                sysAccountInfoEntity.Email = (String)dr["Email"];
                sysAccountInfoEntity.Mobile = (String)dr["Mobile"];
                sysAccountInfoEntity.Status = (Byte)dr["Status"];
                sysAccountInfoEntity.LastIP = (String)dr["LastIP"];
                sysAccountInfoEntity.LoginDate = (DateTime)dr["LoginDate"];
                sysAccountInfoEntity.LogOnTimes = (Int32)dr["LogOnTimes"];
                sysAccountInfoEntity.AddDate = (DateTime)dr["AddDate"];
                list.Add(sysAccountInfoEntity);
            }
            dr.Close();
            return list;
        }

        public List<SysAccountInfoEntity> MakeSysAccountInfoList(DataTable dt)
        {
            List<SysAccountInfoEntity> list = new List<SysAccountInfoEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SysAccountInfoEntity sysAccountInfoEntity = MakeSysAccountInfo(dt.Rows[i]);
                    list.Add(sysAccountInfoEntity);
                }
            }
            return list;
        }

        #endregion
    }
}

