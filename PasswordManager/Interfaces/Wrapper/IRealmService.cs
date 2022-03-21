using System;
namespace PasswordManager.Interfaces.Wrapper
{
    public interface IRealmService
    {
        IRealm GetInstance();
    }
}
