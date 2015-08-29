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
    public partial class DictService:IDictService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IDictBusiness dictBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IDictBusiness DictBusProvider
        {
            get
            {
                 if(dictBusProvider == null)
                 {
                     dictBusProvider = new DictBusiness();//ServiceBusContainer.Instance.Container.Resolve<IDictBusiness>();
                 }
                 return dictBusProvider;
            }
            set
            {
              dictBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DictService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="dictID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public DictEntity GetDict(Int64 dictID)
        {
           return DictBusProvider.GetDict( dictID);
        }

        public DictEntity GetDictByKey(string dictKey, long JournalID)
        {
            return DictBusProvider.GetDictByKey(dictKey,JournalID);
        }

        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<DictEntity></returns>
        public List<DictEntity> GetDictList()
        {
            return DictBusProvider.GetDictList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="dictQuery">DictQuery查询实体对象</param>
        /// <returns>List<DictEntity></returns>
        public List<DictEntity> GetDictList(DictQuery dictQuery)
        {
            return GetDictList(DictBusProvider.GetDictList(dictQuery), dictQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<DictEntity></returns>
        public Pager<DictEntity> GetDictPageList(CommonQuery query)
        {
            return DictBusProvider.GetDictPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<DictEntity></returns>
        public Pager<DictEntity> GetDictPageList(QueryBase query)
        {
            return DictBusProvider.GetDictPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="dictQuery">DictQuery查询实体对象</param>
        /// <returns>Pager<DictEntity></returns>
        public Pager<DictEntity> GetDictPageList(DictQuery dictQuery)
        {
            Pager<DictEntity> pager = DictBusProvider.GetDictPageList(dictQuery);
            if (pager != null)
                pager.ItemList = GetDictList(pager.ItemList.ToList(), dictQuery);
            return pager;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<DictEntity> GetDictList(List<DictEntity> list, DictQuery dictQuery)
        {
            if (list == null || list.Count == 0)
                return list;
            AuthorInfoService service = new AuthorInfoService();
            AuthorInfoQuery query=new AuthorInfoQuery();
            query.JournalID=dictQuery.JournalID;
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
        /// <param name="dict">DictEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddDict(DictEntity dict)
        {
            return DictBusProvider.AddDict(dict);
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
            return DictBusProvider.UpdateDict(dict);
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
            return DictBusProvider.DeleteDict( dictID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="dict">DictEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteDict(DictEntity dict)
        {
            return DictBusProvider.DeleteDict(dict);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dictID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public IList<Int64> BatchDeleteDict(IList<Int64> dictID)
        {
            return DictBusProvider.BatchDeleteDict( dictID);
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
            return DictBusProvider.DictkeyIsExists(model);
        }

        /// <summary>
        /// 保存数据字典
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(DictEntity model)
        {
            ExecResult execResult = new ExecResult();
            model.DictKey = model.DictKey.TextFilter();
            if (DictkeyIsExists(model))
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "该数据字典名称已存在！";
                return execResult;
            }
            bool result = false;
            model.Note = model.Note.TextFilter();
            if (model.DictID == 0)
            {
                result = AddDict(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增数据字典成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增数据字典失败！";
                }
            }
            else
            {
                result = UpdateDict(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "修改数据字典成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "修改数据字典失败！";
                }
            }
            return execResult;
        }

    }
}
