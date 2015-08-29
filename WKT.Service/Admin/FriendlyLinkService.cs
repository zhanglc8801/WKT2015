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
    public class FriendlyLinkService:IFriendlyLinkService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IFriendlyLinkBusiness friendlyLinkBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IFriendlyLinkBusiness FriendlyLinkBusProvider
        {
            get
            {
                if (friendlyLinkBusProvider == null)
                {
                    friendlyLinkBusProvider = new FriendlyLinkBusiness();//ServiceBusContainer.Instance.Container.Resolve<IDictBusiness>();
                }
                return friendlyLinkBusProvider;
            }
            set
            {
                friendlyLinkBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FriendlyLinkService()
        {
        }

        /// <summary>
        /// 获取友情链接分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FriendlyLinkEntity> GetFriendlyLinkPageList(FriendlyLinkQuery query)
        {
            return FriendlyLinkBusProvider.GetFriendlyLinkPageList(query);
        }

        /// <summary>
        /// 获取友情链接数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<FriendlyLinkEntity> GetFriendlyLinkList(FriendlyLinkQuery query)
        {
            return FriendlyLinkBusProvider.GetFriendlyLinkList(query);
        }

        /// <summary>
        /// 获取友情链接实体
        /// </summary>
        /// <param name="LinkID"></param>
        /// <returns></returns>
        public FriendlyLinkEntity GetFriendlyLinkModel(Int64 LinkID)
        {
            return FriendlyLinkBusProvider.GetFriendlyLinkModel(LinkID);
        }

        /// <summary>
        /// 新增友情链接
        /// </summary>
        /// <param name="siteNoticeEntity"></param>
        /// <returns></returns>
        public bool AddFriendlyLink(FriendlyLinkEntity model)
        {
            return FriendlyLinkBusProvider.AddFriendlyLink(model);
        }

        /// <summary>
        /// 编辑友情链接
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateFriendlyLink(FriendlyLinkEntity model)
        {
            return FriendlyLinkBusProvider.UpdateFriendlyLink(model);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteFriendlyLink(Int64[] LinkID)
        {
            return FriendlyLinkBusProvider.BatchDeleteFriendlyLink(LinkID);
        }

        /// <summary>
        /// 保存友情链接
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(FriendlyLinkEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            model.SiteName = model.SiteName.TextFilter();
            model.SiteUrl = model.SiteUrl.TextFilter();            
            if (model.LinkID == 0)
            {
                result = AddFriendlyLink(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增友情链接成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增友情链接失败！";
                }
            }
            else
            {
                result = UpdateFriendlyLink(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "修改友情链接成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "修改友情链接失败！";
                }
            }
            return execResult;
        }
    }
}
