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
using System.Web.Script.Serialization;
using WKT.Common.Data;
using WKT.Log;

namespace HanFang360.InterfaceService.Controllers
{
    /// <summary>
    /// 专家统计
    /// </summary>
    public class ExpertStatController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult StatDetail(int IsHandled, int StatusID, long AuthorID)
        {
            ViewBag.IsHandled = IsHandled;
            ViewBag.StatusID = StatusID;
            ViewBag.AuthorID = AuthorID;
            ViewBag.StartDate = Request["StartDate"];
            ViewBag.EndDate = Request["EndDate"];
            return View();
        }


        [HttpPost]
        public JsonResult ExpertPayMoneyRenderToExcel(WorkloadQuery query, string strDict)
        {
            try
            {
                IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                IAuthorPlatformFacadeService serviceAuthorPlatform = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();

                query.JournalID = CurAuthor.JournalID;
                query.GroupID = query.GroupID;
                IList<WorkloadEntity> list = service.GetWorkloadList(query);
                IList<AuthorDetailEntity> listDetail = serviceAuthorPlatform.GetAuthorDetailList(new AuthorDetailQuery() { GroupID = 3, JournalID = JournalID });
                list = (from author in list join authorDetail in listDetail on author.AuthorID equals authorDetail.AuthorID into JionauthorDetail from authorDetail in JionauthorDetail.DefaultIfEmpty() select new WorkloadEntity() { RealName = author.RealName, LoginName = author.LoginName, AlreadyCount = author.AlreadyCount, WaitCount = author.WaitCount, ZipCode = authorDetail.ZipCode, Address = authorDetail.Address }).ToList<WorkloadEntity>();

                if (list == null || list.Count <= 0)
                {
                    return Json(new { flag = "error", msg = "没有数据不能导出，请先进行查询！" });
                }
                strDict = Server.UrlDecode(strDict);
                JavaScriptSerializer s = new JavaScriptSerializer();
                Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(strDict);
                IDictionary<string, string> dict = ((object[])JsonData.First().Value).Select(p => (Dictionary<string, object>)p).ToDictionary(p => p["key"].ToString(), q => q["value"].ToString());
                RenderToExcel.ExportListToExcel<WorkloadEntity>(list, dict
                    , null
                    , "专家审稿费信息_导出" + DateTime.Now.ToString("yyyy-MM-dd"), false, "xls");
                return Json(new { flag="success"});
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出作者信息出现异常：" + ex.Message);
                return Json(new { flag = "error", msg = "导出作者信息异常！" });
            }
        }


        /// <summary>
        /// 获取当前人可以处理的稿件状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetNewEditorWorkloadList()
        {
            IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            AuthorInfoQuery query = new AuthorInfoQuery();
            query.JournalID = CurAuthor.JournalID;
            query.GroupID = (int)EnumMemberGroup.Editor;
            query.RoleID = 0;
            IList<AuthorInfoEntity> pager = service.GetAuthorListByRole(query);
            Dictionary<string, IList<FlowStatusEntity>> dic = new Dictionary<string, IList<FlowStatusEntity>>();
            if (pager != null  && pager.Count>0)
            {
                foreach (var item in pager)
                {
                    AuthorInfoQuery currentEntity=new AuthorInfoQuery();
                    currentEntity.AuthorID = item.AuthorID;
                    IList<FlowStatusEntity> list= GetFlowStatusList(currentEntity);
                    dic.Add(item.RealName + "-" + item.AuthorID + "-" + item.RoleID + "-" + item.RoleName, list);
                }
            }
            return Json(dic);
        }

