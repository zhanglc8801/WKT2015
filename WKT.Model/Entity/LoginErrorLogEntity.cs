using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{

    [Serializable]
    [DataContract]
    public partial class LoginErrorLogEntity:ObjectBase
    {
        #region 属性、变量声明

		/// <summary>			
		/// PKID : 登录错误日志信息
		/// </summary>
		/// <remarks>表LoginErrorLog主键</remarks>
		[DataMember]
		public Int64 PKID
        {
            get;
            set;
        }

        /// <summary>
        /// 杂志ID
        /// </summary>
        [DataMember]
        public Int64 JournalID
        {
            get;
            set;
        }
		
		/// <summary>
        /// LoginIP : 登录者IP
		/// </summary>
		[DataMember]
		public String LoginIP
        {
            get;
            set;
        }
		
		/// <summary>
        /// LoginHost : 登录者主机名
		/// </summary>
		[DataMember]
		public String LoginHost
        {
            get;
            set;
        }
		
		/// <summary>
        /// LoginName : 登录者用户名
		/// </summary>
		[DataMember]
		public String LoginName
        {
            get;
            set;
        }

		/// <summary>
		/// LoginDate : 尝试登录时间
		/// </summary>
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }
		#endregion

        public LoginErrorLogEntity()
        {
            JournalID = 0;
            LoginIP = string.Empty;
            LoginHost = string.Empty;
            LoginName = string.Empty;
            AddDate = DateTime.Now;
        }


    }
}
