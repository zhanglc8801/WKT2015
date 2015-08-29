using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface IReviewBillBusiness
    {
        /// <summary>
        /// 新增审稿单项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddReviewBill(ReviewBillEntity model);

        /// <summary>
        /// 编辑审稿单项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateReviewBill(ReviewBillEntity model);

        /// <summary>
        /// 获取审稿单项实体
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        ReviewBillEntity GetReviewBill(Int64 itemID);

        /// <summary>
        /// 审稿单项是否已经被使用
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        bool ReviewBillIsEnabled(Int64 JournalID, Int64 itemID);

        /// <summary>
        /// 审稿单项是否存在下级
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        bool ReviewBillIsHavDown(Int64 JournalID, Int64 itemID);

        /// <summary>
        /// 删除审稿单项
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        bool DelReviewBill(Int64 JournalID, Int64 itemID);

        /// <summary>
        /// 获取审稿单项分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<ReviewBillEntity> GetReviewBillPageList(ReviewBillQuery query);

        /// <summary>
        /// 获取审稿单项数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<ReviewBillEntity> GetReviewBillList(ReviewBillQuery query);
    }
}






