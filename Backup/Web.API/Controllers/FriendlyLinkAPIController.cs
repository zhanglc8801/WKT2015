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
    public class FriendlyLinkAPIController : ApiBaseController
    {
        /// <summary>
        /// 获取友情链接分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<FriendlyLinkEntity> GetPageList(FriendlyLinkQuery query)
        {
            IFriendlyLinkService service = ServiceContainer.Instance.Container.Resolve<IFriendlyLinkService>();
            Pager<FriendlyLinkEntity> pager = service.GetFriendlyLinkPageList(query);
            return pager;
        }

        /// <summary>
        /// 获取友情链接列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<FriendlyLinkEntity> GetList(FriendlyLinkQuery query)
        {
            IFriendlyLinkService service = ServiceContainer.Instance.Container.Resolve<IFriendlyLinkService>();
            IList<FriendlyLinkEntity> list = service.GetFriendlyLinkList(query);
            return list;
        }

        /// <summary>
        /// 获取友情链接实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public FriendlyLinkEntity GetModel(FriendlyLinkQuery query)
        {
            IFriendlyLinkService service = ServiceContainer.Instance.Container.Resolve<IFriendlyLinkService>();
            FriendlyLinkEntity model = service.GetFriendlyLinkModel(query.LinkID);
            return model;
        }

        /// <summary>
        /// 保存友情链接
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Save(FriendlyLinkEntity model)
        {
            IFriendlyLinkService service = ServiceContainer.Instance.Container.Resolve<IFriendlyLinkService>();
            return service.Save(model);
        }

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(FriendlyLinkQuery query)
        {
            ExecResult execResult = new ExecResult();
            IFriendlyLinkService service = ServiceContainer.Instance.Container.Resolve<IFriendlyLinkService>();
            Int64[] LinkIDs = query.LinkIDs;
            if (LinkIDs == null)
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有删除任何数据！";
                return execResult;
            }
            string msg = string.Empty;
            bool result = service.BatchDeleteFriendlyLink(LinkIDs);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除友情链接成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除友情链接失败！";
            }
            return execResult;
        }
    }
}
