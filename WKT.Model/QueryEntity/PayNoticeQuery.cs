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
    ///  表'PayNotice'的查询实体表示.
    /// </summary>
    /// <remarks>
    /// 该实体由工具生成，请根据实际需求修改本类
    /// </remarks>
    [DataContract]
    public partial class PayNoticeQuery : QueryBase
    {
        /// <summary>
        /// 作者编号
        /// </summary>
        [DataMember]
        public Int64? AuthorID { get; set; }

        /// <summary>
        /// 作者姓名
        /// </summary>
        [DataMember]
        public string AuthorName { get; set; }

        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public string  CNumber { get; set; }

        /// <summary>
        /// 稿件ID
        /// </summary>
        [DataMember]
        public Int64? CID { get; set; }

        /// <summary>
        /// 稿件标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DataMember]
        public string LoginName { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }

        /// <summary>
        /// 状态 0:未交 1:已交 2:待确认 10:未交以及待确认
        /// </summary>
        [DataMember]
        public Byte? Status { get; set; }

        /// <summary>
        /// PayType : 缴费类型：1=审稿费 2=版面费 3=加急费
        /// </summary>        
        [DataMember]
        public Byte? PayType { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64 NoticeID { get; set; }

        /// <summary>
        /// 主键集合
        /// </summary>
        [DataMember]
        public Int64[] NoticeIDs { get; set; }

        /// <summary>
        ///稿件当前所属人
        /// </summary>
        [DataMember]
        public Int64 EditAuthorID { get; set; }

        /// <summary>
        ///是否批量
        /// </summary>
        [DataMember]
        public bool IsBatch { get; set; }

        public PayNoticeQuery()
        {
        }
    }
}
