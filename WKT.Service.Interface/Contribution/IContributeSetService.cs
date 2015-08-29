using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    /// <summary>
    /// 投稿设置
    /// </summary>
    public partial interface IContributeSetService
    {
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
        bool UpdateStatement(ContributeSetEntity cSetEntity);
        /// <summary>
        /// 设置稿件编号格式
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        bool SetContributeNumberFormat(ContributeSetEntity cSetEntity);

        /// <summary>
        /// 得到稿件编号
        /// </summary>
        /// <param name="query">查询基类，主要用到站点ID</param>
        /// <returns></returns>
        string GetContributeNumber(QueryBase query);

        /// <summary>
        /// 设置稿件的责任编辑
        /// </summary>
        /// <param name="setEntity"></param>
        /// <returns></returns>
        bool SetContributeEditor(SetContributionEditorEntity setEntity);
    }
}






