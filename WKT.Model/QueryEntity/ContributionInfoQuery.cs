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
    ///  表'ContributionInfo'的查询实体表示.
    /// </summary>
    /// <remarks>
    /// 该实体由工具生成，请根据实际需求修改本类
    /// </remarks>
    [DataContract]
    public partial class ContributionInfoQuery : QueryBase
    {
        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public Int64 CID { get; set; }

        /// <summary>
        /// 编号集合
        /// </summary>
        [DataMember]
        public Int64[] CIDs { get; set; }

        /// <summary>
        /// 是否是在修改稿(用于显示修改稿件页的联系电话为单个)
        /// </summary>
        [DataMember]
        public Boolean isModify { get; set; }

        /// <summary>
        /// 是否按版面费百分比获取数据
        /// </summary>
        [DataMember]
        public Boolean isPageFeeGet { get; set; }

        /// <summary>
        /// 获取实体时，是否加载辅助信息
        /// </summary>
        [DataMember]
        public bool IsAuxiliary { get; set; }

        /// <summary>
        /// 稿件状态 -1=草稿 0=新稿件 ，编辑还没有处理 10=审核中 100=已发校样 200=录用稿件 300=刊出稿件 -100=退稿 -2=格式修改 -3=退修稿件,-999=删除
        /// </summary>
        [DataMember]
        public Int32? Status { get; set; }

        /// <summary>
        /// 投稿类型 约稿、投稿等，数据字典
        /// </summary>
        [DataMember]
        public Int32? ContributionType { get; set; }

        /// <summary>
        /// 约稿人，数据字典
        /// </summary>
        [DataMember]
        public Int32? Special { get; set; }

        /// <summary>
        /// SubjectCat : 学科分类，数据字典
        /// </summary>       
        [DataMember]
        public Int32? SubjectCat { get; set; }

        /// <summary>
        /// JChannelID : 拟投栏目ID，期刊栏目设置中的栏目
        /// </summary>       
        [DataMember]
        public Int64? JChannelID { get; set; }

        /// <summary>
        /// Title : 稿件标题
        /// </summary>       
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// CNumber : 稿件编号
        /// </summary>       
        [DataMember]
        public String CNumber { get; set; }

        /// <summary>
        /// 投稿人
        /// </summary>
        [DataMember]
        public Int64? AuthorID { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [DataMember]
        public String Keyword { get; set; }

        /// <summary>
        /// 第一作者
        /// </summary>
        [DataMember]
        public String FirstAuthor { get; set; }

        /// <summary>
        /// 通信作者
        /// </summary>
        [DataMember]
        public String CommunicationAuthor { get; set; }

        /// <summary>
        /// 介绍信
        /// </summary>
        [DataMember]
        public String IntroLetterPath { get; set; }

        /// <summary>
        /// 稿件旗帜标记
        /// </summary>
        [DataMember]
        public string Flag
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件加急标记
        /// </summary>
        [DataMember]
        public bool IsQuick
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件审核状态
        /// </summary>
        [DataMember]
        public string AuditStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 开始投稿日期
        /// </summary>
        [DataMember]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束投稿日期
        /// </summary>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 期
        /// </summary>
        [DataMember]
        public int? Issue { get; set; }

        [DataMember]
        public bool IsReport { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        [DataMember]
        public int? Year { get; set; }


        /// <summary>
        /// 稿费百分比
        /// </summary>
        [DataMember]
        public int Percent
        {
            get;
            set;
        }

        /// <summary>
        /// 按页算稿费
        /// </summary>
        [DataMember]
        public decimal ArticlePayment
        {
            get;
            set;
        }

        public ContributionInfoQuery()
        {
            IsReport = false;
        }



    }
}
