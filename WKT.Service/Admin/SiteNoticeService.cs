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
    public partial class SiteNoticeService:ISiteNoticeService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private ISiteNoticeBusiness siteNoticeBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public ISiteNoticeBusiness SiteNoticeBusProvider
        {
            get
            {
                 if(siteNoticeBusProvider == null)
                 {
                     siteNoticeBusProvider = new SiteNoticeBusiness();//ServiceBusContainer.Instance.Container.Resolve<ISiteNoticeBusiness>();
                 }
                 return siteNoticeBusProvider;
            }
            set
            {
              siteNoticeBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteNoticeService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public SiteNoticeEntity GetSiteNotice(Int64 noticeID)
        {
           return SiteNoticeBusProvider.GetSiteNotice( noticeID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<SiteNoticeEntity></returns>
        public List<SiteNoticeEntity> GetSiteNoticeList()
        {
            return SiteNoticeBusProvider.GetSiteNoticeList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="siteNoticeQuery">SiteNoticeQuery查询实体对象</param>
        /// <returns>List<SiteNoticeEntity></returns>
        public List<SiteNoticeEntity> GetSiteNoticeList(SiteNoticeQuery siteNoticeQuery)
        {
            return SiteNoticeBusProvider.GetSiteNoticeList(siteNoticeQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<SiteNoticeEntity></returns>
        public Pager<SiteNoticeEntity> GetSiteNoticePageList(CommonQuery query)
        {
            return SiteNoticeBusProvider.GetSiteNoticePageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<SiteNoticeEntity></returns>
        public Pager<SiteNoticeEntity> GetSiteNoticePageList(QueryBase query)
        {
            return SiteNoticeBusProvider.GetSiteNoticePageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="siteNoticeQuery">SiteNoticeQuery查询实体对象</param>
        /// <returns>Pager<SiteNoticeEntity></returns>
        public Pager<SiteNoticeEntity> GetSiteNoticePageList(SiteNoticeQuery siteNoticeQuery)
        {
            return SiteNoticeBusProvider.GetSiteNoticePageList(siteNoticeQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="siteNotice">SiteNoticeEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddSiteNotice(SiteNoticeEntity siteNotice)
        {
            return SiteNoticeBusProvider.AddSiteNotice(siteNotice);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="siteNotice">SiteNoticeEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateSiteNotice(SiteNoticeEntity siteNotice)
        {
            return SiteNoticeBusProvider.UpdateSiteNotice(siteNotice);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteSiteNotice(Int64 noticeID)
        {
            return SiteNoticeBusProvider.DeleteSiteNotice( noticeID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="siteNotice">SiteNoticeEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteSiteNotice(SiteNoticeEntity siteNotice)
        {
            return SiteNoticeBusProvider.DeleteSiteNotice(siteNotice);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteNotice(Int64[] noticeID)
        {
            return SiteNoticeBusProvider.BatchDeleteSiteNotice( noticeID);
        }
        
        #endregion
        
        #endregion

        /// <summary>
        /// 保存站点公告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(SiteNoticeEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            model.Title = model.Title.TextFilter();
            model.Keywords = model.Keywords.TextFilter();
            model.Description = model.Description.TextFilter();
            model.Content = model.Content.HtmlFilter();
            if (model.NoticeID == 0)
            {
                result = AddSiteNotice(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增失败！";
                }
            }
            else
            {
                result = UpdateSiteNotice(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "修改成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "修改失败！";
                }
            }
            return execResult;
        }
    }
}
