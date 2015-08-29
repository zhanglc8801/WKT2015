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
    /// 工作量统计
    /// </summary>
    [DataContract]
    [Serializable]
    public class WorkloadEntity
    {
        [DataMember]
        public long AuthorID
        {
            get;
            set;
        }

        /// <summary>
        /// LoginName : 
        /// </summary>
        [DataMember]
        public String LoginName
        {
            get;
            set;
        }

        /// <summary>
        /// RealName : 真实姓名
        /// </summary>
        [DataMember]
        public String RealName
        {
            get;
            set;
        }

        /// <summary>
        /// HandedCount : 处理数量
        /// </summary>
        [DataMember]
        public int AlreadyCount
        {
            get;
            set;
        }

        /// <summary>
        /// HandingCount : 待处理数量
        /// </summary>
        [DataMember]
        public int WaitCount
        {
            get;
            set;
        }

        /// <summary>
        /// StatusID : 状态ID
        /// </summary>
        [DataMember]
        public int StatusID
        {
            get;
            set;
        }

        /// <summary>
        /// StatusName : 状态名
        /// </summary>
        [DataMember]
        public string StatusName
        {
            get;
            set;
        }

        /// <summary>
        /// WorkCount : 工作数量
        /// </summary>
        [DataMember]
        public int WorkCount
        {
            get;
            set;
        }

        /// <summary>
        /// 专家审稿费
        /// </summary>
        [DataMember]
        public string ExpertReviewFee
        {
            get;
            set;
        }

        /// <summary>
        /// 动态显示所有的列
        /// </summary>
        public StringBuilder ShowAllCol
        {
            get;
            set;
        }

        /// <summary>
        /// 邮编
        /// </summary>
        [DataMember]
        public string ZipCode
        {
            get;
            set;
        }

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// 工作单位
        /// </summary>
        [DataMember]
        public string WorkUnit
        {
            get;
            set;
        }

        /// <summary>
        /// 发票单位
        /// </summary>
        [DataMember]
        public string InvoiceUnit
        {
            get;
            set;
        }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember]
        public string Mobile
        {
            get;
            set;
        }

        /// <summary>
        /// 电话
        /// </summary>
        [DataMember]
        public string Tel
        {
            get;
            set;
        }

        /// <summary>
        /// 银行信息：户名
        /// </summary>
        [DataMember]
        public string ReserveField1
        {
            get;
            set;
        }
        /// <summary>
        /// 银行信息：帐号
        /// </summary>
        [DataMember]
        public string ReserveField2
        {
            get;
            set;
        }
        /// <summary>
        /// 银行信息：开户行
        /// </summary>
        [DataMember]
        public string ReserveField3
        {
            get;
            set;
        }

        //private IDictionary<string, int> _DictEditorStatItems = new Dictionary<string, int>();
        ///// <summary>
        ///// DictEditorStatItems : 编辑统计项
        ///// </summary>
        //[DataMember]
        //public IDictionary<string, int> DictEditorStatItems
        //{
        //    get { return _DictEditorStatItems; }
        //    set { _DictEditorStatItems = value; }
        //}
    }
}
