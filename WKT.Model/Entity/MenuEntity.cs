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
	///  表'Menu'的实体表示.
	/// </summary>
	/// <remarks>
	/// 该实体由工具生成，尽量不要手动修改
	/// </remarks>
    [DataContract]
    public partial class MenuEntity : ObjectBase
    {
        /// <summary>			
        /// MenuID : 
        /// </summary>
        /// <remarks>表Menu主键</remarks>
        [DataMember]
        public Int64 MenuID
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
        /// PMenuID : 父ID
        /// </summary>
        [DataMember]
        public Int32 PMenuID
        {
            get;
            set;
        }

        /// <summary>
        /// MenuName : 菜单名称
        /// </summary>
        [DataMember]
        public String MenuName
        {
            get;
            set;
        }

        /// <summary>
        /// MenuUrl : 菜单项链接
        /// </summary>
        [DataMember]
        public String MenuUrl
        {
            get;
            set;
        }

        /// <summary>
        /// IconUrl : 图标
        /// </summary>
        [DataMember]
        public String IconUrl
        {
            get;
            set;
        }

        /// <summary>
        /// SortID : 排序值
        /// </summary>
        [DataMember]
        public Int32 SortID
        {
            get;
            set;
        }

        /// <summary>
        /// MenuType : 菜单类型  1=页面 2=按钮
        /// </summary>
        [DataMember]
        public Byte MenuType
        {
            get;
            set;
        }

        /// <summary>
        /// GroupID : 分组类型 1=编辑 2=作者 3=专家
        /// </summary>
        [DataMember]
        public Byte GroupID
        {
            get;
            set;
        }

        /// <summary>
        /// Status : 状态 1=正常 0=停用
        /// </summary>
        [DataMember]
        public Byte Status
        {
            get;
            set;
        }

        [DataMember]
        public Boolean IsContentMenu
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


        private IList<MenuEntity> _children = new List<MenuEntity>();
        [DataMember]
        public IList<MenuEntity> children
        {
            get { return _children; }
            set { _children = value; }
        }

        public MenuEntity()
        {
            MenuID = (long)0;
            JournalID = (long)0;
            PMenuID = (int)0;
            MenuName = string.Empty;
            MenuUrl = string.Empty;
            IconUrl = string.Empty;
            SortID = (int)0;
            MenuType = (byte)0;
            GroupID = (byte)0;
            Status = (byte)1;
            IsContentMenu = false;
            AddDate = DateTime.Now;
        }
    }
}
