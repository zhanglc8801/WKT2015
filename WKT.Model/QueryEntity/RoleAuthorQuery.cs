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
	///  表'RoleAuthor'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class RoleAuthorQuery :QueryBase
	{

        [DataMember]
        public long? MapID
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
        public String RoleName
        {
            get;
            set;
        }

        [DataMember]
        public long? AuthorID
        {
            get;
            set;
        }

        [DataMember]
        public String RealName
        {
            get;
            set;
        }

        [DataMember]
        public byte? GroupID
        {
            get;
            set;
        }

	    public RoleAuthorQuery()
        {
        }
	}
}
