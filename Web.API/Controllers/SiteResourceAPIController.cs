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
    public class SiteResourceAPIController : ApiBaseController
    {
        /// <summary>
        /// 获取资源文件分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<SiteResourceEntity> GetPageList(SiteResourceQuery query)
        {
            ISiteResourceService service = ServiceContainer.Instance.Container.Resolve<ISiteResourceService>();
            Pager<SiteResourceEntity> pager = service.GetSiteResourcePageList(query);
            return pager;
        }

        /// <summary>
        /// 获取资源文件列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<SiteResourceEntity> GetList(SiteResourceQuery query)
        {
            ISiteResourceService service = ServiceContainer.Instance.Container.Resolve<ISiteResourceService>();
            IList<SiteResourceEntity> list = service.GetSiteResourceList(query);
            return list;
        }

        /// <summary>
        /// 获取资源文件实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public SiteResourceEntity GetModel(SiteResourceQuery query)
        {
            ISiteResourceService service = ServiceContainer.Instance.Container.Resolve<ISiteResourceService>();
            SiteResourceEntity model = service.GetSiteResourceModel(query);
            return model;
        }

        /// <summary>
        /// 保存资源文件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Save(SiteResourceEntity model)
        {
            ISiteResourceService service = ServiceContainer.Instance.Container.Resolve<ISiteResourceService>();
            return service.Save(model);
        }

        /// <summary>
        /// 累计下载次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool DownloadCount(SiteResourceEntity model)
        {            
            ISiteResourceService service = ServiceContainer.Instance.Container.Resolve<ISiteResourceService>();
            bool result = service.AccumulationDownloadCount(model);
            return result;
        }

        /// <summary>
        /// 删除资源文件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(SiteResourceQuery query)
        {
            ExecResult execResult = new ExecResult();
            ISiteResourceService service = ServiceContainer.Instance.Container.Resolve<ISiteResourceService>();
            Int64[] ResourceIDs = query.ResourceIDs;
            if (ResourceIDs == null)
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有删除任何数据！";
                return execResult;
            }
            string msg = string.Empty;
            bool result = service.BatchDeleteSiteResource(ResourceIDs);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除资源文件成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除资源文件失败！";
            }
            return execResult;
        }

    }
}
