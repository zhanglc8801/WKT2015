using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Service.Interface;
using WKT.Service.Wrapper;
using WKT.Log;

namespace Web.API.Controllers
{
    /// <summary>
    /// 稿件Api
    /// </summary>
    public class ContributionAPIController : ApiBaseController
    {
        # region 投稿设置

        /// <summary>
        /// 得到投稿设置信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ContributeSetEntity GetContributeSetInfo(QueryBase query)
        {
            try
            {
                IContributeSetService cSetService = ServiceContainer.Instance.Container.Resolve<IContributeSetService>();
                ContributeSetEntity cSetEntity = cSetService.GetContributeSetInfo(query);
               return cSetEntity;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("得到投稿设置信息出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed,ex.Message));
            }
        }

        /// <summary>
        /// 设置投稿公告
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public ExecResult SetContruibuteStatement(ContributeSetEntity cSetEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IContributeSetService cSetService = ServiceContainer.Instance.Container.Resolve<IContributeSetService>();
                bool flag = cSetService.UpdateStatement(cSetEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "设置成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "设置投稿公告失败，请确认录入信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "设置投稿公告时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 设置稿件编号格式
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public ExecResult SetContruibuteNumberFormat(ContributeSetEntity cSetEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IContributeSetService cSetService = ServiceContainer.Instance.Container.Resolve<IContributeSetService>();
                bool flag = cSetService.SetContributeNumberFormat(cSetEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "设置成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "设置稿件编号格式失败，请确认录入信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "设置稿件编号格式时出现异常：" + ex.Message;
            }
            return result;
        }

        # endregion

        # region 投稿自动分配

        /// <summary>
        /// 得到投稿自动分配设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public EditorAutoAllotEntity GetContributeAutoAllotInfo(EditorAutoAllotQuery query)
        {
            try
            {
                IEditorAutoAllotService cSetService = ServiceContainer.Instance.Container.Resolve<IEditorAutoAllotService>();
                EditorAutoAllotEntity cSetEntity = cSetService.GetEditorAutoAllot(query);
                if (cSetEntity == null)
                {
                    cSetEntity = new EditorAutoAllotEntity();
                    cSetEntity.SubjectAuthorMap = new List<SubjectAuthorMapEntity>();
                }
                else
                {
                    if (cSetEntity.SubjectAuthorMap == null)
                    {
                        cSetEntity.SubjectAuthorMap = new List<SubjectAuthorMapEntity>();
                    }
                }
                if (cSetEntity.SubjectAuthorMap.Count == 0)
                {
                    # region 获取学科分类

                    IDictValueService service = ServiceContainer.Instance.Container.Resolve<IDictValueService>();
                    DictValueQuery dictQuery = new DictValueQuery();
                    dictQuery.JournalID = query.JournalID;
                    dictQuery.DictKey = EnumDictKey.SubjectCat.ToString();
                    List<DictValueEntity> listDictValue = service.GetDictValueList(dictQuery);
                    SubjectAuthorMapEntity subjectItem = null;
                    foreach (DictValueEntity item in listDictValue)
                    {
                        subjectItem = new SubjectAuthorMapEntity();
                        subjectItem.SubjectCategoryID = item.ValueID;
                        subjectItem.CategoryName = item.ValueText;
                        subjectItem.AuthorID = 0;
                        cSetEntity.SubjectAuthorMap.Add(subjectItem);
                    }

                    # endregion
                }
                else
                {
                    IDictValueService service = ServiceContainer.Instance.Container.Resolve<IDictValueService>();
                    IDictionary<int, string> dictValues = service.GetDictValueDcit(query.JournalID, EnumDictKey.SubjectCat.ToString());
                    string subjectCategoryName = "";
                    foreach (SubjectAuthorMapEntity item in cSetEntity.SubjectAuthorMap)
                    {
                         dictValues.TryGetValue(item.SubjectCategoryID,out subjectCategoryName);
                         item.CategoryName = subjectCategoryName;
                         subjectCategoryName = "";
                    }
                }
                return cSetEntity;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("得到投稿自动分配出现异常：" + ex.Message);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        /// <summary>
        /// 设置稿件自动分配
        /// </summary>
        /// <param name="cSetEntity">指定稿件编号，编辑部ID，学科ID</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult SetContruibuteAllowAllot(EditorAutoAllotEntity cSetEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IEditorAutoAllotService cSetService = ServiceContainer.Instance.Container.Resolve<IEditorAutoAllotService>();
                bool flag = cSetService.SetAutoAllot(cSetEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "设置成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "设置稿件自动分配失败，请确认录入信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "设置稿件自动分配时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取投稿自动分配编辑
        /// </summary>
        /// <param name="cSetEntity">指定稿件编号，编辑部ID，学科ID</param>
        /// <returns></returns>
        public AuthorInfoEntity GetAutoAllotEditor(EditorAutoAllotQuery cSetEntity)
        {
            AuthorInfoEntity result = null;
            try
            {
                IEditorAutoAllotService cSetService = ServiceContainer.Instance.Container.Resolve<IEditorAutoAllotService>();
                result = cSetService.GetAutoAllotEditor(cSetEntity);
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("获取投稿自动分配编辑出现异常：" + ex.Message);
            }
            return result;
        }

        # endregion

        # region 稿件处理专区

        /// <summary>
        /// 设置稿件旗帜标记
        /// </summary>
        /// <param name="cEntityList"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult SetContributeFlag(List<ContributionInfoQuery> cEntityList)
        {
            ExecResult result = new ExecResult();
            try
            {
                IContributionInfoService cService = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
                bool flag = cService.BatchUpdateContributionFlag(cEntityList);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "设置稿件旗帜标志成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "设置稿件旗帜标志成功失败，请确认录入信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "设置稿件旗帜标志时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 设置稿件加急标记
        /// </summary>
        /// <param name="cEntityList"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult SetContributeQuick(List<ContributionInfoQuery> cEntityList)
        {
            ExecResult result = new ExecResult();
            try
            {
                IContributionInfoService cService = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
                bool flag = cService.BatchUpdateContributionIsQuick(cEntityList);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "设置稿件加急标志成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "设置稿件加急标志成功失败，请确认录入信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "设置稿件加急标志时出现异常：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 删除稿件
        /// </summary>
        /// <param name="cEntityList"></param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult DeleteContribute(List<ContributionInfoQuery> cEntityList)
        {
            ExecResult result = new ExecResult();
            try
            {
                IContributionInfoService cService = ServiceContainer.Instance.Container.Resolve<IContributionInfoService>();
                foreach (ContributionInfoQuery cQuery in cEntityList)
                {
                    bool flag = cService.UpdateContributionStatus(cQuery);
                    if (!flag)
                    {
                        result.result = EnumJsonResult.failure.ToString();
                        result.msg = "删除稿件失败:" + cQuery.CID.ToString();
                        return result;
                    }
                }
                result.result = EnumJsonResult.success.ToString();
                result.msg = "删除稿件成功";
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "删除稿件时出现异常：" + ex.Message;
            }
            return result;
        }

        # endregion

        [System.Web.Http.AcceptVerbs("POST")]
        [Auth]
        public ExecResult SetContributeEditor(SetContributionEditorEntity setEntity)
        {
            ExecResult result = new ExecResult();
            try
            {
                IContributeSetService cService = ServiceContainer.Instance.Container.Resolve<IContributeSetService>();
                bool flag = cService.SetContributeEditor(setEntity);
                if (flag)
                {
                    result.result = EnumJsonResult.success.ToString();
                    result.msg = "设置稿件责任编辑成功";
                }
                else
                {
                    result.result = EnumJsonResult.failure.ToString();
                    result.msg = "设置稿件责任编辑失败，请确认录入信息是否正确";
                }
            }
            catch (Exception ex)
            {
                result.result = EnumJsonResult.error.ToString();
                result.msg = "设置稿件责任编辑失败时出现异常：" + ex.Message;
            }
            return result;
        }


        

    }
}