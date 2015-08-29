using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;

namespace WKT.Service
{
    public partial class FlowStatusService : IFlowStatusService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IFlowStatusBusiness flowStatusBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IFlowStatusBusiness FlowSetBusProvider
        {
            get
            {
                 if(flowStatusBusProvider == null)
                 {
                      flowStatusBusProvider = new FlowStatusBusiness();//ServiceBusContainer.Instance.Container.Resolve<IFlowSetBusiness>();
                 }
                 return flowStatusBusProvider;
            }
            set
            {
              flowStatusBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FlowStatusService()
        {
        }

        /// <summary>
        /// 获取审稿状态序号
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetFlowStatusSortID(FlowStatusQuery query)
        {
            return FlowSetBusProvider.GetFlowStatusSortID(query);
        }

        /// <summary>
        /// 判断审稿状态对应的稿件状态是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public FlowStatusEntity CheckCStatusIsExists(FlowStatusQuery query)
        {
            return FlowSetBusProvider.CheckCStatusIsExists(query);
        }

        /// <summary>
        /// 根据指定的审稿状态ID，得到审稿状态的基本信息
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="StatusID"></param>
        /// <returns></returns>
        public FlowStatusEntity GetFlowStatusInfoByID(FlowStatusQuery query)
        {
            return FlowSetBusProvider.GetFlowStatusInfoByID(query);
        }

        #region 获取审稿流程状态基本信息及配置信息

        /// <summary>
        /// 获取审稿流程状态基本信息及配置信息
        /// </summary>
        /// <param name="flowStatusQuery"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public FlowStep GetFlowStep(FlowStatusQuery flowStatusQuery)
        {
            return FlowSetBusProvider.GetFlowStep(flowStatusQuery);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="flowStatusQuery">FlowStatusQuery查询实体对象</param>
        /// <returns>List<FlowStatusEntity></returns>
        public List<FlowStatusEntity> GetFlowStatusList(FlowStatusQuery flowStatusQuery)
        {
            return FlowSetBusProvider.GetFlowStatusList(flowStatusQuery);
        }
        
        #endregion 
        
        #region 新增审稿流程状态及配置

        /// <summary>
        /// 新增审稿流程状态及配置
        /// </summary>
        /// <param name="flowStepEntity">FlowStep实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddFlowStatus(FlowStep flowStepEntity)
        {
            return FlowSetBusProvider.AddFlowStatus(flowStepEntity);
        }
        
        #endregion

        #region 更新流程状态基本信息及配置信息

        /// <summary>
        /// 更新流程状态基本信息及配置信息
        /// </summary>
        /// <param name="flowStepEntity">FlowStep实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateFlowStatus(FlowStep flowStepEntity)
        {
            return FlowSetBusProvider.UpdateFlowStatus(flowStepEntity);
        }

        #endregion

        #region 删除审稿流程状态及配置信息

        /// <summary>
        /// 删除审稿流程状态及配置信息
        /// </summary>
        /// <param name="flowStatus">FlowStatusEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteFlowStatus(FlowStatusEntity flowStatus)
        {
            return FlowSetBusProvider.DeleteFlowStatus(flowStatus);
        }
        
        #endregion

        #region 修改审稿状态状态

        /// <summary>
        /// 修改审稿状态状态
        /// </summary>
        /// <param name="flowStatusEntity"></param>
        /// <returns></returns>
        public bool UpdateFlowStatusStatus(FlowStatusEntity flowStatusEntity)
        {
            return FlowSetBusProvider.UpdateFlowStatusStatus(flowStatusEntity);
        }

        # endregion

        # region 获取审稿键值对数据

        /// <summary>
        /// 获取拥有权限的审稿状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<FlowStatusEntity> GetHaveRightFlowStatus(FlowStatusQuery query)
        {
            return FlowSetBusProvider.GetHaveRightFlowStatus(query);
        }

        /// <summary>
        /// 获取拥有权限的审稿状态(用于统计同一稿件一个状态下送多人时按一个计算)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<FlowStatusEntity> GetHaveRightFlowStatusForStat(FlowStatusQuery query)
        {
            return FlowSetBusProvider.GetHaveRightFlowStatusForStat(query);
        }

        /// <summary>
        /// 获取审稿状态键值对，审稿状态名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long, string> GetFlowStatusDictStatusName(FlowStatusQuery query)
        {
            return FlowSetBusProvider.GetFlowStatusDictStatusName(query);
        }

        /// <summary>
        /// 获取审稿状态键值对，审稿状态显示名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long, string> GetFlowStatusDictDisplayName(FlowStatusQuery query)
        {
            return FlowSetBusProvider.GetFlowStatusDictDisplayName(query);
        }

        # endregion
    }
}
