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
using WKT.Common.Data;

namespace Web.Admin.Controllers
{
    public class ReportController : BaseController
    {
        public ActionResult Index(string type)
        {
            ViewBag.Type = type;
            return View();
        }

        public ActionResult EmptyIndex(string type)
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetShowField(string type)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            
            switch (type.ToLower())
            {
                case "author": dict = GetAuthorDetail(); break;
                case "issuesubscribe": dict = GetIssueSubscribe(); break;
                case "financeglance": dict = GetFinanceGlance(); break;
                case "financeaccount": dict = GetFinanceAccount(); break;
                case "financeoutaccount":dict = GetFinanceOutAccount();break;
                case "contributionaccountbyyear": dict = GetContributionAccountByYear(); break;
                case "contributionaccountbyfund": dict = GetContributionAccountByFund(); break;
                case "contributionaccountbyauhor": dict = GetContributionAccountByAuhor(); break;
                case "contributionaccountbytuigao": dict = GetContributionAccountByReturn(); break;// 退稿
                case "contributionaccountbygochenggao": dict = GetContributionAccountByProcessing(); break;// 过程稿
                case "expertpaymoney": dict = GetExpertPayMoneyByReturn(); break;//专家审稿费
            }
            return Json(dict);
        }

        /// <summary>
        /// 获取作者字段信息
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetAuthorDetail()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("AuthorName", "中文名称");
            dict.Add("AuthorModel.LoginName", "登录名");
            //dict.Add("AuthorModel.Pwd", "登录密码");
            //dict.Add("AuthorModel.GroupID", "用户类型");
            //dict.Add("AuthorModel.Status", "用户状态");
            dict.Add("ReserveField2", "用户类型");
            dict.Add("ReserveField3", "用户状态");
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            var list = authorService.GetFieldsSet();
            foreach (var item in list)
            {
                if (!dict.ContainsKey(item.DBField))
                    dict.Add(item.DBField, item.DisplayName);
            }
            dict.Add("AuthorModel.AddDate", "注册时间");
            return dict;
        }

        /// <summary>
        /// 获取期刊订阅字段信息
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetIssueSubscribe()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Subscriber", "单位/个人");
            dict.Add("SubscribeInfo", "订阅详情");
            dict.Add("SubscribeDate", "订阅日期");
            dict.Add("ContactUser", "联系人");
            dict.Add("Mobile", "手机");
            dict.Add("Tel", "电话");
            dict.Add("Fax", "传真");
            dict.Add("Email", "Email");
            dict.Add("ZipCode", "邮编");
            dict.Add("IsInvoiceName", "是否开票");
            dict.Add("InvoiceHead", "发票抬头");
            dict.Add("Address", "投递地址");
            dict.Add("Note", "备注");
            return dict;
        }

        /// <summary>
        /// 获取财务费用一览导出字段信息
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetFinanceGlance()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("CNumber", "稿件编号");
            dict.Add("Title", "稿件标题");
            dict.Add("AuthorName", "第一作者");
            dict.Add("Address", "作者地址");
            dict.Add("Tel", "作者电话");
            dict.Add("Mobile", "作者手机");
            dict.Add("InvoiceUnit", "发票抬头");
            dict.Add("WorkUnit", "单位");
            dict.Add("FeeTypeName", "费用类型");
            dict.Add("ShouldMoney", "应交");
            dict.Add("Amount", "实交");
            dict.Add("InUserName", "入款人");
            dict.Add("InComeDate", "入款日期");
            dict.Add("RemitBillNo", "发票号码");
            dict.Add("PostNo", "挂号号码");
            dict.Add("SendDate", "寄出日期");
            dict.Add("Note", "备注");
            return dict;
        }

        /// <summary>
        /// 获取财务登记与通知
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetFinanceAccount()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("CNumber", "稿件编号");
            dict.Add("Title", "稿件标题");
            dict.Add("FirstAuthor", "第一作者");
            dict.Add("Address", "作者地址");
            dict.Add("Tel", "作者电话");
            dict.Add("Mobile", "作者手机");
            dict.Add("InvoiceUnit", "发票抬头");
            dict.Add("ReadingFeeReportStr", "审稿费");
            dict.Add("LayoutFeeReportStr", "版面费");
            dict.Add("ArticlePaymentFee", "稿费");
            dict.Add("AddDate", "投稿日期");
            dict.Add("ZipCode", "邮编");
            dict.Add("WorkUnit", "作者单位");
            return dict;
        }

        /// <summary>
        /// 获取财务登记与通知
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetFinanceOutAccount()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("CNumber", "稿件编号");
            dict.Add("Title", "稿件标题");
            dict.Add("Year", "年");
            dict.Add("Issue", "期");
            dict.Add("AuthorName", "第一作者");
            dict.Add("Address", "作者地址");
            dict.Add("Tel", "作者电话");
            dict.Add("Mobile", "作者手机");
            dict.Add("InvoiceUnit", "发票抬头");
            dict.Add("LayoutFee", "稿费");
            dict.Add("AddDate", "投稿日期");
            return dict;
        }


        /// <summary>
        /// 获取按时间统计收稿量导出字段信息
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetContributionAccountByYear()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Year", "年");
            dict.Add("Month", "月");
            dict.Add("Account", "收稿量");
            return dict;
        }

        /// <summary>
        /// 获取按基金统计收稿量导出字段信息
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetContributionAccountByFund()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("FundLevel", "基金级别编号");
            dict.Add("FundName", "基金级别");
            dict.Add("Account", "收稿量");
            return dict;
        }

        /// <summary>
        /// 获取按作者统计收稿量导出字段信息
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetContributionAccountByAuhor()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("AuthorID", "作者编号");
            dict.Add("AuthorName", "作者名称");
            dict.Add("Account", "收稿量");
            return dict;
        }

        /// <summary>
        /// 获取退稿导出字段信息
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetContributionAccountByReturn()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Title", "稿件标题");
            dict.Add("AuthorName", "作者");
            dict.Add("AddDate", "处理人");
            dict.Add("SendUserName", "处理时间");
            return dict;
        }

        /// <summary>
        /// 获取过程稿导出字段信息
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetContributionAccountByProcessing()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("StatusName", "状态名称");
            dict.Add("ContributionCount", "稿件数量");
            return dict;
        }

        /// <summary>
        /// 获取专家审稿费导出字段信息
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetExpertPayMoneyByReturn()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("LoginName", "登录名");
            dict.Add("RealName", "真实姓名");
            dict.Add("HandedCount", "已处理");
            dict.Add("HandingCount", "未处理");
            dict.Add("ZipCode", "邮编");
            dict.Add("Address", "地址");

            return dict;
        }


    }
}
