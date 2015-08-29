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
    public class StatQuery : QueryBase
    {
        /// <summary>
        /// 编辑ID
        /// </summary>
        [DataMember]
        public long AuthorID
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public int  Status
        {
            get;
            set;
        }

        public bool IsNewContribution
        {
            get;
            set;
        }

        /// <summary>
        /// 开始日期
        /// </summary>
        [DataMember]
        public DateTime? StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// 结束日期
        /// </summary>
        [DataMember]
        public DateTime? EndDate
        {
            get;
            set;
        }
    }
}
