using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface IExpertApplyBusiness
    {
        /// <summary>
        /// 获取申请信息
        /// </summary>
        /// <param name="PKID"></param>
        /// <returns></returns>
        ExpertApplyLogEntity GetExpertApplyInfo(Int64 PKID);

        /// <summary>
        /// 获取申请信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExpertApplyLogEntity GetExpertApplyInfo(ExpertApplyLogQuery query);

        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<ExpertApplyLogEntity> GetExpertApplyInfoList(ExpertApplyLogQuery query);

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<AuthorInfoEntity></returns>
        Pager<ExpertApplyLogEntity> GetExpertApplyInfoPageList(ExpertApplyLogQuery query);

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="expertApplyLogEntity"></param>
        /// <returns></returns>
        bool SubmitApply(ExpertApplyLogEntity expertApplyLogEntity);

        /// <summary>
        /// 更新申请信息
        /// </summary>
        /// <param name="expertApplyLogEntity"></param>
        /// <returns></returns>
        bool UpdateApply(ExpertApplyLogEntity expertApplyLogEntity);

        /// <summary>
        /// 删除申请信息
        /// </summary>
        /// <param name="expertApplyLogEntity"></param>
        /// <returns></returns>
        bool DelApply(long PKID);

    }
}
