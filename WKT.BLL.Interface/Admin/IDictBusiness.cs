﻿using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface IDictBusiness
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="dictID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        DictEntity GetDict(Int64 dictID);

        DictEntity GetDictByKey(string dictKey, long JournalID);
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<DictEntity></returns>
        List<DictEntity> GetDictList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="dictQuery">DictQuery查询实体对象</param>
        /// <returns>List<DictEntity></returns>
        List<DictEntity> GetDictList(DictQuery dictQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<DictEntity></returns>
        Pager<DictEntity> GetDictPageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<DictEntity></returns>
        Pager<DictEntity> GetDictPageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="dictQuery">DictQuery查询实体对象</param>
        /// <returns>Pager<DictEntity></returns>
        Pager<DictEntity> GetDictPageList(DictQuery dictQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="dict">DictEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddDict(DictEntity dict);
        
        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="dict">DictEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateDict(DictEntity dict);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="dictID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteDict(Int64 dictID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="dict">DictEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteDict(DictEntity dict);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dictID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        IList<Int64> BatchDeleteDict(IList<Int64> dictID);
        
        #endregion
        
        #endregion 

        /// <summary>
        /// 数据字典是否已经存在
        /// </summary>
        /// <param name="model"></param>       
        /// <returns></returns>
        bool DictkeyIsExists(DictEntity model);
    }
}






