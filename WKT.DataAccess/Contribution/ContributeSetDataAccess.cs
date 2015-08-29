using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Data.SQL;
using WKT.Common.Extension;

namespace WKT.DataAccess
{
    public partial class ContributeSetDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db = null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public ContributeSetDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if (db == null)
                throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static ContributeSetDataAccess _instance = new ContributeSetDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static ContributeSetDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }

        # region 得到投稿配置信息

        /// <summary>
        /// 得到投稿配置信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ContributeSetEntity GetContributeSetInfo(QueryBase query)
        {
            ContributeSetEntity cSetEntity = new ContributeSetEntity();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 [CSetID],[JournalID],[CCodeType],[CCodeFormat],[Separator],[RandomDigit],[Statement] FROM dbo.[ContributeSet] WITH(NOLOCK) WHERE JournalID=@JournalID");

            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    cSetEntity = new ContributeSetEntity();
                    cSetEntity.CSetID = (Int64)dr["CSetID"];
                    cSetEntity.JournalID = (Int64)dr["JournalID"];
                    cSetEntity.CCodeType = (Byte)dr["CCodeType"];
                    cSetEntity.CCodeFormat = (String)dr["CCodeFormat"];
                    cSetEntity.Separator = (String)dr["Separator"];
                    cSetEntity.RandomDigit = (Byte)dr["RandomDigit"];
                    cSetEntity.Statement = (String)dr["Statement"];
                }
                dr.Close();
            }
            return cSetEntity;
        }

        # endregion

        # region 更新投稿公告

        /// <summary>
        /// 更新投稿公告
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public bool UpdateStatement(ContributeSetEntity cSetEntity)
        {
            bool flag = false;
            string sql = @" IF EXISTS(SELECT TOP 1 1 FROM dbo.ContributeSet c WITH(NOLOCK) WHERE c.JournalID=@JournalID)
                            BEGIN
	                            UPDATE TOP(1) dbo.ContributeSet SET [Statement]=@Statement WHERE JournalID=@JournalID
                            END
                            ELSE
                            BEGIN
                            INSERT INTO dbo.ContributeSet(JournalID,CCodeType,CCodeFormat,Separator,RandomDigit,Statement) VALUES(@JournalID,3,'','-','3',@Statement)
                            END";
            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, cSetEntity.JournalID);
            db.AddInParameter(cmd, "@Statement", DbType.AnsiString, cSetEntity.Statement.HtmlFilter());

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

        # endregion

        # region 设置稿件编号格式

        /// <summary>
        /// 设置稿件编号格式
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public bool SetContributeNumberFormat(ContributeSetEntity cSetEntity)
        {
            bool flag = false;
            string sql = @" IF EXISTS(SELECT TOP 1 1 FROM dbo.ContributeSet c WITH(NOLOCK) WHERE c.JournalID=@JournalID)
                            BEGIN
	                            UPDATE TOP(1) dbo.ContributeSet SET CCodeType=@CCodeType,CCodeFormat=@CCodeFormat,Separator=@Separator,RandomDigit=@RandomDigit WHERE JournalID=@JournalID
                            END
                            ELSE
                            BEGIN
                            INSERT INTO dbo.ContributeSet(JournalID,CCodeType,CCodeFormat,Separator,RandomDigit) VALUES(@JournalID,@CCodeType,@CCodeFormat,@Separator,@RandomDigit)
                            END";
            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, cSetEntity.JournalID);
            db.AddInParameter(cmd, "@CCodeType", DbType.Int16, cSetEntity.CCodeType);
            db.AddInParameter(cmd, "@CCodeFormat", DbType.String, cSetEntity.CCodeFormat);
            db.AddInParameter(cmd, "@Separator", DbType.String, cSetEntity.Separator);
            db.AddInParameter(cmd, "@RandomDigit", DbType.Int16, cSetEntity.RandomDigit);

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

        # endregion

        # region 得到稿件编号

        /// <summary>
        /// 得到稿件编号
        /// </summary>
        /// <param name="query">查询基类，主要用到站点ID</param>
        /// <returns></returns>
        public string GetContributeNumber(QueryBase query)
        {
            StringBuilder sbCNumber = new StringBuilder("");
            ContributeSetEntity cSetEntity = GetContributeSetInfo(query);
            if (cSetEntity != null)
            {
                string Separator = string.Empty;
                if (!string.IsNullOrEmpty(cSetEntity.Separator))
                {
                    if (cSetEntity.Separator.Contains(","))
                    {
                        string[] paras = cSetEntity.Separator.Split(',');
                        if (paras != null && paras.Length > 0)
                        {
                            Separator = paras[0];
                            if (int.Parse(paras[1]) == 1)
                            {
                                IList<ContributionInfoEntity> list = ContributionInfoDataAccess.Instance.GetContributionInfoList(new ContributionInfoQuery() { JournalID = cSetEntity.JournalID });
                                if (list != null && list.Count > 0)
                                {
                                    ContributionInfoEntity entity = list[0];
                                    DateTime dt = System.DateTime.Now;
                                    if (dt.Year > entity.AddDate.Year || dt.Month > entity.AddDate.Month)
                                    {
                                        JournalSetInfoDataAccess.Instance.UpdateIDPoolByJournalID(entity.JournalID);
                                    }
                                }
                            }

                        }
                    }
                }

                string OrderNO = JournalSetInfoDataAccess.Instance.GetMaxID(query.JournalID, "WKTDB", "ContributeSet", cSetEntity.RandomDigit);
                switch (cSetEntity.CCodeType)
                {
                    case 1:// 日期格式
                        sbCNumber.Append(DateTime.Now.ToString(cSetEntity.CCodeFormat)).Append(string.IsNullOrEmpty(Separator) ? cSetEntity.Separator : Separator).Append(OrderNO);
                        break;
                    case 2:// 自定义格式
                        sbCNumber.Append(cSetEntity.CCodeFormat).Append(string.IsNullOrEmpty(Separator) ? cSetEntity.Separator : Separator).Append(OrderNO);
                        break;
                    case 3:// 年份格式
                        sbCNumber.Append(DateTime.Now.Year).Append(string.IsNullOrEmpty(Separator) ? cSetEntity.Separator : Separator).Append(OrderNO);
                        break;
                }
            }
            return sbCNumber.ToString();
        }

        # endregion

        /// <summary>
        /// 设置稿件的责任编辑
        /// </summary>
        /// <param name="setEntity"></param>
        /// <returns></returns>
        public bool SetContributeEditor(SetContributionEditorEntity setEntity)
        {
            bool flag = false;
            DbCommand cmd = db.GetStoredProcCommand("UP_SetContributionEditor");

            db.AddInParameter(cmd, "@JournalID", DbType.Int64, setEntity.JournalID);
            db.AddInParameter(cmd, "@CIDArray", DbType.String, string.Join(",", setEntity.CIDArray));
            db.AddInParameter(cmd, "@AuthorID", DbType.Int64, setEntity.AuthorID);

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



    }
}
