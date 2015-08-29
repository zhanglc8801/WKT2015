using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public interface ISiteResourceService
    {
        /// <summary>
        /// 获取资源分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<SiteResourceEntity> GetSiteResourcePageList(SiteResourceQuery query);

        /// <summary>
        /// 获取资源数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<SiteResourceEntity> GetSiteResourceList(SiteResourceQuery query);

        /// <summary>
        /// 获取资源实体
        /// </summary>
        /// <param name="ResourceID"></param>
        /// <returns></returns>
        SiteResourceEntity GetSiteResourceModel(SiteResourceQuery query);

        /// <summary>
        /// 新增资源
        /// </summary>
        /// <param name="SiteResourceEntity"></param>
        /// <returns></returns>
        bool AddSiteResource(SiteResourceEntity model);

        /// <summary>
        /// 编辑资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateSiteResource(SiteResourceEntity model);

        /// <summary>
        /// 累加下载次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AccumulationDownloadCount(SiteResourceEntity model);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteSiteResource(Int64[] ResourceID);

        /// <summary>
        /// 保存资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult Save(SiteResourceEntity model);
    }
}
