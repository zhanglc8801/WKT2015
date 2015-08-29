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
	///  表'IssueSubscribe'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class IssueSubscribeEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// SubscribeID : 
		/// </summary>
		/// <remarks>表IssueSubscribe主键</remarks>		
		[DataMember]
		public Int64 SubscribeID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// JournalID : 
		/// </summary>      
		[DataMember]
		public Int64 JournalID
        {
            get;
            set;
        }
		
		/// <summary>
		/// Subscriber : 订阅者，单位名称或个人姓名
		/// </summary>      
		[DataMember]
		public String Subscriber
        {
            get;
            set;
        }
		
		/// <summary>
		/// Mobile : 手机号
		/// </summary>       
		[DataMember]
		public String Mobile
        {
            get;
            set;
        }
		
		/// <summary>
		/// Tel : 电话
		/// </summary>      
		[DataMember]
		public String Tel
        {
            get;
            set;
        }
		
		/// <summary>
		/// Fax : 传真
		/// </summary>       
		[DataMember]
		public String Fax
        {
            get;
            set;
        }
		
		/// <summary>
		/// Email : Email
		/// </summary>      
		[DataMember]
		public String Email
        {
            get;
            set;
        }
		
		/// <summary>
		/// Address : 联系地址
		/// </summary>      
		[DataMember]
		public String Address
        {
            get;
            set;
        }
		
		/// <summary>
		/// ZipCode : 邮编
		/// </summary>      
		[DataMember]
		public String ZipCode
        {
            get;
            set;
        }
		
		/// <summary>
		/// ContactUser : 联系人
		/// </summary>       
		[DataMember]
		public String ContactUser
        {
            get;
            set;
        }
		
		/// <summary>
		/// SubscribeInfo : 订阅详情，订阅哪一年哪一期，多少册
		/// </summary>       
		[DataMember]
		public String SubscribeInfo
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsInvoice : 是否开发票 0=否 1=是
		/// </summary>       
		[DataMember]
		public Boolean IsInvoice
        {
            get;
            set;
        }

        /// <summary>
        /// 是否开票
        /// </summary>
        [DataMember]
        public String IsInvoiceName
        {
            get
            {
                return IsInvoice ? "是" : "否";
            }
        }
		
		/// <summary>
		/// InvoiceHead : 发票抬头
		/// </summary>      
		[DataMember]
		public String InvoiceHead
        {
            get;
            set;
        }
		
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
		/// SubscribeDate : 订阅日期
		/// </summary>       
		[DataMember]
		public DateTime SubscribeDate
        {
            get;
            set;
        }
		#endregion

		#endregion 属性、变量声明
		
	    public IssueSubscribeEntity()
        {
            SubscribeID = (long)0;
            JournalID = (long)0;
            Subscriber = string.Empty;
            Mobile = string.Empty;
            Tel = string.Empty;
            Fax = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
            ZipCode = string.Empty;
            ContactUser = string.Empty;
            SubscribeInfo = string.Empty;
            IsInvoice = false;
            InvoiceHead = string.Empty;
            Note = string.Empty;
            SubscribeDate = DateTime.Now;
        }
	}
}
