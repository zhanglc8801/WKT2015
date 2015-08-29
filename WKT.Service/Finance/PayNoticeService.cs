using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;
using System.Linq;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.BLL;
using WKT.Service.Interface;
using WKT.Common.Extension;
using WKT.Model.Enum;

namespace WKT.Service
{
    public partial class PayNoticeService : IPayNoticeService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IPayNoticeBusiness payNoticeBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IPayNoticeBusiness PayNoticeBusProvider
        {
            get
            {
                if (payNoticeBusProvider == null)
                {
                    payNoticeBusProvider = new PayNoticeBusiness();//ServiceBusContainer.Instance.Container.Resolve<IPayNoticeBusiness>();
                }
                return payNoticeBusProvider;
            }
            set
            {
                payNoticeBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PayNoticeService()
        {
        }

        #region 获取一个实体对象

        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public PayNoticeEntity GetPayNotice(Int64 noticeID)
        {
            return PayNoticeBusProvider.GetPayNotice(noticeID);
        }

        #endregion

        #region 根据条件获取所有实体对象

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<PayNoticeEntity></returns>
        public List<PayNoticeEntity> GetPayNoticeList()
        {
            return PayNoticeBusProvider.GetPayNoticeList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="payNoticeQuery">PayNoticeQuery查询实体对象</param>
        /// <returns>List<PayNoticeEntity></returns>
        public List<PayNoticeEntity> GetPayNoticeList(PayNoticeQuery payNoticeQuery)
        {
            return PayNoticeBusProvider.GetPayNoticeList(payNoticeQuery);
        }

        #endregion

        #region 根据查询条件分页获取对象

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<PayNoticeEntity></returns>
        public Pager<PayNoticeEntity> GetPayNoticePageList(CommonQuery query)
        {
            return PayNoticeBusProvider.GetPayNoticePageList(query);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<PayNoticeEntity></returns>
        public Pager<PayNoticeEntity> GetPayNoticePageList(QueryBase query)
        {
            return PayNoticeBusProvider.GetPayNoticePageList(query);
        }

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="payNoticeQuery">PayNoticeQuery查询实体对象</param>
        /// <returns>Pager<PayNoticeEntity></returns>
        public Pager<PayNoticeEntity> GetPayNoticePageList(PayNoticeQuery payNoticeQuery)
        {
            return PayNoticeBusProvider.GetPayNoticePageList(payNoticeQuery);
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
            return PayNoticeBusProvider.AddPayNotice(payNotice);
        }

        /// <summary>
        /// 批量插入 
        /// </summary>
        /// <param name="payNoticeList"></param>
        /// <returns></returns>
        public bool BatchAddPayNotice(IList<PayNoticeEntity> payNoticeList)
        {
            return PayNoticeBusProvider.BatchAddPayNotice(payNoticeList);
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
            return PayNoticeBusProvider.UpdatePayNotice(payNotice);
        }

                /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="payNoticeList"></param>
        /// <returns></returns>
        public bool BatchUpdatePayNotice(List<PayNoticeEntity> payNoticeList)
        {
            return PayNoticeBusProvider.BatchUpdatePayNotice(payNoticeList);
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
            return PayNoticeBusProvider.DeletePayNotice(noticeID);
        }

        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="payNotice">PayNoticeEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeletePayNotice(PayNoticeEntity payNotice)
        {
            return PayNoticeBusProvider.DeletePayNotice(payNotice);
        }

        #region 批量删除

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeletePayNotice(Int64[] noticeID)
        {
            return PayNoticeBusProvider.BatchDeletePayNotice(noticeID);
        }

        #endregion

        #endregion

        /// <summary>
        /// 改变缴费通知状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult ChangeStatus(PayNoticeQuery query)
        {
            ExecResult result = new ExecResult();
            bool flag = PayNoticeBusProvider.ChangeStatus(query);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "更新缴费通知状态成功！";
            }
            else
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = "更新缴费通知状态失败！";
            }
            return result;
        }

        /// <summary>
        /// 保存稿件费用信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(PayNoticeEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            model.Title = model.Title.TextFilter();
            model.Body = model.Body.HtmlFilter();
            if (model.NoticeID == 0)
            {
                PayNoticeQuery query = new PayNoticeQuery();
                query.JournalID = model.JournalID;
                query.CID = model.CID;
                query.PayType = model.PayType;
                if (PayNoticeBusProvider.PayNotinceIsExists(query))
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "该缴费通知已经存在！";
                    return execResult;
                }
                result = AddPayNotice(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增缴费通知成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增缴费通知失败！";
                }
            }
            else
            {
                result = UpdatePayNotice(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "修改缴费通知成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "修改缴费通知失败！";
                }
            }
            return execResult;
        }



        /// <summary>
        /// 保存稿件费用信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult BatchSave(IList<PayNoticeEntity> list)
        {

            ExecResult execResult = new ExecResult();
            bool result = false;
            var newList = (from data in list where data.NoticeID == 0 select data).ToList<PayNoticeEntity>();
            if (newList != null && newList.Count > 0)
            {
                IList<PayNoticeEntity> matchList = new List<PayNoticeEntity>();
                foreach (var model in newList)
                {
                    model.Title = model.Title.TextFilter();
                    model.Body = model.Body.HtmlFilter();
                    PayNoticeQuery query = new PayNoticeQuery();
                    query.JournalID = model.JournalID;
                    query.CID = model.CID;
                    query.PayType = model.PayType;
                    if (!PayNoticeBusProvider.PayNotinceIsExists(query))
                    {
                        matchList.Add(model);
                    }
                }
                result = BatchAddPayNotice(matchList);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增缴费通知成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增缴费通知失败！";
                }
            }
            var oldList = (from data in list where data.NoticeID != 0 select data).ToList<PayNoticeEntity>();
            if (oldList != null && oldList.Count > 0)
            {
                result=BatchUpdatePayNotice(oldList);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增缴费通知成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增缴费通知失败！";
                }
            }
            return execResult;
        }

        /// <summary>
        /// 删除稿件费用
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public ExecResult Del(Int64[] NoticeID)
        {
            ExecResult result = new ExecResult();
            if (NoticeID == null)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = "没有删除任何数据！";
                return result;
            }
            bool flag = BatchDeletePayNotice(NoticeID);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "删除缴费通知信息成功！";
            }
            else
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "删除缴费通知信息失败！";
            }
            return result;
        }
    }
}
