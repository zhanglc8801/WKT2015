using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class IssueDownLogBusiness : IIssueDownLogBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="downLogID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public IssueDownLogEntity GetIssueDownLog(Int64 downLogID)
        {
           return IssueDownLogDataAccess.Instance.GetIssueDownLog( downLogID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<IssueDownLogEntity></returns>
        public List<IssueDownLogEntity> GetIssueDownLogList()
        {
            return IssueDownLogDataAccess.Instance.GetIssueDownLogList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="issueDownLogQuery">IssueDownLogQuery查询实体对象</param>
        /// <returns>List<IssueDownLogEntity></returns>
        public List<IssueDownLogEntity> GetIssueDownLogList(IssueDownLogQuery issueDownLogQuery)
        {
            return IssueDownLogDataAccess.Instance.GetIssueDownLogList(issueDownLogQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<IssueDownLogEntity></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(CommonQuery query)
        {
            return IssueDownLogDataAccess.Instance.GetIssueDownLogPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<IssueDownLogEntity></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(QueryBase query)
        {
            return IssueDownLogDataAccess.Instance.GetIssueDownLogPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="issueDownLogQuery">IssueDownLogQuery查询实体对象</param>
        /// <returns>Pager<IssueDownLogEntity></returns>
        public Pager<IssueDownLogEntity> GetIssueDownLogPageList(IssueDownLogQuery issueDownLogQuery)
        {
            return IssueDownLogDataAccess.Instance.GetIssueDownLogPageList(issueDownLogQuery);
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
            return IssueDownLogDataAccess.Instance.AddIssueDownLog(issueDownLog);
        }
        #endregion
    }
}