        private IList<FlowStatusEntity> GetFlowStatusList(AuthorInfoQuery author)
        {
            JsonExecResult<FlowStatusEntity> jsonResult = new JsonExecResult<FlowStatusEntity>();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            FlowStatusQuery query = new FlowStatusQuery();
            query.JournalID = JournalID;
            query.CurAuthorID = (long)author.AuthorID;
            query.RoleID = (long)CurAuthor.RoleID;
            query.IsHandled = 2;
            try
            {
                jsonResult.ItemList = service.GetHaveRightFlowStatus(query);
                if (jsonResult.ItemList != null)
                {
                    jsonResult.ItemList = jsonResult.ItemList.Where(p => p.ContributionCount > 0).ToList<FlowStatusEntity>();
                }
                jsonResult.result = EnumJsonResult.success.ToString();
            }
            catch (Exception ex)
            {
                jsonResult.result = EnumJsonResult.error.ToString();
                jsonResult.msg = "获取当前人可以处理的稿件状态出现异常:" + ex.Message;
            }
            return jsonResult.ItemList;
        }

        /// <summary>
        /// 获取编辑工作量统计列表
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetWorkloadList(WorkloadQuery query)
        {
            IRoleAuthorFacadeService roleAuthorService = ServiceContainer.Instance.Container.Resolve<IRoleAuthorFacadeService>();
            RoleAuthorQuery roleAuthorQuery = new RoleAuthorQuery();
            roleAuthorQuery.JournalID = JournalID;
            roleAuthorQuery.GroupID = query.GroupID;
            roleAuthorQuery.OrderStr = " AI.AuthorID ASC";
            roleAuthorQuery.RealName = query.RealName;
            if(query.RoleID>0)
                roleAuthorQuery.RoleID = query.RoleID;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            IList<RoleAuthorEntity> listRoleAuthor;
            if (query.isStatByGroup == true)
            {
                listRoleAuthor = roleAuthorService.GetRoleAuthorDetailList(roleAuthorQuery);
            }
            else
            {
                listRoleAuthor = roleAuthorService.GetRoleAuthorDetailList(roleAuthorQuery).Distinct(new Compare<RoleAuthorEntity>((x, y) => (null != x && null != y) && (x.AuthorID == y.AuthorID))).ToList();
            }
            //去除固定的不需统计的人员ID
            listRoleAuthor.Where(p => p.AuthorID != 84381 && p.AuthorID != 84386 && p.AuthorID != 84388 && p.AuthorID != 95844);
            return Json(new { Rows = listRoleAuthor.Skip((pageIndex - 1) * 50).Take(50), Total = listRoleAuthor.Count });
        }

