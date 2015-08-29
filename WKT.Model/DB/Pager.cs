using System;
using System.Collections.Generic;

namespace WKT.Model
{
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Pager<T>
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalRecords
        {
            get;
            set;
        }

        private int pageSize = 0;
        /// <summary>
        /// 每页条目数
        /// </summary>
        public int PageSize
        {
            get
            {
                if (pageSize <= 0)
                    pageSize =20;
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

        private int currentPage =1;
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
        /// 返回合计金额
        /// </summary>
        public Decimal Money { get; set; }

        /// <summary>
        /// 返回版面费合计金额
        /// </summary>
        public Decimal PageMoney { get; set; }

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
