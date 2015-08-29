using System;
using System.Collections.Generic;

namespace Web.Mobile
{
    public class SitePersonalSettings
    {

        /// <summary>
        /// 标识
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public string SiteID { get; set; }

        /// <summary>
        /// 编辑ID
        /// </summary>
        public string EditorID { get; set; }

        /// <summary>
        /// 在【稿件处理专区】根据处理时间(默认根据稿件编号)对稿件排序
        /// </summary>
        public byte Personal_Order { get; set; }

        /// <summary>
        /// 在【稿件搜索】页面仅显示我的稿件
        /// </summary>
        public byte Personal_OnlyMySearch { get; set; }

        

    }
}