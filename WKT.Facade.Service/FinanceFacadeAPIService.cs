using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;
using WKT.Common.Extension;
using WKT.Common.Utils;
using WKT.Facade.Service.Interface;
using WKT.Model.Enum;
using WKT.Log;
using WKT.Common.SMS;
using WKT.Common.Email;

namespace WKT.Facade.Service
{
    class FinanceFacadeAPIService : ServiceBase, IFinanceFacadeAPIService
    {
        #region 稿件费用相关
        /// <summary>
        /// 获取稿件费用分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceContributeEntity> GetFinanceContributePageList(FinanceContributeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<FinanceContributeEntity> pager = clientHelper.Post<Pager<FinanceContributeEntity>, FinanceContributeQuery>(GetAPIUrl(APIConstant.FINANCECONTRIBUTE_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取稿件费用数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<FinanceContributeEntity> GetFinanceContributeList(FinanceContributeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<FinanceContributeEntity> list = clientHelper.Post<IList<FinanceContributeEntity>, FinanceContributeQuery>(GetAPIUrl(APIConstant.FINANCECONTRIBUTE_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取稿件费用实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public FinanceContributeEntity GetFinanceContributeModel(FinanceContributeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            FinanceContributeEntity model = clientHelper.Post<FinanceContributeEntity, FinanceContributeQuery>(GetAPIUrl(APIConstant.FINANCECONTRIBUTE_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存稿件费用数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveFinanceContribute(FinanceContributeEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, FinanceContributeEntity>(GetAPIUrl(APIConstant.FINANCECONTRIBUTE_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除稿件费用
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelFinanceContribute(FinanceContributeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, FinanceContributeQuery>(GetAPIUrl(APIConstant.FINANCECONTRIBUTE_DEL), query);
            return result;
        }

        /// <summary>
        /// 获取财务入款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceAccountPageList(ContributionInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<FinanceAccountEntity> pager = clientHelper.PostAuth<Pager<FinanceAccountEntity>, ContributionInfoQuery>(GetAPIUrl(APIConstant.FINANCEACCOUNT_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取稿费统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceGaoFeePageList(ContributionInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<FinanceAccountEntity> pager = clientHelper.PostAuth<Pager<FinanceAccountEntity>, ContributionInfoQuery>(GetAPIUrl(APIConstant.FINANCEFAOFEE_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取财务出款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceOutAccountPageList(ContributionInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<FinanceAccountEntity> pager = clientHelper.PostAuth<Pager<FinanceAccountEntity>, ContributionInfoQuery>(GetAPIUrl(APIConstant.FINANCEOUTACCOUNT_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取财务统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceContributeEntity> GetFinanceGlancePageList(FinanceContributeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<FinanceContributeEntity> pager = clientHelper.PostAuth<Pager<FinanceContributeEntity>, FinanceContributeQuery>(GetAPIUrl(APIConstant.FINANCEGLANCE_GETPAGELIST), query);
            return pager;
        }


