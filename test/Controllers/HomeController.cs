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
using WKT.Common.Utils;

namespace HanFang360.InterfaceService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.IP = WKT.Common.Utils.Utils.GetRealIP();
            return View();
        }

        /// <summary>
        /// http://localhost:3901/AccessValidate/?serialNumber=222&IP=127.0.0.1
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AccessValidate(string serialNumber, string IP)
        {

            return base.Json(new { code = "1000001" }, JsonRequestBehavior.AllowGet);
        }

        //http://localhost:3901/home/GetContributionList/?siteid=20130107001&email=zqzuozhe@163.com
        //获取作者最新状态的稿件列表
        [HttpGet]
        public ActionResult GetContributionList(long SiteID, string EMail)
        {

            string contributions = string.Empty;

            //根据邮箱获取作者实体-得到作者ID
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            AuthorInfoQuery query = new AuthorInfoQuery();
            query.LoginName = EMail;
            query.JournalID = SiteID;
            query.GroupID = (int)EnumMemberGroup.Author;
            AuthorInfoEntity authorInfoEntity = authorService.GetAuthorInfo(query);

            //获取作者最新状态的稿件列表
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity cirEntity = new CirculationEntity();
            cirEntity.JournalID = SiteID;
            cirEntity.CurAuthorID = authorInfoEntity.AuthorID;
            //int pageIndex = TypeParse.ToInt(Request.Params["page"], 1);
            cirEntity.CurrentPage = 1;
            cirEntity.PageSize = TypeParse.ToInt(Request.Params["pagesize"], 100);
            Pager<FlowContribution> pager = flowService.GetAuthorContributionList(cirEntity);

            if (pager.ItemList.Count == 0)
            {
                return base.Json(new { code = "1000002", msg = "没有查询到符合条件的数据。" }, JsonRequestBehavior.AllowGet);//没有稿件数据
            }
            else if (pager.ItemList.Count == 1)
            {
                return base.Json(new { code = "1000003", msg = "查询到1条符合条件的数据。" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                contributions += "[";
                for (int i = 0; i < pager.ItemList.Count; i++)
                {
                    contributions += "{\"Title\":\"" + pager.ItemList[i].Title + "\",\"AuthorName\":\"" + pager.ItemList[i].AuthorName + "\",\"AuditStatus\":\"" + pager.ItemList[i].AuditStatus + "\",\"AddDate\":\"" + pager.ItemList[i].AddDate + "\"},";
                }
                contributions.Remove(contributions.LastIndexOf(','), 1);
                contributions += "]";
                return base.Json(new { code = "1000004", msg = "查询到" + pager.ItemList.Count + "条符合条件的数据。", Contributions = contributions }, JsonRequestBehavior.AllowGet);
            }
        }


        
    }
}
