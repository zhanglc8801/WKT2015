using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface IFinancePayDetailService
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="billID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        FinancePayDetailEntity GetFinancePayDetail(Int64 billID);
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<FinancePayDetailEntity></returns>
        List<FinancePayDetailEntity> GetFinancePayDetailList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="financePayDetailQuery">FinancePayDetailQuery查询实体对象</param>
        /// <returns>List<FinancePayDetailEntity></returns>
        List<FinancePayDetailEntity> GetFinancePayDetailList(FinancePayDetailQuery financePayDetailQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<FinancePayDetailEntity></returns>
        Pager<FinancePayDetailEntity> GetFinancePayDetailPageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<FinancePayDetailEntity></returns>
        Pager<FinancePayDetailEntity> GetFinancePayDetailPageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="financePayDetailQuery">FinancePayDetailQuery查询实体对象</param>
        /// <returns>Pager<FinancePayDetailEntity></returns>
        Pager<FinancePayDetailEntity> GetFinancePayDetailPageList(FinancePayDetailQuery financePayDetailQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="financePayDetail">FinancePayDetailEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddFinancePayDetail(FinancePayDetailEntity financePayDetail);
        
        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="financePayDetail">FinancePayDetailEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateFinancePayDetail(FinancePayDetailEntity financePayDetail);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="billID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteFinancePayDetail(Int64 billID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="financePayDetail">FinancePayDetailEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteFinancePayDetail(FinancePayDetailEntity financePayDetail);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="billID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteFinancePayDetail(Int64[] billID);
        
        #endregion
        
        #endregion 
    }
}






