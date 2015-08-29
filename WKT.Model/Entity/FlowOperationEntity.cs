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
	///  表'FlowOperation'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class FlowOperationEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// OperationID : 
		/// </summary>
		/// <remarks>表FlowOperation主键</remarks>
		[DisplayName("")]
		[DataMember]
		public Int64 OperationID
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
		/// OperName : 操作名称
		/// </summary>
        [DisplayName("操作名称")]
        [Required(ErrorMessage = "操作名称不允许为空!")]
        [StringLength(50, ErrorMessage = "请勿输入超过 50 个字!")]
        
		[DataMember]
		public String OperName
        {
            get;
            set;
        }
		
		/// <summary>
		/// DisplayName : 作者看到的状态名称
		/// </summary>
        [DisplayName("作者看到的状态名称")]
        [Required(ErrorMessage = "作者看到的状态名称不允许为空!")]
        [StringLength(50, ErrorMessage = "请勿输入超过 50 个字!")]
        
		[DataMember]
		public String DisplayName
        {
            get;
            set;
        }
		
		/// <summary>
		/// Status : 状态 1=有效 0=无效
		/// </summary>
        [DisplayName("状态 1=有效 0=无效")]
        [Required(ErrorMessage = "状态 1=有效 0=无效不允许为空!")]
        
		[DataMember]
		public Byte Status
        {
            get;
            set;
        }
		
		/// <summary>
		/// SortID : 排序
		/// </summary>
        [DisplayName("排序")]
        [Required(ErrorMessage = "排序不允许为空!")]
        
		[DataMember]
		public Int32 SortID
        {
            get;
            set;
        }
		
		/// <summary>
		/// ContributionCount : 该操作状态下稿件数量
		/// </summary>
        [DisplayName("该操作状态下稿件数量")]
        [Required(ErrorMessage = "该操作状态下稿件数量不允许为空!")]
        
		[DataMember]
		public Int32 ContributionCount
        {
            get;
            set;
        }
		
		/// <summary>
		/// Remark : 备注
		/// </summary>
        [DisplayName("备注")]
        [StringLength(200, ErrorMessage = "请勿输入超过 200 个字!")]
        
		[DataMember]
		public String Remark
        {
            get;
            set;
        }
		
		/// <summary>
		/// AddDate : 添加时间
		/// </summary>
        [DisplayName("添加时间")]
        [Required(ErrorMessage = "添加时间不允许为空!")]
        
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }
		#endregion

		#endregion 属性、变量声明
		
	    public FlowOperationEntity()
        {
            OperationID = (long)0;
            JournalID = (long)0;
            OperName = string.Empty;
            DisplayName = string.Empty;
            Status = (byte)1;
            SortID = (int)0;
            ContributionCount = (int)0;
            Remark = null;
            AddDate = DateTime.Now;
        }
	}
}
