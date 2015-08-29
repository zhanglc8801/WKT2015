using System;

using WKT.Config;
using WKT.Memcached;

namespace WKT.Cache
{
    /// <summary>
    /// 缓存处理类
    /// </summary>
    public class CacheStrategyFactory
    {
        private static volatile CacheStrategyFactory instance = null;
        private static object lockHelper = new object();
        private static ICacheStrategy cachedStrategy;

        private CacheStrategyFactory()
        {
            string cacheStrategy = CacheConfig.CacheStrategy;

            if (cacheStrategy.ToLower() == "memcached")
            {
                if (!string.IsNullOrEmpty(CacheConfig.MemcacheServerList))
                {
                    string[] memcacheServerList = CacheConfig.MemcacheServerList.Split(',');
                    MemCacheHelper.SetServerList("WKT_CacheServer",memcacheServerList);
                    cachedStrategy = new MemCacheStrategy();
                }
                else
                {
                    // 如果没有设置memcache server则采用默认的缓存机制
                    cachedStrategy = new DefaultCacheStrategy();
                }
            }
            else if (cacheStrategy.ToLower() == "entlib")
            {
                cachedStrategy = new DefaultCacheStrategy();
            }
        }

        public static CacheStrategyFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new CacheStrategyFactory();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="o">缓存对象</param>
        /// <param name="sec">缓存时间(单位：秒)</param>
        public void AddObject(string key, object o, int sec)
        {
            cachedStrategy.TimeOut = sec;
            cachedStrategy.AddObject(key, o);
        }

        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="o">缓存对象</param>
        /// <param name="dtTimeout">过期时间</param>
        public void AddObject(string key, object o, DateTime dtTimeout)
        {
            cachedStrategy.AddObject(key, o, dtTimeout);
        }

        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="o">缓存对象</param>
        /// <param name="sec">缓存时间(单位：秒)</param>
        public void AddObject(string key, object o)
        {
            cachedStrategy.AddObject(key, o);
        }

        public void AddObject(string objectId, object o, string[] keys)
        {
            lock (lockHelper)
            {
                cachedStrategy.AddObjectWithDepend(objectId, o, keys);
            }
        }
        public void AddObject(string objectId, object o, ICacheDependency dep)
        {
            lock (lockHelper)
            {
                cachedStrategy.AddObjectWithDepend(objectId, o, dep);
            }
        }

        public void AddObject(string objectId, object o, string file)
        {
            lock (lockHelper)
            {
                cachedStrategy.AddObjectWithFileChange(objectId, o, file);
            }
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object GetObject(string key)
        {
            return cachedStrategy.GetObject(key);
        }

        /// <summary>
        /// 删除缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object DelObject(string key)
        {
            return cachedStrategy.DelObject(key);
        }

        # region xml cache key

        

        # endregion
    }
}
