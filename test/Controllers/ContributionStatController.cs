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
using WKT.Config;
using WKT.Common.Utils;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using System.Drawing;
using DotNet.Highcharts.Helpers;
using System.Web.Script.Serialization;
using WKT.Common.Data;
using WKT.Log;

namespace HanFang360.InterfaceService.Controllers
{
    public class ContributionStatController : BaseController
    {
        public ActionResult Index(int StatType = 0, Int32 Year = 0)
        {
            Highcharts chart = null;
            Year = Year == 0 ? DateTime.Now.Year : Year;
            switch (StatType)
            {
                case 0://时间
                    chart = GetContributionAccountListByYear(Year);
                    break;
                case 1:// 基金
                    chart = GetContributionAccountListByFund();
                    break;
                case 2://作者
                    chart = new Highcharts("按作者统计收稿量");
                    break;
                default://时间
                    chart = GetContributionAccountListByYear(Year);
                    break;
            }
            ViewBag.StatType = StatType;
            ViewBag.Year = Year;
            return View(chart);
        }

        /// <summary>
        /// 按作者统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetContributionAccountListByAuhor(ContributionAccountQuery query)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            query.JournalID = CurAuthor.JournalID;           
            query.CurrentPage = Convert.ToInt32(Request.Params["page"]);
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            Pager<ContributionAccountEntity> pager = service.GetContributionAccountListByAuhor(query);
            return Json(new { Rows = pager.ItemList, Total = pager.TotalRecords, Account=pager.Money });
        }

