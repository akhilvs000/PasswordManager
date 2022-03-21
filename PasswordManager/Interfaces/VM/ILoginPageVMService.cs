using System;
namespace PasswordManager.Interfaces.VM
{
    public interface ILoginPageVMService
    {
        bool Login(string password);
    }
}
