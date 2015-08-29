using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.BLL;
using WKT.Service.Interface;
using WKT.Model.Enum;
using WKT.Common.Extension;

namespace WKT.Service
{
    public partial class FinanceContributeService:IFinanceContributeService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IFinanceContributeBusiness financeContributeBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IFinanceContributeBusiness FinanceContributeBusProvider
        {
            get
            {
                 if(financeContributeBusProvider == null)
                 {
                      financeContributeBusProvider = new FinanceContributeBusiness();//ServiceBusContainer.Instance.Container.Resolve<IFinanceContributeBusiness>();
                 }
                 return financeContributeBusProvider;
            }
            set
            {
              financeContributeBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FinanceContributeService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public FinanceContributeEntity GetFinanceContribute(Int64 pKID)
        {
           return FinanceContributeBusProvider.GetFinanceContribute( pKID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<FinanceContributeEntity></returns>
        public List<FinanceContributeEntity> GetFinanceContributeList()
        {
            return FinanceContributeBusProvider.GetFinanceContributeList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="financeContributeQuery">FinanceContributeQuery查询实体对象</param>
        /// <returns>List<FinanceContributeEntity></returns>
        public List<FinanceContributeEntity> GetFinanceContributeList(FinanceContributeQuery financeContributeQuery)
        {
            return FinanceContributeBusProvider.GetFinanceContributeList(financeContributeQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<FinanceContributeEntity></returns>
        public Pager<FinanceContributeEntity> GetFinanceContributePageList(CommonQuery query)
        {
            return FinanceContributeBusProvider.GetFinanceContributePageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<FinanceContributeEntity></returns>
        public Pager<FinanceContributeEntity> GetFinanceContributePageList(QueryBase query)
        {
            return FinanceContributeBusProvider.GetFinanceContributePageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="financeContributeQuery">FinanceContributeQuery查询实体对象</param>
        /// <returns>Pager<FinanceContributeEntity></returns>
        public Pager<FinanceContributeEntity> GetFinanceContributePageList(FinanceContributeQuery financeContributeQuery)
        {
            return FinanceContributeBusProvider.GetFinanceContributePageList(financeContributeQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="financeContribute">FinanceContributeEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddFinanceContribute(FinanceContributeEntity financeContribute)
        {
            return FinanceContributeBusProvider.AddFinanceContribute(financeContribute);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="financeContribute">FinanceContributeEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateFinanceContribute(FinanceContributeEntity financeContribute)
        {
            return FinanceContributeBusProvider.UpdateFinanceContribute(financeContribute);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteFinanceContribute(Int64 pKID)
        {
            return FinanceContributeBusProvider.DeleteFinanceContribute( pKID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="financeContribute">FinanceContributeEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteFinanceContribute(FinanceContributeEntity financeContribute)
        {
            return FinanceContributeBusProvider.DeleteFinanceContribute(financeContribute);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteFinanceContribute(Int64[] pKID)
        {
            return FinanceContributeBusProvider.BatchDeleteFinanceContribute( pKID);
        }
        
        #endregion
        
        #endregion

        /// <summary>
        /// 保存稿件费用信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(FinanceContributeEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            model.RemitBillNo = model.RemitBillNo.TextFilter();
            model.InvoiceNo = model.InvoiceNo.TextFilter();
            model.PostNo = model.PostNo.TextFilter();
            model.Note = model.Note.TextFilter();
            if (model.IsSystem)
            {
                model.InComeDate = DateTime.Now;
                model.Status = 1;
            }
            if (model.PKID == 0)
            {
                FinanceContributeQuery fQuery = new FinanceContributeQuery();
                fQuery.JournalID = model.JournalID;
                fQuery.CID = model.CID;
                fQuery.FeeType = model.FeeType;
                if (FinanceContributeBusProvider.FinanceContributeIsExists(fQuery))
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "该费用已经存在！";
                    return execResult;
                }
                result = AddFinanceContribute(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增稿件费用成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增稿件费用失败！";
                }                
            }
            else
            {
                result = UpdateFinanceContribute(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "修改稿件费用成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "修改稿件费用失败！";
                }
            }
            if (result)
            {
                //更新缴费通知记录
                if (model.NoticeID > 0)
                {
                    PayNoticeQuery query = new PayNoticeQuery();
                    query.NoticeID = model.NoticeID;
                    query.Status = (Byte)(model.IsSystem ? 1 : 2);
                    PayNoticeBusiness business = new PayNoticeBusiness();
                    business.ChangeStatus(query);
                }
            }
            return execResult;
        }

        /// <summary>
        /// 删除稿件费用
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ExecResult Del(Int64[] PKID)
        {
            ExecResult result = new ExecResult();
            if (PKID == null)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = "没有删除任何数据！";
                return result;
            }
            bool flag = BatchDeleteFinanceContribute(PKID);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "删除稿件费用信息成功！";
            }
            else
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "删除稿件费用信息失败！";
            }
            return result;
        }

        /// <summary>
        /// 获取财务入款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceAccountPageList(ContributionInfoQuery query)
        {
            Pager<FinanceAccountEntity> pager = FinanceContributeBusProvider.GetFinanceAccountPageList(query);
            if (pager == null) return pager;
            IList<FinanceAccountEntity> list = pager.ItemList;
            if (list == null || list.Count == 0)
                return pager;
            
            //AuthorInfoService service = new AuthorInfoService();
            //AuthorInfoQuery aQuery = new AuthorInfoQuery();
            //aQuery.JournalID = query.JournalID;
            //var dict = service.AuthorInfoBusProvider.GetAuthorDict(aQuery);
            //foreach (var mode in list)
            //{
            //    mode.EditAuthorName = dict.GetValue(mode.EditAuthorID, mode.EditAuthorID.ToString());
            //    mode.FirstAuthor = dict.GetValue(mode.FirstAuthorID, mode.FirstAuthorID.ToString());
            //}
            ContributionInfoService service = new ContributionInfoService();
            ContributionInfoQuery cQuery = new ContributionInfoQuery();
            cQuery.JournalID = query.JournalID;
            var dict = service.ContributionInfoBusProvider.GetContributionAuthorDict(cQuery);
            //foreach (var mode in list)
            //{
            //    //mode.EditAuthorName = dict.GetValue(mode.EditAuthorID, mode.EditAuthorID.ToString());
            //    mode.FirstAuthor = dict.GetValue(mode.FirstAuthorID, mode.FirstAuthorID.ToString());
                
            //}
            
            pager.ItemList = list;
            return pager;
        }


        /// <summary>
        /// 获取财务出款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceOutAccountPageList(ContributionInfoQuery query)
        {
            Pager<FinanceAccountEntity> pager = FinanceContributeBusProvider.GetFinanceOutAccountPageList(query);
            if (pager == null) return pager;
            IList<FinanceAccountEntity> list = pager.ItemList;
            if (list == null || list.Count == 0)
                return pager;
            AuthorInfoService service = new AuthorInfoService();
            AuthorInfoQuery aQuery = new AuthorInfoQuery();
            aQuery.JournalID = query.JournalID;
            pager.ItemList = list;
            return pager;
        }

        /// <summary>
        /// 获取稿费统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceGaoFeePageList(ContributionInfoQuery query)
        {
            Pager<FinanceAccountEntity> pager = FinanceContributeBusProvider.GetFinanceGaoFeePageList(query);
            if (pager == null) return pager;
            IList<FinanceAccountEntity> list = pager.ItemList;
            if (list == null || list.Count == 0)
                return pager;
            ContributionInfoService service = new ContributionInfoService();
            ContributionInfoQuery cQuery = new ContributionInfoQuery();
            cQuery.JournalID = query.JournalID;
            var dict = service.ContributionInfoBusProvider.GetContributionAuthorDict(cQuery);
            pager.ItemList = list;
            return pager;
        }


        /// <summary>
        /// 获取财务统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceContributeEntity> GetFinanceGlancePageList(FinanceContributeQuery query)
        {
            Pager<FinanceContributeEntity> pager = FinanceContributeBusProvider.GetFinanceGlancePageList(query);
            if (pager == null) return pager;
            IList<FinanceContributeEntity> list = pager.ItemList;
            if (list == null || list.Count == 0)
                return pager;
            AuthorInfoService service = new AuthorInfoService();
            AuthorInfoQuery aQuery = new AuthorInfoQuery();
            aQuery.JournalID = query.JournalID;
            var dict = service.AuthorInfoBusProvider.GetAuthorDict(aQuery);
            foreach (var mode in list)
            {
                mode.InUserName = dict.GetValue(mode.InUser, mode.InUser.ToString());
            }
            pager.ItemList = list;
            return pager;
        }

       

        /// <summary>
        /// 获取版面费报表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceContributeEntity> GetFinancePageFeeReportPageList(FinanceContributeQuery query)
        {
            Pager<FinanceContributeEntity> pager = FinanceContributeBusProvider.GetFinancePageFeeReportPageList(query);
            if (pager == null) return pager;
            IList<FinanceContributeEntity> list = pager.ItemList;
            if (list == null || list.Count == 0)
                return pager;
            AuthorInfoService service = new AuthorInfoService();
            AuthorInfoQuery aQuery = new AuthorInfoQuery();
            aQuery.JournalID = query.JournalID;
            var dict = service.AuthorInfoBusProvider.GetAuthorDict(aQuery);
            foreach (var mode in list)
            {
                mode.InUserName = dict.GetValue(mode.InUser, mode.InUser.ToString());
            }
            pager.ItemList = list;
            return pager;
        }


    }
}
