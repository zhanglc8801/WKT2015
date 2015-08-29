using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.BLL;
using WKT.Service.Interface;
using WKT.Model.Enum;
using WKT.Common.Extension;

namespace WKT.Service
{
    public partial class ContributionInfoService:IContributionInfoService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IContributionInfoBusiness contributionInfoBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IContributionInfoBusiness ContributionInfoBusProvider
        {
            get
            {
                 if(contributionInfoBusProvider == null)
                 {
                      contributionInfoBusProvider = new ContributionInfoBusiness();//ServiceBusContainer.Instance.Container.Resolve<IContributionInfoBusiness>();
                 }
                 return contributionInfoBusProvider;
            }
            set
            {
              contributionInfoBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ContributionInfoService()
        {
        }

        #region 稿件信息

        /// <summary>
        /// 获取稿件的附件
        /// </summary>
        /// <param name="cQuery">稿件ID</param>
        /// <returns></returns>
        public string GetContributionAttachment(ContributionInfoQuery cQuery)
        {
            return ContributionInfoBusProvider.GetContributionAttachment(cQuery);
        }

        /// <summary>
        /// 投稿
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult AuthorPlatform(ContributionInfoEntity model)
        {
            ExecResult execResult = new ExecResult();
            if (ContributionTitleIsExists(model.JournalID, model.CID, model.Title))
            {
                execResult.result = EnumJsonResult.error.ToString();
                execResult.msg = "该稿件名称已经存在，请重新填写！";
                execResult.resultID = model.CID;
                return execResult;
            }
            string msg = string.Empty;
            bool isAdd = false;
            if (model.CID == 0)
            {
                if (model.Status == -2)
                    msg = "保存稿件信息到格式修改";
                else if (model.Status == -1)
                    msg = "保存稿件为草稿";
                else
                    msg = "投稿";
                // 只有新投稿时才产生稿件编号
                if (model.Status == 0 && string.IsNullOrEmpty(model.CNumber))
                {
                    ContributeSetService service = new ContributeSetService();
                    QueryBase query = new QueryBase();
                    query.JournalID = model.JournalID;
                    model.CNumber = service.GetContributeNumber(query);
                }
                isAdd = true;
            }
            else
            {
                if (model.OldStatus == -3 || model.OldStatus == 100)
                    model.Status = 10;
                msg = "编辑稿件信息";
                if (string.IsNullOrEmpty(model.CNumber))
                {
                    ContributeSetService service = new ContributeSetService();
                    QueryBase query = new QueryBase();
                    query.JournalID = model.JournalID;
                    model.CNumber = service.GetContributeNumber(query);
                }
            }
            model.CID = ContributionInfoBusProvider.AuthorPlatform(model);
            if (model.CID > 0)
            {
                if (isAdd)
                    GetFlow(model, EnumContributionStatus.New);
                else
                {
                    if (model.OldStatus == -3)
                        GetFlow(model, EnumContributionStatus.Retreat);
                    else if(model.OldStatus==100)
                        GetFlow(model, EnumContributionStatus.Proof);
                }
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = msg + "成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = msg + "失败！";
            }
            execResult.resultID = model.CID;
            execResult.resultStr = model.CNumber;
            return execResult;
        }

        /// <summary>
        /// 保存稿件格式修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveFormat(ContributionInfoEntity model)
        {
            ExecResult result = new ExecResult();
            bool flag = ContributionInfoBusProvider.SaveFormat(model);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "修改稿件格式成功！";
            }
            else
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = "修改稿件格式失败！";
            }
            return result;
        }

        /// <summary>
        /// 审稿相关数据处理
        /// </summary>
        /// <param name="model"></param>
        private void GetFlow(ContributionInfoEntity model,EnumContributionStatus enumStatus)
        {
            FlowCirculationBusiness business = new FlowCirculationBusiness();
            CirculationEntity item = new CirculationEntity();
            item.CID = model.CID;
            item.AuthorID = model.AuthorID;
            item.CNumber = model.CNumber;
            item.JournalID = model.JournalID;
            item.SubjectCategoryID = model.SubjectCat;
            item.EnumCStatus = enumStatus;
            item.CPath = model.ContributePath;
            item.FigurePath = model.FigurePath;
            item.OtherPath = model.IntroLetterPath;
            business.AuthorContribution(item);
        }

