using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using WKT.Log;
using WKT.Common.Data;
using WKT.Common.Security;

namespace Web.Admin.Controllers
{
    public class AuthorDetailController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 作者管理
        /// </summary>
        /// <returns></returns>
        public ActionResult AuthorIndex()
        {
            return View();
        }

        /// <summary>
        /// 专家管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ExpertIndex()
        {
            return View();
        }

        /// <summary>
        /// 专家管理(英文)
        /// </summary>
        /// <returns></returns>
        public ActionResult EnExpertIndex()
        {
            return View();
        }

        /// <summary>
        /// 添加修改作者
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public ActionResult Create(Int64 AuthorID = 0, Byte GroupID = 2)
        {
            AuthorDetailEntity model = GetModel(AuthorID);
            model.AuthorModel.GroupID = GroupID;
            return View(model);
        }

        /// <summary>
        /// 作者完善个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateSelf()
        {
            AuthorDetailEntity model = new AuthorDetailEntity ();
            if(CurAuthor.OldGroupID==1)
                model = GetEditorModel(CurAuthor.AuthorID);
            else
                model = GetModel(CurAuthor.AuthorID);
            model.Emial = CurAuthor.LoginName;
            return View(model);
        }


        /// <summary>
        /// 添加修改专家
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public ActionResult CreateExpert(Int64 AuthorID = 0, Byte GroupID = 3)
        {
            AuthorDetailEntity model = GetExpertModel(AuthorID);
            model.AuthorModel.GroupID = GroupID;
            return View(model);
        }

        /// <summary>
        /// 专家完善个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateSelfExpert()
        {
            AuthorDetailEntity model = GetExpertModel(CurAuthor.AuthorID);
            model.Emial = CurAuthor.LoginName;
            return View(model);
        }
        /// <summary>
        /// 编辑修改个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateSelfEditor()
        {
            AuthorDetailEntity model = GetEditorModel(CurAuthor.AuthorID);
            model.Emial = CurAuthor.LoginName;
            return View(model);
        }

        #region 获取作者Model
        /// <summary>
        /// 获取作者Model
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        private AuthorDetailEntity GetModel(Int64 AuthorID)
        {
            AuthorDetailEntity model = null;
            if (AuthorID > 0)
            {
                AuthorDetailQuery query = new AuthorDetailQuery();
                query.JournalID = CurAuthor.JournalID;
                query.AuthorID = AuthorID;
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                model = service.GetAuthorDetailModel(query);
            }
            if (model == null)
                model = new AuthorDetailEntity();
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            var list = authorService.GetFieldsSet();
            if (list == null)
                list = new List<FieldsSet>();
            list.Insert(0, new FieldsSet { DisplayName = "中文姓名", FieldName = "中文姓名", DBField = "AuthorName", IsShow = true, IsRequire = true });
            model.FieldList = list.Where(p => p.IsShow).ToList();
            if (model.AuthorModel == null)
                model.AuthorModel = new AuthorInfoEntity();
            if (model.ExpertGroupList == null)
                model.ExpertGroupList = new List<ExpertGroupMapEntity>();
            return model;
        } 
        #endregion

        #region 获取专家Model
        /// <summary>
        /// 获取专家Model
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        private AuthorDetailEntity GetExpertModel(Int64 AuthorID)
        {
            AuthorDetailEntity model = null;
            if (AuthorID > 0)
            {
                AuthorDetailQuery query = new AuthorDetailQuery();
                query.JournalID = CurAuthor.JournalID;
                query.AuthorID = AuthorID;
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                model = service.GetAuthorDetailModel(query);
            }
            if (model == null)
                model = new AuthorDetailEntity();
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            var list = authorService.GetExpertFieldsSet();
            if (list == null)
                list = new List<FieldsSet>();
            list.Insert(0, new FieldsSet { DisplayName = "中文姓名", FieldName = "中文姓名", DBField = "AuthorName", IsShow = true, IsRequire = true });
            model.FieldList = list.Where(p => p.IsShow).ToList();
            if (model.AuthorModel == null)
                model.AuthorModel = new AuthorInfoEntity();
            if (model.ExpertGroupList == null)
                model.ExpertGroupList = new List<ExpertGroupMapEntity>();
            return model;
        } 
        #endregion

