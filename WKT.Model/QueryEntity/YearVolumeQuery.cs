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
	///  表'YearVolume'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class YearVolumeQuery :QueryBase
	{
        /// <summary>
        /// 状态 1:有效 0:无效
        /// </summary>
        [DataMember]
        public Byte? Status { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64 setID { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64[] setIDs { get; set; }
		
	    public YearVolumeQuery()
        {
        }
	}
}
