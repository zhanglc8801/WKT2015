using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Service.Interface;
using WKT.Service.Wrapper;
using WKT.Model.Enum;

namespace Web.API.Controllers
{
    public class IssueAPIController : ApiBaseController
    {
        #region 年卷设置
        /// <summary>
        /// 获取年卷设置分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<YearVolumeEntity> GetYearVolumePageList(YearVolumeQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            Pager<YearVolumeEntity> pager = service.GetYearVolumePageList(query);
            return pager;
        }

        /// <summary>
        /// 获取年卷设置列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<YearVolumeEntity> GetYearVolumeList(YearVolumeQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            IList<YearVolumeEntity> list = service.GetYearVolumeList(query);
            return list;
        }

        /// <summary>
        /// 获取年卷设置实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public YearVolumeEntity GetYearVolumeModel(YearVolumeQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            YearVolumeEntity model=null;
            if (query.setID == 0)
                model = service.GetMaxYearVolume(query.JournalID);
            else
                model = service.GetYearVolume(query.setID);
            return model;
        }

        /// <summary>
        /// 保存年卷设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveYearVolume(YearVolumeEntity model)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.SaveYearVolume(model);
        }

        /// <summary>
        /// 删除年卷设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult DelYearVolume(YearVolumeQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.DelYearVolume(query.setIDs);
        }
        #endregion

        #region 期设置
        /// <summary>
        /// 获取期设置分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<IssueSetEntity> GetIssueSetPageList(IssueSetQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            Pager<IssueSetEntity> pager = service.GetIssueSetPageList(query);
            return pager;
        }

        /// <summary>
        /// 获取期设置列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<IssueSetEntity> GetIssueSetList(IssueSetQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            IList<IssueSetEntity> list = service.GetIssueSetList(query);
            return list;
        }

        /// <summary>
        /// 获取期设置实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IssueSetEntity GetIssueSetModel(IssueSetQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            IssueSetEntity model = null;
            if (query.IssueSetID == 0)
                model = service.GetMaxIssueSet(query.JournalID);
            else
                model = service.GetIssueSet(query.IssueSetID);
            if (model != null)
            {
                Pager<IssueSetEntity> currentEntity = service.GetIssueSetPageList(query);
              if (currentEntity != null && currentEntity.ItemList!=null && currentEntity.ItemList.Count>0)
              {
                  IList<IssueSetEntity> list= currentEntity.ItemList;
                  IssueSetEntity  single= list.Where(o => o.Issue == model.Issue).SingleOrDefault();
                  model.PrintExpenses = single.PrintExpenses;
              }
            }
            return model;
        }

        /// <summary>
        /// 保存期设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveIssueSet(IssueSetEntity model)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.SaveIssueSet(model);
        }

        /// <summary>
        /// 删除期
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult DelIssueSet(IssueSetQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.DelIssueSet(query.IssueSetIDs);
        }
        #endregion

        #region 期刊栏目
        /// <summary>
        /// 获取期刊栏目分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<JournalChannelEntity> GetJournalChannelPageList(JournalChannelQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            Pager<JournalChannelEntity> pager = service.GetJournalChannelPageList(query);
            return pager;
        }

        /// <summary>
        /// 获取期刊栏目列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<JournalChannelEntity> GetJournalChannelList(JournalChannelQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            IList<JournalChannelEntity> list = service.GetJournalChannelList(query);
            return list;
        }

        /// <summary>
        /// 根据期刊数据 按照期刊栏目数据分组 获取当前期刊数据所属的期刊栏目数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<JournalChannelEntity> GetJournalChannelListByIssueContent(JournalChannelQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            IList<JournalChannelEntity> list = service.GetJournalChannelListByIssueContent(query);
            return list;
        }

        /// <summary>
        /// 获取期刊栏目实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public JournalChannelEntity GetJournalChannelModel(JournalChannelQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            JournalChannelEntity model = service.GetJournalChannel(query.JChannelID);           
            return model;
        }

        /// <summary>
        /// 保存期刊栏目
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveJournalChannel(JournalChannelEntity model)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.SaveJournalChannel(model);
        }

        /// <summary>
        /// 删除期刊栏目
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult DelJournalChannel(JournalChannelQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.DelJournalChannel(query);
        }
        #endregion

        #region 期刊

        /// <summary>
        /// 设置期刊年期
        /// </summary>
        /// <param name="cEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SetContributionYearIssue(IssueContentQuery cEntity)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.SetContributionYearIssue(cEntity);
        }

        /// <summary>
        /// 获取期刊分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<IssueContentEntity> GetIssueContentPageList(IssueContentQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            Pager<IssueContentEntity> pager = service.GetIssueContentPageList(query);
            return pager;
        }

        /// <summary>
        /// 获取期刊列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<IssueContentEntity> GetIssueContentList(IssueContentQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            IList<IssueContentEntity> list = service.GetIssueContentList(query);
            return list;
        }
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<IssueContentEntity> GetIssueContentList2(int Year,int Issue)
        {
            IssueContentQuery query = new IssueContentQuery();
            query.Year = Year;
            query.Issue = Issue;

            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            IList<IssueContentEntity> list = service.GetIssueContentList(query);
            return list;
        }

        /// <summary>
        /// 获取期刊实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IssueContentEntity GetIssueContentModel(IssueContentQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            IssueContentEntity model = service.GetIssueContent(query);
            return model;
        }

        /// <summary>
        /// 保存期刊
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveIssueContent(IssueContentEntity model)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.SaveIssueContent(model);
        }

        /// <summary>
        /// 删除期刊
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult DelIssueContent(IssueContentQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.DelIssueContent(query.contentIDs);
        }

        /// <summary>
        /// 更新期刊内容浏览次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool UpdateIssueContentHits(IssueContentQuery model)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.UpdateIssueContentHits(model);
        }

        /// <summary>
        /// 更新期刊内容浏览次数(RichHTML)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool UpdateIssueContentHtmlHits(IssueContentQuery model)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.UpdateIssueContentHtmlHits(model);
        }

	    /// <summary>
        /// 更新期刊内容下载次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool UpdateIssueContentDownloads(IssueContentQuery model)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.UpdateIssueContentDownloads(model);
        }
        #endregion

        #region DOI注册
        

        [System.Web.Http.AcceptVerbs("POST")]
        public bool DelDoiRegLog(long[] PKID)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.DelDoiRegLog(PKID);
        }

        [System.Web.Http.AcceptVerbs("POST")]
        public DoiRegLogEntity GetDoiRegLog(DoiRegLogQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            DoiRegLogEntity model = service.GetDoiRegLog(query);
            return model;
        }

        [System.Web.Http.AcceptVerbs("POST")]
        public IList<DoiRegLogEntity> GetDoiRegLogList(DoiRegLogQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            IList<DoiRegLogEntity> list = service.GetDoiRegLogList(query);
            return list;
        }

        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<DoiRegLogEntity> GetDoiRegLogPageList(DoiRegLogQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            Pager<DoiRegLogEntity> pager = service.GetDoiRegLogPageList(query);
            return pager;
        }

        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveDoiRegLog(DoiRegLogEntity model)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.SaveDoiRegLog(model);
        }
        #endregion

        #region 期刊订阅

        /// <summary>
        /// 获取期刊订阅分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<IssueSubscribeEntity> GetIssueSubscribePageList(IssueSubscribeQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            Pager<IssueSubscribeEntity> pager = service.GetIssueSubscribePageList(query);
            return pager;
        }

        /// <summary>
        /// 获取期刊订阅列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<IssueSubscribeEntity> GetIssueSubscribeList(IssueSubscribeQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            IList<IssueSubscribeEntity> list = service.GetIssueSubscribeList(query);
            return list;
        }

        /// <summary>
        /// 获取期刊订阅实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IssueSubscribeEntity GetIssueSubscribeModel(IssueSubscribeQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            IssueSubscribeEntity model = service.GetIssueSubscribe(query.subscribeID);
            return model;
        }

        /// <summary>
        /// 保存期刊订阅
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveIssueSubscribe(IssueSubscribeEntity model)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.SaveIssueSubscribe(model);
        }

        /// <summary>
        /// 删除期刊订阅
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult DelIssueSubscribe(IssueSubscribeQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.DelIssueSubscribe(query.subscribeIDs);
        }
        #endregion

        /// <summary>
        /// 获取当前期刊最新的年、卷、期、当期封面图片
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IssueSiteEntity GetCurIssueInfo(IssueSetQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            IssueSiteEntity issueEntity = service.GetCurIssueInfo(query);
            return issueEntity;
        }

        # region 访问下载日志

        /// <summary>
        /// 保存访问日志
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveViewLog(IssueViewLogEntity model)
        {
            ExecResult exeResult = new ExecResult();
            IIssueViewLogService service = ServiceContainer.Instance.Container.Resolve<IIssueViewLogService>();
            try
            {
                bool flag = service.AddIssueViewLog(model);
                if (flag)
                {
                    exeResult.result = EnumJsonResult.success.ToString();
                }
                else
                {
                    exeResult.result = EnumJsonResult.failure.ToString();
                    exeResult.msg = "保存浏览日志失败";
                }
            }
            catch (Exception ex)
            {
                exeResult.result = EnumJsonResult.error.ToString();
                exeResult.msg = "保存浏览日志异常：" + ex.Message;
            }
            return exeResult;
        }

        /// <summary>
        /// 保存下载日志
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveDownloadLog(IssueDownLogEntity model)
        {
            ExecResult exeResult = new ExecResult();
            IIssueDownLogService service = ServiceContainer.Instance.Container.Resolve<IIssueDownLogService>();
            try
            {
                bool flag = service.AddIssueDownLog(model);
                if (flag)
                {
                    exeResult.result = EnumJsonResult.success.ToString();
                }
                else
                {
                    exeResult.result = EnumJsonResult.failure.ToString();
                    exeResult.msg = "保存下载日志失败";
                }
            }
            catch (Exception ex)
            {
                exeResult.result = EnumJsonResult.error.ToString();
                exeResult.msg = "保存下载日志异常：" + ex.Message;
            }
            return exeResult;
        }

        # endregion

        #region 下载次数统计
        /// <summary>
        /// 获取期刊下载次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(IssueDownLogQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.GetIssueDownLogPageList(query);
        }

        /// <summary>
        /// 获取期刊下载明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<IssueDownLogEntity> GetIssueDownLogDetailPageList(IssueDownLogQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.GetIssueDownLogDetailPageList(query);
        }
        #endregion

        #region 浏览次数统计
        /// <summary>
        /// 获取期刊浏览次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<IssueViewLogEntity> GetIssueViewLogPageList(IssueViewLogQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.GetIssueViewLogPageList(query);
        }

        /// <summary>
        /// 获取期刊浏览明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<IssueViewLogEntity> GetIssueViewLogDetailPageList(IssueViewLogQuery query)
        {
            IIssueService service = ServiceContainer.Instance.Container.Resolve<IIssueService>();
            return service.GetIssueViewLogDetailPageList(query);
        }
        #endregion

    }
}
