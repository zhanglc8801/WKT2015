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
    public class ContributionInfoAPIController : ApiBaseController
    {
        #region 投稿相关

        /// <summary>
        /// 获取稿件的附件
        /// </summary>
        /// <param name="cQuery">稿件ID</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public string GetContributionAttachment(ContributionInfoQuery cQuery)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            string result = service.GetContributionAttachment(cQuery);
            return result;
        }

        /// <summary>
        /// 投稿
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Submission(ContributionInfoEntity model)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            ExecResult result = service.AuthorPlatform(model);
            return result;
        }

        /// <summary>
        /// 保存稿件格式修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveFormat(ContributionInfoEntity model)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            ExecResult result = service.SaveFormat(model);
            return result;
        }

        /// <summary>
        /// 新增稿件表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool Add(ContributionInfoEntity model)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            bool result = service.AddContributionInfo(model);
            return result;
        }

        /// <summary>
        /// 编辑稿件表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool Update(ContributionInfoEntity model)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            bool result = service.UpdateContributionInfo(model);
            return result;
        }

        /// <summary>
        /// 获取稿件信息实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ContributionInfoEntity GetModel(ContributionInfoQuery query)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            ContributionInfoEntity model = service.GetContributionInfo(query);
            return model;
        }

        /// <summary>
        /// 更新稿件状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult ChangeStatus(ContributionInfoQuery query)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            ExecResult result = service.ChangeStatus(query);
            return result;
        }

        /// <summary>
        /// 删除投稿信息
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(Int64[] CID)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            ExecResult result = service.DelContributionInfo(CID);           
            return result;
        }

        /// <summary>
        /// 获取稿件分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<ContributionInfoEntity> GetPageList(ContributionInfoQuery query)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            Pager<ContributionInfoEntity> pager = service.GetContributionInfoPageList(query);
            return pager;
        }

        /// <summary>
        /// 获取稿件数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<ContributionInfoEntity> GetList(ContributionInfoQuery query)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            IList<ContributionInfoEntity> pager = service.GetContributionInfoList(query);
            return pager;
        }
        #endregion

        #region 撤稿相关
        /// <summary>
        /// 撤稿
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Draft(RetractionsBillsEntity model)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            ExecResult result = service.DraftContribution(model);
            return result;
        }

        /// <summary>
        /// 新增撤稿表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool AddRetractionsBills(RetractionsBillsEntity model)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            bool result = service.AddRetractionsBills(model);
            return result;
        }

        /// <summary>
        /// 编辑撤稿表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool UpdateRetractionsBills(RetractionsBillsEntity model)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            bool result = service.UpdateRetractionsBills(model);
            return result;
        }

        /// <summary>
        /// 获取撤稿信息实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public RetractionsBillsEntity GetRetractionsBillsModel(RetractionsBillsQuery query)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            RetractionsBillsEntity model = service.GetRetractionsBills(query);
            return model;
        }
        #endregion

        #region 添加稿件备注
        /// <summary>
        /// 保存稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveCRemark(CRemarkEntity model)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            ExecResult result = service.SaveCRemark(model);
            return result;
        }

        /// <summary>
        /// 获取稿件备注实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public CRemarkEntity GetCRemarkModel(CRemarkQuery query)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            CRemarkEntity model = service.GetCRemark(query);
            return model;
        }
        #endregion

        #region 收稿量统计
        /// <summary>
        /// 按年月统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<ContributionAccountEntity> GetContributionAccountListByYear(ContributionAccountQuery query)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            return service.GetContributionAccountListByYear(query);
        }

        /// <summary>
        /// 按基金级别统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<ContributionAccountEntity> GetContributionAccountListByFund(ContributionAccountQuery query)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            return service.GetContributionAccountListByFund(query);
        }

        /// <summary>
        /// 按作者统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<ContributionAccountEntity> GetContributionAccountListByAuhor(ContributionAccountQuery query)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            return service.GetContributionAccountListByAuhor(query);
        }
        #endregion

        /// <summary>
        /// 更新介绍信标记
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult UpdateIntroLetter(ContributionInfoQuery model)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            bool flag = service.UpdateIntroLetter(model);
            ExecResult result = new ExecResult();
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
            }
            else
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "更新介绍信标记出现异常，请稍后再试";
            }
            return result;
        }

        /// <summary>
        /// 处理撤稿申请
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult DealWithdrawal(ContributionInfoQuery cEntity)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            bool flag = service.DealWithdrawal(cEntity);
            ExecResult result = new ExecResult();
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
            }
            else
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "处理撤稿申请出现异常，请稍后再试";
            }
            return result;
        }

        /// <summary>
        /// 撤销删除
        /// </summary>
        /// <param name="cEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult CancelDelete(ContributionInfoQuery cEntity)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            bool flag = service.CancelDelete(cEntity);
            ExecResult result = new ExecResult();
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
            }
            else
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "撤销删除出现异常，请稍后再试";
            }
            return result;
        }

        /// <summary>
        /// 获取稿件作者
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Auth]
        [System.Web.Http.AcceptVerbs("POST")]
        public ContributionAuthorEntity GetContributionAuthorInfo(ContributionAuthorQuery query)
        {
            IContributionInfoService authorService = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            var ContributionAuthorEntity = authorService.GetContributionAuthor(query.CAuthorID);
            return ContributionAuthorEntity;
        }

        /// <summary>
        /// 获取稿件作者详细信息数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<ContributionAuthorEntity> GetContributionAuthorList(ContributionAuthorQuery query)
        {
            IContributionInfoService service = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
            IList<ContributionAuthorEntity> list = service.GetContributionAuthorList(query);
            return list;
        }
        

    }
}
