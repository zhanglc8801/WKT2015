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
	///  表'JournalInfo'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class JournalInfoEntity : ObjectBase
	{
		#region 主属性 --对应数据表主键

		/// <summary>			
		/// JournalID : 站点ID
		/// </summary>
		/// <remarks>表JournalInfo主键</remarks>
		[DataMember]
		public Int64 JournalID
        {
            get;
            set;
        }

		#endregion
		
		#region 属性
		
		/// <summary>
		/// JournalName : 杂志社名称
		/// </summary>
        [DisplayName("杂志社名称")]
        [Required(ErrorMessage = "杂志社名称不允许为空!")]
        [StringLength(50, ErrorMessage = "请勿输入超过 50 个字!")]
		[DataMember]
		public String JournalName
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
		/// ServiceStartDate : 站点服务开始时间
		/// </summary>
        [DisplayName("站点服务开始时间")]
        [Required(ErrorMessage = "站点服务开始时间不允许为空!")]
		[DataMember]
		public DateTime ServiceStartDate
        {
            get;
            set;
        }
		
		/// <summary>
		/// ServiceEndDate : 站点服务终止时间，该时间到期后会终止提供服务
		/// </summary>
        [DisplayName("站点服务终止时间，该时间到期后会终止提供服务")]
        [Required(ErrorMessage = "站点服务终止时间，该时间到期后会终止提供服务不允许为空!")]
		[DataMember]
		public DateTime ServiceEndDate
        {
            get;
            set;
        }
		
		/// <summary>
		/// Linkman : 联系人
		/// </summary>
        [DisplayName("联系人")]
        [Required(ErrorMessage = "联系人不允许为空!")]
        [StringLength(20, ErrorMessage = "请勿输入超过 20 个字!")]
		[DataMember]
		public String Linkman
        {
            get;
            set;
        }
		
		/// <summary>
		/// LinkTel : 联系电话
		/// </summary>
        [DisplayName("联系电话")]
        [StringLength(20, ErrorMessage = "请勿输入超过 20 个字!")]
		[DataMember]
		public String LinkTel
        {
            get;
            set;
        }
		
		/// <summary>
		/// Fax : 传真
		/// </summary>
        [DisplayName("传真")]
        [StringLength(20, ErrorMessage = "请勿输入超过 20 个字!")]
		[DataMember]
		public String Fax
        {
            get;
            set;
        }
		
		/// <summary>
		/// Email : Email
		/// </summary>
        [DisplayName("Email")]
        [RegularExpression(@"^[\w\-\.]+@[\w\-\.]+(\.\w+)+$", ErrorMessage = "Email输入格式有误!")]
        [StringLength(100, ErrorMessage = "请勿输入超过 100 个字!")]
		[DataMember]
		public String Email
        {
            get;
            set;
        }
		
		/// <summary>
		/// Mobile : 手机
		/// </summary>
        [DisplayName("手机")]
        [StringLength(20, ErrorMessage = "请勿输入超过 20 个字!")]
		[DataMember]
		public String Mobile
        {
            get;
            set;
        }
		
		/// <summary>
		/// Address : 联系地址
		/// </summary>
        [DisplayName("联系地址")]
        [StringLength(100, ErrorMessage = "请勿输入超过 100 个字!")]
		[DataMember]
		public String Address
        {
            get;
            set;
        }
		
		/// <summary>
		/// ZipCode : 邮编
		/// </summary>
        [DisplayName("邮编")]
        [StringLength(10, ErrorMessage = "请勿输入超过 10 个字!")]
        
		[DataMember]
		public String ZipCode
        {
            get;
            set;
        }
		
		/// <summary>
		/// AuthorizationCode : 授权码
		/// </summary>
        [DisplayName("授权码")]
        [Required(ErrorMessage = "授权码不允许为空!")]
        [StringLength(200, ErrorMessage = "请勿输入超过 200 个字!")]
		[DataMember]
		public String AuthorizationCode
        {
            get;
            set;
        }
		
		/// <summary>
		/// Status : 状态 0=禁用 1=启用
		/// </summary>
        [DisplayName("状态 0=禁用 1=启用")]
        [Required(ErrorMessage = "状态 0=禁用 1=启用不允许为空!")]
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
        [Required(ErrorMessage = "备注不允许为空!")]
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
		
	    public JournalInfoEntity()
        {
            JournalID = (long)0;
            JournalName = "";
            SiteUrl = "";
            ServiceStartDate = DateTime.MinValue;
            ServiceEndDate = DateTime.MinValue;
            Linkman = "";
            LinkTel = "";
            Fax = "";
            Email = "";
            Mobile = "";
            Address = "";
            ZipCode = "";
            AuthorizationCode = "";
            Status = (byte)1;
            Note = "";
            AddDate = DateTime.Now;
        }
	}
}
