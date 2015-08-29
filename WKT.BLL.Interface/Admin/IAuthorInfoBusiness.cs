using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface IAuthorInfoBusiness
    {
        #region 获取一个实体对象

        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="authorID">作者信息</param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        AuthorInfoEntity GetAuthorInfo(Int64 authorID);

        /// <summary>
        /// 获取作者信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        AuthorInfoEntity GetAuthorInfo(AuthorInfoQuery query);

        /// <summary>
        /// 获取编辑部成员信息
        /// </summary>
        /// <param name="authorID"></param>
        /// <returns></returns>
        AuthorInfoEntity GetMemberInfo(AuthorInfoQuery authorQuery);

        #endregion

        #region 根据条件获取所有实体对象

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<AuthorInfoEntity></returns>
        List<AuthorInfoEntity> GetAuthorInfoList();

        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="authorInfoQuery">AuthorInfoQuery查询实体对象</param>
        /// <returns>List<AuthorInfoEntity></returns>
        List<AuthorInfoEntity> GetAuthorInfoList(AuthorInfoQuery authorInfoQuery);

        /// <summary>
        /// 根据角色获取作者列表
        /// </summary>
        /// <param name="authorInfoQuery">AuthorInfoQuery查询实体对象</param>
        /// <returns>List<AuthorInfoEntity></returns>
        List<AuthorInfoEntity> GetAuthorInfoListByRole(AuthorInfoQuery authorInfoQuery);

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<AuthorInfoEntity></returns>
        Pager<AuthorInfoEntity> GetAuthorInfoPageList(CommonQuery query);


        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<AuthorInfoEntity></returns>
        Pager<AuthorInfoEntity> GetAuthorInfoPageList(QueryBase query);


        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="authorInfoQuery">AuthorInfoQuery查询实体对象</param>
        /// <returns>Pager<AuthorInfoEntity></returns>
        Pager<AuthorInfoEntity> GetAuthorInfoPageList(AuthorInfoQuery authorInfoQuery);

        /// <summary>
        /// 获取编辑部成员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<AuthorInfoEntity> GetMemberInfoPageList(AuthorInfoQuery query);

        /// <summary>
        /// 获取专家列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<AuthorInfoEntity> GetExpertPageList(AuthorInfoQuery query);

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="authorInfo">AuthorInfoEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddAuthorInfo(AuthorInfoEntity authorInfo);

        #endregion

        #region 更新一个持久化对象

        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="authorInfo">AuthorInfoEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateAuthorInfo(AuthorInfoEntity authorInfo);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="authorItem"></param>
        /// <returns></returns>
        bool UpdatePwd(AuthorInfoEntity authorItem);

        /// <summary>
        /// 修改登录信息
        /// </summary>
        /// <param name="authorItem"></param>
        bool UpdateLoginInfo(AuthorInfoEntity authorItem);

        /// <summary>
        /// 修改成员信息
        /// </summary>
        /// <param name="authorInfoEntity"></param>
        /// <returns></returns>
        bool UpdateMembaerInfo(AuthorInfoEntity authorInfoEntity);

        #endregion

        #region 从存储媒介中删除对象

        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="authorID">作者信息</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteAuthorInfo(Int64 authorID);

        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="authorInfo">AuthorInfoEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteAuthorInfo(AuthorInfoEntity authorInfo);

        #region 批量删除

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="authorID">作者信息</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteAuthorInfo(AuthorInfoQuery authorQuery);

        #endregion

        #endregion

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
        /// 获取人员数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IDictionary<Int64, String> GetAuthorDict(AuthorInfoQuery query);

    }
}






