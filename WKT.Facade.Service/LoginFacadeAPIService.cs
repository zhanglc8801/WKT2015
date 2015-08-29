using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Facade.Service.Interface;
using WKT.Model;
using WKT.Common.Utils;

namespace WKT.Facade.Service
{
    public class LoginFacadeAPIService : ServiceBase, ILoginFacadeService
    {

        public ExecResult AddLoginErrorLog(LoginErrorLogEntity loginErrorLogEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, LoginErrorLogEntity>(GetAPIUrl(APIConstant.ADDLOGINERRORLOG), loginErrorLogEntity);
            return execResult;
        }

        public IList<LoginErrorLogEntity> GetLoginErrorLogList(LoginErrorLogQuery loginErrorLogQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<LoginErrorLogEntity> loginErrorLogList = clientHelper.PostAuth<IList<LoginErrorLogEntity>, LoginErrorLogQuery>(GetAPIUrl(APIConstant.GETLOGINERRORLOGLIST), loginErrorLogQuery);
            return loginErrorLogList;
        }

        public ExecResult DeleteLoginErrorLog(LoginErrorLogQuery loginErrorLogQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, LoginErrorLogQuery>(GetAPIUrl(APIConstant.DELETELOGINERRORLOG), loginErrorLogQuery);
            return execResult;
        }

    }
}
