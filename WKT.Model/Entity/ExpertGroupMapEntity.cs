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
    public class ExpertGroupMapEntity:ObjectBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64 PKID { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        [DataMember]
        public Int64 JournalID { get; set; }

        /// <summary>
        /// 作者编号
        /// </summary>
        [DataMember]
        public Int64 AuthorID { get; set; }

        /// <summary>
        /// 分组编号  数据字典
        /// </summary>
        [DataMember]
        public Int32 ExpertGroupID { get; set; }

        /// <summary>
        /// 添加日期
        /// </summary>
        [DataMember]
        public DateTime AddDate { get; set; }

        public ExpertGroupMapEntity()
        {
            PKID = (long)0;
            JournalID = (long)0;
            AuthorID = (long)0;
            ExpertGroupID = 0;
            AddDate = DateTime.Now;
        }
    }
}
