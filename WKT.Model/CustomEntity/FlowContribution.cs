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
    /// 流程稿件信息
    /// </summary>
    public class FlowContribution
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        [DataMember]
        public long FlowLogID
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件ID
        /// </summary>
        [DataMember]
        public Int64 CID
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
        /// SubjectCat : 学科分类，数据字典
        /// </summary>       
        [DataMember]
        public Int32 SubjectCat
        {
            get;
            set;
        }

        /// <summary>
        /// SubjectCat : 学科分类，数据字典
        /// </summary>       
        [DataMember]
        public string SubjectCatName
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
        /// Status : 稿件状态 -1=草稿 0=新稿件 ，编辑还没有处理 10=审核中 100=已发校样 200=录用稿件 300=刊出稿件 -100=退稿 -2=格式修改 -3=退修稿件
        /// </summary>        
        [DataMember]
        public Int32 Status
        {
            get;
            set;
        }

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
        /// IsQuick : 是否是加急稿件 0=否 1=是
        /// </summary>       
        [DataMember]
        public Boolean? IsQuick
        {
            get;
            set;
        }

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
        /// 稿件附件
        /// </summary>
        [DataMember]
        public string ContributePath
        {
            get;
            set;
        }

        /// <summary>
        /// 介绍信附件
        /// </summary>
        [DataMember]
        public string IntroLetterPath
        {
            get;
            set;
        }

        /// <summary>
        /// 审稿状态
        /// </summary>
        [DataMember]
        public string AuditStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 作者姓名
        /// </summary>
        [DataMember]
        public string AuthorName
        {
            get;
            set;
        }

        /// <summary>
        /// 发送人
        /// </summary>
        [DataMember]
        public long SendUserID
        {
            get;
            set;
        }

        /// <summary>
        /// 发送人姓名
        /// </summary>
        [DataMember]
        public string SendUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 接收人
        /// </summary>
        [DataMember]
        public long RecUserID
        {
            get;
            set;
        }

        /// <summary>
        /// 处理人姓名
        /// </summary>
        [DataMember]
        public string RecUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否已阅
        /// </summary>
        [DataMember]
        public bool IsView
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
        /// 通讯作者
        /// </summary>
        [DataMember]
        public string CommunicationAuthor
        {
            get;
            set;
        }

        /// <summary>
        /// 是否有撤稿申请
        /// </summary>
        [DataMember]
        public bool IsRetractions
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

        /// <summary>
        /// 录用栏目
        /// </summary>
        [DataMember]
        public long JChannelID
        {
            get;
            set;
        }

        /// <summary>
        /// 栏目名称
        /// </summary>
        [DataMember]
        public string JChannelName
        {
            get;
            set;
        }

        /// <summary>
        /// 年
        /// </summary>
        [DataMember]
        public int Year
        {
            get;
            set;
        }
        /// <summary>
        /// 卷
        /// </summary>
        [DataMember]
        public int Volume
        {
            get;
            set;
        }

        /// <summary>
        /// 期
        /// </summary>
        [DataMember]
        public int Issue
        {
            get;
            set;
        }

        /// <summary>
        /// 投稿天数
        /// </summary>
        [DataMember]
        public int Days
        {
            get;
            set;
        }

        /// <summary>
        /// 已处理天数
        /// </summary>
        [DataMember]
        public int HandDays
        {
            get;
            set;
        }


        /// <summary>
        /// 联系电话（作者）
        /// </summary>
        [DataMember]
        public string Tel
        {
            get;
            set;
        }

        /// <summary>
        /// 手机（作者）
        /// </summary>
        [DataMember]
        public string Mobile
        {
            get;
            set;
        }


        /// <summary>
        /// 地址单位
        /// </summary>
        [DataMember]
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// 登录名
        /// </summary>
        [DataMember]
        public string LoginName
        {
            get;
            set;
        }

        /// <summary>
        ///备注
        /// </summary>
        [DataMember]
        public string  Remark
        {
            get;
            set;
        }

        /// <summary>
        ///  审毕日期
        /// </summary>
        [DataMember]
        public string  DealDate
        {
            get;
            set;
        }


        /// <summary>
        /// 流程状态值
        /// </summary>
        [DataMember]
        public int CStatus
        {
            get;
            set;
        }

        /// <summary>
        ///送审或已审时间
        /// </summary>
        [DataMember]
        public DateTime HandTime
        {
            get;
            set;
        }

        /// <summary>
        /// 崔审时间
        /// </summary>
        public string  SendDate
        {
            get;
            set;
        }


        /// <summary>
        /// 是否是部分审回
        /// </summary>
        [DataMember]
        public bool IsPart
        {
            get;
            set;
        }
    }
}
