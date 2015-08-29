using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Common.Utils;
using WKT.Common.Security;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Config;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;

namespace HanFang360.InterfaceService.Controllers
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleController : BaseController
    {
        /// <summary>
        /// 角色管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 选择框
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleDialog()
        {
            return View();
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 角色修改
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(long? RoleID)
        {
            RoleInfoEntity roleEntity = new RoleInfoEntity();
            if (RoleID != null)
            {
                ISiteSystemFacadeService sysService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
                roleEntity = sysService.GetRoleEntity(new RoleInfoQuery { RoleID = RoleID.Value});
            }
            return View(roleEntity);
        }

        # region ajax

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult GetRoleListAjax()
        {
            RoleInfoQuery roleQuery = new RoleInfoQuery();
            roleQuery.JournalID = JournalID;
            roleQuery.GroupID = CurAuthor.GroupID;
            ISiteSystemFacadeService sysService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            IList<RoleInfoEntity> listRole = sysService.GetRoleList(roleQuery);
            

            # region 添加专家组
            RoleInfoEntity roleEntity = new RoleInfoEntity();
            roleEntity.RoleID = (long)EnumMemberGroup.Expert;
            roleEntity.RoleName = "专家组";
            roleEntity.GroupID = 3;
            listRole.Add(roleEntity);

            # endregion

            # region 添加英文专家组
            RoleInfoEntity roleEnEntity = new RoleInfoEntity();
            roleEnEntity.RoleID = (long)EnumMemberGroup.EnExpert;
            roleEnEntity.RoleName = "英文专家组";
            roleEnEntity.GroupID = 4;
            listRole.Add(roleEnEntity);

            # endregion

            var result = new { Rows = listRole };
            return Content(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult AddRole(RoleInfoEntity roleEntity)
        {
            roleEntity.JournalID = JournalID;
            roleEntity.GroupID = CurAuthor.GroupID;

            ISiteSystemFacadeService sysService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            ExecResult exeResult = sysService.AddRole(roleEntity);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult UpdateRole(RoleInfoEntity roleEntity)
        {
            roleEntity.JournalID = JournalID;
            roleEntity.GroupID = CurAuthor.GroupID;

            ISiteSystemFacadeService sysService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            ExecResult exeResult = sysService.UpdateRoleInfo(roleEntity);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        /// <summary>
        /// 删除角色列表
        /// </summary>
        /// <returns></returns>
        [AjaxRequest]
        public ActionResult DelRole(long[] IDAarry)
        {
            ExecResult exeResult = new ExecResult();
            if (IDAarry == null || IDAarry.Length == 0)
            {
                exeResult.msg = "请选择要删除的角色";
                exeResult.result = EnumJsonResult.failure.ToString();
                return Content(JsonConvert.SerializeObject(exeResult));
            }
            RoleInfoQuery roleQuery = new RoleInfoQuery();
            roleQuery.JournalID = JournalID;
            roleQuery.RoleIDList = IDAarry.ToList<long>();

            ISiteSystemFacadeService sysService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            exeResult = sysService.DelRole(roleQuery);
            return Content(JsonConvert.SerializeObject(exeResult));
        }

        # endregion
    }
}
