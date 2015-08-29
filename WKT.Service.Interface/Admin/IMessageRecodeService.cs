using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface IMessageRecodeService
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="recodeID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        MessageRecodeEntity GetMessageRecode(MessageRecodeQuery query);
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<MessageRecodeEntity></returns>
        List<MessageRecodeEntity> GetMessageRecodeList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="messageRecodeQuery">MessageRecodeQuery查询实体对象</param>
        /// <returns>List<MessageRecodeEntity></returns>
        List<MessageRecodeEntity> GetMessageRecodeList(MessageRecodeQuery messageRecodeQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<MessageRecodeEntity></returns>
        Pager<MessageRecodeEntity> GetMessageRecodePageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<MessageRecodeEntity></returns>
        Pager<MessageRecodeEntity> GetMessageRecodePageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="messageRecodeQuery">MessageRecodeQuery查询实体对象</param>
        /// <returns>Pager<MessageRecodeEntity></returns>
        Pager<MessageRecodeEntity> GetMessageRecodePageList(MessageRecodeQuery messageRecodeQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="messageRecode">MessageRecodeEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddMessageRecode(MessageRecodeEntity messageRecode);
        
        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="messageRecode">MessageRecodeEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateMessageRecode(MessageRecodeEntity messageRecode);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="recodeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteMessageRecode(Int64 recodeID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="messageRecode">MessageRecodeEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteMessageRecode(MessageRecodeEntity messageRecode);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="recodeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteMessageRecode(Int64[] recodeID);
        
        #endregion
        
        #endregion 

        /// <summary>
        /// 保存发送记录
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool SaveSendRecode(IList<MessageRecodeEntity> list);
    }
}






