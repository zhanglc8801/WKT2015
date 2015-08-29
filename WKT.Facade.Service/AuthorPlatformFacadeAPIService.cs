using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;
using WKT.Common.Utils;
using WKT.Facade.Service.Interface;
using WKT.Model.Enum;
using WKT.Log;

namespace WKT.Facade.Service
{
    public class AuthorPlatformFacadeAPIService : ServiceBase, IAuthorPlatformFacadeService
    {
        #region 作者详细信息
        /// <summary>
        /// 获取作者详细信息分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<AuthorDetailEntity> GetAuthorDetailPageList(AuthorDetailQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<AuthorDetailEntity> pager = clientHelper.Post<Pager<AuthorDetailEntity>, AuthorDetailQuery>(GetAPIUrl(APIConstant.AUTHORDETAIL_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取作者详细信息数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorDetailEntity> GetAuthorDetailList(AuthorDetailQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<AuthorDetailEntity> list = clientHelper.Post<IList<AuthorDetailEntity>, AuthorDetailQuery>(GetAPIUrl(APIConstant.AUTHORDETAIL_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取作者详细信息实体
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        public AuthorDetailEntity GetAuthorDetailModel(AuthorDetailQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            AuthorDetailEntity model = clientHelper.PostAuth<AuthorDetailEntity, AuthorDetailQuery>(GetAPIUrl(APIConstant.AUTHORDETAIL_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存作者详细信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveAuthorDetail(AuthorDetailEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.PostAuth<ExecResult, AuthorDetailEntity>(GetAPIUrl(APIConstant.AUTHORDETAIL_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除作者详细信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelAuthorDetail(AuthorDetailQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, AuthorDetailQuery>(GetAPIUrl(APIConstant.AUTHORDETAIL_DEL), query);
            return result;
        }

        /// <summary>
        /// 设置作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult SetAuthorExpert(AuthorDetailQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, AuthorDetailQuery>(GetAPIUrl(APIConstant.AUTHOR_SETEXPERT), query);
            return result;
        }

        /// <summary>
        /// 取消作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult CancelAuthorExpert(AuthorDetailQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, AuthorDetailQuery>(GetAPIUrl(APIConstant.AUTHOR_CANCELEXPERT), query);
            return result;
        }

        /// <summary>
        /// 设置作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorDetailEntity> GetExpertGroupMapList(ExpertGroupMapEntity query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<AuthorDetailEntity> result = clientHelper.Post<IList<AuthorDetailEntity>, ExpertGroupMapEntity>(GetAPIUrl(APIConstant.AUTHOR_GETEXPERTGROUPMAPLIST), query);
            return result;
        }

        /// <summary>
        /// 取消作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult SaveExpertGroupMap(ExpertGroupMapQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, ExpertGroupMapQuery>(GetAPIUrl(APIConstant.AUTHOR_SAVEEXPERTGROUPMAP), query);
            return result;
        }
        #endregion

        #region 投稿相关
        /// <summary>
        /// 获取稿件分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionInfoEntity> GetContributionInfoPageList(ContributionInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<ContributionInfoEntity> pager = clientHelper.Post<Pager<ContributionInfoEntity>, ContributionInfoQuery>(GetAPIUrl(APIConstant.CONTRIBUTIONINFO_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取稿件数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionInfoEntity> GetContributionInfoList(ContributionInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<ContributionInfoEntity> list = clientHelper.Post<IList<ContributionInfoEntity>, ContributionInfoQuery>(GetAPIUrl(APIConstant.CONTRIBUTIONINFO_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取稿件实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ContributionInfoEntity GetContributionInfoModel(ContributionInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ContributionInfoEntity model = clientHelper.Post<ContributionInfoEntity, ContributionInfoQuery>(GetAPIUrl(APIConstant.CONTRIBUTIONINFO_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 投稿
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveContributionInfo(ContributionInfoEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, ContributionInfoEntity>(GetAPIUrl(APIConstant.CONTRIBUTIONINFO_SAVE), model);
            if (result.result.Equals(EnumJsonResult.success.ToString()) && model.CID == 0 && model.Status != -1)//新投稿，不是草稿
            {
                #region 投稿回执
                Action action = () =>
                    {
                        try
                        {
                            SiteConfigFacadeAPIService service = new SiteConfigFacadeAPIService();

                            MessageTemplateQuery queryTemp = new MessageTemplateQuery();
                            queryTemp.JournalID = model.JournalID;
                            queryTemp.TCategory = -5;//回执
                            var tempList = service.GetMessageTempList(queryTemp).ToList();
                            if (tempList == null)
                                return;
                            var EmailModel = tempList.Find(p => p.TType == 1);
                            var SmsModel = tempList.Find(p => p.TType == 2);
                            if (EmailModel == null && SmsModel == null)
                                return;

                            MessageRecodeEntity LogModel = new MessageRecodeEntity();
                            LogModel.JournalID = model.JournalID;
                            LogModel.SendType = -5;
                            LogModel.SendUser = model.AuthorID;

                            IDictionary<string, string> dict = service.GetEmailVariable();
                            var user = new AuthorFacadeAPIService().GetAuthorInfo(new AuthorInfoQuery() { JournalID = model.JournalID, AuthorID = model.AuthorID });
                            dict["${接收人}$"] = user.RealName;
                            dict["${邮箱}$"] = user.LoginName;
                            dict["${手机}$"] = user.Mobile;
                            dict["${稿件编号}$"] = result.resultStr;
                            dict["${稿件标题}$"] = model.Title;
                            dict["$稿件主键$"] = result.resultID.ToString();

                            ExecResult execResult = new ExecResult();
                            if (EmailModel != null)
                            {
                                LogModel.MsgType = 1;
                                execResult = service.SendEmailOrSms(new Dictionary<Int64, IDictionary<string, string>>() { { model.AuthorID, dict } }, LogModel);
                            }
                            if (SmsModel != null)
                            {
                                LogModel.MsgType = 2;
                                execResult = service.SendEmailOrSms(new Dictionary<Int64, IDictionary<string, string>>() { { model.AuthorID, dict } }, LogModel);
                            }

                            if (!execResult.result.Equals(EnumJsonResult.success.ToString()))
                                throw new Exception(execResult.msg);
                        }
                        catch (Exception ex)
                        {
                            LogProvider.Instance.Error("发送投稿回执失败,稿件编码【" + result.resultStr + "】：" + ex.ToString());
                        }
                    };
                action.BeginInvoke(null, null);
                #endregion
            }
            return result;
        }

        /// <summary>
        /// 保存稿件格式修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveContributionInfoFormat(ContributionInfoEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, ContributionInfoEntity>(GetAPIUrl(APIConstant.CONTRIBUTIONINFO_SAVEFORMAT), model);
            return result;
        }

        /// <summary>
        /// 删除稿件数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelContributionInfo(ContributionInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, ContributionInfoQuery>(GetAPIUrl(APIConstant.CONTRIBUTIONINFO_DEL), query);
            return result;
        }

