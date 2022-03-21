using System;
using PasswordManager.Interfaces.Wrapper;
using Xamarin.Essentials;

namespace PasswordManager.Services.Wrapper
{
    public class XamarinEssentialService : IXamarinEssentialService
    {
        public bool GetPreference(string key, bool defaultValue)
            => Preferences.Get(key, defaultValue);

        public void SetPreference(string key, bool value)
            => Preferences.Set(key, value);

        public string GetPreference(string key, string defaultValue)
            => Preferences.Get(key, defaultValue);

        public void SetPreference(string key, string value)
            => Preferences.Set(key, value);
    }
}
