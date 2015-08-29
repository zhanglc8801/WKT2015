using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;
using WKT.Model.Enum;
using WKT.Common.Extension;

namespace WKT.Service
{
    public class IssueService:IIssueService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IIssueBusiness issueBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IIssueBusiness IssueBusProvider
        {
            get
            {
                 if(issueBusProvider == null)
                 {
                     issueBusProvider = new IssueBusiness();//ServiceBusContainer.Instance.Container.Resolve<IContactWayBusiness>();
                 }
                 return issueBusProvider;
            }
            set
            {
              issueBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public IssueService()
        {
        }

        #region 年卷设置
        /// <summary>
        /// 新增年卷
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        public bool AddYearVolume(YearVolumeEntity yearVolumeEntity)
        {
            return IssueBusProvider.AddYearVolume(yearVolumeEntity);
        }

        /// <summary>
        /// 编辑年卷
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        public bool UpdateYearVolume(YearVolumeEntity yearVolumeEntity)
        {
            return IssueBusProvider.UpdateYearVolume(yearVolumeEntity);
        }

        /// <summary>
        /// 年是否存在
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        public bool YearVolumeYearIsExists(YearVolumeEntity yearVolumeEntity)
        {
            return IssueBusProvider.YearVolumeYearIsExists(yearVolumeEntity);
        }

        /// <summary>
        /// 卷是否存在
        /// </summary>
        /// <param name="yearVolumeEntity"></param>
        /// <returns></returns>
        public bool YearVolumeVolumeIsExists(YearVolumeEntity yearVolumeEntity)
        {
            return IssueBusProvider.YearVolumeVolumeIsExists(yearVolumeEntity);
        }

        /// <summary>
        /// 获取最新的年卷
        /// </summary>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        public YearVolumeEntity GetMaxYearVolume(Int64 JournalID)
        {
            return IssueBusProvider.GetMaxYearVolume(JournalID);
        }

        /// <summary>
        /// 获取年卷试实体
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        public YearVolumeEntity GetYearVolume(Int64 setID)
        {
            return IssueBusProvider.GetYearVolume(setID);
        }

        /// <summary>
        /// 删除年卷
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        public ExecResult DelYearVolume(Int64[] setID)
        {
            ExecResult execResult = new ExecResult();
            if (setID == null)
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有删除任何数据！";
                return execResult;
            }          
            string msg = string.Empty;
            IList<Int64> list = IssueBusProvider.DelYearVolume(setID);
            if (list == null || list.Count < setID.Length)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除年卷设置成功！";
                if (list != null && list.Count > 0)
                    execResult.msg += string.Format("部分编号[{0}]由于存在期刊信息，请先删除", string.Join(",", list));
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除年卷设置失败！";
            }
            return execResult;

        }

        /// <summary>
        /// 获取年卷分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<YearVolumeEntity> GetYearVolumePageList(YearVolumeQuery query)
        {
            return IssueBusProvider.GetYearVolumePageList(query);
        }

        /// <summary>
        /// 获取年卷数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<YearVolumeEntity> GetYearVolumeList(YearVolumeQuery query)
        {
            return IssueBusProvider.GetYearVolumeList(query);
        }

