﻿using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class IssueViewLogBusiness : IIssueViewLogBusiness
    {
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="viewLogID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public IssueViewLogEntity GetIssueViewLog(Int64 viewLogID)
        {
           return IssueViewLogDataAccess.Instance.GetIssueViewLog( viewLogID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<IssueViewLogEntity></returns>
        public List<IssueViewLogEntity> GetIssueViewLogList()
        {
            return IssueViewLogDataAccess.Instance.GetIssueViewLogList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="issueViewLogQuery">IssueViewLogQuery查询实体对象</param>
        /// <returns>List<IssueViewLogEntity></returns>
        public List<IssueViewLogEntity> GetIssueViewLogList(IssueViewLogQuery issueViewLogQuery)
        {
            return IssueViewLogDataAccess.Instance.GetIssueViewLogList(issueViewLogQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<IssueViewLogEntity></returns>
        public Pager<IssueViewLogEntity> GetIssueViewLogPageList(CommonQuery query)
        {
            return IssueViewLogDataAccess.Instance.GetIssueViewLogPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<IssueViewLogEntity></returns>
        public Pager<IssueViewLogEntity> GetIssueViewLogPageList(QueryBase query)
        {
            return IssueViewLogDataAccess.Instance.GetIssueViewLogPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="issueViewLogQuery">IssueViewLogQuery查询实体对象</param>
        /// <returns>Pager<IssueViewLogEntity></returns>
        public Pager<IssueViewLogEntity> GetIssueViewLogPageList(IssueViewLogQuery issueViewLogQuery)
        {
            return IssueViewLogDataAccess.Instance.GetIssueViewLogPageList(issueViewLogQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="issueViewLog">IssueViewLogEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddIssueViewLog(IssueViewLogEntity issueViewLog)
        {
            return IssueViewLogDataAccess.Instance.AddIssueViewLog(issueViewLog);
        }
        #endregion
    }
}
