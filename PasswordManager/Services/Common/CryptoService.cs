using System;
using System.IO;
using System.Security.Cryptography;
using PasswordManager.Interfaces.Common;
using PasswordManager.Models.Util;

namespace PasswordManager.Services.Common
{
    public class CryptoService : ICryptoService
    {
        private const int BlockSize = 128;
        private const int KeySize = 128;

        private static readonly CryptoResult ResultError = new() { IsSuccess = false };
        private static readonly byte[] Salt = { 0x0, 0x20, 0x11, 0x27, 0x3A, 0xB4,
                                    0x57, 0xC6, 0xF1, 0xF0, 0xEE, 0x21, 0x22, 0x45 };

        public CryptoResult EncryptString(string password, string plainText)
        {
            try
            {
                byte[] array;

                using (Aes aes = Aes.Create())
                {
                    aes.Padding = PaddingMode.ANSIX923;

                    // Generate the key and initialization vector.
                    MakeKeyAndIV(password, Salt, KeySize,
                        BlockSize, out byte[] key, out byte[] iv);

                    ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);

                    using MemoryStream memoryStream = new();
                    using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                    using (StreamWriter streamWriter = new(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }

                return new CryptoResult
                {
                    IsSuccess = true,
                    Data = Convert.ToBase64String(array)
                };
            }
            catch
            {
                return ResultError;
            }
        }

        public CryptoResult DecryptString(string password, string cipherText)
        {
            try
            {
                //byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);

                using Aes aes = Aes.Create();
                aes.Padding = PaddingMode.ANSIX923;

                // Generate the key and initialization vector.
                MakeKeyAndIV(password, Salt, KeySize,
                    BlockSize, out byte[] key, out byte[] iv);

                ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);

                using MemoryStream memoryStream = new(buffer);
                using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new(cryptoStream);
                return new CryptoResult
                {
                    IsSuccess = true,
                    Data = streamReader.ReadToEnd()
                };
            }
            catch
            {
                return ResultError;
            }
        }

        // Use the password to generate key bytes.
        private static void MakeKeyAndIV(string password, byte[] salt,
            int key_size_bits, int block_size_bits,
            out byte[] key, out byte[] iv)
        {
            var derive_bytes =
                new Rfc2898DeriveBytes(password, salt, 1000);

            key = derive_bytes.GetBytes(key_size_bits / 8);
            iv = derive_bytes.GetBytes(block_size_bits / 8);
        }
    }
}
