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
	///  表'RoleInfo'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
    [Serializable]
	public partial class RoleInfoEntity : ObjectBase
	{
		/// <summary>			
		/// RoleID : 
		/// </summary>
		/// <remarks>表RoleInfo主键</remarks>
		[DataMember]
		public Int64 RoleID
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
		/// RoleName : 
		/// </summary>
		[DataMember]
		public String RoleName
        {
            get;
            set;
        }
		
		/// <summary>
		/// Note : 备注
		/// </summary>
		[DataMember]
		public String Note
        {
            get;
            set;
        }
		
		/// <summary>
		/// GroupID : 所属分组：1=编辑 2=作者 3=专家
		/// </summary>
		[DataMember]
		public Byte GroupID
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
		
	    public RoleInfoEntity()
        {
            RoleID = (long)0;
            JournalID = (long)0;
            RoleName = "";
            Note = "";
            GroupID = (byte)1;
            AddDate = DateTime.Now;
        }
	}
}
