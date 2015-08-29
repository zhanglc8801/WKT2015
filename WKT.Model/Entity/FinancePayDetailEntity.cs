using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{

		
	/// <summary>
	///  表'FinancePayDetail'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class FinancePayDetailEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// BillID : 
		/// </summary>
		/// <remarks>表FinancePayDetail主键</remarks>		
		[DataMember]
		public Int64 BillID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// JournalID : 编辑部标识
		/// </summary>       
		[DataMember]
		public Int64 JournalID
        {
            get;
            set;
        }
		
		/// <summary>
		/// AuthorID : 作者ID
		/// </summary>       
		[DataMember]
		public Int64 AuthorID
        {
            get;
            set;
        }
		
		/// <summary>
		/// EBankType : 网银类型 1=财富通 2=支付宝 3=易宝支付
		/// </summary>       
		[DataMember]
		public Byte EBankType
        {
            get;
            set;
        }
		
		/// <summary>
		/// TransactionID : 在线支付交易流水号
		/// </summary>      
		[DataMember]
		public String TransactionID
        {
            get;
            set;
        }
		
		/// <summary>
		/// TotalFee : 支付金额
		/// </summary>       
		[DataMember]
		public Decimal TotalFee
        {
            get;
            set;
        }
		
		/// <summary>
		/// Currency : 币种
		/// </summary>      
		[DataMember]
		public String Currency
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsInCome : 收入还是支出 1=收入2=支出
		/// </summary>       
		[DataMember]
		public Byte IsInCome
        {
            get;
            set;
        }
		
		/// <summary>
		/// PayType : 支付类型，事由
		/// </summary>      
		[DataMember]
		public Byte PayType
        {
            get;
            set;
        }
		
		/// <summary>
		/// PayStatus : 支付状态 0=失败 1=成功
		/// </summary>       
		[DataMember]
		public Byte PayStatus
        {
            get;
            set;
        }
		
		/// <summary>
		/// PayDate : 支付时间
		/// </summary>      
		[DataMember]
		public DateTime PayDate
        {
            get;
            set;
        }
		
		/// <summary>
		/// ProductTable : 支付商品信息所在表名
		/// </summary>       
		[DataMember]
		public String ProductTable
        {
            get;
            set;
        }
		
		/// <summary>
		/// ProductID : 购买商品ID，例如：交的是审稿费，这里存放的就是稿件编号
		/// </summary>      
		[DataMember]
		public String ProductID
        {
            get;
            set;
        }
		
		/// <summary>
		/// ProductDes : 购买商品描述
		/// </summary>       
		[DataMember]
		public String ProductDes
        {
            get;
            set;
        }
		
		/// <summary>
		/// UserAccount : 卖方帐号
		/// </summary>       
		[DataMember]
		public String UserAccount
        {
            get;
            set;
        }
		
		/// <summary>
		/// BankID : 银行编号
		/// </summary>       
		[DataMember]
		public String BankID
        {
            get;
            set;
        }
		
		/// <summary>
		/// BankNo : 银行交易号
		/// </summary>       
		[DataMember]
		public String BankNo
        {
            get;
            set;
        }
		
		/// <summary>
		/// AddDate : 保存时间
		/// </summary>      
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }

        /// <summary>
        /// 通知表主键
        /// </summary>
        [DataMember]
        public Int64 NoticeID { get; set; }
		#endregion

		#endregion 属性、变量声明
		
	    public FinancePayDetailEntity()
        {
            BillID = (long)0;
            JournalID = (long)0;
            AuthorID = (long)0;
            EBankType = (byte)3;
            TransactionID = string.Empty;
            TotalFee = 0m;
            Currency = "RMB";
            IsInCome = (byte)1;
            PayType = (byte)0;
            PayStatus = (byte)1;
            PayDate = DateTime.Now;
            ProductTable = string.Empty;
            ProductID = string.Empty;
            ProductDes = null;
            UserAccount = null;
            BankID = null;
            BankNo = null;
            AddDate = DateTime.Now;
            NoticeID = 0;
        }
	}
}
