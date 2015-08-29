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
    public class CRemarkQuery:QueryBase
    {
        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public Int64 CID { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [DataMember]
        public Int64 AuthorID { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64 RemarkID { get; set; }
    }
}
