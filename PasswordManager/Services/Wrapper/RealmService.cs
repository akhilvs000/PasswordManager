using System;
using PasswordManager.Interfaces.Wrapper;
using Realms;

namespace PasswordManager.Services.Wrapper
{
    public class RealmService : IRealmService
    {
        public IRealm GetInstance()
        {
            var config = new RealmConfiguration
            {
                SchemaVersion = 1
            };
            var realmInstance = Realm.GetInstance(config);
            return new RealmWrapper(realmInstance);
        }
    }
}
