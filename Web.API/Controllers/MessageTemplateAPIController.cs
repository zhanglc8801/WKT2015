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
using WKT.Log;

namespace Web.API.Controllers
{
    public class MessageTemplateAPIController : ApiBaseController
    {
        /// <summary>
        /// 获取邮件短信模版分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<MessageTemplateEntity> GetPageList(MessageTemplateQuery query)
        {
            IMessageTemplateService service = ServiceContainer.Instance.Container.Resolve<IMessageTemplateService>();
            Pager<MessageTemplateEntity> pager = service.GetMessageTemplatePageList(query);
            return pager;
        }

        /// <summary>
        /// 获取邮件短信模版列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<MessageTemplateEntity> GetList(MessageTemplateQuery query)
        {
            IMessageTemplateService service = ServiceContainer.Instance.Container.Resolve<IMessageTemplateService>();
            IList<MessageTemplateEntity> list = service.GetMessageTemplateList(query);
            return list;
        }

        /// <summary>
        /// 获取邮件短信模版实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public MessageTemplateEntity GetModel(MessageTemplateQuery query)
        {
            IMessageTemplateService service = ServiceContainer.Instance.Container.Resolve<IMessageTemplateService>();
            MessageTemplateEntity model = null;
            if (query.ModelType == 1)
                model = service.GetMessageTemplate(query.JournalID, query.TCategory.Value, query.TType.Value);
            else
                model = service.GetMessageTemplate(query.TemplateID.Value);
            return model;
        }

        /// <summary>
        /// 保存邮件短信模版文件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Save(MessageTemplateEntity model)
        {
            IMessageTemplateService service = ServiceContainer.Instance.Container.Resolve<IMessageTemplateService>();
            return service.Save(model);
        }

        /// <summary>
        /// 删除邮件短信模版
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult Del(MessageTemplateQuery query)
        {
            ExecResult execResult = new ExecResult();
            IMessageTemplateService service = ServiceContainer.Instance.Container.Resolve<IMessageTemplateService>();
            Int64[] TemplateIDs = query.TemplateIDs;
            if (TemplateIDs == null)
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有删除任何数据！";
                return execResult;
            }
            string msg = query.TType == 2 ? "短信模版" : "邮件模版";
            bool result = service.BatchDeleteMessageTemplate(TemplateIDs);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "删除" + msg + "成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "删除" + msg + "失败！";
            }
            return execResult;
        }

        /// <summary>
        /// 获取模版类型键值对
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IDictionary<int, string> GetTCategoryDict(MessageTemplateQuery query)
        {
            IMessageTemplateService service = ServiceContainer.Instance.Container.Resolve<IMessageTemplateService>();
            return service.GetTCategoryDcit(query.JournalID, query.TType.Value, false);
        }

        /// <summary>
        /// 获取模版类型键值对(去除已经存在模版的模版类型)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IDictionary<int, string> GetTCategoryDictChecked(MessageTemplateQuery query)
        {
            IMessageTemplateService service = ServiceContainer.Instance.Container.Resolve<IMessageTemplateService>();
            return service.GetTCategoryDcit(query.JournalID, query.TType.Value, true);
        }

        /// <summary>
        /// 保存发送记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public bool SaveSendRecode(IList<MessageRecodeEntity> list)
        {
            try
            {
                IMessageRecodeService service = ServiceContainer.Instance.Container.Resolve<IMessageRecodeService>();
                return service.SaveSendRecode(list);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("保存发送记录异常：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取发送记录实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public MessageRecodeEntity GetMsgRecodeModel(MessageRecodeQuery query)
        {
            IMessageRecodeService service = ServiceContainer.Instance.Container.Resolve<IMessageRecodeService>();
            MessageRecodeEntity model = service.GetMessageRecode(query);
            return model;
        }

        /// <summary>
        /// 获取发送记录分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<MessageRecodeEntity> GetMsgRecodePageList(MessageRecodeQuery query)
        {
            IMessageRecodeService service = ServiceContainer.Instance.Container.Resolve<IMessageRecodeService>();
            Pager<MessageRecodeEntity> pager = service.GetMessageRecodePageList(query);
            return pager;
        }

        /// <summary>
        /// 获取发送记录数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<MessageRecodeEntity> GetMsgRecodeList(MessageRecodeQuery query)
        {
            IMessageRecodeService service = ServiceContainer.Instance.Container.Resolve<IMessageRecodeService>();
            IList<MessageRecodeEntity> pager = service.GetMessageRecodeList(query);
            return pager;
        }
    }
}