        /// <summary>
        /// 新增稿件表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddContributionInfo(ContributionInfoEntity model)
        {
            return ContributionInfoBusProvider.AddContributionInfo(model);
        }

        /// <summary>
        /// 编辑稿件表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateContributionInfo(ContributionInfoEntity model)
        {
            return ContributionInfoBusProvider.UpdateContributionInfo(model);
        }

        /// <summary>
        /// 获取稿件信息实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ContributionInfoEntity GetContributionInfo(ContributionInfoQuery query)
        {
            var model = ContributionInfoBusProvider.GetContributionInfo(query.CID);
            if (model == null)
                return model;
            if (query.IsAuxiliary)
            {
                model.AuthorList = GetContributionAuthorList(new ContributionAuthorQuery() { JournalID = query.JournalID, CID = query.CID, isModify = query.isModify });
                model.ReferenceList = GetContributionReferenceList(new ContributionReferenceQuery() { JournalID = query.JournalID, CID = query.CID });
                model.FundList = GetContributionFundList(new ContributionFundQuery() { JournalID = query.JournalID, CID = query.CID });
                model.AttModel = GetContributionInfoAttByCID(query.CID);
            }
            return model;
        }

        /// <summary>
        /// 判断稿件标题是否存在
        /// </summary>
        /// <param name="JournalID"></param>
        /// <param name="CID"></param>
        /// <param name="CTitle"></param>
        /// <returns></returns>
        public bool ContributionTitleIsExists(Int64 JournalID, Int64 CID, string CTitle)
        {
            return ContributionInfoBusProvider.ContributionTitleIsExists(JournalID, CID, CTitle);
        }

