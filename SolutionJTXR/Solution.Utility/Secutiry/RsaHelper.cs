using System;
using System.Security.Cryptography;
using System.Text;

namespace Solution.Utility.Secutiry
{
    /// <summary>
    /// RSA加密解密操作类
    /// </summary>
    public class RsaHelper
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public static string Encryption(string express)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = "yuxianye@Solution2018";//密匙容器的名称，保持加密解密一致才能解密成功
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] plaindata = Encoding.Default.GetBytes(express);//将要加密的字符串转换为字节数组
                byte[] encryptdata = rsa.Encrypt(plaindata, false);//将加密后的字节数据转换为新的加密字节数组
                return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为字符串
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public static string Decrypt(string ciphertext)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = "yuxianye@Solution2018";
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] encryptdata = Convert.FromBase64String(ciphertext);
                byte[] decryptdata = rsa.Decrypt(encryptdata, false);
                return Encoding.Default.GetString(decryptdata);
            }
        }
    }
}