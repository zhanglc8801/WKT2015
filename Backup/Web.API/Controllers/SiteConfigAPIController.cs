using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Service.Interface;
using WKT.Service.Wrapper;
using WKT.Model.Enum;

namespace Web.API.Controllers
{
    public class SiteConfigAPIController : ApiBaseController
    {
        /// <summary>
        /// 获取站点实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public SiteConfigEntity GetSiteConfigModel(SiteConfigQuery query)
        {
            SiteConfigEntity model = null;
            ISiteConfigService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigService>();
            model = service.GetSiteConfig(query);
            return model;
        }

        /// <summary>
        /// 编辑站点实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult UpdateSiteConfig(SiteConfigEntity model)
        {
            ExecResult execResult = new ExecResult();
            ISiteConfigService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigService>();
            bool result = service.UpdateSiteConfig(model);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "修改站点信息成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "修改站点信息失败！";
            }
            return execResult;
        }

        /// <summary>
        /// 更新总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool UpdateSiteAccessCount(SiteConfigQuery query)
        {
            ISiteConfigService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigService>();
            return service.UpdateSiteAccessCount(query);
        }

        /// <summary>
        /// 获取总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public int GetSiteAccessCount(SiteConfigQuery query)
        {
            ISiteConfigService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigService>();
            return service.GetSiteAccessCount(query);
        }
    }
}