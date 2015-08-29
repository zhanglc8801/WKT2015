using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;

namespace WKT.Data.SQL
{
    public static class DataReaderExtensions
    {
        public static bool HasColumn(this IDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public static bool HasColumn(this DataRow dr, string columnName)
        {
            if (dr != null)
            {
                return dr.Table.Columns.Contains(columnName);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取dr(IDataReader)的值    小石 2011/8/31 添加
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dr">IDataReader</param>
        /// <param name="colName">列名</param>
        /// <param name="defaultValue">默认值-可空(默认default(T))</param> 
        /// <returns></returns>
        public static T GetDrValue<T>(this IDataReader dr, string colName, T defaultValue = default(T))
        {
            if (dr == null || string.IsNullOrWhiteSpace(colName) || Convert.IsDBNull(dr[colName]))
                return defaultValue;
            // 先得到列的索引，再通过索引读取数据，比直接使用列名效率高 by sxd
            int colIndex = dr.GetOrdinal(colName);
            return (T)dr[colIndex];
        }

        /// <summary>
        /// 
        /// by tlq
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="colName"></param>
        /// <param name="hasColumn"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetDataRowValue<T>(this DataRow dr, string colName, bool hasColumn = false, T defaultValue = default(T))
        {
            if (dr == null || string.IsNullOrWhiteSpace(colName) || (hasColumn && !dr.HasColumn(colName)) || Convert.IsDBNull(dr[colName]))
                return defaultValue;
            return (T)dr[colName];
        }
    }
}