        /// <summary>
        /// 改变稿件状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult ChangeStatus(ContributionInfoQuery query)
        {
            ExecResult result = new ExecResult();
            bool flag = ContributionInfoBusProvider.ChangeStatus(query);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "更新稿件状态成功！";
            }
            else
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = "更新稿件状态失败！";
            }
            return result;
        }

        /// <summary>
        /// 删除投稿信息
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ExecResult DelContributionInfo(Int64[] CID)
        {
            ExecResult result = new ExecResult();
            bool flag = ContributionInfoBusProvider.DelContributionInfo(CID);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "删除稿件信息成功！";
            }
            else
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = "删除稿件信息失败！";
            }
            return result;
        }

        /// <summary>
        /// 获取稿件分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionInfoEntity> GetContributionInfoPageList(ContributionInfoQuery query)
        {
            return ContributionInfoBusProvider.GetContributionInfoPageList(query);
        }

        /// <summary>
        /// 获取稿件数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionInfoEntity> GetContributionInfoList(ContributionInfoQuery query)
        {
            IList<ContributionInfoEntity> listC =  ContributionInfoBusProvider.GetContributionInfoList(query);
            
            if (query.IsAuxiliary)
            {
                JournalChannelQuery jChannelQuery = new JournalChannelQuery();
                jChannelQuery.JournalID = query.JournalID;
                IssueService service = new IssueService();
                IList<JournalChannelEntity> list = service.GetJournalChannelList(jChannelQuery);
                foreach (ContributionInfoEntity item in listC)
                {
                    JournalChannelEntity itemChannel = list.SingleOrDefault(p => p.JChannelID == item.JChannelID);
                    if (itemChannel != null)
                    {
                        item.JChannelName = itemChannel.ChannelName;
                    }
                    item.AuthorList = GetContributionAuthorList(new ContributionAuthorQuery() { JournalID = item.JournalID, CID = item.CID });
                }
            }
            return listC;
        }
        #endregion

        #region 稿件作者信息
        /// <summary>
        /// 新增稿件作者信息
        /// </summary>
        /// <param name="contributionAuthorEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool AddContributionAuthor(ContributionAuthorEntity contributionAuthorEntity)
        {
            return ContributionInfoBusProvider.AddContributionAuthor(contributionAuthorEntity);
        }

        /// <summary>
        /// 编辑稿件作者信息
        /// </summary>
        /// <param name="contributionAuthorEntity"></param>      
        /// <returns></returns>
        public bool UpdateContributionAuthor(ContributionAuthorEntity contributionAuthorEntity)
        {
            return ContributionInfoBusProvider.UpdateContributionAuthor(contributionAuthorEntity);
        }

        /// <summary>
        /// 获取稿件作者实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionAuthorEntity GetContributionAuthor(Int64 CAuthorID)
        {
            return ContributionInfoBusProvider.GetContributionAuthor(CAuthorID);
        }

        /// <summary>
        /// 根据稿件编号删除稿件作者信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelContributionAuthorByCID(Int64[] CID)
        {
            return ContributionInfoBusProvider.DelContributionAuthorByCID(CID);
        }

        /// <summary>
        /// 删除稿件作者信息
        /// </summary>
        /// <param name="CAuthorID"></param>
        /// <returns></returns>
        public bool DelContributionAuthor(Int64[] CAuthorID)
        {
            return ContributionInfoBusProvider.DelContributionAuthor(CAuthorID);
        }

        /// <summary>
        /// 获取稿件作者分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionAuthorEntity> GetContributionAuthorPageList(ContributionAuthorQuery query)
        {
            return ContributionInfoBusProvider.GetContributionAuthorPageList(query);
        }

        /// <summary>
        /// 获取稿件作者数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionAuthorEntity> GetContributionAuthorList(ContributionAuthorQuery query)
        {
            return ContributionInfoBusProvider.GetContributionAuthorList(query);
        }
        #endregion

        #region 参考文献信息
        /// <summary>
        /// 新增参考文献
        /// </summary>
        /// <param name="contributionReferenceEntity"></param>
        /// <returns></returns>
        public bool AddContributionReference(ContributionReferenceEntity contributionReferenceEntity)
        {
            return ContributionInfoBusProvider.AddContributionReference(contributionReferenceEntity);
        }

        /// <summary>
        /// 编辑参考文献
        /// </summary>
        /// <param name="contributionReferenceEntity"></param>
        /// <returns></returns>
        public bool UpdateContributionReference(ContributionReferenceEntity contributionReferenceEntity)
        {
            return ContributionInfoBusProvider.UpdateContributionReference(contributionReferenceEntity);
        }

        /// <summary>
        /// 获取参考文献实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionReferenceEntity GetContributionReference(Int64 ReferenceID)
        {
            return ContributionInfoBusProvider.GetContributionReference(ReferenceID);
        }

        /// <summary>
        /// 根据稿件编号删除参考文献
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelContributionReferenceByCID(Int64[] CID)
        {
            return ContributionInfoBusProvider.DelContributionReferenceByCID(CID);
        }

        /// <summary>
        /// 删除参考文献
        /// </summary>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        public bool DelContributionReference(Int64[] ReferenceID)
        {
            return ContributionInfoBusProvider.DelContributionReference(ReferenceID);
        }

        /// <summary>
        /// 获取参考文献分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionReferenceEntity> GetContributionReferencePageList(ContributionReferenceQuery query)
        {
            return ContributionInfoBusProvider.GetContributionReferencePageList(query);
        }

        /// <summary>
        /// 获取参考文献数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionReferenceEntity> GetContributionReferenceList(ContributionReferenceQuery query)
        {
            return ContributionInfoBusProvider.GetContributionReferenceList(query);
        }
        #endregion

        #region 基金信息
        /// <summary>
        /// 新增基金信息
        /// </summary>
        /// <param name="contributionFundEntity"></param>
        /// <returns></returns>
        public bool AddContributionFund(ContributionFundEntity contributionFundEntity)
        {
            return ContributionInfoBusProvider.AddContributionFund(contributionFundEntity);
        }

        /// <summary>
        /// 编辑基金信息
        /// </summary>
        /// <param name="contributionFundEntity"></param>
        /// <returns></returns>
        public bool UpdateContributionFund(ContributionFundEntity contributionFundEntity)
        {
            return ContributionInfoBusProvider.UpdateContributionFund(contributionFundEntity);
        }

        /// <summary>
        /// 获取基金信息实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionFundEntity GetContributionFund(Int64 FundID)
        {
            return ContributionInfoBusProvider.GetContributionFund(FundID);
        }

        /// <summary>
        /// 根据稿件编号删除基金信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelContributionFundByCID(Int64[] CID)
        {
            return ContributionInfoBusProvider.DelContributionFundByCID(CID);
        }

        /// <summary>
        /// 删除基金信息
        /// </summary>
        /// <param name="FundID"></param>
        /// <returns></returns>
        public bool DelContributionFund(Int64[] FundID)
        {
            return ContributionInfoBusProvider.DelContributionFund(FundID);
        }

        /// <summary>
        /// 获取基金信息分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionFundEntity> GetContributionFundPageList(ContributionFundQuery query)
        {
            return ContributionInfoBusProvider.GetContributionFundPageList(query);
        }

        /// <summary>
        /// 获取基金信息数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionFundEntity> GetContributionFundList(ContributionFundQuery query)
        {
            return ContributionInfoBusProvider.GetContributionFundList(query);
        }
        #endregion

        #region 大字段信息
        /// <summary>
        /// 获取大字段信息实体
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public ContributionInfoAttEntity GetContributionInfoAtt(Int64 PKID)
        {
            return ContributionInfoBusProvider.GetContributionInfoAtt(PKID);
        }

        /// <summary>
        /// 获取大字段信息实体
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public ContributionInfoAttEntity GetContributionInfoAttByCID(Int64 CID)
        {
            return ContributionInfoBusProvider.GetContributionInfoAttByCID(CID);
        }

        /// <summary>
        /// 根据稿件编号删除大字段信息
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DelContributionInfoAttByCID(Int64[] CID)
        {
            return ContributionInfoBusProvider.DelContributionInfoAttByCID(CID);
        }

        /// <summary>
        /// 删除大字段信息
        /// </summary>
        /// <param name="FundID"></param>
        /// <returns></returns>
        public bool DelContributionInfoAtt(Int64[] PKID)
        {
            return ContributionInfoBusProvider.DelContributionInfoAtt(PKID);
        }
        #endregion

        #region 撤稿相关
        /// <summary>
        /// 撤稿
        /// </summary>
        /// <param name="retractionsBillsEntity"></param>
        /// <returns></returns>
        public ExecResult DraftContribution(RetractionsBillsEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;                
            if (model.PKID == 0)
                result = ContributionInfoBusProvider.DraftContribution(model);
            else
                result = UpdateRetractionsBills(model);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "撤稿成功！";    
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "撤稿失败！";
            }
            return execResult;
        }

        /// <summary>
        /// 添加撤稿信息
        /// </summary>
        /// <param name="retractionsBillsEntity"></param>
        /// <returns></returns>
        public bool AddRetractionsBills(RetractionsBillsEntity model)
        {
            return ContributionInfoBusProvider.AddRetractionsBills(model);
        }

        /// <summary>
        /// 编辑撤稿信息
        /// </summary>
        /// <param name="retractionsBillsEntity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool UpdateRetractionsBills(RetractionsBillsEntity model)
        {
            return ContributionInfoBusProvider.UpdateRetractionsBills(model);
        }

        /// <summary>
        /// 获取撤稿信息
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns></returns>
        public RetractionsBillsEntity GetRetractionsBills(RetractionsBillsQuery rQuery)
        {
            return ContributionInfoBusProvider.GetRetractionsBills(rQuery);
        }
        #endregion

        #region 添加稿件备注
        /// <summary>
        /// 新增稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddCRemark(CRemarkEntity model)
        {
            return ContributionInfoBusProvider.AddCRemark(model);
        }

        /// <summary>
        /// 编辑稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCRemark(CRemarkEntity model)
        {
            return ContributionInfoBusProvider.UpdateCRemark(model);
        }

        /// <summary>
        /// 保存稿件备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult SaveCRemark(CRemarkEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            if (model.RemarkID == 0)
                result = AddCRemark(model);
            else
                result = UpdateCRemark(model);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "添加备注成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "添加备注失败！";
            }
            return execResult;
        }

        /// <summary>
        /// 获取稿件备注
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public CRemarkEntity GetCRemark(CRemarkQuery query)
        {
            return ContributionInfoBusProvider.GetCRemark(query);
        }
        #endregion

        # region 更新稿件标记及状态 by sxd

        /// <summary>
        /// 更新稿件旗帜标记
        /// </summary>        
        /// <param name="cEntityList">稿件信息</param>
        /// <returns></returns>
        public bool BatchUpdateContributionFlag(List<ContributionInfoQuery> cEntityList)
        {
            return ContributionInfoBusProvider.BatchUpdateContributionFlag(cEntityList);
        }

        /// <summary>
        /// 更新稿件旗帜标记
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool UpdateContributionFlag(ContributionInfoQuery cEntity)
        {
            return ContributionInfoBusProvider.UpdateContributionFlag(cEntity);
        }

        /// <summary>
        /// 更新稿件加急标记
        /// </summary>        
        /// <param name="cEntityList">稿件信息</param>
        /// <returns></returns>
        public bool BatchUpdateContributionIsQuick(List<ContributionInfoQuery> cEntityList)
        {
            return ContributionInfoBusProvider.BatchUpdateContributionIsQuick(cEntityList);
        }

        /// <summary>
        /// 更新稿件加急标记
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool UpdateContributionIsQuick(ContributionInfoQuery cEntity)
        {
            return ContributionInfoBusProvider.UpdateContributionIsQuick(cEntity);
        }

        /// <summary>
        /// 更新稿件状态及审核状态
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool UpdateContributionStatus(ContributionInfoQuery cEntity)
        {
            return ContributionInfoBusProvider.UpdateContributionStatus(cEntity);
        }

        # endregion

        #region 收稿量统计
        /// <summary>
        /// 按年月统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionAccountEntity> GetContributionAccountListByYear(ContributionAccountQuery query)
        {
            IList<ContributionAccountEntity> list = ContributionInfoBusProvider.GetContributionAccountListByYear(query);
            if (list == null || list.Count == 0)
            {
                list.Add(new ContributionAccountEntity() { Year = query.Year, Month = 1, Account = 0 });
            }
            var groupYear = list.GroupBy(p => p.Year);
            foreach (var item in groupYear)
            {
                for (int i = 1; i < 13; i++)
                {
                    if (item.Where(p => p.Year == item.Key && p.Month == i).Count() == 0)//补充缺少月份数据
                    {
                        list.Add(new ContributionAccountEntity() { Year = item.Key, Month = i, Account = 0 });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 按基金级别统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<ContributionAccountEntity> GetContributionAccountListByFund(ContributionAccountQuery query)
        {
            IList<ContributionAccountEntity> list = ContributionInfoBusProvider.GetContributionAccountListByFund(query);
            DictValueService service = new DictValueService();
            var dictLevel = service.DictValueBusProvider.GetDictValueDcit(query.JournalID, EnumDictKey.FundLevel.ToString());
            if (list == null)
                list = new List<ContributionAccountEntity>();
            foreach (var item in dictLevel)
            {
                if (list.Where(p => p.FundLevel == item.Key).Count()==0)
                {
                    list.Add(new ContributionAccountEntity() { FundLevel = item.Key, Account = 0 });
                }
            }
            foreach (var model in list)
            {
                model.FundName = dictLevel.GetValue(model.FundLevel, model.FundLevel.ToString());
            }

            return list;
        }

        /// <summary>
        /// 按作者统计收稿量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Pager<ContributionAccountEntity> GetContributionAccountListByAuhor(ContributionAccountQuery query)
        {
            Pager<ContributionAccountEntity> pager = ContributionInfoBusProvider.GetContributionAccountListByAuhor(query);
            if (pager == null) return pager;
            IList<ContributionAccountEntity> list = pager.ItemList;
            if (list == null || list.Count == 0)
                return pager;
            AuthorInfoService service = new AuthorInfoService();
            AuthorInfoQuery aQuery = new AuthorInfoQuery();
            aQuery.JournalID = query.JournalID;
            var dict = service.AuthorInfoBusProvider.GetAuthorDict(aQuery);
            foreach (var model in list)
            {
                model.AuthorName = dict.GetValue(model.AuthorID, model.AuthorID.ToString());
            }
            pager.ItemList = list;
            return pager;
        }
        #endregion

        /// <summary>
        /// 更新介绍信标记
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateIntroLetter(ContributionInfoQuery model)
        {
            return ContributionInfoBusProvider.UpdateIntroLetter(model);
        }

        /// <summary>
        /// 处理撤稿申请
        /// </summary>        
        /// <param name="cEntity">稿件信息</param>
        /// <returns></returns>
        public bool DealWithdrawal(ContributionInfoQuery cEntity)
        {
            return ContributionInfoBusProvider.DealWithdrawal(cEntity);
        }
        /// <summary>
        /// 撤销删除
        /// </summary>
        /// <param name="cEntity"></param>
        /// <returns></returns>
        public bool CancelDelete(ContributionInfoQuery cEntity)
        {
            return ContributionInfoBusProvider.CancelDelete(cEntity);
        }

        /// <summary>
        /// 获取稿件作者数据字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IDictionary<Int64, String> GetContributionAuthorDict(ContributionInfoQuery query)
        {
            return ContributionInfoBusProvider.GetContributionAuthorDict(query);
        }

    }
}
