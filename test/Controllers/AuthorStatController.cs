using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Point = DotNet.Highcharts.Options.Point;

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

namespace HanFang360.InterfaceService.Controllers
{
    /// <summary>
    /// 作者统计
    /// </summary>
    public class AuthorStatController : BaseController
    {
        /// <summary>
        /// 作者统计，默认注册数量统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? StatType)
        {
            Highcharts chart = null;
            switch (StatType)
            {
                case 1:
                    chart = GetAuthorContributeStat();
                    break;
                case 2:// 地区统计
                    chart = GetAuthorProvinceStat();
                    break;
                case 3:// 学历统计
                    chart = GetAuthorEducationStat(3,"学历");
                    break;
                case 4:// 专业统计
                    chart = GetAuthorProfessionalStat();
                    break;
                case 5:// 职称统计
                    chart = GetAuthorEducationStat(5, "职称");
                    break;
                case 6:// 性别统计
                    chart = GetAuthorGenderStat();
                    break;
                default:
                     chart = GetAuthorContributeStat();
                     break;
            }
            ViewBag.StatType = StatType == null ? 1 : StatType.Value;
            return View(chart);
        }

        # region 注册数统计

        /// <summary>
        /// 注册数统计
        /// </summary>
        /// <returns></returns>
        private Highcharts GetAuthorContributeStat()
        {
            IAuthorFacadeService authorStatService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            QueryBase query = new QueryBase();
            query.JournalID = JournalID;
            IDictionary<string, int> dictAuthorStat = authorStatService.GetAuthorContributeStat(query);
            decimal TotalCount = TypeParse.ToDecimal(dictAuthorStat["TotalCount"]);
            decimal AuthorCount = TypeParse.ToDecimal(dictAuthorStat["AuthorCount"]);
            decimal Rate = AuthorCount / TotalCount;
            Rate = Rate * 100;
            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { PlotShadow = false })
                .SetTitle(new Title { Text = string.Format("注册量统计：本站共有作者：{0} 人，其中投稿 {1} 人，占总人数的 {2}%", dictAuthorStat["TotalCount"], dictAuthorStat["AuthorCount"], Math.Round(Rate, 2)) })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.point.y ; }" })//this.percentage
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        Cursor = Cursors.Pointer,
                        DataLabels = new PlotOptionsPieDataLabels
                        {
                            Color = ColorTranslator.FromHtml("#000000"),
                            ConnectorColor = ColorTranslator.FromHtml("#000000"),
                            Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.point.y; }"//Highcharts.numberFormat(this.percentage, 2)
                        }
                    }
                })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "Browser share",
                    Data = new Data(new object[]
                                               {
                                                   new object[] { "投稿人数", dictAuthorStat["AuthorCount"]},
                                                   new object[] { "暂无投稿人数", dictAuthorStat["TotalCount"]-dictAuthorStat["AuthorCount"] }
                                               })
                });
            return chart;
        }

        # endregion

        # region 地区统计

        /// <summary>
        /// 地区统计
        /// </summary>
        /// <returns></returns>
        private Highcharts GetAuthorProvinceStat()
        {
            IAuthorFacadeService authorStatService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            QueryBase query = new QueryBase();
            query.JournalID = JournalID;
            IList<AuthorStatEntity> authorStatList = authorStatService.GetAuthorProvinceStat(query);

            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar })
                .SetTitle(new Title { Text = "作者按地区统计" })
                .SetXAxis(new XAxis
                {
                    Categories = authorStatList.Select(p => p.StatItem).ToArray<string>(),
                    Title = new XAxisTitle { Text = string.Empty }
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle
                    {
                        Text = "作者注册数",
                        Align = AxisTitleAligns.High
                    }
                })
                .SetTooltip(new Tooltip { Formatter = "function() { return ''+ this.series.name +': '+ this.y; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Bar = new PlotOptionsBar
                    {
                        DataLabels = new PlotOptionsBarDataLabels { Enabled = true },
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
                               new Series { Name = "地区", Data = new Data(authorStatList.Select(p => (object)p.Count).ToArray<object>()) }
                           });
            return chart;
        }

        # endregion

        # region 专业统计

        /// <summary>
        /// 专业统计
        /// </summary>
        /// <returns></returns>
        private Highcharts GetAuthorProfessionalStat()
        {
            IAuthorFacadeService authorStatService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            QueryBase query = new QueryBase();
            query.JournalID = JournalID;
            IList<AuthorStatEntity> authorStatList = authorStatService.GetAuthorProfessionalStat(query);

            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar })
                .SetTitle(new Title { Text = "作者按专业统计" })
                .SetXAxis(new XAxis
                {
                    Categories = authorStatList.Select(p => p.StatItem).ToArray<string>(),
                    Title = new XAxisTitle { Text = string.Empty }
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle
                    {
                        Text = "作者注册数",
                        Align = AxisTitleAligns.High
                    }
                })
                .SetTooltip(new Tooltip { Formatter = "function() { return ''+ this.series.name +': '+ this.y; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Bar = new PlotOptionsBar
                    {
                        DataLabels = new PlotOptionsBarDataLabels { Enabled = true },
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
                               new Series { Name = "专业", Data = new Data(authorStatList.Select(p => (object)p.Count).ToArray<object>()) }
                           });
            return chart;
        }

        # endregion

        # region 学历统计、职称统计

        /// <summary>
        /// 地区统计
        /// </summary>
        /// <returns></returns>
        private Highcharts GetAuthorEducationStat(int StatType,string StatItem)
        {
            IAuthorFacadeService authorStatService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            QueryBase query = new QueryBase();
            query.JournalID = JournalID;
            IList<AuthorStatEntity> authorStatList = new List<AuthorStatEntity>();
            if (StatType == 3)
            {
                authorStatList = authorStatService.GetAuthorEducationStat(query);
            }
            else if (StatType == 5)
            {
                authorStatList = authorStatService.GetAuthorJobTitleStat(query);
            }
            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title { Text = string.Format("作者按{0}统计", StatItem) })
                .SetXAxis(new XAxis
                {
                    Categories = authorStatList.Select(p => p.StatItem).ToArray<string>(),
                    Title = new XAxisTitle { Text = string.Empty }
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title = new YAxisTitle
                    {
                        Text = "作者注册数",
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
                               new Series { Name = StatItem, Data = new Data(authorStatList.Select(p => (object)p.Count).ToArray<object>()) }
                           });
            return chart;
        }

        # endregion

        # region 性别统计

        /// <summary>
        /// 性别统计
        /// </summary>
        /// <returns></returns>
        private Highcharts GetAuthorGenderStat()
        {
            IAuthorFacadeService authorStatService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            QueryBase query = new QueryBase();
            query.JournalID = JournalID;
            IList<AuthorStatEntity> listAuthorGender = authorStatService.GetAuthorGenderStat(query);
            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { PlotShadow = false })
                .SetTitle(new Title { Text = "作者按性别统计" })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.point.y ; }" })//this.percentage
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        Cursor = Cursors.Pointer,
                        DataLabels = new PlotOptionsPieDataLabels
                        {
                            Color = ColorTranslator.FromHtml("#000000"),
                            ConnectorColor = ColorTranslator.FromHtml("#000000"),
                            Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.point.y; }"//Highcharts.numberFormat(this.percentage, 2)
                        }
                    }
                })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "Browser share",
                    Data = new Data(listAuthorGender.Select(p=> new object[] {p.StatItem,p.Count}).ToArray())
                });
            return chart;
        }

        # endregion
    }
}
