using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model
{
    /// <summary>
    /// 分页实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PagerInfo<T>
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalRecords
        {
            get;
            set;
        }

        private int pageSize = 20;
        /// <summary>
        /// 每页条目数
        /// </summary>
        public int PageSize
        {
            get
            {
                if (pageSize <= 0)
                    pageSize = 20;
                return pageSize;
            }
            set
            {
                pageSize = value;
            }
        }


        private int totalPage = 0;
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (totalPage == 0)
                {
                    totalPage = (int)TotalRecords / PageSize;
                    if ((TotalRecords % PageSize) > 0)
                        totalPage++;
                }
                return totalPage;
            }
            set
            {
                totalPage = 0;
            }
        }

        private int currentPage = 0;
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage
        {
            get
            {

                return currentPage;
            }
            set
            {

                currentPage = value;
            }
        }

        /// <summary>
        /// 数据list
        /// </summary>
        public List<T> ItemList
        {
            get;
            set;
        }
    }
}
