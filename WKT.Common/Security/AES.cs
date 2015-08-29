using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace WKT.Common.Security
{
    public class AES
    {
        #region 获取密钥/向量
        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string Key
        {
            get { return "wkzlc%$&9o@(08sx"; }
        }

        /// <summary>
        /// 获取向量
        /// </summary>
        private static string IV
        {
            get { return @"L+\~f4,Ir)b$=pkf"; }
        } 
        #endregion

        #region AESEncrypt-AES加密
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string plainStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIV = Encoding.UTF8.GetBytes(IV);
            byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

            string encrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return encrypt;
        } 
        #endregion

        #region AESEncrypt-AES加密-加密失败时是否返回 null
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <param name="returnNull">加密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string plainStr, bool returnNull)
        {
            string encrypt = AESEncrypt(plainStr);
            return returnNull ? encrypt : (encrypt == null ? String.Empty : encrypt);
        } 
        #endregion

        #region AESDecrypt-AES解密
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(string encryptStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIV = Encoding.UTF8.GetBytes(IV);
            byte[] byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return decrypt;
        } 
        #endregion

        #region AESDecrypt-AES解密-解密失败时是否返回 null
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <param name="returnNull">解密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(string encryptStr, bool returnNull)
        {
            string decrypt = AESDecrypt(encryptStr);
            return returnNull ? decrypt : (decrypt == null ? String.Empty : decrypt);
        } 
        #endregion

        //=====AES加密/解密(使用自定义Key)========
        
        #region AES加密/ECB/NoPadding
        /// <summary>
        /// AES加密/ECB/NoPadding
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
            Byte[] tmpArray = { };
            //key不足16位时用1补齐
            if (key.Length % 16 != 0)
            {
                int keyLen = 16 - (key.Length % 16);
                for (int i = 0; i < keyLen; i++)
                {
                    key += "1";
                }
            }
            //加密数据不足16位时用0补齐
            if (toEncryptArray.Length % 16 != 0)
            {
                int len = 16 - (toEncryptArray.Length % 16);
                tmpArray = Enumerable.Repeat((byte)0, len).ToArray();
            }
            toEncryptArray = toEncryptArray.Concat(tmpArray).ToArray();
            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.None
            };
            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string AesEncrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
            Byte[] tmpArray = { };
            
            //加密数据不足16位时用0补齐
            if (toEncryptArray.Length % 16 != 0)
            {
                int len = 16 - (toEncryptArray.Length % 16);
                tmpArray = Enumerable.Repeat((byte)0, len).ToArray();
            }
            toEncryptArray = toEncryptArray.Concat(tmpArray).ToArray();
            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(Key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.None
            };
            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        } 
        #endregion

        #region AES解密/ECB/NoPadding
        /// <summary>
        /// AES解密/ECB/NoPadding
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);
            //key不足16位时用1补齐
            if (key.Length % 16 != 0)
            {
                int keyLen = 16 - (key.Length % 16);
                for (int i = 0; i < keyLen; i++)
                {
                    key += "1";
                }
            }
            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.None
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray).Replace("\0", "");
        }

        public static string AesDecrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);
            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(Key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.None
            };
            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray).Replace("\0", "");
        } 
        #endregion

    }
}
