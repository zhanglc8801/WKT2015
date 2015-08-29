using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Config
{
    public class CacheConfig
    {
        public static string ErrorMessage
        {
            get {
                return "请检查配置文件目录(config)中是否正确配置cache.config文件";
            }
        }

        private static System.Timers.Timer baseConfigTimer = new System.Timers.Timer(60000);

        private static CacheConfigInfo m_configinfo;

		/// <summary>
        /// 静态构造函数初始化相应实例和定时器
		/// </summary>
        static CacheConfig()
        {
            m_configinfo = CacheConfigFileManager.LoadConfig();
            baseConfigTimer.AutoReset = true;
            baseConfigTimer.Enabled = true;
            baseConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed); 
            baseConfigTimer.Start();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetConfig();
        }


        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetConfig()
        {
            m_configinfo = CacheConfigFileManager.LoadConfig();
        }


        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetRealConfig()
        {
            m_configinfo = CacheConfigFileManager.LoadRealConfig();
        }

		public static CacheConfigInfo GetCacheConfig()
		{
            return m_configinfo;
		}

		/// <summary>
		/// 缓存策略
		/// </summary>
        public static string CacheStrategy
		{
			get
			{
                return GetCacheConfig().CacheStrategy;
			}
		}

        /// <summary>
        /// 默认缓存时间
        /// </summary>
        public static int Timeout
        {
            get
            {
                return GetCacheConfig().Timeout;
            }
        }

        /// <summary>
        /// 返回Memcache缓存服务器列表
        /// </summary>
        public static string MemcacheServerList
        {
            get
            {
                return GetCacheConfig().MemcacheServerList;
            }
        }

        /// <summary>
        /// 保存配置实例
        /// </summary>
        /// <param name="baseconfiginfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(CacheConfigInfo cacheConfiginfo)
        {
            CacheConfigFileManager acfm = new CacheConfigFileManager();
            CacheConfigFileManager.ConfigInfo = cacheConfiginfo;
            return acfm.SaveConfig();
        }
    }
}
