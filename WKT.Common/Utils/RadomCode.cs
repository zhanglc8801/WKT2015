using System;
using System.Collections.Generic;
using System.Text;

namespace WKT.Common.Utils
{
    /// <summary>
    /// �������
    /// </summary>
    public class RadomCode
    {

        private static string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string numbers = "0123456789";
        private static string letnums = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();

        #region GenerateCode
        /// <summary>
        /// �õ�һ������Ĵ�(��������)
        /// </summary>
        /// <param name="length">���������</param>
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
        /// �õ�һ������Ĵ�(ֻ������)
        /// </summary>
        /// <param name="length">���������</param>
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
        /// �õ�һ������Ĵ�(ֻ����ĸ)
        /// </summary>
        /// <param name="length">���������</param>
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
