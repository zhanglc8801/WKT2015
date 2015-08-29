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
    public partial class FlowNodeConditionDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public FlowNodeConditionDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static FlowNodeConditionDataAccess _instance = new FlowNodeConditionDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static FlowNodeConditionDataAccess Instance
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
        public string FlowNodeConditionQueryToSQLWhere(FlowNodeConditionQuery query)
        {
            return string.Empty;
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string FlowNodeConditionQueryToSQLOrder(FlowNodeConditionQuery query)
        {
            return " FlowConditionID DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public FlowNodeConditionEntity GetFlowNodeCondition(Int64 flowConditionID)
        {
            FlowNodeConditionEntity flowNodeConditionEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  FlowConditionID,JournalID,OperationID,ConditionType,ConditionExp,AddDate FROM dbo.FlowNodeCondition WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  FlowConditionID=@FlowConditionID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@FlowConditionID",DbType.Int64,flowConditionID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                flowNodeConditionEntity = MakeFlowNodeCondition(dr);
            }
            return flowNodeConditionEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<FlowNodeConditionEntity> GetFlowNodeConditionList()
        {
            List<FlowNodeConditionEntity> flowNodeConditionEntity=new List<FlowNodeConditionEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  FlowConditionID,JournalID,OperationID,ConditionType,ConditionExp,AddDate FROM dbo.FlowNodeCondition WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                flowNodeConditionEntity = MakeFlowNodeConditionList(dr);
            }
            return flowNodeConditionEntity;
        }
        
        public List<FlowNodeConditionEntity> GetFlowNodeConditionList(FlowNodeConditionQuery query)
        {
            List<FlowNodeConditionEntity> list = new List<FlowNodeConditionEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT FlowConditionID,JournalID,OperationID,ConditionType,ConditionExp,AddDate FROM dbo.FlowNodeCondition WITH(NOLOCK)");
            string whereSQL = FlowNodeConditionQueryToSQLWhere(query);
            string orderBy=FlowNodeConditionQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeFlowNodeConditionList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<FlowNodeConditionEntity></returns>
        public Pager<FlowNodeConditionEntity> GetFlowNodeConditionPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("FlowNodeCondition","FlowConditionID,JournalID,OperationID,ConditionType,ConditionExp,AddDate",query.Order,query.Where,query.CurrentPage,query.PageSize,out recordCount);
            Pager<FlowNodeConditionEntity>  pager = new Pager<FlowNodeConditionEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeFlowNodeConditionList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<FlowNodeConditionEntity> GetFlowNodeConditionPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("FlowNodeCondition","FlowConditionID,JournalID,OperationID,ConditionType,ConditionExp,AddDate"," FlowConditionID DESC","",query.CurrentPage,query.PageSize,out recordCount);
            Pager<FlowNodeConditionEntity>  pager=new Pager<FlowNodeConditionEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeFlowNodeConditionList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<FlowNodeConditionEntity> GetFlowNodeConditionPageList(FlowNodeConditionQuery query)
        {
            int recordCount=0;
            string whereSQL=FlowNodeConditionQueryToSQLWhere(query);
            string orderBy=FlowNodeConditionQueryToSQLOrder(query);
            DataSet ds=db.GetPagingData("FlowNodeCondition","FlowConditionID,JournalID,OperationID,ConditionType,ConditionExp,AddDate",orderBy,whereSQL,query.CurrentPage,query.PageSize,out recordCount);
            Pager<FlowNodeConditionEntity>  pager=new Pager<FlowNodeConditionEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeFlowNodeConditionList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddFlowNodeCondition(FlowNodeConditionEntity flowNodeConditionEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @FlowConditionID");
            sqlCommandText.Append(", @JournalID");
            sqlCommandText.Append(", @FlowSetID");
            sqlCommandText.Append(", @ConditionType");
            sqlCommandText.Append(", @ConditionExp");
            sqlCommandText.Append(", @AddDate");
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.FlowNodeCondition ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
            
            db.AddInParameter(cmd,"@FlowConditionID",DbType.Int64,flowNodeConditionEntity.FlowConditionID);
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,flowNodeConditionEntity.JournalID);
            db.AddInParameter(cmd, "@FlowSetID", DbType.Int64, flowNodeConditionEntity.FlowSetID);
            db.AddInParameter(cmd,"@ConditionType",DbType.Byte,flowNodeConditionEntity.ConditionType);
            db.AddInParameter(cmd,"@ConditionExp",DbType.AnsiString,flowNodeConditionEntity.ConditionExp);
            db.AddInParameter(cmd,"@AddDate",DbType.DateTime,flowNodeConditionEntity.AddDate);
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
     
        public bool UpdateFlowNodeCondition(FlowNodeConditionEntity flowNodeConditionEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  FlowConditionID=@FlowConditionID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" JournalID=@JournalID");
            sqlCommandText.Append(", FlowSetID=@FlowSetID");
            sqlCommandText.Append(", ConditionType=@ConditionType");
            sqlCommandText.Append(", ConditionExp=@ConditionExp");
            sqlCommandText.Append(", AddDate=@AddDate");
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.FlowNodeCondition SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@FlowConditionID",DbType.Int64,flowNodeConditionEntity.FlowConditionID);
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,flowNodeConditionEntity.JournalID);
            db.AddInParameter(cmd, "@FlowSetID", DbType.Int64, flowNodeConditionEntity.FlowSetID);
            db.AddInParameter(cmd,"@ConditionType",DbType.Byte,flowNodeConditionEntity.ConditionType);
            db.AddInParameter(cmd,"@ConditionExp",DbType.AnsiString,flowNodeConditionEntity.ConditionExp);
            db.AddInParameter(cmd,"@AddDate",DbType.DateTime,flowNodeConditionEntity.AddDate);

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
        
        public bool DeleteFlowNodeCondition(FlowNodeConditionEntity flowNodeConditionEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.FlowNodeCondition");
            sqlCommandText.Append(" WHERE  FlowConditionID=@FlowConditionID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@FlowConditionID",DbType.Int64,flowNodeConditionEntity.FlowConditionID);
            
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
        
        public bool DeleteFlowNodeCondition(Int64 flowConditionID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.FlowNodeCondition");
            sqlCommandText.Append(" WHERE  FlowConditionID=@FlowConditionID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@FlowConditionID",DbType.Int64,flowConditionID);
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
        /// <param name="flowConditionID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteFlowNodeCondition(Int64[] flowConditionID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from FlowNodeCondition where ");
           
            for(int i=0;i<flowConditionID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( FlowConditionID=@FlowConditionID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<flowConditionID.Length;i++)
            {
            db.AddInParameter(cmd,"@FlowConditionID"+i,DbType.Int64,flowConditionID[i]);
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
        
        public FlowNodeConditionEntity MakeFlowNodeCondition(IDataReader dr)
        {
            FlowNodeConditionEntity flowNodeConditionEntity = null;
            if(dr.Read())
            {
             flowNodeConditionEntity=new FlowNodeConditionEntity();
             flowNodeConditionEntity.FlowConditionID = (Int64)dr["FlowConditionID"];
             flowNodeConditionEntity.JournalID = (Int64)dr["JournalID"];
             flowNodeConditionEntity.FlowSetID = (Int64)dr["FlowSetID"];
             flowNodeConditionEntity.ConditionType = (Byte)dr["ConditionType"];
             flowNodeConditionEntity.ConditionExp = (String)dr["ConditionExp"];
             flowNodeConditionEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return flowNodeConditionEntity;
        }
        
        public FlowNodeConditionEntity MakeFlowNodeCondition(DataRow dr)
        {
            FlowNodeConditionEntity flowNodeConditionEntity=null;
            if(dr!=null)
            {
                 flowNodeConditionEntity=new FlowNodeConditionEntity();
                 flowNodeConditionEntity.FlowConditionID = (Int64)dr["FlowConditionID"];
                 flowNodeConditionEntity.JournalID = (Int64)dr["JournalID"];
                 flowNodeConditionEntity.FlowSetID = (Int64)dr["FlowSetID"];
                 flowNodeConditionEntity.ConditionType = (Byte)dr["ConditionType"];
                 flowNodeConditionEntity.ConditionExp = (String)dr["ConditionExp"];
                 flowNodeConditionEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return flowNodeConditionEntity;
        }

        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<FlowNodeConditionEntity> MakeFlowNodeConditionList(IDataReader dr)
        {
            List<FlowNodeConditionEntity> list=new List<FlowNodeConditionEntity>();
            while(dr.Read())
            {
             FlowNodeConditionEntity flowNodeConditionEntity=new FlowNodeConditionEntity();
            flowNodeConditionEntity.FlowConditionID = (Int64)dr["FlowConditionID"];
            flowNodeConditionEntity.JournalID = (Int64)dr["JournalID"];
            flowNodeConditionEntity.FlowSetID = (Int64)dr["FlowSetID"];
            flowNodeConditionEntity.ConditionType = (Byte)dr["ConditionType"];
            flowNodeConditionEntity.ConditionExp = (String)dr["ConditionExp"];
            flowNodeConditionEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(flowNodeConditionEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<FlowNodeConditionEntity> MakeFlowNodeConditionList(DataTable dt)
        {
            List<FlowNodeConditionEntity> list=new List<FlowNodeConditionEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   FlowNodeConditionEntity flowNodeConditionEntity=MakeFlowNodeCondition(dt.Rows[i]);
                   list.Add(flowNodeConditionEntity);
                }
            }
            return list;
        }
        
        #endregion

    }
}

