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
    /// <summary>
    /// 作者Api
    /// </summary>
    public class AuthorAPIController : ApiBaseController
    {
        # region 作者登录、注册、忘记密码、修改密码、更新登录信息

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public AuthorInfoEntity Login(AuthorInfoQuery authorQuery)
        {
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                IList<AuthorInfoEntity> listAuthor = authorService.GetAuthorInfoList(authorQuery);
                if (listAuthor != null && listAuthor.Count == 1)
                {
                    return listAuthor[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("作者登录出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed,ex.Message));
            }
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="email">注册邮箱</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult Reg(AuthorInfoEntity authorEntity)
        {
            ExecResult execResult = new ExecResult();
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();

                AuthorInfoQuery queryAuthor = new AuthorInfoQuery();
                queryAuthor.LoginName = authorEntity.LoginName;
                queryAuthor.JournalID = authorEntity.JournalID;

                IList<AuthorInfoEntity> list = authorService.GetAuthorInfoList(queryAuthor);
                if (list != null && list.Count > 0)
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "该登录邮箱已经存在";
                }
                else
                {

                    authorEntity.Pwd = WKT.Common.Security.MD5Handle.Encrypt(authorEntity.Pwd);
                    authorService.AddAuthorInfo(authorEntity);
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "注册成功";
                }
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "注册失败：" + ex.Message;
                LogProvider.Instance.Error("作者注册出现异常：" + ex.Message);
            }
            return execResult;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ExecResult EditPwd(AuthorInfoEntity authorEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                bool flag = authorService.UpdatePwd(authorEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "修改密码失败，请确认作者信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "修改登录密码时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 记录登录信息
        /// </summary>
        /// <param name="authorEntity"></param>
        /// <returns></returns>
        public ExecResult RecordLoginInfo(AuthorInfoEntity authorEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                bool flag = authorService.UpdateLoginInfo(authorEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "操作失败，请确认作者信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "记录作者登录信息时出现异常：" + ex.Message;
            }
            return result;
        }

        # endregion

        # region 令牌　作者忘记密码、重置密码

        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult InsertToken(TokenEntity tokenEntity)
        {
            ExecResult execResult = new ExecResult();
            try
            {
                ITokenService tokenService = ServiceContainer.Instance.Container.Resolve<ITokenService>();
                tokenService.AddToken(tokenEntity);
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "添加令牌成功";
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "添加令牌失败：" + ex.Message;
                LogProvider.Instance.Error("添加令牌出现异常：" + ex.Message);
            }
            return execResult;
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public TokenEntity GetToken(TokenQuery tokenQuery)
        {
            TokenEntity tokenEntity = null;
            try
            {
                ITokenService tokenService = ServiceContainer.Instance.Container.Resolve<ITokenService>();
                tokenEntity = tokenService.GetToken(tokenQuery);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取令牌出现异常：" + ex.Message);
            }
            return tokenEntity;
        }


        # endregion

        # region 编辑部成员信息获取及设置

        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public Pager<AuthorInfoEntity> GetAuthorInfoList(AuthorInfoQuery authorQuery)
        {
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                Pager<AuthorInfoEntity> listAuthor = authorService.GetAuthorInfoPageList(authorQuery);
                return listAuthor;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("作者登录出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<AuthorInfoEntity> GetAuthorInfoListByRole(AuthorInfoQuery authorQuery)
        {
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                IList<AuthorInfoEntity> listAuthor = authorService.GetAuthorInfoListByRole(authorQuery);
                return listAuthor;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("作者登录出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<RoleAuthorEntity> GetAuthorRoleList(RoleAuthorQuery authorQuery)
        {
            try
            {
                IRoleAuthorService roleAuthorService = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
                IList<RoleAuthorEntity> listAuthor = roleAuthorService.GetRoleAuthorList(authorQuery);
                return listAuthor;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("作者登录出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 设置编辑部成员角色
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult SetAurhoRole(RoleAuthorEntity roleAuthorEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                RoleAuthorQuery roleAuthorQuery = new RoleAuthorQuery();
                roleAuthorQuery.AuthorID = roleAuthorEntity.AuthorID;
                roleAuthorQuery.JournalID = roleAuthorEntity.JournalID;
                roleAuthorQuery.RoleID = roleAuthorEntity.RoleID;

                IRoleAuthorService roleAuthorService = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
                List<RoleAuthorEntity> checkIsExist = roleAuthorService.GetRoleAuthorList(roleAuthorQuery);
                if (checkIsExist != null && checkIsExist.Count > 0)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    bool flag = roleAuthorService.AddRoleAuthor(roleAuthorEntity);
                    if (flag)
                    {
                        result.result = EnumJsonResult.success.ToString();
                        result.msg = "成功";
                    }
                    else
                    {
                        result.result = EnumJsonResult.failure.ToString();
                        result.msg = "设置作者角色失败，请确认作者信息是否正确";
                    }
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "设置作者角色时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 设置编辑部成员角色
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult DelAurhoRole(RoleAuthorEntity roleAuthorEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IRoleAuthorService roleAuthorService = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
                bool flag = roleAuthorService.DeleteRoleAuthor(roleAuthorEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "删除作者角色失败，请确认作者信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "删除作者角色时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 设置编辑部成员角色
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Auth]
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult SetAuthorExceptionMenuRight(AuthorMenuRightExceptionEntity authorMenuRightException)
        {
            ExecResult result = new ExecResult();
            try
            {
                IRoleMenuService roleMenuService = ServiceContainer.Instance.Container.Resolve<IRoleMenuService>();
                bool flag = roleMenuService.SetAuthorExceptionMenuRight(authorMenuRightException);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "设置作者菜单权限例外失败，请确认设置信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "设置作者菜单权限例外失败：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取指定的编辑部成员信息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Auth]
        [System.Web.Http.AcceptVerbs("POST")]
        public AuthorInfoEntity GetMemberInfo(AuthorInfoQuery authorQueryEntity)
        {
            AuthorInfoEntity authorEntity = new AuthorInfoEntity();
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                authorEntity = authorService.GetMemberInfo(authorQueryEntity);
            }
            catch (Exception ex)
            {
                authorEntity = null;
                WKT.Log.LogProvider.Instance.Error("获取编辑部成员信息出现异常：" + ex.Message);
            }
            return authorEntity;
        }

        /// <summary>
        /// 获取编辑部成员列表
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Auth]
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<AuthorInfoEntity> GetMemberInfoList(AuthorInfoQuery authorQueryEntity)
        {
            Pager<AuthorInfoEntity> pagerMember = new Pager<AuthorInfoEntity>();
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                pagerMember = authorService.GetMemberInfoPageList(authorQueryEntity);
            }
            catch (Exception ex)
            {
                pagerMember = null;
                WKT.Log.LogProvider.Instance.Error("获取编辑部成员列表信息时出现异常：" + ex.Message);
            }
            return pagerMember;
        }

        /// <summary>
        /// 获取专家列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Auth]
        [System.Web.Http.AcceptVerbs("POST")]
        public Pager<AuthorInfoEntity> GetExpertPageList(AuthorInfoQuery query)
        {
            Pager<AuthorInfoEntity> pagerExpert = new Pager<AuthorInfoEntity>();
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                pagerExpert = authorService.GetExpertPageList(query);
            }
            catch (Exception ex)
            {
                pagerExpert = null;
                WKT.Log.LogProvider.Instance.Error("获取专家列表时出现异常：" + ex.Message);
            }
            return pagerExpert;
        }

        /// <summary>
        /// 修改编辑部成员
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Auth]
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult EditMember(AuthorInfoEntity authorEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                bool flag = false;
                if (authorEntity.GroupID == 2)
                {
                    flag = authorService.UpdateAuthorInfo(authorEntity);
                }
                else
                {
                  flag=authorService.UpdateMembaerInfo(authorEntity);
                }
               
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "修改成员" + authorEntity .AuthorID+ "信息失败，请确认成员信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "修改编辑部成员信息时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 修改作者信息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Auth]
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult EditAuthor(AuthorInfoEntity authorEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                bool flag = authorService.UpdateAuthorInfo(authorEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "修改作者信息信息失败，请确认作者信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "修改作者信息时出现异常：" + ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 删除编辑部成员
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Auth]
        [System.Web.Http.AcceptVerbs("POST")]
        public ExecResult DeleteMember(AuthorInfoQuery authorQuery)
        {
            ExecResult result = new ExecResult();
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                bool flag = authorService.BatchDeleteAuthorInfo(authorQuery);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "删除编辑部成员信息失败，请确认成员信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "删除编辑部成员信息时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取编辑部成员键值对
        /// </summary>
        /// <param name="authoerQuery"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IDictionary<long, string> GetMemberDict(AuthorInfoQuery authoerQuery)
        {
            IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
            return authorService.GetAuthorDict(authoerQuery);
        }

        # endregion

        # region 作者统计

        /// <summary>
        /// 作者总数及投稿作者数量统计
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IDictionary<String, Int32> GetAuthorContributeStat(QueryBase query)
        {
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                IDictionary<String, Int32> dictAuthorStat = authorService.GetAuthorContributeStat(query);
                return dictAuthorStat;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("作者总数及投稿作者数量统计出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取作者按省份统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<AuthorStatEntity> GetAuthorProvinceStat(QueryBase query)
        {
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                IList<AuthorStatEntity> listAuthorStat = authorService.GetAuthorProvinceStat(query);
                return listAuthorStat;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取作者按省份统计数据出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取作者按学历统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<AuthorStatEntity> GetAuthorEducationStat(QueryBase query)
        {
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                IList<AuthorStatEntity> listAuthorStat = authorService.GetAuthorEducationStat(query);

                IDictValueService service = ServiceContainer.Instance.Container.Resolve<IDictValueService>();
                IDictionary<int, string> dictValues = service.GetDictValueDcit(query.JournalID, EnumDictKey.Education.ToString());

                if (listAuthorStat != null)
                {
                    if (dictValues != null)
                    {
                        foreach (AuthorStatEntity item in listAuthorStat)
                        {
                            if (dictValues.ContainsKey(item.StatID))
                            {
                                item.StatItem = dictValues[item.StatID];
                            }
                            else
                            {
                                item.StatItem = "未知";
                            }
                        }
                    }
                }

                return listAuthorStat;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取作者按学历统计数据出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取作者按专业统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<AuthorStatEntity> GetAuthorProfessionalStat(QueryBase query)
        {
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                IList<AuthorStatEntity> listAuthorStat = authorService.GetAuthorProfessionalStat(query);
                return listAuthorStat;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取作者按专业统计数据出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取作者按职称统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<AuthorStatEntity> GetAuthorJobTitleStat(QueryBase query)
        {
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                IList<AuthorStatEntity> listAuthorStat = authorService.GetAuthorJobTitleStat(query);
                IDictValueService service = ServiceContainer.Instance.Container.Resolve<IDictValueService>();
                IDictionary<int, string> dictValues = service.GetDictValueDcit(query.JournalID, EnumDictKey.JobTitle.ToString());

                if (listAuthorStat != null)
                {
                    if (dictValues != null)
                    {
                        foreach (AuthorStatEntity item in listAuthorStat)
                        {
                            if (dictValues.ContainsKey(item.StatID))
                            {
                                item.StatItem = dictValues[item.StatID];
                            }
                            else
                            {
                                item.StatItem = "未知";
                            }
                        }
                    }
                }
                return listAuthorStat;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取作者按职称统计数据出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 获取作者按性别统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<AuthorStatEntity> GetAuthorGenderStat(QueryBase query)
        {
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                IList<AuthorStatEntity> listAuthorStat = authorService.GetAuthorGenderStat(query);
                return listAuthorStat;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取作者按性别统计数据出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        # endregion

        # region 编辑、专家工作量统计

        /// <summary>
        /// 编辑、专家工作量统计
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public IList<WorkloadEntity> GetWorkloadList(WorkloadQuery query)
        {
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                IList<WorkloadEntity> listAuthorStat = authorService.GetWorkloadList(query);
                return listAuthorStat;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("编辑、专家工作量统计出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        

        /// <summary>
        /// 编辑、专家处理稿件明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<StatDealContributionDetailEntity> GetDealContributionDetail(StatQuery query)
        {
            try
            {
                IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
                IList<StatDealContributionDetailEntity> listAuthorStat = authorService.GetDealContributionDetail(query);
                return listAuthorStat;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取编辑、专家处理稿件明细出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        # endregion

        [Auth]
        [System.Web.Http.AcceptVerbs("POST")]
        public AuthorInfoEntity GetAuthorInfo(AuthorInfoQuery query)
        {
            IAuthorInfoService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorInfoService>();
            var authorEntity = authorService.GetAuthorInfo(query);
            return authorEntity;
        }
    }
}