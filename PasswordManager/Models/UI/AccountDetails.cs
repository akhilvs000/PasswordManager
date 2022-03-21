using System;
using Prism.Mvvm;

namespace PasswordManager.Models.UI
{
    public class AccountDetails : BindableBase
    {
        public string Uuid { get; set; }

        private string _websiteName;
        public string WebsiteName
        {
            get => _websiteName;
            set => SetProperty(ref _websiteName, value);
        }

        private string _websiteUrl;
        public string WebsiteUrl
        {
            get => _websiteUrl;
            set => SetProperty(ref _websiteUrl, value);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
    }
}
