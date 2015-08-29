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
    public partial class ContributeSetService:IContributeSetService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IContributeSetBusiness contributeSetBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IContributeSetBusiness ContributeSetBusProvider
        {
            get
            {
                 if(contributeSetBusProvider == null)
                 {
                      contributeSetBusProvider = new ContributeSetBusiness();//ServiceBusContainer.Instance.Container.Resolve<IContributeSetBusiness>();
                 }
                 return contributeSetBusProvider;
            }
            set
            {
              contributeSetBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ContributeSetService()
        {
        }

        /// <summary>
        /// 得到投稿配置信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ContributeSetEntity GetContributeSetInfo(QueryBase query)
        {
            return ContributeSetBusProvider.GetContributeSetInfo(query);
        }

        /// <summary>
        /// 更新投稿公告
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public bool UpdateStatement(ContributeSetEntity cSetEntity)
        {
            return ContributeSetBusProvider.UpdateStatement(cSetEntity);
        }

        /// <summary>
        /// 设置稿件编号格式
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public bool SetContributeNumberFormat(ContributeSetEntity cSetEntity)
        {
            return ContributeSetBusProvider.SetContributeNumberFormat(cSetEntity);
        }

        /// <summary>
        /// 得到稿件编号
        /// </summary>
        /// <param name="query">查询基类，主要用到站点ID</param>
        /// <returns></returns>
        public string GetContributeNumber(QueryBase query)
        {
            return ContributeSetBusProvider.GetContributeNumber(query);
        }

        /// <summary>
        /// 设置稿件的责任编辑
        /// </summary>
        /// <param name="setEntity"></param>
        /// <returns></returns>
        public bool SetContributeEditor(SetContributionEditorEntity setEntity)
        {
            return ContributeSetBusProvider.SetContributeEditor(setEntity);
        }
    }
}
