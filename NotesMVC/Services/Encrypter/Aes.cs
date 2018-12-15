using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NotesMVC.Services.Encrypter {
    public class AesCryptograph : ICryptograph {

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// Decrypt text.
        /// </summary>
        /// <param name="encodedText"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public string Decrypt(byte[] encodedText, string secretKey) {

            using (var aes = Aes.Create()) {

                aes.Padding = PaddingMode.PKCS7;

                GetKeyAndIV(secretKey, out var key, out var iv);

                aes.Key = key;
                aes.IV = iv;

                var decryptor = aes.CreateDecryptor();

                using (var memStrem = new MemoryStream(encodedText)) {
                    using (var cryptoStream = new CryptoStream(memStrem, decryptor, CryptoStreamMode.Read)) {
                        using (var srReader = new StreamReader(cryptoStream)) {

                            return srReader.ReadToEnd();

                        }

                    }

                }

            }

        }

        /// <summary>
        /// Encrypt text.s
        /// </summary>
        /// <param name="text"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public byte[] Encrypt(string text, string secretKey) {

            using (var aes = Aes.Create()) {

                aes.Padding = PaddingMode.PKCS7;

                GetKeyAndIV(secretKey, out var key, out var iv);

                aes.Key = key;
                aes.IV = iv;

                var encoder = aes.CreateEncryptor();

                using (var memStream = new MemoryStream()) {

                    using (var csStream = new CryptoStream(memStream, encoder, CryptoStreamMode.Write)) {

                        using (var writeStream = new StreamWriter(csStream)) {
                            writeStream.Write(text);
                        }

                    }

                    return memStream.ToArray();

                }

            }

        }

        /// <summary>
        /// Generate secret key.
        /// </summary>
        /// <returns></returns>
        public string GenerateSecretKey() {

            using (var aes = Aes.Create()) {

                var minSizeKey = aes.LegalKeySizes[0].MinSize / 8;
                var minSizeIV = aes.BlockSize / 8;

                var key = new char[minSizeKey];
                var iv = new char[minSizeIV];

                var random = new Random();

                for (var i = 0; i < minSizeKey; i++) {
                    key[i] = chars[random.Next(0, chars.Length)];
                }

                for (var i = 0; i < minSizeIV; i++) {
                    iv[i] = chars[random.Next(0, chars.Length)];
                }

                return new string(key) + new string(iv);

            }

        }

        /// <summary>
        /// Get key and initial vector(iv)
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        protected void GetKeyAndIV(string secretKey, out byte[] key, out byte[] iv) {

            var reverseSecretKey = new string(secretKey.Reverse().ToArray());

            using (var md5 = MD5.Create()) {

                key = md5.ComputeHash(Encoding.ASCII.GetBytes(secretKey));
                iv = md5.ComputeHash(Encoding.ASCII.GetBytes(reverseSecretKey));

            }

        }

    }
}
