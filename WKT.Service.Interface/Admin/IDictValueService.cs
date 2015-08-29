using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface IDictValueService
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="dictValueID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        DictValueEntity GetDictValue(Int64 dictValueID);
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<DictValueEntity></returns>
        List<DictValueEntity> GetDictValueList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="dictValueQuery">DictValueQuery查询实体对象</param>
        /// <returns>List<DictValueEntity></returns>
        List<DictValueEntity> GetDictValueList(DictValueQuery dictValueQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<DictValueEntity></returns>
        Pager<DictValueEntity> GetDictValuePageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<DictValueEntity></returns>
        Pager<DictValueEntity> GetDictValuePageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="dictValueQuery">DictValueQuery查询实体对象</param>
        /// <returns>Pager<DictValueEntity></returns>
        Pager<DictValueEntity> GetDictValuePageList(DictValueQuery dictValueQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="dictValue">DictValueEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddDictValue(DictValueEntity dictValue);
        
        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="dictValue">DictValueEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateDictValue(DictValueEntity dictValue);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="dictValueID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteDictValue(Int64 dictValueID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="dictValue">DictValueEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteDictValue(DictValueEntity dictValue);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dictValueID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteDictValue(Int64[] dictValueID);
        
        #endregion
        
        #endregion 

        /// <summary>
        /// 数据字典值是否已经存在
        /// </summary>
        /// <param name="model"></param>       
        /// <returns></returns>
        bool DictValueIsExists(DictValueEntity model);

        /// <summary>
        /// 保存数据字典值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult Save(DictValueEntity model);

        /// <summary>
        /// 获取数据字典键值对
        /// </summary>
        /// <param name="dictKey"></param>
        /// <returns></returns>
        IDictionary<int, string> GetDictValueDcit(Int64 JournalID, string dictKey);
    }
}






