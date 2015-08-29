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
	///  表'DBServerInfo'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class DBServerInfoEntity : ObjectBase
	{
		#region 属性、变量声明

		#region 主属性 --对应数据表主键

		/// <summary>			
		/// DBServerID : 
		/// </summary>
		/// <remarks>表DBServerInfo主键</remarks>
		[DataMember]
		public Int32 DBServerID
        {
            get;
            set;
        }

		#endregion
		
		#region 属性
		
		/// <summary>
		/// ServerIP : 服务器IP
		/// </summary>
        [DisplayName("服务器IP")]
        [Required(ErrorMessage = "服务器IP不允许为空!")]
        [StringLength(50, ErrorMessage = "请勿输入超过 50 个字!")]
        
		[DataMember]
		public String ServerIP
        {
            get;
            set;
        }
		
		/// <summary>
		/// Port : 端口号
		/// </summary>
        [DisplayName("端口号")]
        [Required(ErrorMessage = "端口号不允许为空!")]
        
		[DataMember]
		public Int32 Port
        {
            get;
            set;
        }
		
		/// <summary>
		/// Account : 账号
		/// </summary>
        [DisplayName("账号")]
        [Required(ErrorMessage = "账号不允许为空!")]
        [StringLength(50, ErrorMessage = "请勿输入超过 50 个字!")]
        
		[DataMember]
		public String Account
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
		
	    public DBServerInfoEntity()
        {
            DBServerID = (int)0;
            ServerIP = string.Empty;
            Port = (int)1431;
            Account = string.Empty;
            Pwd = string.Empty;
            Note = "";
            Status = (byte)1;
            AddDate = DateTime.Now;
        }
	}
}
