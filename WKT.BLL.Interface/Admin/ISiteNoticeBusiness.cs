using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface ISiteNoticeBusiness
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        SiteNoticeEntity GetSiteNotice(Int64 noticeID);
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<SiteNoticeEntity></returns>
        List<SiteNoticeEntity> GetSiteNoticeList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="siteNoticeQuery">SiteNoticeQuery查询实体对象</param>
        /// <returns>List<SiteNoticeEntity></returns>
        List<SiteNoticeEntity> GetSiteNoticeList(SiteNoticeQuery siteNoticeQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<SiteNoticeEntity></returns>
        Pager<SiteNoticeEntity> GetSiteNoticePageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<SiteNoticeEntity></returns>
        Pager<SiteNoticeEntity> GetSiteNoticePageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="siteNoticeQuery">SiteNoticeQuery查询实体对象</param>
        /// <returns>Pager<SiteNoticeEntity></returns>
        Pager<SiteNoticeEntity> GetSiteNoticePageList(SiteNoticeQuery siteNoticeQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="siteNotice">SiteNoticeEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddSiteNotice(SiteNoticeEntity siteNotice);
        
        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="siteNotice">SiteNoticeEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateSiteNotice(SiteNoticeEntity siteNotice);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteSiteNotice(Int64 noticeID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteNotice">SiteNoticeEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteSiteNotice(SiteNoticeEntity siteNotice);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteSiteNotice(Int64[] noticeID);
        
        #endregion
        
        #endregion 
    }
}






