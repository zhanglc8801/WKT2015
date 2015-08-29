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
	///  表'SiteMessage'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class SiteMessageQuery :QueryBase
	{
        /// <summary>
        /// 发送人
        /// </summary>
        [DataMember]
        public Int64? SendUser { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        [DataMember]
        public Int64? ReciverID { get; set; }

        /// <summary>
        /// 是否查看 0：否 1：是
        /// </summary>
        [DataMember]
        public Byte? IsView { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64 MessageID { get; set; }

        /// <summary>
        /// 主键集合
        /// </summary>
        [DataMember]
        public Int64[] MessageIDs { get; set; }

        /// <summary>
        /// 为true时查询某个用户相关的记录，SendUser=ReciverID 且必须都有值
        /// </summary>
        [DataMember]
        public bool IsUserRelevant { get; set; }

        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public Int64? CID { get; set; }
		
	    public SiteMessageQuery()
        {
        }
	}
}