        /// <summary>
        /// 按时间统计收稿量
        /// </summary>
        /// <returns></returns>
        private Highcharts GetContributionAccountListByYear(Int32 Year)
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            ContributionAccountQuery query = new ContributionAccountQuery();
            query.JournalID = JournalID;
            query.Year = Year;
            IList<ContributionAccountEntity> list = service.GetContributionAccountListByYear(query);
            if (list == null || list.Count == 0)
                return new Highcharts("chart");
            list = list.OrderBy(p => p.Month).ToList();
            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title { Text = "按年(" + Year .ToString()+ "年)统计收稿量" })
                .SetXAxis(new XAxis
                {
                    Categories = list.Select(p => p.MonthName).ToArray<string>(),
                    Title = new XAxisTitle { Text = "月份" }
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle
                    {
                        Text = "收稿量",
                        Align = AxisTitleAligns.High
                    }
                })
                .SetTooltip(new Tooltip { Formatter = "function() { return ''+ this.series.name +': '+ this.y; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        PointPadding = 0.2,
                        BorderWidth = 0,
                        PointWidth = 20
                    }
                })
                .SetLegend(new Legend
                {
                    Layout = Layouts.Vertical,
                    Align = HorizontalAligns.Right,
                    VerticalAlign = VerticalAligns.Top,
                    Floating = true,
                    BorderWidth = 1,
                    BackgroundColor = ColorTranslator.FromHtml("#FFFFFF"),
                    Shadow = true
                })
                .SetCredits(new Credits { Enabled = false })
                .SetSeries(new[]
                           {
                               new Series { Name = "收稿量", Data = new Data(list.Select(p => (object)p.Account).ToArray<object>()) }
                           });
            return chart;
        }

        /// <summary>
        /// 按基金统计收稿量
        /// </summary>
        /// <returns></returns>
        private Highcharts GetContributionAccountListByFund()
        {
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            ContributionAccountQuery query = new ContributionAccountQuery();
            query.JournalID = JournalID;
            IList<ContributionAccountEntity> list = service.GetContributionAccountListByFund(query);
            if (list == null || list.Count == 0)
                return new Highcharts("chart");
            list = list.OrderBy(p => p.FundLevel).ToList();
            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title { Text = "按基金统计收稿量" })
                .SetXAxis(new XAxis
                {
                    Categories = list.Select(p => p.FundName).ToArray<string>(),
                    Title = new XAxisTitle { Text = "基金级别" }
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle
                    {
                        Text = "收稿量",
                        Align = AxisTitleAligns.High
                    }
                })
                .SetTooltip(new Tooltip { Formatter = "function() { return ''+ this.series.name +': '+ this.y; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        PointPadding = 0.2,
                        BorderWidth = 0,
                        PointWidth = 20
                    }
                })
                .SetLegend(new Legend
                {
                    Layout = Layouts.Vertical,
                    Align = HorizontalAligns.Right,
                    VerticalAlign = VerticalAligns.Top,
                    Floating = true,
                    BorderWidth = 1,
                    BackgroundColor = ColorTranslator.FromHtml("#FFFFFF"),
                    Shadow = true
                })
                .SetCredits(new Credits { Enabled = false })
                .SetSeries(new[]
                           {
                               new Series { Name = "收稿量", Data = new Data(list.Select(p => (object)p.Account).ToArray<object>()) }
                           });
            return chart;
        }

        [HttpPost]
        public ActionResult ContributionAccountToExcel(ContributionAccountQuery query, string strDict)
        {
            string msg = string.Empty;
            try
            {
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                query.JournalID = CurAuthor.JournalID;
                query.IsReport = true;
                strDict = Server.UrlDecode(strDict);
                JavaScriptSerializer s = new JavaScriptSerializer();
                Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(strDict);
                IDictionary<string, string> dict = ((object[])JsonData.First().Value).Select(p => (Dictionary<string, object>)p).ToDictionary(p => p["key"].ToString(), q => q["value"].ToString());

                IList<ContributionAccountEntity> list = new List<ContributionAccountEntity>();
                IList<FlowStatusEntity> listFs = new List<FlowStatusEntity>();
                IList<FlowContribution> listFc = new List<FlowContribution>();
                switch (query.Kind)
                {
                    case 4:
                        msg = "按过程稿统计收稿量";
                        listFs = GetContributionProcessList();
                        break;
                    case 3:
                        msg = "按退稿统计收稿量";
                        listFc = GetContributionReturnList();
                        break;
                    case 2:
                        msg = "按作者统计收稿量";
                        var pager = service.GetContributionAccountListByAuhor(query);
                        if (pager != null) list = pager.ItemList;
                        break;
                    case 1:
                        msg = "按基金统计收稿量";
                        list = service.GetContributionAccountListByFund(query);
                        break;
                    case 0:
                        msg = "按时间统计收稿量";
                        list = service.GetContributionAccountListByYear(query);
                        break;

                }
                if(query.Kind == 1 || query.Kind == 2 || query.Kind == 0)
                {
                    if (list == null || list.Count <= 0)
                    {
                        return Content("没有数据不能导出，请先进行查询！");
                    }
                    RenderToExcel.ExportListToExcel<ContributionAccountEntity>(list, dict
                        , null
                        , msg + "_导出" + DateTime.Now.ToString("yyyy-MM-dd"), false, "xls");
                }
                else if (query.Kind == 3)
                {
                    if (listFc == null || listFc.Count <= 0)
                    {
                        return Content("没有数据不能导出，请先进行查询！");
                    }

                    RenderToExcel.ExportListToExcel<FlowContribution>(listFc, dict
                        , null
                        , msg + "_导出" + DateTime.Now.ToString("yyyy-MM-dd"), false, "xls");
                }
                else if(query.Kind == 4)
                {
                    if (listFs == null || listFs.Count <= 0)
                    {
                        return Content("没有数据不能导出，请先进行查询！");
                    }

                    RenderToExcel.ExportListToExcel<FlowStatusEntity>(listFs, dict
                        , null
                        , msg + "_导出" + DateTime.Now.ToString("yyyy-MM-dd"), false, "xls");
                }
                return Content("导出成功！");
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("导出" + msg + "出现异常：" + ex.Message);
                return Content("导出" + msg + "异常！");
            }
        }

        /// <summary>
        /// 获取退稿
        /// </summary>
        private IList<FlowContribution> GetContributionReturnList()
        {
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity cirEntity = new CirculationEntity();
            cirEntity.JournalID = JournalID;
            cirEntity.CStatus = -100;
            cirEntity.CurrentPage = 1;
            cirEntity.PageSize = TypeParse.ToInt(Request.Params["pagesize"], 100000);
            Pager<FlowContribution> pager = new Pager<FlowContribution>();
            try
            {
                pager = service.GetFlowContributionList(cirEntity);
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("获取退稿导出数据出现异常:" + ex.Message);
            }
            if (pager != null)
            {
                return pager.ItemList;
            }
            else
            {
                return new List<FlowContribution>();
            }
        }

        /// <summary>
        /// 获取过程稿
        /// </summary>
        private IList<FlowStatusEntity> GetContributionProcessList()
        {
            IList<FlowStatusEntity> list = new List<FlowStatusEntity>();
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            FlowStatusQuery query = new FlowStatusQuery();
            query.JournalID = JournalID;
            query.IsHandled = 2;
            query.CurAuthorID = 0;
            query.RoleID = 0;
            try
            {
                list = service.GetHaveRightFlowStatus(query);                
            }
            catch (Exception ex)
            {
                WKT.Log.LogProvider.Instance.Error("获取过程稿导出数据出现异常:" + ex.Message);
            }
            return list;
        }
    }
}
