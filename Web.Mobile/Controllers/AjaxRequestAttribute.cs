using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Mobile.Controllers
{
    public class AjaxRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ContentResult rContent = new ContentResult();
                rContent.ContentType = "text/html";
                rContent.Content = "{\"result\":\"error\",\"msg\":\"请正确请求\"}";
                filterContext.Result = rContent;
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

    }


}