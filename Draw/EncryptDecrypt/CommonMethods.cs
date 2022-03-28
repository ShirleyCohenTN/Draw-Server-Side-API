using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

namespace Draw.EncryptDecrypt
{
    public static class CommonMethods
    {


        //SHA245 Cryptographically
        public static string HashString(string passwordString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(passwordString))
                sb.Append(b.ToString("X3"));
            return sb.ToString();
        }

        public static byte[] GetHash(string passwordString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(passwordString));
        }







        //-----------------------------------------------------------------------------
        //MD5 Cryptographically - currently not in use, we use SHA 256

        public static string hash = ConfigurationManager.AppSettings["hash"];

        public static string ConvertToEncrypt(string password)
        {
            //converting the string to byte array
            byte[] data = UTF8Encoding.UTF8.GetBytes(password);
            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                //the ComputeHash methods of MD5CryptoServiceProvider class returns the hash as an array of 16 bytes
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));

                //create a new TripleDESCryptoServiceProvider object to generate a key and initialization vector 
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7})
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    var encrypted_password = Convert.ToBase64String(results, 0, results.Length);
                    return encrypted_password;
                }
            }
        }



        public static string ConvertToDecrypt(string base64EncodeData)
        {
            byte[] data = Convert.FromBase64String(base64EncodeData);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));

                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();//changing to .CreateDecryptor()
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    var decrypted_password = UTF8Encoding.UTF8.GetString(results); // converting the byte array to string
                    return decrypted_password;
                }
            }
        }



 

    }
}