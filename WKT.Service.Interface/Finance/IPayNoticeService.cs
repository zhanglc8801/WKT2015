using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface IPayNoticeService
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        PayNoticeEntity GetPayNotice(Int64 noticeID);
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<PayNoticeEntity></returns>
        List<PayNoticeEntity> GetPayNoticeList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="payNoticeQuery">PayNoticeQuery查询实体对象</param>
        /// <returns>List<PayNoticeEntity></returns>
        List<PayNoticeEntity> GetPayNoticeList(PayNoticeQuery payNoticeQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<PayNoticeEntity></returns>
        Pager<PayNoticeEntity> GetPayNoticePageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<PayNoticeEntity></returns>
        Pager<PayNoticeEntity> GetPayNoticePageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="payNoticeQuery">PayNoticeQuery查询实体对象</param>
        /// <returns>Pager<PayNoticeEntity></returns>
        Pager<PayNoticeEntity> GetPayNoticePageList(PayNoticeQuery payNoticeQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="payNotice">PayNoticeEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddPayNotice(PayNoticeEntity payNotice);

        bool BatchAddPayNotice(IList<PayNoticeEntity> payNoticeList);

        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="payNotice">PayNoticeEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdatePayNotice(PayNoticeEntity payNotice);


                /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="payNoticeList"></param>
        /// <returns></returns>
        bool BatchUpdatePayNotice(List<PayNoticeEntity> payNoticeList);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeletePayNotice(Int64 noticeID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="payNotice">PayNoticeEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeletePayNotice(PayNoticeEntity payNotice);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeletePayNotice(Int64[] noticeID);
        
        #endregion
        
        #endregion 

        /// <summary>
        /// 改变缴费通知状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ExecResult ChangeStatus(PayNoticeQuery query);

        /// <summary>
        /// 保存稿件费用信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult Save(PayNoticeEntity model);

        /// <summary>
        /// 保存稿件费用信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult BatchSave(IList<PayNoticeEntity> list);

        /// <summary>
        /// 删除稿件费用
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        ExecResult Del(Int64[] NoticeID);
    }
}






