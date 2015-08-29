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

namespace WKT.Service
{
    public partial class MessageRecodeService:IMessageRecodeService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IMessageRecodeBusiness messageRecodeBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IMessageRecodeBusiness MessageRecodeBusProvider
        {
            get
            {
                 if(messageRecodeBusProvider == null)
                 {
                      messageRecodeBusProvider = new MessageRecodeBusiness();//ServiceBusContainer.Instance.Container.Resolve<IMessageRecodeBusiness>();
                 }
                 return messageRecodeBusProvider;
            }
            set
            {
              messageRecodeBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageRecodeService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="recodeID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public MessageRecodeEntity GetMessageRecode(MessageRecodeQuery query)
        {
           return MessageRecodeBusProvider.GetMessageRecode( query);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<MessageRecodeEntity></returns>
        public List<MessageRecodeEntity> GetMessageRecodeList()
        {
            return MessageRecodeBusProvider.GetMessageRecodeList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="messageRecodeQuery">MessageRecodeQuery查询实体对象</param>
        /// <returns>List<MessageRecodeEntity></returns>
        public List<MessageRecodeEntity> GetMessageRecodeList(MessageRecodeQuery messageRecodeQuery)
        {
            return GetMsgList(MessageRecodeBusProvider.GetMessageRecodeList(messageRecodeQuery), messageRecodeQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<MessageRecodeEntity></returns>
        public Pager<MessageRecodeEntity> GetMessageRecodePageList(CommonQuery query)
        {
            return MessageRecodeBusProvider.GetMessageRecodePageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<MessageRecodeEntity></returns>
        public Pager<MessageRecodeEntity> GetMessageRecodePageList(QueryBase query)
        {
            return MessageRecodeBusProvider.GetMessageRecodePageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="messageRecodeQuery">MessageRecodeQuery查询实体对象</param>
        /// <returns>Pager<MessageRecodeEntity></returns>
        public Pager<MessageRecodeEntity> GetMessageRecodePageList(MessageRecodeQuery messageRecodeQuery)
        {
            Pager<MessageRecodeEntity> pager = MessageRecodeBusProvider.GetMessageRecodePageList(messageRecodeQuery);
            if (pager != null)
                pager.ItemList = GetMsgList(pager.ItemList.ToList(), messageRecodeQuery);
            return pager;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<MessageRecodeEntity> GetMsgList(List<MessageRecodeEntity> list, MessageRecodeQuery mQuery)
        {
            if (list == null || list.Count == 0)
                return list;
            AuthorInfoService service = new AuthorInfoService();
            AuthorInfoEntity entity = new AuthorInfoEntity();
            AuthorInfoQuery query = new AuthorInfoQuery();
            query.JournalID = mQuery.JournalID;
            var dict = service.AuthorInfoBusProvider.GetAuthorDict(query);
            

            //获取稿件作者数据字典
            ContributionInfoService cservice = new ContributionInfoService();
            ContributionInfoEntity centity = new ContributionInfoEntity();
            ContributionInfoQuery cquery = new ContributionInfoQuery();
            cquery.JournalID = mQuery.JournalID;
            cquery.CID = (long)mQuery.CID;
            var cdict = cservice.GetContributionAuthorDict(cquery);
            centity = cservice.GetContributionInfo(cquery);

            foreach (var mode in list)
            {
                mode.SendUserName = dict.GetValue(mode.SendUser, mode.SendUser.ToString());
                entity = service.GetAuthorInfo(mode.ReciveUser);
                if(entity!=null)
                    mode.ReciveUserName = dict.GetValue(mode.ReciveUser, mode.ReciveUser.ToString());
                else
                    mode.ReciveUserName = cdict.GetValue(mode.ReciveUser, mode.ReciveUser.ToString());                  
                                
            }
            return list;
        }
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="messageRecode">MessageRecodeEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddMessageRecode(MessageRecodeEntity messageRecode)
        {
            return MessageRecodeBusProvider.AddMessageRecode(messageRecode);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="messageRecode">MessageRecodeEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateMessageRecode(MessageRecodeEntity messageRecode)
        {
            return MessageRecodeBusProvider.UpdateMessageRecode(messageRecode);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="recodeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteMessageRecode(Int64 recodeID)
        {
            return MessageRecodeBusProvider.DeleteMessageRecode( recodeID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="messageRecode">MessageRecodeEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteMessageRecode(MessageRecodeEntity messageRecode)
        {
            return MessageRecodeBusProvider.DeleteMessageRecode(messageRecode);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="recodeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteMessageRecode(Int64[] recodeID)
        {
            return MessageRecodeBusProvider.BatchDeleteMessageRecode( recodeID);
        }
        
        #endregion
        
        #endregion

        /// <summary>
        /// 保存发送记录
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool SaveSendRecode(IList<MessageRecodeEntity> list)
        {
            return MessageRecodeBusProvider.SaveSendRecode(list);
        }
    }
}
