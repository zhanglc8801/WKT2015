using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WKT.Model
{
    [Serializable]
    [BsonIgnoreExtraElements]
    public class AccessLog
    {
        /// <summary>
        /// Mongo产生的自动ID
        /// </summary>
        [BsonId]
        public ObjectId _id
        {
            get;
            set;
        }

        /// <summary>
        /// 杂志ID
        /// </summary>
        public long JournalID
        {
            get;
            set;
        }

        /// <summary>
        /// 浏览器类型
        /// </summary>
        public string BrowserType { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 浏览器版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 系统平台
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        public string UrlReferrer { get; set; }

        /// <summary>
        /// 访问者IP
        /// </summary>
        public string UserHostAddress { get; set; }

        /// <summary>
        /// 是否通过认证
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// HttpMethod
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime LogDateTime { get; set; }

        /// <summary>
        /// 访问者城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 访问者地区
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 访问者国家
        /// </summary>
        public string Country { get; set; }
    }
}
