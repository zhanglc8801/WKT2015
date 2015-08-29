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
	///  表'FlowAuthRole'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class FlowAuthRoleEntity : ObjectBase
    {
        #region 属性
        
        /// <summary>			
		/// RoleAuthID : 
		/// </summary>
		/// <remarks>表FlowAuthRole主键</remarks>
		[DataMember]
		public Int64 RoleAuthID
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
        /// ActionID : 流程环节ID
        /// </summary>
        [DataMember]
        public Int64 ActionID
        {
            get;
            set;
        }
		
		/// <summary>
		/// RoleID : 角色ID
		/// </summary>
		[DataMember]
		public Int64 RoleID
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

        public string RoleName
        {
            get;
            set;
        }
		
	    public FlowAuthRoleEntity()
        {
            RoleAuthID = 0;
            JournalID = 0;
            ActionID = 0;
            RoleID = 0;
            AddDate = DateTime.Now;
        }
	}
}
