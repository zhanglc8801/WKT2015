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
	///  表'ContributionInfoAtt'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class ContributionInfoAttEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// PKID : 
		/// </summary>
		/// <remarks>表ContributionInfoAtt主键</remarks>		
		[DataMember]
		public Int64 PKID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// CID : 稿件递增ID
		/// </summary>       
		[DataMember]
		public Int64 CID
        {
            get;
            set;
        }
		
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
		/// Abstract : 稿件摘要
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
		/// Reference : 参考文献内容
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
		/// Remark : 备注
		/// </summary>      
		[DataMember]
		public String Remark
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
		#endregion

		#endregion 属性、变量声明
		
	    public ContributionInfoAttEntity()
        {
            PKID = (long)0;
            CID = (long)0;
            JournalID = (long)0;
            Abstract = string.Empty;
            EnAbstract = string.Empty;
            Reference = string.Empty;
            Funds = string.Empty;
            Remark = string.Empty;
            AddDate = DateTime.Now;
        }
	}
}
