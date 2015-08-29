using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public interface IFriendlyLinkService
    {
        /// <summary>
        /// 获取友情链接分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<FriendlyLinkEntity> GetFriendlyLinkPageList(FriendlyLinkQuery query);

        /// <summary>
        /// 获取友情链接数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<FriendlyLinkEntity> GetFriendlyLinkList(FriendlyLinkQuery query);

        /// <summary>
        /// 获取友情链接实体
        /// </summary>
        /// <param name="LinkID"></param>
        /// <returns></returns>
        FriendlyLinkEntity GetFriendlyLinkModel(Int64 LinkID);

        /// <summary>
        /// 新增友情链接
        /// </summary>
        /// <param name="siteNoticeEntity"></param>
        /// <returns></returns>
        bool AddFriendlyLink(FriendlyLinkEntity model);

        /// <summary>
        /// 编辑友情链接
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateFriendlyLink(FriendlyLinkEntity model);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteFriendlyLink(Int64[] LinkID);

        /// <summary>
        /// 保存友情链接
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult Save(FriendlyLinkEntity model);
    }
}
