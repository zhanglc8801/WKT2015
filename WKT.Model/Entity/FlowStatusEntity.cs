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
	///  表'FlowStatus'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class FlowStatusEntity : ObjectBase
    {
        #region 属性
        
        /// <summary>			
        /// StautsID : 
		/// </summary>
		/// <remarks>表FlowStatus主键</remarks>
		[DataMember]
        public Int64 StatusID
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
        /// StatusName : 状态名称
		/// </summary>
		[DataMember]
        public String StatusName
        {
            get;
            set;
        }
		
		/// <summary>
		/// DisplayName : 作者看到的状态名称
		/// </summary>
		[DataMember]
		public String DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// 所属角色
        /// </summary>
        [DataMember]
        public long RoleID
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
		/// Status : 状态 1=有效 0=无效
		/// </summary>
		[DataMember]
		public Byte Status
        {
            get;
            set;
        }

        /// <summary>
        /// 对应的稿件状态
        /// </summary>
        [DataMember]
        public int CStatus
        {
            get;
            set;
        }
		
		/// <summary>
		/// SortID : 排序
		/// </summary>
		[DataMember]
		public Int32 SortID
        {
            get;
            set;
        }
		
		/// <summary>
		/// ContributionCount : 该操作状态下稿件数量
		/// </summary>
		[DataMember]
		public Int32 ContributionCount
        {
            get;
            set;
        }
		
		/// <summary>
		/// Remark : 备注
		/// </summary>
		[DataMember]
		public String Remark
        {
            get;
            set;
        }
		
		/// <summary>
		/// EditDate : 修改时间
		/// </summary>
		[DataMember]
		public DateTime EditDate
        {
            get;
            set;
        }
		
		/// <summary>
		/// EditAuthorID : 修改人
		/// </summary>
		[DataMember]
		public Int64 EditAuthorID
        {
            get;
            set;
        }
		
		/// <summary>
		/// InAuthorID : 录入人
		/// </summary>
		[DataMember]
		public Int64 InAuthorID
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

        public FlowStatusEntity()
        {
            StatusID = 0;
            JournalID = 0;
            StatusName = string.Empty;
            DisplayName = string.Empty;
            Status = 1;
            SortID = 0;
            ContributionCount = 0;
            Remark = null;
            EditDate = DateTime.Now;
            EditAuthorID = 0;
            InAuthorID = 0;
            AddDate = DateTime.Now;
        }
	}
}
