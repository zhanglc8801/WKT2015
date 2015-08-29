using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.BLL.Interface;
using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;
namespace WKT.BLL
{
    public class NoticeContentBusiness : INoticeContentBusiness
    {
        #region 获取一个实体对象

        public NoticeContentEntity GetNoticeContent(string  dictKey)
        {
            return NoticeContentDataAccess.Instance.GetNoticeContent(dictKey);
        }

        #endregion

        #region 根据条件获取所有实体对象

        public List<NoticeContentEntity> GetDictList()
        {
            return NoticeContentDataAccess.Instance.GetDictList();
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        public bool AddDict(NoticeContentEntity dictEntity)
        {
            return NoticeContentDataAccess.Instance.AddDict(dictEntity);
        }

        #endregion

        #region 更新数据

        public bool UpdateDict(NoticeContentEntity dictEntity)
        {
            return NoticeContentDataAccess.Instance.UpdateDict(dictEntity);
        }

        #endregion
    }
}
