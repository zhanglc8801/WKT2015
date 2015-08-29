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
	///  表'SubjectAuthorMap'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract,Serializable]
	public partial class SubjectAuthorMapEntity : ObjectBase
	{
		#region 属性、变量声明
        
		/// <summary>			
		/// MapID : 
		/// </summary>
		/// <remarks>表SubjectAuthorMap主键</remarks>
		[DataMember]
		public Int64 MapID
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
		/// SubjectCategoryID : 
		/// </summary>
		[DataMember]
		public Int32 SubjectCategoryID
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
		/// AddDate : 添加时间
		/// </summary>
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }

		#endregion 属性、变量声明

        [DataMember]
        public string AuthorName
        {
            get;
            set;
        }

        /// <summary>
        /// 学科分类名称
        /// </summary>
        [DataMember]
        public string CategoryName
        {
            get;
            set;
        }
		
	    public SubjectAuthorMapEntity()
        {
            MapID = 0;
            JournalID = 0;
            SubjectCategoryID = 0;
            AuthorID = 0;
            AddDate = DateTime.Now;
        }
	}
}
