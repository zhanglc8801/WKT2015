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
	///  表'RetractionsBills'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class RetractionsBillsEntity : ObjectBase
	{
		#region 属性、变量声明
		#region 主属性 --对应数据表主键
		/// <summary>			
		/// PKID : 
		/// </summary>
		/// <remarks>表RetractionsBills主键</remarks>		
		[DataMember]
		public Int64 PKID
        {
            get;
            set;
        }
		#endregion
		
		#region 属性
		
		/// <summary>
		/// JournalID : 编辑部ID
		/// </summary>       
		[DataMember]
		public Int64 JournalID
        {
            get;
            set;
        }
		
		/// <summary>
		/// CID : 稿件ID
		/// </summary>       
		[DataMember]
		public Int64 CID
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
		/// Reason : 撤稿原因
		/// </summary>        
		[DataMember]
		public String Reason
        {
            get;
            set;
        }
		
		/// <summary>
		/// ApplyDate : 申请日期
		/// </summary>       
		[DataMember]
		public DateTime ApplyDate
        {
            get;
            set;
        }
		
		/// <summary>
		/// Handler : 处理人，即当前登录人
		/// </summary>       
		[DataMember]
		public Int64 Handler
        {
            get;
            set;
        }

        /// <summary>
        /// Handler : 处理人，即当前登录人
        /// </summary>       
        [DataMember]
        public string HandlerName
        {
            get;
            set;
        }
		
		/// <summary>
		/// HandAdvice : 处理意见
		/// </summary>       
		[DataMember]
		public String HandAdvice
        {
            get;
            set;
        }
		
		/// <summary>
		/// HandDate : 处理时间
		/// </summary>        
		[DataMember]
		public DateTime? HandDate
        {
            get;
            set;
        }
		
		/// <summary>
		/// Status : 状态 0=未处理 1=已处理
		/// </summary>       
		[DataMember]
		public Byte Status
        {
            get;
            set;
        }
		#endregion

		#endregion 属性、变量声明
		
	    public RetractionsBillsEntity()
        {
            PKID = (long)0;
            JournalID = (long)0;
            CID = (long)0;
            AuthorID = (long)0;
            Reason = string.Empty;
            ApplyDate = DateTime.Now;
            Handler = (long)0;
            HandAdvice = string.Empty;
            HandDate = null;
            Status = (byte)0;
        }
	}
}
