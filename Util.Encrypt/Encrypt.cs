using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace Util.Encrypt
{
    public class Encrypt
    {
        //private static string key = ConfigurationManager.AppSettings.Get("KeySalary");
        private static string key = "3IRDsCEqL5dadhrs";
        public static string EncryptSalary(string salary)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] salaryBytes = Encoding.UTF8.GetBytes(salary);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.GenerateIV();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(salaryBytes, 0, salaryBytes.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    byte[] encryptedSalaryBytes = msEncrypt.ToArray();
                    return Convert.ToBase64String(encryptedSalaryBytes);
                }
            }
        }

        public static string DecryptSalary(string encryptedSalary)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] encryptedSalaryBytes = Convert.FromBase64String(encryptedSalary);

            using (Aes aesAlg = Aes.Create())
            {
                byte[] iv = new byte[aesAlg.IV.Length];
                Buffer.BlockCopy(encryptedSalaryBytes, 0, iv, 0, aesAlg.IV.Length);

                aesAlg.Key = keyBytes;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new System.IO.MemoryStream())
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        csDecrypt.Write(encryptedSalaryBytes, aesAlg.IV.Length, encryptedSalaryBytes.Length - aesAlg.IV.Length);
                        csDecrypt.FlushFinalBlock();
                    }
                    byte[] salaryBytes = msDecrypt.ToArray();
                    return Encoding.UTF8.GetString(salaryBytes, 0, salaryBytes.Length);
                }
            }
        }
    }
}
