using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    [Serializable]
    [DataContract]
    public class FieldsSet
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        [DataMember]
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        [DataMember]
        public string FieldName
        {
            get;
            set;
        }

        /// <summary>
        /// 字段
        /// </summary>
        [DataMember]
        public string DBField
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        [DataMember]
        public bool IsShow
        {
            get;
            set;
        }

        /// <summary>
        /// 是否必填
        /// </summary>
        [DataMember]
        public bool IsRequire
        {
            get;
            set;
        }
    }
}
