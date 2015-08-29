using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model
{
    public class NoticeContentQuery
    {
        /// <summary>
        /// 字典ID
        /// </summary>
        public int DicID
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
        /// 格式化帮助文档内容
        /// </summary>
        public string FormateContent
        {
            get
            {
                return null;
            }
        }
    }
}
