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

namespace HanFang360.InterfaceService.Controllers
{
    public class SiteSetController : BaseController
    {
       
        public ActionResult Index()
        {
            SiteConfigQuery query = new SiteConfigQuery();
            query.JournalID = CurAuthor.JournalID;
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigEntity model = service.GetSiteConfigModel(query);
            if (model == null)
                model = new SiteConfigEntity();
            ViewBag.AuthorID = CurAuthor.AuthorID;
            return View(model);
        }

        [HttpPost]
        [AjaxRequest]
        public ActionResult Edit(SiteConfigEntity model, bool IsFinance, bool isRegAct,bool isLoginVerify, bool isViewMoreFlow, bool isViewHistoryFlow, bool isAutoHandle, bool isStatByGroup, bool isPersonal_Order, bool isPersonal_OnlyMySearch)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            if (model.SiteConfigID == 0)
                model.InUserID = CurAuthor.AuthorID;
            else
                model.EditUserID = CurAuthor.AuthorID;
            model.JournalID = CurAuthor.JournalID;
            ExecResult result = service.EditSiteConfig(model);

            if (result.result == EnumJsonResult.success.ToString())
            {
                //同步设置到全局配置文件
                SiteConfigInfo config = SiteConfig.GetSiteConfig();
                if (config != null)
                {
                    config.SiteName = model.Title;
                    config.IsFinance = IsFinance ? 1 : 0;
                    config.isRegAct = isRegAct ? 1 : 0;
                    config.isLoginVerify = isLoginVerify ? 1 : 0;
                    config.isViewMoreFlow = isViewMoreFlow ? 1 : 0;
                    config.isViewHistoryFlow = isViewHistoryFlow ? 1 : 0;
                    config.isAutoHandle = isAutoHandle ? 1 : 0;
                    config.isStatByGroup = isStatByGroup ? 1 : 0;
                    SiteConfig.SaveConfig(config);
                }
                //同步设置到个人配置文件
                PersonalConfig personalConfig = new PersonalConfig(CurAuthor.AuthorID);
                PersonalConfigInfo personalConfigInfo = personalConfig.GetPersonalConfig();
                if (personalConfig != null)
                {
                    personalConfigInfo.isPersonal_Order = isPersonal_Order ? 1 : 0;
                    personalConfigInfo.isPersonal_OnlyMySearch = isPersonal_OnlyMySearch ? 1 : 0;
                    personalConfig.SaveConfig(personalConfigInfo);
                }

            }
            return Json(new { result = result.result, msg = result.msg });
        }
    }
}
