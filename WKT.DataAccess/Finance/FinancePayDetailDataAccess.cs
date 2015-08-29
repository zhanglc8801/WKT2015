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
    public partial class FinancePayDetailDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public FinancePayDetailDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static FinancePayDetailDataAccess _instance = new FinancePayDetailDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static FinancePayDetailDataAccess Instance
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
        public string FinancePayDetailQueryToSQLWhere(FinancePayDetailQuery query)
        {
            return string.Empty;
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string FinancePayDetailQueryToSQLOrder(FinancePayDetailQuery query)
        {
            return " BillID DESC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public FinancePayDetailEntity GetFinancePayDetail(Int64 billID)
        {
            FinancePayDetailEntity financePayDetailEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  BillID,JournalID,AuthorID,EBankType,TransactionID,TotalFee,Currency,IsInCome,PayType,PayStatus,PayDate,ProductTable,ProductID,ProductDes,UserAccount,BankID,BankNo,AddDate FROM dbo.FinancePayDetail WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  BillID=@BillID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@BillID",DbType.Int64,billID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                financePayDetailEntity = MakeFinancePayDetail(dr);
            }
            return financePayDetailEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<FinancePayDetailEntity> GetFinancePayDetailList()
        {
            List<FinancePayDetailEntity> financePayDetailEntity=new List<FinancePayDetailEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  BillID,JournalID,AuthorID,EBankType,TransactionID,TotalFee,Currency,IsInCome,PayType,PayStatus,PayDate,ProductTable,ProductID,ProductDes,UserAccount,BankID,BankNo,AddDate FROM dbo.FinancePayDetail WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                financePayDetailEntity = MakeFinancePayDetailList(dr);
            }
            return financePayDetailEntity;
        }
        
        public List<FinancePayDetailEntity> GetFinancePayDetailList(FinancePayDetailQuery query)
        {
            List<FinancePayDetailEntity> list = new List<FinancePayDetailEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT BillID,JournalID,AuthorID,EBankType,TransactionID,TotalFee,Currency,IsInCome,PayType,PayStatus,PayDate,ProductTable,ProductID,ProductDes,UserAccount,BankID,BankNo,AddDate FROM dbo.FinancePayDetail WITH(NOLOCK)");
            string whereSQL = FinancePayDetailQueryToSQLWhere(query);
            string orderBy=FinancePayDetailQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeFinancePayDetailList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<FinancePayDetailEntity></returns>
        public Pager<FinancePayDetailEntity> GetFinancePayDetailPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("FinancePayDetail","BillID,JournalID,AuthorID,EBankType,TransactionID,TotalFee,Currency,IsInCome,PayType,PayStatus,PayDate,ProductTable,ProductID,ProductDes,UserAccount,BankID,BankNo,AddDate",query.Order,query.Where,query.CurrentPage,query.PageSize,out recordCount);
            Pager<FinancePayDetailEntity>  pager = new Pager<FinancePayDetailEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeFinancePayDetailList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<FinancePayDetailEntity> GetFinancePayDetailPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("FinancePayDetail","BillID,JournalID,AuthorID,EBankType,TransactionID,TotalFee,Currency,IsInCome,PayType,PayStatus,PayDate,ProductTable,ProductID,ProductDes,UserAccount,BankID,BankNo,AddDate"," BillID DESC","",query.CurrentPage,query.PageSize,out recordCount);
            Pager<FinancePayDetailEntity>  pager=new Pager<FinancePayDetailEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeFinancePayDetailList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<FinancePayDetailEntity> GetFinancePayDetailPageList(FinancePayDetailQuery query)
        {
            int recordCount=0;
            string whereSQL=FinancePayDetailQueryToSQLWhere(query);
            string orderBy=FinancePayDetailQueryToSQLOrder(query);
            DataSet ds=db.GetPagingData("FinancePayDetail","BillID,JournalID,AuthorID,EBankType,TransactionID,TotalFee,Currency,IsInCome,PayType,PayStatus,PayDate,ProductTable,ProductID,ProductDes,UserAccount,BankID,BankNo,AddDate",orderBy,whereSQL,query.CurrentPage,query.PageSize,out recordCount);
            Pager<FinancePayDetailEntity>  pager=new Pager<FinancePayDetailEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeFinancePayDetailList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddFinancePayDetail(FinancePayDetailEntity financePayDetailEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();           
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @AuthorID");
            sqlCommandText.Append(", @EBankType");
            sqlCommandText.Append(", @TransactionID");
            sqlCommandText.Append(", @TotalFee");
            sqlCommandText.Append(", @Currency");
            sqlCommandText.Append(", @IsInCome");
            sqlCommandText.Append(", @PayType");
            sqlCommandText.Append(", @PayStatus");
            sqlCommandText.Append(", @PayDate");
            sqlCommandText.Append(", @ProductTable");
            sqlCommandText.Append(", @ProductID");
            sqlCommandText.Append(", @ProductDes");
            sqlCommandText.Append(", @UserAccount");
            sqlCommandText.Append(", @BankID");
            sqlCommandText.Append(", @BankNo");          

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.FinancePayDetail ({0},AddDate) VALUES ({1},getdate())", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));
            
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,financePayDetailEntity.JournalID);
            db.AddInParameter(cmd,"@AuthorID",DbType.Int64,financePayDetailEntity.AuthorID);
            db.AddInParameter(cmd,"@EBankType",DbType.Byte,financePayDetailEntity.EBankType);
            db.AddInParameter(cmd,"@TransactionID",DbType.AnsiString,financePayDetailEntity.TransactionID);
            db.AddInParameter(cmd,"@TotalFee",DbType.Decimal,financePayDetailEntity.TotalFee);
            db.AddInParameter(cmd,"@Currency",DbType.AnsiString,financePayDetailEntity.Currency);
            db.AddInParameter(cmd,"@IsInCome",DbType.Byte,financePayDetailEntity.IsInCome);
            db.AddInParameter(cmd,"@PayType",DbType.Byte,financePayDetailEntity.PayType);
            db.AddInParameter(cmd,"@PayStatus",DbType.Byte,financePayDetailEntity.PayStatus);
            db.AddInParameter(cmd,"@PayDate",DbType.DateTime,financePayDetailEntity.PayDate);
            db.AddInParameter(cmd,"@ProductTable",DbType.AnsiString,financePayDetailEntity.ProductTable);
            db.AddInParameter(cmd,"@ProductID",DbType.AnsiString,financePayDetailEntity.ProductID);
            db.AddInParameter(cmd,"@ProductDes",DbType.AnsiString,financePayDetailEntity.ProductDes);
            db.AddInParameter(cmd,"@UserAccount",DbType.AnsiString,financePayDetailEntity.UserAccount);
            db.AddInParameter(cmd,"@BankID",DbType.AnsiString,financePayDetailEntity.BankID);
            db.AddInParameter(cmd,"@BankNo",DbType.AnsiString,financePayDetailEntity.BankNo);           
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
     
        public bool UpdateFinancePayDetail(FinancePayDetailEntity financePayDetailEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  BillID=@BillID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" JournalID=@JournalID");
            sqlCommandText.Append(", AuthorID=@AuthorID");
            sqlCommandText.Append(", EBankType=@EBankType");
            sqlCommandText.Append(", TransactionID=@TransactionID");
            sqlCommandText.Append(", TotalFee=@TotalFee");
            sqlCommandText.Append(", Currency=@Currency");
            sqlCommandText.Append(", IsInCome=@IsInCome");
            sqlCommandText.Append(", PayType=@PayType");
            sqlCommandText.Append(", PayStatus=@PayStatus");
            sqlCommandText.Append(", PayDate=@PayDate");
            sqlCommandText.Append(", ProductTable=@ProductTable");
            sqlCommandText.Append(", ProductID=@ProductID");
            sqlCommandText.Append(", ProductDes=@ProductDes");
            sqlCommandText.Append(", UserAccount=@UserAccount");
            sqlCommandText.Append(", BankID=@BankID");
            sqlCommandText.Append(", BankNo=@BankNo");
            sqlCommandText.Append(", AddDate=@AddDate");
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.FinancePayDetail SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@BillID",DbType.Int64,financePayDetailEntity.BillID);
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,financePayDetailEntity.JournalID);
            db.AddInParameter(cmd,"@AuthorID",DbType.Int64,financePayDetailEntity.AuthorID);
            db.AddInParameter(cmd,"@EBankType",DbType.Byte,financePayDetailEntity.EBankType);
            db.AddInParameter(cmd,"@TransactionID",DbType.AnsiString,financePayDetailEntity.TransactionID);
            db.AddInParameter(cmd,"@TotalFee",DbType.Decimal,financePayDetailEntity.TotalFee);
            db.AddInParameter(cmd,"@Currency",DbType.AnsiString,financePayDetailEntity.Currency);
            db.AddInParameter(cmd,"@IsInCome",DbType.Byte,financePayDetailEntity.IsInCome);
            db.AddInParameter(cmd,"@PayType",DbType.Byte,financePayDetailEntity.PayType);
            db.AddInParameter(cmd,"@PayStatus",DbType.Byte,financePayDetailEntity.PayStatus);
            db.AddInParameter(cmd,"@PayDate",DbType.DateTime,financePayDetailEntity.PayDate);
            db.AddInParameter(cmd,"@ProductTable",DbType.AnsiString,financePayDetailEntity.ProductTable);
            db.AddInParameter(cmd,"@ProductID",DbType.AnsiString,financePayDetailEntity.ProductID);
            db.AddInParameter(cmd,"@ProductDes",DbType.AnsiString,financePayDetailEntity.ProductDes);
            db.AddInParameter(cmd,"@UserAccount",DbType.AnsiString,financePayDetailEntity.UserAccount);
            db.AddInParameter(cmd,"@BankID",DbType.AnsiString,financePayDetailEntity.BankID);
            db.AddInParameter(cmd,"@BankNo",DbType.AnsiString,financePayDetailEntity.BankNo);
            db.AddInParameter(cmd,"@AddDate",DbType.DateTime,financePayDetailEntity.AddDate);

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
        
        public bool DeleteFinancePayDetail(FinancePayDetailEntity financePayDetailEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.FinancePayDetail");
            sqlCommandText.Append(" WHERE  BillID=@BillID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@BillID",DbType.Int64,financePayDetailEntity.BillID);
            
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
        
        public bool DeleteFinancePayDetail(Int64 billID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.FinancePayDetail");
            sqlCommandText.Append(" WHERE  BillID=@BillID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@BillID",DbType.Int64,billID);
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
        /// <param name="billID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteFinancePayDetail(Int64[] billID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from FinancePayDetail where ");
           
            for(int i=0;i<billID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( BillID=@BillID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<billID.Length;i++)
            {
            db.AddInParameter(cmd,"@BillID"+i,DbType.Int64,billID[i]);
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
        
        public FinancePayDetailEntity MakeFinancePayDetail(IDataReader dr)
        {
            FinancePayDetailEntity financePayDetailEntity = null;
            if(dr.Read())
            {
             financePayDetailEntity=new FinancePayDetailEntity();
             financePayDetailEntity.BillID = (Int64)dr["BillID"];
             financePayDetailEntity.JournalID = (Int64)dr["JournalID"];
             financePayDetailEntity.AuthorID = (Int64)dr["AuthorID"];
             financePayDetailEntity.EBankType = (Byte)dr["EBankType"];
             financePayDetailEntity.TransactionID = (String)dr["TransactionID"];
             financePayDetailEntity.TotalFee = (Decimal)dr["TotalFee"];
             financePayDetailEntity.Currency = (String)dr["Currency"];
             financePayDetailEntity.IsInCome = (Byte)dr["IsInCome"];
             financePayDetailEntity.PayType = (Byte)dr["PayType"];
             financePayDetailEntity.PayStatus = (Byte)dr["PayStatus"];
             financePayDetailEntity.PayDate = (DateTime)dr["PayDate"];
             financePayDetailEntity.ProductTable = (String)dr["ProductTable"];
             financePayDetailEntity.ProductID = (String)dr["ProductID"];
             financePayDetailEntity.ProductDes = Convert.IsDBNull(dr["ProductDes"]) ? null : (String)dr["ProductDes"];
             financePayDetailEntity.UserAccount = Convert.IsDBNull(dr["UserAccount"]) ? null : (String)dr["UserAccount"];
             financePayDetailEntity.BankID = Convert.IsDBNull(dr["BankID"]) ? null : (String)dr["BankID"];
             financePayDetailEntity.BankNo = Convert.IsDBNull(dr["BankNo"]) ? null : (String)dr["BankNo"];
             financePayDetailEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return financePayDetailEntity;
        }
        
        public FinancePayDetailEntity MakeFinancePayDetail(DataRow dr)
        {
            FinancePayDetailEntity financePayDetailEntity=null;
            if(dr!=null)
            {
                 financePayDetailEntity=new FinancePayDetailEntity();
                 financePayDetailEntity.BillID = (Int64)dr["BillID"];
                 financePayDetailEntity.JournalID = (Int64)dr["JournalID"];
                 financePayDetailEntity.AuthorID = (Int64)dr["AuthorID"];
                 financePayDetailEntity.EBankType = (Byte)dr["EBankType"];
                 financePayDetailEntity.TransactionID = (String)dr["TransactionID"];
                 financePayDetailEntity.TotalFee = (Decimal)dr["TotalFee"];
                 financePayDetailEntity.Currency = (String)dr["Currency"];
                 financePayDetailEntity.IsInCome = (Byte)dr["IsInCome"];
                 financePayDetailEntity.PayType = (Byte)dr["PayType"];
                 financePayDetailEntity.PayStatus = (Byte)dr["PayStatus"];
                 financePayDetailEntity.PayDate = (DateTime)dr["PayDate"];
                 financePayDetailEntity.ProductTable = (String)dr["ProductTable"];
                 financePayDetailEntity.ProductID = (String)dr["ProductID"];
                 financePayDetailEntity.ProductDes = Convert.IsDBNull(dr["ProductDes"]) ? null : (String)dr["ProductDes"];
                 financePayDetailEntity.UserAccount = Convert.IsDBNull(dr["UserAccount"]) ? null : (String)dr["UserAccount"];
                 financePayDetailEntity.BankID = Convert.IsDBNull(dr["BankID"]) ? null : (String)dr["BankID"];
                 financePayDetailEntity.BankNo = Convert.IsDBNull(dr["BankNo"]) ? null : (String)dr["BankNo"];
                 financePayDetailEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return financePayDetailEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<FinancePayDetailEntity> MakeFinancePayDetailList(IDataReader dr)
        {
            List<FinancePayDetailEntity> list=new List<FinancePayDetailEntity>();
            while(dr.Read())
            {
             FinancePayDetailEntity financePayDetailEntity=new FinancePayDetailEntity();
            financePayDetailEntity.BillID = (Int64)dr["BillID"];
            financePayDetailEntity.JournalID = (Int64)dr["JournalID"];
            financePayDetailEntity.AuthorID = (Int64)dr["AuthorID"];
            financePayDetailEntity.EBankType = (Byte)dr["EBankType"];
            financePayDetailEntity.TransactionID = (String)dr["TransactionID"];
            financePayDetailEntity.TotalFee = (Decimal)dr["TotalFee"];
            financePayDetailEntity.Currency = (String)dr["Currency"];
            financePayDetailEntity.IsInCome = (Byte)dr["IsInCome"];
            financePayDetailEntity.PayType = (Byte)dr["PayType"];
            financePayDetailEntity.PayStatus = (Byte)dr["PayStatus"];
            financePayDetailEntity.PayDate = (DateTime)dr["PayDate"];
            financePayDetailEntity.ProductTable = (String)dr["ProductTable"];
            financePayDetailEntity.ProductID = (String)dr["ProductID"];
            financePayDetailEntity.ProductDes = Convert.IsDBNull(dr["ProductDes"]) ? null : (String)dr["ProductDes"];
            financePayDetailEntity.UserAccount = Convert.IsDBNull(dr["UserAccount"]) ? null : (String)dr["UserAccount"];
            financePayDetailEntity.BankID = Convert.IsDBNull(dr["BankID"]) ? null : (String)dr["BankID"];
            financePayDetailEntity.BankNo = Convert.IsDBNull(dr["BankNo"]) ? null : (String)dr["BankNo"];
            financePayDetailEntity.AddDate = (DateTime)dr["AddDate"];
               list.Add(financePayDetailEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<FinancePayDetailEntity> MakeFinancePayDetailList(DataTable dt)
        {
            List<FinancePayDetailEntity> list=new List<FinancePayDetailEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   FinancePayDetailEntity financePayDetailEntity=MakeFinancePayDetail(dt.Rows[i]);
                   list.Add(financePayDetailEntity);
                }
            }
            return list;
        }
        
        #endregion

    }
}

