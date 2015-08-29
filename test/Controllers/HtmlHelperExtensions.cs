using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Reflection;
using System.ComponentModel;
using System.Configuration;
using System.Text.RegularExpressions;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WKT.Config;
using WKT.Common.Extension;
using WKT.Common.Utils;
using WKT.Common.Security;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;


namespace HanFang360.InterfaceService.Controllers
{
    public static class HtmlHelperExtensions
    {
        #region 全局配置属性

        /// <summary>
        /// 当前登录人信息
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string CurUserLoginName(this HtmlHelper helper)
        {
            if(TicketTool.IsLogin())
            {
                var LoginAuthor = JsonConvert.DeserializeObject<AuthorInfoEntity>(TicketTool.GetUserData());
                return LoginAuthor.LoginName;
            }
            return "";
        }

        /// <summary>
        /// 访问统计地址
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string StatUrl(this HtmlHelper helper)
        {
            return SiteConfig.APIHost + "web/stat?JournalID=" + ConfigurationManager.AppSettings["SiteID"];
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string SiteName(this HtmlHelper helper)
        {
            return SiteConfig.SiteName;
        }      

        /// <summary>
        /// 是否显示财务信息
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static bool IsFinance(this HtmlHelper helper)
        {
            return SiteConfig.IsFinance;
        }
        /// <summary>
        /// 作者注册时是否需要激活
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static bool isRegAct(this HtmlHelper helper)
        {
            return SiteConfig.isRegAct;
        }
        
        /// <summary>
        /// 下载文章时是否验证登录状态
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static bool isLoginVerify(this HtmlHelper helper)
        {
            return SiteConfig.isLoginVerify;
        }

        /// <summary>
        /// 是否允许作者查看更多审稿信息
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static bool isViewMoreFlow(this HtmlHelper helper)
        {
            return SiteConfig.isViewMoreFlow;
        }

        /// <summary>
        /// 是否允许作者查看更多审稿信息
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static bool isViewHistoryFlow(this HtmlHelper helper)
        {
            return SiteConfig.isViewHistoryFlow;
        }
        /// <summary>
        /// 是否自动处理撤稿申请
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static bool isAutoHandle(this HtmlHelper helper)
        {
            return SiteConfig.isAutoHandle;
        }

        /// <summary>
        /// 是否在编辑部工作量中使用按组统计
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static bool isStatByGroup(this HtmlHelper helper)
        {
            return SiteConfig.isStatByGroup;
        }

        /// <summary>
        /// 计费设置-审稿费金额
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static decimal ReviewFeeText(this HtmlHelper helper)
        {
            return SiteConfig.ReviewFeeText;
        }
        /// <summary>
        /// 计费设置-版面费金额
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static decimal PageFeeText(this HtmlHelper helper)
        {
            return SiteConfig.PageFeeText;
        }
        /// <summary>
        /// 计费设置-稿费-按篇金额
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static decimal GaoFeeText1(this HtmlHelper helper)
        {
            return SiteConfig.GaoFeeText1;
        }
        /// <summary>
        /// 计费设置-稿费-按页金额
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static decimal GaoFeeText2(this HtmlHelper helper)
        {
            return SiteConfig.GaoFeeText2;
        }

        /// <summary>
        /// 计费设置-稿费-按版面费百分比
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static decimal GaoFeeText3(this HtmlHelper helper)
        {
            return SiteConfig.GaoFeeText3;
        }

        /// <summary>
        /// 专家审稿费
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static decimal ExpertReviewFee(this HtmlHelper helper)
        {
            return SiteConfig.ExpertReviewFee;
        }

        /// <summary>
        /// 上传文件格式
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string UploadFileExt(this HtmlHelper helper)
        {
            return SiteConfig.UploadFileExt;
        }

        /// <summary>
        /// 上传HTML格式
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string UploadHtmExt(this HtmlHelper helper)
        {
            return SiteConfig.UploadHtmExt;
        }

        /// <summary>
        /// 上传路径
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string UploadPath(this HtmlHelper helper)
        {
            return SiteConfig.UploadPath;
        }

        /// <summary>
        /// 上传图片格式
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string UploadImgExt(this HtmlHelper helper)
        {
            return SiteConfig.UploadImgExt;
        }

        /// <summary>
        /// 获取稿件相关附件格式限制
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="type">0:稿件 1:附图 3:稿件pdf 4:介绍信</param>
        /// <returns></returns>
        public static string ContributionInfoFileExt(this HtmlHelper helper, Int32 type)
        {
            return SiteConfig.GetContributionInfoFileExt(type);
        }

        /// <summary>
        /// 站点首页
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string SiteHome(this HtmlHelper helper)
        {
            return WKT.Common.Utils.Utils.SiteHome();
        }
        
        /// <summary>
        /// 资源根路径
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string RootPath(this HtmlHelper helper)
        {
            return ConfigurationManager.AppSettings["RootPath"];
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetImages(this HtmlHelper helper, string fileName)
        {
            return string.Format("{0}/content/images/{1}", SiteConfig.RootPath, fileName);
        }

        /// <summary>
        /// 获取图标
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetIcons(this HtmlHelper helper, string fileName)
        {
            return string.Format("{0}/content/icons/{1}", SiteConfig.RootPath, fileName);
        }

        #endregion

        #region 个人配置属性
        //是否根据处理时间对稿件排序
        public static bool isPersonal_Order(this HtmlHelper helper)
        {
            if (TicketTool.IsLogin())
            {
                var LoginAuthor = JsonConvert.DeserializeObject<AuthorInfoEntity>(TicketTool.GetUserData());
                PersonalConfig pconfig = new PersonalConfig(LoginAuthor.AuthorID);
                return pconfig.isPersonal_Order;
            }
            else
            {
                return false;
            }
        }
        //是否在稿件搜索页仅显示当前登录者的稿件
        public static bool isPersonal_OnlyMySearch(this HtmlHelper helper)
        {
            if (TicketTool.IsLogin())
            {
                var LoginAuthor = JsonConvert.DeserializeObject<AuthorInfoEntity>(TicketTool.GetUserData());
                PersonalConfig pconfig = new PersonalConfig(LoginAuthor.AuthorID);
                return pconfig.isPersonal_OnlyMySearch;
            }
            else
            {
                return false;
            }

        }

        #endregion

        #region 下拉框

        /// <summary>
        /// 枚举键值集合下拉框控件
        /// </summary>
        /// <param name="helper"></param>       
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <param name="selectedValue">选中值</param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public static string SelectDictionary<T>(this HtmlHelper helper, string id, string width, string selectedValue, string defaultValue, string defaultText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);
            IDictionary<object, object> dict = typeof(T).GetEnumDictionary();
            foreach (KeyValuePair<object, object> obj in dict)
            {
                if (obj.Key.ToString().Equals(selectedValue))
                {
                    sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", obj.Key.ToString(), obj.Value.ToString());
                }
                else
                {
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", obj.Key.ToString(), obj.Value.ToString());
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 角色下拉框控件
        /// </summary>
        /// <param name="helper"></param>       
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <param name="selectedValue">选中值</param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public static string SelectRole(this HtmlHelper helper, string id, string width, string selectedValue, string defaultValue, string defaultText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);
            
            RoleInfoQuery roleQuery = new RoleInfoQuery();
            roleQuery.JournalID = SiteConfig.SiteID;
            roleQuery.GroupID = (int)EnumMemberGroup.Editor;
            ISiteSystemFacadeService sysService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            IList<RoleInfoEntity> listRole = sysService.GetRoleList(roleQuery);

            foreach (RoleInfoEntity item in listRole)
            {
                if (item.RoleID.ToString().Equals(selectedValue))
                {
                    sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.RoleID.ToString(), item.RoleName);
                }
                else
                {
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.RoleID.ToString(), item.RoleName);
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 带有作者和专家的角色下拉框控件
        /// </summary>
        /// <param name="helper"></param>       
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <param name="selectedValue">选中值</param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public static string SelectAllRole(this HtmlHelper helper, string id, string width, string selectedValue, string defaultValue, string defaultText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);

            RoleInfoQuery roleQuery = new RoleInfoQuery();
            roleQuery.JournalID = SiteConfig.SiteID;
            roleQuery.GroupID = (int)EnumMemberGroup.Editor;
            ISiteSystemFacadeService sysService = ServiceContainer.Instance.Container.Resolve<ISiteSystemFacadeService>();
            IList<RoleInfoEntity> listRole = sysService.GetRoleList(roleQuery);

            # region 添加作者和专家组

            RoleInfoEntity roleAuthor = new RoleInfoEntity();
            roleAuthor.RoleID = 2;
            roleAuthor.RoleName = "作者组";

            RoleInfoEntity roleExpert = new RoleInfoEntity();
            roleExpert.RoleID = 3;
            roleExpert.RoleName = "专家组";

            listRole.Insert(0,roleExpert);
            listRole.Insert(0, roleAuthor);

            # endregion

            foreach (RoleInfoEntity item in listRole)
            {
                if (item.RoleID.ToString().Equals(selectedValue))
                {
                    sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.RoleID.ToString(), item.RoleName);
                }
                else
                {
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.RoleID.ToString(), item.RoleName);
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 邮件短信模版下拉框
        /// </summary>
        /// <param name="helper"></param>       
        /// <param name="id"></param>
        /// <param name="width">带px 如：150px</param>
        /// <param name="ttype">类型 1:邮件 2:短信</param>
        /// <param name="selectedValue">选中值</param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public static string SelectMsgTemplateList(this HtmlHelper helper, string id, string width, int ttype, string selectedValue, string defaultValue, string defaultText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);
            MessageTemplateQuery query = new MessageTemplateQuery();
            query.JournalID = SiteConfig.SiteID;
            Byte i = (Byte)ttype;
            query.TType = i;
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            IList<MessageTemplateEntity> list = service.GetMessageTempList(query);
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item.TemplateID.ToString().Equals(selectedValue))
                    {
                        sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.TemplateID.ToString(), item.Title);
                    }
                    else
                    {
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.TemplateID.ToString(), item.Title);
                    }
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 邮件短信模版类型下拉框
        /// </summary>
        /// <param name="helper"></param>       
        /// <param name="id"></param>
        /// <param name="width">带px 如：150px</param>
        /// <param name="ttype">类型 1:邮件 2:短信</param>
        /// <param name="selectedValue">选中值</param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>
        /// <param name="isChecked">是否去掉已经存在的模版 默认false</param>
        /// <returns></returns>
        public static string SelectTCategory(this HtmlHelper helper, string id, string width,int ttype, string selectedValue, string defaultValue, string defaultText,bool isChecked=false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);
            MessageTemplateQuery query = new MessageTemplateQuery();
            query.JournalID = SiteConfig.SiteID;
            Byte i = (Byte)ttype;
            query.TType = i;
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            IDictionary<int, string> dict = null;
            if (isChecked)
                dict = service.GetTCategoryDictChecked(query);
            else
                dict = service.GetTCategoryDict(query);
            if (dict != null && dict.Count > 0)
            {
                foreach (var item in dict)
                {
                    if (item.Key.ToString().Equals(selectedValue))
                    {
                        sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.Key.ToString(), item.Value);
                    }
                    else
                    {
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Key.ToString(), item.Value);
                    }
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取短信邮件全局变量下拉框
        /// </summary>
        /// <param name="helper"></param>       
        /// <param name="id"></param>
        /// <param name="width">带px 如：150px</param>       
        /// <param name="selectedValue">选中值</param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>      
        /// <returns></returns>
        public static string SelectEmailVariable(this HtmlHelper helper, string id, string width, string selectedValue, string defaultValue, string defaultText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);           
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            IDictionary<string, string> dict = service.GetEmailVariable();          
            if (dict != null && dict.Count > 0)
            {
                foreach (var item in dict)
                {
                    if (item.Key.ToString().Equals(selectedValue))
                    {
                        sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.Key, item.Value);
                    }
                    else
                    {
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Key, item.Value);
                    }
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取数据字典下拉框
        /// </summary>
        /// <param name="helper"></param>       
        /// <param name="id"></param>
        /// <param name="width">带px 如：150px</param>   
        /// <param name="dictKey">字典值</param>
        /// <param name="selectedValue">选中值</param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>      
        /// <returns></returns>
        public static string SelectDcitValue(this HtmlHelper helper, string id, string width,string dictKey, string selectedValue, string defaultValue, string defaultText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            DictValueQuery query = new DictValueQuery();
            query.JournalID = SiteConfig.SiteID;
            query.DictKey = dictKey;
            IDictionary<int, string> dict = service.GetDictValueDcit(query);
            if (dict != null && dict.Count > 0)
            {
                foreach (var item in dict)
                {
                    if (item.Key.ToString().Equals(selectedValue))
                    {
                        sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.Key, item.Value);
                    }
                    else
                    {
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Key, item.Value);
                    }
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取期刊栏目设置中的栏目下拉框
        /// </summary>
        /// <param name="helper"></param>       
        /// <param name="id"></param>
        /// <param name="width">带px 如：150px</param>       
        /// <param name="selectedValue">选中值</param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>      
        /// <returns></returns>
        public static string SelectJChannel(this HtmlHelper helper, string id, string width, string selectedValue, string defaultValue, string defaultText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);           
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IList<JournalChannelEntity> list = service.GetJournalChannelList(new JournalChannelQuery() { JournalID = SiteConfig.SiteID });
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.JChannelID.ToString().Equals(selectedValue))
                    {
                        sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.JChannelID, item.ChannelName);
                    }
                    else
                    {
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.JChannelID, item.ChannelName);
                    }
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取系统分组
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string SelSystemGroup(this HtmlHelper helper,string id,string width,string selectedValue,string defaultValue,string defaultText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);
            IDictionary<int, string> dict = new Dictionary<int, string>() { { 2, "作者" }, { 1, "编辑部成员" }, { 3, "专家" } };
            if (dict != null && dict.Count > 0)
            {
                foreach (var item in dict)
                {
                    if (item.Key.ToString().Equals(selectedValue))
                    {
                        sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.Key, item.Value);
                    }
                    else
                    {
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Key, item.Value);
                    }
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取专家分组
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>        
        /// <param name="list"></param>
        /// <returns></returns>
        public static string SelectExpertGroup(this HtmlHelper helper, string name, IList<ExpertGroupMapEntity> list = null)
        {
            ISiteConfigFacadeService service = ServiceContainer.Instance.Container.Resolve<ISiteConfigFacadeService>();
            DictValueQuery query = new DictValueQuery();
            query.JournalID = SiteConfig.SiteID;
            query.DictKey = EnumDictKey.ExpertGroupMap.ToString();
            IDictionary<int, string> dict = service.GetDictValueDcit(query);
            StringBuilder strHtml = new StringBuilder();
            int i = 1;
            if (list == null)
                list = new List<ExpertGroupMapEntity>();
            foreach (var item in dict)
            {              
                strHtml.Append("<input id=\"chkEpertGroup_")
                    .Append(item.Key)
                    .Append("\" type=\"checkbox\" name=\"")
                    .Append(name)
                    .Append("\" value=\"")
                    .Append(item.Key)
                    .Append("\" "); 
                if (list.Where(p => p.ExpertGroupID == item.Key).Count() > 0)
                {
                    strHtml.Append(" checked=\"checked\" ");
                }
                strHtml.Append(" /><label for=\"chkEpertGroup_")
                    .Append(item.Key)
                    .Append("\" style=\"margin-right:30px;\">")
                    .Append(item.Value)
                    .Append("</label>");               
                if (i % 5 == 0)
                    strHtml.Append("<br />");
                i++;
            }
            return strHtml.ToString();
        }

