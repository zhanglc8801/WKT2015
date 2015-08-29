using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using WKT.Cache;

namespace WKT.Facade.Service.Wrapper
{
    /// <summary>
    /// 读取配置文件解析服务接口
    /// </summary>
    public class ServiceContainer
    {
        private static ServiceContainer _instance = new ServiceContainer();
        private string configFile = "";
        private string _ContainerName = "ContainerService";
        static readonly object lockcontainer = new object();
        private static IUnityContainer serviceUnityContainer;

        /// <summary>
        /// 构造函数，得到Unit配置文件
        /// </summary>
        private ServiceContainer()
        {
            configFile = WKT.Config.BaseConfig.FacadeServiceUnityConfigPath;
            if (string.IsNullOrEmpty(configFile))
            {
                throw new Exception("请检查配置文件目录(config)中是否正确配置unity.facade.service.config文件");
            }
            else
            {
                configFile = AppDomain.CurrentDomain.BaseDirectory + configFile;
            }
        }

        /// <summary>
        /// 单例方式实例化
        /// </summary>
        public static ServiceContainer Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 初始化Unity的IoC容器对象
        /// </summary>
        public IUnityContainer Container {
            get {
                lock (lockcontainer)
                {
                    if (serviceUnityContainer == null)
                    {
                         object cacheObj = CacheStrategyFactory.Instance.GetObject(CacheKey.SERVICE_IOC_UNITYCONFIGKEY);
                         if (cacheObj == null)
                         {
                             serviceUnityContainer = new UnityContainer();
                             ExeConfigurationFileMap basicFileMap = new ExeConfigurationFileMap
                             {
                                 ExeConfigFilename = configFile
                             };
                             UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager
                                 .OpenMappedExeConfiguration(basicFileMap, ConfigurationUserLevel.None)
                                 .GetSection("unity");
                             section.Configure(serviceUnityContainer, _ContainerName);
                             CacheStrategyFactory.Instance.AddObject(CacheKey.SERVICE_IOC_UNITYCONFIGKEY, serviceUnityContainer, configFile);
                         }
                         else
                         {
                             serviceUnityContainer = (UnityContainer)cacheObj;
                         }
                    }
                    return serviceUnityContainer;
                }
            }
        }
    }
}
