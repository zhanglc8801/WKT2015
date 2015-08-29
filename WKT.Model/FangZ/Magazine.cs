using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model.FangZ
{
    public class Magazine
    {
        public string magzineId
        {
            get;
            set;
        }

        public string magzineName
        {
            get;
            set;
        }

        public List<magazinecolumn> magazinecolumns
        {
            get;
            set;
        }
    }

    public class magazinecolumn
    {
        public string name
        {
            get;
            set;
        }
    }
}
