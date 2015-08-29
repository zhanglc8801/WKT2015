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
	///  表'SiteChannel'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class SiteChannelQuery :QueryBase
	{
        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public byte? Status { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public Int64? ChannelID { get; set; }

        [DataMember]
        public byte? IsNav
        {
            get;
            set;
        }

        /// <summary>
        /// url
        /// </summary>
        [DataMember]
        public string ChannelUrl { get; set; }
		
	    public SiteChannelQuery()
        {
        }
	}
}
