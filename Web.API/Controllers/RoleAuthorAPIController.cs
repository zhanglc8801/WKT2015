using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using WKT.Model;
using WKT.Service.Interface;
using WKT.Service.Wrapper;
using WKT.Model.Enum;

namespace Web.API.Controllers
{
    public class RoleAuthorAPIController : ApiBaseController
    {
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns></returns>
        [Auth]
        [System.Web.Http.AcceptVerbs("POST")]
        public RoleAuthorEntity GetRoleAuthor(long mapID)
        {
            IRoleAuthorService roleAuthorService = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
            var RoleAuthorEntity = roleAuthorService.GetRoleAuthor(mapID);
            return RoleAuthorEntity;
        }
        /// <summary>
        /// 根据条件获取所有实体对象
        /// </summary>
        /// <param name="roleAuthorQuery"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<RoleAuthorEntity> GetRoleAuthorList(RoleAuthorQuery roleAuthorQuery)
        {
            IRoleAuthorService service = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
            IList<RoleAuthorEntity> list = service.GetRoleAuthorList(roleAuthorQuery);
            return list;
        }

        /// <summary>
        /// 根据条件获取所有实体对象(包含角色名称)
        /// </summary>
        /// <param name="roleAuthorQuery"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        public IList<RoleAuthorEntity> GetRoleAuthorDetailList(RoleAuthorQuery roleAuthorQuery)
        {
            IRoleAuthorService service = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
            IList<RoleAuthorEntity> list = service.GetRoleAuthorDetailList(roleAuthorQuery);
            return list;
        }

        #region 根据查询条件分页获取对象
        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(CommonQuery query)
        {
            IRoleAuthorService service = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
            Pager<RoleAuthorEntity> pager = service.GetRoleAuthorPageList(query);
            return pager;
        }

        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(QueryBase query)
        {
            IRoleAuthorService service = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
            Pager<RoleAuthorEntity> pager = service.GetRoleAuthorPageList(query);
            return pager;
        }

        public Pager<RoleAuthorEntity> GetRoleAuthorPageList(RoleAuthorQuery roleAuthorQuery)
        {
            IRoleAuthorService service = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
            Pager<RoleAuthorEntity> pager = service.GetRoleAuthorPageList(roleAuthorQuery);
            return pager;
        }
        #endregion

        /// <summary>
        /// 持久化一个新对象（保存新对象到存储媒介中）
        /// </summary>
        /// <param name="roleAuthor"></param>
        /// <returns></returns>
        public ExecResult AddRoleAuthor(RoleAuthorEntity roleAuthor)
        {
            ExecResult result = new ExecResult();
            try
            {
                IRoleAuthorService roleAuthorService = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
                bool flag = roleAuthorService.AddRoleAuthor(roleAuthor);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "操作失败，请确认登录信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "更新角色作者信息时出现异常：" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 更新一个持久化对象
        /// </summary>
        /// <param name="roleAuthor"></param>
        /// <returns></returns>
        public ExecResult UpdateRoleAuthor(RoleAuthorEntity roleAuthor)
        {
            ExecResult result = new ExecResult();
            try
            {
                IRoleAuthorService roleAuthorService = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
                bool flag = roleAuthorService.UpdateRoleAuthor(roleAuthor);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "操作失败";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "更新角色作者信息时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 从存储媒介中删除对象
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns></returns>
        public ExecResult DeleteRoleAuthor(long mapID)
        {
            IRoleAuthorService service = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
            ExecResult result = new ExecResult();
            try
            {
                IRoleAuthorService roleAuthorService = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
                bool flag = roleAuthorService.DeleteRoleAuthor(mapID);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "操作失败";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "删除角色作者信息时出现异常：" + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 从存储媒介中删除对象
        /// </summary>
        /// <param name="roleAuthor"></param>
        /// <returns></returns>
        public ExecResult DeleteRoleAuthor(RoleAuthorEntity roleAuthor)
        {
            IRoleAuthorService service = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
            ExecResult result = new ExecResult();
            try
            {
                IRoleAuthorService roleAuthorService = ServiceContainer.Instance.Container.Resolve<IRoleAuthorService>();
                bool flag = roleAuthorService.DeleteRoleAuthor(roleAuthor);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "操作失败";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "删除角色作者信息时出现异常：" + ex.Message;
            }
            return result;
        }

    }
}
