using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

using WKT.Model;
using WKT.Service.Interface;
using WKT.Log;
using WKT.Data.MongoDB;
using WKT.Model.Enum;

namespace WKT.Service.System
{
    /// <summary>
    /// 基于MongoDB实现票据服务
    /// </summary>
    public class MongoTicketService : ITicketService
    {
        /// <summary>
        /// 退出
        /// </summary>
        public void Logout(string token)
        {
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="Domain"></param>
        public void Logout(string token, string Domain)
        {
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public bool IsLogin(string token)
        {
            return true;
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        public bool IsLogin(string token, string Domain)
        {
            return true;
        }

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        public string GetUserData(string token)
        {
            return "";
        }

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        public string GetUserData(string token, string Domain)
        {
            return "";
        }
    }
}
