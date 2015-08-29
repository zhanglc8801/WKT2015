using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
//using WKT.Common.Utils;

namespace WKT.Model
{

		
	/// <summary>
	///  表'SiteMessage'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class SiteMessageEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键		
		[DataMember]
		public Int64 MessageID
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
		/// ReciverID : 接收人
		/// </summary>       
		[DataMember]
		public Int64 ReciverID
        {
            get;
            set;
        }

        /// <summary>
        /// ReciverName : 接收人
        /// </summary>       
        [DataMember]
        public String ReciverName
        {
            get;
            set;
        }
		
		/// <summary>
		/// Title : 消息标题
		/// </summary>       
		[DataMember]
		public String Title
        {
            get;
            set;
        }
		
		/// <summary>
		/// Content : 消息内容
		/// </summary>       
		[DataMember]
		public String Content
        {
            get;
            set;
        }

        /// <summary>
        /// 截取后的消息内容
        /// </summary>       
        [DataMember]
        public String SimpleContent { get; set; }

        /// <summary>
        /// 是否查看 1:是 0:否
        /// </summary>
        [DataMember]
        public Byte IsView { get; set; }
		
		/// <summary>
		/// SendDate : 发送日期
		/// </summary>       
		[DataMember]
		public DateTime SendDate
        {
            get;
            set;
        }
		#endregion

		#endregion 属性、变量声明
		
	    public SiteMessageEntity()
        {
            MessageID = (long)0;
            JournalID = (long)0;
            CID = (long)0;
            SendUser = (long)0;
            ReciverID = (long)0;
            Title = string.Empty;
            Content = string.Empty;
            IsView = (Byte)0;
            SendDate = DateTime.Now;
        }
	}
}
