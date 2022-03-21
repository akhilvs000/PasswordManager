
using System.Collections.Generic;

namespace PasswordManager
{
    public static class Constants
    {
        public static class Pages
        {
            public const string NavigationPage = nameof(Xamarin.Forms.NavigationPage);
            public const string SetMasterPasswordPage = nameof(Views.SetMasterPasswordPage);
            public const string LoginPage = nameof(Views.LoginPage);
            public const string AddLoginItemPage = nameof(Views.AddLoginItemsPage);
            public const string AccountListPage = nameof(Views.AccountListPage);
        }

        public static class Parameters
        {
            public const string MasterPassword = nameof(MasterPassword);
            public const string AccountDetails = nameof(AccountDetails);
            public const string Reload = nameof(Reload);
        }

        public static class AppConfig
        {
            public const string Lang_EN = "en";
            public const string Lang_DE = "de";

            public const char LanguageSeparator = '-';

            public static readonly HashSet<string> SupportedLanguages = new HashSet<string>
            {
                Lang_EN, Lang_DE
            };
        }
    }
}
