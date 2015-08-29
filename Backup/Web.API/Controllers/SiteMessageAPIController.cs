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
    public class SiteMessageAPIController : ApiBaseController
    {
        /// <summary>
        /// 获取站内消息分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<SiteMessageEntity> GetPageList(SiteMessageQuery query)
        {
            ISiteMessageService service = ServiceContainer.Instance.Container.Resolve<ISiteMessageService>();
            Pager<SiteMessageEntity> pager = service.GetSiteMessagePageList(query);
            return pager;
        }

        /// <summary>
        /// 获取站内消息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<SiteMessageEntity> GetList(SiteMessageQuery query)
        {
            ISiteMessageService service = ServiceContainer.Instance.Container.Resolve<ISiteMessageService>();
            IList<SiteMessageEntity> list = service.GetSiteMessageList(query);
            return list;
        }

        /// <summary>
        /// 获取站内消息实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public SiteMessageEntity GetModel(SiteMessageQuery query)
        {
            ISiteMessageService service = ServiceContainer.Instance.Container.Resolve<ISiteMessageService>();
            SiteMessageEntity model = service.GetSiteMessage(query.JournalID, query.MessageID);
            return model;
        }

        /// <summary>
        /// 保存站内消息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Save(SiteMessageEntity model)
        {
            ISiteMessageService service = ServiceContainer.Instance.Container.Resolve<ISiteMessageService>();
            return service.Save(model);
        }

        /// <summary>
        /// 删除站内消息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(SiteMessageQuery query)
        {
            ISiteMessageService service = ServiceContainer.Instance.Container.Resolve<ISiteMessageService>();
            return service.Del(query.MessageIDs);
        }

        /// <summary>
        /// 阅读消息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool Viewed(SiteMessageQuery query)
        {
            ISiteMessageService service = ServiceContainer.Instance.Container.Resolve<ISiteMessageService>();
            return service.UpdateMsgViewed(query.JournalID, query.MessageID);
        }

    }
}
