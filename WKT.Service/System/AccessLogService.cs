using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

using WKT.Model;
using WKT.Service.Interface;
using WKT.Log;
using WKT.Data.MongoDB;
using WKT.Model.Enum;

namespace WKT.Service
{
    public class AccessLogService : IAccessLogService
    {
        /// <summary>
        /// 记录访问日志
        /// </summary>
        /// <param name="logEntity"></param>
        public void AddAccessLog(AccessLog logEntity)
        {
            try
            {
                using (MongoDBHelper mongoHelper = new MongoDBHelper(null,true))
                {
                    mongoHelper.InsertOne<AccessLog>(EnumMongoDBTable.AccessLog.ToString(), logEntity);
                }
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("添加访问记录出现异常：" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 获取访问日志列表
        /// </summary>
        /// <param name="GalleryID"></param>
        /// <returns></returns>
        public Pager<AccessLog> GetArtistListByGalleryID(AccessLogQuery query)
        {
            Pager<AccessLog> accessLogList = null;
            using (MongoDBHelper mongoHelper = new MongoDBHelper(null, true))
            {
                accessLogList = mongoHelper.GetAll<AccessLog>(EnumMongoDBTable.AccessLog.ToString(),
                    null, query.PageSize, query.CurrentPage, SortBy.Ascending("LogDateTime"), null);
                if (accessLogList == null)
                {
                    accessLogList = new Pager<AccessLog>();
                }
            }
            return accessLogList;
        }

    }
}
