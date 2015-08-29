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
	///  表'FlowStatus'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class FlowStatusQuery : QueryBase
	{
        [DataMember]
        public long? StatusID
        {
            get;
            set;
        }

        /// <summary>
        /// 当前登录人
        /// </summary>
        [DataMember]
        public long CurAuthorID
        {
            get;
            set;
        }

        /// <summary>
        /// 当前登录人角色ID
        /// </summary>
        [DataMember]
        public long RoleID
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件状态
        /// </summary>
        [DataMember]
        public int CStatus
        {
            get;
            set;
        }

        [DataMember]
        public DateTime? StartDate
        {
            get;
            set;
        }

        [DataMember]
        public DateTime? EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是已处理 0=待处理 1=已处理 2=全部 3=非可操作状态下的全部已处理
        /// </summary>
        [DataMember]
        public byte IsHandled
        {
            get;
            set;
        }

        public FlowStatusQuery()
        {
            IsHandled = 0;
        }
	}
}
