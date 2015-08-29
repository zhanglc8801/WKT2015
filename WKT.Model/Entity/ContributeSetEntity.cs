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
	///  表'ContributeSet'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class ContributeSetEntity : ObjectBase
	{
		/// <summary>			
		/// CSetID : 
		/// </summary>
		/// <remarks>表ContributeSet主键</remarks>
		[DataMember]
		public Int64 CSetID
        {
            get;
            set;
        }
		
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
		/// CCodeType : 稿件编号类型 1=日期格式 2=自定义 3=年份格式
		/// </summary>
		[DataMember]
		public Byte CCodeType
        {
            get;
            set;
        }
		
		/// <summary>
		/// CCodeFormat : 稿件编码格式，如果是日期格式，这里存放的是日期格式化字符串，如果是自定义格式这里存的是前缀
		/// </summary>
		[DataMember]
		public String CCodeFormat
        {
            get;
            set;
        }
		
		/// <summary>
		/// Separator : 分隔符
		/// </summary>
		[DataMember]
		public String Separator
        {
            get;
            set;
        }
		
		/// <summary>
		/// RandomDigit : 稿件编号，在设置了格式后后面跟几位随机数
		/// </summary>
		[DataMember]
		public Byte RandomDigit
        {
            get;
            set;
        }
		
		/// <summary>
		/// Statement : 投稿声明
		/// </summary>
		[DataMember]
		public String Statement
        {
            get;
            set;
        }
		
		/// <summary>
		/// InUser : 录入人
		/// </summary>
		[DataMember]
		public Int64 InUser
        {
            get;
            set;
        }
		
		/// <summary>
		/// EditUser :  修改人
		/// </summary>
		[DataMember]
		public Int64 EditUser
        {
            get;
            set;
        }
		
		/// <summary>
		/// EditDate :  修改时间
		/// </summary>
		[DataMember]
		public DateTime EditDate
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
		
	    public ContributeSetEntity()
        {
            CSetID = (long)0;
            JournalID = (long)0;
            CCodeType = (byte)3;
            CCodeFormat = string.Empty;
            Separator = "-";
            RandomDigit = (byte)3;
            Statement = string.Empty;
            InUser = (long)0;
            EditUser = (long)0;
            EditDate = DateTime.Now;
            AddDate = DateTime.Now;
        }
	}
}
