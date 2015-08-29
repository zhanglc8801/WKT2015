using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Config;
using WKT.Common.Email;
using WKT.Model;
using WKT.Model.Enum;
using WKT.Cache;
using WKT.Common.Utils;
using WKT.Facade.Service.Interface;

namespace WKT.Facade.Service
{
    public class ServiceBase
    {
        # region 得到角色名称

        private IDictionary<long, string> _dictRole = null;
        /// <summary>
        /// 角色键值对
        /// </summary>
        public IDictionary<long, string> DictRole
        {
            get
            {
                if (_dictRole == null)
                {
                    HttpClientHelper clientHelper = new HttpClientHelper();
                    RoleInfoQuery roleQuery = new RoleInfoQuery();
                    roleQuery.JournalID = SiteConfig.SiteID; 
                    roleQuery.GroupID = (byte)EnumMemberGroup.Editor;
                    _dictRole = clientHelper.PostAuth<IDictionary<long, string>, RoleInfoQuery>(GetAPIUrl(APIConstant.SYSGETROLEINFODICT), roleQuery);
                    if (!_dictRole.ContainsKey(1))
                    {
                        _dictRole.Add(1, "编辑");
                    }
                    if (!_dictRole.ContainsKey(2))
                    {
                        _dictRole.Add(2, "作者");
                    }
                    if (!_dictRole.ContainsKey(3))
                    {
                        _dictRole.Add(3, "专家");
                    }
                    if (!_dictRole.ContainsKey(4))
                    {
                        _dictRole.Add(4, "英文专家");
                    }
                    return _dictRole;
                }
                else
                {
                    return _dictRole;
                }

            }
            set { value = _dictRole;
                 _dictRole = value;
                }
        }

        /// <summary>
        /// 得到角色名称
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public string GetRoleName(long RoleID)
        {
            if (DictRole.ContainsKey(RoleID))
            {
                return DictRole[RoleID];
            }
            else
            {
                return "";
            }
        }

        # endregion

        # region 得到成员名称

        private IDictionary<long, string> _dictMember = null;

        /// <summary>
        /// 成员
        /// </summary>
        public IDictionary<long, string> DictMember
        {
            get
            {
                if (_dictMember == null)
                {
                    HttpClientHelper clientHelper = new HttpClientHelper();
                    AuthorInfoQuery authorQuery = new AuthorInfoQuery();
                    authorQuery.JournalID = SiteConfig.SiteID;
                    _dictMember = clientHelper.PostAuth<IDictionary<long, string>, AuthorInfoQuery>(GetAPIUrl(APIConstant.AUTHOR_GETMEMBERDICT), authorQuery);
                    return _dictMember;
                }
                else
                {
                    return _dictMember;
                }

            }
        }

        /// <summary>
        /// 得到成员名称
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public string GetMemberName(long AuthorID)
        {
            if (DictMember.ContainsKey(AuthorID))
            {
                return DictMember[AuthorID];
            }
            else
            {
                return "";
            }
        }

        # endregion

        /// <summary>
        /// 获取审稿链接
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        public string AuditContributionUrl(long AuthorID)
        {
            # region 新增自动登陆Token

            string strToekCode = RadomCode.GenerateCode(10) + DateTime.Now.Ticks;
            IAuthorFacadeService authorService = new AuthorFacadeAPIService();
            TokenEntity getPwdToken = new TokenEntity();
            getPwdToken.Token = strToekCode;
            getPwdToken.JournalID = SiteConfig.SiteID;
            getPwdToken.Type = 2; // 审稿连接Toekn
            getPwdToken.AuthorID = AuthorID;
            authorService.InsertToken(getPwdToken);

            # endregion

            return "http://" + Utils.GetHost() + SiteConfig.RootPath + "/user/expertredirect?AuthorID=" + AuthorID + "&Token=" + strToekCode;
        }

        /// <summary>
        /// 获取作者链接
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        public string AuditAuthorContributionUrl(long AuthorID)
        {
            # region 新增自动登陆Token

            string strToekCode = RadomCode.GenerateCode(10) + DateTime.Now.Ticks;
            IAuthorFacadeService authorService = new AuthorFacadeAPIService();
            TokenEntity getPwdToken = new TokenEntity();
            getPwdToken.Token = strToekCode;
            getPwdToken.JournalID = SiteConfig.SiteID;
            getPwdToken.Type = 2; // 审稿连接Toekn
            getPwdToken.AuthorID = AuthorID;
            authorService.InsertToken(getPwdToken);

            # endregion

            return "http://" + Utils.GetHost() + SiteConfig.RootPath + "/user/authorredirect?AuthorID=" + AuthorID + "&Token=" + strToekCode;
        }


        /// <summary>
        /// 获取API地址
        /// </summary>
        /// <param name="ActionUrl"></param>
        /// <returns></returns>
        public string GetAPIUrl(string ActionUrl)
        {
            return SiteConfig.APIHost + ActionUrl;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="RecevieMail"></param>
        /// <param name="mailConfig"></param>
        /// <returns></returns>
        public bool SendMail(string Subject, string Body, string RecevieMail, WKT.Model.SiteConfigEntity mailConfig = null)
        {           
            try
            {
                if (mailConfig == null)
                {
                    mailConfig = null;/// TODO:获取站点的邮件配置
                }
                return EmailUtils.SendMail(mailConfig.MailServer, mailConfig.MailPort, mailConfig.MailAccount, mailConfig.MailPwd
                    , mailConfig.SendMail, RecevieMail, "", "", Subject, Body, null, 2, true, "GB2312", true,true);
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
    }
}
