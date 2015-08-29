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
	///  表'MessageRecode'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class MessageRecodeQuery :QueryBase
	{
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public Int64? RecodeID { get; set; }

        /// <summary>
        /// 编号集合
        /// </summary>
        [DataMember]
        public Int64[] RecodeIDs { get; set; }

        /// <summary>
        /// 发送人
        /// </summary>
        [DataMember]
        public Int64? SendUser { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        [DataMember]
        public Int64? ReciveUser { get; set; }

        /// <summary>
        /// 接受地址
        /// </summary>
        [DataMember]
        public String ReciveAddress { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        [DataMember]
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// 类型 1邮件 2短信
        /// </summary>
        [DataMember]
        public Byte? MsgType {get;set;}

        /// <summary>
        /// 发送类型 -1=注册模板 -2=找回密码 -3=审稿费 -4=版面费，还要附加上在审稿操作中设置的各种操作模板
        /// </summary>
        [DataMember]
        public Int32? SendType { get; set; }

        /// <summary>
        /// 为true时查询某个用户相关的记录，SendUser=ReciveUser 且都有值
        /// </summary>
        [DataMember]
        public bool IsUserRelevant { get; set; }

        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public Int64? CID { get; set; }
		
	    public MessageRecodeQuery()
        {
        }
	}
}
