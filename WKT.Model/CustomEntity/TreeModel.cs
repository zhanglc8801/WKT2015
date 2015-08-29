using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    [Serializable]
    [DataContract]
    public class TreeModel
    {
        /// <summary>
        /// menuid
        /// </summary>
        [DataMember]
        public string key
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string text
        {
            get;
            set;
        }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public Int64 Id
        {
            get;
            set;
        }

        /// <summary>
        /// 图标
        /// </summary>
        [DataMember]
        public string icon
        {
            get;
            set;
        }

        /// <summary>
        /// 链接地址
        /// </summary>
        [DataMember]
        public string url
        {
            get;
            set;
        }

        /// <summary>
        /// 是否展开
        /// </summary>
        [DataMember]
        public bool isexpand
        {
            get;
            set;
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        [DataMember]
        public bool ischecked
        {
            get;
            set;
        }

        [DataMember]
        public bool isLeaf
        {
            get;
            set;
        }

        private IList<TreeModel> _children = new List<TreeModel>();
        /// <summary>
        /// 子节点
        /// </summary>
        [DataMember]
        public IList<TreeModel> children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