        /// <summary>
        /// 获取期刊年卷下拉框
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <param name="selectedValue"></param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public static string SelectIssueYear(this HtmlHelper helper, string id, string width, string selectedValue, string defaultValue, string defaultText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IList<YearVolumeEntity> list = service.GetYearVolumeList(new YearVolumeQuery() { JournalID = SiteConfig.SiteID });
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.Year.ToString().Equals(selectedValue))
                    {
                        sb.AppendFormat("<option value=\"{0}\" alt=\"{1}\" selected=\"selected\">{0}年</option>", item.Year, item.Volume);
                    }
                    else
                    {
                        sb.AppendFormat("<option value=\"{0}\" alt=\"{1}\">{0}年</option>", item.Year, item.Volume);
                    }
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取期刊期数下拉框
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <param name="selectedValue"></param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public static string SelectIssueSet(this HtmlHelper helper, string id, string width, string selectedValue, string defaultValue, string defaultText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IList<IssueSetEntity> list = service.GetIssueSetList(new IssueSetQuery() { JournalID = SiteConfig.SiteID });
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.Issue.ToString().Equals(selectedValue))
                    {
                        sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">NO.{0}</option>", item.Issue);
                    }
                    else
                    {
                        sb.AppendFormat("<option value=\"{0}\" >NO.{0}</option>", item.Issue);
                    }
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取审稿状态
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>        
        /// <param name="list"></param>
        /// <returns></returns>
        public static string SelectFlowStatus(this HtmlHelper helper, long StatusID, string id, string width, string selectedValue, string defaultValue, string defaultText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);
            IFlowFacadeService service = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            FlowStatusQuery query = new FlowStatusQuery();
            query.JournalID = SiteConfig.SiteID;
            IDictionary<long,string> dictFlowStatus = service.GetFlowStatusDictStatusName(query);
            foreach (var item in dictFlowStatus)
            {
                if (item.Key != StatusID)
                {
                    if (item.Key.ToString().Equals(selectedValue))
                    {
                        sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.Key, item.Value);
                    }
                    else
                    {
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Key, item.Value);
                    }
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取年下拉框
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <param name="selectedValue"></param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public static string SelectYear(this HtmlHelper helper, string id, string width, string selectedValue, string defaultValue, string defaultText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{1}\" style=\"width:{2}\" class=\"input-select\">", id, id, width);
            if (!string.IsNullOrWhiteSpace(defaultText))
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);
            var list = new List<Int32>();
            Int32 year = DateTime.Now.Year;
            for (int i = year - 5; i <= year; i++)
            {
                list.Add(i);
            }
            foreach (var item in list)
            {
                if (item.ToString().Equals(selectedValue))
                {
                    sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">{0}年</option>", item);
                }
                else
                {
                    sb.AppendFormat("<option value=\"{0}\" >{0}年</option>", item);
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }
        #endregion

        #region 方法

        /// <summary>
        /// 获取截取内容
        /// </summary>
        /// <param name="board"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetSubHtmlText(this HtmlHelper helper, string htmlStr, Int32 length)
        {
            string text = string.Empty;
            if (!string.IsNullOrWhiteSpace(htmlStr))
            {
                text = Regex.Replace(htmlStr, @"(<[^>]*>)|([\s　])", string.Empty, RegexOptions.IgnoreCase);
                text = text.Replace("\n", "").Replace("\t", "");
                if (text.Length > length)
                    text = text.Substring(0, length);
            }
            return text;
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string UrlEncode(this HtmlHelper helper, string param)
        {
            return HttpContext.Current.Server.UrlEncode(param);
        }

        /// <summary>
        /// 获取当前期刊最新的年、卷、期、当期封面图片
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IssueSiteEntity GetCurIssueInfo(this HtmlHelper helper)
        {
            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            IssueSetQuery query = new IssueSetQuery();
            query.JournalID = GetJournalID();
            IssueSiteEntity issueInfoEntity = service.GetCurIssueInfo(query);
            if (issueInfoEntity == null)
            {
                issueInfoEntity = new IssueSiteEntity();
            }
            return issueInfoEntity;
        }
        /// <summary>
        /// 获取杂志ID
        /// </summary>
        /// <returns></returns>
        private static long GetJournalID()
        {
            return TypeParse.ToLong(ConfigurationManager.AppSettings["SiteID"]);
        }
        
        #endregion
    }
}
