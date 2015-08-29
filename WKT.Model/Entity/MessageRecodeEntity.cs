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
	///  表'MessageRecode'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class MessageRecodeEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// RecodeID : 
		/// </summary>
		/// <remarks>表MessageRecode主键</remarks>		
		[DataMember]
		public Int64 RecodeID
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
        /// 稿件编号
        /// </summary>
        [DataMember]
        public Int64 CID { get; set; }
		
		/// <summary>
		/// SendUser : 发送人
		/// </summary>      
		[DataMember]
		public Int64 SendUser
        {
            get;
            set;
        }

        /// <summary>
        /// SendUserName : 发送人
        /// </summary>      
        [DataMember]
        public String SendUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 发送地址
        /// </summary>      
        [DataMember]
        public String SendAddress
        {
            get;
            set;
        }
		
		/// <summary>
		/// ReciveUser : 接收人
		/// </summary>      
		[DataMember]
		public Int64 ReciveUser
        {
            get;
            set;
        }

        /// <summary>
        /// ReciveUserName : 接收人
        /// </summary>      
        [DataMember]
        public String ReciveUserName
        {
            get;
            set;
        }
		
		/// <summary>
		/// ReciveAddress : 接收地址，如果是邮件则是邮件地址，如果是短信则是手机号码
		/// </summary>      
		[DataMember]
		public String ReciveAddress
        {
            get;
            set;
        }
		
		/// <summary>
		/// SendDate : 
		/// </summary>       
		[DataMember]
		public DateTime SendDate
        {
            get;
            set;
        }
		
		/// <summary>
		/// MsgType : 消息类型 1=邮件 2=短信
		/// </summary>     
		[DataMember]
		public Byte MsgType
        {
            get;
            set;
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        [DataMember]
        public String MsgTypeName
        {
            get
            {
                switch (MsgType)
                {
                    case 1: return "邮件";
                    case 2: return "短信";
                    case 3: return "站内消息";
                    default: return string.Empty;
                }
            }
        }

        [DataMember]
        public long MessageID
        {
            get;set;
        }
		
		/// <summary>
		/// SendType : 发送类型 -1=注册模板 -2=找回密码 -3=审稿费 -4=版面费，还要附加上在审稿操作中设置的各种操作模板
		/// </summary>       
		[DataMember]
		public Int32? SendType
        {
            get;
            set;
        }
		
		/// <summary>
		/// MsgTitle : 消息标题
		/// </summary>      
		[DataMember]
		public String MsgTitle
        {
            get;
            set;
        }
		
		/// <summary>
		/// MsgContent : 消息内容
		/// </summary>       
		[DataMember]
		public String MsgContent
        {
            get;
            set;
        }

        /// <summary>
        /// SMServices : 短信服务商  1=商讯·中国；2=商信通
        /// </summary>       
        [DataMember]
        public int SMServices
        {
            get;
            set;
        }
		
		/// <summary>
		/// AddDate : 添加时间
		/// </summary>      
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }


        /// <summary>
        /// 模板ID
        /// </summary>
        [DataMember]
        public Int64 TemplateID
        {
            get;
            set;
        }

        [DataMember]
        public Int32 Index { get; set; }

        /// <summary>
        /// 邮件发送人称呼
        /// </summary>
        [DataMember]
        public String sendMailName { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        [DataMember]
        public String[] FilePath { get; set; }
		#endregion

		#endregion 属性、变量声明
		
	    public MessageRecodeEntity()
        {
            RecodeID = (long)0;
            JournalID = (long)0;
            CID = (long)0;
            MessageID = 0;
            SendUser = (long)0;
            ReciveUser = (long)0;
            ReciveAddress = string.Empty;
            SendDate = DateTime.MinValue;
            MsgType = (byte)0;
            SendType = null;
            AddDate = DateTime.Now;
            Index = 0;
            sendMailName = string.Empty;
            FilePath = null;
        }
	}
}
