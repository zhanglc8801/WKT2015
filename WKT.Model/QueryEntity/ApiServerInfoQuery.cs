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
	///  表'ApiServerInfo'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class ApiServerInfoQuery :QueryBase
	{
        /// <summary>
        /// 站点名称
        /// </summary>
        public string SiteName
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public byte? Status
        {
            get;
            set;
        }
		
	    public ApiServerInfoQuery()
        {
        }
	}
}
