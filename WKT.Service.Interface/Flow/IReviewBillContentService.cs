using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface IReviewBillContentService
    {
        /// <summary>
        /// 保存审稿单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult SaveReviewBillContent(ReviewBillContentQuery query);

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

        /// <summary>
        /// 根据稿件编号获取审稿单信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<ReviewBillContentEntity> GetReviewBillContentListByCID(ReviewBillContentQuery query);
    }
}






