using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Service.Interface;
using WKT.Service.Wrapper;
using WKT.Model.Enum;

namespace Web.API.Controllers
{
    public class DictAPIController:ApiBaseController
    {
        #region 数据字典
        /// <summary>
        /// 获取数据字典分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<DictEntity> GetDictPageList(DictQuery query)
        {
            IDictService service = ServiceContainer.Instance.Container.Resolve<IDictService>();
            Pager<DictEntity> pager = service.GetDictPageList(query);
            return pager;
        }

        /// <summary>
        /// 获取数据字典实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public DictEntity GetDictModel(DictQuery query)
        {
            IDictService service = ServiceContainer.Instance.Container.Resolve<IDictService>();
            DictEntity model = service.GetDict(query.DictID);
            return model;
        }

        /// <summary>
        /// 获取数据字典实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public DictEntity GetDictModelByKey(DictQuery query)
        {
            IDictService service = ServiceContainer.Instance.Container.Resolve<IDictService>();
            DictEntity model = service.GetDictByKey(query.DictKey,query.JournalID);
            return model;
        }

        /// <summary>
        /// 保存数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveDict(DictEntity model)
        {
            ExecResult execResult = new ExecResult();
            IDictService service = ServiceContainer.Instance.Container.Resolve<IDictService>();
            return service.Save(model);           
        }

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult DelDict(DictQuery query)
        {
            ExecResult execResult = new ExecResult();
            IDictService service = ServiceContainer.Instance.Container.Resolve<IDictService>();
            IList<Int64> dictIDs = query.DictIDs.ToList();
            if (dictIDs == null)
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有删除任何数据！";
                return execResult;
            }
            string msg = string.Empty;
            IList<Int64> list = service.BatchDeleteDict(dictIDs);
            if (list == null || list.Count < dictIDs.Count)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除数据字典成功！";
                if (list != null && list.Count > 0)
                    execResult.msg += string.Format("部分编号[{0}]由于存在数据字典值，请先删除", string.Join(",", list));
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除数据字典失败！";
            }
            return execResult;
        }
        #endregion

        #region 数据字典值
        /// <summary>
        /// 获取数据字典值分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<DictValueEntity> GetDictValuePageList(DictValueQuery query)
        {
            IDictValueService service = ServiceContainer.Instance.Container.Resolve<IDictValueService>();
            Pager<DictValueEntity> pager = service.GetDictValuePageList(query);
            return pager;
        }

        /// <summary>
        /// 获取数据字典值实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public DictValueEntity GetDictValueModel(DictValueQuery query)
        {
            IDictValueService service = ServiceContainer.Instance.Container.Resolve<IDictValueService>();
            DictValueEntity model = service.GetDictValue(query.DictValueID);
            return model;
        }

        /// <summary>
        /// 保存数据字典值
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SaveDictValue(DictValueEntity model)
        {
            ExecResult execResult = new ExecResult();
            IDictValueService service = ServiceContainer.Instance.Container.Resolve<IDictValueService>();
            return service.Save(model);
        }

        /// <summary>
        /// 删除数据字典值
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult DelDictValue(DictValueQuery query)
        {
            ExecResult execResult = new ExecResult();
            IDictValueService service = ServiceContainer.Instance.Container.Resolve<IDictValueService>();
            Int64[] dictValueIDs = query.DictValueIDs;
            if (dictValueIDs == null)
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有删除任何数据！";
                return execResult;
            }
            string msg = string.Empty;
            bool result = service.BatchDeleteDictValue(dictValueIDs);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除数据字典成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除数据字典失败！";
            }
            return execResult;
        }

        /// <summary>
        /// 获取数据字典值键值对
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IDictionary<int, string> GetDictValueDcit(DictValueQuery query)
        {
            IDictValueService service = ServiceContainer.Instance.Container.Resolve<IDictValueService>();
            return service.GetDictValueDcit(query.JournalID, query.DictKey);
        }


        /// <summary>
        /// 根据查询条件获取所有数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<DictValueEntity> GetDictValueList(DictValueQuery query)
        {
            IDictValueService service = ServiceContainer.Instance.Container.Resolve<IDictValueService>();
            return service.GetDictValueList(query);
        }

        #endregion
    }
}