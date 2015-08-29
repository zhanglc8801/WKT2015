using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class ReviewBillContentBusiness : IReviewBillContentBusiness
    {
        /// <summary>
        /// 保存审稿单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool SaveReviewBillContent(IList<ReviewBillContentEntity> list)
        {
            return ReviewBillContentDataAccess.Instance.SaveReviewBillContent(list);
        }

        /// <summary>
        /// 添加审稿单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddReviewBillContent(ReviewBillContentEntity model)
        {
            return ReviewBillContentDataAccess.Instance.AddReviewBillContent(model);
        }

        /// <summary>
        /// 删除专家审稿单
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="AuthorID"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public bool DelReviewBillContent(Int64 JournalID, Int64 AuthorID, Int64 CID)
        {
            return ReviewBillContentDataAccess.Instance.DelReviewBillContent(JournalID, AuthorID, CID);
        }

        /// <summary>
        /// 获取审稿单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ReviewBillContentEntity> GetReviewBillContentList(ReviewBillContentQuery query)
        {
            return ReviewBillContentDataAccess.Instance.GetReviewBillContentList(query);
        }
    }
}
