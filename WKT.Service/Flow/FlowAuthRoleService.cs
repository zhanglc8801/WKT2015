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
    public partial class FlowAuthRoleService:IFlowAuthRoleService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IFlowAuthRoleBusiness flowAuthRoleBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IFlowAuthRoleBusiness FlowAuthRoleBusProvider
        {
            get
            {
                 if(flowAuthRoleBusProvider == null)
                 {
                      flowAuthRoleBusProvider = new FlowAuthRoleBusiness();//ServiceBusContainer.Instance.Container.Resolve<IFlowAuthRoleBusiness>();
                 }
                 return flowAuthRoleBusProvider;
            }
            set
            {
              flowAuthRoleBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FlowAuthRoleService()
        {
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="flowAuthRoleQuery">FlowAuthRoleQuery查询实体对象</param>
        /// <returns>Pager<FlowAuthRoleEntity></returns>
        public List<FlowAuthRoleEntity> GetFlowAuthRoleList(FlowAuthRoleQuery flowAuthRoleQuery)
        {
            return FlowAuthRoleBusProvider.GetFlowAuthRoleList(flowAuthRoleQuery);
        }

        /// <summary>
        /// 设置审稿流程环节角色权限
        /// </summary>
        /// <param name="flowAuthRoleList">审稿流程环节角色权限列表</param>
        /// <returns></returns>
        public bool SaveFlowAuthRole(List<FlowAuthRoleEntity> flowAuthRoleList)
        {
            return FlowAuthRoleBusProvider.SaveFlowAuthRole(flowAuthRoleList);
        }

         /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteAuthRole(List<FlowAuthRoleEntity> flowAuthRoleList)
        {
            return FlowAuthRoleBusProvider.BatchDeleteAuthRole(flowAuthRoleList);
        }
    }
}
