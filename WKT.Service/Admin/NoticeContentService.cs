using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Service.Interface;
using WKT.BLL.Interface;
using WKT.BLL;
using WKT.Model;

namespace WKT.Service
{
    public class NoticeContentService : INoticeContentService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private INoticeContentBusiness noticeContentBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public INoticeContentBusiness NoticeContentBusProvider
        {
            get
            {
                if (noticeContentBusProvider == null)
                {
                    noticeContentBusProvider = new NoticeContentBusiness();//ServiceBusContainer.Instance.Container.Resolve<IAuthorInfoBusiness>();
                }
                return noticeContentBusProvider;
            }
            set
            {
                noticeContentBusProvider = value;
            }
        }
        public NoticeContentEntity GetNoticeContent(string dictKey)
        {
            return NoticeContentBusProvider.GetNoticeContent(dictKey);
        }

        public List<NoticeContentEntity> GetDictList()
        {
            throw new NotImplementedException();
        }

        public bool AddDict(NoticeContentEntity dictEntity)
        {
            return NoticeContentBusProvider.AddDict(dictEntity);
        }

        public bool UpdateDict(NoticeContentEntity dictEntity)
        {
            return NoticeContentBusProvider.UpdateDict(dictEntity);
        }
    }
}
