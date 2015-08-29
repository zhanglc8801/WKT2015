using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public partial class JournalSetInfoBusiness : IJournalSetInfoBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="setID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public JournalSetInfoEntity GetJournalSetInfo(Int32 setID)
        {
           return JournalSetInfoDataAccess.Instance.GetJournalSetInfo( setID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<JournalSetInfoEntity></returns>
        public List<JournalSetInfoEntity> GetJournalSetInfoList()
        {
            return JournalSetInfoDataAccess.Instance.GetJournalSetInfoList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="journalSetInfoQuery">JournalSetInfoQuery查询实体对象</param>
        /// <returns>List<JournalSetInfoEntity></returns>
        public List<JournalSetInfoEntity> GetJournalSetInfoList(JournalSetInfoQuery journalSetInfoQuery)
        {
            return JournalSetInfoDataAccess.Instance.GetJournalSetInfoList(journalSetInfoQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<JournalSetInfoEntity></returns>
        public Pager<JournalSetInfoEntity> GetJournalSetInfoPageList(CommonQuery query)
        {
            return JournalSetInfoDataAccess.Instance.GetJournalSetInfoPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<JournalSetInfoEntity></returns>
        public Pager<JournalSetInfoEntity> GetJournalSetInfoPageList(QueryBase query)
        {
            return JournalSetInfoDataAccess.Instance.GetJournalSetInfoPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="journalSetInfoQuery">JournalSetInfoQuery查询实体对象</param>
        /// <returns>Pager<JournalSetInfoEntity></returns>
        public Pager<JournalSetInfoEntity> GetJournalSetInfoPageList(JournalSetInfoQuery journalSetInfoQuery)
        {
            return JournalSetInfoDataAccess.Instance.GetJournalSetInfoPageList(journalSetInfoQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="journalSetInfo">JournalSetInfoEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddJournalSetInfo(JournalSetInfoEntity journalSetInfo)
        {
            return JournalSetInfoDataAccess.Instance.AddJournalSetInfo(journalSetInfo);
        }
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="journalSetInfo">JournalSetInfoEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateJournalSetInfo(JournalSetInfoEntity journalSetInfo)
        {
            return JournalSetInfoDataAccess.Instance.UpdateJournalSetInfo(journalSetInfo);
        }
        
        #endregion 

        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="setID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteJournalSetInfo(Int32 setID)
        {
            return JournalSetInfoDataAccess.Instance.DeleteJournalSetInfo( setID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="journalSetInfo">JournalSetInfoEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteJournalSetInfo(JournalSetInfoEntity journalSetInfo)
        {
            return JournalSetInfoDataAccess.Instance.DeleteJournalSetInfo(journalSetInfo);
        }
        
        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="setID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteJournalSetInfo(Int32[] setID)
        {
            return JournalSetInfoDataAccess.Instance.BatchDeleteJournalSetInfo( setID);
        }
        #endregion
        
        #endregion

         /// <summary>
        /// 获取指定库指定表的最大ID
        /// </summary>
        /// <param name="DBName">数据库名称</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public int GetMaxID(long JournalID, string DBName, string TableName)
        {
            return JournalSetInfoDataAccess.Instance.GetMaxID(JournalID, DBName, TableName);
        }

        /// <summary>
        /// 获取指定库指定表的最大ID
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="DBName">数据库名称</param>
        /// <param name="TableName">表名</param>
        /// <param name="Num">位数</param>
        /// <returns></returns>
        public string GetMaxID(long JournalID, string DBName, string TableName, int Num)
        {
            return JournalSetInfoDataAccess.Instance.GetMaxID(JournalID, DBName, TableName, Num);
        }
    }
}
