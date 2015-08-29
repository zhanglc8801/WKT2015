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
	///  表'ContactWay'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class ContactWayQuery : QueryBase
	{
        /// <summary>
        /// 站点唯一标识
        /// </summary>
        [DataMember]
        public Int64 JournalID { get; set; }

        /// <summary>
        /// 栏目编码
        /// </summary>
        [DataMember]
        public Int64 ChannelID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public Int64 ContactID { get; set; }

        /// <summary>
        /// 编号集合
        /// </summary>
        [DataMember]
        public Int64[] ContactIDs { get; set; }
		
	    public ContactWayQuery()
        {
        }
	}
}
