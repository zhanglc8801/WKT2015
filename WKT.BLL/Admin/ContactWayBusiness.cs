using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class ContactWayBusiness : IContactWayBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public ContactWayEntity GetContactWay(Int64 contactID)
        {
           return ContactWayDataAccess.Instance.GetContactWay( contactID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<ContactWayEntity></returns>
        public List<ContactWayEntity> GetContactWayList()
        {
            return ContactWayDataAccess.Instance.GetContactWayList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="contactWayQuery">ContactWayQuery查询实体对象</param>
        /// <returns>List<ContactWayEntity></returns>
        public List<ContactWayEntity> GetContactWayList(ContactWayQuery contactWayQuery)
        {
            return ContactWayDataAccess.Instance.GetContactWayList(contactWayQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<ContactWayEntity></returns>
        public Pager<ContactWayEntity> GetContactWayPageList(CommonQuery query)
        {
            return ContactWayDataAccess.Instance.GetContactWayPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<ContactWayEntity></returns>
        public Pager<ContactWayEntity> GetContactWayPageList(QueryBase query)
        {
            return ContactWayDataAccess.Instance.GetContactWayPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="contactWayQuery">ContactWayQuery查询实体对象</param>
        /// <returns>Pager<ContactWayEntity></returns>
        public Pager<ContactWayEntity> GetContactWayPageList(ContactWayQuery contactWayQuery)
        {
            return ContactWayDataAccess.Instance.GetContactWayPageList(contactWayQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="contactWay">ContactWayEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddContactWay(ContactWayEntity contactWay)
        {
            return ContactWayDataAccess.Instance.AddContactWay(contactWay);
        }
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="contactWay">ContactWayEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateContactWay(ContactWayEntity contactWay)
        {
            return ContactWayDataAccess.Instance.UpdateContactWay(contactWay);
        }
        
        #endregion 

        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteContactWay(Int64 contactID)
        {
            return ContactWayDataAccess.Instance.DeleteContactWay( contactID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="contactWay">ContactWayEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteContactWay(ContactWayEntity contactWay)
        {
            return ContactWayDataAccess.Instance.DeleteContactWay(contactWay);
        }
        
        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteContactWay(Int64[] contactID)
        {
            return ContactWayDataAccess.Instance.BatchDeleteContactWay( contactID);
        }
        #endregion
        
        #endregion
    }
}
