
namespace WKT.Cache
{
    public class DefaultCacheDependency : ICacheDependency
    {
       public string Dependkey { get; set; }
        public EnumDependType DependType { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dependkey">缓存依赖项key</param>
        public DefaultCacheDependency(string dependkey)
        {
            Dependkey = dependkey;
            DependType = EnumDependType.FileDepend;
        }
    }
}
