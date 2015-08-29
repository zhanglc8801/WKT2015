using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Service.Interface;
using WKT.Service.Wrapper;
using WKT.Model.Enum;

namespace Web.API.Controllers
{
    public class ContantWayAPIController : ApiBaseController
    {
        /// <summary>
        /// 获取联系人分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<ContactWayEntity> GetPageList(ContactWayQuery query)
        {
            IContactWayService service = ServiceContainer.Instance.Container.Resolve<IContactWayService>();
            Pager<ContactWayEntity> pager = service.GetContactWayPageList(query);
            return pager;
        }

        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<ContactWayEntity> GetList(ContactWayQuery query)
        {
            IContactWayService service = ServiceContainer.Instance.Container.Resolve<IContactWayService>();
            IList<ContactWayEntity> list = service.GetContactWayList(query);
            return list;
        }

        /// <summary>
        /// 获取联系人实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ContactWayEntity GetModel(ContactWayQuery query)
        {
            IContactWayService service = ServiceContainer.Instance.Container.Resolve<IContactWayService>();
            ContactWayEntity model = service.GetContactWay(query.ContactID);
            return model;
        }

        /// <summary>
        /// 保存联系人
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Save(ContactWayEntity model)
        {
            IContactWayService service = ServiceContainer.Instance.Container.Resolve<IContactWayService>();
            return service.Save(model);
        }

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(ContactWayQuery query)
        {
            ExecResult execResult = new ExecResult();
            IContactWayService service = ServiceContainer.Instance.Container.Resolve<IContactWayService>();
            Int64[] ContactIDs = query.ContactIDs;
            if (ContactIDs == null)
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有删除任何数据！";
                return execResult;
            }
            string msg = string.Empty;
            bool result = service.BatchDeleteContactWay(ContactIDs);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除联系人成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除联系人失败！";
            }
            return execResult;
        }
    }
}
