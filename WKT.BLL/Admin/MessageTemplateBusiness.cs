using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class MessageTemplateBusiness : IMessageTemplateBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public MessageTemplateEntity GetMessageTemplate(Int64 templateID)
        {
           return MessageTemplateDataAccess.Instance.GetMessageTemplate( templateID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<MessageTemplateEntity></returns>
        public List<MessageTemplateEntity> GetMessageTemplateList()
        {
            return MessageTemplateDataAccess.Instance.GetMessageTemplateList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="messageTemplateQuery">MessageTemplateQuery查询实体对象</param>
        /// <returns>List<MessageTemplateEntity></returns>
        public List<MessageTemplateEntity> GetMessageTemplateList(MessageTemplateQuery messageTemplateQuery)
        {
            return MessageTemplateDataAccess.Instance.GetMessageTemplateList(messageTemplateQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<MessageTemplateEntity></returns>
        public Pager<MessageTemplateEntity> GetMessageTemplatePageList(CommonQuery query)
        {
            return MessageTemplateDataAccess.Instance.GetMessageTemplatePageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<MessageTemplateEntity></returns>
        public Pager<MessageTemplateEntity> GetMessageTemplatePageList(QueryBase query)
        {
            return MessageTemplateDataAccess.Instance.GetMessageTemplatePageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="messageTemplateQuery">MessageTemplateQuery查询实体对象</param>
        /// <returns>Pager<MessageTemplateEntity></returns>
        public Pager<MessageTemplateEntity> GetMessageTemplatePageList(MessageTemplateQuery messageTemplateQuery)
        {
            return MessageTemplateDataAccess.Instance.GetMessageTemplatePageList(messageTemplateQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="messageTemplate">MessageTemplateEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddMessageTemplate(MessageTemplateEntity messageTemplate)
        {
            return MessageTemplateDataAccess.Instance.AddMessageTemplate(messageTemplate);
        }
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="messageTemplate">MessageTemplateEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateMessageTemplate(MessageTemplateEntity messageTemplate)
        {
            return MessageTemplateDataAccess.Instance.UpdateMessageTemplate(messageTemplate);
        }
        
        #endregion 

        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteMessageTemplate(Int64 templateID)
        {
            return MessageTemplateDataAccess.Instance.DeleteMessageTemplate( templateID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="messageTemplate">MessageTemplateEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteMessageTemplate(MessageTemplateEntity messageTemplate)
        {
            return MessageTemplateDataAccess.Instance.DeleteMessageTemplate(messageTemplate);
        }
        
        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteMessageTemplate(Int64[] templateID)
        {
            return MessageTemplateDataAccess.Instance.BatchDeleteMessageTemplate( templateID);
        }
        #endregion
        
        #endregion

        /// <summary>
        /// 获取已经使用模版类型
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="TType"></param>
        /// <returns></returns>
        public IList<Int32> GetUserdTCategory(Int64 JournalID, Byte TType)
        {
            return MessageTemplateDataAccess.Instance.GetUserdTCategory(JournalID, TType);
        }

        /// <summary>
        /// 获取短信邮件模版实体
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="TCategory"></param>
        /// <param name="TType"></param>
        /// <returns></returns>
        public MessageTemplateEntity GetMessageTemplate(Int64 JournalID, Int32 TCategory, Byte TType)
        {
            return MessageTemplateDataAccess.Instance.GetMessageTemplate(JournalID, TCategory,TType);
        }
    }
}
