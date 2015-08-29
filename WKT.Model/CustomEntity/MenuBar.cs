using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model.CustomEntity
{
    /// <summary>
    /// 菜单工具实体
    /// </summary>
    public class MenuBar
    {
        public int width
        {
            get;
            set;
        }

        public List<MenuItem> items
        {
            get;set;
        }
    }

    public class MenuItem
    {
        public string text
        {
            get;
            set;
        }

        public string icon
        {
            get;
            set;
        }

        public List<MenuItem> children
        {
            get;
            set;
        }
    }
}
