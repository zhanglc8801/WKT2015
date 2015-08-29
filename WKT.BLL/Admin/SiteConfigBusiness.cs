using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class SiteConfigBusiness : ISiteConfigBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="siteConfigID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public SiteConfigEntity GetSiteConfig(Int64 siteConfigID)
        {
           return SiteConfigDataAccess.Instance.GetSiteConfig( siteConfigID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<SiteConfigEntity></returns>
        public List<SiteConfigEntity> GetSiteConfigList()
        {
            return SiteConfigDataAccess.Instance.GetSiteConfigList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="siteConfigQuery">SiteConfigQuery查询实体对象</param>
        /// <returns>List<SiteConfigEntity></returns>
        public List<SiteConfigEntity> GetSiteConfigList(SiteConfigQuery siteConfigQuery)
        {
            return SiteConfigDataAccess.Instance.GetSiteConfigList(siteConfigQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<SiteConfigEntity></returns>
        public Pager<SiteConfigEntity> GetSiteConfigPageList(CommonQuery query)
        {
            return SiteConfigDataAccess.Instance.GetSiteConfigPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<SiteConfigEntity></returns>
        public Pager<SiteConfigEntity> GetSiteConfigPageList(QueryBase query)
        {
            return SiteConfigDataAccess.Instance.GetSiteConfigPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="siteConfigQuery">SiteConfigQuery查询实体对象</param>
        /// <returns>Pager<SiteConfigEntity></returns>
        public Pager<SiteConfigEntity> GetSiteConfigPageList(SiteConfigQuery siteConfigQuery)
        {
            return SiteConfigDataAccess.Instance.GetSiteConfigPageList(siteConfigQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="siteConfig">SiteConfigEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddSiteConfig(SiteConfigEntity siteConfig)
        {
            return SiteConfigDataAccess.Instance.AddSiteConfig(siteConfig);
        }
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="siteConfig">SiteConfigEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateSiteConfig(SiteConfigEntity siteConfig)
        {
            return SiteConfigDataAccess.Instance.UpdateSiteConfig(siteConfig);
        }
        
        #endregion 

        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteConfigID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteSiteConfig(Int64 siteConfigID)
        {
            return SiteConfigDataAccess.Instance.DeleteSiteConfig( siteConfigID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteConfig">SiteConfigEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteSiteConfig(SiteConfigEntity siteConfig)
        {
            return SiteConfigDataAccess.Instance.DeleteSiteConfig(siteConfig);
        }
        
        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="siteConfigID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteConfig(Int64[] siteConfigID)
        {
            return SiteConfigDataAccess.Instance.BatchDeleteSiteConfig( siteConfigID);
        }
        #endregion
        
        #endregion

        /// <summary>
        /// 获取站点配置实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SiteConfigEntity GetSiteConfig(SiteConfigQuery query)
        {
            return SiteConfigDataAccess.Instance.GetSiteConfig(query);
        }

        /// <summary>
        /// 更新总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool UpdateSiteAccessCount(SiteConfigQuery query)
        {
            return SiteConfigDataAccess.Instance.UpdateSiteAccessCount(query);
        }

	    /// <summary>
        /// 获取总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetSiteAccessCount(SiteConfigQuery query)
        {
            return SiteConfigDataAccess.Instance.GetSiteAccessCount(query);
        }
    }
}
