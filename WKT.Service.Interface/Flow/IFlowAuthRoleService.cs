using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface IFlowAuthRoleService
    {       
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="flowAuthRoleQuery">FlowAuthRoleQuery查询实体对象</param>
        /// <returns>Pager<FlowAuthRoleEntity></returns>
        List<FlowAuthRoleEntity> GetFlowAuthRoleList(FlowAuthRoleQuery flowAuthRoleQuery);

        /// <summary>
        /// 设置审稿流程环节角色权限
        /// </summary>
        /// <param name="flowAuthRoleList">审稿流程环节角色权限列表</param>
        /// <returns></returns>
        bool SaveFlowAuthRole(List<FlowAuthRoleEntity> flowAuthRoleList);

         /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteAuthRole(List<FlowAuthRoleEntity> flowAuthRoleList);
    }
}






