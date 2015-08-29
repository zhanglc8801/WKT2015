﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    [DataContract]
    public class SiteContentQuery:QueryBase
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
        public Int64 ContentID { get; set; }

        /// <summary>
        /// 编号集合
        /// </summary>
        [DataMember]
        public Int64[] ContentIDs { get; set; }

        /// <summary>
        /// 是否是图片新闻
        /// </summary>
        [DataMember]
        public bool IsPhoto
        {
            get;
            set;
        }

        public SiteContentQuery()
        {
            IsPhoto = false;
        }
    }
}
