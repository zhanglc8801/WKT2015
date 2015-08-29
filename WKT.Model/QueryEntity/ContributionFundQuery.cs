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
	///  表'ContributionFund'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class ContributionFundQuery :QueryBase
	{
        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public Int64? CID { get; set; }
		
	    public ContributionFundQuery()
        {
        }
	}
}
