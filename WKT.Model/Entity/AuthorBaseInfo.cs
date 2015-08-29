using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    /// <summary>
    /// 作者基本信息
    /// </summary>
    [DataContract]
    [Serializable]
    public class AuthorBaseInfo : ObjectBase
    {
        [DataMember]
        public long AuthorID
        {
            get;
            set;
        }

        [DataMember]
        public string LoginName
        {
            get;
            set;
        }

        [DataMember]
        public string RealName
        {
            get;
            set;
        }

        [DataMember]
        public string Pwd
        {
            get;
            set;
        }

        /// <summary>
        /// 该用户角色列表
        /// </summary>
        public List<long> ListRoleID
        {
            get;
            set;
        }
    }
}
