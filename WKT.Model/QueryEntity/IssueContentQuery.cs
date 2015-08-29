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
	///  表'IssueContent'的查询实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，请根据实际需求修改本类
	/// </remarks>
	[DataContract]
	public partial class IssueContentQuery :QueryBase
	{
        /// <summary>
        /// 年
        /// </summary>
        [DataMember]
        public Int32? Year { get; set; }

        /// <summary>
        /// 卷
        /// </summary>
        [DataMember]
        public Int32? Volume { get; set; }

        /// <summary>
        /// 期
        /// </summary>
        [DataMember]
        public Int32? Issue { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DataMember]
        public String Authors { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [DataMember]
        public String WorkUnit { get; set; }

        /// <summary>
        /// DOI
        /// </summary>
        [DataMember]
        public String DOI { get; set; }

        /// <summary>
        /// 是否已注册DOI
        /// </summary>
        [DataMember]
        public Boolean isRegDoi { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [DataMember]
        public string Keywords { get; set; }

        [DataMember]
        public long? JChannelID
        {
            get;
            set;
        }

        /// <summary>
        /// 获取实体时，是否加载辅助信息
        /// </summary>
        [DataMember]
        public bool IsAuxiliary { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64 contentID { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public Int64[] contentIDs { get; set; }

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

        /// <summary>
        /// 稿件ID
        /// </summary>
        [DataMember]
        public Int64 CID { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DataMember]
        public long? AuthorID { get; set; }

        /// <summary>
        /// 稿件编号
        /// </summary>
        [DataMember]
        public String CNumber { get; set; }

        /// <summary>
        /// 排序字符串
        /// </summary>
        [DataMember]
        public new String OrderStr
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SortName))
                    return " SortID DESC";
                return SortName + SortOrder;
            }
        }
		
        /// <summary>
        /// 热点
        /// </summary>
        [DataMember]
        public bool IsHot
        {
            get;
            set;
        }


	    public IssueContentQuery()
        {
            
        }
	}
}
