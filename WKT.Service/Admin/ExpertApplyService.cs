using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;

namespace WKT.Service
{
    public partial class ExpertApplyService : IExpertApplyService
    {
         /// <summary>
        /// 总线接口
        /// </summary>
        private IExpertApplyBusiness expertApplyBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IExpertApplyBusiness ExpertApplyBusProvider
        {
            get
            {
                if (expertApplyBusProvider == null)
                {
                    expertApplyBusProvider = new ExpertApplyBusiness();
                }
                return expertApplyBusProvider;
            }
            set
            {
                expertApplyBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExpertApplyService()
        {
        }
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="PKID"></param>
        /// <returns></returns>
        public ExpertApplyLogEntity GetExpertApplyInfo(long PKID)
        {
            return ExpertApplyBusProvider.GetExpertApplyInfo(PKID);
        }
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExpertApplyLogEntity GetExpertApplyInfo(ExpertApplyLogQuery query)
        {
            return ExpertApplyBusProvider.GetExpertApplyInfo(query);
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<ExpertApplyLogEntity> GetExpertApplyInfoList(ExpertApplyLogQuery query)
        {
            return ExpertApplyBusProvider.GetExpertApplyInfoList(query);
        }
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ExpertApplyLogEntity> GetExpertApplyInfoPageList(ExpertApplyLogQuery query)
        {
            return ExpertApplyBusProvider.GetExpertApplyInfoPageList(query);
        }
        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="expertApplyLogEntity"></param>
        /// <returns></returns>
        public bool SubmitApply(ExpertApplyLogEntity expertApplyLogEntity)
        {
            return ExpertApplyBusProvider.SubmitApply(expertApplyLogEntity);
        }
        /// <summary>
        /// 更新申请信息
        /// </summary>
        /// <param name="expertApplyLogEntity"></param>
        /// <returns></returns>
        public bool UpdateApply(ExpertApplyLogEntity expertApplyLogEntity)
        {
            return ExpertApplyBusProvider.UpdateApply(expertApplyLogEntity);
        }
        /// <summary>
        /// 删除申请信息
        /// </summary>
        /// <param name="expertApplyLogEntity"></param>
        /// <returns></returns>
        public bool DelApply(long PKID)
        {
            return ExpertApplyBusProvider.DelApply(PKID);
        }
    }
}
