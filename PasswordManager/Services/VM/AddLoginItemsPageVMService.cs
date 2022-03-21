using System;
using PasswordManager.Interfaces.Common;
using PasswordManager.Interfaces.Infrastructure;
using PasswordManager.Interfaces.VM;
using PasswordManager.Models.Entity;
using PasswordManager.Models.UI;

namespace PasswordManager.Services.VM
{
    public class AddLoginItemsPageVMService : IAddLoginItemsPageVMService
    {
        #region readonly

        private readonly IDatabaseClient _databaseClient;
        private readonly ICryptoService _cryptoService;

        #endregion

        public AddLoginItemsPageVMService
        (
            IDatabaseClient databaseClient,
            ICryptoService cryptoService)
        {
            _databaseClient = databaseClient;
            _cryptoService = cryptoService;
        }

        #region public methods

        public bool SaveAccountDetails(AccountDetails account, string masterPassword)
        {
            try
            {
                //encrypt the password field with master password
                var encyptedpassword = _cryptoService.EncryptString(masterPassword, account.Password);

                if (!encyptedpassword.IsSuccess || string.IsNullOrEmpty(encyptedpassword.Data))
                    return false;

                var loginItem = new LoginItem
                {
                    Uuid = string.IsNullOrEmpty(account.Uuid) ? Guid.NewGuid().ToString() : account.Uuid,
                    WebsiteName = account.WebsiteName,
                    Url = account.WebsiteUrl,
                    Username = account.Username,
                    Password = encyptedpassword.Data
                };

                var dboperationResult = _databaseClient.AddOrUpdate(loginItem);

                if (!dboperationResult.IsSuccess)
                    return false;

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public void DeleteAccountDetails(AccountDetails account)
        {
            try
            {
                var realm = _databaseClient.GetRealmInstance();
                var loginItem = _databaseClient.Find<LoginItem>(account.Uuid);

                _databaseClient.Write(() =>
                {
                    if (loginItem.IsSuccess && loginItem.Data != null)
                    {
                        realm.Remove(loginItem.Data);
                    }
                });
            }
            catch(Exception) { }
        }

        #endregion

    }
}
