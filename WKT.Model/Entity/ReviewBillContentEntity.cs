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
	///  表'ReviewBillContent'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class ReviewBillContentEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// ItemContentID : 
		/// </summary>
		/// <remarks>表ReviewBillContent主键</remarks>		
		[DataMember]
		public Int64 ItemContentID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// CID : 
		/// </summary>       
		[DataMember]
		public Int64 CID
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
		/// ItemID : 选项编号
		/// </summary>       
		[DataMember]
		public Int64 ItemID
        {
            get;
            set;
        }
		
		/// <summary>
		/// ContentValue : 输入内容  针对录入型
		/// </summary>       
		[DataMember]
		public String ContentValue
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsChecked : 是否被选中  针对单选、多选
		/// </summary>       
		[DataMember]
		public Boolean IsChecked
        {
            get;
            set;
        }

        /// <summary>
        /// 添加专家
        /// </summary>
        [DataMember]
        public Int64 AddUser { get; set; }
		
		/// <summary>
		/// AddDate : 
		/// </summary>       
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// 选项类型 1=单选 2=多选 3=文本框
        /// </summary>
        [DataMember]
        public Byte ItemType { get; set; }

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
        /// 父级编号
        /// </summary>
        [DataMember]
        public Int64 PItemID { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [DataMember]
        public Int32 SortID { get; set; }
		#endregion

		#endregion 属性、变量声明
		
	    public ReviewBillContentEntity()
        {
            ItemContentID = (long)0;
            CID = (long)0;
            JournalID = (long)0;
            ItemID = (long)0;
            ContentValue = string.Empty;
            IsChecked = false;
            AddUser = (long)0;
            AddDate = DateTime.Now;
        }
	}
}
