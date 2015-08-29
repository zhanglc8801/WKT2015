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
	///  表'FlowAuthAuthor'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class FlowAuthAuthorEntity : ObjectBase
    {
        #region 属性
        
        /// <summary>			
		/// AuthorAuthID : 
		/// </summary>
		/// <remarks>表FlowAuthAuthor主键</remarks>
		[DataMember]
		public Int64 AuthorAuthID
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
        /// ActionID : 审稿操作ID
        /// </summary>
        [DataMember]
        public Int64 ActionID
        {
            get;
            set;
        }
		
		/// <summary>
		/// AuthorID : 编辑ID
		/// </summary>
		[DataMember]
		public Int64 AuthorID
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

        public string AuthorName
        {
            get;
            set;
        }
		
	    public FlowAuthAuthorEntity()
        {
            AuthorAuthID = 0;
            JournalID = 0;
            ActionID = 0;
            AuthorID = 0;
            AddDate = DateTime.Now;
        }
	}
}
