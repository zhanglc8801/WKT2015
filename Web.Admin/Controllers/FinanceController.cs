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
using System.Web.Script.Serialization;
using WKT.Common.Data;
using WKT.Log;
using WKT.Config;

namespace Web.Admin.Controllers
{
    public class FinanceController : BaseController
    {
        #region 稿件费用管理
        public ActionResult FContributeIndex()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetFContributePageList(FinanceContributeQuery query)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            query.JournalID = CurAuthor.JournalID;
            query.IsShowAuthor = true;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<FinanceContributeEntity> pager = service.GetFinanceContributePageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        [HttpPost]
        public ActionResult GetFContributeList(FinanceContributeQuery query)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            query.JournalID = CurAuthor.JournalID;
            IList<FinanceContributeEntity> list = service.GetFinanceContributeList(query);
            return Json(new { list });
        }

        [HttpPost]
        public ActionResult FContributeDetail(Int64 PKID = 0)
        {
            return View(GetFContributeModel(PKID));
        }

        #region 审稿费/版面费入款页
        private FinanceContributeEntity GetFContributeModel(Int64 PKID)
        {
            FinanceContributeEntity model = null;
            if (PKID > 0)
            {
                FinanceContributeQuery query = new FinanceContributeQuery();
                query.JournalID = CurAuthor.JournalID;
                query.PKID = PKID;
                IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
                model = service.GetFinanceContributeModel(query);
            }
            if (model == null)
                model = new FinanceContributeEntity();
            return model;
        }

        public ActionResult FContributeCreate(Byte FeeType = 0, Int64 PKID = 0)
        {
            ViewBag.IsAuthor = (CurAuthor.GroupID == 2 ? 1 : 0);
            FinanceContributeEntity model = GetFContributeModel(PKID);
            if (FeeType > 0)
                model.FeeType = FeeType;
            return View(model);
        } 
        #endregion


        #region 稿费入款页
        private FinanceContributeEntity GetFinanceArticleModel(Int64 PKID)
        {
            FinanceContributeEntity model = null;
            if (PKID > 0)
            {
                FinanceContributeQuery query = new FinanceContributeQuery();
                query.JournalID = CurAuthor.JournalID;
                query.PKID = PKID;
                IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
                model = service.GetFinanceContributeModel(query);
            }
            if (model == null)
                model = new FinanceContributeEntity();
            return model;
        }
        public ActionResult FinanceArticleCreate(Byte FeeType = 0, Int64 PKID = 0)
        {
            ViewBag.IsAuthor = (CurAuthor.GroupID == 2 ? 1 : 0);
            FinanceContributeEntity model = GetFinanceArticleModel(PKID);
            if (FeeType > 0)
                model.FeeType = FeeType;
            return View(model);
        } 
        #endregion