        /// <summary>
        /// 改变稿件状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult ChangeContributionInfoStatus(ContributionInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, ContributionInfoQuery>(GetAPIUrl(APIConstant.CONTRIBUTIONINFO_CHANGESTATUS), query);
            return result;
        }
        #endregion

        #region 撤稿相关
        /// <summary>
        /// 撤稿
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult DraftContributionInfo(RetractionsBillsEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, RetractionsBillsEntity>(GetAPIUrl(APIConstant.DRAFT_DRAFT), model);
            return result;
        }

        /// <summary>
        /// 新增撤稿表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddRetractionsBills(RetractionsBillsEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            bool result = clientHelper.Post<bool, RetractionsBillsEntity>(GetAPIUrl(APIConstant.DRAFT_ADD), model);
            return result;
        }

        /// <summary>
        /// 编辑撤稿表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateRetractionsBills(RetractionsBillsEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            bool result = clientHelper.Post<bool, RetractionsBillsEntity>(GetAPIUrl(APIConstant.DRAFT_UPDATE), model);
            return result;
        }

        /// <summary>
        /// 获取撤稿信息实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public RetractionsBillsEntity GetRetractionsBillsModel(RetractionsBillsQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            RetractionsBillsEntity model = clientHelper.Post<RetractionsBillsEntity, RetractionsBillsQuery>(GetAPIUrl(APIConstant.DRAFT_GETMODEL), query);
            if (model != null)
            {
                if (model.Handler > 0)
                {
                    model.HandlerName = GetMemberName(model.Handler);
                }
            }
            return model;
        }
        #endregion

        #region 稿件备注相关
        /// <summary>
        /// 保存稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveCRemark(CRemarkEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, CRemarkEntity>(GetAPIUrl(APIConstant.CREMARK_SAVE), model);
            return result;
        }

        /// <summary>
        /// 获取稿件备注实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public CRemarkEntity GetCRemarkModel(CRemarkQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            CRemarkEntity model = clientHelper.Post<CRemarkEntity, CRemarkQuery>(GetAPIUrl(APIConstant.CREMARK_GETMODEL), query);
            return model;
        }
        #endregion

        #region 收稿量统计
        /// <summary>
        /// 按年月统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionAccountEntity> GetContributionAccountListByYear(ContributionAccountQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<ContributionAccountEntity> list = clientHelper.Post<IList<ContributionAccountEntity>, ContributionAccountQuery>(GetAPIUrl(APIConstant.CONTRIBUTIONACCOUNT_GETYEARLIST), query);
            return list;
        }

        /// <summary>
        /// 按基金级别统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public IList<ContributionAccountEntity> GetContributionAccountListByFund(ContributionAccountQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<ContributionAccountEntity> list = clientHelper.Post<IList<ContributionAccountEntity>, ContributionAccountQuery>(GetAPIUrl(APIConstant.CONTRIBUTIONACCOUNT_GETFUNDLIST), query);
            return list;
        }

        /// <summary>
        /// 按作者统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>       
        public Pager<ContributionAccountEntity> GetContributionAccountListByAuhor(ContributionAccountQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<ContributionAccountEntity> pager = clientHelper.Post<Pager<ContributionAccountEntity>, ContributionAccountQuery>(GetAPIUrl(APIConstant.CONTRIBUTIONACCOUNT_GETAUTHORLIST), query);
            return pager;
        }
        #endregion
    }
}
