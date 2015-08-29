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
    /// <summary>
    /// 站点内容
    /// </summary>
    public class SiteContentAPIController : ApiBaseController
    {
        # region 网站留言板

        /// <summary>
        /// 获取留言分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<GuestbookEntity> GetGuestBookPageList(GuestbookQuery query)
        {
            IGuestbookService service = ServiceContainer.Instance.Container.Resolve<IGuestbookService>();
            Pager<GuestbookEntity> pager = service.GetGuestbookPageList(query);
            return pager;
        }

        /// <summary>
        /// 保存新闻资讯
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult  SaveGuestBook(GuestbookEntity model)
        {
            ExecResult exeResult = new ExecResult();
            try
            {
                IGuestbookService service = ServiceContainer.Instance.Container.Resolve<IGuestbookService>();
                bool flag = service.AddGuestbook(model);
                if (flag)
                {
                    exeResult.result = EnumJsonResult.success.ToString();
                }
                else
                {
                    exeResult.result = EnumJsonResult.failure.ToString();
                }
            }
            catch (Exception ex)
            {
                exeResult.result = EnumJsonResult.error.ToString();
                exeResult.msg = "保存留言出现异常：" + ex.Message;
            }
            return exeResult;
        }

        # endregion

        # region 资讯相关

        /// <summary>
        /// 获取新闻资讯分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<SiteContentEntity> GetPageList(SiteContentQuery query)
        {
            ISiteContentService service = ServiceContainer.Instance.Container.Resolve<ISiteContentService>();
            Pager<SiteContentEntity> pager = service.GetSiteContentPageList(query);
            return pager;
        }

        /// <summary>
        /// 获取新闻资讯列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<SiteContentEntity> GetList(SiteContentQuery query)
        {
            ISiteContentService service = ServiceContainer.Instance.Container.Resolve<ISiteContentService>();
            IList<SiteContentEntity> list = service.GetSiteContentList(query);
            return list;
        }

        /// <summary>
        /// 获取新闻资讯实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public SiteContentEntity GetModel(SiteContentQuery query)
        {
            ISiteContentService service = ServiceContainer.Instance.Container.Resolve<ISiteContentService>();
            SiteContentEntity model = service.GetSiteContentModel(query.ContentID);
            return model;
        }

        /// <summary>
        /// 保存新闻资讯
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Save(SiteContentEntity model)
        {
            ISiteContentService service = ServiceContainer.Instance.Container.Resolve<ISiteContentService>();
            return service.Save(model);
        }

        /// <summary>
        /// 删除新闻资讯
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(SiteContentQuery query)
        {
            ExecResult execResult = new ExecResult();
            ISiteContentService service = ServiceContainer.Instance.Container.Resolve<ISiteContentService>();
            Int64[] ContentIDs = query.ContentIDs;
            if (ContentIDs == null)
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有删除任何数据！";
                return execResult;
            }
            string msg = string.Empty;
            bool result = service.BatchDeleteSiteContent(ContentIDs);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除新闻资讯成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除新闻资讯失败！";
            }
            return execResult;
        }

        # endregion
    }
}
