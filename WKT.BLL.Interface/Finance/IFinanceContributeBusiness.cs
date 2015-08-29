using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface IFinanceContributeBusiness
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        FinanceContributeEntity GetFinanceContribute(Int64 pKID);
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<FinanceContributeEntity></returns>
        List<FinanceContributeEntity> GetFinanceContributeList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="financeContributeQuery">FinanceContributeQuery查询实体对象</param>
        /// <returns>List<FinanceContributeEntity></returns>
        List<FinanceContributeEntity> GetFinanceContributeList(FinanceContributeQuery financeContributeQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<FinanceContributeEntity></returns>
        Pager<FinanceContributeEntity> GetFinanceContributePageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<FinanceContributeEntity></returns>
        Pager<FinanceContributeEntity> GetFinanceContributePageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="financeContributeQuery">FinanceContributeQuery查询实体对象</param>
        /// <returns>Pager<FinanceContributeEntity></returns>
        Pager<FinanceContributeEntity> GetFinanceContributePageList(FinanceContributeQuery financeContributeQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="financeContribute">FinanceContributeEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddFinanceContribute(FinanceContributeEntity financeContribute);
        
        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="financeContribute">FinanceContributeEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateFinanceContribute(FinanceContributeEntity financeContribute);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteFinanceContribute(Int64 pKID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="financeContribute">FinanceContributeEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteFinanceContribute(FinanceContributeEntity financeContribute);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteFinanceContribute(Int64[] pKID);
        
        #endregion
        
        #endregion 

        /// <summary>
        /// 缴费通知是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        bool FinanceContributeIsExists(FinanceContributeQuery query);

        /// <summary>
        /// 获取财务入款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<FinanceAccountEntity> GetFinanceAccountPageList(ContributionInfoQuery query);

        /// <summary>
        /// 获取稿费统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<FinanceAccountEntity> GetFinanceGaoFeePageList(ContributionInfoQuery query);

        /// <summary>
        /// 获取财务出款通知分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<FinanceAccountEntity> GetFinanceOutAccountPageList(ContributionInfoQuery query);

        /// <summary>
        /// 获取财务统计一览表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<FinanceContributeEntity> GetFinanceGlancePageList(FinanceContributeQuery query);

        /// <summary>
        /// 获取版面费报表分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Pager<FinanceContributeEntity> GetFinancePageFeeReportPageList(FinanceContributeQuery query);

    }
}






