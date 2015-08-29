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
	///  表'RoleMenu'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class RoleMenuQuery :QueryBase
	{
        /// <summary>
        /// RoleID
        /// </summary>
        [DataMember]
        public long? RoleID
        {
            get;
            set;
        }

        /// <summary>
        /// RoleID
        /// </summary>
        [DataMember]
        public IList<long> RoleIDList
        {
            get;
            set;
        }

        /// <summary>
        /// GroupID
        /// </summary>
        [DataMember]
        public int? GroupID
        {
            get;
            set;
        }

        /// <summary>
        /// AuthorID
        /// </summary>
        [DataMember]
        public long? AuthorID
        {
            get;
            set;
        }

        private bool _isExpend = true;
        [DataMember]
        public bool IsExpend
        {
            get { return _isExpend; }
            set { _isExpend = value; }
        }

        [DataMember]
        public string Url
        {
            get;
            set;
        }

	    public RoleMenuQuery()
        {
        }
	}
}
