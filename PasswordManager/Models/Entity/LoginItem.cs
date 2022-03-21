using System;
using Realms;

namespace PasswordManager.Models.Entity
{
    public class LoginItem : RealmObject
    {
        [PrimaryKey]
        public string Uuid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string WebsiteName { get; set; }
        public string Url { get; set; }
        public bool IsDummyItem { get; set; }
    }
}
