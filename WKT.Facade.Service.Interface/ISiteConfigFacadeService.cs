using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Model;

namespace WKT.Facade.Service.Interface
{
    public interface ISiteConfigFacadeService
    {
        #region 站点配置
        /// <summary>
        /// 获取站点配置实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        SiteConfigEntity GetSiteConfigModel(SiteConfigQuery query);

        /// <summary>
        /// 修改站点配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult EditSiteConfig(SiteConfigEntity model);
        #endregion

        #region 数据字典
        /// <summary>
        /// 获取数据字典分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<DictEntity> GetDictPageList(DictQuery query);

        /// <summary>
        /// 获取数据字典实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        DictEntity GetDictModel(DictQuery query);

                /// <summary>
        /// 获取数据字典实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        DictEntity GetDictModelByKey(DictQuery query);
        /// <summary>
        /// 保存数据字典
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveDict(DictEntity model);

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelDict(DictQuery query);

        /// <summary>
        /// 获取数据字典值分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<DictValueEntity> GetDictValuePageList(DictValueQuery query);

        /// <summary>
        /// 获取数据字典值实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        DictValueEntity GetDictValueModel(DictValueQuery query);

        /// <summary>
        /// 保存数据字典值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveDictValue(DictValueEntity model);

        /// <summary>
        /// 删除数据字典值
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelDictValue(DictValueQuery query);

        /// <summary>
        /// 获取数据字典值键值对
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IDictionary<int, string> GetDictValueDcit(DictValueQuery query);


        IList<DictValueEntity> GetDictValueList(DictValueQuery query);
        #endregion

        #region 栏目相关
        /// <summary>
        /// 获取栏目属性数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<SiteChannelEntity> GetSiteChannelList(SiteChannelQuery query);

        /// <summary>
        /// 获取栏目属性数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<TreeModel> GetSiteChannelTreeList(SiteChannelQuery query);

        /// <summary>
        /// 获取栏目实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        SiteChannelEntity GetSiteChannelModel(SiteChannelQuery query);

        /// <summary>
        /// 保存栏目数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveSiteChannel(SiteChannelEntity model);

        /// <summary>
        /// 删除数据字典值
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelSiteChannel(SiteChannelQuery query);
        #endregion

        #region 联系人相关
        /// <summary>
        /// 获取联系人分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<ContactWayEntity> GetContactWayPageList(ContactWayQuery query);

        /// <summary>
        /// 获取联系人数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<ContactWayEntity> GetContactWayList(ContactWayQuery query);

        /// <summary>
        /// 获取联系人实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ContactWayEntity GetContactWayModel(ContactWayQuery query);

        /// <summary>
        /// 保存联系人数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveContactWay(ContactWayEntity model);

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelContactWay(ContactWayQuery query);
        #endregion

        #region 站点公告相关
        /// <summary>
        /// 获取站点公告分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<SiteNoticeEntity> GetSiteNoticePageList(SiteNoticeQuery query);

        /// <summary>
        /// 获取站点公告数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<SiteNoticeEntity> GetSiteNoticeList(SiteNoticeQuery query);

        /// <summary>
        /// 获取站点公告实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        SiteNoticeEntity GetSiteNoticeModel(SiteNoticeQuery query);

        /// <summary>
        /// 保存站点公告数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveSiteNotice(SiteNoticeEntity model);

        /// <summary>
        /// 删除站点公告
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelSiteNotice(SiteNoticeQuery query);
        #endregion

        #region 友情链接相关
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
        /// <param name="query"></param>
        /// <returns></returns>
        FriendlyLinkEntity GetFriendlyLinkModel(FriendlyLinkQuery query);

        /// <summary>
        /// 保存友情链接数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveFriendlyLink(FriendlyLinkEntity model);

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelFriendlyLink(FriendlyLinkQuery query);
        #endregion

        #region 新闻资讯相关
        /// <summary>
        /// 获取新闻资讯分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<SiteContentEntity> GetSiteContentPageList(SiteContentQuery query);

        /// <summary>
        /// 获取新闻资讯数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<SiteContentEntity> GetSiteContentList(SiteContentQuery query);

        /// <summary>
        /// 获取新闻资讯实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        SiteContentEntity GetSiteContentModel(SiteContentQuery query);

        /// <summary>
        /// 保存新闻资讯数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveSiteContent(SiteContentEntity model);

        /// <summary>
        /// 删除新闻资讯
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelSiteContent(SiteContentQuery query);
        #endregion

        #region 资源文件相关
        /// <summary>
        /// 获取资源文件分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<SiteResourceEntity> GetSiteResourcePageList(SiteResourceQuery query);

        /// <summary>
        /// 获取资源文件数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<SiteResourceEntity> GetSiteResourceList(SiteResourceQuery query);

        /// <summary>
        /// 获取资源文件实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        SiteResourceEntity GetSiteResourceModel(SiteResourceQuery query);

        /// <summary>
        /// 保存资源文件数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveSiteResource(SiteResourceEntity model);

        /// <summary>
        /// 累加资源文件下载次数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AccumSiteResourceDownLoadCount(SiteResourceEntity model);

        /// <summary>
        /// 删除资源文件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelSiteResource(SiteResourceQuery query);
        #endregion

        #region 内容块相关
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
        /// <param name="query"></param>
        /// <returns></returns>
        SiteBlockEntity GetSiteBlockModel(SiteBlockQuery query);

