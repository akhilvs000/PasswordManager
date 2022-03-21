using System;
using System.Linq;
using PasswordManager.Interfaces.Common;
using PasswordManager.Interfaces.Infrastructure;
using PasswordManager.Interfaces.VM;
using PasswordManager.Models.Entity;

namespace PasswordManager.Services.VM
{
    public class LoginPageVMService : ILoginPageVMService
    {
        #region private fields

        private readonly ICryptoService _cryptoService;
        private IDatabaseClient _databaseClient;

        #endregion

        public LoginPageVMService
        (
            ICryptoService cryptoService,
            IDatabaseClient databaseClient
        )
        {
            _cryptoService = cryptoService;
            _databaseClient = databaseClient;
        }

        #region public methods

        public bool Login(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                    return false;

                var dummyLoginitem = _databaseClient.GetAll<LoginItem>()?
                                .Data?.FirstOrDefault(x => x.IsDummyItem);

                //return false if no dummy login item
                if (dummyLoginitem is null)
                    return false;

                //decrypt dummy login password with entered password
                var decryptedItem = _cryptoService.DecryptString(password, dummyLoginitem.Password);

                //if decryption fails, means the password entered is not the
                // password used for encryption for dummy login item
                if (!decryptedItem.IsSuccess || string.IsNullOrEmpty(decryptedItem.Data))
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
