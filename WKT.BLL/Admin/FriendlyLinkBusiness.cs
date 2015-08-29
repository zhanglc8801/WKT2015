using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public class FriendlyLinkBusiness : IFriendlyLinkBusiness
    {
        /// <summary>
        /// 获取友情链接分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FriendlyLinkEntity> GetFriendlyLinkPageList(FriendlyLinkQuery query)
        {
            return FriendlyLinkDataAccess.Instance.GetFriendlyLinkPageList(query);
        }

        /// <summary>
        /// 获取友情链接数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<FriendlyLinkEntity> GetFriendlyLinkList(FriendlyLinkQuery query)
        {
            return FriendlyLinkDataAccess.Instance.GetFriendlyLinkList(query);
        }

        /// <summary>
        /// 获取友情链接实体
        /// </summary>
        /// <param name="LinkID"></param>
        /// <returns></returns>
        public FriendlyLinkEntity GetFriendlyLinkModel(Int64 LinkID)
        {
            return FriendlyLinkDataAccess.Instance.GetFriendlyLinkModel(LinkID);
        }

        /// <summary>
        /// 新增友情链接
        /// </summary>
        /// <param name="siteNoticeEntity"></param>
        /// <returns></returns>
        public bool AddFriendlyLink(FriendlyLinkEntity model)
        {
            return FriendlyLinkDataAccess.Instance.AddFriendlyLink(model);
        }

        /// <summary>
        /// 编辑友情链接
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateFriendlyLink(FriendlyLinkEntity model)
        {
            return FriendlyLinkDataAccess.Instance.UpdateFriendlyLink(model);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteFriendlyLink(Int64[] LinkID)
        {
            return FriendlyLinkDataAccess.Instance.BatchDeleteFriendlyLink(LinkID);
        }
    }
}
