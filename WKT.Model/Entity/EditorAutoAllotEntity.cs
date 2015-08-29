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
	///  表'EditorAutoAllot'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract,Serializable]
	public partial class EditorAutoAllotEntity : ObjectBase
    {
        #region 属性

        /// <summary>			
		/// PKID : 
		/// </summary>
		/// <remarks>表EditorAutoAllot主键</remarks>
		[DataMember]
		public Int64 PKID
        {
            get;
            set;
        }
		

		/// <summary>
		/// JournalID : 杂志社ID
		/// </summary>
		[DataMember]
		public Int64 JournalID
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件自动分配方式1=按学科分类 2=奇偶 3=固定责编
        /// </summary>
        [DataMember]
        public Byte AllotPattern
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
        /// 奇数责编
        /// </summary>
        [DataMember]
        public Int64 OddAuthorID
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

        [DataMember]
        public string AuthorName
        {
            get;
            set;
        }

        /// <summary>
        /// 奇数责任编辑姓名
        /// </summary>
        [DataMember]
        public string OddAuthorName
        {
            get;
            set;
        }

        /// <summary>
        /// 学科分类和编辑对应列表
        /// </summary>
        [DataMember]
        public List<SubjectAuthorMapEntity> SubjectAuthorMap
        {
            get;
            set;
        }

	    public EditorAutoAllotEntity()
        {
            PKID = 0;
            JournalID = 0;
            AllotPattern = 1;
            AuthorID = 0;
            OddAuthorID = 0;
            AddDate = DateTime.Now;
        }
	}
}
