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
	///  表'YearVolume'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract,Serializable]
	public partial class YearVolumeEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// SetID : 
		/// </summary>
		/// <remarks>表YearVolume主键</remarks>		
		[DataMember]
		public Int64 SetID
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
		/// Year : 年
		/// </summary>       
		[DataMember]
		public Int32 Year
        {
            get;
            set;
        }
		
		/// <summary>
		/// Volume : 卷
		/// </summary>       
		[DataMember]
		public Int32 Volume
        {
            get;
            set;
        }
		
		/// <summary>
		/// Status : 状态 1=有效 0=无效
		/// </summary>       
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
		
	    public YearVolumeEntity()
        {
            SetID = (long)0;
            JournalID = (long)0;
            Year = (int)2005;
            Volume = (int)1;
            Status = (byte)0;
            AddDate = DateTime.Now;
        }
	}
}
