using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public partial class ExpertApplyBusiness : IExpertApplyBusiness
    {
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="PKID">唯一标识</param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public ExpertApplyLogEntity GetExpertApplyInfo(long PKID)
        {
            return ExpertApplyDataAccess.Instance.GetExpertApplyInfo(PKID);
        }
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="query">专家申请信息</param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public ExpertApplyLogEntity GetExpertApplyInfo(ExpertApplyLogQuery query)
        {
            return ExpertApplyDataAccess.Instance.GetExpertApplyInfo(query);
        }
        /// <summary>
        /// 根据条件获取所有实体对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<ExpertApplyLogEntity> GetExpertApplyInfoList(ExpertApplyLogQuery query)
        {
            return ExpertApplyDataAccess.Instance.GetExpertApplyInfoList(query);
        }
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ExpertApplyLogEntity> GetExpertApplyInfoPageList(ExpertApplyLogQuery query)
        {
            return ExpertApplyDataAccess.Instance.GetExpertApplyInfoPageList(query);
        }
        /// <summary>
        /// 持久化一个新对象
        /// </summary>
        /// <param name="expertApplyLogEntity"></param>
        /// <returns></returns>
        public bool SubmitApply(ExpertApplyLogEntity expertApplyLogEntity)
        {
            return ExpertApplyDataAccess.Instance.SubmitApply(expertApplyLogEntity);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="expertApplyLogEntity"></param>
        /// <returns></returns>
        public bool UpdateApply(ExpertApplyLogEntity expertApplyLogEntity)
        {
            return ExpertApplyDataAccess.Instance.UpdateApply(expertApplyLogEntity);
        }
        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="PKID"></param>
        /// <returns></returns>
        public bool DelApply(long PKID)
        {
            return ExpertApplyDataAccess.Instance.DelApply(PKID);
        }
    }
}
