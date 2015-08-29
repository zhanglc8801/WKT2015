using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Model;

namespace WKT.Facade.Service.Interface
{
    public interface IExpertApplyFacadeService
    {
        ExpertApplyLogEntity GetExpertApplyInfo(ExpertApplyLogQuery query);

        IList<ExpertApplyLogEntity> GetExpertApplyInfoList(ExpertApplyLogQuery query);

        Pager<ExpertApplyLogEntity> GetExpertApplyInfoPageList(ExpertApplyLogQuery query);

        ExecResult SubmitApply(ExpertApplyLogEntity expertApplyLogEntity);

        ExecResult UpdateApply(ExpertApplyLogEntity expertApplyLogEntity);

        ExecResult DelApply(long PKID);


    }
}
