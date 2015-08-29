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
	///  表'ReviewBill'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class ReviewBillEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// ItemID : 
		/// </summary>
		/// <remarks>表ReviewBill主键</remarks>	
		[DataMember]
		public Int64 ItemID
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
		/// Title : 标题
		/// </summary>       
		[DataMember]
		public String Title
        {
            get;
            set;
        }
		
		/// <summary>
		/// ItemType : 选项类型 1=单选 2=多选 3=文本框
		/// </summary>       
		[DataMember]
		public Byte ItemType
        {
            get;
            set;
        }

        /// <summary>
        /// 选项类型 1=单选 2=多选 3=文本框
        /// </summary>       
        [DataMember]
        public String ItemTypeName
        {
            get
            {
                switch (ItemType)
                {
                    case 1: return "单选";
                    case 2: return "多选";
                    case 3: return "文本框";
                    default: return ItemType.ToString();
                }
            }
        }
		
		/// <summary>
		/// PItemID : 父级
		/// </summary>       
		[DataMember]
		public Int64 PItemID
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
		/// AddDate : 
		/// </summary>      
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }
		#endregion

		#endregion 属性、变量声明
		
	    public ReviewBillEntity()
        {
            ItemID = (long)0;
            JournalID = (long)0;
            Title = string.Empty;
            ItemType = (byte)1;
            PItemID = (long)0;
            SortID = (int)0;
            AddDate = DateTime.Now;
        }
	}
}
