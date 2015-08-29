using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.BLL;
using WKT.Service.Interface;
using WKT.Model.Enum;

namespace WKT.Service
{
    public partial class ReviewBillService:IReviewBillService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IReviewBillBusiness reviewBillBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IReviewBillBusiness ReviewBillBusProvider
        {
            get
            {
                 if(reviewBillBusProvider == null)
                 {
                      reviewBillBusProvider = new ReviewBillBusiness();//ServiceBusContainer.Instance.Container.Resolve<IReviewBillBusiness>();
                 }
                 return reviewBillBusProvider;
            }
            set
            {
              reviewBillBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReviewBillService()
        {
        }

        /// <summary>
        /// 新增审稿单项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddReviewBill(ReviewBillEntity model)
        {
            return ReviewBillBusProvider.AddReviewBill(model);
        }

        /// <summary>
        /// 编辑审稿单项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateReviewBill(ReviewBillEntity model)
        {            
            return ReviewBillBusProvider.UpdateReviewBill(model);
        }

        /// <summary>
        /// 获取审稿单项实体
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public ReviewBillEntity GetReviewBill(Int64 itemID)
        {
            return ReviewBillBusProvider.GetReviewBill(itemID);
        }

        /// <summary>
        /// 审稿单项是否已经被使用
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public bool ReviewBillIsEnabled(Int64 JournalID, Int64 itemID)
        {
            return ReviewBillBusProvider.ReviewBillIsEnabled(JournalID, itemID);
        }

        /// <summary>
        /// 审稿单项是否存在下级
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public bool ReviewBillIsHavDown(Int64 JournalID, Int64 itemID)
        {
            return ReviewBillBusProvider.ReviewBillIsHavDown(JournalID, itemID);
        }

        /// <summary>
        /// 删除审稿单项
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public ExecResult DelReviewBill(Int64 JournalID, Int64 itemID)
        {
            ExecResult execResult = new ExecResult();
            if (ReviewBillIsHavDown(JournalID, itemID))
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "该审稿单项存在下级，不允许删除！";
                return execResult;
            }
            bool result = ReviewBillBusProvider.DelReviewBill(JournalID, itemID);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除审稿单项成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除审稿单项失败！";
            }
            return execResult;
        }

        /// <summary>
        /// 获取审稿单项分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ReviewBillEntity> GetReviewBillPageList(ReviewBillQuery query)
        {
            return ReviewBillBusProvider.GetReviewBillPageList(query);
        }

        /// <summary>
        /// 获取审稿单项数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ReviewBillEntity> GetReviewBillList(ReviewBillQuery query)
        {
            return ReviewBillBusProvider.GetReviewBillList(query);
        }

        /// <summary>
        /// 保存审稿单项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveReviewBill(ReviewBillEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            if (model.ItemID == 0)
                result = AddReviewBill(model);
            else
            {
                if (ReviewBillIsEnabled(model.JournalID, model.ItemID))
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "该审稿单项已经被使用，不允许编辑！";
                    return execResult;
                }
                result = UpdateReviewBill(model);
            }
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "保存审稿单项成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "保存审稿单项失败！";
            }
            return execResult;
        }
    }
}
