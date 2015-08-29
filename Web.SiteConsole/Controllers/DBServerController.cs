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
    /// DBServer管理
    /// </summary>
    public class DBServerController : BaseController
    {
        # region 首页

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.TotalCount = 0;
            IDBServerInfoService dbServerService = ServiceContainer.Instance.Container.Resolve<IDBServerInfoService>();
            DBServerInfoQuery queryEntity = new DBServerInfoQuery();
            queryEntity.CurrentPage = 1;
            Pager<DBServerInfoEntity> pagerDBServerList = dbServerService.GetDBServerInfoPageList(queryEntity);
            IList<DBServerInfoEntity> listServer = new List<DBServerInfoEntity>();
            if (pagerDBServerList != null)
            {
                listServer = pagerDBServerList.ItemList;
                ViewBag.TotalCount = pagerDBServerList.TotalRecords;
            }

            return View(listServer);
        }

        public ActionResult IndexAjax(DBServerInfoQuery queryEntity)
        {
            if (!Request.IsAjaxRequest())
            {
                return Content("{\"result\":\"error\",\"msg\":\"非法访问\"}");
            }
            else
            {
                IDBServerInfoService dbServerService = ServiceContainer.Instance.Container.Resolve<IDBServerInfoService>();
                queryEntity.CurrentPage = queryEntity.CurrentPage + 1;
                WKT.Model.Pager<DBServerInfoEntity> dbServerPagerList = dbServerService.GetDBServerInfoPageList(queryEntity);
                if (dbServerPagerList != null)
                {
                    return Content("{\"result\":\"success\",\"msg\":\"成功\",\"data\":" + JsonConvert.SerializeObject(dbServerPagerList) + "}");
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
        public ActionResult Add(int? DBServerID)
        {
            DBServerInfoEntity dbServerEntity = null;
            ViewBag.IsEdit = false;
            if (DBServerID != null)
            {
                ViewBag.IsEdit = true;
                IDBServerInfoService dbServerService = ServiceContainer.Instance.Container.Resolve<IDBServerInfoService>();
                dbServerEntity = dbServerService.GetDBServerInfo(DBServerID.Value);
                dbServerEntity.Pwd = WKT.Common.Security.DES.Decrypt(dbServerEntity.Pwd);
            }
            return View(dbServerEntity);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAjax(DBServerInfoEntity dbServerEntity)
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
                    IDBServerInfoService dbServerService = ServiceContainer.Instance.Container.Resolve<IDBServerInfoService>();
                    dbServerEntity.Pwd = WKT.Common.Security.DES.Encrypt(dbServerEntity.Pwd);
                    bool flag = dbServerService.AddDBServerInfo(dbServerEntity);
                    if (flag)
                    {
                        msg = "{\"result\":\"success\"}";
                    }
                    else
                    {
                        msg = "{\"result\":\"failure\",\"msg\":\"新增DB Server 失败，请检查\"}";
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
                LogProvider.Instance.Error("新增DB Server失败:" + ex.Message);
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="accountEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditAjax(DBServerInfoEntity dbServerEntity)
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
                    IDBServerInfoService dbServerService = ServiceContainer.Instance.Container.Resolve<IDBServerInfoService>();
                    dbServerEntity.Pwd = WKT.Common.Security.DES.Encrypt(dbServerEntity.Pwd);
                    bool flag = dbServerService.UpdateDBServerInfo(dbServerEntity);
                    if (flag)
                    {
                        msg = "{\"result\":\"success\"}";
                    }
                    else
                    {
                        msg = "{\"result\":\"failure\",\"msg\":\"修改DB Server信息失败，请检查\"}";
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
                LogProvider.Instance.Error("修改DB Server信息失败:" + ex.Message);
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
                    return Content("{\"result\":\"failure\",\"msg\":\"请选择要删除的DB Server\"}");
                }
                string msg = "";
                IDBServerInfoService dbServerService = ServiceContainer.Instance.Container.Resolve<IDBServerInfoService>();
                bool flag = dbServerService.BatchDeleteDBServerInfo(IDAarry);
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
                LogProvider.Instance.Error("删除DBServer失败:" + ex.Message);
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        # endregion
    }
}
