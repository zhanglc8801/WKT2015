using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;
using WKT.Common.Extension;

namespace WKT.Service
{
    public partial class SiteConfigService:ISiteConfigService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private ISiteConfigBusiness siteConfigBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public ISiteConfigBusiness SiteConfigBusProvider
        {
            get
            {
                 if(siteConfigBusProvider == null)
                 {
                     siteConfigBusProvider = new SiteConfigBusiness();//ServiceBusContainer.Instance.Container.Resolve<ISiteConfigBusiness>();
                 }
                 return siteConfigBusProvider;
            }
            set
            {
              siteConfigBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteConfigService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="siteConfigID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public SiteConfigEntity GetSiteConfig(Int64 siteConfigID)
        {
           return SiteConfigBusProvider.GetSiteConfig( siteConfigID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<SiteConfigEntity></returns>
        public List<SiteConfigEntity> GetSiteConfigList()
        {
            return SiteConfigBusProvider.GetSiteConfigList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="siteConfigQuery">SiteConfigQuery查询实体对象</param>
        /// <returns>List<SiteConfigEntity></returns>
        public List<SiteConfigEntity> GetSiteConfigList(SiteConfigQuery siteConfigQuery)
        {
            return SiteConfigBusProvider.GetSiteConfigList(siteConfigQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<SiteConfigEntity></returns>
        public Pager<SiteConfigEntity> GetSiteConfigPageList(CommonQuery query)
        {
            return SiteConfigBusProvider.GetSiteConfigPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<SiteConfigEntity></returns>
        public Pager<SiteConfigEntity> GetSiteConfigPageList(QueryBase query)
        {
            return SiteConfigBusProvider.GetSiteConfigPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="siteConfigQuery">SiteConfigQuery查询实体对象</param>
        /// <returns>Pager<SiteConfigEntity></returns>
        public Pager<SiteConfigEntity> GetSiteConfigPageList(SiteConfigQuery siteConfigQuery)
        {
            return SiteConfigBusProvider.GetSiteConfigPageList(siteConfigQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="siteConfig">SiteConfigEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddSiteConfig(SiteConfigEntity siteConfig)
        {
            return SiteConfigBusProvider.AddSiteConfig(siteConfig);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="siteConfig">SiteConfigEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateSiteConfig(SiteConfigEntity siteConfig)
        {
            siteConfig.Title = siteConfig.Title.TextFilter();
            siteConfig.Keywords = siteConfig.Keywords.TextFilter();
            siteConfig.Description = siteConfig.Description.TextFilter();
            siteConfig.ICPCode = siteConfig.ICPCode.TextFilter();
            siteConfig.Address = siteConfig.Address.TextFilter();
            siteConfig.ZipCode = siteConfig.ZipCode.TextFilter();
            siteConfig.SendMail = siteConfig.SendMail.TextFilter();
            siteConfig.MailServer = siteConfig.MailServer.TextFilter();
            siteConfig.MailAccount = siteConfig.MailAccount.TextFilter();
            siteConfig.SMSUserName = siteConfig.SMSUserName.TextFilter();
            siteConfig.EBankAccount = siteConfig.EBankAccount.TextFilter();
            siteConfig.EBankCode = siteConfig.EBankCode.TextFilter();
            siteConfig.EBankEncryKey = siteConfig.EBankEncryKey.TextFilter();
            if (siteConfig.SiteConfigID == 0)
                return SiteConfigBusProvider.AddSiteConfig(siteConfig);
            return SiteConfigBusProvider.UpdateSiteConfig(siteConfig);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteConfigID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteSiteConfig(Int64 siteConfigID)
        {
            return SiteConfigBusProvider.DeleteSiteConfig( siteConfigID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteConfig">SiteConfigEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteSiteConfig(SiteConfigEntity siteConfig)
        {
            return SiteConfigBusProvider.DeleteSiteConfig(siteConfig);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="siteConfigID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteConfig(Int64[] siteConfigID)
        {
            return SiteConfigBusProvider.BatchDeleteSiteConfig( siteConfigID);
        }
        
        #endregion
        
        #endregion

        /// <summary>
        /// 获取站点配置实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SiteConfigEntity GetSiteConfig(SiteConfigQuery query)
        {
            return SiteConfigBusProvider.GetSiteConfig(query);
        }

        /// <summary>
        /// 更新总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool UpdateSiteAccessCount(SiteConfigQuery query)
        {
            return SiteConfigBusProvider.UpdateSiteAccessCount(query);
        }

	    /// <summary>
        /// 获取总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetSiteAccessCount(SiteConfigQuery query)
        {
            return SiteConfigBusProvider.GetSiteAccessCount(query);
        }
    }
}
