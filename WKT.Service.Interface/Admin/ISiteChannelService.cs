using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface ISiteChannelService
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        SiteChannelEntity GetSiteChannel(SiteChannelQuery query);
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<SiteChannelEntity></returns>
        List<SiteChannelEntity> GetSiteChannelList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="siteChannelQuery">SiteChannelQuery查询实体对象</param>
        /// <returns>List<SiteChannelEntity></returns>
        List<SiteChannelEntity> GetSiteChannelList(SiteChannelQuery siteChannelQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<SiteChannelEntity></returns>
        Pager<SiteChannelEntity> GetSiteChannelPageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<SiteChannelEntity></returns>
        Pager<SiteChannelEntity> GetSiteChannelPageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="siteChannelQuery">SiteChannelQuery查询实体对象</param>
        /// <returns>Pager<SiteChannelEntity></returns>
        Pager<SiteChannelEntity> GetSiteChannelPageList(SiteChannelQuery siteChannelQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="siteChannel">SiteChannelEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddSiteChannel(SiteChannelEntity siteChannel);
        
        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="siteChannel">SiteChannelEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateSiteChannel(SiteChannelEntity siteChannel);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteSiteChannel(Int64 channelID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteChannel">SiteChannelEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteSiteChannel(SiteChannelEntity siteChannel);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteSiteChannel(Int64[] channelID);
        
        #endregion
        
        #endregion 

        /// <summary>
        /// 保存栏目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult Save(SiteChannelEntity model);
    }
}






