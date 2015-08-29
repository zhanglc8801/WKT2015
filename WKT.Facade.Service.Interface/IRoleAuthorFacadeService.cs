using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Model;

namespace WKT.Facade.Service.Interface
{
    public interface IRoleAuthorFacadeService
    {
        #region 获取一个实体对象

        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        RoleAuthorEntity GetRoleAuthor(Int64 mapID);

        #endregion

        #region 根据条件获取所有实体对象

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<RoleAuthorEntity></returns>
        IList<RoleAuthorEntity> GetRoleAuthorList();

        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="roleAuthorQuery">RoleAuthorQuery查询实体对象</param>
        /// <returns>List<RoleAuthorEntity></returns>
        IList<RoleAuthorEntity> GetRoleAuthorList(RoleAuthorQuery roleAuthorQuery);

        /// <summary>
        /// 获取所有符合查询条件的数据(作者登录名、姓名、角色名)
        /// </summary>
        /// <param name="roleAuthorQuery"></param>
        /// <returns></returns>
        IList<RoleAuthorEntity> GetRoleAuthorDetailList(RoleAuthorQuery roleAuthorQuery);

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<RoleAuthorEntity></returns>
        Pager<RoleAuthorEntity> GetRoleAuthorPageList(CommonQuery query);


        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<RoleAuthorEntity></returns>
        Pager<RoleAuthorEntity> GetRoleAuthorPageList(QueryBase query);

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="roleAuthorQuery">RoleAuthorQuery查询实体对象</param>
        /// <returns>Pager<RoleAuthorEntity></returns>
        Pager<RoleAuthorEntity> GetRoleAuthorPageList(RoleAuthorQuery roleAuthorQuery);

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="roleAuthor">RoleAuthorEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        ExecResult AddRoleAuthor(RoleAuthorEntity roleAuthor);

        #endregion

        #region 更新一个持久化对象

        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="roleAuthor">RoleAuthorEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        ExecResult UpdateRoleAuthor(RoleAuthorEntity roleAuthor);

        #endregion

        #region 从存储媒介中删除对象

        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        ExecResult DeleteRoleAuthor(Int64 mapID);

        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="roleAuthor">RoleAuthorEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        ExecResult DeleteRoleAuthor(RoleAuthorEntity roleAuthor);

        #region 批量删除

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        ExecResult BatchDeleteRoleAuthor(Int64[] mapID);

        #endregion

        #endregion 



    }
}
