using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Common.Utils;
using WKT.Data.SQL;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class FlowActionDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public FlowActionDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static FlowActionDataAccess _instance = new FlowActionDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static FlowActionDataAccess Instance
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
        public string FlowActionQueryToSQLWhere(FlowActionQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JournalID = " + query.JournalID);
            if (query.StatusID > 0)
            {
                sbWhere.Append(" AND StatusID = " + query.StatusID);
            }
            else if (query.ToStatusID > 0)
            {
                sbWhere.Append(" AND ToStatusID = " + query.ToStatusID);
            }
            else if (query.ActionName.Length > 0)
            {
                sbWhere.Append(" AND ActionName like '%" + query.ActionName+"%'");
            }
            return sbWhere.ToString();
        }

        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string FlowActionQueryToSQLOrder(FlowActionQuery query)
        {
            return " SortID ASC, ActionID DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 根据条件获取所有实体对象

        /// <summary>
        /// 获取操作实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public FlowActionEntity GetFlowActionEntity(FlowActionQuery query)
        {
            FlowActionEntity flowActionEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT [ActionID],[JournalID],[StatusID],[ActionName],[DisplayName],[ActionType],[SMSTemplate],[EmailTemplate],[TOStatusID],[CStatus],ActionRoleID,[SortID],[Status],IsShowLog,IsRetractionSendMsg FROM dbo.FlowAction fa WITH(NOLOCK) WHERE fa.JournalID=@JournalID AND fa.ActionID=@ActionID ");
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@ActionID", DbType.Int64, query.ActionID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                flowActionEntity = MakeFlowAction(dr);
            }
            return flowActionEntity;
        }

        /// <summary>
        /// 获取指定审稿状态下可以做的操作，带有权限
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<FlowActionEntity> GetFlowActionByStatus(FlowActionQuery query)
        {
            List<FlowActionEntity> list = new List<FlowActionEntity>();
            DbCommand cmd = db.GetStoredProcCommand("UP_GetHaveRightActionListByStatusID");
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@StatusID", DbType.Int64, query.StatusID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, query.AuthorID);
            db.AddInParameter(cmd, "@RoleID", DbType.Int64, query.RoleID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                FlowActionEntity flowActionEntity = null;
                while (dr.Read())
                {
                    flowActionEntity = new FlowActionEntity();
                    flowActionEntity.ActionID = (Int64)dr["ActionID"];
                    flowActionEntity.StatusID = (Int64)dr["StatusID"];
                    flowActionEntity.ActionName = (String)dr["ActionName"];
                    flowActionEntity.DisplayName = (String)dr["DisplayName"];
                    flowActionEntity.ActionType = (Byte)dr["ActionType"];
                    list.Add(flowActionEntity);
                }
                dr.Close();
            }
            return list;
        }

        /// <summary>
        /// 根据条件获取所有实体对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<FlowActionEntity> GetFlowActionList(FlowActionQuery query)
        {
            List<FlowActionEntity> list = new List<FlowActionEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT ActionID,JournalID,StatusID,ActionName,DisplayName,ActionType,CStatus,ActionRoleID,SMSTemplate,EmailTemplate,TOStatusID,Status,IsShowLog,IsRetractionSendMsg,SortID,AddDate FROM dbo.FlowAction WITH(NOLOCK)");
            string whereSQL = FlowActionQueryToSQLWhere(query);
            string orderBy = FlowActionQueryToSQLOrder(query);
            if (!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if (!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeFlowActionList(dr);
            }
            return list;
        }

        #endregion
        
        #region 保存
        
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="flowActionEntity"></param>
        /// <returns></returns>
        public bool AddFlowAction(FlowActionEntity flowActionEntity)
        {
            bool flag = false;

            DbCommand cmd = db.GetStoredProcCommand("UP_AddFlowAction");
            
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,flowActionEntity.JournalID);
            db.AddInParameter(cmd, "@StatusID", DbType.Int64, flowActionEntity.StatusID);
            db.AddInParameter(cmd,"@ActionName",DbType.AnsiString,flowActionEntity.ActionName);
            db.AddInParameter(cmd,"@DisplayName",DbType.AnsiString,flowActionEntity.DisplayName);
            db.AddInParameter(cmd,"@ActionType",DbType.Byte,flowActionEntity.ActionType);
            db.AddInParameter(cmd, "@CStatus", DbType.Int32, flowActionEntity.CStatus);
            db.AddInParameter(cmd, "@ActionRoleID", DbType.Int64, flowActionEntity.ActionRoleID);
            db.AddInParameter(cmd, "@SMSTemplate", DbType.Int32, flowActionEntity.SMSTemplate);
            db.AddInParameter(cmd, "@EmailTemplate", DbType.Int32, flowActionEntity.EmailTemplate);
            db.AddInParameter(cmd, "@TOStatusID", DbType.Int64, flowActionEntity.TOStatusID);
            db.AddInParameter(cmd, "@Status", DbType.Byte, flowActionEntity.Status);
            db.AddInParameter(cmd, "@IsShowLog", DbType.Byte, flowActionEntity.IsShowLog);
            db.AddInParameter(cmd, "@IsRetractionSendMsg", DbType.Byte, flowActionEntity.IsRetractionSendMsg);
            db.AddInParameter(cmd, "@SortID", DbType.Int32, flowActionEntity.SortID);

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
     
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="flowActionEntity"></param>
        /// <returns></returns>
        public bool UpdateFlowAction(FlowActionEntity flowActionEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append(" ActionID=@ActionID AND JournalID=@JournalID AND StatusID=@StatusID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("ActionName=@ActionName");
            sqlCommandText.Append(", DisplayName=@DisplayName");
            sqlCommandText.Append(", ActionType=@ActionType");
            sqlCommandText.Append(", CStatus=@CStatus");
            sqlCommandText.Append(", ActionRoleID=@ActionRoleID");
            sqlCommandText.Append(", SMSTemplate=@SMSTemplate");
            sqlCommandText.Append(", EmailTemplate=@EmailTemplate");
            sqlCommandText.Append(", TOStatusID=@TOStatusID");
            sqlCommandText.Append(", Status=@Status");
            sqlCommandText.Append(", IsShowLog=@IsShowLog");
            sqlCommandText.Append(", IsRetractionSendMsg=@IsRetractionSendMsg");
            sqlCommandText.Append(", SortID=@SortID");
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.FlowAction SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@ActionID",DbType.Int64,flowActionEntity.ActionID);
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,flowActionEntity.JournalID);
            db.AddInParameter(cmd, "@StatusID", DbType.Int64, flowActionEntity.StatusID);
            db.AddInParameter(cmd,"@ActionName",DbType.AnsiString,flowActionEntity.ActionName);
            db.AddInParameter(cmd,"@DisplayName",DbType.AnsiString,flowActionEntity.DisplayName);
            db.AddInParameter(cmd,"@ActionType",DbType.Byte,flowActionEntity.ActionType);
            db.AddInParameter(cmd, "@CStatus", DbType.Int32, flowActionEntity.CStatus);
            db.AddInParameter(cmd, "@ActionRoleID", DbType.Int64, flowActionEntity.ActionRoleID);
            db.AddInParameter(cmd, "@SMSTemplate", DbType.Int32, flowActionEntity.SMSTemplate);
            db.AddInParameter(cmd, "@EmailTemplate", DbType.Int32, flowActionEntity.EmailTemplate);
            db.AddInParameter(cmd, "@TOStatusID", DbType.Int64, flowActionEntity.TOStatusID);
            db.AddInParameter(cmd, "@Status", DbType.Byte, flowActionEntity.Status);
            db.AddInParameter(cmd, "@IsShowLog", DbType.Byte, flowActionEntity.IsShowLog);
            db.AddInParameter(cmd, "@IsRetractionSendMsg", DbType.Byte, flowActionEntity.IsRetractionSendMsg);
            db.AddInParameter(cmd, "@SortID", DbType.Int32, flowActionEntity.SortID);

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
        
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="flowActionEntity"></param>
        /// <returns></returns>
        public bool DeleteFlowAction(FlowActionEntity flowActionEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.FlowAction");
            sqlCommandText.Append(" WHERE  ActionID=@ActionID AND JournalID=@JournalID AND StatusID=@StatusID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd, "@ActionID",DbType.Int64,flowActionEntity.ActionID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, flowActionEntity.JournalID);
            db.AddInParameter(cmd, "@StatusID", DbType.Int64, flowActionEntity.StatusID);
            
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
        
        #region 根据数据组装一个对象
        
        public FlowActionEntity MakeFlowAction(IDataReader dr)
        {
            FlowActionEntity flowActionEntity = null;
            if (dr.Read())
            {
                flowActionEntity = new FlowActionEntity();
                flowActionEntity.ActionID = (Int64)dr["ActionID"];
                flowActionEntity.JournalID = (Int64)dr["JournalID"];
                flowActionEntity.StatusID = (Int64)dr["StatusID"];
                flowActionEntity.ActionName = (String)dr["ActionName"];
                flowActionEntity.DisplayName = (String)dr["DisplayName"];
                flowActionEntity.ActionType = (Byte)dr["ActionType"];
                flowActionEntity.CStatus = (Int32)dr["CStatus"];
                flowActionEntity.ActionRoleID = TypeParse.ToLong(dr["ActionRoleID"],0);
                flowActionEntity.SMSTemplate = (Int32)dr["SMSTemplate"];
                flowActionEntity.EmailTemplate = (Int32)dr["EmailTemplate"];
                flowActionEntity.TOStatusID = (Int64)dr["TOStatusID"];
                flowActionEntity.SortID = (Int32)dr["SortID"];
                flowActionEntity.Status = (Byte)dr["Status"];
                flowActionEntity.IsShowLog = (Byte)dr["IsShowLog"];
                flowActionEntity.IsRetractionSendMsg = (Byte)dr["IsRetractionSendMsg"];
                if(dr.HasColumn("AddDate"))
                {
                    flowActionEntity.AddDate = (DateTime)dr["AddDate"];
                }
            }
            dr.Close();
            return flowActionEntity;
        }
        
        public FlowActionEntity MakeFlowAction(DataRow dr)
        {
            FlowActionEntity flowActionEntity = new FlowActionEntity();
            flowActionEntity.ActionID = (Int64)dr["ActionID"];
            flowActionEntity.JournalID = (Int64)dr["JournalID"];
            flowActionEntity.StatusID = (Int64)dr["StatusID"];
            flowActionEntity.ActionName = (String)dr["ActionName"];
            flowActionEntity.DisplayName = (String)dr["DisplayName"];
            flowActionEntity.ActionType = (Byte)dr["ActionType"];
            flowActionEntity.CStatus = (Int32)dr["CStatus"];
            flowActionEntity.SMSTemplate = (Int32)dr["SMSTemplate"];
            flowActionEntity.EmailTemplate = (Int32)dr["EmailTemplate"];
            flowActionEntity.TOStatusID = (Int64)dr["TOStatusID"];
            flowActionEntity.SortID = (Int32)dr["SortID"];
            flowActionEntity.Status = (Byte)dr["Status"];
            flowActionEntity.IsShowLog = (Byte)dr["IsShowLog"];
            flowActionEntity.IsRetractionSendMsg = (Byte)dr["IsRetractionSendMsg"];
            flowActionEntity.AddDate = (DateTime)dr["AddDate"];
            return flowActionEntity;
        }

        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<FlowActionEntity> MakeFlowActionList(IDataReader dr)
        {
            List<FlowActionEntity> list=new List<FlowActionEntity>();
            FlowActionEntity flowActionEntity = null;
            while (dr.Read())
            {
                flowActionEntity = new FlowActionEntity();
                flowActionEntity.ActionID = (Int64)dr["ActionID"];
                flowActionEntity.JournalID = (Int64)dr["JournalID"];
                flowActionEntity.StatusID = (Int64)dr["StatusID"];
                flowActionEntity.ActionName = (String)dr["ActionName"];
                flowActionEntity.DisplayName = (String)dr["DisplayName"];
                flowActionEntity.ActionType = (Byte)dr["ActionType"];
                flowActionEntity.CStatus = (Int32)dr["CStatus"];
                flowActionEntity.ActionRoleID = TypeParse.ToLong(dr["ActionRoleID"], 0);
                flowActionEntity.SMSTemplate = (Int32)dr["SMSTemplate"];
                flowActionEntity.EmailTemplate = (Int32)dr["EmailTemplate"];
                flowActionEntity.TOStatusID = (Int64)dr["TOStatusID"];
                flowActionEntity.SortID = (Int32)dr["SortID"];
                flowActionEntity.Status = (Byte)dr["Status"];
                flowActionEntity.IsShowLog = (Byte)dr["IsShowLog"];
                flowActionEntity.IsRetractionSendMsg = (Byte)dr["IsRetractionSendMsg"];
                flowActionEntity.AddDate = (DateTime)dr["AddDate"];
                list.Add(flowActionEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<FlowActionEntity> MakeFlowActionList(DataTable dt)
        {
            List<FlowActionEntity> list=new List<FlowActionEntity>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FlowActionEntity flowActionEntity = MakeFlowAction(dt.Rows[i]);
                list.Add(flowActionEntity);
            }
            return list;
        }
        
        #endregion

    }
}

