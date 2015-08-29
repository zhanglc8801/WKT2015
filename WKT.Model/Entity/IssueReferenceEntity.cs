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
	///  表'IssueReference'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class IssueReferenceEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// ReferenceID : 
		/// </summary>
		/// <remarks>表IssueReference主键</remarks>		
		[DataMember]
		public Int64 ReferenceID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// ContentID : 
		/// </summary>       
		[DataMember]
		public Int64 ContentID
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
		/// Title : 文献格式，格式如下: 作者,作者,作者.文章标题.杂志名称.2009;11(4):102-107.
		/// </summary>      
		[DataMember]
		public String Title
        {
            get;
            set;
        }
		
		/// <summary>
		/// RefUrl : 文献URL
		/// </summary>       
		[DataMember]
		public String RefUrl
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
		
	    public IssueReferenceEntity()
        {
            ReferenceID = (long)0;
            ContentID = (long)0;
            JournalID = (long)0;
            Title = null;
            RefUrl = null;
            AddDate = DateTime.Now;
        }
	}
}
