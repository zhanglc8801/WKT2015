using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Config;
using WKT.Log;
using WKT.Model.Enum;
using WKT.Model;
using WKT.Model.FangZ;
using WKT.Common.Xml;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace HanFang360.InterfaceService.Controllers
{
    public class FangzApiController : Controller
    {
        /// <summary>
        /// 单篇送排
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ActionResult SingleSendTypesetting (long CID)
        {
            ExecResult eResult = new ExecResult();
            FangZService.JournalXService jService = new FangZService.JournalXService();
            try
            {
                # region get token

                // 验证令牌正确性
                TokenQuery tokenQuery = new TokenQuery();
                tokenQuery.JournalID = SiteConfig.SiteID;
                tokenQuery.AuthorID = 0;
                tokenQuery.Type = 3;
                tokenQuery.ExpireDate = DateTime.Now.AddDays(-7);
                IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                TokenEntity tokenEntity = authorService.GetToken(tokenQuery);
                string SafetyCode = "";
                if (tokenEntity != null)
                {
                    SafetyCode = tokenEntity.Token;
                }

                # endregion

                magorder orderEntity = new magorder();
                orderEntity.magName = SiteConfig.SiteName;
                orderEntity.magEname = "";
                orderEntity.magId = SiteConfig.SiteID.ToString();
                orderEntity.manuId = CID.ToString();
                orderEntity.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //1:正确
                //0:未知异常
                //-1:组版设置错误,请到方正书畅管理端检查组版设置.
                //-2:更新刊物结构失败
                //-3:获取刊期结构失败
                //-4:任务正在处理中,请稍后送排.
                //-5:请传送排版结构并完成管理平台的“组版管理设置”后再进行“送排”
                //-6:请指定栏目或稿件后再进行“送排”
                //-7:请完成单稿件任务后再进行“送排”
                //-8:该单篇稿件已通过整期送排，无法再次“送排”
                //-9:方正书畅管理平台刊物信息未配置或配置有误，请确认后再试
                string resultXml = jService.MessageNotification(SafetyCode, "PublishMagazineDoc", XmlUtils.Serialize(orderEntity));
                if (resultXml == "1")
                {
                    eResult.result = "success";
                }
                else if (resultXml == "0")
                {
                    eResult.result = "failure";
                    eResult.msg = "未知异常";
                }
                else if (resultXml == "-1")
                {
                    eResult.result = "failure";
                    eResult.msg = "组版设置错误,请到方正书畅管理端检查组版设置";
                }
                else if (resultXml == "-2")
                {
                    eResult.result = "failure";
                    eResult.msg = "更新刊物结构失败";
                }
                else if (resultXml == "-3")
                {
                    eResult.result = "failure";
                    eResult.msg = "获取刊期结构失败";
                }
                else if (resultXml == "-4")
                {
                    eResult.result = "failure";
                    eResult.msg = "任务正在处理中,请稍后送排";
                }
                else if (resultXml == "-5")
                {
                    eResult.result = "failure";
                    eResult.msg = "请传送排版结构并完成管理平台的“组版管理设置”后再进行“送排”";
                }
                else if (resultXml == "-6")
                {
                    eResult.result = "failure";
                    eResult.msg = "请指定栏目或稿件后再进行“送排”";
                }
                else if (resultXml == "-7")
                {
                    eResult.result = "failure";
                    eResult.msg = "请完成单稿件任务后再进行“送排”";
                }
                else if (resultXml == "-8")
                {
                    eResult.result = "failure";
                    eResult.msg = "该单篇稿件已通过整期送排，无法再次“送排”";
                }
                else if (resultXml == "-9")
                {
                    eResult.result = "failure";
                    eResult.msg = "方正书畅管理平台刊物信息未配置或配置有误，请确认后再试";
                }
                else
                {
                    eResult.result = "failure";
                    eResult.msg = "失败";
                }
            }
            catch(Exception ex){
                LogProvider.Instance.Error("调用单篇文件送排失败：" + ex.Message);
                eResult.result = "error";
                eResult.msg = "调用单篇文件送排失败:" + ex.Message;
            }
            return Content(JsonConvert.SerializeObject(eResult));
        }
    }
}
