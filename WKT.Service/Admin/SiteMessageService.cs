using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.BLL;
using WKT.Service.Interface;
using WKT.Common.Extension;
using WKT.Common.Utils;
using WKT.Model.Enum;

namespace WKT.Service
{
    public partial class SiteMessageService:ISiteMessageService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private ISiteMessageBusiness siteMessageBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public ISiteMessageBusiness SiteMessageBusProvider
        {
            get
            {
                 if(siteMessageBusProvider == null)
                 {
                      siteMessageBusProvider = new SiteMessageBusiness();//ServiceBusContainer.Instance.Container.Resolve<ISiteMessageBusiness>();
                 }
                 return siteMessageBusProvider;
            }
            set
            {
              siteMessageBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteMessageService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public SiteMessageEntity GetSiteMessage(Int64 JournalID, Int64 messageID)
        {           
            var model = SiteMessageBusProvider.GetSiteMessage(messageID);
            if (model != null)
            {
                AuthorInfoService service = new AuthorInfoService();
                var dict = service.AuthorInfoBusProvider.GetAuthorDict(new AuthorInfoQuery() { JournalID = JournalID });
                model.SendUserName = dict.GetValue(model.SendUser, model.SendUser.ToString());
                model.ReciverName = dict.GetValue(model.ReciverID, model.ReciverID.ToString());
            }
            return model;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<SiteMessageEntity></returns>
        public List<SiteMessageEntity> GetSiteMessageList()
        {
            return SiteMessageBusProvider.GetSiteMessageList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="siteMessageQuery">SiteMessageQuery查询实体对象</param>
        /// <returns>List<SiteMessageEntity></returns>
        public List<SiteMessageEntity> GetSiteMessageList(SiteMessageQuery siteMessageQuery)
        {
            return GetMsgList(SiteMessageBusProvider.GetSiteMessageList(siteMessageQuery), siteMessageQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<SiteMessageEntity></returns>
        public Pager<SiteMessageEntity> GetSiteMessagePageList(CommonQuery query)
        {
            return SiteMessageBusProvider.GetSiteMessagePageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<SiteMessageEntity></returns>
        public Pager<SiteMessageEntity> GetSiteMessagePageList(QueryBase query)
        {
            return SiteMessageBusProvider.GetSiteMessagePageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="siteMessageQuery">SiteMessageQuery查询实体对象</param>
        /// <returns>Pager<SiteMessageEntity></returns>
        public Pager<SiteMessageEntity> GetSiteMessagePageList(SiteMessageQuery siteMessageQuery)
        {
            Pager<SiteMessageEntity> pager = SiteMessageBusProvider.GetSiteMessagePageList(siteMessageQuery);
            if (pager != null)
                pager.ItemList = GetMsgList(pager.ItemList.ToList(), siteMessageQuery);
            return pager;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<SiteMessageEntity> GetMsgList(List<SiteMessageEntity> list, SiteMessageQuery sQuery)
        {
            if (list == null || list.Count == 0)
                return list;
            AuthorInfoService service = new AuthorInfoService();
            AuthorInfoQuery query = new AuthorInfoQuery();
            query.JournalID = sQuery.JournalID;
            var dict = service.AuthorInfoBusProvider.GetAuthorDict(query);
            foreach (var model in list)
            {
                model.SendUserName = dict.GetValue(model.SendUser, model.SendUser.ToString());
                model.ReciverName = dict.GetValue(model.ReciverID, model.ReciverID.ToString());
                model.SimpleContent = TextHelper.GetSubHtmlText(model.Content, 100);
            }
            return list;
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
            return SiteMessageBusProvider.AddSiteMessage(siteMessage);
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
            return SiteMessageBusProvider.UpdateSiteMessage(siteMessage);
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
            return SiteMessageBusProvider.DeleteSiteMessage( messageID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteMessage">SiteMessageEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteSiteMessage(SiteMessageEntity siteMessage)
        {
            return SiteMessageBusProvider.DeleteSiteMessage(siteMessage);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteMessage(Int64[] messageID)
        {
            return SiteMessageBusProvider.BatchDeleteSiteMessage( messageID);
        }
        
        #endregion
        
        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(SiteMessageEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            model.Title = model.Title.TextFilter();
            model.Content = model.Content.HtmlFilter();
            if (model.MessageID == 0)
            {
                result = AddSiteMessage(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "发送站内消息成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "发送站内消息失败！";
                }
            }
            else
            {
                result = UpdateSiteMessage(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "编辑站内消息成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "编辑站内消息失败！";
                }
            }
            return execResult;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        public ExecResult Del(Int64[] MessageID)
        {
            ExecResult result = new ExecResult();
            if (MessageID == null)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = "没有删除任何数据！";
                return result;
            }
            bool flag = BatchDeleteSiteMessage(MessageID);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "删除站内信息成功！";
            }
            else
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "删除站内信息失败！";
            }
            return result;
        }

         /// <summary>
        /// 修改消息为已查看
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        public bool UpdateMsgViewed(Int64 JournalID, Int64 MessageID)
        {
            return SiteMessageBusProvider.UpdateMsgViewed(JournalID, MessageID);
        }
    }
}