        /// <summary>
        /// 保存年卷设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveYearVolume(YearVolumeEntity model)
        {
            ExecResult execResult = new ExecResult();
            if (YearVolumeYearIsExists(model))
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "该年信息已经存在！";
                return execResult;
            }
            if (YearVolumeVolumeIsExists(model))
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "该卷信息已经存在！";
                return execResult;
            }
            bool result = false;
            if (model.SetID == 0)
                result = AddYearVolume(model);
            else
                result = UpdateYearVolume(model);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "保存年卷设置成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "保存年卷设置失败！";
            }
            return execResult;
        }
        #endregion

        #region 期数设置
        /// <summary>
        /// 新增期设置
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        public bool AddIssueSet(IssueSetEntity issueSetEntity)
        {
            return IssueBusProvider.AddIssueSet(issueSetEntity);
        }

        /// <summary>
        /// 编辑期设置
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        public bool UpdateIssueSet(IssueSetEntity issueSetEntity)
        {
            return IssueBusProvider.UpdateIssueSet(issueSetEntity);
        }

        /// <summary>
        /// 期设置是否存在
        /// </summary>
        /// <param name="issueSetEntity"></param>
        /// <returns></returns>
        public bool IssueSetIsExists(IssueSetEntity issueSetEntity)
        {
            return IssueBusProvider.IssueSetIsExists(issueSetEntity);
        }

        /// <summary>
        /// 获取最新的期设置
        /// </summary>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        public IssueSetEntity GetMaxIssueSet(Int64 JournalID)
        {
            return IssueBusProvider.GetMaxIssueSet(JournalID);
        }

        public IssueSetEntity GetJournalOfCostByYearAndIssue(int year, int issue)
        {
            return IssueBusProvider.GetJournalOfCostByYearAndIssue(year, issue);
        }

        /// <summary>
        /// 获取期设置
        /// </summary>
        /// <param name="IssueSetID"></param>
        /// <returns></returns>
        public IssueSetEntity GetIssueSet(Int64 IssueSetID)
        {
            return IssueBusProvider.GetIssueSet(IssueSetID);
        }

        /// <summary>
        /// 删除期设置
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        public ExecResult DelIssueSet(Int64[] IssueSetID)
        {
            ExecResult execResult = new ExecResult();
            if (IssueSetID == null)
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有删除任何数据！";
                return execResult;
            }
            string msg = string.Empty;
            IList<Int64> list = IssueBusProvider.DelIssueSet(IssueSetID);
            if (list == null || list.Count < IssueSetID.Length)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除期设置成功！";
                if (list != null && list.Count > 0)
                    execResult.msg += string.Format("部分编号[{0}]由于存在期刊信息，请先删除", string.Join(",", list));
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除期设置失败！";
            }
            return execResult;            
        }

        /// <summary>
        /// 获取期设置分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueSetEntity> GetIssueSetPageList(IssueSetQuery query)
        {
            return IssueBusProvider.GetIssueSetPageList(query);
        }

        /// <summary>
        /// 获取期设置数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<IssueSetEntity> GetIssueSetList(IssueSetQuery query)
        {
            return IssueBusProvider.GetIssueSetList(query);
        }

        /// <summary>
        /// 保存期设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveIssueSet(IssueSetEntity model)
        {
            ExecResult execResult = new ExecResult();
            if (IssueSetIsExists(model))
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "该期设置已经存在！";
                return execResult;
            }           
            bool result = false;
            if (model.IssueSetID == 0)
                result = AddIssueSet(model);
            else
                result = UpdateIssueSet(model);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "保存期设置成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "保存期设置失败！";
            }
            return execResult;
        }
        #endregion

        #region 期刊栏目
        /// <summary>
        /// 新增期刊栏目
        /// </summary>
        /// <param name="journalChannelEntity"></param>
        /// <returns></returns>
        public bool AddJournalChannel(JournalChannelEntity journalChannelEntity)
        {
            return IssueBusProvider.AddJournalChannel(journalChannelEntity);
        }

        /// <summary>
        /// 编辑期刊栏目
        /// </summary>
        /// <param name="journalChannelEntity"></param>
        /// <returns></returns>
        public bool UpdateJournalChannel(JournalChannelEntity journalChannelEntity)
        {
            return IssueBusProvider.UpdateJournalChannel(journalChannelEntity);
        }

        /// <summary>
        /// 获取期刊栏目
        /// </summary>
        /// <param name="jChannelID"></param>
        /// <returns></returns>
        public JournalChannelEntity GetJournalChannel(Int64 jChannelID)
        {
            return IssueBusProvider.GetJournalChannel(jChannelID);
        }

        /// <summary>
        /// 删除期刊栏目设置
        /// </summary>
        /// <param name="setID"></param>
        /// <returns></returns>
        public ExecResult DelJournalChannel(JournalChannelQuery query)
        {
            ExecResult execResult = new ExecResult();
            if (DelJournalChannelJudge(query.JChannelID, query.JournalID, 0))
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "该栏目存在下级，不允许删除！";
                return execResult;
            }
            if (DelJournalChannelJudge(query.JChannelID, query.JournalID, 1))
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "该栏目存在期刊信息，不允许删除！";
                return execResult;
            }
            if (DelJournalChannelJudge(query.JChannelID, query.JournalID, 2))
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "该栏目存在稿件信息，不允许删除！";
                return execResult;
            }
            bool result = IssueBusProvider.DelJournalChannel(query.JChannelID);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除期刊栏目成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除期刊栏目失败！";
            }
            return execResult;            
        }

        /// <summary>
        /// 期刊栏目前需要做的判断
        /// </summary>
        /// <param name="jChannelID"></param>
        /// <param name="JournalID"></param>
        /// <param name="type">0:是否存在下级 1:是否存在期刊表中 2:是否存在稿件表中</param>
        /// <returns>true:不能删除</returns>
        public bool DelJournalChannelJudge(Int64 jChannelID, Int64 JournalID, Int32 type)
        {
            return IssueBusProvider.DelJournalChannelJudge(jChannelID, JournalID, type);
        }

        /// <summary>
        /// 获取期刊栏目分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<JournalChannelEntity> GetJournalChannelPageList(JournalChannelQuery query)
        {
            return IssueBusProvider.GetJournalChannelPageList(query);
        }

        /// <summary>
        /// 获取期刊栏目数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<JournalChannelEntity> GetJournalChannelList(JournalChannelQuery query)
        {
            return IssueBusProvider.GetJournalChannelList(query);
        }

        /// <summary>
        /// 根据期刊数据 按照期刊栏目数据分组 获取当前期刊数据所属的期刊栏目数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public   IList<JournalChannelEntity> GetJournalChannelListByIssueContent(JournalChannelQuery query)
        {
            return IssueBusProvider.GetJournalChannelListByIssueContent(query);
        }

        /// <summary>
        /// 保存期刊栏目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveJournalChannel(JournalChannelEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            model.ChannelName = model.ChannelName.TextFilter();
            if (model.JChannelID == 0)
                result = AddJournalChannel(model);
            else
                result = UpdateJournalChannel(model);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "保存期刊栏目成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "保存期刊栏目失败！";
            }
            return execResult;
        }
        #endregion

        #region 期刊内容

        /// <summary>
        /// 设置录用稿件的年期
        /// </summary>
        /// <returns></returns>
        public ExecResult SetContributionYearIssue(IssueContentQuery cEntity)
        {
            ExecResult execResult = new ExecResult();
            bool result = IssueBusProvider.SetContributionYearIssue(cEntity);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "设置录用稿件的年期成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "设置录用稿件的年期失败！";
            }
            return execResult;  
        }

        /// <summary>
        /// 保存期刊信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveIssueContent(IssueContentEntity model)
        {
            ExecResult execResult = new ExecResult();
            model.Title = model.Title.TextFilter();
            model.EnTitle = model.EnTitle.TextFilter();
            model.Authors = model.Authors.TextFilter();
            model.EnAuthors = model.EnAuthors.TextFilter();
            model.WorkUnit = model.WorkUnit.TextFilter();
            model.EnWorkUnit = model.EnWorkUnit.TextFilter();
            model.Keywords = model.Keywords.TextFilter();
            model.EnKeywords = model.EnKeywords.TextFilter();
            model.CLC = model.CLC.TextFilter();
            model.DOI = model.DOI.TextFilter();
            model.Abstract = model.Abstract.HtmlFilter();
            model.EnAbstract = model.EnAbstract.HtmlFilter();
            model.Reference = model.Reference.HtmlFilter();
            model.Funds = model.Funds.HtmlFilter();
            model.AuthorIntro = model.AuthorIntro.HtmlFilter();
            model.StartPageNum = model.StartPageNum;
            model.EndPageNum = model.EndPageNum;
            model.FileKey = model.FileKey.TextFilter();            
            model.ContentID = IssueBusProvider.SaveIssueContent(model);
            if (model.ContentID>0)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "保存期刊信息成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "保存期刊信息失败！";
            }
            execResult.resultID = model.ContentID;
            return execResult;            
        }

        /// <summary>
        /// 新增期刊表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddIssueContent(IssueContentEntity model)
        {
            return IssueBusProvider.AddIssueContent(model);
        }

        /// <summary>
        /// 编辑期刊表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContent(IssueContentEntity model)
        {
            return IssueBusProvider.UpdateIssueContent(model);
        }

        /// <summary>
        /// 获取期刊实体
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        public IssueContentEntity GetIssueContent(IssueContentQuery query)
        {
            IssueContentEntity model = IssueBusProvider.GetIssueContent(query);
            if (query.IsAuxiliary)
            {
                model.ReferenceList = GetIssueReferenceList(new IssueReferenceQuery() { JournalID = query.JournalID, ContentID = query.contentID });
            }
            return model;
        }

        /// <summary>
        /// 删除期刊信息
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ExecResult DelIssueContent(Int64[] contentID)
        {
            ExecResult execResult = new ExecResult();
            bool result = IssueBusProvider.DelIssueContent(contentID);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除期刊信息成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除期刊信息失败！";
            }
            return execResult;            
        }

        /// <summary>
        /// 获取期刊分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueContentEntity> GetIssueContentPageList(IssueContentQuery query)
        {
            return IssueBusProvider.GetIssueContentPageList(query);
        }

        /// <summary>
        /// 获取期刊数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<IssueContentEntity> GetIssueContentList(IssueContentQuery query)
        {
            return IssueBusProvider.GetIssueContentList(query);
        }

        /// <summary>
        /// 更新期刊内容浏览次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContentHits(IssueContentQuery model)
        {
            return IssueBusProvider.UpdateIssueContentHits(model);
        }

        /// <summary>
        /// 更新期刊内容浏览次数(RichHTML)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContentHtmlHits(IssueContentQuery model)
        {
            return IssueBusProvider.UpdateIssueContentHtmlHits(model);
        }

	    /// <summary>
        /// 更新期刊内容下载次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIssueContentDownloads(IssueContentQuery model)
        {
            return IssueBusProvider.UpdateIssueContentDownloads(model);
        }

        #endregion

        #region DOI注册
        public bool AddDoiRegLog(DoiRegLogEntity model)
        {
            return IssueBusProvider.AddDoiRegLog(model);
        }
        public bool UpdateDoiRegLog(DoiRegLogEntity model)
        {
            return IssueBusProvider.UpdateDoiRegLog(model);
        }
        public bool DelDoiRegLog(long[] PKID)
        {
            return IssueBusProvider.DelDoiRegLog(PKID);
        }

        public DoiRegLogEntity GetDoiRegLog(DoiRegLogQuery query)
        {
            return IssueBusProvider.GetDoiRegLog(query);
        }

        public IList<DoiRegLogEntity> GetDoiRegLogList(DoiRegLogQuery query)
        {
            return IssueBusProvider.GetDoiRegLogList(query);
        }

        public Pager<DoiRegLogEntity> GetDoiRegLogPageList(DoiRegLogQuery query)
        {
            return IssueBusProvider.GetDoiRegLogPageList(query);
        }
        public ExecResult SaveDoiRegLog(DoiRegLogEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            if (model.isUpdate == true)
                result = UpdateDoiRegLog(model);
            else
                result = AddDoiRegLog(model);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "保存DOI注册记录成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "注册记录成功失败！";
            }
            return execResult;
        }

        #endregion

        #region 期刊参考文献
        /// <summary>
        /// 新增期刊参考文献
        /// </summary>
        /// <param name="issueReferenceEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool AddIssueReference(IssueReferenceEntity issueReferenceEntity)
        {
            return IssueBusProvider.AddIssueReference(issueReferenceEntity);
        }

        /// <summary>
        /// 编辑期刊参考文献
        /// </summary>
        /// <param name="issueReferenceEntity"></param>
        /// <returns></returns>
        public bool UpdateIssueReference(IssueReferenceEntity issueReferenceEntity)
        {
            return IssueBusProvider.UpdateIssueReference(issueReferenceEntity);
        }

        /// <summary>
        /// 获取参考文献实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public IssueReferenceEntity GetIssueReference(Int64 ReferenceID)
        {
            return IssueBusProvider.GetIssueReference(ReferenceID);
        }

        /// <summary>
        /// 根据期刊编号删除参考文献
        /// </summary>
        /// <param name="ContentID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelIssueReferenceByContentID(Int64[] ContentID)
        {
            return IssueBusProvider.DelIssueReferenceByContentID(ContentID);
        }

        /// <summary>
        /// 删除期刊参考文献
        /// </summary>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        public bool DelIssueReference(Int64[] ReferenceID)
        {
            return IssueBusProvider.DelIssueReference(ReferenceID);
        }

        /// <summary>
        /// 获取参考文献分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueReferenceEntity> GetIssueReferencePageList(IssueReferenceQuery query)
        {
            return IssueBusProvider.GetIssueReferencePageList(query);
        }

        /// <summary>
        /// 获取参考文献数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<IssueReferenceEntity> GetIssueReferenceList(IssueReferenceQuery query)
        {
            return IssueBusProvider.GetIssueReferenceList(query);
        }
        #endregion

        #region 期刊订阅

        /// <summary>
        /// 新增期刊订阅
        /// </summary>
        /// <param name="issueSubscribeEntity"></param>
        /// <returns></returns>
        public bool AddIssueSubscribe(IssueSubscribeEntity issueSubscribeEntity)
        {
            return IssueBusProvider.AddIssueSubscribe(issueSubscribeEntity);
        }

        /// <summary>
        /// 编辑期刊订阅
        /// </summary>
        /// <param name="issueSubscribeEntity"></param>
        /// <returns></returns>
        public bool UpdateIssueSubscribe(IssueSubscribeEntity issueSubscribeEntity)
        {
            return IssueBusProvider.UpdateIssueSubscribe(issueSubscribeEntity);
        }

        /// <summary>
        /// 获取期刊订阅
        /// </summary>
        /// <param name="subscribeID"></param>
        /// <returns></returns>
        public IssueSubscribeEntity GetIssueSubscribe(Int64 subscribeID)
        {
            return IssueBusProvider.GetIssueSubscribe(subscribeID);
        }

        /// <summary>
        /// 删除期刊参考文献
        /// </summary>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        public ExecResult DelIssueSubscribe(Int64[] subscribeID)
        {
            ExecResult execResult = new ExecResult();
            bool result = IssueBusProvider.DelIssueSubscribe(subscribeID);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除期刊订阅信息成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除期刊订阅信息失败！";
            }
            return execResult;
        }

        /// <summary>
        /// 获取期刊订阅分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueSubscribeEntity> GetIssueSubscribePageList(IssueSubscribeQuery query)
        {
            return IssueBusProvider.GetIssueSubscribePageList(query);
        }

        /// <summary>
        /// 获取期刊订阅数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<IssueSubscribeEntity> GetIssueSubscribeList(IssueSubscribeQuery query)
        {
            return IssueBusProvider.GetIssueSubscribeList(query);
        }

        /// <summary>
        /// 保存期刊订阅
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveIssueSubscribe(IssueSubscribeEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            model.Subscriber = model.Subscriber.TextFilter();
            model.Mobile = model.Mobile.TextFilter();
            model.Tel = model.Tel.TextFilter();
            model.Fax = model.Fax.TextFilter();
            model.Email = model.Email.TextFilter();
            model.Address = model.Address.TextFilter();
            model.ZipCode = model.ZipCode.TextFilter();
            model.ContactUser = model.ContactUser.TextFilter();
            model.SubscribeInfo = model.SubscribeInfo.TextFilter();
            model.InvoiceHead = model.InvoiceHead.TextFilter();
            model.Note = model.Note.TextFilter();           
            if (model.SubscribeID == 0)
                result = AddIssueSubscribe(model);
            else
                result = UpdateIssueSubscribe(model);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "保存期刊订阅成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "保存期刊订阅失败！";
            }
            return execResult;
        }
        #endregion

        /// <summary>
        /// 获取当前期刊最新的年、卷、期、当期封面图片
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IssueSiteEntity GetCurIssueInfo(IssueSetQuery query)
        {
            return IssueBusProvider.GetCurIssueInfo(query);
        }

        #region 下载次数统计
        /// <summary>
        /// 获取期刊下载次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(IssueDownLogQuery query)
        {
            return IssueBusProvider.GetIssueDownLogPageList(query);
        }

        /// <summary>
        /// 获取期刊下载明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogDetailPageList(IssueDownLogQuery query)
        {
            return IssueBusProvider.GetIssueDownLogDetailPageList(query);
        }
        #endregion

        #region 浏览次数统计
        /// <summary>
        /// 获取期刊下载次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueViewLogEntity> GetIssueViewLogPageList(IssueViewLogQuery query)
        {
            return IssueBusProvider.GetIssueViewLogPageList(query);
        }

        /// <summary>
        /// 获取期刊下载明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<IssueViewLogEntity> GetIssueViewLogDetailPageList(IssueViewLogQuery query)
        {
            return IssueBusProvider.GetIssueViewLogDetailPageList(query);
        }
        #endregion



        
    }
}
