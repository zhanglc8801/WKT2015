using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public partial class RoleMenuBusiness : IRoleMenuBusiness
    {
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="roleMenuQuery">RoleMenuQuery查询实体对象</param>
        /// <returns>List<RoleMenuEntity></returns>
        public List<RoleMenuEntity> GetRoleMenuList(RoleMenuQuery roleMenuQuery)
        {
            return RoleMenuDataAccess.Instance.GetRoleMenuList(roleMenuQuery);
        }

        /// <summary>
        /// 获取指定角色拥有的菜单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<MenuEntity> GetHaveRightMenuList(RoleMenuQuery query)
        {
            return RoleMenuDataAccess.Instance.GetHaveRightMenuList(query);
        }


        /// <summary>
        /// 获取指定角色拥有权限的菜单ID
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long, long> GetRoleMenuDict(RoleMenuQuery query)
        {
            return RoleMenuDataAccess.Instance.GetRoleMenuDict(query);
        }

        /// <summary>
        /// 获取指定用户例外权限的菜单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<long, string> GetAuthorExceptionMenuDict(AuthorMenuRightExceptionEntity authorExceptionMenuEntity)
        {
            return RoleMenuDataAccess.Instance.GetAuthorExceptionMenuDict(authorExceptionMenuEntity);
        }

        /// <summary>
        /// 保存角色拥有权限的菜单映射关系
        /// </summary>
        /// <param name="menuMapList">菜单权限列表</param>
        /// <returns></returns>
        public bool SaveRoleMenuRight(List<RoleMenuEntity> menuMapList)
        {
            return RoleMenuDataAccess.Instance.SaveRoleMenuRight(menuMapList);
        }

        /// <summary>
        /// 给指定角色内的用户设置例外菜单权限
        /// </summary>
        /// <param name="authorExceptionRight"></param>
        /// <returns></returns>
        public bool SetAuthorExceptionMenuRight(AuthorMenuRightExceptionEntity authorExceptionRight)
        {
            return RoleMenuDataAccess.Instance.SetAuthorExceptionMenuRight(authorExceptionRight);
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
            return RoleMenuDataAccess.Instance.IsHaveAccessRight(query);
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
            return RoleMenuDataAccess.Instance.IsHaveAccessRightByGroup(query);
        }
    }
}
