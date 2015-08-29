using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface IFlowActionBusiness
    {       
        #region 根据条件获取所有实体对象

        /// <summary>
        /// 获取操作实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        FlowActionEntity GetFlowActionEntity(FlowActionQuery query);

        /// <summary>
        /// 根据当前操作状态获取可以做的操作
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<FlowActionEntity> GetFlowActionByStatus(FlowActionQuery query);
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="flowActionQuery">FlowActionQuery查询实体对象</param>
        /// <returns>List<FlowActionEntity></returns>
        List<FlowActionEntity> GetFlowActionList(FlowActionQuery flowActionQuery);
        
        #endregion 
        
        #region 新增
        
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="flowAction">FlowActionEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddFlowAction(FlowActionEntity flowAction);
        
        #endregion 
        
        #region 更新
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="flowAction">FlowActionEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateFlowAction(FlowActionEntity flowAction);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="flowAction">FlowActionEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteFlowAction(FlowActionEntity flowAction);
        
        #endregion 
    }
}






