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
    public class IssueSiteEntity
    {
        /// <summary>
        /// 年
        /// </summary>
        [DataMember]
        public int Year { get; set; }
        /// <summary>
        /// 卷
        /// </summary>
        [DataMember]
        public int Volume { get; set; }
        /// <summary>
        /// 期
        /// </summary>
        [DataMember]
        public int Issue { get; set; }

        /// <summary>
        /// 当前期标题图
        /// </summary>
        [DataMember]
        public String TitlePhoto { get; set; }
    }
}
