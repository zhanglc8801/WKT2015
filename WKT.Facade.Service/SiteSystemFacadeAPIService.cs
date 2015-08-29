using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;
using WKT.Cache;
using WKT.Common.Utils;
using WKT.Config;
using WKT.Facade.Service.Interface;

namespace WKT.Facade.Service
{
    public class SiteSystemFacadeAPIService : ServiceBase, ISiteSystemFacadeService
    {
        # region Menu

        # region 获取菜单列表

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<TreeModel> GetTreeNodeList(MenuQuery query)
        {
            # region 找到内容菜单，添加栏目列表

            Func<IList<TreeModel>> funcGetSiteChanneNodes = () =>
            {
                IList<TreeModel> channelTreeList = new List<TreeModel>();
                SiteChannelQuery channelQuery = new SiteChannelQuery();
                channelQuery.JournalID = query.JournalID;
                channelQuery.Status = 1;
                SiteConfigFacadeAPIService siteConfigAPIService = new SiteConfigFacadeAPIService();
                channelTreeList = siteConfigAPIService.GetSiteChannelTreeList(channelQuery);
                TreeModel root = channelTreeList.Single(p => p.Id == 0);
                return root.children;
            };

            # endregion

            # region 构造Menu

            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<MenuEntity> listAllMenu = clientHelper.Post<IList<MenuEntity>,MenuQuery>(GetAPIUrl(APIConstant.SYSGETSYSMENUAJAX), query);
            IList<TreeModel> listResult = new List<TreeModel>();
            
            if (listAllMenu != null)
            {
                IList<MenuEntity> listRoot = listAllMenu.Where(item => item.PMenuID == 0).ToList<MenuEntity>();
                TreeModel treeNode = null;
                bool isFirst = true;
                foreach (MenuEntity item in listRoot)
                {
                    treeNode = new TreeModel();
                    treeNode.text = item.MenuName;
                    treeNode.url = SiteConfig.RootPath + item.MenuUrl;
                    treeNode.icon = SiteConfig.RootPath + item.IconUrl;
                    treeNode.isexpand = true;
                    IList<MenuEntity> listChild = listAllMenu.Where(p => p.PMenuID == item.MenuID).ToList<MenuEntity>();
                    if (listChild != null)
                    {
                        // 二级
                        foreach (MenuEntity itemChild in listChild)
                        {
                            TreeModel treeChildNode = new TreeModel();
                            treeChildNode.text = itemChild.MenuName;
                            treeChildNode.url = SiteConfig.RootPath + itemChild.MenuUrl;
                            treeChildNode.icon = SiteConfig.RootPath + itemChild.IconUrl;
                            treeChildNode.isexpand = query.GroupID == 1 ? isFirst : true;
                            // 如果是网站内容管理节点，则载入站点栏目设置
                            if (itemChild.IsContentMenu)
                            {
                                treeChildNode.children = funcGetSiteChanneNodes();
                                treeNode.children.Add(treeChildNode);
                            }
                            else
                            {
                                treeNode.children.Add(treeChildNode);
                                // 三级
                                IList<MenuEntity> lastListChild = listAllMenu.Where(p => p.PMenuID == itemChild.MenuID).ToList<MenuEntity>();
                                foreach (MenuEntity lastChild in lastListChild)
                                {
                                    TreeModel treeLastNode = new TreeModel();
                                    treeLastNode.text = lastChild.MenuName;
                                    treeLastNode.url = SiteConfig.RootPath + lastChild.MenuUrl;
                                    treeLastNode.icon = SiteConfig.RootPath + lastChild.IconUrl;
                                    treeLastNode.isexpand = query.GroupID == 1 ? false : true;
                                    treeChildNode.children.Add(treeLastNode);
                                }
                            }
                            isFirst = false;
                        }
                    }
                    
                    listResult.Add(treeNode);
                }
            }

            # endregion

            return listResult;
        }

        # endregion

        # region 获取菜单列表，带有权限标示

