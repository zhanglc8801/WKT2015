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
    public class SiteNoticeAPIController : ApiBaseController
    {
        /// <summary>
        /// 获取站点公告分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<SiteNoticeEntity> GetPageList(SiteNoticeQuery query)
        {
            ISiteNoticeService service = ServiceContainer.Instance.Container.Resolve<ISiteNoticeService>();
            Pager<SiteNoticeEntity> pager = service.GetSiteNoticePageList(query);
            return pager;
        }

        /// <summary>
        /// 获取站点公告列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<SiteNoticeEntity> GetList(SiteNoticeQuery query)
        {
            ISiteNoticeService service = ServiceContainer.Instance.Container.Resolve<ISiteNoticeService>();
            IList<SiteNoticeEntity> list = service.GetSiteNoticeList(query);
            return list;
        }

        /// <summary>
        /// 获取站点公告实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public SiteNoticeEntity GetModel(SiteNoticeQuery query)
        {
            ISiteNoticeService service = ServiceContainer.Instance.Container.Resolve<ISiteNoticeService>();
            SiteNoticeEntity model = service.GetSiteNotice(query.NoticeID);
            return model;
        }

        /// <summary>
        /// 保存站点公告
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Save(SiteNoticeEntity model)
        {
            ISiteNoticeService service = ServiceContainer.Instance.Container.Resolve<ISiteNoticeService>();
            return service.Save(model);
        }

        /// <summary>
        /// 删除站点公告
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(SiteNoticeQuery query)
        {
            ExecResult execResult = new ExecResult();
            ISiteNoticeService service = ServiceContainer.Instance.Container.Resolve<ISiteNoticeService>();
            Int64[] NoticeIDs = query.NoticeIDs;
            if (NoticeIDs == null)
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有删除任何数据！";
                return execResult;
            }
            string msg = string.Empty;
            bool result = service.BatchDeleteSiteNotice(NoticeIDs);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除失败！";
            }
            return execResult;
        }
    }
}
