using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface IRoleMenuBusiness
    {       

        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="roleMenuQuery">RoleMenuQuery查询实体对象</param>
        /// <returns>List<RoleMenuEntity></returns>
        List<RoleMenuEntity> GetRoleMenuList(RoleMenuQuery roleMenuQuery);

        /// <summary>
        /// 获取指定角色拥有的菜单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<MenuEntity> GetHaveRightMenuList(RoleMenuQuery query);

        /// <summary>
        /// 获取指定角色拥有权限的菜单ID
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IDictionary<long, long> GetRoleMenuDict(RoleMenuQuery query);

        /// <summary>
        /// 获取指定用户例外权限的菜单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IDictionary<long, string> GetAuthorExceptionMenuDict(AuthorMenuRightExceptionEntity authorExceptionMenuEntity);

        /// <summary>
        /// 保存角色拥有权限的菜单映射关系
        /// </summary>
        /// <param name="menuMapList">菜单权限列表</param>
        /// <returns></returns>
        bool SaveRoleMenuRight(List<RoleMenuEntity> menuMapList);

        /// <summary>
        /// 给指定角色内的用户设置例外菜单权限
        /// </summary>
        /// <param name="authorExceptionRight"></param>
        /// <returns></returns>
        bool SetAuthorExceptionMenuRight(AuthorMenuRightExceptionEntity authorExceptionRight);

        /// <summary>
        /// 是否有权限访问当前地址
        /// 如果该地址出现在menu表中了，则进行判断，如果没有则不判断
        /// </summary>
        /// <param name="RoleIDList"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        bool IsHaveAccessRight(RoleMenuQuery query);

        /// <summary>
        /// 是否有权限访问当前地址,根据分组判断
        /// 如果该地址出现在menu表中了，则进行判断，如果没有则不判断
        /// </summary>
        /// <param name="RoleIDList"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        bool IsHaveAccessRightByGroup(RoleMenuQuery query);

    }
}






