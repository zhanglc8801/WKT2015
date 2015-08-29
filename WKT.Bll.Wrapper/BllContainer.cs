using System;
using System.Configuration;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using WKT.Cache;

namespace WKT.Bll.Wrapper
{
    public sealed class BllContainer
    {
        private static BllContainer _instance = new BllContainer();
        private string configFile = "";
        private string _ContainerName = "ContainerBLL";
        static readonly object lockobj = new object();
        static readonly object lockcontainer = new object();
        private static IUnityContainer bllUnityContainer;

        /// <summary>
        /// 构造函数，得到Unit配置文件
        /// </summary>
        private BllContainer()
        {
            configFile = WKT.Config.BaseConfig.BllUnityConfigPath;
            if (string.IsNullOrEmpty(configFile))
            {
                throw new Exception("请检查配置文件目录(config)中是否正确配置unity.bll.config文件");
            }
            else
            {
                configFile = AppDomain.CurrentDomain.BaseDirectory + configFile;
            }
        }

        /// <summary>
        /// 单例方式实例化
        /// </summary>
        public static BllContainer Instance
        {
            get
            {
               return _instance;
            }
        }

        /// <summary>
        /// 初始化Unity的IoC容器对象
        /// </summary>
        public IUnityContainer Container
        {
            get
            {
                lock (lockcontainer)
                {
                    if (bllUnityContainer == null)
                    {
                        object cacheObj = CacheStrategyFactory.Instance.GetObject(CacheKey.BLL_IOC_UNITYCONFIGKEY);
                        if (cacheObj == null)
                        {
                            bllUnityContainer = new UnityContainer();
                            ExeConfigurationFileMap basicFileMap = new ExeConfigurationFileMap
                            {
                                ExeConfigFilename = configFile
                            };
                            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager
                                .OpenMappedExeConfiguration(basicFileMap, ConfigurationUserLevel.None)
                                .GetSection("unity");
                            section.Configure(bllUnityContainer, _ContainerName);
                            CacheStrategyFactory.Instance.AddObject(CacheKey.BLL_IOC_UNITYCONFIGKEY, bllUnityContainer, configFile);
                        }
                        else
                        {
                            bllUnityContainer = (UnityContainer)cacheObj;
                        }
                    }
                    return bllUnityContainer;
                }
            }
        }
        /// <summary>
        /// 创建一个实例
        /// </summary>
        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
