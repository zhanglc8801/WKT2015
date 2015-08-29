using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Config;
using WKT.Log;
using WKT.Common.Utils;
using WKT.Facade.Service.Interface;

namespace WKT.Facade.Service
{
    public class ContributionFacadeAPIService : ServiceBase, IContributionFacadeService
    {
        # region 投稿设置

        /// <summary>
        /// 获取稿件的附件
        /// </summary>
        /// <param name="cQuery">稿件ID</param>
        /// <returns></returns>        
        public string GetContributionAttachment(ContributionInfoQuery cQuery)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            string cAttPath = clientHelper.PostAuth<string, ContributionInfoQuery>(GetAPIUrl(APIConstant.C_GETCONTRIBUTIONATTACHMENT), cQuery);
            return cAttPath;
        }

        /// <summary>
        /// 得到投稿配置信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ContributeSetEntity GetContributeSetInfo(QueryBase query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ContributeSetEntity cSetResult = clientHelper.PostAuth<ContributeSetEntity, QueryBase>(GetAPIUrl(APIConstant.CONTRIBUTIOGETINFO), query);
            return cSetResult;
        }

        /// <summary>
        /// 更新投稿公告
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public ExecResult SetContruibuteStatement(ContributeSetEntity cSetEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, ContributeSetEntity>(GetAPIUrl(APIConstant.CONTRIBUTIOSTATEMENTSET), cSetEntity);
            return execResult;
        }

        /// <summary>
        /// 设置稿件编号格式
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public ExecResult SetContributeNumberFormat(ContributeSetEntity cSetEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, ContributeSetEntity>(GetAPIUrl(APIConstant.CONTRIBUTIONNUMBERSET), cSetEntity);
            return execResult;
        }

        /// <summary>
        /// 获取投稿字段设置
        /// </summary>
        /// <returns></returns>
        public IList<FieldsSet> GetFieldsSet()
        {
            IList<FieldsSet> list = new List<FieldsSet>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Utils.GetMapPath(SiteConfig.RootPath + "/data/ContributionFieldSet.config"));
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
            catch(Exception ex)
            {
                LogProvider.Instance.Error("获取投稿字段设置时出现异常：" + ex.Message);
            }
            return list;
        }

        /// <summary>
        /// 设置投稿字段
        /// </summary>
        /// <returns></returns>
        public ExecResult SetFields(List<FieldsSet> fieldsArray)
        {
            ExecResult execResult = new ExecResult();
            try
            {
                string path = Utils.GetMapPath(SiteConfig.RootPath + "/data/ContributionFieldSet.config");
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
                execResult.msg = "保存投稿字段设置出现异常：" + ex.Message;
                LogProvider.Instance.Error("保存投稿字段设置出现异常：" + ex.Message);
            }
            return execResult;
        }


        /// <summary>
        /// 获取稿件作者字段设置
        /// </summary>
        /// <returns></returns>
        public IList<FieldsSet> GetContributionAuthorFieldsSet()
        {
            IList<FieldsSet> list = new List<FieldsSet>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Utils.GetMapPath(SiteConfig.RootPath + "/data/ContributionAuthorFieldSet.config"));
                XmlNodeList fieldList = xmlDoc.GetElementsByTagName("Field");
                FieldsSet fieldItem = null;
                foreach (XmlNode nodeItem in fieldList)
                {
                    fieldItem = new FieldsSet();
                    fieldItem.DisplayName = nodeItem.SelectSingleNode("DisplayName").InnerText;
                    fieldItem.FieldName = nodeItem.SelectSingleNode("FieldName").InnerText;
                    fieldItem.DBField = nodeItem.SelectSingleNode("DBField").InnerText;
                    //fieldItem.IsShow = TypeParse.ToBool(nodeItem.SelectSingleNode("IsShow").InnerText, false);
                    fieldItem.IsRequire = TypeParse.ToBool(nodeItem.SelectSingleNode("IsRequire").InnerText, false);
                    list.Add(fieldItem);
                }
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取投稿字段设置时出现异常：" + ex.Message);
            }
            return list;
        }

        /// <summary>
        /// 设置稿件作者字段
        /// </summary>
        /// <returns></returns>
        public ExecResult SetContributionAuthorFields(List<FieldsSet> fieldsArray)
        {
            ExecResult execResult = new ExecResult();
            try
            {
                string path = Utils.GetMapPath(SiteConfig.RootPath + "/data/ContributionAuthorFieldSet.config");
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
                    //nodeItem.SelectSingleNode("IsShow").InnerText = fieldsSetEntity != null ? fieldsSetEntity.IsShow.ToString() : "false";
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
                execResult.msg = "保存稿件作者字段设置出现异常：" + ex.Message;
                LogProvider.Instance.Error("保存稿件作者字段设置出现异常：" + ex.Message);
            }
            return execResult;
        }

        # endregion

        # region 投稿自动分配

        /// <summary>
        /// 得到投稿自动分配设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public EditorAutoAllotEntity GetAllowAllotInfo(EditorAutoAllotQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            EditorAutoAllotEntity cSetResult = clientHelper.PostAuth<EditorAutoAllotEntity, EditorAutoAllotQuery>(GetAPIUrl(APIConstant.CONTRIBUTION_GETAUTOALLOTINFO), query);
            if (cSetResult.SubjectAuthorMap != null && cSetResult.SubjectAuthorMap.Count > 0)
            {
                foreach (SubjectAuthorMapEntity item in cSetResult.SubjectAuthorMap)
                {
                    item.AuthorName = GetMemberName(item.AuthorID);
                }
            }
            else
            {
                cSetResult.SubjectAuthorMap = new List<SubjectAuthorMapEntity>(0);
            }
            cSetResult.AuthorName = GetMemberName(cSetResult.AuthorID);
            cSetResult.OddAuthorName = GetMemberName(cSetResult.OddAuthorID);
            return cSetResult;
        }

        /// <summary>
        /// 设置稿件自动分配
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public ExecResult SetAllowAllot(EditorAutoAllotEntity cSetEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, EditorAutoAllotEntity>(GetAPIUrl(APIConstant.CONTRIBUTION_SETALLOWALLOT), cSetEntity);
            return execResult;
        }

        /// <summary>
        /// 得到投稿自动分配编辑
        /// </summary>
        /// <param name="query">指定稿件编号，编辑部ID，学科ID</param>
        /// <returns></returns>
        public AuthorInfoEntity GetAutoAllotEditor(EditorAutoAllotQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            AuthorInfoEntity authorResult = clientHelper.PostAuth<AuthorInfoEntity, EditorAutoAllotQuery>(GetAPIUrl(APIConstant.CONTRIBUTION_GETAUTOALLOTEDITOR), query);
            return authorResult;
        }

        # endregion

        # region 稿件处理专区

        /// <summary>
        /// 设置稿件旗帜标记
        /// </summary>
        /// <param name="cEntityList"></param>
        /// <returns></returns>
        public ExecResult SetContributeFlag(List<ContributionInfoQuery> cEntityList)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, List<ContributionInfoQuery>>(GetAPIUrl(APIConstant.CONTRIBUTION_SETCONTRIBUTEFLAG), cEntityList);
            return execResult;
        }

        /// <summary>
        /// 设置稿件加急标记
        /// </summary>
        /// <param name="cEntityList"></param>
        /// <returns></returns>
        public ExecResult SetContributeQuick(List<ContributionInfoQuery> cEntityList)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, List<ContributionInfoQuery>>(GetAPIUrl(APIConstant.CONTRIBUTION_SETCONTRIBUTEQUICK), cEntityList);
            return execResult;
        }

        /// <summary>
        /// 删除稿件
        /// </summary>
        /// <param name="cEntityList"></param>
        /// <returns></returns>
        public ExecResult DeleteContribute(List<ContributionInfoQuery> cEntityList)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, List<ContributionInfoQuery>>(GetAPIUrl(APIConstant.CONTRIBUTION_DELETECONTRIBUTE), cEntityList);
            return execResult;
        }

        # endregion

        /// <summary>
        /// 设置稿件的责任编辑
        /// </summary>
        /// <param name="setEntity"></param>
        /// <returns></returns>
        public ExecResult SetContributeEditor(SetContributionEditorEntity setEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, SetContributionEditorEntity>(GetAPIUrl(APIConstant.CONTRIBUTION_SETCONTRIBUTEEDITOR), setEntity);
            return execResult;
        }

        /// <summary>
        /// 更新介绍信标记
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult UpdateIntroLetter(ContributionInfoQuery model)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, ContributionInfoQuery>(GetAPIUrl(APIConstant.C_UPDATEINTROLETTER), model);
            return execResult;
        }

         /// <summary>
        /// 处理撤稿申请
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public ExecResult DealWithdrawal(ContributionInfoQuery cEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, ContributionInfoQuery>(GetAPIUrl(APIConstant.C_DEALWITHDRAWAL), cEntity);
            return execResult;
        }
        /// <summary>
        /// 撤销删除
        /// </summary>
        /// <param name="cEntity"></param>
        /// <returns></returns>
        public ExecResult CancelDelete(ContributionInfoQuery cEntity)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ExecResult execResult = clientHelper.PostAuth<ExecResult, ContributionInfoQuery>(GetAPIUrl(APIConstant.C_CANCELDELETE), cEntity);
            return execResult;
        }
        
        /// <summary>
        /// 获取稿件作者
        /// </summary>
        /// <param name="queryContributionAuthor"></param>
        /// <returns></returns>
        public ContributionAuthorEntity GetContributionAuthorInfo(ContributionAuthorQuery queryContributionAuthor)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            ContributionAuthorEntity contributionAuthorEntity = clientHelper.PostAuth<ContributionAuthorEntity, ContributionAuthorQuery>(GetAPIUrl(APIConstant.C_GETCONTRIBUTIONAUTHORINFO), queryContributionAuthor);
            return contributionAuthorEntity;
        }

        /// <summary>
        /// 获取稿件作者详细信息数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionAuthorEntity> GetContributionAuthorList(ContributionAuthorQuery query)
        {
            HttpClientHelper clientHelper = new HttpClientHelper();
            IList<ContributionAuthorEntity> list = clientHelper.Post<IList<ContributionAuthorEntity>, ContributionAuthorQuery>(GetAPIUrl(APIConstant.C_GETCONTRIBUTIONAUTHORLIST), query);
            return list;
        }


    }
}
