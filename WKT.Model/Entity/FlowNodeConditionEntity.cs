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
	///  表'FlowNodeCondition'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class FlowNodeConditionEntity : ObjectBase
    {
        #region 属性
        
        /// <summary>			
		/// FlowConditionID : 
		/// </summary>
		/// <remarks>表FlowNodeCondition主键</remarks>
		[DataMember]
		public Int64 FlowConditionID
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
        /// FlowSetID : 流程环节ID
		/// </summary>
		[DataMember]
        public Int64 FlowSetID
        {
            get;
            set;
        }
		
		/// <summary>
		/// ConditionType : 条件类型 1=流入条件 2=流出条件
		/// </summary>
		[DataMember]
		public Byte ConditionType
        {
            get;
            set;
        }
		
		/// <summary>
		/// ConditionExp : 条件表达式
		/// </summary>
		[DataMember]
		public String ConditionExp
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

	    public FlowNodeConditionEntity()
        {
            FlowConditionID = (long)0;
            JournalID = (long)0;
            FlowSetID = (long)0;
            ConditionType = (byte)0;
            ConditionExp = string.Empty;
            AddDate = DateTime.Now;
        }
	}
}
