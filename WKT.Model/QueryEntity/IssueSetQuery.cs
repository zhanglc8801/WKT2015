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
	///  表'IssueSet'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class IssueSetQuery :QueryBase
	{
        /// <summary>
        /// 状态 1:有效 0:无效
        /// </summary>
        [DataMember]
        public Byte? Status { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64 IssueSetID { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64[] IssueSetIDs { get; set; }



        /// <summary>
        /// 确定年
        /// </summary>
        [DataMember]
        public int Year
        {
            get;
            set;
        }
		
	    public IssueSetQuery()
        {
        }
	}
}
