using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    /// <summary>
    /// 投稿设置
    /// </summary>
    public partial class ContributeSetBusiness : IContributeSetBusiness
    {
        /// <summary>
        /// 得到投稿配置信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ContributeSetEntity GetContributeSetInfo(QueryBase query)
        {
            return ContributeSetDataAccess.Instance.GetContributeSetInfo(query);
        }

        /// <summary>
        /// 更新投稿公告
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public bool UpdateStatement(ContributeSetEntity cSetEntity)
        {
            return ContributeSetDataAccess.Instance.UpdateStatement(cSetEntity);
        }

        /// <summary>
        /// 设置稿件编号格式
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public bool SetContributeNumberFormat(ContributeSetEntity cSetEntity)
        {
            return ContributeSetDataAccess.Instance.SetContributeNumberFormat(cSetEntity);
        }

        /// <summary>
        /// 得到稿件编号
        /// </summary>
        /// <param name="query">查询基类，主要用到站点ID</param>
        /// <returns></returns>
        public string GetContributeNumber(QueryBase query)
        {
            return ContributeSetDataAccess.Instance.GetContributeNumber(query);
        }

         /// <summary>
        /// 设置稿件的责任编辑
        /// </summary>
        /// <param name="setEntity"></param>
        /// <returns></returns>
        public bool SetContributeEditor(SetContributionEditorEntity setEntity)
        {
            return ContributeSetDataAccess.Instance.SetContributeEditor(setEntity);
        }
    }
}
