using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using WKT.Model.Enum;

namespace WKT.Model
{
    /// <summary>
    /// 审稿流程
    /// </summary>
    [DataContract,Serializable]
    public class CirculationEntity : QueryBase
    {
        # region 投稿专用

        /// <summary>
        /// 稿件ID
        /// </summary>
        [DataMember]
        public long CID
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public string CNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 作者ID
        /// </summary>
        [DataMember]
        public long AuthorID
        {
            get;
            set;
        }

        /// <summary>
        /// 学科分类
        /// </summary>
        [DataMember]
        public int SubjectCategoryID
        {
            get;
            set;
        }

        # endregion

        /// <summary>
        /// 稿件标题
        /// </summary>
        [DataMember]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件关键词
        /// </summary>
        [DataMember]
        public string Keyword
        {
            get;
            set;
        }

        /// <summary>
        /// 旗帜标记
        /// </summary>
        [DataMember]
        public string Flag
        {
            get;
            set;
        }

        /// <summary>
        /// 第一作者
        /// </summary>
        [DataMember]
        public string FirstAuthor
        {
            get;
            set;
        }

        /// <summary>
        /// 第一作者单位
        /// </summary>
        [DataMember]
        public string FirstAuthorWorkUnit
        {
            get;
            set;
        }

        /// <summary>
        /// 开始日期
        /// </summary>
        [DataMember]
        public DateTime? StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// 结束日期
        /// </summary>
        [DataMember]
        public DateTime? EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 录用年
        /// </summary>
        [DataMember]
        public int Year
        {
            get;
            set;
        }

        /// <summary>
        /// 录用期
        /// </summary>
        [DataMember]
        public int Issue
        {
            get;
            set;
        }

        /// <summary>
        /// 查询日志用
        /// </summary>
        [DataMember]
        public byte GroupID
        {
            get;
            set;
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        [DataMember]
        public String SortName { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        [DataMember]
        public String SortOrder { get; set; }


        # region 流转

        [DataMember]
        public long? CurAuthorID
        {
            get;
            set;
        }

        [DataMember]
        public long FlowLogID
        {
            get;
            set;
        }

        /// <summary>
        /// 审稿状态ID
        /// </summary>
        [DataMember]
        public long? StatusID
        {
            get;
            set;
        }

        /// <summary>
        /// 要转入的下一个状态的ID
        /// </summary>
        [DataMember]
        public long? ToStatusID
        {
            get;
            set;
        }

        /// <summary>
        /// 审稿操作
        /// </summary>
        [DataMember]
        public long ActionID
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件ID，多个ID之间用逗号分隔
        /// </summary>
        [DataMember]
        public string CIDS
        {
            get;
            set;
        }

        /// <summary>
        /// 处理状态 0=待处理 1=已处理
        /// </summary>
        [DataMember]
        public Byte Status
        {
            get;
            set;
        }

        /// <summary>
        /// 处理状态 0=待处理 1=已处理
        /// </summary>
        [DataMember]
        public EnumContributionStatus EnumCStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件处理日志ID
        /// </summary>
        [DataMember]
        public IDictionary<long, long> DictLogID
        {
            get;
            set;
        }

        /// <summary>
        /// 处理意见
        /// </summary>
        [DataMember]
        public String DealAdvice { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        [DataMember]
        public String CPath { get; set; }

        [DataMember]
        public string CFileName { get; set; }

        /// <summary>
        /// 图表地址
        /// </summary>
        [DataMember]
        public String FigurePath { get; set; }

        [DataMember]
        public string FFileName { get; set; }

        /// <summary>
        /// 其他地址
        /// </summary>
        [DataMember]
        public String OtherPath { get; set; }

        /// <summary>
        /// 是否有审稿单 0=否 1=是
        /// </summary>
        [DataMember]
        public byte IsHaveBill { get; set; }
        # endregion

        /// <summary>
        /// 是否是搜索
        /// </summary>
        [DataMember]
        public bool IsSearch
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是统计
        /// </summary>
        [DataMember]
        public bool IsStat
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是已处理 0=待处理 1=已处理 2=All 3=非可操作状态下的全部已处理
        /// </summary>
        [DataMember]
        public byte? IsHandled
        {
            get;
            set;
        }
        /// <summary>
        /// 是否允许作者查看更多审稿信息
        /// </summary>
        [DataMember]
        public byte? isViewMoreFlow
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许专家查看历史审稿信息
        /// </summary>
        [DataMember]
        public byte? isViewHistoryFlow
        {
            get;
            set;
        }

        /// <summary>
        /// 是否根据处理时间对稿件排序
        /// </summary>
        [DataMember]
        public byte? isPersonal_Order
        {
            get;
            set;
        }

        /// <summary>
        /// 是否在稿件搜索页仅显示当前登录者的稿件
        /// </summary>
        [DataMember]
        public byte? isPersonal_OnlyMySearch
        {
            get;
            set;
        }


        /// <summary>
        /// 稿件状态
        /// </summary>
        [DataMember]
        public int? CStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 专家已审
        /// </summary>
        [DataMember]
        public bool IsExpertAudited
        {
            get;
            set;
        }

        /// <summary>
        /// 模板ID
        /// </summary>
        [DataMember]
        public Int64 TemplateID
        {
            get;
            set;
        }

        /// <summary>
        /// 接收者
        /// </summary>
        [DataMember]
        public Int64 RecUserID
        {
            get;
            set;
        }

        /// <summary>
        /// 发送者
        /// </summary>
        [DataMember]
        public Int64 SendUserID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是专家审回到固定编辑
        /// </summary>
        [DataMember]
        public bool IsExpertToEditor
        {
            get;
            set;
        }


        public CirculationEntity()
        {
            DealAdvice = "";
            IsSearch = false;
            IsHandled = 0;
        }
    }
}
