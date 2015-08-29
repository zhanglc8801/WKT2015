using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Log;
using WKT.Config;
using WKT.Common.Utils;
using WKT.Facade.Service.Interface;

namespace WKT.Facade.Service
{
    /// <summary>
    /// 作者API实现方式
    /// </summary>
    public class AuthorFacadeAPIService : ServiceBase,IAuthorFacadeService
    {
        # region 作者登录、注册、忘记密码、修改密码、记录登录信息

        /// <summary>
        /// 作者登录
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        public AuthorInfoEntity AuthorLogin(AuthorInfoQuery queryAuthor)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            AuthorInfoEntity authorEntity = clientHelper.PostAuth<AuthorInfoEntity, AuthorInfoQuery>(GetAPIUrl(APIConstant.AUTHORLOGIN), queryAuthor);
            if (authorEntity != null)
            {
                if (authorEntity.Pwd == WKT.Common.Security.MD5Handle.Encrypt(queryAuthor.Pwd))
                {
                    # region 设置该用户的角色列表

                    RoleAuthorQuery roleAuthorQuery = new RoleAuthorQuery();
                    roleAuthorQuery.JournalID = queryAuthor.JournalID;
                    roleAuthorQuery.AuthorID = authorEntity.AuthorID;
                    IList<RoleAuthorEntity> roleAuthorList = clientHelper.PostAuth<IList<RoleAuthorEntity>, RoleAuthorQuery>(GetAPIUrl(APIConstant.AUTHORGETROLELIST), roleAuthorQuery);
                    if (roleAuthorList != null && roleAuthorList.Count > 0)
                    {
                        authorEntity.RoleIDList = roleAuthorList.Select(p => p.RoleID).ToList<long>();
                        authorEntity.RoleID = authorEntity.RoleIDList[0];
                    }
                    # endregion
                    if (authorEntity.RoleIDList == null)
                    {
                        authorEntity.RoleIDList = new List<long> { -1};
                    }
                    
                    return authorEntity;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 作者注册
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        public ExecResult AuthorReg(AuthorInfoEntity authorEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, AuthorInfoEntity>(GetAPIUrl(APIConstant.AUTHORREG), authorEntity);
            return execResult;
        }

        /// <summary>
        /// 作者忘记密码
        /// </summary>
        /// <param name="queryAuthor"></param>
        /// <returns></returns>
        public ExecResult AuthorRetakePwd(AuthorInfoQuery authorQuery)
        {
            ExecResult execResult = new ExecResult();
            HttpClientHelper clientHelper = new HttpClientHelper();
            // 查找该邮箱地址是否存在
            AuthorInfoEntity authorEntity = clientHelper.PostAuth<AuthorInfoEntity, AuthorInfoQuery>(GetAPIUrl(APIConstant.AUTHORLOGIN), authorQuery);
            if (authorEntity != null)
            {
                // 如果存在则把密码解密后发送到邮箱
                ///TODO:发送邮件
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "密码已经发送到注册邮箱，请查收";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有该作者信息，请验证输入的邮箱地址是否正确";
            }
            return execResult;
        }

        /// <summary>
        /// 作者修改密码
        /// </summary>
        /// <param name="authorEntity"></param>
        /// <returns></returns>
        public ExecResult EditPwd(AuthorInfoEntity authorEntity)
        {
            ExecResult execResult = new ExecResult();
            HttpClientHelper clientHelper = new HttpClientHelper();

            // 根据登录名得到登录人
            AuthorInfoQuery queryAuthor = new AuthorInfoQuery();
            queryAuthor.AuthorID = authorEntity.AuthorID;
            queryAuthor.JournalID = authorEntity.JournalID;
            AuthorInfoEntity checkPwdEntity = clientHelper.PostAuth<AuthorInfoEntity, AuthorInfoQuery>(GetAPIUrl(APIConstant.AUTHORLOGIN), queryAuthor);
            if (checkPwdEntity == null)
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "没有该作者信息，请确认作者信息是否正确";
            }
            else
            {
                if (string.IsNullOrEmpty(queryAuthor.Pwd))
                {
                    authorEntity.Pwd = WKT.Common.Security.MD5Handle.Encrypt(authorEntity.NewPwd);
                    execResult = clientHelper.PostAuth<ExecResult, AuthorInfoEntity>(GetAPIUrl(APIConstant.AUTHOREDITPWD), authorEntity);
                }
                else
                {
                    if (checkPwdEntity.Pwd == WKT.Common.Security.MD5Handle.Encrypt(authorEntity.Pwd))
                    {
                        authorEntity.Pwd = WKT.Common.Security.MD5Handle.Encrypt(authorEntity.NewPwd);
                        execResult = clientHelper.PostAuth<ExecResult, AuthorInfoEntity>(GetAPIUrl(APIConstant.AUTHOREDITPWD), authorEntity);
                    }
                    else
                    {
                        execResult.result = EnumJsonResult.failure.ToString();
                        execResult.msg = "输入的旧密码不正确，请确认";
                    }
                }
            }
            return execResult;
        }

        /// <summary>
        /// 记录登录信息
        /// </summary>
        /// <param name="authorEntity"></param>
        /// <returns></returns>
        public ExecResult RecordLoginInfo(AuthorInfoEntity authorEntity)
        {
            ExecResult execResult = new ExecResult();
            HttpClientHelper clientHelper = new HttpClientHelper();
            execResult = clientHelper.PostAuth<ExecResult, AuthorInfoEntity>(GetAPIUrl(APIConstant.AUTHORRECORDLOGININFO), authorEntity);
            return execResult;
        }

        # endregion

        # region 令牌操作

        /// <summary>
        /// 新增令牌
        /// </summary>
        /// <param name="tokenEntity"></param>
        /// <returns></returns>
        public ExecResult InsertToken(TokenEntity tokenEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, TokenEntity>(GetAPIUrl(APIConstant.AUTHOR_TOKEN_ADD), tokenEntity);
            return execResult;
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="tokenQuery"></param>
        /// <returns></returns>
        public TokenEntity GetToken(TokenQuery tokenQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            TokenEntity tokenEntity = clientHelper.PostAuth<TokenEntity, TokenQuery>(GetAPIUrl(APIConstant.AUTHOR_TOKEN_GET), tokenQuery);
            return tokenEntity;
        }

        # endregion

        # region 获取编辑部成员获取及设置

        /// <summary>
        /// 获取编辑部成员列表
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        public Pager<AuthorInfoEntity> GetAuthorList(AuthorInfoQuery queryAuthor)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<AuthorInfoEntity> authorList = clientHelper.PostAuth<Pager<AuthorInfoEntity>, AuthorInfoQuery>(GetAPIUrl(APIConstant.AUTHOGETAUTHORINFOLIST), queryAuthor);
            return authorList;
        }

        /// <summary>
        /// 根据角色获取编辑部成员列表
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        public IList<AuthorInfoEntity> GetAuthorListByRole(AuthorInfoQuery queryAuthor)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<AuthorInfoEntity> authorList = clientHelper.PostAuth<IList<AuthorInfoEntity>, AuthorInfoQuery>(GetAPIUrl(APIConstant.AUTHOGETAUTHORINFOLISTBYROLE), queryAuthor);
            return authorList;
        }

