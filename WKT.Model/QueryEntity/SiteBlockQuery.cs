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
    public class SiteBlockQuery:QueryBase
    {
        /// <summary>
        /// 栏目编码
        /// </summary>
        [DataMember]
        public Int64? ChannelID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public Int64 BlockID { get; set; }

        /// <summary>
        /// 编号集合
        /// </summary>
        [DataMember]
        public Int64[] BlockIDs { get; set; }

        public SiteBlockQuery()
        { }
    }
}
