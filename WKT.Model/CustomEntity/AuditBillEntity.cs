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
    /// 审稿信息
    /// </summary>
    [DataContract, Serializable]
    public class AuditBillEntity
    {
        /// <summary>
        /// 编辑部ID
        /// </summary>
        [DataMember]
        public long JournalID
        {
            get;
            set;
        }

        /// <summary>
        /// 处理日志ID
        /// </summary>
        [DataMember]
        public long FlowLogID
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件ID
        /// </summary>
        [DataMember]
        public long CID
        {
            get;
            set;
        }

        /// <summary>
        /// 接收人，多个接收人之间用逗号分割
        /// </summary>
        [DataMember]
        public string ReveiverList
        {
            get;
            set;
        }

        /// <summary>
        /// 审毕时间
        /// </summary>
        [DataMember]
        public DateTime? AuditedDate
        {
            get;
            set;
        }

        /// <summary>
        /// 是否发邮件
        /// </summary>
        [DataMember]
        public bool IsEmail
        {
            get;
            set;
        }

        /// <summary>
        /// 邮件标题
        /// </summary>
        [DataMember]
        public string EmailTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 邮件内容
        /// </summary>
        [DataMember]
        public string EmailBody
        {
            get;
            set;
        }

        /// <summary>
        /// 审稿意见
        /// </summary>
        [DataMember]
        public string DealAdvice
        {
            get;
            set;
        }

        [DataMember]
        private IDictionary<long, string> _dictDealAdvice = new Dictionary<long, string>();
        /// <summary>
        /// 审稿意见
        /// </summary>
        [DataMember]
        public IDictionary<long, string> DictDealAdvice
        {
            get
            {
                return _dictDealAdvice;
            }
            set
            {
                _dictDealAdvice = value;
            }
        }

        /// <summary>
        ///  是否发送短信
        /// </summary>
        [DataMember]
        public bool IsSMS
        {
            get;
            set;
        }

        /// <summary>
        /// 短信内容
        /// </summary>
        [DataMember]
        public string SMSBody
        {
            get;
            set;
        }
        /// <summary>
        /// 稿件附件
        /// </summary>
        [DataMember]
        public string CPath
        {
            get;
            set;
        }
        /// <summary>
        /// 稿件自定义文件名
        /// </summary>
        [DataMember]
        public string CFileName
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件图表附件
        /// </summary>
        [DataMember]
        public string FigurePath
        {
            get;
            set;
        }

        /// <summary>
        /// 附件自定义文件名
        /// </summary>
        [DataMember]
        public string FFileName { get; set; }

        /// <summary>
        /// 其他附件
        /// </summary>
        [DataMember]
        public string OtherPath
        {
            get;
            set;
        }

        /// <summary>
        /// 状态ID
        /// </summary>
        [DataMember]
        public long StatusID
        {
            get;
            set;
        }

        /// <summary>
        /// 操作ID
        /// </summary>
        [DataMember]
        public long ActionID
        {
            get;
            set;
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        [DataMember]
        public int ActionType
        {
            get;
            set;
        }

        /// <summary>
        /// 处理人
        /// </summary>
        [DataMember]
        public long Processer
        {
            get;
            set;
        }

        /// <summary>
        /// 是否继续送交 
        /// </summary>
        [DataMember]
        public bool IsContinueSubmit
        {
            get;
            set;
        }


        /// <summary>
        /// 邮件模板ID
        /// </summary>
        [DataMember]
        public long TemplateID
        {
            get;
            set;
        }
    }
}
