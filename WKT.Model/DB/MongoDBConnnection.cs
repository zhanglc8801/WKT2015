using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model
{
    public sealed class MongoDBConnnection
    {
        /// <summary>
        /// MongoDB Host
        /// </summary>
        public string Host
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["MongoDBHost"]))
                {
                    return "127.0.0.1";
                }
                else
                {
                    return ConfigurationManager.AppSettings["MongoDBHost"];
                }
            }
        }

        public int Port
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["MongoDBPort"]))
                {
                    return 27017;
                }
                else
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["MongoDBPort"]);
                }
            }
        }

        /// <summary>
        /// 用户名密码
        /// </summary>
        public string UserName
        {
            get
            {
                return ConfigurationManager.AppSettings["MongoDBUser"];
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["MongoDBPassword"];
            }
        }

        /// <summary>
        /// 默认数据库
        /// </summary>
        public string DefaultDataBase
        {
            get;
            set;
        }
    }
}
