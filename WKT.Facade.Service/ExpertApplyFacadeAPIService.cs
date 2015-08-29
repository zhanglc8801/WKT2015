using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Log;
using WKT.Config;
using WKT.Common.Utils;
using WKT.Facade.Service.Interface;

namespace WKT.Facade.Service
{
    public class ExpertApplyFacadeAPIService : ServiceBase, IExpertApplyFacadeService
    {
        /// <summary>
        /// 获取专家申请信息实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExpertApplyLogEntity GetExpertApplyInfo(ExpertApplyLogQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExpertApplyLogEntity expertApplyEntity = clientHelper.PostAuth<ExpertApplyLogEntity, ExpertApplyLogQuery>(GetAPIUrl(APIConstant.EXPERTAPPLY_GETMODEL), query);
            return expertApplyEntity;
        }
        /// <summary>
        /// 获取专家申请信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ExpertApplyLogEntity> GetExpertApplyInfoList(ExpertApplyLogQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<ExpertApplyLogEntity> listExpertApplyInfo = clientHelper.PostAuth<IList<ExpertApplyLogEntity>, ExpertApplyLogQuery>(GetAPIUrl(APIConstant.EXPERTAPPLY_GETLIST), query);
            return listExpertApplyInfo;
        }
        /// <summary>
        /// 获取专家申请分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ExpertApplyLogEntity> GetExpertApplyInfoPageList(ExpertApplyLogQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<ExpertApplyLogEntity> pager = clientHelper.Post<Pager<ExpertApplyLogEntity>, ExpertApplyLogQuery>(GetAPIUrl(APIConstant.EXPERTAPPLY_GETPAGELIST), query);
            return pager;
        }
        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="expertApplyLogEntity"></param>
        /// <returns></returns>
        public ExecResult SubmitApply(ExpertApplyLogEntity expertApplyLogEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, ExpertApplyLogEntity>(GetAPIUrl(APIConstant.EXPERTAPPLY_SUBMITAPPLY), expertApplyLogEntity);
            return execResult;
        }
        /// <summary>
        /// 更新申请信息
        /// </summary>
        /// <param name="expertApplyLogEntity"></param>
        /// <returns></returns>
        public ExecResult UpdateApply(ExpertApplyLogEntity expertApplyLogEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, ExpertApplyLogEntity>(GetAPIUrl(APIConstant.EXPERTAPPLY_UPDATEAPPLY), expertApplyLogEntity);
            return execResult;
        }

        /// <summary>
        /// 删除专家申请信息
        /// </summary>
        /// <param name="PKID"></param>
        /// <returns></returns>
        public ExecResult DelApply(long PKID)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, long>(GetAPIUrl(APIConstant.EXPERTAPPLY_DELAPPLY), PKID);
            return execResult;
        }
    }
}
