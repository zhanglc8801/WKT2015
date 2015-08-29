using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;
using WKT.Common.Utils;
using WKT.Config;
using Newtonsoft.Json;

namespace Web.Admin.Controllers
{
    public class ExpertController : BaseController
    {
        private const string VERFIYCODEKEY = "WKT_AUTH_VERFIFYCODE";
        /// <summary>
        /// 专家助选页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SelDialog()
        {
            return View();
        }

        # region 获取专家列表

        /// <summary>
        /// 获取当前编辑部成员列表
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetExpertList(AuthorInfoQuery query)
        {
            IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            if (query.GroupID == null) query.GroupID = 0;
            if (query.GroupID.Value == 0)
                query.GroupID = CurAuthor.GroupID;
            else
                query.GroupID = null;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            query.CurrentPage = pageIndex;
            query.PageSize = TypeParse.ToInt(Request.Params["pagesize"], 10);
            Pager<AuthorInfoEntity> pager = service.GetExpertPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        # endregion


        /// <summary>
        /// 英文专家助选页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SelDialogEn()
        {
            return View();
        }

        # region 获取英文专家列表

        /// <summary>
        /// 获取当前编辑部成员列表
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetEnExpertList(AuthorInfoQuery query)
        {
            IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.IsSelEnExpert = true;
            if (query.GroupID == null) query.GroupID = 0;
            if (query.GroupID.Value == 0)
                query.GroupID = CurAuthor.GroupID;
            else
                query.GroupID = null;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            query.CurrentPage = pageIndex;
            query.PageSize = TypeParse.ToInt(Request.Params["pagesize"], 10);
            Pager<AuthorInfoEntity> pager = service.GetExpertPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        # endregion

        /// <summary>
        /// 待审稿件
        /// </summary>
        /// <returns></returns>
        public ActionResult Wait()
        {
            return View();
        }
        public ActionResult EnWait()
        {
            return View();
        }

        public ActionResult EditorWait(int authorID)
        {
            ViewBag.authorID = authorID;

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

        # region 待审稿件 ajax

        [AjaxRequest]
        public ActionResult GetWaitPageList(Int64 authorID=0)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity query = new CirculationEntity();
            query.JournalID = CurAuthor.JournalID;
            query.CurAuthorID =authorID==0?CurAuthor.AuthorID:authorID;
            query.Status = 0;
            query.GroupID =3;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<FlowContribution> pager = service.GetExpertContributionList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [AjaxRequest]
        public ActionResult GetEnWaitPageList(Int64 authorID = 0)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity query = new CirculationEntity();
            query.JournalID = CurAuthor.JournalID;
            query.CurAuthorID = authorID == 0 ? CurAuthor.AuthorID : authorID;
            query.Status = 0;
            query.GroupID = 4;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<FlowContribution> pager = service.GetExpertContributionList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }


        [HttpPost]
        [AjaxRequest]
        public ActionResult ExpertDeledit(CirculationEntity model,Int64 authorID=0)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            model.EnumCStatus = EnumContributionStatus.Reject;
            model.JournalID = CurAuthor.JournalID;
            model.AuthorID =authorID==0?CurAuthor.AuthorID:authorID;
            bool result = service.ExpertDeledit(model);
            if (result)
                return Json(new { result = EnumJsonResult.success.ToString(), msg = "拒审操作成功！" });
            else
                return Json(new { result = EnumJsonResult.failure.ToString(), msg = "拒审操作失败！" });
        }

        # endregion

        /// <summary>
        /// 已审稿件
        /// </summary>
        /// <returns></returns>
        public ActionResult Editlist()
        {
            return View();
        }

        [AjaxRequest]
        public ActionResult GetEditlistPageList()
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity query = new CirculationEntity();
            query.JournalID = CurAuthor.JournalID;
            query.CurAuthorID = CurAuthor.AuthorID;
            query.Status = 1;
            query.GroupID = (byte)EnumMemberGroup.Expert;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<FlowContribution> pager = service.GetExpertContributionList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 拒审稿件
        /// </summary>
        /// <returns></returns>
        public ActionResult Deledit()
        {
            return View();
        }

        [AjaxRequest]
        public ActionResult GetDeleditPageList()
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity query = new CirculationEntity();
            query.JournalID = CurAuthor.JournalID;
            query.CurAuthorID = CurAuthor.AuthorID;
            query.EnumCStatus = EnumContributionStatus.Reject;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<FlowContribution> pager = service.GetSynchroStatusContributionList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 收到信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ReciveSmsIndex()
        {
            return View();
        }


        /// <summary>
        /// 信息内容查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MsgContentView(MessageRecodeQuery query,Int64 RecodeID)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            query.JournalID = CurAuthor.JournalID;
            query.ReciveUser = CurAuthor.AuthorID;
            IList<MessageRecodeEntity> pager = service.GetMessageRecodeList(query);
            if (pager != null)
            {
              IList<MessageRecodeEntity>  messageList=  pager.Where(c => c.RecodeID == RecodeID).ToList<MessageRecodeEntity>();
              if (messageList != null && messageList.Count > 0)
              {
                  MessageRecodeEntity entity=  messageList.FirstOrDefault();
                  ViewBag.content = entity.MsgContent;
              }
            }
            return View();
        }

        /// <summary>
        /// 得到专家字段设置
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult GetFieldsAjax()
        {
            try
            {
                IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                var result = new { Rows = service.GetExpertFieldsSet() };
                return Content(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("获取专家字段设置出现异常：" + ex.Message);
                return Content("获取专家设置出现异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 保存专家字段设置
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
                        execResult = service.SetExpertFields(FieldsArray);
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

        

        
        




    }
}
