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
	///  表'ContactWay'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class ContactWayEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// ContactID : 
		/// </summary>
		/// <remarks>表ContactWay主键</remarks>		
		[DataMember]
		public Int64 ContactID
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
        /// 栏目编码
        /// </summary>
        [DataMember]
        public Int64 ChannelID { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [DataMember]
        public String Company { get; set; }
		
		/// <summary>
		/// LinkMan : 联系人
		/// </summary>     
		[DataMember]
		public String LinkMan
        {
            get;
            set;
        }
		
		/// <summary>
		/// Address : 联系地址
		/// </summary>        
		[DataMember]
		public String Address
        {
            get;
            set;
        }
		
		/// <summary>
		/// ZipCode : 邮编
		/// </summary>      
		[DataMember]
		public String ZipCode
        {
            get;
            set;
        }
		
		/// <summary>
		/// Tel : 联系电话
		/// </summary>       
		[DataMember]
		public String Tel
        {
            get;
            set;
        }
		
		/// <summary>
		/// Fax : 传真
		/// </summary>        
		[DataMember]
		public String Fax
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
		
	    public ContactWayEntity()
        {
            ContactID = (long)0;
            JournalID = (long)0;
            ChannelID = (long)0;
            Company = string.Empty;
            LinkMan = string.Empty;
            Address = string.Empty;
            ZipCode = string.Empty;
            Tel = string.Empty;
            Fax = string.Empty;
            AddDate = DateTime.Now;
        }
	}
}