        /// <summary>
        /// 设置作者角色
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        public ExecResult SetAurhorRole(RoleAuthorEntity roleAuthorEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, RoleAuthorEntity>(GetAPIUrl(APIConstant.AUTHORSETROLE), roleAuthorEntity);
            return execResult;
        }

        /// <summary>
        /// 删除作者角色
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        public ExecResult DelAurhorRole(RoleAuthorEntity roleAuthorEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, RoleAuthorEntity>(GetAPIUrl(APIConstant.AUTHORDELROLE), roleAuthorEntity);
            return execResult;
        }

        /// <summary>
        /// 设置作者角色菜单例外
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        public ExecResult SetAurhorMenuRightException(AuthorMenuRightExceptionEntity authorMenuRightException)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, AuthorMenuRightExceptionEntity>(GetAPIUrl(APIConstant.AUTHORSETMENURIGHTEXCEPTION), authorMenuRightException);
            return execResult;
        }

        /// <summary>
        /// 编辑编辑部成员信息
        /// </summary>
        /// <param name="authorQueryEntity"></param>
        /// <returns></returns>
        public ExecResult EditMember(AuthorInfoEntity authorEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, AuthorInfoEntity>(GetAPIUrl(APIConstant.AUTHOREDITMEMEBERINFO), authorEntity);
            return execResult;
        }

        /// <summary>
        /// 编辑作者信息
        /// </summary>
        /// <param name="authorQueryEntity"></param>
        /// <returns></returns>
        public ExecResult EditAuthor(AuthorInfoEntity authorEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, AuthorInfoEntity>(GetAPIUrl(APIConstant.AUTHOREDITAUTHORINFO), authorEntity);
            return execResult;
        }


