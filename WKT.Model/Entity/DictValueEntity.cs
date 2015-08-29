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
	///  表'DictValue'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract,Serializable]
	public partial class DictValueEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// DictValueID : 
		/// </summary>
		/// <remarks>表DictValue主键</remarks>	
		[DataMember]
		public Int64 DictValueID
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
		/// DictKey : 数据字典Key
		/// </summary>       
		[DataMember]
		public String DictKey
        {
            get;
            set;
        }
		
		/// <summary>
		/// ValueID : 字典Value
		/// </summary>       
		[DataMember]
		public Int32 ValueID
        {
            get;
            set;
        }
		
		/// <summary>
		/// ValueText : 数据字典值
		/// </summary>      
		[DataMember]
		public String ValueText
        {
            get;
            set;
        }
		
		/// <summary>
		/// SortID : 排序
		/// </summary>       
		[DataMember]
		public Int32 SortID
        {
            get;
            set;
        }
		
		/// <summary>
		/// InUserID : 添加人
		/// </summary>       
		[DataMember]
		public Int64 InUserID
        {
            get;
            set;
        }

        /// <summary>
        /// InUserID : 添加人
        /// </summary>       
        [DataMember]
        public String InUserName
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
		/// EditDate : 修改时间
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
		#endregion

		#endregion 属性、变量声明
		
	    public DictValueEntity()
        {
            DictValueID = (long)0;
            JournalID = (long)0;
            DictKey = string.Empty;
            ValueID = (int)0;
            ValueText = string.Empty;
            SortID = (int)0;
            InUserID = (long)0;
            EditUserID = (long)0;
            EditDate = DateTime.Now;
            AddDate = DateTime.Now;
        }
	}
}
