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
	///  表'ApiServerInfo'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class ApiServerInfoEntity : ObjectBase
	{
		#region 属性、变量声明

		#region 主属性 --对应数据表主键

		/// <summary>			
		/// ApiServerID : 
		/// </summary>
		/// <remarks>表ApiServerInfo主键</remarks>
		[DataMember]
		public Int32 ApiServerID
        {
            get;
            set;
        }

		#endregion
		
		#region 属性
		
		/// <summary>
		/// SiteName : 站点名称
		/// </summary>
        [DisplayName("站点名称")]
        [Required(ErrorMessage = "站点名称不允许为空!")]
        [StringLength(50, ErrorMessage = "请勿输入超过 50 个字!")]
        
		[DataMember]
		public String SiteName
        {
            get;
            set;
        }
		
		/// <summary>
		/// SiteUrl : 站点Url，例如：http://api.scj.cn
		/// </summary>
        [DisplayName("站点Url，例如：http://api.scj.cn")]
        [Required(ErrorMessage = "站点Url，例如：http://api.scj.cn不允许为空!")]
        [StringLength(100, ErrorMessage = "请勿输入超过 100 个字!")]
        
		[DataMember]
		public String SiteUrl
        {
            get;
            set;
        }
		
		/// <summary>
		/// Status : 站点状态 0=禁用 1=启用
		/// </summary>
        [DisplayName("站点状态 0=禁用 1=启用")]
        [Required(ErrorMessage = "站点状态 0=禁用 1=启用不允许为空!")]
        
		[DataMember]
		public Byte Status
        {
            get;
            set;
        }
		
		/// <summary>
		/// Note : 备注
		/// </summary>
        [DisplayName("备注")]
        [StringLength(200, ErrorMessage = "请勿输入超过 200 个字!")]
		[DataMember]
		public String Note
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

		#endregion 属性、变量声明
		
	    public ApiServerInfoEntity()
        {
            ApiServerID = (int)0;
            SiteName = string.Empty;
            SiteUrl = string.Empty;
            Status = (byte)1;
            Note = string.Empty;
            AddDate = DateTime.Now;
        }
	}
}
