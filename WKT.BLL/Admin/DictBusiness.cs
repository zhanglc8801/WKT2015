using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class DictBusiness : IDictBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="dictID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public DictEntity GetDict(Int64 dictID)
        {
           return DictDataAccess.Instance.GetDict( dictID);
        }

        public DictEntity GetDictByKey(string dictKey, long JournalID)
        {
            return DictDataAccess.Instance.GetDictByKey(dictKey,JournalID);
        }

        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<DictEntity></returns>
        public List<DictEntity> GetDictList()
        {
            return DictDataAccess.Instance.GetDictList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="dictQuery">DictQuery查询实体对象</param>
        /// <returns>List<DictEntity></returns>
        public List<DictEntity> GetDictList(DictQuery dictQuery)
        {
            return DictDataAccess.Instance.GetDictList(dictQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<DictEntity></returns>
        public Pager<DictEntity> GetDictPageList(CommonQuery query)
        {
            return DictDataAccess.Instance.GetDictPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<DictEntity></returns>
        public Pager<DictEntity> GetDictPageList(QueryBase query)
        {
            return DictDataAccess.Instance.GetDictPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="dictQuery">DictQuery查询实体对象</param>
        /// <returns>Pager<DictEntity></returns>
        public Pager<DictEntity> GetDictPageList(DictQuery dictQuery)
        {
            return DictDataAccess.Instance.GetDictPageList(dictQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="dict">DictEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddDict(DictEntity dict)
        {
            return DictDataAccess.Instance.AddDict(dict);
        }
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="dict">DictEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateDict(DictEntity dict)
        {
            return DictDataAccess.Instance.UpdateDict(dict);
        }
        
        #endregion 

        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="dictID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteDict(Int64 dictID)
        {
            return DictDataAccess.Instance.DeleteDict( dictID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="dict">DictEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteDict(DictEntity dict)
        {
            return DictDataAccess.Instance.DeleteDict(dict);
        }
        
        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dictID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public IList<Int64> BatchDeleteDict(IList<Int64> dictID)
        {
            return DictDataAccess.Instance.BatchDeleteDict( dictID);
        }
        #endregion
        
        #endregion

        /// <summary>
        /// 数据字典是否已经存在
        /// </summary>
        /// <param name="model"></param>       
        /// <returns></returns>
        public bool DictkeyIsExists(DictEntity model)
        {
            return DictDataAccess.Instance.DictkeyIsExists(model);
        }

    }
}
