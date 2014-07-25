using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
namespace MarvellousWorks.PracticalPattern.Common
{
    public static class CryptoHelper
    {
        #region Private field
        /// <summary>
        /// Hash algorithm
        /// </summary>
        private static SHA1 sha;

        /// <summary>
        /// Crypto transformer of encryption
        /// </summary>
        private static ICryptoTransform et;

        /// <summary>
        /// Crypto transformer of decryption
        /// </summary>
        private static ICryptoTransform dt;
        #endregion

        #region Constructor
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <remarks>�޲����Ĺ��캯������Ĭ�ϵķ�ʽ��ʼ���������ԡ�
        /// </remarks>
        static CryptoHelper()
        {
            CryptoHelper.sha = new SHA1CryptoServiceProvider();
            DESCryptoServiceProvider cryptoService = new DESCryptoServiceProvider();
            byte[] cryptoKey = { 136, 183, 142, 217, 175, 71, 90, 239 };
            byte[] cryptoIV = { 227, 105, 5, 40, 162, 158, 143, 156 };


            cryptoService.Key = cryptoKey;
            cryptoService.IV = cryptoIV;

            CryptoHelper.et = cryptoService.CreateEncryptor();
            CryptoHelper.dt = cryptoService.CreateDecryptor();
        }
        #endregion

        #region hash / comparehash
        /// <overrides>
        /// Computes the hash value of plain text using the given hash provider instance
        /// </overrides>
        /// <summary>
        /// Computes the hash value of plain text using the given hash provider instance
        /// </summary>
        /// <param name="plaintext">The input for which to compute the hash.</param>
        /// <returns>The computed hash code.</returns>
        private static byte[] CreateHash(byte[] plaintext)
        {
            return CryptoHelper.sha.ComputeHash(plaintext);
        }

        /// <summary>
        /// Ϊ���ı��ַ��������ϣֵ
        /// </summary>
        /// <param name="plaintext">��Ҫ�����ϣֵ�Ĵ��ı��ַ���</param>
        /// <returns>������Ĺ�ϣֵ</returns>
        /// <remarks>
        /// Ϊ���ı��ַ��������ϣֵ��
        /// </remarks>
        public static string CreateHash(string plaintext)
        {
            byte[] plainTextBytes = UnicodeEncoding.Unicode.GetBytes(plaintext);
            byte[] resultBytes = CreateHash(plainTextBytes);

            return Convert.ToBase64String(resultBytes);
        }

        /// <overrides>
        /// Compares plain text input with a computed hash using the given hash provider instance.
        /// </overrides>
        /// <summary>
        /// Compares plain text input with a computed hash using the given hash provider instance.
        /// </summary>
        /// <remarks>
        /// Use this method to compare hash values. Since hashes may contain a random "salt" value, two seperately generated
        /// hashes of the same plain text may result in different values. 
        /// </remarks>
        /// <param name="plaintext">The input for which you want to compare the hash to.</param>
        /// <param name="hashedText">The hash value for which you want to compare the input to.</param>
        /// <returns><c>true</c> if plainText hashed is equal to the hashedText. Otherwise, <c>false</c>.</returns>
        private static bool HashCheck(byte[] plaintext, byte[] hashedText)
        {
            if ((hashedText == null) || (hashedText.Length <= 0))
                return false;

            byte[] hashedResult = CryptoHelper.sha.ComputeHash(plaintext);
            if (hashedText.Length != hashedResult.Length)
                return false;

            for (int i = 0; i < hashedResult.Length; i++)
            {
                if (hashedText[i] != hashedResult[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// �Ƚ��ɴ��ı���������Ĺ�ϣֵ������Ĺ�ϣֵ�Ƿ�һ�¡�
        /// </summary>
        /// <param name="plaintext">������Ҫ���й�ϣ�Ƚϵ��ַ���</param>
        /// <param name="hashedText">��ϣ��</param>
        /// <returns><c>true</c> ������ı���ϣֵ������Ĺ�ϣֵ��ȣ����򷵻�<c>false</c>.</returns>
        /// <remarks>
        /// ��������������Ƚ��ɴ��ı�������Ĺ�ϣֵ�͸����Ĺ�ϣֵ�Ƿ�һ�¡�
        /// </remarks>
        public static bool HashCheck(string plaintext, string hashedText)
        {
            byte[] plainTextBytes = UnicodeEncoding.Unicode.GetBytes(plaintext);
            byte[] hashedTextBytes = Convert.FromBase64String(hashedText);

            bool result = HashCheck(plainTextBytes, hashedTextBytes);

            return result;
        }
        #endregion

        #region nvative key encrypt / decrypt

        /// <summary>
        /// ���ַ����Ľ��ܴ���
        /// </summary>
        /// <param name="source">��Ҫ���ܵ��ַ���</param>
        /// <returns>���ܺ���ַ���</returns>
        /// <remarks>���ַ����Ľ��ܴ���
        /// </remarks>
        public static string Decrypt(string source)
        {
            byte[] buff = Convert.FromBase64String(source);
            using (MemoryStream mem = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(mem, CryptoHelper.dt, CryptoStreamMode.Write))
                {
                    stream.Write(buff, 0, buff.Length);
                    stream.Close();
                }
                return Encoding.Unicode.GetString(mem.ToArray());
            }
        }

        /// <summary>
        /// ���ַ����ļ��ܴ���
        /// </summary>
        /// <param name="source">��Ҫ���ܵ��ַ���</param>
        /// <returns>���ܺ���ַ���</returns>
        /// <remarks>���ַ����ļ��ܴ���
        /// </remarks>
        public static string Encrypt(string source)
        {
            byte[] buff = Encoding.Unicode.GetBytes(source);

            MemoryStream mem = new MemoryStream();
            CryptoStream stream = new CryptoStream(mem, CryptoHelper.et, CryptoStreamMode.Write);
            stream.Write(buff, 0, buff.Length);
            stream.FlushFinalBlock();
            stream.Clear();

            return Convert.ToBase64String(mem.ToArray());
        }
        #endregion

        #region encode / decode
        private const string CodePageName = "utf-32";

        /// <summary>
        /// ���ַ�������Base64����
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Encode(string source)
        {
            byte[] buffer = Encoding.GetEncoding(CodePageName).GetBytes(source);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// ���ַ�������Base64����
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Decode(string source)
        {
            byte[] buffer = Convert.FromBase64String(source);
            return Encoding.GetEncoding(CodePageName).GetString(buffer);
        }
        #endregion
    }
}