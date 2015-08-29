using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model.CustomEntity
{
    public class Flows
    {
        /// <summary>
        /// 节点数
        /// </summary>
        public int total
        {
            get;
            set;
        }

        /// <summary>
        /// 各个节点
        /// </summary>
        public IList<FlowNode> list
        {
            get;
            set;
        }
    }

    public class FlowNode
    {
        public string id
        {
            get;
            set;
        }

        /// <summary>
        /// 顺序号
        /// </summary>
        public string prcs_id
        {
            get;
            set;
        }

        /// <summary>
        /// 鼠标移到节点上的提示
        /// </summary>
        public string title
        {
            get;
            set;
        }

        /// <summary>
        /// 节点上的显示内容，支持HTML标签
        /// </summary>
        public string prcs_content
        {
            get;
            set;
        }

        /// <summary>
        /// 左侧位置
        /// </summary>
        public int left
        {
            get;
            set;
        }

        /// <summary>
        /// 顶部位置
        /// </summary>
        public int top
        {
            get;
            set;
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public EnumProcType prcs_type
        {
            get;
            set;
        }

        /// <summary>
        /// 下一个环节,prcs_id,多个下个环节id用逗号分隔
        /// </summary>
        public string to
        {
            get;
            set;
        }
    }

    public enum EnumProcType
    {
        start,
        general,
        end
    }
}
