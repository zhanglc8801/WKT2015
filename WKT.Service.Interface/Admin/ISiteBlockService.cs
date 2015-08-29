using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public interface ISiteBlockService
    {
        /// <summary>
        /// 获取内容块分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<SiteBlockEntity> GetSiteBlockPageList(SiteBlockQuery query);

        /// <summary>
        /// 获取内容块数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<SiteBlockEntity> GetSiteBlockList(SiteBlockQuery query);

        /// <summary>
        /// 获取内容块实体
        /// </summary>
        /// <param name="BlockID"></param>
        /// <returns></returns>
        SiteBlockEntity GetSiteBlockModel(Int64 BlockID);

        /// <summary>
        /// 新增内容块
        /// </summary>
        /// <param name="SiteBlockEntity"></param>
        /// <returns></returns>
        bool AddSiteBlock(SiteBlockEntity model);

        /// <summary>
        /// 编辑内容块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateSiteBlock(SiteBlockEntity model);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="BlockID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteSiteBlock(Int64[] BlockID);

        /// <summary>
        /// 保存内容块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult Save(SiteBlockEntity model);
    }
}
