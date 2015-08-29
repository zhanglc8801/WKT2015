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
	///  表'FinanceContribute'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class FinanceContributeQuery :QueryBase
	{
        /// <summary>
        /// 投稿人
        /// </summary>
        [DataMember]
        public Int64? AuthorID { get; set; }

        /// <summary>
        /// 费用类型 1=审稿费 2=版面费 3=加急费
        /// </summary>
        [DataMember]
        public Byte? FeeType { get; set; }

        /// <summary>
        /// 是否确认成功 1:确认 0:未确认
        /// </summary>
        [DataMember]
        public Byte? Status { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64 PKID { get; set; }

        /// <summary>
        /// 主键集合
        /// </summary>
        [DataMember]
        public Int64[] PKIDs { get; set; }

        /// <summary>
        /// 是否显示作者信息
        /// </summary>
        [DataMember]
        public bool IsShowAuthor { get; set; }

        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public Int64? CID { get; set; }

        /// <summary>
        /// Title : 稿件标题
        /// </summary>       
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// CNumber : 稿件编号
        /// </summary>       
        [DataMember]
        public String CNumber { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [DataMember]
        public String Keyword { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        [DataMember]
        public Int32? Year { get; set; }

        /// <summary>
        /// 期
        /// </summary>
        [DataMember]
        public Int32? Issue { get; set; }

        /// <summary>
        /// 第一作者
        /// </summary>
        [DataMember]
        public String FirstAuthor { get; set; }

        /// <summary>
        /// 开始入款日期
        /// </summary>
        [DataMember]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束入款日期
        /// </summary>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 是否导出
        /// </summary>
        [DataMember]
        public bool IsReport { get; set; }
		
	    public FinanceContributeQuery()
        {
        }
	}
}