        /// <summary>
        /// 获取菜单列表，带有权限标示
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<TreeModel> GetTreeNodeListHaveRole(RoleMenuQuery queryRoleMenu)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            // 拥有权限的菜单键值对
            IDictionary<long, long> dictHaveRightMenu = clientHelper.PostAuth<IDictionary<long, long>, RoleMenuQuery>(GetAPIUrl(APIConstant.SYSGETTREENODELISTHAVERIGHT), queryRoleMenu);

            // 菜单查询条件
            MenuQuery menuQuery = new MenuQuery();
            menuQuery.JournalID = queryRoleMenu.JournalID;
            menuQuery.GroupID = queryRoleMenu.GroupID.Value;
            menuQuery.Status = 1;
            IList<MenuEntity> listAllMenu = clientHelper.Post<IList<MenuEntity>, MenuQuery>(GetAPIUrl(APIConstant.SYSGETSYSMENUAJAX), menuQuery);
            IList<TreeModel> listResult = new List<TreeModel>();
            if (listAllMenu != null)
            {
                IList<MenuEntity> listRoot = listAllMenu.Where(item => item.PMenuID == 0).ToList<MenuEntity>();
                TreeModel treeNode = null;
                foreach (MenuEntity item in listRoot)
                {
                    treeNode = new TreeModel();
                    treeNode.key = item.MenuID.ToString();
                    treeNode.text = item.MenuName;
                    treeNode.url = SiteConfig.RootPath + item.MenuUrl;
                    treeNode.icon = SiteConfig.RootPath + item.IconUrl;
                    treeNode.isexpand = true;
                    if (dictHaveRightMenu.ContainsKey(item.MenuID))
                    {
                        treeNode.ischecked = true;
                    }
                    else
                    {
                        treeNode.ischecked = false;
                    }
                    IList<MenuEntity> listChild = listAllMenu.Where(p => p.PMenuID == item.MenuID).ToList<MenuEntity>();
                    if (listChild != null)
                    {
                        // 二级
                        foreach (MenuEntity itemChild in listChild)
                        {
                            TreeModel treeChildNode = new TreeModel();
                            treeChildNode.key = itemChild.MenuID.ToString();
                            treeChildNode.text = itemChild.MenuName;
                            treeChildNode.url = SiteConfig.RootPath + itemChild.MenuUrl;
                            treeChildNode.icon = SiteConfig.RootPath + itemChild.IconUrl;
                            treeChildNode.isexpand = true;
                            if (dictHaveRightMenu.ContainsKey(itemChild.MenuID))
                            {
                                treeChildNode.ischecked = true;
                            }
                            else
                            {
                                treeChildNode.ischecked = false;
                            }
                            treeNode.children.Add(treeChildNode);
                            // 三级
                            IList<MenuEntity> lastListChild = listAllMenu.Where(p => p.PMenuID == itemChild.MenuID).ToList<MenuEntity>();
                            foreach (MenuEntity lastChild in lastListChild)
                            {
                                TreeModel treeLastNode = new TreeModel();
                                treeLastNode.key = lastChild.MenuID.ToString();
                                treeLastNode.text = lastChild.MenuName;
                                treeLastNode.url = SiteConfig.RootPath + lastChild.MenuUrl;
                                treeLastNode.icon = SiteConfig.RootPath + lastChild.IconUrl;
                                treeLastNode.isexpand = true;
                                if (dictHaveRightMenu.ContainsKey(lastChild.MenuID))
                                {
                                    treeLastNode.ischecked = true;
                                }
                                else
                                {
                                    treeLastNode.ischecked = false;
                                }

                                treeChildNode.children.Add(treeLastNode);
                            }
                        }
                    }

                    listResult.Add(treeNode);
                }
            }
            return listResult;
        }

        # endregion

        # region 获取菜单列表，只取有权限的

        /// <summary>
        /// 获取菜单列表，带有权限标示
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<TreeModel> GetHaveRightMenuAjaxByRole(RoleMenuQuery queryRoleMenu)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
          
