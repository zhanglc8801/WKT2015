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
	///  表'FlowConfig'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class FlowConfigEntity : ObjectBase
	{
		#region 属性、变量声明
        
		/// <summary>			
		/// FlowConfigID : 
		/// </summary>
		/// <remarks>表FlowConfig主键</remarks>
		[DataMember]
		public Int64 FlowConfigID
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
        /// StatusID : 流程状态ID
		/// </summary>
		[DataMember]
        public Int64 StatusID
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsAllowBack : 是否允许回退 0=不允许 1=允许回退到前一步骤 2=允许回退到之前任意步骤
		/// </summary>
		[DataMember]
		public Byte IsAllowBack
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsMultiPerson : 是否是多人处理环节 0=不是 1=是
		/// </summary>
		[DataMember]
		public Boolean IsMultiPerson
        {
            get;
            set;
        }
		
		/// <summary>
		/// MultiPattern : 多人处理步骤处理方式 1=所有人处理后才流转到下一个环节 2=一个人处理后即可流转到下一个环节
		/// </summary>
		[DataMember]
		public Byte MultiPattern
        {
            get;
            set;
        }
		
		/// <summary>
		/// TimeoutDay : 办理时限，超过这个时间算超时，可发超时提醒
		/// </summary>
		[DataMember]
		public Int32 TimeoutDay
        {
            get;
            set;
        }
		
		/// <summary>
		/// TimeoutPattern : 超时计算方式：1=本步骤接收后在开始计时 2=上一步骤转交后开始计时
		/// </summary>
		[DataMember]
		public Byte TimeoutPattern
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsSMSRemind : 是否短信提醒 0=否 1=是
		/// </summary>
		[DataMember]
		public Boolean IsSMSRemind
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsEmailRemind : 是否邮件提醒 0=否 1=是
		/// </summary>
		[DataMember]
		public Boolean IsEmailRemind
        {
            get;
            set;
        }
		
		/// <summary>
		/// RangeDay : 每隔几天提醒一次
		/// </summary>
		[DataMember]
		public Int32 RangeDay
        {
            get;
            set;
        }
		
		/// <summary>
		/// RemindCount : 一共提醒几次
		/// </summary>
		[DataMember]
		public Int32 RemindCount
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsRetraction : 超时提醒完成后是否撤稿 0=否 1=是
		/// </summary>
		[DataMember]
		public Boolean IsRetraction
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
		
	    public FlowConfigEntity()
        {
            FlowConfigID = 0;
            JournalID = 0;
            StatusID = 0;
            IsAllowBack = 0;
            IsMultiPerson = false;
            MultiPattern = 1;
            TimeoutDay = 0;
            TimeoutPattern = 1;
            IsSMSRemind = false;
            IsEmailRemind = false;
            RangeDay = 1;
            RemindCount = 1;
            IsRetraction = false;
            AddDate = DateTime.Now;
        }
	}
}
