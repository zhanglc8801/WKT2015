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
	///  表'ContributionAuthor'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class ContributionAuthorQuery :QueryBase
	{
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public Int64 CAuthorID { get; set; }

        /// <summary>
        /// 编号集合
        /// </summary>
        [DataMember]
        public Int64[] CAuthorIDs { get; set; }

        /// <summary>
        /// 是否是在修改稿(用于显示修改稿件页的联系电话为单个)
        /// </summary>
        [DataMember]
        public Boolean isModify { get; set; }

        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public Int64? CID { get; set; }
		
	    public ContributionAuthorQuery()
        {
        }
	}
}
