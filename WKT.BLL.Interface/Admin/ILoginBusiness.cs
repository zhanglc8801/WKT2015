using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface  ILoginBusiness
    {
        #region 获取一个实体对象

        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="LoginName">登录名</param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        LoginErrorLogEntity GetLoginErrorLog(string LoginName);

        /// <summary>
        /// 获取登录日志错误信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        LoginErrorLogEntity GetLoginErrorLogInfo(LoginErrorLogQuery query);

        #endregion

        #region 根据条件获取所有实体对象

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<LoginErrorLogEntity></returns>
        List<LoginErrorLogEntity> GetLoginErrorLogList();

        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="authorInfoQuery">AuthorInfoQuery查询实体对象</param>
        /// <returns>List<AuthorInfoEntity></returns>
        List<LoginErrorLogEntity> GetLoginErrorLogList(LoginErrorLogQuery loginErrorLogQuery);


        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="loginErrorLogEntity">AuthorInfoEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddLoginErrorLog(LoginErrorLogEntity loginErrorLogEntity);

        #endregion

        #region 更新一个持久化对象

        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="loginErrorLogEntity">AuthorInfoEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateLoginErrorLog(LoginErrorLogEntity loginErrorLogEntity);


        #endregion

        #region 从存储媒介中删除对象
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="loginErrorLogEntity">AuthorInfoEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteLoginErrorLog(LoginErrorLogQuery loginErrorLogQuery);

        #endregion


    }
}
