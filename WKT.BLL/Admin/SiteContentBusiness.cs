using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public class SiteContentBusiness:ISiteContentBusiness
    {
        /// <summary>
        /// 获取资讯分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteContentEntity> GetSiteContentPageList(SiteContentQuery query)
        {
            return SiteContentDataAccess.Instance.GetSiteContentPageList(query);
        }

        /// <summary>
        /// 获取资讯数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteContentEntity> GetSiteContentList(SiteContentQuery query)
        {
            return SiteContentDataAccess.Instance.GetSiteContentList(query);
        }

        /// <summary>
        /// 获取资讯实体
        /// </summary>
        /// <param name="ContentID"></param>
        /// <returns></returns>
        public SiteContentEntity GetSiteContentModel(Int64 ContentID)
        {
            return SiteContentDataAccess.Instance.GetSiteContentModel(ContentID);
        }

        /// <summary>
        /// 新增资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddSiteContent(SiteContentEntity model)
        {
            return SiteContentDataAccess.Instance.AddSiteContent(model);
        }

        /// <summary>
        /// 编辑资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSiteContent(SiteContentEntity model)
        {
            return SiteContentDataAccess.Instance.UpdateSiteContent(model);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteContent(Int64[] ContentID)
        {
            return SiteContentDataAccess.Instance.BatchDeleteSiteContent(ContentID);
        }
    }
}
