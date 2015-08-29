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
    public class SiteContentEntity:ObjectBase
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public Int64 ContentID { get; set; }

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
        /// 标题
        /// </summary>
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// 外链
        /// </summary>
        [DataMember]
        public String Linkurl { get; set; }

        /// <summary>
        /// 标题颜色
        /// </summary>
        [DataMember]
        public String TitleColor { get; set; }

        /// <summary>
        /// 是否粗体 0:否 1:是
        /// </summary>
        [DataMember]
        public bool IsBold { get; set; }

        /// <summary>
        /// 是否斜体 0:否 1:是
        /// </summary>
        [DataMember]
        public bool IsItalic { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [DataMember]
        public String Source { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DataMember]
        public String Author { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [DataMember]
        public String Tags { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [DataMember]
        public String Abstruct { get; set; }

        /// <summary>
        /// 标题图
        /// </summary>
        [DataMember]
        public String TitlePhoto { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public Int64 InAuthor { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public String InAuthorName { get; set; }

        /// <summary>
        /// 编辑人
        /// </summary>
        [DataMember]
        public Int64 EditAuthor { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime AddDate { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        [DataMember]
        public DateTime EditDate { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public String Content { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        [DataMember]
        public String FJPath { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        [DataMember]
        public Int32 SortID { get; set; }


        public SiteContentEntity()
        {
            ContentID = (long)0;
            JournalID = (long)0;
            ChannelID = (long)0;
            SortID = 0;
            Title = string.Empty;
            Linkurl = string.Empty;
            TitleColor = string.Empty;
            IsBold = false;
            IsItalic = false;
            Source = string.Empty;
            Author = string.Empty;
            Tags = string.Empty;
            Abstruct = string.Empty;
            TitlePhoto = string.Empty;
            FJPath = string.Empty;
            InAuthor = (long)0;
            EditAuthor = (long)0;
            EditDate = DateTime.Now;
            AddDate = DateTime.Now;
        }
    }
}
