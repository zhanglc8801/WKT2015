using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public partial class FlowAuthAuthorBusiness : IFlowAuthAuthorBusiness
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="flowAuthAuthorEntity"></param>
        /// <returns></returns>
        public bool AddFlowAuthAuthor(FlowAuthAuthorEntity flowAuthAuthorEntity)
        {
            return FlowAuthAuthorDataAccess.Instance.AddFlowAuthAuthor(flowAuthAuthorEntity);
        }

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="flowAuthAuthorQuery">FlowAuthAuthorQuery查询实体对象</param>
        /// <returns>Pager<FlowAuthAuthorEntity></returns>
        public List<FlowAuthAuthorEntity> GetFlowAuthAuthorList(FlowAuthAuthorQuery flowAuthAuthorQuery)
        {
            return FlowAuthAuthorDataAccess.Instance.GetFlowAuthAuthorList(flowAuthAuthorQuery);
        }

        /// <summary>
        /// 设置审稿流程环节作者权限
        /// </summary>
        /// <param name="flowAuthAuthorList">审稿流程环节作者权限列表</param>
        /// <returns></returns>
        public bool SaveFlowAuthAuthor(List<FlowAuthAuthorEntity> flowAuthAuthorList)
        {
            return FlowAuthAuthorDataAccess.Instance.SaveFlowAuthAuthor(flowAuthAuthorList);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteAuthAuthor(List<FlowAuthAuthorEntity> flowAuthAuthorList)
        {
            return FlowAuthAuthorDataAccess.Instance.BatchDeleteAuthAuthor(flowAuthAuthorList);
        }
    }
}
