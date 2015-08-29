using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;
using WKT.Model.Enum;
using WKT.Common.Extension;

namespace WKT.Service
{
    public partial class SiteChannelService:ISiteChannelService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private ISiteChannelBusiness siteChannelBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public ISiteChannelBusiness SiteChannelBusProvider
        {
            get
            {
                 if(siteChannelBusProvider == null)
                 {
                     siteChannelBusProvider = new SiteChannelBusiness();//ServiceBusContainer.Instance.Container.Resolve<ISiteChannelBusiness>();
                 }
                 return siteChannelBusProvider;
            }
            set
            {
              siteChannelBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteChannelService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public SiteChannelEntity GetSiteChannel(SiteChannelQuery query)
        {
           return SiteChannelBusProvider.GetSiteChannel( query );
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<SiteChannelEntity></returns>
        public List<SiteChannelEntity> GetSiteChannelList()
        {
            return SiteChannelBusProvider.GetSiteChannelList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="siteChannelQuery">SiteChannelQuery查询实体对象</param>
        /// <returns>List<SiteChannelEntity></returns>
        public List<SiteChannelEntity> GetSiteChannelList(SiteChannelQuery siteChannelQuery)
        {
            return SiteChannelBusProvider.GetSiteChannelList(siteChannelQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<SiteChannelEntity></returns>
        public Pager<SiteChannelEntity> GetSiteChannelPageList(CommonQuery query)
        {
            return SiteChannelBusProvider.GetSiteChannelPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<SiteChannelEntity></returns>
        public Pager<SiteChannelEntity> GetSiteChannelPageList(QueryBase query)
        {
            return SiteChannelBusProvider.GetSiteChannelPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="siteChannelQuery">SiteChannelQuery查询实体对象</param>
        /// <returns>Pager<SiteChannelEntity></returns>
        public Pager<SiteChannelEntity> GetSiteChannelPageList(SiteChannelQuery siteChannelQuery)
        {
            return SiteChannelBusProvider.GetSiteChannelPageList(siteChannelQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="siteChannel">SiteChannelEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddSiteChannel(SiteChannelEntity siteChannel)
        {
            return SiteChannelBusProvider.AddSiteChannel(siteChannel);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="siteChannel">SiteChannelEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateSiteChannel(SiteChannelEntity siteChannel)
        {
            return SiteChannelBusProvider.UpdateSiteChannel(siteChannel);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteSiteChannel(Int64 channelID)
        {
            return SiteChannelBusProvider.DeleteSiteChannel( channelID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteChannel">SiteChannelEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteSiteChannel(SiteChannelEntity siteChannel)
        {
            return SiteChannelBusProvider.DeleteSiteChannel(siteChannel);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteChannel(Int64[] channelID)
        {
            return SiteChannelBusProvider.BatchDeleteSiteChannel( channelID);
        }
        
        #endregion
        
        #endregion

        /// <summary>
        /// 保存栏目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(SiteChannelEntity model)
        {
            ExecResult execResult = new ExecResult();            
            bool result = false;
            model.Keywords = model.Keywords.TextFilter();
            model.Description = model.Description.TextFilter();
            model.ChannelUrl = model.ChannelUrl.TextFilter();         
            if (model.ChannelID == 0)
            {
                result = AddSiteChannel(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增栏目成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增栏目失败！";
                }
            }
            else
            {
                result = UpdateSiteChannel(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "修改栏目成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "修改栏目失败！";
                }
            }
            return execResult;
        }
    }
}
