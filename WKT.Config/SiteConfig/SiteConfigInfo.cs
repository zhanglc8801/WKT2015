using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Config
{
    /// <summary>
    /// 站点配置
    /// </summary>
    [Serializable]
    public class SiteConfigInfo : IConfigInfo
    {
        private string _uploadfileExt = "*.jpg;*.jpeg;*.gif;*.png;*.pdf;*.doc;*.docx;*.xls;*.xlsx";
        private string _uploadhtmExt = "*.htm;*.html;*.mht";
        /// <summary>
        /// 上传文件类型
        /// </summary>
        public string UploadFileExt
        {
            get { return _uploadfileExt; }
            set { _uploadfileExt = value; }
        }

        public string UploadHtmExt
        {
            get { return _uploadhtmExt; }
            set { _uploadhtmExt = value; }
        }

        /// <summary>
        /// 上传图片类型
        /// </summary>
        public string UploadImgExt
        {
            get;
            set;
        }

        /// <summary>
        /// 上传虚拟路径
        /// </summary>
        public string UploadPath
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件
        /// </summary>
        public string ContributePath { get; set; }

        /// <summary>
        /// 附图
        /// </summary>
        public string FigurePath { get; set; }

        /// <summary>
        /// 稿件pdf
        /// </summary>
        public string PDFPath { get; set; }

        /// <summary>
        /// 介绍信
        /// </summary>
        public string IntroLetterPath { get; set; }

        /// <summary>
        /// qq
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// Api站点
        /// </summary>
        public string APIHost
        {
            get;
            set;
        }

        /// <summary>
        /// Api授权Key
        /// </summary>
        public string APIPublicKey
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string SiteName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示财务相关信息 0:不显示 1:显示
        /// </summary>
        public int IsFinance { get; set; }

        /// <summary>
        /// 计费设置-审稿费
        /// </summary>
        public Decimal ReviewFeeText { get; set; }

        /// <summary>
        /// 计费设置-版面费
        /// </summary>
        public Decimal PageFeeText { get; set; }

        /// <summary>
        /// 计费设置-稿费-按篇
        /// </summary>
        public Decimal GaoFeeText1 { get; set; }

        /// <summary>
        /// 计费设置-稿费-按页
        /// </summary>
        public Decimal GaoFeeText2 { get; set; }

        /// <summary>
        /// 计费设置-稿费-按版面费百分比
        /// </summary>
        public Decimal GaoFeeText3 { get; set; }

        /// <summary>
        /// 专家审稿费
        /// </summary>
        public Decimal ExpertReviewFee { get; set; }

    }
}