        #region 获取编辑Model
        /// <summary>
        /// 获取编辑Model
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        private AuthorDetailEntity GetEditorModel(Int64 AuthorID)
        {
            AuthorDetailEntity model = null;
            if (AuthorID > 0)
            {
                AuthorDetailQuery query = new AuthorDetailQuery();
                query.JournalID = CurAuthor.JournalID;
                query.AuthorID = AuthorID;
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                model = service.GetAuthorDetailModel(query);
            }
            if (model == null)
                model = new AuthorDetailEntity();
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            var list = authorService.GetEditorFieldsSet();
            if (list == null)
                list = new List<FieldsSet>();
            list.Insert(0, new FieldsSet { DisplayName = "中文姓名", FieldName = "中文姓名", DBField = "AuthorName", IsShow = true, IsRequire = true });
            model.FieldList = list.Where(p => p.IsShow).ToList();
            if (model.AuthorModel == null)
                model.AuthorModel = new AuthorInfoEntity();
            return model;
        } 
        #endregion

        [HttpPost]
        [AjaxRequest]
        public ActionResult Save(AuthorDetailEntity model)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            model.JournalID = CurAuthor.JournalID;           
            ExecResult result = service.SaveAuthorDetail(model);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult GetAuthorPageList(AuthorDetailQuery query)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.GroupID = 2;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<AuthorDetailEntity> pager = service.GetAuthorDetailPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetExpertPageList(AuthorDetailQuery query)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<AuthorDetailEntity> pager = service.GetAuthorDetailPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult Delete(Int64[] AuthorIDs)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            AuthorDetailQuery query = new AuthorDetailQuery();
            query.JournalID = CurAuthor.JournalID;
            query.AuthorIDs = AuthorIDs;
            ExecResult result = service.DelAuthorDetail(query);
            return Json(new { result = result.result, msg = result.msg });
        }

        /// <summary>
        /// 设置为专家
        /// </summary>
        /// <param name="AuthorIDs"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult SetExpert(Int64[] AuthorIDs)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            AuthorDetailQuery query = new AuthorDetailQuery();
            query.JournalID = CurAuthor.JournalID;
            query.AuthorIDs = AuthorIDs;
            ExecResult result = service.SetAuthorExpert(query);
            return Json(new { result = result.result, msg = result.msg });
        }

        /// <summary>
        /// 取消设置为专家
        /// </summary>
        /// <param name="AuthorIDs"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult CancelExpert(Int64[] AuthorIDs)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            AuthorDetailQuery query = new AuthorDetailQuery();
            query.JournalID = CurAuthor.JournalID;
            query.AuthorIDs = AuthorIDs;
            ExecResult result = service.CancelAuthorExpert(query);
            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult AuthorRenderToExcel(AuthorDetailQuery query, string strDict)
        {
            try
            {               
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                query.JournalID = CurAuthor.JournalID;
                query.GroupID = 2;
                query.OrderStr = "AddDate DESC";
                IList<AuthorDetailEntity> list = service.GetAuthorDetailList(query);
                if (list == null || list.Count <= 0)
                {
                    return Content("没有数据不能导出，请先进行查询！");
                }
                strDict = Server.UrlDecode(strDict);
                JavaScriptSerializer s = new JavaScriptSerializer();
                Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(strDict);
                IDictionary<string, string> dict = ((object[])JsonData.First().Value).Select(p => (Dictionary<string, object>)p).ToDictionary(p => p["key"].ToString(), q => q["value"].ToString());
                //获取学历[Education]数据字典显示名-dict2[list[i].Education].ToString()
                ISiteConfigFacadeService service2 = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                DictValueQuery query2 = new DictValueQuery();
                query2.JournalID = CurAuthor.JournalID;
                query2.DictKey = "Education";
                IDictionary<int, string> dict2 = service2.GetDictValueDcit(query2);
                //获取职称[JobTitle]数据字典显示名-dict3[list[i].JobTitle].ToString()
                DictValueQuery query3 = new DictValueQuery();
                query3.JournalID = CurAuthor.JournalID;
                query3.DictKey = "JobTitle";
                IDictionary<int, string> dict3 = service2.GetDictValueDcit(query3);
                //替换字段内容
                if (dict != null && dict.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].AuthorModel.GroupID == 2)
                            list[i].ReserveField2 = "作者";
                        if (list[i].AuthorModel.Status == 1)
                            list[i].ReserveField3 = "正常";
                        else
                            list[i].ReserveField3 = "已停用";
                        list[i].ReserveField4 = dict2[list[i].Education].ToString();
                        list[i].ReserveField5 = dict3[list[i].JobTitle].ToString();
                    }
                }
                //移除字段
                dict.Remove("Education");
                dict.Remove("JobTitle");
                //开始导出Excel
                RenderToExcel.ExportListToExcel<AuthorDetailEntity>(list, dict
                    , null
                    , "作者信息_导出" + DateTime.Now.ToString("yyyy-MM-dd"), false, "xls");
                return Content("导出成功！");
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出作者信息出现异常：" + ex.Message);
                return Content("导出作者信息异常！");
            }
        }

        [HttpPost]
        public ActionResult ExpertRenderToExcel(AuthorDetailQuery query, string strDict)
        {
            try
            {                
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                query.JournalID = CurAuthor.JournalID;
                query.GroupID = 3;
                IList<AuthorDetailEntity> list = service.GetAuthorDetailList(query);
                if (list == null || list.Count <= 0)
                {
                    return Content("没有数据不能导出，请先进行查询！");
                }
                strDict = Server.UrlDecode(strDict);
                JavaScriptSerializer s = new JavaScriptSerializer();
                Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(strDict);
                IDictionary<string, string> dict = ((object[])JsonData.First().Value).Select(p => (Dictionary<string, object>)p).ToDictionary(p => p["key"].ToString(), q => q["value"].ToString());
                //获取学历[Education]数据字典显示名-dict2[list[i].Education].ToString()
                ISiteConfigFacadeService service2 = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                DictValueQuery query2 = new DictValueQuery();
                query2.JournalID = CurAuthor.JournalID;
                query2.DictKey = "Education";
                IDictionary<int, string> dict2 = service2.GetDictValueDcit(query2);
                //获取职称[JobTitle]数据字典显示名-dict3[list[i].JobTitle].ToString()
                DictValueQuery query3 = new DictValueQuery();
                query3.JournalID = CurAuthor.JournalID;
                query3.DictKey = "JobTitle";
                IDictionary<int, string> dict3 = service2.GetDictValueDcit(query3);
                //替换字段内容
                if (dict != null && dict.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].AuthorModel.GroupID == 2)
                            list[i].ReserveField2 = "作者";
                        if (list[i].AuthorModel.Status == 1)
                            list[i].ReserveField3 = "正常";
                        else
                            list[i].ReserveField3 = "已停用";
                        list[i].ReserveField4 = dict2[list[i].Education].ToString();
                        list[i].ReserveField5 = dict3[list[i].JobTitle].ToString();
                    }
                }
                //开始导出Excel
                RenderToExcel.ExportListToExcel<AuthorDetailEntity>(list, dict
                    , null
                    , "专家信息_导出" + DateTime.Now.ToString("yyyy-MM-dd"), false, "xls");
                return Content("导出成功！");
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出专家信息出现异常：" + ex.Message);
                return Content("导出专家信息异常！");
            }
        }

        #region 专家分组设置
        public ActionResult ExpertGroupMap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetExpertGroupList()
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            DictValueQuery query=new DictValueQuery();
            query.JournalID=JournalID;
            query.DictKey=EnumDictKey.ExpertGroupMap.ToString();
            query.CurrentPage = 1;
            query.PageSize = 1000;
            Pager<DictValueEntity> pager = service.GetDictValuePageList(query);
            if (pager.ItemList != null && pager.ItemList.Count > 0)
                pager.ItemList = pager.ItemList.OrderBy(p => p.ValueID).ToList();
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetExpertListByGroup(Int32 GroupID)
        {
            string html = GetExpertGroupMapHtml(GroupID);
            return Json(new { result = html });
        }

        [HttpPost]
        public ActionResult SaveExpertGroup(ExpertGroupMapQuery query)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            query.JournalID = JournalID;
            if (query.list == null)
                query.list = new List<ExpertGroupMapEntity>();
            ExecResult result = service.SaveExpertGroupMap(query);
            return Json(new { result = result.result, msg = result.msg });
        }

        /// <summary>
        /// 获取显示html
        /// </summary>
        /// <returns></returns>
        private string GetExpertGroupMapHtml(Int32 GroupID)
        {
            StringBuilder strHtml = new StringBuilder();

            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            ExpertGroupMapEntity query = new ExpertGroupMapEntity();
            query.JournalID = JournalID;
            query.ExpertGroupID = GroupID;
            var list = service.GetExpertGroupMapList(query);
            if (list != null && list.Count > 0)
            {
                strHtml.Append("<table>");
                int i = 1;
                foreach (var model in list)
                {
                    if (i == 1)
                        strHtml.Append("<tr>");
                    strHtml.Append("<td style=\"padding-left:10px;height:25px;width:130px;\">");
                    strHtml.Append("<input id=\"chkExpertGroup_")
                     .Append(model.AuthorID)
                     .Append("\" type=\"checkbox\" name=\"ExpertGroup\" value=\"")
                     .Append(model.AuthorID)
                     .Append("\" ");
                    if (model.IsChecked)
                    {
                        strHtml.Append(" checked=\"checked\" ");
                    }
                    strHtml.Append("  key=\"")
                        .Append(model.AuthorName)
                        .Append("\"/><label for=\"chkExpertGroup_")
                        .Append(model.AuthorID)
                        .Append("\" style=\"margin-right:30px;\" ");
                    if (!string.IsNullOrWhiteSpace(model.ResearchTopics))
                    {
                        strHtml.Append(" title=\"研究方向：");
                        strHtml.Append(model.ResearchTopics);
                        strHtml.Append("\"");
                    }
                    strHtml.Append(" >");
                    strHtml.Append(model.AuthorName);
                    strHtml.Append("</label></td>");
                    if (i == 5)
                    {
                        strHtml.Append("</tr>");
                        i = 0;
                    }
                    i++;
                }
                strHtml.Append("</table>");
            }
            return strHtml.ToString();
        }
        #endregion

        #region 专家分组设置(英文)
        public ActionResult EnExpertGroupMap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetEnExpertGroupList()
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            DictValueQuery query = new DictValueQuery();
            query.JournalID = JournalID;
            query.DictKey = EnumDictKey.EnExpertGroupMap.ToString();
            query.CurrentPage = 1;
            query.PageSize = 1000;
            Pager<DictValueEntity> pager = service.GetDictValuePageList(query);
            if (pager.ItemList != null && pager.ItemList.Count > 0)
                pager.ItemList = pager.ItemList.OrderBy(p => p.ValueID).ToList();
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetEnExpertListByGroup(Int32 GroupID)
        {
            string html = GetEnExpertGroupMapHtml(GroupID);
            return Json(new { result = html });
        }

        [HttpPost]
        public ActionResult SaveEnExpertGroup(ExpertGroupMapQuery query)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            query.JournalID = JournalID;
            if (query.list == null)
                query.list = new List<ExpertGroupMapEntity>();
            ExecResult result = service.SaveExpertGroupMap(query);
            return Json(new { result = result.result, msg = result.msg });
        }
        /// <summary>
        /// 获取显示html
        /// </summary>
        /// <returns></returns>
        private string GetEnExpertGroupMapHtml(Int32 GroupID)
        {
            StringBuilder strHtml = new StringBuilder();

            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            ExpertGroupMapEntity query = new ExpertGroupMapEntity();
            query.JournalID = JournalID;
            query.ExpertGroupID = GroupID;
            var list = service.GetExpertGroupMapList(query);
            if (list != null && list.Count > 0)
            {
                strHtml.Append("<table>");
                int i = 1;
                foreach (var model in list)
                {
                    if (i == 1)
                        strHtml.Append("<tr>");
                    strHtml.Append("<td style=\"padding-left:10px;height:25px;width:130px;\">");
                    strHtml.Append("<input id=\"chkExpertGroup_")
                     .Append(model.AuthorID)
                     .Append("\" type=\"checkbox\" name=\"ExpertGroup\" value=\"")
                     .Append(model.AuthorID)
                     .Append("\" ");
                    if (model.IsChecked)
                    {
                        strHtml.Append(" checked=\"checked\" ");
                    }
                    strHtml.Append("  key=\"")
                        .Append(model.AuthorName)
                        .Append("\"/><label for=\"chkExpertGroup_")
                        .Append(model.AuthorID)
                        .Append("\" style=\"margin-right:30px;\" ");
                    if (!string.IsNullOrWhiteSpace(model.ResearchTopics))
                    {
                        strHtml.Append(" title=\"研究方向：");
                        strHtml.Append(model.ResearchTopics);
                        strHtml.Append("\"");
                    }
                    strHtml.Append(" >");
                    strHtml.Append(model.AuthorName);
                    strHtml.Append("</label></td>");
                    if (i == 5)
                    {
                        strHtml.Append("</tr>");
                        i = 0;
                    }
                    i++;
                }
                strHtml.Append("</table>");
            }
            return strHtml.ToString();
        }

        #endregion

    }
}
