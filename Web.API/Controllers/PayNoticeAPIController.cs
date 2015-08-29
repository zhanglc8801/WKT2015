using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Service.Interface;
using WKT.Service.Wrapper;
using WKT.Model.Enum;

namespace Web.API.Controllers
{
    public class PayNoticeAPIController : ApiBaseController
    {
        /// <summary>
        /// 获取缴费通知分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<PayNoticeEntity> GetPageList(PayNoticeQuery query)
        {
            IPayNoticeService service = ServiceContainer.Instance.Container.Resolve<IPayNoticeService>();
            Pager<PayNoticeEntity> pager = service.GetPayNoticePageList(query);
            return pager;
        }

        /// <summary>
        /// 获取缴费通知列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<PayNoticeEntity> GetList(PayNoticeQuery query)
        {
            IPayNoticeService service = ServiceContainer.Instance.Container.Resolve<IPayNoticeService>();
            IList<PayNoticeEntity> list = service.GetPayNoticeList(query);
            return list;
        }

        /// <summary>
        /// 获取缴费通知实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public PayNoticeEntity GetModel(PayNoticeQuery query)
        {
            IPayNoticeService service = ServiceContainer.Instance.Container.Resolve<IPayNoticeService>();
            PayNoticeEntity model = service.GetPayNotice(query.NoticeID);
            return model;
        }

        /// <summary>
        /// 保存缴费通知
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Save(PayNoticeEntity model)
        {
            IPayNoticeService service = ServiceContainer.Instance.Container.Resolve<IPayNoticeService>();
            return service.Save(model);
        }


        /// <summary>
        /// 保存缴费通知
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult BatchSave(IList<PayNoticeEntity> list)
        {
            IPayNoticeService service = ServiceContainer.Instance.Container.Resolve<IPayNoticeService>();
            return service.BatchSave(list);
        }

        /// <summary>
        /// 删除缴费通知
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(PayNoticeQuery query)
        {
            IPayNoticeService service = ServiceContainer.Instance.Container.Resolve<IPayNoticeService>();
            return service.Del(query.NoticeIDs);
        }

        /// <summary>
        /// 更新缴费通知状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult ChangeStatus(PayNoticeQuery query)
        {
            IPayNoticeService service = ServiceContainer.Instance.Container.Resolve<IPayNoticeService>();
            ExecResult result = service.ChangeStatus(query);
            return result;
        }
    }
}
