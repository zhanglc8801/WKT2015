using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.DataAccess
{
    public class DataAccessBase
    {
        /// <summary>
        /// 分页语句(不带ROW_NUMBER)
        /// </summary>
        protected string SQL_Page_Select
        {
            get
            {
                return "SELECT * FROM ({0}) t WHERE ROW_ID between {1} and {2} ORDER BY ROW_ID";
            }
        }

        /// <summary>
        /// 分页语句(带ROW_NUMBER)
        /// </summary>
        protected string SQL_Page_Select_ROWNumber
        {
            get
            {
                return "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {3}) AS ROW_ID,* From ({0}) t) sp WHERE ROW_ID between {1} and {2} ORDER BY ROW_ID";
            }
        }
    }
}
