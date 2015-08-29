using System;

namespace WKT.Cache
{
    /// <summary>
    /// 公共缓存策略接口
    /// </summary>
    public interface ICacheStrategy
    {
        /// <summary>
        /// 添加指定Key的对象
        /// </summary>
        /// <param name="objId">缓存Key</param>
        /// <param name="o"></param>
        void AddObject(string objId, object o);

        /// <summary>
        /// 添加指定Key的对象
        /// </summary>
        /// <param name="objId">缓存Key</param>
        /// <param name="o">缓存对象</param>
        /// <param name="dtTimeout">过期时间</param>
        void AddObject(string objId, object o,DateTime dtTimeout);

        // 添加指定ID的对象(关联指定文件组)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="files"></param>
        void AddObjectWithFileChange(string objId, object o, string files);
        /// <summary>
        /// 添加指定ID的对象(关联指定键值组)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="dependKey"></param>
        void AddObjectWithDepend(string objId, object o, string[] dependKey);
        /// <summary>
        /// 添加指定ID的对象(关联ICacheDependency)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="dep"></param>
        void AddObjectWithDepend(string objId, object o, ICacheDependency dep);

        /// <summary>
        /// 返回指定Key的对象
        /// </summary>
        /// <param name="objId">缓存Key</param>
        /// <returns></returns>
        object GetObject(string objId);

        /// <summary>
        /// 返回指定ID的cache     
        /// </summary>
        /// <param name="objId">DATA_key1，CTIME_key1,DEPEND_key1,DEPCTIME_key1</param>
        /// <returns></returns>
        object GetCache(string objId);

        /// <summary>
        /// 删除指定Key的对象
        /// </summary>
        /// <param name="objId">缓存Key</param>
        /// <returns></returns>
        object DelObject(string objId);
        /// <summary>
        /// 到期时间,单位：秒
        /// </summary>
        int TimeOut { set; get; }
    }
}
