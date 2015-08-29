using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    [Serializable]
    [DataContract]
    public partial class ExpertApplyLogQuery : QueryBase
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
        /// 邮编
        /// </summary>
        [DataMember]
        public String ZipCode
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
        /// ResearchTopics : 研究方向
        /// </summary>
        [DataMember]
        public String ResearchTopics
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public String Remark
        {
            get;
            set;
        }

        /// <summary>
        /// Status : 状态
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
        /// 排序字符串
        /// </summary>
        [DataMember]
        public String OrderByStr
        {
            get;
            set;
        }

        #endregion


        public ExpertApplyLogQuery()
        {
            
        }

    }
}
