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
using WKT.Common.Pay;

namespace Web.Admin.Controllers
{
    public class AuthorFinanceController : BaseController
    {
        #region 待交稿费
        /// <summary>
        /// 待交稿费
        /// </summary>
        /// <param name="FeeType"></param>
        /// <returns></returns>
        public ActionResult ToMakeMoney(Byte PayType=1)
        {
            ViewBag.PayType = PayType;
            return View();
        }

        [HttpPost]
        public ActionResult GetToMakeMoneyPageList(PayNoticeQuery query)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            query.JournalID = CurAuthor.JournalID;
            query.AuthorID = CurAuthor.AuthorID;
            query.Status = 10;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<PayNoticeEntity> pager = service.GetPayNoticePageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }

        /// <summary>
        /// 在线支付
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public ActionResult GoServiceRecharge(Int64 NoticeID)
        {            
            return View(GetModel(NoticeID));
        }

        /// <summary>
        /// 在线支付
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ProductTable"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GoPay(PayNoticeEntity model, string productTable = "ContributionInfo")
        {
            SiteConfigEntity config = GetSiteConfig();
            if (config == null || config.EBankType != 3)
            {
                return Json(new { result = EnumJsonResult.failure.ToString(), msg = "本网暂未开通网银支付！请通过邮局汇款方式邮寄给我们，汇款地址请查看交费通知单，谢谢！" });
            }

            //商户扩展信息包含4项信息用英文逗号分隔如：交费类型,产品表名,产品表主键字段值,产品描述
            string strKzInfo = model.PayType.ToString() + "," + productTable + "," + model.CNumber + ",用户：" + CurAuthor.LoginName + ",支付编号" + model.CNumber + model.PayTypeName + "," + model.NoticeID;

            string url = new YeepayHelper(config.EBankCode,config.EBankEncryKey)
                .CreateBuyUrl(model.Amount.ToString(), model.CNumber, model.PayTypeName, model.CTitle, "http://" + Request.Url.Authority.ToString() + "/AuthorFinance/YeepayCallback/", strKzInfo);
            
            return Json(new { result = EnumJsonResult.success.ToString(), url = url });
        }

        public ActionResult YeepayCallback()
        {
            YeepayHelper myYeepay = new YeepayHelper();
            string message=string.Empty;
            FinancePayDetailEntity model = myYeepay.GetPayResult((msg) => { message = msg; });
            if (model!= null)
            {
                model.JournalID = CurAuthor.JournalID;
                model.AuthorID = CurAuthor.AuthorID;
                model.EBankType = 3;
                model.PayDate = DateTime.Now;
                IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
                service.AddFinancePayDetail(model);

                //更新缴费通知记录
                if (model.NoticeID > 0)
                {
                    PayNoticeQuery query = new PayNoticeQuery();
                    query.NoticeID = model.NoticeID;
                    query.Status = 2;
                    service.ChangePayNoticeStatus(query);
                }
            }
            return Content(message);
        }

        private PayNoticeEntity GetModel(Int64 NoticeID)
        {
            PayNoticeEntity model = null;
            if (NoticeID > 0)
            {
                PayNoticeQuery query = new PayNoticeQuery();
                query.JournalID = CurAuthor.JournalID;
                query.NoticeID = NoticeID;
                IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
                model = service.GetPayNoticeModel(query);
            }
            if (model == null)
                model = new PayNoticeEntity();
            return model;
        }

        private SiteConfigEntity GetSiteConfig()
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigQuery query = new SiteConfigQuery();
            query.JournalID = CurAuthor.JournalID;
            return service.GetSiteConfigModel(query);
        }
        #endregion

        #region 费用一览
        public ActionResult CostOf()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult GetCostOfPageList(FinanceContributeQuery query)
        {
            IFinanceFacadeAPIService service = ServiceContainer.Instance.Container.Resolve<IFinanceFacadeAPIService>();
            query.JournalID = CurAuthor.JournalID;
            query.AuthorID = CurAuthor.AuthorID;
            query.IsShowAuthor = false;
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<FinanceContributeEntity> pager = service.GetFinanceContributePageList(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords });
        }
        #endregion
    }
}
