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
using WKT.Common.Security;
using WKT.Common.Extension;

namespace WKT.DataAccess
{
    /// <summary>
    /// 稿件流转
    /// </summary>
    public class FlowCirculationDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db = null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public FlowCirculationDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if (db == null)
                throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }

        private static FlowCirculationDataAccess _instance = new FlowCirculationDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static FlowCirculationDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }

        # region 作者投稿

        /// <summary>
        /// 判断是否是第一次投稿而不是退修或校样
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns>true 是第一次投稿 false 不是第一次投稿</returns>
        public bool IsFirstContribute(CirculationEntity cirEntity)
        {
            bool isFirst = false;

            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1 FlowLogID FROM dbo.FlowLogInfo f WITH(NOLOCK) WHERE f.CID=@CID AND JournalID=@JournalID");
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, cirEntity.JournalID);
            db.AddInParameter(cmd, "@CID", DbType.Int64, cirEntity.CID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                isFirst = !dr.Read();
                dr.Close();
            }
            return isFirst;
        }

        /// <summary>
        /// 作者投稿
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public bool AuthorContribute(CirculationEntity cirEntity)
        {
            bool flag = false;
            long EditorID = 0;

            try
            {

                # region 得到自动分配责编

                EditorAutoAllotQuery editorQuery = new EditorAutoAllotQuery();
                editorQuery.JournalID = cirEntity.JournalID;
                editorQuery.CNumber = cirEntity.CNumber;
                editorQuery.SubjectCategoryID = cirEntity.SubjectCategoryID;
                // 得到自动分配责编
                AuthorInfoEntity autoAllotAuthor = EditorAutoAllotDataAccess.Instance.GetAutoAllotEditor(editorQuery);
                if (autoAllotAuthor != null) // 没有设置自动分配责任编辑
                {
                    EditorID = autoAllotAuthor.AuthorID;
                }
                # endregion

                DbCommand cmd = db.GetStoredProcCommand("UP_AuthorContribute");
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, cirEntity.JournalID);
                db.AddInParameter(cmd, "@CID", DbType.Int64, cirEntity.CID);
                db.AddInParameter(cmd, "@AuthorID", DbType.Int64, cirEntity.AuthorID);
                db.AddInParameter(cmd, "@Editor", DbType.Int64, EditorID);
                db.AddInParameter(cmd, "@CPath", DbType.String, cirEntity.CPath == null ? "" : cirEntity.CPath);
                db.AddInParameter(cmd, "@FigurePath", DbType.String, cirEntity.FigurePath == null ? "" : cirEntity.FigurePath);
                db.AddInParameter(cmd, "@OtherPath", DbType.String, cirEntity.OtherPath == null ? "" : cirEntity.OtherPath);
                db.AddInParameter(cmd, "@DealAdvice", DbType.String, cirEntity.DealAdvice == null ? "" : cirEntity.DealAdvice.HtmlFilter());
                db.ExecuteNonQuery(cmd);

                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return flag;
        }

        /// <summary>
        /// 得到稿件的处理人
        /// </summary>
        /// <param name="cirEntity">稿件ID(CID)、稿件状态(EnumCStatus)、编辑部ID(JournalID)</param>
        /// <returns></returns>
        public AuthorInfoEntity GetContributionProcesser(CirculationEntity cirEntity)
        {
            AuthorInfoEntity authorEntity = null;
            DbCommand cmd = db.GetStoredProcCommand("UP_GetContributionEditor");
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, cirEntity.JournalID);
            db.AddInParameter(cmd, "@CID", DbType.Int64, cirEntity.CID);
            db.AddInParameter(cmd, "@CStatus", DbType.Int32, (int)cirEntity.EnumCStatus);
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                if (reader.Read())
                {
                    authorEntity = new AuthorInfoEntity();
                    authorEntity.AuthorID = (Int64)reader["AuthorID"];
                    authorEntity.LoginName = reader["LoginName"].ToString();
                    authorEntity.RealName = reader.IsDBNull(reader.GetOrdinal("RealName")) ? "" : reader["RealName"].ToString();
                    authorEntity.Mobile = reader.IsDBNull(reader.GetOrdinal("Mobile")) ? "" : reader["Mobile"].ToString();
                }
                reader.Close();
            }
            return authorEntity;
        }

        # endregion

        # region 稿件列表

        /// <summary>
        /// 根据当前所处环节获取稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public Pager<FlowContribution> GetFlowContributionList(CirculationEntity cirEntity)
        {
            int recordCount = 0;
            List<SqlParameter> listParameters = new List<SqlParameter>();
            string orderby = "c.CID DESC";// "c.CID DESC";
            if (cirEntity.isPersonal_Order ==1)
                orderby = "c.CID DESC | t.HandTime DESC";
            if (cirEntity.SortName != null)
            {
                if (cirEntity.SortName == "AddDate")
                    orderby = "c.AddDate "+ cirEntity.SortOrder;
                else if (cirEntity.SortName == "CNumber")
                    orderby = "c.CID " + cirEntity.SortOrder;
                else if(cirEntity.SortName=="HandTime")
                    orderby = "f.AddDate DESC | " + "t.HandTime " + cirEntity.SortOrder;
                else
                    orderby = "c.CID DESC";
            }


            StringBuilder sbSQL = new StringBuilder("SELECT f.FlowLogID,c.CID,c.JournalID,c.CNumber,c.Title,c.SubjectCat,c.ContributionType,c.Special,c.AuthorID,ca.AuthorName as FirstAuthor,ca2.AuthorName as CommunicationAuthor,c.Status,fs.StatusName as AuditStatus,f.SendUserID,f.RecUserID,c.Flag,c.IsQuick,f.IsView,c.IsPayAuditFee,c.IsPayPageFee,c.IsRetractions,c.AddDate,c.IntroLetterPath,c.[Year],c.Volumn,c.Issue,c.HireChannelID,ca.Tel,ca.Mobile,ca.Address,au.LoginName,cr.Remark,f.AddDate as HandTime,f.DealDate,fs.CStatus,mr.SendDate ");
            if (cirEntity.IsExpertAudited)
            {
                sbSQL.Append(" ,(select count(*) from FlowLogInfo where CID=c.CID AND Status=0 AND  RecUserID IN (SELECT AuthorID FROM AuthorInfo WHERE GroupID=3) ) AS IsPart  ");
            }
            else
            {
                sbSQL.Append(" ,-1000 as IsPart  ");
            }
            sbSQL.Append(" FROM dbo.FlowLogInfo f WITH(NOLOCK) INNER JOIN dbo.FlowStatus fs WITH(NOLOCK) ON f.TargetStatusID=fs.StatusID AND f.JournalID=fs.JournalID ");

            if (!cirEntity.IsSearch && !cirEntity.IsStat && cirEntity.IsHandled!=3)
            {
                sbSQL.Append(" AND f.Status=0 ");
            }
            if (cirEntity.IsSearch)
            {
                //sbSQL.Append(" AND f.Status=0 ");
                if (cirEntity.StatusID == -9)//-9 由Home\index下的[稿件状态]搜索传递 指示是否查询已撤稿件
                    sbSQL.Append(" INNER JOIN dbo.ContributionInfo c WITH(NOLOCK) ON c.CID=f.CID AND c.JournalID=f.JournalID AND c.Status=-1 LEFT JOIN  (select CID ,JouranalID,IsFirst,AuthorName,WorkUnit,Tel,Mobile,AuthorID,Address from  dbo.ContributionAuthor where CAuthorID in (select  min(CAuthorID) from   dbo.ContributionAuthor where IsFirst=1  group  by CID)) ca   ON c.JournalID=ca.JouranalID AND c.CID=ca.CID LEFT JOIN  (select CID ,JouranalID,IsFirst,AuthorName,Tel,Mobile,AuthorID,Address from  dbo.ContributionAuthor where CAuthorID in (select  min(CAuthorID) from   dbo.ContributionAuthor where IsCommunication=1  group  by CID)) ca2   ON c.JournalID=ca2.JouranalID AND c.CID=ca2.CID LEFT JOIN (select  CID ,JournalID,SendDate,ReciveUser  from MessageRecode where  RecodeID in (select  max(RecodeID)  from MessageRecodeAndTemplate where TemplateID=" + cirEntity.TemplateID + ")) as mr on mr.CID=ca.CID and ca.JouranalID=mr.JournalID AND ReciveUser=f.RecUserID  LEFT JOIN ( select  CID,JournalID,Remark from CRemark where  RemarkID in (  select MAX(RemarkID) as RemarkCount FROM CRemark group by  CID,JournalID)) AS cr  ON cr.CID=c.CID AND cr.JournalID=c.JournalID  LEFT JOIN dbo.ContributionInfoAtt cia  WITH(NOLOCK) ON cia.JournalID=c.JournalID and cia.CID=c.CID left join AuthorInfo as au on au.AuthorID=ca.AuthorID   WHERE f.JournalID=@JournalID");
                else if (cirEntity.StatusID == -10)//-10 由Home\index下的[稿件状态]搜索传递 指示是否查询已删除稿件
                    sbSQL.Append(" INNER JOIN dbo.ContributionInfo c WITH(NOLOCK) ON c.CID=f.CID AND c.JournalID=f.JournalID AND c.Status=-999 LEFT JOIN  (select CID ,JouranalID,IsFirst,AuthorName,WorkUnit,Tel,Mobile,AuthorID,Address from  dbo.ContributionAuthor where CAuthorID in (select  min(CAuthorID) from   dbo.ContributionAuthor where IsFirst=1  group  by CID)) ca   ON c.JournalID=ca.JouranalID AND c.CID=ca.CID LEFT JOIN  (select CID ,JouranalID,IsFirst,AuthorName,Tel,Mobile,AuthorID,Address from  dbo.ContributionAuthor where CAuthorID in (select  min(CAuthorID) from   dbo.ContributionAuthor where IsCommunication=1  group  by CID)) ca2   ON c.JournalID=ca2.JouranalID AND c.CID=ca2.CID LEFT JOIN (select  CID ,JournalID,SendDate,ReciveUser  from MessageRecode where  RecodeID in (select  max(RecodeID)  from MessageRecodeAndTemplate where TemplateID=" + cirEntity.TemplateID + ")) as mr on mr.CID=ca.CID and ca.JouranalID=mr.JournalID AND ReciveUser=f.RecUserID  LEFT JOIN ( select  CID,JournalID,Remark from CRemark where  RemarkID in (  select MAX(RemarkID) as RemarkCount FROM CRemark group by  CID,JournalID)) AS cr  ON cr.CID=c.CID AND cr.JournalID=c.JournalID LEFT JOIN dbo.ContributionInfoAtt cia  WITH(NOLOCK) ON cia.JournalID=c.JournalID and cia.CID=c.CID  left join AuthorInfo as au on au.AuthorID=ca.AuthorID   WHERE f.JournalID=@JournalID");
                else
                    sbSQL.Append(" INNER JOIN dbo.ContributionInfo c WITH(NOLOCK) ON c.CID=f.CID AND c.JournalID=f.JournalID  LEFT JOIN  (select CID ,JouranalID,IsFirst,AuthorName,WorkUnit,Tel,Mobile,AuthorID,Address from  dbo.ContributionAuthor where CAuthorID in (select  min(CAuthorID) from   dbo.ContributionAuthor where IsFirst=1  group  by CID)) ca   ON c.JournalID=ca.JouranalID AND c.CID=ca.CID LEFT JOIN  (select CID ,JouranalID,IsFirst,AuthorName,Tel,Mobile,AuthorID,Address from  dbo.ContributionAuthor where CAuthorID in (select  min(CAuthorID) from   dbo.ContributionAuthor where IsCommunication=1  group  by CID)) ca2   ON c.JournalID=ca2.JouranalID AND c.CID=ca2.CID LEFT JOIN (select  CID ,JournalID,SendDate,ReciveUser  from MessageRecode where  RecodeID in (select  max(RecodeID)  from MessageRecodeAndTemplate where TemplateID=" + cirEntity.TemplateID + ")) as mr on mr.CID=ca.CID and ca.JouranalID=mr.JournalID AND ReciveUser=f.RecUserID  LEFT JOIN ( select  CID,JournalID,Remark from CRemark where  RemarkID in (  select MAX(RemarkID) as RemarkCount FROM CRemark group by  CID,JournalID)) AS cr  ON cr.CID=c.CID AND cr.JournalID=c.JournalID  LEFT JOIN dbo.ContributionInfoAtt cia  WITH(NOLOCK) ON cia.JournalID=c.JournalID and cia.CID=c.CID  left join AuthorInfo as au on au.AuthorID=ca.AuthorID   WHERE f.JournalID=@JournalID AND (f.Status=0 or f.Status=2)");
                
            }
            else
                sbSQL.Append(" INNER JOIN dbo.ContributionInfo c WITH(NOLOCK) ON c.CID=f.CID AND c.JournalID=f.JournalID AND c.Status NOT IN(-999,-1)  LEFT JOIN  (select CID ,JouranalID,IsFirst,AuthorName,WorkUnit,Tel,Mobile,AuthorID,Address from  dbo.ContributionAuthor where CAuthorID in (select  min(CAuthorID) from   dbo.ContributionAuthor where IsFirst=1  group  by CID)) ca   ON c.JournalID=ca.JouranalID AND c.CID=ca.CID LEFT JOIN  (select CID ,JouranalID,IsFirst,AuthorName,Tel,Mobile,AuthorID,Address from  dbo.ContributionAuthor where CAuthorID in (select  min(CAuthorID) from   dbo.ContributionAuthor where IsCommunication=1  group  by CID)) ca2   ON c.JournalID=ca2.JouranalID AND c.CID=ca2.CID  LEFT JOIN (select  CID ,JournalID,SendDate,ReciveUser  from MessageRecode where  RecodeID in (select  max(RecodeID)  from MessageRecodeAndTemplate where TemplateID=" + cirEntity.TemplateID + ")) as mr on mr.CID=ca.CID and ca.JouranalID=mr.JournalID AND ReciveUser=f.RecUserID  LEFT JOIN ( select  CID,JournalID,Remark from CRemark where  RemarkID in (  select MAX(RemarkID) as RemarkCount FROM CRemark group by  CID,JournalID)) AS cr  ON cr.CID=c.CID AND cr.JournalID=c.JournalID  LEFT JOIN dbo.ContributionInfoAtt cia  WITH(NOLOCK) ON cia.JournalID=c.JournalID and cia.CID=c.CID  left join AuthorInfo as au on au.AuthorID=ca.AuthorID   WHERE f.JournalID=@JournalID");
            SqlParameter pJournalID = new SqlParameter("@JournalID", SqlDbType.BigInt);
            pJournalID.Value = cirEntity.JournalID;
            listParameters.Add(pJournalID);

            //模板ID  添加催审时间  文海峰 2014-1-7
            if (cirEntity.CurAuthorID != null && !cirEntity.IsSearch)
            {
                SqlParameter pRecUserID = new SqlParameter("@RecUserID", SqlDbType.BigInt);
                pRecUserID.Value = cirEntity.CurAuthorID;
                listParameters.Add(pRecUserID);

                if (cirEntity.IsHandled == 1)// 已处理
                {
                    sbSQL.Append(" AND f.SendUserID=@RecUserID ");
                }
                else if (cirEntity.IsHandled == 0)// 待处理
                {
                    sbSQL.Append(" AND f.RecUserID=@RecUserID ");
                }
                else
                {
                    sbSQL.Append(" AND (f.RecUserID=@RecUserID OR f.SendUserID=@RecUserID) ");
                }
            }
            if (cirEntity.StatusID != null)
            {
                if (cirEntity.StatusID > 0)
                {
                    SqlParameter pStatusID = new SqlParameter("@StatusID", SqlDbType.BigInt);
                    pStatusID.Value = cirEntity.StatusID;
                    listParameters.Add(pStatusID);
                    sbSQL.Append(" AND fs.StatusID=@StatusID");
                }
            }
            if (cirEntity.AuthorID > 0)
            {
                SqlParameter pAuthorID = new SqlParameter("@AuthorID", SqlDbType.BigInt);
                pAuthorID.Value = cirEntity.AuthorID;
                listParameters.Add(pAuthorID);
                sbSQL.Append(" AND c.AuthorID=@AuthorID");
            }

            if (!string.IsNullOrEmpty(cirEntity.CNumber))
            {
                // SqlParameter pCNumber = new SqlParameter("@CNumber", SqlDbType.VarChar,50);
                // pCNumber.Value = SecurityUtils.SafeSqlString(cirEntity.CNumber);
                // listParameters.Add(pCNumber);
                sbSQL.Append(" AND c.CNumber like '%" + cirEntity.CNumber + "%'");
            }
            if (cirEntity.SubjectCategoryID > 0)
            {
                SqlParameter pSubjectCat = new SqlParameter("@SubjectCat", SqlDbType.Int);
                pSubjectCat.Value = cirEntity.SubjectCategoryID;
                listParameters.Add(pSubjectCat);
                sbSQL.Append(" AND c.SubjectCat=@SubjectCat");
            }
            if (!string.IsNullOrEmpty(cirEntity.Title))
            {
                sbSQL.Append(" AND c.Title LIKE '%" + SecurityUtils.SafeSqlString(cirEntity.Title) + "%'");
            }
            if (!string.IsNullOrEmpty(cirEntity.Keyword))
            {
                sbSQL.Append(" AND c.Keywords LIKE '%" + SecurityUtils.SafeSqlString(cirEntity.Keyword) + "%'");
            }
            if (!string.IsNullOrEmpty(cirEntity.Flag))
            {
                SqlParameter pFlag = new SqlParameter("@Flag", SqlDbType.VarChar, 20);
                pFlag.Value = SecurityUtils.SafeSqlString(cirEntity.Flag);
                listParameters.Add(pFlag);
                sbSQL.Append(" AND c.Flag=@Flag");
            }
            // 第一作者
            if (!string.IsNullOrEmpty(cirEntity.FirstAuthor))
            {
                sbSQL.Append(" AND ca.AuthorName LIKE '" + SecurityUtils.SafeSqlString(cirEntity.FirstAuthor) + "%'");
            }

            // 第一作者单位
            if (!string.IsNullOrEmpty(cirEntity.FirstAuthorWorkUnit))
            {
                sbSQL.Append(" AND ca.WorkUnit LIKE '" + SecurityUtils.SafeSqlString(cirEntity.FirstAuthorWorkUnit) + "%' AND ca.IsFirst=1");
            }

            if (cirEntity.IsStat == true)
            {
                // 处理日期
                if (cirEntity.StartDate != null && cirEntity.EndDate != null)
                {
                    sbSQL.Append(" AND f.AddDate >= '").Append(cirEntity.StartDate.Value.ToString("yyyy-MM-dd")).Append(" ' AND f.AddDate < '").Append(cirEntity.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd")).Append("'");
                }
                else if (cirEntity.StartDate != null)
                {
                    sbSQL.Append(" AND f.AddDate >= '").Append(cirEntity.StartDate.Value.ToString("yyyy-MM-dd")).Append("'");
                }
                else if (cirEntity.EndDate != null)
                {
                    sbSQL.Append(" AND f.AddDate < '").Append(cirEntity.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd")).Append("'");
                }
                orderby = "f.AddDate DESC | t.HandTime DESC";
            }
            else
            {
                // 投稿日期
                if (cirEntity.StartDate != null && cirEntity.EndDate != null)
                {
                    sbSQL.Append(" AND c.AddDate >= '").Append(cirEntity.StartDate.Value.ToString("yyyy-MM-dd")).Append(" ' AND c.AddDate < '").Append(cirEntity.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd")).Append("'");
                }
                else if (cirEntity.StartDate != null)
                {
                    sbSQL.Append(" AND c.AddDate >= '").Append(cirEntity.StartDate.Value.ToString("yyyy-MM-dd")).Append("'");
                }
                else if (cirEntity.EndDate != null)
                {
                    sbSQL.Append(" AND c.AddDate < '").Append(cirEntity.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd")).Append("'");
                }
            }  
            //录用年
            if (cirEntity.Year>0)
            {
                sbSQL.Append(" AND c.Year = '").Append(cirEntity.Year).Append("'");
            }
            //录用期
            if (cirEntity.Issue>0)
            {
                sbSQL.Append(" AND c.Issue = '").Append(cirEntity.Issue).Append("'");
            }
            //是否是搜索
            if (cirEntity.IsSearch)
            {
                SqlParameter pRecUserID = new SqlParameter("@RecUserID", SqlDbType.BigInt);
                pRecUserID.Value = cirEntity.CurAuthorID;
                listParameters.Add(pRecUserID);
                if(cirEntity.isPersonal_OnlyMySearch==1)
                    sbSQL.Append(" AND f.FlowLogID in  (SELECT fil.FlowLogID FROM dbo.FlowLogInfo fil WITH(NOLOCK) WHERE fil.CID=f.CID AND fil.JournalID=f.JournalID and (f.RecUserID=@RecUserID or f.SendUserID=@RecUserID))");
                else
                    sbSQL.Append(" AND f.FlowLogID in  (SELECT fil.FlowLogID FROM dbo.FlowLogInfo fil WITH(NOLOCK) WHERE fil.CID=f.CID AND fil.JournalID=f.JournalID)");
            }
            DataSet ds = db.PageingQuery(cirEntity.CurrentPage, cirEntity.PageSize, sbSQL.ToString(), orderby, listParameters.ToArray(), ref recordCount);
            Pager<FlowContribution> pager = new Pager<FlowContribution>();
            if (ds != null && ds.Tables.Count > 0)
            {
                List<FlowContribution> list = new List<FlowContribution>();
                if (ds != null)
                {
                    FlowContribution cEntity = null;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        cEntity = new FlowContribution();
                        cEntity.FlowLogID = (Int64)row["FlowLogID"];
                        cEntity.CID = (Int64)row["CID"];
                        cEntity.JournalID = (Int64)row["JournalID"];
                        cEntity.CNumber = row["CNumber"].ToString();
                        cEntity.Title = row["Title"].ToString();
                        cEntity.SubjectCat = (Int32)row["SubjectCat"];
                        cEntity.ContributionType = (Int32)row["ContributionType"];
                        cEntity.Special = (Int32)row["Special"];
                        cEntity.AuthorID = (Int64)row["AuthorID"];
                        cEntity.FirstAuthor = row.IsNull("FirstAuthor") ? "" : row["FirstAuthor"].ToString();
                        cEntity.CommunicationAuthor = row.IsNull("CommunicationAuthor") ? "" : row["CommunicationAuthor"].ToString();
                        cEntity.Status = (Int32)row["Status"];
                        cEntity.AuditStatus = row.IsNull("AuditStatus") ? "审核中" : (String)row["AuditStatus"];
                        cEntity.IsQuick = TypeParse.ToBool(row["IsQuick"]);
                        cEntity.Flag = row.IsNull("Flag") ? "" : row["Flag"].ToString();
                        cEntity.RecUserID = row.IsNull("RecUserID") ? 0 : (Int64)row["RecUserID"];
                        cEntity.SendUserID = row.IsNull("SendUserID") ? 0 : (Int64)row["SendUserID"];
                        cEntity.IsView = row.IsNull("IsView") ? false : TypeParse.ToBool(row["IsView"]);
                        cEntity.IsPayAuditFee = row.IsNull("IsPayAuditFee") ? (byte)0 : (byte)row["IsPayAuditFee"];
                        cEntity.IsPayPageFee = row.IsNull("IsPayPageFee") ? (byte)0 : (byte)row["IsPayPageFee"];
                        cEntity.IsRetractions = row.IsNull("IsRetractions") ? false : TypeParse.ToBool(row["IsRetractions"]);
                        cEntity.IntroLetterPath = row.IsNull("IntroLetterPath") ? "" : row["IntroLetterPath"].ToString();
                        cEntity.AddDate = Convert.ToDateTime(row["AddDate"]);
                        cEntity.Days = Utils.SpanDays(cEntity.AddDate);
                        cEntity.HandDays = Utils.SpanDays(row.IsNull("HandTime") ? cEntity.AddDate : TypeParse.ToDateTime(row["HandTime"]));
                        cEntity.JChannelID = row.IsNull("HireChannelID") ? 0 : TypeParse.ToLong(row["HireChannelID"]);
                        cEntity.Year = row.IsNull("Year") ? 0 : TypeParse.ToInt(row["Year"]);
                        cEntity.Volume = row.IsNull("Volumn") ? 0 : TypeParse.ToInt(row["Volumn"]);
                        cEntity.Issue = row.IsNull("Issue") ? 0 : TypeParse.ToInt(row["Issue"]);

                        //文海峰2013-11-19
                        cEntity.HandTime = Convert.ToDateTime(row["HandTime"]);
                        cEntity.SendDate =Convert.IsDBNull(row["SendDate"])?"":Convert.ToString(row["SendDate"]);
                        
                        cEntity.CStatus = row.IsNull("CStatus") ? 0 : Convert.ToInt32(row["CStatus"]);    
                        cEntity.Remark = row.IsNull("Remark") ? "" : TypeParse.ToString(row["Remark"]);
                        cEntity.LoginName = row.IsNull("LoginName") ? "" : (String)row["LoginName"];
                        cEntity.Address = row.IsNull("Address") ? "" : (String)row["Address"];
                        cEntity.Tel = row.IsNull("Tel") ? "" : (String)row["Tel"];
                        cEntity.Mobile = row.IsNull("Mobile") ? "" : (String)row["Mobile"];
                        cEntity.IsPart = row.IsNull("IsPart") ? false : ((Int32)row["IsPart"]>0?true:false);
                        list.Add(cEntity);
                    }
                }
                pager.ItemList = list;
            }
            pager.CurrentPage = cirEntity.CurrentPage;
            pager.PageSize = cirEntity.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        # endregion

        # region 获取特殊稿件状态的稿件列表

        /// <summary>
        /// 获取特殊稿件状态的稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public Pager<FlowContribution> GetStatusContributionList(CirculationEntity cirEntity)
        {
            int recordCount = 0;
            string sql = @"     SELECT c.CID,c.JournalID,c.CNumber,c.Title,c.AuthorID,fi.SendUserID,fi.AddDate
                                FROM dbo.ContributionInfo c WITH(NOLOCK),
	                                dbo.FlowLogInfo fi WITH(NOLOCK),
	                                dbo.FlowAction fa WITH(NOLOCK)
                                WHERE c.JournalID=@JournalID AND c.JournalID=fi.JournalID AND fi.JournalID=fa.JournalID 
	                                AND c.CID=fi.CID 
	                                AND fa.CStatus=c.Status AND fa.ActionID=fi.ActionID AND c.Status=@CStatus";
            List<SqlParameter> listParameters = new List<SqlParameter>();
            SqlParameter pJournalID = new SqlParameter("@JournalID", SqlDbType.BigInt);
            pJournalID.Value = cirEntity.JournalID;
            listParameters.Add(pJournalID);

            SqlParameter pCStatus = new SqlParameter("@CStatus", SqlDbType.Int);
            pCStatus.Value = cirEntity.CStatus;
            listParameters.Add(pCStatus);

            DataSet ds = db.PageingQuery(cirEntity.CurrentPage, cirEntity.PageSize, sql, "c.CNumber", listParameters.ToArray(), ref recordCount);
            Pager<FlowContribution> pager = new Pager<FlowContribution>();
            if (ds != null && ds.Tables.Count > 0)
            {
                List<FlowContribution> list = new List<FlowContribution>();
                if (ds != null)
                {
                    FlowContribution cEntity = null;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        cEntity = new FlowContribution();
                        cEntity.CID = (Int64)row["CID"];
                        cEntity.JournalID = (Int64)row["JournalID"];
                        cEntity.CNumber = row["CNumber"].ToString();
                        cEntity.Title = row["Title"].ToString();
                        cEntity.AuthorID = (Int64)row["AuthorID"];
                        cEntity.SendUserID = row.IsNull("SendUserID") ? 0 : (Int64)row["SendUserID"];
                        cEntity.AddDate = Convert.ToDateTime(row["AddDate"]);
                        list.Add(cEntity);
                    }
                }
                pager.ItemList = list;
            }
            pager.CurrentPage = cirEntity.CurrentPage;
            pager.PageSize = cirEntity.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        # endregion

        # region 获取作者最新稿件状态稿件列表

        /// <summary>
        /// 获取作者最新稿件状态稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public Pager<FlowContribution> GetAuthorContributionList(CirculationEntity cirEntity)
        {
            int recordCount = 0;
            List<SqlParameter> listParameters = new List<SqlParameter>();
            StringBuilder sbSQL = new StringBuilder("SELECT c.CID,c.JournalID,c.CNumber,c.Title,c.SubjectCat,c.AuthorID,ca.AuthorName as FirstAuthor,c.Status,(SELECT TOP 1 fs.DisplayName FROM dbo.FlowLogInfo f WITH(NOLOCK) INNER JOIN dbo.FlowStatus fs WITH(NOLOCK) ON f.TargetStatusID=fs.StatusID AND f.JournalID=fs.JournalID AND f.Status=0  WHERE f.CID=c.CID AND f.JournalID=c.JournalID ORDER BY f.AddDate DESC) as AuditStatus,c.Flag,c.IsQuick,c.IsPayAuditFee,c.IsPayPageFee,c.IsRetractions,c.AddDate FROM dbo.ContributionInfo c WITH(NOLOCK) INNER JOIN dbo.ContributionAuthor ca WITH(NOLOCK) ON c.JournalID=ca.JouranalID AND c.CID=ca.CID AND ca.IsFirst=1 WHERE c.JournalID=@JournalID AND c.AuthorID=@AuthorID");
            SqlParameter pJournalID = new SqlParameter("@JournalID", SqlDbType.BigInt);
            pJournalID.Value = cirEntity.JournalID;
            listParameters.Add(pJournalID);
            SqlParameter pAuthorID = new SqlParameter("@AuthorID", SqlDbType.BigInt);
            pAuthorID.Value = cirEntity.CurAuthorID;
            listParameters.Add(pAuthorID);


            if (!string.IsNullOrEmpty(cirEntity.CNumber))
            {
                SqlParameter pCNumber = new SqlParameter("@CNumber", SqlDbType.VarChar, 50);
                pCNumber.Value = SecurityUtils.SafeSqlString(cirEntity.CNumber);
                listParameters.Add(pCNumber);
                sbSQL.Append(" AND c.CNumber=@CNumber");
            }
            if (cirEntity.SubjectCategoryID > 0)
            {
                SqlParameter pSubjectCat = new SqlParameter("@SubjectCat", SqlDbType.Int);
                pSubjectCat.Value = cirEntity.SubjectCategoryID;
                listParameters.Add(pSubjectCat);
                sbSQL.Append(" AND c.SubjectCat=@SubjectCat");
            }
            if (!string.IsNullOrEmpty(cirEntity.Title))
            {
                SqlParameter pTitle = new SqlParameter("@Title", SqlDbType.VarChar, 200);
                pTitle.Value = SecurityUtils.SafeSqlString(cirEntity.Title);
                listParameters.Add(pTitle);
                sbSQL.Append(" AND c.Title=@Title");
            }
            if (!string.IsNullOrEmpty(cirEntity.Keyword))
            {
                sbSQL.Append(" AND c.Keyword LIKE '%" + SecurityUtils.SafeSqlString(cirEntity.Keyword) + "%'");
            }
            if (!string.IsNullOrEmpty(cirEntity.Flag))
            {
                SqlParameter pFlag = new SqlParameter("@Flag", SqlDbType.VarChar, 20);
                pFlag.Value = SecurityUtils.SafeSqlString(cirEntity.Flag);
                listParameters.Add(pFlag);
                sbSQL.Append(" AND c.Flag=@Flag");
            }
            // 第一作者
            if (!string.IsNullOrEmpty(cirEntity.FirstAuthor))
            {
                sbSQL.Append(" AND ca.AuthorName = '" + SecurityUtils.SafeSqlString(cirEntity.FirstAuthor) + "'");
            }
            // 投稿日期
            if (cirEntity.StartDate != null && cirEntity.EndDate != null)
            {
                sbSQL.Append(" AND c.AddDate >= '").Append(cirEntity.StartDate.Value.ToString("yyyy-MM-dd")).Append(" ' AND c.AddDate < '").Append(cirEntity.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd")).Append("'");
            }
            else if (cirEntity.StartDate != null)
            {
                sbSQL.Append(" AND c.AddDate >= '").Append(cirEntity.StartDate.Value.ToString("yyyy-MM-dd")).Append("'");
            }
            else if (cirEntity.EndDate != null)
            {
                sbSQL.Append(" AND c.AddDate < '").Append(cirEntity.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd")).Append("'");
            }
            DataSet ds = db.PageingQuery(cirEntity.CurrentPage, cirEntity.PageSize, sbSQL.ToString(), "c.AddDate DESC", listParameters.ToArray(), ref recordCount);
            Pager<FlowContribution> pager = new Pager<FlowContribution>();
            if (ds != null && ds.Tables.Count > 0)
            {
                List<FlowContribution> list = new List<FlowContribution>();
                if (ds != null)
                {
                    FlowContribution cEntity = null;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        cEntity = new FlowContribution();
                        cEntity.FlowLogID = 0;
                        cEntity.CID = (Int64)row["CID"];
                        cEntity.JournalID = (Int64)row["JournalID"];
                        cEntity.CNumber = (String)row["CNumber"];
                        cEntity.Title = (String)row["Title"];
                        cEntity.SubjectCat = (Int32)row["SubjectCat"];
                        cEntity.AuthorID = (Int64)row["AuthorID"];
                        cEntity.FirstAuthor = row.IsNull("FirstAuthor") ? "" : (String)row["FirstAuthor"];
                        cEntity.Status = (Int32)row["Status"];
                        cEntity.IsQuick = TypeParse.ToBool(row["IsQuick"]);
                        cEntity.Flag = (String)row["Flag"];
                        cEntity.AuditStatus = row.IsNull("AuditStatus") ? "审核中" : (String)row["AuditStatus"];
                        cEntity.RecUserID = 0;
                        cEntity.IsPayAuditFee = row.IsNull("IsPayAuditFee") ? (byte)0 : (byte)row["IsPayAuditFee"];
                        cEntity.IsPayPageFee = row.IsNull("IsPayPageFee") ? (byte)0 : (byte)row["IsPayPageFee"];
                        cEntity.IsRetractions = row.IsNull("IsRetractions") ? false : TypeParse.ToBool(row["IsRetractions"]);
                        cEntity.AddDate = Convert.ToDateTime(row["AddDate"]);
                        list.Add(cEntity);
                    }
                }
                pager.ItemList = list;
            }
            pager.CurrentPage = cirEntity.CurrentPage;
            pager.PageSize = cirEntity.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        # endregion

        # region 获取环节步骤及操作列表

        # region 得到下一步骤信息

        /// <summary>
        /// 得到下一步骤信息
        /// </summary>
        /// <param name="ciEntity">流转信息</param>
        /// <returns></returns>
        public FlowStep GetNextFlowStep(CirculationEntity ciEntity)
        {
            FlowStep flowStep = new FlowStep();

            # region 1.得到稿件信息

            IList<long> listCID = ciEntity.CIDS.Split(',').Select(p => TypeParse.ToLong(p, 0)).ToList<long>();
            StringBuilder sqlGetContributionText = new StringBuilder();
            sqlGetContributionText.Append("SELECT CID,Title FROM dbo.ContributionInfo c WITH(NOLOCK)");
            sqlGetContributionText.Append(" WHERE  JournalID=@JournalID AND CID IN (" + string.Join(",", listCID) + ")");

            DbCommand cmdGetCInfo = db.GetSqlStringCommand(sqlGetContributionText.ToString());
            db.AddInParameter(cmdGetCInfo, "@JournalID", DbType.Int64, ciEntity.JournalID);

            IDictionary<long, string> dictContribution = new Dictionary<long, string>();
            using (IDataReader dr = db.ExecuteReader(cmdGetCInfo))
            {
                long CID = 0;
                string title = "";
                while (dr.Read())
                {
                    CID = (Int64)dr["CID"];
                    title = dr["Title"].ToString();
                    if (!dictContribution.ContainsKey(CID))
                    {
                        dictContribution.Add(CID, title);
                    }
                }
                dr.Close();
            }

            flowStep.DictContribution = dictContribution;

            # endregion

            # region 2.得到节点的人员信息


            List<AuthorInfoEntity> listAuthorInfo = new List<AuthorInfoEntity>();

            long FirstCID = flowStep.DictContribution.First().Key;
            DbCommand cmdAuthor = db.GetStoredProcCommand("UP_GetStepHaveRightAuthor");
            db.AddInParameter(cmdAuthor, "@JournalID", DbType.Int64, ciEntity.JournalID);
            db.AddInParameter(cmdAuthor, "@ActionID", DbType.Int64, ciEntity.ActionID);
            db.AddInParameter(cmdAuthor, "@CID", DbType.Int64, FirstCID);
            db.AddInParameter(cmdAuthor, "@FlowLogID", DbType.Int64, ciEntity.DictLogID[FirstCID]);

            using (IDataReader dr = db.ExecuteReader(cmdAuthor))
            {
                AuthorInfoEntity authorInfoEntity = null;
                while (dr.Read())
                {
                    authorInfoEntity = new AuthorInfoEntity();
                    authorInfoEntity.AuthorID = (Int64)dr["AuthorID"];
                    authorInfoEntity.JournalID = (Int64)dr["JournalID"];
                    authorInfoEntity.LoginName = (String)dr["LoginName"];
                    authorInfoEntity.RealName = (String)dr["RealName"];
                    authorInfoEntity.Mobile = (String)dr["Mobile"];
                    listAuthorInfo.Add(authorInfoEntity);
                }
                dr.Close();
            }

            flowStep.FlowAuthorList = listAuthorInfo;

            # endregion

            if (ciEntity.ActionID > 0)
            {
                # region 3.得到审稿状态的配置信息

                StringBuilder sqlGetFlowConfigText = new StringBuilder();
                sqlGetFlowConfigText.Append("SELECT TOP 1 fc.FlowConfigID,fc.JournalID,fc.StatusID,fc.IsAllowBack,fc.IsMultiPerson,MultiPattern,TimeoutDay,TimeoutPattern,IsSMSRemind,IsEmailRemind,RangeDay,RemindCount,fa.IsRetractionSendMsg FROM dbo.FlowConfig fc WITH(NOLOCK) INNER JOIN dbo.FlowAction fa WITH(NOLOCK) ON fc.JournalID=fa.JournalID AND fc.StatusID=fa.TOStatusID WHERE fc.JournalID=@JournalID AND fa.ActionID=@ActionID ");

                DbCommand cmdGetFlowConfig = db.GetSqlStringCommand(sqlGetFlowConfigText.ToString());
                db.AddInParameter(cmdGetFlowConfig, "@JournalID", DbType.Int64, ciEntity.JournalID);
                db.AddInParameter(cmdGetFlowConfig, "@ActionID", DbType.Int64, ciEntity.ActionID);

                using (IDataReader dr = db.ExecuteReader(cmdGetFlowConfig))
                {
                    FlowConfigEntity flowConfigEntity = new FlowConfigEntity();
                    if (dr.Read())
                    {
                        flowConfigEntity.FlowConfigID = (Int64)dr["FlowConfigID"];
                        flowConfigEntity.JournalID = (Int64)dr["JournalID"];
                        flowConfigEntity.StatusID = (Int64)dr["StatusID"];
                        flowConfigEntity.IsAllowBack = (Byte)dr["IsAllowBack"];
                        flowConfigEntity.IsMultiPerson = (Boolean)dr["IsMultiPerson"];
                        flowConfigEntity.MultiPattern = (Byte)dr["MultiPattern"];
                        flowConfigEntity.TimeoutDay = (Int32)dr["TimeoutDay"];
                        flowConfigEntity.TimeoutPattern = (Byte)dr["TimeoutPattern"];
                        flowConfigEntity.IsSMSRemind = (Boolean)dr["IsSMSRemind"];
                        flowConfigEntity.IsEmailRemind = (Boolean)dr["IsEmailRemind"];
                        flowConfigEntity.RangeDay = (Int32)dr["RangeDay"];
                        flowConfigEntity.RemindCount = (Int32)dr["RemindCount"];
                    }
                    dr.Close();
                    flowStep.FlowConfig = flowConfigEntity;
                }


                # endregion

                # region 4.得到当前节点信息

                FlowActionQuery query = new FlowActionQuery();
                query.JournalID = ciEntity.JournalID;
                query.ActionID = ciEntity.ActionID;
                FlowActionEntity actionEntity = FlowActionDataAccess.Instance.GetFlowActionEntity(query);
                flowStep.FlowAction = actionEntity;

                # endregion

            }

            return flowStep;
        }

        # endregion

        # region 处理审稿流程

        /// <summary>
        /// 处理审稿流程
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        public bool ProcessDealBackContribution(CirculationEntity cirEntity)
        {
            bool flag = false;
            try
            {
                if (cirEntity.IsExpertToEditor)
                {
                    DbCommand cmd = db.GetStoredProcCommand("UP_DealBackStatusContribution_ExpertToEditor");
                    db.AddInParameter(cmd, "@JournalID", DbType.Int64, cirEntity.JournalID);
                    db.AddInParameter(cmd, "@CID", DbType.Int64, cirEntity.CID);
                    db.AddInParameter(cmd, "@CStatus", DbType.Int32, (int)cirEntity.EnumCStatus);
                    db.AddInParameter(cmd, "@SendUserID", DbType.Int64, cirEntity.AuthorID);
                    db.AddInParameter(cmd, "@RecUserID", DbType.Int64, cirEntity.RecUserID);
                    db.AddInParameter(cmd, "@CPath", DbType.String, string.IsNullOrEmpty(cirEntity.CPath) ? "" : cirEntity.CPath);
                    db.AddInParameter(cmd, "@CFileName", DbType.String, string.IsNullOrEmpty(cirEntity.CFileName) ? "" : cirEntity.CFileName);
                    db.AddInParameter(cmd, "@FigurePath", DbType.String, string.IsNullOrEmpty(cirEntity.FigurePath) ? "" : cirEntity.FigurePath);
                    db.AddInParameter(cmd, "@FFileName", DbType.String, string.IsNullOrEmpty(cirEntity.FFileName) ? "" : cirEntity.FFileName);
                    db.AddInParameter(cmd, "@OtherPath", DbType.String, string.IsNullOrEmpty(cirEntity.OtherPath) ? "" : cirEntity.OtherPath);
                    db.AddInParameter(cmd, "@DealAdvice", DbType.String, string.IsNullOrEmpty(cirEntity.DealAdvice) ? "" : cirEntity.DealAdvice);
                    db.AddInParameter(cmd, "@IsHaveBill", DbType.Int16, cirEntity.IsHaveBill);
                    db.ExecuteNonQuery(cmd);
                    flag = true;
                }
                else
                {
                    DbCommand cmd = db.GetStoredProcCommand("UP_DealBackStatusContribution");
                    db.AddInParameter(cmd, "@JournalID", DbType.Int64, cirEntity.JournalID);
                    db.AddInParameter(cmd, "@CID", DbType.Int64, cirEntity.CID);
                    db.AddInParameter(cmd, "@CStatus", DbType.Int32, (int)cirEntity.EnumCStatus);
                    db.AddInParameter(cmd, "@SendUserID", DbType.Int64, cirEntity.AuthorID);
                    db.AddInParameter(cmd, "@CPath", DbType.String, string.IsNullOrEmpty(cirEntity.CPath) ? "" : cirEntity.CPath);
                    db.AddInParameter(cmd, "@CFileName", DbType.String, string.IsNullOrEmpty(cirEntity.CFileName) ? "" : cirEntity.CFileName);
                    db.AddInParameter(cmd, "@FigurePath", DbType.String, string.IsNullOrEmpty(cirEntity.FigurePath) ? "" : cirEntity.FigurePath);
                    db.AddInParameter(cmd, "@FFileName", DbType.String, string.IsNullOrEmpty(cirEntity.FFileName) ? "" : cirEntity.FFileName);
                    db.AddInParameter(cmd, "@OtherPath", DbType.String, string.IsNullOrEmpty(cirEntity.OtherPath) ? "" : cirEntity.OtherPath);
                    db.AddInParameter(cmd, "@DealAdvice", DbType.String, string.IsNullOrEmpty(cirEntity.DealAdvice) ? "" : cirEntity.DealAdvice);
                    db.AddInParameter(cmd, "@IsHaveBill", DbType.Int16, cirEntity.IsHaveBill);
                    db.ExecuteNonQuery(cmd);
                    flag = true;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 处理审稿流程
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        public bool ProcessFlowCirulation(AuditBillEntity auditBillEntity)
        {
            bool flag = false;
            try
            {
                string[] ReceiverArray = auditBillEntity.ReveiverList.Split(',');
                long ReceiverID = 0;
                foreach (string Receiver in ReceiverArray)
                {
                    if (!string.IsNullOrEmpty(Receiver))
                    {
                        ReceiverID = TypeParse.ToLong(Receiver, 0);
                        if (ReceiverID > 0)
                        {
                            DbCommand cmd = db.GetStoredProcCommand("UP_FlowCirculation");
                            db.AddInParameter(cmd, "@ActionID", DbType.Int64, auditBillEntity.ActionID);
                            db.AddInParameter(cmd, "@FlowLogID", DbType.Int64, auditBillEntity.FlowLogID);
                            db.AddInParameter(cmd, "@JournalID", DbType.Int64, auditBillEntity.JournalID);
                            db.AddInParameter(cmd, "@CID", DbType.Int64, auditBillEntity.CID);
                            db.AddInParameter(cmd, "@Processer", DbType.Int64, auditBillEntity.Processer);
                            db.AddInParameter(cmd, "@Receiver", DbType.Int64, ReceiverID);
                            db.AddInParameter(cmd, "@IsContinueSubmit", DbType.Boolean, auditBillEntity.IsContinueSubmit);
                            if (auditBillEntity.DictDealAdvice.ContainsKey(ReceiverID))
                            {
                                //db.AddInParameter(cmd, "@DealAdvice", DbType.String, TextHelper.SubStr(Utils.ClearHtml(auditBillEntity.DictDealAdvice[ReceiverID]), 1000));
                                db.AddInParameter(cmd, "@DealAdvice", DbType.String, auditBillEntity.DictDealAdvice[ReceiverID]);
                            }
                            else
                            {
                                //db.AddInParameter(cmd, "@DealAdvice", DbType.String, string.IsNullOrEmpty(auditBillEntity.DealAdvice) ? "" : TextHelper.SubStr(Utils.ClearHtml(auditBillEntity.DealAdvice), 1000));
                                db.AddInParameter(cmd, "@DealAdvice", DbType.String, string.IsNullOrEmpty(auditBillEntity.DealAdvice) ? "" : auditBillEntity.DealAdvice);
                            }
                            db.AddInParameter(cmd, "@CPath", DbType.String, auditBillEntity.CPath);
                            db.AddInParameter(cmd, "@CFileName", DbType.String, auditBillEntity.CFileName);
                            db.AddInParameter(cmd, "@FigurePath", DbType.String, auditBillEntity.FigurePath);
                            db.AddInParameter(cmd, "@FFileName", DbType.String, auditBillEntity.FFileName);
                            db.AddInParameter(cmd, "@OtherPath", DbType.String, auditBillEntity.OtherPath);
                            db.ExecuteNonQuery(cmd);
                        }
                    }
                }

                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 处理审稿流程
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        public bool ProcessFlowCirulationNoAction(AuditBillEntity auditBillEntity)
        {
            bool flag = false;
            try
            {
                string[] ReceiverArray = auditBillEntity.ReveiverList.Split(',');
                long ReceiverID = 0;
                foreach (string Receiver in ReceiverArray)
                {
                    if (!string.IsNullOrEmpty(Receiver))
                    {
                        ReceiverID = TypeParse.ToLong(Receiver, 0);
                        if (ReceiverID > 0)
                        {
                            DbCommand cmd = db.GetStoredProcCommand("UP_FlowCirculationNoActionID");
                            db.AddInParameter(cmd, "@StatusID", DbType.Int64, auditBillEntity.StatusID);
                            db.AddInParameter(cmd, "@FlowLogID", DbType.Int64, auditBillEntity.FlowLogID);
                            db.AddInParameter(cmd, "@JournalID", DbType.Int64, auditBillEntity.JournalID);
                            db.AddInParameter(cmd, "@CID", DbType.Int64, auditBillEntity.CID);
                            db.AddInParameter(cmd, "@Processer", DbType.Int64, auditBillEntity.Processer);
                            db.AddInParameter(cmd, "@Receiver", DbType.Int64, ReceiverID);

                            if (auditBillEntity.DictDealAdvice.ContainsKey(ReceiverID))
                            {
                                //db.AddInParameter(cmd, "@DealAdvice", DbType.String, TextHelper.SubStr(Utils.ClearHtml(auditBillEntity.DictDealAdvice[ReceiverID]), 1000));
                                db.AddInParameter(cmd, "@DealAdvice", DbType.String, auditBillEntity.DictDealAdvice[ReceiverID]);
                            }
                            else
                            {
                                //db.AddInParameter(cmd, "@DealAdvice", DbType.String, string.IsNullOrEmpty(auditBillEntity.DealAdvice) ? "" : TextHelper.SubStr(Utils.ClearHtml(auditBillEntity.DealAdvice), 1000));
                                db.AddInParameter(cmd, "@DealAdvice", DbType.String, string.IsNullOrEmpty(auditBillEntity.DealAdvice) ? "" : auditBillEntity.DealAdvice);
                            }
                            db.AddInParameter(cmd, "@CPath", DbType.String, auditBillEntity.CPath);
                            db.AddInParameter(cmd, "@CFileName", DbType.String, auditBillEntity.CFileName);
                            db.AddInParameter(cmd, "@FigurePath", DbType.String, auditBillEntity.FigurePath);
                            db.AddInParameter(cmd, "@FFileName", DbType.String, auditBillEntity.FFileName);
                            db.AddInParameter(cmd, "@OtherPath", DbType.String, auditBillEntity.OtherPath);
                            db.ExecuteNonQuery(cmd);
                        }
                    }
                }

                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 处理在入款时改变稿件状态
        /// </summary>
        /// <param name="auditBillEntity"></param>
        /// <returns></returns>
        public bool DealFinaceInAccount(CirculationEntity cirEntity)
        {
            bool flag = false;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("UP_DealFinaceInAccount");
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, cirEntity.JournalID);
                db.AddInParameter(cmd, "@CID", DbType.Int64, cirEntity.CID);
                db.AddInParameter(cmd, "@Status", DbType.Int32, cirEntity.StatusID);
                db.AddInParameter(cmd, "@SendUserID", DbType.Int64, cirEntity.SendUserID);
                db.AddInParameter(cmd, "@ToStatus", DbType.Int32, cirEntity.ToStatusID);
                db.AddInParameter(cmd, "@CPath", DbType.String, string.IsNullOrEmpty(cirEntity.CPath) ? "" : cirEntity.CPath);
                db.AddInParameter(cmd, "@FigurePath", DbType.String, string.IsNullOrEmpty(cirEntity.FigurePath) ? "" : cirEntity.FigurePath);
                db.AddInParameter(cmd, "@OtherPath", DbType.String, string.IsNullOrEmpty(cirEntity.OtherPath) ? "" : cirEntity.OtherPath);
                db.AddInParameter(cmd, "@DealAdvice", DbType.String, string.IsNullOrEmpty(cirEntity.DealAdvice) ? "" : cirEntity.DealAdvice);
                db.AddInParameter(cmd, "@IsHaveBill", DbType.Int16, cirEntity.IsHaveBill);
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        # endregion

        # endregion

        # region 处理日志

        /// <summary>
        /// 获取处理日志信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public FlowLogInfoEntity GetFlowLogEntity(FlowLogInfoQuery query)
        {
            string sqlGetFlowlog = "SELECT TOP 1 fi.CID,fi.SendUserID,fi.RecUserID,StatusID,ActionID,TargetStatusID,CPath,CFileName,FigurePath,FFileName,OtherPath FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE JournalID=@JournalID AND FlowlogID=@FlowlogID";
            DbCommand cmdGetFlowlog = db.GetSqlStringCommand(sqlGetFlowlog);
            db.AddInParameter(cmdGetFlowlog, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmdGetFlowlog, "@FlowlogID", DbType.Int64, query.FlowLogID.Value);
            FlowLogInfoEntity flowlogEntity = new FlowLogInfoEntity();
            using (IDataReader dr = db.ExecuteReader(cmdGetFlowlog))
            {
                while (dr.Read())
                {
                    flowlogEntity.CID = TypeParse.ToLong(dr["CID"]);
                    flowlogEntity.SendUserID = TypeParse.ToLong(dr["SendUserID"]);
                    flowlogEntity.RecUserID = TypeParse.ToLong(dr["RecUserID"]);
                    flowlogEntity.StatusID = TypeParse.ToLong(dr["StatusID"]);
                    flowlogEntity.ActionID = TypeParse.ToLong(dr["ActionID"]);
                    flowlogEntity.TargetStatusID = TypeParse.ToLong(dr["TargetStatusID"]);
                    flowlogEntity.CPath = dr.IsDBNull(dr.GetOrdinal("CPath")) ? "" : dr["CPath"].ToString();
                    flowlogEntity.CFileName = dr.IsDBNull(dr.GetOrdinal("CFileName")) ? "" : dr["CFileName"].ToString();
                    flowlogEntity.FigurePath = dr.IsDBNull(dr.GetOrdinal("FigurePath")) ? "" : dr["FigurePath"].ToString();
                    flowlogEntity.FFileName = dr.IsDBNull(dr.GetOrdinal("FFileName")) ? "" : dr["FFileName"].ToString();
                    flowlogEntity.OtherPath = dr.IsDBNull(dr.GetOrdinal("OtherPath")) ? "" : dr["OtherPath"].ToString();
                }
                dr.Close();
            }
            return flowlogEntity;
        }

        /// <summary>
        /// 更新日志的查看状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public bool UpdateFlowLogIsView(FlowLogQuery flowLogQuery)
        {
            bool flag = false;
            try
            {
                string sql = "UPDATE dbo.FlowLogInfo SET IsView=1 WHERE FlowLogInfo.FlowLogID=@FlowLogID AND FlowLogInfo.JournalID=@JournalID";
                DbCommand cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "@FlowLogID", DbType.Int64, flowLogQuery.FlowLogID);
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, flowLogQuery.JournalID);
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 更新日志的下载状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public bool UpdateFlowLogIsDown(FlowLogQuery flowLogQuery)
        {
            bool flag = false;
            try
            {
                string sql = "UPDATE dbo.FlowLogInfo SET IsDown=1 WHERE FlowLogInfo.FlowLogID=@FlowLogID AND FlowLogInfo.JournalID=@JournalID";
                DbCommand cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "@FlowLogID", DbType.Int64, flowLogQuery.FlowLogID);
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, flowLogQuery.JournalID);
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public string GetFlowLogAttachment(FlowLogQuery flowLogQuery)
        {
            string CPath = "";
            string sql = "SELECT TOP 1 CPath FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.FlowLogID=@FlowLogID AND fi.JournalID=@JournalID";
            if (flowLogQuery.CID > 0 && flowLogQuery.FlowLogID == 0)
            {
                sql = "SELECT TOP 1 CPath FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.CID=@CID AND fi.JournalID=@JournalID AND ISNULL(CPath,'')<>'' ORDER BY AddDate DESC";
            }
            DbCommand cmd = db.GetSqlStringCommand(sql);
            if (flowLogQuery.CID > 0 && flowLogQuery.FlowLogID == 0)
            {
                db.AddInParameter(cmd, "@CID", DbType.Int64, flowLogQuery.CID);
            }
            else
            {
                db.AddInParameter(cmd, "@FlowLogID", DbType.Int64, flowLogQuery.FlowLogID);
            }
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, flowLogQuery.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if ((dr.Read()))
                {
                    CPath = dr.IsDBNull(dr.GetOrdinal("CPath")) ? "" : dr["CPath"].ToString();
                }
                dr.Close();
            }
            return CPath;
        }

        /// <summary>
        /// 获取处理日志的附件
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public IDictionary<string, string> GetFlowLogAllAttachment(FlowLogQuery flowLogQuery)
        {
            IDictionary<string, string> dictAttachment = new Dictionary<string, string>();
            string sql = "SELECT TOP 1 CPath,FigurePath,OtherPath FROM dbo.FlowLogInfo fi WITH(NOLOCK) WHERE fi.CID=@CID AND fi.JournalID=@JournalID AND (ISNULL(CPath,'')<>'' OR ISNULL(FigurePath,'')<>'' OR ISNULL(OtherPath,'')<>'') ORDER BY fi.AddDate DESC ";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@CID", DbType.Int64, flowLogQuery.CID);
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, flowLogQuery.JournalID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if ((dr.Read()))
                {
                    if (!dr.IsDBNull(dr.GetOrdinal("CPath")))
                    {
                        string cPath = dr["CPath"].ToString().Trim();
                        if (!string.IsNullOrEmpty(cPath))
                        {
                            dictAttachment.Add("cpath", cPath);
                        }
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("FigurePath")))
                    {
                        string figurePath = dr["FigurePath"].ToString().Trim();
                        if (!string.IsNullOrEmpty(figurePath))
                        {
                            dictAttachment.Add("figurepath", figurePath);
                        }
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("OtherPath")))
                    {
                        string otherPath = dr["OtherPath"].ToString().Trim();
                        if (!string.IsNullOrEmpty(otherPath))
                        {
                            dictAttachment.Add("otherpath", otherPath);
                        }
                    }
                }
                dr.Close();
            }
            return dictAttachment;
        }

        /// <summary>
        /// 获取稿件的处理日志
        /// </summary>
        /// <param name="cirEntity">稿件ID,JournalID,分组ID</param>
        /// <returns></returns>
        public IList<FlowLogInfoEntity> GetFlowLog(CirculationEntity cirEntity)
        {
            List<FlowLogInfoEntity> listFlowLogInfo = new List<FlowLogInfoEntity>();
            DbCommand cmd = null;
            if (cirEntity.isViewMoreFlow == 1)
            {
                cmd = db.GetStoredProcCommand("UP_GetContributionFlowLogForMoreFlow");
                db.AddInParameter(cmd, "@CID", DbType.Int64, cirEntity.CID);
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, cirEntity.JournalID);
                db.AddInParameter(cmd, "@GroupID", DbType.Byte, cirEntity.GroupID);
            }

            else if (cirEntity.isViewHistoryFlow == 1)
            {
                cmd = db.GetStoredProcCommand("UP_GetContributionFlowLogForMoreExpertFlow");
                db.AddInParameter(cmd, "@CID", DbType.Int64, cirEntity.CID);
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, cirEntity.JournalID);
                db.AddInParameter(cmd, "@ExpertID", DbType.Int64, cirEntity.AuthorID);
            }
            else
            {
                cmd = db.GetStoredProcCommand("UP_GetContributionFlowLog"); 
                db.AddInParameter(cmd, "@CID", DbType.Int64, cirEntity.CID);
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, cirEntity.JournalID);
                db.AddInParameter(cmd, "@GroupID", DbType.Byte, cirEntity.GroupID);
            }

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                FlowLogInfoEntity flowLogEntity = null;
                while (dr.Read())
                {
                    flowLogEntity = new FlowLogInfoEntity();
                    flowLogEntity.FlowLogID = (Int64)dr["FlowLogID"];
                    flowLogEntity.CID = (Int64)dr["CID"];
                    flowLogEntity.CNumber = dr["CNumber"].ToString();
                    flowLogEntity.JournalID = (Int64)dr["JournalID"];
                    flowLogEntity.SendUserID = (Int64)dr["SendUserID"];
                    flowLogEntity.RecUserID = (Int64)dr["RecUserID"];
                    flowLogEntity.StatusName = dr["StatusName"].ToString();
                    flowLogEntity.CStatus = dr["CStatus"] == System.DBNull.Value ? 0 : (int)dr["CStatus"];
                    flowLogEntity.ActionID = (Int64)dr["ActionID"];
                    flowLogEntity.ActionType = dr.IsDBNull(dr.GetOrdinal("ActionType")) ? (Byte)0 : (Byte)dr["ActionType"];
                    flowLogEntity.DealAdvice = dr.IsDBNull(dr.GetOrdinal("DealAdvice")) ? "" : dr["DealAdvice"].ToString();
                    flowLogEntity.AddDate = TypeParse.ToDateTime(dr["AddDate"]);
                    flowLogEntity.CPath = dr.IsDBNull(dr.GetOrdinal("CPath")) ? "" : dr["CPath"].ToString();
                    flowLogEntity.CFileName = dr.IsDBNull(dr.GetOrdinal("CFileName")) ? "" : dr["CFileName"].ToString();

                    flowLogEntity.FigurePath = dr.IsDBNull(dr.GetOrdinal("FigurePath")) ? "" : dr["FigurePath"].ToString();
                    flowLogEntity.FFileName = dr.IsDBNull(dr.GetOrdinal("FFileName")) ? "" : dr["FFileName"].ToString();
                    flowLogEntity.OtherPath = dr.IsDBNull(dr.GetOrdinal("OtherPath")) ? "" : dr["OtherPath"].ToString();
                    flowLogEntity.IsView = dr.IsDBNull(dr.GetOrdinal("IsView")) ? false : (Boolean)dr["IsView"];
                    flowLogEntity.IsDown = dr.IsDBNull(dr.GetOrdinal("IsDown")) ? false : (Boolean)dr["IsDown"];
                    flowLogEntity.IsHaveBill = dr.IsDBNull(dr.GetOrdinal("IsHaveBill")) ? (Byte)0 : (Byte)dr["IsHaveBill"];
                    flowLogEntity.DealDate = dr.GetDrValue<DateTime?>("DealDate");
                    flowLogEntity.TargetStatusID = (Int64)dr["TargetStatusID"];
                    if (flowLogEntity.ActionType == 2 || flowLogEntity.ActionType == 4)// 如果是原路返回接受者和发送者相反
                    {
                        flowLogEntity.SendRoleID = dr.IsDBNull(dr.GetOrdinal("RoleID")) ? 0 : (Int64)dr["RoleID"];
                        flowLogEntity.SendUserName = dr["RecUserName"].ToString();
                        flowLogEntity.SendUserEmail = dr["RecUserEmail"].ToString();
                        flowLogEntity.RecUserName = dr["SendUserName"].ToString();
                        flowLogEntity.RecUserEmail = dr["SendUserEmail"].ToString();
                        flowLogEntity.RecRoleID = dr.IsDBNull(dr.GetOrdinal("ActionRoleID")) ? 0 : (Int64)dr["ActionRoleID"];
                    }
                    else
                    {
                        flowLogEntity.SendRoleID = dr.IsDBNull(dr.GetOrdinal("RoleID")) ? 0 : (Int64)dr["RoleID"];
                        flowLogEntity.SendUserName = dr["SendUserName"].ToString();
                        flowLogEntity.SendUserEmail = dr["SendUserEmail"].ToString();
                        flowLogEntity.RecUserName = dr["RecUserName"].ToString();
                        flowLogEntity.RecUserEmail = dr["RecUserEmail"].ToString();
                        flowLogEntity.RecRoleID = dr.IsDBNull(dr.GetOrdinal("ActionRoleID")) ? 0 : (Int64)dr["ActionRoleID"];
                    }
                    listFlowLogInfo.Add(flowLogEntity);
                }
                dr.Close();
            }
            return listFlowLogInfo;
        }

        # endregion

        # region 获取作者和专家稿件列表

        /// <summary>
        /// 获取专家待审、已审稿件列表
        /// </summary>
        /// <param name="cirEntity">需要传值:JournalID，CurAuthorID，Status</param>
        /// <returns></returns>
        public Pager<FlowContribution> GetExpertContributionList(CirculationEntity cirEntity)
        {
            int recordCount = 0;
            List<SqlParameter> listParameters = new List<SqlParameter>();
            StringBuilder sbSQL = new StringBuilder("SELECT f.FlowLogID,c.CID,c.JournalID,c.CNumber,c.Title,c.SubjectCat,c.AuthorID,ca.AuthorName as FirstAuthor,c.Status,(SELECT TOP 1 fst.StatusName FROM dbo.FlowLogInfo fli WITH(NOLOCK) INNER JOIN dbo.FlowStatus fst WITH(NOLOCK) ON fli.JournalID=fst.JournalID AND fli.TargetStatusID=fst.StatusID WHERE fli.JournalID=f.JournalID AND fli.CID=f.CID and fli.RecUserID=@RecUserID ORDER BY fli.FlowLogID DESC) as AuditStatus,f.RecUserID,c.Flag,c.IsQuick,f.IsView,c.IsPayAuditFee,c.IsPayPageFee,c.IsRetractions,c.ContributePath,c.AddDate FROM dbo.FlowLogInfo f WITH(NOLOCK) INNER JOIN dbo.FlowStatus fs WITH(NOLOCK) ON f.TargetStatusID=fs.StatusID AND f.JournalID=fs.JournalID AND f.Status=@Status AND fs.ActionRoleID=@GroupID INNER JOIN dbo.ContributionInfo c WITH(NOLOCK) ON f.CID=c.CID AND f.JournalID=c.JournalID AND c.Status<>-999 AND c.Status<>-1 AND c.Status<>-100 INNER JOIN dbo.ContributionAuthor ca WITH(NOLOCK) ON c.JournalID=ca.JouranalID AND c.CID=ca.CID AND ca.IsFirst=1 WHERE f.JournalID=@JournalID AND f.RecUserID=@RecUserID AND f.ActionID<>0");

            # region 设置查询参数

            SqlParameter pJournalID = new SqlParameter("@JournalID", SqlDbType.BigInt);
            pJournalID.Value = cirEntity.JournalID;
            listParameters.Add(pJournalID);
            SqlParameter pRecUserID = new SqlParameter("@RecUserID", SqlDbType.BigInt);
            pRecUserID.Value = cirEntity.CurAuthorID;
            listParameters.Add(pRecUserID);
            SqlParameter pStatus = new SqlParameter("@Status", SqlDbType.TinyInt);
            pStatus.Value = cirEntity.Status;
            listParameters.Add(pStatus);

            SqlParameter pGroupID = new SqlParameter("@GroupID", SqlDbType.TinyInt);
            pGroupID.Value = cirEntity.GroupID;
            listParameters.Add(pGroupID);

            # endregion

            DataSet ds = db.PageingQuery(cirEntity.CurrentPage, cirEntity.PageSize, sbSQL.ToString(), "f.AddDate DESC", listParameters.ToArray(), ref recordCount);
            Pager<FlowContribution> pager = new Pager<FlowContribution>();
            if (ds != null && ds.Tables.Count > 0)
            {
                List<FlowContribution> list = new List<FlowContribution>();
                if (ds != null)
                {
                    FlowContribution cEntity = null;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        cEntity = new FlowContribution();
                        cEntity.FlowLogID = (Int64)row["FlowLogID"];
                        cEntity.CID = (Int64)row["CID"];
                        cEntity.JournalID = (Int64)row["JournalID"];
                        cEntity.CNumber = (String)row["CNumber"];
                        cEntity.Title = (String)row["Title"];
                        cEntity.SubjectCat = (Int32)row["SubjectCat"];
                        cEntity.AuthorID = (Int64)row["AuthorID"];
                        cEntity.FirstAuthor = row.IsNull("FirstAuthor") ? "" : (String)row["FirstAuthor"];
                        cEntity.Status = (Int32)row["Status"];
                        cEntity.IsQuick = TypeParse.ToBool(row["IsQuick"]);
                        cEntity.Flag = (String)row["Flag"];
                        cEntity.AuditStatus = row.IsNull("AuditStatus") ? "审核中" : (String)row["AuditStatus"];
                        cEntity.RecUserID = row.IsNull("RecUserID") ? 0 : (Int64)row["RecUserID"];
                        cEntity.IsView = row.IsNull("IsView") ? false : TypeParse.ToBool(row["IsView"]);
                        cEntity.IsPayAuditFee = row.IsNull("IsPayAuditFee") ? (byte)0 : (byte)row["IsPayAuditFee"];
                        cEntity.IsPayPageFee = row.IsNull("IsPayPageFee") ? (byte)0 : (byte)row["IsPayPageFee"];
                        cEntity.IsRetractions = row.IsNull("IsRetractions") ? false : TypeParse.ToBool(row["IsRetractions"]);
                        cEntity.ContributePath = row.IsNull("ContributePath") ? "" : row["ContributePath"].ToString();
                        cEntity.AddDate = Convert.ToDateTime(row["AddDate"]);
                        list.Add(cEntity);
                    }
                }
                pager.ItemList = list;
            }
            pager.CurrentPage = cirEntity.CurrentPage;
            pager.PageSize = cirEntity.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        # region 获取同步状态稿件列表，例如：专家拒审

        /// <summary>
        /// 获取同步状态稿件列表，例如：专家拒审
        /// </summary>
        /// <param name="cirEntity">需要传值:JournalID，CurAuthorID，EnumCStatus</param>
        /// <returns></returns>
        public Pager<FlowContribution> GetSynchroStatusContributionList(CirculationEntity cirEntity)
        {
            int recordCount = 0;
            List<SqlParameter> listParameters = new List<SqlParameter>();
            StringBuilder sbSQL = new StringBuilder("SELECT f.FlowLogID,c.CID,c.JournalID,c.CNumber,c.Title,c.SubjectCat,c.AuthorID,ca.AuthorName as FirstAuthor,c.Status,fs.StatusName as AuditStatus,f.RecUserID,c.Flag,c.IsQuick,f.IsView,c.IsPayAuditFee,c.IsPayPageFee,c.IsRetractions,c.ContributePath,c.AddDate FROM dbo.FlowLogInfo f WITH(NOLOCK) INNER JOIN dbo.FlowStatus fs WITH(NOLOCK) ON f.TargetStatusID=fs.StatusID AND f.JournalID=fs.JournalID AND fs.CStatus=@CStatusID INNER JOIN dbo.FlowAction fa WITH(NOLOCK) ON fs.JournalID=fa.JournalID AND fs.StatusID=fa.StatusID INNER JOIN dbo.ContributionInfo c WITH(NOLOCK) ON f.CID=c.CID AND f.JournalID=c.JournalID INNER JOIN dbo.ContributionAuthor ca WITH(NOLOCK) ON c.JournalID=ca.JouranalID AND c.CID=ca.CID AND ca.IsFirst=1 WHERE f.JournalID=@JournalID AND f.SendUserID=@RecUserID");

            # region 设置查询参数

            SqlParameter pJournalID = new SqlParameter("@JournalID", SqlDbType.BigInt);
            pJournalID.Value = cirEntity.JournalID;
            listParameters.Add(pJournalID);
            SqlParameter pRecUserID = new SqlParameter("@RecUserID", SqlDbType.BigInt);
            pRecUserID.Value = cirEntity.CurAuthorID;
            listParameters.Add(pRecUserID);
            SqlParameter pCStatus = new SqlParameter("@CStatusID", SqlDbType.Int);
            pCStatus.Value = (int)cirEntity.EnumCStatus;
            listParameters.Add(pCStatus);

            # endregion

            DataSet ds = db.PageingQuery(cirEntity.CurrentPage, cirEntity.PageSize, sbSQL.ToString(), "f.AddDate DESC", listParameters.ToArray(), ref recordCount);
            Pager<FlowContribution> pager = new Pager<FlowContribution>();
            if (ds != null && ds.Tables.Count > 0)
            {
                List<FlowContribution> list = new List<FlowContribution>();
                if (ds != null)
                {
                    FlowContribution cEntity = null;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        cEntity = new FlowContribution();
                        cEntity.FlowLogID = (Int64)row["FlowLogID"];
                        cEntity.CID = (Int64)row["CID"];
                        cEntity.JournalID = (Int64)row["JournalID"];
                        cEntity.CNumber = (String)row["CNumber"];
                        cEntity.Title = (String)row["Title"];
                        cEntity.SubjectCat = (Int32)row["SubjectCat"];
                        cEntity.AuthorID = (Int64)row["AuthorID"];
                        cEntity.FirstAuthor = row.IsNull("FirstAuthor") ? "" : (String)row["FirstAuthor"];
                        cEntity.Status = (Int32)row["Status"];
                        cEntity.AuditStatus = (String)row["AuditStatus"];
                        cEntity.IsQuick = TypeParse.ToBool(row["IsQuick"]);
                        cEntity.Flag = (String)row["Flag"];
                        cEntity.AuditStatus = (string)row["AuditStatus"];
                        cEntity.RecUserID = row.IsNull("RecUserID") ? 0 : (Int64)row["RecUserID"];
                        cEntity.IsView = row.IsNull("IsView") ? false : TypeParse.ToBool(row["IsView"]);
                        cEntity.IsPayAuditFee = row.IsNull("IsPayAuditFee") ? (byte)0 : (byte)row["IsPayAuditFee"];
                        cEntity.IsPayPageFee = row.IsNull("IsPayPageFee") ? (byte)0 : (byte)row["IsPayPageFee"];
                        cEntity.IsRetractions = row.IsNull("IsRetractions") ? false : TypeParse.ToBool(row["IsRetractions"]);
                        cEntity.ContributePath = row.IsNull("ContributePath") ? "" : row["ContributePath"].ToString();
                        cEntity.AddDate = Convert.ToDateTime(row["AddDate"]);
                        list.Add(cEntity);
                    }
                }
                pager.ItemList = list;
            }
            pager.CurrentPage = cirEntity.CurrentPage;
            pager.PageSize = cirEntity.PageSize;
            pager.TotalRecords = recordCount;
            return pager;
        }

        # endregion

        # endregion

        # region 状态合并操作

        /// <summary>
        /// 查看该稿件是否存在多个状态
        /// </summary>
        /// <param name="flowLogQuery">流程日志ID</param>
        /// <returns></returns>
        public long JudgeIsMoreStatus(FlowLogInfoQuery flowLogQuery)
        {
            long flag = 0;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("UP_JudgeContributionIsMoreStatus");
                db.AddInParameter(cmd, "@CID", DbType.Int64, flowLogQuery.CID.Value);
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, flowLogQuery.JournalID);
                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        flag = TypeParse.ToLong(reader[0]);
                    }
                    reader.Close();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 根据当前所处环节获取稿件列表
        /// </summary>
        /// <param name="cirEntity"></param>
        /// <returns></returns>
        public List<FlowContribution> GetContributionMoreStatusList(FlowLogInfoQuery query)
        {
            DbCommand cmd = db.GetStoredProcCommand("UP_GetContributionMoreStatusList");
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
            db.AddInParameter(cmd, "@CID", DbType.Int64, query.CID.Value);

            List<FlowContribution> list = new List<FlowContribution>();
            FlowContribution cEntity = null;
            using (IDataReader reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    cEntity = new FlowContribution();
                    cEntity.FlowLogID = (Int64)reader["FlowLogID"];
                    cEntity.CID = (Int64)reader["CID"];
                    cEntity.JournalID = (Int64)reader["JournalID"];
                    cEntity.CNumber = reader["CNumber"].ToString();
                    cEntity.Title = reader["Title"].ToString();
                    cEntity.AuthorID = (Int64)reader["AuthorID"];
                    cEntity.Status = (Int32)reader["Status"];
                    cEntity.AuditStatus = reader.IsDBNull(reader.GetOrdinal("AuditStatus")) ? "审核中" : reader["AuditStatus"].ToString();
                    cEntity.RecUserID = reader.IsDBNull(reader.GetOrdinal("RecUserID")) ? 0 : (Int64)reader["RecUserID"];
                    cEntity.AddDate = TypeParse.ToDateTime(reader["AddDate"]);
                    list.Add(cEntity);
                }
                reader.Close();
            }
            return list;
        }

        /// <summary>
        /// 合并多状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool MergeMoreStatus(FlowLogInfoQuery query)
        {
            bool flag = false;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("UP_MergeContributionMoreStatus");
                db.AddInParameter(cmd, "@CID", DbType.Int64, query.CID.Value);
                db.AddInParameter(cmd, "@JournalID", DbType.Int64, query.JournalID);
                db.AddInParameter(cmd, "@FlowLogID", DbType.Int64, query.FlowLogID.Value);
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return flag;
        }

        # endregion

        # region 继续送交

        /// <summary>
        /// 继续送交s
        /// </summary>
        /// <param name="sendEntity"></param>
        /// <returns></returns>
        public bool ContinuSend(ContinuSendEntity sendEntity)
        {
            bool flag = false;
            try
            {
                foreach (long RecUserID in sendEntity.RecArrayID)
                {
                    DbCommand cmd = db.GetStoredProcCommand("UP_ContinuSend");
                    db.AddInParameter(cmd, "@JournalID", DbType.Int64, sendEntity.JournalID);
                    db.AddInParameter(cmd, "@CID", DbType.Int64, sendEntity.CID);
                    db.AddInParameter(cmd, "@RecUserID", DbType.Int64, RecUserID);
                    db.AddInParameter(cmd, "@SendUserID", DbType.Int64, sendEntity.SendUserID);
                    db.AddInParameter(cmd, "@StatusID", DbType.Int64, sendEntity.StatusID);
                    db.ExecuteNonQuery(cmd);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return flag;
        }

        # endregion
    }
}
