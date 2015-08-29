using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface ISiteMessageService
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        SiteMessageEntity GetSiteMessage(Int64 JournalID, Int64 messageID);
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<SiteMessageEntity></returns>
        List<SiteMessageEntity> GetSiteMessageList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="siteMessageQuery">SiteMessageQuery查询实体对象</param>
        /// <returns>List<SiteMessageEntity></returns>
        List<SiteMessageEntity> GetSiteMessageList(SiteMessageQuery siteMessageQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<SiteMessageEntity></returns>
        Pager<SiteMessageEntity> GetSiteMessagePageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<SiteMessageEntity></returns>
        Pager<SiteMessageEntity> GetSiteMessagePageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="siteMessageQuery">SiteMessageQuery查询实体对象</param>
        /// <returns>Pager<SiteMessageEntity></returns>
        Pager<SiteMessageEntity> GetSiteMessagePageList(SiteMessageQuery siteMessageQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="siteMessage">SiteMessageEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddSiteMessage(SiteMessageEntity siteMessage);
        
        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="siteMessage">SiteMessageEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateSiteMessage(SiteMessageEntity siteMessage);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteSiteMessage(Int64 messageID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteMessage">SiteMessageEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteSiteMessage(SiteMessageEntity siteMessage);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteSiteMessage(Int64[] messageID);
        
        #endregion
        
        #endregion 

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult Save(SiteMessageEntity model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        ExecResult Del(Int64[] MessageID);

         /// <summary>
        /// 修改消息为已查看
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        bool UpdateMsgViewed(Int64 JournalID, Int64 MessageID);
    }
}






