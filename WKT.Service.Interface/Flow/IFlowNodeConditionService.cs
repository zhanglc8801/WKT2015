using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface IFlowNodeConditionService
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="flowConditionID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        FlowNodeConditionEntity GetFlowNodeCondition(Int64 flowConditionID);
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<FlowNodeConditionEntity></returns>
        List<FlowNodeConditionEntity> GetFlowNodeConditionList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="flowNodeConditionQuery">FlowNodeConditionQuery查询实体对象</param>
        /// <returns>List<FlowNodeConditionEntity></returns>
        List<FlowNodeConditionEntity> GetFlowNodeConditionList(FlowNodeConditionQuery flowNodeConditionQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<FlowNodeConditionEntity></returns>
        Pager<FlowNodeConditionEntity> GetFlowNodeConditionPageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<FlowNodeConditionEntity></returns>
        Pager<FlowNodeConditionEntity> GetFlowNodeConditionPageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="flowNodeConditionQuery">FlowNodeConditionQuery查询实体对象</param>
        /// <returns>Pager<FlowNodeConditionEntity></returns>
        Pager<FlowNodeConditionEntity> GetFlowNodeConditionPageList(FlowNodeConditionQuery flowNodeConditionQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="flowNodeCondition">FlowNodeConditionEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddFlowNodeCondition(FlowNodeConditionEntity flowNodeCondition);
        
        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="flowNodeCondition">FlowNodeConditionEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateFlowNodeCondition(FlowNodeConditionEntity flowNodeCondition);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="flowConditionID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteFlowNodeCondition(Int64 flowConditionID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="flowNodeCondition">FlowNodeConditionEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteFlowNodeCondition(FlowNodeConditionEntity flowNodeCondition);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="flowConditionID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteFlowNodeCondition(Int64[] flowConditionID);
        
        #endregion
        
        #endregion 
    }
}






