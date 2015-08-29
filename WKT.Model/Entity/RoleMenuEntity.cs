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
	///  表'RoleMenu'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class RoleMenuEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// MapID : 
		/// </summary>
		/// <remarks>表RoleMenu主键</remarks>
		[DisplayName("")]
		[DataMember]
		public Int64 MapID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// JournalID : 
		/// </summary>
        [DisplayName("")]
        [Required(ErrorMessage = "不允许为空!")]
        
		[DataMember]
		public Int64 JournalID
        {
            get;
            set;
        }
		
		/// <summary>
		/// RoleID : 
		/// </summary>
        [DisplayName("")]
        
		[DataMember]
		public Int64? RoleID
        {
            get;
            set;
        }
		
		/// <summary>
		/// MenuID : 
		/// </summary>
        [DisplayName("")]
        
		[DataMember]
		public Int64? MenuID
        {
            get;
            set;
        }
		
		/// <summary>
		/// AddDate : 
		/// </summary>
        [DisplayName("")]
        [Required(ErrorMessage = "不允许为空!")]
        
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }
		#endregion

		#endregion 属性、变量声明
		
	    public RoleMenuEntity()
        {
            MapID = (long)0;
            JournalID = (long)0;
            RoleID = null;
            MenuID = null;
            AddDate = DateTime.Now;
        }
	}
}
