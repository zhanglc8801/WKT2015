using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WKT.Common.Utils;
using WKT.Common.Security;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Config;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;
using System.Configuration;

namespace Web.Mobile.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// 验证码Key
        /// </summary>
        private const string VERFIYCODEKEY = "WKT_AUTH_VERFIFYCODE";
        private const string PRELOGINUSERNAME = "WKT_PRELOGINUSERNAME";
        private const string PRELOGINUSERID = "WKT_PRELOGINUSERID";

        # region 登录

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Group">登录分组:Author，Expert，Editor</param>
        /// <returns></returns>
        public ActionResult Login(string Group)
        {
            if (TicketTool.IsLogin())
            {
                Response.Redirect(SiteConfig.RootPath + "/", true);
            }

            string refererUrl = "";
            if (!string.IsNullOrEmpty(Request.QueryString["returnurl"]))
            {
                refererUrl = Request.QueryString["returnurl"];
            }
            ViewBag.RefererUrl = refererUrl;

            ViewBag.Group = "";
            if (!string.IsNullOrEmpty(Group))
            {
                ViewBag.Group = Group;
            }

            # region 得到上一次的登录名

            string userName = "";
            HttpCookie cookie = Request.Cookies[PRELOGINUSERNAME];
            if (cookie != null)
            {
                userName = cookie.Value.ToString();
            }

            ViewBag.UserName = Server.UrlDecode(userName);

            # endregion

            return View();
        }

        [AjaxRequest]
        public JsonResult CheckLoginErrorLogAjax(string LoginName, string Pwd, string Group, int IsAutoLogin)
        {
            ILoginFacadeService loginService = ServiceContainer.Instance.Container.Resolve<ILoginFacadeService>();
            LoginErrorLogQuery loginErrorQuery = new LoginErrorLogQuery();
            loginErrorQuery.JournalID = SiteConfig.SiteID;
            loginErrorQuery.LoginName = LoginName;
            IList<LoginErrorLogEntity> list = loginService.GetLoginErrorLogList(loginErrorQuery);
            if (list.Count < 10)
            {
                return base.Json(new { flag = LoginNoVerifyCodeAjax(LoginName, Pwd, Group, IsAutoLogin), Count = 9 - list.Count });
            }
            else
            {
                if (list[0].AddDate.AddDays(1) > DateTime.Now)
                    return base.Json(new { flag = "LoginLock" });
                else
                {
                    #region 删除登录错误时记录的Cookie
                    HttpCookie LoginErrorCookie = new HttpCookie("LoginErrorCookie", LoginName);
                    LoginErrorCookie.Expires = DateTime.Now.AddDays(0);
                    Response.Cookies.Add(LoginErrorCookie);
                    #endregion
                    #region 删除登录错误日志
                    loginService.DeleteLoginErrorLog(loginErrorQuery);
                    #endregion
                    return base.Json(new { flag = LoginNoVerifyCodeAjax(LoginName, Pwd, Group, IsAutoLogin) });
                }
            }

        }

        /// <summary>
        /// 登录-不需要验证码
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="Pwd"></param>
        /// <param name="Group"></param>
        /// <param name="IsAutoLogin"></param>
        /// <returns></returns>
        public int LoginNoVerifyCodeAjax(string LoginName, string Pwd, string Group, int IsAutoLogin)
        {
            ExecResult authorJson = new ExecResult();
            if (string.IsNullOrEmpty(LoginName))
                return -1; 
            else if (string.IsNullOrEmpty(Pwd))
                return -1;
            else
            {
                AuthorInfoQuery queryAuthor = new AuthorInfoQuery();
                queryAuthor.LoginName = LoginName;
                queryAuthor.Pwd = Pwd;
                queryAuthor.JournalID = SiteConfig.SiteID;
                IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                AuthorInfoEntity authorEntity = authorService.AuthorLogin(queryAuthor);
                if (authorEntity != null)
                {
                    if (authorEntity.Status == 0)
                        return 0;//用户未激活
                    else
                    {
                        # region 记录登录信息

                        AuthorInfoEntity loginEntity = new AuthorInfoEntity();
                        loginEntity.AuthorID = authorEntity.AuthorID;
                        loginEntity.JournalID = authorEntity.JournalID;
                        loginEntity.LoginIP = Utils.GetRealIP();
                        loginEntity.LoginDate = DateTime.Now;
                        // 修改登录信息
                        authorService.RecordLoginInfo(loginEntity);

                        # endregion

                        # region 记录登录名Cookie

                        // 记录登录名Cookie
                        HttpCookie userNameCookie = new HttpCookie(PRELOGINUSERNAME, Server.UrlEncode(authorEntity.LoginName));
                        userNameCookie.Expires = DateTime.Now.AddYears(1);
                        Response.Cookies.Add(userNameCookie);

                        # endregion

                        # region 记录登ID Cookie

                        // 记录登录ID Cookie
                        HttpCookie userIDCookie = new HttpCookie(PRELOGINUSERID, authorEntity.AuthorID.ToString());
                        userIDCookie.Expires = DateTime.Now.AddMonths(1);
                        Response.Cookies.Add(userIDCookie);

                        # endregion

                        #region 删除登录错误时记录的Cookie
                        HttpCookie LoginErrorCookie = new HttpCookie("LoginErrorCookie", LoginName);
                        LoginErrorCookie.Expires = DateTime.Now.AddDays(0);
                        Response.Cookies.Add(LoginErrorCookie);
                        #endregion

                        #region 删除登录错误日志
                        ILoginFacadeService loginService = ServiceContainer.Instance.Container.Resolve<ILoginFacadeService>();
                        LoginErrorLogQuery loginErrorQuery = new LoginErrorLogQuery();
                        loginErrorQuery.JournalID = SiteConfig.SiteID;
                        loginErrorQuery.LoginName = LoginName;
                        loginService.DeleteLoginErrorLog(loginErrorQuery);
                        #endregion

                        // 重新设置分组
                        authorEntity.GroupID = GetGroup(authorEntity, Group);
                        // 保存登录ticket
                        TicketTool.SetCookie(authorEntity.AuthorID.ToString(), JsonConvert.SerializeObject(authorEntity), IsAutoLogin == 1 ? true : false);
                        return 1; ;
                    }
                }
                else
                {
                    #region 记录登录错误日志
                    ILoginFacadeService loginService = ServiceContainer.Instance.Container.Resolve<ILoginFacadeService>();
                    ExecResult loginErrorLogResult = new ExecResult();
                    LoginErrorLogEntity loginErrorEntity = new LoginErrorLogEntity();
                    loginErrorEntity.JournalID = SiteConfig.SiteID;
                    loginErrorEntity.LoginIP = WKT.Common.Utils.Utils.GetRealIP();
                    loginErrorEntity.LoginHost = WKT.Common.Utils.Utils.GetPCName();
                    loginErrorEntity.LoginName = LoginName;
                    loginErrorEntity.AddDate = DateTime.Now;
                    loginErrorLogResult = loginService.AddLoginErrorLog(loginErrorEntity);
                    #endregion
                    LoginErrorLogQuery loginErrorQuery = new LoginErrorLogQuery();
                    loginErrorQuery.JournalID = SiteConfig.SiteID;
                    loginErrorQuery.LoginName = LoginName;
                    if (loginService.GetLoginErrorLogList(loginErrorQuery).Count > 1)
                    {
                        # region 如果两次登录错误则记录Cookie
                        HttpCookie LoginErrorCookie = new HttpCookie("LoginErrorCookie", LoginName);
                        LoginErrorCookie.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Add(LoginErrorCookie);
                        # endregion
                        return -1;//用户名或密码错误
                    }
                    else
                        return -1;//用户名或密码错误
                }

            }
            //return Content(JsonConvert.SerializeObject(authorJson));
        }

        # endregion

        # region 注册

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Reg()
        {
            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult RegAjax(string LoginName, string Pwd, string RealName, string VerifyCode)
        {
            ExecResult regResult = new ExecResult();
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
                AuthorInfoEntity authorEntity = new AuthorInfoEntity();
                authorEntity.JournalID = SiteConfig.SiteID;
                authorEntity.LoginName = LoginName;
                authorEntity.RealName = RealName;
                authorEntity.Pwd = Pwd;
                if (GetSiteSetCfg.GetSiteGlobalSet(SiteConfig.SiteID.ToString()).Rows[0]["IsEnableRegActivate"] == "1")
                    authorEntity.Status = 0;
                else
                    authorEntity.Status = 1;
                IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                regResult = authorService.AuthorReg(authorEntity);
                // 注册成功后自动登录
                if (string.Compare(regResult.result, EnumJsonResult.success.ToString(), true) == 0)
                {
                    // 发送邮件
                    ISiteConfigFacadeService siteConfigService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
                    # region 获取模板内容
                    MessageTemplateQuery msgTempQuery = new MessageTemplateQuery();
                    msgTempQuery.JournalID = SiteConfig.SiteID;
                    msgTempQuery.ModelType = 1;
                    msgTempQuery.TCategory = -1;
                    msgTempQuery.TType = 1;
                    string MailContent = string.Empty;
                    string MailTitle = string.Empty;
                    MessageTemplateEntity msgTempEntity = siteConfigService.GetMessageTempModel(msgTempQuery);
                    if (msgTempEntity != null)
                    {
                        AuthorInfoEntity entity = authorService.GetAuthorInfo(new AuthorInfoQuery() { JournalID = authorEntity.JournalID, Status = authorEntity.Status, LoginName = LoginName });
                        IDictionary<string, string> dict = siteConfigService.GetEmailVariable();
                        dict["${接收人}$"] = authorEntity.RealName;
                        dict["${网站名称}$"] = SiteConfig.SiteName;
                        dict["${作者链接}$"] = AuditAuthorContributionUrl(entity.AuthorID);
                        MailContent = siteConfigService.GetEmailOrSmsContent(dict, msgTempEntity.TContent);

                    }
                    # region 发送邮件

                    MessageRecodeEntity messageEntity = new MessageRecodeEntity();
                    messageEntity.MsgType = 1;
                    messageEntity.JournalID = SiteConfig.SiteID;
                    if (GetSiteSetCfg.GetSiteGlobalSet(SiteConfig.SiteID.ToString()).Rows[0]["IsEnableRegActivate"] == "1")
                        messageEntity.MsgTitle = string.IsNullOrEmpty(MailTitle) ? SiteConfig.SiteName + "：请激活您的帐号" : MailTitle;
                    else
                        messageEntity.MsgTitle = msgTempEntity.Title; ;
                    messageEntity.MsgContent = string.IsNullOrEmpty(MailContent) ? "" : MailContent;
                    messageEntity.SendUser = 0;

                    IList<String> ReciveAddressList = new List<string>(1);
                    ReciveAddressList.Add(LoginName);
                    regResult = siteConfigService.SendEmailOrSms(ReciveAddressList, messageEntity);
                    # endregion
                    # endregion
                    //注册成功后自动登录
                    if (GetSiteSetCfg.GetSiteGlobalSet(SiteConfig.SiteID.ToString()).Rows[0]["IsEnableRegActivate"] == "0")
                    {
                        AuthorInfoQuery authorQuery = new AuthorInfoQuery();
                        authorQuery.JournalID = SiteConfig.SiteID;
                        authorQuery.LoginName = LoginName;
                        authorQuery.Status = authorEntity.Status;
                        AuthorInfoEntity loginAuthor = authorService.GetAuthorInfo(authorQuery);
                        // 保存登录ticket
                        TicketTool.SetCookie(loginAuthor.AuthorID.ToString(), JsonConvert.SerializeObject(loginAuthor), false);
                    }
                }
            }
            else
            {
                regResult.result = EnumJsonResult.error.ToString();
                regResult.msg = "请输入正确的验证码";
            }
            return Content(JsonConvert.SerializeObject(regResult));
        }

        # endregion

        /// <summary>
        /// 检测作者是否注册
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckIsRegAjax(string LoginName)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(LoginName, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                return base.Json(new { flag = "err" });//邮箱格式不正确
            }
            else
            {
                AuthorInfoQuery authorQuery = new AuthorInfoQuery();
                authorQuery.JournalID = SiteConfig.SiteID;
                authorQuery.LoginName = LoginName;
                IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                AuthorInfoEntity authorEntity = null;
                authorEntity = authorService.GetAuthorInfo(authorQuery);
                if (authorEntity == null)
                    return base.Json(new { flag = "1" });//可以注册
                else
                    return base.Json(new { flag = "0" });//已注册过
            }
        }


        # region 忘记密码

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        public ActionResult RetakePwd()
        {
            return View();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult RetakePwdAjax(string LoginName, string VerifyCode)
        {
            ExecResult sendResult = new ExecResult();

            if (!string.IsNullOrEmpty(LoginName))
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
                    IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();

                    # region 获取作者信息

                    AuthorInfoQuery queryAuthor = new AuthorInfoQuery();
                    queryAuthor.LoginName = LoginName;
                    queryAuthor.JournalID = SiteConfig.SiteID;
                    queryAuthor.Status = 1;

                    AuthorInfoEntity authorEntity = authorService.GetAuthorInfo(queryAuthor);

                    # endregion

                    if (authorEntity != null)
                    {
                        # region 新增Token

                        string strToekCode = RadomCode.GenerateCode(10) + DateTime.Now.Ticks;

                        TokenEntity getPwdToken = new TokenEntity();
                        getPwdToken.Token = strToekCode;
                        getPwdToken.JournalID = SiteConfig.SiteID;
                        getPwdToken.Type = 1;
                        getPwdToken.AuthorID = authorEntity.AuthorID;
                        ExecResult tokenResult = authorService.InsertToken(getPwdToken);

                        # endregion

                        if (string.Compare(tokenResult.result, EnumJsonResult.success.ToString(), false) == 0)
                        {
                            // 发送邮件
                            ISiteConfigFacadeService siteConfigService = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();

                            # region 获取模板内容

                            MessageTemplateQuery msgTempQuery = new MessageTemplateQuery();
                            msgTempQuery.JournalID = SiteConfig.SiteID;
                            msgTempQuery.ModelType = 1;
                            msgTempQuery.TCategory = -2;
                            msgTempQuery.TType = 1;
                            MessageTemplateEntity msgTempEntity = siteConfigService.GetMessageTempModel(msgTempQuery);
                            if (msgTempEntity == null)
                            {
                                sendResult.result = EnumJsonResult.error.ToString();
                                sendResult.msg = "请在后台邮件模板设置中设置忘记密码的邮件模板";
                            }
                            else
                            {
                                IDictionary<string, string> dict = siteConfigService.GetEmailVariable();
                                dict["${接收人}$"] = authorEntity.RealName;
                                dict.Add("${AuthorName}$", authorEntity.RealName);
                                dict.Add("${SiteUrl}$", GetAdminUrl());
                                dict.Add("${AuthorID}$", authorEntity.AuthorID.ToString());
                                dict.Add("${TokenCode}$", strToekCode);

                                string MailContent = siteConfigService.GetEmailOrSmsContent(dict, msgTempEntity.TContent);

                                # region 发送邮件

                                MessageRecodeEntity messageEntity = new MessageRecodeEntity();
                                messageEntity.MsgType = 1;
                                messageEntity.JournalID = SiteConfig.SiteID;
                                messageEntity.MsgTitle = "忘记密码:" + SiteConfig.SiteName;
                                messageEntity.MsgContent = MailContent;
                                messageEntity.SendUser = 0;

                                IList<String> ReciveAddressList = new List<string>(1);
                                ReciveAddressList.Add(LoginName);
                                sendResult = siteConfigService.SendEmailOrSms(ReciveAddressList, messageEntity);

                                # endregion
                            }
                            # endregion
                        }
                        else
                        {
                            sendResult.result = EnumJsonResult.error.ToString();
                            sendResult.msg = "令牌服务异常，请稍后再试";
                        }
                    }
                    else
                    {
                        sendResult.result = EnumJsonResult.error.ToString();
                        sendResult.msg = "输入的邮箱错误，请验证是否正确";
                    }
                }
                else
                {
                    sendResult.result = EnumJsonResult.error.ToString();
                    sendResult.msg = "请输入正确的验证码";
                }
            }
            else
            {
                sendResult.result = EnumJsonResult.error.ToString();
                sendResult.msg = "请输入注册邮箱";
            }
            return Content(JsonConvert.SerializeObject(sendResult));
        }

        # endregion

        # region 重置密码

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <param name="TokenCode"></param>
        /// <returns></returns>
        public ActionResult ResetPwd(long AuthorID, string Token)
        {
            if (string.IsNullOrEmpty(Token))
            {
                return Content("重置密码令牌为空");
            }
            else
            {
                // 验证令牌正确性
                TokenQuery tokenQuery = new TokenQuery();
                tokenQuery.JournalID = SiteConfig.SiteID;
                tokenQuery.AuthorID = AuthorID;
                tokenQuery.Token = Token;
                tokenQuery.ExpireDate = DateTime.Now.AddHours(-2);
                IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                TokenEntity tokenEntity = authorService.GetToken(tokenQuery);
                if (tokenEntity == null)
                {
                    return Content("重置密码令牌不存在或已过期，请重新获取密码重置令牌.");
                }
                ViewBag.AuthorID = AuthorID;
                ViewBag.TokenCode = Token;
            }
            return View();
        }

        [AjaxRequest]
        public ActionResult ResetPwdAjax(string VerifyCode, string Pwd, string NewPwd, long AuthorID, string TokenCode)
        {
            ExecResult execResult = new ExecResult();
            if (!string.IsNullOrEmpty(VerifyCode))
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
                    // 验证令牌正确性
                    TokenQuery tokenQuery = new TokenQuery();
                    tokenQuery.JournalID = SiteConfig.SiteID;
                    tokenQuery.AuthorID = AuthorID;
                    tokenQuery.Token = TokenCode;
                    tokenQuery.ExpireDate = DateTime.Now.AddHours(-2);
                    IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                    TokenEntity tokenEntity = authorService.GetToken(tokenQuery);
                    if (tokenEntity == null)
                    {
                        execResult.msg = "重置密码令牌不存在或已过期，请重新获取密码重置令牌.";
                        execResult.result = EnumJsonResult.failure.ToString();
                    }
                    else
                    {
                        if (string.Compare(Pwd, NewPwd) == 0)
                        {
                            AuthorInfoEntity authorEntity = new AuthorInfoEntity();
                            authorEntity.AuthorID = AuthorID;
                            authorEntity.JournalID = SiteConfig.SiteID;
                            authorEntity.Pwd = "";
                            authorEntity.NewPwd = NewPwd;
                            execResult = authorService.EditPwd(authorEntity);
                        }
                        else
                        {
                            execResult.msg = "两次输入的密码不一致，请确认.";
                            execResult.result = EnumJsonResult.failure.ToString();
                        }
                    }
                }
                else
                {
                    execResult.msg = "请输入正确的验证码.";
                    execResult.result = EnumJsonResult.failure.ToString();
                }
            }
            else
            {
                execResult.msg = "请输入验证码.";
                execResult.result = EnumJsonResult.failure.ToString();
            }

            return Content(JsonConvert.SerializeObject(execResult));
        }

        # endregion

        # region 专家自动登录

        /// <summary>
        /// 专家自动登录
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <param name="TokenCode"></param>
        /// <returns></returns>
        public ActionResult ExpertRedirect(long AuthorID, string Token)
        {
            if (string.IsNullOrEmpty(Token))
            {
                return Content("重置密码令牌为空");
            }
            else
            {
                // 验证令牌正确性
                TokenQuery tokenQuery = new TokenQuery();
                tokenQuery.JournalID = SiteConfig.SiteID;
                tokenQuery.AuthorID = AuthorID;
                tokenQuery.Token = Token;
                tokenQuery.ExpireDate = DateTime.Now.AddMonths(-3);
                IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                TokenEntity tokenEntity = authorService.GetToken(tokenQuery);
                if (tokenEntity == null)
                {
                    return Content("自动登录令牌不存在或已过期，请重新获取登录链接或<a href=\"" + SiteConfig.RootPath + "/\">输入用户名密码进行登录.</a>");
                }

                // 验证是否是专家
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                AuthorDetailQuery authorQuery = new AuthorDetailQuery();
                authorQuery.JournalID = SiteConfig.SiteID;
                authorQuery.GroupID = 3;
                authorQuery.CurrentPage = 1;
                authorQuery.PageSize = 1;
                authorQuery.AuthorIDs = new long[] { AuthorID };
                Pager<AuthorDetailEntity> expertPager = service.GetAuthorDetailPageList(authorQuery);
                if (expertPager != null && expertPager.ItemList.Count == 1)
                {
                    AuthorInfoEntity authorEntity = new AuthorInfoEntity();
                    authorEntity.AuthorID = AuthorID;
                    authorEntity.GroupID = 3;
                    authorEntity.JournalID = SiteConfig.SiteID;
                    authorEntity.RealName = expertPager.ItemList[0].AuthorModel.RealName;
                    authorEntity.LoginName = expertPager.ItemList[0].AuthorModel.LoginName;
                    // 保存登录ticket
                    TicketTool.SetCookie(authorEntity.AuthorID.ToString(), JsonConvert.SerializeObject(authorEntity), false);
                }
                else
                {
                    return Content("请确认您的ID正确，或<a href=\"" + SiteConfig.RootPath + "/\">输入用户名密码进行登录.</a>");
                }
            }
            Response.Redirect(SiteConfig.RootPath + "/", true);
            return Content("");
        }

        # endregion

        # region 作者自动登录
        /// <summary>
        /// 作者自动登录
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <param name="TokenCode"></param>
        /// <returns></returns>
        public ActionResult AuthorRedirect(long AuthorID, string Token)
        {
            if (string.IsNullOrEmpty(Token))
            {
                return Content("重置密码令牌为空");
            }
            else
            {
                // 验证令牌正确性
                TokenQuery tokenQuery = new TokenQuery();
                tokenQuery.JournalID = SiteConfig.SiteID;
                tokenQuery.AuthorID = AuthorID;
                tokenQuery.Token = Token;
                tokenQuery.ExpireDate = DateTime.Now.AddMonths(-3);
                IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                TokenEntity tokenEntity = authorService.GetToken(tokenQuery);
                if (tokenEntity == null)
                {
                    return Content("自动登录令牌不存在或已过期，请重新获取登录链接或<a href=\"" + SiteConfig.RootPath + "/\">输入用户名密码进行登录.</a>");
                }

                // 验证是否是作者
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                AuthorDetailQuery authorQuery = new AuthorDetailQuery();
                authorQuery.JournalID = SiteConfig.SiteID;
                authorQuery.GroupID = 2;
                authorQuery.CurrentPage = 1;
                authorQuery.PageSize = 1;
                authorQuery.AuthorIDs = new long[] { AuthorID };
                Pager<AuthorDetailEntity> authorPager = service.GetAuthorDetailPageList(authorQuery);
                if (authorPager != null && authorPager.ItemList.Count == 1)
                {
                    AuthorInfoEntity authorEntity = new AuthorInfoEntity();
                    authorEntity.AuthorID = AuthorID;
                    authorEntity.GroupID = 2;
                    authorEntity.JournalID = SiteConfig.SiteID;
                    authorEntity.RealName = authorPager.ItemList[0].AuthorModel.RealName;
                    authorEntity.LoginName = authorPager.ItemList[0].AuthorModel.LoginName;
                    // 保存登录ticket
                    TicketTool.SetCookie(authorEntity.AuthorID.ToString(), JsonConvert.SerializeObject(authorEntity), false);
                }
                else
                {
                    return Content("请确认您的ID正确，或<a href=\"" + SiteConfig.RootPath + "/\">输入用户名密码进行登录.</a>");
                }
            }
            Response.Redirect(SiteConfig.RootPath + "/", true);
            return Content("");
        }

        /// <summary>
        /// 激活用户账号
        /// </summary>
        public ActionResult Activate(long AuthorID, string Token)
        {
            if (!string.IsNullOrEmpty(Token))
            {
                // 验证令牌正确性
                TokenQuery tokenQuery = new TokenQuery();
                tokenQuery.JournalID = SiteConfig.SiteID;
                tokenQuery.AuthorID = AuthorID;
                tokenQuery.Token = Token;
                tokenQuery.ExpireDate = DateTime.Now.AddMonths(-3);
                IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                TokenEntity tokenEntity = authorService.GetToken(tokenQuery);
                if (tokenEntity == null)
                {
                    return Content("自动登录令牌不存在或已过期，请重新获取登录链接或<a href=\"" + SiteConfig.RootPath + "/\">输入用户名密码进行登录.</a>");
                }
                AuthorInfoEntity currentAuthorEntity = new AuthorInfoEntity() { Status = 1, AuthorID = AuthorID, GroupID = 2 };
                //跟新作者账号为有效
                authorService.EditMember(currentAuthorEntity);
                // 保存登录ticket

                // 验证是否是作者
                IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                AuthorDetailQuery authorQuery = new AuthorDetailQuery();
                authorQuery.JournalID = SiteConfig.SiteID;
                authorQuery.GroupID = 2;
                authorQuery.CurrentPage = 1;
                authorQuery.PageSize = 1;
                authorQuery.AuthorIDs = new long[] { AuthorID };
                Pager<AuthorDetailEntity> authorPager = service.GetAuthorDetailPageList(authorQuery);
                if (authorPager != null && authorPager.ItemList.Count == 1)
                {
                    AuthorInfoEntity authorEntity = new AuthorInfoEntity();
                    authorEntity.AuthorID = AuthorID;
                    authorEntity.GroupID = 2;
                    authorEntity.JournalID = SiteConfig.SiteID;
                    authorEntity.Status = 1;
                    authorEntity.RealName = authorPager.ItemList[0].AuthorModel.RealName;
                    authorEntity.LoginName = authorPager.ItemList[0].AuthorModel.LoginName;

                    TicketTool.SetCookie(authorEntity.AuthorID.ToString(), JsonConvert.SerializeObject(authorEntity), false);
                }
                else
                {
                    return Content("请确认您的ID正确，或<a href=\"" + SiteConfig.RootPath + "/User/Login\">输入用户名密码进行登录.</a>");
                }
            }
            Response.Redirect(TypeParse.ToLong(ConfigurationManager.AppSettings["RootPath"]) + "/User/Login", true);
            return Content("");
        }
        # endregion
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            TicketTool.Logout();
            #region 删除登录名Cookie
            HttpCookie userNameCookie = new HttpCookie(PRELOGINUSERNAME, "");
            userNameCookie.Expires = DateTime.Now.AddDays(0);
            Response.Cookies.Add(userNameCookie);
            #endregion
            #region 删除登录IDCookie
            HttpCookie userIDCookie = new HttpCookie(PRELOGINUSERID, "");
            userIDCookie.Expires = DateTime.Now.AddDays(0);
            Response.Cookies.Add(userIDCookie);
            #endregion
            Response.Write("<script>alert('已成功退出登录!');</script>");
            Response.Redirect(SiteConfig.RootPath + "/", true);
            return Content("");
        }

        # region 根据登录参数设置登录分组

        /// <summary>
        /// 根据登录参数设置登录分组
        /// </summary>
        /// <param name="authorEntity"></param>
        /// <param name="Group"></param>
        /// <returns></returns>
        private Byte GetGroup(AuthorInfoEntity authorEntity, string Group)
        {
            if (!string.IsNullOrEmpty(Group))
            {
                if (authorEntity.GroupID == (byte)EnumMemberGroup.Author)// 如果是作者，则只能以作者身份登录
                {
                    // 如果作者要以专家身份登录，则判断该作者是否是专家
                    if (string.Compare(Group, EnumMemberGroup.Expert.ToString(), true) == 0)
                    {
                        if (authorEntity.RoleIDList.Contains((long)EnumMemberGroup.Expert))
                        {
                            return (byte)EnumMemberGroup.Expert;
                        }
                        else
                        {
                            return (byte)EnumMemberGroup.Author;
                        }
                    }
                    else
                    {
                        return (byte)EnumMemberGroup.Author;
                    }
                }
                else if (authorEntity.GroupID == (byte)EnumMemberGroup.Expert)// 如果是专家，可以以作者、专家身份登录
                {
                    if (string.Compare(Group, EnumMemberGroup.Editor.ToString(), true) == 0)
                    {
                        return (byte)EnumMemberGroup.Author;
                    }
                    else if (string.Compare(Group, EnumMemberGroup.Author.ToString(), true) == 0)
                    {
                        return (byte)EnumMemberGroup.Author;
                    }
                    else
                    {
                        return (byte)EnumMemberGroup.Expert;
                    }
                }
                else if (authorEntity.GroupID == (byte)EnumMemberGroup.Editor) // 如果是编辑部，可以以作者、专家身份、其他角色登录
                {
                    if (string.Compare(Group, EnumMemberGroup.Author.ToString(), true) == 0)
                    {
                        return (byte)EnumMemberGroup.Author;
                    }
                    else if (string.Compare(Group, EnumMemberGroup.Expert.ToString(), true) == 0)
                    {
                        return (byte)EnumMemberGroup.Expert;
                    }
                    else
                    {
                        return (byte)EnumMemberGroup.Editor;
                    }
                }
                else
                {
                    return authorEntity.GroupID;
                }
            }
            else
            {
                return authorEntity.GroupID;
            }
        }

        # endregion

        /// <summary>
        /// 获取管理页面地址
        /// </summary>
        /// <returns></returns>
        private string GetAdminUrl()
        {
            return "http://" + Request.Url.Host + SiteConfig.RootPath;
        }


        /// <summary>
        /// 获取作者链接
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        public string AuditAuthorContributionUrl(long AuthorID)
        {
            # region 新增自动登陆Token

            string strToekCode = RadomCode.GenerateCode(10) + DateTime.Now.Ticks;
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            TokenEntity getPwdToken = new TokenEntity();
            getPwdToken.Token = strToekCode;
            getPwdToken.JournalID = SiteConfig.SiteID;
            getPwdToken.Type = 2; // 审稿连接Toekn
            getPwdToken.AuthorID = AuthorID;
            authorService.InsertToken(getPwdToken);

            # endregion

            return "http://" + Utils.GetHost() + SiteConfig.RootPath + "/user/Activate?AuthorID=" + AuthorID + "&Token=" + strToekCode;
        }

        /// <summary>
        /// 编辑为作者投稿 获取作者自动登录链接
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AuditAuthorEditorUrl(long AuthorID)
        {
            # region 新增自动登陆Token

            string strToekCode = RadomCode.GenerateCode(10) + DateTime.Now.Ticks;
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            TokenEntity getPwdToken = new TokenEntity();
            getPwdToken.Token = strToekCode;
            getPwdToken.JournalID = SiteConfig.SiteID;
            getPwdToken.Type = 2; // 审稿连接Toekn
            getPwdToken.AuthorID = AuthorID;
            authorService.InsertToken(getPwdToken);

            # endregion

            return Json(new { flag = "http://" + Utils.GetHost() + SiteConfig.RootPath + "/user/AuthorRedirect?AuthorID=" + AuthorID + "&Token=" + strToekCode });
        }

    }
}
