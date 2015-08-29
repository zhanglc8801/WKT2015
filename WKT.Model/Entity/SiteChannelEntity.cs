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
	///  表'SiteChannel'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class SiteChannelEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// ChannelID : 
		/// </summary>
		/// <remarks>表SiteChannel主键</remarks>	
		[DataMember]
		public Int64 ChannelID
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
        /// PChannelID : 
        /// </summary>
        /// <remarks>表SiteChannel主键</remarks>	
        [DataMember]
        public Int64 PChannelID
        {
            get;
            set;
        }
		
		/// <summary>
		/// ContentType : 内容类型 1=资讯 2=版块 3=图片 4=期刊 5=内容页
		/// </summary>       
		[DataMember]
		public Byte ContentType
        {
            get;
            set;
        }
		
		/// <summary>
		/// ChannelType : 栏目类型 0=不在前台呈现，只是用来区分录入内容 1=首页 2=列表页 3=内容页
		/// </summary>        
		[DataMember]
        public Byte IsNav
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
		/// Description : 栏目描述
		/// </summary>       
		[DataMember]
		public String Description
        {
            get;
            set;
        }
		
		/// <summary>
		/// ChannelUrl : 栏目URL
		/// </summary>       
		[DataMember]
		public String ChannelUrl
        {
            get;
            set;
        }
		
		/// <summary>
		/// SortID : 栏目排序
		/// </summary>     
		[DataMember]
		public Int32 SortID
        {
            get;
            set;
        }
		
		/// <summary>
		/// Status : 状态 1=启用 0=禁用
		/// </summary>       
		[DataMember]
		public Byte Status
        {
            get;
            set;
        }
		
		/// <summary>
		/// AddDate : 录入日期
		/// </summary>       
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }
		#endregion

		#endregion 属性、变量声明

        /// <summary>
        /// 下级栏目
        /// </summary>
        [DataMember]
        public IList<SiteChannelEntity> children { get; set; }
		
	    public SiteChannelEntity()
        {
            ChannelID = (long)0;
            JournalID = (long)0;
            PChannelID = (long)0;
            ContentType = (byte)0;
            IsNav = (byte)0;
            Keywords = string.Empty;
            Description = string.Empty;
            ChannelUrl = string.Empty;
            SortID = (int)0;
            Status = (byte)1;
            AddDate = DateTime.Now;
        }
	}
}
