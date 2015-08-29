using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Common.Xml;
using WKT.Common.Utils;
using WKT.Config;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace Web.Admin.Controllers
{
    /// <summary>
    /// 作者、编辑、专家
    /// </summary>
    public class AuthorController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelDialog()
        {
            return View();
        }

        /// <summary>
        /// 字段设置
        /// </summary>
        /// <returns></returns>
        public ActionResult FieldSet()
        {
            return View();
        }

        # region ajax

        /// <summary>
        /// 获取作者列表
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetAuthorList(AuthorInfoQuery query)
        {
            IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            query.JournalID = JournalID;
            query.GroupID = (int)EnumMemberGroup.Author;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            query.CurrentPage = pageIndex;
            query.PageSize = TypeParse.ToInt(Request.Params["pagesize"], 10);
            Pager<AuthorInfoEntity> pager = service.GetMemberInfoList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 获取作者列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AuthorSearchList(string RealName)
        {
            IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            AuthorInfoQuery query = new AuthorInfoQuery();
            query.RealName = Server.UrlDecode(RealName);
            query.JournalID = JournalID;
            query.GroupID = (int)EnumMemberGroup.Author;
            query.CurrentPage = 1;
            query.PageSize = 10;
            Pager<AuthorInfoEntity> pager = service.GetMemberInfoList(query);
            var filteredItems = pager.ItemList.Select(p => new { RealName=p.RealName,AuthorID=p.AuthorID});
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        

        /// <summary>
        /// 得到字段设置
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult GetFieldsAjax()
        {
            try
            {
                IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                var result = new { Rows = service.GetFieldsSet() };
                return Content(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("获取作者字段设置出现异常：" + ex.Message);
                return Content("获取作者字段设置出现异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 保存字段设置
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult SaveFieldsSetAjax(List<FieldsSet> FieldsArray)
        {
            ExecResult execResult = new ExecResult();
            if (FieldsArray != null)
            {
                if (FieldsArray.Count() > 0)
                {
                    if (string.IsNullOrEmpty(FieldsArray[0].DisplayName))
                    {
                        execResult.result = EnumJsonResult.error.ToString();
                        execResult.msg = "具体的值没有取到";
                    }
                    else
                    {
                        IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                        execResult = service.SetFields(FieldsArray);
                    }
                }
                else
                {
                    execResult.result = EnumJsonResult.error.ToString();
                    execResult.msg = "接收到的参数个数为0";
                }
            }
            else
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "接收到的参数为空";
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion
    }
}
