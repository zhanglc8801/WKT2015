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
    public class SiteBlockEntity:ObjectBase
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public Int64 BlockID { get; set; }

        /// <summary>
        /// 站点唯一标识
        /// </summary>
        [DataMember]
        public Int64 JournalID { get; set; }

        /// <summary>
        /// 栏目ID
        /// </summary>
        [DataMember]
        public Int64 ChannelID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [DataMember]
        public String Linkurl { get; set; }

        /// <summary>
        /// 标题图
        /// </summary>
        [DataMember]
        public String TitlePhoto { get; set; }

        /// <summary>
        /// 内容简介 
        /// </summary>
        [DataMember]
        public String Note { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [DataMember]
        public DateTime AddDate { get; set; }

        public SiteBlockEntity()
        {
            BlockID = (long)0;
            JournalID = (long)0;
            ChannelID = (long)0;
            Title = string.Empty;
            Linkurl = "http://";
            TitlePhoto = "/Content/img/none1.jpg";
            Note = string.Empty;
            AddDate = DateTime.Now;
        }
    }
}
