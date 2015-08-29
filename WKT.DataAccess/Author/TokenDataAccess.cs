using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Data.SQL;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class TokenDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public TokenDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static TokenDataAccess _instance = new TokenDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static TokenDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }
        
        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="tokenID"></param>
        /// <returns></returns>
        public TokenEntity GetToken(TokenQuery tokenQuery)
        {
            TokenEntity tokenEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 TokenID,AuthorID,Token,Type,AddDate FROM dbo.Token WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  JournalID=@JournalID AND AuthorID=@AuthorID AND AddDate>=@ExpireDate");
            if (!string.IsNullOrEmpty(tokenQuery.Token))
            {
                sqlCommandText.Append(" AND Token=@Token");
            }
            if (tokenQuery.Type > 0)
            {
                sqlCommandText.Append(" AND Type=@Type");
            }
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, tokenQuery.JournalID);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, tokenQuery.AuthorID);
            if (!string.IsNullOrEmpty(tokenQuery.Token))
            {
                db.AddInParameter(cmd, "@Token", DbType.String, tokenQuery.Token);
            }
            if (tokenQuery.Type > 0)
            {
                db.AddInParameter(cmd, "@Type", DbType.Int16, tokenQuery.Type);
            }
            db.AddInParameter(cmd, "@ExpireDate", DbType.DateTime, tokenQuery.ExpireDate);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    tokenEntity = new TokenEntity();
                    tokenEntity.TokenID = (Int64)dr["TokenID"];
                    tokenEntity.AuthorID = (Int64)dr["AuthorID"];
                    tokenEntity.Token = (String)dr["Token"];
                    tokenEntity.Type = (Byte)dr["Type"];
                    tokenEntity.AddDate = (DateTime)dr["AddDate"];
                }
                dr.Close();
            }
            return tokenEntity;
        }
        
        /// <summary>
        /// 新增令牌
        /// </summary>
        /// <param name="tokenEntity"></param>
        /// <returns></returns>
        public bool AddToken(TokenEntity tokenEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" @JournalID");
            sqlCommandText.Append(", @AuthorID");
            sqlCommandText.Append(", @Token");
            sqlCommandText.Append(", @Type");
              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.Token ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, tokenEntity.JournalID);
            db.AddInParameter(cmd,"@AuthorID",DbType.Int64,tokenEntity.AuthorID);
            db.AddInParameter(cmd,"@Token",DbType.AnsiString,tokenEntity.Token);
            db.AddInParameter(cmd,"@Type",DbType.Byte,tokenEntity.Type);
            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch(SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }
    }
}

