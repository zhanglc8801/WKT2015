using System;

using WKT.Memcached;
using WKT.Config;

namespace WKT.Cache
{
    public class MemCacheStrategy : ICacheStrategy
    {
        private static readonly MemCacheStrategy instance = new MemCacheStrategy();

        /// <summary>
        /// 默认缓存存活期为3600秒(1小时)
        /// </summary>
        protected int _timeOut = CacheConfig.Timeout;

        /// <summary>
        /// 构造函数
        /// </summary>
        static MemCacheStrategy()
        {

        }


        /// <summary>
        /// 设置到期相对时间[单位: 秒] 
        /// </summary>
        public virtual int TimeOut
        {
            set { _timeOut = value > 0 ? value : CacheConfig.Timeout; }
            get { return _timeOut > 0 ? _timeOut : CacheConfig.Timeout; }
        }

        private void RemoveCache(string objId)
        {
            if (!string.IsNullOrEmpty(objId.Trim()))
            {
                MemCacheHelper.Delete(objId);
            }
        }

        private bool KeyExists(string objId)
        {
            if (string.IsNullOrEmpty(objId))
                return false;
            return MemCacheHelper.Get(objId) == null ? false : true;
        }

        private void AddCache(string objId, object o)
        {
            AddCache(objId,o,null);
        }

        private void AddCache(string objId, object o, DateTime? dtTimeout)
        {
            if (string.IsNullOrEmpty(objId.Trim()) || o == null)
            {
                return;
            }
            bool flag = false;
            if (dtTimeout == null)
            {
                flag = MemCacheHelper.Insert(objId, o, GetDateTime());
            }
            else
            {
                flag = MemCacheHelper.Insert(objId, o, dtTimeout.Value);
            }
            if (!flag)
            {
                System.Diagnostics.Debug.WriteLine("add object to memcache is error");
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        public virtual void AddObject(string objId, object o)
        {
            string data_key = CacheKey.DATA + objId;
            string ctime_key = CacheKey.CTIME + objId;
            string ctime_value = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //DATA
            AddCache(data_key, o);
            //CTIME
            AddCache(ctime_key, ctime_value);
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        public virtual void AddObject(string objId, object o,DateTime dtTimeout)
        {
            string data_key = CacheKey.DATA + objId;
            string ctime_key = CacheKey.CTIME + objId;
            string ctime_value = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //DATA
            AddCache(data_key, o, dtTimeout);
            //CTIME
            AddCache(ctime_key, ctime_value, dtTimeout);
        }

        /// <summary>
        /// 添加指定ID的cache 有依赖项 
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="dependkey">依赖项key，目前只支持设置一个依赖key</param>
        public void AddObjectWithDepend(string objId, object o, string[] dependkey)
        {
            if (dependkey.Length > 0)
            {
                string depend_key = CacheKey.DEPEND + objId;
                string depend_value = dependkey[0];

                string depctime_key = CacheKey.DEPCTIME + objId;
                object depctime_value = GetCache(CacheKey.CTIME + dependkey[0]);


                //判断dependkey是否存在 
                if (depctime_value != null)
                {
                    AddObject(objId, o);
                    //Depend key
                    AddCache(depend_key, depend_value);
                    //DEPTIME
                    AddCache(depctime_key, depctime_value);
                }
                else// 如果依赖项不存在，只添加普通缓存
                {
                    AddObject(objId, o);
                }
            }
        }
        /// <summary>
        /// 添加指定ID的cache 有依赖项
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="dep">ICacheDependency</param>
        public void AddObjectWithDepend(string objId, object o, ICacheDependency dep)
        {
            if (dep.DependType == EnumDependType.CacheDepend)
            {
                string depend_key = CacheKey.DEPEND + objId;
                string depend_value = dep.Dependkey;

                string depctime_key = CacheKey.DEPCTIME + objId;
                object depctime_value = GetCache(CacheKey.CTIME + dep.Dependkey);

                //判断dependkey是否存在 
                if (depctime_value != null)
                {
                    AddObject(objId, o);

                    //Depend key
                    AddCache(depend_key, depend_value);
                    //DEPTIME
                    AddCache(depctime_key, depctime_value);
                }
                else
                {
                    AddObject(objId, o);
                }
            }
            else// 文件依赖
            {
               ///TODO:
            }
        }

        /// <summary>
        /// 添加指定ID的cache 有依赖项
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="dep">ICacheDependency</param>
        public void AddObjectWithFileChange(string objId, object o, string files)
        {
            
        }

        /// <summary>
        /// 返回指定Key的对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        /// <returns>对象</returns>
        public virtual object GetCache(string objId)
        {
            if (string.IsNullOrEmpty(objId.Trim()))
            {
                return null;
            }
            return MemCacheHelper.Get(objId);
        }

        /// <summary>
        /// 返回指定Key的对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        /// <returns>对象</returns>
        public virtual object GetObject(string objId)
        {
            string data_key = CacheKey.DATA + objId;
            string ctime_key = CacheKey.CTIME + objId;
            string depend_key = CacheKey.DEPEND + objId;
            string depctime_key = CacheKey.DEPCTIME + objId;

            object obj = null;
            //判断objId是否依赖于其他key
            if (!KeyExists(depend_key) && !KeyExists(depctime_key))
            {
                obj = GetCache(data_key);
            }
            else
            {
                object depkey = GetCache(depend_key);//depend key 
                string oldtime = GetCache(depctime_key).ToString();
                string newtime = System.Convert.ToString(GetCache(CacheKey.CTIME + depkey.ToString()));
                //判断依赖项的key是否过期
                if (oldtime == newtime)
                {
                    obj = GetCache(data_key);
                }
                else
                {
                    RemoveCache(objId);
                }
            }
            return obj;
        }

        /// <summary>
        /// 删除指定Key的对象
        /// </summary>
        /// <param name="objId">对象的Key</param>
        /// <returns>对象</returns>
        public virtual object DelObject(string objId)
        {
            if (string.IsNullOrEmpty(objId.Trim()))
            {
                return null;
            }

            string data_key = CacheKey.DATA + objId;
            string ctime_key = CacheKey.CTIME + objId;
            string depend_key = CacheKey.DEPEND + objId;
            string depctime_key = CacheKey.DEPCTIME + objId;

            RemoveCache(data_key);
            RemoveCache(ctime_key);
            RemoveCache(depend_key);
            RemoveCache(depctime_key);

            return null;
        }

        private DateTime GetDateTime()
        {
            int hour = 0; int min = 0; int sec = 0;
            hour = TimeOut / 3600;
            int temp = TimeOut % 3600;
            min = temp / 60;
            sec = temp % 60;

            DateTime cacheTime = DateTime.Now.AddHours(hour).AddMinutes(min).AddSeconds(sec);
            return cacheTime;
        }

        private DateTime GetDateTime(int paramTimeout)
        {
            if (paramTimeout <= 0)
            {
                paramTimeout = TimeOut;
            }
            int hour = 0; int min = 0; int sec = 0;
            hour = paramTimeout / 3600;
            int temp = paramTimeout % 3600;
            min = temp / 60;
            sec = temp % 60;

            DateTime cacheTime = DateTime.Now.AddHours(hour).AddMinutes(min).AddSeconds(sec);
            return cacheTime;
        }
    }
}