        /// <summary>
        /// 导出编辑部工作量列表到Excel
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WorkloadListToExcel(WorkloadQuery query)
        {
            IRoleAuthorFacadeService roleAuthorService = ServiceContainer.Instance.Container.Resolve<IRoleAuthorFacadeService>();
            RoleAuthorQuery roleAuthorQuery = new RoleAuthorQuery();
            roleAuthorQuery.JournalID = JournalID;
            roleAuthorQuery.GroupID = 1;
            roleAuthorQuery.OrderStr = " AI.AuthorID ASC";
            roleAuthorQuery.RealName = query.RealName;
            if (query.RoleID > 0)
                roleAuthorQuery.RoleID = query.RoleID;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            IList<RoleAuthorEntity> listRoleAuthor = roleAuthorService.GetRoleAuthorDetailList(roleAuthorQuery);      
            List<WorkloadEntity> list=new List<WorkloadEntity> ();
            for (int i = 0; i < listRoleAuthor.Count; i++)
            {
                WorkloadEntity workloadEntity = new WorkloadEntity();
                FlowStatusQuery fquery = new FlowStatusQuery();
                JsonExecResult<FlowStatusEntity> jsonResult0 = new JsonExecResult<FlowStatusEntity>();
                JsonExecResult<FlowStatusEntity> jsonResult1 = new JsonExecResult<FlowStatusEntity>();
                IFlowFacadeService fservice = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                fquery.JournalID = JournalID;
                fquery.CurAuthorID=listRoleAuthor[i].AuthorID;
                fquery.RoleID=listRoleAuthor[i].RoleID;
                fquery.StartDate = query.StartDate;
                fquery.EndDate = query.EndDate;
                #region 待处理
                try
                {
                    fquery.IsHandled = 0;//0=待处理
                    jsonResult0.ItemList = fservice.GetHaveRightFlowStatusForStat(fquery);
                    if (jsonResult0.ItemList != null && jsonResult0.ItemList.Count>0)
                    {
                        jsonResult0.ItemList = jsonResult0.ItemList.Where(p => p.ContributionCount > 0).ToList<FlowStatusEntity>();
                        for (int m = 0; m < jsonResult0.ItemList.Count; m++)
                        {
                            workloadEntity.WaitCount += jsonResult0.ItemList[m].ContributionCount;
                        }
                    }

                }
                catch (Exception ex)
                {
                    jsonResult0.result = EnumJsonResult.error.ToString();
                    jsonResult0.msg = "获取当前人可以处理的稿件状态出现异常:" + ex.Message;
                }
                #endregion

                #region 已处理
                try
                {
                    fquery.IsHandled = 1;//1=已处理
                    jsonResult1.ItemList = fservice.GetHaveRightFlowStatusForStat(fquery);
                    if (jsonResult1.ItemList != null && jsonResult1.ItemList.Count>0)
                    {
                        jsonResult1.ItemList = jsonResult1.ItemList.Where(p => p.ContributionCount > 0).ToList<FlowStatusEntity>();
                        for (int n = 0; n < jsonResult1.ItemList.Count; n++)
                        {
                            workloadEntity.WorkCount += jsonResult1.ItemList[n].ContributionCount;
                        }
                    }
                }
                catch (Exception ex)
                {
                    jsonResult1.result = EnumJsonResult.error.ToString();
                    jsonResult1.msg = "获取当前人可以处理的稿件状态出现异常:" + ex.Message;
                }
                #endregion

                workloadEntity.AuthorID = listRoleAuthor[i].AuthorID;
                workloadEntity.RealName = listRoleAuthor[i].RealName;
                if(workloadEntity.WorkCount>0)
                    list.Add(workloadEntity);
                
            }
            //去除固定的不需统计的人员ID
            list.Where(p => p.AuthorID != 84381 && p.AuthorID != 84386 && p.AuthorID != 84388 && p.AuthorID != 95844);
            //去除List中的重复项
            List<WorkloadEntity> WorkloadList = list.Distinct(new Compare<WorkloadEntity>((x, y) => (null != x && null != y) && (x.AuthorID == y.AuthorID))).ToList();
            
            string[] titleFiles = new string[] { "编辑姓名", "已处理", "待处理" };
            int[] titleWidth = new int[] { 80, 80, 80};
            string[] dataFiles = new string[] { "RealName", "WorkCount", "WaitCount" };
            string[] fomateFiles = new string[] { "", "", "" };
            string strTempPath = "/UploadFile/TempFile/" + "WorkloadListAll.xls";
            ExcelHelperEx.CreateExcel<WorkloadEntity>("编辑部工作量统计", titleFiles, titleWidth, dataFiles, fomateFiles, WorkloadList, strTempPath);
            return Json(new { flag = 1, ExcelPath = strTempPath });

        }

        #region 去除List中的重复项
        public delegate bool EqualsComparer<T>(T x, T y);
        public class Compare<T> : IEqualityComparer<T>
        {
            private EqualsComparer<T> _equalsComparer;
            public Compare(EqualsComparer<T> equalsComparer)
            {
                this._equalsComparer = equalsComparer;
            }
            public bool Equals(T x, T y)
            {
                if (null != this._equalsComparer)
                    return this._equalsComparer(x, y);
                else
                    return false;
            }
            public int GetHashCode(T obj)
            {
                return obj.ToString().GetHashCode();
            }
        } 
        #endregion


