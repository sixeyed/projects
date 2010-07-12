using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sixeyed.EC2Launcher.Cryptography
{
    /// <summary>
    /// Wrapper class for encrypting and descrypting string values
    /// </summary>
    /// <remarks>
    /// Uses <see cref="RijndaelManaged"/> provider, defaulting to SHA1 hash,
    /// 256-bit key size, fixed passphrase, IV and salt
    /// </remarks>
    public static class CryptoProvider
    {
        /// <summary>
        /// Encrypts the given plain text string
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="passPhrase"></param>
        /// <param name="saltValue"></param>
        /// <param name="passwordIterations"></param>
        /// <param name="initVector"></param>
        /// <param name="keySize"></param>
        /// <param name="hashAlgorithm"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText,
                                     string passPhrase = "3745ed5a-1998-4ba7-a626-c6e272e3a5c0",
                                     string saltValue = "bf9c7fcf-c81e-4516-98ef-0a8079dab59d",
                                     int passwordIterations = 2,
                                     string initVector = "@1B2c3D4e5F6g7H8",
                                     int keySize = 256,
                                     string hashAlgorithm = "SHA1")
        {
            var transform = GetTransform(true, passPhrase, saltValue, passwordIterations, initVector, keySize, hashAlgorithm);
            
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }        

        /// <summary>
        /// Decrypts the given cipher text string
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="passPhrase"></param>
        /// <param name="saltValue"></param>
        /// <param name="passwordIterations"></param>
        /// <param name="initVector"></param>
        /// <param name="keySize"></param>
        /// <param name="hashAlgorithm"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText,
                                 string passPhrase = "3745ed5a-1998-4ba7-a626-c6e272e3a5c0",
                                 string saltValue = "bf9c7fcf-c81e-4516-98ef-0a8079dab59d",
                                 int passwordIterations = 2,
                                 string initVector = "@1B2c3D4e5F6g7H8",
                                 int keySize = 256,
                                 string hashAlgorithm = "SHA1")
        {
            var transform = GetTransform(false, passPhrase, saltValue, passwordIterations, initVector, keySize, hashAlgorithm);

            var cipherTextBytes = Convert.FromBase64String(cipherText);
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();

            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            return plainText;
        }

        private static ICryptoTransform GetTransform(bool encrypt, string passPhrase, string saltValue, int passwordIterations, string initVector, int keySize, string hashAlgorithm)
        {
            var initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            var saltValueBytes = Encoding.UTF8.GetBytes(saltValue);

            var password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
            var keyBytes = password.GetBytes(keySize / 8);

            var symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            if (encrypt)
                return symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            else
                return symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
        }
    }
}
