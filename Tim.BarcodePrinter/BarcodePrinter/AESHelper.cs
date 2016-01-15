using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Tim.BarcodePrinter
{
    /// <summary>
    /// AES 对称加密算法
    /// </summary>
    public static class AESHelper
    {
        private static string key = "A8E7B757-9625-467b-8E2A-7837F1D17B79";

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptedString">密文</param>
        /// <param name="key">key</param>
        /// <returns>解密结果</returns>
        public static string Decrypt(string encryptedString)
        {
            byte[] keyArray = ShortMD5(key);
            byte[] encryptArray = Convert.FromBase64String(encryptedString);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(encryptArray, 0, encryptArray.Length);

            string data = Encoding.UTF8.GetString(resultArray);

            return data;
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="data">明文</param>
        /// <param name="key">key</param>
        /// <returns>加密结果</returns>
        public static string Encrypt(string data)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(data);
            byte[] keyBytes = ShortMD5(key);
            Aes kgen = Aes.Create("AES");
            kgen.Mode = CipherMode.ECB;
            kgen.Padding = PaddingMode.PKCS7;
            kgen.Key = keyBytes;
            ICryptoTransform cTransform = kgen.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            string encryptedString = Convert.ToBase64String(resultArray);

            return encryptedString;
        }

        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static byte[] ShortMD5(string key)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] b = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            return b;
        }

    }
}
