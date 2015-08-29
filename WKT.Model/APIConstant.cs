using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model
{
    /// <summary>
    /// Api 常量
    /// </summary>
    public class APIConstant
    {
        # region 作者相关

        /// <summary>
        /// 用户登录
        /// </summary>
        public const string AUTHORLOGIN = "authorapi/login";

        /// <summary>
        /// 记录登录错误日志信息
        /// </summary>
        public const string ADDLOGINERRORLOG = "loginapi/AddLoginErrorLog";
        /// <summary>
        /// 获取登录错误日志信息
        /// </summary>
        public const string GETLOGINERRORLOGLIST = "loginapi/GetLoginErrorLogList";

        /// <summary>
        /// 删除登录错误日志信息
        /// </summary>
        public const string DELETELOGINERRORLOG = "loginapi/DeleteLoginErrorLog";

        /// <summary>
        /// 用户注册
        /// </summary>
        public const string AUTHORREG = "authorapi/reg";

        /// <summary>
        /// 新增令牌
        /// </summary>
        public const string AUTHOR_TOKEN_ADD = "authorapi/InsertToken";

        /// <summary>
        /// 获取令牌
        /// </summary>
        public const string AUTHOR_TOKEN_GET = "authorapi/GetToken";


        /// <summary>
        /// 用户修改密码
        /// </summary>
        public const string AUTHOREDITPWD = "authorapi/editpwd";

        /// <summary>
        /// 记录登录信息
        /// </summary>
        public const string AUTHORRECORDLOGININFO = "authorapi/RecordLoginInfo";

        /// <summary>
        /// 获取作者列表
        /// </summary>
        public const string AUTHOGETAUTHORINFOLIST = "authorapi/GetAuthorInfoList";

        /// <summary>
        /// 根据角色获取作者列表
        /// </summary>
        public const string AUTHOGETAUTHORINFOLISTBYROLE = "authorapi/GetAuthorInfoListByRole";

        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        public const string AUTHORGETROLELIST = "authorapi/GetAuthorRoleList";

        /// <summary>
        /// 设置作者到某个角色
        /// </summary>
        public const string AUTHORSETROLE = "authorapi/SetAurhoRole";

        /// <summary>
        /// 删除作者角色
        /// </summary>
        public const string AUTHORDELROLE = "authorapi/DelAurhoRole";

        /// <summary>
        /// 删除作者角色菜单例外
        /// </summary>
        public const string AUTHORSETMENURIGHTEXCEPTION = "authorapi/SetAuthorExceptionMenuRight";

        /// <summary>
        /// 编辑指定的编辑部成员信息
        /// </summary>
        public const string AUTHOREDITMEMEBERINFO = "authorapi/EditMember";

        /// <summary>
        /// 编辑指定的作者信息
        /// </summary>
        public const string AUTHOREDITAUTHORINFO = "authorapi/EditAuthor";

        /// <summary>
        /// 获取指定的编辑部成员信息
        /// </summary>
        public const string AUTHORGETMEMBERINFO = "authorapi/GetMemberInfo";

        /// <summary>
        /// 获取编辑部成员列表
        /// </summary>
        public const string AUTHORGETMEMBERINFOLIST = "authorapi/GetMemberInfoList";

        /// <summary>
        /// 获取专家列表
        /// </summary>
        public const string AUTHOR_GETEXPERTPAGELIST = "authorapi/GetExpertPageList";

        /// <summary>
        /// 获取编辑部成员键值对
        /// </summary>
        public const string AUTHOR_GETMEMBERDICT = "authorapi/GetMemberDict";

        /// <summary>
        /// 获取作者实体
        /// </summary>
        public const string AUTHOR_GETAUTHORINFO = "authorapi/GetAuthorInfo";

        # endregion

        # region 作者统计

        /// <summary>
        /// 作者数量统计
        /// </summary>
        public const string ATHOR_STAT_GETAUTHORCONTRIBUTE = "authorapi/GetAuthorContributeStat";

        /// <summary>
        /// 作者省份统计
        /// </summary>
        public const string ATHOR_STAT_GETAUTHORPROVINCE = "authorapi/GetAuthorProvinceStat";

        /// <summary>
        /// 作者学历统计
        /// </summary>
        public const string ATHOR_STAT_GETAUTHOREDUCATION = "authorapi/GetAuthorEducationStat";

        /// <summary>
        /// 作者专业统计
        /// </summary>
        public const string ATHOR_STAT_GETAUTHORPROFESSIONAL = "authorapi/GetAuthorProfessionalStat";

        /// <summary>
        /// 作者职称统计
        /// </summary>
        public const string ATHOR_STAT_GETAUTHORJOBTITLE = "authorapi/GetAuthorJobTitleStat";

        /// <summary>
        /// 作者性别统计
        /// </summary>
        public const string ATHOR_STAT_GETAUTHORGENDER = "authorapi/GetAuthorGenderStat";

        /// <summary>
        /// 编辑、专家工作量统计
        /// </summary>
        public const string ATHOR_STAT_WORKLOAD = "authorapi/GetWorkloadList";

        /// <summary>
        /// 获取编辑、专家处理稿件明细
        /// </summary>
        public const string AUOR_STAT_GETDEALCONTRIBUTIONDETAIL = "authorapi/GetDealContributionDetail";


        # endregion

        # region 系统相关

        # region menu

        /// <summary>
        /// 获取系统菜单
        /// </summary>
        public const string SYSGETSYSMENUAJAX = "systemapi/GetMenuList";

        /// <summary>
        /// 获取有权限的系统菜单
        /// </summary>
        public const string SYSGETHAVERIGHTMENULIST = "systemapi/GetHaveRightMenu";

        /// <summary>
        /// 获取拥有权限的菜单ID列表
        /// </summary>
        public const string SYSGETTREENODELISTHAVERIGHT = "systemapi/GetTreeNodeListHaveRight";

        /// <summary>
        /// 获取作者例外菜单ID列表
        /// </summary>
        public const string SYSGETAUTHOREXCEPTIONRIGHTMENU = "systemapi/GetAuthorExceptionMenu";

        /// <summary>
        /// 获取指定的Menu
        /// </summary>
        public const string SYSGETMENUENTITY = "systemapi/GetMenu";

        /// <summary>
        /// 更新菜单状态
        /// </summary>
        public const string SYSUPDATEMENUSTATUS = "systemapi/UpdateMenuStatus";

        /// <summary>
        /// 菜单角色赋权
        /// </summary>
        public const string SYSSETMENUROLERIGHT = "systemapi/SetMenuRight";

        /// <summary>
        /// 是否有当前地址的访问权限
        /// </summary>
        public const string SYSISHAVEMENURIGHT = "systemapi/IsHaveMenuRight";

        /// <summary>
        /// 是否有当前地址的访问权限，根据成员分组
        /// </summary>
        public const string SYS_ISHAVEACCESSRIGHTBYGROUP= "systemapi/IsHaveAccessRightByGroup";

        # endregion

        # region role

        /// <summary>
        /// 获取角色列表
        /// </summary>
        public const string SYSGETROLELIST = "systemapi/GetRoleList";

        /// <summary>
        /// 获取角色实体
        /// </summary>
        public const string SYSGETROLEENTITY = "systemapi/GetRoleEntity";

        /// <summary>
        /// 更新角色信息
        /// </summary>
        public const string SYSUPDATEROLEINFO = "systemapi/UpdateRoleInfo";

        /// <summary>
        /// 新增角色信息
        /// </summary>
        public const string SYSADDROLEINFO = "systemapi/AddRole";

        /// <summary>
        /// 删除角色信息
        /// </summary>
        public const string SYSDELROLEINFO = "systemapi/DelRole";

        public const string SYSGETROLEINFODICT = "systemapi/GetRoleInfoDict";

        # endregion

        #region roleAuthor
        public const string GETROLEAUTHOR           = "RoleAuthorAPI/GetRoleAuthor";
        public const string GETROLEAUTHORLIST       = "RoleAuthorAPI/GetRoleAuthorList";
        public const string GETROLEAUTHORDETAILLIST = "RoleAuthorAPI/GetRoleAuthorDetailList";
        public const string GETROLEAUTHORPAGELIST   = "RoleAuthorAPI/GetRoleAuthorPageList";
        public const string ADDROLEAUTHOR           = "RoleAuthorAPI/AddRoleAuthor";
        public const string UPDATEROLEAUTHOR        = "RoleAuthorAPI/UpdateRoleAuthor";
        public const string DELETEROLEAUTHOR        = "RoleAuthorAPI/DeleteRoleAuthor";
        public const string BATCHDELETEROLEAUTHOR   = "RoleAuthorAPI/BatchDeleteRoleAuthor";
        #endregion

        # region member

        /// <summary>
        /// 修改编辑部成员信息
        /// </summary>
        public const string SYSEDITMEMBERINFO = "AuthorAPI/EditMember";

        /// <summary>
        /// 删除编辑部成员信息
        /// </summary>
        public const string SYSDELMEMBERINFO = "AuthorAPI/DeleteMember";

        # endregion

        # endregion

        # region 稿件相关

        /// <summary>
        /// 稿件编号设置
        /// </summary>
        public const string CONTRIBUTIONNUMBERSET = "ContributionAPI/SetContruibuteNumberFormat";

        /// <summary>
        /// 投稿公告设置
        /// </summary>
        public const string CONTRIBUTIOSTATEMENTSET = "ContributionAPI/SetContruibuteStatement";

        /// <summary>
        /// 获取稿件设置
        /// </summary>
        public const string CONTRIBUTIOGETINFO = "ContributionAPI/GetContributeSetInfo";

        /// <summary>
        /// 获取稿件附件
        /// </summary>
        public const string C_GETCONTRIBUTIONATTACHMENT = "ContributionInfoAPI/GetContributionAttachment";

        /// <summary>
        /// 更新介绍信
        /// </summary>
        public const string C_UPDATEINTROLETTER = "ContributionInfoAPI/UpdateIntroLetter";

        /// <summary>
        /// 处理撤稿申请
        /// </summary>
        public const string C_DEALWITHDRAWAL = "ContributionInfoAPI/DealWithdrawal";

        /// <summary>
        /// 撤销删除
        /// </summary>
        public const string C_CANCELDELETE = "ContributionInfoAPI/CancelDelete";

        /// <summary>
        /// 获取稿件作者
        /// </summary>
        public const string C_GETCONTRIBUTIONAUTHORINFO = "ContributionInfoAPI/GetContributionAuthorInfo";

        /// <summary>
        /// 获取稿件作者详细信息数据
        /// </summary>
        public const string C_GETCONTRIBUTIONAUTHORLIST = "ContributionInfoAPI/GetContributionAuthorList";

        # region 投稿自动分配

        /// <summary>
        /// 得到投稿自动分配设置
        /// </summary>
        public const string CONTRIBUTION_GETAUTOALLOTINFO = "ContributionAPI/GetContributeAutoAllotInfo";

        /// <summary>
        /// 投稿自动分配设置
        /// </summary>
        public const string CONTRIBUTION_SETALLOWALLOT = "ContributionAPI/SetContruibuteAllowAllot";

        /// <summary>
        /// 获取投稿自动分配编辑
        /// </summary>
        public const string CONTRIBUTION_GETAUTOALLOTEDITOR = "ContributionAPI/GetAutoAllotEditor";

        # endregion

        # region 稿件处理专区

        /// <summary>
        /// 设置稿件旗帜标记
        /// </summary>
        public const string CONTRIBUTION_SETCONTRIBUTEFLAG = "ContributionAPI/SetContributeFlag";
        
        /// <summary>
        /// 设置稿件旗帜标记
        /// </summary>
        public const string CONTRIBUTION_SETCONTRIBUTEQUICK = "ContributionAPI/SetContributeQuick";

        /// <summary>
        /// 删除稿件
        /// </summary>
        public const string CONTRIBUTION_DELETECONTRIBUTE = "ContributionAPI/DeleteContribute";

        /// <summary>
        /// 设置稿件责任编辑
        /// </summary>
        public const string CONTRIBUTION_SETCONTRIBUTEEDITOR = "ContributionAPI/SetContributeEditor";

        # endregion

        # endregion

        #region 站点信息配置相关
        /// <summary>
        /// 获取站点信息实体
        /// </summary>
        public const string GETSITECONFIGMODELAJAX = "SiteConfigAPI/GetSiteConfigModel";

        /// <summary>
        /// 修改站点信息
        /// </summary>
        public const string UPDATESITECONFIGAJAX = "SiteConfigAPI/UpdateSiteConfig";

        /// <summary>
        /// 更新站点访问量
        /// </summary>
        public const string SITECONFIG_UPDATESITEACCESSCOUNT = "SiteConfigAPI/UpdateSiteAccessCount";

        /// <summary>
        /// 获取站点访问量
        /// </summary>
        public const string SITECONFIG_GETSITEACCESSCOUNT = "SiteConfigAPI/GetSiteAccessCount";

        #endregion

        #region 数据字典相关
        /// <summary>
        /// 获取数据字典分页数据
        /// </summary>
        public const string DICT_GETPAGELIST = "DictAPI/GetDictPageList";

        /// <summary>
        /// 获取数据字典实体
        /// </summary>
        public const string DICT_GETMODEL = "DictAPI/GetDictModel";

        /// <summary>
        /// 获取数据字典实体
        /// </summary>
        public const string DICT_GETMODELBYKEY = "DictAPI/GetDictModelByKey";

        /// <summary>
        /// 保存数据字典
        /// </summary>
        public const string DICT_SAVE = "DictAPI/SaveDict";

        /// <summary>
        /// 删除数据字典
        /// </summary>
        public const string DICT_DELETE = "DictAPI/DelDict";

        /// <summary>
        /// 获取数据字典值分页数据
        /// </summary>
        public const string DICTVALUE_GETPAGELIST = "DictAPI/GetDictValuePageList";

        /// <summary>
        /// 获取数据字典值实体
        /// </summary>
        public const string DICTVALUE_GETMODEL = "DictAPI/GetDictValueModel";

        /// <summary>
        /// 保存数据字典值
        /// </summary>
        public const string DICTVALUE_SAVE = "DictAPI/SaveDictValue";

        /// <summary>
        /// 删除数据字典值
        /// </summary>
        public const string DICTVALUE_DELETE = "DictAPI/DelDictValue";

        /// <summary>
        /// 获取数据字典值键值对
        /// </summary>
        public const string DICTVALUE_DICT = "DictAPI/GetDictValueDcit";

        public const string DICTVALUE_DICTLIST = "DictAPI/GetDictValueList";
        #endregion

        #region 栏目相关
        /// <summary>
        /// 获取栏目列表
        /// </summary>
        public const string SITECHANNEL_GETLIST = "SiteChannelAPI/GetList";

        /// <summary>
        /// 获取栏目实体
        /// </summary>
        public const string SITECHANNEL_GETMODEL = "SiteChannelAPI/GetModel";

        /// <summary>
        /// 保存栏目
        /// </summary>
        public const string SITECHANNEL_SAVE = "SiteChannelAPI/Save";

        /// <summary>
        /// 删除栏目
        /// </summary>
        public const string SITECHANNEL_DEL = "SiteChannelAPI/Del";
        #endregion

        #region 联系人相关
        /// <summary>
        /// 获取联系人分页列表
        /// </summary>
        public const string ContactWay_GETPAGELIST = "ContantWayAPI/GetPageList";

        /// <summary>
        /// 获取联系人列表
        /// </summary>
        public const string ContactWay_GETLIST = "ContantWayAPI/GetList";

        /// <summary>
        /// 获取联系人实体
        /// </summary>
        public const string ContactWay_GETMODEL = "ContantWayAPI/GetModel";

        /// <summary>
        /// 保存联系人
        /// </summary>
        public const string ContactWay_SAVE = "ContantWayAPI/Save";

        /// <summary>
        /// 删除联系人
        /// </summary>
        public const string ContactWay_DEL = "ContantWayAPI/Del";
        
        #endregion

        #region 站点公告相关
        /// <summary>
        /// 获取站点公告分页列表
        /// </summary>
        public const string SITENOTICE_GETPAGELIST = "SiteNoticeAPI/GetPageList";

        /// <summary>
        /// 获取站点公告列表
        /// </summary>
        public const string SITENOTICE__GETLIST = "SiteNoticeAPI/GetList";

        /// <summary>
        /// 获取站点公告实体
        /// </summary>
        public const string SITENOTICE__GETMODEL = "SiteNoticeAPI/GetModel";

        /// <summary>
        /// 保存站点公告
        /// </summary>
        public const string SITENOTICE__SAVE = "SiteNoticeAPI/Save";

        /// <summary>
        /// 删除站点公告
        /// </summary>
        public const string SITENOTICE__DEL = "SiteNoticeAPI/Del";

        #endregion

        #region 友情链接相关
        /// <summary>
        /// 获取友情链接分页列表
        /// </summary>
        public const string FRIENDLYLINK_GETPAGELIST = "FriendlyLinkAPI/GetPageList";

        /// <summary>
        /// 获取友情链接列表
        /// </summary>
        public const string FRIENDLYLINK_GETLIST = "FriendlyLinkAPI/GetList";

        /// <summary>
        /// 获取友情链接实体
        /// </summary>
        public const string FRIENDLYLINK_GETMODEL = "FriendlyLinkAPI/GetModel";

        /// <summary>
        /// 保存友情链接
        /// </summary>
        public const string FRIENDLYLINK_SAVE = "FriendlyLinkAPI/Save";

        /// <summary>
        /// 删除友情链接
        /// </summary>
        public const string FRIENDLYLINK_DEL = "FriendlyLinkAPI/Del";

        #endregion

        #region 新闻资讯相关
        /// <summary>
        /// 获取新闻资讯分页列表
        /// </summary>
        public const string SITECONTENT_GETPAGELIST = "SiteContentAPI/GetPageList";

        /// <summary>
        /// 获取新闻资讯列表
        /// </summary>
        public const string SITECONTENT_GETLIST = "SiteContentAPI/GetList";

        /// <summary>
        /// 获取新闻资讯实体
        /// </summary>
        public const string SITECONTENT_GETMODEL = "SiteContentAPI/GetModel";

        /// <summary>
        /// 保存新闻资讯
        /// </summary>
        public const string SITECONTENT_SAVE = "SiteContentAPI/Save";

        /// <summary>
        /// 删除新闻资讯
        /// </summary>
        public const string SITECONTENT_DEL = "SiteContentAPI/Del";

        #endregion

        #region 资源文件相关
        /// <summary>
        /// 获取资源文件分页列表
        /// </summary>
        public const string SITERESOURCE_GETPAGELIST = "SiteResourceAPI/GetPageList";

        /// <summary>
        /// 获取资源文件列表
        /// </summary>
        public const string SITERESOURCE_GETLIST = "SiteResourceAPI/GetList";

        /// <summary>
        /// 获取资源文件实体
        /// </summary>
        public const string SITERESOURCE_GETMODEL = "SiteResourceAPI/GetModel";

        /// <summary>
        /// 保存资源文件
        /// </summary>
        public const string SITERESOURCE_SAVE = "SiteResourceAPI/Save";

        /// <summary>
        /// 累加资源文件下载次数
        /// </summary>
        public const string SITERESOURCE_DOWNLOADCOUNT = "SiteResourceAPI/DownloadCount";

        /// <summary>
        /// 删除资源文件
        /// </summary>
        public const string SITERESOURCE_DEL = "SiteResourceAPI/Del";

        #endregion

        #region 内容块相关
        /// <summary>
        /// 获取内容块分页列表
        /// </summary>
        public const string SITEBLOCK_GETPAGELIST = "SiteBlockAPI/GetPageList";

        /// <summary>
        /// 获取内容块列表
        /// </summary>
        public const string SITEBLOCK_GETLIST = "SiteBlockAPI/GetList";

        /// <summary>
        /// 获取内容块实体
        /// </summary>
        public const string SITEBLOCK_GETMODEL = "SiteBlockAPI/GetModel";

        /// <summary>
        /// 保存内容块
        /// </summary>
        public const string SITEBLOCK_SAVE = "SiteBlockAPI/Save";

        /// <summary>
        /// 删除内容块
        /// </summary>
        public const string SITEBLOCK_DEL = "SiteBlockAPI/Del";

        #endregion

        #region 站内消息相关
        /// <summary>
        /// 获取站内消息分页列表
        /// </summary>
        public const string SITEMESSAGE_GETPAGELIST = "SiteMessageAPI/GetPageList";

        /// <summary>
        /// 获取站内消息列表
        /// </summary>
        public const string SITEMESSAGE_GETLIST = "SiteMessageAPI/GetList";

        /// <summary>
        /// 获取站内消息实体
        /// </summary>
        public const string SITEMESSAGE_GETMODEL = "SiteMessageAPI/GetModel";

        /// <summary>
        /// 保存站内消息
        /// </summary>
        public const string SITEMESSAGE_SAVE = "SiteMessageAPI/Save";

        /// <summary>
        /// 删除站内消息
        /// </summary>
        public const string SITEMESSAGE_DEL = "SiteMessageAPI/Del";

        /// <summary>
        /// 阅读站内消息
        /// </summary>
        public const string SITEMESSAGE_VIEWED = "SiteMessageAPI/Viewed";

        #endregion

        #region 邮件短信相关
        /// <summary>
        /// 获取邮件短信模版分页列表
        /// </summary>
        public const string MESSAGETEMP_GETPAGELIST = "MessageTemplateAPI/GetPageList";

        /// <summary>
        /// 获取邮件短信模版列表
        /// </summary>
        public const string MESSAGETEMP_GETLIST = "MessageTemplateAPI/GetList";

        /// <summary>
        /// 获取邮件短信模版实体
        /// </summary>
        public const string MESSAGETEMP_GETMODEL = "MessageTemplateAPI/GetModel";

        /// <summary>
        /// 保存邮件短信模版
        /// </summary>
        public const string MESSAGETEMP_SAVE = "MessageTemplateAPI/Save";

        /// <summary>
        /// 删除邮件短信模版
        /// </summary>
        public const string MESSAGETEMP_DEL = "MessageTemplateAPI/Del";

        /// <summary>
        /// 获取模版类型键值对
        /// </summary>
        public const string MESSAGETEMP_TCATEGORYDICT = "MessageTemplateAPI/GetTCategoryDict";

        /// <summary>
        /// 获取模版类型键值对(去除已经存在模版的模版类型)
        /// </summary>
        public const string MESSAGETEMP_TCATEGORYDICTCHECKED = "MessageTemplateAPI/GetTCategoryDictChecked";

        /// <summary>
        /// 保存发送记录
        /// </summary>
        public const string MSGRECODE_SAVE = "MessageTemplateAPI/SaveSendRecode";

        /// <summary>
        /// 获取发送分页记录
        /// </summary>
        public const string MSGRECODE_GETPAGELIST = "MessageTemplateAPI/GetMsgRecodePageList";

        /// <summary>
        /// 获取发送记录
        /// </summary>
        public const string MSGRECODE_GETLIST = "MessageTemplateAPI/GetMsgRecodeList";

        /// <summary>
        /// 获取发送记录实体
        /// </summary>
        public const string MSGRECODE_GETMSGRECODEMODEL = "MessageTemplateAPI/GetMsgRecodeModel";

        #endregion

        #region 作者详细信息相关
        /// <summary>
        /// 获取作者详细信息分页列表
        /// </summary>
        public const string AUTHORDETAIL_GETPAGELIST = "AuthorDetailAPI/GetPageList";

        /// <summary>
        /// 获取作者详细信息列表
        /// </summary>
        public const string AUTHORDETAIL_GETLIST = "AuthorDetailAPI/GetList";

        /// <summary>
        /// 获取作者详细信息实体
        /// </summary>
        public const string AUTHORDETAIL_GETMODEL = "AuthorDetailAPI/GetModel";

        /// <summary>
        /// 保存作者详细信息
        /// </summary>
        public const string AUTHORDETAIL_SAVE = "AuthorDetailAPI/Save";

        /// <summary>
        /// 删除作者详细信息
        /// </summary>
        public const string AUTHORDETAIL_DEL = "AuthorDetailAPI/Del";

        /// <summary>
        /// 设置为专家信息
        /// </summary>
        public const string AUTHOR_SETEXPERT = "AuthorDetailAPI/SetExpert";

        /// <summary>
        /// 取消作者为专家
        /// </summary>
        public const string AUTHOR_CANCELEXPERT = "AuthorDetailAPI/CancelExpert";

        /// <summary>
        /// 获取专家分组信息
        /// </summary>
        public const string AUTHOR_GETEXPERTGROUPMAPLIST = "AuthorDetailAPI/GetExpertGroupMapList";

        /// <summary>
        /// 保存专家分组信息
        /// </summary>
        public const string AUTHOR_SAVEEXPERTGROUPMAP = "AuthorDetailAPI/SaveExpertGroupMap";

        #endregion

        #region 投稿相关
        /// <summary>
        /// 获取稿件分页列表
        /// </summary>
        public const string CONTRIBUTIONINFO_GETPAGELIST = "ContributionInfoAPI/GetPageList";

        /// <summary>
        /// 获取稿件列表
        /// </summary>
        public const string CONTRIBUTIONINFO_GETLIST = "ContributionInfoAPI/GetList";

        /// <summary>
        /// 获取稿件实体
        /// </summary>
        public const string CONTRIBUTIONINFO_GETMODEL = "ContributionInfoAPI/GetModel";

        /// <summary>
        /// 投稿
        /// </summary>
        public const string CONTRIBUTIONINFO_SAVE = "ContributionInfoAPI/Submission";

        /// <summary>
        /// 保存稿件格式修改
        /// </summary>
        public const string CONTRIBUTIONINFO_SAVEFORMAT = "ContributionInfoAPI/SaveFormat";

        /// <summary>
        /// 删除稿件
        /// </summary>
        public const string CONTRIBUTIONINFO_DEL = "ContributionInfoAPI/Del";

        /// <summary>
        /// 更改稿件状态
        /// </summary>
        public const string CONTRIBUTIONINFO_CHANGESTATUS = "ContributionInfoAPI/ChangeStatus";

        /// <summary>
        /// 保存稿件备注
        /// </summary>
        public const string CREMARK_SAVE = "ContributionInfoAPI/SaveCRemark";

        /// <summary>
        /// 获取稿件备注实体
        /// </summary>
        public const string CREMARK_GETMODEL = "ContributionInfoAPI/GetCRemarkModel";

        #endregion

        #region 撤稿相关
        /// <summary>
        /// 撤稿
        /// </summary>
        public const string DRAFT_DRAFT = "ContributionInfoAPI/Draft";

        /// <summary>
        /// 新增撤稿表信息
        /// </summary>
        public const string DRAFT_ADD = "ContributionInfoAPI/AddRetractionsBills";

        /// <summary>
        /// 编辑撤稿表信息
        /// </summary>
        public const string DRAFT_UPDATE = "ContributionInfoAPI/UpdateRetractionsBills";

        /// <summary>
        /// 获取撤稿信息实体
        /// </summary>
        public const string DRAFT_GETMODEL = "ContributionInfoAPI/GetRetractionsBillsModel";
        #endregion

        # region 流程相关

        # region 审稿环节配置

        /// <summary>
        /// 获取拥有权限的审稿状态
        /// </summary>
        public const string FLOW_GETHAVERIGHTFLOWSTATUS = "FlowApi/GetHaveRightFlowStatus";

        /// <summary>
        /// 获取拥有权限的审稿状态(用于统计同一稿件一个状态下送多人时按一个计算)
        /// </summary>
        public const string FLOW_GETHAVERIGHTFLOWSTATUSFORSTAT = "FlowApi/GetHaveRightFlowStatusForStat";

        /// <summary>
        /// 获取审稿状态键值对，审稿状态名称
        /// </summary>
        public const string FLOW_GETFLOWSTATUSDICTSTATUSNAME = "FlowApi/GetFlowStatusDictStatusName";

        /// <summary>
        /// 获取审稿状态键值对，审稿状态显示名称
        /// </summary>
        public const string FLOW_GETFLOWSTATUSDICTDISPLAYNAME = "FlowApi/GetFlowStatusDictDisplayName";

        /// <summary>
        /// 获取审稿状态列表
        /// </summary>
        public const string FLOW_GETFLOWSTATUSLIST= "FlowApi/GetFlowStatusList";

        /// <summary>
        /// 获取审稿状态序号
        /// </summary>
        public const string FLOW_GETFLOWSTATUSSORTID = "FlowApi/GetFlowStatusSortID";

        /// <summary>
        /// 判断审稿状态对应的稿件状态是否存在
        /// </summary>
        public const string FLOW_CHECKCSTATUSISEXISTS = "FlowApi/CheckCStatusIsExists";

        /// <summary>
        /// 根据指定的审稿状态ID，得到审稿状态的基本信息
        /// </summary>
        public const string FLOW_GETFLOWSTATUSINFOBYID = "FlowApi/GetFlowStatusInfoByID";

        /// <summary>
        /// 获取审稿步骤基本信息及配置信息
        /// </summary>
        public const string FLOW_GETFLOWSTEPINFO = "FlowApi/GetFlowStepInfo";

        /// <summary>
        /// 新增审稿状态
        /// </summary>
        public const string FLOW_ADDFLOWSTATUS = "FlowApi/AddFlowStatus";

        /// <summary>
        /// 修改审稿状态
        /// </summary>
        public const string FLOW_EDITFLOWSTATUS = "FlowApi/EditFlowStatus";

        /// <summary>
        /// 删除审稿状态
        /// </summary>
        public const string FLOW_DELFLOWSTATUS = "FlowApi/DelFlowStatus";

        /// <summary>
        /// 删除审稿状态
        /// </summary>
        public const string FLOW_UPDATEFLOWSTATUS = "FlowApi/UpdateFlowStatus";

        # endregion

        # region 审稿操作设置

        /// <summary>
        /// 获取操作实体
        /// </summary>
        public const string FLOW_GETFLOWACTIONENTITY = "FlowApi/GetFlowActionEntity";

        /// <summary>
        /// 根据当前操作状态获取可以做的操作
        /// </summary>
        public const string FLOW_GETFLOWACTIONBYSTATUS = "FlowApi/GetFlowActionByStatus";

        /// <summary>
        /// 获取审稿操作设置
        /// </summary>
        public const string FLOW_GETFLOWACTIONLIST = "FlowApi/GetFlowActionList";

        /// <summary>
        /// 新增审稿操作
        /// </summary>
        public const string FLOW_ADDFLOWACTION = "FlowApi/AddFlowAction";

        /// <summary>
        /// 修改审稿操作
        /// </summary>
        public const string FLOW_EDITFLOWACTION = "FlowApi/EditFlowAction";

        /// <summary>
        /// 删除审稿操作
        /// </summary>
        public const string FLOW_DELFLOWACTION = "FlowApi/DelFlowAction";

        # endregion

        # region 审稿环节权限配置

        /// <summary>
        /// 获取审稿环节作者权限设置
        /// </summary>
        public const string FLOW_GETFLOWAUTHAUTHORLIST = "FlowApi/GetFlowAuthAuthorList";

        /// <summary>
        /// 设置审稿环节作者权限
        /// </summary>
        public const string FLOW_SETFLOWAUTHAUTHOR = "FlowApi/SetFlowAuthAuthor";

        /// <summary>
        /// 删除审稿环节作者权限
        /// </summary>
        public const string FLOW_DELETEFLOWAUTHAUTHOR = "FlowApi/DeleteFlowAuthAuthor";

        /// <summary>
        /// 获取审稿环节角色权限设置
        /// </summary>
        public const string FLOW_GETFLOWAUTHROLELIST = "FlowApi/GetFlowAuthRoleList";

        /// <summary>
        /// 设置审稿环节角色权限
        /// </summary>
        public const string FLOW_SETFLOWAUTHROLE = "FlowApi/SetFlowAuthRole";

        /// <summary>
        /// 删除审稿环节角色权限
        /// </summary>
        public const string FLOW_DELETEFLOWAUTHROLE = "FlowApi/DeleteFlowAuthRole";

        # endregion

        # region 审稿环节条件设置

        # endregion

        # region 流程流转

        /// <summary>
        /// 获取当前环节的稿件列表
        /// </summary>
        public const string FLOW_GETFLOWCONTRIBUTIONLIST = "FlowApi/GetFlowContributionList";

        /// <summary>
        /// 获取作者最新稿件状态稿件列表
        /// </summary>
        public const string FLOW_GETAUTHORCONTRIBUTIONLIST = "FlowApi/GetAuthorContributionList";

        /// <summary>
        /// 获取专家待审、已审稿件列表
        /// </summary>
        public const string FLOW_GETEXPERTCONTRIBUTIONLIST = "FlowApi/GetExpertContributionList";

        /// <summary>
        /// 获取同步状态稿件列表，例如：专家拒审，作者退修、已发校样、录用、退稿稿件列表
        /// </summary>
        public const string FLOW_GETSYNCHROSTATUSCONTRIBUTIONLIST = "FlowApi/GetSynchroStatusContributionList";

        /// <summary>
        /// 获取下一环节信息
        /// </summary>
        public const string FLOW_GETNEXTFLOWSTEP = "FlowApi/GetNextFlowStep";

        /// <summary>
        /// 得到稿件的处理人
        /// </summary>
        public const string FLOW_GETCONTRIBUTIONPROCESSER = "FlowApi/GetContributionProcesser";

        /// <summary>
        /// 提交审稿单
        /// </summary>
        public const string FLOW_SUBMITAUDITBILL = "FlowApi/SubmitAuditBill";

        /// <summary>
        /// 提取流程日志
        /// </summary>
        public const string FLOW_GETFLOWLOGENTITY = "FlowApi/GetFlowLogEntity";

        /// <summary>
        /// 提取流程日志
        /// </summary>
        public const string FLOW_GETFLOWLOG = "FlowApi/GetFlowLog";

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        public const string FLOW_GETFLOWLOGATTACHMENT = "FlowApi/GetFlowLogAttachment";

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        public const string FLOW_GETFLOWLOGATTALLACHMENT = "FlowApi/GetFlowLogAllAttachment";

        /// <summary>
        /// 更新日志的查看状态
        /// </summary>
        public const string FLOW_UPDATEFLOWLOGISVIEW = "FlowApi/UpdateFlowLogIsView";

        /// <summary>
        /// 更新日志的下载状态
        /// </summary>
        public const string FLOW_UPDATEFLOWLOGISDOWN = "FlowApi/UpdateFlowLogIsDown";

        /// <summary>
        /// 专家拒审
        /// </summary>
        public const string FLOW_EXPERTDELEDIT = "FlowApi/ExpertDeledit";

        /// <summary>
        /// 处理在入款时改变稿件状态
        /// </summary>
        public const string DealFinaceInAccount = "FlowAPI/DealFinaceInAccount";

        /// <summary>
        /// 查看该稿件是否存在多个状态
        /// </summary>
        public const string FLOW_JUDGEISMORESTATUS = "FlowApi/JudgeIsMoreStatus";

        /// <summary>
        /// 合并多状态
        /// </summary>
        public const string FLOW_MERGEMORESTATUS = "FlowApi/MergeMoreStatus";

        /// <summary>
        /// 继续送交s
        /// </summary>
        public const string FLOW_CONTINUSEND = "FlowApi/ContinuSend";

        /// <summary>
        /// 根据当前所处环节获取稿件列表
        /// </summary>
        public const string FLOW_GETCONTRIBTIONMORESTATUSLIST = "FlowApi/GetContributionMoreStatusList";

        # endregion

        #region 审稿单相关
        /// <summary>
        /// 获取审稿单项分页列表
        /// </summary>
        public const string REVIEWBILL_GETPAGELIST = "FlowAPI/GetReviewBillPageList";

        /// <summary>
        /// 获取审稿单项列表
        /// </summary>
        public const string REVIEWBILL_GETLIST = "FlowAPI/GetReviewBillList";

        /// <summary>
        /// 获取审稿单项实体
        /// </summary>
        public const string REVIEWBILL_GETMODEL = "FlowAPI/GetReviewBilllModel";

        /// <summary>
        /// 保存审稿单项
        /// </summary>
        public const string REVIEWBILL_SAVE = "FlowAPI/SaveReviewBill";

        /// <summary>
        /// 删除审稿单项
        /// </summary>
        public const string REVIEWBILL_DEL = "FlowAPI/DelReviewBill";

        /// <summary>
        /// 审稿单项是否已经使用
        /// </summary>
        public const string REVIEWBILL_ISENABLED = "FlowAPI/ReviewBillIsEnabled";

        /// <summary>
        /// 保存审稿单
        /// </summary>
        public const string REVIEWBILLCONTENT_SAVE = "FlowAPI/SaveReviewBillContent";

        /// <summary>
        /// 获取审稿单项列表
        /// </summary>
        public const string REVIEWBILLCONTENT_GETLIST = "FlowAPI/GetReviewBillContentList";

        /// <summary>
        /// 获取审稿单初始化项
        /// </summary>
        public const string REVIEWBILLCONTENT_GETLISTBYCID = "FlowAPI/GetReviewBillContentListByCID";
        #endregion

        # endregion

        #region 财务相关

        #region 稿件费用相关
        /// <summary>
        /// 获取稿件费用分页列表
        /// </summary>
        public const string FINANCECONTRIBUTE_GETPAGELIST = "FinanceContributeAPI/GetPageList";

        /// <summary>
        /// 获取稿件费用列表
        /// </summary>
        public const string FINANCECONTRIBUTE_GETLIST = "FinanceContributeAPI/GetList";

        /// <summary>
        /// 获取稿件费用实体
        /// </summary>
        public const string FINANCECONTRIBUTE_GETMODEL = "FinanceContributeAPI/GetModel";

        /// <summary>
        /// 保存稿件费用
        /// </summary>
        public const string FINANCECONTRIBUTE_SAVE = "FinanceContributeAPI/Save";

        /// <summary>
        /// 删除稿件费用
        /// </summary>
        public const string FINANCECONTRIBUTE_DEL = "FinanceContributeAPI/Del";

        /// <summary>
        /// 获取财务入款通知分页列表
        /// </summary>
        public const string FINANCEACCOUNT_GETPAGELIST = "FinanceContributeAPI/GetFinanceAccountPageList";

        /// <summary>
        /// 获取财务出款通知分页列表
        /// </summary>
        public const string FINANCEOUTACCOUNT_GETPAGELIST = "FinanceContributeAPI/GetFinanceOutAccountPageList";

        /// <summary>
        /// 获取财务统计一览表分页数据
        /// </summary>
        public const string FINANCEGLANCE_GETPAGELIST = "FinanceContributeAPI/GetFinanceGlancePageList";

        /// <summary>
        /// 获取稿费统计一览表分页数据
        /// </summary>
        public const string FINANCEFAOFEE_GETPAGELIST = "FinanceContributeAPI/GetFinanceGaoFeePageList";

        /// <summary>
        /// 获取版面费报表分页数据
        /// </summary>
        public const string FINANCEGLANCE_GETPAGEFEEREPORTPAGELIST = "FinanceContributeAPI/GetFinancePageFeeReportPageList";

        #endregion

        #region 缴费通知相关
        /// <summary>
        /// 获取缴费通知分页列表
        /// </summary>
        public const string PAYNOTICE_GETPAGELIST = "PayNoticeAPI/GetPageList";

        /// <summary>
        /// 获取缴费通知列表
        /// </summary>
        public const string PAYNOTICE_GETLIST = "PayNoticeAPI/GetList";

        /// <summary>
        /// 获取缴费通知实体
        /// </summary>
        public const string PAYNOTICE_GETMODEL = "PayNoticeAPI/GetModel";

        /// <summary>
        /// 保存缴费通知
        /// </summary>
        public const string PAYNOTICE_SAVE = "PayNoticeAPI/Save";

        /// <summary>
        /// 保存缴费通知
        /// </summary>
        public const string PAYNOTICE_BATCHSAVE = "PayNoticeAPI/BatchSave";

        /// <summary>
        /// 删除缴费通知
        /// </summary>
        public const string PAYNOTICE_DEL = "PayNoticeAPI/Del";

        /// <summary>
        /// 更新缴费通知状态
        /// </summary>
        public const string PAYNOTICE_CHANGESTATUS = "PayNoticeAPI/ChangeStatus";
        #endregion

        #region 支付记录相关
        /// <summary>
        /// 新增支付记录
        /// </summary>
        public const string FINANCEPAYDETAIL_ADD = "FinancePayDetailAPI/Add";
        #endregion
        #endregion

        #region 期刊相关

        #region 年卷设置
         /// <summary>
        /// 获取年卷设置分页列表
        /// </summary>
        public const string YEARVOLUME_GETPAGELIST = "IssueAPI/GetYearVolumePageList";

        /// <summary>
        /// 获取年卷设置列表
        /// </summary>
        public const string YEARVOLUME_GETLIST = "IssueAPI/GetYearVolumeList";

        /// <summary>
        /// 获取年卷设置实体
        /// </summary>
        public const string YEARVOLUME_GETMODEL = "IssueAPI/GetYearVolumeModel";

        /// <summary>
        /// 保存年卷设置
        /// </summary>
        public const string YEARVOLUME_SAVE = "IssueAPI/SaveYearVolume";

        /// <summary>
        /// 删除年卷设置
        /// </summary>
        public const string YEARVOLUME_DEL = "IssueAPI/DelYearVolume";
        #endregion

        #region 期设置
        /// <summary>
        /// 获取期设置分页列表
        /// </summary>
        public const string ISSUESET_GETPAGELIST = "IssueAPI/GetIssueSetPageList";

        /// <summary>
        /// 获取期设置列表
        /// </summary>
        public const string ISSUESET_GETLIST = "IssueAPI/GetIssueSetList";

        /// <summary>
        /// 获取期设置实体
        /// </summary>
        public const string ISSUESET_GETMODEL = "IssueAPI/GetIssueSetModel";

        /// <summary>
        /// 保存期设置
        /// </summary>
        public const string ISSUESET_SAVE = "IssueAPI/SaveIssueSet";

        /// <summary>
        /// 删除期设置
        /// </summary>
        public const string ISSUESET_DEL = "IssueAPI/DelIssueSet";
        #endregion

        #region 期刊栏目
        /// <summary>
        /// 获取期刊栏目分页列表
        /// </summary>
        public const string JOURNALCHANNEL_GETPAGELIST = "IssueAPI/GetJournalChannelPageList";

        /// <summary>
        /// 获取期刊栏目列表
        /// </summary>
        public const string JOURNALCHANNEL_GETLIST = "IssueAPI/GetJournalChannelList";

        /// <summary>
        /// 根据期刊数据 按照期刊栏目数据分组 获取当前期刊数据所属的期刊栏目数据列表
        /// </summary>
        public const string JOURNALCHANNEL_GETLIST_BY_ISSUECONTENT = "IssueAPI/GetJournalChannelListByIssueContent";


        /// <summary>
        /// 获取期刊栏目实体
        /// </summary>
        public const string JOURNALCHANNEL_GETMODEL = "IssueAPI/GetJournalChannelModel";

        /// <summary>
        /// 保存期刊栏目
        /// </summary>
        public const string JOURNALCHANNEL_SAVE = "IssueAPI/SaveJournalChannel";

        /// <summary>
        /// 删除期刊栏目
        /// </summary>
        public const string JOURNALCHANNEL_DEL = "IssueAPI/DelJournalChannel";
        #endregion

        #region 期刊
        /// <summary>
        /// 获取期刊分页列表
        /// </summary>
        public const string ISSUECONTENT_GETPAGELIST = "IssueAPI/GetIssueContentPageList";

        /// <summary>
        /// 获取期刊分页列表（不包含已注册DOI的数据）
        /// </summary>
        public const string ISSUECONTENT_GETDOIPAGELIST = "IssueAPI/GetIssueContentDoiPageList";

        /// <summary>
        /// 获取期刊列表
        /// </summary>
        public const string ISSUECONTENT_GETLIST = "IssueAPI/GetIssueContentList";

        /// <summary>
        /// 获取期刊实体
        /// </summary>
        public const string ISSUECONTENT_GETMODEL = "IssueAPI/GetIssueContentModel";

        /// <summary>
        /// 保存期刊
        /// </summary>
        public const string ISSUECONTENT_SAVE = "IssueAPI/SaveIssueContent";

        /// <summary>
        /// 设置期刊年卷期
        /// </summary>
        public const string ISSUE_SETCONTRIBUTIONYEARISSUE = "IssueAPI/SetContributionYearIssue";

        /// <summary>
        /// 删除期刊
        /// </summary>
        public const string ISSUECONTENT_DEL = "IssueAPI/DelIssueContent";

        /// <summary>
        /// 获取当前期刊最新的年、卷、期、当期封面图片
        /// </summary>
        public const string ISSUE_GETCURISSUEINFO = "IssueAPI/GetCurIssueInfo";


        /// <summary>
        /// 更新期刊浏览次数
        /// </summary>
        public const string ISSUECONTENT_UPDATEHITS = "IssueAPI/UpdateIssueContentHits";

        /// <summary>
        /// 更新期刊浏览次数(RichHTML)
        /// </summary>
        public const string ISSUECONTENT_UPDATEHTMLHITS = "IssueAPI/UpdateIssueContentHtmlHits";

        /// <summary>
        /// 更新期刊下载次数
        /// </summary>
        public const string ISSUECONTENT_UPDATEDOWNLOADS = "IssueAPI/UpdateIssueContentDownloads";

        /// <summary>
        /// 保存期刊浏览日志
        /// </summary>
        public const string ISSUE_SAVEVIEWLOG = "IssueAPI/SaveViewLog";

        /// <summary>
        /// 保存期刊下载日志
        /// </summary>
        public const string ISSUE_SAVEDOWNLOADLOG = "IssueAPI/SaveDownloadLog";


        #endregion

        public const string DOIREGLOG_SAVEDOIREGLOG = "IssueAPI/SaveDoiRegLog";

        public const string DOIREGLOG_DELDOIREGLOG = "IssueAPI/DelDoiRegLog";

        public const string DOIREGLOG_GETDOIREGLOG = "IssueAPI/GetDoiRegLog";

        public const string DOIREGLOG_GETPAGELIST = "IssueAPI/GetDoiRegLogPageList";

        public const string DOIREGLOG_GETLIST = "IssueAPI/GetDoiRegLogList";

        #region 期刊订阅
        /// <summary>
        /// 获取期刊订阅分页列表
        /// </summary>
        public const string ISSUESUBSCRIBE_GETPAGELIST = "IssueAPI/GetIssueSubscribePageList";

        /// <summary>
        /// 获取期刊订阅列表
        /// </summary>
        public const string ISSUESUBSCRIBE_GETLIST = "IssueAPI/GetIssueSubscribeList";

        /// <summary>
        /// 获取期刊订阅实体
        /// </summary>
        public const string ISSUESUBSCRIBE_GETMODEL = "IssueAPI/GetIssueSubscribeModel";

        /// <summary>
        /// 保存期刊订阅
        /// </summary>
        public const string ISSUESUBSCRIBE_SAVE = "IssueAPI/SaveIssueSubscribe";

        /// <summary>
        /// 删除期刊订阅
        /// </summary>
        public const string ISSUESUBSCRIBE_DEL = "IssueAPI/DelIssueSubscribe";
        #endregion

        #region 下载次数统计
        /// <summary>
        /// 获取期刊下载次数
        /// </summary>    
        public const string ISSUEDOWNLOAD_GETPAGELIST = "IssueAPI/GetIssueDownLogPageList";

        /// <summary>
        /// 获取期刊下载明细
        /// </summary>
        public const string ISSUEDOWNLOAD_GETDETAILPAGELIST = "IssueAPI/GetIssueDownLogDetailPageList";
       
        #endregion

        #region 浏览次数统计
        /// <summary>
        /// 获取期刊下载次数
        /// </summary>    
        public const string ISSUEVIEW_GETPAGELIST = "IssueAPI/GetIssueViewLogPageList";

        /// <summary>
        /// 获取期刊下载明细
        /// </summary>
        public const string ISSUEVIEW_GETDETAILPAGELIST = "IssueAPI/GetIssueViewLogDetailPageList";

        #endregion

        #endregion

        # region 留言相关

        /// <summary>
        /// 获取留言分页列表
        /// </summary>
        public const string SITE_GUESTBOOKIE_GETPAGELIST = "SiteContentAPI/GetGuestBookPageList";

        /// <summary>
        /// 保存留言
        /// </summary>
        public const string SITE_GUESTBOOKE_SAVE = "SiteContentAPI/SaveGuestBook";

        # endregion

        #region 收稿量统计
        /// <summary>
        /// 按年月统计收稿量
        /// </summary>
        public const string CONTRIBUTIONACCOUNT_GETYEARLIST = "ContributionInfoAPI/GetContributionAccountListByYear";

        /// <summary>
        /// 按基金级别统计收稿量
        /// </summary>
        public const string CONTRIBUTIONACCOUNT_GETFUNDLIST = "ContributionInfoAPI/GetContributionAccountListByFund";

        /// <summary>
        /// 按作者统计收稿量
        /// </summary>
        public const string CONTRIBUTIONACCOUNT_GETAUTHORLIST = "ContributionInfoAPI/GetContributionAccountListByAuhor";
        #endregion

        #region 自荐为审稿专家
        //获取专家申请分页数据
        public const string EXPERTAPPLY_GETPAGELIST = "ExpertApplyAPI/GetPageList";

        //获取专家申请数据列表
        public const string EXPERTAPPLY_GETLIST = "ExpertApplyAPI/GetList";

        //获取专家申请实体
        public const string EXPERTAPPLY_GETMODEL = "ExpertApplyAPI/GetModel";

        //提交申请
        public const string EXPERTAPPLY_SUBMITAPPLY = "ExpertApplyAPI/SubmitApply";

        //更新申请信息
        public const string EXPERTAPPLY_UPDATEAPPLY = "ExpertApplyAPI/UpdateApply";

        //删除专家申请信息
        public const string EXPERTAPPLY_DELAPPLY = "ExpertApplyAPI/DelApply"; 
        #endregion


    }
}
