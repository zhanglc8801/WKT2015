using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    [Serializable]
    [DataContract]
    public class DoiRegLogQuery : QueryBase
    {
        #region 属性、变量声明

        /// <summary>			
        /// PKID : 登录错误日志信息
        /// </summary>
        /// <remarks>表DoiRegLog主键</remarks>
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
        /// DoiFileName : 上传DOI注册文件后系统返回的文件名
        /// </summary>
        [DataMember]
        public String DoiFileName
        {
            get;
            set;
        }

        /// <summary>
        /// State : 状态
        /// </summary>
        [DataMember]
        public String State
        {
            get;
            set;
        }

        /// <summary>
        /// Year : 年
        /// </summary>
        [DataMember]
        public int Year
        {
            get;
            set;
        }

        /// <summary>
        /// Issue : 期
        /// </summary>
        [DataMember]
        public int Issue
        {
            get;
            set;
        }

        /// <summary>
        /// AdminID : 注册者ID
        /// </summary>
        [DataMember]
        public Int64 AdminID
        {
            get;
            set;
        }

        /// <summary>
        /// LoginDate : 尝试登录时间
        /// </summary>
        [DataMember]
        public DateTime AddDate
        {
            get;
            set;
        }
        #endregion

        public DoiRegLogQuery()
        {
            
        }


    }
}
