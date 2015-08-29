﻿using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class SiteMessageBusiness : ISiteMessageBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public SiteMessageEntity GetSiteMessage(Int64 messageID)
        {
           return SiteMessageDataAccess.Instance.GetSiteMessage( messageID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<SiteMessageEntity></returns>
        public List<SiteMessageEntity> GetSiteMessageList()
        {
            return SiteMessageDataAccess.Instance.GetSiteMessageList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="siteMessageQuery">SiteMessageQuery查询实体对象</param>
        /// <returns>List<SiteMessageEntity></returns>
        public List<SiteMessageEntity> GetSiteMessageList(SiteMessageQuery siteMessageQuery)
        {
            return SiteMessageDataAccess.Instance.GetSiteMessageList(siteMessageQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<SiteMessageEntity></returns>
        public Pager<SiteMessageEntity> GetSiteMessagePageList(CommonQuery query)
        {
            return SiteMessageDataAccess.Instance.GetSiteMessagePageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<SiteMessageEntity></returns>
        public Pager<SiteMessageEntity> GetSiteMessagePageList(QueryBase query)
        {
            return SiteMessageDataAccess.Instance.GetSiteMessagePageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="siteMessageQuery">SiteMessageQuery查询实体对象</param>
        /// <returns>Pager<SiteMessageEntity></returns>
        public Pager<SiteMessageEntity> GetSiteMessagePageList(SiteMessageQuery siteMessageQuery)
        {
            return SiteMessageDataAccess.Instance.GetSiteMessagePageList(siteMessageQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="siteMessage">SiteMessageEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddSiteMessage(SiteMessageEntity siteMessage)
        {
            return SiteMessageDataAccess.Instance.AddSiteMessage(siteMessage);
        }
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="siteMessage">SiteMessageEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateSiteMessage(SiteMessageEntity siteMessage)
        {
            return SiteMessageDataAccess.Instance.UpdateSiteMessage(siteMessage);
        }
        
        #endregion 

        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteSiteMessage(Int64 messageID)
        {
            return SiteMessageDataAccess.Instance.DeleteSiteMessage( messageID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteMessage">SiteMessageEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteSiteMessage(SiteMessageEntity siteMessage)
        {
            return SiteMessageDataAccess.Instance.DeleteSiteMessage(siteMessage);
        }
        
        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteMessage(Int64[] messageID)
        {
            return SiteMessageDataAccess.Instance.BatchDeleteSiteMessage( messageID);
        }
        #endregion
        
        #endregion

         /// <summary>
        /// 修改消息为已查看
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        public bool UpdateMsgViewed(Int64 JournalID, Int64 MessageID)
        {
            return SiteMessageDataAccess.Instance.UpdateMsgViewed(JournalID, MessageID);
        }
    }
}
