using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model
{
    /// <summary>
    /// 执行结果
    /// </summary>
    [Serializable]
    public class ExecResult
    {
        /// <summary>
        /// 执行结果
        /// success、error、failure
        /// </summary>
        public string result
        {
            get;
            set;
        }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string msg
        {
            get;
            set;
        }

        /// <summary>
        /// 返回ID
        /// </summary>
        public Int64 resultID { get; set; }

        /// <summary>
        /// 返回其它内容(如稿件编号)
        /// </summary>
        public String resultStr { get; set; }
    }
}
