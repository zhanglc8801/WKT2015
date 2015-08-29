using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public interface ISiteContentBusiness
    {
        /// <summary>
        /// 获取资讯分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<SiteContentEntity> GetSiteContentPageList(SiteContentQuery query);

        /// <summary>
        /// 获取资讯数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<SiteContentEntity> GetSiteContentList(SiteContentQuery query);

        /// <summary>
        /// 获取资讯实体
        /// </summary>
        /// <param name="ContentID"></param>
        /// <returns></returns>
        SiteContentEntity GetSiteContentModel(Int64 ContentID);

        /// <summary>
        /// 新增资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddSiteContent(SiteContentEntity model);

        /// <summary>
        /// 编辑资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateSiteContent(SiteContentEntity model);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteSiteContent(Int64[] ContentID);
    }
}
