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
	///  表'PayNotice'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class PayNoticeEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// NoticeID : 
		/// </summary>
		/// <remarks>表PayNotice主键</remarks>		
		[DataMember]
		public Int64 NoticeID
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
		/// PayType : 缴费类型：1=审稿费 2=版面费 3=加急费
		/// </summary>        
		[DataMember]
		public Byte PayType
        {
            get;
            set;
        }

        /// <summary>
        /// PayType : 缴费类型：1=审稿费 2=版面费 3=加急费
        /// </summary>        
        [DataMember]
        public String PayTypeName
        {
            get
            {
                switch (PayType)
                {
                    case 1: return "审稿费";
                    case 2: return "版面费";
                    case 3: return "加急费";
                    case 4: return "稿  费";
                    default: return string.Empty;
                }
            }
        }
		
		/// <summary>
		/// CID : 稿件编号
		/// </summary>       
		[DataMember]
		public Int64 CID
        {
            get;
            set;
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
		/// Title : 通知标题
		/// </summary>       
		[DataMember]
		public String Title
        {
            get;
            set;
        }
		
		/// <summary>
		/// Body : 通知内容
		/// </summary>       
		[DataMember]
		public String Body
        {
            get;
            set;
        }
		
		/// <summary>
		/// SendDate : 通知时间
		/// </summary>       
		[DataMember]
		public DateTime SendDate
        {
            get;
            set;
        }
		
		/// <summary>
		/// Status : 状态 0=未交 1=已交 2=待确认
		/// </summary>       
		[DataMember]
		public Byte Status
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public String StatusName
        {
            get
            {
                switch (Status)
                {
                    case 2: return "待确认";
                    case 1: return "已交";
                    default: return "<span style=\"color:red\">未交</span>";
                }
            }
        }
		#endregion

        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public String CNumber { get; set; }

        /// <summary>
        /// 稿件标题
        /// </summary>
        [DataMember]
        public String CTitle { get; set; }

        /// <summary>
        /// 稿件作者
        /// </summary>
        [DataMember]
        public Int64 AuthorID { get; set; }

        /// <summary>
        /// 是否发送短信
        /// </summary>
        [DataMember]
        public bool IsSms { get; set; }


        /// <summary>
        /// 短信内容
        /// </summary>
        [DataMember]
        public String SmsContent { get; set; }

        /// <summary>
        /// 发送人
        /// </summary>
        [DataMember]
        public Int64 SendUser { get; set; }


        /// <summary>
        /// 作者姓名
        /// </summary>
        [DataMember]
        public string AuthorName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DataMember]
        public string LoginName { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }

		#endregion 属性、变量声明
		
	    public PayNoticeEntity()
        {
            NoticeID = (long)0;
            JournalID = (long)0;
            PayType = (byte)0;
            CID = (long)0;
            Amount = 0m;
            Title = string.Empty;
            Body = string.Empty;
            SendDate = DateTime.Now;
            Status = (byte)0;
            CNumber = string.Empty;
            CTitle = string.Empty;
        }
	}
}
