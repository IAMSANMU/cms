using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Imow.Framework.Tool
{
    public class EncryptionHelper
    {
        /// <summary>
        /// MD5加密 32位小写
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string En32MD5(string text)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(text);
            byte[] hs = md5.ComputeHash(bs);
            string str = "";
            for (int i = 0; i < hs.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                str += Convert.ToString(hs[i], 16).PadLeft(2, '0');

            }
            return str.PadLeft(32, '0').ToLower();
        }
        /// <returns>32位字符串</returns>
        public static String GetMD5(String orgValue)
        {
            return GetMD5(orgValue, Encoding.UTF8);
        }

        /// <returns>32位字符串</returns>
        public static String GetMD5(String orgValue, Encoding encode)
        {
            string result = "";

            //Create 
            var md5 = MD5.Create();

            //注意编码UTF8、UTF7、Unicode等的选择 
            byte[] bytResult = md5.ComputeHash(encode.GetBytes(orgValue));

            //字节类型的数组转换为字符串 
            for (int i = 0; i < bytResult.Length; i++)
            {
                //16进制转换 
                result += string.Format("{0:x}", bytResult[i]).PadLeft(2, '0');
            }

            return result;
        }

        /// <returns>64位字符串</returns>
        public static String GetSHA256(String orgValue)
        {
            return GetSHA256(orgValue, Encoding.UTF8);
        }

        /// <returns>64位字符串</returns>
        public static String GetSHA256(String orgValue, Encoding encode)
        {
            var sha256 = new SHA256Managed();
            var bytResult = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(orgValue));
            var result = "";
            //字节类型的数组转换为字符串 
            for (int i = 0; i < bytResult.Length; i++)
            {
                //16进制转换 
                result += string.Format("{0:x}", bytResult[i]).PadLeft(2, '0');
            }

            return result;
        }

        /// <returns>64位字符串</returns>
        public static string GetMD5AndSHA256(String orgValue)
        {
            var md5 = GetMD5(orgValue);

            return GetSHA256(md5);
        }

        /// <summary>
        /// 生成MD5摘要
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static byte[] GetMD5(byte[] original)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyhash = hashmd5.ComputeHash(original);
            hashmd5 = null;
            return keyhash;
        }

        /// <summary>
        /// 数据加密
        /// </summary>
        /// <param name="original">数据源</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] original, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = GetMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        /// <summary>
        /// 数据加密
        /// </summary>
        /// <param name="original">数据源</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original, string key)
        {
            byte[] buff = System.Text.Encoding.Default.GetBytes(original);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }

        /// <summary>
        /// 使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = GetMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        /// <summary>
        /// 使用给定密钥字符串解密string,返回指定编码方式明文
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            byte[] buff = Convert.FromBase64String(encrypted);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }

        /// <summary>
        /// 使用给定密钥字符串解密string
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encrypted, string key)
        {
            return Decrypt(encrypted, key, Encoding.Default);
        }

        /// <summary>
        /// 生成MD5码,生成的数据同Encrypt(original,1)
        /// </summary>
        /// <param name="original"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetMD5(string original, string encoding)
        {
            MD5 hashmd5 = new MD5CryptoServiceProvider();
            byte[] byteOriginal = hashmd5.ComputeHash(Encoding.GetEncoding(encoding).GetBytes(original));
            StringBuilder ciphertext = new StringBuilder(32);
            for (int i = 0; i < byteOriginal.Length; i++)
            {
                ciphertext.Append(byteOriginal[i].ToString("x").PadLeft(2, '0'));
            }
            return ciphertext.ToString();
        }

        #region 异或加解密

        /// <summary>
        /// 使用异或方法进行加密
        /// </summary>
        public static string XOREncrypt(string original, string key = "this is key")
        {
            var code = "";
            var keyLength = key.Length;

            for (int i = 0; i < original.Length; i += keyLength)
            {
                var factLength = (i + keyLength) > original.Length ? (original.Length - i) : keyLength;
                code += XOR(original.Substring(i, factLength), key, true);
            }
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(code));

            return base64;
        }
        /// <summary>
        /// 使用异或方法进行解密
        /// </summary>
        public static string XORDecrypt(string original, string key = "this is key")
        {
            var code = "";
            var keyLength = key.Length;
            original = System.Text.Encoding.Default.GetString(Convert.FromBase64String(original));
            for (int i = 0; i < original.Length; i += keyLength)
            {
                var factLength = (i + keyLength) > original.Length ? (original.Length - i) : keyLength;
                code += XOR(original.Substring(i, factLength), key, true);
            }

            return code;
        }

        private static String XOR(String a, String b, bool truncation)
        {
            var aBytes = Encoding.UTF8.GetBytes(a);
            var bBytes = Encoding.UTF8.GetBytes(b);

            int len = aBytes.Length > bBytes.Length ? bBytes.Length : aBytes.Length;
            byte[] cs = new byte[len];
            for (int i = 0; i < len; ++i)
            {
                cs[i] = (byte)((int)aBytes[i] ^ (int)bBytes[i]);
            }
            if (truncation)
            {
                return Encoding.UTF8.GetString(cs);
            }
            else
            {
                if (aBytes.Length > bBytes.Length)
                {
                    Array.Copy(cs, 0, aBytes, 0, len);
                    return Encoding.Default.GetString(aBytes);
                }
                else
                {
                    Array.Copy(cs, 0, bBytes, 0, len);
                    return Encoding.Default.GetString(bBytes);
                }
            }
        }

        #endregion
    }
}
