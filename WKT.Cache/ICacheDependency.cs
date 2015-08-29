
namespace WKT.Cache
{
    public interface ICacheDependency
    {
        string Dependkey { get; set; }
        EnumDependType DependType { get; set; }
    }

    /// <summary>
    /// 缓存依赖类型
    /// </summary>
    public enum EnumDependType
    {
        CacheDepend = 0,
        FileDepend = 1
    }
}
