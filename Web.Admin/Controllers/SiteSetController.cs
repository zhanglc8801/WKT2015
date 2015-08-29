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
using System.Data;

namespace Web.Admin.Controllers
{
    public class SiteSetController : BaseController
    {
       



        public ActionResult Index()
        {
            //获取配置信息
            DataTable GlobalSetDT = GetSiteSetCfg.GetSiteGlobalSet(CurAuthor.JournalID.ToString());
            DataTable PersonalSetDT = GetSiteSetCfg.GetSitePersonalSet(CurAuthor.JournalID.ToString(), CurAuthor.AuthorID.ToString());
            //获取基本信息
            SiteConfigQuery query = new SiteConfigQuery();
            query.JournalID = CurAuthor.JournalID;
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            SiteConfigEntity model = service.GetSiteConfigModel(query);
            if (model == null)
                model = new SiteConfigEntity();
            ViewBag.AuthorID = CurAuthor.AuthorID;
            ViewBag.IsShowPwdInput = CurAuthor.LoginName == GlobalSetDT.Rows[0]["SysSuperAdmin"].ToString() ? 1 : 0;//是否显示password类型的输入框中的内容
            ViewBag.IsEnableRegActivate = GlobalSetDT.Rows[0]["IsEnableRegActivate"].ToString();
            ViewBag.IsHideEditorInfoForAuthor = GlobalSetDT.Rows[0]["IsHideEditorInfoForAuthor"].ToString();
            ViewBag.IsHideEditorInfoForExpert = GlobalSetDT.Rows[0]["IsHideEditorInfoForExpert"].ToString();
            ViewBag.ShowNameForHide = GlobalSetDT.Rows[0]["ShowNameForHide"].ToString();
            ViewBag.IsStopNotLoginDownPDF = GlobalSetDT.Rows[0]["IsStopNotLoginDownPDF"].ToString();
            ViewBag.ShowMoreFlowInfoForAuthor = GlobalSetDT.Rows[0]["ShowMoreFlowInfoForAuthor"].ToString();
            ViewBag.ShowHistoryFlowInfoForExpert = GlobalSetDT.Rows[0]["ShowHistoryFlowInfoForExpert"].ToString();
            ViewBag.isAutoHandle = GlobalSetDT.Rows[0]["isAutoHandle"].ToString();
            ViewBag.isStatByGroup = GlobalSetDT.Rows[0]["isStatByGroup"].ToString();

            ViewBag.Personal_Order = PersonalSetDT.Rows[0]["Personal_Order"].ToString();
            ViewBag.Personal_OnlyMySearch = PersonalSetDT.Rows[0]["Personal_OnlyMySearch"].ToString();

            return View(model);
        }

        [HttpPost]
        [AjaxRequest]
        public ActionResult Edit(SiteConfigEntity model, bool IsFinance, int isRegAct, int IsHideEditorInfoForAuthor, int IsHideEditorInfoForExpert, int IsStopNotLoginDownPDF, int ShowMoreFlowInfoForAuthor,int ShowHistoryFlowInfoForExpert,int isAutoHandle,int isStatByGroup,int isPersonal_Order, int isPersonal_OnlyMySearch)
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
                    SiteConfig.SaveConfig(config);

                    string globalSql = "update SiteGlobalSettings set IsEnableRegActivate='" + isRegAct + "',IsHideEditorInfoForAuthor='" + IsHideEditorInfoForAuthor + "',IsHideEditorInfoForExpert='" + IsHideEditorInfoForExpert + "',IsStopNotLoginDownPDF='" + IsStopNotLoginDownPDF + "',ShowMoreFlowInfoForAuthor='" + ShowMoreFlowInfoForAuthor + "',ShowHistoryFlowInfoForExpert='" + ShowHistoryFlowInfoForExpert + "',isAutoHandle='" + isAutoHandle + "',isStatByGroup='" + isStatByGroup + "' where SiteID='" + CurAuthor.JournalID + "'";
                    SQLiteHelper.ExeSql(globalSql);

                }
                //同步设置到个人配置文件
                string PersonalSql = "update SitePersonalSettings set Personal_Order='" + isPersonal_Order + "',Personal_OnlyMySearch='" + isPersonal_OnlyMySearch + "' where SiteID='" + CurAuthor.JournalID + "' and EditorID='" + CurAuthor.AuthorID + "'";
                SQLiteHelper.ExeSql(PersonalSql);
                

            }
            return Json(new { result = result.result, msg = result.msg });
        }
    }
}
