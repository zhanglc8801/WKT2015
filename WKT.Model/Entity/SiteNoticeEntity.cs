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
	///  表'SiteNotice'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class SiteNoticeEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// NoticeID : 
		/// </summary>
		/// <remarks>表SiteNotice主键</remarks>		
		[DataMember]
		public Int64 NoticeID
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
        /// 栏目编码
        /// </summary>
        [DataMember]
        public Int64 ChannelID { get; set; }
		
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
		/// Keywords : 关键字
		/// </summary>       
		[DataMember]
		public String Keywords
        {
            get;
            set;
        }
		
		/// <summary>
		/// Description : 描述
		/// </summary>       
		[DataMember]
		public String Description
        {
            get;
            set;
        }
		
		/// <summary>
		/// Content : 内容
		/// </summary>       
		[DataMember]
		public String Content
        {
            get;
            set;
        }

	    /// <summary>
	    /// 附件
	    /// </summary>
	    [DataMember]
	    public String FjPath
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

		#endregion 属性、变量声明
		
	    public SiteNoticeEntity()
        {
            NoticeID = (long)0;
            JournalID = (long)0;
            ChannelID = (long)0;
            Title = string.Empty;
            Keywords = string.Empty;
            Description = string.Empty;
            Content = string.Empty;
	        FjPath = string.Empty;
            AddDate = DateTime.MinValue;
        }
	}
}
