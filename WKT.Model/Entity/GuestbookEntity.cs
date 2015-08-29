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
	///  表'Guestbook'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class GuestbookEntity : ObjectBase
	{
		/// <summary>			
		/// MessageID : 
		/// </summary>
		/// <remarks>表Guestbook主键</remarks>
		[DataMember]
		public Int64 MessageID
        {
            get;
            set;
        }
		
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
		/// PMessageID : 回复留言ID
		/// </summary>
		[DataMember]
		public Int64 PMessageID
        {
            get;
            set;
        }
		
		/// <summary>
		/// UserName : 姓名
		/// </summary>
		[DataMember]
		public String UserName
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
		/// Tel : 联系电话，包括手机
		/// </summary>
		[DataMember]
		public String Tel
        {
            get;
            set;
        }
		
		/// <summary>
		/// Title : 主题
		/// </summary>
		[DataMember]
		public String Title
        {
            get;
            set;
        }
		
		/// <summary>
		/// MessageContent : 
		/// </summary>
		[DataMember]
		public String MessageContent
        {
            get;
            set;
        }

        /// <summary>
        /// ReplyContent
        /// </summary>
        [DataMember]
        public String ReplyContent
        {
            get;
            set;
        }
		
		/// <summary>
		/// IP : 
		/// </summary>
		[DataMember]
		public String IP
        {
            get;
            set;
        }
		
		/// <summary>
		/// AddDate : 
		/// </summary>
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }

		#endregion
		
	    public GuestbookEntity()
        {
            MessageID = 0;
            JournalID = 0;
            PMessageID = 0;
            UserName = string.Empty;
            Email = string.Empty;
            Tel = string.Empty;
            Title = string.Empty;
            MessageContent = string.Empty;
            ReplyContent = "";
            IP = string.Empty;
            AddDate = DateTime.MinValue;
        }
	}
}