        /// <summary>
        /// 根据编辑ID获取工作量
        /// </summary>
        /// <param name="fquery"></param>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult GetWorkloadListForAuthorID(FlowStatusQuery fquery)
        {
            JsonExecResult<FlowStatusEntity> jsonResult0 = new JsonExecResult<FlowStatusEntity>();
            JsonExecResult<FlowStatusEntity> jsonResult1 = new JsonExecResult<FlowStatusEntity>();
            IFlowFacadeService fservice = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            fquery.JournalID = JournalID;            

            #region 待处理
            try
            {
                fquery.IsHandled = 0;//0=待处理
                jsonResult0.ItemList = fservice.GetHaveRightFlowStatusForStat(fquery);
                if (jsonResult0.ItemList != null)
                {
                    jsonResult0.ItemList = jsonResult0.ItemList.Where(p => p.ContributionCount > 0).ToList<FlowStatusEntity>();
                }
                jsonResult0.result = EnumJsonResult.success.ToString();

            }
            catch (Exception ex)
            {
                jsonResult0.result = EnumJsonResult.error.ToString();
                jsonResult0.msg = "获取当前人可以处理的稿件状态出现异常:" + ex.Message;
            } 
            #endregion

            #region 已处理
            try
            {
                fquery.IsHandled = 1;//1=已处理
                jsonResult1.ItemList = fservice.GetHaveRightFlowStatusForStat(fquery);
                if (jsonResult1.ItemList != null)
                {
                    jsonResult1.ItemList = jsonResult1.ItemList.Where(p => p.ContributionCount > 0).ToList<FlowStatusEntity>();
                }
                jsonResult1.result = EnumJsonResult.success.ToString();
            }
            catch (Exception ex)
            {
                jsonResult1.result = EnumJsonResult.error.ToString();
                jsonResult1.msg = "获取当前人可以处理的稿件状态出现异常:" + ex.Message;
            } 
            #endregion

            return Json(new { Rows0 = jsonResult0.ItemList, Rows1 = jsonResult1.ItemList });
        }

