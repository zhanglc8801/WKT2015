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
	///  表'AuthorMenuRightException'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class AuthorMenuRightExceptionEntity : ObjectBase
	{
		/// <summary>			
		/// ExceptionID : 
		/// </summary>
		/// <remarks>表AuthorMenuRightException主键</remarks>
		[DataMember]
		public Int64 ExceptionID
        {
            get;
            set;
        }
		
		/// <summary>
		/// JournalID : 编辑部ID
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
        /// RoleID : 角色ID
        /// </summary>
        [DataMember]
        public Int64 RoleID
        {
            get;
            set;
        }
		
		/// <summary>
		/// MenuID : 菜单ID
		/// </summary>
		[DataMember]
		public Int64 MenuID
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

        [DataMember]
        public IList<long> MenuIDList
        {
            get;
            set;
        }
		
	    public AuthorMenuRightExceptionEntity()
        {
            ExceptionID = (long)0;
            JournalID = (long)0;
            AuthorID = (long)0;
            MenuID = (long)0;
            AddDate = DateTime.Now;
        }
	}
}
