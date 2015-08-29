using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Service.Interface;
using WKT.Service.Wrapper;
using WKT.Log;

namespace Web.API.Controllers
{
    public class LoginAPIController : ApiBaseController
    {
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult AddLoginErrorLog(LoginErrorLogEntity loginErrorLogEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                ILoginService loginService = ServiceContainer.Instance.Container.Resolve<ILoginService>();
                bool flag = loginService.AddLoginErrorLog(loginErrorLogEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "操作失败，请确认登录信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "记录登录错误日志信息时出现异常：" + ex.Message;
            }
            return result;
        }


        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<LoginErrorLogEntity> GetLoginErrorLogList(LoginErrorLogQuery loginErrorLogQuery)
        {
            try
            {
                ILoginService loginService = ServiceContainer.Instance.Container.Resolve<ILoginService>();
                IList<LoginErrorLogEntity> listLoginErrorLog = loginService.GetLoginErrorLogList(loginErrorLogQuery);
                return listLoginErrorLog;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取登录错误日志信息出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult DeleteLoginErrorLog(LoginErrorLogQuery loginErrorLogQuery)
        {
            ExecResult result = new ExecResult();
            try
            {
                ILoginService loginService = ServiceContainer.Instance.Container.Resolve<ILoginService>();
                bool flag = loginService.DeleteLoginErrorLog(loginErrorLogQuery);
                if( flag )
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "操作失败，请确认登录信息是否正确";
                }
            }
            catch( Exception ex )
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "记录登录错误日志信息时出现异常：" + ex.Message;
            }
            return result;
        }

    }
}
