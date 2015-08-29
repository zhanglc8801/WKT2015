using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

using WKT.Log;

namespace Web.API.Controllers
{
    public class NotImplExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                LogProvider.Instance.Error("WebAPI 请求异常:" + context.Exception.ToString());
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                context.Response.Content = new StringContent("WebAPI 请求异常:" + context.Exception.Message);
                base.OnException(context); 
            }
        }
    }
}
