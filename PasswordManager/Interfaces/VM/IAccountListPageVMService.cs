using System;
using System.Collections.ObjectModel;
using PasswordManager.Models.UI;

namespace PasswordManager.Interfaces.VM
{
    public interface IAccountListPageVMService
    {
        ObservableCollection<AccountDetails> GetAccountDetailsList(string masterpassword);
    }
}
