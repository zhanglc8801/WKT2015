using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Common.Extension
{
    /// <summary>
    /// Function: 数据扩展类
    /// (
    ///    使用前引用该命名空间，使用：
    ///    DataRow dr = new BUser().GetUser();  
    ///    MO_user mo_user = dr.ToModel<MO_user>();
    ///    DataTable dt = new BUser().GetUsers();
    ///    List<MO_user> list = dt.ToList<Mo_user>();
    /// )
    /// by sxd
    /// </summary>
    public static class DataExtension
    {
        /// <summary>
        /// DataRow扩展方法：将DataRow类型转化为指定类型的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public static T ToModel<T>(this DataRow dr) where T : class, new()
        {
            return ToModel<T>(dr, true);
        }
        /// <summary>
        /// DataRow扩展方法：将DataRow类型转化为指定类型的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dateTimeToString">是否需要将日期转换为字符串，默认为转换,值为true</param>
        /// <returns></returns>
        /// <summary>
        public static T ToModel<T>(this DataRow dr, bool dateTimeToString) where T : class, new()
        {
            if (dr != null)
                return ToList<T>(dr.Table, dateTimeToString).First();

            return null;
        }

        /// <summary>
        /// DataTable扩展方法：将DataTable类型转化为指定类型的实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            return ToList<T>(dt, true);
        }

        /// <summary>
        /// DataTable扩展方法：将DataTable类型转化为指定类型的实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dateTimeToString">是否需要将日期转换为字符串，默认为转换,值为true</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt, bool dateTimeToString) where T : class, new()
        {
            List<T> list = new List<T>();

            if (dt != null)
            {
                List<PropertyInfo> infos = new List<PropertyInfo>();

                Array.ForEach<PropertyInfo>(typeof(T).GetProperties(), p =>
                {
                    if (dt.Columns.Contains(p.Name) == true)
                    {
                        infos.Add(p);
                    }
                });

                SetList<T>(list, infos, dt, dateTimeToString);
            }

            return list;
        }

        #region 私有方法

        private static void SetList<T>(List<T> list, List<PropertyInfo> infos, DataTable dt, bool dateTimeToString) where T : class, new()
        {
            foreach (DataRow dr in dt.Rows)
            {
                T model = new T();

                infos.ForEach(p =>
                {
                    if (dr[p.Name] != DBNull.Value)
                    {
                        object tempValue = dr[p.Name];
                        if (dr[p.Name].GetType() == typeof(DateTime) && dateTimeToString == true)
                        {
                            tempValue = dr[p.Name].ToString();
                        }
                        try
                        {
                            p.SetValue(model, tempValue, null);
                        }
                        catch { }
                    }
                });
                list.Add(model);
            }
        }

        #endregion
    }
}
