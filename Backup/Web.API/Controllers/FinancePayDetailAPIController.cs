using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Service.Interface;
using WKT.Service.Wrapper;
using WKT.Model.Enum;

namespace Web.API.Controllers
{
    public class FinancePayDetailAPIController : ApiBaseController
    {
        /// <summary>
        /// 保存支付记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Add(FinancePayDetailEntity model)
        {
            IFinancePayDetailService service = ServiceContainer.Instance.Container.Resolve<IFinancePayDetailService>();
            bool result = service.AddFinancePayDetail(model);
            ExecResult execResult = new ExecResult();
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "新增支付记录成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "新增支付记录失败！";
            }
            return execResult;
        }

    }
}
