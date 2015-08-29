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
    public class SiteResourceEntity:ObjectBase
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public Int64 ResourceID { get; set; }

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
        /// 文件名称
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// 文件说明
        /// </summary>
        [DataMember]
        public String FileIntro { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [DataMember]
        public String FilePath { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        [DataMember]
        public String FileEx { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        [DataMember]
        public Decimal FileSize { get; set; }

        /// <summary>
        /// 下载次数
        /// </summary>
        [DataMember]
        public Int32 DownloadCount { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public Int64 InUserID { get; set; }

        /// <summary>
        /// 编辑人
        /// </summary>
        [DataMember]
        public Int64 EditUserID { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        [DataMember]
        public DateTime EditDate { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [DataMember]
        public DateTime AddDate { get; set; }

        public SiteResourceEntity()
        {
            ResourceID = (long)0;
            JournalID = (long)0;
            ChannelID = (long)0;
            Name = string.Empty;
            FileIntro = string.Empty;
            FilePath = string.Empty;
            FileEx = string.Empty;
            FileSize = (decimal)0;
            DownloadCount = 0;
            InUserID = (long)0;
            EditUserID = (long)0;
            EditDate = DateTime.Now;
            AddDate = DateTime.Now;
        }
    }
}
