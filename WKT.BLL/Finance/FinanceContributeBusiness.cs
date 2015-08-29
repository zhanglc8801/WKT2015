using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class FinanceContributeBusiness : IFinanceContributeBusiness
    {
        #region 获取一个实体对象

        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public FinanceContributeEntity GetFinanceContribute(Int64 pKID)
        {
            return FinanceContributeDataAccess.Instance.GetFinanceContribute(pKID);
        }

        #endregion

        #region 根据条件获取所有实体对象

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<FinanceContributeEntity></returns>
        public List<FinanceContributeEntity> GetFinanceContributeList()
        {
            return FinanceContributeDataAccess.Instance.GetFinanceContributeList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="financeContributeQuery">FinanceContributeQuery查询实体对象</param>
        /// <returns>List<FinanceContributeEntity></returns>
        public List<FinanceContributeEntity> GetFinanceContributeList(FinanceContributeQuery financeContributeQuery)
        {
            return FinanceContributeDataAccess.Instance.GetFinanceContributeList(financeContributeQuery);
        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<FinanceContributeEntity></returns>
        public Pager<FinanceContributeEntity> GetFinanceContributePageList(CommonQuery query)
        {
            return FinanceContributeDataAccess.Instance.GetFinanceContributePageList(query);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<FinanceContributeEntity></returns>
        public Pager<FinanceContributeEntity> GetFinanceContributePageList(QueryBase query)
        {
            return FinanceContributeDataAccess.Instance.GetFinanceContributePageList(query);
        }

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="financeContributeQuery">FinanceContributeQuery查询实体对象</param>
        /// <returns>Pager<FinanceContributeEntity></returns>
        public Pager<FinanceContributeEntity> GetFinanceContributePageList(FinanceContributeQuery financeContributeQuery)
        {
            return FinanceContributeDataAccess.Instance.GetFinanceContributePageList(financeContributeQuery);
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="financeContribute">FinanceContributeEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddFinanceContribute(FinanceContributeEntity financeContribute)
        {
            return FinanceContributeDataAccess.Instance.AddFinanceContribute(financeContribute);
        }
        #endregion

        #region 更新一个持久化对象

        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="financeContribute">FinanceContributeEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateFinanceContribute(FinanceContributeEntity financeContribute)
        {
            return FinanceContributeDataAccess.Instance.UpdateFinanceContribute(financeContribute);
        }

        #endregion

        #region 从存储媒介中删除对象

        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteFinanceContribute(Int64 pKID)
        {
            return FinanceContributeDataAccess.Instance.DeleteFinanceContribute(pKID);
        }

        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="financeContribute">FinanceContributeEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteFinanceContribute(FinanceContributeEntity financeContribute)
        {
            return FinanceContributeDataAccess.Instance.DeleteFinanceContribute(financeContribute);
        }

        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteFinanceContribute(Int64[] pKID)
        {
            return FinanceContributeDataAccess.Instance.BatchDeleteFinanceContribute(pKID);
        }
        #endregion

        #endregion

        /// <summary>
        /// 缴费通知是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool FinanceContributeIsExists(FinanceContributeQuery query)
        {
            return FinanceContributeDataAccess.Instance.FinanceContributeIsExists(query);
        }

        /// <summary>
        /// 获取财务入款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceAccountPageList(ContributionInfoQuery query)
        {
            return FinanceContributeDataAccess.Instance.GetFinanceAccountPageList(query);
        }

        /// <summary>
        /// 获取稿费统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceGaoFeePageList(ContributionInfoQuery query)
        {
            return FinanceContributeDataAccess.Instance.GetFinanceGaoFeePageList(query);
        }

        /// <summary>
        /// 获取财务出款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceAccountEntity> GetFinanceOutAccountPageList(ContributionInfoQuery query)
        {
            return FinanceContributeDataAccess.Instance.GetFinanceOutAccountPageList(query);
        }


        /// <summary>
        /// 获取财务统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceContributeEntity> GetFinanceGlancePageList(FinanceContributeQuery query)
        {
            return FinanceContributeDataAccess.Instance.GetFinanceGlancePageList(query);
        }
        
        /// <summary>
        /// 获取版面费报表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<FinanceContributeEntity> GetFinancePageFeeReportPageList(FinanceContributeQuery query)
        {
            return FinanceContributeDataAccess.Instance.GetFinancePageFeeReportPageList(query);
        }

    }
}
