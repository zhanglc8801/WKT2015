using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class PayNoticeBusiness : IPayNoticeBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public PayNoticeEntity GetPayNotice(Int64 noticeID)
        {
           return PayNoticeDataAccess.Instance.GetPayNotice( noticeID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<PayNoticeEntity></returns>
        public List<PayNoticeEntity> GetPayNoticeList()
        {
            return PayNoticeDataAccess.Instance.GetPayNoticeList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="payNoticeQuery">PayNoticeQuery查询实体对象</param>
        /// <returns>List<PayNoticeEntity></returns>
        public List<PayNoticeEntity> GetPayNoticeList(PayNoticeQuery payNoticeQuery)
        {
            return PayNoticeDataAccess.Instance.GetPayNoticeList(payNoticeQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<PayNoticeEntity></returns>
        public Pager<PayNoticeEntity> GetPayNoticePageList(CommonQuery query)
        {
            return PayNoticeDataAccess.Instance.GetPayNoticePageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<PayNoticeEntity></returns>
        public Pager<PayNoticeEntity> GetPayNoticePageList(QueryBase query)
        {
            return PayNoticeDataAccess.Instance.GetPayNoticePageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="payNoticeQuery">PayNoticeQuery查询实体对象</param>
        /// <returns>Pager<PayNoticeEntity></returns>
        public Pager<PayNoticeEntity> GetPayNoticePageList(PayNoticeQuery payNoticeQuery)
        {
            return PayNoticeDataAccess.Instance.GetPayNoticePageList(payNoticeQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="payNotice">PayNoticeEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddPayNotice(PayNoticeEntity payNotice)
        {
            return PayNoticeDataAccess.Instance.AddPayNotice(payNotice);
        }

        /// <summary>
        ///  批量插入
        /// </summary>
        /// <param name="payNoticeList"></param>
        /// <returns></returns>
        public bool BatchAddPayNotice(IList<PayNoticeEntity> payNoticeList)
        {
            return PayNoticeDataAccess.Instance.BatchAddPayNotice(payNoticeList);
        }
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="payNotice">PayNoticeEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdatePayNotice(PayNoticeEntity payNotice)
        {
            return PayNoticeDataAccess.Instance.UpdatePayNotice(payNotice);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="payNoticeList"></param>
        /// <returns></returns>
        public bool BatchUpdatePayNotice(List<PayNoticeEntity> payNoticeList)
        {
            return PayNoticeDataAccess.Instance.BatchUpdatePayNotice(payNoticeList);
        }

        #endregion 

        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeletePayNotice(Int64 noticeID)
        {
            return PayNoticeDataAccess.Instance.DeletePayNotice( noticeID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="payNotice">PayNoticeEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeletePayNotice(PayNoticeEntity payNotice)
        {
            return PayNoticeDataAccess.Instance.DeletePayNotice(payNotice);
        }
        
        #region 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeletePayNotice(Int64[] noticeID)
        {
            return PayNoticeDataAccess.Instance.BatchDeletePayNotice( noticeID);
        }
        #endregion
        
        #endregion

        /// <summary>
        /// 改变缴费通知状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool ChangeStatus(PayNoticeQuery query)
        {
            return PayNoticeDataAccess.Instance.ChangeStatus(query);
        }

        /// <summary>
        /// 缴费通知是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool PayNotinceIsExists(PayNoticeQuery query)
        {
            return PayNoticeDataAccess.Instance.PayNotinceIsExists(query);
        }

    }
}
