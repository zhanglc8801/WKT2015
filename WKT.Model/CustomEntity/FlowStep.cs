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
    /// 审稿流程步骤
    /// </summary>
    [DataContract]
    [Serializable]
    public class FlowStep
    {
        /// <summary>
        /// 审稿状态基本信息
        /// </summary>
        [DataMember]
        public FlowStatusEntity FlowStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 审稿流程操作基本信息
        /// </summary>
        [DataMember]
        public FlowActionEntity FlowAction
        {
            get;
            set;
        }

        /// <summary>
        /// 审稿状态配置信息
        /// </summary>
        [DataMember]
        public FlowConfigEntity FlowConfig
        {
            get;
            set;
        }

        /// <summary>
        /// 审稿状态列表
        /// </summary>
        [DataMember]
        public List<FlowStatusEntity> FlowStatusList
        {
            get;
            set;
        }

        /// <summary>
        /// 审稿流程步骤操作信息
        /// </summary>
        [DataMember]
        public IList<FlowActionEntity> FlowActions
        {
            get;
            set;
        }

        /// <summary>
        /// 审稿成员列表
        /// </summary>
        [DataMember]
        public List<AuthorInfoEntity> FlowAuthorList
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件信息
        /// </summary>
        [DataMember]
        public IDictionary<long,string> DictContribution
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件处理日志ID
        /// </summary>
        [DataMember]
        public IDictionary<long, long> DictLogID
        {
            get;
            set;
        }

        private MessageTemplateEntity _emailTempate = new MessageTemplateEntity();
        public MessageTemplateEntity EmailTemplate
        {
            get { return _emailTempate; }
            set { _emailTempate = value; }
        }

        private MessageTemplateEntity _smsTempate = new MessageTemplateEntity();
        public MessageTemplateEntity SMSTemplate
        {
            get { return _smsTempate; }
            set { _smsTempate = value; }
        }


        /// <summary>
        /// SMSUserID:用户ID（商信通）
        /// </summary>
        [DataMember]
        public String SMSUserID
        {
            get;
            set;
        }
        /// <summary>
        /// SMSChildName:子账户名（商信通）
        /// </summary>
        [DataMember]
        public String SMSChildName
        {
            get;
            set;
        }
        /// <summary>
        /// SMSUserPwd:账户密码（商信通）
        /// </summary>
        [DataMember]
        public String SMSUserPwd
        {
            get;
            set;
        }


    }
}