using System;
namespace PasswordManager.Interfaces.Wrapper
{
    public interface IXamarinEssentialService
    {
        bool GetPreference(string key, bool defaultValue);
        void SetPreference(string key, bool value);
        string GetPreference(string key, string defaultValue);
        void SetPreference(string key, string value);
    }
}
