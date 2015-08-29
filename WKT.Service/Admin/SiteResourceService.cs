using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;
using WKT.Model.Enum;
using WKT.Common.Extension;

namespace WKT.Service
{
    public class SiteResourceService:ISiteResourceService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private ISiteResourceBusiness siteResourceBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public ISiteResourceBusiness SiteResourceBusProvider
        {
            get
            {
                if (siteResourceBusProvider == null)
                {
                    siteResourceBusProvider = new SiteResourceBusiness();//ServiceBusContainer.Instance.Container.Resolve<IDictBusiness>();
                }
                return siteResourceBusProvider;
            }
            set
            {
                siteResourceBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteResourceService()
        {
        }

        /// <summary>
        /// 获取资源分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteResourceEntity> GetSiteResourcePageList(SiteResourceQuery query)
        {
            return SiteResourceBusProvider.GetSiteResourcePageList(query);
        }

        /// <summary>
        /// 获取资源数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteResourceEntity> GetSiteResourceList(SiteResourceQuery query)
        {
            return SiteResourceBusProvider.GetSiteResourceList(query);
        }

        /// <summary>
        /// 获取资源实体
        /// </summary>
        /// <param name="ResourceID"></param>
        /// <returns></returns>
        public SiteResourceEntity GetSiteResourceModel(SiteResourceQuery query)
        {
            return SiteResourceBusProvider.GetSiteResourceModel(query);
        }

        /// <summary>
        /// 新增资源
        /// </summary>
        /// <param name="SiteResourceEntity"></param>
        /// <returns></returns>
        public bool AddSiteResource(SiteResourceEntity model)
        {
            return SiteResourceBusProvider.AddSiteResource(model);
        }

        /// <summary>
        /// 编辑资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSiteResource(SiteResourceEntity model)
        {
            return SiteResourceBusProvider.UpdateSiteResource(model);
        }

        /// <summary>
        /// 累加下载次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AccumulationDownloadCount(SiteResourceEntity model)
        {
            return SiteResourceBusProvider.AccumulationDownloadCount(model);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteResource(Int64[] ResourceID)
        {
            return SiteResourceBusProvider.BatchDeleteSiteResource(ResourceID);
        }

        /// <summary>
        /// 保存资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(SiteResourceEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            model.Name = model.Name.TextFilter();
            model.FileIntro = model.FileIntro.TextFilter();
            if (model.ResourceID == 0)
            {
                result = AddSiteResource(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增资源文件成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增资源文件失败！";
                }
            }
            else
            {
                result = UpdateSiteResource(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "修改资源文件成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "修改资源文件失败！";
                }
            }
            return execResult;
        }
    }
}
