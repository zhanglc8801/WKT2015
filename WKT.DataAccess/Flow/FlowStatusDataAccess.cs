using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Data.SQL;
using WKT.Common.Utils;
using WKT.Common.Security;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class FlowStatusDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public FlowStatusDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static FlowStatusDataAccess _instance = new FlowStatusDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static FlowStatusDataAccess Instance
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
        public string FlowStatusQueryToSQLWhere(FlowStatusQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JournalID = " + query.JournalID);
            if (query.StatusID != null)
            {
                sbWhere.Append(" AND StatusID = " + query.StatusID.Value);
            }
            return sbWhere.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string FlowStatusQueryToSQLOrder(FlowStatusQuery query)
        {
            return " SortID ASC,StatusID DESC";
        }
        
        #endregion 组装SQL条件

        # region 获取审稿状态序号

        /// <summary>
        /// 获取审稿环节序号
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetFlowStatusSortID(FlowStatusQuery query)
        {
            int SortID = 0;
            string sql = "SELECT TOP 1 SortID FROM dbo.FlowStatus f WITH(NOLOCK) WHERE JournalID=@JournalID ORDER BY SortID DESC";
            DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,query.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    SortID = TypeParse.ToInt(dr["SortID"], 0);
                }
                dr.Close();
                SortID = SortID + 1;
            }
            return SortID;
        }

        # endregion

        # region 判断审稿状态对应的稿件状态是否存在

        /// <summary>
        /// 判断审稿状态对应的稿件状态是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public FlowStatusEntity CheckCStatusIsExists(FlowStatusQuery query)
        {
            FlowStatusEntity statusEntity = null;
            string sql = "SELECT TOP 1 StatusID,StatusName FROM dbo.FlowStatus f WITH(NOLOCK) WHERE JournalID=@JournalID AND CStatus=@CStatus ORDER BY SortID DESC";
            DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@CStatus", DbType.Int32, query.CStatus);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    statusEntity = new FlowStatusEntity();
                    statusEntity.StatusID = (Int64)dr["StatusID"];
                    statusEntity.StatusName = dr["StatusName"].ToString();
                }
                dr.Close();
            }
            return statusEntity;
        }

        # endregion

        # region 根据条件获取所有实体对象

        /// <summary>
        /// 获取拥有权限的审稿状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<FlowStatusEntity> GetHaveRightFlowStatus(FlowStatusQuery query)
        {
            IList<FlowStatusEntity> listStatus = new List<FlowStatusEntity>();
            string strSotreName = "UP_GetHaveRightStatusList";

            if(query.IsHandled == 0)// 待处理
            {
                strSotreName = "UP_GetHaveRightStatusListByNoHanded"; 
            }
            else if (query.IsHandled == 1)//已处理
            {
                strSotreName = "UP_GetHaveRightStatusListByHanded";
            }
            else if (query.IsHandled == 3)//全部已处理
            {
                strSotreName = "UP_GetHaveRightStatusListByHandedAll";
            }
            DbCommand cmd = db.GetStoredProcCommand(strSotreName);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, query.CurAuthorID);
            db.AddInParameter(cmd, "@RoleID", DbType.Int64, query.RoleID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                FlowStatusEntity statusEntity = null;
                while (dr.Read())
                {
                    statusEntity = new FlowStatusEntity();
                    statusEntity.StatusID = TypeParse.ToLong(dr["StatusID"], 0);
                    statusEntity.StatusName = dr["StatusName"].ToString();
                    statusEntity.ContributionCount = TypeParse.ToInt(dr["ContributionCount"], 0) ;
                    statusEntity.CStatus = TypeParse.ToInt(dr["CStatus"], 0);
                    listStatus.Add(statusEntity);
                }
                dr.Close();
            }
            return listStatus;
        }

        /// <summary>
        /// 获取拥有权限的审稿状态(用于统计同一稿件一个状态下送多人时按一个计算)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<FlowStatusEntity> GetHaveRightFlowStatusForStat(FlowStatusQuery query)
        {
            IList<FlowStatusEntity> listStatus = new List<FlowStatusEntity>();
            string strSotreName = "UP_GetHaveRightStatusList";

            if (query.IsHandled == 0)// 待处理
            {
                strSotreName = "UP_GetHaveRightStatusListByNoHandedForStat";
            }
            else if (query.IsHandled == 1)
            {
                strSotreName = "UP_GetHaveRightStatusListByHandedForStat";
            }
            DbCommand cmd = db.GetStoredProcCommand(strSotreName);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, query.CurAuthorID);
            db.AddInParameter(cmd, "@RoleID", DbType.Int64, query.RoleID);
            db.AddInParameter(cmd, "@StartDate", DbType.DateTime, query.StartDate == null ? Convert.ToDateTime("2000-01-01") : query.StartDate);
            db.AddInParameter(cmd, "@EndDate", DbType.DateTime, query.EndDate == null ? DateTime.Now.AddDays(1) : query.EndDate.Value.AddDays(1));
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                FlowStatusEntity statusEntity = null;
                while (dr.Read())
                {
                    statusEntity = new FlowStatusEntity();
                    statusEntity.StatusID = TypeParse.ToLong(dr["StatusID"], 0);
                    statusEntity.StatusName = dr["StatusName"].ToString();
                    //statusEntity.RoleID = TypeParse.ToInt(dr["RoleID"], 0);
                    statusEntity.ContributionCount = TypeParse.ToInt(dr["ContributionCount"], 0);
                    statusEntity.CStatus = TypeParse.ToInt(dr["CStatus"], 0);
                    listStatus.Add(statusEntity);
                }
                dr.Close();
            }
            return listStatus;
        }

        /// <summary>
        /// 获取审稿状态键值对，审稿状态名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long,string> GetFlowStatusDictStatusName(FlowStatusQuery query)
        {
            IDictionary<long, string> dictStatusName = new Dictionary<long, string>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT StatusID,StatusName FROM dbo.FlowStatus WITH(NOLOCK) WHERE JournalID=@JournalID AND Status=1 ORDER BY SortID ASC,StatusID ASC");
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            long StatusID = 0;
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    StatusID = TypeParse.ToLong(dr["StatusID"], 0);
                    if (!dictStatusName.ContainsKey(StatusID))
                    {
                        dictStatusName.Add(StatusID, dr["StatusName"].ToString());
                    }
                }
                dr.Close();
            }
            return dictStatusName;
        }

        /// <summary>
        /// 获取审稿状态键值对，审稿状态显示名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long, string> GetFlowStatusDictDisplayName(FlowStatusQuery query)
        {
            IDictionary<long, string> dictDisplayName = new Dictionary<long, string>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT StatusID,DisplayName FROM dbo.FlowStatus WITH(NOLOCK) WHERE JournalID=@JournalID AND Status=1 ORDER BY SortID ASC,StatusID ASC");
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            long StatusID = 0;
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    StatusID = TypeParse.ToLong(dr["StatusID"], 0);
                    if (!dictDisplayName.ContainsKey(StatusID))
                    {
                        dictDisplayName.Add(StatusID, dr["DisplayName"].ToString());
                    }
                }
                dr.Close();
            }
            return dictDisplayName;
        }

        /// <summary>
        /// 获取所有满足查询条件的审稿状态列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<FlowStatusEntity> GetFlowStatusList(FlowStatusQuery query)
        {
            List<FlowStatusEntity> list = new List<FlowStatusEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT StatusID,JournalID,StatusName,DisplayName,Status,SortID,RoleID,ActionRoleID,CStatus FROM dbo.FlowStatus WITH(NOLOCK)");
            string whereSQL = FlowStatusQueryToSQLWhere(query);
            string orderBy = FlowStatusQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeFlowStatusList(dr);
            }
            return list;
        }
       
        #endregion

        # region 根据指定的审稿状态ID，得到审稿状态的基本信息

        /// <summary>
        /// 根据指定的审稿状态ID，得到审稿状态的基本信息
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="StatusID"></param>
        /// <returns></returns>
        public FlowStatusEntity GetFlowStatusInfoByID(FlowStatusQuery query)
        {
            FlowStatusEntity flowStatusEntity = null;

            # region 获取流程步骤基本信息

            StringBuilder sqlGetFlowSetText = new StringBuilder();
            sqlGetFlowSetText.Append("SELECT TOP 1 StatusID,JournalID,StatusName,DisplayName,Status,SortID,RoleID,ActionRoleID,CStatus FROM dbo.FlowSet WITH(NOLOCK)");
            sqlGetFlowSetText.Append(" WHERE  FlowSetID=@FlowSetID AND JournalID=@JournalID ");

            DbCommand cmd = db.GetSqlStringCommand(sqlGetFlowSetText.ToString());
            db.AddInParameter(cmd, "@StatusID", DbType.Int64, query.StatusID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    flowStatusEntity = new FlowStatusEntity();
                    flowStatusEntity.StatusID = (Int64)dr["StatusID"];
                    flowStatusEntity.JournalID = (Int64)dr["JournalID"];
                    flowStatusEntity.StatusName = (String)dr["StatusName"];
                    flowStatusEntity.DisplayName = (String)dr["DisplayName"];
                    flowStatusEntity.Status = (Byte)dr["Status"];
                    flowStatusEntity.SortID = (Int32)dr["SortID"];
                    flowStatusEntity.RoleID = (Int64)dr["RoleID"];
                    flowStatusEntity.ActionRoleID = (Int64)dr["ActionRoleID"];
                    flowStatusEntity.CStatus = (Int32)dr["CStatus"];
                }
                dr.Close();
            }

            # endregion

            return flowStatusEntity;
        }

        # endregion

        # region 获取审稿流程状态基本信息及配置信息

        /// <summary>
        /// 获取审稿流程基本信息及配置信息
        /// </summary>
        /// <param name="flowSetQuery"></param>
        /// <returns></returns>
        public FlowStep GetFlowStep(FlowStatusQuery flowStatusQuery)
        {
            FlowStep flowStep = new FlowStep();

            # region 获取流程状态基本信息

            StringBuilder sqlGetFlowSetText = new StringBuilder();
            sqlGetFlowSetText.Append("SELECT TOP 1 StatusID,JournalID,StatusName,DisplayName,Status,SortID,RoleID,ActionRoleID,CStatus,ContributionCount,Remark,EditDate,EditAuthorID,InAuthorID,AddDate FROM dbo.FlowStatus WITH(NOLOCK)");
            sqlGetFlowSetText.Append(" WHERE  StatusID=@StatusID AND JournalID=@JournalID ");

            DbCommand cmd = db.GetSqlStringCommand(sqlGetFlowSetText.ToString());
            db.AddInParameter(cmd, "@StatusID", DbType.Int64, flowStatusQuery.StatusID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, flowStatusQuery.JournalID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                flowStep.FlowStatus = MakeFlowStatus(dr);
            }

            # endregion

            # region 获取流程状态配置信息

            StringBuilder sqlGetFlowConfigText = new StringBuilder();
            sqlGetFlowConfigText.Append("SELECT TOP 1  FlowConfigID,JournalID,StatusID,IsAllowBack,IsMultiPerson,MultiPattern,TimeoutDay,TimeoutPattern,IsSMSRemind,IsEmailRemind,RangeDay,RemindCount,IsRetraction,AddDate FROM dbo.FlowConfig WITH(NOLOCK)");
            sqlGetFlowConfigText.Append(" WHERE  StatusID=@StatusID AND JournalID=@JournalID ");

            DbCommand cmdGetFlowConfig = db.GetSqlStringCommand(sqlGetFlowConfigText.ToString());
            db.AddInParameter(cmdGetFlowConfig, "@StatusID", DbType.Int64, flowStatusQuery.StatusID);
            db.AddInParameter(cmdGetFlowConfig, "@JournalID", DbType.Int64, flowStatusQuery.JournalID);

            using (IDataReader dr = db.ExecuteReader(cmdGetFlowConfig))
            {
                flowStep.FlowConfig = MakeFlowConfig(dr);
            }

            # endregion

            return flowStep;
        }

        # endregion

        #region 新增审稿流程状态及配置

        /// <summary>
        /// 新增审稿流程状态及配置
        /// </summary>
        /// <param name="flowStepEntity"></param>
        /// <returns></returns>
        public bool AddFlowSet(FlowStep flowStepEntity)
        {
            bool flag = false;

            IDbConnection connection = db.GetConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            long FlowStatusID = 0;
            try
            {
                # region insert FlowStatus ,流程状态基本信息

                StringBuilder sqlInsertFlowSetText = new StringBuilder();
                sqlInsertFlowSetText.Append("@JournalID");
                sqlInsertFlowSetText.Append(", @StatusName");
                sqlInsertFlowSetText.Append(", @DisplayName");
                sqlInsertFlowSetText.Append(", @Status");
                sqlInsertFlowSetText.Append(", @SortID");
                sqlInsertFlowSetText.Append(", @RoleID"); 
                sqlInsertFlowSetText.Append(", @ActionRoleID");
                sqlInsertFlowSetText.Append(", @CStatus");
                sqlInsertFlowSetText.Append(", @Remark");
                sqlInsertFlowSetText.Append(", @InAuthorID");

                DbCommand insertFlowSetCmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.FlowStatus ({0}) VALUES ({1}) SELECT @@IDENTITY", sqlInsertFlowSetText.ToString().Replace("@", ""), sqlInsertFlowSetText.ToString()));

                db.AddInParameter(insertFlowSetCmd, "@JournalID", DbType.Int64, flowStepEntity.FlowStatus.JournalID);
                db.AddInParameter(insertFlowSetCmd, "@StatusName", DbType.AnsiString, flowStepEntity.FlowStatus.StatusName);
                db.AddInParameter(insertFlowSetCmd, "@DisplayName", DbType.AnsiString, flowStepEntity.FlowStatus.DisplayName);
                db.AddInParameter(insertFlowSetCmd, "@Status", DbType.Byte, flowStepEntity.FlowStatus.Status);
                db.AddInParameter(insertFlowSetCmd, "@SortID", DbType.Int32, flowStepEntity.FlowStatus.SortID);
                db.AddInParameter(insertFlowSetCmd, "@RoleID", DbType.Int64, flowStepEntity.FlowStatus.RoleID);
                db.AddInParameter(insertFlowSetCmd, "@ActionRoleID", DbType.Int64, flowStepEntity.FlowStatus.ActionRoleID);
                db.AddInParameter(insertFlowSetCmd, "@CStatus", DbType.Int32, flowStepEntity.FlowStatus.CStatus);
                db.AddInParameter(insertFlowSetCmd, "@Remark", DbType.AnsiString, flowStepEntity.FlowStatus.Remark);
                db.AddInParameter(insertFlowSetCmd, "@InAuthorID", DbType.Int64, flowStepEntity.FlowStatus.InAuthorID);

                object objFlowStatusID = db.ExecuteScalar(insertFlowSetCmd, (DbTransaction)transaction);
                if (objFlowStatusID == null)
                {
                    throw new Exception("新增审稿状态出现异常，得到的流程状态ID为空");
                }
                FlowStatusID = TypeParse.ToLong(objFlowStatusID, 0);
                if (FlowStatusID == 0)
                {
                    throw new Exception("新增审稿状态出现异常，得到的流程状态ID为0");
                }

                # endregion

                # region Insert FlowConfig,流程状态配置信息

                StringBuilder sqlInsertFlowConfig = new StringBuilder();

                sqlInsertFlowConfig.Append("@JournalID");
                sqlInsertFlowConfig.Append(", @StatusID");
                sqlInsertFlowConfig.Append(", @IsAllowBack");
                sqlInsertFlowConfig.Append(", @IsMultiPerson");
                sqlInsertFlowConfig.Append(", @MultiPattern");
                sqlInsertFlowConfig.Append(", @TimeoutDay");
                sqlInsertFlowConfig.Append(", @TimeoutPattern");
                sqlInsertFlowConfig.Append(", @IsSMSRemind");
                sqlInsertFlowConfig.Append(", @IsEmailRemind");
                sqlInsertFlowConfig.Append(", @RangeDay");
                sqlInsertFlowConfig.Append(", @RemindCount");
                sqlInsertFlowConfig.Append(", @IsRetraction");

                DbCommand cmdInsertFlowConfig = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.FlowConfig ({0}) VALUES ({1})", sqlInsertFlowConfig.ToString().Replace("@", ""), sqlInsertFlowConfig.ToString()));

                db.AddInParameter(cmdInsertFlowConfig, "@JournalID", DbType.Int64, flowStepEntity.FlowConfig.JournalID);
                db.AddInParameter(cmdInsertFlowConfig, "@StatusID", DbType.Int64, FlowStatusID);
                db.AddInParameter(cmdInsertFlowConfig, "@IsAllowBack", DbType.Byte, flowStepEntity.FlowConfig.IsAllowBack);
                db.AddInParameter(cmdInsertFlowConfig, "@IsMultiPerson", DbType.Boolean, flowStepEntity.FlowConfig.IsMultiPerson);
                db.AddInParameter(cmdInsertFlowConfig, "@MultiPattern", DbType.Byte, flowStepEntity.FlowConfig.MultiPattern);
                db.AddInParameter(cmdInsertFlowConfig, "@TimeoutDay", DbType.Int32, flowStepEntity.FlowConfig.TimeoutDay);
                db.AddInParameter(cmdInsertFlowConfig, "@TimeoutPattern", DbType.Byte, flowStepEntity.FlowConfig.TimeoutPattern);
                db.AddInParameter(cmdInsertFlowConfig, "@IsSMSRemind", DbType.Boolean, flowStepEntity.FlowConfig.IsSMSRemind);
                db.AddInParameter(cmdInsertFlowConfig, "@IsEmailRemind", DbType.Boolean, flowStepEntity.FlowConfig.IsEmailRemind);
                db.AddInParameter(cmdInsertFlowConfig, "@RangeDay", DbType.Int32, flowStepEntity.FlowConfig.RangeDay);
                db.AddInParameter(cmdInsertFlowConfig, "@RemindCount", DbType.Int32, flowStepEntity.FlowConfig.RemindCount);
                db.AddInParameter(cmdInsertFlowConfig, "@IsRetraction", DbType.Boolean, flowStepEntity.FlowConfig.IsRetraction);

                db.ExecuteNonQuery(cmdInsertFlowConfig,(DbTransaction)transaction);

                # endregion

                transaction.Commit();
                flag = true;
            }
            catch (SqlException sqlEx)
            {
                transaction.Rollback();
                throw sqlEx;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return flag;
        }
        
        #endregion

        #region 更新流程状态基本信息及配置信息

        /// <summary>
        /// 更新流程状态基本信息及配置信息
        /// </summary>
        /// <param name="flowStepEntity"></param>
        /// <returns></returns>
        public bool UpdateFlowStatus(FlowStep flowStepEntity)
        {
            bool flag = false;

            IDbConnection connection = db.GetConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            try
            {
                # region 更新审稿状态基本信息

                StringBuilder whereUpdateFlowStatusText = new StringBuilder();
                whereUpdateFlowStatusText.Append(" StatusID=@StatusID AND JournalID=@JournalID ");

                StringBuilder sqlUpdateFlowStatusText = new StringBuilder();
                sqlUpdateFlowStatusText.Append("StatusName=@StatusName");
                sqlUpdateFlowStatusText.Append(", DisplayName=@DisplayName");
                sqlUpdateFlowStatusText.Append(", Status=@Status");
                sqlUpdateFlowStatusText.Append(", SortID=@SortID");
                sqlUpdateFlowStatusText.Append(", RoleID=@RoleID"); 
                sqlUpdateFlowStatusText.Append(", ActionRoleID=@ActionRoleID");
                sqlUpdateFlowStatusText.Append(", CStatus=@CStatus");
                sqlUpdateFlowStatusText.Append(", Remark=@Remark");
                sqlUpdateFlowStatusText.Append(", EditDate=@EditDate");
                sqlUpdateFlowStatusText.Append(", EditAuthorID=@EditAuthorID");

                DbCommand cmdUpdateFlowSet = db.GetSqlStringCommand(String.Format("UPDATE dbo.FlowStatus SET {0} WHERE  {1}", sqlUpdateFlowStatusText.ToString(), whereUpdateFlowStatusText.ToString()));

                db.AddInParameter(cmdUpdateFlowSet, "@StatusID", DbType.Int64, flowStepEntity.FlowStatus.StatusID);
                db.AddInParameter(cmdUpdateFlowSet, "@JournalID", DbType.Int64, flowStepEntity.FlowStatus.JournalID);
                db.AddInParameter(cmdUpdateFlowSet, "@StatusName", DbType.AnsiString, flowStepEntity.FlowStatus.StatusName);
                db.AddInParameter(cmdUpdateFlowSet, "@DisplayName", DbType.AnsiString, flowStepEntity.FlowStatus.DisplayName);
                db.AddInParameter(cmdUpdateFlowSet, "@Status", DbType.Byte, flowStepEntity.FlowStatus.Status);
                db.AddInParameter(cmdUpdateFlowSet, "@SortID", DbType.Int32, flowStepEntity.FlowStatus.SortID);
                db.AddInParameter(cmdUpdateFlowSet, "@RoleID", DbType.Int64, flowStepEntity.FlowStatus.RoleID);
                db.AddInParameter(cmdUpdateFlowSet, "@ActionRoleID", DbType.Int64, flowStepEntity.FlowStatus.ActionRoleID);
                db.AddInParameter(cmdUpdateFlowSet, "@CStatus", DbType.Int32, flowStepEntity.FlowStatus.CStatus);
                db.AddInParameter(cmdUpdateFlowSet, "@Remark", DbType.AnsiString, flowStepEntity.FlowStatus.Remark);
                db.AddInParameter(cmdUpdateFlowSet, "@EditDate", DbType.DateTime, flowStepEntity.FlowStatus.EditDate);
                db.AddInParameter(cmdUpdateFlowSet, "@EditAuthorID", DbType.Int64, flowStepEntity.FlowStatus.EditAuthorID);

                db.ExecuteNonQuery(cmdUpdateFlowSet, (DbTransaction)transaction);

                # endregion

                # region 更新审稿配置信息

                StringBuilder whereUpdateFlowConfigText = new StringBuilder();
                whereUpdateFlowConfigText.Append("  FlowConfigID=@FlowConfigID AND JournalID=@JournalID AND StatusID=@StatusID");
                StringBuilder sqlUpdteFlowConfigText = new StringBuilder();
                sqlUpdteFlowConfigText.Append("IsAllowBack=@IsAllowBack");
                sqlUpdteFlowConfigText.Append(", IsMultiPerson=@IsMultiPerson");
                sqlUpdteFlowConfigText.Append(", MultiPattern=@MultiPattern");
                sqlUpdteFlowConfigText.Append(", TimeoutDay=@TimeoutDay");
                sqlUpdteFlowConfigText.Append(", TimeoutPattern=@TimeoutPattern");
                sqlUpdteFlowConfigText.Append(", IsSMSRemind=@IsSMSRemind");
                sqlUpdteFlowConfigText.Append(", IsEmailRemind=@IsEmailRemind");
                sqlUpdteFlowConfigText.Append(", RangeDay=@RangeDay");
                sqlUpdteFlowConfigText.Append(", RemindCount=@RemindCount");
                sqlUpdteFlowConfigText.Append(", IsRetraction=@IsRetraction");

                DbCommand cmdUpdateFlowConfig = db.GetSqlStringCommand(String.Format("UPDATE dbo.FlowConfig SET {0} WHERE  {1}", sqlUpdteFlowConfigText.ToString(), whereUpdateFlowConfigText.ToString()));

                db.AddInParameter(cmdUpdateFlowConfig, "@FlowConfigID", DbType.Int64, flowStepEntity.FlowConfig.FlowConfigID);
                db.AddInParameter(cmdUpdateFlowConfig, "@JournalID", DbType.Int64, flowStepEntity.FlowConfig.JournalID);
                db.AddInParameter(cmdUpdateFlowConfig, "@StatusID", DbType.Int64, flowStepEntity.FlowConfig.StatusID);
                db.AddInParameter(cmdUpdateFlowConfig, "@IsAllowBack", DbType.Byte, flowStepEntity.FlowConfig.IsAllowBack);
                db.AddInParameter(cmdUpdateFlowConfig, "@IsMultiPerson", DbType.Boolean, flowStepEntity.FlowConfig.IsMultiPerson);
                db.AddInParameter(cmdUpdateFlowConfig, "@MultiPattern", DbType.Byte, flowStepEntity.FlowConfig.MultiPattern);
                db.AddInParameter(cmdUpdateFlowConfig, "@TimeoutDay", DbType.Int32, flowStepEntity.FlowConfig.TimeoutDay);
                db.AddInParameter(cmdUpdateFlowConfig, "@TimeoutPattern", DbType.Byte, flowStepEntity.FlowConfig.TimeoutPattern);
                db.AddInParameter(cmdUpdateFlowConfig, "@IsSMSRemind", DbType.Boolean, flowStepEntity.FlowConfig.IsSMSRemind);
                db.AddInParameter(cmdUpdateFlowConfig, "@IsEmailRemind", DbType.Boolean, flowStepEntity.FlowConfig.IsEmailRemind);
                db.AddInParameter(cmdUpdateFlowConfig, "@RangeDay", DbType.Int32, flowStepEntity.FlowConfig.RangeDay);
                db.AddInParameter(cmdUpdateFlowConfig, "@RemindCount", DbType.Int32, flowStepEntity.FlowConfig.RemindCount);
                db.AddInParameter(cmdUpdateFlowConfig, "@IsRetraction", DbType.Boolean, flowStepEntity.FlowConfig.IsRetraction);

                db.ExecuteNonQuery(cmdUpdateFlowConfig, (DbTransaction)transaction);

                # endregion

                transaction.Commit();
                flag = true;
            }
            catch (SqlException sqlEx)
            {
                transaction.Rollback();
                throw sqlEx;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return flag;
        }
        
        #endregion

        #region 删除流程状态及状态配置信息

        /// <summary>
        /// 删除流程状态及状态配置信息
        /// </summary>
        /// <param name="flowStatusEntity"></param>
        /// <returns></returns>
        public bool DeleteFlowStatus(FlowStatusEntity flowStatusEntity)
        {
            bool flag = false;

            IDbConnection connection = db.GetConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            try
            {
                # region 删除流程状态配置信息

                StringBuilder sqlDelFlowConfigText = new StringBuilder();
                sqlDelFlowConfigText.Append("DELETE FROM dbo.FlowConfig");
                sqlDelFlowConfigText.Append(" WHERE  StatusID=@StatusID AND JournalID=@JournalID ");
                DbCommand cmdDelFlowConfig = db.GetSqlStringCommand(sqlDelFlowConfigText.ToString());
                db.AddInParameter(cmdDelFlowConfig, "@StatusID", DbType.Int64, flowStatusEntity.StatusID);
                db.AddInParameter(cmdDelFlowConfig, "@JournalID", DbType.Int64, flowStatusEntity.JournalID);
                db.ExecuteNonQuery(cmdDelFlowConfig, (DbTransaction)transaction);

                # endregion

                # region 删除流程状态基本信息

                StringBuilder sqlDelFlowSetText = new StringBuilder();
                sqlDelFlowSetText.Append("DELETE FROM dbo.FlowStatus");
                sqlDelFlowSetText.Append(" WHERE  StatusID=@StatusID AND JournalID=@JournalID");
                DbCommand cmdDelFlowSet = db.GetSqlStringCommand(sqlDelFlowSetText.ToString());
                db.AddInParameter(cmdDelFlowSet, "@StatusID", DbType.Int64, flowStatusEntity.StatusID);
                db.AddInParameter(cmdDelFlowSet, "@JournalID", DbType.Int64, flowStatusEntity.JournalID);
                db.ExecuteNonQuery(cmdDelFlowSet, (DbTransaction)transaction);

                # endregion

                transaction.Commit();
                flag = true;
            }
            catch (SqlException sqlEx)
            {
                transaction.Rollback();
                throw sqlEx;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }

            return flag;
        }
        
        #endregion

        #region 修改审稿状态状态

        /// <summary>
        /// 修改审稿状态状态
        /// </summary>
        /// <param name="flowStatusEntity"></param>
        /// <returns></returns>
        public bool UpdateFlowStatusStatus(FlowStatusEntity flowStatusEntity)
        {
            bool flag = false;

            try
            {
                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("UPDATE dbo.FlowStatus SET Status=@Status");
                sqlText.Append(" WHERE  StatusID=@StatusID AND JournalID=@JournalID");
                DbCommand cmdUpdate = db.GetSqlStringCommand(sqlText.ToString());
                db.AddInParameter(cmdUpdate, "@StatusID", DbType.Int64, flowStatusEntity.StatusID);
                db.AddInParameter(cmdUpdate, "@JournalID", DbType.Int64, flowStatusEntity.JournalID);
                db.AddInParameter(cmdUpdate, "@Status", DbType.Byte, flowStatusEntity.Status);
                db.ExecuteNonQuery(cmdUpdate);

                flag = true;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }

            return flag;
        }

        #endregion

        #region 根据数据组装一个对象

        public FlowConfigEntity MakeFlowConfig(IDataReader dr)
        {
            FlowConfigEntity flowConfigEntity = null;
            if (dr.Read())
            {
                flowConfigEntity = new FlowConfigEntity();
                flowConfigEntity.FlowConfigID = (Int64)dr["FlowConfigID"];
                flowConfigEntity.JournalID = (Int64)dr["JournalID"];
                flowConfigEntity.StatusID = (Int64)dr["StatusID"];
                flowConfigEntity.IsAllowBack = (Byte)dr["IsAllowBack"];
                flowConfigEntity.IsMultiPerson = (Boolean)dr["IsMultiPerson"];
                flowConfigEntity.MultiPattern = (Byte)dr["MultiPattern"];
                flowConfigEntity.TimeoutDay = (Int32)dr["TimeoutDay"];
                flowConfigEntity.TimeoutPattern = (Byte)dr["TimeoutPattern"];
                flowConfigEntity.IsSMSRemind = (Boolean)dr["IsSMSRemind"];
                flowConfigEntity.IsEmailRemind = (Boolean)dr["IsEmailRemind"];
                flowConfigEntity.RangeDay = (Int32)dr["RangeDay"];
                flowConfigEntity.RemindCount = (Int32)dr["RemindCount"];
                flowConfigEntity.IsRetraction = (Boolean)dr["IsRetraction"];
                flowConfigEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return flowConfigEntity;
        }
        
        public FlowStatusEntity MakeFlowStatus(IDataReader dr)
        {
            FlowStatusEntity flowStatusEntity = null;
            if (dr.Read())
            {
                flowStatusEntity = new FlowStatusEntity();
                flowStatusEntity.StatusID = (Int64)dr["StatusID"];
                flowStatusEntity.JournalID = (Int64)dr["JournalID"];
                flowStatusEntity.StatusName = (String)dr["StatusName"];
                flowStatusEntity.DisplayName = (String)dr["DisplayName"];
                flowStatusEntity.Status = (Byte)dr["Status"];
                flowStatusEntity.SortID = (Int32)dr["SortID"];
                flowStatusEntity.RoleID = (Int64)dr["RoleID"]; 
                flowStatusEntity.ActionRoleID = (Int64)dr["ActionRoleID"];
                flowStatusEntity.CStatus = (Int32)dr["CStatus"];
                flowStatusEntity.ContributionCount = (Int32)dr["ContributionCount"];
                flowStatusEntity.Remark = Convert.IsDBNull(dr["Remark"]) ? null : (String)dr["Remark"];
                flowStatusEntity.EditDate = (DateTime)dr["EditDate"];
                flowStatusEntity.EditAuthorID = (Int64)dr["EditAuthorID"];
                flowStatusEntity.InAuthorID = (Int64)dr["InAuthorID"];
                flowStatusEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return flowStatusEntity;
        }
        
        public FlowStatusEntity MakeFlowStatus(DataRow dr)
        {
            FlowStatusEntity flowStatusEntity = null;
            if (dr != null)
            {
                flowStatusEntity = new FlowStatusEntity();
                flowStatusEntity.StatusID = (Int64)dr["StatusID"];
                flowStatusEntity.JournalID = (Int64)dr["JournalID"];
                flowStatusEntity.StatusName = (String)dr["StatusName"];
                flowStatusEntity.DisplayName = (String)dr["DisplayName"];
                flowStatusEntity.Status = (Byte)dr["Status"];
                flowStatusEntity.SortID = (Int32)dr["SortID"];
                flowStatusEntity.RoleID = (Int64)dr["RoleID"];
                flowStatusEntity.ActionRoleID = (Int64)dr["ActionRoleID"];
                flowStatusEntity.CStatus = (Int32)dr["CStatus"];
                flowStatusEntity.ContributionCount = (Int32)dr["ContributionCount"];
                flowStatusEntity.Remark = Convert.IsDBNull(dr["Remark"]) ? null : (String)dr["Remark"];
                flowStatusEntity.EditDate = (DateTime)dr["EditDate"];
                flowStatusEntity.EditAuthorID = (Int64)dr["EditAuthorID"];
                flowStatusEntity.InAuthorID = (Int64)dr["InAuthorID"];
                flowStatusEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return flowStatusEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据

        public List<FlowStatusEntity> MakeFlowStatusList(IDataReader dr)
        {
            List<FlowStatusEntity> list = new List<FlowStatusEntity>();
            FlowStatusEntity flowStatusEntity = null;
            while (dr.Read())
            {
                flowStatusEntity = new FlowStatusEntity();
                flowStatusEntity.StatusID = (Int64)dr["StatusID"];
                flowStatusEntity.JournalID = (Int64)dr["JournalID"];
                flowStatusEntity.StatusName = (String)dr["StatusName"];
                flowStatusEntity.DisplayName = (String)dr["DisplayName"];
                flowStatusEntity.Status = (Byte)dr["Status"];
                flowStatusEntity.SortID = (Int32)dr["SortID"];
                flowStatusEntity.RoleID = (Int64)dr["RoleID"];
                flowStatusEntity.ActionRoleID = (Int64)dr["ActionRoleID"];
                flowStatusEntity.CStatus = (Int32)dr["CStatus"];
                list.Add(flowStatusEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<FlowStatusEntity> MakeFlowStatusList(DataTable dt)
        {
            List<FlowStatusEntity> list = new List<FlowStatusEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FlowStatusEntity flowStatusEntity = MakeFlowStatus(dt.Rows[i]);
                    list.Add(flowStatusEntity);
                }
            }
            return list;
        }
        
        #endregion

    }
}

