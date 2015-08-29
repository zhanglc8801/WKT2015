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
    public class AuthorDetailAPIController : AuthorAPIController
    {
        /// <summary>
        /// 获取新闻资讯分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<AuthorDetailEntity> GetPageList(AuthorDetailQuery query)
        {
            IAuthorDetailService service = ServiceContainer.Instance.Container.Resolve<IAuthorDetailService>();
            Pager<AuthorDetailEntity> pager = service.GetAuthorDetailPageList(query);
            return pager;
        }

        /// <summary>
        /// 获取新闻资讯列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<AuthorDetailEntity> GetList(AuthorDetailQuery query)
        {
            IAuthorDetailService service = ServiceContainer.Instance.Container.Resolve<IAuthorDetailService>();
            IList<AuthorDetailEntity> list = service.GetAuthorDetailList(query);
            return list;
        }

        /// <summary>
        /// 获取作者详细信息实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public AuthorDetailEntity GetModel(AuthorDetailQuery query)
        {
            IAuthorDetailService service = ServiceContainer.Instance.Container.Resolve<IAuthorDetailService>();
            AuthorDetailEntity model = service.GetAuthorDetailModel(query.AuthorID);
            return model;
        }

        /// <summary>
        /// 保存作者详细信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Save(AuthorDetailEntity model)
        {
            try
            {
                IAuthorDetailService service = ServiceContainer.Instance.Container.Resolve<IAuthorDetailService>();
                return service.Save(model);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("保存用户信息失败：" + ex.ToString());
                ExecResult result = new ExecResult();
                result.result = EnumJsonResult.failure.ToString();
                result.msg = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// 删除作者信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(AuthorDetailQuery query)
        {
            IAuthorDetailService service = ServiceContainer.Instance.Container.Resolve<IAuthorDetailService>();
            return service.DelAuthorDetail(query.AuthorIDs);
        }

        /// <summary>
        /// 设置作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SetExpert(AuthorDetailQuery query)
        {
            IAuthorDetailService service = ServiceContainer.Instance.Container.Resolve<IAuthorDetailService>();
            return service.SetAuthorExpert(query);
        }

        /// <summary>
        /// 取消作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult CancelExpert(AuthorDetailQuery query)
        {
            IAuthorDetailService service = ServiceContainer.Instance.Container.Resolve<IAuthorDetailService>();
            return service.CancelAuthorExpert(query);
        }

        #region 专家分组配置
        /// <summary>
        /// 获取专家分组信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<AuthorDetailEntity> GetExpertGroupMapList(ExpertGroupMapEntity query)
        {
            IAuthorDetailService service = ServiceContainer.Instance.Container.Resolve<IAuthorDetailService>();
            return service.GetExpertGroupMapList(query);
        }

        /// <summary>
        /// 保存专家分组信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ExpertGroupID"></param>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveExpertGroupMap(ExpertGroupMapQuery query)
        {
            IAuthorDetailService service = ServiceContainer.Instance.Container.Resolve<IAuthorDetailService>();
            return service.SaveExpertGroupMap(query.list, query.ExpertGroupID, query.JournalID);
        }
        #endregion
    }
}
