using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface ISiteConfigService
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="siteConfigID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        SiteConfigEntity GetSiteConfig(Int64 siteConfigID);
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<SiteConfigEntity></returns>
        List<SiteConfigEntity> GetSiteConfigList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="siteConfigQuery">SiteConfigQuery查询实体对象</param>
        /// <returns>List<SiteConfigEntity></returns>
        List<SiteConfigEntity> GetSiteConfigList(SiteConfigQuery siteConfigQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<SiteConfigEntity></returns>
        Pager<SiteConfigEntity> GetSiteConfigPageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<SiteConfigEntity></returns>
        Pager<SiteConfigEntity> GetSiteConfigPageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="siteConfigQuery">SiteConfigQuery查询实体对象</param>
        /// <returns>Pager<SiteConfigEntity></returns>
        Pager<SiteConfigEntity> GetSiteConfigPageList(SiteConfigQuery siteConfigQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="siteConfig">SiteConfigEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddSiteConfig(SiteConfigEntity siteConfig);
        
        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="siteConfig">SiteConfigEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateSiteConfig(SiteConfigEntity siteConfig);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteConfigID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteSiteConfig(Int64 siteConfigID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteConfig">SiteConfigEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteSiteConfig(SiteConfigEntity siteConfig);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="siteConfigID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteSiteConfig(Int64[] siteConfigID);
        
        #endregion
        
        #endregion 

        /// <summary>
        /// 获取站点配置实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        SiteConfigEntity GetSiteConfig(SiteConfigQuery query);

        /// <summary>
        /// 更新总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        bool UpdateSiteAccessCount(SiteConfigQuery query);

	    /// <summary>
        /// 获取总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        int GetSiteAccessCount(SiteConfigQuery query);
    }
}






