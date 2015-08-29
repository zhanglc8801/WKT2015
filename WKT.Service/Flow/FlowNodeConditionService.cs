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
    public partial class FlowNodeConditionService:IFlowNodeConditionService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IFlowNodeConditionBusiness flowNodeConditionBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IFlowNodeConditionBusiness FlowNodeConditionBusProvider
        {
            get
            {
                 if(flowNodeConditionBusProvider == null)
                 {
                      flowNodeConditionBusProvider = new FlowNodeConditionBusiness();//ServiceBusContainer.Instance.Container.Resolve<IFlowNodeConditionBusiness>();
                 }
                 return flowNodeConditionBusProvider;
            }
            set
            {
              flowNodeConditionBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FlowNodeConditionService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="flowConditionID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public FlowNodeConditionEntity GetFlowNodeCondition(Int64 flowConditionID)
        {
           return FlowNodeConditionBusProvider.GetFlowNodeCondition( flowConditionID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<FlowNodeConditionEntity></returns>
        public List<FlowNodeConditionEntity> GetFlowNodeConditionList()
        {
            return FlowNodeConditionBusProvider.GetFlowNodeConditionList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="flowNodeConditionQuery">FlowNodeConditionQuery查询实体对象</param>
        /// <returns>List<FlowNodeConditionEntity></returns>
        public List<FlowNodeConditionEntity> GetFlowNodeConditionList(FlowNodeConditionQuery flowNodeConditionQuery)
        {
            return FlowNodeConditionBusProvider.GetFlowNodeConditionList(flowNodeConditionQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<FlowNodeConditionEntity></returns>
        public Pager<FlowNodeConditionEntity> GetFlowNodeConditionPageList(CommonQuery query)
        {
            return FlowNodeConditionBusProvider.GetFlowNodeConditionPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<FlowNodeConditionEntity></returns>
        public Pager<FlowNodeConditionEntity> GetFlowNodeConditionPageList(QueryBase query)
        {
            return FlowNodeConditionBusProvider.GetFlowNodeConditionPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="flowNodeConditionQuery">FlowNodeConditionQuery查询实体对象</param>
        /// <returns>Pager<FlowNodeConditionEntity></returns>
        public Pager<FlowNodeConditionEntity> GetFlowNodeConditionPageList(FlowNodeConditionQuery flowNodeConditionQuery)
        {
            return FlowNodeConditionBusProvider.GetFlowNodeConditionPageList(flowNodeConditionQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="flowNodeCondition">FlowNodeConditionEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddFlowNodeCondition(FlowNodeConditionEntity flowNodeCondition)
        {
            return FlowNodeConditionBusProvider.AddFlowNodeCondition(flowNodeCondition);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="flowNodeCondition">FlowNodeConditionEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateFlowNodeCondition(FlowNodeConditionEntity flowNodeCondition)
        {
            return FlowNodeConditionBusProvider.UpdateFlowNodeCondition(flowNodeCondition);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="flowConditionID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteFlowNodeCondition(Int64 flowConditionID)
        {
            return FlowNodeConditionBusProvider.DeleteFlowNodeCondition( flowConditionID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="flowNodeCondition">FlowNodeConditionEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteFlowNodeCondition(FlowNodeConditionEntity flowNodeCondition)
        {
            return FlowNodeConditionBusProvider.DeleteFlowNodeCondition(flowNodeCondition);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="flowConditionID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteFlowNodeCondition(Int64[] flowConditionID)
        {
            return FlowNodeConditionBusProvider.BatchDeleteFlowNodeCondition( flowConditionID);
        }
        
        #endregion
        
        #endregion
    }
}
