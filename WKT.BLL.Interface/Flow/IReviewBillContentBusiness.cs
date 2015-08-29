using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface IReviewBillContentBusiness
    {
        /// <summary>
        /// 保存审稿单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool SaveReviewBillContent(IList<ReviewBillContentEntity> list);

        /// <summary>
        /// 添加审稿单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddReviewBillContent(ReviewBillContentEntity model);

        /// <summary>
        /// 删除专家审稿单
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="AuthorID"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        bool DelReviewBillContent(Int64 JournalID, Int64 AuthorID, Int64 CID);

        /// <summary>
        /// 获取审稿单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<ReviewBillContentEntity> GetReviewBillContentList(ReviewBillContentQuery query);
    }
}






