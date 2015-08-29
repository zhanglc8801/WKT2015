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
    public partial class FlowActionService : IFlowActionService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IFlowActionBusiness flowActionBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IFlowActionBusiness FlowActionBusProvider
        {
            get
            {
                 if(flowActionBusProvider == null)
                 {
                      flowActionBusProvider = new FlowActionBusiness();//ServiceBusContainer.Instance.Container.Resolve<IFlowActionBusiness>();
                 }
                 return flowActionBusProvider;
            }
            set
            {
              flowActionBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FlowActionService()
        {
        }
        
        #region 根据条件获取所有实体对象

        /// <summary>
        /// 获取操作实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public FlowActionEntity GetFlowActionEntity(FlowActionQuery query)
        {
            return FlowActionBusProvider.GetFlowActionEntity(query);
        }

        /// <summary>
        /// 根据当前操作状态获取可以做的操作
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<FlowActionEntity> GetFlowActionByStatus(FlowActionQuery query)
        {
            return FlowActionBusProvider.GetFlowActionByStatus(query);
        }

        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="flowActionQuery">FlowActionQuery查询实体对象</param>
        /// <returns>List<FlowActionEntity></returns>
        public List<FlowActionEntity> GetFlowActionList(FlowActionQuery flowActionQuery)
        {
            return FlowActionBusProvider.GetFlowActionList(flowActionQuery);
        }
        
        #endregion 
        
        #region 新增
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="flowAction">FlowActionEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddFlowAction(FlowActionEntity flowAction)
        {
            return FlowActionBusProvider.AddFlowAction(flowAction);
        }
        
        #endregion
        
        #region 更新
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="flowAction">FlowActionEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateFlowAction(FlowActionEntity flowAction)
        {
            return FlowActionBusProvider.UpdateFlowAction(flowAction);
        }

        #endregion
        
        #region 删除
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="flowAction">FlowActionEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteFlowAction(FlowActionEntity flowAction)
        {
            return FlowActionBusProvider.DeleteFlowAction(flowAction);
        }
        
        #endregion
    }
}
