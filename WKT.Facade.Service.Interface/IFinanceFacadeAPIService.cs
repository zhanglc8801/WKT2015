using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;

namespace WKT.Facade.Service.Interface
{
    public interface IFinanceFacadeAPIService
    {

        #region 稿件费用相关
        /// <summary>
        /// 获取稿件费用分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<FinanceContributeEntity> GetFinanceContributePageList(FinanceContributeQuery query);

        /// <summary>
        /// 获取稿件费用数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<FinanceContributeEntity> GetFinanceContributeList(FinanceContributeQuery query);

        /// <summary>
        /// 获取稿件费用实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        FinanceContributeEntity GetFinanceContributeModel(FinanceContributeQuery query);

        /// <summary>
        /// 保存稿件费用数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveFinanceContribute(FinanceContributeEntity model);

        /// <summary>
        /// 删除稿件费用
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelFinanceContribute(FinanceContributeQuery query);

         /// <summary>
        /// 获取财务入款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<FinanceAccountEntity> GetFinanceAccountPageList(ContributionInfoQuery query);

        /// <summary>
        /// 获取稿费统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<FinanceAccountEntity> GetFinanceGaoFeePageList(ContributionInfoQuery query);

        /// <summary>
        /// 获取财务出款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<FinanceAccountEntity> GetFinanceOutAccountPageList(ContributionInfoQuery query);
        /// <summary>
        /// 获取财务统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<FinanceContributeEntity> GetFinanceGlancePageList(FinanceContributeQuery query);

        

        /// <summary>
        /// 获取版面费报表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<FinanceContributeEntity> GetFinancePageFeeReportPageList(FinanceContributeQuery query);

        #endregion

        #region 缴费通知
        /// <summary>
        /// 获取缴费通知分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<PayNoticeEntity> GetPayNoticePageList(PayNoticeQuery query);

        /// <summary>
        /// 获取缴费通知数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<PayNoticeEntity> GetPayNoticeList(PayNoticeQuery query);

        /// <summary>
        /// 获取缴费通知实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        PayNoticeEntity GetPayNoticeModel(PayNoticeQuery query);

        /// <summary>
        /// 保存缴费通知数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SavePayNotice(PayNoticeEntity model);

        /// <summary>
        /// 保存缴费通知数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult BatchSavePayNotice(IList<PayNoticeEntity> list);

        /// <summary>
        /// 删除缴费通知
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelPayNotice(PayNoticeQuery query);

        /// <summary>
        /// 更新缴费通知状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult ChangePayNoticeStatus(PayNoticeQuery query);
        #endregion

        #region 财务收支明细相关
        /// <summary>
        /// 新增支付记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult AddFinancePayDetail(FinancePayDetailEntity model);
        #endregion
    }
}
