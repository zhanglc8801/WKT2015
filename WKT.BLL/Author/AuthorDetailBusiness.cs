using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public partial class AuthorDetailBusiness : IAuthorDetailBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public AuthorDetailEntity GetAuthorDetail(Int64 pKID)
        {
           return AuthorDetailDataAccess.Instance.GetAuthorDetail( pKID);
        }
        
        /// <summary>
        /// 根据作者编号获取实体
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        public AuthorDetailEntity GetAuthorDetailModel(Int64 AuthorID)
        {
            return AuthorDetailDataAccess.Instance.GetAuthorDetailModel(AuthorID);
        }
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<AuthorDetailEntity></returns>
        public List<AuthorDetailEntity> GetAuthorDetailList()
        {
            return AuthorDetailDataAccess.Instance.GetAuthorDetailList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="authorDetailQuery">AuthorDetailQuery查询实体对象</param>
        /// <returns>List<AuthorDetailEntity></returns>
        public List<AuthorDetailEntity> GetAuthorDetailList(AuthorDetailQuery authorDetailQuery)
        {
            return AuthorDetailDataAccess.Instance.GetAuthorDetailList(authorDetailQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<AuthorDetailEntity></returns>
        public Pager<AuthorDetailEntity> GetAuthorDetailPageList(CommonQuery query)
        {
            return AuthorDetailDataAccess.Instance.GetAuthorDetailPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<AuthorDetailEntity></returns>
        public Pager<AuthorDetailEntity> GetAuthorDetailPageList(QueryBase query)
        {
            return AuthorDetailDataAccess.Instance.GetAuthorDetailPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="authorDetailQuery">AuthorDetailQuery查询实体对象</param>
        /// <returns>Pager<AuthorDetailEntity></returns>
        public Pager<AuthorDetailEntity> GetAuthorDetailPageList(AuthorDetailQuery authorDetailQuery)
        {
            return AuthorDetailDataAccess.Instance.GetAuthorDetailPageList(authorDetailQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="authorDetail">AuthorDetailEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddAuthorDetail(AuthorDetailEntity authorDetail)
        {
            return AuthorDetailDataAccess.Instance.AddAuthorDetail(authorDetail);
        }
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="authorDetail">AuthorDetailEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateAuthorDetail(AuthorDetailEntity authorDetail)
        {
            return AuthorDetailDataAccess.Instance.UpdateAuthorDetail(authorDetail);
        }
        
        #endregion 

        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteAuthorDetail(Int64 pKID)
        {
            return AuthorDetailDataAccess.Instance.DeleteAuthorDetail( pKID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="authorDetail">AuthorDetailEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteAuthorDetail(AuthorDetailEntity authorDetail)
        {
            return AuthorDetailDataAccess.Instance.DeleteAuthorDetail(authorDetail);
        }
        
        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteAuthorDetail(Int64[] pKID)
        {
            return AuthorDetailDataAccess.Instance.BatchDeleteAuthorDetail( pKID);
        }
        #endregion
        
        #endregion

        /// <summary>
        /// 保存作者信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveAuthor(AuthorDetailEntity model)
        {
            return AuthorDetailDataAccess.Instance.SaveAuthor(model);
        }

        /// <summary>
        /// 删除作者信息
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        public bool DelAuthorDetail(Int64[] AuthorID)
        {
            return AuthorDetailDataAccess.Instance.DelAuthorDetail(AuthorID);
        }

        /// <summary>
        /// 设置作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool SetAuthorExpert(AuthorDetailQuery query)
        {
            return AuthorDetailDataAccess.Instance.SetAuthorExpert(query);
        }

        /// <summary>
        /// 取消作者为专家
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool CancelAuthorExpert(AuthorDetailQuery query)
        {
            return AuthorDetailDataAccess.Instance.CancelAuthorExpert(query);
        }

        #region 专家分组配置
        /// <summary>
        /// 获取专家分组信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorDetailEntity> GetExpertGroupMapList(ExpertGroupMapEntity query)
        {
            return AuthorDetailDataAccess.Instance.GetExpertGroupMapList(query);
        }

        /// <summary>
        /// 保存专家分组信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ExpertGroupID"></param>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        public bool SaveExpertGroupMap(IList<ExpertGroupMapEntity> list, Int32 ExpertGroupID, Int64 JournalID)
        {
            return AuthorDetailDataAccess.Instance.SaveExpertGroupMap(list, ExpertGroupID, JournalID);
        }
        #endregion
    }
}
