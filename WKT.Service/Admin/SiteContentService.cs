using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;
using WKT.Model.Enum;
using WKT.Common.Extension;

namespace WKT.Service
{
    public class SiteContentService:ISiteContentService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private ISiteContentBusiness siteContentBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public ISiteContentBusiness SiteContentBusProvider
        {
            get
            {
                if (siteContentBusProvider == null)
                {
                    siteContentBusProvider = new SiteContentBusiness();//ServiceBusContainer.Instance.Container.Resolve<IDictBusiness>();
                }
                return siteContentBusProvider;
            }
            set
            {
                siteContentBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteContentService()
        {
        }

        /// <summary>
        /// 获取资讯分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<SiteContentEntity> GetSiteContentPageList(SiteContentQuery query)
        {
            Pager<SiteContentEntity> pager= SiteContentBusProvider.GetSiteContentPageList(query);
            if (pager != null)
                pager.ItemList = GetSiteContentList(pager.ItemList, query);
            return pager;
        }

        /// <summary>
        /// 获取资讯数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<SiteContentEntity> GetSiteContentList(SiteContentQuery query)
        {
            return GetSiteContentList(SiteContentBusProvider.GetSiteContentList(query), query);
        }

        /// <summary>
        /// 获取资讯实体
        /// </summary>
        /// <param name="ContentID"></param>
        /// <returns></returns>
        public SiteContentEntity GetSiteContentModel(Int64 ContentID)
        {
            return SiteContentBusProvider.GetSiteContentModel(ContentID);
        }

        /// <summary>
        /// 新增资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddSiteContent(SiteContentEntity model)
        {
            return SiteContentBusProvider.AddSiteContent(model);
        }

        /// <summary>
        /// 编辑资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSiteContent(SiteContentEntity model)
        {
            return SiteContentBusProvider.UpdateSiteContent(model);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="noticeID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteSiteContent(Int64[] ContentID)
        {
            return SiteContentBusProvider.BatchDeleteSiteContent(ContentID);
        }

        /// <summary>
        /// 保存资讯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(SiteContentEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            model.Title = model.Title.TextFilter();
            model.Linkurl = model.Linkurl.TextFilter();
            model.TitleColor = model.TitleColor.TextFilter();
            model.Source = model.Source.TextFilter();
            model.Author = model.Author.TextFilter();
            model.Tags = model.Tags.TextFilter();
            model.Abstruct = model.Abstruct.TextFilter();
            model.Content = model.Content.HtmlFilter();
            if (model.ContentID == 0)
            {
                result = AddSiteContent(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增新闻资讯成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增新闻资讯失败！";
                }
            }
            else
            {
                result = UpdateSiteContent(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "修改新闻资讯成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "修改新闻资讯失败！";
                }
            }
            return execResult;
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IList<SiteContentEntity> GetSiteContentList(IList<SiteContentEntity> list, SiteContentQuery siteContentQuery)
        {
            if (list == null || list.Count == 0)
                return list;
            AuthorInfoService service = new AuthorInfoService();
            AuthorInfoQuery query = new AuthorInfoQuery();
            query.JournalID = siteContentQuery.JournalID;
            var dict = service.AuthorInfoBusProvider.GetAuthorDict(query);
            foreach (var mode in list)
            {
                mode.InAuthorName = dict.GetValue(mode.InAuthor, mode.InAuthor.ToString());
            }
            return list;
        }
    }
}
