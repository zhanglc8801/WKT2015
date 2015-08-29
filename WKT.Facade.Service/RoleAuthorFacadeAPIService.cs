using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Facade.Service.Interface;
using WKT.Model;
using WKT.Common.Utils;

namespace WKT.Facade.Service
{
    class RoleAuthorFacadeAPIService : ServiceBase, IRoleAuthorFacadeService
    {
        #region 获取一个实体对象
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns></returns>
        public RoleAuthorEntity GetRoleAuthor(long mapID)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            RoleAuthorQuery roleAuthorQuery = new RoleAuthorQuery();
            roleAuthorQuery.MapID = mapID;
            RoleAuthorEntity roleAuthorEntity = clientHelper.PostAuth<RoleAuthorEntity, RoleAuthorQuery>(GetAPIUrl(APIConstant.GETROLEAUTHORLIST), roleAuthorQuery);
            return roleAuthorEntity;
        } 
        #endregion

        #region 根据条件获取所有实体对象
        /// <summary>
        /// 根据条件获取所有实体对象
        /// </summary>
        /// <returns></returns>
        public IList<RoleAuthorEntity> GetRoleAuthorList()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据条件获取所有实体对象
        /// </summary>
        /// <param name="roleAuthorQuery"></param>
        /// <returns></returns>
        public IList<RoleAuthorEntity> GetRoleAuthorList(RoleAuthorQuery roleAuthorQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<RoleAuthorEntity> roleAuthorList = clientHelper.PostAuth<IList<RoleAuthorEntity>, RoleAuthorQuery>(GetAPIUrl(APIConstant.GETROLEAUTHORLIST), roleAuthorQuery);
            return roleAuthorList;
        }
        /// <summary>
        /// 根据条件获取所有实体对象(包含角色名称)
        /// </summary>
        /// <param name="roleAuthorQuery"></param>
        /// <returns></returns>
        public IList<RoleAuthorEntity> GetRoleAuthorDetailList(RoleAuthorQuery roleAuthorQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<RoleAuthorEntity> roleAuthorList = clientHelper.PostAuth<IList<RoleAuthorEntity>, RoleAuthorQuery>(GetAPIUrl(APIConstant.GETROLEAUTHORDETAILLIST), roleAuthorQuery);
            return roleAuthorList;
        } 
        #endregion

        #region 根据查询条件分页获取对象
        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(CommonQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<RoleAuthorEntity> RoleAuthorPageList = clientHelper.PostAuth<Pager<RoleAuthorEntity>, CommonQuery>(GetAPIUrl(APIConstant.GETROLEAUTHORPAGELIST), query);
            return RoleAuthorPageList;
        }

        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(QueryBase query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<RoleAuthorEntity> RoleAuthorPageList = clientHelper.PostAuth<Pager<RoleAuthorEntity>, QueryBase>(GetAPIUrl(APIConstant.GETROLEAUTHORPAGELIST), query);
            return RoleAuthorPageList;
        }

        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(RoleAuthorQuery roleAuthorQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<RoleAuthorEntity> RoleAuthorPageList = clientHelper.PostAuth<Pager<RoleAuthorEntity>, RoleAuthorQuery>(GetAPIUrl(APIConstant.GETROLEAUTHORPAGELIST), roleAuthorQuery);
            return RoleAuthorPageList;
        } 
        #endregion

        /// <summary>
        /// 持久化一个新对象（保存新对象到存储媒介中）
        /// </summary>
        /// <param name="roleAuthor"></param>
        /// <returns></returns>
        public ExecResult AddRoleAuthor(RoleAuthorEntity roleAuthor)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, RoleAuthorEntity>(GetAPIUrl(APIConstant.ADDROLEAUTHOR), roleAuthor);
            return execResult;
        }
        /// <summary>
        /// 更新一个持久化对象
        /// </summary>
        /// <param name="roleAuthor"></param>
        /// <returns></returns>
        public ExecResult UpdateRoleAuthor(RoleAuthorEntity roleAuthor)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, RoleAuthorEntity>(GetAPIUrl(APIConstant.UPDATEROLEAUTHOR), roleAuthor);
            return execResult;
        }
        /// <summary>
        /// 从存储媒介中删除对象
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns></returns>
        public ExecResult DeleteRoleAuthor(long mapID)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            RoleAuthorEntity roleAuthor = new RoleAuthorEntity();
            roleAuthor.MapID = mapID;
            ExecResult execResult = clientHelper.PostAuth<ExecResult, RoleAuthorEntity>(GetAPIUrl(APIConstant.DELETEROLEAUTHOR), roleAuthor);
            return execResult;
        }
        /// <summary>
        /// 从存储媒介中删除对象
        /// </summary>
        /// <param name="roleAuthor"></param>
        /// <returns></returns>
        public ExecResult DeleteRoleAuthor(RoleAuthorEntity roleAuthor)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, RoleAuthorEntity>(GetAPIUrl(APIConstant.DELETEROLEAUTHOR), roleAuthor);
            return execResult;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns></returns>
        public ExecResult BatchDeleteRoleAuthor(long[] mapID)
        {
            throw new NotImplementedException();
        }





        
    }
}