        /// <summary>
        /// 获取编辑部成员信息
        /// </summary>
        /// <param name="authorQueryEntity"></param>
        /// <returns></returns>
        public AuthorInfoEntity GetMemberInfo(AuthorInfoQuery authorQueryEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            AuthorInfoEntity authorEntity = clientHelper.PostAuth<AuthorInfoEntity, AuthorInfoQuery>(GetAPIUrl(APIConstant.AUTHORGETMEMBERINFO), authorQueryEntity);
            authorEntity.Pwd = "";
            return authorEntity;
        }

        /// <summary>
        /// 获取编辑部成员列表
        /// </summary>
        /// <param name="authorQueryEntity"></param>
        /// <returns></returns>
        public Pager<AuthorInfoEntity> GetMemberInfoList(AuthorInfoQuery authorQueryEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<AuthorInfoEntity> authorPagerList = clientHelper.PostAuth<Pager<AuthorInfoEntity>, AuthorInfoQuery>(GetAPIUrl(APIConstant.AUTHORGETMEMBERINFOLIST), authorQueryEntity);
            return authorPagerList;
        }

        /// <summary>
        /// 获取专家列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<AuthorInfoEntity> GetExpertPageList(AuthorInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            Pager<AuthorInfoEntity> authorPagerList = clientHelper.PostAuth<Pager<AuthorInfoEntity>, AuthorInfoQuery>(GetAPIUrl(APIConstant.AUTHOR_GETEXPERTPAGELIST), query);
            return authorPagerList;
        }

        # endregion

        # region 作者字段设置

        /// <summary>
        /// 获取作者字段设置
        /// </summary>
        /// <returns></returns>
        public IList<FieldsSet> GetFieldsSet()
        {
            IList<FieldsSet> list = new List<FieldsSet>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Utils.GetMapPath(SiteConfig.RootPath + "/data/AuthorFieldSet.config"));
                XmlNodeList fieldList = xmlDoc.GetElementsByTagName("Field");
                FieldsSet fieldItem = null;
                foreach (XmlNode nodeItem in fieldList)
                {
                    fieldItem = new FieldsSet();
                    fieldItem.DisplayName = nodeItem.SelectSingleNode("DisplayName").InnerText;
                    fieldItem.FieldName = nodeItem.SelectSingleNode("FieldName").InnerText;
                    fieldItem.DBField = nodeItem.SelectSingleNode("DBField").InnerText;
                    fieldItem.IsShow = TypeParse.ToBool(nodeItem.SelectSingleNode("IsShow").InnerText, false);
                    fieldItem.IsRequire = TypeParse.ToBool(nodeItem.SelectSingleNode("IsRequire").InnerText, false);
                    list.Add(fieldItem);
                }
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取作者字段设置时出现异常：" + ex.Message);
            }
            return list;
        }

        /// <summary>
        /// 设置作者字段
        /// </summary>
        /// <returns></returns>
        public ExecResult SetFields(List<FieldsSet> fieldsArray)
        {
            ExecResult execResult = new ExecResult();
            try
            {
                string path = Utils.GetMapPath(SiteConfig.RootPath + "/data/AuthorFieldSet.config");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlNodeList fieldList = xmlDoc.GetElementsByTagName("Field");
                string dbField = "";
                FieldsSet fieldsSetEntity = null;
                foreach (XmlNode nodeItem in fieldList)
                {
                    dbField = nodeItem.SelectSingleNode("DBField").InnerText;
                    fieldsSetEntity = fieldsArray.Single(p => p.DBField == dbField);
                    nodeItem.SelectSingleNode("DisplayName").InnerText = fieldsSetEntity != null ? fieldsSetEntity.DisplayName : "";
                    nodeItem.SelectSingleNode("IsShow").InnerText = fieldsSetEntity != null ? fieldsSetEntity.IsShow.ToString() : "false";
                    nodeItem.SelectSingleNode("IsRequire").InnerText = fieldsSetEntity != null ? fieldsSetEntity.IsRequire.ToString() : "false";
                    dbField = "";
                    fieldsSetEntity = null;
                }
                xmlDoc.Save(path);
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "成功";
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "保存作者字段设置出现异常：" + ex.Message;
                LogProvider.Instance.Error("保存作者字段设置出现异常：" + ex.Message);
            }
            return execResult;
        }

        # endregion

        # region 专家字段设置

        /// <summary>
        /// 获取专家字段设置
        /// </summary>
        /// <returns></returns>
        public IList<FieldsSet> GetExpertFieldsSet()
        {
            IList<FieldsSet> list = new List<FieldsSet>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Utils.GetMapPath(SiteConfig.RootPath + "/data/ExpertFieldSet.config"));
                XmlNodeList fieldList = xmlDoc.GetElementsByTagName("Field");
                FieldsSet fieldItem = null;
                foreach (XmlNode nodeItem in fieldList)
                {
                    fieldItem = new FieldsSet();
                    fieldItem.DisplayName = nodeItem.SelectSingleNode("DisplayName").InnerText;
                    fieldItem.FieldName = nodeItem.SelectSingleNode("FieldName").InnerText;
                    fieldItem.DBField = nodeItem.SelectSingleNode("DBField").InnerText;
                    fieldItem.IsShow = TypeParse.ToBool(nodeItem.SelectSingleNode("IsShow").InnerText, false);
                    fieldItem.IsRequire = TypeParse.ToBool(nodeItem.SelectSingleNode("IsRequire").InnerText, false);
                    list.Add(fieldItem);
                }
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取专家字段设置时出现异常：" + ex.Message);
            }
            return list;
        }

        /// <summary>
        /// 设置专家字段
        /// </summary>
        /// <returns></returns>
        public ExecResult SetExpertFields(List<FieldsSet> fieldsArray)
        {
            ExecResult execResult = new ExecResult();
            try
            {
                string path = Utils.GetMapPath(SiteConfig.RootPath + "/data/ExpertFieldSet.config");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlNodeList fieldList = xmlDoc.GetElementsByTagName("Field");
                string dbField = "";
                FieldsSet fieldsSetEntity = null;
                foreach (XmlNode nodeItem in fieldList)
                {
                    dbField = nodeItem.SelectSingleNode("DBField").InnerText;
                    fieldsSetEntity = fieldsArray.Single(p => p.DBField == dbField);
                    nodeItem.SelectSingleNode("DisplayName").InnerText = fieldsSetEntity != null ? fieldsSetEntity.DisplayName : "";
                    nodeItem.SelectSingleNode("IsShow").InnerText = fieldsSetEntity != null ? fieldsSetEntity.IsShow.ToString() : "false";
                    nodeItem.SelectSingleNode("IsRequire").InnerText = fieldsSetEntity != null ? fieldsSetEntity.IsRequire.ToString() : "false";
                    dbField = "";
                    fieldsSetEntity = null;
                }
                xmlDoc.Save(path);
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "成功";
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "保存专家字段设置出现异常：" + ex.Message;
                LogProvider.Instance.Error("保存专家字段设置出现异常：" + ex.Message);
            }
            return execResult;
        }

        # endregion

        # region 编辑字段设置

        /// <summary>
        /// 获取编辑字段设置
        /// </summary>
        /// <returns></returns>
        public IList<FieldsSet> GetEditorFieldsSet()
        {
            IList<FieldsSet> list = new List<FieldsSet>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Utils.GetMapPath(SiteConfig.RootPath + "/data/EditorFieldSet.config"));
                XmlNodeList fieldList = xmlDoc.GetElementsByTagName("Field");
                FieldsSet fieldItem = null;
                foreach (XmlNode nodeItem in fieldList)
                {
                    fieldItem = new FieldsSet();
                    fieldItem.DisplayName = nodeItem.SelectSingleNode("DisplayName").InnerText;
                    fieldItem.FieldName = nodeItem.SelectSingleNode("FieldName").InnerText;
                    fieldItem.DBField = nodeItem.SelectSingleNode("DBField").InnerText;
                    fieldItem.IsShow = TypeParse.ToBool(nodeItem.SelectSingleNode("IsShow").InnerText, false);
                    fieldItem.IsRequire = TypeParse.ToBool(nodeItem.SelectSingleNode("IsRequire").InnerText, false);
                    list.Add(fieldItem);
                }
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取专家字段设置时出现异常：" + ex.Message);
            }
            return list;
        }

        /// <summary>
        /// 设置编辑字段
        /// </summary>
        /// <returns></returns>
        public ExecResult SetEditorFields(List<FieldsSet> fieldsArray)
        {
            ExecResult execResult = new ExecResult();
            try
            {
                string path = Utils.GetMapPath(SiteConfig.RootPath + "/data/EditorFieldSet.config");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlNodeList fieldList = xmlDoc.GetElementsByTagName("Field");
                string dbField = "";
                FieldsSet fieldsSetEntity = null;
                foreach (XmlNode nodeItem in fieldList)
                {
                    dbField = nodeItem.SelectSingleNode("DBField").InnerText;
                    fieldsSetEntity = fieldsArray.Single(p => p.DBField == dbField);
                    nodeItem.SelectSingleNode("DisplayName").InnerText = fieldsSetEntity != null ? fieldsSetEntity.DisplayName : "";
                    nodeItem.SelectSingleNode("IsShow").InnerText = fieldsSetEntity != null ? fieldsSetEntity.IsShow.ToString() : "false";
                    nodeItem.SelectSingleNode("IsRequire").InnerText = fieldsSetEntity != null ? fieldsSetEntity.IsRequire.ToString() : "false";
                    dbField = "";
                    fieldsSetEntity = null;
                }
                xmlDoc.Save(path);
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "成功";
            }
            catch (Exception ex)
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "保存编辑字段设置出现异常：" + ex.Message;
                LogProvider.Instance.Error("保存编辑字段设置出现异常：" + ex.Message);
            }
            return execResult;
        }

        # endregion

        # region 作者统计

        /// <summary>
        /// 作者总数及投稿作者数量统计
        /// </summary>
        /// <returns></returns>
        public IDictionary<String, Int32> GetAuthorContributeStat(QueryBase query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IDictionary<String, Int32> dictAuthorStat = clientHelper.PostAuth<IDictionary<String, Int32>, QueryBase>(GetAPIUrl(APIConstant.ATHOR_STAT_GETAUTHORCONTRIBUTE), query);
            return dictAuthorStat;
        }

        /// <summary>
        /// 获取作者按省份统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorProvinceStat(QueryBase query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<AuthorStatEntity> listAuthorStat = clientHelper.PostAuth<IList<AuthorStatEntity>, QueryBase>(GetAPIUrl(APIConstant.ATHOR_STAT_GETAUTHORPROVINCE), query);
            return listAuthorStat;
        }

        /// <summary>
        /// 获取作者按学历统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorEducationStat(QueryBase query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<AuthorStatEntity> listAuthorStat = clientHelper.PostAuth<IList<AuthorStatEntity>, QueryBase>(GetAPIUrl(APIConstant.ATHOR_STAT_GETAUTHOREDUCATION), query);
            return listAuthorStat;
        }

        /// <summary>
        /// 获取作者按专业统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorProfessionalStat(QueryBase query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<AuthorStatEntity> listAuthorStat = clientHelper.PostAuth<IList<AuthorStatEntity>, QueryBase>(GetAPIUrl(APIConstant.ATHOR_STAT_GETAUTHORPROFESSIONAL), query);
            return listAuthorStat;
        }

        /// <summary>
        /// 获取作者按职称统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorJobTitleStat(QueryBase query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<AuthorStatEntity> listAuthorStat = clientHelper.PostAuth<IList<AuthorStatEntity>, QueryBase>(GetAPIUrl(APIConstant.ATHOR_STAT_GETAUTHORJOBTITLE), query);
            return listAuthorStat;
        }

        /// <summary>
        /// 获取作者按性别统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorStatEntity> GetAuthorGenderStat(QueryBase query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<AuthorStatEntity> listAuthorStat = clientHelper.PostAuth<IList<AuthorStatEntity>, QueryBase>(GetAPIUrl(APIConstant.ATHOR_STAT_GETAUTHORGENDER), query);
            return listAuthorStat;
        }

        # endregion

        /// <summary>
        /// 编辑、专家工作量统计
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<WorkloadEntity> GetWorkloadList(WorkloadQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<WorkloadEntity> listAuthorStat = clientHelper.PostAuth<IList<WorkloadEntity>, WorkloadQuery>(GetAPIUrl(APIConstant.ATHOR_STAT_WORKLOAD), query);
            return listAuthorStat;
        }      

        /// <summary>
        /// 编辑、专家处理稿件明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<StatDealContributionDetailEntity> GetDealContributionDetail(StatQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<StatDealContributionDetailEntity> listAuthorStat = clientHelper.PostAuth<IList<StatDealContributionDetailEntity>, StatQuery>(GetAPIUrl(APIConstant.AUOR_STAT_GETDEALCONTRIBUTIONDETAIL), query);
            return listAuthorStat;
        }

        /// <summary>
        /// 获取作者实体
        /// </summary>
        /// <param name="authorQueryEntity"></param>
        /// <returns></returns>
        public AuthorInfoEntity GetAuthorInfo(AuthorInfoQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            AuthorInfoEntity authorEntity = clientHelper.PostAuth<AuthorInfoEntity, AuthorInfoQuery>(GetAPIUrl(APIConstant.AUTHOR_GETAUTHORINFO), query);            
            return authorEntity;
        }
    }
}
