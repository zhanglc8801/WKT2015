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
	///  表'IssueDownLog'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class IssueDownLogEntity : ObjectBase
	{
		#region 属性、变量声明
        
		/// <summary>			
		/// DownLogID : 
		/// </summary>
		/// <remarks>表IssueDownLog主键</remarks>
		[DataMember]
		public Int64 DownLogID
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
		/// AuthorID : 浏览人
		/// </summary>
		[DataMember]
		public Int64 AuthorID
        {
            get;
            set;
        }
		
		/// <summary>
		/// Daytime : 
		/// </summary>
		[DataMember]
		public Int32 Daytime
        {
            get;
            set;
        }
		
		/// <summary>
		/// Year : 年
		/// </summary>
		[DataMember]
		public Int32 Year
        {
            get;
            set;
        }
		
		/// <summary>
		/// Month : 月
		/// </summary>
		[DataMember]
		public Int32 Month
        {
            get;
            set;
        }
		
		/// <summary>
		/// IP : 日，例如：20121201
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
		public DateTime AddDate
        {
            get;
            set;
        }

		#endregion 属性、变量声明

        /// <summary>
        /// 稿件ID
        /// </summary>
        [DataMember]
        public Int64 CID { get; set; }

        /// <summary>
        /// 稿件编码
        /// </summary>
        [DataMember]
        public String CNumber { get; set; }

        /// <summary>
        /// 期刊标题
        /// </summary>
        [DataMember]
        public String Title { get; set; }

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
        /// 下载次数
        /// </summary>
        [DataMember]
        public Int64 DownLoadCount { get; set; }
		
	    public IssueDownLogEntity()
        {
            DownLogID = (long)0;
            JournalID = (long)0;
            ContentID = (long)0;
            AuthorID = (long)0;
            Daytime = (int)0;
            Year = (int)0;
            Month = (int)0;
            IP = string.Empty;
            AddDate = DateTime.Now;
        }
	}
}
