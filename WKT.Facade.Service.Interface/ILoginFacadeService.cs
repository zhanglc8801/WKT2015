using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Model;

namespace WKT.Facade.Service.Interface
{
    public interface ILoginFacadeService
    {
        /// <summary>
        /// 记录登录日志信息
        /// </summary>
        /// <param name="loginErrorLogEntity"></param>
        /// <returns></returns>
        ExecResult AddLoginErrorLog(LoginErrorLogEntity loginErrorLogEntity);

        /// <summary>
        /// 获取登录错误日志信息
        /// </summary>
        /// <param name="loginErrorLogQuery"></param>
        /// <returns></returns>
        IList<LoginErrorLogEntity> GetLoginErrorLogList(LoginErrorLogQuery loginErrorLogQuery);

        /// <summary>
        /// 删除登录错误日志
        /// </summary>
        /// <param name="loginErrorLogQuery"></param>
        /// <returns></returns>
        ExecResult DeleteLoginErrorLog(LoginErrorLogQuery loginErrorLogQuery);

    }
}
