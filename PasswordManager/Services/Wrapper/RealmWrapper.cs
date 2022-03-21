using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PasswordManager.Interfaces.Wrapper;
using Realms;

namespace PasswordManager.Services.Wrapper
{
    public class RealmWrapper : IRealm
    {
        private readonly Realm _realm;

        public RealmWrapper(Realm realm)
            => _realm = realm;

        public IQueryable<T> All<T>() where T : RealmObject
            => _realm.All<T>();

        public void Write(Action action)
            => _realm.Write(action);

        public void AddAll<T>(IEnumerable<T> list) where T : RealmObject
        {
            using (var transaction = _realm.BeginWrite())
            {
                foreach (var item in list)
                    _realm.Add(item);
                transaction.Commit();
            }
        }

        public T Add<T>(T obj, bool update = false) where T : RealmObject
            => _realm.Add(obj, update);

        public void RemoveRange<T>(IQueryable<T> range) where T : RealmObject
            => _realm.RemoveRange(range);

        public T Find<T>(string primaryKey) where T : RealmObject
            => _realm.Find<T>(primaryKey);

        public T Find<T>(long? primaryKey) where T : RealmObject
            => _realm.Find<T>(primaryKey);

        public Task WriteAsync(Action<IRealm> action)
            => _realm.WriteAsync(realm => action(new RealmWrapper(realm)));

        public void Remove(RealmObject obj)
            => _realm.Remove(obj);

        public void RemoveAll<T>() where T : RealmObject
            => _realm.RemoveAll<T>();

        public void RemoveAll()
            => _realm.RemoveAll();
    }
}
