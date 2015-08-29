using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Service.Interface;
using WKT.BLL;
using WKT.BLL.Interface;

namespace WKT.Service
{
    public partial class JournalInfoService:IJournalInfoService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IJournalInfoBusiness journalInfoBusiness = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IJournalInfoBusiness JournalInfoBusProvider
        {
            get{
                 if(journalInfoBusiness == null)
                 {
                      journalInfoBusiness = new JournalInfoBusiness();
                 }
                 return journalInfoBusiness;
            }
            set{
              journalInfoBusiness=value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public JournalInfoService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="journalID">站点ID</param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public JournalInfoEntity GetJournalInfo(Int64 journalID)
        {
           return JournalInfoBusProvider.GetJournalInfo( journalID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<JournalInfoEntity></returns>
        public List<JournalInfoEntity> GetJournalInfoList()
        {
            return JournalInfoBusProvider.GetJournalInfoList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="journalInfoQuery">JournalInfoQuery查询实体对象</param>
        /// <returns>List<JournalInfoEntity></returns>
        public List<JournalInfoEntity> GetJournalInfoList(JournalInfoQuery journalInfoQuery)
        {
            return JournalInfoBusProvider.GetJournalInfoList(journalInfoQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<JournalInfoEntity></returns>
        public Pager<JournalInfoEntity> GetJournalInfoPageList(CommonQuery query)
        {
            return JournalInfoBusProvider.GetJournalInfoPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<JournalInfoEntity></returns>
        public Pager<JournalInfoEntity> GetJournalInfoPageList(QueryBase query)
        {
            return JournalInfoBusProvider.GetJournalInfoPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="journalInfoQuery">JournalInfoQuery查询实体对象</param>
        /// <returns>Pager<JournalInfoEntity></returns>
        public Pager<JournalInfoEntity> GetJournalInfoPageList(JournalInfoQuery journalInfoQuery)
        {
            return JournalInfoBusProvider.GetJournalInfoPageList(journalInfoQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="journalInfo">JournalInfoEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddJournalInfo(JournalInfoEntity journalInfo)
        {
            return JournalInfoBusProvider.AddJournalInfo(journalInfo);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="journalInfo">JournalInfoEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateJournalInfo(JournalInfoEntity journalInfo)
        {
            return JournalInfoBusProvider.UpdateJournalInfo(journalInfo);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="journalID">站点ID</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteJournalInfo(Int64 journalID)
        {
            return JournalInfoBusProvider.DeleteJournalInfo( journalID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="journalInfo">JournalInfoEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteJournalInfo(JournalInfoEntity journalInfo)
        {
            return JournalInfoBusProvider.DeleteJournalInfo(journalInfo);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="journalID">站点ID</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteJournalInfo(Int64[] journalID)
        {
            return JournalInfoBusProvider.BatchDeleteJournalInfo( journalID);
        }
        
        #endregion
        
        #endregion
    }
}
