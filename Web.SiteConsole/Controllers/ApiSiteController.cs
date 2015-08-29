using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;

using WKT.Common.Extension;
using WKT.Log;
using WKT.Model;
using WKT.Service.Interface;
using WKT.Service.Wrapper;

namespace Web.SiteConsole.Controllers
{
    /// <summary>
    /// Api站点管理
    /// </summary>
    public class ApiSiteController : BaseController
    {
        # region 首页

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.TotalCount = 0;
            IApiServerInfoService apiServerService = ServiceContainer.Instance.Container.Resolve<IApiServerInfoService>();
            ApiServerInfoQuery queryEntity = new ApiServerInfoQuery();
            queryEntity.CurrentPage = 1;
            Pager<ApiServerInfoEntity> pagerApiServerList = apiServerService.GetApiServerInfoPageList(queryEntity);
            IList<ApiServerInfoEntity> listServer = new List<ApiServerInfoEntity>();
            if (pagerApiServerList != null)
            {
                listServer = pagerApiServerList.ItemList;
                ViewBag.TotalCount = pagerApiServerList.TotalRecords;
            }

            return View(listServer);
        }

        public ActionResult IndexAjax(ApiServerInfoQuery queryEntity)
        {
            if (!Request.IsAjaxRequest())
            {
                return Content("{\"result\":\"error\",\"msg\":\"非法访问\"}");
            }
            else
            {
                IApiServerInfoService apiServerService = ServiceContainer.Instance.Container.Resolve<IApiServerInfoService>();
                queryEntity.CurrentPage = queryEntity.CurrentPage + 1;
                WKT.Model.Pager<ApiServerInfoEntity> apiServerPagerList = apiServerService.GetApiServerInfoPageList(queryEntity);
                if (apiServerPagerList != null)
                {
                    return Content("{\"result\":\"success\",\"msg\":\"成功\",\"data\":" + JsonConvert.SerializeObject(apiServerPagerList) + "}");
                }
                else
                {
                    return Content("{\"result\":\"error\",\"msg\":\"系统出现异常，请稍后再试\"}");
                }
            }
        }

        # endregion

        # region 新增、修改

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(int? ApiServerID)
        {
            ApiServerInfoEntity apiServerEntity = null;
            ViewBag.IsEdit = false;
            if (ApiServerID != null)
            {
                ViewBag.IsEdit = true;
                IApiServerInfoService apiServerService = ServiceContainer.Instance.Container.Resolve<IApiServerInfoService>();
                apiServerEntity = apiServerService.GetApiServerInfo(ApiServerID.Value);                
            }
            return View(apiServerEntity);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAjax(ApiServerInfoEntity apiServerEntity)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Content("{\"result\":\"failure\",\"msg\":\"非法请求\"}");
                }
                string msg = "";
                if (ModelState.IsValid)
                {
                    IApiServerInfoService apiServerService = ServiceContainer.Instance.Container.Resolve<IApiServerInfoService>();
                    bool flag = apiServerService.AddApiServerInfo(apiServerEntity);
                    if (flag)
                    {
                        msg = "{\"result\":\"success\"}";
                    }
                    else
                    {
                        msg = "{\"result\":\"failure\",\"msg\":\"新增 API Server 失败，请检查\"}";
                    }
                }
                else
                {
                    msg = "{\"result\":\"failure\",\"msg\":\"" + this.ExpendErrors() + "\"}";
                }
                return Content(msg);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("新增 API Server失败:" + ex.Message);
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="apiServerEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditAjax(ApiServerInfoEntity apiServerEntity)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Content("{\"result\":\"failure\",\"msg\":\"非法请求\"}");
                }
                string msg = "";
                if (ModelState.IsValid)
                {
                    IApiServerInfoService apiServerService = ServiceContainer.Instance.Container.Resolve<IApiServerInfoService>();
                    bool flag = apiServerService.UpdateApiServerInfo(apiServerEntity);
                    if (flag)
                    {
                        msg = "{\"result\":\"success\"}";
                    }
                    else
                    {
                        msg = "{\"result\":\"failure\",\"msg\":\"修改 API Server信息失败，请检查\"}";
                    }
                }
                else
                {
                    msg = "{\"result\":\"failure\",\"msg\":\"" + this.ExpendErrors() + "\"}";
                }
                return Content(msg);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("修改 API Server信息失败:" + ex.Message);
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        # endregion

        # region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAjax(int[] IDAarry)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Content("{\"result\":\"failure\",\"msg\":\"非法请求\"}");
                }
                if (IDAarry == null || IDAarry.Length == 0)
                {
                    return Content("{\"result\":\"failure\",\"msg\":\"请选择要删除的API Server\"}");
                }
                string msg = "";
                IApiServerInfoService apiServerService = ServiceContainer.Instance.Container.Resolve<IApiServerInfoService>();
                bool flag = apiServerService.BatchDeleteApiServerInfo(IDAarry);
                if (flag)
                {
                    msg = "{\"result\":\"success\"}";
                }
                else
                {
                    msg = "{\"result\":\"failure\"，\"msg\":\"删除失败，请检查\"}";
                }
                return Content(msg);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("删除APIServer失败:" + ex.Message);
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        # endregion
    }
}
