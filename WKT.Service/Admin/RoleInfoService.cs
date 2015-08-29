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
    public partial class RoleInfoService:IRoleInfoService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IRoleInfoBusiness roleInfoBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IRoleInfoBusiness RoleInfoBusProvider
        {
            get
            {
                 if(roleInfoBusProvider == null)
                 {
                     roleInfoBusProvider = new RoleInfoBusiness();//ServiceBusContainer.Instance.Container.Resolve<IRoleInfoBusiness>();
                 }
                 return roleInfoBusProvider;
            }
            set
            {
              roleInfoBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleInfoService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public RoleInfoEntity GetRoleInfo(Int64 roleID)
        {
           return RoleInfoBusProvider.GetRoleInfo( roleID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<RoleInfoEntity></returns>
        public List<RoleInfoEntity> GetRoleInfoList()
        {
            return RoleInfoBusProvider.GetRoleInfoList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="roleInfoQuery">RoleInfoQuery查询实体对象</param>
        /// <returns>List<RoleInfoEntity></returns>
        public List<RoleInfoEntity> GetRoleInfoList(RoleInfoQuery roleInfoQuery)
        {
            return RoleInfoBusProvider.GetRoleInfoList(roleInfoQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<RoleInfoEntity></returns>
        public Pager<RoleInfoEntity> GetRoleInfoPageList(CommonQuery query)
        {
            return RoleInfoBusProvider.GetRoleInfoPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<RoleInfoEntity></returns>
        public Pager<RoleInfoEntity> GetRoleInfoPageList(QueryBase query)
        {
            return RoleInfoBusProvider.GetRoleInfoPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="roleInfoQuery">RoleInfoQuery查询实体对象</param>
        /// <returns>Pager<RoleInfoEntity></returns>
        public Pager<RoleInfoEntity> GetRoleInfoPageList(RoleInfoQuery roleInfoQuery)
        {
            return RoleInfoBusProvider.GetRoleInfoPageList(roleInfoQuery);
        }

        /// <summary>
        /// 得到角色Dict
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long, string> GetRoleInfoDict(RoleInfoQuery query)
        {
            return RoleInfoBusProvider.GetRoleInfoDict(query);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="roleInfo">RoleInfoEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddRoleInfo(RoleInfoEntity roleInfo)
        {
            return RoleInfoBusProvider.AddRoleInfo(roleInfo);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="roleInfo">RoleInfoEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateRoleInfo(RoleInfoEntity roleInfo)
        {
            return RoleInfoBusProvider.UpdateRoleInfo(roleInfo);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteRoleInfo(Int64 roleID)
        {
            return RoleInfoBusProvider.DeleteRoleInfo( roleID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="roleInfo">RoleInfoEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteRoleInfo(RoleInfoEntity roleInfo)
        {
            return RoleInfoBusProvider.DeleteRoleInfo(roleInfo);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteRoleInfo(RoleInfoQuery queryRole)
        {
            return RoleInfoBusProvider.BatchDeleteRoleInfo(queryRole);
        }
        
        #endregion
        
        #endregion
    }
}
