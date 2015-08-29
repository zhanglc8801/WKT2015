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
	///  表'IssueReference'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class IssueReferenceQuery :QueryBase
	{
        /// <summary>
        /// 期刊编号
        /// </summary>
        [DataMember]
        public Int64? ContentID { get; set; }
		
	    public IssueReferenceQuery()
        {
        }
	}
}
