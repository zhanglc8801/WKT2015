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
	///  表'Menu'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
    [Serializable]
	public partial class MenuQuery : QueryBase
	{
        /// <summary>
        /// 分组ID
        /// </summary>
        [DataMember]
        public int GroupID
        {
            get;
            set;
        }

        [DataMember]
        public long MenuID
        {
            get;
            set;
        }

        [DataMember]
        public IList<long> MenuIDList
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public int? Status
        {
            get;
            set;
        }
		
	    public MenuQuery()
        {
        }
	}
}