        /// <summary>
        /// 获取版面费报表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceContributeEntity> GetFinancePageFeeReportPageList(FinanceContributeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<FinanceContributeEntity> pager = clientHelper.PostAuth<Pager<FinanceContributeEntity>, FinanceContributeQuery>(GetAPIUrl(APIConstant.FINANCEGLANCE_GETPAGEFEEREPORTPAGELIST), query);
            return pager;
        }

        #endregion

        #region 缴费通知
        /// <summary>
        /// 获取缴费通知分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<PayNoticeEntity> GetPayNoticePageList(PayNoticeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<PayNoticeEntity> pager = clientHelper.Post<Pager<PayNoticeEntity>, PayNoticeQuery>(GetAPIUrl(APIConstant.PAYNOTICE_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取缴费通知数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<PayNoticeEntity> GetPayNoticeList(PayNoticeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<PayNoticeEntity> list = clientHelper.Post<IList<PayNoticeEntity>, PayNoticeQuery>(GetAPIUrl(APIConstant.PAYNOTICE_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取缴费通知实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public PayNoticeEntity GetPayNoticeModel(PayNoticeQuery query)
        {            
            PayNoticeEntity model = null;
            try
            {
                if (query.NoticeID > 0)
                {
                    HttpClientHelper clientHelper = new HttpClientHelper();
                    model = clientHelper.Post<PayNoticeEntity, PayNoticeQuery>(GetAPIUrl(APIConstant.PAYNOTICE_GETMODEL), query);
                }
                if (model == null)
                {
                    model = new PayNoticeEntity();
                    model.JournalID = query.JournalID;
                    model.PayType = query.PayType.Value;
                    model.CID = query.CID.Value;
                    SiteConfigFacadeAPIService service = new SiteConfigFacadeAPIService();
                    MessageTemplateEntity temp = null;
                    if (model.PayType == 1)
                        temp = service.GetMessageTemplate(model.JournalID, -3, 1);
                    else if (model.PayType == 2)
                        temp = service.GetMessageTemplate(model.JournalID, -4, 1);
                    if (temp != null)
                        model.Body = temp.TContent;
                    if (!string.IsNullOrWhiteSpace(model.Body))
                    {
                        AuthorPlatformFacadeAPIService authorService = new AuthorPlatformFacadeAPIService();
                        ContributionInfoQuery authorQuery = new ContributionInfoQuery();
                        authorQuery.JournalID = model.JournalID;
                        authorQuery.CID = model.CID;
                        authorQuery.IsAuxiliary = false;
                        var contribution = authorService.GetContributionInfoModel(authorQuery);
                        if (contribution != null)
                        {
                            IDictionary<string, string> dict = service.GetEmailVariable();
                            var user = new AuthorFacadeAPIService().GetAuthorInfo(new AuthorInfoQuery() { JournalID = model.JournalID, AuthorID = contribution.AuthorID });
                            if (!query.IsBatch)
                            {
                                dict["${接收人}$"] = user.RealName;
                                dict["${邮箱}$"] = user.LoginName;
                                dict["${手机}$"] = user.Mobile;
                                dict["${稿件编号}$"] = contribution.CNumber;
                                dict["${稿件标题}$"] = contribution.Title;
                                model.Body = service.GetEmailOrSmsContent(dict, service.GetSiteConfig(model.JournalID), model.Body);
                            }
                            else
                            {
                                query.AuthorName = user.RealName;
                                query.LoginName = user.LoginName;
                                query.Mobile = user.Mobile;
                                query.CNumber = contribution.CNumber;
                                query.Title = contribution.Title;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取通知单失败：" + ex.ToString());
                model = new PayNoticeEntity();
                model.JournalID = query.JournalID;
                model.PayType = query.PayType.Value;
                model.CID = query.CID.Value;
            }
            return model;
        }

        /// <summary>
        /// 保存缴费通知数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SavePayNotice(PayNoticeEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, PayNoticeEntity>(GetAPIUrl(APIConstant.PAYNOTICE_SAVE), model);
            if (result.result == EnumJsonResult.success.ToString())
            {
                SiteConfigFacadeAPIService service = new SiteConfigFacadeAPIService();
                MessageRecodeEntity logModel = new MessageRecodeEntity();
                logModel.MsgType = 1;
                logModel.JournalID = model.JournalID;
                logModel.SendUser = model.SendUser;
                logModel.MsgTitle = model.Title;
                logModel.MsgContent = model.Body;
                logModel.CID = model.CID;
                if (model.PayType == 1)
                    logModel.SendType = -3;
                else if (model.PayType == 2)
                    logModel.SendType = -4;
                IList<Int64> userList = new List<Int64>() { model.AuthorID };
                var emailResult = service.SendEmailOrSms(userList, logModel);
                result.msg += emailResult.msg;
                if (model.IsSms && !string.IsNullOrWhiteSpace(model.SmsContent))
                {
                    logModel.MsgType = 2;
                    logModel.MsgContent = model.SmsContent;
                    var smsResult = service.SendEmailOrSms(userList, logModel);
                    result.msg += smsResult.msg;
                }
            }
            return result;
        }

        /// <summary>
        /// 批量保存缴费通知数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult BatchSavePayNotice(IList<PayNoticeEntity> list)
        {
            ExecResult result = new ExecResult();
            if (list != null && list.Count > 0)
            {
                HttpClientHelper clientHelper = new HttpClientHelper();
                result = clientHelper.Post<ExecResult, IList<PayNoticeEntity>>(GetAPIUrl(APIConstant.PAYNOTICE_BATCHSAVE), list);
                if (result.result == EnumJsonResult.success.ToString())
                {
                    int index = 0;
                    string returnData = string.Empty;
                    foreach (var model in list)
                    {
                        SiteConfigFacadeAPIService service = new SiteConfigFacadeAPIService();
                        MessageRecodeEntity logModel = new MessageRecodeEntity();
                        logModel.MsgType = 1;
                        logModel.JournalID = model.JournalID;
                        logModel.SendUser = model.SendUser;
                        logModel.MsgTitle = model.Title;
                        logModel.MsgContent = model.Body;
                        logModel.CID = model.CID;
                        if (model.PayType == 1)
                            logModel.SendType = -3;
                        else if (model.PayType == 2)
                            logModel.SendType = -4;
                        IList<Int64> userList = new List<Int64>() { model.AuthorID };
                        var emailResult = service.SendEmailOrSms(userList, logModel);
                        index++;
                        returnData = emailResult.msg;
                        if (model.IsSms && !string.IsNullOrWhiteSpace(model.SmsContent))
                        {
                            logModel.MsgType = 2;
                            logModel.MsgContent = model.SmsContent;
                            var smsResult = service.SendEmailOrSms(userList, logModel);
                            result.msg += smsResult.msg;
                        }

                    }
                    result.msg += returnData + "共计通知 " + index + " 人";
                }
            }

            return result;
        }



        /// <summary>
        /// 删除缴费通知
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelPayNotice(PayNoticeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, PayNoticeQuery>(GetAPIUrl(APIConstant.PAYNOTICE_DEL), query);
            return result;
        }

        /// <summary>
        /// 更新缴费通知状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult ChangePayNoticeStatus(PayNoticeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, PayNoticeQuery>(GetAPIUrl(APIConstant.PAYNOTICE_CHANGESTATUS), query);
            return result;
        }
        #endregion

        #region 财务收支明细相关
        /// <summary>
        /// 新增支付记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult AddFinancePayDetail(FinancePayDetailEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, FinancePayDetailEntity>(GetAPIUrl(APIConstant.FINANCEPAYDETAIL_ADD), model);
            return result;
        }
        #endregion
    }
}
