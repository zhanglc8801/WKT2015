using System;
using System.Collections.Generic;
using System.Text;
using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class AuthorInfoBusiness : IAuthorInfoBusiness
    {
        #region 获取一个实体对象

        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="authorID">作者信息</param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public AuthorInfoEntity GetAuthorInfo(Int64 authorID)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorInfo(authorID);
        }

        /// <summary>
        /// 获取作者信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public AuthorInfoEntity GetAuthorInfo(AuthorInfoQuery query)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorInfo(query);
        }

        /// <summary>
        /// 获取编辑部成员信息
        /// </summary>
        /// <param name="authorID"></param>
        /// <returns></returns>
        public AuthorInfoEntity GetMemberInfo(AuthorInfoQuery authorQuery)
        {
            return AuthorInfoDataAccess.Instance.GetMemberInfo(authorQuery);
        }

        #endregion

        #region 根据条件获取所有实体对象

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<AuthorInfoEntity></returns>
        public List<AuthorInfoEntity> GetAuthorInfoList()
        {
            return AuthorInfoDataAccess.Instance.GetAuthorInfoList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="authorInfoQuery">AuthorInfoQuery查询实体对象</param>
        /// <returns>List<AuthorInfoEntity></returns>
        public List<AuthorInfoEntity> GetAuthorInfoList(AuthorInfoQuery authorInfoQuery)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorInfoList(authorInfoQuery);
        }

        /// <summary>
        /// 根据角色获取作者列表
        /// </summary>
        /// <param name="authorInfoQuery">AuthorInfoQuery查询实体对象</param>
        /// <returns>List<AuthorInfoEntity></returns>
        public List<AuthorInfoEntity> GetAuthorInfoListByRole(AuthorInfoQuery authorInfoQuery)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorInfoListByRole(authorInfoQuery);
        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<AuthorInfoEntity></returns>
        public Pager<AuthorInfoEntity> GetAuthorInfoPageList(CommonQuery query)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorInfoPageList(query);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<AuthorInfoEntity></returns>
        public Pager<AuthorInfoEntity> GetAuthorInfoPageList(QueryBase query)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorInfoPageList(query);
        }

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="authorInfoQuery">AuthorInfoQuery查询实体对象</param>
        /// <returns>Pager<AuthorInfoEntity></returns>
        public Pager<AuthorInfoEntity> GetAuthorInfoPageList(AuthorInfoQuery authorInfoQuery)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorInfoPageList(authorInfoQuery);
        }

        /// <summary>
        /// 获取编辑部成员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<AuthorInfoEntity> GetMemberInfoPageList(AuthorInfoQuery query)
        {
            return AuthorInfoDataAccess.Instance.GetMemberInfoPageList(query);
        }

        /// <summary>
        /// 获取专家列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<AuthorInfoEntity> GetExpertPageList(AuthorInfoQuery query)
        {
            return AuthorInfoDataAccess.Instance.GetExpertPageList(query);
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="authorInfo">AuthorInfoEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddAuthorInfo(AuthorInfoEntity authorInfo)
        {
            return AuthorInfoDataAccess.Instance.AddAuthorInfo(authorInfo);
        }
        #endregion

        #region 更新一个持久化对象

        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="authorInfo">AuthorInfoEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateAuthorInfo(AuthorInfoEntity authorInfo)
        {
            return AuthorInfoDataAccess.Instance.UpdateAuthorInfo(authorInfo);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="authorItem"></param>
        /// <returns></returns>
        public bool UpdatePwd(AuthorInfoEntity authorItem)
        {
            return AuthorInfoDataAccess.Instance.UpdatePwd(authorItem);
        }

        /// <summary>
        /// 修改登录信息
        /// </summary>
        /// <param name="authorItem"></param>
        public bool UpdateLoginInfo(AuthorInfoEntity authorItem)
        {
            return AuthorInfoDataAccess.Instance.UpdateLoginInfo(authorItem);
        }

        /// <summary>
        /// 修改成员信息
        /// </summary>
        /// <param name="authorInfoEntity"></param>
        /// <returns></returns>
        public bool UpdateMembaerInfo(AuthorInfoEntity authorInfoEntity)
        {
            return AuthorInfoDataAccess.Instance.UpdateMembaerInfo(authorInfoEntity);
        }

        #endregion

        #region 从存储媒介中删除对象

        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="authorID">作者信息</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteAuthorInfo(Int64 authorID)
        {
            return AuthorInfoDataAccess.Instance.DeleteAuthorInfo(authorID);
        }

        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="authorInfo">AuthorInfoEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteAuthorInfo(AuthorInfoEntity authorInfo)
        {
            return AuthorInfoDataAccess.Instance.DeleteAuthorInfo(authorInfo);
        }

        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="authorID">作者信息</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteAuthorInfo(AuthorInfoQuery authorQuery)
        {
            return AuthorInfoDataAccess.Instance.BatchDeleteAuthorInfo(authorQuery);
        }
        #endregion

        #endregion

        # region 作者统计

        /// <summary>
        /// 作者总数及投稿作者数量统计
        /// </summary>
        /// <returns></returns>
        public IDictionary<String, Int32> GetAuthorContributeStat(QueryBase query)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorContributeStat(query);
        }

		/// <summary>
        /// 获取作者按省份统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorProvinceStat(QueryBase query)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorProvinceStat(query);
        }

		/// <summary>
        /// 获取作者按学历统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorEducationStat(QueryBase query)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorEducationStat(query);
        }

		/// <summary>
        /// 获取作者按专业统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorProfessionalStat(QueryBase query)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorProfessionalStat(query);
        }

		/// <summary>
        /// 获取作者按职称统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorJobTitleStat(QueryBase query)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorJobTitleStat(query);
        }

		/// <summary>
        /// 获取作者按性别统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorGenderStat(QueryBase query)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorGenderStat(query);
        }

        # endregion

        # region 编辑、专家工作量统计

        /// <summary>
        /// 编辑、专家工作量统计
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<WorkloadEntity> GetWorkloadList(WorkloadQuery query)
        {
            return AuthorInfoDataAccess.Instance.GetWorkloadList(query);
        }

        # endregion

        /// <summary>
        /// 编辑、专家处理稿件明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<StatDealContributionDetailEntity> GetDealContributionDetail(StatQuery query)
        {
            return AuthorInfoDataAccess.Instance.GetDealContributionDetail(query);
        }

        /// <summary>
        /// 获取人员数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<Int64, String> GetAuthorDict(AuthorInfoQuery query)
        {
            return AuthorInfoDataAccess.Instance.GetAuthorDict(query);
        }
    }
}
