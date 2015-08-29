using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

using WKT.Model;

namespace WKT.Data.MongoDB
{
    public sealed class MongoDBHelper : IDisposable
    {
        public static readonly string defaultConnectionstring = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
        public static readonly string defaultDBName = ConfigurationManager.AppSettings["MongoDB"];

        private bool isDisposed = false;
        private MongoServer server = null;
        private MongoDatabase database = null;

        # region 构造函数

        public MongoDBHelper()
        {
            server = MongoServer.Create(defaultConnectionstring);
            //获取数据库或者创建数据库（不存在的话）。
            database = server.GetDatabase(defaultDBName);
        }

        /// <summary>
        /// 使用安全认证
        /// </summary>
        /// <param name="isHaveAuth">没有实际意义</param>
        /// <param name="mongoConn"></param>
        public MongoDBHelper(MongoDBConnnection mongoConn, bool isHaveAuth)
        {
            if (mongoConn == null)
            {
                mongoConn = new MongoDBConnnection();
            }
            server = MongoServer.Create(new MongoServerSettings
            {
                Server = new MongoServerAddress(mongoConn.Host, mongoConn.Port),
                SafeMode = SafeMode.True,
                DefaultCredentials = new MongoCredentials(mongoConn.UserName, mongoConn.Password)
            });
            //获取数据库或者创建数据库（不存在的话）。
            database = server.GetDatabase(defaultDBName);
        }

        /// <summary>
        /// 安全认证
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public MongoDBHelper(string host, int port, string userName, string password)
        {
            server = MongoServer.Create(new MongoServerSettings
            {
                Server = new MongoServerAddress(host, port),
                SafeMode = SafeMode.True,
                DefaultCredentials = new MongoCredentials(userName, password)
            });
            //获取数据库或者创建数据库（不存在的话）。
            database = server.GetDatabase(defaultDBName);
        }

        public MongoDBHelper(string host, int port, string dbname, string username, string password)
        {
            server = MongoServer.Create(new MongoServerSettings
            {
                Server = new MongoServerAddress(host, port),
                SafeMode = SafeMode.True,
                DefaultCredentials = new MongoCredentials(username, password)
            });
            //获取数据库或者创建数据库（不存在的话）。
            database = server.GetDatabase(dbname);
        }

        public MongoDBHelper(string _connstr)
        {
            server = MongoServer.Create(_connstr);
            //获取数据库或者创建数据库（不存在的话）。
            database = server.GetDatabase(defaultDBName);
        }

        public MongoDBHelper(string _connstr, string dbname)
        {
            server = MongoServer.Create(_connstr);
            //获取数据库或者创建数据库（不存在的话）。
            database = server.GetDatabase(dbname);
        }

        # endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this.isDisposed)
                return;
            if (this.server != null && this.server.State == MongoServerState.Connected)
            {
                this.server.Disconnect();
            }
            GC.SuppressFinalize(this);
            this.isDisposed = true;
        }

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public SafeModeResult InsertOne<T>(string collectionName, T entity)
        {
            SafeModeResult result = new SafeModeResult();

            if (null == entity)
            {
                return null;
            }

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.Insert(entity);
            }
            return result;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public IEnumerable<SafeModeResult> InsertAll<T>(string collectionName, IEnumerable<T> entitys)
        {
            IEnumerable<SafeModeResult> result = null;
            if (null == entitys)
            {
                return null;
            }
            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.InsertBatch(entitys);
            }
            return result;
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public SafeModeResult UpdateOne<T>(string collectionName, T entity)
        {
            SafeModeResult result = null;

            //开始连接数据库
            using (server.RequestStart(database))
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.Save(entity);
            }

            return result;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒") 或者 Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="update">更新设置。调用示例：Update.Set("Title", "yanc") 或者 Update.Set("Title", "yanc").Set("Author", "yanc2") 等等</param>
        /// <returns></returns>
        public SafeModeResult UpdateAll<T>(string collectionName, IMongoQuery query, IMongoUpdate update)
        {
            SafeModeResult result;

            if (null == query || null == update)
            {
                return null;
            }

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.Update(query, update, UpdateFlags.Multi);
            }

            return result;
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒") 或者 Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public SafeModeResult Delete(string collectionName, IMongoQuery query)
        {
            SafeModeResult result = null;

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.Remove(query);
            }

            return result;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public SafeModeResult DeleteAll(string collectionName)
        {
            SafeModeResult result = null;

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.RemoveAll();
            }

