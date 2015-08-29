using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class DictValueBusiness : IDictValueBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="dictValueID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public DictValueEntity GetDictValue(Int64 dictValueID)
        {
           return DictValueDataAccess.Instance.GetDictValue( dictValueID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<DictValueEntity></returns>
        public List<DictValueEntity> GetDictValueList()
        {
            return DictValueDataAccess.Instance.GetDictValueList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="dictValueQuery">DictValueQuery查询实体对象</param>
        /// <returns>List<DictValueEntity></returns>
        public List<DictValueEntity> GetDictValueList(DictValueQuery dictValueQuery)
        {
            return DictValueDataAccess.Instance.GetDictValueList(dictValueQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<DictValueEntity></returns>
        public Pager<DictValueEntity> GetDictValuePageList(CommonQuery query)
        {
            return DictValueDataAccess.Instance.GetDictValuePageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<DictValueEntity></returns>
        public Pager<DictValueEntity> GetDictValuePageList(QueryBase query)
        {
            return DictValueDataAccess.Instance.GetDictValuePageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="dictValueQuery">DictValueQuery查询实体对象</param>
        /// <returns>Pager<DictValueEntity></returns>
        public Pager<DictValueEntity> GetDictValuePageList(DictValueQuery dictValueQuery)
        {
            return DictValueDataAccess.Instance.GetDictValuePageList(dictValueQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="dictValue">DictValueEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddDictValue(DictValueEntity dictValue)
        {
            return DictValueDataAccess.Instance.AddDictValue(dictValue);
        }
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="dictValue">DictValueEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateDictValue(DictValueEntity dictValue)
        {
            return DictValueDataAccess.Instance.UpdateDictValue(dictValue);
        }
        
        #endregion 

        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="dictValueID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteDictValue(Int64 dictValueID)
        {
            return DictValueDataAccess.Instance.DeleteDictValue( dictValueID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="dictValue">DictValueEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteDictValue(DictValueEntity dictValue)
        {
            return DictValueDataAccess.Instance.DeleteDictValue(dictValue);
        }
        
        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dictValueID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteDictValue(Int64[] dictValueID)
        {
            return DictValueDataAccess.Instance.BatchDeleteDictValue( dictValueID);
        }
        #endregion
        
        #endregion

        /// <summary>
        /// 数据字典值是否已经存在
        /// </summary>
        /// <param name="model"></param>       
        /// <returns></returns>
        public bool DictValueIsExists(DictValueEntity model)
        {
            return DictValueDataAccess.Instance.DictValueIsExists(model);
        }

        /// <summary>
        /// 获取数据字典键值对
        /// </summary>
        /// <param name="dictKey"></param>
        /// <returns></returns>
        public IDictionary<int, string> GetDictValueDcit(Int64 JournalID, string dictKey)
        {
            return DictValueDataAccess.Instance.GetDictValueDcit(JournalID, dictKey);
        }
    }
}
