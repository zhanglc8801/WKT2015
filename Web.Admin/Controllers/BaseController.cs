using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Common.Utils;
using WKT.Common.Security;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Config;
using WKT.Log;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace Web.Admin.Controllers
{
    /// <summary>
    /// 基类
    /// </summary>
    public class BaseController : Controller
    {
        ISiteSystemFacadeService systemFacadeService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();

        public long JournalID
        {
            get {
                return CurAuthor == null ? 0 : CurAuthor.JournalID;
            }
        }

        public AuthorInfoEntity CurAuthor
        {
            get;
            set;
        }

        public string AdminUrl
        {
            get
            {
                string strAppPath = ApplicationPath;
                if(!strAppPath.StartsWith("/"))
                {
                    strAppPath = "/" + strAppPath;
                }
                return "http://" + Request.UserHostName + strAppPath;
            }
        }

        public string ApplicationPath
        {
            get
            {
                if (Request.ApplicationPath.EndsWith("/"))
                {
                    return Request.ApplicationPath;
                }
                else
                {
                    return Request.ApplicationPath + "/"; 
                }
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (TicketTool.IsLogin())
            {
                CurAuthor = JsonConvert.DeserializeObject<AuthorInfoEntity>(TicketTool.GetUserData());
                if (CurAuthor == null)
                {
                    filterContext.HttpContext.Response.Redirect(SiteConfig.RootPath + "/user/login/", true);
                }
                else
                {
                    string cururl = filterContext.HttpContext.Request.Path.ToLower();
                    if (!cururl.EndsWith("/"))
                    {
                        cururl = cururl + "/";
                    }

                    if (CurAuthor.GroupID == (byte)EnumMemberGroup.Editor)
                    {
                        # region check is have access cur url right

                        RoleMenuQuery roleQuery = new RoleMenuQuery();
                        roleQuery.RoleIDList = CurAuthor.RoleIDList;
                        roleQuery.Url = cururl;
                        roleQuery.JournalID = JournalID;
                        if (!systemFacadeService.IsHaveAccessRight(roleQuery))
                        {
                            filterContext.HttpContext.Response.Redirect("/?url=" + filterContext.HttpContext.Server.UrlEncode("/home/noright"), true);
                        }
                        CurAuthor.RoleIDList.Add(2);
                        CurAuthor.RoleIDList.Add(3);
                        # endregion

                        ViewBag.SiteTitle = "稿件管理平台";
                    }
                    else if (CurAuthor.GroupID == (byte)EnumMemberGroup.Expert || CurAuthor.GroupID == (byte)EnumMemberGroup.EnExpert)
                    {
                        # region check is have access cur url right

                        RoleMenuQuery roleQuery = new RoleMenuQuery();
                        roleQuery.GroupID = CurAuthor.GroupID;
                        roleQuery.Url = cururl;
                        roleQuery.JournalID = JournalID;
                        if (!systemFacadeService.IsHaveAccessRightByGroup(roleQuery))
                        {
                            filterContext.HttpContext.Response.Redirect("/?url=" + filterContext.HttpContext.Server.UrlEncode("/home/noright"), true);
                        }

                        # endregion
                        CurAuthor.RoleIDList.Add(2);
                        ViewBag.SiteTitle = "稿件专家处理平台";
                    }
                    else
                    {
                        # region check is have access cur url right

                        RoleMenuQuery roleQuery = new RoleMenuQuery();
                        roleQuery.GroupID = CurAuthor.GroupID;
                        roleQuery.Url = cururl;
                        roleQuery.JournalID = JournalID;
                        if (!systemFacadeService.IsHaveAccessRightByGroup(roleQuery))
                        {
                            filterContext.HttpContext.Response.Redirect("/?url=" + filterContext.HttpContext.Server.UrlEncode("/home/noright"), true);
                        }

                        # endregion

                        if(CurAuthor.OldGroupID==1 || CurAuthor.OldGroupID==3)
                            CurAuthor.RoleIDList.Add(3);

                        ViewBag.SiteTitle = "作者投稿管理平台";
                    }
                    
                    ViewBag.CurUserName = CurAuthor.RealName;
                }
            }
            else
            {
                filterContext.HttpContext.Response.Redirect( SiteConfig.RootPath + "/user/login/", true);
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            LogProvider.Instance.Error("[Controller:" + controllerName + ",ActionName:" + actionName + "]" + filterContext.Exception.ToString());
            // 执行基类中的OnException
            base.OnException(filterContext);
        }

        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetUploadPath(string path)
        {
            string folder = SiteConfig.UploadPath;
            string uploadPath;
            if (!string.IsNullOrWhiteSpace(folder))
            {
                uploadPath = folder + path;
            }
            else
            {
                uploadPath = Server.MapPath(path);
            }
            return uploadPath;
        }
    }
}