            return result;
        }

        #endregion

        #region 获取单条信息

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒") 或者 Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public T GetOne<T>(string collectionName, IMongoQuery query)
        {
            T result = default(T);

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                if (null == query)
                {
                    result = myCollection.FindOneAs<T>();
                }
                else
                {
                    result = myCollection.FindOneAs<T>(query);
                }
            }
            return result;
        }

        #endregion

        #region 获取多个

        /// <summary>
        /// 如果不清楚具体的数量，一般不要用这个函数。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public List<T> GetAll<T>(string collectionName)
        {
            List<T> result = new List<T>();
            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                foreach (T entity in myCollection.FindAllAs<T>())
                {
                    result.Add(entity);
                }
            }

            return result;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="count"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒") 或者 Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public List<T> GetAll<T>(string collectionName, IMongoQuery query, string[] getfileds)
        {
            List<T> result = new List<T>();

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                MongoCursor<T> myCursor;

                if (null == query)
                {
                    myCursor = myCollection.FindAllAs<T>();
                }
                else
                {
                    myCursor = myCollection.FindAs<T>(query);
                }

                if (null != getfileds)
                {
                    myCursor.SetFields(getfileds);
                }

                foreach (T entity in myCursor)
                {
                    result.Add(entity);
                }
            }
            return result;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T> GetAll<T>(string collectionName, int count)
        {
            return this.GetAll<T>(collectionName, count, null);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="count"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒") 或者 Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public List<T> GetAll<T>(string collectionName, int count, IMongoQuery query)
        {
            return this.GetAll<T>(collectionName, count, query, null);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="count"></param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title") 或者 SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <returns></returns>
        public List<T> GetAll<T>(string collectionName, int count, IMongoQuery query, IMongoSortBy sortBy)
        {
            Pager<T> pagerInfo = new Pager<T>();
            pagerInfo.CurrentPage = 1;
            pagerInfo.PageSize = count;
            return this.GetAll<T>(collectionName, query, pagerInfo, sortBy, null);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒") 或者 Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pagerInfo"></param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title") 或者 SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="fields">只返回所需要的字段的数据。调用示例："Title" 或者 new string[] { "Title", "Author" }等等</param>
        /// <returns></returns>
        public List<T> GetAll<T>(string collectionName, IMongoQuery query, Pager<T> pagerInfo, IMongoSortBy sortBy, params string[] fields)
        {
            List<T> result = new List<T>();

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                MongoCursor<T> myCursor;

                if (null == query)
                {
                    myCursor = myCollection.FindAllAs<T>();
                }
                else
                {
                    myCursor = myCollection.FindAs<T>(query);
                }

                if (null != sortBy)
                {
                    myCursor.SetSortOrder(sortBy);
                }

                if (null != fields)
                {
                    myCursor.SetFields(fields);
                }

                foreach (T entity in myCursor.SetSkip((pagerInfo.CurrentPage - 1) * pagerInfo.PageSize).SetLimit(pagerInfo.PageSize))//.SetSkip(100).SetLimit(10)是指读取第一百条后的10条数据。
                {
                    result.Add(entity);
                }
            }

            return result;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒") 或者 Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pagerInfo"></param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title") 或者 SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="fields">只返回所需要的字段的数据。调用示例："Title" 或者 new string[] { "Title", "Author" }等等</param>
        /// <returns></returns>
        public Pager<T> GetAll<T>(string collectionName, IMongoQuery query, int PageSize, int PageIndex, IMongoSortBy sortBy, params string[] fields)
        {
            Pager<T> pagerReturn = new Pager<T>();
            List<T> result = new List<T>();

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                MongoCursor<T> myCursor;

                if (null == query)
                {
                    myCursor = myCollection.FindAllAs<T>();
                }
                else
                {
                    myCursor = myCollection.FindAs<T>(query);
                }

                if (null != sortBy)
                {
                    myCursor.SetSortOrder(sortBy);
                }

                if (null != fields)
                {
                    myCursor.SetFields(fields);
                }

                foreach (T entity in myCursor.SetSkip((PageIndex - 1) * PageSize).SetLimit(PageSize))//.SetSkip(100).SetLimit(10)是指读取第一百条后的10条数据。
                {
                    result.Add(entity);
                }
                if (myCursor != null)
                {
                    pagerReturn.TotalRecords = myCursor.Count();
                }
                pagerReturn.ItemList = result;
            }

            return pagerReturn;
        }

        #endregion

        # region 获取指定查询条件，指定值所在第几页

        /// <summary>
        /// 分页查询
        /// </summary>        
        /// <param name="collectionName"></param>
        /// <param name="ID">得到该ID所在页码</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒") 或者 Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        public int GetPageIndex(string collectionName, string keyfiledname, int keyvalue, IMongoQuery query, int pageSize, EnumOperator enumOper)
        {
            int pageIndex = 0;

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                MongoCursor myCursor;
                QueryConditionList coditionList = null;
                switch (enumOper)
                {
                    case EnumOperator.GT:
                        coditionList = Query.GT(keyfiledname, BsonValue.Create(keyvalue));
                        break;
                    case EnumOperator.GTE:
                        coditionList = Query.GTE(keyfiledname, BsonValue.Create(keyvalue));
                        break;
                    case EnumOperator.LT:
                        coditionList = Query.LT(keyfiledname, BsonValue.Create(keyvalue));
                        break;
                    case EnumOperator.LTE:
                        coditionList = Query.LTE(keyfiledname, BsonValue.Create(keyvalue));
                        break;
                }
                if (null == query)
                {
                    myCursor = myCollection.Find(coditionList);
                }
                else
                {
                    myCursor = myCollection.Find(Query.And(query, coditionList));
                }
                long total = myCursor.Count();
                if (total > 0)
                {
                    pageIndex = (int)total / pageSize;
                    if ((total % pageSize) > 0)
                        pageIndex++;
                }
            }

            return pageIndex;
        }

        # endregion

        #region 索引

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="keyNames"></param>
        public void CreateIndex(string collectionName, params string[] keyNames)
        {
            SafeModeResult result = new SafeModeResult();

            if (null == keyNames)
            {
                return;
            }

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                if (!myCollection.IndexExists(keyNames))
                {
                    myCollection.EnsureIndex(keyNames);
                }
            }

        }
        #endregion
    }
}
