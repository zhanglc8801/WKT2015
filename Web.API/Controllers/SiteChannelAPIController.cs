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
    public class SiteChannelAPIController : ApiBaseController
    {
        /// <summary>
        /// 获取栏目数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<SiteChannelEntity> GetList(SiteChannelQuery query)
        {
            ISiteChannelService service = ServiceContainer.Instance.Container.Resolve<ISiteChannelService>();
            IList<SiteChannelEntity> list = service.GetSiteChannelList(query);
            return list;
        }

        /// <summary>
        /// 获取栏目实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public SiteChannelEntity GetModel(SiteChannelQuery query)
        {
            ISiteChannelService service = ServiceContainer.Instance.Container.Resolve<ISiteChannelService>();
            SiteChannelEntity model = service.GetSiteChannel(query);
            return model;
        }

        /// <summary>
        /// 保存栏目数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Save(SiteChannelEntity model)
        {
            ExecResult execResult = new ExecResult();
            ISiteChannelService service = ServiceContainer.Instance.Container.Resolve<ISiteChannelService>();
            return service.Save(model);
        }

        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(SiteChannelQuery query)
        {
            ExecResult execResult = new ExecResult();
            ISiteChannelService service = ServiceContainer.Instance.Container.Resolve<ISiteChannelService>();
            bool result = service.DeleteSiteChannel(query.ChannelID.Value);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除栏目成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除栏目失败！";
            }
            return execResult;
        }
    }
}