        /// <summary>
        /// 导出某个编辑的工作量列表到Excel
        /// </summary>
        /// <param name="fquery"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WorkloadListForEditorToExcel(FlowStatusQuery fquery)
        {
            JsonExecResult<FlowStatusEntity> jsonResult0 = new JsonExecResult<FlowStatusEntity>();
            JsonExecResult<FlowStatusEntity> jsonResult1 = new JsonExecResult<FlowStatusEntity>();
            IFlowFacadeService fservice = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            fquery.JournalID = JournalID;

            
            List<WorkloadEntity> list = new List<WorkloadEntity>();

            #region 已处理
            try
            {
                fquery.IsHandled = 1;//1=已处理
                jsonResult1.ItemList = fservice.GetHaveRightFlowStatusForStat(fquery);
                if (jsonResult1.ItemList != null)
                {
                    int WorkCount=0;
                    jsonResult1.ItemList = jsonResult1.ItemList.Where(p => p.ContributionCount > 0).ToList<FlowStatusEntity>();
                    for (int n = 0; n < jsonResult1.ItemList.Count; n++)
                    {
                        WorkloadEntity workloadEntity = new WorkloadEntity();
                        workloadEntity.StatusName = jsonResult1.ItemList[n].StatusName;
                        workloadEntity.AlreadyCount = jsonResult1.ItemList[n].ContributionCount;
                        WorkCount += jsonResult1.ItemList[n].ContributionCount;
                        workloadEntity.RealName = Request.Params["RealName"];
                        list.Add(workloadEntity);
                    }
                    WorkloadEntity workloadEntityCount = new WorkloadEntity();
                    workloadEntityCount.StatusName = "已处理总计：";
                    workloadEntityCount.AlreadyCount = WorkCount;
                    list.Add(workloadEntityCount);
                }
            }
            catch (Exception ex)
            {
                jsonResult1.result = EnumJsonResult.error.ToString();
                jsonResult1.msg = "获取当前人可以处理的稿件状态出现异常:" + ex.Message;
            }
            #endregion

            #region 待处理
            try
            {
                fquery.IsHandled = 0;//0=待处理
                jsonResult0.ItemList = fservice.GetHaveRightFlowStatusForStat(fquery);
                if (jsonResult0.ItemList != null)
                {
                    int WaitCount = 0;
                    jsonResult0.ItemList = jsonResult0.ItemList.Where(p => p.ContributionCount > 0).ToList<FlowStatusEntity>();
                    for (int m = 0; m < jsonResult0.ItemList.Count; m++)
                    {
                        WorkloadEntity workloadEntity = new WorkloadEntity();
                        workloadEntity.StatusName=jsonResult0.ItemList[m].StatusName;
                        workloadEntity.AlreadyCount = jsonResult0.ItemList[m].ContributionCount;
                        WaitCount += jsonResult0.ItemList[m].ContributionCount;
                        workloadEntity.RealName = Request.Params["RealName"];
                        list.Add(workloadEntity);
                    }
                    WorkloadEntity workloadEntityCount = new WorkloadEntity();
                    workloadEntityCount.StatusName = "待处理总计：";
                    workloadEntityCount.AlreadyCount = WaitCount;
                    list.Add(workloadEntityCount);
                }

            }
            catch (Exception ex)
            {
                jsonResult0.result = EnumJsonResult.error.ToString();
                jsonResult0.msg = "获取当前人可以处理的稿件状态出现异常:" + ex.Message;
            }
            #endregion

            string[] titleFiles = new string[] { "编辑姓名","稿件状态", "数量" };
            int[] titleWidth = new int[] { 80, 80, 80 };
            string[] dataFiles = new string[] { "RealName","StatusName", "AlreadyCount" };
            string[] fomateFiles = new string[] { "", "", "" };
            string strTempPath = "/UploadFile/TempFile/" + "WorkloadListForEditor.xls";
            ExcelHelperEx.CreateExcel<WorkloadEntity>("编辑(" + Request.Params["RealName"] + ")工作量统计", titleFiles, titleWidth, dataFiles, fomateFiles, list, strTempPath);

            return Json(new { flag = 1, ExcelPath = strTempPath });

        }

        /// <summary>
        /// 获取专家工作量统计列表
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        [HttpPost]
        public ActionResult GetExpertWorkloadList(WorkloadQuery query)
        {
            decimal totalMoney = 0;
            IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            query.JournalID = JournalID;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            int pageSize = TypeParse.ToInt(Request.Params["pageSize"], 15);
            IList<WorkloadEntity> listAuthor = service.GetWorkloadList(query);
            for (int i = 0; i < listAuthor.Count;i++ )
            {
                listAuthor[i].ExpertReviewFee = (SiteConfig.ExpertReviewFee * listAuthor[i].AlreadyCount).ToString("C2");
                totalMoney += SiteConfig.ExpertReviewFee * listAuthor[i].AlreadyCount;
            }
            if(query.isIgnoreNoWork==true)
            {
                listAuthor=listAuthor.Where(p=>p.AlreadyCount>0).OrderByDescending(x=>x.AlreadyCount).ToList();
                return Json(new { Rows = listAuthor.Where(p => p.AlreadyCount > 0).Skip((pageIndex - 1) * pageSize).Take(pageSize), Total = listAuthor.Count, TotalMoney = totalMoney.ToString("C2") });
            } 
            else
                return Json(new { Rows = listAuthor.Skip((pageIndex - 1) * pageSize).Take(pageSize), Total = listAuthor.Count, TotalMoney = totalMoney.ToString("C2") });
        }

