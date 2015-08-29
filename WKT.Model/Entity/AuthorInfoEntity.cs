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
	///  表'AuthorInfo'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
    [Serializable]
	public partial class AuthorInfoEntity : ObjectBase
	{
		#region 属性、变量声明

		/// <summary>			
		/// AuthorID : 作者信息
		/// </summary>
		/// <remarks>表AuthorInfo主键</remarks>
		[DataMember]
		public Int64 AuthorID
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
		/// LoginName : 
		/// </summary>
		[DataMember]
		public String LoginName
        {
            get;
            set;
        }
		
		/// <summary>
		/// Pwd : 
		/// </summary>
		[DataMember]
		public String Pwd
        {
            get;
            set;
        }
		
		/// <summary>
		/// RealName : 真实姓名
		/// </summary>
		[DataMember]
		public String RealName
        {
            get;
            set;
        }
		
		/// <summary>
		/// LoginIP : 登录ID
		/// </summary>
		[DataMember]
		public String LoginIP
        {
            get;
            set;
        }
		
		/// <summary>
		/// LoginCount : 登录次数
		/// </summary>
		[DataMember]
		public Int32 LoginCount
        {
            get;
            set;
        }
		
		/// <summary>
		/// LoginDate : 登录时间
		/// </summary>
		[DataMember]
		public DateTime LoginDate
        {
            get;
            set;
        }

        /// <summary>
        /// GroupID : 所属分组 1=编辑 2=作者 3=专家 4=英文专家
        /// </summary>
        [DataMember]
        public Byte GroupID
        {
            get;
            set;
        }

        /// <summary>
        /// OldGroupID : 所属分组，用于重新设置分组后保留原分组的值 1=编辑 2=作者 3=专家 4=英文专家
        /// </summary>
        [DataMember]
        public Byte OldGroupID
        {
            get;
            set;
        }
		
		/// <summary>
		/// Status : 状态 1=正常 0=删除
		/// </summary>
		[DataMember]
		public Byte Status
        {
            get;
            set;
        }

        /// <summary>
        /// 积分
        /// </summary>
        [DataMember]
        public int Points
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

        /// <summary>
        /// 手机号
        /// </summary>
        [DataMember]
        public string Mobile
        {
            get;
            set;
        }

        /// <summary>
        /// 新密码
        /// </summary>
        [DataMember]
        public string NewPwd
        {
            get;
            set;
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        [DataMember]
        public IList<long> RoleIDList
        {
            get;
            set;
        }

        /// <summary>
        /// RoleID
        /// </summary>
        [DataMember]
        public long? RoleID
        {
            get;
            set;
        }

        [DataMember]
        public string RoleName
        {
            get;
            set;
        }

        /// <summary>
        /// OldRoleID
        /// </summary>
        [DataMember]
        public long OldRoleID
        {
            get;
            set;
        }

        # region 专家助选属性

        /// <summary>
        /// 审稿数量
        /// </summary>
        [DataMember]
        public int AuditCount
        {
            get;
            set;
        }

        [DataMember]
        public int AuditedCount
        {
            get;
            set;
        }

        /// <summary>
        /// 研究方向
        /// </summary>
        [DataMember]
        public string ExpertList
        {
            get;
            set;
        }

        # endregion

        public AuthorInfoEntity()
        {
            AuthorID = (long)0;
            JournalID = (long)0;
            LoginName = "";
            Pwd = "";
            //RealName = "";
            Mobile = "";
            LoginIP = "";
            LoginCount = (int)0;
            LoginDate = DateTime.Now;
            Status = (byte)1;
            //GroupID = (byte)2;
            AddDate = DateTime.Now;
        }
	}
}
