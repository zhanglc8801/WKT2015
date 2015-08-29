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
	///  表'ReviewBillContent'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class ReviewBillContentQuery :QueryBase
	{
        /// <summary>
        /// 添加专家编号
        /// </summary>
        [DataMember]
        public Int64? AddUser { get; set; }

        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public Int64? CID { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64 ItemContentID { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64[] ItemContentIDs { get; set; }

        /// <summary>
        /// 是否是复审稿件
        /// </summary>
        [DataMember]
        public Boolean IsReReview { get; set; }

        /// <summary>
        /// 是否由英文专家审回
        /// </summary>
        [DataMember]
        public Boolean IsEnExpert { get; set; }

        /// <summary>
        /// 列表
        /// </summary>
        [DataMember]
        public IList<ReviewBillContentEntity> list { get; set; }

        /// <summary>
        /// 相关附件url
        /// </summary>
        [DataMember]
        public string PathUrl { get; set; }

        [DataMember]
        public string CFileName { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        [DataMember]
        public string figurePath { get; set; }

        [DataMember]
        public string FFileName { get; set; }

        /// <summary>
        /// 其他附件(介绍信)
        /// </summary>
        [DataMember]
        public string OtherPath { get; set; }

        /// <summary>
        /// 审稿意见
        /// </summary>
        [DataMember]
        public string DealAdvice { get; set; }
		
	    public ReviewBillContentQuery()
        {
            CID = 0;
        }
	}
}
