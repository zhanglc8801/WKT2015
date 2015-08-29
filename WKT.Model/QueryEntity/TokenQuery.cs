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
	///  表'Token'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract,Serializable]
	public partial class TokenQuery :QueryBase
	{
        [DataMember]
        public long AuthorID
        {
            get;
            set;
        }

        /// <summary>
        /// 令牌编码
        /// </summary>
        [DataMember]
        public string Token
        {
            get;
            set;
        }

        /// <summary>
        /// 有效期，令牌添加时间大于等于该日期有效
        /// </summary>
        [DataMember]
        public DateTime ExpireDate
        {
            get;
            set;
        }

        /// <summary>
        /// 类型
        /// </summary>
        [DataMember]
        public int Type
        {
            get;
            set;
        }

	    public TokenQuery()
        {
            Type = 0;
        }
	}
}
