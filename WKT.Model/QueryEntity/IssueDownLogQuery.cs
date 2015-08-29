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
	///  表'IssueDownLog'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class IssueDownLogQuery :QueryBase
	{
        /// <summary>
        /// 期刊ID
        /// </summary>
        [DataMember]
        public Int64? ContentID { get; set; }

        /// <summary>
        /// 期刊标题
        /// </summary>
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public String RealName { get; set; }

        /// <summary>
        /// 下载日期_年
        /// </summary>
        [DataMember]
        public Int32 Year { get; set; }

        /// <summary>
        /// 下载日期_月
        /// </summary>
        [DataMember]
        public Int32 Month { get; set; }

        /// <summary>
        /// 下载日期_日
        /// </summary>
        [DataMember]
        public String Daytime { get; set; }

        /// <summary>
        /// 下载客户端IP
        /// </summary>
        [DataMember]
        public String IP { get; set; }

        /// <summary>
        /// 是否导出
        /// </summary>
        [DataMember]
        public bool IsReport { get; set; }

	    public IssueDownLogQuery()
        {
        }
	}
}
