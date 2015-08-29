using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Data.SQL;
using WKT.Common.Utils;
using WKT.Common.Extension;
using WKT.Model;


namespace WKT.DataAccess
{
    public class NoticeContentDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db = null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public NoticeContentDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if (db == null)
                throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static NoticeContentDataAccess _instance = new NoticeContentDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static NoticeContentDataAccess Instance
        {
            get
            {

                return _instance;
            }
        }


        #region 组装SQL条件

        /// <summary>
        /// 将查询实体转换为Where语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Where语句，不包含Where</returns>
        /// </summary>
        public string DictQueryToSQLWhere(NoticeContentQuery query)
        {
            string where = "  AND DicID=" + query.DicID;
            return where;
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string DictQueryToSQLOrder(DictQuery query)
        {
            return " DictID DESC";
        }
        #endregion 组装SQL条件

        #region 获取一个实体对象

        public NoticeContentEntity GetNoticeContent(string dicKey)
        {
            NoticeContentEntity dictEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  DicID,DicKey,Content,AddTime,UpdateTime FROM dbo.NoticeContent WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  DicKey=@DicKey");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@DicKey", DbType.String, dicKey);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                dictEntity = MakeDict(dr);
            }
            return dictEntity;
        }

        #endregion

        #region 根据条件获取所有实体对象

        public List<NoticeContentEntity> GetDictList()
        {
            List<NoticeContentEntity> dictEntity = new List<NoticeContentEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  DicID,Content FROM dbo.NoticeContent WITH(NOLOCK)");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                dictEntity = MakeDictList(dr);
            }
            return dictEntity;
        }

        #endregion

        #region 持久化一个新对象（保存新对象到存储媒介中）

        public bool AddDict(NoticeContentEntity dictEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @DicID");
            sqlCommandText.Append(", @DicKey");
            sqlCommandText.Append(", @Content");
            sqlCommandText.Append(", @AddTime");
            sqlCommandText.Append(", @UpdateTime");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.NoticeContent ({0}) VALUES ({1})", sqlCommandText.ToString().Replace("@", ""), sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@DicID", DbType.Int64, dictEntity.DicID);
            db.AddInParameter(cmd, "@DicKey", DbType.String, dictEntity.DicKey);
            db.AddInParameter(cmd, "@Content", DbType.AnsiString, dictEntity.Content);
            db.AddInParameter(cmd, "@AddTime", DbType.DateTime, dictEntity.AddTime);
            db.AddInParameter(cmd, "@UpdateTime", DbType.DateTime, dictEntity.UpdateTime);
            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }

        #endregion

        #region 更新数据

        public bool UpdateDict(NoticeContentEntity dictEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  DicKey=@DicKey");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" Content=@Content");
            sqlCommandText.Append(", UpdateTime=getdate()");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.NoticeContent SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            db.AddInParameter(cmd, "@DicKey", DbType.Int64, dictEntity.DicKey);
            db.AddInParameter(cmd, "@Content", DbType.AnsiString, dictEntity.Content);

            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }

        #endregion

        #region 根据数据组装一个对象

        public NoticeContentEntity MakeDict(IDataReader dr)
        {
            NoticeContentEntity dictEntity = null;
            if (dr.Read())
            {
                dictEntity = new NoticeContentEntity();
                dictEntity.DicID = Convert.ToInt32(dr["DicID"]);
                dictEntity.DicKey = (string)dr["DicKey"];
                dictEntity.Content = Convert.ToString(dr["Content"]);
                dictEntity.AddTime = (DateTime)dr["AddTime"];
                dictEntity.UpdateTime = (DateTime)dr["UpdateTime"];
            }
            dr.Close();
            return dictEntity;
        }

        public NoticeContentEntity MakeDict(DataRow dr)
        {
            NoticeContentEntity dictEntity = null;
            if (dr != null)
            {
                dictEntity = new NoticeContentEntity();
                dictEntity.DicID = Convert.ToInt32(dr["DicID"]);
                dictEntity.DicKey = (string)dr["DicKey"];
                dictEntity.Content = Convert.ToString(dr["Content"]);
                dictEntity.AddTime = (DateTime)dr["AddTime"];
                dictEntity.UpdateTime = (DateTime)dr["UpdateTime"];
            }
            return dictEntity;
        }
        #endregion

        #region 根据数据组装一组对象数据

        public List<NoticeContentEntity> MakeDictList(IDataReader dr)
        {
            List<NoticeContentEntity> list = new List<NoticeContentEntity>();
            while (dr.Read())
            {
                NoticeContentEntity dictEntity = new NoticeContentEntity();
                dictEntity.DicID = Convert.ToInt32(dr["DicID"]);
                dictEntity.DicKey = (string)dr["DicKey"];
                dictEntity.Content = Convert.ToString(dr["Content"]);
                dictEntity.AddTime = (DateTime)dr["AddTime"];
                dictEntity.UpdateTime = (DateTime)dr["UpdateTime"];
                list.Add(dictEntity);
            }
            dr.Close();
            return list;
        }


        public List<NoticeContentEntity> MakeDictList(DataTable dt)
        {
            List<NoticeContentEntity> list = new List<NoticeContentEntity>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    NoticeContentEntity dictEntity = MakeDict(dt.Rows[i]);
                    list.Add(dictEntity);
                }
            }
            return list;
        }

        #endregion
    }
}
