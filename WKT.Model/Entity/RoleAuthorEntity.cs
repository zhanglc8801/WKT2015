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
	///  表'RoleAuthor'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class RoleAuthorEntity : ObjectBase
	{
		/// <summary>			
		/// MapID : 
		/// </summary>
		/// <remarks>表RoleAuthor主键</remarks>
		[DataMember]
		public Int64 MapID
        {
            get;
            set;
        }
		
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
		/// RoleID : 
		/// </summary>
		[DataMember]
		public Int64 RoleID
        {
            get;
            set;
        }

        /// <summary>
        /// RoleName : 
        /// </summary>
        [DataMember]
        public string RoleName
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
        /// 真实姓名
        /// </summary>
        [DataMember]
        public string RealName
        {
            get;
            set;
        }
        /// <summary>
        /// 登录名
        /// </summary>
        [DataMember]
        public string LoginName
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
		
	    public RoleAuthorEntity()
        {
            MapID = (long)0;
            JournalID = (long)0;
            RoleID = (long)0;
            RoleName = string.Empty;
            AuthorID = (long)0;
            RealName = string.Empty;
            LoginName = string.Empty;
            AddDate = DateTime.MinValue;
        }
	}
}
