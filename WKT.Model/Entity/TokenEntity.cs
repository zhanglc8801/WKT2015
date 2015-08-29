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
	///  表'Token'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class TokenEntity : ObjectBase
	{
		#region 属性、变量声明
        
		/// <summary>			
		/// TokenID : 
		/// </summary>
		/// <remarks>表Token主键</remarks>
		[DataMember]
		public Int64 TokenID
        {
            get;
            set;
        }

        [DataMember]
        public long JournalID
        {
            get;
            set;
        }
		
		
		/// <summary>
		/// AuthorID : 
		/// </summary>
		[DataMember]
		public Int64 AuthorID
        {
            get;
            set;
        }
		
		/// <summary>
		/// Token : 令牌
		/// </summary>
		[DataMember]
		public String Token
        {
            get;
            set;
        }
		
		/// <summary>
		/// Type : 令牌类型 1=获取密码
		/// </summary>
		[DataMember]
		public Byte Type
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

		#endregion 属性、变量声明
		
	    public TokenEntity()
        {
            TokenID = (long)0;
            AuthorID = (long)0;
            Token = string.Empty;
            Type = (byte)0;
            AddDate = DateTime.Now;
        }
	}
}
