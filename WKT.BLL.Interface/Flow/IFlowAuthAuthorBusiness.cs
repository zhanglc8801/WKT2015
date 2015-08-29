using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface IFlowAuthAuthorBusiness
    { 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="flowAuthAuthorEntity"></param>
        /// <returns></returns>
        bool AddFlowAuthAuthor(FlowAuthAuthorEntity flowAuthAuthorEntity);

        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="flowAuthAuthorQuery">FlowAuthAuthorQuery查询实体对象</param>
        /// <returns>Pager<FlowAuthAuthorEntity></returns>
        List<FlowAuthAuthorEntity> GetFlowAuthAuthorList(FlowAuthAuthorQuery flowAuthAuthorQuery);
                
        /// <summary>
        /// 设置审稿流程环节作者权限
        /// </summary>
        /// <param name="flowAuthAuthorList">审稿流程环节作者权限列表</param>
        /// <returns></returns>
        bool SaveFlowAuthAuthor(List<FlowAuthAuthorEntity> flowAuthAuthorList);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteAuthAuthor(List<FlowAuthAuthorEntity> flowAuthAuthorList);
    }
}






