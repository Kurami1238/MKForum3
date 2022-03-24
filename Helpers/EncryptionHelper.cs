using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MKForum.Helpers
{
    public class EncryptionHelper
    {

        //加密
        public byte[] AesEncrypt(string plainText, byte[] Key, byte[] IV)
        {
            // 驗證參數
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // 產生加密器
                ICryptoTransform encryptor =
                    aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt =
                       new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        //解密
        public string AesDecrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // 建立解碼器
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }



        //============================雜湊==================================

        public string HashPasswordkey()
        {
            return "sd9DSFs8SDdsf8df9SFf&SF98";
        }

        /// <summary> 產生 32 bytes 隨機鹽 </summary>
        /// <returns></returns>
        public byte[] BuildNewSalt()
        {
            byte[] randBytes = new byte[32];
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
            rand.GetBytes(randBytes);
            return randBytes;
        }


        /// <summary> 密碼進行雜湊 </summary>
        /// <param name="pwd">密碼</param>
        /// <param name="key">金鑰</param>
        /// <param name="salt">鹽</param>
        /// <returns></returns>
        public byte[] GetHashPassword(string pwd, string key, byte[] salt)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(pwd);
            byte[] totalBytes = salt.Union(pwdBytes).ToArray();

            HMACSHA512 hmacsha512 = new HMACSHA512(keyByte);
            byte[] hashPwd = hmacsha512.ComputeHash(totalBytes);
            return hashPwd;
        }

    }

}