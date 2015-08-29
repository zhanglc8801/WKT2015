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
	///  表'AuthorDetail'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class AuthorDetailQuery :QueryBase
	{
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public Int64 PKID { get; set; }

        /// <summary>
        /// 作者编号  AuthorInfo表主键
        /// </summary>
        [DataMember]
        public Int64 AuthorID { get; set; }

        /// <summary>
        /// 作者编号  AuthorInfo表主键
        /// </summary>
        [DataMember]
        public Int64[] AuthorIDs { get;set;}

        /// <summary>
        /// 1:编辑 2:作者 3:专家
        /// </summary>
        [DataMember]
        public Byte? GroupID { get; set; }

        /// <summary>
        /// 1:启用 0:禁用
        /// </summary>
        [DataMember]
        public Byte? Status { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [DataMember]
        public String LoginName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public String RealName { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        [DataMember]
        public String WorkUnit { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public String Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [DataMember]
        public String ZipCode { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember]
        public String Mobile { get; set; }
		
	    public AuthorDetailQuery()
        {
        }
	}
}
