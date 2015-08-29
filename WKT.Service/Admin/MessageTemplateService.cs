using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.BLL;
using WKT.Service.Interface;
using WKT.Model.Enum;
using WKT.Common.Extension;

namespace WKT.Service
{
    public partial class MessageTemplateService:IMessageTemplateService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IMessageTemplateBusiness messageTemplateBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IMessageTemplateBusiness MessageTemplateBusProvider
        {
            get
            {
                 if(messageTemplateBusProvider == null)
                 {
                      messageTemplateBusProvider = new MessageTemplateBusiness();//ServiceBusContainer.Instance.Container.Resolve<IMessageTemplateBusiness>();
                 }
                 return messageTemplateBusProvider;
            }
            set
            {
              messageTemplateBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageTemplateService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public MessageTemplateEntity GetMessageTemplate(Int64 templateID)
        {
           MessageTemplateEntity model= MessageTemplateBusProvider.GetMessageTemplate(templateID);
           if (model != null)
           {
               var dict = GetTCategoryDcit(model.JournalID, model.TType, false);
               model.TCategoryName = dict.ContainsKey(model.TCategory) ? dict[model.TCategory] : string.Empty;
           }
           return model;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<MessageTemplateEntity></returns>
        public List<MessageTemplateEntity> GetMessageTemplateList()
        {
            return MessageTemplateBusProvider.GetMessageTemplateList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="messageTemplateQuery">MessageTemplateQuery查询实体对象</param>
        /// <returns>List<MessageTemplateEntity></returns>
        public List<MessageTemplateEntity> GetMessageTemplateList(MessageTemplateQuery messageTemplateQuery)
        {
            return MessageTemplateBusProvider.GetMessageTemplateList(messageTemplateQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<MessageTemplateEntity></returns>
        public Pager<MessageTemplateEntity> GetMessageTemplatePageList(CommonQuery query)
        {
            return MessageTemplateBusProvider.GetMessageTemplatePageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<MessageTemplateEntity></returns>
        public Pager<MessageTemplateEntity> GetMessageTemplatePageList(QueryBase query)
        {
            return MessageTemplateBusProvider.GetMessageTemplatePageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="messageTemplateQuery">MessageTemplateQuery查询实体对象</param>
        /// <returns>Pager<MessageTemplateEntity></returns>
        public Pager<MessageTemplateEntity> GetMessageTemplatePageList(MessageTemplateQuery messageTemplateQuery)
        {
            return MessageTemplateBusProvider.GetMessageTemplatePageList(messageTemplateQuery);
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
            return MessageTemplateBusProvider.AddMessageTemplate(messageTemplate);
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
            return MessageTemplateBusProvider.UpdateMessageTemplate(messageTemplate);
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
            return MessageTemplateBusProvider.DeleteMessageTemplate( templateID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="messageTemplate">MessageTemplateEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteMessageTemplate(MessageTemplateEntity messageTemplate)
        {
            return MessageTemplateBusProvider.DeleteMessageTemplate(messageTemplate);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteMessageTemplate(Int64[] templateID)
        {
            return MessageTemplateBusProvider.BatchDeleteMessageTemplate( templateID);
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
            return MessageTemplateBusProvider.GetUserdTCategory(JournalID, TType);
        }

        /// <summary>
        /// 获取模版类型键值对
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="TType"></param>
        /// <param name="isChecked">是否验证该模版已经存在</param>
        /// <returns></returns>
        public IDictionary<int, string> GetTCategoryDcit(Int64 JournalID, Byte TType,bool isChecked)
        {
            IDictionary<int, string> dict = new DictValueService().GetDictValueDcit(JournalID, EnumDictKey.TCategory.ToString());
            if (dict != null && dict.Count > 0)
            {
                if (isChecked)
                {
                    IList<Int32> list = GetUserdTCategory(JournalID, TType);
                    if (list != null && list.Count > 0)
                        dict = dict.Where(p => !list.Contains(p.Key)).ToDictionary(p => p.Key, q => q.Value);
                }
            }
            return dict;
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
            return MessageTemplateBusProvider.GetMessageTemplate(JournalID, TCategory, TType);
        }

        /// <summary>
        /// 保存模版
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(MessageTemplateEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            model.Title = model.Title.TextFilter();
            string msg = string.Empty;
            if (model.TType == 2)
            {
                msg = "短信模版";
                model.TContent = model.TContent.TextFilter();
            }
            else
            {
                msg = "邮件模版";
                model.TContent = model.TContent.HtmlFilter();
            }
            if (model.TemplateID == 0)
            {
                result = AddMessageTemplate(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增" + msg + "成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增" + msg + "失败！";
                }
            }
            else
            {
                result = UpdateMessageTemplate(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "修改" + msg + "成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "修改" + msg + "失败！";
                }
            }
            return execResult;
        }
    }
}
