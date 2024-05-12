namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framework v4.5.2)

    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    #endregion Using System Library (.NET Framework v4.5.2)

    /// <summary>
    /// Advanced Encryption Standard (AES).
    /// </summary>
    public sealed class AES
    {
        /// <summary>
        /// The secret key to be used for the symmetric algorithm.
        /// The key size must be 128, 192, or 256 bits.
        /// </summary>
        private const int KeySize = 256;

        /// <summary>
        /// The initialization vector to be used for the symmetric algorithm.
        /// </summary>
        private const string InitVector = "1B2c3D4e5F6g7H8i"; /* 128 bits block size (16 bytes) */

        /// <summary>
        /// The number of iterations for the operation.
        /// </summary>
        private const int Iterations = 3;

        /// <summary>
        /// The name of the hash algorithm for the operation.
        /// </summary>
        private const string HashAlgorithm = "SHA-256";

        /// <summary>
        /// The key salt to use to derive the key.
        /// </summary>
        private const string KeySalt = "1001001100101100000001011010010"; /* binary */

        /// <summary>
        /// The password for which to derive the key.
        /// </summary>
        private const string PassPhrase = "499602D2"; /* hex */

        /// <summary>
        /// Standardization of input parameters.
        /// </summary>
        /// <param name="passPhrase"></param>
        /// <param name="keySalt"></param>
        /// <param name="hashAlgorithm"></param>
        /// <param name="initVector"></param>
        /// <param name="keySize"></param>
        /// <param name="iterations"></param>
        private static void ParametersStandardization(ref string passPhrase, ref string keySalt, ref string hashAlgorithm, ref string initVector, ref int? keySize, ref int? iterations)
        {
            passPhrase = passPhrase ?? PassPhrase;
            keySalt = keySalt ?? KeySalt;
            hashAlgorithm = hashAlgorithm ?? HashAlgorithm;
            initVector = initVector ?? InitVector;
            keySize = keySize ?? KeySize;
            iterations = iterations ?? Iterations;
        }

        /// <summary>
        /// AES - Encrypt function
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="passPhrase"></param>
        /// <param name="keySalt"></param>
        /// <param name="hashAlgorithm"></param>
        /// <param name="initVector"></param>
        /// <param name="keySize"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string passPhrase = null, string keySalt = null, string hashAlgorithm = null, string initVector = null, int? keySize = null, int? iterations = null)
        {
            try
            {
                // Standardization of input parameters.
                ParametersStandardization(ref passPhrase, ref keySalt, ref hashAlgorithm, ref initVector, ref keySize, ref iterations);
                // Convert strings into byte arrays.
                // Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8
                // encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(keySalt);
                // Convert our plaintext into a byte array.
                // Let us assume that plaintext contains UTF8-encoded characters.
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                // First, we must create a password, from which the key will be derived.
                // This password will be generated from the specified passphrase and
                // salt value. The password will be created using the specified hash
                // algorithm. Password creation can be done in several iterations.
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, iterations.Value);
                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(keySize.Value / 8);
                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged
                {
                    // It is reasonable to set encryption mode to Cipher Block Chaining
                    // (CBC). Use default options for other symmetric key parameters.
                    Mode = CipherMode.CBC
                };
                // Generate encryptor from the existing key bytes and initialization
                // vector. Key size will be defined based on the number of the key
                // bytes.
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream();
                // Define cryptographic stream (always use Write mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                // Start encrypting.
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                // Finish encrypting.
                cryptoStream.FlushFinalBlock();
                // Convert our encrypted data from a memory stream into a byte array.
                byte[] cipherTextBytes = memoryStream.ToArray();
                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();
                // Convert encrypted data into a base64-encoded string.
                return Convert.ToBase64String(cipherTextBytes);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// AES - The asynchronous version of <see cref="Encrypt"/>
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="passPhrase"></param>
        /// <param name="keySalt"></param>
        /// <param name="hashAlgorithm"></param>
        /// <param name="initVector"></param>
        /// <param name="keySize"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public static async Task<string> EncryptAsync(string plainText, string passPhrase = null, string keySalt = null, string hashAlgorithm = null, string initVector = null, int? keySize = null, int? iterations = null)
        {
            return await Task.Run(() => Encrypt(plainText, passPhrase, keySalt, hashAlgorithm, initVector, keySize, iterations));
        }

        /// <summary>
        /// AES - Decrypt function
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="passPhrase"></param>
        /// <param name="keySalt"></param>
        /// <param name="hashAlgorithm"></param>
        /// <param name="initVector"></param>
        /// <param name="keySize"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText, string passPhrase = null, string keySalt = null, string hashAlgorithm = null, string initVector = null, int? keySize = null, int? iterations = null)
        {
            try
            {
                // Standardization of input parameters.
                ParametersStandardization(ref passPhrase, ref keySalt, ref hashAlgorithm, ref initVector, ref keySize, ref iterations);
                // Convert strings defining encryption key characteristics into byte
                // arrays. Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8
                // encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(keySalt);
                // Convert our ciphertext into a byte array.
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                // First, we must create a password, from which the key will be
                // derived. This password will be generated from the specified
                // passphrase and salt value. The password will be created using
                // the specified hash algorithm. Password creation can be done in
                // several iterations.
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, iterations.Value);
                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(keySize.Value / 8);
                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged
                {
                    // It is reasonable to set encryption mode to Cipher Block Chaining
                    // (CBC). Use default options for other symmetric key parameters.
                    Mode = CipherMode.CBC
                };
                // Generate decryptor from the existing key bytes and initialization
                // vector. Key size will be defined based on the number of the key
                // bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                // Define memory stream which will be used to hold encrypted data.
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext.
                byte[] plainTextBytes = new byte[cipherTextBytes.Length + 1];
                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();
                // Convert decrypted data into a string.
                // Let us assume that the original plaintext string was UTF8-encoded.
                // Return decrypted string.
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// AES - The asynchronous version of <see cref="Decrypt"/>
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="passPhrase"></param>
        /// <param name="keySalt"></param>
        /// <param name="hashAlgorithm"></param>
        /// <param name="initVector"></param>
        /// <param name="keySize"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public static async Task<string> DecryptAsync(string cipherText, string passPhrase = null, string keySalt = null, string hashAlgorithm = null, string initVector = null, int? keySize = null, int? iterations = null)
        {
            return await Task.Run(() => Decrypt(cipherText, passPhrase, keySalt, hashAlgorithm, initVector, keySize, iterations));
        }
    }

    /// <summary>
    /// Triple Data Encryption Standard (3DES).
    /// </summary>
    public sealed class DES
    {
        /// <summary>
        /// The input to compute the hash code.
        /// </summary>
        private const string HashKey = "1001001100101100000001011010010";

        /// <summary>
        /// 3DES - Encrypt function
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string hashKey = null)
        {
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                using (var provider = new MD5CryptoServiceProvider())
                {
                    var keyBytes = provider.ComputeHash(Encoding.UTF8.GetBytes(hashKey ?? HashKey));
                    provider.Clear();
                    using (var triple = new TripleDESCryptoServiceProvider { Key = keyBytes, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        using (var transform = triple.CreateEncryptor())
                        {
                            var cipherTextBytes = transform.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
                            triple.Clear();
                            return Convert.ToBase64String(cipherTextBytes, 0, cipherTextBytes.Length);
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 3DES - The asynchronous version of <see cref="Encrypt"/>
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public static async Task<string> EncryptAsync(string plainText, string hashKey = null)
        {
            return await Task.Run(() => Encrypt(plainText, hashKey));
        }

        /// <summary>
        /// 3DES - Decrypt function
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText, string hashKey = null)
        {
            try
            {
                var cipherTextBytes = Convert.FromBase64String(cipherText);
                using (var provider = new MD5CryptoServiceProvider())
                {
                    var keyBytes = provider.ComputeHash(Encoding.UTF8.GetBytes(hashKey ?? HashKey));
                    provider.Clear();
                    using (var triple = new TripleDESCryptoServiceProvider { Key = keyBytes, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        using (var transform = triple.CreateDecryptor())
                        {
                            var plainTextBytes = transform.TransformFinalBlock(cipherTextBytes, 0, cipherTextBytes.Length);
                            triple.Clear();
                            return Encoding.UTF8.GetString(plainTextBytes);
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 3DES - The asynchronous version of <see cref="Decrypt"/>
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public static async Task<string> DecryptAsync(string cipherText, string hashKey = null)
        {
            return await Task.Run(() => Decrypt(cipherText, hashKey));
        }
    }
}