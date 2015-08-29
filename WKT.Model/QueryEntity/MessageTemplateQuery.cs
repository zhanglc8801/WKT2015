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
	///  表'MessageTemplate'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class MessageTemplateQuery :QueryBase
	{
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public Int64? TemplateID { get; set; }

        /// <summary>
        /// 编号集合
        /// </summary>
        [DataMember]
        public Int64[] TemplateIDs { get; set; }

        /// <summary>
        /// 模板类型 1=邮件模板 2=短信模板
        /// </summary>
        [DataMember]
        public Byte? TType { get; set; }

        /// <summary>
        /// 发送类型 -1=注册模板 -2=找回密码 -3=审稿费 -4=版面费，还要附加上在审稿操作中设置的各种操作模板
        /// </summary>
        [DataMember]
        public Int32? TCategory { get; set; }

        /// <summary>
        /// 模版名称
        /// </summary>
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// 实体类型 0:根据主键获取实体 1:根据模版类型获取实体
        /// </summary>
        [DataMember]
        public Int32 ModelType { get; set; }
		
	    public MessageTemplateQuery()
        {
        }
	}
}
