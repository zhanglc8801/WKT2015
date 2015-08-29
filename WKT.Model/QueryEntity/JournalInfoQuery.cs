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
	///  表'JournalInfo'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class JournalInfoQuery :QueryBase
	{
        /// <summary>
        /// 编辑部名称
        /// </summary>
        public string JournalName
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

        /// <summary>
        /// 服务开始日期
        /// </summary>
        public string ServiceStartDate
        {
            get;
            set;
        }

        /// <summary>
        /// 服务终止日期
        /// </summary>
        public string ServiceEndDate
        {
            get;
            set;
        }

	    public JournalInfoQuery()
        {
        }
	}
}
