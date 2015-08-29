﻿using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public partial class ApiServerInfoBusiness : IApiServerInfoBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="apiServerID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public ApiServerInfoEntity GetApiServerInfo(Int32 apiServerID)
        {
           return ApiServerInfoDataAccess.Instance.GetApiServerInfo( apiServerID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<ApiServerInfoEntity></returns>
        public List<ApiServerInfoEntity> GetApiServerInfoList()
        {
            return ApiServerInfoDataAccess.Instance.GetApiServerInfoList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="apiServerInfoQuery">ApiServerInfoQuery查询实体对象</param>
        /// <returns>List<ApiServerInfoEntity></returns>
        public List<ApiServerInfoEntity> GetApiServerInfoList(ApiServerInfoQuery apiServerInfoQuery)
        {
            return ApiServerInfoDataAccess.Instance.GetApiServerInfoList(apiServerInfoQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<ApiServerInfoEntity></returns>
        public Pager<ApiServerInfoEntity> GetApiServerInfoPageList(CommonQuery query)
        {
            return ApiServerInfoDataAccess.Instance.GetApiServerInfoPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<ApiServerInfoEntity></returns>
        public Pager<ApiServerInfoEntity> GetApiServerInfoPageList(QueryBase query)
        {
            return ApiServerInfoDataAccess.Instance.GetApiServerInfoPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="apiServerInfoQuery">ApiServerInfoQuery查询实体对象</param>
        /// <returns>Pager<ApiServerInfoEntity></returns>
        public Pager<ApiServerInfoEntity> GetApiServerInfoPageList(ApiServerInfoQuery apiServerInfoQuery)
        {
            return ApiServerInfoDataAccess.Instance.GetApiServerInfoPageList(apiServerInfoQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="apiServerInfo">ApiServerInfoEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddApiServerInfo(ApiServerInfoEntity apiServerInfo)
        {
            return ApiServerInfoDataAccess.Instance.AddApiServerInfo(apiServerInfo);
        }
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="apiServerInfo">ApiServerInfoEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateApiServerInfo(ApiServerInfoEntity apiServerInfo)
        {
            return ApiServerInfoDataAccess.Instance.UpdateApiServerInfo(apiServerInfo);
        }
        
        #endregion 

        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="apiServerID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteApiServerInfo(Int32 apiServerID)
        {
            return ApiServerInfoDataAccess.Instance.DeleteApiServerInfo( apiServerID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="apiServerInfo">ApiServerInfoEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteApiServerInfo(ApiServerInfoEntity apiServerInfo)
        {
            return ApiServerInfoDataAccess.Instance.DeleteApiServerInfo(apiServerInfo);
        }
        
        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="apiServerID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteApiServerInfo(Int32[] apiServerID)
        {
            return ApiServerInfoDataAccess.Instance.BatchDeleteApiServerInfo( apiServerID);
        }
        #endregion
        
        #endregion
    }
}
