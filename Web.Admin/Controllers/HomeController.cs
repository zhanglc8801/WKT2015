using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Log;
using WKT.Config;
using WKT.Common;
using WKT.Common.Security;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace Web.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            string url = Request.QueryString["url"];
            ViewBag.RETURL = url;
            ViewBag.IsShowMoreRole = false;
            if (CurAuthor != null)
            {
                if (CurAuthor.RoleIDList != null)
                {
                    if (CurAuthor.GroupID == (Byte)EnumMemberGroup.Editor && CurAuthor.RoleIDList.Count > 1)
                    {
                        ViewBag.RoleList = SelectRole("selRoleList", "65px;");
                        ViewBag.IsShowMoreRole = true;
                    }
                    if (CurAuthor.GroupID == (Byte)EnumMemberGroup.Expert && CurAuthor.RoleIDList.Count > 1)
                    {
                        ViewBag.RoleList = SelectExpertRole("selRoleList", "80px;");
                        ViewBag.IsShowMoreRole = true;
                    }

                    if (CurAuthor.GroupID == (Byte)EnumMemberGroup.Author && CurAuthor.RoleIDList.Count > 1)
                    {
                        ViewBag.RoleList = SelectAuthorRole("selRoleList", "80px;");
                        ViewBag.IsShowMoreRole = true;
                    }
                }
                ViewBag.Group = CurAuthor.GroupID;
                ViewBag.Author = CurAuthor.AuthorID;


                ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                DictValueQuery query = new DictValueQuery();
                query.JournalID = SiteConfig.SiteID;
                query.DictKey = "NotAccessSearch";
                IDictionary<int, string> dict = service.GetDictValueDcit(query);
                if (dict != null && dict.Count > 0)
                {
                    foreach (var item in dict)
                    {
                        if (item.Value == CurAuthor.AuthorID.ToString())
                        {
                            ViewBag.IsShowSearch = false;
                            break;
                        }
                        else
                        {
                            ViewBag.IsShowSearch = true;
                        }
                    }
                }

            }
            


            //ISiteConfigFacadeService facadeService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            //DictValueQuery dictQuery = new DictValueQuery();
            //dictQuery.JournalID = JournalID;
            //dictQuery.DictKey = "NotAccessSearch";
            //IList<DictValueEntity> dictList = facadeService.GetDictValueList(dictQuery);

            //文海峰 2014-1-8
            IFlowFacadeService siteConfigService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            IList<FlowStatusEntity> flowStatuslist = siteConfigService.GetFlowStatusList(new FlowStatusQuery() { JournalID = JournalID });
            ViewBag.flowStatuslist = flowStatuslist;
            return View();
        }

        # region get cur role

        /// <summary>
        /// 角色下拉框控件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private string SelectRole(string id, string width)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2};border:0px;\" class=\"input-select\">", id, id, width);
            RoleInfoQuery roleQuery = new RoleInfoQuery();
            roleQuery.JournalID = SiteConfig.SiteID;
            roleQuery.GroupID = (int)EnumMemberGroup.Editor;
            ISiteSystemFacadeService sysService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            IList<RoleInfoEntity> listRole = sysService.GetRoleList(roleQuery);

            foreach (RoleInfoEntity item in listRole)
            {
                // 当前用户的角色
                if (CurAuthor.RoleIDList.Contains(item.RoleID))
                {
                    if (item.RoleID == CurAuthor.RoleID)
                    {
                        sb.AppendFormat("<option value=\"{0}\" selected>{1}</option>", item.RoleID.ToString(), item.RoleName);
                    }
                    else
                    {
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.RoleID.ToString(), item.RoleName);
                    }
                }
            }
            sb.AppendFormat("<option value=\"{0}\">{1}</option>", 2, "投稿作者");
            sb.AppendFormat("<option value=\"{0}\">{1}</option>", 3, "审稿专家");
            sb.Append("</select>");
            return sb.ToString();
        }

        private string SelectExpertRole(string id, string width)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2};border:0px;\" class=\"input-select\">", id, id, width);
            RoleInfoQuery roleQuery = new RoleInfoQuery();
            roleQuery.JournalID = SiteConfig.SiteID;
            roleQuery.GroupID = (int)EnumMemberGroup.Editor;
            ISiteSystemFacadeService sysService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            IList<RoleInfoEntity> listRole = sysService.GetRoleList(roleQuery);

            sb.AppendFormat("<option value=\"{0}\">{1}</option>", 3, "审稿专家");
            foreach (RoleInfoEntity item in listRole)
            {
                // 当前用户的角色
                if (CurAuthor.RoleIDList.Contains(item.RoleID))
                {
                    if (item.RoleID == CurAuthor.RoleID)
                    {
                        sb.AppendFormat("<option value=\"{0}\" selected>{1}</option>", item.RoleID.ToString(), item.RoleName);
                    }
                    else
                    {

                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.RoleID.ToString(), item.RoleName);
                    }
                }
            }
            sb.AppendFormat("<option value=\"{0}\">{1}</option>", 2, "投稿作者");
            sb.Append("</select>");
            return sb.ToString();
        }

        private string SelectAuthorRole(string id, string width)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2};border:0px;\" class=\"input-select\">", id, id, width);
            RoleInfoQuery roleQuery = new RoleInfoQuery();
            roleQuery.JournalID = SiteConfig.SiteID;
            roleQuery.GroupID = (int)EnumMemberGroup.Editor;
            ISiteSystemFacadeService sysService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            IList<RoleInfoEntity> listRole = sysService.GetRoleList(roleQuery);

            sb.AppendFormat("<option value=\"{0}\">{1}</option>", 2, "投稿作者");
            foreach (RoleInfoEntity item in listRole)
            {
                // 当前用户的角色
                if (CurAuthor.RoleIDList.Contains(item.RoleID))
                {
                    if (item.RoleID == CurAuthor.RoleID)
                    {
                        sb.AppendFormat("<option value=\"{0}\" selected>{1}</option>", item.RoleID.ToString(), item.RoleName);
                    }
                    else
                    {

                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.RoleID.ToString(), item.RoleName);
                    }
                }
            }
            sb.AppendFormat("<option value=\"{0}\">{1}</option>", 3, "审稿专家");
            sb.Append("</select>");
            return sb.ToString();
        }

        # endregion

        # region ajax

        [AjaxRequest]
        [HttpPost]
        public ActionResult ChanageCurRole(long RoleID)
        {
            ExecResult exeResult = new ExecResult();
            try
            {
                if (CurAuthor.RoleIDList.Contains(RoleID))
                {
                    AuthorInfoEntity curAuthorEntity = JsonConvert.DeserializeObject<AuthorInfoEntity>(TicketTool.GetUserData());
                    curAuthorEntity.RoleID = RoleID;
                    if (RoleID == 3)
                        curAuthorEntity.GroupID = 3;
                    else if (RoleID == 2)
                        curAuthorEntity.GroupID = 2;
                    else
                        curAuthorEntity.GroupID = 1;
                    TicketTool.UpdateCookie(curAuthorEntity.AuthorID.ToString(), JsonConvert.SerializeObject(curAuthorEntity));
                    exeResult.result = EnumJsonResult.success.ToString();
                }
                else
                {
                    exeResult.result = EnumJsonResult.failure.ToString();
                    exeResult.msg = "您没有指定的角色权限";
                }
            }
            catch (Exception ex)
            {
                exeResult.result = EnumJsonResult.error.ToString();
                exeResult.msg = "修改当前角色异常:" + ex.Message;
                LogProvider.Instance.Error("修改当前角色异常：" + ex.Message);
            }
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        # endregion

        public ActionResult NoRight()
        {
            return View();
        }

        public ActionResult LoginAjax()
        {
            return Content("");
        }
        public ActionResult Welcome()
        {
            string qq = SiteConfig.QQ;
            ViewBag.qq = qq;
            ViewBag.Group = CurAuthor.GroupID;
            SiteConfigQuery query = new SiteConfigQuery();
            query.JournalID = CurAuthor.JournalID;
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigEntity model = service.GetSiteConfigModel(query);
            if (model == null)
                model = new SiteConfigEntity();
            return View(model);
        }

        public ActionResult HomePage()
        {

            return View();
        }

        public ActionResult HomePageForAuthor()
        {
            return View();
        }

        public ActionResult HomePageForExpert()
        {
            return View();
        }


    }
}
