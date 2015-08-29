using System;
using System.Collections.Generic;
using System.Text;

namespace WKT.Common.Utils
{
    /// <summary>
    /// 随机代码
    /// </summary>
    public class RadomCode
    {

        private static string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string numbers = "0123456789";
        private static string letnums = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();

        #region GenerateCode
        /// <summary>
        /// 得到一个随机的串(带有数字)
        /// </summary>
        /// <param name="length">随机串长度</param>
        /// <returns></returns>
        public static string GenerateCode(int length)
        {
            StringBuilder result = new StringBuilder();
            int maxValue = letnums.Length;

            for (int i = 0; i < length; i++)
            {
                result.Append(letnums[random.Next(maxValue)]);
            }
            return result.ToString();
        }
        #endregion

        #region GenerateNumberCode
        /// <summary>
        /// 得到一个随机的串(只有数字)
        /// </summary>
        /// <param name="length">随机串长度</param>
        /// <returns></returns>
        public static string GenerateNumberCode(int length)
        {
            StringBuilder result = new StringBuilder();
            int maxValue = numbers.Length;

            for (int i = 0; i < length; i++)
            {
                result.Append(numbers[random.Next(maxValue)]);
            }
            return result.ToString();
        }
        #endregion

        #region GenerateLetterCode

        /// <summary>
        /// 得到一个随机的串(只有字母)
        /// </summary>
        /// <param name="length">随机串长度</param>
        /// <returns></returns>
        public static string GenerateLetterCode(int length)
        {
            StringBuilder result = new StringBuilder();
            int maxValue = letters.Length;

            for (int i = 0; i < length; i++)
            {
                result.Append(letters[random.Next(maxValue)]);
            }
            return result.ToString();
        }

        #endregion
    }
}
