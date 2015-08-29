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
	///  表'RoleInfo'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
    [Serializable]
	public partial class RoleInfoQuery : QueryBase
	{
        /// <summary>
        /// 分组 1=编辑 2=作者 3=专家
        /// </summary>
        [DataMember]
        public byte? GroupID
        {
            get;
            set;
        }

        [DataMember]
        public long? RoleID
        {
            get;
            set;
        }

        [DataMember]
        public IList<long> RoleIDList
        {
            get;
            set;
        }
		
	    public RoleInfoQuery()
        {
        }
	}
}
