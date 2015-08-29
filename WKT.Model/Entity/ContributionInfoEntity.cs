using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{

		
	/// <summary>
	///  表'ContributionInfo'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class ContributionInfoEntity : ObjectBase
	{
		#region 属性、变量声明

		#region 主属性 --对应数据表主键
		/// <summary>			
		/// CID : 
		/// </summary>
		/// <remarks>表ContributionInfo主键</remarks>		
		[DataMember]
		public Int64 CID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// CNumber : 稿件编号
		/// </summary>       
		[DataMember]
		public String CNumber
        {
            get;
            set;
        }
		
		/// <summary>
		/// AuthorID : 作者ID
		/// </summary>       
		[DataMember]
		public Int64 AuthorID
        {
            get;
            set;
        }

        /// <summary>
        /// AuthorID:作者名称
        /// </summary>
        public string AuthorName
        {
            get;
            set;
        }



		/// <summary>
		/// JournalID : 杂志ID
		/// </summary>      
		[DataMember]
		public Int64 JournalID
        {
            get;
            set;
        }
		
		/// <summary>
		/// Title : 稿件标题
		/// </summary>       
		[DataMember]
		public String Title
        {
            get;
            set;
        }
		
		/// <summary>
		/// EnTitle : 英文标题
		/// </summary>      
		[DataMember]
		public String EnTitle
        {
            get;
            set;
        }
		
		/// <summary>
		/// SubjectCat : 学科分类，数据字典
		/// </summary>       
		[DataMember]
		public Int32 SubjectCat
        {
            get;
            set;
        }
		
		/// <summary>
		/// JChannelID : 拟投栏目ID，期刊栏目设置中的栏目
		/// </summary>       
		[DataMember]
		public Int64 JChannelID
        {
            get;
            set;
        }

        /// <summary>
        /// JChannelName : 拟投栏目ID，期刊栏目设置中的栏目
        /// </summary>       
        [DataMember]
        public string JChannelName
        {
            get;
            set;
        }
		
		/// <summary>
		/// ContributionType : 投稿类型 约稿、投稿等，数据字典
		/// </summary>       
		[DataMember]
		public Int32 ContributionType
        {
            get;
            set;
        }

        /// <summary>
        /// Special : 约稿人，数据字典
        /// </summary>       
        [DataMember]
        public Int32 Special
        {
            get;
            set;
        }
		
		/// <summary>
		/// Keywords : 中文关键词
		/// </summary>      
		[DataMember]
		public String Keywords
        {
            get;
            set;
        }
		
		/// <summary>
		/// EnKeywords : 英文关键词
		/// </summary>       
		[DataMember]
		public String EnKeywords
        {
            get;
            set;
        }
		
		/// <summary>
		/// CLC : 中图分类号
		/// </summary>       
		[DataMember]
		public String CLC
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsFund : 是否有基金项目 1=有 0=无
		/// </summary>       
		[DataMember]
		public Boolean IsFund
        {
            get;
            set;
        }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [DataMember]
        public String InvoiceTitle { get;set;}
		
		/// <summary>
		/// CommendExpert : 推荐专家
		/// </summary>       
		[DataMember]
		public String CommendExpert
        {
            get;
            set;
        }
		
		/// <summary>
		/// ContributePath : 稿件上传后的保存目录
		/// </summary>       
		[DataMember]
		public String ContributePath
        {
            get;
            set;
        }
		
		/// <summary>
		/// FigurePath : 附图上传后的保存路径
		/// </summary>       
		[DataMember]
		public String FigurePath
        {
            get;
            set;
        }
		
		/// <summary>
		/// PDFPath : 稿件PDF格式文件保存路径
		/// </summary>       
		[DataMember]
		public String PDFPath
        {
            get;
            set;
        }
		
		/// <summary>
		/// IntroLetterPath : 介绍信路径
		/// </summary>       
		[DataMember]
		public String IntroLetterPath
        {
            get;
            set;
        }
		
		/// <summary>
		/// WordNiumber : 稿件字数
		/// </summary>      
		[DataMember]
		public Int32? WordNiumber
        {
            get;
            set;
        }
		
		/// <summary>
		/// FigureNumber : 附图数
		/// </summary>       
		[DataMember]
		public Int32? FigureNumber
        {
            get;
            set;
        }
		/// <summary>
		/// TableNumber : 附表数
		/// </summary>       
		[DataMember]
		public Int32? TableNumber
        {
            get;
            set;
        }
		
		/// <summary>
		/// ReferenceNumber : 参考文献数
		/// </summary>      
		[DataMember]
		public Int32? ReferenceNumber
        {
            get;
            set;
        }
		
		/// <summary>
        /// Status : 稿件状态 -1=草稿 0=新稿件 ，编辑还没有处理 10=审核中 100=已发校样 200=录用稿件 300=刊出稿件 -100=退稿 -2=格式修改 -3=退修稿件,-999=删除
		/// </summary>        
		[DataMember]
		public Int32 Status
        {
            get;
            set;
        }

        /// <summary>
        /// 旧状态
        /// </summary>
        [DataMember]
        public Int32 OldStatus { get; set; }
		
		/// <summary>
		/// IsPayPageFee : 是否已交版面费 1=已交 0=未交
		/// </summary>      
		[DataMember]
		public Byte? IsPayPageFee
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsPayAuditFee : 是否已交审稿费 0=未交 1=已交
		/// </summary>      
		[DataMember]
		public Byte? IsPayAuditFee
        {
            get;
            set;
        }
		
		/// <summary>
		/// Year : 刊出后，显示刊出后所在的年
		/// </summary>      
		[DataMember]
		public Int32? Year
        {
            get;
            set;
        }
		
		/// <summary>
		/// Volumn : 刊出后，显示刊出后所在的卷
		/// </summary>       
		[DataMember]
		public Int32? Volumn
        {
            get;
            set;
        }
		
		/// <summary>
		/// Issue : 刊出后，显示刊出后所在的期
		/// </summary>        
		[DataMember]
		public Int32? Issue
        {
            get;
            set;
        }
		
		/// <summary>
		/// SortID : 排序
		/// </summary>       
		[DataMember]
		public Int32? SortID
        {
            get;
            set;
        }
		
		/// <summary>
		/// HireChannelID : 录用后所属栏目
		/// </summary>       
		[DataMember]
		public Int64? HireChannelID
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsQuick : 是否是加急稿件 0=否 1=是
		/// </summary>       
		[DataMember]
		public Boolean? IsQuick
        {
            get;
            set;
        }

        /// <summary>
        /// IsRetractions : 是否申请撤搞 true:是
        /// </summary>       
        [DataMember]
        public Boolean? IsRetractions
        {
            get;
            set;
        }
		
		/// <summary>
		/// ReserveField : 备用字段
		/// </summary>      
		[DataMember]
		public String ReserveField
        {
            get;
            set;
        }
		
		/// <summary>
		/// ReserveField1 : 备用字段
		/// </summary>      
		[DataMember]
		public String ReserveField1
        {
            get;
            set;
        }
		
		/// <summary>
		/// ReserveField2 : 备用字段
		/// </summary>       
		[DataMember]
		public String ReserveField2
        {
            get;
            set;
        }
		
		/// <summary>
		/// ReserveField3 : 备用字段
		/// </summary>       
		[DataMember]
		public String ReserveField3
        {
            get;
            set;
        }
		
		/// <summary>
		/// ReserveField4 : 备用字段
		/// </summary>       
		[DataMember]
		public String ReserveField4
        {
            get;
            set;
        }
		
		/// <summary>
		/// ReserveField5 : 备用字段
		/// </summary>       
		[DataMember]
		public String ReserveField5
        {
            get;
            set;
        }
		
		/// <summary>
		/// AddDate : 投稿日期
		/// </summary>       
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }

        
		#endregion

        /// <summary>
        /// 大字段
        /// </summary>
        [DataMember]
        public ContributionInfoAttEntity AttModel { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DataMember]
        public IList<ContributionAuthorEntity> AuthorList { get; set; }

        /// <summary>
        /// 基金
        /// </summary>
        [DataMember]
        public IList<ContributionFundEntity> FundList { get; set; }

        /// <summary>
        /// 参考文献
        /// </summary>
        [DataMember]
        public IList<ContributionReferenceEntity> ReferenceList { get; set; }
        
        /// <summary>
        /// 稿件字段设置列表
        /// </summary>
        [DataMember]
        public List<FieldsSet> FieldList { get; set; }

        /// <summary>
        /// 稿件作者字段设置列表
        /// </summary>
        [DataMember]
        public List<FieldsSet> ContributeAuthorFieldList { get; set; }

        /// <summary>
        /// 最新的一条流程记录
        /// </summary>
        [DataMember]
        public FlowLogInfoEntity FlowLogInfo { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [DataMember]
        public CRemarkEntity CRemarkInfo { get; set; }

		#endregion 属性、变量声明

        /// <summary>
        /// 旗帜标志
        /// </summary>
        [DataMember]
        public string Flag
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件状态
        /// </summary>
        public long StatusID
        {
            get;
            set;
        }

        /// <summary>
        /// 流程日志ID
        /// </summary>
        public long FlowLogID
        {
            get;
            set;
        }
		
	    public ContributionInfoEntity()
        {
            CID = (long)0;
            CNumber = string.Empty;
            AuthorID = (long)0;
            JournalID = (long)0;
            Title = string.Empty;
            EnTitle = null;
            SubjectCat = (int)0;
            JChannelID = (long)0;
            ContributionType = 0;
            Keywords = null;
            EnKeywords = null;
            CLC = null;
            IsFund = false;
            InvoiceTitle = string.Empty;
            CommendExpert = null;
            ContributePath = null;
            FigurePath = null;
            PDFPath = null;
            IntroLetterPath = null;
            WordNiumber = null;
            FigureNumber = null;
            TableNumber = null;
            ReferenceNumber = null;
            Status = (int)0;
            OldStatus = (int)0;
            IsPayPageFee = null;
            IsPayAuditFee = null;
            Year = null;
            Volumn = null;
            Issue = null;
            SortID = null;
            HireChannelID = null;
            IsQuick = null;
            IsRetractions = null;
            ReserveField = null;
            ReserveField1 = null;
            ReserveField2 = null;
            ReserveField3 = null;
            ReserveField4 = null;
            ReserveField5 = null;
            AddDate = DateTime.Now;
            IsRetractions = false;
            AttModel = null;
            AuthorList = null;
            FundList = null;
            ReferenceList = null;           
        }
	}

    /// <summary>
    /// 收稿量统计
    /// </summary>
    [DataContract]
    public class ContributionAccountEntity:ObjectBase
    {
        /// <summary>
        /// 作者编号
        /// </summary>
        [DataMember]
        public Int64 AuthorID { get; set; }

        /// <summary>
        /// 作者姓名
        /// </summary>
        [DataMember]
        public String AuthorName { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        [DataMember]
        public Int32 Year { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        [DataMember]
        public String YearName
        {
            get { return Year.ToString() + "年"; }
        }

        /// <summary>
        /// 月
        /// </summary>
        [DataMember]
        public Int32 Month { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        [DataMember]
        public String MonthName
        {
            get { return Month.ToString() + "月"; }
        }

        /// <summary>
        /// 基金级别
        /// </summary>
        [DataMember]
        public Int32 FundLevel { get; set; }

        /// <summary>
        /// 基金级别
        /// </summary>
        [DataMember]
        public String FundName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public Decimal Account { get; set; }
    }

    /// <summary>
    /// 收稿量统计
    /// </summary>
    [DataContract]
    public class ContributionAccountQuery : QueryBase
    {
        /// <summary>
        /// 0:时间 1:基金 2:作者
        /// </summary>
        [DataMember]
        public Int32 Kind { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        [DataMember]
        public Int32 Year { get; set; }

        /// <summary>
        /// 是否导出
        /// </summary>
        [DataMember]
        public bool IsReport { get; set; }
    }

   


}
