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
	///  表'ContributionFund'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class ContributionFundEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// FundID : 
		/// </summary>
		/// <remarks>表ContributionFund主键</remarks>		
		[DataMember]
		public Int64 FundID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// CID : 稿件编号
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
		/// FundLevel : 基金级别 ，数据字典
		/// </summary>       
		[DataMember]
		public Int32 FundLevel
        {
            get;
            set;
        }
		
		/// <summary>
		/// FundCode : 基金编码
		/// </summary>       
		[DataMember]
		public String FundCode
        {
            get;
            set;
        }
		
		/// <summary>
		/// FundName : 基金名称
		/// </summary>      
		[DataMember]
		public String FundName
        {
            get;
            set;
        }
		
		/// <summary>
		/// FundCertPath : 基金证明文件保存路径
		/// </summary>       
		[DataMember]
		public String FundCertPath
        {
            get;
            set;
        }
		
		/// <summary>
		/// Note : 备注
		/// </summary>      
		[DataMember]
		public String Note
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
		
	    public ContributionFundEntity()
        {
            FundID = (long)0;
            CID = (long)0;
            JournalID = (long)0;
            FundLevel = (int)0;
            FundCode = string.Empty;
            FundName = string.Empty;
            FundCertPath = null;
            Note = null;
            AddDate = DateTime.Now;
        }
	}
}
