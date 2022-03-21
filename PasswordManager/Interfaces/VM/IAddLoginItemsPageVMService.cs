using System;
using PasswordManager.Models.UI;

namespace PasswordManager.Interfaces.VM
{
    public interface IAddLoginItemsPageVMService
    {
        bool SaveAccountDetails(AccountDetails account, string masterPassword);
        void DeleteAccountDetails(AccountDetails account);
    }
}
