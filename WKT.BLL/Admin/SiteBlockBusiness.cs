using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public class SiteBlockBusiness:ISiteBlockBusiness
    {
        /// <summary>
        /// 获取内容块分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteBlockEntity> GetSiteBlockPageList(SiteBlockQuery query)
        {
            return SiteBlockDataAccess.Instance.GetSSiteBlockPageList(query);
        }

        /// <summary>
        /// 获取内容块数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteBlockEntity> GetSiteBlockList(SiteBlockQuery query)
        {
            return SiteBlockDataAccess.Instance.GetSiteBlockList(query);
        }


        /// <summary>
        /// 获取内容块实体
        /// </summary>
        /// <param name="BlockID"></param>
        /// <returns></returns>
        public SiteBlockEntity GetSiteBlockModel(Int64 BlockID)
        {
            return SiteBlockDataAccess.Instance.GetSiteBlockModel(BlockID);
        }

        /// <summary>
        /// 新增内容块
        /// </summary>
        /// <param name="SiteBlockEntity"></param>
        /// <returns></returns>
        public bool AddSiteBlock(SiteBlockEntity model)
        {
            return SiteBlockDataAccess.Instance.AddSiteBlock(model);
        }

        /// <summary>
        /// 编辑内容块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSiteBlock(SiteBlockEntity model)
        {
            return SiteBlockDataAccess.Instance.UpdateSiteBlock(model);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="BlockID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteBlock(Int64[] BlockID)
        {
            return SiteBlockDataAccess.Instance.BatchDeleteSiteBlock(BlockID);
        }
    }
}
