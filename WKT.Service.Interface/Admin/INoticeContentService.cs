using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface INoticeContentService
    {
        #region 获取一个实体对象

        NoticeContentEntity GetNoticeContent(string dictKey);

        #endregion

        #region 根据条件获取所有实体对象

        List<NoticeContentEntity> GetDictList();

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        bool AddDict(NoticeContentEntity dictEntity);

        #endregion

        #region 更新数据

        bool UpdateDict(NoticeContentEntity dictEntity);

        #endregion
    }
}
