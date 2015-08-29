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
	///  表'ContributionAuthor'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class ContributionAuthorEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// CAuthorID : 
		/// </summary>
		/// <remarks>表ContributionAuthor主键</remarks>		
		[DataMember]
		public Int64 CAuthorID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// CID : 稿件编号
		/// </summary>       
		[DataMember]
		public Int64 CID
        {
            get;
            set;
        }
		
		/// <summary>
		/// JouranalID : 
		/// </summary>     
		[DataMember]
		public Int64 JouranalID
        {
            get;
            set;
        }
		
		/// <summary>
		/// AuthorID : 作者ID
		/// </summary>      
		[DataMember]
		public Int64 AuthorID
        {
            get;
            set;
        }
		
		/// <summary>
		/// AuthorName : 作者名称
		/// </summary>      
		[DataMember]
		public String AuthorName
        {
            get;
            set;
        }
		
		/// <summary>
		/// Gender : 性别 1=男 2=女
		/// </summary>       
		[DataMember]
        public String Gender
        {
            get;
            set;
        }
		
		/// <summary>
		/// Birthday : 出生年月日
		/// </summary>       
		[DataMember]
		public DateTime? Birthday
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
        /// Mobile : 手机
        /// </summary>       
        [DataMember]
        public String Mobile
        {
            get;
            set;
        }
		

		/// <summary>
		/// Email : Email
		/// </summary>       
		[DataMember]
		public String Email
        {
            get;
            set;
        }
		
		/// <summary>
		/// Nation : 民族
		/// </summary>       
		[DataMember]
		public String Nation
        {
            get;
            set;
        }
		
		/// <summary>
		/// NativePlace : 籍贯
		/// </summary>        
		[DataMember]
		public String NativePlace
        {
            get;
            set;
        }
		
		/// <summary>
		/// WorkUnit : 工作单位
		/// </summary>       
		[DataMember]
		public String WorkUnit
        {
            get;
            set;
        }
		
		/// <summary>
		/// SectionOffice : 科室
		/// </summary>      
		[DataMember]
		public String SectionOffice
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
		/// IsFirst : 是否是第一作者 1=是 0=否
		/// </summary>       
		[DataMember]
		public Boolean IsFirst
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsCommunication : 是否是通讯作者 1=是 0=否
		/// </summary>      
		[DataMember]
		public Boolean IsCommunication
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
		
	    public ContributionAuthorEntity()
        {
            CAuthorID = (long)0;
            CID = (long)0;
            JouranalID = (long)0;
            AuthorID = (long)0;
            AuthorName = string.Empty;
            Gender = "1";
            Birthday = null;
            Tel = null;
            Mobile = null;
            Email = null;
            Nation = "汉";
            NativePlace = null;
            WorkUnit = null;
            SectionOffice = null;
            Address = null;
            ZipCode = null;
            AddDate = DateTime.Now;
        }
	}
}
