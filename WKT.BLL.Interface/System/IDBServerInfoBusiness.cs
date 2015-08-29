using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface IDBServerInfoBusiness
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="dBServerID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        DBServerInfoEntity GetDBServerInfo(Int32 dBServerID);
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<DBServerInfoEntity></returns>
        List<DBServerInfoEntity> GetDBServerInfoList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="dBServerInfoQuery">DBServerInfoQuery查询实体对象</param>
        /// <returns>List<DBServerInfoEntity></returns>
        List<DBServerInfoEntity> GetDBServerInfoList(DBServerInfoQuery dBServerInfoQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<DBServerInfoEntity></returns>
        Pager<DBServerInfoEntity> GetDBServerInfoPageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<DBServerInfoEntity></returns>
        Pager<DBServerInfoEntity> GetDBServerInfoPageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="dBServerInfoQuery">DBServerInfoQuery查询实体对象</param>
        /// <returns>Pager<DBServerInfoEntity></returns>
        Pager<DBServerInfoEntity> GetDBServerInfoPageList(DBServerInfoQuery dBServerInfoQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="dBServerInfo">DBServerInfoEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddDBServerInfo(DBServerInfoEntity dBServerInfo);
        
        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="dBServerInfo">DBServerInfoEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateDBServerInfo(DBServerInfoEntity dBServerInfo);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="dBServerID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteDBServerInfo(Int32 dBServerID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="dBServerInfo">DBServerInfoEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteDBServerInfo(DBServerInfoEntity dBServerInfo);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dBServerID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteDBServerInfo(Int32[] dBServerID);
        
        #endregion
        
        #endregion 
    }
}






