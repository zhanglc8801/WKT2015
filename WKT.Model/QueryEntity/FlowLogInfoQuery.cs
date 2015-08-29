﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
	/// <summary>
	///  表'FlowLogInfo'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class FlowLogInfoQuery :QueryBase
	{
        /// <summary>
        /// 稿件ID
        /// </summary>
        [DataMember]
        public Int64? CID { get; set; }

        /// <summary>
        /// 稿件流程ID
        /// </summary>
        [DataMember]
        public Int64? FlowLogID { get; set; }

        /// <summary>
        /// 作者编号
        /// </summary>
        [DataMember]
        public Int64? AuhorID { get; set; }

        /// <summary>
        /// 所属分组 1=编辑 2=作者 3=专家
        /// </summary>
        [DataMember]
        public Byte? GroupID { get; set; }
		
	    public FlowLogInfoQuery()
        {
        }
	}
}
