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
	///  表'Dict'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract,Serializable]
	public partial class DictEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// DictID : 
		/// </summary>
		/// <remarks>表Dict主键</remarks>		
		[DataMember]
		public Int64 DictID
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
		/// DictKey : 
		/// </summary>      
		[DataMember]
		public String DictKey
        {
            get;
            set;
        }
		
		/// <summary>
		/// Note : 
		/// </summary>       
		[DataMember]
		public String Note
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
		
	    public DictEntity()
        {
            DictID = (long)0;
            JournalID = (long)0;
            DictKey = string.Empty;
            Note = string.Empty;
            InUserID = (long)0;
            EditUserID = (long)0;
            EditDate = DateTime.Now;
            AddDate = DateTime.Now;
        }
	}
}
