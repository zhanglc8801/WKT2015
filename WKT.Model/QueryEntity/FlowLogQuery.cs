using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    [Serializable]
    [DataContract]
    public class FlowLogQuery : QueryBase
    {
        /// <summary>
        /// 流程日志ID
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

        public FlowLogQuery()
        {
            FlowLogID = 0;
            CID = 0;
        }
    }
}
