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
    public partial class FlowAuthAuthorService:IFlowAuthAuthorService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IFlowAuthAuthorBusiness flowAuthAuthorBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IFlowAuthAuthorBusiness FlowAuthAuthorBusProvider
        {
            get
            {
                 if(flowAuthAuthorBusProvider == null)
                 {
                      flowAuthAuthorBusProvider = new FlowAuthAuthorBusiness();//ServiceBusContainer.Instance.Container.Resolve<IFlowAuthAuthorBusiness>();
                 }
                 return flowAuthAuthorBusProvider;
            }
            set
            {
              flowAuthAuthorBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FlowAuthAuthorService()
        {
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="flowAuthAuthorQuery">FlowAuthAuthorQuery查询实体对象</param>
        /// <returns>Pager<FlowAuthAuthorEntity></returns>
        public List<FlowAuthAuthorEntity> GetFlowAuthAuthorList(FlowAuthAuthorQuery flowAuthAuthorQuery)
        {
            return FlowAuthAuthorBusProvider.GetFlowAuthAuthorList(flowAuthAuthorQuery);
        }

        /// <summary>
        /// 设置审稿流程环节作者权限
        /// </summary>
        /// <param name="flowAuthAuthorList">审稿流程环节作者权限列表</param>
        /// <returns></returns>
        public bool SaveFlowAuthAuthor(List<FlowAuthAuthorEntity> flowAuthAuthorList)
        {
            return FlowAuthAuthorBusProvider.SaveFlowAuthAuthor(flowAuthAuthorList);
        }
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteAuthAuthor(List<FlowAuthAuthorEntity> flowAuthAuthorList)
        {
            return FlowAuthAuthorBusProvider.BatchDeleteAuthAuthor(flowAuthAuthorList);
        }
    }
}
