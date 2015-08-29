using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Common.Utils;
using WKT.Common.Security;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Config;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace Web.Admin.Controllers
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    public class MenuController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SetExceptionMenu(long RoleID,long AuthorID)
        {
            ViewBag.RoleID = RoleID;
            ViewBag.AuthorID = AuthorID;
            return View();
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="MenuID"></param>
        /// <returns></returns>
        public ActionResult View(long? MenuID)
        {
            MenuEntity menuEntity = new MenuEntity();
            if (MenuID != null)
            {
                MenuQuery query = new MenuQuery();
                query.JournalID = JournalID;
                query.MenuID = MenuID.Value;

                ISiteSystemFacadeService siteSystemService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
                menuEntity = siteSystemService.GetMenu(query);
            }
            return View(menuEntity);
        }

        # region Ajax

        /// <summary>
        /// 获取菜单列表Ajax
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetMenuListAjax()
        {
            MenuQuery menuQuery = new MenuQuery();
            menuQuery.JournalID = JournalID;
            menuQuery.GroupID = CurAuthor.GroupID;
            menuQuery.Status = 1;
            ISiteSystemFacadeService siteSystemService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            return Content(JsonConvert.SerializeObject(siteSystemService.GetTreeNodeList(menuQuery)));
        }

        /// <summary>
        /// 获取菜单列表Ajax,角色权限用
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetMenuAjaxByRole(long RoleID)
        {
            RoleMenuQuery menuRoleQuery = new RoleMenuQuery();
            menuRoleQuery.JournalID = JournalID;
            menuRoleQuery.RoleID = RoleID;
            menuRoleQuery.GroupID = CurAuthor.GroupID;
            ISiteSystemFacadeService siteSystemService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            return Content(JsonConvert.SerializeObject(siteSystemService.GetTreeNodeListHaveRole(menuRoleQuery)));
        }

        /// <summary>
        /// 获取菜单列表Ajax
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetExceptionMenuAjaxByRole(long RoleID,long? AuthorID)
        {
            RoleMenuQuery menuRoleQuery = new RoleMenuQuery();
            menuRoleQuery.JournalID = JournalID;
            menuRoleQuery.RoleID = RoleID;
            menuRoleQuery.GroupID = CurAuthor.GroupID;
            menuRoleQuery.AuthorID = AuthorID;
            ISiteSystemFacadeService siteSystemService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            return Content(JsonConvert.SerializeObject(siteSystemService.GetHaveRightMenuAjaxByRole(menuRoleQuery)));
        }

        /// <summary>
        /// 获取当前用户拥有权限的菜单列表
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetHaveRightMenuAjax()
        {
            RoleMenuQuery menuRoleQuery = new RoleMenuQuery();
            menuRoleQuery.JournalID = JournalID;
            menuRoleQuery.RoleID = CurAuthor.RoleID;
            menuRoleQuery.AuthorID = CurAuthor.AuthorID;
            menuRoleQuery.GroupID = CurAuthor.GroupID;
            menuRoleQuery.IsExpend = CurAuthor.GroupID == 1 ? false : true;
            ISiteSystemFacadeService siteSystemService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            return Content(JsonConvert.SerializeObject(siteSystemService.GetHaveRightMenu(menuRoleQuery)));
        }


        /// <summary>
        /// 获取菜单列表Ajax
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetMenuAjax(byte? Status)
        {
            MenuQuery menuQuery = new MenuQuery();
            menuQuery.JournalID = JournalID;
            menuQuery.GroupID = 0;
            if (Status != null)
            {
                menuQuery.Status = Status.Value;
            }
            ISiteSystemFacadeService siteSystemService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            var result = new { Rows = siteSystemService.GetAllMenuList(menuQuery) };
            return Content(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 修改菜单状态
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult SetMenuStatus(byte Status,long[] IDAarry)
        {
            ExecResult execResult = new ExecResult();
            if (IDAarry != null && IDAarry.Length > 0)
            {
                MenuQuery menuQuery = new MenuQuery();
                menuQuery.JournalID = JournalID;
                menuQuery.Status = Status;
                menuQuery.MenuIDList = IDAarry.ToList<long>();
                ISiteSystemFacadeService siteSystemService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
                execResult = siteSystemService.UpdateMenuStatus(menuQuery);
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "请选择要设置的菜单";
            }
            return Content(JsonConvert.SerializeObject(execResult));
        }

        /// <summary>
        /// 菜单赋权
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult SetMenuRight(long RoleID, long[] IDAarry)
        {
            ExecResult execResult = new ExecResult();
            if (IDAarry != null && IDAarry.Length > 0)
            {
                List<RoleMenuEntity> menuRoleList = new List<RoleMenuEntity>();
                RoleMenuEntity roleMenuEntity = null;
                foreach (long MenuID in IDAarry)
                {
                    roleMenuEntity = new RoleMenuEntity();
                    roleMenuEntity.JournalID = JournalID;
                    roleMenuEntity.RoleID = RoleID;
                    roleMenuEntity.MenuID = MenuID;
                    roleMenuEntity.AddDate = DateTime.Now;
                    menuRoleList.Add(roleMenuEntity);
                }
                ISiteSystemFacadeService siteSystemService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
                execResult = siteSystemService.SetMenuRight(menuRoleList);
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "请选择要赋权的菜单";
            }
            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion
    }
}
