using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public partial class FlowNodeConditionBusiness : IFlowNodeConditionBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="flowConditionID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public FlowNodeConditionEntity GetFlowNodeCondition(Int64 flowConditionID)
        {
           return FlowNodeConditionDataAccess.Instance.GetFlowNodeCondition( flowConditionID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<FlowNodeConditionEntity></returns>
        public List<FlowNodeConditionEntity> GetFlowNodeConditionList()
        {
            return FlowNodeConditionDataAccess.Instance.GetFlowNodeConditionList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="flowNodeConditionQuery">FlowNodeConditionQuery查询实体对象</param>
        /// <returns>List<FlowNodeConditionEntity></returns>
        public List<FlowNodeConditionEntity> GetFlowNodeConditionList(FlowNodeConditionQuery flowNodeConditionQuery)
        {
            return FlowNodeConditionDataAccess.Instance.GetFlowNodeConditionList(flowNodeConditionQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<FlowNodeConditionEntity></returns>
        public Pager<FlowNodeConditionEntity> GetFlowNodeConditionPageList(CommonQuery query)
        {
            return FlowNodeConditionDataAccess.Instance.GetFlowNodeConditionPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<FlowNodeConditionEntity></returns>
        public Pager<FlowNodeConditionEntity> GetFlowNodeConditionPageList(QueryBase query)
        {
            return FlowNodeConditionDataAccess.Instance.GetFlowNodeConditionPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="flowNodeConditionQuery">FlowNodeConditionQuery查询实体对象</param>
        /// <returns>Pager<FlowNodeConditionEntity></returns>
        public Pager<FlowNodeConditionEntity> GetFlowNodeConditionPageList(FlowNodeConditionQuery flowNodeConditionQuery)
        {
            return FlowNodeConditionDataAccess.Instance.GetFlowNodeConditionPageList(flowNodeConditionQuery);
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
            return FlowNodeConditionDataAccess.Instance.AddFlowNodeCondition(flowNodeCondition);
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
            return FlowNodeConditionDataAccess.Instance.UpdateFlowNodeCondition(flowNodeCondition);
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
            return FlowNodeConditionDataAccess.Instance.DeleteFlowNodeCondition( flowConditionID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="flowNodeCondition">FlowNodeConditionEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteFlowNodeCondition(FlowNodeConditionEntity flowNodeCondition)
        {
            return FlowNodeConditionDataAccess.Instance.DeleteFlowNodeCondition(flowNodeCondition);
        }
        
        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="flowConditionID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteFlowNodeCondition(Int64[] flowConditionID)
        {
            return FlowNodeConditionDataAccess.Instance.BatchDeleteFlowNodeCondition( flowConditionID);
        }
        #endregion
        
        #endregion
    }
}
