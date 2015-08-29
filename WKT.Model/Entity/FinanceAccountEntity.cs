using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    [DataContract]
    public class FinanceAccountEntity : ObjectBase
    {
        /// <summary>
        /// 稿件ID
        /// </summary>
        [DataMember]
        public Int64 CID { get; set; }

        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public String CNumber { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DataMember]
        public Int64 UserID { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        [DataMember]
        public String WorkUnit
        {
            get;
            set;
        }

        /// <summary>
        /// 审稿费通知ID
        /// </summary>
        [DataMember]
        public Int64 ReadingFeeNotice { get; set; }

        /// <summary>
        /// 审稿费通知状态
        /// </summary>
        [DataMember]
        public String ReadingFeeNoticeStatus { get; set; }

        /// <summary>
        /// 版面费通知ID
        /// </summary>
        [DataMember]
        public Int64 LayoutFeeNotice { get; set; }

        /// <summary>
        /// 版面费通知状态
        /// </summary>
        [DataMember]
        public String LayoutFeeNoticeStatus { get; set; }

        /// <summary>
        /// 审稿费ID
        /// </summary>
        [DataMember]
        public Int64 ReadingFeeID { get; set; }

        /// <summary>
        /// 审稿费交付状态 0:待确认 1:已确认
        /// </summary>
        [DataMember]
        public Byte? ReadingFeeStatus { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public String Address { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [DataMember]
        public String InvoiceUnit { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [DataMember]
        public String Tel { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember]
        public String Mobile { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [DataMember]
        public string ZipCode { get; set; }

        /// <summary>
        /// 审稿费
        /// </summary>
        [DataMember]
        public Decimal ReadingFee { get; set; }

        /// <summary>
        /// 审稿费
        /// </summary>
        [DataMember]
        public String ReadingFeeStr
        {
            get
            {
                if (ReadingFeeStatus == null)
                    return string.Empty;
                switch (ReadingFeeStatus.Value)
                {
                    case 0: return "待确认(" + ReadingFee.ToString("C2") + ")";
                    case 1: return ReadingFee.ToString("C2");
                    default: return string.Empty;
                }
            }
        }

        /// <summary>
        /// 审稿费
        /// </summary>
        [DataMember]
        public String ReadingFeeReportStr
        {
            get
            {
                if (ReadingFeeStatus == null)
                    return string.Empty;
                switch (ReadingFeeStatus.Value)
                {
                    case 0: return "待确认:" + ReadingFee.ToString("C2");
                    case 1: return "已交:" + ReadingFee.ToString("C2");
                    default: return string.Empty;
                }
            }
        }

        /// <summary>
        /// 审稿费备注
        /// </summary>
        [DataMember]
        public String Note { get; set; }

        /// <summary>
        /// 版面费ID
        /// </summary>
        [DataMember]
        public Int64 LayoutFeeID { get; set; }

        /// <summary>
        /// 版面费交付状态 0:待确认 1:已确认
        /// </summary>
        [DataMember]
        public Byte? LayoutFeeStatus { get; set; }

        /// <summary>
        /// 版面费
        /// </summary>
        [DataMember]
        public Decimal LayoutFee { get; set; }

        /// <summary>
        /// 版面费
        /// </summary>
        [DataMember]
        public String LayoutFeeStr
        {
            get
            {
                if (LayoutFeeStatus == null)
                    return string.Empty;
                switch (LayoutFeeStatus.Value)
                {
                    case 0: return "待确认(" + LayoutFee.ToString("C2") + ")";
                    case 1: return LayoutFee.ToString("C2");
                    default: return string.Empty;
                }
            }
        }

        /// <summary>
        /// 版面费
        /// </summary>
        [DataMember]
        public String LayoutFeeReportStr
        {
            get
            {
                if (LayoutFeeStatus == null)
                    return string.Empty;
                switch (LayoutFeeStatus.Value)
                {
                    case 0: return "待确认:" + LayoutFee.ToString("C2");
                    case 1: return "已交:" + LayoutFee.ToString("C2");
                    default: return string.Empty;
                }
            }
        }

        /// <summary>
        /// 版面费备注
        /// </summary>
        [DataMember]
        public String PageNote { get; set; }

        /// <summary>
        /// ArticleType : 稿费计费方式：0=按篇数 1=按页数
        /// </summary>        
        [DataMember]
        public String ArticleType
        {
            get;
            set;
        }

        /// <summary>
        /// ArticleCount : 篇数/页数
        /// </summary>        
        [DataMember]
        public Decimal ArticleCount
        {
            get;
            set;
        }


        /// <summary>
        /// 稿费寄出状态 0:待确认 1:已确认
        /// </summary>
        [DataMember]
        public Byte? ArticlePaymentFeeStatus { get; set; }

        /// <summary>
        /// 稿费ID
        /// </summary>
        [DataMember]
        public Int64 ArticlePaymentFeeID { get; set; }
        /// <summary>
        /// 稿费
        /// </summary>
        [DataMember]
        public Decimal ArticlePaymentFee { get; set; }

        
        /// <summary>
        /// 稿费
        /// </summary>
        [DataMember]
        public String ArticlePaymentFeeStr
        {
            get
            {
                if (ArticlePaymentFeeStatus == null)
                    return string.Empty;
                switch (ArticlePaymentFeeStatus.Value)
                {
                    case 0: return "待确认(" + ArticlePaymentFee.ToString("C2") + ")";
                    case 1: return ArticlePaymentFee.ToString("C2");
                    default: return string.Empty;
                }
            }
        }

        /// <summary>
        /// 稿费
        /// </summary>
        [DataMember]
        public String ArticlePaymentFeeReportStr
        {
            get
            {
                if (ArticlePaymentFeeStatus == null)
                    return string.Empty;
                switch (ArticlePaymentFeeStatus.Value)
                {
                    case 0: return "待确认:" + ArticlePaymentFee.ToString("C2");
                    case 1: return "已寄出:" + ArticlePaymentFee.ToString("C2");
                    default: return string.Empty;
                }
            }
        }

        /// <summary>
        /// 稿费备注
        /// </summary>
        [DataMember]
        public String ArticlePaymentNote { get; set; }

        /// <summary>
        /// 稿件标题
        /// </summary>
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// 责任编辑ID
        /// </summary>
        [DataMember]
        public Int64 EditAuthorID { get; set; }

        /// <summary>
        /// 责任编辑
        /// </summary>
        [DataMember]
        public String EditAuthorName { get; set; }

        /// <summary>
        /// 作者ID
        /// </summary>
        [DataMember]
        public Int64 AuthorID { get; set; }

        /// <summary>
        /// 第一作者ID
        /// </summary>
        [DataMember]
        public Int64 FirstAuthorID { get; set; }

        /// <summary>
        /// 第一作者
        /// </summary>
        [DataMember]
        public String FirstAuthor { get; set; }

        /// <summary>
        /// 通信作者ID
        /// </summary>
        [DataMember]
        public Int64 CommunicationAuthorID { get; set; }

        /// <summary>
        /// 通信作者
        /// </summary>
        [DataMember]
        public String CommunicationAuthor { get; set; }

        /// <summary>
        /// 投稿时间
        /// </summary>
        [DataMember]
        public DateTime AddDate { get; set; }

        /// <summary>
        /// 旗舰标记
        /// </summary>
        [DataMember]
        public String Flag { get; set; }


        /// <summary>
        /// 确定年
        /// </summary>
        [DataMember]
        public Int64 Year
        {
            get;
            set;
        }

        /// <summary>
        /// 确定期
        /// </summary>
        [DataMember]
        public Int64 Issue
        {
            get;
            set;
        }
    }
}
