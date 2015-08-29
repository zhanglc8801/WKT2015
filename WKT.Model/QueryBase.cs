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
    /// 查询实体
    /// </summary>
    [Serializable]
    [DataContract]
    public class QueryBase
    {
        /// <summary>
        /// 杂志ID
        /// </summary>
        [DataMember]
        public long JournalID
        {
            get;
            set;
        }

        /// <summary>
        /// Unity Service Key
        /// </summary>
        [DataMember]
        public string ServiceKey
        {
            get;
            set;
        }

        /// <summary>
        /// 排序  如：Id desc
        /// </summary>
        [DataMember]
        public String OrderStr { get; set; }

        #region  分页

        private int pageSize = 10;
        /// <summary>
        /// 每页记录数
        /// </summary>
        [DataMember]
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (value <= 0)
                    pageSize = 10;
                else
                    pageSize = value;
            }
        }

        private int currentPage = 0;
        /// <summary>
        /// 当前页面（页号）
        /// </summary>
        [DataMember]
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                if (value < 0)
                    currentPage = 1;
                else
                    currentPage = value;
            }
        }

        /// <summary>
        /// 开始页码
        /// </summary>
        [DataMember]
        public Int32 StartIndex
        {
            get
            {
                return (CurrentPage - 1) * PageSize + 1;
            }
        }

        /// <summary>
        /// 结束页码
        /// </summary>
        [DataMember]
        public Int32 EndIndex
        {
            get
            {
                return CurrentPage * PageSize;
            }
        }
        #endregion
    }
}
