using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using Microsoft.Practices.Unity;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Service.Interface;
using WKT.Service.Wrapper;
using WKT.Log;

namespace Web.API.Controllers
{
    public class ExpertApplyAPIController : ApiController
    {

        
        /// <summary>
        /// 获取专家申请信息实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public ExpertApplyLogEntity GetModel(ExpertApplyLogQuery query)
        {
            IExpertApplyService service = ServiceContainer.Instance.Container.Resolve<IExpertApplyService>();
            ExpertApplyLogEntity model = service.GetExpertApplyInfo(query);
            return model;
        }

        /// <summary>
        /// 获取专家申请信息列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<ExpertApplyLogEntity> GetList(ExpertApplyLogQuery query)
        {
            IExpertApplyService service = ServiceContainer.Instance.Container.Resolve<IExpertApplyService>();
            IList<ExpertApplyLogEntity> list = service.GetExpertApplyInfoList(query);
            return list;
        }

        /// <summary>
        /// 获取专家申请分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<ExpertApplyLogEntity> GetPageList(ExpertApplyLogQuery query)
        {
            IExpertApplyService service = ServiceContainer.Instance.Container.Resolve<IExpertApplyService>();
            Pager<ExpertApplyLogEntity> pager = service.GetExpertApplyInfoPageList(query);
            return pager;
        }

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="expertApplyEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult SubmitApply(ExpertApplyLogEntity expertApplyEntity)
        {
            ExecResult execResult = new ExecResult();
            try
            {
                IExpertApplyService expertApplyService = ServiceContainer.Instance.Container.Resolve<IExpertApplyService>();

                ExpertApplyLogQuery queryExpertApply = new ExpertApplyLogQuery();
                queryExpertApply.LoginName = expertApplyEntity.LoginName;
                queryExpertApply.JournalID = expertApplyEntity.JournalID;

                //验证是否提交过申请
                IList<ExpertApplyLogEntity> list = expertApplyService.GetExpertApplyInfoList(queryExpertApply);
                if (list != null && list.Count > 0)
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "此邮箱提交过申请！请更换其他邮箱提交申请或联系编辑部。";
                }
                else//提交申请
                {
                    expertApplyService.SubmitApply(expertApplyEntity);
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "申请提交成功，请等待编辑部审核。\r\n审核结果将会以邮件形式发送到您的邮箱。";
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "提交申请失败：" + ex.Message;
                LogProvider.Instance.Error("提交申请出现异常：" + ex.Message);
            }
            return execResult;
        }
        /// <summary>
        /// 更新申请信息
        /// </summary>
        /// <param name="expertApplyEntity"></param>
        /// <returns></returns>
        public ExecResult UpdateApply(ExpertApplyLogEntity expertApplyEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IExpertApplyService service = ServiceContainer.Instance.Container.Resolve<IExpertApplyService>();
                bool flag = service.UpdateApply(expertApplyEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "审核失败";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "审核专家申请时出现异常：" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 删除专家申请信息
        /// </summary>
        /// <param name="expertApplyEntity"></param>
        /// <returns></returns>
        public ExecResult DelApply(long PKID)
        {
            ExecResult result = new ExecResult();
            try
            {
                IExpertApplyService service = ServiceContainer.Instance.Container.Resolve<IExpertApplyService>();
                bool flag = service.DelApply(PKID);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "删除成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "删除失败";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "删除专家申请时出现异常：" + ex.Message;
            }
            return result;
        }


    }
}
