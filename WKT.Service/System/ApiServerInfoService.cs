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
    public partial class ApiServerInfoService:IApiServerInfoService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IApiServerInfoBusiness apiServerInfoBusiness=null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IApiServerInfoBusiness ApiServerInfoBusProvider
        {
            get{
                 if(apiServerInfoBusiness==null)
                 {
                      apiServerInfoBusiness = new ApiServerInfoBusiness();
                 }
                 return apiServerInfoBusiness;
            }
            set{
              apiServerInfoBusiness=value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiServerInfoService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="apiServerID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public ApiServerInfoEntity GetApiServerInfo(Int32 apiServerID)
        {
           return ApiServerInfoBusProvider.GetApiServerInfo( apiServerID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<ApiServerInfoEntity></returns>
        public List<ApiServerInfoEntity> GetApiServerInfoList()
        {
            return ApiServerInfoBusProvider.GetApiServerInfoList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="apiServerInfoQuery">ApiServerInfoQuery查询实体对象</param>
        /// <returns>List<ApiServerInfoEntity></returns>
        public List<ApiServerInfoEntity> GetApiServerInfoList(ApiServerInfoQuery apiServerInfoQuery)
        {
            return ApiServerInfoBusProvider.GetApiServerInfoList(apiServerInfoQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<ApiServerInfoEntity></returns>
        public Pager<ApiServerInfoEntity> GetApiServerInfoPageList(CommonQuery query)
        {
            return ApiServerInfoBusProvider.GetApiServerInfoPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<ApiServerInfoEntity></returns>
        public Pager<ApiServerInfoEntity> GetApiServerInfoPageList(QueryBase query)
        {
            return ApiServerInfoBusProvider.GetApiServerInfoPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="apiServerInfoQuery">ApiServerInfoQuery查询实体对象</param>
        /// <returns>Pager<ApiServerInfoEntity></returns>
        public Pager<ApiServerInfoEntity> GetApiServerInfoPageList(ApiServerInfoQuery apiServerInfoQuery)
        {
            return ApiServerInfoBusProvider.GetApiServerInfoPageList(apiServerInfoQuery);
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
            return ApiServerInfoBusProvider.AddApiServerInfo(apiServerInfo);
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
            return ApiServerInfoBusProvider.UpdateApiServerInfo(apiServerInfo);
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
            return ApiServerInfoBusProvider.DeleteApiServerInfo( apiServerID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="apiServerInfo">ApiServerInfoEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteApiServerInfo(ApiServerInfoEntity apiServerInfo)
        {
            return ApiServerInfoBusProvider.DeleteApiServerInfo(apiServerInfo);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="apiServerID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteApiServerInfo(Int32[] apiServerID)
        {
            return ApiServerInfoBusProvider.BatchDeleteApiServerInfo( apiServerID);
        }
        
        #endregion
        
        #endregion
    }
}
