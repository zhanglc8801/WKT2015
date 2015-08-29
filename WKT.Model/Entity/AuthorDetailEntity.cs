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
	///  表'AuthorDetail'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
	[DataContract]
	public partial class AuthorDetailEntity : ObjectBase
	{
        #region 属性、变量声明
        #region 主属性 --对应数据表主键
        /// <summary>			
        /// PKID : 
        /// </summary>
        /// <remarks>表AuthorDetail主键</remarks>      
        [DataMember]
        public Int64 PKID
        {
            get;
            set;
        }
        #endregion

        #region 属性

        /// <summary>
        /// AuthorID : 作者信息
        /// </summary>       
        [DataMember]
        public Int64 AuthorID
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
        /// AuthorName : 作者姓名
        /// </summary>       
        [DataMember]
        public String AuthorName
        {
            get;
            set;
        }

        /// <summary>
        /// EnglishName : 作者英文名称
        /// </summary>       
        [DataMember]
        public String EnglishName
        {
            get;
            set;
        }

        /// <summary>
        /// Gender : 性别，1=男 2=女
        /// </summary>        
        [DataMember]
        public String Gender
        {
            get;
            set;
        }

        /// <summary>
        /// Nation : 民族
        /// </summary>       
        [DataMember]
        public String Nation
        {
            get;
            set;
        }

        /// <summary>
        /// Birthday : 
        /// </summary>       
        [DataMember]
        public DateTime? Birthday
        {
            get;
            set;
        }

        /// <summary>
        /// NativePlace : 籍贯
        /// </summary>       
        [DataMember]
        public String NativePlace
        {
            get;
            set;
        }

        /// <summary>
        /// 所在省份
        /// </summary>
        [DataMember]
        public String Province { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
        [DataMember]
        public String City { get; set; }

        /// <summary>
        /// 所在区县
        /// </summary>
        [DataMember]
        public String Area { get; set; }

        /// <summary>
        /// IDCard : 身份证号
        /// </summary>       
        [DataMember]
        public String IDCard
        {
            get;
            set;
        }

        /// <summary>
        /// Address : 联系地址
        /// </summary>       
        [DataMember]
        public String Address
        {
            get;
            set;
        }

        /// <summary>
        /// ZipCode : 邮编
        /// </summary>       
        [DataMember]
        public String ZipCode
        {
            get;
            set;
        }

        /// <summary>
        /// Mobile : 手机号
        /// </summary>      
        [DataMember]
        public String Mobile
        {
            get;
            set;
        }

        /// <summary>
        /// Tel : 办公电话
        /// </summary>      
        [DataMember]
        public String Tel
        {
            get;
            set;
        }

        /// <summary>
        /// Fax : 传真
        /// </summary>       
        [DataMember]
        public String Fax
        {
            get;
            set;
        }

        /// <summary>
        /// Education : 学历
        /// </summary>      
        [DataMember]
        public Int32 Education
        {
            get;
            set;
        }

        /// <summary>
        /// Professional : 专业
        /// </summary>      
        [DataMember]
        public String Professional
        {
            get;
            set;
        }

        /// <summary>
        /// JobTitle : 职称
        /// </summary>      
        [DataMember]
        public Int32 JobTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Job : 职务
        /// </summary>     
        [DataMember]
        public String Job
        {
            get;
            set;
        }

        /// <summary>
        /// ResearchTopics : 研究方向
        /// </summary>      
        [DataMember]
        public String ResearchTopics
        {
            get;
            set;
        }

        /// <summary>
        /// WorkUnit : 工作单位
        /// </summary>     
        [DataMember]
        public String WorkUnit
        {
            get;
            set;
        }

        /// <summary>
        /// WorkUnitLevel : 工作单位级别，可以用来设置数据字典，例如：三甲、二甲等
        /// </summary>     
        [DataMember]
        public Int32 WorkUnitLevel
        {
            get;
            set;
        }

        /// <summary>
        /// SectionOffice : 科室
        /// </summary>     
        [DataMember]
        public String SectionOffice
        {
            get;
            set;
        }

        /// <summary>
        /// InvoiceUnit : 发票抬头
        /// </summary>       
        [DataMember]
        public String InvoiceUnit
        {
            get;
            set;
        }

        /// <summary>
        /// Mentor : 导师
        /// </summary>      
        [DataMember]
        public String Mentor
        {
            get;
            set;
        }

        /// <summary>
        /// 作者著作
        /// </summary>
        [DataMember]
        public string AuthorOpus
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
        /// qq
        /// </summary>
        [DataMember]
        public String QQ { get; set; }

        /// <summary>
        /// MSN
        /// </summary>
        [DataMember]
        public String MSN { get; set; }

        /// <summary>
        /// ReserveField : 备用字段
        /// </summary>      
        [DataMember]
        public String ReserveField
        {
            get;
            set;
        }

        /// <summary>
        /// ReserveField1 : 备用字段
        /// </summary>     
        [DataMember]
        public String ReserveField1
        {
            get;
            set;
        }

        /// <summary>
        /// ReserveField2 : 备用字段
        /// </summary>      
        [DataMember]
        public String ReserveField2
        {
            get;
            set;
        }

        /// <summary>
        /// ReserveField3 : 备用字段
        /// </summary>       
        [DataMember]
        public String ReserveField3
        {
            get;
            set;
        }

        /// <summary>
        /// ReserveField4 : 备用字段
        /// </summary>      
        [DataMember]
        public String ReserveField4
        {
            get;
            set;
        }

        /// <summary>
        /// ReserveField5 : 备用字段
        /// </summary>    
        [DataMember]
        public String ReserveField5
        {
            get;
            set;
        }

        /// <summary>
        /// AddDate : 添加日期
        /// </summary>      
        [DataMember]
        public DateTime AddDate
        {
            get;
            set;
        }
        #endregion

        #endregion 属性、变量声明

        /// <summary>
        /// 邮件
        /// </summary>
        [DataMember]
        public String Emial { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        [DataMember]
        public bool IsChecked { get; set; }

        /// <summary>
        /// 登录信息
        /// </summary>
        [DataMember]
        public AuthorInfoEntity AuthorModel { get; set; }

        /// <summary>
        /// 专家分组
        /// </summary>
        [DataMember]
        public IList<ExpertGroupMapEntity> ExpertGroupList { get; set; }

        /// <summary>
        /// 字段信息
        /// </summary>
        [DataMember]
        public List<FieldsSet> FieldList { get; set; }

        public AuthorDetailEntity()
        {
            PKID = (long)0;
            AuthorID = (long)0;
            JournalID = (long)0;
            AuthorName = string.Empty;
            EnglishName = string.Empty;
            Gender = "1";
            Nation = string.Empty;
            Birthday = null;
            NativePlace = string.Empty;
            Province = string.Empty;
            City = string.Empty;
            Area = string.Empty;
            IDCard = string.Empty;
            Address = string.Empty;
            ZipCode = string.Empty;
            Mobile = string.Empty;
            Tel = string.Empty;
            Fax = string.Empty;
            Education = 0;
            Professional = string.Empty;
            JobTitle = 0;
            Job = string.Empty;
            ResearchTopics = string.Empty;
            WorkUnit = string.Empty;
            WorkUnitLevel = (int)0;
            SectionOffice = string.Empty;
            InvoiceUnit = string.Empty;
            Mentor = string.Empty;
            Remark = string.Empty;
            QQ = string.Empty;
            MSN = string.Empty;
            ReserveField = null;
            ReserveField1 = null;
            ReserveField2 = null;
            ReserveField3 = null;
            ReserveField4 = null;
            ReserveField5 = null;
            AddDate = DateTime.Now;
        }
	}

}
