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
	///  表'JournalSetInfo'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class JournalSetInfoEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// SetID : 
		/// </summary>
		/// <remarks>表JournalSetInfo主键</remarks>
		[DisplayName("")]
		[DataMember]
		public Int32 SetID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// JournalID : 站点名称
		/// </summary>
        [DisplayName("站点名称")]
        [Required(ErrorMessage = "站点名称不允许为空!")]
        
		[DataMember]
		public Int64 JournalID
        {
            get;
            set;
        }
		
		/// <summary>
		/// ApiSiteID : Api站点ID
		/// </summary>
        [DisplayName("Api站点ID")]
        [Required(ErrorMessage = "Api站点ID不允许为空!")]
        
		[DataMember]
		public Int32 ApiSiteID
        {
            get;
            set;
        }
		
		/// <summary>
		/// DBServerID : db server 站点ID
		/// </summary>
        [DisplayName("db server 站点ID")]
        [Required(ErrorMessage = "db server 站点ID不允许为空!")]
        
		[DataMember]
		public Int32 DBServerID
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
		
	    public JournalSetInfoEntity()
        {
            SetID = (int)0;
            JournalID = (long)0;
            ApiSiteID = (int)0;
            DBServerID = (int)0;
            AddDate = DateTime.Now;
        }
	}
}
