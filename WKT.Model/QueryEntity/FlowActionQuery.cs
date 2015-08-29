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
	///  表'FlowAction'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class FlowActionQuery : QueryBase
	{
        [DataMember]
        public long StatusID
        {
            get;
            set;
        }
        /// <summary>
        /// 操作ID
        /// </summary>
        [DataMember]
        public long ActionID
        {
            get;
            set;
        }
        /// <summary>
        /// 操作名称
        /// </summary>
        [DataMember]
        public string ActionName
        {
            get;
            set;
        }

        [DataMember]
        public int ActionType
        {
            get;
            set;
        }

        [DataMember]
        public long ToStatusID
        {
            get;
            set;
        }

        # region 权限查询

        /// <summary>
        /// 作者ID
        /// </summary>
        [DataMember]
        public long AuthorID
        {
            get;
            set;
        }

        /// <summary>
        /// 角色ID
        /// </summary>
        [DataMember]
        public long RoleID
        {
            get;
            set;
        }

        # endregion

        public FlowActionQuery()
        {
        }
	}
}
