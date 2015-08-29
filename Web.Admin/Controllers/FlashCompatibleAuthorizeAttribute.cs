using System;
using System.Security;
using System.Security.Principal;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Admin.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class FlashCompatibleAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// The key to the authentication token that should be submitted somewhere in the request.
        /// </summary>
        private const string TOKEN_KEY = "authCookie";

        /// <summary>
        /// This changes the behavior of AuthorizeCore so that it will only authorize
        /// users if a valid token is submitted with the request.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            string token = httpContext.Request.Params[TOKEN_KEY];

            if (token != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

                if (ticket != null)
                {
                    FormsIdentity identity = new FormsIdentity(ticket);
                    string[] roles = System.Web.Security.Roles.GetRolesForUser(identity.Name);
                    GenericPrincipal principal = new GenericPrincipal(identity, roles);
                    httpContext.User = principal;
                }
            }

            return base.AuthorizeCore(httpContext);
        }
    }
}
