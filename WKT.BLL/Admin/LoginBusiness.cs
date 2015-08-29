using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public partial class LoginBusiness:ILoginBusiness
    {
        #region 获取一个实体对象

        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="authorID">作者信息</param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public LoginErrorLogEntity GetLoginErrorLog(string LoginName)
        {
            return LoginDataAccess.Instance.GetLoginErrorLog(LoginName);
        }

        /// <summary>
        /// 获取登录错误日志信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public LoginErrorLogEntity GetLoginErrorLogInfo(LoginErrorLogQuery query)
        {
            return LoginDataAccess.Instance.GetLoginErrorLogInfo(query);
        }
        #endregion

        #region 根据条件获取所有实体对象

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<AuthorInfoEntity></returns>
        public List<LoginErrorLogEntity> GetLoginErrorLogList()
        {
            return LoginDataAccess.Instance.GetLoginErrorLogList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="authorInfoQuery">AuthorInfoQuery查询实体对象</param>
        /// <returns>List<AuthorInfoEntity></returns>
        public List<LoginErrorLogEntity> GetLoginErrorLogList(LoginErrorLogQuery loginErrorLogQuery)
        {
            return LoginDataAccess.Instance.GetLoginErrorLogList(loginErrorLogQuery);
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="authorInfo">AuthorInfoEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddLoginErrorLog(LoginErrorLogEntity loginErrorLogEntity)
        {
            return LoginDataAccess.Instance.AddLoginErrorLog(loginErrorLogEntity);
        }
        #endregion

        #region 更新一个持久化对象

        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="loginErrorLogEntity">LoginErrorLogEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateLoginErrorLog(LoginErrorLogEntity loginErrorLogEntity)
        {
            return LoginDataAccess.Instance.UpdateLoginErrorLog(loginErrorLogEntity);
        }

        #endregion

        #region 从存储媒介中删除对象
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="authorID">作者信息</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteLoginErrorLog(LoginErrorLogQuery loginErrorLogQuery)
        {
            return LoginDataAccess.Instance.DeleteLoginErrorLog(loginErrorLogQuery);
        }

        #endregion

    }
}
