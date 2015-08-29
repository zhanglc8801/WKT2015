using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;

namespace WKT.Service
{
    public partial class GuestbookService:IGuestbookService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IGuestbookBusiness guestbookBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IGuestbookBusiness GuestbookBusProvider
        {
            get
            {
                 if(guestbookBusProvider == null)
                 {
                     guestbookBusProvider = new GuestbookBusiness();//ServiceBusContainer.Instance.Container.Resolve<IGuestbookBusiness>();
                 }
                 return guestbookBusProvider;
            }
            set
            {
              guestbookBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public GuestbookService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public GuestbookEntity GetGuestbook(Int64 messageID)
        {
           return GuestbookBusProvider.GetGuestbook( messageID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<GuestbookEntity></returns>
        public List<GuestbookEntity> GetGuestbookList()
        {
            return GuestbookBusProvider.GetGuestbookList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="guestbookQuery">GuestbookQuery查询实体对象</param>
        /// <returns>List<GuestbookEntity></returns>
        public List<GuestbookEntity> GetGuestbookList(GuestbookQuery guestbookQuery)
        {
            return GuestbookBusProvider.GetGuestbookList(guestbookQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<GuestbookEntity></returns>
        public Pager<GuestbookEntity> GetGuestbookPageList(CommonQuery query)
        {
            return GuestbookBusProvider.GetGuestbookPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<GuestbookEntity></returns>
        public Pager<GuestbookEntity> GetGuestbookPageList(QueryBase query)
        {
            return GuestbookBusProvider.GetGuestbookPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="guestbookQuery">GuestbookQuery查询实体对象</param>
        /// <returns>Pager<GuestbookEntity></returns>
        public Pager<GuestbookEntity> GetGuestbookPageList(GuestbookQuery guestbookQuery)
        {
            return GuestbookBusProvider.GetGuestbookPageList(guestbookQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="guestbook">GuestbookEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddGuestbook(GuestbookEntity guestbook)
        {
            return GuestbookBusProvider.AddGuestbook(guestbook);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="guestbook">GuestbookEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateGuestbook(GuestbookEntity guestbook)
        {
            return GuestbookBusProvider.UpdateGuestbook(guestbook);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteGuestbook(Int64 messageID)
        {
            return GuestbookBusProvider.DeleteGuestbook( messageID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="guestbook">GuestbookEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteGuestbook(GuestbookEntity guestbook)
        {
            return GuestbookBusProvider.DeleteGuestbook(guestbook);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteGuestbook(Int64[] messageID)
        {
            return GuestbookBusProvider.BatchDeleteGuestbook( messageID);
        }
        
        #endregion
        
        #endregion
    }
}
