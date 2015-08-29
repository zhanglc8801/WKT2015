using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;

namespace WKT.Service
{
    public partial class LoginService : ILoginService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private ILoginBusiness loginBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public ILoginBusiness LoginBusProvider
        {
            get
            {
                if (loginBusProvider == null)
                {
                    loginBusProvider = new LoginBusiness();//ServiceBusContainer.Instance.Container.Resolve<IAuthorInfoBusiness>();
                }
                return loginBusProvider;
            }
            set
            {
                loginBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginService()
        {
        }
        #region 获取一个实体对象
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="LoginName">登录名</param>
        /// <returns></returns>
        public LoginErrorLogEntity GetLoginErrorLog(string LoginName)
        {
            return LoginBusProvider.GetLoginErrorLog(LoginName);
        }
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public LoginErrorLogEntity GetLoginErrorLogInfo(LoginErrorLogQuery query)
        {
            return LoginBusProvider.GetLoginErrorLogInfo(query);
        } 
        #endregion

        #region 根据条件获取所有实体对象
        public List<LoginErrorLogEntity> GetLoginErrorLogList()
        {
            return LoginBusProvider.GetLoginErrorLogList();
        }

        public List<LoginErrorLogEntity> GetLoginErrorLogList(LoginErrorLogQuery loginErrorLogQuery)
        {
            return LoginBusProvider.GetLoginErrorLogList(loginErrorLogQuery);
        } 
        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）
        public bool AddLoginErrorLog(LoginErrorLogEntity loginErrorLogEntity)
        {
            return LoginBusProvider.AddLoginErrorLog(loginErrorLogEntity);
        } 
        #endregion

        #region 更新一个持久化对象
        public bool UpdateLoginErrorLog(LoginErrorLogEntity loginErrorLogEntity)
        {
            return LoginBusProvider.UpdateLoginErrorLog(loginErrorLogEntity);
        } 
        #endregion

        #region 从存储媒介中删除对象
        public bool DeleteLoginErrorLog(LoginErrorLogQuery loginErrorLogQuery)
        {
            return LoginBusProvider.DeleteLoginErrorLog(loginErrorLogQuery);
        } 
        #endregion
    }
}
