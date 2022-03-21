using System;
using PasswordManager.Models.Util;

namespace PasswordManager.Interfaces.Common
{
    public interface ICryptoService
    {
        /// <summary>
        /// Encrypt a plainText with the masterpasword
        /// </summary>
        /// <param name="password"></param>
        /// <param name="plainText"></param>
        /// <returns></returns>
        CryptoResult EncryptString(string password, string plainText);

        /// <summary>
        /// Decyrpt a cipher text with the password provided
        /// </summary>
        /// <param name="password"></param>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        CryptoResult DecryptString(string password, string cipherText);
    }
}
