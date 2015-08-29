using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;

using WKT.Common.Extension;
using WKT.Log;
using WKT.Model;
using WKT.Service.Interface;
using WKT.Service.Wrapper;

namespace Web.SiteConsole.Controllers
{
    /// <summary>
    /// 账户管理
    /// </summary>
    public class AccountController : BaseController
    {
        # region 首页

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.TotalCount = 0;
            ISysAccountInfoService accountService = ServiceContainer.Instance.Container.Resolve<ISysAccountInfoService>();
            SysAccountInfoQuery queryEntity = new SysAccountInfoQuery();
            queryEntity.CurrentPage = 1;
            Pager<SysAccountInfoEntity> pagerAccountList = accountService.GetSysAccountInfoPageList(queryEntity);
            IList<SysAccountInfoEntity> listAccount = new List<SysAccountInfoEntity>();
            if (pagerAccountList != null)
            {
                listAccount = pagerAccountList.ItemList;
                ViewBag.TotalCount = pagerAccountList.TotalRecords;
            }

            return View(listAccount);
        }

        public ActionResult IndexAjax(SysAccountInfoQuery queryEntity)
        {
            if (!Request.IsAjaxRequest())
            {
                return Content("{\"result\":\"error\",\"msg\":\"非法访问\"}");
            }
            else
            {
                ISysAccountInfoService accountService = ServiceContainer.Instance.Container.Resolve<ISysAccountInfoService>();
                queryEntity.CurrentPage = queryEntity.CurrentPage + 1;
                WKT.Model.Pager<SysAccountInfoEntity> sysAccountList = accountService.GetSysAccountInfoPageList(queryEntity);
                if (sysAccountList != null)
                {
                    return Content("{\"result\":\"success\",\"msg\":\"成功\",\"data\":" + JsonConvert.SerializeObject(sysAccountList) + "}");
                }
                else
                {
                    return Content("{\"result\":\"error\",\"msg\":\"系统出现异常，请稍后再试\"}");
                }
            }
        }

        # endregion

        # region 添加、修改

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(int? AdminID)
        {
            SysAccountInfoEntity sysAccountEntity = null;
            ViewBag.IsEdit = false;
            if (AdminID != null)
            {
                ViewBag.IsEdit = true;
                ISysAccountInfoService accountService = ServiceContainer.Instance.Container.Resolve<ISysAccountInfoService>();
                sysAccountEntity = accountService.GetSysAccountInfo(AdminID.Value);
                sysAccountEntity.Pwd = WKT.Common.Security.DES.Decrypt(sysAccountEntity.Pwd);
            }
            return View(sysAccountEntity);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAjax(SysAccountInfoEntity accountEntity)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Content("{\"result\":\"failure\",\"msg\":\"非法请求\"}");
                }
                string msg = "";
                if (ModelState.IsValid)
                {
                    ISysAccountInfoService accountService = ServiceContainer.Instance.Container.Resolve<ISysAccountInfoService>();
                    SysAccountInfoQuery queryEntity = new SysAccountInfoQuery();
                    queryEntity.LoginName = WKT.Common.Security.SecurityUtils.SafeSqlString(accountEntity.LoginName);
                    IList<SysAccountInfoEntity> list = accountService.GetSysAccountInfoList(queryEntity);
                    if (list.Count > 0)
                    {
                        msg = "{\"result\":\"failure\",\"msg\":\"该登录名已经存在\"}";
                    }
                    else
                    {
                        accountEntity.Pwd = WKT.Common.Security.DES.Encrypt(accountEntity.Pwd);
                        bool flag = accountService.AddSysAccountInfo(accountEntity);
                        if (flag)
                        {
                            msg = "{\"result\":\"success\"}";
                        }
                        else
                        {
                            msg = "{\"result\":\"failure\",\"msg\":\"添加失败，请检查\"}";
                        }
                    }
                }
                else
                {
                    msg = "{\"result\":\"failure\",\"msg\":\"" + this.ExpendErrors() + "\"}";
                }
                return Content(msg);
            }
            catch(Exception ex)
            {
                LogProvider.Instance.Error("添加管理账户失败:" + ex.Message);
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="accountEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditAjax(SysAccountInfoEntity accountEntity)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Content("{\"result\":\"failure\",\"msg\":\"非法请求\"}");
                }
                string msg = "";
                if (ModelState.IsValid)
                {
                    ISysAccountInfoService accountService = ServiceContainer.Instance.Container.Resolve<ISysAccountInfoService>();
                    accountEntity.Pwd = WKT.Common.Security.DES.Encrypt(accountEntity.Pwd);
                    bool flag = accountService.UpdateSysAccountInfo(accountEntity);
                    if (flag)
                    {
                        msg = "{\"result\":\"success\"}";
                    }
                    else
                    {
                        msg = "{\"result\":\"failure\",\"msg\":\"修改失败，请检查\"}";
                    }
                }
                else
                {
                    msg = "{\"result\":\"failure\",\"msg\":\"" + this.ExpendErrors() + "\"}";
                }
                return Content(msg);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("修改管理账户失败:" + ex.Message);
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        # endregion

        # region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAjax(int[] IDAarry)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Content("{\"result\":\"failure\",\"msg\":\"非法请求\"}");
                }
                if (IDAarry == null || IDAarry.Length == 0)
                {
                    return Content("{\"result\":\"failure\",\"msg\":\"请选择要删除的账户\"}");
                }
                string msg = "";
                ISysAccountInfoService accountService = ServiceContainer.Instance.Container.Resolve<ISysAccountInfoService>();
                bool flag = accountService.BatchDeleteSysAccountInfo(IDAarry);
                if (flag)
                {
                    msg = "{\"result\":\"success\"}";
                }
                else
                {
                    msg = "{\"result\":\"failure\"，\"msg\":\"删除失败，请检查\"}";
                }
                return Content(msg);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("删除管理账户失败:" + ex.Message);
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        # endregion

        # region 修改密码

        public ActionResult EditPwd()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPwdAjax(string OldPwd,string NewPwd)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Content("{\"result\":\"failure\",\"msg\":\"非法请求\"}");
                }
                string msg = "";

                if (AccountEntity.Pwd != WKT.Common.Security.DES.Encrypt(OldPwd))
                {
                    msg = "{\"result\":\"failure\",\"msg\":\"请输入正确的旧密码\"}";
                }
                else
                {
                    ISysAccountInfoService accountService = ServiceContainer.Instance.Container.Resolve<ISysAccountInfoService>();                    
                    bool flag = accountService.UpdatePwd(AccountEntity.AdminID,WKT.Common.Security.DES.Encrypt(NewPwd));
                    if (flag)
                    {
                        msg = "{\"result\":\"success\"}";
                    }
                    else
                    {
                        msg = "{\"result\":\"failure\",\"msg\":\"修改密码失败，请检查\"}";
                    }
                }
                return Content(msg);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("修改密码失败:" + ex.Message);
                return Content("{\"result\":\"error\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        # endregion
    }
}