        /// <summary>
        /// 导出专家信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExpertWorkloadListToExcel(WorkloadQuery query)
        {
            decimal totalMoney = 0;
            decimal money = Convert.ToDecimal(Request.Params["Money"]);
            IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            query.JournalID = JournalID;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            IList<WorkloadEntity> listAuthor = service.GetWorkloadList(query);
            for (int i = 0; i < listAuthor.Count; i++)
            {
                listAuthor[i].ExpertReviewFee = (SiteConfig.ExpertReviewFee * listAuthor[i].AlreadyCount).ToString("C2");
                totalMoney += SiteConfig.ExpertReviewFee * listAuthor[i].AlreadyCount;
            }

            string[] titleFiles = new string[] { "专家姓名", "登录名", "待审稿件", "已审稿件", "审稿费", "发票抬头", "户名","帐号","开户行","专家单位", "专家地址", "邮编", "联系电话", "手机" };
            int[] titleWidth = new int[] { 50, 100, 50,50,80,100,50,150,100,120,150,50,100,80 };
            string[] dataFiles = new string[] { "RealName", "LoginName", "WaitCount", "AlreadyCount", "ExpertReviewFee", "InvoiceUnit", "ReserveField1", "ReserveField2", "ReserveField3", "WorkUnit", "Address", "ZipCode", "Tel", "Mobile" };
            string[] fomateFiles = new string[] {"", "", "", "", "", "", "", "", "","","","","",""};
            string strTempPath = "/UploadFile/TempFile/" + "ExpertWorkloadList.xls";        
            if (query.isIgnoreNoWork == true)
            {
                listAuthor = listAuthor.Where(p => p.AlreadyCount > 0).OrderByDescending(x => x.AlreadyCount).ToList();
                ExcelHelperEx.CreateExcel<WorkloadEntity>("专家信息列表_审稿费每篇" + money + "元,总计_" + totalMoney+"元", titleFiles, titleWidth, dataFiles, fomateFiles, listAuthor, strTempPath);
            }
            else
                ExcelHelperEx.CreateExcel<WorkloadEntity>("专家信息列表_审稿费每篇" + money + "元,总计_" + totalMoney + "元", titleFiles, titleWidth, dataFiles, fomateFiles, listAuthor, strTempPath);
            return Json(new { flag = 1, ExcelPath = strTempPath });
        }

        /// <summary>
        /// 获取专家统计详情列表
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetStatDetail(StatQuery query)
        {
            IAuthorFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            query.JournalID = JournalID;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            IList<StatDealContributionDetailEntity> listStatDetail = service.GetDealContributionDetail(query);
            return Json(new { Rows = listStatDetail.Skip((pageIndex - 1) * 10).Take(10), Total = listStatDetail.Count });
        }

        /// <summary>
        /// 获取当前状态的稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetEditorContributionListAjax(CirculationEntity cirEntity)
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            cirEntity.JournalID = JournalID;
            cirEntity.IsHandled = cirEntity.Status;
            int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            cirEntity.CurrentPage = pageIndex;
            cirEntity.PageSize = TypeParse.ToInt(Request.Params["pagesize"], 10);
            Pager<FlowContribution> pager = service.GetFlowContributionList(cirEntity);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 保存专家审稿费设置
        /// </summary>
        /// <param name="ReviewFee"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxRequest]
        public ActionResult SaveConfig(decimal ExpertReviewFee)
        {
            SiteConfigInfo config = SiteConfig.GetSiteConfig();
            if (config != null)
            {
                try
                {
                    config.ExpertReviewFee = ExpertReviewFee;
                    SiteConfig.SaveConfig(config);
                }
                catch (Exception ex)
                {
                    return Json(new { Msg = "审稿费设置在保存时发生错误，详细信息：" + ex.Message });
                }
                return Json(new { Msg = "设置已保存！" });
            }
            else
                return Json(new { Msg = "配置文件加载失败,请检查config\\siteconfig.config文件是否存在。" });

        }


    }
}
