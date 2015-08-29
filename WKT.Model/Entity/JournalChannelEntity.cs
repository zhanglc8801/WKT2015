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
	///  表'JournalChannel'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract,Serializable]
	public partial class JournalChannelEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// JChannelID : 
		/// </summary>
		/// <remarks>表JournalChannel主键</remarks>		
		[DataMember]
		public Int64 JChannelID
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
		/// PChannelID : 父栏目ID
		/// </summary>       
		[DataMember]
		public Int64 PChannelID
        {
            get;
            set;
        }
		
		/// <summary>
		/// ChannelName : 栏目名称
		/// </summary>       
		[DataMember]
		public String ChannelName
        {
            get;
            set;
        }
		
		/// <summary>
		/// SortID : 排序值
		/// </summary>       
		[DataMember]
		public Int32 SortID
        {
            get;
            set;
        }
		
		/// <summary>
		/// Status : 状态 1=启用 0=停用
		/// </summary>      
		[DataMember]
		public Byte Status
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
		
	    public JournalChannelEntity()
        {
            JChannelID = (long)0;
            JournalID = (long)0;
            PChannelID = (long)0;
            ChannelName = string.Empty;
            SortID = (int)0;
            Status = (byte)1;
            AddDate = DateTime.Now;
        }
	}
}
