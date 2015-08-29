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
    /// 工作量统计
    /// </summary>
    [DataContract]
    [Serializable]
    public class WorkloadQuery : QueryBase
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public Int64 AuthorID
        {
            get;
            set;
        }

        [DataMember]
        public byte GroupID
        {
            get;
            set;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public Int32 RoleID
        {
            get;
            set;
        }

        [DataMember]
        public string OrderBy
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
        public bool isStatByGroup
        {
            get;
            set;
        }

        [DataMember]
        public DateTime? StartDate
        {
            get;
            set;
        }

        [DataMember]
        public DateTime? EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 新稿件统计
        /// </summary>
        [DataMember]
        public bool IsNewContributes
        {
            get;
            set;
        }

        /// <summary>
        /// 是否忽略掉审稿数为0的数据
        /// </summary>
        [DataMember]
        public bool isIgnoreNoWork
        {
            get;
            set;
        }
        

        /// <summary>
        /// 收稿量中的状态集合，通过逗号隔开，这几种状态下的文章才能被算为收稿量中
        /// </summary>
        [DataMember]
        public string StatusList
        {
            get;
            set;
        }
    }
}
