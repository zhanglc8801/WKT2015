using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface IFlowStatusBusiness
    {
        /// <summary>
        /// 获取审稿状态序号
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        int GetFlowStatusSortID(FlowStatusQuery query);

        /// <summary>
        /// 判断审稿状态对应的稿件状态是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        FlowStatusEntity CheckCStatusIsExists(FlowStatusQuery query);

        /// <summary>
        /// 根据指定的审稿状态ID，得到审稿状态的基本信息
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="StatusID"></param>
        /// <returns></returns>
        FlowStatusEntity GetFlowStatusInfoByID(FlowStatusQuery query);

        #region 获取审稿状态基本信息及配置信息

        /// <summary>
        /// 获取审稿状态基本信息及配置信息
        /// </summary>
        /// <param name="flowSetQuery"></param>
        /// <returns></returns>
        FlowStep GetFlowStep(FlowStatusQuery flowStatusQuery);
        
        #endregion
        
        #region 根据条件获取所有实体对象

        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="flowSetQuery">FlowSetQuery查询实体对象</param>
        /// <returns>List<FlowSetEntity></returns>
        List<FlowStatusEntity> GetFlowStatusList(FlowStatusQuery flowStatusQuery);
        
        #endregion 

        #region 新增审稿流程环节及配置

        /// <summary>
        /// 新增审稿流程环节及配置
        /// </summary>
        /// <param name="flowStepEntity">FlowStep实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddFlowStatus(FlowStep flowStepEntity);
        
        #endregion 
        
        #region 更新流程状态基本信息及配置信息

        /// <summary>
        /// 更新流程状态基本信息及配置信息
        /// </summary>
        /// <param name="flowStepEntity">FlowStep实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateFlowStatus(FlowStep flowStepEntity);
        
        #endregion 
       
        #region 删除审稿流程状态及配置信息
        
        /// <summary>
        /// 删除审稿流程状态及配置信息
        /// </summary>
        /// <param name="flowStatus">FlowStatusEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteFlowStatus(FlowStatusEntity flowStatus);
        
        #endregion 

        #region 修改审稿状态状态

        /// <summary>
        /// 修改审稿状态状态
        /// </summary>
        /// <param name="flowStatusEntity"></param>
        /// <returns></returns>
        bool UpdateFlowStatusStatus(FlowStatusEntity flowStatusEntity);

        # endregion

        # region 获取审稿键值对数据

        /// <summary>
        /// 获取拥有权限的审稿状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<FlowStatusEntity> GetHaveRightFlowStatus(FlowStatusQuery query);

        /// <summary>
        /// 获取拥有权限的审稿状态(用于统计同一稿件一个状态下送多人时按一个计算)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<FlowStatusEntity> GetHaveRightFlowStatusForStat(FlowStatusQuery query);

        /// <summary>
        /// 获取审稿状态键值对，审稿状态名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IDictionary<long, string> GetFlowStatusDictStatusName(FlowStatusQuery query);

        /// <summary>
        /// 获取审稿状态键值对，审稿状态显示名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IDictionary<long, string> GetFlowStatusDictDisplayName(FlowStatusQuery query);

        # endregion
    }
}






