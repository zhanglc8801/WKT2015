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
    public class ExpertGroupMapQuery:QueryBase
    {
        /// <summary>
        /// 作者编号
        /// </summary>
        [DataMember]
        public Int64? AuthorID { get; set; }

        /// <summary>
        /// 分组编号  数据字典
        /// </summary>
        [DataMember]
        public Int32 ExpertGroupID { get; set; }

        /// <summary>
        /// 集合
        /// </summary>
        [DataMember]
        public IList<ExpertGroupMapEntity> list { get; set; }
    }
}
