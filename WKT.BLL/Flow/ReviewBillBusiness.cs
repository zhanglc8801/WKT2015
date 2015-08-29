using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class ReviewBillBusiness : IReviewBillBusiness
    {
        /// <summary>
        /// 新增审稿单项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddReviewBill(ReviewBillEntity model)
        {
            return ReviewBillDataAccess.Instance.AddReviewBill(model);
        }

        /// <summary>
        /// 编辑审稿单项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateReviewBill(ReviewBillEntity model)
        {
            return ReviewBillDataAccess.Instance.UpdateReviewBill(model);
        }

        /// <summary>
        /// 获取审稿单项实体
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public ReviewBillEntity GetReviewBill(Int64 itemID)
        {
            return ReviewBillDataAccess.Instance.GetReviewBill(itemID);
        }

        /// <summary>
        /// 审稿单项是否已经被使用
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public bool ReviewBillIsEnabled(Int64 JournalID, Int64 itemID)
        {
            return ReviewBillDataAccess.Instance.ReviewBillIsEnabled(JournalID, itemID);
        }

        /// <summary>
        /// 审稿单项是否存在下级
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public bool ReviewBillIsHavDown(Int64 JournalID, Int64 itemID)
        {
            return ReviewBillDataAccess.Instance.ReviewBillIsHavDown(JournalID, itemID);
        }

        /// <summary>
        /// 删除审稿单项
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public bool DelReviewBill(Int64 JournalID, Int64 itemID)
        {
            return ReviewBillDataAccess.Instance.DelReviewBill(JournalID, itemID);
        }

        /// <summary>
        /// 获取审稿单项分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ReviewBillEntity> GetReviewBillPageList(ReviewBillQuery query)
        {
            return ReviewBillDataAccess.Instance.GetReviewBillPageList(query);
        }

        /// <summary>
        /// 获取审稿单项数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ReviewBillEntity> GetReviewBillList(ReviewBillQuery query)
        {
            return ReviewBillDataAccess.Instance.GetReviewBillList(query);
        }
    }
}
