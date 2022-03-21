using System;
using PasswordManager.Interfaces.Common;
using PasswordManager.Interfaces.Infrastructure;
using PasswordManager.Interfaces.VM;
using PasswordManager.Models.Entity;

namespace PasswordManager.Services.VM
{
    public class SetMasterPasswordPageVMService : ISetMasterPasswordPageVMService
    {
        #region private fields

        private readonly IDatabaseClient _databaseClient;
        private readonly ICryptoService _cryptoService;
        private readonly IPreferenceService _preferenceService;

        #endregion
        public SetMasterPasswordPageVMService
        (
            IDatabaseClient databaseClient,
            ICryptoService cryptoService,
            IPreferenceService preferenceService
        )
        {
            _databaseClient = databaseClient;
            _cryptoService = cryptoService;
            _preferenceService = preferenceService;
        }

        #region public methods

        public bool SetMasterPassword(string masterPassword)
        {
            try
            {
                //create a demo login item and encypt the demologin item password with master password
                //Master password is not saved
                //Encrypted dummy LoginItem is used to validate the master password
                var dummyLoginItemPassword = Guid.NewGuid().ToString();

                var encrytpedDummyPassword = _cryptoService.EncryptString(masterPassword, dummyLoginItemPassword);

                if (!encrytpedDummyPassword.IsSuccess || string.IsNullOrEmpty(encrytpedDummyPassword.Data))
                    return false;

                var demoLoginItem = new LoginItem
                {
                    IsDummyItem = true,
                    Password = encrytpedDummyPassword.Data
                };

                var dbResult = _databaseClient.AddOrUpdate(demoLoginItem);

                if (!dbResult.IsSuccess)
                    return false;

                //set IsMasterPasswordSet preference to true
                _preferenceService.IsMasterPasswordSet = true;

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
