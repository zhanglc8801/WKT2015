using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public class SiteResourceBusiness:ISiteResourceBusiness
    {
        /// <summary>
        /// 获取资源分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteResourceEntity> GetSiteResourcePageList(SiteResourceQuery query)
        {
            return SiteResourceDataAccess.Instance.GetSiteResourcePageList(query);
        }

        /// <summary>
        /// 获取资源数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteResourceEntity> GetSiteResourceList(SiteResourceQuery query)
        {
            return SiteResourceDataAccess.Instance.GetSiteResourceList(query);
        }

        /// <summary>
        /// 获取资源实体
        /// </summary>
        /// <param name="ResourceID"></param>
        /// <returns></returns>
        public SiteResourceEntity GetSiteResourceModel(SiteResourceQuery query)
        {
            return SiteResourceDataAccess.Instance.GetSiteResourceModel(query);
        }

        /// <summary>
        /// 新增资源
        /// </summary>
        /// <param name="SiteResourceEntity"></param>
        /// <returns></returns>
        public bool AddSiteResource(SiteResourceEntity model)
        {
            return SiteResourceDataAccess.Instance.AddSiteResource(model);
        }

        /// <summary>
        /// 编辑资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSiteResource(SiteResourceEntity model)
        {
            return SiteResourceDataAccess.Instance.UpdateSiteResource(model);
        }

        /// <summary>
        /// 累加下载次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AccumulationDownloadCount(SiteResourceEntity model)
        {
            return SiteResourceDataAccess.Instance.AccumulationDownloadCount(model);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteResource(Int64[] ResourceID)
        {
            return SiteResourceDataAccess.Instance.BatchDeleteSiteResource(ResourceID);
        }
    }
}
