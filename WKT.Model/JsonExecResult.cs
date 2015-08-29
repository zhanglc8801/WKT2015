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
    public class JsonExecResult<T>
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
        /// 返回对象
        /// </summary>
        public T ReturnObject
        {
            get;
            set;
        }

        private IList<T> _itemList = new List<T>();
        /// <summary>
        /// 数据list
        /// </summary>
        public IList<T> ItemList
        {
            get
            {
                return _itemList;
            }
            set
            {
                _itemList = value;
            }
        }
    }
}
