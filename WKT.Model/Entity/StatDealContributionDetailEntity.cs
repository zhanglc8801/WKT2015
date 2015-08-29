using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    [DataContract]
    [Serializable]
    public class StatDealContributionDetailEntity
    {
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
        /// 稿件编码
        /// </summary>
        [DataMember]
        public string CNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件标题
        /// </summary>
        [DataMember]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        [DataMember]
        public string StatusName
        {
            get;
            set;
        }

        /// <summary>
        /// 处理时间
        /// </summary>
        [DataMember]
        public DateTime DealDate
        {
            get;
            set;
        }
    }
}
