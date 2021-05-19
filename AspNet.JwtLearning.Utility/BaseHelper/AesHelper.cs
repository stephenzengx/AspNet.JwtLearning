using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AspNet.JwtLearning.Utility
{
    //aes加密 取代md5加密 (加密解密密码？) 手动实现aes
    public static class AesHelper
    {
        // set permutations
        private const string strPermutation = "ouiveyxaqtd";
        private const int bytePermutation1 = 0x19;
        private const int bytePermutation2 = 0x59;
        private const int bytePermutation3 = 0x17;
        private const int bytePermutation4 = 0x41;

        private static readonly PasswordDeriveBytes passbytes = new PasswordDeriveBytes(
            strPermutation,
            new byte[] { bytePermutation1, bytePermutation2, bytePermutation3, bytePermutation4 }
        );

        private static Aes aes;

        static AesHelper()
        {
            aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Encrypt(string data)
        {
            return Convert.ToBase64String(EncryptData(Encoding.UTF8.GetBytes(data)));
            // reference https://msdn.microsoft.com/en-us/library/ds4kkd55(v=vs.110).aspx
        }

        private static byte[] EncryptData(byte[] byteData)
        {
            using (MemoryStream memstream = new MemoryStream())
            {
                CryptoStream cryptostream = new CryptoStream(memstream,aes.CreateEncryptor(), CryptoStreamMode.Write);
                cryptostream.Write(byteData, 0, byteData.Length);
                cryptostream.FlushFinalBlock();
                cryptostream.Close();

                return memstream.ToArray();
            };
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Decrypt(string data)
        {
            return Encoding.UTF8.GetString(DecryptData(Convert.FromBase64String(data)));
            // reference https://msdn.microsoft.com/en-us/library/system.convert.frombase64string(v=vs.110).aspx
        }

        private static byte[] DecryptData(byte[] byteData)
        {
            using (MemoryStream memstream = new MemoryStream())
            {
                CryptoStream cryptostream = new CryptoStream(memstream, aes.CreateDecryptor(), CryptoStreamMode.Write);
                cryptostream.Write(byteData, 0, byteData.Length);
                cryptostream.FlushFinalBlock();
                cryptostream.Close();

                return memstream.ToArray();
            };
        }
    }

}