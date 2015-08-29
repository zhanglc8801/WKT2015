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
    public class JournalController : BaseController
    {
        # region 首页

        /// <summary>
        /// 编辑部管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.TotalCount = 0;
            IJournalInfoService journalService = ServiceContainer.Instance.Container.Resolve<IJournalInfoService>();
            JournalInfoQuery queryEntity = new JournalInfoQuery();
            queryEntity.CurrentPage = 1;
            Pager<JournalInfoEntity> pagerJournalList = journalService.GetJournalInfoPageList(queryEntity);
            IList<JournalInfoEntity> listJournal = new List<JournalInfoEntity>();
            if (pagerJournalList != null)
            {
                listJournal = pagerJournalList.ItemList;
                ViewBag.TotalCount = pagerJournalList.TotalRecords;
            }

            return View(listJournal);
        }

        public ActionResult IndexAjax(JournalInfoQuery queryEntity)
        {
            if (!Request.IsAjaxRequest())
            {
                return Content("{\"result\":\"error\",\"msg\":\"非法访问\"}");
            }
            else
            {
                IJournalInfoService journalService = ServiceContainer.Instance.Container.Resolve<IJournalInfoService>();
                queryEntity.CurrentPage = queryEntity.CurrentPage + 1;
                WKT.Model.Pager<JournalInfoEntity> journalList = journalService.GetJournalInfoPageList(queryEntity);
                if (journalList != null)
                {
                    return Content("{\"result\":\"success\",\"msg\":\"成功\",\"data\":" + JsonConvert.SerializeObject(journalList) + "}");
                }
                else
                {
                    return Content("{\"result\":\"error\",\"msg\":\"系统出现异常，请稍后再试\"}");
                }
            }
        }

        # endregion

        # region 添加、修改

        /// <summary>
        /// 添加编辑部
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(int? JournalID)
        {
            JournalInfoEntity journalInfoEntity = null;
            ViewBag.IsEdit = false;
            if (JournalID != null)
            {
                ViewBag.IsEdit = true;
                IJournalInfoService journalService = ServiceContainer.Instance.Container.Resolve<IJournalInfoService>();
                journalInfoEntity = journalService.GetJournalInfo(JournalID.Value);
            }
            return View(journalInfoEntity);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAjax(JournalInfoEntity journalEntity)
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
                    IJournalInfoService journalService = ServiceContainer.Instance.Container.Resolve<IJournalInfoService>();
                    JournalInfoQuery queryEntity = new JournalInfoQuery();
                    queryEntity.JournalName = WKT.Common.Security.SecurityUtils.SafeSqlString(journalEntity.JournalName);
                    IList<JournalInfoEntity> list = journalService.GetJournalInfoList(queryEntity);
                    if (list.Count > 0)
                    {
                        msg = "{\"result\":\"failure\",\"msg\":\"该编辑部已经存在\"}";
                    }
                    else
                    {
                        bool flag = journalService.AddJournalInfo(journalEntity);
                        if (flag)
                        {
                            msg = "{\"result\":\"success\"}";
                        }
                        else
                        {
                            msg = "{\"result\":\"failure\",\"msg\":\"添加失败，请检查\"}";
                        }
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
                LogProvider.Instance.Error("添加编辑部异常:" + ex.Message);
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="accountEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditAjax(JournalInfoEntity journalEntity)
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
                    IJournalInfoService journalService = ServiceContainer.Instance.Container.Resolve<IJournalInfoService>();
                    bool flag = journalService.UpdateJournalInfo(journalEntity);
                    if (flag)
                    {
                        msg = "{\"result\":\"success\"}";
                    }
                    else
                    {
                        msg = "{\"result\":\"failure\",\"msg\":\"修改失败，请检查\"}";
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
                LogProvider.Instance.Error("修改编辑部信息异常:" + ex.Message);
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
        public ActionResult DeleteAjax(long[] IDAarry)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Content("{\"result\":\"failure\",\"msg\":\"非法请求\"}");
                }
                if (IDAarry == null || IDAarry.Length == 0)
                {
                    return Content("{\"result\":\"failure\",\"msg\":\"请选择要删除的编辑部\"}");
                }
                string msg = "";
                IJournalInfoService journalService = ServiceContainer.Instance.Container.Resolve<IJournalInfoService>();
                bool flag = journalService.BatchDeleteJournalInfo(IDAarry);
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
                LogProvider.Instance.Error("删除编辑部异常:" + ex.Message);
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        # endregion

        /// <summary>
        /// 编辑部设置
        /// </summary>
        /// <returns></returns>
        public ActionResult Set()
        {
            return View();
        }
    }
}
