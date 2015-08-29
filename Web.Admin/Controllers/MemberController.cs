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
    /// 编辑部成员管理
    /// </summary>
    public class MemberController : BaseController
    {
        /// <summary>
        /// 成员管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        # region 新增编辑部成员

        /// <summary>
        /// 新增编辑部成员
        /// </summary>
        /// <returns></returns>
        public ActionResult AddMember()
        {
            if (Request.UrlReferrer != null)
            {
                if (Request.UrlReferrer.AbsolutePath.Equals(ApplicationPath + "member/",StringComparison.CurrentCultureIgnoreCase))
                {
                    return View();
                }
                else
                {
                    return Content("非法请求");
                }
            }
            else
            {
                return Content("非法请求");
            }
        }

        # region 添加编辑部成员

        /// <summary>
        /// 添加编辑部成员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxRequest]
        public ActionResult AddMemberAjax(AuthorInfoEntity authorEntity)
        {
            ExecResult regResult = new ExecResult();
            authorEntity.JournalID = SiteConfig.SiteID;
            authorEntity.GroupID = (int)EnumMemberGroup.Editor;
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            regResult = authorService.AuthorReg(authorEntity);
            return Content(JsonConvert.SerializeObject(regResult));
        }

        # endregion

        # endregion

        # region 编辑编辑部成员

        /// <summary>
        /// 新增编辑部成员
        /// </summary>
        /// <returns></returns>
        public ActionResult EditMember(long AuthorID)
        {
            if (Request.UrlReferrer != null)
            {
                if (Request.UrlReferrer.AbsolutePath.Equals(ApplicationPath + "member/",StringComparison.CurrentCultureIgnoreCase))
                {
                    IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                    AuthorInfoQuery authorQuery = new AuthorInfoQuery();
                    authorQuery.JournalID = JournalID;
                    authorQuery.AuthorID = AuthorID;
                    AuthorInfoEntity authorEntity = authorService.GetMemberInfo(authorQuery);
                    if (authorEntity == null)
                    {
                        return Content("请选择要编辑的成员");
                    }
                    else
                    {
                        return View(authorEntity);
                    }
                }
                else
                {
                    return Content("非法请求");
                }
            }
            else
            {
                return Content("非法请求null");
            }
        }

        # region 编辑编辑部成员

        /// <summary>
        /// 编辑编辑部成员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxRequest]
        public ActionResult EditMemberAjax(AuthorInfoEntity authorEntity)
        {
            ExecResult regResult = new ExecResult();
            authorEntity.JournalID = SiteConfig.SiteID;
            authorEntity.GroupID = (int)EnumMemberGroup.Editor;
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            regResult = authorService.EditMember(authorEntity);
            return Content(JsonConvert.SerializeObject(regResult));
        }

        # endregion

        # endregion

        # region 删除编辑部成员

        /// <summary>
        /// 删除编辑部成员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxRequest]
        public ActionResult DelMemberAjax(long[] IDAarry)
        {
            ExecResult exeResult = new ExecResult();
            if(IDAarry == null || IDAarry.Length == 0){
                exeResult.msg = "请选择要删除的成员";
                exeResult.result = EnumJsonResult.failure.ToString();
                return Content(JsonConvert.SerializeObject(exeResult));
            }
            AuthorInfoQuery authorQuery = new AuthorInfoQuery();
            authorQuery.JournalID = JournalID;
            authorQuery.AuthorIDList = IDAarry.ToList<long>();

            ISiteSystemFacadeService sysService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            exeResult = sysService.DelMember(authorQuery);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        # endregion

        /// <summary>
        /// 选择窗体
        /// </summary>
        /// <returns></returns>
        public ActionResult SelDialog(Int32 isAll = 0)
        {
            ViewBag.isAll = isAll;
            return View();
        }

        /// <summary>
        /// 编辑工作量统计
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkLoadStat()
        {            
            return View();
        }

        /// <summary>
        /// 编辑工作量统计
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkLoadStatNew()
        {
            return View();
        }

        public ActionResult NewContribute()
        {
            return View();
        }


        # region 编辑部成员

        /// <summary>
        /// 获取当前编辑部成员列表
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetMemberList(AuthorInfoQuery query)
        {
            IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            if (query.GroupID == null) query.GroupID = 0;
            if (query.GroupID.Value == 0)
                query.GroupID = CurAuthor.GroupID;
            else
                query.GroupID = null;
            int pageIndex = TypeParse.ToInt(Request.Params["page"],1);
            query.CurrentPage = pageIndex;
            query.PageSize = TypeParse.ToInt(Request.Params["pagesize"],10);
            Pager<AuthorInfoEntity> pager = service.GetMemberInfoList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 根据角色ID获取成员列表
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetMemberListByRole(long RoleID)
        {
            AuthorInfoQuery queryAuthor = new AuthorInfoQuery();
            queryAuthor.JournalID = JournalID;
            queryAuthor.GroupID = (byte)EnumMemberGroup.Editor;
            queryAuthor.RoleID = RoleID;
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            IList<AuthorInfoEntity> listAuthor = authorService.GetAuthorListByRole(queryAuthor);
            var result = new { Rows = listAuthor };
            return Content(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 添加用户到指定角色
        /// </summary>
        /// <param name="authorEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult AddAurhorRole(long RoleID, long[] IDAarry)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                foreach (long AuthorID in IDAarry)
                {
                    RoleAuthorEntity roleAuthorEntity = new RoleAuthorEntity();
                    roleAuthorEntity.AuthorID = AuthorID;
                    roleAuthorEntity.RoleID = RoleID;
                    roleAuthorEntity.JournalID = JournalID;
                    IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                    execResult = authorService.SetAurhorRole(roleAuthorEntity);
                    if (execResult.result == EnumJsonResult.error.ToString() || execResult.result == EnumJsonResult.failure.ToString())
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "添加用户到当前角色出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("添加用户到当前角色出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="authorEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult DelAurhorRole(long RoleID, long[] IDAarry)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                foreach (long AuthorID in IDAarry)
                {
                    RoleAuthorEntity roleAuthorEntity = new RoleAuthorEntity();
                    roleAuthorEntity.AuthorID = AuthorID;
                    roleAuthorEntity.RoleID = RoleID;
                    roleAuthorEntity.JournalID = JournalID;
                    IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                    execResult = authorService.DelAurhorRole(roleAuthorEntity);
                    if (execResult.result == EnumJsonResult.error.ToString() || execResult.result == EnumJsonResult.failure.ToString())
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "从当前角色移除成员出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("从当前角色移除成员出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        /// <summary>
        /// 添加用户到指定角色
        /// </summary>
        /// <param name="authorEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult SetAurhorException(long RoleID,long[] AuthorIDArray, long[] IDAarry)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                foreach (long AuthorID in AuthorIDArray)
                {
                    AuthorMenuRightExceptionEntity authorMenuRightExceptionEntity = new AuthorMenuRightExceptionEntity();
                    authorMenuRightExceptionEntity.AuthorID = AuthorID;
                    authorMenuRightExceptionEntity.MenuIDList = IDAarry.ToList<long>();
                    authorMenuRightExceptionEntity.JournalID = JournalID;
                    authorMenuRightExceptionEntity.RoleID = RoleID;
                    IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                    execResult = authorService.SetAurhorMenuRightException(authorMenuRightExceptionEntity);
                    if (execResult.result == EnumJsonResult.error.ToString() || execResult.result == EnumJsonResult.failure.ToString())
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "设置用户菜单权限例外时现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("设置用户菜单权限例外时现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion

        # region 修改密码

        /// <summary>
        /// 登录后修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult EditPwd()
        {
            return View();
        }

        [AjaxRequest]
        public ActionResult EditPwdAjax(AuthorInfoEntity authorEntity)
        {
            ExecResult execResult = new ExecResult();

            try
            {
                authorEntity.AuthorID = CurAuthor.AuthorID;
                authorEntity.JournalID = JournalID;
                IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                execResult = authorService.EditPwd(authorEntity);
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "修改密码出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("修改密码出现异常：" + ex.Message);
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion
    }
}
