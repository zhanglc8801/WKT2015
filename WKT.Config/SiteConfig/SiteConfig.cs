using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace WKT.Config
{
    public class SiteConfig
    {
        public static string ErrorMessage
        {
            get
            {
                return "请检查配置文件目录(config)中是否正确配置siteconfig.config文件";
            }
        }

        private static System.Timers.Timer baseConfigTimer = new System.Timers.Timer(60000);

        private static SiteConfigInfo m_configinfo;

        /// <summary>
        /// 静态构造函数初始化相应实例和定时器
        /// </summary>
        static SiteConfig()
        {
            m_configinfo = SiteConfigFileManager.LoadConfig();
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
            m_configinfo = SiteConfigFileManager.LoadConfig();
        }


        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetRealConfig()
        {
            m_configinfo = SiteConfigFileManager.LoadRealConfig();
        }

        public static SiteConfigInfo GetSiteConfig()
        {
            return m_configinfo;
        }

        /// <summary>
        /// 获取稿件相关附件格式限制
        /// </summary>
        /// <param name="type">0:稿件 1:附图 3:稿件pdf 4:介绍信</param>
        /// <returns></returns>
        public static string GetContributionInfoFileExt(int type)
        {
            switch (type)
            {
                case 1: return GetSiteConfig().FigurePath;
                case 2: return GetSiteConfig().PDFPath;
                case 3: return GetSiteConfig().IntroLetterPath;
                case 4: return GetSiteConfig().UploadHtmExt;
                default: return GetSiteConfig().ContributePath;
            }
        }

        /// <summary>
        /// 允许上传文件格式
        /// </summary>
        public static string UploadFileExt
        {
            get
            {
                return GetSiteConfig().UploadFileExt;
            }
        }

        /// <summary>
        /// 允许上传HTML文件格式
        /// </summary>
        public static string UploadHtmExt
        {
            get
            {
                return GetSiteConfig().UploadHtmExt;
            }
        }

        /// <summary>
        /// 允许上传图片格式
        /// </summary>
        public static string UploadImgExt
        {
            get
            {
                return GetSiteConfig().UploadImgExt;
            }
        }

        /// <summary>
        /// 上传文件虚拟路径
        /// </summary>
        public static string UploadPath
        {
            get
            {
                return GetSiteConfig().UploadPath;
            }
        }

        /// <summary>
        /// qq
        /// </summary>
        public static string QQ
        {
            get
            {
                return GetSiteConfig().QQ;
            }
        }

        /// <summary>
        /// Api站点
        /// </summary>
        public static string APIHost
        {
            get
            {
                return GetSiteConfig().APIHost;
            }
        }

        
        /// <summary>
        /// Api授权Key
        /// </summary>
        public static string APIPublicKey
        {
            get
            {
                return GetSiteConfig().APIPublicKey;
            }
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        public static string SiteName
        {
            get
            {
                return GetSiteConfig().SiteName;
            }
        }

        /// <summary>
        /// 站点标识
        /// </summary>
        public static long SiteID
        {
            get
            {
                return WKT.Common.Utils.TypeParse.ToLong(ConfigurationManager.AppSettings["SiteID"], 0);
            }
        }

        /// <summary>
        /// 站点根目录
        /// </summary>
        public static string RootPath
        {
            get
            {
                return ConfigurationManager.AppSettings["RootPath"];
            }
        }

        /// <summary>
        /// 是否显示财务信息
        /// </summary>
        public static bool IsFinance
        {
            get
            {
                return GetSiteConfig().IsFinance == 1;
            }
        }
        
        /// <summary>
        /// 计费设置-审稿费金额
        /// </summary>
        public static decimal ReviewFeeText
        {
            get
            {
                return GetSiteConfig().ReviewFeeText;
            }
        }
        /// <summary>
        /// 计费设置-版面费金额
        /// </summary>
        public static decimal PageFeeText
        {
            get
            {
                return GetSiteConfig().PageFeeText;
            }
        }
        /// <summary>
        /// 计费设置-稿费-按篇金额
        /// </summary>
        public static decimal GaoFeeText1
        {
            get
            {
                return GetSiteConfig().GaoFeeText1;
            }
        }
        /// <summary>
        /// 计费设置-稿费-按页金额
        /// </summary>
        public static decimal GaoFeeText2
        {
            get
            {
                return GetSiteConfig().GaoFeeText2;
            }
        }

        /// <summary>
        /// 计费设置-稿费-按版面费百分比
        /// </summary>
        public static decimal GaoFeeText3
        {
            get
            {
                return GetSiteConfig().GaoFeeText3;
            }
        }

        /// <summary>
        /// 专家审稿费
        /// </summary>
        public static decimal ExpertReviewFee
        {
            get
            {
                return GetSiteConfig().ExpertReviewFee;
            }
        }

        /// <summary>
        /// 保存配置实例
        /// </summary>
        /// <param name="baseconfiginfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(SiteConfigInfo siteConfiginfo)
        {
            SiteConfigFileManager acfm = new SiteConfigFileManager();
            SiteConfigFileManager.ConfigInfo = siteConfiginfo;
            return acfm.SaveConfig();
        }
    }
}
