using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.BLL;
using WKT.Service.Interface;
using WKT.Model.Enum;

namespace WKT.Service
{
    public partial class ReviewBillContentService:IReviewBillContentService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IReviewBillContentBusiness reviewBillContentBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IReviewBillContentBusiness ReviewBillContentBusProvider
        {
            get
            {
                 if(reviewBillContentBusProvider == null)
                 {
                      reviewBillContentBusProvider = new ReviewBillContentBusiness();//ServiceBusContainer.Instance.Container.Resolve<IReviewBillContentBusiness>();
                 }
                 return reviewBillContentBusProvider;
            }
            set
            {
              reviewBillContentBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReviewBillContentService()
        {
        }

        /// <summary>
        /// 保存审稿单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public ExecResult SaveReviewBillContent(ReviewBillContentQuery query)
        {
            ExecResult execResult = new ExecResult();
            List<ReviewBillContentEntity> list = new List<ReviewBillContentEntity>();
             bool result = true;
             if (query.list!=null && query.list.Count > 0)
             {
                 list = query.list.ToList();
                 if (list == null || list.Count == 0)
                 {
                     execResult.result = EnumJsonResult.failure.ToString();
                     execResult.msg = "没有需要保存的数据！";
                 }
                 foreach (var item in list)
                 {
                     item.JournalID = query.JournalID;
                     item.AddUser = query.AddUser.Value;
                 }

                 ReviewBillService service = new ReviewBillService();
                 var billList = service.GetReviewBillList(new ReviewBillQuery() { JournalID = query.JournalID, PItemID = 0 });
                 ReviewBillContentEntity model = null;
                 foreach (var item in billList)
                 {
                     if (list.Find(p => p.ItemID == item.ItemID) != null)
                         continue;
                     model = new ReviewBillContentEntity();
                     model.ItemContentID = 0;
                     model.JournalID = query.JournalID;
                     model.AddUser = query.AddUser.Value;
                     model.CID = list[0].CID;
                     model.ItemID = item.ItemID;
                     model.ContentValue = string.Empty;
                     model.IsChecked = false;
                     list.Add(model);
                 }
                 result = ReviewBillContentBusProvider.SaveReviewBillContent(list);
             }
            if (result)
            {
                FlowCirculationBusiness business = new FlowCirculationBusiness();
                CirculationEntity item = new CirculationEntity();
                item.CID = query.CID == null ? 0 : query.CID.Value;
                item.AuthorID = query.AddUser.Value;
                item.JournalID = query.JournalID;
                if (query.IsEnExpert == true)
                {
                    if (query.IsReReview == true)
                        item.EnumCStatus = EnumContributionStatus.ReAuditedEn;
                    else
                        item.EnumCStatus = EnumContributionStatus.AuditedEn;
                }
                else
                {
                    if (query.IsReReview == true)
                        item.EnumCStatus = EnumContributionStatus.ReAudited;
                    else
                        item.EnumCStatus = EnumContributionStatus.Audited;
                }
                
                item.DealAdvice = WKT.Common.Security.SecurityUtils.SafeSqlString(query.DealAdvice);
                item.CPath = query.PathUrl;
                item.CFileName = query.CFileName;
                item.OtherPath = query.OtherPath;
                item.FigurePath = query.figurePath;
                item.FFileName = query.FFileName;
                item.IsHaveBill = 1;

                //专家审回到固定编辑设置
                DictValueBusiness siteConfigBusiness = new DictValueBusiness();
                DictValueQuery DictQuery = new DictValueQuery();
                IDictionary<int, string> dict = siteConfigBusiness.GetDictValueDcit(query.JournalID, "ExpertToEditor");
                if (dict != null && dict.Count > 0)
                {
                    foreach (var dictItem in dict)
                    {
                        item.IsExpertToEditor = true;
                        item.RecUserID = Convert.ToInt64(dictItem.Value);
                    }

                }

                if (business.AuthorContribution(item))
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "保存审稿单成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "保存审核信息失败！"; 
                }
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "保存审稿单失败！";
            }
            return execResult;
        }

        /// <summary>
        /// 添加审稿单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddReviewBillContent(ReviewBillContentEntity model)
        {
            return ReviewBillContentBusProvider.AddReviewBillContent(model);
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
            return ReviewBillContentBusProvider.DelReviewBillContent(JournalID, AuthorID, CID);
        }

        /// <summary>
        /// 获取审稿单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ReviewBillContentEntity> GetReviewBillContentList(ReviewBillContentQuery query)
        {
            return ReviewBillContentBusProvider.GetReviewBillContentList(query);
        }

        /// <summary>
        /// 根据稿件编号获取审稿单信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ReviewBillContentEntity> GetReviewBillContentListByCID(ReviewBillContentQuery query)
        {
            var list = GetReviewBillContentList(query);
            if (list == null || list.Count == 0)
            {
                ReviewBillService service = new ReviewBillService();
                var billList = service.GetReviewBillList(new ReviewBillQuery() { JournalID = query.JournalID });
                ReviewBillContentEntity model = null;
                foreach (var item in billList)
                {
                    model = new ReviewBillContentEntity();
                    model.ItemContentID = 0;
                    model.JournalID = query.JournalID;
                    model.CID = query.CID.Value;
                    model.ItemID = item.ItemID;
                    model.ContentValue = string.Empty;
                    model.IsChecked = false;
                    model.AddUser = query.AddUser.Value;
                    model.AddDate = item.AddDate;
                    model.Title = item.Title;
                    model.ItemType = item.ItemType;
                    model.PItemID = item.PItemID;
                    model.SortID = item.SortID;
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
