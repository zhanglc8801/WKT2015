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
	///  表'FinanceContribute'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class FinanceContributeEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// PKID : 
		/// </summary>
		/// <remarks>表FinanceContribute主键</remarks>		
		[DataMember]
		public Int64 PKID
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
		/// CID : 稿件ID
		/// </summary>      
		[DataMember]
		public Int64 CID
        {
            get;
            set;
        }        
		
		/// <summary>
		/// FeeType : 费用类型 1=审稿费 2=版面费 3=加急费 4=稿费
		/// </summary>       
		[DataMember]
		public Byte FeeType
        {
            get;
            set;
        }

        [DataMember]
        public String FeeTypeName
        {
            get
            {
                switch (FeeType)
                {
                    case 1: return "审稿费";
                    case 2: return "版面费";
                    case 3: return "加急费";
                    case 4: return "稿费"; 
                    default: return string.Empty;
                }
            }
        }
		
		/// <summary>
		/// Amount : 缴费金额
		/// </summary>       
		[DataMember]
		public Decimal Amount
        {
            get;
            set;
        }

        /// <summary>
        /// Money : 审稿费
        /// </summary>       
        [DataMember]
        public Decimal Money
        {
            get;
            set;
        }
        /// <summary>
        /// PageMoney : 版面费
        /// </summary>       
        [DataMember]
        public Decimal PageMoney
        {
            get;
            set;
        }

        /// <summary>
        /// PageMoney : 版面费通知金额
        /// </summary>       
        [DataMember]
        public Decimal PageMoneyNotice
        {
            get;
            set;
        }

        /// <summary>
        /// ArticleMoney : 稿费
        /// </summary>       
        [DataMember]
        public Decimal ArticleMoney
        {
            get;
            set;
        }

        /// <summary>
        /// ArticleMoney : 稿费类型 0=按篇 1=按页
        /// </summary>       
        [DataMember]
        public Int32 ArticleType
        {
            get;
            set;
        }

        /// <summary>
        /// ArticleMoney : 篇数/页数
        /// </summary>       
        [DataMember]
        public Decimal ArticleCount
        {
            get;
            set;
        }
		
		/// <summary>
		/// PayType : 支付方式：1=网银支付 2=邮局汇款 3=银行转帐 4=现金支付
		/// </summary>        
		[DataMember]
		public Byte? PayType
        {
            get;
            set;
        }

        [DataMember]
        public String PayTypeName
        {
            get
            {
                if(PayType==null)return string.Empty;
                switch (PayType.Value)
                {
                    case 1: return "网银支付";
                    case 2: return "邮局汇款";
                    case 3: return "银行转帐";
                    case 4: return "现金支付";
                    default: return string.Empty;
                }
            }
        }
		
		/// <summary>
		/// RemitBillNo : 汇款单号
		/// </summary>       
		[DataMember]
		public String RemitBillNo
        {
            get;
            set;
        }
		
		/// <summary>
		/// InUser : 录入人
		/// </summary>       
		[DataMember]
		public Int64 InUser
        {
            get;
            set;
        }

        /// <summary>
        /// 录入人
        /// </summary>
        [DataMember]
        public String InUserName { get; set; }
		
		/// <summary>
		/// InComeDate : 入款日期
		/// </summary>       
		[DataMember]
		public DateTime? InComeDate
        {
            get;
            set;
        }
		
		/// <summary>
        /// InvoiceNo : 发票号码
		/// </summary>       
		[DataMember]
		public String InvoiceNo
        {
            get;
            set;
        }
		
		/// <summary>
		/// PostNo : 挂号号码
		/// </summary>       
		[DataMember]
		public String PostNo
        {
            get;
            set;
        }
		
		/// <summary>
		/// SendDate : 寄出日期
		/// </summary>       
		[DataMember]
		public DateTime? SendDate
        {
            get;
            set;
        }

        /// <summary>
        /// 状态 0:待确认 1:已确认
        /// </summary>
        [DataMember]
        public Byte Status { get; set; }
		
		/// <summary>
		/// Note : 备注
		/// </summary>       
		[DataMember]
		public String Note
        {
            get;
            set;
        }
		
		/// <summary>
		/// AddDate : 录入时间
		/// </summary>       
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件名称
        /// </summary>      
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public String CNumber { get; set; }

        /// <summary>
        /// 投稿人
        /// </summary>
        [DataMember]
        public Int64 UserID { get; set; }

        /// <summary>
        /// 责任编辑ID
        /// </summary>
        [DataMember]
        public Int64 EditAuthorID { get; set; }

        /// <summary>
        /// 责任编辑
        /// </summary>
        [DataMember]
        public String EditAuthorName { get; set; }

        /// <summary>
        /// 作者编号
        /// </summary>
        [DataMember]
        public Int64 AuthorID { get; set; }

        /// <summary>
        /// 第一作者ID
        /// </summary>
        [DataMember]
        public Int64 FirstAuthorID { get; set; }

        /// <summary>
        /// 通讯作者ID
        /// </summary>
        [DataMember]
        public Int64 CommunicationAuthorID { get; set; }

        /// <summary>
        /// 作者名称
        /// </summary>
        [DataMember]
        public String AuthorName { get; set; }

        /// <summary>
        /// 第一作者名称
        /// </summary>
        [DataMember]
        public String FirstAuthorName { get; set; }

        /// <summary>
        /// 通讯作者名称
        /// </summary>
        [DataMember]
        public String CommunicationAuthorName { get; set; }

        /// <summary>
        /// 作者Email
        /// </summary>
        [DataMember]
        public String Email { get; set; }

        /// <summary>
        /// 应交金额
        /// </summary>
        [DataMember]
        public Decimal ShouldMoney { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        [DataMember]
        public String WorkUnit { get; set; }

        /// <summary>
        /// 是否编辑平台
        /// </summary>
        [DataMember]
        public bool IsSystem { get; set; }

        /// <summary>
        /// 通知表主键
        /// </summary>
        [DataMember]
        public Int64 NoticeID { get; set; }

        /// <summary>
        /// 通知表主键(版面费)
        /// </summary>
        [DataMember]
        public Int64 PageNoticeID { get; set; }
		#endregion

		#endregion 属性、变量声明

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public String Address { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [DataMember]
        public String InvoiceUnit { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [DataMember]
        public String ZipCode { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [DataMember]
        public String Tel { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember]
        public String Mobile { get; set; }
		
	    public FinanceContributeEntity()
        {
            PKID = (long)0;
            JournalID = (long)0;
            CID = (long)0;
            FeeType = (byte)3;
            Amount = 0m;
            PayType = null;
            RemitBillNo = null;
            InUser = (long)0;
            InComeDate = null;
            InvoiceNo = null;
            PostNo = null;
            SendDate = null;
            Status = 0;
            Note = null;
            AddDate = DateTime.Now;
            Title = string.Empty;
            AuthorID = 0;
            AuthorName = string.Empty;
            Address = "";
            InvoiceUnit = "";
            Tel = "";
            Mobile = "";
        }
	}
}
