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
	///  表'IssueSet'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract,Serializable]
	public partial class IssueSetEntity : ObjectBase
	{
		#region 属性、变量声明

		#region 主属性 --对应数据表主键
		/// <summary>			
		/// IssueSetID : 
		/// </summary>
		/// <remarks>表IssueSet主键</remarks>		
		[DataMember]
		public Int64 IssueSetID
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
		/// Issue : 
		/// </summary>      
		[DataMember]
		public Int32 Issue
        {
            get;
            set;
        }
		
		/// <summary>
		/// TitlePhoto : 封面图片
		/// </summary>       
		[DataMember]
		public String TitlePhoto
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

        /// <summary>
        /// Year : 年
        /// </summary>      
        [DataMember]
        public int  Year
        {
            get;
            set;
        }
        /// <summary>
        /// Type : 费用类型
        /// </summary>      
        [DataMember]
        public int Type
        {
            get;
            set;
        }

        /// <summary>
        /// PrintExpenses : 费用
        /// </summary>      
        [DataMember]
        public decimal PrintExpenses
        {
            get;
            set;
        }
		#endregion

		#endregion 属性、变量声明
		
	    public IssueSetEntity()
        {
            IssueSetID = (long)0;
            JournalID = (long)0;
            Issue = (int)0;
            TitlePhoto = string.Empty;
            Status = (byte)0;
            AddDate = DateTime.Now;

        }
	}
}
