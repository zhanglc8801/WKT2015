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
    ///  表'FlowLogInfo'的实体表示.
    /// </summary>
    /// <remarks>
    /// 该实体由工具生成，尽量不要手动修改
    /// </remarks>
    [DataContract]
    public partial class FlowLogInfoEntity : ObjectBase
    {
        #region 属性、变量声明

        /// <summary>			
        /// FlowLogID : 
        /// </summary>
        /// <remarks>表FlowLogInfo主键</remarks>		
        [DataMember]
        public Int64 FlowLogID
        {
            get;
            set;
        }

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
        /// CNumber : 稿件编号
        /// </summary>       
        [DataMember]
        public string CNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 发送人
        /// </summary>
        [DataMember]
        public Int64 SendUserID
        {
            get;
            set;
        }

        /// <summary>
        /// 接收人
        /// </summary>
        [DataMember]
        public Int64 RecUserID
        {
            get;
            set;
        }

        /// <summary>
        /// StatusID : 审稿状态ID
        /// </summary>       
        [DataMember]
        public Int64 StatusID
        {
            get;
            set;
        }

        /// <summary>
        /// CStatus : 当前状态下稿件状态ID
        /// </summary>       
        [DataMember]
        public int CStatus
        {
            get;
            set;
        }

        /// <summary>
        /// ActionID : 操作ID
        /// </summary>       
        [DataMember]
        public Int64 ActionID
        {
            get;
            set;
        }

        /// <summary>
        /// ActionType : 操作类型
        /// </summary>       
        [DataMember]
        public Byte ActionType
        {
            get;
            set;
        }

        /// <summary>
        /// 目标审稿状态
        /// </summary>
        [DataMember]
        public Int64 TargetStatusID
        {
            get;
            set;
        }

        /// <summary>
        /// Status : 是否有效 0=无效 1=有效
        /// </summary>       
        [DataMember]
        public Byte Status
        {
            get;
            set;
        }

        /// <summary>
        /// IsView : 是否已阅
        /// </summary>       
        [DataMember]
        public bool IsView
        {
            get;
            set;
        }

        /// <summary>
        /// IsDown : 是否已下载
        /// </summary>       
        [DataMember]
        public bool IsDown
        {
            get;
            set;
        }


        /// <summary>
        /// 处理意见
        /// </summary>
        [DataMember]
        public string DealAdvice
        {
            get;
            set;
        }

        public string FormatDealAdvice
        {
            get
            {
                string returnData = string.Empty;
                if (!string.IsNullOrEmpty(DealAdvice))
                {
                    returnData = DealAdvice.Replace("\n\t", "<p>").Replace("\n<p>", "</p>");
                }
                return returnData;
            }
        }

        /// <summary>
        /// 处理时间
        /// </summary>
        [DataMember]
        public DateTime? DealDate
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件附件
        /// </summary>
        [DataMember]
        public string CPath
        {
            get;
            set;
        }
        /// <summary>
        /// 稿件自定义文件名
        /// </summary>
        [DataMember]
        public string CFileName { get; set; }

        /// <summary>
        /// 图表附件
        /// </summary>
        [DataMember]
        public string FigurePath
        {
            get;
            set;
        }
        /// <summary>
        /// 附件自定义文件名
        /// </summary>
        [DataMember]
        public string FFileName { get; set; }

        /// <summary>
        /// 其他附件
        /// </summary>
        [DataMember]
        public string OtherPath
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

        /// <summary>
        /// 发送人角色ID
        /// </summary>
        [DataMember]
        public long SendRoleID
        {
            get;
            set;
        }

        /// <summary>
        /// 接收人角色ID
        /// </summary>
        [DataMember]
        public long RecRoleID
        {
            get;
            set;
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        [DataMember]
        public string StatusName
        {
            get;
            set;
        }



        #endregion

        /// <summary>
        /// 发送人所属分组
        /// </summary>
        public string SendUserGroupName
        {
            get;
            set;
        }

        /// <summary>
        /// 发送人姓名
        /// </summary>
        [DataMember]
        public string SendUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 接收人姓名
        /// </summary>
        [DataMember]
        public string RecUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 接收人所属分组
        /// </summary>
        public string RecUserGroupName
        {
            get;
            set;
        }

        /// <summary>
        /// 发送人邮箱
        /// </summary>
        [DataMember]
        public string SendUserEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 接收人邮箱
        /// </summary>
        [DataMember]
        public string RecUserEmail
        {
            get;
            set;
        }

        /// <summary>
        /// 是否有审稿单 0=否 1=是
        /// </summary>
        [DataMember]
        public byte IsHaveBill
        {
            get;
            set;
        }

        public FlowLogInfoEntity()
        {
            FlowLogID = (long)0;
            JournalID = (long)0;
            CID = (long)0;
            StatusID = (long)0;
            ActionID = (long)0;
            Status = (byte)1;
            SendUserID = (long)0;
            RecUserID = (byte)0;
            DealAdvice = string.Empty;
            IsHaveBill = 0;
            AddDate = DateTime.Now;
        }
    }
}
