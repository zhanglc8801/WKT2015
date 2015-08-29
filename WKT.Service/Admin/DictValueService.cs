using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;
using WKT.Model.Enum;
using WKT.Common.Extension;

namespace WKT.Service
{
    public partial class DictValueService:IDictValueService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IDictValueBusiness dictValueBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IDictValueBusiness DictValueBusProvider
        {
            get
            {
                 if(dictValueBusProvider == null)
                 {
                     dictValueBusProvider = new DictValueBusiness();//ServiceBusContainer.Instance.Container.Resolve<IDictValueBusiness>();
                 }
                 return dictValueBusProvider;
            }
            set
            {
              dictValueBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DictValueService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="dictValueID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public DictValueEntity GetDictValue(Int64 dictValueID)
        {
           return DictValueBusProvider.GetDictValue( dictValueID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<DictValueEntity></returns>
        public List<DictValueEntity> GetDictValueList()
        {
            return DictValueBusProvider.GetDictValueList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="dictValueQuery">DictValueQuery查询实体对象</param>
        /// <returns>List<DictValueEntity></returns>
        public List<DictValueEntity> GetDictValueList(DictValueQuery dictValueQuery)
        {
            return GetDictValueList(DictValueBusProvider.GetDictValueList(dictValueQuery), dictValueQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<DictValueEntity></returns>
        public Pager<DictValueEntity> GetDictValuePageList(CommonQuery query)
        {
            return DictValueBusProvider.GetDictValuePageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<DictValueEntity></returns>
        public Pager<DictValueEntity> GetDictValuePageList(QueryBase query)
        {
            return DictValueBusProvider.GetDictValuePageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="dictValueQuery">DictValueQuery查询实体对象</param>
        /// <returns>Pager<DictValueEntity></returns>
        public Pager<DictValueEntity> GetDictValuePageList(DictValueQuery dictValueQuery)
        {
            Pager<DictValueEntity> pager= DictValueBusProvider.GetDictValuePageList(dictValueQuery);
            if (pager != null)
                pager.ItemList = GetDictValueList(pager.ItemList.ToList(), dictValueQuery);
            return pager;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<DictValueEntity> GetDictValueList(List<DictValueEntity> list, DictValueQuery dictValueQuery)
        {
            if (list == null || list.Count == 0)
                return list;
            AuthorInfoService service = new AuthorInfoService();
            AuthorInfoQuery query = new AuthorInfoQuery();
            query.JournalID = dictValueQuery.JournalID;
            var dict = service.AuthorInfoBusProvider.GetAuthorDict(query);
            foreach (var mode in list)
            {
                mode.InUserName = dict.GetValue(mode.InUserID, mode.InUserID.ToString());
            }
            return list;
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
            return DictValueBusProvider.AddDictValue(dictValue);
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
            return DictValueBusProvider.UpdateDictValue(dictValue);
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
            return DictValueBusProvider.DeleteDictValue( dictValueID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="dictValue">DictValueEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteDictValue(DictValueEntity dictValue)
        {
            return DictValueBusProvider.DeleteDictValue(dictValue);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dictValueID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteDictValue(Int64[] dictValueID)
        {
            return DictValueBusProvider.BatchDeleteDictValue( dictValueID);
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
            return DictValueBusProvider.DictValueIsExists(model);
        }

        /// <summary>
        /// 保存数据字典值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(DictValueEntity model)
        {
            ExecResult execResult = new ExecResult();
            model.DictKey = model.DictKey.TextFilter();
            if (DictValueIsExists(model))
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "该数据字典值Value已存在！";
                return execResult;
            }
            bool result = false;
            model.ValueText = model.ValueText.TextFilter();
            if (model.DictValueID == 0)
            {
                result = AddDictValue(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增数据字典值成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增数据字典值失败！";
                }
            }
            else
            {
                result = UpdateDictValue(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "修改数据字典值成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "修改数据字典值失败！";
                }
            }
            return execResult;
        }

        /// <summary>
        /// 获取数据字典键值对
        /// </summary>
        /// <param name="dictKey"></param>
        /// <returns></returns>
        public IDictionary<int, string> GetDictValueDcit(Int64 JournalID, string dictKey)
        {
            return DictValueBusProvider.GetDictValueDcit(JournalID, dictKey);
        }
    }
}
