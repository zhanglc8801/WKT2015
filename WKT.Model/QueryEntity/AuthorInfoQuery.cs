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
	///  表'AuthorInfo'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
    [Serializable]
    [DataContract]
	public partial class AuthorInfoQuery : QueryBase
	{
        [DataMember]
        public long? AuthorID
        {
            get;
            set;
        }

        /// <summary>
        /// 作者编号集合
        /// </summary>
        [DataMember]
        public long[] AuthorIDs
        {
            get;
            set;
        }

        [DataMember]
        public string LoginName
        {
            get;
            set;
        }

        [DataMember]
        public string RealName
        {
            get;
            set;
        }

        [DataMember]
        public string Pwd
        {
            get;
            set;
        }

        [DataMember]
        public byte? Status
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

        /// <summary>
        /// 专家所属分组
        /// </summary>
        [DataMember]
        public int? ExpertGroupID
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
        public string Remark
        {
            get;
            set;
        }

        [DataMember]
        public IList<long> AuthorIDList
        {
            get;
            set;
        }

        /// <summary>
        /// 研究方向
        /// </summary>
        [DataMember]
        public string ResearchTopics
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是在选择英文专家
        /// </summary>
        [DataMember]
        public bool IsSelEnExpert
        {
            get;
            set;
        }

	    public AuthorInfoQuery()
        {
        }
	}
}
