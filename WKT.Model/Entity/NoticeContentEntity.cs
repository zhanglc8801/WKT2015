using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model
{
    public class NoticeContentEntity
    {
        /// <summary>
        /// 字典ID
        /// </summary>
        public long  DicID
        {
            get;
            set;
        }

        /// <summary>
        /// 关联的字典
        /// </summary>
        public string DicKey
        {
            get;
            set;
        }

        /// <summary>
        /// 帮助文档内容
        /// </summary>
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }

        /// <summary>
        /// 跟新时间
        /// </summary>
        public DateTime UpdateTime
        {
            get;
            set;
        }


        public NoticeContentEntity()
        {
            DicID = 0;
            Content = string.Empty;
            AddTime = UpdateTime = System.DateTime.Now;
        }
    }
}
