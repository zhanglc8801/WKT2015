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
using WKT.Log;

namespace Web.API.Controllers
{
    public class FinanceContributeAPIController : ApiBaseController
    {
        /// <summary>
        /// 获取稿件费用分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<FinanceContributeEntity> GetPageList(FinanceContributeQuery query)
        {
            IFinanceContributeService service = ServiceContainer.Instance.Container.Resolve<IFinanceContributeService>();
            Pager<FinanceContributeEntity> pager = service.GetFinanceContributePageList(query);
            return pager;
        }

        /// <summary>
        /// 获取稿件费用列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<FinanceContributeEntity> GetList(FinanceContributeQuery query)
        {
            IFinanceContributeService service = ServiceContainer.Instance.Container.Resolve<IFinanceContributeService>();
            IList<FinanceContributeEntity> list = service.GetFinanceContributeList(query);
            return list;
        }

        /// <summary>
        /// 获取稿件费用实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public FinanceContributeEntity GetModel(FinanceContributeQuery query)
        {
            IFinanceContributeService service = ServiceContainer.Instance.Container.Resolve<IFinanceContributeService>();
            FinanceContributeEntity model = service.GetFinanceContribute(query.PKID);
            return model;
        }

        /// <summary>
        /// 保存稿件费用
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Save(FinanceContributeEntity model)
        {
            try
            {
                IFinanceContributeService service = ServiceContainer.Instance.Container.Resolve<IFinanceContributeService>();
                return service.Save(model);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("保存稿件费用信息失败：" + ex.ToString());
                ExecResult result = new ExecResult();
                result.result = EnumJsonResult.failure.ToString();
                result.msg = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// 删除稿件费用
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(FinanceContributeQuery query)
        {            
            IFinanceContributeService service = ServiceContainer.Instance.Container.Resolve<IFinanceContributeService>();
            return service.Del(query.PKIDs);
        }

        /// <summary>
        /// 获取财务入款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public Pager<FinanceAccountEntity> GetFinanceAccountPageList(ContributionInfoQuery query)
        {
            IFinanceContributeService service = ServiceContainer.Instance.Container.Resolve<IFinanceContributeService>();
            return service.GetFinanceAccountPageList(query);
        }

        /// <summary>
        /// 获取稿费统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public Pager<FinanceAccountEntity> GetFinanceGaoFeePageList(ContributionInfoQuery query)
        {
            IFinanceContributeService service = ServiceContainer.Instance.Container.Resolve<IFinanceContributeService>();
            Pager<FinanceAccountEntity> pager = service.GetFinanceGaoFeePageList(query);
            return pager;
        }

        /// <summary>
        /// 获取财务出款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public Pager<FinanceAccountEntity> GetFinanceOutAccountPageList(ContributionInfoQuery query)
        {
            IFinanceContributeService service = ServiceContainer.Instance.Container.Resolve<IFinanceContributeService>();
            return service.GetFinanceOutAccountPageList(query);
        }

        /// <summary>
        /// 获取财务统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public Pager<FinanceContributeEntity> GetFinanceGlancePageList(FinanceContributeQuery query)
        {
            IFinanceContributeService service = ServiceContainer.Instance.Container.Resolve<IFinanceContributeService>();
            Pager<FinanceContributeEntity> pager = service.GetFinanceGlancePageList(query);
            return pager;
        }

        

        /// <summary>
        /// 获取版面费报表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public Pager<FinanceContributeEntity> GetFinancePageFeeReportPageList(FinanceContributeQuery query)
        {
            IFinanceContributeService service = ServiceContainer.Instance.Container.Resolve<IFinanceContributeService>();
            Pager<FinanceContributeEntity> pager = service.GetFinancePageFeeReportPageList(query);
            return pager;
        }

    }
}
