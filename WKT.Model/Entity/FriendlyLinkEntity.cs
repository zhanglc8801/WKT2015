using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    /// <summary>
    /// 友情链接实体
    /// </summary>
    [DataContract]
    public class FriendlyLinkEntity : ObjectBase
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public Int64 LinkID { get; set; }

        /// <summary>
        /// 站点唯一标识
        /// </summary>
        [DataMember]
        public Int64 JournalID { get; set; }

        /// <summary>
        /// 栏目编码
        /// </summary>
        [DataMember]
        public Int64 ChannelID { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember]
        public String SiteName { get; set; }

        /// <summary>
        /// 站点URL
        /// </summary>
        [DataMember]
        public String SiteUrl { get; set; }

        /// <summary>
        /// 站点LOGO
        /// </summary>
        [DataMember]
        public String LogoUrl { get; set; }

        /// <summary>
        /// 链接类型 0：文字链接 1：图片链接
        /// </summary>
        [DataMember]
        public Byte LinkType { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [DataMember]
        public Int32 SortID { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [DataMember]
        public DateTime AddDate { get; set; }

        public FriendlyLinkEntity()
        {
            LinkID = (long)0;
            JournalID = (long)0;
            ChannelID = (long)0;
            SiteName = string.Empty;
            SiteUrl = "http://";
            LogoUrl = string.Empty;
            LinkType = (byte)0;
            SortID = 0;
            AddDate = DateTime.Now;
        }
    }
}
