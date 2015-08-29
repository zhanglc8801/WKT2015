using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;
using WKT.Model.Enum;
using WKT.Common.Extension;

namespace WKT.Service
{
    public class SiteBlockService : ISiteBlockService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private ISiteBlockBusiness siteBlockBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public ISiteBlockBusiness SiteBlockBusProvider
        {
            get
            {
                if (siteBlockBusProvider == null)
                {
                    siteBlockBusProvider = new SiteBlockBusiness();//ServiceBusContainer.Instance.Container.Resolve<IDictBusiness>();
                }
                return siteBlockBusProvider;
            }
            set
            {
                siteBlockBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteBlockService()
        {
        }

        /// <summary>
        /// 获取内容块分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteBlockEntity> GetSiteBlockPageList(SiteBlockQuery query)
        {
            return SiteBlockBusProvider.GetSiteBlockPageList(query);
        }

        /// <summary>
        /// 获取内容块数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteBlockEntity> GetSiteBlockList(SiteBlockQuery query)
        {
            return SiteBlockBusProvider.GetSiteBlockList(query);
        }


        /// <summary>
        /// 获取内容块实体
        /// </summary>
        /// <param name="BlockID"></param>
        /// <returns></returns>
        public SiteBlockEntity GetSiteBlockModel(Int64 BlockID)
        {
            return SiteBlockBusProvider.GetSiteBlockModel(BlockID);
        }

        /// <summary>
        /// 新增内容块
        /// </summary>
        /// <param name="SiteBlockEntity"></param>
        /// <returns></returns>
        public bool AddSiteBlock(SiteBlockEntity model)
        {
            return SiteBlockBusProvider.AddSiteBlock(model);
        }

        /// <summary>
        /// 编辑内容块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSiteBlock(SiteBlockEntity model)
        {
            return SiteBlockBusProvider.UpdateSiteBlock(model);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="BlockID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteBlock(Int64[] BlockID)
        {
            return SiteBlockBusProvider.BatchDeleteSiteBlock(BlockID);
        }

        /// <summary>
        /// 保存内容块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(SiteBlockEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            model.Title = model.Title.TextFilter();
            model.Linkurl = model.Linkurl.TextFilter();
            model.Note = model.Note.TextFilter();
            if (model.BlockID == 0)
            {
                result = AddSiteBlock(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增内容块成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增内容块失败！";
                }
            }
            else
            {
                result = UpdateSiteBlock(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "修改内容块成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "修改内容块失败！";
                }
            }
            return execResult;
        }
    }
}
