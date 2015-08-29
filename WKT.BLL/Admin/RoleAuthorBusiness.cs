using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class RoleAuthorBusiness : IRoleAuthorBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public RoleAuthorEntity GetRoleAuthor(Int64 mapID)
        {
           return RoleAuthorDataAccess.Instance.GetRoleAuthor( mapID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<RoleAuthorEntity></returns>
        public List<RoleAuthorEntity> GetRoleAuthorList()
        {
            return RoleAuthorDataAccess.Instance.GetRoleAuthorList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="roleAuthorQuery">RoleAuthorQuery查询实体对象</param>
        /// <returns>List<RoleAuthorEntity></returns>
        public List<RoleAuthorEntity> GetRoleAuthorList(RoleAuthorQuery roleAuthorQuery)
        {
            return RoleAuthorDataAccess.Instance.GetRoleAuthorList(roleAuthorQuery);
        }

        /// <summary>
        /// 获取所有符合查询条件的数据(包含角色名称)
        /// </summary>
        /// <param name="roleAuthorQuery"></param>
        /// <returns></returns>
        public List<RoleAuthorEntity> GetRoleAuthorDetailList(RoleAuthorQuery roleAuthorQuery)
        {
            return RoleAuthorDataAccess.Instance.GetRoleAuthorDetailList(roleAuthorQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<RoleAuthorEntity></returns>
        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(CommonQuery query)
        {
            return RoleAuthorDataAccess.Instance.GetRoleAuthorPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<RoleAuthorEntity></returns>
        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(QueryBase query)
        {
            return RoleAuthorDataAccess.Instance.GetRoleAuthorPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="roleAuthorQuery">RoleAuthorQuery查询实体对象</param>
        /// <returns>Pager<RoleAuthorEntity></returns>
        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(RoleAuthorQuery roleAuthorQuery)
        {
            return RoleAuthorDataAccess.Instance.GetRoleAuthorPageList(roleAuthorQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="roleAuthor">RoleAuthorEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddRoleAuthor(RoleAuthorEntity roleAuthor)
        {
            return RoleAuthorDataAccess.Instance.AddRoleAuthor(roleAuthor);
        }
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="roleAuthor">RoleAuthorEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateRoleAuthor(RoleAuthorEntity roleAuthor)
        {
            return RoleAuthorDataAccess.Instance.UpdateRoleAuthor(roleAuthor);
        }
        
        #endregion 

        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteRoleAuthor(Int64 mapID)
        {
            return RoleAuthorDataAccess.Instance.DeleteRoleAuthor( mapID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="roleAuthor">RoleAuthorEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteRoleAuthor(RoleAuthorEntity roleAuthor)
        {
            return RoleAuthorDataAccess.Instance.DeleteRoleAuthor(roleAuthor);
        }
        
        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteRoleAuthor(Int64[] mapID)
        {
            return RoleAuthorDataAccess.Instance.BatchDeleteRoleAuthor( mapID);
        }
        #endregion
        
        #endregion
    }
}