        /// <summary>
        /// 保存内容块数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveSiteBlock(SiteBlockEntity model);

        /// <summary>
        /// 删除内容块
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelSiteBlock(SiteBlockQuery query);
        #endregion

        #region 站内消息相关
        /// <summary>
        /// 获取站内消息分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<SiteMessageEntity> GetSiteMessagePageList(SiteMessageQuery query);

        /// <summary>
        /// 获取站内消息数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<SiteMessageEntity> GetSiteMessageList(SiteMessageQuery query);

        /// <summary>
        /// 获取站内消息实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        SiteMessageEntity GetSiteMessageModel(SiteMessageQuery query);

        /// <summary>
        /// 保存站内消息数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveSiteMessage(SiteMessageEntity model);

        /// <summary>
        /// 删除站内消息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelSiteMessage(SiteMessageQuery query);

        /// <summary>
        /// 阅读站内消息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        bool UpdateMsgViewed(SiteMessageQuery query);
        #endregion

        #region 邮件短信相关
        /// <summary>
        /// 获取邮件短信模版分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<MessageTemplateEntity> GetMessageTempPageList(MessageTemplateQuery query);

        /// <summary>
        /// 获取邮件短信模版数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<MessageTemplateEntity> GetMessageTempList(MessageTemplateQuery query);

        /// <summary>
        /// 获取邮件短信模版实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        MessageTemplateEntity GetMessageTempModel(MessageTemplateQuery query);

        /// <summary>
        /// 保存邮件短信模版数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveMessageTemp(MessageTemplateEntity model);

        /// <summary>
        /// 删除邮件短信模版
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult DelMessageTemp(MessageTemplateQuery query);

        /// <summary>
        /// 获取模版类型键值对
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IDictionary<int, string> GetTCategoryDict(MessageTemplateQuery query);

        /// <summary>
        /// 获取模版类型键值对(去除已经存在模版的模版类型)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IDictionary<int, string> GetTCategoryDictChecked(MessageTemplateQuery query);

        /// <summary>
        /// 获取短信邮件全局变量
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetEmailVariable();

        /// <summary>
        /// 获取替换模版后的邮件短信内容
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="config"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        string GetEmailOrSmsContent(IDictionary<string, string> dict, string content);

        #region 发送邮件短信
        /// <summary>
        /// 保存邮件短信发送记录
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool SaveSendRecode(IList<MessageRecodeEntity> list);

        /// <summary>
        /// 获取发送记录分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<MessageRecodeEntity> GetMessageRecodePageList(MessageRecodeQuery query);

        /// <summary>
        /// 获取发送记录数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<MessageRecodeEntity> GetMessageRecodeList(MessageRecodeQuery query);

        /// <summary>
        /// 获取发送记录实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        MessageRecodeEntity GetMsgRecodeModel(MessageRecodeQuery query);

        /// <summary>
        /// 发送短信或邮件(不使用模版发送)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SendEmailOrSms(MessageRecodeEntity model);

        /// <summary>
        /// 发送短信或邮件(不使用模版发送)   选择作者信息
        /// </summary>
        /// <param name="ReciveUserList"></param>
        ///<param name="entity">(必填项 MsgType,JournalID,SendUser,MsgTitle,MsgContent)</param>      
        /// <returns></returns>
        ExecResult SendEmailOrSms(IList<Int64> ReciveUserList, MessageRecodeEntity entity, bool isRecode = true);

        /// <summary>
        /// 发送短信或邮件(不使用模版发送)  直接使用短信号码或邮件地址
        /// </summary>
        /// <param name="ReciveAddressList"></param>
        /// <param name="entity">(必填项 MsgType,JournalID,SendUser,MsgTitle,MsgContent)</param>
        /// <returns></returns>
        ExecResult SendEmailOrSms(IList<String> ReciveAddressList, MessageRecodeEntity entity, bool isRecode = true);

        /// <summary>
        /// 发送短信或邮件(使用模版)
        /// </summary>
        /// <param name="ReciveUserdict">子级键值对中必须包含 $稿件主键$ </param>
        /// <param name="entity">(必填项 MsgType,JournalID,SendType,SendUser)</param>      
        /// <returns></returns>
        ExecResult SendEmailOrSms(IDictionary<Int64, IDictionary<string, string>> ReciveUserdict, MessageRecodeEntity entity, bool isRecode = true);

        /// <summary>
        /// 只发送邮件(手动输入单个地址)
        /// </summary>
        /// <param name="MsgTitle">邮件标题</param>
        /// <param name="MsgContent">邮件内容</param>
        /// <param name="ReciveAddress">接收地址</param>
        /// <param name="SendMail">发件人(名称 不是邮件帐号)</param>
        /// <param name="JournalID">杂志ID</param>
        /// <returns></returns>
        ExecResult SendEmail(string MsgTitle, string MsgContent, string ReciveAddress,string SendMail, long JournalID);

        #endregion

        #endregion

        # region 留言相关

        /// <summary>
        /// 获取留言分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<GuestbookEntity> GetSiteGuestBookPageList(GuestbookQuery query);

        /// <summary>
        /// 保存留言
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveSiteGuestBook(GuestbookEntity model);

        # endregion

        # region 站点访问量

        /// <summary>
        /// 更新总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        bool UpdateSiteAccessCount(SiteConfigQuery query);

	    /// <summary>
        /// 获取总访问数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        int GetSiteAccessCount(SiteConfigQuery query);

        # endregion
    }
}
