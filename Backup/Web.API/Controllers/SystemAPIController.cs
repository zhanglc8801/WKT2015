using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Service.Interface;
using WKT.Service.Wrapper;

namespace Web.API.Controllers
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public class SystemAPIController : ApiBaseController
    {
        [System.Web.Http.AcceptVerbs("GET")]
        public string Test()
        {
            string strMD5 = WKT.Common.Security.MD5Handle.Encrypt("123456");
            return "sucess!," + strMD5;
        }

        # region Menu

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<MenuEntity> GetMenuList(MenuQuery queryMenu)
        {
            IList<MenuEntity> listAllMenu = null;
            IMenuService menuService = ServiceContainer.Instance.Container.Resolve<IMenuService>();
            listAllMenu = menuService.GetMenuList(queryMenu);
            return listAllMenu;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<MenuEntity> GetHaveRightMenu(RoleMenuQuery queryRoleMenu)
        {
            IList<MenuEntity> listAllMenu = null;
            IRoleMenuService menuService = ServiceContainer.Instance.Container.Resolve<IRoleMenuService>();
            listAllMenu = menuService.GetHaveRightMenuList(queryRoleMenu);
            return listAllMenu;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool IsHaveMenuRight(RoleMenuQuery queryRoleMenu)
        {
            bool flag = false;
            IRoleMenuService menuService = ServiceContainer.Instance.Container.Resolve<IRoleMenuService>();
            flag = menuService.IsHaveAccessRight(queryRoleMenu);
            return flag;
        }

        /// <summary>
        /// 是否有权限访问当前地址,根据分组判断
        /// 如果该地址出现在menu表中了，则进行判断，如果没有则不判断
        /// </summary>
        /// <param name="RoleIDList"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool IsHaveAccessRightByGroup(RoleMenuQuery query)
        {
            bool flag = false;
            IRoleMenuService menuService = ServiceContainer.Instance.Container.Resolve<IRoleMenuService>();
            flag = menuService.IsHaveAccessRightByGroup(query);
            return flag;
        }

        /// <summary>
        /// 获取拥有权限的菜单ID
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IDictionary<long, long> GetTreeNodeListHaveRight(RoleMenuQuery queryRoleMenu)
        {
            IDictionary<long, long> dictHaveRightMenu = new Dictionary<long, long>();
            IRoleMenuService menuRoleService = ServiceContainer.Instance.Container.Resolve<IRoleMenuService>();
            dictHaveRightMenu = menuRoleService.GetRoleMenuDict(queryRoleMenu);
            return dictHaveRightMenu;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IDictionary<long, string> GetAuthorExceptionMenu(AuthorMenuRightExceptionEntity authorExcMenu)
        {
            IDictionary<long, string> dictHaveRightMenu = new Dictionary<long, string>();
            IRoleMenuService menuRoleService = ServiceContainer.Instance.Container.Resolve<IRoleMenuService>();
            dictHaveRightMenu = menuRoleService.GetAuthorExceptionMenuDict(authorExcMenu);
            return dictHaveRightMenu;
        }

        /// <summary>
        /// 获取指定菜单信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public MenuEntity GetMenu(MenuQuery queryMenu)
        {
            IMenuService menuService = ServiceContainer.Instance.Container.Resolve<IMenuService>();
            MenuEntity menuEntity = menuService.GetMenu(queryMenu.MenuID);
            return menuEntity;
        }

        /// <summary>
        /// 更新菜单状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult UpdateMenuStatus(MenuQuery menuQuery)
        {
            ExecResult execResult = new ExecResult();
            try
            {
                IMenuService menuService = ServiceContainer.Instance.Container.Resolve<IMenuService>();
                menuService.UpdateStaus(menuQuery);
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "更新成功";
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "更新菜单状态出现异常：" + ex.Message;
            }
            return execResult;
        }

        /// <summary>
        /// 菜单角色赋权
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult SetMenuRight(List<RoleMenuEntity> listRoleMenu)
        {
            ExecResult execResult = new ExecResult();
            try
            {
                IRoleMenuService menuRoleService = ServiceContainer.Instance.Container.Resolve<IRoleMenuService>();
                menuRoleService.SaveRoleMenuRight(listRoleMenu);
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "菜单赋权成功";
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "菜单赋权出现异常：" + ex.Message;
            }
            return execResult;
        }

        # endregion

        # region role

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="roleQuery"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<RoleInfoEntity> GetRoleList(RoleInfoQuery roleQuery)
        {
            IRoleInfoService roleService = ServiceContainer.Instance.Container.Resolve<IRoleInfoService>();
            IList<RoleInfoEntity> listRole = roleService.GetRoleInfoList(roleQuery);
            return listRole;
        }

        /// <summary>
        /// 得到指定的角色信息
        /// </summary>
        /// <param name="roleQuery"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public RoleInfoEntity GetRoleEntity(RoleInfoQuery roleQuery)
        {
            IRoleInfoService roleService = ServiceContainer.Instance.Container.Resolve<IRoleInfoService>();
            RoleInfoEntity roleEntity = roleService.GetRoleInfo(roleQuery.RoleID.Value);
            return roleEntity;
        }

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="roleQuery"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult UpdateRoleInfo(RoleInfoEntity roleEntity)
        {
            ExecResult execResult = new ExecResult();
            IRoleInfoService roleService = ServiceContainer.Instance.Container.Resolve<IRoleInfoService>();
            try
            {
                bool flag = roleService.UpdateRoleInfo(roleEntity);
                if (flag)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "更新成功";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "更新失败，请检查输入的角色信息";
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "更新角色信息时出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("更新角色信息时出现异常：" + ex.Message);
            }
            return execResult;
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleQuery"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult AddRole(RoleInfoEntity roleEntity)
        {
            ExecResult execResult = new ExecResult();
            IRoleInfoService roleService = ServiceContainer.Instance.Container.Resolve<IRoleInfoService>();
            try
            {
                bool flag = roleService.AddRoleInfo(roleEntity);
                if (flag)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增角色成功";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增角色失败，请检查输入的角色信息";
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "新增角色信息时出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("新增角色信息时出现异常：" + ex.Message);
            }
            return execResult;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleQuery"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult DelRole(RoleInfoQuery queryRole)
        {
            ExecResult execResult = new ExecResult();
            IRoleInfoService roleService = ServiceContainer.Instance.Container.Resolve<IRoleInfoService>();
            try
            {
                bool flag = roleService.BatchDeleteRoleInfo(queryRole);
                if (flag)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "删除角色成功";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "删除角色失败，请检查输入的角色信息";
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "删除角色信息时出现异常：" + ex.Message;
                WKT.Log.LogProvider.Instance.Error("删除角色信息时出现异常：" + ex.Message);
            }
            return execResult;
        }

        /// <summary>
        /// 得到角色键值对信息
        /// </summary>
        /// <param name="roleQuery"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IDictionary<long,string> GetRoleInfoDict(RoleInfoQuery roleQuery)
        {
            IRoleInfoService roleService = ServiceContainer.Instance.Container.Resolve<IRoleInfoService>();
            IDictionary<long, string> dictRoleInfo = roleService.GetRoleInfoDict(roleQuery);
            return dictRoleInfo;
        }

        # endregion
    }
}