        /// <summary>
        /// 入款
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FContributeSave(FinanceContributeEntity model)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            model.JournalID = CurAuthor.JournalID;
            model.InUser = CurAuthor.AuthorID;
            model.IsSystem = CurAuthor.GroupID == 1;
            if (model.PKID == 0)
            {
                #region 改变稿件状态 为已经交费
                ISiteConfigFacadeService facadeService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                IList<DictValueEntity> dicteEntity = null;
                IList<DictValueEntity> noticDicteEntity = null;
                if (model.FeeType == 1)
                {
                    noticDicteEntity = facadeService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = "PayNotice" });
                    dicteEntity = facadeService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = "Payed" });
                }
                else
                {
                    noticDicteEntity = facadeService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = "PayPageNotice" });
                    dicteEntity = facadeService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = "PagePayed" });
                }
                if (dicteEntity != null && dicteEntity.Count > 0 && noticDicteEntity != null && noticDicteEntity.Count > 0)
                {
                    ISiteConfigFacadeService currentService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                    CirculationEntity cirEntity = new CirculationEntity();
                    cirEntity.JournalID = CurAuthor.JournalID;
                    cirEntity.SendUserID = CurAuthor.AuthorID;
                    cirEntity.CID = model.CID;
                    cirEntity.StatusID = noticDicteEntity.FirstOrDefault<DictValueEntity>().ValueID;
                    cirEntity.ToStatusID = dicteEntity.FirstOrDefault<DictValueEntity>().ValueID;

                    IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                    flowService.DealFinaceInAccount(cirEntity);
                }

                #endregion
            }
            ExecResult result = service.SaveFinanceContribute(model);

            return Json(new { result = result.result, msg = result.msg });
        }

        [HttpPost]
        public ActionResult FContributeDelete(Int64[] PKIDs)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            FinanceContributeQuery query = new FinanceContributeQuery();
            query.JournalID = CurAuthor.JournalID;
            query.PKIDs = PKIDs;
            ExecResult result = service.DelFinanceContribute(query);
            return Json(new { result = result.result, msg = result.msg });
        }
        #endregion

        #region 入款通知

        /// <summary>
        /// 入款通知
        /// </summary>
        /// <returns></returns>
        public ActionResult FinanceAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetFinanceAccountPageList(ContributionInfoQuery query)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            query.JournalID = CurAuthor.JournalID;
            query.OrderStr = Request.Params["sortorder"];//排序类型(只按稿件编号排序)
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<FinanceAccountEntity> pager = service.GetFinanceAccountPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 获取稿费统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFinanceGaoFeePageList(ContributionInfoQuery query)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            query.JournalID = CurAuthor.JournalID;
            if (SiteConfig.GaoFeeText3 > 0)
                query.isPageFeeGet = true;
            query.OrderStr = Request.Params["sortorder"];//排序类型(只按稿件编号排序)
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<FinanceAccountEntity> pager = service.GetFinanceGaoFeePageList(query);
            if (SiteConfig.GaoFeeText3 > 0)
                return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords, SolidMoney = (pager.Money * SiteConfig.GaoFeeText3 / 100).ToString("C2") });
            else
                return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords, SolidMoney = pager.Money.ToString("C2") });
        }

        /// <summary>
        /// 导出作者稿费统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FinanceGaoFeeToExcel(ContributionInfoQuery query)
        {
            try
            {
                IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
                query.JournalID = CurAuthor.JournalID;
                query.Status = 1;
                query.IsReport = true;
                if (SiteConfig.GaoFeeText3 > 0)
                    query.isPageFeeGet = true;
                IList<FinanceAccountEntity> list = service.GetFinanceGaoFeePageList(query).ItemList;
                if (list == null || list.Count <= 0)
                {
                    return Content("没有数据不能导出，请先进行查询！");
                }
                string strTempPath = "/UploadFile/TempFile/" + "FinanceGaoFee.xls";
                if (SiteConfig.GaoFeeText3 > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].ArticlePaymentFee = list[i].LayoutFee * SiteConfig.GaoFeeText3 / 100;
                    }
                    string[] titleFiles = new string[] { "稿件编号", "稿件标题", "第一作者", "通讯作者", "发票抬头", "应付稿费", "手机", "联系电话", "单位", "地址", "邮编" };
                    int[] titleWidth = new int[] { 80, 200, 40, 40, 100, 60, 80, 90, 100, 150, 60 };
                    string[] dataFiles = new string[] { "CNumber", "Title", "FirstAuthor", "CommunicationAuthor", "InvoiceUnit", "ArticlePaymentFee", "Mobile", "Tel", "WorkUnit", "Address", "ZipCode" };
                    string[] fomateFiles = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
                    ExcelHelperEx.CreateExcel<FinanceAccountEntity>("作者稿费统计一览表", titleFiles, titleWidth, dataFiles, fomateFiles, list, strTempPath);
                }
                else
                {
                    string[] titleFiles = new string[] { "稿件编号", "稿件标题", "第一作者", "通讯作者", "发票抬头", "应付稿费", "稿费备注", "手机", "联系电话", "单位", "地址", "邮编" };
                    int[] titleWidth = new int[] { 80, 200, 40, 40, 100, 60, 80, 80, 90, 100, 150, 60 };
                    string[] dataFiles = new string[] { "CNumber", "Title", "FirstAuthor", "CommunicationAuthor", "InvoiceUnit", "ArticlePaymentFeeStr", "ArticlePaymentNote", "Mobile", "Tel", "WorkUnit", "Address", "ZipCode" };
                    string[] fomateFiles = new string[] { "", "", "", "", "", "", "", "", "", "", "", "" };
                    ExcelHelperEx.CreateExcel<FinanceAccountEntity>("作者稿费统计一览表", titleFiles, titleWidth, dataFiles, fomateFiles, list, strTempPath);
                }
                return Json(new { flag = 1, ExcelPath = strTempPath });
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出作者稿费信息出现异常：" + ex.Message);
                return Content("导出作者稿费信息异常！");
            }
        }

        #region 入款登记与通知_导出(已注释)
        //[HttpPost]
        //public ActionResult FinanceAccountRenderToExcel(ContributionInfoQuery query, string strDict)
        //{
        //    try
        //    {
        //        IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
        //        query.JournalID = CurAuthor.JournalID;
        //        query.IsReport = true;
        //        IList<FinanceAccountEntity> list = service.GetFinanceAccountPageList(query).ItemList;

        //        if (list == null || list.Count <= 0)
        //        {
        //            return Content("没有数据不能导出，请先进行查询！");
        //        }
        //        strDict = Server.UrlDecode(strDict);
        //        //list = list.Select(o =>
        //        //{
        //        //    o.ArticlePaymentFee = o.ArticlePaymentFee * query.ArticlePayment;
        //        //    o.WorkUnit = o.WorkUnit + " " + o.Address;
        //        //    o.Address = o.WorkUnit + " " + o.Address;
        //        //    return o;
        //        //}).ToList();
        //        JavaScriptSerializer s = new JavaScriptSerializer();
        //        Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(strDict);
        //        IDictionary<string, string> dict = ((object[])JsonData.First().Value).Select(p => (Dictionary<string, object>)p).ToDictionary(p => p["key"].ToString(), q => q["value"].ToString());
        //        RenderToExcel.ExportListToExcel<FinanceAccountEntity>(list, dict
        //            , null
        //            , "入款登记与通知_导出" + DateTime.Now.ToString("yyyy-MM-dd"), false, "xls");
        //        return Content("导出成功！");
        //    }
        //    catch (Exception ex)
        //    {
        //        LogProvider.Instance.Error("导出入款登记与通知信息出现异常：" + ex.Message);
        //        return Content("导出入款登记与通知信息异常！");
        //    }
        //} 
        #endregion


        [HttpPost]
        public ActionResult FinanceOutAccountRenderToExcel(ContributionInfoQuery query, string strDict)
        {
            try
            {
                IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
                query.JournalID = CurAuthor.JournalID;
                query.IsReport = true;
                IList<FinanceAccountEntity> list = service.GetFinanceAccountPageList(query).ItemList;
                if (list == null || list.Count <= 0)
                {
                    return Content("没有数据不能导出，请先进行查询！");
                }
                strDict = Server.UrlDecode(strDict);
                JavaScriptSerializer s = new JavaScriptSerializer();
                Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(strDict);
                IDictionary<string, string> dict = ((object[])JsonData.First().Value).Select(p => (Dictionary<string, object>)p).ToDictionary(p => p["key"].ToString(), q => q["value"].ToString());
                IList<FinanceAccountEntity> outList = list.Select(o => { o.LayoutFee = Math.Round(o.LayoutFee / 100 * query.Percent); return o; }).ToList<FinanceAccountEntity>();
                RenderToExcel.ExportListToExcel<FinanceAccountEntity>(list, dict
                    , null
                    , "出款_导出" + DateTime.Now.ToString("yyyy-MM-dd"), false, "xls");
                return Content("导出成功！");
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出入款登记与通知信息出现异常：" + ex.Message);
                return Content("导出入款登记与通知信息异常！");
            }
        }


        /// <summary>
        /// 通知单
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public ActionResult PayNotice(Byte PayType, Int64 NoticeID, Int64 CID)
        {
            PayNoticeQuery query = new PayNoticeQuery();
            query.JournalID = JournalID;
            query.NoticeID = NoticeID;
            query.PayType = PayType;
            query.CID = CID;
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            PayNoticeEntity model = service.GetPayNoticeModel(query);
            return View(model);
        }


        #region 批量通知交费单
        /// <summary>
        /// 批量通知交费单
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public ActionResult BatchPayNotice()
        {
            string str = Request.QueryString["Array"];
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            IList<PayNoticeEntity> list = new List<PayNoticeEntity>();
            if (!string.IsNullOrEmpty(str))
            {
                string[] paras = str.Split('|');
                if (paras != null && paras.Length > 0)
                {
                    foreach (var item in paras)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            string[] paras1 = item.Split(',');
                            if (paras1 != null && paras1.Length > 0)
                            {
                                PayNoticeQuery query = new PayNoticeQuery();
                                query.JournalID = JournalID;
                                query.NoticeID = int.Parse(paras1[1]);
                                query.PayType = byte.Parse(paras1[0]);
                                query.CID = int.Parse(paras1[2]);
                                query.AuthorID = int.Parse(paras1[3]);
                                query.IsBatch = true;
                                PayNoticeEntity model = service.GetPayNoticeModel(query);
                                if (model != null)
                                {
                                    model.AuthorID = int.Parse(paras1[3]);
                                    model.AuthorName = query.AuthorName;
                                    model.CTitle = query.Title;
                                    model.Mobile = query.Mobile;
                                    model.LoginName = query.LoginName;
                                    if (!string.IsNullOrEmpty(model.Body))
                                    {
                                        ViewBag.body = model.Body;
                                    }
                                    list.Add(model);
                                }

                                ViewBag.payType = byte.Parse(paras1[0]);
                            }

                        }
                    }
                }
            }
            TempData["list"] = list;

            return View();
        } 
        #endregion

        /// <summary>
        /// 获取通知缴费金额
        /// </summary>
        /// <param name="PayType"></param>
        /// <param name="NoticeID"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult GetPayNoticeAmount(Byte PayType, Int64 NoticeID, Int64 CID)
        {
            PayNoticeQuery query = new PayNoticeQuery();
            query.JournalID = JournalID;
            query.NoticeID = NoticeID;
            query.PayType = PayType;
            query.CID = CID;
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            PayNoticeEntity model = service.GetPayNoticeModel(query);
            return Json(new { Amount = model.Amount });
        }


        [HttpPost]
        public ActionResult SavePayNotice(PayNoticeEntity model)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            model.JournalID = CurAuthor.JournalID;
            model.SendUser = CurAuthor.AuthorID;
            model.Status = 0;
            model.Body = Server.UrlDecode(model.Body).Replace("${金额}$", model.Amount.ToString());
            if (model.NoticeID == 0)
            {
                #region 改变稿件状态  为 通知交审稿费

                int actionID = 0;
                ISiteConfigFacadeService facadeService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                DictEntity dicteEntity = null;
                if (model.PayType == 1)//审稿费
                {
                    dicteEntity = facadeService.GetDictModelByKey(new DictQuery() { JournalID = CurAuthor.JournalID, DictKey = "PayNotice" });
                }
                else //版面费
                {
                    dicteEntity = facadeService.GetDictModelByKey(new DictQuery() { JournalID = CurAuthor.JournalID, DictKey = "PayPageNotice" });

                }
                if (dicteEntity != null)
                {
                    ISiteConfigFacadeService currentService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                    IList<DictValueEntity> list = currentService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = dicteEntity.DictKey });
                    if (list != null && list.Count > 0)
                    {
                        DictValueEntity entity = list.Single<DictValueEntity>();
                        if (entity != null)
                        {
                            actionID = entity.ValueID;
                            #region 获取流程操作

                            FlowActionQuery actionQuery = new FlowActionQuery();
                            actionQuery.JournalID = JournalID;
                            actionQuery.ToStatusID = actionID;
                            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                            IList<FlowActionEntity> actionEntityList = flowService.GetFlowActionList(actionQuery);
                            //long statusID = actionEntity != null ? actionEntity.StatusID : 0;

                            #endregion

                            #region 根据审稿状态获取  审稿流程日志ID

                            CirculationEntity cirQuery = new CirculationEntity();
                            AuditBillEntity auditBillEntity = new AuditBillEntity();
                            cirQuery.CID = model.CID;
                            cirQuery.JournalID = JournalID;
                            cirQuery.GroupID = 1;
                            IFlowFacadeService flowInfoLogService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                            IList<FlowLogInfoEntity> flowLogEntityList = flowInfoLogService.GetFlowLog(cirQuery);
                            FlowActionEntity single = null;
                            FlowLogInfoEntity flowLogEntity = null;
                            if (flowLogEntityList != null && flowLogEntityList.Count > 0)
                            {
                                auditBillEntity.ReveiverList = flowLogEntityList[flowLogEntityList.Count-1].SendUserID.ToString();
                                flowLogEntity = flowLogEntityList.OrderByDescending(o => o.FlowLogID).Take(1).SingleOrDefault();
                                single = actionEntityList.Where(o => o.StatusID == flowLogEntity.TargetStatusID).FirstOrDefault();
                            }

                            #endregion

                            IFlowFacadeService flowFacadeService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                            
                            auditBillEntity.JournalID = JournalID;
                            auditBillEntity.Processer = CurAuthor.AuthorID;

                            
                            if (single != null)
                            {
                                auditBillEntity.ActionID = single.ActionID;
                                auditBillEntity.StatusID = single.StatusID;
                            }
                            auditBillEntity.ActionType = 1;
                            auditBillEntity.CID = model.CID;
                            if (flowLogEntity != null)
                            {
                                auditBillEntity.FlowLogID = flowLogEntity.FlowLogID;
                                auditBillEntity.CPath = "";
                                auditBillEntity.FigurePath = "";
                                auditBillEntity.OtherPath = "";
                            }
                            flowFacadeService.SubmitAuditBill(auditBillEntity);
                        }
                    }
                }

                #endregion
            }
            ExecResult result = service.SavePayNotice(model);

            return Json(new { result = result.result, msg = result.msg });
        }

        /// <summary>
        /// 批量通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchSavePayNotice(PayNoticeEntity model)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            IList<PayNoticeEntity> list = (IList<PayNoticeEntity>)TempData["list"];
            if (list != null && list.Count > 0)
            {
                #region 批量改变稿件状态  为 通知交审稿费
                int actionID = 0;
                ISiteConfigFacadeService facadeService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                DictEntity dicteEntity = null;
                if (model.PayType == 1)//审稿费
                {
                    dicteEntity = facadeService.GetDictModelByKey(new DictQuery() { JournalID = CurAuthor.JournalID, DictKey = "PayNotice" });
                }
                else //版面费
                {
                    dicteEntity = facadeService.GetDictModelByKey(new DictQuery() { JournalID = CurAuthor.JournalID, DictKey = "PayPageNotice" });

                }
                if (dicteEntity != null)
                {
                    ISiteConfigFacadeService currentService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                    IList<DictValueEntity> currentList = currentService.GetDictValueList(new DictValueQuery() { JournalID = CurAuthor.JournalID, DictKey = dicteEntity.DictKey });
                    if (currentList != null && currentList.Count > 0)
                    {
                        DictValueEntity entity = currentList.Single<DictValueEntity>();
                        if (entity != null)
                        {
                            actionID = entity.ValueID;
                            IFlowFacadeService flowFacadeService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();

                            #region 获取流程操作

                            FlowActionQuery actionQuery = new FlowActionQuery();
                            actionQuery.JournalID = JournalID;
                            actionQuery.ToStatusID = actionID;
                            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                            IList<FlowActionEntity> actionEntityList = flowService.GetFlowActionList(actionQuery);
                            #endregion

                            foreach (var item in list)
                            {
                                item.Status = 0;
                                item.Body = Server.UrlDecode(model.Body).Replace("${金额}$", model.Amount.ToString()).Replace("${接收人}$",item.AuthorName).Replace("${邮箱}$",item.LoginName).Replace("${稿件编号}$",item.CNumber).Replace("${稿件标题}$",item.CTitle).Replace("${手机}$",item.Mobile);                   
                                item.SendUser = CurAuthor.AuthorID;
                                item.Amount = model.Amount;
                                item.Title = model.Title;
                                if (item.NoticeID == 0)
                                {
                                    #region 根据审稿状态获取  审稿流程日志ID

                                    CirculationEntity cirQuery = new CirculationEntity();
                                    cirQuery.CID = item.CID;
                                    cirQuery.JournalID = JournalID;
                                    cirQuery.GroupID = 1;
                                    IFlowFacadeService flowInfoLogService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
                                    IList<FlowLogInfoEntity> flowLogEntityList = flowInfoLogService.GetFlowLog(cirQuery);
                                    FlowActionEntity single = null;
                                    FlowLogInfoEntity flowLogEntity = null;
                                    if (flowLogEntityList != null && flowLogEntityList.Count > 0)
                                    {
                                        flowLogEntity = flowLogEntityList.OrderByDescending(o => o.FlowLogID).Take(1).SingleOrDefault();
                                        single = actionEntityList.Where(o => o.StatusID == flowLogEntity.TargetStatusID).SingleOrDefault();
                                    }
                                    #endregion

                                    #region 批量提交审稿状态

                                    AuditBillEntity auditBillEntity = new AuditBillEntity();
                                    auditBillEntity.JournalID = JournalID;
                                    auditBillEntity.Processer = CurAuthor.AuthorID;
                                    auditBillEntity.ActionType = 1;
                                    auditBillEntity.CID = item.CID;
                                    if (single != null && flowLogEntity != null)
                                    {
                                        auditBillEntity.ActionID = single.ActionID;
                                        auditBillEntity.StatusID = single.StatusID;
                                        auditBillEntity.ReveiverList = flowLogEntity.RecUserID.ToString();
                                        auditBillEntity.FlowLogID = flowLogEntity.FlowLogID;
                                        auditBillEntity.CPath = string.IsNullOrEmpty(flowLogEntity.CPath) ? "" : flowLogEntity.CPath;
                                        auditBillEntity.FigurePath = string.IsNullOrEmpty(flowLogEntity.FigurePath) ? "" : flowLogEntity.FigurePath;
                                        auditBillEntity.OtherPath = string.IsNullOrEmpty(flowLogEntity.OtherPath) ? "" : flowLogEntity.OtherPath;
                                        flowFacadeService.SubmitAuditBill(auditBillEntity);

                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            ExecResult result = service.BatchSavePayNotice(list);

            return Json(new { result = result.result, msg = result.msg });
        }

        #endregion

        #region 出款通知

        public ActionResult FinanceOutAccount()
        {
          
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            //获取所有年
            YearVolumeQuery query = new YearVolumeQuery();
            query.JournalID = JournalID;
            IList<YearVolumeEntity> listYearVolume = service.GetYearVolumeList(query);
            //获取所有期
            IssueSetQuery issueSetQuery = new IssueSetQuery();
            issueSetQuery.JournalID = JournalID;
            IList<IssueSetEntity> listIssue = service.GetIssueSetList(issueSetQuery);

            ViewBag.listYearVolume = listYearVolume;
            ViewBag.listIssue = listIssue;

            return View();
        }


        [HttpPost]
        public ActionResult GetFinanceOutAccountPageList(ContributionInfoQuery query)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            query.JournalID = CurAuthor.JournalID;
            query.Year = Convert.ToInt32(query.Year) == 0 ? -1 : query.Year;
            query.Issue = Convert.ToInt32(query.Issue) == 0 ? -1 : query.Issue;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<FinanceAccountEntity> pager = service.GetFinanceAccountPageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        #endregion


        #region 财务统计一览
        //信封导出打印
        public ActionResult LetterPrint()
        {
            //获取站点配置信息
            SiteConfigQuery query = new SiteConfigQuery();
            query.JournalID = CurAuthor.JournalID;
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigEntity model = service.GetSiteConfigModel(query);
            if (model == null)
                model = new SiteConfigEntity();
            return View(model);
        }

        

        //作者稿费统计
        public ActionResult FinanceGaoFee()
        {
            return View();
        }

        //作者版面费报表
        public ActionResult FinancePageFeeReport()
        {
            return View();
        }

        //财务统计一览表
        public ActionResult FinanceGlance()
        {
            return View();
        }

        /// <summary>
        /// 获取财务统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFinanceGlancePageList(FinanceContributeQuery query)
         {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            query.JournalID = CurAuthor.JournalID;
            query.Status = 1;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<FinanceContributeEntity> pager = service.GetFinanceGlancePageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords, SolidMoney = pager.Money.ToString("C2") });
        }

        /// <summary>
        /// 导出财务统计一览表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="strDict"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FinanceGlanceToExcel(FinanceContributeQuery query)
        {
            try
            {
                IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
                query.JournalID = CurAuthor.JournalID;
                query.Status = 1;
                query.IsReport = true;
                IList<FinanceContributeEntity> list = service.GetFinanceGlancePageList(query).ItemList;
                if (list == null || list.Count <= 0)
                {
                    return Content("没有数据不能导出，请先进行查询！");
                }
                string[] titleFiles = new string[] { "稿件编号", "稿件标题", "通讯作者", "第一作者", "单位", "费用类型", "交费金额", "备注", "入款人", "入款日期", "发票抬头", "发票号码", "挂号号码", "寄出日期" };
                int[] titleWidth = new int[] { 80, 200, 40, 40, 100, 40, 60, 100, 40, 80, 120, 100, 80, 100 };
                string[] dataFiles = new string[] { "CNumber", "Title", "CommunicationAuthorName", "FirstAuthorName", "WorkUnit", "FeeTypeName", "Amount", "Note", "InUserName", "InComeDate", "InvoiceUnit", "InvoiceNo", "PostNo", "SendDate" };
                string[] fomateFiles = new string[] { "", "", "", "", "", "", "", "", "", "{0:yyyy-MM-dd}", "", "", "", "{0:yyyy-MM-dd}" };
                string strTempPath = "/UploadFile/TempFile/" + "FinanceGlance.xls";
                ExcelHelperEx.CreateExcel<FinanceContributeEntity>("财务统计一览表", titleFiles, titleWidth, dataFiles, fomateFiles, list, strTempPath);
                return Json(new { flag = 1, ExcelPath = strTempPath });
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出稿件费用信息出现异常：" + ex.Message);
                return Content("导出稿件费用信息异常！");
            }
        }

        //获取版面费报表分页数据
        [HttpPost]
        public ActionResult GetFinancePageFeeReportPageList(FinanceContributeQuery query)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            query.JournalID = CurAuthor.JournalID;
            //query.Status = 1;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<FinanceContributeEntity> pager = service.GetFinancePageFeeReportPageList(query);
            
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords, PageMoney = pager.PageMoney.ToString("C2") });
        }
        
        /// <summary>
        /// 导出作者版面费报表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FinancePageFeeRenderToExcel(FinanceContributeQuery query)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            query.JournalID = CurAuthor.JournalID;
            //query.Status = 1;
            query.IsReport = true;
            IList<FinanceContributeEntity> list = service.GetFinancePageFeeReportPageList(query).ItemList;
            string[] titleFiles = new string[] { "稿件编号", "稿件标题", "版面费","备注","通讯作者", "联系电话","手机", "Email", "工作单位", "发票单位", "联系地址","邮编","投稿日期" };
            int[] titleWidth = new int[] { 80, 200, 40, 100, 80, 100, 80, 80, 150,150,150,80,100 };
            string[] dataFiles = new string[] { "CNumber", "Title", "PageMoney", "Note", "AuthorName", "Tel", "Mobile", "Email", "WorkUnit", "InvoiceUnit","Address","ZipCode","AddDate" };
            string[] fomateFiles = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "{0:yyyy-MM-dd}" };
            string strTempPath = "/UploadFile/TempFile/" + "FinanceContribute.xls";
            ExcelHelperEx.CreateExcel<FinanceContributeEntity>("作者版面费报表", titleFiles, titleWidth, dataFiles, fomateFiles, list, strTempPath);
            return Json(new { flag = 1, ExcelPath = strTempPath });
        }

        /// <summary>
        /// 导出入款登记与通知
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FinanceAccountToExcel(ContributionInfoQuery query)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            query.JournalID = CurAuthor.JournalID;
            query.IsReport = true;
            IList<FinanceAccountEntity> list = service.GetFinanceAccountPageList(query).ItemList;

            string[] titleFiles = new string[] { "稿件编号", "稿件标题", "审稿费(通知)", "版面费(通知)", "审稿费(已入款)","备注", "版面费(已入款)","备注", "第一作者","通讯作者", "联系电话", "手机", "发票单位", "邮编", "投稿日期" };
            int[] titleWidth = new int[] { 60, 200, 60, 60, 70, 80, 70, 80, 50, 50, 70, 70, 120, 40,120 };
            string[] dataFiles = new string[] { "CNumber", "Title", "ReadingFeeNoticeStatus", "LayoutFeeNoticeStatus", "ReadingFeeStr", "Note", "LayoutFeeStr", "PageNote", "FirstAuthor", "CommunicationAuthor", "Tel", "Mobile", "InvoiceUnit", "ZipCode", "AddDate" };
            string[] fomateFiles = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "","", "{0:yyyy-MM-dd}" };
            string strTempPath = "/UploadFile/TempFile/" + "FinanceAccount.xls";
            ExcelHelperEx.CreateExcel<FinanceAccountEntity>("作者版面费报表", titleFiles, titleWidth, dataFiles, fomateFiles, list, strTempPath);
            return Json(new { flag = 1, ExcelPath = strTempPath });
        }


        #endregion

        
        /// <summary>
        /// 保存计费设置(审稿费与版面费)
        /// </summary>
        /// <param name="ReviewFeeText"></param>
        /// <param name="PageFeeText"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxRequest]
        public ActionResult SaveConfig(decimal ReviewFeeText, decimal PageFeeText)
        { 
            SiteConfigInfo config = SiteConfig.GetSiteConfig();
            if (config != null)
            {
                try
                {
                    config.ReviewFeeText = ReviewFeeText;
                    config.PageFeeText = PageFeeText;
                    SiteConfig.SaveConfig(config);
                }
                catch (Exception ex)
                {
                    return Json(new { Msg = "计费设置在保存时发生错误，详细信息："+ex.Message });
                }
                return Json(new { Msg = "计费设置已成功保存！" });
            }
            else
                return Json(new { Msg = "配置文件加载失败,请检查config\\siteconfig.config文件是否存在。" });

        }
        /// <summary>
        /// 保存计费设置(稿费)
        /// </summary>
        /// <param name="GaoFeeText1"></param>
        /// <param name="GaoFeeText2"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxRequest]
        public ActionResult SaveConfigForGaoFee(decimal GaoFeeText1, decimal GaoFeeText2, decimal GaoFeeText3)
        {
            SiteConfigInfo config = SiteConfig.GetSiteConfig();
            if (config != null)
            {
                try
                {
                    config.GaoFeeText1 = GaoFeeText1;
                    config.GaoFeeText2 = GaoFeeText2;
                    config.GaoFeeText3 = GaoFeeText3;
                    SiteConfig.SaveConfig(config);
                }
                catch (Exception ex)
                {
                    return Json(new { Msg = "计费设置在保存时发生错误，详细信息：" + ex.Message });
                }
                return Json(new { Msg = "计费设置已成功保存！" });
            }
            else
                return Json(new { Msg = "配置文件加载失败,请检查config\\siteconfig.config文件是否存在。" });

        }


    }
}
