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
	///  表'IssueViewLog'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class IssueViewLogEntity : ObjectBase
	{
		#region 属性、变量声明
        
		/// <summary>			
		/// ViewLogID : 
		/// </summary>
		/// <remarks>表IssueViewLog主键</remarks>
		[DataMember]
		public Int64 ViewLogID
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
		/// ContentID : 
		/// </summary>
		[DataMember]
		public Int64 ContentID
        {
            get;
            set;
        }

        /// <summary>
        /// 期刊标题
        /// </summary>
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// AuthorID : 浏览人
        /// </summary>
        [DataMember]
        public Int64 AuthorID
        {
            get;
            set;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public String RealName { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [DataMember]
        public String LoginName { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember]
        public String Mobile { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        [DataMember]
        public Int64 ViewCount { get; set; }
		
		
		
		/// <summary>
		/// Daytime : 年月日
		/// </summary>
		[DataMember]
		public Int32 Daytime
        {
            get;
            set;
        }
		
		/// <summary>
		/// Year : 
		/// </summary>
		[DataMember]
		public Int32 Year
        {
            get;
            set;
        }
		
		/// <summary>
		/// Month : 
		/// </summary>
		[DataMember]
		public Int32 Month
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
		/// AddDate : 添加日期
		/// </summary>
		[DataMember]
		public DateTime? AddDate
        {
            get;
            set;
        }

		#endregion 属性、变量声明
		
	    public IssueViewLogEntity()
        {
            ViewLogID = (long)0;
            JournalID = (long)0;
            ContentID = (long)0;
            Title = string.Empty;
            AuthorID = (long)0;
            Daytime = (int)0;
            Year = (int)0;
            Month = (int)0;
            IP = string.Empty;
            AddDate = null;
        }
	}
}
