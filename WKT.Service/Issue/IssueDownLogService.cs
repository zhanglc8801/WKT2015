using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;

namespace WKT.Service
{
    public partial class IssueDownLogService : IIssueDownLogService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IIssueDownLogBusiness issueDownLogBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IIssueDownLogBusiness IssueDownLogBusProvider
        {
            get
            {
                 if(issueDownLogBusProvider == null)
                 {
                      issueDownLogBusProvider = new IssueDownLogBusiness();//ServiceBusContainer.Instance.Container.Resolve<IIssueDownLogBusiness>();
                 }
                 return issueDownLogBusProvider;
            }
            set
            {
              issueDownLogBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public IssueDownLogService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="downLogID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public IssueDownLogEntity GetIssueDownLog(Int64 downLogID)
        {
           return IssueDownLogBusProvider.GetIssueDownLog( downLogID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<IssueDownLogEntity></returns>
        public List<IssueDownLogEntity> GetIssueDownLogList()
        {
            return IssueDownLogBusProvider.GetIssueDownLogList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="issueDownLogQuery">IssueDownLogQuery查询实体对象</param>
        /// <returns>List<IssueDownLogEntity></returns>
        public List<IssueDownLogEntity> GetIssueDownLogList(IssueDownLogQuery issueDownLogQuery)
        {
            return IssueDownLogBusProvider.GetIssueDownLogList(issueDownLogQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<IssueDownLogEntity></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(CommonQuery query)
        {
            return IssueDownLogBusProvider.GetIssueDownLogPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<IssueDownLogEntity></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(QueryBase query)
        {
            return IssueDownLogBusProvider.GetIssueDownLogPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="issueDownLogQuery">IssueDownLogQuery查询实体对象</param>
        /// <returns>Pager<IssueDownLogEntity></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(IssueDownLogQuery issueDownLogQuery)
        {
            return IssueDownLogBusProvider.GetIssueDownLogPageList(issueDownLogQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="issueDownLog">IssueDownLogEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddIssueDownLog(IssueDownLogEntity issueDownLog)
        {
            return IssueDownLogBusProvider.AddIssueDownLog(issueDownLog);
        }
        
        #endregion

    }
}
