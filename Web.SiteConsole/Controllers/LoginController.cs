using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Model;
using WKT.Log;
using WKT.Common;
using WKT.Common.Security;
using WKT.Service.Interface;
using WKT.Service.Wrapper;

namespace Web.SiteConsole.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 验证码Key
        /// </summary>
        private const string VERFIYCODEKEY = "WKT_SITECONTROLE_VERFIFYCODE";

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginAjax(string LoginName, string Password, string VerifyCode)
        {
            string returnUrl = "/";

            # region func define

            Func<SysAccountInfoEntity, string, JsonResult> loginFunc = (accountEntity, pwd) =>
            {
                if (accountEntity == null)
                    return Json(new { result = "failure", msg = "用户名或密码错误！", url = returnUrl });
                if (accountEntity.Status == 1)// 禁用
                    return Json(new { result = "failure", msg = "您的账号已经禁用！", url = returnUrl });
                if (!accountEntity.Pwd.Equals(pwd))
                    return Json(new { result = "failure", msg = "用户名或密码错误！", url = returnUrl });

                accountEntity.LogOnTimes = accountEntity.LogOnTimes + 1;
                accountEntity.LastIP = WKT.Common.Utils.Utils.GetRealIP();
                accountEntity.LoginDate = DateTime.Now;

                # region 更新登录情况

                ISysAccountInfoService accountService = ServiceContainer.Instance.Container.Resolve<ISysAccountInfoService>();
                accountService.UpdateAccountLoginInfo(accountEntity.AdminID, accountEntity.LastIP, accountEntity.LoginDate.ToString("yyyy-MM-dd HH:mm:ss"));

                # endregion

                string jsonObject = JsonConvert.SerializeObject(accountEntity);
                // 保存登录ticket
                TicketTool.SetCookie(accountEntity.AdminID.ToString(), jsonObject);
                
                return Json(new { result = "success", msg = "登录成功！", url = returnUrl });
            };

            # endregion

            try
            {
                # region 得到验证码

                string code = "";
                HttpCookie cookie = Request.Cookies[VERFIYCODEKEY];
                if (cookie != null)
                {
                    code = cookie.Value.ToString();
                }
                # endregion

                if (WKT.Common.Security.DES.Encrypt(VerifyCode.ToLower()) == code)
                {
                    ISysAccountInfoService accountService = ServiceContainer.Instance.Container.Resolve<ISysAccountInfoService>();
                    SysAccountInfoQuery queryEntity = new SysAccountInfoQuery();
                    queryEntity.LoginName = SecurityUtils.SafeSqlString(LoginName);
                    IList<SysAccountInfoEntity> list = accountService.GetSysAccountInfoList(queryEntity);
                    if (list == null || list.Count == 0)
                        return Json(new { result = "failure", msg = "用户名或密码错误！"});
                    if (list.Count == 1)
                    {
                        Password = DES.Encrypt(Password);
                        return loginFunc(list[0], Password);
                    }
                    else
                        return Json(new { result = "failure", msg = "用户名重复，请确认是否输入正确"});
                }
                else
                {
                    return Json(new { result = "failure", msg = "请输入正确的验证码！"});
                }
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("系统出现异常，请稍后再试：" + ex.ToString());
                return Json(new { result = "error", msg = "系统出现异常，请稍后再试：" + ex.Message });
            }
        }

        # region 退出

        public ActionResult Logout()
        {
            TicketTool.Logout();
            // remove VerifyCode cookie
            Response.Cookies.Remove(VERFIYCODEKEY);
            return Redirect("/login/");
        }

        # endregion
    }
}
