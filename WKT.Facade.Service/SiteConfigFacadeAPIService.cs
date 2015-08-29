using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;
using WKT.Common.Extension;
using WKT.Common.Utils;
using WKT.Facade.Service.Interface;
using WKT.Model.Enum;
using WKT.Log;
using WKT.Common.SMS;
using WKT.Common.Email;

namespace WKT.Facade.Service
{
    public class SiteConfigFacadeAPIService : ServiceBase, ISiteConfigFacadeService
    {
        #region 站点配置
        /// <summary>
        /// 获取站点信息实体
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        public SiteConfigEntity GetSiteConfigModel(SiteConfigQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            SiteConfigEntity model = clientHelper.PostAuth<SiteConfigEntity, SiteConfigQuery>(GetAPIUrl(APIConstant.GETSITECONFIGMODELAJAX), query);
            return model;
        }

        /// <summary>
        /// 修改站点配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult EditSiteConfig(SiteConfigEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.PostAuth<ExecResult, SiteConfigEntity>(GetAPIUrl(APIConstant.UPDATESITECONFIGAJAX), model);
            return result;
        }
        #endregion

        #region 数据字典
        /// <summary>
        /// 获取数据字典分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<DictEntity> GetDictPageList(DictQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<DictEntity> pager = clientHelper.Post<Pager<DictEntity>, DictQuery>(GetAPIUrl(APIConstant.DICT_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取数据字典实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DictEntity GetDictModel(DictQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            DictEntity model = clientHelper.Post<DictEntity, DictQuery>(GetAPIUrl(APIConstant.DICT_GETMODEL), query);
            return model;
        }

        public DictEntity GetDictModelByKey(DictQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            DictEntity model = clientHelper.Post<DictEntity, DictQuery>(GetAPIUrl(APIConstant.DICT_GETMODELBYKEY), query);
            return model;
        }

        /// <summary>
        /// 保存数据字典
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveDict(DictEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, DictEntity>(GetAPIUrl(APIConstant.DICT_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelDict(DictQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, DictQuery>(GetAPIUrl(APIConstant.DICT_DELETE), query);
            return result;
        }

        /// <summary>
        /// 获取数据字典值分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<DictValueEntity> GetDictValuePageList(DictValueQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<DictValueEntity> pager = clientHelper.Post<Pager<DictValueEntity>, DictValueQuery>(GetAPIUrl(APIConstant.DICTVALUE_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取数据字典值实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DictValueEntity GetDictValueModel(DictValueQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            DictValueEntity model = clientHelper.Post<DictValueEntity, DictValueQuery>(GetAPIUrl(APIConstant.DICTVALUE_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存数据字典值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveDictValue(DictValueEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, DictValueEntity>(GetAPIUrl(APIConstant.DICTVALUE_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除数据字典值
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelDictValue(DictValueQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, DictValueQuery>(GetAPIUrl(APIConstant.DICTVALUE_DELETE), query);
            return result;
        }

        /// <summary>
        /// 获取数据字典值键值对
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<int, string> GetDictValueDcit(DictValueQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IDictionary<int, string> result = clientHelper.Post<IDictionary<int, string>, DictValueQuery>(GetAPIUrl(APIConstant.DICTVALUE_DICT), query);
            return result;
        }


        public IList<DictValueEntity> GetDictValueList(DictValueQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<DictValueEntity> result = clientHelper.Post<IList<DictValueEntity>, DictValueQuery>(GetAPIUrl(APIConstant.DICTVALUE_DICTLIST), query);
            return result;
        }
        #endregion

        #region 栏目相关
        /// <summary>
        /// 获取栏目属性数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteChannelEntity> GetSiteChannelList(SiteChannelQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<SiteChannelEntity> list = clientHelper.Post<IList<SiteChannelEntity>, SiteChannelQuery>(GetAPIUrl(APIConstant.SITECHANNEL_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取栏目属性数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="isGetSysFun">是否获取系统功能菜单</param>
        /// <returns></returns>
        public IList<TreeModel> GetSiteChannelTreeList(SiteChannelQuery query, bool isGetSysFun)
        {
            var list = GetSiteChannelList(query);
            TreeModel treeNode = new TreeModel();
            treeNode.Id = 0;
            treeNode.text = "栏目";
            treeNode.url = "";
            treeNode.icon = "";
            treeNode.isexpand = true;
            if (list != null && list.Count > 0)
            {
                var first = list.Where(p => p.PChannelID == 0);
                TreeModel node = null;
                foreach (var item in first)
                {
                    if (isGetSysFun)
                    {
                        if (item.ContentType > 0 && (item.ContentType != (byte)EnumContentType.SystemFun))
                        {
                            node = new TreeModel();
                            node.Id = item.ChannelID;
                            node.text = item.Keywords;
                            node.url = GetChannelContentTypeUrl(item.ContentType, item.ChannelID);
                            node.icon = "";
                            node.isexpand = true;
                            GetSiteChannelTreeList(node, list);
                            treeNode.children.Add(node);
                        }
                    }
                    else
                    {
                        if (item.ContentType > 0)
                        {
                            node = new TreeModel();
                            node.Id = item.ChannelID;
                            node.text = item.Keywords;
                            node.url = GetChannelContentTypeUrl(item.ContentType, item.ChannelID);
                            node.icon = "";
                            node.isexpand = true;
                            GetSiteChannelTreeList(node, list);
                            treeNode.children.Add(node);
                        }
                    }
                }
            }
            IList<TreeModel> resultList = new List<TreeModel>();
            resultList.Add(treeNode);
            return resultList;
        }

        /// <summary>
        /// 获取栏目属性数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<TreeModel> GetSiteChannelTreeList(SiteChannelQuery query)
        {
            return GetSiteChannelTreeList(query, false);
        }

        /// <summary>
        /// 处理子级
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        private void GetSiteChannelTreeList(TreeModel treeNode, IList<SiteChannelEntity> list)
        {
            var child = list.Where(p => p.PChannelID == treeNode.Id);
            if (child.Count() == 0)
            {
                treeNode.isexpand = false;
                treeNode.children = null;
                return;
            }
            TreeModel node = null;
            foreach (var item in child)
            {
                node = new TreeModel();
                node.Id = item.ChannelID;
                node.text = item.Keywords;
                node.url = GetChannelContentTypeUrl(item.ContentType, item.ChannelID);
                node.icon = "";
                node.isexpand = true;
                GetSiteChannelTreeList(node, list);
                treeNode.children.Add(node);
            }
        }

        /// <summary>
        ///  获取栏目后台链接地址
        /// </summary>
        /// <param name="ContentType"></param>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        private string GetChannelContentTypeUrl(Int32 ContentType, Int64 ChannelID)
        {
            switch (ContentType)
            {
                case 1: return WKT.Config.SiteConfig.RootPath + "/SiteContent/Index?ChannelID=" + ChannelID;
                case 2: return WKT.Config.SiteConfig.RootPath + "/SiteBlock/Index?ChannelID=" + ChannelID;
                case 3: return WKT.Config.SiteConfig.RootPath + "/SiteResource/Index?ChannelID=" + ChannelID;
                case 4: return WKT.Config.SiteConfig.RootPath + "/SiteNotice/Index?ChannelID=" + ChannelID;
                case 5: return WKT.Config.SiteConfig.RootPath + "/ContactWay/Index?ChannelID=" + ChannelID;
                case 6: return WKT.Config.SiteConfig.RootPath + "/FriendlyLink/Index?ChannelID=" + ChannelID;
                default: return string.Empty;
            }
        }

        /// <summary>
        /// 获取栏目实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SiteChannelEntity GetSiteChannelModel(SiteChannelQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            SiteChannelEntity model = clientHelper.Post<SiteChannelEntity, SiteChannelQuery>(GetAPIUrl(APIConstant.SITECHANNEL_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存栏目数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveSiteChannel(SiteChannelEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, SiteChannelEntity>(GetAPIUrl(APIConstant.SITECHANNEL_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelSiteChannel(SiteChannelQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, SiteChannelQuery>(GetAPIUrl(APIConstant.SITECHANNEL_DEL), query);
            return result;
        }
        #endregion

        #region 联系人相关
        /// <summary>
        /// 获取联系人分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContactWayEntity> GetContactWayPageList(ContactWayQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<ContactWayEntity> pager = clientHelper.Post<Pager<ContactWayEntity>, ContactWayQuery>(GetAPIUrl(APIConstant.ContactWay_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取联系人数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContactWayEntity> GetContactWayList(ContactWayQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<ContactWayEntity> list = clientHelper.Post<IList<ContactWayEntity>, ContactWayQuery>(GetAPIUrl(APIConstant.ContactWay_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取联系人实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ContactWayEntity GetContactWayModel(ContactWayQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ContactWayEntity model = clientHelper.Post<ContactWayEntity, ContactWayQuery>(GetAPIUrl(APIConstant.ContactWay_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存联系人数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveContactWay(ContactWayEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, ContactWayEntity>(GetAPIUrl(APIConstant.ContactWay_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelContactWay(ContactWayQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, ContactWayQuery>(GetAPIUrl(APIConstant.ContactWay_DEL), query);
            return result;
        }
        #endregion

        #region 站点公告相关
        /// <summary>
        /// 获取站点公告分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteNoticeEntity> GetSiteNoticePageList(SiteNoticeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<SiteNoticeEntity> pager = clientHelper.Post<Pager<SiteNoticeEntity>, SiteNoticeQuery>(GetAPIUrl(APIConstant.SITENOTICE_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取站点公告数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteNoticeEntity> GetSiteNoticeList(SiteNoticeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<SiteNoticeEntity> list = clientHelper.Post<IList<SiteNoticeEntity>, SiteNoticeQuery>(GetAPIUrl(APIConstant.SITENOTICE__GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取站点公告实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SiteNoticeEntity GetSiteNoticeModel(SiteNoticeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            SiteNoticeEntity model = clientHelper.Post<SiteNoticeEntity, SiteNoticeQuery>(GetAPIUrl(APIConstant.SITENOTICE__GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存站点公告数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveSiteNotice(SiteNoticeEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, SiteNoticeEntity>(GetAPIUrl(APIConstant.SITENOTICE__SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除站点公告
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelSiteNotice(SiteNoticeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, SiteNoticeQuery>(GetAPIUrl(APIConstant.SITENOTICE__DEL), query);
            return result;
        }
        #endregion

        #region 友情链接相关
        /// <summary>
        /// 获取友情链接分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FriendlyLinkEntity> GetFriendlyLinkPageList(FriendlyLinkQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<FriendlyLinkEntity> pager = clientHelper.Post<Pager<FriendlyLinkEntity>, FriendlyLinkQuery>(GetAPIUrl(APIConstant.FRIENDLYLINK_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取友情链接数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<FriendlyLinkEntity> GetFriendlyLinkList(FriendlyLinkQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<FriendlyLinkEntity> list = clientHelper.Post<IList<FriendlyLinkEntity>, FriendlyLinkQuery>(GetAPIUrl(APIConstant.FRIENDLYLINK_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取友情链接实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public FriendlyLinkEntity GetFriendlyLinkModel(FriendlyLinkQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            FriendlyLinkEntity model = clientHelper.Post<FriendlyLinkEntity, FriendlyLinkQuery>(GetAPIUrl(APIConstant.FRIENDLYLINK_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存友情链接数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveFriendlyLink(FriendlyLinkEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, FriendlyLinkEntity>(GetAPIUrl(APIConstant.FRIENDLYLINK_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelFriendlyLink(FriendlyLinkQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, FriendlyLinkQuery>(GetAPIUrl(APIConstant.FRIENDLYLINK_DEL), query);
            return result;
        }
        #endregion

        #region 新闻资讯相关
        /// <summary>
        /// 获取新闻资讯分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteContentEntity> GetSiteContentPageList(SiteContentQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<SiteContentEntity> pager = clientHelper.Post<Pager<SiteContentEntity>, SiteContentQuery>(GetAPIUrl(APIConstant.SITECONTENT_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取新闻资讯数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteContentEntity> GetSiteContentList(SiteContentQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<SiteContentEntity> list = clientHelper.Post<IList<SiteContentEntity>, SiteContentQuery>(GetAPIUrl(APIConstant.SITECONTENT_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取新闻资讯实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SiteContentEntity GetSiteContentModel(SiteContentQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            SiteContentEntity model = clientHelper.Post<SiteContentEntity, SiteContentQuery>(GetAPIUrl(APIConstant.SITECONTENT_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存新闻资讯数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveSiteContent(SiteContentEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, SiteContentEntity>(GetAPIUrl(APIConstant.SITECONTENT_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除新闻资讯
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelSiteContent(SiteContentQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, SiteContentQuery>(GetAPIUrl(APIConstant.SITECONTENT_DEL), query);
            return result;
        }
        #endregion

        #region 资源文件相关
        /// <summary>
        /// 获取资源文件分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteResourceEntity> GetSiteResourcePageList(SiteResourceQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<SiteResourceEntity> pager = clientHelper.Post<Pager<SiteResourceEntity>, SiteResourceQuery>(GetAPIUrl(APIConstant.SITERESOURCE_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取资源文件数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteResourceEntity> GetSiteResourceList(SiteResourceQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<SiteResourceEntity> list = clientHelper.Post<IList<SiteResourceEntity>, SiteResourceQuery>(GetAPIUrl(APIConstant.SITERESOURCE_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取资源文件实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SiteResourceEntity GetSiteResourceModel(SiteResourceQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            SiteResourceEntity model = clientHelper.Post<SiteResourceEntity, SiteResourceQuery>(GetAPIUrl(APIConstant.SITERESOURCE_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存资源文件数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveSiteResource(SiteResourceEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, SiteResourceEntity>(GetAPIUrl(APIConstant.SITERESOURCE_SAVE), model);
            return result;
        }

        /// <summary>
        /// 累加资源文件下载次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AccumSiteResourceDownLoadCount(SiteResourceEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            bool result = clientHelper.Post<bool, SiteResourceEntity>(GetAPIUrl(APIConstant.SITERESOURCE_DOWNLOADCOUNT), model);
            return result;
        }

        /// <summary>
        /// 删除资源文件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelSiteResource(SiteResourceQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, SiteResourceQuery>(GetAPIUrl(APIConstant.SITERESOURCE_DEL), query);
            return result;
        }
        #endregion

        #region 内容块相关
        /// <summary>
        /// 获取内容块分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteBlockEntity> GetSiteBlockPageList(SiteBlockQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<SiteBlockEntity> pager = clientHelper.Post<Pager<SiteBlockEntity>, SiteBlockQuery>(GetAPIUrl(APIConstant.SITEBLOCK_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取内容块数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteBlockEntity> GetSiteBlockList(SiteBlockQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<SiteBlockEntity> list = clientHelper.Post<IList<SiteBlockEntity>, SiteBlockQuery>(GetAPIUrl(APIConstant.SITEBLOCK_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取内容块实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SiteBlockEntity GetSiteBlockModel(SiteBlockQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            SiteBlockEntity model = clientHelper.Post<SiteBlockEntity, SiteBlockQuery>(GetAPIUrl(APIConstant.SITEBLOCK_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存内容块数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveSiteBlock(SiteBlockEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, SiteBlockEntity>(GetAPIUrl(APIConstant.SITEBLOCK_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除内容块
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelSiteBlock(SiteBlockQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, SiteBlockQuery>(GetAPIUrl(APIConstant.SITEBLOCK_DEL), query);
            return result;
        }
        #endregion

        #region 站内消息相关
        /// <summary>
        /// 获取站内消息分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteMessageEntity> GetSiteMessagePageList(SiteMessageQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<SiteMessageEntity> pager = clientHelper.Post<Pager<SiteMessageEntity>, SiteMessageQuery>(GetAPIUrl(APIConstant.SITEMESSAGE_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取站内消息数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteMessageEntity> GetSiteMessageList(SiteMessageQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<SiteMessageEntity> list = clientHelper.Post<IList<SiteMessageEntity>, SiteMessageQuery>(GetAPIUrl(APIConstant.SITEMESSAGE_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取站内消息实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SiteMessageEntity GetSiteMessageModel(SiteMessageQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            SiteMessageEntity model = clientHelper.Post<SiteMessageEntity, SiteMessageQuery>(GetAPIUrl(APIConstant.SITEMESSAGE_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存站内消息数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveSiteMessage(SiteMessageEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, SiteMessageEntity>(GetAPIUrl(APIConstant.SITEMESSAGE_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除站内消息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelSiteMessage(SiteMessageQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, SiteMessageQuery>(GetAPIUrl(APIConstant.SITEMESSAGE_DEL), query);
            return result;
        }

        /// <summary>
        /// 阅读站内消息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool UpdateMsgViewed(SiteMessageQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            bool result = clientHelper.Post<bool, SiteMessageQuery>(GetAPIUrl(APIConstant.SITEMESSAGE_VIEWED), query);
            return result;
        }
        #endregion

        #region 邮件短信相关
        /// <summary>
        /// 获取邮件短信模版分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<MessageTemplateEntity> GetMessageTempPageList(MessageTemplateQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<MessageTemplateEntity> pager = clientHelper.Post<Pager<MessageTemplateEntity>, MessageTemplateQuery>(GetAPIUrl(APIConstant.MESSAGETEMP_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取邮件短信模版数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<MessageTemplateEntity> GetMessageTempList(MessageTemplateQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<MessageTemplateEntity> list = clientHelper.Post<IList<MessageTemplateEntity>, MessageTemplateQuery>(GetAPIUrl(APIConstant.MESSAGETEMP_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取邮件短信模版实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public MessageTemplateEntity GetMessageTempModel(MessageTemplateQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            MessageTemplateEntity model = clientHelper.Post<MessageTemplateEntity, MessageTemplateQuery>(GetAPIUrl(APIConstant.MESSAGETEMP_GETMODEL), query);
            return model;
        }

        /// <summary>
        /// 保存邮件短信模版数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveMessageTemp(MessageTemplateEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, MessageTemplateEntity>(GetAPIUrl(APIConstant.MESSAGETEMP_SAVE), model);
            return result;
        }

        /// <summary>
        /// 删除邮件短信模版
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult DelMessageTemp(MessageTemplateQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, MessageTemplateQuery>(GetAPIUrl(APIConstant.MESSAGETEMP_DEL), query);
            return result;
        }

        /// <summary>
        /// 获取模版类型键值对
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<int, string> GetTCategoryDict(MessageTemplateQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IDictionary<int, string> dict = clientHelper.Post<IDictionary<int, string>, MessageTemplateQuery>(GetAPIUrl(APIConstant.MESSAGETEMP_TCATEGORYDICT), query);
            return dict;
        }

        /// <summary>
        /// 获取模版类型键值对(去除已经存在模版的模版类型)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<int, string> GetTCategoryDictChecked(MessageTemplateQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IDictionary<int, string> dict = clientHelper.Post<IDictionary<int, string>, MessageTemplateQuery>(GetAPIUrl(APIConstant.MESSAGETEMP_TCATEGORYDICTCHECKED), query);
            return dict;
        }

        /// <summary>
        /// 获取短信邮件全局变量
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetEmailVariable()
        {
            string[] arry = new string[] { "接收人","发送人","邮箱", "手机", "稿件编号", "稿件标题"
                , "审稿链接","作者链接", "系统日期", "系统时间", "网站名称", "编辑部地址", "编辑部邮编","审毕日期","金额" };
            return arry.ToDictionary(p => "${" + p + "}$", q => q);
        }

        #region 发送邮件短信
        /// <summary>
        /// 保存邮件短信发送记录
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool SaveSendRecode(IList<MessageRecodeEntity> list)
        {
            if (list == null || list.Count == 0)
                return false;
            HttpClientHelper clientHelper = new HttpClientHelper();
            bool result = clientHelper.Post<bool, IList<MessageRecodeEntity>>(GetAPIUrl(APIConstant.MSGRECODE_SAVE), list);
            return result;
        }

        /// <summary>
        /// 获取发送记录分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<MessageRecodeEntity> GetMessageRecodePageList(MessageRecodeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<MessageRecodeEntity> pager = clientHelper.Post<Pager<MessageRecodeEntity>, MessageRecodeQuery>(GetAPIUrl(APIConstant.MSGRECODE_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 获取发送记录数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<MessageRecodeEntity> GetMessageRecodeList(MessageRecodeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<MessageRecodeEntity> list = clientHelper.Post<IList<MessageRecodeEntity>, MessageRecodeQuery>(GetAPIUrl(APIConstant.MSGRECODE_GETLIST), query);
            return list;
        }

        /// <summary>
        /// 获取发送记录实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public MessageRecodeEntity GetMsgRecodeModel(MessageRecodeQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            MessageRecodeEntity model = clientHelper.Post<MessageRecodeEntity, MessageRecodeQuery>(GetAPIUrl(APIConstant.MSGRECODE_GETMSGRECODEMODEL), query);
            return model;
        }

        /// <summary>
        /// 发送短信或邮件(不使用模版发送)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SendEmailOrSms(MessageRecodeEntity model)
        {
            ExecResult result = new ExecResult();
            IList<MessageRecodeEntity> list = new List<MessageRecodeEntity>();
            model.Index = 0;
            model.MsgTitle = model.MsgTitle.HtmlFilter();
            model.MsgContent = model.MsgContent.HtmlFilter();
            list.Add(model);
            string error = string.Empty;
            bool flag = SendEmailOrSms(list, model.MsgType == 1, true, model, ref error);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "发送成功！";
            }
            else
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = "发送失败！" + error;
            }
            return result;
        }

        /// <summary>
        /// 发送短信或邮件(不使用模版发送)   选择作者信息
        /// </summary>
        /// <param name="ReciveUserList"></param>
        ///<param name="entity">(必填项 MsgType,JournalID,SendUser,MsgTitle,MsgContent)</param>      
        /// <returns></returns>
        public ExecResult SendEmailOrSms(IList<Int64> ReciveUserList, MessageRecodeEntity entity, bool isRecode = true)
        {
            ExecResult result = new ExecResult();
            bool isEmail = entity.MsgType == 1;
            string msg = isEmail ? "邮件" : "短信";
            if (ReciveUserList == null || ReciveUserList.Count == 0)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = msg + "发送失败，请选择" + msg + "接收人！";
                return result;
            }
            IDictionary<Int64, string> userDcit = null;
            bool isCard=false;
            //发送贺卡  需要人名
            if (entity.MsgContent.Contains("{贺卡}"))
            {
                userDcit = GetAuthorDcit(ReciveUserList, entity.JournalID);
                isCard=true;
            }
            else
            {
                userDcit = GetAuthorDcit(ReciveUserList, entity.JournalID, isEmail);
                if(userDcit==null)
                    userDcit = GetContributionAuthorDcit(ReciveUserList, entity.JournalID, isEmail);
            }
            if (userDcit == null || userDcit.Count == 0)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = msg + "发送失败，接收人的" + msg + "非法！";
                return result;
            }
            IList<MessageRecodeEntity> list = new List<MessageRecodeEntity>();
            MessageRecodeEntity model = null;
            int index = 0;

            string error = string.Empty;
            bool flag = false;
            if (isCard)
            {
                foreach (var item in userDcit)
                {
                    string[] strs = item.Value.Split(',');
                    model = new MessageRecodeEntity();
                    model.JournalID = entity.JournalID;
                    model.CID = entity.CID;
                    model.ReciveUser = item.Key;
                    model.ReciveAddress =strs[0];
                    model.MsgType = (Byte)(isEmail ? 1 : 2);
                    model.SendType = entity.SendType == null ? 0 : entity.SendType;
                    model.SendUser = entity.SendUser;
                    model.SendDate = DateTime.Now;
                    model.MsgTitle = entity.MsgTitle.HtmlFilter();
                    model.MsgContent = entity.MsgContent.HtmlFilter().Replace("{贺卡}",strs[1]);
                    model.AddDate = DateTime.Now;
                    model.FilePath = entity.FilePath;
                    model.Index = index;
                    model.TemplateID = entity.TemplateID;
                    list.Add(model);
                    index++;
                }

                flag=SendEmailOrSms(list, isEmail, false, entity, ref error);
            }
            else
            {
                foreach (var item in userDcit)
                {
                    model = new MessageRecodeEntity();
                    model.JournalID = entity.JournalID;
                    model.CID = entity.CID;
                    model.ReciveUser = item.Key;
                    model.ReciveAddress = item.Value;
                    model.MsgType = (Byte)(isEmail ? 1 : 2);
                    model.SendType = entity.SendType == null ? 0 : entity.SendType;
                    model.SendUser = entity.SendUser;
                    model.SendDate = DateTime.Now;
                    model.MsgTitle = entity.MsgTitle.HtmlFilter();
                    model.MsgContent = entity.MsgContent.HtmlFilter();
                    model.AddDate = DateTime.Now;
                    model.FilePath = entity.FilePath;
                    model.Index = index;
                    model.TemplateID = entity.TemplateID;
                    list.Add(model);
                    index++;
                }
               flag=SendEmailOrSms(list, isEmail, true, entity, ref error);
            }
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = msg + "发送成功！";
                var except = ReciveUserList.Except(userDcit.Select(p => p.Key));
                if (except.Count() > 0)
                {
                    result.msg += string.Format("部分作者[{0}]发送失败", string.Join(",", except));
                }
            }
            else
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = msg + "发送失败！" + error;
            }
            return result;
        }






        /// <summary>
        /// 发送短信或邮件(不使用模版发送)  直接使用短信号码或邮件地址
        /// </summary>
        /// <param name="ReciveAddressList"></param>
        /// <param name="entity">(必填项 MsgType,JournalID,SendUser,MsgTitle,MsgContent)</param>
        /// <returns></returns>
        public ExecResult SendEmailOrSms(IList<String> ReciveAddressList, MessageRecodeEntity entity, bool isRecode = true)
        {
            ExecResult result = new ExecResult();
            bool isEmail = entity.MsgType == 1;
            string msg = isEmail ? "邮件地址" : "短信号码";
            if (ReciveAddressList == null || ReciveAddressList.Count == 0)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = msg + "发送失败，请输入正确" + msg + "！";
                return result;
            }
            if (isEmail)
                ReciveAddressList = ReciveAddressList.Where(p => !string.IsNullOrWhiteSpace(p))
                    .Where(p => p.IsEmail()).ToList();
            else
                ReciveAddressList = ReciveAddressList.Where(p => !string.IsNullOrWhiteSpace(p))
                   .Where(p => p.IsMobilePhone()).ToList();
            if (ReciveAddressList == null || ReciveAddressList.Count == 0)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = msg + "发送失败，请输入正确" + msg + "！";
                return result;
            }
            IList<MessageRecodeEntity> list = new List<MessageRecodeEntity>();
            MessageRecodeEntity model = null;
            int index = 0;
            foreach (var item in ReciveAddressList)
            {
                model = new MessageRecodeEntity();
                model.JournalID = entity.JournalID;
                model.CID = entity.CID;
                model.ReciveUser = entity.ReciveUser;
                model.ReciveAddress = item;
                model.MsgType = entity.MsgType;
                model.SendType = entity.SendType == null ? 0 : entity.SendType;
                model.SendUser = entity.SendUser;
                model.SendDate = DateTime.Now;
                model.MsgTitle = entity.MsgTitle.HtmlFilter();
                model.MsgContent = entity.MsgContent.HtmlFilter();
                model.AddDate = DateTime.Now;
                model.FilePath = entity.FilePath;
                model.Index = index;
                list.Add(model);
                index++;
            }
            string error = string.Empty;
            bool flag = SendEmailOrSms(list, isEmail, true, entity, ref error, null, isRecode);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = msg + "发送成功！";
            }
            else
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = msg + "发送失败！" + error;
            }
            return result;
        }

        /// <summary>
        /// 发送短信或邮件(使用模版)
        /// </summary>
        /// <param name="ReciveUserdict">子级键值对中必须包含 $稿件主键$ </param>
        /// <param name="entity">(必填项 MsgType,JournalID,SendType,SendUser)</param>      
        /// <returns></returns>
        public ExecResult SendEmailOrSms(IDictionary<Int64, IDictionary<string, string>> ReciveUserdict, MessageRecodeEntity entity, bool isRecode = true)
        {
            ExecResult result = new ExecResult();
            bool isEmail = entity.MsgType == 1;
            string msg = isEmail ? "邮件" : "短信";
            if (ReciveUserdict == null || ReciveUserdict.Count == 0)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = msg + "发送失败，请选择" + msg + "接收人！";
                return result;
            }
            MessageTemplateEntity temp = GetMessageTemplate(entity.JournalID, entity.SendType.Value, entity.MsgType);
            if (temp == null)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = msg + "发送失败，获取模版信息失败！";
                return result;

            }
            IDictionary<Int64, string> userDcit = GetAuthorDcit(ReciveUserdict.Keys.ToList(), entity.JournalID, isEmail);
            if (userDcit == null || userDcit.Count == 0)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = msg + "发送失败，接收人的" + msg + "非法！";
                return result;
            }
            SiteConfigEntity config = GetSiteConfig(entity.JournalID);
            if (config == null)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = msg + "发送失败，请先配置站点的" + msg + "信息！";
                return result;
            }
            IList<MessageRecodeEntity> list = new List<MessageRecodeEntity>();
            MessageRecodeEntity model = null;
            int index = 0;
            foreach (var item in userDcit)
            {
                model = new MessageRecodeEntity();
                model.JournalID = entity.JournalID;
                model.CID = ReciveUserdict[item.Key]["$稿件主键$"].TryParse<Int64>();
                model.ReciveUser = item.Key;
                model.ReciveAddress = item.Value;
                model.MsgType = entity.MsgType;
                model.SendType = entity.SendType;
                model.SendUser = entity.SendUser;
                model.SendDate = DateTime.Now;
                model.MsgTitle = temp.Title.HtmlFilter();
                model.MsgContent = GetEmailOrSmsContent(ReciveUserdict[item.Key], config, temp.TContent).HtmlFilter();
                model.AddDate = DateTime.Now;
                model.FilePath = entity.FilePath;
                model.Index = index;
                list.Add(model);
                index++;
            }
            string error = string.Empty;
            bool flag = SendEmailOrSms(list, isEmail, false, model, ref error, null, isRecode);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = msg + "发送成功！";
            }
            else
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = msg + "发送失败！" + error;
            }
            return result;
        }

        /// <summary>
        /// 发送邮件短信
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isEmail">true:邮件 false:短信</param>
        /// <param name="isMass">true:群发(没有使用模版) false:一条一条发</param>
        /// <param name="sendMailName">邮件发件人称呼</param>
        /// <returns></returns>
        private bool SendEmailOrSms(IList<MessageRecodeEntity> list, bool isEmail, bool isMass, MessageRecodeEntity entity, ref string errorMsg, SiteConfigEntity config = null, bool IsRecode = true)
        {
            try
            {
                if (config == null)
                    config = GetSiteConfig(list[0].JournalID);
                List<Int32> errorList = new List<Int32>();
                if (isEmail)   //发送邮件
                {
                    if (string.IsNullOrWhiteSpace(config.MailServer)
                        || string.IsNullOrWhiteSpace(config.MailAccount)
                        || string.IsNullOrWhiteSpace(config.MailPwd))
                    {
                        errorMsg = "请配置邮件发送相关信息！";
                        return false;
                    }
                    if (entity != null)
                    {
                        if (!string.IsNullOrWhiteSpace(entity.sendMailName))
                            config.SendMail = entity.sendMailName;
                    }
                    if (isMass)  //群发
                    {
                        if (!EmailUtils.SendMailEx(config.MailServer, config.MailPort, config.MailAccount, config.MailPwd, config.SendMail
                            , string.Join("，", list.Select(p => p.ReciveAddress)), string.Empty, string.Empty, list[0].MsgTitle, list[0].MsgContent
                            , list[0].FilePath, 2, true, "UTF-8", true, config.MailIsSSL))
                            errorList.AddRange(list.Select(p => p.Index));
                    }
                    else  //一条一条的发
                    {
                        foreach (var model in list)
                        {
                            if (!EmailUtils.SendMailEx(config.MailServer, config.MailPort, config.MailAccount, config.MailPwd, config.SendMail
                            , model.ReciveAddress, string.Empty, string.Empty, model.MsgTitle, model.MsgContent
                            , model.FilePath, 2, true, "UTF-8", true, config.MailIsSSL))
                                errorList.Add(model.Index);
                        }
                    }
                }
                else //发送短信
                {
                    if (string.IsNullOrWhiteSpace(config.SMSUserName))
                    {
                        errorMsg = "请配置短信发送相关信息！";
                        return false;
                    }
                    if (isMass)
                    {
                        if (entity != null)
                        {
                            if (!SmsHelper.SendSmsSHP(string.Join(",", list.Select(p => p.ReciveAddress)), list[0].MsgContent, config.SMSUserName, config.SMSPwd))
                                errorList.AddRange(list.Select(p => p.Index));
                            
                        }

                    }
                    else
                    {
                        if (entity != null)
                        {
                            foreach (var model in list)
                            {
                                if (!SmsHelper.SendSmsSHP(model.ReciveAddress, model.MsgContent, config.SMSUserName, config.SMSPwd))
                                    errorList.Add(model.Index);
                            }
                            
                        }


                    }
                }

                var listResult = list.Where(p => !errorList.Contains(p.Index)).ToList();

                if (listResult == null || listResult.Count == 0)
                    return false;

                if (IsRecode)
                {
                    SaveSendRecode(listResult);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("发送邮件短信异常：" + ex.Message);
                errorMsg = ex.Message;
                return false;
            }
        }


        #region 只发送邮件(手动输入单个地址)
        /// <summary>
        /// 只发送邮件(手动输入单个地址)
        /// </summary>
        /// <param name="MsgTitle"></param>
        /// <param name="MsgContent"></param>
        /// <param name="ReciveAddress"></param>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        public ExecResult SendEmail(string MsgTitle, string MsgContent, string ReciveAddress, string SendMail, long JournalID)
        {
            ExecResult result = new ExecResult();
            bool flag = SendEmailOnly(MsgTitle, MsgContent, ReciveAddress, SendMail, JournalID);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "发送成功！";
            }
            else
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = "发送失败！";
            }
            return result;
        }
        /// <summary>
        /// 只发送邮件
        /// </summary>
        /// <param name="MsgTitle">邮件标题</param>
        /// <param name="MsgContent">邮件内容</param>
        /// <param name="ReciveAddress">接收地址</param>
        /// <param name="JournalID">杂志ID</param>
        /// <returns></returns>
        private bool SendEmailOnly(string MsgTitle, string MsgContent, string ReciveAddress, string SendMail, long JournalID)
        {
            SiteConfigEntity config = GetSiteConfig(JournalID);
            if (EmailUtils.SendMailEx(config.MailServer, config.MailPort, config.MailAccount, config.MailPwd, SendMail, ReciveAddress, string.Empty, string.Empty, MsgTitle, MsgContent, null, 2, true, "GB2312", true, config.MailIsSSL))
                return true;
            else
                return false;
        } 
        #endregion

        /// <summary>
        /// 获取作者信息
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="isEmail"></param>
        /// <returns></returns>
        private IDictionary<Int64, string> GetAuthorDcit(IList<Int64> userList, Int64 JournalID, bool isEmail)
        {
            if (isEmail)
            {
                AuthorInfoQuery query = new AuthorInfoQuery();
                query.AuthorIDs = userList.Distinct().ToArray();
                query.JournalID = JournalID;
                query.PageSize = userList.Count;
                query.CurrentPage = 1;
                AuthorFacadeAPIService service = new AuthorFacadeAPIService();
                var list = service.GetAuthorList(query).ItemList;
                if (list == null || list.Count == 0)
                    return null;
                return list.Where(p => !string.IsNullOrWhiteSpace(p.LoginName))
                    .ToDictionary(p => p.AuthorID, q => q.LoginName);
            }
            else
            {
                AuthorDetailQuery query = new AuthorDetailQuery();
                query.AuthorIDs = userList.Distinct().ToArray();
                query.JournalID = JournalID;
                query.PageSize = userList.Count;
                query.CurrentPage = 1;
                AuthorPlatformFacadeAPIService service = new AuthorPlatformFacadeAPIService();
                var list = service.GetAuthorDetailList(query);
                return list.Where(p => !string.IsNullOrWhiteSpace(p.Mobile))
                    .Where(p => p.Mobile.IsMobilePhone())
                    .ToDictionary(p => p.AuthorID, q => q.Mobile);
            }
        }

        /// <summary>
        /// 获取稿件作者信息
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="JournalID"></param>
        /// <param name="isEmail"></param>
        /// <returns></returns>
        private IDictionary<Int64, string> GetContributionAuthorDcit(IList<Int64> userList, Int64 JournalID, bool isEmail)
        {
            if (isEmail)
            {
                ContributionAuthorQuery query = new ContributionAuthorQuery();
                //query.CAuthorIDs = userList.Distinct().ToArray();
                query.CAuthorID = userList[0];
                query.JournalID = JournalID;
                query.PageSize = userList.Count;
                query.CurrentPage = 1;
                ContributionFacadeAPIService service = new ContributionFacadeAPIService();
                var list = service.GetContributionAuthorList(query);
                if (list == null || list.Count == 0)
                    return null;
                return list.Where(p => !string.IsNullOrWhiteSpace(p.Email))
                    .ToDictionary(p => p.CAuthorID, q => q.Email);
            }
            else
            {
                ContributionAuthorQuery query = new ContributionAuthorQuery();
                //query.CAuthorIDs = userList.Distinct().ToArray();
                query.CAuthorID = userList[0];
                query.JournalID = JournalID;
                query.PageSize = userList.Count;
                query.CurrentPage = 1;
                ContributionFacadeAPIService service = new ContributionFacadeAPIService();
                var list = service.GetContributionAuthorList(query);
                if (list == null || list.Count == 0)
                    return null;
                return list.Where(p => !string.IsNullOrWhiteSpace(p.Mobile))
                    .ToDictionary(p => p.CAuthorID, q => q.Mobile);
            }
            
        }

        /// <summary>
        /// 获取专家信息  为了发贺卡
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="isEmail"></param>
        /// <returns></returns>
        private IDictionary<Int64, string> GetAuthorDcit(IList<Int64> userList, Int64 JournalID)
        {

            AuthorInfoQuery query = new AuthorInfoQuery();
            query.AuthorIDs = userList.Distinct().ToArray();
            query.JournalID = JournalID;
            query.PageSize = userList.Count;
            query.CurrentPage = 1;
            AuthorFacadeAPIService service = new AuthorFacadeAPIService();
            var list = service.GetAuthorList(query).ItemList;
            if (list == null || list.Count == 0)
                return null;
            return list.Where(p => !string.IsNullOrWhiteSpace(p.LoginName))
                .ToDictionary(p => p.AuthorID, q => { return q.LoginName + "," + q.RealName; });

        }

        /// <summary>
        /// 获取邮件短信模版实体
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="TCategory"></param>
        /// <returns></returns>
        public MessageTemplateEntity GetMessageTemplate(Int64 JournalID, Int32 TCategory, Byte TType)
        {
            MessageTemplateQuery query = new MessageTemplateQuery();
            query.JournalID = JournalID;
            query.TCategory = TCategory;
            query.TType = TType;
            query.ModelType = 1;
            return GetMessageTempModel(query);
        }

        /// <summary>
        /// 获取站点配置实体
        /// </summary>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        public SiteConfigEntity GetSiteConfig(Int64 JournalID)
        {
            SiteConfigQuery query = new SiteConfigQuery();
            query.JournalID = JournalID;
            SiteConfigEntity model = GetSiteConfigModel(query);
            if (string.IsNullOrWhiteSpace(model.SendMail))
                model.SendMail = model.MailAccount;
            return model;
        }

        /// <summary>
        /// 获取替换模版后的邮件短信内容
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="config"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string GetEmailOrSmsContent(IDictionary<string, string> dict, SiteConfigEntity config, string content)
        {
            if (dict == null)
                dict = new Dictionary<string, string>();
            dict["${系统日期}$"] = DateTime.Now.ToString("yyyy-MM-dd");
            dict["${系统时间}$"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (config != null)
            {
                dict["${网站名称}$"] = config.Title;
                dict["${编辑部地址}$"] = config.Address;
                dict["${编辑部邮编}$"] = config.ZipCode;
            }
            foreach (var item in dict)
            {
                if(item.Key!="${金额}$")
                content = content.Replace(item.Key, item.Value);
            }
            return content;
        }
        /// <summary>
        /// 获取替换模版后的邮件短信内容
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="config"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string GetEmailOrSmsContent(IDictionary<string, string> dict, string content)
        {
            SiteConfigEntity config = GetSiteConfig(WKT.Config.SiteConfig.SiteID);
            if (dict == null)
                dict = new Dictionary<string, string>();
            dict["${系统日期}$"] = DateTime.Now.ToString("yyyy-MM-dd");
            dict["${系统时间}$"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dict["${网站名称}$"] = config.Title;
            dict["${编辑部地址}$"] = config.Address;
            dict["${编辑部邮编}$"] = config.ZipCode;
            if (!string.IsNullOrEmpty(content))
            {
                foreach (var item in dict)
                {
                    content = content.Replace(item.Key, item.Value);
                }
            }

            return content;
        }

        #endregion
        #endregion

        #region 留言相关

        /// <summary>
        /// 获取留言分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<GuestbookEntity> GetSiteGuestBookPageList(GuestbookQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<GuestbookEntity> pager = clientHelper.Post<Pager<GuestbookEntity>, GuestbookQuery>(GetAPIUrl(APIConstant.SITE_GUESTBOOKIE_GETPAGELIST), query);
            return pager;
        }

        /// <summary>
        /// 保存留言
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveSiteGuestBook(GuestbookEntity model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult result = clientHelper.Post<ExecResult, GuestbookEntity>(GetAPIUrl(APIConstant.SITE_GUESTBOOKE_SAVE), model);
            return result;
        }

        # endregion

        # region 站点访问量

        /// <summary>
        /// 更新总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool UpdateSiteAccessCount(SiteConfigQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            return clientHelper.Post<bool, SiteConfigQuery>(GetAPIUrl(APIConstant.SITECONFIG_UPDATESITEACCESSCOUNT), query);
        }

        /// <summary>
        /// 获取总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetSiteAccessCount(SiteConfigQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            int result = clientHelper.Post<int, SiteConfigQuery>(GetAPIUrl(APIConstant.SITECONFIG_GETSITEACCESSCOUNT), query);
            return result;
        }

        # endregion
    }
}
