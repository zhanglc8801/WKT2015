
namespace WKT.Cache
{
    /// <summary>
    /// Memcache缓存依赖
    /// </summary>
    public class MemCacheDependency : ICacheDependency
    {
        public string Dependkey { get; set; }
        public EnumDependType DependType { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dependkey">缓存依赖项key</param>
        public MemCacheDependency(string dependkey)
        {
            Dependkey = dependkey;
            DependType = EnumDependType.CacheDepend;
        }
    }
}
