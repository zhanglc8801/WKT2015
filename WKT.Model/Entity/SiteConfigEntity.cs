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
	///  表'SiteConfig'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class SiteConfigEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// SiteConfigID : 
		/// </summary>
		/// <remarks>表SiteConfig主键</remarks>		
		[DataMember]
		public Int64 SiteConfigID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
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
		/// Title : 站点标题
		/// </summary>        
		[DataMember]
		public String Title
        {
            get;
            set;
        }

        /// <summary>
        /// EnTitle : 站点英文标题
        /// </summary>        
        [DataMember]
        public String EnTitle
        {
            get;
            set;
        }

        /// <summary>
        /// CN : 国内刊号
        /// </summary>        
        [DataMember]
        public String CN
        {
            get;
            set;
        }
        /// <summary>
        /// ISSN : 国际刊号
        /// </summary>        
        [DataMember]
        public String ISSN
        {
            get;
            set;
        }
		
		/// <summary>
		/// Keywords : 站点关键字
		/// </summary>       
		[DataMember]
		public String Keywords
        {
            get;
            set;
        }

        /// <summary>
        /// Home : 站点主页地址(如:www.xxx.com)
        /// </summary>       
        [DataMember]
        public String Home
        {
            get;
            set;
        }
		
		/// <summary>
		/// Description : 站点描述
		/// </summary>       
		[DataMember]
		public String Description
        {
            get;
            set;
        }
		
		/// <summary>
		/// ICPCode : ICP备案号
		/// </summary>       
		[DataMember]
		public String ICPCode
        {
            get;
            set;
        }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [DataMember]
        public String ZipCode { get; set; }

        /// <summary>
        /// 通讯地址
        /// </summary>
        [DataMember]
        public String Address { get; set; }
		
		/// <summary>
		/// SendMail : 邮件发送人
		/// </summary>       
		[DataMember]
		public String SendMail
        {
            get;
            set;
        }
		
		/// <summary>
		/// MailServer : 邮件服务器地址
		/// </summary>        
		[DataMember]
		public String MailServer
        {
            get;
            set;
        }
		
		/// <summary>
		/// MailPort : 端口号
		/// </summary>        
		[DataMember]
		public Int32 MailPort
        {
            get;
            set;
        }
		
		/// <summary>
		/// MailAccount : 邮件账号
		/// </summary>       
		[DataMember]
		public String MailAccount
        {
            get;
            set;
        }
		
		/// <summary>
		/// MailPwd : 邮件账号密码
		/// </summary>       
		[DataMember]
		public String MailPwd
        {
            get;
            set;
        }
		
		/// <summary>
		/// MailIsSSL : 邮件是否加密链接
		/// </summary>        
		[DataMember]
		public Boolean MailIsSSL
        {
            get;
            set;
        }
		
		/// <summary>
		/// SMSUserName : 短信发送账号
		/// </summary>       
		[DataMember]
		public String SMSUserName
        {
            get;
            set;
        }
		
		/// <summary>
		/// SMSPwd : 短信发送密码
		/// </summary>       
		[DataMember]
		public String SMSPwd
        {
            get;
            set;
        }

        /// <summary>
        /// 网银类型 1=财富通 2=支付宝 3=易宝支付
        /// </summary>
        [DataMember]
        public Byte EBankType { get; set; }

        /// <summary>
        /// 网银帐号
        /// </summary>
        [DataMember]
        public String EBankAccount { get; set; }

        /// <summary>
        /// 网银商户编码
        /// </summary>
        [DataMember]
        public String EBankCode { get; set; }

        /// <summary>
        /// 网银密钥
        /// </summary>
        [DataMember]
        public String EBankEncryKey { get; set; }


        /// <summary>
        /// DOI注册用户名
        /// </summary>
        [DataMember]
        public String DoiUserName { get; set; }

        /// <summary>
        /// DOI用户密码
        /// </summary>
        [DataMember]
        public String DoiUserPwd { get; set; }

        /// <summary>
        /// DOI期刊ID(可登录中文DOI查询)
        /// </summary>
        [DataMember]
        public String DoiJournalID { get; set; }

        /// <summary>
        /// DOI前缀
        /// </summary>
        [DataMember]
        public String DoiPrefix { get; set; }






		
		/// <summary>
		/// InUserID : 录入人
		/// </summary>        
		[DataMember]
		public Int64 InUserID
        {
            get;
            set;
        }
		
		/// <summary>
		/// EditUserID : 修改人
		/// </summary>       
		[DataMember]
		public Int64 EditUserID
        {
            get;
            set;
        }
		
		/// <summary>
		/// EditDate : 修改日期
		/// </summary>        
		[DataMember]
		public DateTime EditDate
        {
            get;
            set;
        }
		
		/// <summary>
		/// AddDate : 添加日期
		/// </summary>       
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }
		#endregion

		#endregion 属性、变量声明
		
	    public SiteConfigEntity()
        {
            SiteConfigID = (long)0;
            JournalID = (long)0;
            Title = string.Empty;
            EnTitle = string.Empty;
            CN = string.Empty;
            ISSN = string.Empty;
            Home = string.Empty;
            Keywords = string.Empty;
            Description = string.Empty;
            ICPCode = string.Empty;
            ZipCode = string.Empty;
            Address = string.Empty;
            SendMail = string.Empty;
            MailServer = string.Empty;
            MailPort = (int)25;
            MailAccount = string.Empty;
            MailPwd = string.Empty;
            MailIsSSL = false;
            SMSUserName = string.Empty;
            SMSPwd = string.Empty;
            InUserID = (long)0;
            EditUserID = (long)0;
            EditDate = DateTime.Now;
            AddDate = DateTime.Now;
            EBankType = (Byte)3;
            EBankAccount = string.Empty;
            EBankCode = string.Empty;
            EBankEncryKey = string.Empty;
        }
	}
}
