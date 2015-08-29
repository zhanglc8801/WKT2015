using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;

using WKT.Common.Security;

namespace Web.API.Controllers
{
    /// <summary>
    /// 安全验证属性
    /// </summary>
    public class AuthAttribute : ActionFilterAttribute
    {
        private const string RQUESTHEADERTOKENKEY = "Authorization-Token";
        private const string AUTHSITE = "Request-Site";
        private const string AUTHSITEID = "Request-SiteID";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string token;

            try
            {
                // 首先判断请求是否包含了Token，如果没有包含可返回异常请求信息
                if (actionContext.Request.Headers.Contains(RQUESTHEADERTOKENKEY))
                {
                    token = actionContext.Request.Headers.GetValues(RQUESTHEADERTOKENKEY).First();
                    string authSiteID = actionContext.Request.Headers.GetValues(AUTHSITEID).First();
                    string authSite = actionContext.Request.Headers.GetValues(AUTHSITE).First();/// TODO:这里还可以对授权站点进行验证
                    // 验证Token是否正确
                    if (RSAClass.Decrypt(token) == (authSite + authSiteID + DateTime.Now.ToString("yyyyMMdd")))
                    {
                        base.OnActionExecuting(actionContext);
                    }
                    else
                    {
                        // 验证失败
                        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                        {
                            Content = new StringContent("Unauthorized Site")
                        };
                        return;
                    }
                }
                else
                {
                    // 验证失败
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("Unauthorized Site")
                    };
                    return;
                }
            }
            catch (Exception)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Missing Authorization-Token")
                };
                return;
            }
        }
    }
}