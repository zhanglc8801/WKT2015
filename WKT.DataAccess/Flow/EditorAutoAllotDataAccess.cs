using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Data.SQL;
using WKT.Common.Utils;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class EditorAutoAllotDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public EditorAutoAllotDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static EditorAutoAllotDataAccess _instance = new EditorAutoAllotDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static EditorAutoAllotDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }
        
        #region 得到投稿自动分配配置

        /// <summary>
        /// 得到投稿自动分配配置
        /// </summary>
        /// <param name="autoAllotQuery"></param>
        /// <returns></returns>
        public EditorAutoAllotEntity GetEditorAutoAllot(EditorAutoAllotQuery autoAllotQuery)
        {
            EditorAutoAllotEntity editorAutoAllotEntity = null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  PKID,JournalID,AllotPattern,AuthorID,OddAuthorID,AddDate FROM dbo.EditorAutoAllot WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  JournalID=@JournalID");
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, autoAllotQuery.JournalID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                editorAutoAllotEntity = MakeEditorAutoAllot(dr);
                if (editorAutoAllotEntity != null)
                {
                    // 如果是按照学科分类设置，则取学科分类的明细
                    if (editorAutoAllotEntity.AllotPattern == 1)
                    {
                        editorAutoAllotEntity.SubjectAuthorMap = new List<SubjectAuthorMapEntity>();
                        StringBuilder sqlGetSubjectCategoryText = new StringBuilder();
                        sqlGetSubjectCategoryText.Append("SELECT SubjectCategoryID,AuthorID FROM dbo.SubjectAuthorMap WITH(NOLOCK)");
                        sqlGetSubjectCategoryText.Append(" WHERE  JournalID=@JournalID");
                        DbCommand cmdGetSubjectCategory = db.GetSqlStringCommand(sqlGetSubjectCategoryText.ToString());
                        db.AddInParameter(cmdGetSubjectCategory, "@JournalID", DbType.Int64, autoAllotQuery.JournalID);
                        using (IDataReader drSubject = db.ExecuteReader(cmdGetSubjectCategory))
                        {
                            SubjectAuthorMapEntity subjectMap = null;
                            while (drSubject.Read())
                            {
                                subjectMap = new SubjectAuthorMapEntity();
                                subjectMap.SubjectCategoryID = TypeParse.ToInt(drSubject["SubjectCategoryID"]);
                                subjectMap.AuthorID = TypeParse.ToInt(drSubject["AuthorID"]);
                                subjectMap.JournalID = autoAllotQuery.JournalID;
                                editorAutoAllotEntity.SubjectAuthorMap.Add(subjectMap);
                            }
                            drSubject.Close();
                        }
                    }
                }
                dr.Close();
            }
            return editorAutoAllotEntity;
        }

        /// <summary>
        /// 得到投稿自动分配配置
        /// </summary>
        /// <param name="autoAllotQuery"></param>
        /// <returns></returns>
        public bool CheckSubjectMapIsExists(SubjectAuthorMapEntity subjectMap)
        {
            bool flag = false;
            StringBuilder sqlGetSubjectCategoryText = new StringBuilder();
            sqlGetSubjectCategoryText.Append("SELECT SubjectCategoryID,AuthorID FROM dbo.SubjectAuthorMap WITH(NOLOCK)  WHERE JournalID=@JournalID AND SubjectCategoryID=@SubjectCategoryID");
            DbCommand cmdGetSubjectCategory = db.GetSqlStringCommand(sqlGetSubjectCategoryText.ToString());
            db.AddInParameter(cmdGetSubjectCategory, "@JournalID", DbType.Int64, subjectMap.JournalID);
            db.AddInParameter(cmdGetSubjectCategory, "@SubjectCategoryID", DbType.Int64, subjectMap.SubjectCategoryID);
            using (IDataReader drSubject = db.ExecuteReader(cmdGetSubjectCategory))
            {
                if (drSubject.Read())
                {
                    flag = true;
                }
                drSubject.Close();
            }
            return flag;
        }
        
        #endregion

        # region 设置投稿自动配置

        /// <summary>
        /// 更新投稿自动配置
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public bool SetAutoAllot(EditorAutoAllotEntity cSetEntity)
        {
            bool flag = false;
            if (cSetEntity.SubjectAuthorMap != null && cSetEntity.SubjectAuthorMap.Count > 0)
            {
                flag = SaveAutoAllotTran(cSetEntity);
            }
            else
            {
                flag = SaveAutoAllot(cSetEntity);
            }
            return flag;
        }

        /// <summary>
        /// 保存投稿自动分配设置
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        private bool SaveAutoAllot(EditorAutoAllotEntity cSetEntity)
        {
            bool flag = false;
            string sql = @" IF EXISTS(SELECT TOP 1 1 FROM dbo.EditorAutoAllot e WITH(NOLOCK) WHERE e.JournalID=@JournalID)
                            BEGIN
                                UPDATE TOP(1) dbo.EditorAutoAllot SET AllotPattern=@AllotPattern,OddAuthorID=@OddAuthorID,AuthorID=@AuthorID WHERE JournalID=@JournalID
                            END
                            ELSE
                            BEGIN
                            INSERT INTO dbo.EditorAutoAllot(JournalID,AllotPattern,AuthorID,OddAuthorID) VALUES(@JournalID,@AllotPattern,@AuthorID,@OddAuthorID)
                            END";
            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, cSetEntity.JournalID);
            db.AddInParameter(cmd, "@AllotPattern", DbType.Byte, cSetEntity.AllotPattern);
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, cSetEntity.AuthorID);
            db.AddInParameter(cmd, "@OddAuthorID", DbType.Int64, cSetEntity.OddAuthorID);

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

        /// <summary>
        /// 保存投稿自动分配设置
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        private bool SaveAutoAllotTran(EditorAutoAllotEntity cSetEntity)
        {
            bool flag = false;

            IDbConnection connection = db.GetConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                # region 保存投稿自动分配设置

                string sql = @" IF EXISTS(SELECT TOP 1 1 FROM dbo.EditorAutoAllot e WITH(NOLOCK) WHERE e.JournalID=@JournalID)
                            BEGIN
                                UPDATE TOP(1) dbo.EditorAutoAllot SET AllotPattern=@AllotPattern,OddAuthorID=@OddAuthorID,AuthorID=@OddAuthorID WHERE JournalID=@JournalID
                            END
                            ELSE
                            BEGIN
                                INSERT INTO dbo.EditorAutoAllot(JournalID,AllotPattern,AuthorID,OddAuthorID) VALUES(@JournalID,@AllotPattern,@AuthorID,@OddAuthorID)
                            END";
                DbCommand cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "@JournalID", DbType.Int64, cSetEntity.JournalID);
                db.AddInParameter(cmd, "@AllotPattern", DbType.Byte, cSetEntity.AllotPattern);
                db.AddInParameter(cmd, "@AuthorID", DbType.Int64, cSetEntity.AuthorID);
                db.AddInParameter(cmd, "@OddAuthorID", DbType.Int64, cSetEntity.OddAuthorID);

                db.ExecuteNonQuery(cmd, (DbTransaction)transaction);

                # endregion

                # region 保存学科分配匹配设置

                List<SubjectAuthorMapEntity> listSubjectInsertMap = new List<SubjectAuthorMapEntity>();
                List<SubjectAuthorMapEntity> listSubjectUpdateMap = new List<SubjectAuthorMapEntity>();
                foreach (SubjectAuthorMapEntity item in cSetEntity.SubjectAuthorMap)
                {
                    bool isexists = CheckSubjectMapIsExists(item);
                    if (isexists)
                    {
                        listSubjectUpdateMap.Add(item);
                    }
                    else
                    {
                        listSubjectInsertMap.Add(item);
                    }
                }

                # region insert

                if (listSubjectInsertMap.Count > 0)
                {
                    StringBuilder sbSaveSubjectSql = new StringBuilder("");
                    sbSaveSubjectSql.Append(" INSERT INTO SubjectAuthorMap(JournalID,SubjectCategoryID,AuthorID) ");
                    sbSaveSubjectSql.Append(" VALUES ");

                    int i = 1;
                    foreach (SubjectAuthorMapEntity item in listSubjectInsertMap)
                    {
                        sbSaveSubjectSql.Append(" (@JournalID").Append(i).Append(",@SubjectCategoryID").Append(i).Append(",@AuthorID").Append(i).Append("),");
                        i++;
                    }
                    sbSaveSubjectSql.Remove(sbSaveSubjectSql.Length - 1, 1);// 移除掉最后一个,
                    i = 1;
                    DbCommand cmdSaveSubject = db.GetSqlStringCommand(sbSaveSubjectSql.ToString());
                    foreach (SubjectAuthorMapEntity item in listSubjectInsertMap)
                    {
                        db.AddInParameter(cmdSaveSubject, "@JournalID" + i.ToString(), DbType.Int64, item.JournalID);
                        db.AddInParameter(cmdSaveSubject, "@SubjectCategoryID" + i.ToString(), DbType.Int32, item.SubjectCategoryID);
                        db.AddInParameter(cmdSaveSubject, "@AuthorID" + i.ToString(), DbType.Int64, item.AuthorID);
                        i++;
                    }
                    db.ExecuteNonQuery(cmdSaveSubject, (DbTransaction)transaction);
                }

                # endregion

                # region update

                if (listSubjectUpdateMap.Count > 0)
                {
                    StringBuilder sbUpdateSubjectSql = new StringBuilder("");

                    int i = 1;
                    foreach (SubjectAuthorMapEntity item in listSubjectUpdateMap)
                    {
                        sbUpdateSubjectSql.Append("UPDATE dbo.SubjectAuthorMap SET AuthorID=@AuthorID").Append(i).Append(" WHERE JournalID=@JournalID").Append(i).Append(" AND SubjectCategoryID=@SubjectCategoryID").Append(i).Append(";");
                        i++;
                    }
                    i = 1;
                    DbCommand cmdUpdateSubject = db.GetSqlStringCommand(sbUpdateSubjectSql.ToString());
                    foreach (SubjectAuthorMapEntity item in listSubjectUpdateMap)
                    {
                        db.AddInParameter(cmdUpdateSubject, "@JournalID" + i.ToString(), DbType.Int64, item.JournalID);
                        db.AddInParameter(cmdUpdateSubject, "@SubjectCategoryID" + i.ToString(), DbType.Int32, item.SubjectCategoryID);
                        db.AddInParameter(cmdUpdateSubject, "@AuthorID" + i.ToString(), DbType.Int64, item.AuthorID);
                        i++;
                    }
                    db.ExecuteNonQuery(cmdUpdateSubject, (DbTransaction)transaction);
                }

                # endregion

                # endregion

                transaction.Commit();
                flag = true;
            }
            catch (SqlException sqlEx)
            {
                transaction.Rollback();
                throw sqlEx;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return flag;
        }

        # endregion

        # region 根据稿件设置得到稿件责任编辑

        /// <summary>
        /// 得到投稿自动分配的编辑
        /// </summary>
        /// <param name="autoAllotQuery"></param>
        /// <returns></returns>
        public AuthorInfoEntity GetAutoAllotEditor(EditorAutoAllotQuery autoAllotQuery)
        {
            AuthorInfoEntity authorInfoEntity = null;
            long AuthorID = 0;

            # region bak

            //EditorAutoAllotEntity editorAutoAllotEntity = GetEditorAutoAllot(autoAllotQuery);
            //if (editorAutoAllotEntity != null)
            //{                
            //    switch (editorAutoAllotEntity.AllotPattern)
            //    {
            //        case 1:// 按学科分类
            //            string sql = "SELECT TOP 1 AuthorID FROM dbo.SubjectAuthorMap s WITH(NOLOCK) WHERE JournalID=@JournalID AND SubjectCategoryID=@SubjectCategoryID";
            //            DbCommand cmd = db.GetSqlStringCommand(sql);
            //            db.AddInParameter(cmd, "@JournalID", DbType.Int64, autoAllotQuery.JournalID);
            //            db.AddInParameter(cmd, "@SubjectCategoryID", DbType.Int32, autoAllotQuery.SubjectCategoryID);
            //            using (IDataReader dr = db.ExecuteReader(cmd))
            //            {
            //                if (dr.Read())
            //                {
            //                    AuthorID = TypeParse.ToLong(dr["AuthorID"], 0);
            //                }
            //                dr.Close();
            //            }
            //            break;
            //        case 2:// 奇偶分配
            //            // 得到尾数
            //            string mantissa = autoAllotQuery.CNumber.Substring(autoAllotQuery.CNumber.Length - 2, 1);
            //            int evenOdd = TypeParse.ToInt(mantissa, 0) % 2; // 判断奇偶
            //            if (evenOdd == 0)
            //            {
            //                // 偶数
            //                AuthorID = editorAutoAllotEntity.AuthorID;
            //            }
            //            else
            //            {
            //                // 奇数
            //                AuthorID = editorAutoAllotEntity.OddAuthorID;
            //            }
            //            break;
            //        case 3:// 固定责编
            //            AuthorID = editorAutoAllotEntity.AuthorID;
            //            break;
            //    }
            //    if (AuthorID > 0)
            //    {
            //        AuthorInfoQuery query = new AuthorInfoQuery();
            //        query.JournalID = editorAutoAllotEntity.JournalID;
            //        query.AuthorID = AuthorID;
            //        authorInfoEntity = AuthorInfoDataAccess.Instance.GetMemberInfo(query);
            //    }
            //}

            # endregion

            DbCommand cmd = db.GetSqlStringCommand("SELECT dbo.fn_GetAutoAllotEditor(@JournalID,@CNumber,@SubjectCategoryID)");

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, autoAllotQuery.JournalID);
            db.AddInParameter(cmd, "CNumber", DbType.String, autoAllotQuery.CNumber);
            db.AddInParameter(cmd, "@SubjectCategoryID", DbType.Int32, autoAllotQuery.SubjectCategoryID);

            try
            {
                object obj = db.ExecuteScalar(cmd);
                if (obj != null)
                {
                    long.TryParse(obj.ToString(), out AuthorID);
                }
                if (AuthorID > 0)
                {
                    AuthorInfoQuery query = new AuthorInfoQuery();
                    query.JournalID = autoAllotQuery.JournalID;
                    query.AuthorID = AuthorID;
                    authorInfoEntity = AuthorInfoDataAccess.Instance.GetMemberInfo(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return authorInfoEntity;
        }

        # endregion

        #region 根据数据组装一个对象

        public EditorAutoAllotEntity MakeEditorAutoAllot(IDataReader dr)
        {
            EditorAutoAllotEntity editorAutoAllotEntity = null;
            if (dr.Read())
            {
                editorAutoAllotEntity = new EditorAutoAllotEntity();
                editorAutoAllotEntity.PKID = (Int64)dr["PKID"];
                editorAutoAllotEntity.JournalID = (Int64)dr["JournalID"];
                editorAutoAllotEntity.AllotPattern = (Byte)dr["AllotPattern"];
                editorAutoAllotEntity.AuthorID = (Int64)dr["AuthorID"];
                editorAutoAllotEntity.OddAuthorID = (Int64)dr["OddAuthorID"];
                editorAutoAllotEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return editorAutoAllotEntity;
        }
        
        #endregion
    }
}

