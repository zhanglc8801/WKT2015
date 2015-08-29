using System;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

using WKT.Config;

namespace WKT.Cache
{
    /// <summary>
    /// 默认缓存机制
    /// </summary>
    public class DefaultCacheStrategy : ICacheStrategy
    {
        private static readonly DefaultCacheStrategy instance = new DefaultCacheStrategy();

        /// <summary>
        /// 默认缓存存活期为3600秒(1小时)
        /// </summary>
        protected int _timeOut = CacheConfig.Timeout;

        private static object syncObj = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        static DefaultCacheStrategy()
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


        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        public virtual void AddObject(string objId, object o)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            CacheFactory.GetCacheManager().Add(objId, o, CacheItemPriority.High, null, new AbsoluteTime(DateTime.Now.AddSeconds(TimeOut)));
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        public virtual void AddObject(string objId, object o,DateTime dtTimeout)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            CacheFactory.GetCacheManager().Add(objId, o, CacheItemPriority.High, null, new AbsoluteTime(dtTimeout));
        }

        /// <summary>
        /// 文件依赖
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="files"></param>
        public void AddObjectWithFileChange(string objId, object o, string files)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }
            FileDependency _fileDep = new FileDependency(files);
            CacheFactory.GetCacheManager().Add(objId, o, CacheItemPriority.High, null, _fileDep);
        }

        /// <summary>
        /// 依赖项
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="dependKey"></param>
        public void AddObjectWithDepend(string objId, object o, string[] dependKey)
        {
            
        }

        public void AddObjectWithDepend(string objId, object o, ICacheDependency dep)
        {
            if (dep.DependType == EnumDependType.FileDepend)
            {
                AddObjectWithFileChange(objId, o, dep.Dependkey);
            }
            else
            {
                AddObjectWithDepend(objId, o, new string []{dep.Dependkey});
            }
        }

        public object GetCache(string objId)
        {
            if (objId == null || objId.Length == 0)
            {
                return null;
            }
            return CacheFactory.GetCacheManager().GetData(objId);
        }

        /// <summary>
        /// 返回指定Key的对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        /// <returns>对象</returns>
        public virtual object GetObject(string objId)
        {
            if (objId == null || objId.Length == 0)
            {
                return null;
            }
            return CacheFactory.GetCacheManager().GetData(objId);
        }

        /// <summary>
        /// 删除指定Key的对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        /// <returns>对象</returns>
        public virtual object DelObject(string objId)
        {
            if (objId == null || objId.Length == 0)
            {
                return null;
            }
            try
            {
                CacheFactory.GetCacheManager().Remove(objId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 建立回调委托的一个实例
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="reason"></param>
        public void onRemove(string key, object val, CacheItemRemovedReason reason)
        {
            switch (reason)
            {
                case CacheItemRemovedReason.Scavenged:
                    break;
                case CacheItemRemovedReason.Expired:
                    {
                        //CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(this.onRemove);

                        //webCache.Insert(key, val, null, System.DateTime.Now.AddMinutes(TimeOut),
                        //    System.Web.Caching.Cache.NoSlidingExpiration,
                        //    System.Web.Caching.CacheItemPriority.High,
                        //    callBack);
                        break;
                    }
                case CacheItemRemovedReason.Removed:
                    {
                        break;
                    }
                case CacheItemRemovedReason.Unknown:
                    {
                        break;
                    }
                default: break;
            }			
        }
    }
}
