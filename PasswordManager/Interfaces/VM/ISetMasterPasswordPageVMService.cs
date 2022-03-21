using System;
namespace PasswordManager.Interfaces.VM
{
    public interface ISetMasterPasswordPageVMService
    {
        bool SetMasterPassword(string masterPassword);
    }
}
