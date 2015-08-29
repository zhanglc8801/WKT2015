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
    public partial class RoleMenuService:IRoleMenuService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IRoleMenuBusiness roleMenuBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IRoleMenuBusiness RoleMenuBusProvider
        {
            get
            {
                 if(roleMenuBusProvider == null)
                 {
                     roleMenuBusProvider = new RoleMenuBusiness();//ServiceBusContainer.Instance.Container.Resolve<IRoleMenuBusiness>();
                 }
                 return roleMenuBusProvider;
            }
            set
            {
              roleMenuBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleMenuService()
        {
        }
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="roleMenuQuery">RoleMenuQuery查询实体对象</param>
        /// <returns>List<RoleMenuEntity></returns>
        public List<RoleMenuEntity> GetRoleMenuList(RoleMenuQuery roleMenuQuery)
        {
            return RoleMenuBusProvider.GetRoleMenuList(roleMenuQuery);
        }

        /// <summary>
        /// 获取指定角色拥有的菜单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<MenuEntity> GetHaveRightMenuList(RoleMenuQuery query)
        {
            return RoleMenuBusProvider.GetHaveRightMenuList(query);
        }

        /// <summary>
        /// 获取指定角色拥有权限的菜单ID
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long, long> GetRoleMenuDict(RoleMenuQuery query)
        {
            return RoleMenuBusProvider.GetRoleMenuDict(query);
        }

        /// <summary>
        /// 获取指定用户例外权限的菜单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long, string> GetAuthorExceptionMenuDict(AuthorMenuRightExceptionEntity authorExceptionMenuEntity)
        {
            return RoleMenuBusProvider.GetAuthorExceptionMenuDict(authorExceptionMenuEntity);
        }

        /// <summary>
        /// 保存角色拥有权限的菜单映射关系
        /// </summary>
        /// <param name="menuMapList">菜单权限列表</param>
        /// <returns></returns>
        public bool SaveRoleMenuRight(List<RoleMenuEntity> menuMapList)
        {
            return RoleMenuBusProvider.SaveRoleMenuRight(menuMapList);
        }

        /// <summary>
        /// 给指定角色内的用户设置例外菜单权限
        /// </summary>
        /// <param name="authorExceptionRight"></param>
        /// <returns></returns>
        public bool SetAuthorExceptionMenuRight(AuthorMenuRightExceptionEntity authorExceptionRight)
        {
            return RoleMenuBusProvider.SetAuthorExceptionMenuRight(authorExceptionRight);
        }

        /// <summary>
        /// 是否有权限访问当前地址
        /// 如果该地址出现在menu表中了，则进行判断，如果没有则不判断
        /// </summary>
        /// <param name="RoleIDList"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsHaveAccessRight(RoleMenuQuery query)
        {
            return RoleMenuBusProvider.IsHaveAccessRight(query);
        }

        /// <summary>
        /// 是否有权限访问当前地址,根据分组判断
        /// 如果该地址出现在menu表中了，则进行判断，如果没有则不判断
        /// </summary>
        /// <param name="RoleIDList"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsHaveAccessRightByGroup(RoleMenuQuery query)
        {
            return RoleMenuBusProvider.IsHaveAccessRightByGroup(query);
        }
    }
}
