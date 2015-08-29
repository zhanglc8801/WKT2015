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
	///  表'DictValue'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class DictValueQuery :QueryBase
	{
        /// <summary>
        /// 站点唯一标识
        /// </summary>
        [DataMember]
        public Int64 JournalID { get; set; }

        /// <summary>
        /// 字典编号
        /// </summary>
        [DataMember]
        public Int64 DictID { get; set; }

        /// <summary>
        /// 字典值编号
        /// </summary>
        [DataMember]
        public Int64 DictValueID { get; set; }

        /// <summary>
        /// 字典值编号集合
        /// </summary>
        [DataMember]
        public Int64[] DictValueIDs { get; set; }

        /// <summary>
        /// 字典key
        /// </summary>
        [DataMember]
        public String DictKey { get; set; }
		
	    public DictValueQuery()
        {
        }
	}
}
