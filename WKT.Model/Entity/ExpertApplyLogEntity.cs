using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    [Serializable]
    [DataContract]
    public partial class ExpertApplyLogEntity:ObjectBase
    {
        #region 属性、变量声明

        /// <summary>			
        /// PKID : 申请成为审稿专家日志表
        /// </summary>
        /// <remarks>表ExpertApplyLogEntity主键</remarks>
        [DataMember]
        public Int64 PKID
        {
            get;
            set;
        }

        /// <summary>
        /// 杂志ID
        /// </summary>
        [DataMember]
        public Int64 JournalID
        {
            get;
            set;
        }

        /// <summary>
        /// LoginName：申请者邮箱
        /// </summary>
        [DataMember]
        public String LoginName
        {
            get;
            set;
        }

        /// <summary>
        /// RealName : 申请者真实姓名
        /// </summary>
        [DataMember]
        public String RealName
        {
            get;
            set;
        }

        /// <summary>
        /// Gender : 申请者性别 1=男 2=女
        /// </summary>
        [DataMember]
        public int Gender
        {
            get;
            set;
        }

        /// <summary>
        /// Birthday : 出生日期
        /// </summary>
        [DataMember]
        public DateTime Birthday
        {
            get;
            set;
        }

        /// <summary>
        /// Education:学历
        /// </summary>
        [DataMember]
        public int Education
        {
            get;
            set;
        }

        /// <summary>
        /// JobTitle：职称
        /// </summary>
        [DataMember]
        public int JobTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 电话
        /// </summary>
        [DataMember]
        public String Tel
        {
            get;
            set;
        }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember]
        public String Mobile
        {
            get;
            set;
        }

        /// <summary>
        /// 单位
        /// </summary>
        [DataMember]
        public String WorkUnit
        {
            get;
            set;
        }

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public String Address
        {
            get;
            set;
        }

        /// <summary>
        /// ReviewDomain : 审稿领域
        /// </summary>
        [DataMember]
        public String ReviewDomain
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
        /// BankID : 银行账号
        /// </summary>
        [DataMember]
        public String BankID
        {
            get;
            set;
        }

        /// <summary>
        /// BankType : 银行类型
        /// </summary>
        [DataMember]
        public int BankType
        {
            get;
            set;
        }

        /// <summary>
        /// 邮编
        /// </summary>
        [DataMember]
        public String ZipCode
        {
            get;
            set;
        }

        /// <summary>
        /// 备注：用于标注开户行、未通过原因等
        /// </summary>
        [DataMember]
        public String Remark
        {
            get;
            set;
        }

        /// <summary>
        /// Status : 状态 1=通过 0=申请中 -1=未通过
        /// </summary>
        [DataMember]
        public Byte Status
        {
            get;
            set;
        }

        /// <summary>
        /// ActionUser : 操作人ID
        /// </summary>
        [DataMember]
        public Int64 ActionUser
        {
            get;
            set;
        }

        /// <summary>
        /// AddDate : 申请日期
        /// </summary>
        [DataMember]
        public DateTime AddDate
        {
            get;
            set;
        }

        /// <summary>
        /// ActionDate : 执行操作日期
        /// </summary>
        [DataMember]
        public DateTime ActionDate
        {
            get;
            set;
        }
        #endregion


        public ExpertApplyLogEntity()
        {
            JournalID = 0;
            LoginName = string.Empty;
            RealName = string.Empty;
            Gender = 1;
            Birthday = DateTime.Now;
            Education = 1;
            JobTitle = 1;
            Tel = string.Empty;
            Mobile = string.Empty;
            WorkUnit = string.Empty;
            Address = string.Empty;
            ReviewDomain = string.Empty;
            ResearchTopics = string.Empty;
            BankID = string.Empty;
            BankType = 1;
            ZipCode = string.Empty;
            Remark = string.Empty;
            Status = 0;
            ActionUser = 0;
            AddDate = DateTime.Now;
        }

    }
}
