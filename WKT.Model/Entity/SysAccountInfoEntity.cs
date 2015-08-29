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
	///  表'SysAccountInfo'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
    [DataContract]
    [Serializable]
	public partial class SysAccountInfoEntity : ObjectBase
	{
		#region 属性、变量声明

		#region 主属性 --对应数据表主键

		/// <summary>			
		/// 主键ID
		/// </summary>
		/// <remarks>表SysAccountInfo主键</remarks>
        [DataMember]
		public Int32 AdminID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// UserName : 用户名
		/// </summary>
        [DisplayName("用户名")]
        [Required(ErrorMessage = "用户名不允许为空!")]
        [StringLength(20, ErrorMessage = "请勿输入超过 20 个字!")]
		[DataMember]
		public String UserName
        {
            get;
            set;
        }
		
		/// <summary>
		/// LoginName : 登录名
		/// </summary>
        [DisplayName("登录名")]
        [Required(ErrorMessage = "登录名不允许为空!")]
        [StringLength(50, ErrorMessage = "请勿输入超过 50 个字!")]
        
		[DataMember]
		public String LoginName
        {
            get;
            set;
        }
		
		/// <summary>
		/// Pwd : 密码
		/// </summary>
        [DisplayName("密码")]
        [Required(ErrorMessage = "密码不允许为空!")]
        [StringLength(50, ErrorMessage = "请勿输入超过 50 个字!")]
		[DataMember]
		public String Pwd
        {
            get;
            set;
        }
		
		/// <summary>
		/// Gender : 性别 0=男 1=女
		/// </summary>
        [DisplayName("性别 0=男 1=女")]
        [Required(ErrorMessage = "性别 0=男 1=女不允许为空!")]
		[DataMember]
		public Byte Gender
        {
            get;
            set;
        }
		
		/// <summary>
		/// Email : 邮件
		/// </summary>
        [DisplayName("邮件")]
        [Required(ErrorMessage = "邮件不允许为空!")]
        [RegularExpression(@"^[\w\-\.]+@[\w\-\.]+(\.\w+)+$", ErrorMessage = "邮件输入格式有误!")]
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
        [Required(ErrorMessage = "手机不允许为空!")]
        [StringLength(30, ErrorMessage = "请勿输入超过 30 个字!")]
		[DataMember]
		public String Mobile
        {
            get;
            set;
        }
		
		/// <summary>
		/// Status : 状态 0=正常 1=停用
		/// </summary>
        [DisplayName("状态 0=正常 1=停用")]
		[DataMember]
		public Byte Status
        {
            get;
            set;
        }
		
		/// <summary>
		/// LastIP : 登录IP
		/// </summary>
        [DataMember]
		public String LastIP
        {
            get;
            set;
        }
		
		/// <summary>
		/// LoginDate : 最后登录时间
		/// </summary>
        [DataMember]
		public DateTime LoginDate
        {
            get;
            set;
        }
		
		/// <summary>
		/// LogOnTimes : 登录次数
		/// </summary>
        [DataMember]
		public Int32 LogOnTimes
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

		#endregion 属性、变量声明
		
	    public SysAccountInfoEntity()
        {
            AdminID = (int)0;
            UserName = string.Empty;
            LoginName = string.Empty;
            Pwd = string.Empty;
            Gender = (byte)0;
            Email = string.Empty;
            Mobile = string.Empty;
            Status = (byte)0;
            LastIP = string.Empty;
            LoginDate = DateTime.Now;
            LogOnTimes = (int)0;
            AddDate = DateTime.Now;
        }
	}
}
