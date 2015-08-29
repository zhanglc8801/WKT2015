using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;

namespace WKT.Facade.Service.Interface
{
    /// <summary>
    /// 稿件Service
    /// </summary>
    public interface IContributionFacadeService
    {
        # region 投稿字段设置

        /// <summary>
        /// 获取稿件的附件
        /// </summary>
        /// <param name="cQuery">稿件ID</param>
        /// <returns></returns>
        string GetContributionAttachment(ContributionInfoQuery cQuery);

        /// <summary>
        /// 得到投稿配置信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ContributeSetEntity GetContributeSetInfo(QueryBase query);
        /// <summary>
        /// 更新投稿公告
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        ExecResult SetContruibuteStatement(ContributeSetEntity cSetEntity);
        /// <summary>
        /// 设置稿件编号格式
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        ExecResult SetContributeNumberFormat(ContributeSetEntity cSetEntity);

        /// <summary>
        /// 获取投稿字段设置
        /// </summary>
        /// <returns></returns>
        IList<FieldsSet> GetFieldsSet();

        /// <summary>
        /// 设置投稿字段
        /// </summary>
        /// <returns></returns>
        ExecResult SetFields(List<FieldsSet> fieldsArray);


        /// <summary>
        /// 获取稿件作者字段设置
        /// </summary>
        /// <returns></returns>
        IList<FieldsSet> GetContributionAuthorFieldsSet();

        /// <summary>
        /// 设置稿件作者字段
        /// </summary>
        /// <returns></returns>
        ExecResult SetContributionAuthorFields(List<FieldsSet> fieldsArray);

        # endregion

        # region 投稿自动分配

        /// <summary>
        /// 得到投稿自动分配设置
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        EditorAutoAllotEntity GetAllowAllotInfo(EditorAutoAllotQuery query);

        /// <summary>
        /// 设置稿件自动分配
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        ExecResult SetAllowAllot(EditorAutoAllotEntity cSetEntity);

        /// <summary>
        /// 得到投稿自动分配编辑
        /// </summary>
        /// <param name="query">指定稿件编号，编辑部ID，学科ID</param>
        /// <returns></returns>
        AuthorInfoEntity GetAutoAllotEditor(EditorAutoAllotQuery query);

        # endregion

        # region 稿件处理专区

        /// <summary>
        /// 设置稿件旗帜标记
        /// </summary>
        /// <param name="cEntityList"></param>
        /// <returns></returns>
        ExecResult SetContributeFlag(List<ContributionInfoQuery> cEntityList);

        /// <summary>
        /// 设置稿件加急标记
        /// </summary>
        /// <param name="cEntityList"></param>
        /// <returns></returns>
        ExecResult SetContributeQuick(List<ContributionInfoQuery> cEntityList);

        /// <summary>
        /// 删除稿件
        /// </summary>
        /// <param name="cEntityList"></param>
        /// <returns></returns>
        ExecResult DeleteContribute(List<ContributionInfoQuery> cEntityList);

        # endregion

        /// <summary>
        /// 设置稿件的责任编辑
        /// </summary>
        /// <param name="setEntity"></param>
        /// <returns></returns>
        ExecResult SetContributeEditor(SetContributionEditorEntity setEntity);

        /// <summary>
        /// 更新介绍信标记
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult UpdateIntroLetter(ContributionInfoQuery model);

         /// <summary>
        /// 处理撤稿申请
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        ExecResult DealWithdrawal(ContributionInfoQuery cEntity);

        /// <summary>
        /// 撤销删除
        /// </summary>
        /// <param name="cEntity"></param>
        /// <returns></returns>
        ExecResult CancelDelete(ContributionInfoQuery cEntity);

        /// <summary>
        /// 获取稿件作者
        /// </summary>
        /// <param name="queryContributionAuthor"></param>
        /// <returns></returns>
        ContributionAuthorEntity GetContributionAuthorInfo(ContributionAuthorQuery queryContributionAuthor);

        /// <summary>
        /// 获取稿件作者详细信息数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<ContributionAuthorEntity> GetContributionAuthorList(ContributionAuthorQuery query);

    }
}
