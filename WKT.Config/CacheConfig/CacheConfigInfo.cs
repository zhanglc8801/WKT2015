using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Config
{
    [Serializable]
    public class CacheConfigInfo : IConfigInfo
    {
        /// <summary>
        /// 使用的缓存策略
        /// memcached
        /// entlib
        /// </summary>
        public string CacheStrategy { get; set; }
        /// <summary>
        /// 默认的缓存时间
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// Memcached服务器列表，多个服务器之间以逗号分割
        /// </summary>
        public string MemcacheServerList { get; set; }
    }
}
