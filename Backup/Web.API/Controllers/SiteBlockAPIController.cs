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
    public class SiteBlockAPIController : ApiBaseController
    {
        /// <summary>
        /// 获取内容块分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<SiteBlockEntity> GetPageList(SiteBlockQuery query)
        {
            ISiteBlockService service = ServiceContainer.Instance.Container.Resolve<ISiteBlockService>();
            Pager<SiteBlockEntity> pager = service.GetSiteBlockPageList(query);
            return pager;
        }

        /// <summary>
        /// 获取内容块列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<SiteBlockEntity> GetList(SiteBlockQuery query)
        {
            ISiteBlockService service = ServiceContainer.Instance.Container.Resolve<ISiteBlockService>();
            IList<SiteBlockEntity> list = service.GetSiteBlockList(query);
            return list;
        }

        /// <summary>
        /// 获取内容块实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public SiteBlockEntity GetModel(SiteBlockQuery query)
        {
            ISiteBlockService service = ServiceContainer.Instance.Container.Resolve<ISiteBlockService>();
            SiteBlockEntity model = service.GetSiteBlockModel(query.BlockID);
            return model;
        }

        /// <summary>
        /// 保存内容块
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Save(SiteBlockEntity model)
        {
            ISiteBlockService service = ServiceContainer.Instance.Container.Resolve<ISiteBlockService>();
            return service.Save(model);
        }

        /// <summary>
        /// 删除内容块
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(SiteBlockQuery query)
        {
            ExecResult execResult = new ExecResult();
            ISiteBlockService service = ServiceContainer.Instance.Container.Resolve<ISiteBlockService>();
            Int64[] BlockIDs = query.BlockIDs;
            if (BlockIDs == null)
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有删除任何数据！";
                return execResult;
            }
            string msg = string.Empty;
            bool result = service.BatchDeleteSiteBlock(BlockIDs);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除内容块成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除内容块失败！";
            }
            return execResult;
        }
    }
}
