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
	///  表'MessageTemplate'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class MessageTemplateEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// TemplateID : 
		/// </summary>
		/// <remarks>表MessageTemplate主键</remarks>		
		[DataMember]
		public Int64 TemplateID
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
		/// TType : 模板类型 1=邮件模板 2=短信模板
		/// </summary>       
		[DataMember]
		public Byte TType
        {
            get;
            set;
        }
		
		/// <summary>
		/// Title : 标题
		/// </summary>       
		[DataMember]
		public String Title
        {
            get;
            set;
        }
		
		/// <summary>
		/// TContent : 内容
		/// </summary>       
		[DataMember]
		public String TContent
        {
            get;
            set;
        }
		
		/// <summary>
		/// TCategory : 模板类型 -1=注册模板 -2=找回密码 -3=审稿费 -4=版面费 -5=投稿回执 -6审稿单前言，还要附加上在审稿操作中设置的各种操作模板
		/// </summary>      
		[DataMember]
		public Int32 TCategory
        {
            get;
            set;
        }

        /// <summary>
        /// 模板类型
        /// </summary>
        [DataMember]
        public String TCategoryName
        {
            get;
            set;
        }
		
		/// <summary>
		/// EditUser : 修改人
		/// </summary>       
		[DataMember]
		public Int64 EditUser
        {
            get;
            set;
        }
		
		/// <summary>
		/// EditDate : 修改时间
		/// </summary>       
		[DataMember]
		public DateTime EditDate
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
		/// AddDate : 添加时间
		/// </summary>      
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }
		#endregion

		#endregion 属性、变量声明
		
	    public MessageTemplateEntity()
        {
            TemplateID = (long)0;
            JournalID = (long)0;
            TType = 1;
            Title = string.Empty;
            TContent = string.Empty;
            TCategory = -1;
            EditUser = (long)0;
            EditDate = DateTime.Now;
            InUser = (long)0;
            AddDate = DateTime.Now;
        }
	}
}
