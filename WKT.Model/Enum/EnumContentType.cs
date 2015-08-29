using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WKT.Model.Enum
{
    /// <summary>
    /// 内容类型枚举
    /// </summary>
    public enum EnumContentType
    {
        /// <summary>
        /// 资讯
        /// </summary>
        [Description("资讯")]
        Information=1,

        /// <summary>
        /// 版块
        /// </summary>
        [Description("版块")]
        Section = 2,

        /// <summary>
        /// 资源
        /// </summary>
        [Description("资源")]
        Resources = 3,

        /// <summary>
        /// 描述类
        /// </summary>
        [Description("描述类")]
        SiteDescribe = 4,

        /// <summary>
        /// 联系我们
        /// </summary>
        [Description("联系我们")]
        ContactWay = 5,       

        /// <summary>
        /// 友情链接 
        /// </summary>
        [Description("友情链接 ")]
        FriendlyLink = 6,

        /// <summary>
        /// 系统功能 
        /// </summary>
        [Description("系统功能 ")]
        SystemFun = 7
    }
}
