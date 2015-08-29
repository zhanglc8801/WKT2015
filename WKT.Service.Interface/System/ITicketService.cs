using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Service.Interface
{
    /// <summary>
    /// 票据Service
    /// Domain参数 是指单站点有效。例如调用了 IsLogin("scj.cn"),是判断在这个站点下是否登录
    /// </summary>
    public interface ITicketService
    {
        /// <summary>
        /// 退出
        /// </summary>
        void Logout(string token);

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="Domain"></param>
        void Logout(string token,string Domain);

        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        bool IsLogin(string token);

        /// <summary>
        /// 是否登录
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        bool IsLogin(string token,string Domain);

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        string GetUserData(string token);

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        string GetUserData(string token,string Domain);

    }
}
