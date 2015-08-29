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
using WKT.Common.Extension;

namespace Web.API.Controllers
{
    public class FlowAPIController : ApiBaseController
    {
        # region 审稿状态

        /// <summary>
        /// 获取拥有权限的审稿状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<FlowStatusEntity> GetHaveRightFlowStatus(FlowStatusQuery query)
        {
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                IList<FlowStatusEntity> listStatus = flowStatusService.GetHaveRightFlowStatus(query);
                return listStatus;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取拥有权限的审稿状态出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        /// <summary>
        /// 获取拥有权限的审稿状态(用于统计同一稿件一个状态下送多人时按一个计算)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<FlowStatusEntity> GetHaveRightFlowStatusForStat(FlowStatusQuery query)
        {
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                IList<FlowStatusEntity> listStatus = flowStatusService.GetHaveRightFlowStatusForStat(query);
                return listStatus;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取拥有权限的审稿状态出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取审稿状态键值对，审稿状态名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IDictionary<long, string> GetFlowStatusDictStatusName(FlowStatusQuery query)
        {
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                IDictionary<long, string> dictStatusName = flowStatusService.GetFlowStatusDictStatusName(query);
                return dictStatusName;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取审稿状态键值对[状态名称]出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取审稿状态键值对，审稿状态显示名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IDictionary<long, string> GetFlowStatusDictDisplayName(FlowStatusQuery query)
        {
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                IDictionary<long, string> dictStatusDisplay = flowStatusService.GetFlowStatusDictDisplayName(query);
                return dictStatusDisplay;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取审稿状态键值对[显示名称]出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 根据指定的审稿状态ID，得到审稿状态的基本信息
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="StatusID"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public FlowStatusEntity GetFlowStatusInfoByID(FlowStatusQuery query)
        {
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                FlowStatusEntity statusEntity = flowStatusService.GetFlowStatusInfoByID(query);
                return statusEntity;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("根据指定的审稿状态ID，得到审稿状态的基本信息出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取审稿状态列表
        /// </summary>
        /// <param name="queryFlowSet"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public List<FlowStatusEntity> GetFlowStatusList(FlowStatusQuery queryFlowStatus)
        {
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                List<FlowStatusEntity> listFlowStatus = flowStatusService.GetFlowStatusList(queryFlowStatus);
                return listFlowStatus;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取审稿流程状态列表出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取审稿流程配置步骤序号
        /// </summary>
        /// <param name="queryFlowSet"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public int GetFlowStatusSortID(FlowStatusQuery queryFlowStatus)
        {
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                int SortID = flowStatusService.GetFlowStatusSortID(queryFlowStatus);
                return SortID;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取审稿流程状态序号出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 判断审稿状态对应的稿件状态是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public FlowStatusEntity CheckCStatusIsExists(FlowStatusQuery query)
        {
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                FlowStatusEntity statusEntity = flowStatusService.CheckCStatusIsExists(query);
                return statusEntity;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("判断审稿状态对应的稿件状态是否存在出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取审稿步骤基本信息及配置信息
        /// </summary>
        /// <param name="queryFlowSet"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public FlowStep GetFlowStepInfo(FlowStatusQuery queryFlowStatus)
        {
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                FlowStep stepEntity = flowStatusService.GetFlowStep(queryFlowStatus);
                return stepEntity;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取审稿流程状态信息及配置信息出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 新增审稿环节
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult AddFlowStatus(FlowStep flowStepEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                bool flag = flowStatusService.AddFlowStatus(flowStepEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "添加审稿状态失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "添加审稿状态时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 修改审稿环节
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult EditFlowStatus(FlowStep flowStepEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                bool flag = flowStatusService.UpdateFlowStatus(flowStepEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "修改审稿状态失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "修改审稿状态时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 删除审稿状态
        /// </summary>
        /// <param name="flowStatusEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult DelFlowStatus(FlowStatusEntity flowStatusEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                bool flag = flowStatusService.DeleteFlowStatus(flowStatusEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "删除审稿状态失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "删除审稿状态时出现异常：" + ex.Message;
            }
            return result;
        }

        #region 修改审稿状态状态

        /// <summary>
        /// 修改审稿状态状态
        /// </summary>
        /// <param name="flowStatusEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult UpdateFlowStatus(FlowStatusEntity flowStatusEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowStatusService flowStatusService = ServiceContainer.Instance.Container.Resolve<IFlowStatusService>();
                bool flag = flowStatusService.UpdateFlowStatusStatus(flowStatusEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "修改审稿状态状态失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "修改审稿状态状态时出现异常：" + ex.Message;
            }
            return result;
        }

        # endregion

        # endregion

        # region 审稿操作

        /// <summary>
        /// 获取操作实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public FlowActionEntity GetFlowActionEntity(FlowActionQuery query)
        {
            try
            {
                IFlowActionService flowActionService = ServiceContainer.Instance.Container.Resolve<IFlowActionService>();
                FlowActionEntity flowActionEntity = flowActionService.GetFlowActionEntity(query);
                return flowActionEntity;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取操作实体出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 根据当前操作状态获取可以做的操作
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public List<FlowActionEntity> GetFlowActionByStatus(FlowActionQuery query)
        {
            try
            {
                IFlowActionService flowActionService = ServiceContainer.Instance.Container.Resolve<IFlowActionService>();
                List<FlowActionEntity> listFlowAction = flowActionService.GetFlowActionByStatus(query);
                return listFlowAction;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("根据当前操作状态获取可以做的操作出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取审稿操作设置
        /// </summary>
        /// <param name="queryFlowSet"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public List<FlowActionEntity> GetFlowActionList(FlowActionQuery queryFlowAction)
        {
            try
            {
                IFlowActionService flowActionService = ServiceContainer.Instance.Container.Resolve<IFlowActionService>();
                List<FlowActionEntity> listFlowAction = flowActionService.GetFlowActionList(queryFlowAction);
                return listFlowAction;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取审稿操作出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 新增审稿操作
        /// </summary>
        /// <param name="flowActionEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult AddFlowAction(FlowActionEntity flowActionEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowActionService flowActionService = ServiceContainer.Instance.Container.Resolve<IFlowActionService>();
                bool flag = flowActionService.AddFlowAction(flowActionEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "添加审稿操作失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "添加审稿操作时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 修改审稿操作
        /// </summary>
        /// <param name="flowActionEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult EditFlowAction(FlowActionEntity flowActionEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowActionService flowActionService = ServiceContainer.Instance.Container.Resolve<IFlowActionService>();
                bool flag = flowActionService.UpdateFlowAction(flowActionEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "修改审稿操作失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "修改审稿操作时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 删除审稿操作
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult DelFlowAction(FlowActionEntity flowActionEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowActionService flowActionService = ServiceContainer.Instance.Container.Resolve<IFlowActionService>();
                bool flag = flowActionService.DeleteFlowAction(flowActionEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "删除审稿操作失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "删除审稿操作时出现异常：" + ex.Message;
            }
            return result;
        }

        # endregion

        # region 审稿环节权限配置

        /// <summary>
        /// 获取审稿环节作者权限设置
        /// </summary>
        /// <param name="queryFlowAuthAuthor"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public List<FlowAuthAuthorEntity> GetFlowAuthAuthorList(FlowAuthAuthorQuery queryFlowAuthAuthor)
        {
            try
            {
                IFlowAuthAuthorService flowAuthAuthorService = ServiceContainer.Instance.Container.Resolve<IFlowAuthAuthorService>();
                List<FlowAuthAuthorEntity> listFlowAuthAuthor = flowAuthAuthorService.GetFlowAuthAuthorList(queryFlowAuthAuthor);
                return listFlowAuthAuthor;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取审稿流程作者权限出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 设置审稿环节作者权限
        /// </summary>
        /// <param name="flowAuthAuthorEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult SetFlowAuthAuthor(List<FlowAuthAuthorEntity> flowAuthAuthorEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowAuthAuthorService flowAuthAuthorService = ServiceContainer.Instance.Container.Resolve<IFlowAuthAuthorService>();
                bool flag = flowAuthAuthorService.SaveFlowAuthAuthor(flowAuthAuthorEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "设置审稿流程环节作者权限失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "设置审稿流程环节作者权限出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 删除审稿环节作者权限
        /// </summary>
        /// <param name="flowAuthAuthorEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult DeleteFlowAuthAuthor(List<FlowAuthAuthorEntity> flowAuthAuthorEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowAuthAuthorService flowAuthAuthorService = ServiceContainer.Instance.Container.Resolve<IFlowAuthAuthorService>();
                bool flag = flowAuthAuthorService.BatchDeleteAuthAuthor(flowAuthAuthorEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "删除审稿流程环节作者权限失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "删除审稿流程环节作者权限出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取审稿环节角色权限设置
        /// </summary>
        /// <param name="queryFlowAuthRole"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public List<FlowAuthRoleEntity> GetFlowAuthRoleList(FlowAuthRoleQuery queryFlowAuthRole)
        {
            try
            {
                IFlowAuthRoleService flowAuthRoleService = ServiceContainer.Instance.Container.Resolve<IFlowAuthRoleService>();
                List<FlowAuthRoleEntity> listFlowAuthRole = flowAuthRoleService.GetFlowAuthRoleList(queryFlowAuthRole);
                return listFlowAuthRole;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取审稿流程角色权限出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 设置审稿环节角色权限
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult DeleteFlowAuthRole(List<FlowAuthRoleEntity> flowAuthRoleEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowAuthRoleService flowAuthRoleService = ServiceContainer.Instance.Container.Resolve<IFlowAuthRoleService>();
                bool flag = flowAuthRoleService.BatchDeleteAuthRole(flowAuthRoleEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "删除审稿流程环节角色权限失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "删除审稿流程环节角色权限出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 删除审稿环节角色权限
        /// </summary>
        /// <param name="flowSetEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult SetFlowAuthRole(List<FlowAuthRoleEntity> flowAuthRoleEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowAuthRoleService flowAuthRoleService = ServiceContainer.Instance.Container.Resolve<IFlowAuthRoleService>();
                bool flag = flowAuthRoleService.SaveFlowAuthRole(flowAuthRoleEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "设置审稿流程环节角色权限失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "设置审稿流程环节角色权限出现异常：" + ex.Message;
            }
            return result;
        }

        # endregion

        # region 审稿环节条件设置

        # endregion

        # region 流程流转

        /// <summary>
        /// 得到稿件的处理人
        /// </summary>
        /// <param name="cirEntity">稿件ID(CID)、稿件状态(EnumCStatus)、编辑部ID(JournalID)</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public AuthorInfoEntity GetContributionProcesser(CirculationEntity cirEntity)
        {
            try
            {
                IFlowCirculationService flowCirculationService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                AuthorInfoEntity authorEntity = flowCirculationService.GetContributionProcesser(cirEntity);
                return authorEntity;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("得到稿件的处理人出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取稿件列表
        /// </summary>
        /// <param name="queryFlowSet"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public Pager<FlowContribution> GetFlowContributionList(CirculationEntity cirEntity)
        {
            try
            {
                IFlowCirculationService flowCirculationService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                Pager<FlowContribution> listCortibutionAction = flowCirculationService.GetFlowContributionList(cirEntity);
                if (listCortibutionAction != null)
                {
                    IDictValueService dictService = ServiceContainer.Instance.Container.Resolve<IDictValueService>();
                    var dict = dictService.GetDictValueDcit(cirEntity.JournalID, EnumDictKey.SubjectCat.ToString());
                    foreach (FlowContribution item in listCortibutionAction.ItemList)
                    {
                        item.SubjectCatName = dict.GetValue(item.SubjectCat, "");
                    }
                }
                return listCortibutionAction;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取当前步骤的稿件列表出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取作者最新稿件状态稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public Pager<FlowContribution> GetAuthorContributionList(CirculationEntity cirEntity)
        {
            try
            {
                IFlowCirculationService flowCirculationService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                Pager<FlowContribution> listCortibutionAction = flowCirculationService.GetAuthorContributionList(cirEntity);
                return listCortibutionAction;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取作者最新稿件状态稿件列表出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取专家待审、已审稿件列表
        /// </summary>
        /// <param name="cirEntity">需要传值:JournalID，CurAuthorID，Status</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public Pager<FlowContribution> GetExpertContributionList(CirculationEntity cirEntity)
        {
            try
            {
                IFlowCirculationService flowCirculationService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                Pager<FlowContribution> listCortibutionAction = flowCirculationService.GetExpertContributionList(cirEntity);
                return listCortibutionAction;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取专家待审、已审稿件列表出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


        /// <summary>
        /// 获取同步状态稿件列表，例如：专家拒审，作者退修、已发校样、录用、退稿稿件列表
        /// </summary>
        /// <param name="cirEntity">需要传值:JournalID，CurAuthorID，EnumCStatus</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public Pager<FlowContribution> GetSynchroStatusContributionList(CirculationEntity cirEntity)
        {
            try
            {
                IFlowCirculationService flowCirculationService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                Pager<FlowContribution> listCortibutionAction = flowCirculationService.GetSynchroStatusContributionList(cirEntity);
                return listCortibutionAction;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取同步状态稿件列表出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 得到下一步骤信息
        /// </summary>
        /// <param name="ciEntity">流转信息</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public FlowStep GetNextFlowStep(CirculationEntity cirEntity)
        {
            try
            {
                IFlowCirculationService flowCirculationService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                FlowStep stepEntity = flowCirculationService.GetNextFlowStep(cirEntity);
                return stepEntity;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("得到下一步骤信息出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 提交审稿单
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult SubmitAuditBill(AuditBillEntity auditBillEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowCirculationService flowCirService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                bool flag = flowCirService.SubmitAuditBill(auditBillEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "提交审稿单失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "提交审稿单时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取处理日志信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public FlowLogInfoEntity GetFlowLogEntity(FlowLogInfoQuery query)
        {
            try
            {
                IFlowCirculationService flowCirculationService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                FlowLogInfoEntity flowlogEntity = flowCirculationService.GetFlowLogEntity(query);
                return flowlogEntity;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取指定的处理日志信息出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取稿件的处理日志
        /// </summary>
        /// <param name="cirEntity">稿件ID,JournalID,分组</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<FlowLogInfoEntity> GetFlowLog(CirculationEntity cirEntity)
        {
            try
            {
                IFlowCirculationService flowCirculationService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                IList<FlowLogInfoEntity> listFlowLog = flowCirculationService.GetFlowLog(cirEntity);
                return listFlowLog;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取稿件的流程处理日志出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 更新日志的查看状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult UpdateFlowLogIsView(FlowLogQuery flowLogQuery)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowCirculationService flowService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                bool flag = flowService.UpdateFlowLogIsView(flowLogQuery);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "更新日志的查看状态失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "更新日志的查看状态出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 更新日志的下载状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult UpdateFlowLogIsDown(FlowLogQuery flowLogQuery)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowCirculationService flowService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                bool flag = flowService.UpdateFlowLogIsDown(flowLogQuery);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "更新日志的下载状态失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "更新日志的下载状态出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public string GetFlowLogAttachment(FlowLogQuery flowLogQuery)
        {
            try
            {
                IFlowCirculationService flowCirculationService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                string CPath = flowCirculationService.GetFlowLogAttachment(flowLogQuery);
                return CPath;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取流程日志附件出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IDictionary<string, string> GetFlowLogAllAttachment(FlowLogQuery flowLogQuery)
        {
            try
            {
                IFlowCirculationService flowCirculationService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                IDictionary<string, string> dictPath = flowCirculationService.GetFlowLogAllAttachment(flowLogQuery);
                return dictPath;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取流程日志附件出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 专家拒审
        /// </summary>
        /// <param name="cirEntity">CID,AuthorID,CNumber,JournalID,EnumCStatus</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public bool ExpertDeledit(CirculationEntity cirEntity)
        {
            try
            {
                IFlowCirculationService flowCirculationService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                bool result = flowCirculationService.ExpertDeledit(cirEntity);
                return result;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("专家拒审出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 处理在入款时改变稿件状态
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult DealFinaceInAccount(CirculationEntity cirEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowCirculationService flowCirService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                bool flag = flowCirService.DealFinaceInAccount(cirEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "入款失败";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "提交审稿单时出现异常：" + ex.Message;
            }

            return result;
        }

        # endregion

        #region 审稿单相关
        /// <summary>
        /// 获取审稿单项分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public Pager<ReviewBillEntity> GetReviewBillPageList(ReviewBillQuery query)
        {
            IReviewBillService service = ServiceContainer.Instance.Container.Resolve<IReviewBillService>();
            Pager<ReviewBillEntity> pager = service.GetReviewBillPageList(query);
            return pager;
        }

        /// <summary>
        /// 获取审稿单项列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<ReviewBillEntity> GetReviewBillList(ReviewBillQuery query)
        {
            IReviewBillService service = ServiceContainer.Instance.Container.Resolve<IReviewBillService>();
            IList<ReviewBillEntity> list = service.GetReviewBillList(query);
            return list;
        }

        /// <summary>
        /// 获取审稿单项实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ReviewBillEntity GetReviewBilllModel(ReviewBillQuery query)
        {
            IReviewBillService service = ServiceContainer.Instance.Container.Resolve<IReviewBillService>();
            ReviewBillEntity model = service.GetReviewBill(query.ItemID);           
            return model;
        }

        /// <summary>
        /// 保存审稿单项
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult SaveReviewBill(ReviewBillEntity model)
        {
            IReviewBillService service = ServiceContainer.Instance.Container.Resolve<IReviewBillService>();
            return service.SaveReviewBill(model);
        }

        /// <summary>
        /// 删除审稿单项
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult DelReviewBill(ReviewBillQuery query)
        {
            IReviewBillService service = ServiceContainer.Instance.Container.Resolve<IReviewBillService>();
            return service.DelReviewBill(query.JournalID, query.ItemID);
        }

        /// <summary>
        /// 审稿单项是否已经使用
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public bool ReviewBillIsEnabled(ReviewBillQuery query)
        {
            IReviewBillService service = ServiceContainer.Instance.Container.Resolve<IReviewBillService>();
            return service.ReviewBillIsEnabled(query.JournalID, query.ItemID);
        }

        /// <summary>
        /// 保存审稿单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult SaveReviewBillContent(ReviewBillContentQuery query)
        {
            IReviewBillContentService service = ServiceContainer.Instance.Container.Resolve<IReviewBillContentService>();
            return service.SaveReviewBillContent(query);
        }

        /// <summary>
        /// 获取审稿单项列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<ReviewBillContentEntity> GetReviewBillContentList(ReviewBillContentQuery query)
        {
            IReviewBillContentService service = ServiceContainer.Instance.Container.Resolve<IReviewBillContentService>();
            IList<ReviewBillContentEntity> list = service.GetReviewBillContentList(query);
            return list;
        }

        /// <summary>
        /// 获取审稿单初始化项
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<ReviewBillContentEntity> GetReviewBillContentListByCID(ReviewBillContentQuery query)
        {
            IReviewBillContentService service = ServiceContainer.Instance.Container.Resolve<IReviewBillContentService>();
            IList<ReviewBillContentEntity> list = service.GetReviewBillContentListByCID(query);
            return list;
        }
        #endregion

        # region 稿件多状态合并

        /// <summary>
        /// 查看该稿件是否存在多个状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public long JudgeIsMoreStatus(FlowLogInfoQuery flowLogQuery)
        {
            IFlowCirculationService service = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
            return service.JudgeIsMoreStatus(flowLogQuery);
        }

		/// <summary>
        /// 合并多状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult MergeMoreStatus(FlowLogInfoQuery query)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowCirculationService flowCirService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                bool flag = flowCirService.MergeMoreStatus(query);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "合并稿件多状态失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "合并稿件多状态出现异常：" + ex.Message;
            }
            return result;
        }

		/// <summary>
        /// 根据当前所处环节获取稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public List<FlowContribution> GetContributionMoreStatusList(FlowLogInfoQuery query)
        {
            IFlowCirculationService service = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
            List<FlowContribution> list = service.GetContributionMoreStatusList(query);
            return list;
        }

        # endregion

        /// <summary>
        /// 继续送交s
        /// </summary>
        /// <param name="sendEntity"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult ContinuSend(ContinuSendEntity sendEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IFlowCirculationService flowCirService = ServiceContainer.Instance.Container.Resolve<IFlowCirculationService>();
                bool flag = flowCirService.ContinuSend(sendEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "继续送交失败，请确认信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "继续送交出现异常：" + ex.Message;
            }
            return result;
        }
    }
}
