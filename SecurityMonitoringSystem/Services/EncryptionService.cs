using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecurityMonitoringSystem.Services
{
    public class EncryptionService
    {
        // 256-bit key for AES
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("A1B2C3D4E5F678901234567890ABCDEF");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("A1B2C3D4E5F67890");

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var msDecrypt = new MemoryStream(cipherBytes))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch
            {
                throw new CryptographicException("Cipher text is invalid or corrupted.");
            }
        }
    }
}
