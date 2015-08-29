
namespace WKT.Cache
{
    /// <summary>
    /// 缓存Key
    /// </summary>
    public class CacheKey
    {
        public static string WKTVERSION = "WKT_V1.0";

        //DATA域   CTIME域  DEPEND域   DEPCTIME域
        public const string DATA = "DATA_";
        public const string CTIME = "CTIME_";
        public const string DEPEND = "DEPEND_";
        public const string DEPCTIME = "DEPCTIME_";

        public const string BLL_IOC_UNITYCONFIGKEY = "BLL_IOC_UNITYCONFIGKEY";
        public const string SERVICE_IOC_UNITYCONFIGKEY = "SERVICE_IOC_UNITYCONFIGKEY";

        /// <summary>
        /// 杂志社导航,杂志社ID
        /// </summary>
        public const string SITE_GETJOURNAL_CHANNELLIST = "SITE_GETJOURNAL_CHANNELLIST_{0}";

        /// <summary>
        /// 杂志社内容块,杂志社ID、几条记录
        /// </summary>
        public const string SITE_GETJOURNAL_BLOCKCONTENTLIST = "SITE_GETJOURNAL_BLOCKCONTENTLIST_{0}_{1}";

        /// <summary>
        /// 杂志社公告,杂志社ID、栏目ID、几条记录
        /// </summary>
        public const string SITE_GETJOURNAL_NOTICELIST = "SITE_GETJOURNAL_NOTICELIST_{0}_{1}_{2}";

        /// <summary>
        /// 杂志社年卷,杂志社ID
        /// </summary>
        public const string SITE_GETJOURNAL_YEARVOLUMNLIST = "SITE_GETJOURNAL_YEARVOLUMNLIST_{0}";

        /// <summary>
        /// 杂志社期列表,杂志社ID
        /// </summary>
        public const string SITE_GETJOURNAL_ISSLUELIST = "SITE_GETJOURNAL_ISSLUELIST_{0}";

        /// <summary>
        /// 杂志社期刊栏目,杂志社ID
        /// </summary>
        public const string SITE_GETJOURNAL_JOURNALICHANNELLIST = "SITE_GETJOURNAL_JOURNALICHANNELLIST_{0}";

        /// <summary>
        /// 杂志社期刊内容列表,杂志社ID、第几页、每页大小
        /// </summary>
        public const string SITE_GETJOURNAL_ISSUECONTENTLIST = "SITE_GETJOURNAL_ISSUECONTENTLIST_{0}_{1}_{2}";

        /// <summary>
        /// 杂志社期刊内容列表,杂志社ID、第几页、每页大小 期 年
        /// </summary>
        public const string SITE_GETJOURNAL_ISSUECONTENTISSUESLIST = "SITE_GETJOURNAL_ISSUECONTENTLIST_{0}_{1}_{2}_{3}_{4}";
    }
}
