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
	///  表'FlowAction'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class FlowActionEntity : ObjectBase
	{
		#region 属性、变量声明
        
		/// <summary>			
		/// ActionID : 
		/// </summary>
		/// <remarks>表FlowAction主键</remarks>
		[DataMember]
		public Int64 ActionID
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
        /// StatusID : 审稿状态ID
		/// </summary>
		[DataMember]
		public Int64 StatusID
        {
            get;
            set;
        }
		
		/// <summary>
		/// ActionName : 操作名称
		/// </summary>
		[DataMember]
		public String ActionName
        {
            get;
            set;
        }
		
		/// <summary>
		/// DisplayName : 显示名称
		/// </summary>
		[DataMember]
		public String DisplayName
        {
            get;
            set;
        }
		
		/// <summary>
        /// ActionType : 3=系统消息 2=原路返回 1=改变状态
		/// </summary>
		[DataMember]
		public Byte ActionType
        {
            get;
            set;
        }

        /// <summary>
        /// 短信模板
        /// </summary>
        [DataMember]
        public int SMSTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// 邮件模板
        /// </summary>
        [DataMember]
        public int EmailTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// 目标状态ID
        /// </summary>
        [DataMember]
        public long TOStatusID
        {
            get;
            set;
        }

        /// <summary>
        /// 作者投稿状态 操作执行后要修改的稿件的状态
        /// </summary>
        [DataMember]
        public int CStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 排序值
        /// </summary>
        [DataMember]
        public int SortID
        {
            get;
            set;
        }

        /// <summary>
        /// 操作状态 1=有效 0=无效
        /// </summary>
        [DataMember]
        public Byte Status
        {
            get;
            set;
        }

        /// <summary>
        /// 响应分组
        /// </summary>
        [DataMember]
        public long ActionRoleID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否记录显示流程日志
        /// 1=是 0=否
        /// </summary>
        [DataMember]
        public Byte IsShowLog
        {
            get;
            set;
        }

        /// <summary>
        /// 标示是否是在从作者/专家撤稿状态下发送通知
        /// 1=是 0=否
        /// </summary>
        [DataMember]
        public Byte IsRetractionSendMsg
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

        public string StatusName
        {
            get;
            set;
        }
		
	    public FlowActionEntity()
        {
            ActionID = 0;
            JournalID = 0;
            StatusID = 0;
            ActionName = string.Empty;
            DisplayName = string.Empty;
            ActionType = 1;
            CStatus = 0;
            SMSTemplate = 0;
            EmailTemplate = 0;
            TOStatusID = 0;
            SortID = 0;
            Status = 1;
            ActionRoleID = 0;
            AddDate = DateTime.Now;
        }
	}
}
