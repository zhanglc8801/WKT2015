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
	///  表'IssueContent'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract,Serializable]
	public partial class IssueContentEntity : ObjectBase
	{
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// ContentID : 
		/// </summary>
		/// <remarks>表IssueContent主键</remarks>		
		[DataMember]
		public Int64 ContentID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// JournalID : 
		/// </summary>       
		[DataMember]
		public Int64 JournalID
        {
            get;
            set;
        }

        /// <summary>
        /// CID : 
        /// </summary>       
        [DataMember]
        public Int64 CID
        {
            get;
            set;
        }

        /// <summary>
        /// AuthorID : 
        /// </summary>       
        [DataMember]
        public Int64 AuthorID
        {
            get;
            set;
        }
		
		/// <summary>
		/// Year : 年
		/// </summary>       
		[DataMember]
		public Int32 Year
        {
            get;
            set;
        }
		
		/// <summary>
		/// Volume : 卷
		/// </summary>      
		[DataMember]
		public Int32 Volume
        {
            get;
            set;
        }
		
		/// <summary>
		/// Issue : 期
		/// </summary>       
		[DataMember]
		public Int32 Issue
        {
            get;
            set;
        }
		
		/// <summary>
		/// JChannelID : 所属期刊栏目ID
		/// </summary>       
		[DataMember]
		public Int64 JChannelID
        {
            get;
            set;
        }

        /// <summary>
        /// ChannelName:所属期刊栏目名称
        /// </summary>
        [DataMember]
        public string ChannelName
        {
            get;
            set;
        }

		/// <summary>
		/// Title : 标题
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
		/// Authors : 作者
		/// </summary>      
		[DataMember]
		public String Authors
        {
            get;
            set;
        }
		
		/// <summary>
		/// EnAuthors : 英文作者
		/// </summary>      
		[DataMember]
		public String EnAuthors
        {
            get;
            set;
        }
		
		/// <summary>
		/// WorkUnit : 工作单位
		/// </summary>       
		[DataMember]
		public String WorkUnit
        {
            get;
            set;
        }
		
		/// <summary>
		/// EnWorkUnit : 英文单位
		/// </summary>       
		[DataMember]
		public String EnWorkUnit
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
		/// DOI : 文章唯一标识符
		/// </summary>       
		[DataMember]
		public String DOI
        {
            get;
            set;
        }

        /// <summary>
        /// 是否已注册DOI
        /// </summary>
        [DataMember]
        public Boolean isRegDoi
        {
            get;
            set;
        }
		
		/// <summary>
		/// Abstract : 中文摘要
		/// </summary>       
		[DataMember]
		public String Abstract
        {
            get;
            set;
        }
		
		/// <summary>
		/// EnAbstract : 英文摘要
		/// </summary>       
		[DataMember]
		public String EnAbstract
        {
            get;
            set;
        }
		
		/// <summary>
		/// Reference : 参考文献
		/// </summary>       
		[DataMember]
		public String Reference
        {
            get;
            set;
        }
		
		/// <summary>
		/// Funds : 基金项目
		/// </summary>        
		[DataMember]
		public String Funds
        {
            get;
            set;
        }
		
		/// <summary>
		/// AuthorIntro : 作者简介
		/// </summary>       
		[DataMember]
		public String AuthorIntro
        {
            get;
            set;
        }
		
		/// <summary>
        /// StartPageNumber : 开始页码
		/// </summary>        
		[DataMember]
        public Int32 StartPageNum
        {
            get;
            set;
        }

        /// <summary>
        /// EndPageNumber : 结束页码
        /// </summary>        
        [DataMember]
        public Int32 EndPageNum
        {
            get;
            set;
        }
		
		/// <summary>
		/// FilePath : 附件保存路径
		/// </summary>       
		[DataMember]
		public String FilePath
        {
            get;
            set;
        }
		
		/// <summary>
		/// FileKey : 文件Key
		/// </summary>       
		[DataMember]
		public String FileKey
        {
            get;
            set;
        }
		
		/// <summary>
		/// FileSize : 文件大小
		/// </summary>        
		[DataMember]
		public Decimal FileSize
        {
            get;
            set;
        }
		
		/// <summary>
		/// FileExt : 文件扩展名
		/// </summary>       
		[DataMember]
		public String FileExt
        {
            get;
            set;
        }

        /// <summary>
        /// Html文件保存路径
        /// </summary>
        [DataMember]
        public String HtmlPath
        {
            get;
            set;
        }

        /// <summary>
        /// Html文件大小
        /// </summary>
        [DataMember]
        public Decimal HtmlSize
        {
            get;
            set;
        }

        /// <summary>
        /// Html文件访问次数
        /// </summary>
        [DataMember]
        public Int32 HtmlHits
        {
            get;
            set;
        }
		
		/// <summary>
		/// Hits : 点击次数
		/// </summary>       
		[DataMember]
		public Int32 Hits
        {
            get;
            set;
        }
		
		/// <summary>
		/// Downloads : 下载次数
		/// </summary>       
		[DataMember]
		public Int32 Downloads
        {
            get;
            set;
        }
		
		/// <summary>
		/// ViewMoney : 查看该篇文章需要花费金额
		/// </summary>       
		[DataMember]
		public Decimal ViewMoney
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsHot : 是否是热门内容 0=否 1=是
		/// </summary>       
		[DataMember]
		public Boolean IsHot
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsCommend : 是否是推荐内容 0=否 1=是
		/// </summary>       
		[DataMember]
		public Boolean IsCommend
        {
            get;
            set;
        }
		
		/// <summary>
		/// IsTop : 是否置顶 0=否 1=是
		/// </summary>       
		[DataMember]
		public Boolean IsTop
        {
            get;
            set;
        }
		
		/// <summary>
		/// SortID : 排序值
		/// </summary>        
		[DataMember]
		public Int32 SortID
        {
            get;
            set;
        }
		
		/// <summary>
		/// InUser : 录入人
		/// </summary>       
		[DataMember]
		public Int64 InUser
        {
            get;
            set;
        }
		
		/// <summary>
		/// EditUser : 修改人
		/// </summary>       
		[DataMember]
		public Int64 EditUser
        {
            get;
            set;
        }
		
		/// <summary>
		/// EditDate : 修改时间
		/// </summary>       
		[DataMember]
		public DateTime EditDate
        {
            get;
            set;
        }
		
		/// <summary>
		/// AddDate : 添加时间
		/// </summary>       
		[DataMember]
		public DateTime AddDate
        {
            get;
            set;
        }

        /// <summary>
        /// CNumber : 稿件编号
        /// </summary>       
        [DataMember]
        public String CNumber
        {
            get;
            set;
        }

		#endregion

        /// <summary>
        /// 参考文献
        /// </summary>
        [DataMember]
        public IList<IssueReferenceEntity> ReferenceList { get; set; }
		
	    public IssueContentEntity()
        {
            ContentID = 0;
            JournalID = 0;
            CID = 0;
            AuthorID = 0;
            Year = 0;
            Volume = 0;
            Issue = 0;
            JChannelID = 0;
            ChannelName = string.Empty;
            Title = string.Empty;
            EnTitle = string.Empty;
            Authors = string.Empty;
            EnAuthors = string.Empty;
            WorkUnit = string.Empty;
            EnWorkUnit = string.Empty;
            Keywords = string.Empty;
            EnKeywords = string.Empty;
            CLC = string.Empty;
            DOI = string.Empty;
            Abstract = string.Empty;
            EnAbstract = string.Empty;
            Reference = string.Empty;
            Funds = string.Empty;
            AuthorIntro = string.Empty;
            StartPageNum = 0;
            EndPageNum = 0;
            FilePath = string.Empty;
            FileKey = string.Empty;
            FileSize = 0m;
            FileExt = string.Empty;
            HtmlPath = string.Empty;
            HtmlSize = 0;
            Hits = 0;
            Downloads = 0;
            ViewMoney = 0m;
            IsHot = false;
            IsCommend = false;
            IsTop = false;
            SortID = 0;
            InUser = 0;
            EditUser = 0;
            EditDate = DateTime.Now;
            AddDate = DateTime.Now;
            ReferenceList = null;
        }
	}
}
