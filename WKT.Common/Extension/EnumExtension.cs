using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace WKT.Common.Extension
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="e">枚举类型</param>
        /// <returns></returns>
        public static string GetEnumDesc(this Enum e)
        {
            try
            {
                FieldInfo ms = e.GetType().GetField(e.ToString());
                DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])ms.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (EnumAttributes.Length > 0)
                {
                    return EnumAttributes[0].Description;
                }
                return e.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取枚举键值集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<object, object> GetEnumDictionary(this Type type)
        {
            FieldInfo[] fields = type.GetFields();
            var dic = new Dictionary<object, object>();
            foreach (FieldInfo field in fields)
            {
                string desription;
                object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs != null && objs.Length != 0)
                {
                    var da = (DescriptionAttribute)objs[0];
                    desription = da.Description;
                }
                else
                {
                    desription = field.Name;
                }
                if (field.Name.Equals("value__")) 
                    continue;

                dic.Add(((int)Enum.Parse(type, field.Name)).ToString(), desription);
            }
            return dic;
        }
    }
}
