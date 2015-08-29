using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public partial class FlowAuthRoleBusiness : IFlowAuthRoleBusiness
    {   
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="flowAuthRoleEntity"></param>
        /// <returns></returns>
        public bool AddFlowAuthRole(FlowAuthRoleEntity flowAuthRoleEntity)
        {
            return FlowAuthRoleDataAccess.Instance.AddFlowAuthRole(flowAuthRoleEntity);
        }

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="flowAuthRoleQuery">FlowAuthRoleQuery查询实体对象</param>
        /// <returns>Pager<FlowAuthRoleEntity></returns>
        public List<FlowAuthRoleEntity> GetFlowAuthRoleList(FlowAuthRoleQuery flowAuthRoleQuery)
        {
            return FlowAuthRoleDataAccess.Instance.GetFlowAuthRoleList(flowAuthRoleQuery);
        }

        /// <summary>
        /// 设置审稿流程环节角色权限
        /// </summary>
        /// <param name="flowAuthRoleList">审稿流程环节角色权限列表</param>
        /// <returns></returns>
        public bool SaveFlowAuthRole(List<FlowAuthRoleEntity> flowAuthRoleList)
        {
            return FlowAuthRoleDataAccess.Instance.SaveFlowAuthRole(flowAuthRoleList);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteAuthRole(List<FlowAuthRoleEntity> flowAuthRoleList)
        {
            return FlowAuthRoleDataAccess.Instance.BatchDeleteAuthRole(flowAuthRoleList);
        }
    }
}
