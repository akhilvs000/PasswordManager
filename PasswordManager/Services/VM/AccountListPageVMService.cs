using System;
using System.Collections.ObjectModel;
using System.Linq;
using PasswordManager.Interfaces.Common;
using PasswordManager.Interfaces.Infrastructure;
using PasswordManager.Interfaces.VM;
using PasswordManager.Models.Entity;
using PasswordManager.Models.UI;

namespace PasswordManager.Services.VM
{
    public class AccountListPageVMService : IAccountListPageVMService
    {
        #region readonly

        private ICryptoService _cryptoService;
        private IDatabaseClient _databaseClient;

        #endregion

        public AccountListPageVMService
        (
            ICryptoService cryptoService,
            IDatabaseClient databaseClient
        )
        {
            _cryptoService = cryptoService;
            _databaseClient = databaseClient;
        }

        #region public methods

        public ObservableCollection<AccountDetails> GetAccountDetailsList(string masterpassword)
        {
            try
            {
                var loginItems = _databaseClient.GetAll<LoginItem>()?.Data?.Where(x => !x.IsDummyItem);

                return ExtractAccountDetails(loginItems, masterpassword);
            }
            catch(Exception)
            {
                return new ObservableCollection<AccountDetails>();
            }
        }

        #endregion

        #region private methods

        private ObservableCollection<AccountDetails> ExtractAccountDetails(IQueryable<LoginItem> loginItems, string masterpassword)
        {
            var accountList = new ObservableCollection<AccountDetails>();

            if (loginItems?.Any() != true)
                return new ObservableCollection<AccountDetails>();

            foreach(var item in loginItems)
            {
                //decrypt the encypted password field with master password
                var decryptedPassword = _cryptoService.DecryptString(masterpassword, item.Password);

                if (!decryptedPassword.IsSuccess || string.IsNullOrEmpty(decryptedPassword.Data))
                    continue;

                accountList.Add(new AccountDetails
                {
                    Uuid = item.Uuid,
                    WebsiteName = item.WebsiteName,
                    WebsiteUrl = item.Url,
                    Username = item.Username,
                    Password = decryptedPassword.Data
                });
            }

            return accountList;
        }

        #endregion
    }
}
