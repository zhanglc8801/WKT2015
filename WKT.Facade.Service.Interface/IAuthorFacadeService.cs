using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Model;

namespace WKT.Facade.Service.Interface
{
    public interface IAuthorFacadeService
    {
        # region 作者登录、忘记密码、注册、修改密码、记录登录信息

        /// <summary>
        /// 作者登录
        /// </summary>
        /// <param name="queryAuthor"></param>
        /// <returns></returns>
        AuthorInfoEntity AuthorLogin(AuthorInfoQuery queryAuthor);

        /// <summary>
        /// 作者忘记密码
        /// </summary>
        /// <param name="queryAuthor"></param>
        /// <returns></returns>
        ExecResult AuthorRetakePwd(AuthorInfoQuery queryAuthor);

        /// <summary>
        /// 作者注册
        /// </summary>
        /// <param name="authorEntity"></param>
        /// <returns></returns>
        ExecResult AuthorReg(AuthorInfoEntity authorEntity);

        /// <summary>
        /// 作者修改密码
        /// </summary>
        /// <param name="authorEntity"></param>
        /// <returns></returns>
        ExecResult EditPwd(AuthorInfoEntity authorEntity);

        /// <summary>
        /// 记录登录信息
        /// </summary>
        /// <param name="authorEntity"></param>
        /// <returns></returns>
        ExecResult RecordLoginInfo(AuthorInfoEntity authorEntity);

        # endregion

        # region 令牌操作

        /// <summary>
        /// 新增令牌
        /// </summary>
        /// <param name="tokenEntity"></param>
        /// <returns></returns>
        ExecResult InsertToken(TokenEntity tokenEntity);

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="tokenQuery"></param>
        /// <returns></returns>
        TokenEntity GetToken(TokenQuery tokenQuery);

        # endregion

        # region 获取编辑部成员列表

        /// <summary>
        /// 获取编辑部成员列表
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        Pager<AuthorInfoEntity> GetAuthorList(AuthorInfoQuery queryAuthor);

        /// <summary>
        /// 根据角色获取编辑部成员列表
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        IList<AuthorInfoEntity> GetAuthorListByRole(AuthorInfoQuery queryAuthor);

        /// <summary>
        /// 设置作者角色
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        ExecResult SetAurhorRole(RoleAuthorEntity roleAuthorEntity);

        /// <summary>
        /// 删除作者角色
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        ExecResult DelAurhorRole(RoleAuthorEntity roleAuthorEntity);

        /// <summary>
        /// 设置作者角色菜单例外
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        ExecResult SetAurhorMenuRightException(AuthorMenuRightExceptionEntity authorMenuRightException);

        /// <summary>
        /// 编辑编辑部成员信息
        /// </summary>
        /// <param name="authorQueryEntity"></param>
        /// <returns></returns>
        ExecResult EditMember(AuthorInfoEntity authorQueryEntity);

                /// <summary>
        /// 编辑作者成员信息
        /// </summary>
        /// <param name="authorQueryEntity"></param>
        /// <returns></returns>
        ExecResult EditAuthor(AuthorInfoEntity authorQueryEntity);
        /// <summary>
        /// 获取编辑部成员信息
        /// </summary>
        /// <param name="authorQueryEntity"></param>
        /// <returns></returns>
        AuthorInfoEntity GetMemberInfo(AuthorInfoQuery authorQueryEntity);

        /// <summary>
        /// 获取编辑部成员列表
        /// </summary>
        /// <param name="authorQueryEntity"></param>
        /// <returns></returns>
        Pager<AuthorInfoEntity> GetMemberInfoList(AuthorInfoQuery authorQueryEntity);

        /// <summary>
        /// 获取专家列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<AuthorInfoEntity> GetExpertPageList(AuthorInfoQuery query);

        # endregion

        # region 设置作者字段

        /// <summary>
        /// 获取作者字段设置
        /// </summary>
        /// <returns></returns>
        IList<FieldsSet> GetFieldsSet();

        /// <summary>
        /// 设置作者字段
        /// </summary>
        /// <returns></returns>
        ExecResult SetFields(List<FieldsSet> fieldsArray);

        # endregion

        # region 设置专家字段

        /// <summary>
        /// 获取专家字段设置
        /// </summary>
        /// <returns></returns>
        IList<FieldsSet> GetExpertFieldsSet();

        /// <summary>
        /// 设置专家字段
        /// </summary>
        /// <returns></returns>
        ExecResult SetExpertFields(List<FieldsSet> fieldsArray);

        # endregion

        # region 设置编辑字段

        /// <summary>
        /// 获取编辑字段设置
        /// </summary>
        /// <returns></returns>
        IList<FieldsSet> GetEditorFieldsSet();

        /// <summary>
        /// 设置编辑字段
        /// </summary>
        /// <returns></returns>
        ExecResult SetEditorFields(List<FieldsSet> fieldsArray);

        # endregion

        # region 作者统计

        /// <summary>
        /// 作者总数及投稿作者数量统计
        /// </summary>
        /// <returns></returns>
        IDictionary<String, Int32> GetAuthorContributeStat(QueryBase query);

        /// <summary>
        /// 获取作者按省份统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<AuthorStatEntity> GetAuthorProvinceStat(QueryBase query);

        /// <summary>
        /// 获取作者按学历统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<AuthorStatEntity> GetAuthorEducationStat(QueryBase query);

        /// <summary>
        /// 获取作者按专业统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<AuthorStatEntity> GetAuthorProfessionalStat(QueryBase query);

        /// <summary>
        /// 获取作者按职称统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<AuthorStatEntity> GetAuthorJobTitleStat(QueryBase query);

        /// <summary>
        /// 获取作者按性别统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<AuthorStatEntity> GetAuthorGenderStat(QueryBase query);

        # endregion

        /// <summary>
        /// 编辑、专家工作量统计
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<WorkloadEntity> GetWorkloadList(WorkloadQuery query);

        /// <summary>
        /// 编辑、专家处理稿件明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<StatDealContributionDetailEntity> GetDealContributionDetail(StatQuery query);

        /// <summary>
        /// 获取作者实体
        /// </summary>
        /// <param name="authorQueryEntity"></param>
        /// <returns></returns>
        AuthorInfoEntity GetAuthorInfo(AuthorInfoQuery query);
    }
}
