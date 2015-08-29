using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;

namespace WKT.Facade.Service.Interface
{
    public interface ISiteSystemFacadeService
    {
        # region Menu

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<TreeModel> GetTreeNodeList(MenuQuery query);

        /// <summary>
        /// 获取菜单列表，带有权限标示
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<TreeModel> GetTreeNodeListHaveRole(RoleMenuQuery queryRoleMenu);

        /// <summary>
        /// 获取菜单列表，带有权限标示
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<TreeModel> GetHaveRightMenuAjaxByRole(RoleMenuQuery queryRoleMenu);

         /// <summary>
        /// 获取菜单列表，带有权限标示
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<TreeModel> GetHaveRightMenu(RoleMenuQuery queryRoleMenu);

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<MenuEntity> GetAllMenuList(MenuQuery query);

        /// <summary>
        /// 获取指定菜单信息
        /// </summary>
        /// <param name="queryMenu"></param>
        /// <returns></returns>
        MenuEntity GetMenu(MenuQuery queryMenu);

        /// <summary>
        /// 更新菜单状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult UpdateMenuStatus(MenuQuery menuQuery);

        /// <summary>
        /// 菜单角色赋权
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult SetMenuRight(List<RoleMenuEntity> listRoleMenu);

        /// <summary>
        /// 是否有权限访问当前地址
        /// 如果该地址出现在menu表中了，则进行判断，如果没有则不判断
        /// </summary>
        /// <param name="RoleIDList"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        bool IsHaveAccessRight(RoleMenuQuery queryRoleMenu);

        /// <summary>
        /// 是否有权限访问当前地址,根据分组判断
        /// 如果该地址出现在menu表中了，则进行判断，如果没有则不判断
        /// </summary>
        /// <param name="query"></param>        
        /// <returns></returns>
        bool IsHaveAccessRightByGroup(RoleMenuQuery query);

        # endregion

        # region Role

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="roleQuery"></param>
        /// <returns></returns>
        IList<RoleInfoEntity> GetRoleList(RoleInfoQuery roleQuery);

        /// <summary>
        /// 获取角色实体
        /// </summary>
        /// <param name="roleQuery"></param>
        /// <returns></returns>
        RoleInfoEntity GetRoleEntity(RoleInfoQuery roleQuery);

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        ExecResult UpdateRoleInfo(RoleInfoEntity roleEntity);

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        ExecResult AddRole(RoleInfoEntity roleEntity);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="queryRole"></param>
        /// <returns></returns>
        ExecResult DelRole(RoleInfoQuery queryRole);

        # endregion

        # region 成员

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="queryRole"></param>
        /// <returns></returns>
        ExecResult DelMember(AuthorInfoQuery authorQuery);

        /// <summary>
        /// 编辑成员
        /// </summary>
        /// <param name="queryRole"></param>
        /// <returns></returns>
        ExecResult EditMember(AuthorInfoEntity authorEntity);

        # endregion
    }
}
