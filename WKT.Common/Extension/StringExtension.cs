using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WKT.Common.Extension
{

    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 2009-2-6
        /// 数据库空时间
        /// </summary>
        public static readonly DateTime NullSqlDateTime = ((DateTime)System.Data.SqlTypes.SqlDateTime.Null);

        #region 类型转化

        public static bool ToBoolean(this string source)
        {
            bool reValue;
            bool.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为Byte型
        /// </summary>
        /// <returns>Byte</returns>
        public static Byte ToByte(this string source)
        {
            Byte reValue;
            Byte.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为Short型
        /// </summary>
        /// <returns>Short</returns>
        public static short ToShort(this string source)
        {
            short reValue;
            short.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为Short型
        /// </summary>
        /// <returns>Short</returns>
        public static short ToInt16(this string source)
        {
            short reValue;
            short.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为int32型
        /// </summary>
        /// <returns>int32</returns>
        public static int ToInt32(this string source)
        {
            int reValue;
            Int32.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为int64型
        /// </summary>
        /// <returns>int64</returns>
        public static long ToInt64(this string source)
        {
            long reValue;
            Int64.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为Double型
        /// </summary>
        /// <returns>decimal</returns>
        public static Double ToDouble(this string source)
        {
            Double reValue;
            Double.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为decimal型
        /// </summary>
        /// <returns>decimal</returns>
        public static decimal ToDecimal(this string source)
        {
            decimal reValue;
            decimal.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为日期为空里返回NullSqlDateTime,byark
        /// </summary>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime(this string source)
        {
            DateTime reValue;
            if (DateTime.TryParse(source, out reValue))
                return reValue;
            else
                return NullSqlDateTime;
        }
        /// <summary>
        /// 转化为日期为空里返回NullSqlDateTime,byark
        /// </summary>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTimeByNum(this string source)
        {
            //20050102010101
            DateTime reValue = NullSqlDateTime;
            if (source.Length == 14)
            {
                if (!DateTime.TryParse(source.Substring(0, 4) + "-" + source.Substring(4, 2) + "-" + source.Substring(6, 2) + " "
                    + source.Substring(8, 2) + ":" + source.Substring(10, 2) + ":" + source.Substring(12, 2), out reValue))
                    reValue = NullSqlDateTime;
            }
            return reValue;
        }
        /// <summary>
        /// 转化为数字类型的日期
        /// </summary>
        /// <returns>DateTime</returns>
        public static decimal ToDateTimeDecimal(this string source)
        {
            DateTime reValue;
            if (DateTime.TryParse(source, out reValue))
            {
                return reValue.ToString("yyyyMMddHHmmss").ToDecimal();
            }
            else
                return 0;
        }
        /// <summary>
        /// HDF
        /// 2009-3-12
        /// 将时间转换成数字
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal ToDateTimeDecimal(this DateTime source)
        {
            return source.ToString("yyyyMMddHHmmss").ToDecimal();
        }
        #endregion

        #region 判断

        /// <summary>
        /// 判断是否是正确的电子邮件格式 
        /// </summary>
        public static bool IsEmail(this string source)
        {
            return Regex.IsMatch(source, @"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", RegexOptions.Compiled);
        }

        /// <summary>判断是否是正确的身份证编码格式</summary>
        public static bool IsIDCard(this string source)
        {
            return Regex.IsMatch(source, @"^\d{17}(\d|x)$|^\d{15}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 判断是否是正确的身份证编码格式
        /// </summary>
        public static bool IsIDCard(this string source, out string mesage)
        {
            if (source.Length == 18)
                return IsIDCard18(source, out mesage);
            else if (source.Length == 15)
                return IsIDCard15(source, out mesage);
            else
            {
                mesage = "身份证长度不对";
                return false;
            }
        }

        /// <summary>
        /// 判断是否是正确的身份证编码格式
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="mesage"></param>
        /// <returns></returns>
        public static bool IsIDCard18(string Id, out string mesage)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                mesage = "不是有效的身份证号";
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                mesage = "省份不合法";
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                mesage = "生日不合法";
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                mesage = "不是有效的身份证号";
                return false;//校验码验证
            }
            mesage = "正确";
            return true;//符合GB11643-1999标准
        }

        /// <summary>
        /// 判断是否是正确的身份证编码格式
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="mesage"></param>
        /// <returns></returns>
        public static bool IsIDCard15(string Id, out string mesage)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                mesage = "不是有效的身份证号";
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                mesage = "省份不合法";
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                mesage = "生日不合法";
                return false;//生日验证
            }
            mesage = "正确";
            return true;//符合15位身份证标准
        }

        /// <summary>
        /// 判断是否是正确的邮政编码格式
        /// </summary>
        public static bool IsPostcode(this string source)
        {
            return Regex.IsMatch(source, @"^[1-9]{1}(\d){5}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 判断是否是正确的中国移动或联通电话
        /// </summary>
        public static bool IsMobilePhone(this string source)
        {
            bool data = false;
            /// 匹配移动手机号
            if (Regex.IsMatch(source, @"^1(3[4-9]|5[012789]|8[78])\d{8}$", RegexOptions.Compiled))
            {
                data = true;
            }
            /// 匹配电信手机号
            else if (Regex.IsMatch(source, @"^18[09]\d{8}$", RegexOptions.Compiled))
            {
                data = true;
            }
            /// 匹配联通手机号
            else if (Regex.IsMatch(source, @"^1(3[0-2]|5[56]|8[56])\d{8}$", RegexOptions.Compiled))
            {
                data = true;
            }
            /// 匹配CDMA手机号
            else if (Regex.IsMatch(source, @"^1[35]3\d{8}$", RegexOptions.Compiled))
            {
                data = true;
            }

            return data;
        }

        /// <summary>
        /// 判断是否是正确的中国固定电话 
        /// </summary>
        public static bool IsTelephone(this string source)
        {
            return Regex.IsMatch(source, @"^((\d{3,4})|\d{3,4}-|\s)?\d{8}$", RegexOptions.Compiled);
        }
        /// <summary>
        /// 包含html
        /// </summary>
        public static bool IsHasHtml(this string source)
        {
            Regex reg = new Regex(@"<|>");
            if (reg.IsMatch(source))
                return true;
            return false;
        }
        /// <summary>
        /// 是否匹配正则表达式，匹配返回true，否则false
        /// </summary>
        public static bool IsMatchRegex(this string source, string regex)
        {
            Regex r = new Regex(regex);
            if (r.IsMatch(source))
                return true;
            return false;
        }

        /// <summary>
        /// 判断字符串是否是IP，如果是返回True，不是返回False
        /// </summary>
        public static bool IsIP(this string source)
        {
            //Regex regex = new Regex(@"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))");
            Regex regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])"
                + @"\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$", RegexOptions.Compiled);
            if (regex.Match(source).Success)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否包含中文或全角字符 
        /// </summary>
        public static bool IsHasChinese(this string checkStr)
        {
            ASCIIEncoding n = new ASCIIEncoding();
            byte[] b = n.GetBytes(checkStr);
            for (int i = 0; i <= b.Length - 1; i++)
                if (b[i] == 63) return true;  //判断是否(T)为汉字或全脚符号 
            return false;
        }

        /// <summary>
        /// 是否是Url
        /// </summary>
        /// <param name="checkStr"></param>
        /// <returns></returns>
        public static bool IsURLAddress(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"[a-zA-z]+://[^s]*", RegexOptions.Compiled);
        }

        /// <summary>
        /// 是否是中文
        /// </summary>
        public static bool IsAllChinese(this string checkStr)
        {
            checkStr = checkStr.Trim();
            if (checkStr == string.Empty) return false;
            Regex reg = new Regex(@"^([\u4e00-\u9fa5]*)$", RegexOptions.Compiled);
            if (reg.IsMatch(checkStr))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否为正整数
        /// </summary>
        public static bool IsInt(this string intStr)
        {
            Regex regex = new Regex("^\\d+$", RegexOptions.Compiled);
            return regex.IsMatch(intStr.Trim());
        }

        /// <summary>
        /// 非负整数
        /// </summary>
        public static bool IsIntWithZero(this string intStr)
        {
            return Regex.IsMatch(intStr, @"^\\d+$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="checkStr"></param>
        /// <returns></returns>
        public static bool IsNumber(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^[+-]?[0123456789]*[.]?[0123456789]*$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 是否是Decimal类型
        /// </summary>
        /// <param name="checkStr"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^[0-9]+/.?[0-9]{0,2}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 是否是日期格式
        /// </summary>
        /// <param name="checkStr"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^[ ]*[012 ]?[0123456789]?[0123456789]{2}[ ]*[-]{1}[ ]*[01]?[0123456789]{1}[ ]*[-]{1}[ ]*[0123]?[0123456789]"
                + @"{1}[ ]*[012]?[0123456789]{1}[ ]*[:]{1}[ ]*[012345]?[0123456789]{1}[ ]*[:]{1}[ ]*[012345]?[0123456789]{1}[ ]*$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 判断是否是XML 1.0允许的字符
        /// </summary>
        private static bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */        ||
                 character == 0xA /* == '\n' == 10  */        ||
                 character == 0xD /* == '\r' == 13  */        ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }

        /// <summary>
        /// 判断是否是合法的 XML 1.0标准允许的字符串 true：标准 false：包含不标准的字符
        /// </summary>
        public static bool IsLegalXmlChar(this string xml)
        {
            if (string.IsNullOrEmpty(xml)) return true;
            foreach (char c in xml)
                if (!IsLegalXmlChar(c)) return false;
            return true;
        }

        #endregion

        #region 安全，过滤，辅助

        /// <summary>
        /// 获取字符串长度，按中文2位，英文1位进行计算
        /// </summary>
        public static int CharCodeLength(this string source)
        {
            int n = 0;
            foreach (var c in source.ToCharArray())
                if ((int)c < 128)
                    n++;
                else
                    n += 2;
            return n;
        }

        /// <summary>
        /// 文本内容安全过滤
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string TextFilter(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return "";
            string[] tags = new string[] { "body", "embed", "frame","script","frameset","html","iframe","img","style","layer","link","ilayer","meta","object","applet"};
            return WKT.Common.Utils.TextHelper.TagClean(source, tags);
        }

        /// <summary>
        /// html内容安全过滤
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string HtmlFilter(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return "";
            string[] tags = new string[] { "body", "embed", "frame", "script", "frameset", "html", "iframe","layer", "link", "ilayer", "meta", "object", "applet" };
            return WKT.Common.Utils.TextHelper.TagClean(source, tags);
        }


        #endregion

    }
}
