using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;

namespace WKT.Service
{
    public partial class AuthorInfoService : IAuthorInfoService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IAuthorInfoBusiness authorInfoBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IAuthorInfoBusiness AuthorInfoBusProvider
        {
            get
            {
                if (authorInfoBusProvider == null)
                {
                    authorInfoBusProvider = new AuthorInfoBusiness();//ServiceBusContainer.Instance.Container.Resolve<IAuthorInfoBusiness>();
                }
                return authorInfoBusProvider;
            }
            set
            {
                authorInfoBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AuthorInfoService()
        {
        }

        #region 获取一个实体对象

        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="authorID">作者信息</param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public AuthorInfoEntity GetAuthorInfo(Int64 authorID)
        {
            return AuthorInfoBusProvider.GetAuthorInfo(authorID);
        }

        /// <summary>
        /// 获取作者信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public AuthorInfoEntity GetAuthorInfo(AuthorInfoQuery query)
        {
            return AuthorInfoBusProvider.GetAuthorInfo(query);
        }

        /// <summary>
        /// 获取编辑部成员信息
        /// </summary>
        /// <param name="authorID"></param>
        /// <returns></returns>
        public AuthorInfoEntity GetMemberInfo(AuthorInfoQuery authorQuery)
        {
            return AuthorInfoBusProvider.GetMemberInfo(authorQuery);
        }

        #endregion

        #region 根据条件获取所有实体对象

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<AuthorInfoEntity></returns>
        public List<AuthorInfoEntity> GetAuthorInfoList()
        {
            return AuthorInfoBusProvider.GetAuthorInfoList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="authorInfoQuery">AuthorInfoQuery查询实体对象</param>
        /// <returns>List<AuthorInfoEntity></returns>
        public List<AuthorInfoEntity> GetAuthorInfoList(AuthorInfoQuery authorInfoQuery)
        {
            try
            {
                return AuthorInfoBusProvider.GetAuthorInfoList(authorInfoQuery);
            }
            catch (Exception ex)
            {
                Log.LogProvider.Instance.Error("查询作者信息异常：" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 根据角色获取作者列表
        /// </summary>
        /// <param name="authorInfoQuery">AuthorInfoQuery查询实体对象</param>
        /// <returns>List<AuthorInfoEntity></returns>
        public List<AuthorInfoEntity> GetAuthorInfoListByRole(AuthorInfoQuery authorInfoQuery)
        {
            try
            {
                return AuthorInfoBusProvider.GetAuthorInfoListByRole(authorInfoQuery);
            }
            catch (Exception ex)
            {
                Log.LogProvider.Instance.Error("根据角色获取作者列表异常：" + ex.Message);
                throw ex;
            }
        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<AuthorInfoEntity></returns>
        public Pager<AuthorInfoEntity> GetAuthorInfoPageList(CommonQuery query)
        {
            return AuthorInfoBusProvider.GetAuthorInfoPageList(query);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<AuthorInfoEntity></returns>
        public Pager<AuthorInfoEntity> GetAuthorInfoPageList(QueryBase query)
        {
            return AuthorInfoBusProvider.GetAuthorInfoPageList(query);
        }

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="authorInfoQuery">AuthorInfoQuery查询实体对象</param>
        /// <returns>Pager<AuthorInfoEntity></returns>
        public Pager<AuthorInfoEntity> GetAuthorInfoPageList(AuthorInfoQuery authorInfoQuery)
        {
            return AuthorInfoBusProvider.GetAuthorInfoPageList(authorInfoQuery);
        }

        /// <summary>
        /// 获取编辑部成员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<AuthorInfoEntity> GetMemberInfoPageList(AuthorInfoQuery query)
        {
            return AuthorInfoBusProvider.GetMemberInfoPageList(query);
        }

        /// <summary>
        /// 获取专家列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<AuthorInfoEntity> GetExpertPageList(AuthorInfoQuery query)
        {
            return AuthorInfoBusProvider.GetExpertPageList(query);
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
            return AuthorInfoBusProvider.AddAuthorInfo(authorInfo);
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
            return AuthorInfoBusProvider.UpdateAuthorInfo(authorInfo);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="authorItem"></param>
        /// <returns></returns>
        public bool UpdatePwd(AuthorInfoEntity authorItem)
        {
            return AuthorInfoBusProvider.UpdatePwd(authorItem);
        }

        /// <summary>
        /// 修改登录信息
        /// </summary>
        /// <param name="authorItem"></param>
        public bool UpdateLoginInfo(AuthorInfoEntity authorItem)
        {
            return AuthorInfoBusProvider.UpdateLoginInfo(authorItem);
        }

        /// <summary>
        /// 修改成员信息
        /// </summary>
        /// <param name="authorInfoEntity"></param>
        /// <returns></returns>
        public bool UpdateMembaerInfo(AuthorInfoEntity authorInfoEntity)
        {
            return AuthorInfoBusProvider.UpdateMembaerInfo(authorInfoEntity);
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
            return AuthorInfoBusProvider.DeleteAuthorInfo(authorID);
        }

        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="authorInfo">AuthorInfoEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteAuthorInfo(AuthorInfoEntity authorInfo)
        {
            return AuthorInfoBusProvider.DeleteAuthorInfo(authorInfo);
        }

        #region 批量删除

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="authorID">作者信息</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteAuthorInfo(AuthorInfoQuery authorQuery)
        {
            return AuthorInfoBusProvider.BatchDeleteAuthorInfo(authorQuery);
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
            return AuthorInfoBusProvider.GetAuthorContributeStat(query);
        }

        /// <summary>
        /// 获取作者按省份统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorProvinceStat(QueryBase query)
        {
            return AuthorInfoBusProvider.GetAuthorProvinceStat(query);
        }

        /// <summary>
        /// 获取作者按学历统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorEducationStat(QueryBase query)
        {
            return AuthorInfoBusProvider.GetAuthorEducationStat(query);
        }

        /// <summary>
        /// 获取作者按专业统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorProfessionalStat(QueryBase query)
        {
            return AuthorInfoBusProvider.GetAuthorProfessionalStat(query);
        }

        /// <summary>
        /// 获取作者按职称统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorJobTitleStat(QueryBase query)
        {
            return AuthorInfoBusProvider.GetAuthorJobTitleStat(query);
        }

        /// <summary>
        /// 获取作者按性别统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorGenderStat(QueryBase query)
        {
            return AuthorInfoBusProvider.GetAuthorGenderStat(query);
        }

        # endregion

        /// <summary>
        /// 编辑、专家工作量统计
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<WorkloadEntity> GetWorkloadList(WorkloadQuery query)
        {
            return AuthorInfoBusProvider.GetWorkloadList(query);
        }

        /// <summary>
        /// 编辑、专家处理稿件明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<StatDealContributionDetailEntity> GetDealContributionDetail(StatQuery query)
        {
            return AuthorInfoBusProvider.GetDealContributionDetail(query);
        }

        /// <summary>
        /// 获取人员数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<Int64, String> GetAuthorDict(AuthorInfoQuery query)
        {
            return AuthorInfoBusProvider.GetAuthorDict(query);
        }
    }
}
