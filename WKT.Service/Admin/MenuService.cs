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
    public partial class MenuService:IMenuService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IMenuBusiness menuBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IMenuBusiness MenuBusProvider
        {
            get
            {
                 if(menuBusProvider == null)
                 {
                     menuBusProvider = new MenuBusiness();//ServiceBusContainer.Instance.Container.Resolve<IMenuBusiness>();
                 }
                 return menuBusProvider;
            }
            set
            {
              menuBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MenuService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public MenuEntity GetMenu(Int64 menuID)
        {
           return MenuBusProvider.GetMenu( menuID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<MenuEntity></returns>
        public List<MenuEntity> GetMenuList()
        {
            return MenuBusProvider.GetMenuList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="menuQuery">MenuQuery查询实体对象</param>
        /// <returns>List<MenuEntity></returns>
        public List<MenuEntity> GetMenuList(MenuQuery menuQuery)
        {
            return MenuBusProvider.GetMenuList(menuQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<MenuEntity></returns>
        public Pager<MenuEntity> GetMenuPageList(CommonQuery query)
        {
            return MenuBusProvider.GetMenuPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<MenuEntity></returns>
        public Pager<MenuEntity> GetMenuPageList(QueryBase query)
        {
            return MenuBusProvider.GetMenuPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="menuQuery">MenuQuery查询实体对象</param>
        /// <returns>Pager<MenuEntity></returns>
        public Pager<MenuEntity> GetMenuPageList(MenuQuery menuQuery)
        {
            return MenuBusProvider.GetMenuPageList(menuQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="menu">MenuEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddMenu(MenuEntity menu)
        {
            return MenuBusProvider.AddMenu(menu);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="menu">MenuEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateMenu(MenuEntity menu)
        {
            return MenuBusProvider.UpdateMenu(menu);
        }

        /// <summary>
        /// 更新菜单状态
        /// </summary>
        /// <param name="menuEntity"></param>
        /// <returns></returns>
        public bool UpdateStaus(MenuQuery menuQuery)
        {
            return MenuBusProvider.UpdateStaus(menuQuery);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteMenu(Int64 menuID)
        {
            return MenuBusProvider.DeleteMenu( menuID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="menu">MenuEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteMenu(MenuEntity menu)
        {
            return MenuBusProvider.DeleteMenu(menu);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteMenu(Int64[] menuID)
        {
            return MenuBusProvider.BatchDeleteMenu( menuID);
        }
        
        #endregion
        
        #endregion
    }
}
