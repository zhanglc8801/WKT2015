using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    [DataContract, Serializable]
    public class CRemarkEntity : ObjectBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64 RemarkID { get; set; }

        /// <summary>
        /// 站点标识
        /// </summary>
        [DataMember]
        public Int64 JournalID { get; set; }

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
        /// 备注信息
        /// </summary>
        [DataMember]
        public String Remark { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [DataMember]
        public DateTime AddDate { get; set; }

        public CRemarkEntity()
        {
            RemarkID = 0;
            JournalID = 0;
            CID = 0;
            AuthorID = 0;
            Remark = string.Empty;
            AddDate = DateTime.Now;
        }
    }
}
