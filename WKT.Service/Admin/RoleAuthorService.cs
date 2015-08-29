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
    public partial class RoleAuthorService:IRoleAuthorService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IRoleAuthorBusiness roleAuthorBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IRoleAuthorBusiness RoleAuthorBusProvider
        {
            get
            {
                 if(roleAuthorBusProvider == null)
                 {
                     roleAuthorBusProvider = new RoleAuthorBusiness();//ServiceBusContainer.Instance.Container.Resolve<IRoleAuthorBusiness>();
                 }
                 return roleAuthorBusProvider;
            }
            set
            {
              roleAuthorBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleAuthorService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public RoleAuthorEntity GetRoleAuthor(Int64 mapID)
        {
           return RoleAuthorBusProvider.GetRoleAuthor( mapID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<RoleAuthorEntity></returns>
        public List<RoleAuthorEntity> GetRoleAuthorList()
        {
            return RoleAuthorBusProvider.GetRoleAuthorList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="roleAuthorQuery">RoleAuthorQuery查询实体对象</param>
        /// <returns>List<RoleAuthorEntity></returns>
        public List<RoleAuthorEntity> GetRoleAuthorList(RoleAuthorQuery roleAuthorQuery)
        {
            return RoleAuthorBusProvider.GetRoleAuthorList(roleAuthorQuery);
        }
        /// <summary>
        /// 获取所有符合查询条件的数据(包含角色名称)
        /// </summary>
        /// <param name="roleAuthorQuery"></param>
        /// <returns></returns>
        public List<RoleAuthorEntity> GetRoleAuthorDetailList(RoleAuthorQuery roleAuthorQuery)
        {
            return RoleAuthorBusProvider.GetRoleAuthorDetailList(roleAuthorQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<RoleAuthorEntity></returns>
        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(CommonQuery query)
        {
            return RoleAuthorBusProvider.GetRoleAuthorPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<RoleAuthorEntity></returns>
        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(QueryBase query)
        {
            return RoleAuthorBusProvider.GetRoleAuthorPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="roleAuthorQuery">RoleAuthorQuery查询实体对象</param>
        /// <returns>Pager<RoleAuthorEntity></returns>
        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(RoleAuthorQuery roleAuthorQuery)
        {
            return RoleAuthorBusProvider.GetRoleAuthorPageList(roleAuthorQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="roleAuthor">RoleAuthorEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddRoleAuthor(RoleAuthorEntity roleAuthor)
        {
            return RoleAuthorBusProvider.AddRoleAuthor(roleAuthor);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="roleAuthor">RoleAuthorEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateRoleAuthor(RoleAuthorEntity roleAuthor)
        {
            return RoleAuthorBusProvider.UpdateRoleAuthor(roleAuthor);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteRoleAuthor(Int64 mapID)
        {
            return RoleAuthorBusProvider.DeleteRoleAuthor( mapID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="roleAuthor">RoleAuthorEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteRoleAuthor(RoleAuthorEntity roleAuthor)
        {
            return RoleAuthorBusProvider.DeleteRoleAuthor(roleAuthor);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteRoleAuthor(Int64[] mapID)
        {
            return RoleAuthorBusProvider.BatchDeleteRoleAuthor( mapID);
        }
        
        #endregion
        
        #endregion
    }
}