            // 给指定的作者设置了例外的菜单
            IDictionary<long, string> dictExceptionRightMenu = new Dictionary<long, string>();
            if (queryRoleMenu.AuthorID != null)
            {
                AuthorMenuRightExceptionEntity authorExecEntity = new AuthorMenuRightExceptionEntity();
                authorExecEntity.AuthorID = queryRoleMenu.AuthorID.Value;
                authorExecEntity.JournalID = queryRoleMenu.JournalID;
                dictExceptionRightMenu = clientHelper.PostAuth<IDictionary<long, string>, AuthorMenuRightExceptionEntity>(GetAPIUrl(APIConstant.SYSGETAUTHOREXCEPTIONRIGHTMENU), authorExecEntity);
            }

            IList<MenuEntity> listHaveRightMenuList = clientHelper.Post<IList<MenuEntity>, RoleMenuQuery>(GetAPIUrl(APIConstant.SYSGETHAVERIGHTMENULIST), queryRoleMenu);
            IList<TreeModel> listResult = new List<TreeModel>();
            if (listHaveRightMenuList != null)
            {
                IList<MenuEntity> listRoot = listHaveRightMenuList.Where(item => item.PMenuID == 0).ToList<MenuEntity>();
                TreeModel treeNode = null;
                foreach (MenuEntity item in listRoot)
                {
                    treeNode = new TreeModel();
                    treeNode.key = item.MenuID.ToString();
                    treeNode.text = item.MenuName;
                    treeNode.url = SiteConfig.RootPath + item.MenuUrl;
                    treeNode.icon = SiteConfig.RootPath + item.IconUrl;
                    treeNode.isexpand = queryRoleMenu.IsExpend;
                    treeNode.ischecked = dictExceptionRightMenu.ContainsKey(item.MenuID) ? false : true;

                    IList<MenuEntity> listChild = listHaveRightMenuList.Where(p => p.PMenuID == item.MenuID).ToList<MenuEntity>();
                    if (listChild != null)
                    {
                        // 二级
                        foreach (MenuEntity itemChild in listChild)
                        {

                            TreeModel treeChildNode = new TreeModel();
                            treeChildNode.key = itemChild.MenuID.ToString();
                            treeChildNode.text = itemChild.MenuName;
                            treeChildNode.url = SiteConfig.RootPath + itemChild.MenuUrl;
                            treeChildNode.icon = SiteConfig.RootPath + itemChild.IconUrl;
                            treeChildNode.isexpand = queryRoleMenu.IsExpend;
                            treeChildNode.ischecked = dictExceptionRightMenu.ContainsKey(itemChild.MenuID) ? false : true;
                            treeNode.children.Add(treeChildNode);
                            // 三级
                            IList<MenuEntity> lastListChild = listHaveRightMenuList.Where(p => p.PMenuID == itemChild.MenuID).ToList<MenuEntity>();
                            foreach (MenuEntity lastChild in lastListChild)
                            {

                                TreeModel treeLastNode = new TreeModel();
                                treeLastNode.key = lastChild.MenuID.ToString();
                                treeLastNode.text = lastChild.MenuName;
                                treeLastNode.url = SiteConfig.RootPath + lastChild.MenuUrl;
                                treeLastNode.icon = SiteConfig.RootPath + lastChild.IconUrl;
                                treeLastNode.isexpand = queryRoleMenu.IsExpend;
                                treeLastNode.ischecked = dictExceptionRightMenu.ContainsKey(lastChild.MenuID) ? false : true;
                                treeChildNode.children.Add(treeLastNode);
                            }
                        }
                    }
                    listResult.Add(treeNode);
                }
            }
            return listResult;
        }

        # endregion

        # region 获取菜单列表，只取有权限的，首页左侧菜单

        /// <summary>
        /// 获取菜单列表，带有权限标示
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<TreeModel> GetHaveRightMenu(RoleMenuQuery queryRoleMenu)
        {
            if (queryRoleMenu.GroupID == 2)
            {
                MenuQuery query = new MenuQuery();
                query.JournalID = queryRoleMenu.JournalID;
                query.GroupID = 2;
                query.Status = 1;
                return GetTreeNodeList(query);
            }

            # region 找到内容菜单，添加栏目列表

            Func<IList<TreeModel>> funcGetSiteChanneNodes = () =>
            {
                IList<TreeModel> channelTreeList = new List<TreeModel>();
                SiteChannelQuery channelQuery = new SiteChannelQuery();
                channelQuery.JournalID = queryRoleMenu.JournalID;
                channelQuery.Status = 1;
                SiteConfigFacadeAPIService siteConfigAPIService = new SiteConfigFacadeAPIService();
                channelTreeList = siteConfigAPIService.GetSiteChannelTreeList(channelQuery,true);
                TreeModel root = channelTreeList.Single(p => p.Id == 0);
                return root.children;
            };

            # endregion

            IList<TreeModel> listResult = new List<TreeModel>();

            HttpClientHelper clientHelper = new HttpClientHelper();

            // 给指定的作者设置了例外的菜单
            IDictionary<long, string> dictExceptionRightMenu = new Dictionary<long, string>();
            if (queryRoleMenu.AuthorID != null)
            {
                AuthorMenuRightExceptionEntity authorExecEntity = new AuthorMenuRightExceptionEntity();
                authorExecEntity.AuthorID = queryRoleMenu.AuthorID.Value;
                authorExecEntity.JournalID = queryRoleMenu.JournalID;
                dictExceptionRightMenu = clientHelper.PostAuth<IDictionary<long, string>, AuthorMenuRightExceptionEntity>(GetAPIUrl(APIConstant.SYSGETAUTHOREXCEPTIONRIGHTMENU), authorExecEntity);
            }
            IList<MenuEntity> listHaveRightMenuList = clientHelper.Post<IList<MenuEntity>, RoleMenuQuery>(GetAPIUrl(APIConstant.SYSGETHAVERIGHTMENULIST), queryRoleMenu);

            if (listHaveRightMenuList != null)
            {
                IList<MenuEntity> listRoot = listHaveRightMenuList.Where(item => item.PMenuID == 0).ToList<MenuEntity>();
                TreeModel treeNode = null;
                foreach (MenuEntity item in listRoot)
                {
                    if (!dictExceptionRightMenu.ContainsKey(item.MenuID))
                    {
                        bool first = true;

                        treeNode = new TreeModel();
                        treeNode.key = item.MenuID.ToString();
                        treeNode.text = item.MenuName;
                        treeNode.url = SiteConfig.RootPath + item.MenuUrl;
                        treeNode.icon = SiteConfig.RootPath + item.IconUrl;
                        treeNode.isexpand = queryRoleMenu.IsExpend;

                        IList<MenuEntity> listChild = listHaveRightMenuList.Where(p => p.PMenuID == item.MenuID).ToList<MenuEntity>();
                        if (listChild != null)
                        {
                            treeNode.isexpand = (first != queryRoleMenu.IsExpend) && first ? first : queryRoleMenu.IsExpend;
                            // 二级
                            foreach (MenuEntity itemChild in listChild)
                            {
                                if (!dictExceptionRightMenu.ContainsKey(itemChild.MenuID))
                                {
                                    TreeModel treeChildNode = new TreeModel();
                                    treeChildNode.key = itemChild.MenuID.ToString();
                                    treeChildNode.text = itemChild.MenuName;
                                    treeChildNode.url = SiteConfig.RootPath + itemChild.MenuUrl;
                                    treeChildNode.icon = SiteConfig.RootPath + itemChild.IconUrl;
                                    treeChildNode.isexpand = queryRoleMenu.IsExpend;
                                    // 如果是网站内容管理节点，则载入站点栏目设置
                                    if (itemChild.IsContentMenu)
                                    {
                                        treeChildNode.children = funcGetSiteChanneNodes();
                                        treeNode.children.Add(treeChildNode);
                                    }
                                    else
                                    {
                                        treeNode.children.Add(treeChildNode);
                                        // 三级
                                        IList<MenuEntity> lastListChild = listHaveRightMenuList.Where(p => p.PMenuID == itemChild.MenuID).ToList<MenuEntity>();
                                        foreach (MenuEntity lastChild in lastListChild)
                                        {
                                            if (!dictExceptionRightMenu.ContainsKey(lastChild.MenuID))
                                            {
                                                TreeModel treeLastNode = new TreeModel();
                                                treeLastNode.key = lastChild.MenuID.ToString();
                                                treeLastNode.text = lastChild.MenuName;
                                                treeLastNode.url = SiteConfig.RootPath + lastChild.MenuUrl;
                                                treeLastNode.icon = SiteConfig.RootPath + lastChild.IconUrl;
                                                treeLastNode.isexpand = queryRoleMenu.IsExpend;
                                                treeChildNode.children.Add(treeLastNode);
                                            }
                                        }
                                    }
                                }
                            }
                            first = false;
                        }
                        listResult.Add(treeNode);
                    }
                }
            }
            return listResult;
        }

        # endregion

        # region 获取所有Menu

        /// <summary>
        /// 获取所有列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<MenuEntity> GetAllMenuList(MenuQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();

            IList<MenuEntity> listAllMenu = clientHelper.Post<IList<MenuEntity>,MenuQuery>(GetAPIUrl(APIConstant.SYSGETSYSMENUAJAX), query);
            IList<MenuEntity> listResult = new List<MenuEntity>();

            if (listAllMenu != null)
            {
                IList<MenuEntity> listRoot = listAllMenu.Where(item => item.PMenuID == 0).ToList<MenuEntity>();
                foreach (MenuEntity item in listRoot)
                {
                    item.MenuUrl = SiteConfig.RootPath + item.MenuUrl;
                    item.IconUrl = SiteConfig.RootPath + item.IconUrl;
                    IList<MenuEntity> listChild = listAllMenu.Where(p => p.PMenuID == item.MenuID).ToList<MenuEntity>();
                    if (listChild != null)
                    {
                        // 二级
                        foreach (MenuEntity itemChild in listChild)
                        {
                            itemChild.MenuUrl = SiteConfig.RootPath + itemChild.MenuUrl;
                            itemChild.IconUrl = SiteConfig.RootPath + itemChild.IconUrl;
                            item.children.Add(itemChild);
                            // 三级
                            IList<MenuEntity> lastListChild = listAllMenu.Where(p => p.PMenuID == itemChild.MenuID).ToList<MenuEntity>();
                            foreach (MenuEntity lastChild in lastListChild)
                            {
                                lastChild.MenuUrl = SiteConfig.RootPath + lastChild.MenuUrl;
                                lastChild.IconUrl = SiteConfig.RootPath + lastChild.IconUrl;
                                itemChild.children.Add(lastChild);
                            }
                        }
                    }

                    listResult.Add(item);
                }
            }
            return listResult;
        }

        # endregion

        # region 获取指定的菜单信息

        /// <summary>
        /// 获取指定菜单信息
        /// </summary>
        /// <param name="queryMenu"></param>
        /// <returns></returns>
        public MenuEntity GetMenu(MenuQuery queryMenu)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            MenuEntity menuItem = clientHelper.PostAuth<MenuEntity, MenuQuery>(GetAPIUrl(APIConstant.SYSGETMENUENTITY), queryMenu);
            menuItem.MenuUrl = SiteConfig.RootPath + menuItem.MenuUrl;
            menuItem.IconUrl = SiteConfig.RootPath + menuItem.IconUrl;
            return menuItem;
        }

        # endregion

        # region 更新菜单状态

        /// <summary>
        /// 更新菜单状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult UpdateMenuStatus(MenuQuery menuQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, MenuQuery>(GetAPIUrl(APIConstant.SYSUPDATEMENUSTATUS), menuQuery);
            return execResult;
        }

        # endregion

        # region 判断是否有访问该页面地址的权限

        /// <summary>
        /// 是否有权限访问当前地址
        /// 如果该地址出现在menu表中了，则进行判断，如果没有则不判断
        /// </summary>
        /// <param name="RoleIDList"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool IsHaveAccessRight(RoleMenuQuery queryRoleMenu)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            bool flag = clientHelper.PostAuth<bool, RoleMenuQuery>(GetAPIUrl(APIConstant.SYSISHAVEMENURIGHT), queryRoleMenu);
            return flag;
        }

        /// <summary>
        /// 是否有权限访问当前地址,根据分组判断
        /// 如果该地址出现在menu表中了，则进行判断，如果没有则不判断
        /// </summary>
        /// <param name="query"></param>        
        /// <returns></returns>
        public bool IsHaveAccessRightByGroup(RoleMenuQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            bool flag = clientHelper.PostAuth<bool, RoleMenuQuery>(GetAPIUrl(APIConstant.SYS_ISHAVEACCESSRIGHTBYGROUP), query);
            return flag;
        }

        # endregion

        # region 菜单角色赋权

        /// <summary>
        /// 菜单角色赋权
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>        
        public ExecResult SetMenuRight(List<RoleMenuEntity> listRoleMenu)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, List<RoleMenuEntity>>(GetAPIUrl(APIConstant.SYSSETMENUROLERIGHT), listRoleMenu);
            return execResult;
        }

        # endregion

        # endregion

        # region Role

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="roleQuery"></param>
        /// <returns></returns>
        public IList<RoleInfoEntity> GetRoleList(RoleInfoQuery roleQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<RoleInfoEntity> listRole = clientHelper.PostAuth<IList<RoleInfoEntity>, RoleInfoQuery>(GetAPIUrl(APIConstant.SYSGETROLELIST), roleQuery);
            return listRole;
        }

        /// <summary>
        /// 获取角色实体
        /// </summary>
        /// <param name="roleQuery"></param>
        /// <returns></returns>
        public RoleInfoEntity GetRoleEntity(RoleInfoQuery roleQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            RoleInfoEntity roleEntity = clientHelper.PostAuth<RoleInfoEntity, RoleInfoQuery>(GetAPIUrl(APIConstant.SYSGETROLEENTITY), roleQuery);
            return roleEntity;
        }

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        public ExecResult UpdateRoleInfo(RoleInfoEntity roleEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, RoleInfoEntity>(GetAPIUrl(APIConstant.SYSUPDATEROLEINFO), roleEntity);
            return execResult;
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        public ExecResult AddRole(RoleInfoEntity roleEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, RoleInfoEntity>(GetAPIUrl(APIConstant.SYSADDROLEINFO), roleEntity);
            return execResult;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="queryRole"></param>
        /// <returns></returns>
        public ExecResult DelRole(RoleInfoQuery queryRole)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, RoleInfoQuery>(GetAPIUrl(APIConstant.SYSDELROLEINFO), queryRole);
            return execResult;
        }

        # endregion

        # region 成员

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="queryRole"></param>
        /// <returns></returns>
        public ExecResult DelMember(AuthorInfoQuery authorQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, AuthorInfoQuery>(GetAPIUrl(APIConstant.SYSDELMEMBERINFO), authorQuery);
            return execResult;
        }

        /// <summary>
        /// 编辑成员
        /// </summary>
        /// <param name="queryRole"></param>
        /// <returns></returns>
        public ExecResult EditMember(AuthorInfoEntity authorEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, AuthorInfoEntity>(GetAPIUrl(APIConstant.SYSEDITMEMBERINFO), authorEntity);
            return execResult;
        }

        # endregion
    }
}
